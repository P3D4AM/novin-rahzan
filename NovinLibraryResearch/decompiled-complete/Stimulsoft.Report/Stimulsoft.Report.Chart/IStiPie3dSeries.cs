using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public interface IStiPie3dSeries : IStiPieSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiFontIconsSeries, IStiSeriesBorderThickness, IStiAllowApplyBorderColor, IStiAllowApplyBrush, IStiShowZerosSeries
{
	StiPie3dOptions Options3D { get; set; }
}
