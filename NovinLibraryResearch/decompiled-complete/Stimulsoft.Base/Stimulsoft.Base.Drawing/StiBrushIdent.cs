using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Base.Drawing;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiBrushIdent
{
	Empty = 1,
	Solid,
	Gradient,
	Glare,
	Glass,
	Hatch,
	Default,
	Style
}
