using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Dashboard;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiChartLabelsPosition
{
	None,
	Center,
	InsideBase,
	InsideEnd,
	Left,
	Outside,
	OutsideBase,
	OutsideEnd,
	OutsideLeft,
	OutsideRight,
	Right,
	TwoColumns,
	Value,
	Total
}
