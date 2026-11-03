using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Gauge;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiScaleMode
{
	V1 = 1,
	V2
}
