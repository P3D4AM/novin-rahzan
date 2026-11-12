using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Dashboard;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiFilterCondition
{
	EqualTo,
	NotEqualTo,
	GreaterThan,
	GreaterThanOrEqualTo,
	LessThan,
	LessThanOrEqualTo,
	Between,
	NotBetween,
	Containing,
	NotContaining,
	BeginningWith,
	EndingWith,
	IsNull,
	IsNotNull
}
