using System.ComponentModel;

namespace Microsoft.SqlServer.Management.SqlScriptPublish;

[TypeConverter(typeof(LocalizedEnumConverter))]
public enum ResultType
{
	None,
	InProgress,
	Success,
	Warning,
	Error
}
