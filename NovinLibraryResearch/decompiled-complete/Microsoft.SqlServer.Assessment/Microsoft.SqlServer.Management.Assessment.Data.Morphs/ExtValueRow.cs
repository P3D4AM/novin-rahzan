using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.SqlServer.Management.Assessment.Data.Morphs;

[DebuggerDisplay("{ToDebugString()}")]
internal class ExtValueRow : IDataRow
{
	public IDataRow BaseRow { get; }

	public IReadOnlyDictionary<string, object> ExtraData { get; }

	public ExtValueRow(IDataRow baseRow, IReadOnlyDictionary<string, object> extraData)
	{
		BaseRow = baseRow;
		ExtraData = extraData;
	}

	public bool TryGetData(string column, out object value)
	{
		if (!ExtraData.TryGetValue(column, out value))
		{
			return BaseRow.TryGetData(column, out value);
		}
		return true;
	}

	public string ToDebugString()
	{
		return string.Join(", ", ExtraData.Select((KeyValuePair<string, object> p) => $"{p.Key}:{p.Value}"));
	}
}
