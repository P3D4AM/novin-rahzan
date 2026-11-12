using System.Collections.Generic;
using System.Linq;

namespace Microsoft.SqlServer.Management.Assessment.Data.Morphs.Indexes;

internal class RedundantIndexesMorph : IndexMorph
{
	protected override IEnumerable<Measurement> ProcessTableIndexes(IGrouping<string, IndexDetails> tableIndexes)
	{
		HashSet<string> hashSet = new HashSet<string>(IndexMorph.Duplicates(tableIndexes).SelectMany((IGrouping<string, IndexDetails> s) => s.Select((IndexDetails i) => i.Name)));
		List<IndexDetails> list = new List<IndexDetails>();
		IndexDetails[] array = tableIndexes.ToArray();
		for (int j = 0; j < array.Length - 1; j++)
		{
			for (int k = j + 1; k < array.Length; k++)
			{
				if (array[j].IsSimilarTo(array[k]))
				{
					if (!hashSet.Contains(array[j].Name))
					{
						list.Add(array[j]);
					}
					if (!hashSet.Contains(array[k].Name))
					{
						list.Add(array[k]);
					}
				}
			}
		}
		if (list.Any())
		{
			yield return new Measurement
			{
				{ "Table", tableIndexes.Key },
				{
					"Indexes",
					string.Join(",", list.Select((IndexDetails d) => d.Name).Distinct())
				}
			};
		}
	}
}
