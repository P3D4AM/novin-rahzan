using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Base.Licenses;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiLicenseResultFormat
{
	Object = 1,
	Base64
}
