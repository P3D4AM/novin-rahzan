using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public interface IStiPieSeries : IStiSeries, ICloneable, IStiJsonReportObject, IStiFontIconsSeries, IStiSeriesBorderThickness, IStiAllowApplyBorderColor, IStiAllowApplyBrush, IStiShowZerosSeries
{
	float StartAngle { get; set; }

	Color BorderColor { get; set; }

	StiBrush Brush { get; set; }

	bool Lighting { get; set; }

	float Diameter { get; set; }

	float Distance { get; set; }

	double[] CutPieListValues { get; set; }
}
