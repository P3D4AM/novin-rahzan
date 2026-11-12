using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Base.Drawing;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiBorderIdent
{
	Border = 1,
	AdvancedBorder
}
