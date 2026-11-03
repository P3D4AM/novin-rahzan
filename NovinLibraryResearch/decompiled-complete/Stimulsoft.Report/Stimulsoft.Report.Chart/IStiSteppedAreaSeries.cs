using System;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public interface IStiSteppedAreaSeries : IStiSteppedLineSeries, IStiBaseLineSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiAllowApplyColorNegative, IStiShowNullsSeries, IStiShowZerosSeries, IStiAllowApplyBrushNegative
{
	bool TopmostLine { get; set; }

	StiBrush Brush { get; set; }
}
