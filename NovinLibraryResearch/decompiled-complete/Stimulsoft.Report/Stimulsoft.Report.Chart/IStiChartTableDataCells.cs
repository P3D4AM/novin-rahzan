using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;

namespace Stimulsoft.Report.Chart;

public interface IStiChartTableDataCells : ICloneable, IStiJsonReportObject, IStiDefault
{
	bool IsDefaultExceptTextColor { get; }

	Font Font { get; set; }

	Color TextColor { get; set; }

	bool ShrinkFontToFit { get; set; }

	float ShrinkFontToFitMinimumSize { get; set; }
}
