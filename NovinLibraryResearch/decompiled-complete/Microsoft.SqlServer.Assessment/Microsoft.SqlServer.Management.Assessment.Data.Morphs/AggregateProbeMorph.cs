using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Microsoft.SqlServer.Management.Assessment.Data.Morphs;

internal class AggregateProbeMorph : MultidimensionalCachedMorph
{
	public Dictionary<string, IAggregateFunc> Map { get; } = new Dictionary<string, IAggregateFunc>();


	[JsonProperty("group")]
	public Grouper Grouper { get; set; } = Grouper.Default;


	public override IEnumerable<IDataRow> Transform(IDataView source)
	{
		foreach (DataRowGroup item in Grouper.Transform(source))
		{
			foreach (KeyValuePair<string, IAggregateFunc> pair in Map)
			{
				if (item.Result.ContainsKey(pair.Key))
				{
					throw new SqlAssessmentEvaluationException("Do not use the same field '" + pair.Key + "' for grouping and aggregating simultaneously.");
				}
				object value;
				IEnumerable<object> sequence = item.Data.Select((IDataRow row) => (!row.TryGetData(pair.Key, out value)) ? null : value);
				item.Result.Add(pair.Key, pair.Value.GetAggregate(sequence));
			}
			yield return item.Result;
		}
	}
}
