namespace Microsoft.SqlServer.Management.Assessment.Tools;

internal class TwoFieldMeasurement : IDataRow
{
	public string FieldName1 { get; }

	public object Value1 { get; }

	public string FieldName2 { get; }

	public object Value2 { get; }

	public TwoFieldMeasurement(string fieldName1, object value1, string fieldName2, object value2)
	{
		FieldName1 = fieldName1;
		Value1 = value1;
		FieldName2 = fieldName2;
		Value2 = value2;
	}

	public bool TryGetData(string column, out object value)
	{
		if (string.CompareOrdinal(FieldName1, column) == 0)
		{
			value = Value1;
			return true;
		}
		if (string.CompareOrdinal(FieldName2, column) == 0)
		{
			value = Value2;
			return true;
		}
		value = null;
		return false;
	}

	public static Measurement Join(Measurement acc, SingleFieldMeasurement item)
	{
		acc.Add(item.FieldName, item.Value);
		return acc;
	}
}
