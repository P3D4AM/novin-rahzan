using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Data.Functions;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiSystemVariableObject
{
	Line,
	LineABC,
	LineRoman
}
