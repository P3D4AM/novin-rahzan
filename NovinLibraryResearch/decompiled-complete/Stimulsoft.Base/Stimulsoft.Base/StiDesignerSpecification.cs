using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Base;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiDesignerSpecification
{
	Auto,
	Developer,
	BICreator,
	Beginner
}
