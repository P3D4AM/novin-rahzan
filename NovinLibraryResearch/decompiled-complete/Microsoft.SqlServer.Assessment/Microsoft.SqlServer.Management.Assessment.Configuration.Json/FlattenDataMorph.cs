using System.Collections.Generic;
using Microsoft.SqlServer.Management.Assessment.Data;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Json;

internal class FlattenDataMorph : MultidimensionalCachedMorph
{
	public List<string> Columns { get; set; }

	public override IEnumerable<IDataRow> Transform(IDataView source)
	{
		return TransformRows(source);
	}

	internal IEnumerable<IDataRow> TransformRows(IEnumerable<IDataRow> source)
	{
		if (Columns == null)
		{
			yield break;
		}
		Measurement measurement = new Measurement();
		foreach (IDataRow row in source)
		{
			foreach (string column in Columns)
			{
				if (row.TryGetData(column, out var value))
				{
					if (measurement.TryGetData(column, out var _))
					{
						yield return measurement;
						measurement = new Measurement();
					}
					measurement.Add(column, value);
				}
				value = null;
			}
		}
		if (measurement.Count > 0)
		{
			yield return measurement;
		}
	}
}
