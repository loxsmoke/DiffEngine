using LoxSmoke.DiffEngine.Sequences.TextFile;
using System.Collections.Generic;

namespace LoxSmoke.DiffEngine
{
    /// <summary>
    /// DiffFinder extension methods
    /// </summary>
    public static class DiffFinderExtensions
    {
        /// <summary>
        /// Compares two sequences of strings and returns a list of differences between them.
        /// The method identifies lines that are inserted, deleted, or unchanged between the two sequences 
        /// and records the index positions from both sequences. 
        /// It processes each difference and its contents line by line, updating the indices accordingly.
        /// </summary>
        /// <param name="finder">An instance of <see cref="DiffFinder{StringSequence}"/> used to find differences between the sequences.</param>
        /// <param name="sequence1">The first sequence of strings to compare.</param>
        /// <param name="sequence2">The second sequence of strings to compare.</param>
        /// <returns>
        /// A list of tuples where each tuple represents a difference found between the two sequences. 
        /// The tuple contains the type of difference (<see cref="DiffOperationType"/>), the string line from the sequence, 
        /// and the corresponding indices in the first and second sequences.
        /// </returns>
        public static List<(DiffOperationType diff, string line, int index1, int index2)> 
            Compare(this DiffFinder<StringSequence> finder, IEnumerable<string> sequence1, IEnumerable<string> sequence2)
        {
            var (preparedSequence1, preparedSequence2) = StringSequence.Load(sequence1, sequence2);
            DiffOperationList<StringSequence> diffList = finder.CreateDiffList(preparedSequence1, preparedSequence2);

            var result = new List<(DiffOperationType diff, string line, int index1, int index2)>();
            int line1 = 0, line2 = 0;
            foreach (var diff in diffList.Diffs)
            {
                foreach (var lin in diff.Contents.Lines)
                {
                    result.Add((diff.Operation, lin, line1, line2));

                    if (diff.Operation == DiffOperationType.Insert) line2++;
                    else if (diff.Operation == DiffOperationType.Delete) line1++;
                    else
                    {
                        line1++;
                        line2++;
                    }
                }
            }
            return result;
        }
    }
}
