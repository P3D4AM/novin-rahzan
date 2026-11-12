namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal abstract class TypeCompactor
{
	public abstract TypeBucket RegisterDataRow(int rowId);
}
