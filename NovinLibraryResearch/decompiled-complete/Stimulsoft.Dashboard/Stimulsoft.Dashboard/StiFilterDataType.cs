using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Dashboard;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiFilterDataType
{
	String,
	Numeric,
	DateTime,
	Boolean,
	Expression
}
