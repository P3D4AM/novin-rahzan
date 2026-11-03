using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public interface IStiLegend : ICloneable, IStiJsonReportObject, IStiDefault
{
	StiLegendCoreXF Core { get; set; }

	bool AllowApplyStyle { get; set; }

	IStiChart Chart { get; set; }

	bool HideSeriesWithEmptyTitle { get; set; }

	bool ShowShadow { get; set; }

	Color BorderColor { get; set; }

	StiBrush Brush { get; set; }

	Color TitleColor { get; set; }

	Color LabelsColor { get; set; }

	StiLegendDirection Direction { get; set; }

	StiLegendHorAlignment HorAlignment { get; set; }

	StiLegendVertAlignment VertAlignment { get; set; }

	Font TitleFont { get; set; }

	Font Font { get; set; }

	bool Visible { get; set; }

	bool MarkerVisible { get; set; }

	bool MarkerBorder { get; set; }

	Size MarkerSize { get; set; }

	StiMarkerAlignment MarkerAlignment { get; set; }

	int Columns { get; set; }

	int HorSpacing { get; set; }

	int VertSpacing { get; set; }

	SizeD Size { get; set; }

	string Title { get; set; }
}
