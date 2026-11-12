using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.SqlServer.Management.Assessment;

[JsonConverter(typeof(StringEnumConverter))]
public enum SeverityLevel
{
	Information,
	Warning,
	Critical
}
