using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Base.Wpf;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiWpfOffice2013Theme
{
	Office2013White,
	Office2013LightGray,
	Office2013DarkGray,
	Office2013VeryDarkGray
}
