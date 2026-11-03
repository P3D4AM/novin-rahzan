using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Dashboard;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiOnlineMapLocationColorType
{
	Single,
	ColorEach,
	Group,
	Value
}
