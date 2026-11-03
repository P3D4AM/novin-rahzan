using System;

namespace Stimulsoft.Report.Chart;

[Flags]
public enum StiSeriesLabelsValueType
{
	Value = 0,
	SeriesTitle = 1,
	Argument = 2,
	Tag = 3,
	Weight = 4,
	ValueArgument = 5,
	ArgumentValue = 6,
	SeriesTitleValue = 7,
	SeriesTitleArgument = 8
}
