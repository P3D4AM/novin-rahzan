using System;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public interface IStiStackedAreaSeries : IStiStackedLineSeries, IStiStackedBaseLineSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiShowNullsSeries, IStiAllowApplyBrushNegative
{
	StiBrush Brush { get; set; }
}
