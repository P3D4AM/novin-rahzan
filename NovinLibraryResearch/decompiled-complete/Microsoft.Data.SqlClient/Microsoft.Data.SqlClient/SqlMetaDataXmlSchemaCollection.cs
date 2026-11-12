namespace Microsoft.Data.SqlClient;

internal sealed class SqlMetaDataXmlSchemaCollection
{
	internal string Database;

	internal string OwningSchema;

	internal string Name;

	public void CopyFrom(SqlMetaDataXmlSchemaCollection original)
	{
		if (original != null)
		{
			Database = original.Database;
			OwningSchema = original.OwningSchema;
			Name = original.Name;
		}
	}
}
