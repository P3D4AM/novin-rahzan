using System;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public interface IStiSteppedRangeSeries : IStiSteppedLineSeries, IStiBaseLineSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiAllowApplyColorNegative, IStiShowNullsSeries, IStiShowZerosSeries, IStiRangeSeries, IStiAllowApplyBrushNegative
{
	StiBrush Brush { get; set; }
}
