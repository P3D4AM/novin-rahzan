using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Dashboard;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiInteractionIdent
{
	Chart = 1,
	Gauge,
	Image,
	Indicator,
	OnlineMap,
	PivotTable,
	Progress,
	RegionMap,
	Table,
	TableColumn,
	Text,
	Cards
}
