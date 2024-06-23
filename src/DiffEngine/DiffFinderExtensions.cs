using LoxSmoke.DiffEngine.Sequences.TextFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoxSmoke.DiffEngine
{
    public static class DiffFinderExtensions
    {
        public static List<(DiffOperationType diff, List<string> lines)> 
            CreateDiff(this DiffFinder<StringSequence> finder, IEnumerable<string> sequence1, IEnumerable<string> sequence2)
        {
            var (preparedSequence1, preparedSequence2) = StringSequence.Load(sequence1, sequence2);
            DiffOperationList<StringSequence> diffList = finder.CreateDiffList(preparedSequence1, preparedSequence2);
            return diffList.Diffs.Select(diff => (diff.Operation, diff.Contents.Lines.ToList())).ToList();
        }
    }
}
