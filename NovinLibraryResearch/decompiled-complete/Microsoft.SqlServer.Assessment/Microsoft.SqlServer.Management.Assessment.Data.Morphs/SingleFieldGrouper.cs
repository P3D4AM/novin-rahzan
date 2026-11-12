using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.SqlServer.Management.Assessment.Data.Morphs;

internal class SingleFieldGrouper : Grouper
{
	public string FieldName { get; set; }

	public SingleFieldGrouper(string fieldName)
	{
		FieldName = fieldName;
	}

	public override IEnumerable<DataRowGroup> Transform(IDataView source)
	{
		IEnumerable<string> enumerable = source.Select(GetGroupName).Distinct<string>(StringComparer.Ordinal);
		foreach (string name in enumerable)
		{
			Measurement result = new Measurement { { FieldName, name } };
			IEnumerable<IDataRow> data = source.Where((IDataRow r) => string.CompareOrdinal(GetGroupName(r), name) == 0);
			yield return new DataRowGroup(result, data);
		}
	}

	private string GetGroupName(IDataRow row)
	{
		if (row.TryGetData(FieldName, out var value) && value != null)
		{
			return value.Normalize<string>();
		}
		return string.Empty;
	}
}
