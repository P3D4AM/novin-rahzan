using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Data.Engine;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiDataSortDirection
{
	Ascending = 1,
	Descending,
	None
}
