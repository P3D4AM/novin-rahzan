using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Dashboard.Components.Chart;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiChartLabelsStyle
{
	Value,
	PercentOfTotal,
	Category,
	CategoryValue,
	CategoryPercentOfTotal
}
