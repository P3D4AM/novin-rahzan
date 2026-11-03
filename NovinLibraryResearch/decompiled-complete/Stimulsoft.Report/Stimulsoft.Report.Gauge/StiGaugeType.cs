using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Gauge;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiGaugeType
{
	FullCircular,
	HalfCircular,
	Linear,
	HorizontalLinear,
	Bullet
}
