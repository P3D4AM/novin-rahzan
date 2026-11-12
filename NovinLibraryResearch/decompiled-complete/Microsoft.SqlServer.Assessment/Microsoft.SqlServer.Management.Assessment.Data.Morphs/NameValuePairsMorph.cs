using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.SqlServer.Management.Assessment.Data.Morphs;

internal class NameValuePairsMorph : MultidimensionalCachedMorph
{
	public string KeyColumn { get; set; }

	public string ValueColumn { get; set; }

	public Dictionary<string, string> Map { get; } = new Dictionary<string, string>(StringComparer.Ordinal);


	public override IEnumerable<IDataRow> Transform(IDataView source)
	{
		yield return TryGetNameValuePairs(source);
	}

	private Measurement TryGetNameValuePairs(IDataView source)
	{
		Measurement measurement = new Measurement();
		if (string.IsNullOrEmpty(KeyColumn) || string.IsNullOrEmpty(ValueColumn) || Map.Values.Any(string.IsNullOrEmpty))
		{
			return measurement;
		}
		foreach (IDataRow item in source)
		{
			if (item.TryGetData(KeyColumn, out var value) && !string.IsNullOrEmpty(value.Normalize<string>()) && Map.TryGetValue(value.Normalize<string>(), out var value2) && !string.IsNullOrEmpty(value2) && item.TryGetData(ValueColumn, out var value3))
			{
				measurement.Add(value2, value3);
			}
		}
		return measurement;
	}
}
