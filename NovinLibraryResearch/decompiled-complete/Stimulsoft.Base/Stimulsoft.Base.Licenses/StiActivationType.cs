using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Base.Licenses;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiActivationType
{
	Server = 1,
	Developer
}
