using System.Collections.Generic;
using System.Linq;

namespace Microsoft.SqlServer.Management.Assessment.Data.Morphs.Indexes;

internal class DuplicateIndexesMorph : IndexMorph
{
	protected override IEnumerable<Measurement> ProcessTableIndexes(IGrouping<string, IndexDetails> tableIndexes)
	{
		foreach (IGrouping<string, IndexDetails> item in IndexMorph.Duplicates(tableIndexes))
		{
			IndexDetails indexToSkip = item.OrderBy((IndexDetails i) => i.OrderedColumns).FirstOrDefault((IndexDetails i) => i.IsPrimaryKey && i.IsUnique);
			if (indexToSkip == null)
			{
				indexToSkip = item.OrderBy((IndexDetails i) => i.OrderedColumns).FirstOrDefault((IndexDetails i) => i.IsPrimaryKey || i.IsUnique);
			}
			if (indexToSkip == null)
			{
				indexToSkip = item.OrderBy((IndexDetails i) => i.OrderedColumns).FirstOrDefault();
			}
			IndexDetails[] source = item.Where((IndexDetails d) => d != indexToSkip).ToArray();
			if (source.Any())
			{
				yield return new Measurement
				{
					{ "Table", tableIndexes.Key },
					{
						"Indexes",
						string.Join(",", item.Select((IndexDetails d) => d.Name))
					},
					{
						"ToDelete",
						string.Join(",", source.Select((IndexDetails d) => d.Name))
					}
				};
			}
		}
	}
}
