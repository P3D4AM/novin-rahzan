using System.Collections.Generic;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public interface IStiChart
{
	StiChartConditionsCollection SeriesLabelsConditions { get; set; }

	StiSeriesCollection Series { get; set; }

	IStiArea Area { get; set; }

	IStiChartStyle Style { get; set; }

	IStiSeriesLabels SeriesLabels { get; set; }

	IStiLegend Legend { get; set; }

	IStiChartTitle Title { get; set; }

	IStiChartTable Table { get; set; }

	StiStripsCollection Strips { get; set; }

	StiConstantLinesCollection ConstantLines { get; set; }

	int HorSpacing { get; set; }

	int VertSpacing { get; set; }

	bool IsDesigning { get; }

	StiBrush Brush { get; set; }

	List<StiAnimation> PreviousAnimations { get; set; }

	bool IsAnimationChangingValues { get; }

	double ConvertToHInches(double value);
}
