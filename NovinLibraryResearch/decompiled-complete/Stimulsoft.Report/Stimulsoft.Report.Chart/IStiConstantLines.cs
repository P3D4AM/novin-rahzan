using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public interface IStiConstantLines : ICloneable, IStiJsonReportObject
{
	StiConstantLinesCoreXF Core { get; set; }

	bool AllowApplyStyle { get; set; }

	bool Antialiasing { get; set; }

	StiConstantLines.StiTextPosition Position { get; set; }

	Font Font { get; set; }

	string Text { get; set; }

	bool TitleVisible { get; set; }

	StiConstantLines.StiOrientation Orientation { get; set; }

	float LineWidth { get; set; }

	StiPenStyle LineStyle { get; set; }

	Color LineColor { get; set; }

	bool ShowInLegend { get; set; }

	bool ShowBehind { get; set; }

	string AxisValue { get; set; }

	bool Visible { get; set; }

	IStiChart Chart { get; set; }
}
