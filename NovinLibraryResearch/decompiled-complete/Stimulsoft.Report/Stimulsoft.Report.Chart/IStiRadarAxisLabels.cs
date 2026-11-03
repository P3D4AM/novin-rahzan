using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public interface IStiRadarAxisLabels : ICloneable, IStiJsonReportObject
{
	StiRadarAxisLabelsCoreXF Core { get; set; }

	bool RotationLabels { get; set; }

	bool AllowApplyStyle { get; set; }

	bool DrawBorder { get; set; }

	string TextBefore { get; set; }

	string TextAfter { get; set; }

	string Format { get; set; }

	Font Font { get; set; }

	bool Antialiasing { get; set; }

	Color Color { get; set; }

	Color BorderColor { get; set; }

	StiBrush Brush { get; set; }

	float Width { get; set; }

	bool WordWrap { get; set; }

	bool PreventIntersection { get; set; }
}
