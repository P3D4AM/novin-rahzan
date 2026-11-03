using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public interface IStiStrips : ICloneable, IStiJsonReportObject
{
	StiStripsCoreXF Core { get; set; }

	bool AllowApplyStyle { get; set; }

	bool ShowBehind { get; set; }

	StiBrush StripBrush { get; set; }

	bool Antialiasing { get; set; }

	Font Font { get; set; }

	string Text { get; set; }

	bool TitleVisible { get; set; }

	Color TitleColor { get; set; }

	StiStrips.StiOrientation Orientation { get; set; }

	bool ShowInLegend { get; set; }

	string MaxValue { get; set; }

	string MinValue { get; set; }

	bool Visible { get; set; }

	IStiChart Chart { get; set; }
}
