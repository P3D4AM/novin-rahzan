namespace Microsoft.Data.SqlClient;

internal sealed class SqlReturnValue : SqlMetaDataPriv
{
	internal ushort parmIndex;

	internal string parameter;

	internal readonly SqlBuffer value;

	internal SqlReturnValue()
	{
		value = new SqlBuffer();
	}
}
