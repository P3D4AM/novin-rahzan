using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal struct DataRow<TId>
{
	private readonly MetadataTypeInfo typeInfo;

	private readonly TId[] values;

	public TId[] Values => values;

	public MetadataTypeInfo TypeInfo => typeInfo;

	public DataRow(MetadataTypeInfo typeInfo, TId[] values)
	{
		this.typeInfo = typeInfo;
		this.values = values;
	}
}
