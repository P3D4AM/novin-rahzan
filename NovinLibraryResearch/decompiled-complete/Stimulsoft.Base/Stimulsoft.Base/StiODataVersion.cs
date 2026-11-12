using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Base;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiODataVersion
{
	V3,
	V4
}
