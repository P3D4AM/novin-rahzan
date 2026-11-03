using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiReportCacheMode
{
	Off,
	On,
	Auto
}
