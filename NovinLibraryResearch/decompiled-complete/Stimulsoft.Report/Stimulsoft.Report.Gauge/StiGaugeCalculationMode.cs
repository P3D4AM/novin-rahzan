using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Gauge;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiGaugeCalculationMode
{
	Auto = 1,
	Custom
}
