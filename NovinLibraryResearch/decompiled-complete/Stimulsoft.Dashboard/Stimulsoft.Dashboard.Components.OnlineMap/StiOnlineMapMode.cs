using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Dashboard.Components.OnlineMap;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiOnlineMapMode
{
	Location,
	Coordinate
}
