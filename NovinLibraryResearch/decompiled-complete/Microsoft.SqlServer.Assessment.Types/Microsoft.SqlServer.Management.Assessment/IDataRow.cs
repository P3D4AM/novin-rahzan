namespace Microsoft.SqlServer.Management.Assessment;

public interface IDataRow
{
	bool TryGetData(string column, out object value);
}
