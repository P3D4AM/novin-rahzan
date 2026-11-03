using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public interface IStiTrendLine : ICloneable, IStiDefault, IStiJsonReportObject
{
	bool AllowApplyStyle { get; set; }

	StiTrendLineCoreXF Core { get; set; }

	float LineWidth { get; set; }

	StiPenStyle LineStyle { get; set; }

	Color LineColor { get; set; }

	Font Font { get; set; }

	bool ShowShadow { get; set; }

	bool TitleVisible { get; set; }

	string Text { get; set; }

	StiTrendLine.StiTextPosition Position { get; set; }
}
