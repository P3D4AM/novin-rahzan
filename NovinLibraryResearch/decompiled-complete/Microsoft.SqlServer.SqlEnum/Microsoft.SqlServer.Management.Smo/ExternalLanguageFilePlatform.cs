using System.ComponentModel;

namespace Microsoft.SqlServer.Management.Smo;

[TypeConverter(typeof(ExternalLanguageFilePlatform))]
public enum ExternalLanguageFilePlatform
{
	[TsqlSyntaxString("NONE")]
	Default,
	[TsqlSyntaxString("WINDOWS")]
	Windows,
	[TsqlSyntaxString("LINUX")]
	Linux
}
