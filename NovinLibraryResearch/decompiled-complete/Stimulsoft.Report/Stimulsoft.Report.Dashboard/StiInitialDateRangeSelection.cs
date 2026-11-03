using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Dashboard;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiInitialDateRangeSelection
{
	DayTomorrow,
	DayToday,
	DayYesterday,
	WeekNext,
	WeekCurrent,
	WeekPrevious,
	MonthNext,
	MonthCurrent,
	MonthPrevious,
	QuarterNext,
	QuarterCurrent,
	QuarterPrevious,
	QuarterFirst,
	QuarterSecond,
	QuarterThird,
	QuarterFourth,
	YearNext,
	YearCurrent,
	YearPrevious,
	Last7Days,
	Last14Days,
	Last30Days,
	DateToWeek,
	DateToMonth,
	DateToQuarter,
	DateToYear
}
