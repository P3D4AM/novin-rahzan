using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;

namespace Stimulsoft.Report.Chart;

public interface IStiAxisTitle : ICloneable, IStiJsonReportObject, IStiDefault
{
	StiAxisTitleCoreXF Core { get; set; }

	bool AllowApplyStyle { get; set; }

	Font Font { get; set; }

	string Text { get; set; }

	Color Color { get; set; }

	bool Antialiasing { get; set; }

	StringAlignment Alignment { get; set; }

	StiDirection Direction { get; set; }

	StiTitlePosition Position { get; set; }
}
