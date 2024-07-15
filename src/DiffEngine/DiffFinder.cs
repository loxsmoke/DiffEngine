using LoxSmoke.DiffEngine.Extensions;
using LoxSmoke.DiffEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LoxSmoke.DiffEngine
{
    /// <summary>
    /// Generic difference finder. Compares two sequences of elements and finds equalities, insertions and deletions.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DiffFinder<T>
        where T : class, IItemSequence<T>
    {
        /// <summary>
        /// Diff timeout. Null for no timeout.
        /// </summary>
        public TimeSpan? Timeout { get; set; }
        Stopwatch stopwatch;

        /// <summary>
        /// Return True if timeout has expired. False if timeout did not expire or if it was not set.
        /// </summary>
        public bool TimeoutExpired =>
            Timeout.HasValue &&
            (Timeout.Value.TotalMilliseconds == 0 ||
            stopwatch != null &&
            Timeout.Value.TotalMilliseconds < stopwatch.ElapsedMilliseconds);
        /// <summary>
        /// True if diff was stopped due to timeout expiration. 
        /// False if diff ran to completion.
        /// </summary>
        public bool DiffAbortedByTimeout { get; protected set; }

        /// <summary>
        /// Start a timer if needed for timeout
        /// </summary>
        void StartTimer()
        {
            if (Timeout.HasValue) stopwatch = Stopwatch.StartNew();
            else stopwatch = null;
            DiffAbortedByTimeout = false;
        }

        /// <summary>
        /// Find the differences between two sequences.
        /// </summary>
        /// <param name="sequence1">Old sequence.</param>
        /// <param name="sequence2">New sequence.</param>
        /// <returns>List of Diff objects.</returns>
        public DiffOperationList<T> CreateDiffList(T sequence1, T sequence2)
        {
            StartTimer();

            var diffList = new DiffOperationList<T>();

            // Check the simple case if both sequences are equal.
            if (sequence1.Equals(sequence2))
            {
                if (sequence1.IsEmpty()) return diffList;
                return sequence1.AsEqual().ToList();
            }

            // Remove common prefix.
            var commonprefix = sequence1.CommonPrefix(sequence2);
            if (!commonprefix.IsEmpty())
            {
                diffList.AddFirst(commonprefix.AsEqual());
                sequence1 = sequence1.RightFrom(commonprefix.Length);
                sequence2 = sequence2.RightFrom(commonprefix.Length);
            }

            // Remove common suffix.
            var commonsuffix = sequence1.CommonSuffix(sequence2);
            if (!commonsuffix.IsEmpty())
            {
                sequence1 = sequence1.LeftExcept(commonsuffix.Length);
                sequence2 = sequence2.LeftExcept(commonsuffix.Length);
            }

            // Compute the diff of the middle.
            if (sequence1.IsEmpty())
            {
                // Just add
                diffList.Add(sequence2.AsInsert());
            }
            else if (sequence2.IsEmpty())
            {
                // Just delete
                diffList.Add(sequence1.AsDelete());
            }
            else
            {
                var diffs = FindDiffs(sequence1, sequence2);
                diffList.AddRange(diffs);
            }

            // Add the common suffix.
            if (!commonsuffix.IsEmpty())
            {
                diffList.Add(commonsuffix.AsEqual());
            }

            diffList.CleanupMerge();
            return diffList;
        }

        /// <summary>
        /// Compare lengths of sequences and return the pair (shorter, longer)
        /// </summary>
        /// <param name="sequence1"></param>
        /// <param name="sequence2"></param>
        /// <returns></returns>
        static (T shorter, T longer) ShortLong(T sequence1, T sequence2)
        {
            return sequence1.Length > sequence2.Length ? (sequence2, sequence1) : (sequence1, sequence2);
        }

        /// <summary>
        /// Find the differences between two sequences. Here sequences are assumed to have no common prefix or suffix.
        /// </summary>
        /// <param name="sequence1">Old sequence.</param>
        /// <param name="sequence2">New sequence.</param>
        /// <returns>List of Diff objects.</returns>
        private DiffOperationList<T> FindDiffs(T sequence1, T sequence2)
        {
            var (shortSequence, longSequence) = ShortLong(sequence1, sequence2);
            var i = longSequence.IndexOf(shortSequence);
            if (i >= 0)
            {
                // Shorter sequence is inside the longer sequence.
                var operation = sequence1.Length > sequence2.Length ? DiffOperationType.Delete : DiffOperationType.Insert;

                return new DiffOperationList<T>(
                    longSequence.Left(i).As(operation),
                    shortSequence.AsEqual(),
                    longSequence.RightFrom(i + shortSequence.Length).As(operation));
            }

            if (shortSequence.Length == 1)
            {
                // Single item sequence.
                // After the previous check sequences cannot be equal.
                return new DiffOperationList<T>(sequence1.AsDelete(), sequence2.AsInsert());
            }

            // Check to see if the problem can be split in two. Do this only if we have timeout set. 
            // HalfMatch may be faster but the diff could be not optimal.
            var match = Timeout.HasValue ? HalfMatch(sequence1, sequence2) : null;
            if (match == null) return Bisect(sequence1, sequence2);

            // Process two sub-problems
            var headDiffs = CreateDiffList(match.longHead, match.shortHead);
            var tailDiffs = CreateDiffList(match.longTail, match.shortTail);
            // Merge the results.
            headDiffs.Add(match.common.AsEqual());
            headDiffs.AddRange(tailDiffs);
            return headDiffs;
        }

#pragma warning disable CS1591
        public class MatchStruct
        {
            public T longHead;
            public T longTail;

            public T shortHead;
            public T shortTail;

            public T common;

            public MatchStruct SwappedShortLong()
            {
                return new MatchStruct(shortHead, shortTail, longHead, longTail, common);
            }

            public MatchStruct()
            { }

            public MatchStruct(
                T longHead,
                T longTail,
                T shortHead,
                T shortTail,
                T common)
            {
                this.longHead = longHead;
                this.longTail = longTail;
                this.shortHead = shortHead;
                this.shortTail = shortTail;
                this.common = common;
            }

            /// <summary>
            /// Determines whether two object instances are equal.
            /// </summary>
            /// <param name="obj">The object to compare with the current object.</param>
            /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
            public override bool Equals(object obj)
            {
                return Equals(obj as MatchStruct);
            }

            /// <summary>
            /// Determines whether two object instances are equal.
            /// </summary>
            /// <param name="other">The object to compare with the current object.</param>
            /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
            public bool Equals(MatchStruct other)
            {
                if (other == null) return false;
                return
                    other.longHead.Equals(longHead) &&
                    other.longTail.Equals(longTail) &&
                    other.shortHead.Equals(shortHead) &&
                    other.shortTail.Equals(shortTail) &&
                    other.common.Equals(common);
            }

            /// <inheritdoc/>
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }

        public MatchStruct HalfMatch(T sequence1, T sequence2)
        {
            var (shortSequence, longSequence) = ShortLong(sequence1, sequence2);
            // Small optimization. Ignore short cases
            if (longSequence.Length < 4 || shortSequence.Length * 2 < longSequence.Length)
            {
                return null;
            }

            // First check if the second quarter is the seed for a half-match.
            var hm1 = HalfMatchFromIndex(longSequence, shortSequence, (longSequence.Length + 3) / 4);
            // Check again based on the third quarter.
            var hm2 = HalfMatchFromIndex(longSequence, shortSequence, (longSequence.Length + 1) / 2);
            if (hm1 == null && hm2 == null)
            {
                return null;
            }

            var hm = (hm2 == null) ? hm1 :
                (hm1 == null) ? hm2 :
                (hm1.common.Length > hm2.common.Length ? hm1 : hm2);

            // A half-match was found, sort out the return data.
            return sequence1.Length > sequence2.Length ? hm : hm.SwappedShortLong();
        }

        private MatchStruct HalfMatchFromIndex(T longSequence, T shortSequence, int i)
        {
            // Start with a 1/4 length at position i as a seed.
            var seed = longSequence.Mid(i, longSequence.Length / 4);
            var j = -1;
            MatchStruct best = null;
            while (j < shortSequence.Length &&
                (j = shortSequence.IndexOf(seed, j + 1)) != -1)
            {
                var prefix = longSequence.RightFrom(i).CommonPrefix(shortSequence.RightFrom(j));
                var suffix = longSequence.Left(i).CommonSuffix(shortSequence.Left(j));
                if (best == null || best.common.Length < suffix.Length + prefix.Length)
                {
                    if (best == null) best = new MatchStruct();
                    best.common = suffix.Concat(prefix);
                    best.longHead = longSequence.Left(i - suffix.Length);
                    best.longTail = longSequence.RightFrom(i + prefix.Length);

                    best.shortHead = shortSequence.Left(j - suffix.Length);
                    best.shortTail = shortSequence.RightFrom(j + prefix.Length);
                }
            }
            return (best != null && best.common.Length * 2 >= longSequence.Length) ? best : null;
        }

        /// <summary>
        /// Find the 'middle snake' of a diff, split the problem in two
        /// and return the recursively constructed diff.
        /// See Myers 1986 paper: An O(ND) Difference Algorithm and Its Variations.
        /// </summary>
        /// <param name="sequence1">Old sequence.</param>
        /// <param name="sequence2">New sequence.</param>
        /// <returns>List of Diff objects.</returns>
        public DiffOperationList<T> Bisect(T sequence1, T sequence2)
        {
            int length1 = sequence1.Length;
            int length2 = sequence2.Length;
            var totalLength = length1 + length2;
            var sequenceIndexes1 = new int[totalLength + 1];
            var sequenceIndexes2 = new int[totalLength + 1];
            for (var x = 0; x < sequenceIndexes1.Length; x++)
            {
                sequenceIndexes1[x] = -1;
                sequenceIndexes2[x] = -1;
            }
            var maxD = (totalLength + 1) / 2;
            sequenceIndexes1[maxD + 1] = 0;
            sequenceIndexes2[maxD + 1] = 0;
            int delta = length1 - length2;
            // If the total number of items is odd, then the front path will
            // collide with the reverse path.
            bool front = (delta % 2 != 0);
            // Offsets for start and end of k loop.
            // Prevents mapping of space beyond the grid.
            int k1start = 0; // positive
            int k1end = 0; // positive
            int k2start = 0;
            int k2end = 0;
            for (var segmentLength = 0; segmentLength < maxD; segmentLength++)
            {
                // Bail out if timeout is reached.
                if (TimeoutExpired)
                {
                    DiffAbortedByTimeout = true;
                    break;
                }
                // Walk the front path one step.
                for (var k1 = k1start - segmentLength; k1 <= segmentLength - k1end; k1 += 2)
                {
                    int x1;
                    if (k1 == -segmentLength ||
                        k1 != segmentLength && sequenceIndexes1[maxD + k1 - 1] < sequenceIndexes1[maxD + k1 + 1])
                    {
                        x1 = sequenceIndexes1[maxD + k1 + 1];
                    }
                    else
                    {
                        x1 = sequenceIndexes1[maxD + k1 - 1] + 1;
                    }
                    int y1 = x1 - k1;
                    while (x1 < length1 &&
                        y1 < length2 && sequence1.ItemEquals(x1, sequence2, y1))
                    {
                        x1++;
                        y1++;
                    }
                    sequenceIndexes1[maxD + k1] = x1;
                    if (x1 > length1)
                    {
                        // Ran off the right of the graph.
                        k1end += 2;
                    }
                    else if (y1 > length2)
                    {
                        // Ran off the bottom of the graph.
                        k1start += 2;
                    }
                    else if (front &&
                            maxD + delta - k1 >= 0 &&
                            maxD + delta - k1 < sequenceIndexes2.Length &&
                            sequenceIndexes2[maxD + delta - k1] != -1 &&
                            x1 >= length1 - sequenceIndexes2[maxD + delta - k1])
                    {
                        // Overlap detected.
                        return BisectSplit(sequence1, x1, sequence2, y1);
                    }
                }

                // Walk the reverse path one step.
                for (var k2 = k2start - segmentLength; k2 <= segmentLength - k2end; k2 += 2)
                {
                    int x2;
                    if (k2 == -segmentLength ||
                        k2 != segmentLength && sequenceIndexes2[maxD + k2 - 1] < sequenceIndexes2[maxD + k2 + 1])
                    {
                        x2 = sequenceIndexes2[maxD + k2 + 1];
                    }
                    else
                    {
                        x2 = sequenceIndexes2[maxD + k2 - 1] + 1;
                    }
                    var y2 = x2 - k2;
                    while (x2 < length1 && y2 < length2 &&
                        sequence1.ItemEquals(length1 - x2 - 1, sequence2, length2 - y2 - 1))
                    {
                        x2++;
                        y2++;
                    }
                    sequenceIndexes2[maxD + k2] = x2;
                    if (x2 > length1)
                    {
                        // Ran off the left of the graph.
                        k2end += 2;
                    }
                    else if (y2 > length2)
                    {
                        // Ran off the top of the graph.
                        k2start += 2;
                    }
                    else if (!front &&
                        maxD + delta - k2 >= 0 &&
                        maxD + delta - k2 < sequenceIndexes1.Length &&
                        sequenceIndexes1[maxD + delta - k2] != -1)
                    {
                        var x1 = sequenceIndexes1[maxD + delta - k2];
                        var y1 = x1 - (delta - k2);
                        // Mirror x2 onto top-left coordinate system.
                        x2 = length1 - sequenceIndexes2[maxD + k2];
                        if (x1 >= x2)
                        {
                            // Overlap detected.
                            return BisectSplit(sequence1, x1, sequence2, y1);
                        }
                    }
                }
            }
            // Diff took too long and hit the timeout or
            // number of diffs equals number of items, no commonality at all.
            return new DiffOperationList<T>(sequence1.AsDelete(), sequence2.AsInsert());
        }

        /// <summary>
        /// Given the location of the 'middle snake', split the diff in two parts
        /// and recurse.
        /// </summary>
        /// <param name="sequence1">Old sequence.</param>
        /// <param name="index1">Index of split point in sequence1.</param>
        /// <param name="sequence2">New sequence.</param>
        /// <param name="index2">Index of split point in sequence2.</param>
        /// <returns>List of Diff objects.</returns>
        private DiffOperationList<T> BisectSplit(T sequence1, int index1, T sequence2, int index2)
        {
            var head1 = sequence1.Left(index1);
            var tail1 = sequence1.RightFrom(index1);

            var head2 = sequence2.Left(index2);
            var tail2 = sequence2.RightFrom(index2);

            // Compute both diffs serially.
            var headDiffs = CreateDiffList(head1, head2);
            var tailDiffs = CreateDiffList(tail1, tail2);

            headDiffs.AddRange(tailDiffs);
            return headDiffs;
        }
    }
}
