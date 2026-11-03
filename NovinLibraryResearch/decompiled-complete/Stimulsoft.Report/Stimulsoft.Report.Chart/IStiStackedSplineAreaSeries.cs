using System;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public interface IStiStackedSplineAreaSeries : IStiStackedSplineSeries, IStiStackedBaseLineSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiShowNullsSeries, IStiAllowApplyColorNegative, IStiAllowApplyBrushNegative
{
	StiBrush Brush { get; set; }
}
