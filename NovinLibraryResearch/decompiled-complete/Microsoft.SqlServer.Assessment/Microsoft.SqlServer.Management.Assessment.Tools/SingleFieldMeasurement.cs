namespace Microsoft.SqlServer.Management.Assessment.Tools;

internal class SingleFieldMeasurement : IDataRow
{
	public string FieldName { get; }

	public object Value { get; }

	public SingleFieldMeasurement(string fieldName, object value)
	{
		FieldName = fieldName;
		Value = value;
	}

	public bool TryGetData(string column, out object value)
	{
		value = Value;
		return string.CompareOrdinal(FieldName, column) == 0;
	}

	public static Measurement Join(Measurement acc, SingleFieldMeasurement item)
	{
		acc.Add(item.FieldName, item.Value);
		return acc;
	}
}
