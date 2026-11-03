using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public interface IStiCandlestickSeries : IStiSeries, ICloneable, IStiJsonReportObject, IStiFinancialSeries
{
	Color BorderColor { get; set; }

	Color BorderColorNegative { get; set; }

	float BorderWidth { get; set; }

	StiBrush Brush { get; set; }

	StiBrush BrushNegative { get; set; }
}
