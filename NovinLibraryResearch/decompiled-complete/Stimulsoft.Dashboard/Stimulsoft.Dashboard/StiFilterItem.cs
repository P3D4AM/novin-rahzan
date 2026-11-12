using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Dashboard;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiFilterItem
{
	Argument,
	Value,
	ValueEnd,
	Expression,
	ValueOpen,
	ValueClose,
	ValueLow,
	ValueHigh
}
