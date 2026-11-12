using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Base.Wpf;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiSkinBackground
{
	White,
	LightGray,
	DarkGray,
	VeryDarkGray
}
