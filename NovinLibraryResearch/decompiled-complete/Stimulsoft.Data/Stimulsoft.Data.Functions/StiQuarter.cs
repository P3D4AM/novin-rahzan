using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Data.Functions;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiQuarter
{
	Q1 = 1,
	Q2,
	Q3,
	Q4
}
