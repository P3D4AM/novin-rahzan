using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Data.Engine;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiDataJoinType
{
	Inner = 1,
	Left,
	Right,
	Cross,
	Full
}
