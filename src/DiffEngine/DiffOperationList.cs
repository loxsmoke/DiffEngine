using LoxSmoke.DiffEngine.Extensions;
using LoxSmoke.DiffEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoxSmoke.DiffEngine
{
    /// <summary>
    /// The data structure representing a list of Diff objects. 
    /// For example: DELETE:Hello,INSERT:Goodbye,EQUAL:world.
    /// means: delete "Hello", add "Goodbye" and keep " world."
    /// </summary>
    public class DiffOperationList<T>
        where T : class, IItemSequence<T>
    {
        /// <summary>
        /// The list of diff operations.
        /// </summary>
        public List<DiffOperation<T>> Diffs = new List<DiffOperation<T>>();

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DiffOperationList()
        { }

        /// <summary>
        /// Create the diff list from diff operation objects.
        /// </summary>
        /// <param name="objs"></param>
        public DiffOperationList(params DiffOperation<T>[] objs)
        {
            Diffs.AddRange(objs);
        }

        /// <summary>
        /// Clone the list.
        /// </summary>
        /// <returns></returns>
        public DiffOperationList<T> Clone()
        {
            var clone = MemberwiseClone() as DiffOperationList<T>;
            clone.Diffs = Diffs?.Select(diffClone => diffClone.Clone()).ToList();
            return clone;
        }

        /// <summary>
        /// Add an operation object.
        /// </summary>
        /// <param name="diff"></param>
        public void Add(DiffOperation<T> diff)
        {
            Diffs.Add(diff);
        }

        /// <summary>
        /// Insert the operation object as the first item in the list.
        /// </summary>
        /// <param name="diff"></param>
        public void AddFirst(DiffOperation<T> diff)
        {
            Diffs.Insert(0, diff);
        }

        /// <summary>
        /// Add all elements of the list. 
        /// </summary>
        /// <param name="other">The list of elements to add.</param>
        public void AddRange(DiffOperationList<T> other)
        {
            Diffs.AddRange(other.Diffs);
        }

        /// <summary>
        /// Determines whether two object instances are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as DiffOperationList<T>);
        }

        /// <summary>
        /// Determines whether two object instances are equal.
        /// </summary>
        /// <param name="list">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public bool Equals(DiffOperationList<T> list)
        {
            if (list is null) return false;
            if (object.ReferenceEquals(this, list) &&
                Diffs == null && list.Diffs == null)
                return true;
            if ((Diffs != null) != (list.Diffs != null)) return false;
            return Diffs.SequenceEqual(list.Diffs);
        }

        /// <summary>
        /// Return the minimal debug string. Show only the number of items in the list. 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Diffs count={Diffs.Count}";
        }

        /// <summary>
        /// Compute the Levenshtein distance; <see href="https://en.wikipedia.org/wiki/Levenshtein_distance">Wikipedia article here</see>
        /// the number of inserted, deleted or substituted items.
        /// </summary>
        /// <returns>Number of changes.</returns>
        public int LevenshteinDistance
        {
            get
            {
                var levenshtein = 0;
                var insertions = 0;
                var deletions = 0;
                foreach (var diff in Diffs)
                {
                    switch (diff.Operation)
                    {
                        case DiffOperationType.Insert:
                            insertions += diff.Contents.Length;
                            break;
                        case DiffOperationType.Delete:
                            deletions += diff.Contents.Length;
                            break;
                        case DiffOperationType.Equal:
                            // A deletion and an insertion is one substitution.
                            levenshtein += Math.Max(insertions, deletions);
                            insertions = 0;
                            deletions = 0;
                            break;
                    }
                }
                levenshtein += Math.Max(insertions, deletions);
                return levenshtein;
            }
        }

        /// <summary>
        /// Reorder and merge like edit sections.
        /// Any edit section can move as long as it doesn't cross an equality.
        /// </summary>
        public void CleanupMerge()
        {
            // Add a dummy entry at the end.
            Diffs.Add(((T)null).AsEqual());
            var pointer = 0;
            var deleteCount = 0;
            var insertCount = 0;
            T deleteDiff = null;
            T insertDiff = null;
            while (pointer < Diffs.Count)
            {
                switch (Diffs[pointer].Operation)
                {
                    case DiffOperationType.Insert:
                        insertCount++;
                        if (insertDiff == null) insertDiff = Diffs[pointer].Contents;
                        else insertDiff = insertDiff.Concat(Diffs[pointer].Contents);
                        pointer++;
                        break;
                    case DiffOperationType.Delete:
                        deleteCount++;
                        if (deleteDiff == null) deleteDiff = Diffs[pointer].Contents;
                        else deleteDiff = deleteDiff.Concat(Diffs[pointer].Contents);
                        pointer++;
                        break;
                    case DiffOperationType.Equal:
                        // Upon reaching an equality, check for prior redundancies.
                        if (deleteCount + insertCount > 1)
                        {
                            if (deleteCount != 0 && insertCount != 0)
                            {
                                // Factor out any common prefixes
                                var commonPrefix = insertDiff.CommonPrefix(deleteDiff);
                                if (!commonPrefix.IsEmpty())
                                {
                                    var index = pointer - deleteCount - insertCount - 1;
                                    if (index >= 0 && Diffs[index].IsEqual)
                                    {
                                        Diffs[index].Contents = Diffs[index].Contents.Concat(commonPrefix);
                                    }
                                    else
                                    {
                                        AddFirst(commonPrefix.AsEqual());
                                        pointer++;
                                    }
                                    insertDiff = insertDiff.RightFrom(commonPrefix.Length);
                                    deleteDiff = deleteDiff.RightFrom(commonPrefix.Length);
                                }
                                // Factor out any common suffixies
                                var commonSuffix = insertDiff.CommonSuffix(deleteDiff);
                                if (!commonSuffix.IsEmpty())
                                {
                                    Diffs[pointer].Contents = commonSuffix.Concat(Diffs[pointer].Contents);
                                    insertDiff = insertDiff.LeftExcept(commonSuffix.Length);
                                    deleteDiff = deleteDiff.LeftExcept(commonSuffix.Length);
                                }
                            }
                            // Delete the offending records and add the merged ones
                            pointer -= deleteCount + insertCount;
                            Diffs.RemoveRange(pointer, deleteCount + insertCount);
                            if (deleteDiff != null && !deleteDiff.IsEmpty())
                            {
                                Diffs.Insert(pointer, deleteDiff.AsDelete());
                                pointer++;
                            }
                            if (insertDiff != null && !insertDiff.IsEmpty())
                            {
                                Diffs.Insert(pointer, insertDiff.AsInsert());
                                pointer++;
                            }
                            pointer++;
                        }
                        else if (pointer != 0 && Diffs[pointer - 1].IsEqual)
                        {
                            // Merge this equality with the previous one
                            Diffs[pointer - 1].Contents = Diffs[pointer - 1].Contents.Concat(Diffs[pointer].Contents);
                            Diffs.RemoveAt(pointer);
                        }
                        else
                        {
                            pointer++;
                        }
                        insertCount = 0;
                        deleteCount = 0;
                        deleteDiff = null;
                        insertDiff = null;
                        break;
                }
            }
            if (Diffs.Last().Contents == null)
            {
                Diffs.RemoveAt(Diffs.Count - 1);  // Remove the dummy entry at the end.
            }

            // Second pass: look for single edits surrounded on both sides by
            // equalities which can be shifted sideways to eliminate an equality.
            // e.g: A<ins>BA</ins>C -> <ins>AB</ins>AC
            var changes = false;
            pointer = 1;
            // Intentionally ignore the first and last element (don't need checking).
            while (pointer < (Diffs.Count - 1))
            {
                var prevDiff = Diffs[pointer - 1];
                var thisDiff = Diffs[pointer];
                var nextDiff = Diffs[pointer + 1];
                if (prevDiff.IsEqual && nextDiff.IsEqual)
                {
                    // This is a single edit surrounded by equalities.
                    if (thisDiff.Contents.EndsWith(prevDiff.Contents))
                    {
                        // Shift the edit over the previous equality.
                        thisDiff.Contents = prevDiff.Contents.Concat(thisDiff.Contents.LeftExcept(prevDiff.Contents.Length));
                        nextDiff.Contents = prevDiff.Contents.Concat(nextDiff.Contents);
                        Diffs.RemoveAt(pointer - 1);
                        changes = true;
                    }
                    else if (thisDiff.Contents.StartsWith(nextDiff.Contents))
                    {
                        // Shift the edit over the next equality.
                        prevDiff.Contents = prevDiff.Contents.Concat(nextDiff.Contents);
                        thisDiff.Contents = thisDiff.Contents.RightFrom(nextDiff.Contents.Length).Concat(nextDiff.Contents);
                        Diffs.RemoveAt(pointer + 1);
                        changes = true;
                    }
                }
                pointer++;
            }
            // If shifts were made, the diff needs reordering and another shift sweep.
            if (changes)
            {
                CleanupMerge();
            }
        }

        /// <inheritdoc/>

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
