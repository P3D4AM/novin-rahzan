namespace Microsoft.SqlServer.Management.Assessment.Data;

internal interface IRowMorph
{
	bool TryGetData(IDataRow source, string column, out object value);
}
