using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public interface ISystemDataTypeLookup
{
	ISystemDataType Find(DataTypeSpec typeSpec, bool isMaximum);

	ISystemDataType Find(DataTypeSpec typeSpec, int precisionOrMaxLength);

	ISystemDataType Find(DataTypeSpec typeSpec, int precision, int scale);
}
