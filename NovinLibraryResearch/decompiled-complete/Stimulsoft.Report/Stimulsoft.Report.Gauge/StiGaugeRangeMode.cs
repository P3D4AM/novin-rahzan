using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Gauge;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiGaugeRangeMode
{
	Percentage = 1,
	Value
}
