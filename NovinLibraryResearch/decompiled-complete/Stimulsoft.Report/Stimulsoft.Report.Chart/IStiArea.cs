using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public interface IStiArea : ICloneable, IStiJsonReportObject
{
	StiAreaCoreXF Core { get; set; }

	IStiChart Chart { get; set; }

	bool AllowApplyStyle { get; set; }

	bool ColorEach { get; set; }

	bool ShowShadow { get; set; }

	Color BorderColor { get; set; }

	StiBrush Brush { get; set; }

	bool IsDefaultSeriesTypeFullStackedColumnSeries { get; }

	bool IsDefaultSeriesTypeFullStackedBarSeries { get; }

	Type GetDefaultSeriesType();

	Type[] GetSeriesTypes();

	Type GetDefaultSeriesLabelsType();

	Type[] GetSeriesLabelsTypes();
}
