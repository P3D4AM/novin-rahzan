using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public interface IStiChartTitle : ICloneable, IStiJsonReportObject, IStiDefault
{
	StiChartTitleCoreXF Core { get; set; }

	bool AllowApplyStyle { get; set; }

	Font Font { get; set; }

	string Text { get; set; }

	StiBrush Brush { get; set; }

	bool Antialiasing { get; set; }

	StringAlignment Alignment { get; set; }

	StiChartTitleDock Dock { get; set; }

	int Spacing { get; set; }

	bool Visible { get; set; }

	IStiChart Chart { get; set; }
}
