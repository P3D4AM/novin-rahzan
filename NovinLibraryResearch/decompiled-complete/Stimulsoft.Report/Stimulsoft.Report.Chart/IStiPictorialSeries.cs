using System;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public interface IStiPictorialSeries : IStiSeries, ICloneable, IStiJsonReportObject, IStiFontIconsSeries
{
	StiBrush Brush { get; set; }
}
