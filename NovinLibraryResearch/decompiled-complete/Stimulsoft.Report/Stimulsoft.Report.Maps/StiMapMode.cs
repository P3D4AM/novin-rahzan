using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Maps;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiMapMode
{
	Choropleth,
	Online
}
