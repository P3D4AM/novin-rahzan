using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;

namespace Stimulsoft.Report.Chart;

public interface IStiChartTable : ICloneable, IStiJsonReportObject, IStiDefault
{
	StiChartTableCore Core { get; set; }

	IStiChart Chart { get; set; }

	bool AllowApplyStyle { get; set; }

	[Obsolete]
	[EditorBrowsable(EditorBrowsableState.Never)]
	Font Font { get; set; }

	bool MarkerVisible { get; set; }

	Color GridLineColor { get; set; }

	Color TextColor { get; set; }

	bool GridLinesHor { get; set; }

	bool GridLinesVert { get; set; }

	bool GridOutline { get; set; }

	bool Visible { get; set; }

	string Format { get; set; }

	IStiChartTableHeader Header { get; set; }

	IStiChartTableDataCells DataCells { get; set; }
}
