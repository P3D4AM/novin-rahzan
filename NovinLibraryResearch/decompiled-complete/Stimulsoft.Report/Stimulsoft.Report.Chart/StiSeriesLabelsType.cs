using System;

namespace Stimulsoft.Report.Chart;

[Flags]
public enum StiSeriesLabelsType
{
	Axis = 1,
	Pie = 2,
	Doughnut = 4,
	Radar = 8,
	Funnel = 0xA,
	Treemap = 0xC,
	Pie3d = 0xD,
	All = 0xF
}
