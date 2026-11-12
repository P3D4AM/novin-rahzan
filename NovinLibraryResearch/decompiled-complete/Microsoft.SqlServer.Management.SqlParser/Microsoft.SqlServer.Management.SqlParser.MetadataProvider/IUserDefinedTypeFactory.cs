using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public interface IUserDefinedTypeFactory
{
	IMutableUserDefinedClrType CreateClrType(ISchema schema, string name);

	IMutableUserDefinedDataType CreateDataType(ISchema schema, string name);

	IMutableUserDefinedTableType CreateTableType(ISchema schema, string name);
}
