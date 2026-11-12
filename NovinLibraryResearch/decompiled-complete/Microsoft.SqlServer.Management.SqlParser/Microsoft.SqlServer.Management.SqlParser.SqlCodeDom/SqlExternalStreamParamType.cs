using System.ComponentModel;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public enum SqlExternalStreamParamType
{
	[Description("DATA_SOURCE")]
	DataSource,
	[Description("LOCATION")]
	Location,
	[Description("FILE_FORMAT")]
	FileFormat,
	[Description("INPUT_OPTIONS")]
	InputOptions,
	[Description("OUTPUT_OPTIONS")]
	OutputOptions,
	Empty
}
