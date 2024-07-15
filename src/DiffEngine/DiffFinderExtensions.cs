using LoxSmoke.DiffEngine.Sequences.TextFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoxSmoke.DiffEngine
{
    public static class DiffFinderExtensions
    {
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
