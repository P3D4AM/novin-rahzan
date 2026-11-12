using System;

namespace Microsoft.SqlServer.Management.Assessment.Data.Morphs;

internal class ExtValueRowMorph : IRowMorph
{
	private string Column { get; }

	private object Value { get; }

	public ExtValueRowMorph(string column, object value)
	{
		Column = column;
		Value = value;
	}

	public bool TryGetData(IDataRow source, string column, out object value)
	{
		if (string.Compare(column, Column, StringComparison.OrdinalIgnoreCase) != 0)
		{
			return source.TryGetData(column, out value);
		}
		value = Value;
		return true;
	}
}
