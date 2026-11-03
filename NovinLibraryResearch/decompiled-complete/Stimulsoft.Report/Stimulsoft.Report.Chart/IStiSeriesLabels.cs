using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components.TextFormats;

namespace Stimulsoft.Report.Chart;

public interface IStiSeriesLabels : ICloneable, IStiJsonReportObject
{
	bool AllowApplyStyle { get; set; }

	bool ShowZeros { get; set; }

	bool ShowNulls { get; set; }

	bool MarkerVisible { get; set; }

	int Step { get; set; }

	string ValueTypeSeparator { get; set; }

	string TextBefore { get; set; }

	string TextAfter { get; set; }

	float Angle { get; set; }

	string Format { get; set; }

	bool Antialiasing { get; set; }

	bool Visible { get; set; }

	bool DrawBorder { get; set; }

	bool UseSeriesColor { get; set; }

	StiMarkerAlignment MarkerAlignment { get; set; }

	StiSeriesLabelsValueType ValueType { get; set; }

	StiSeriesLabelsValueType LegendValueType { get; set; }

	Size MarkerSize { get; set; }

	Color LabelColor { get; set; }

	Color BorderColor { get; set; }

	StiBrush Brush { get; set; }

	Font Font { get; set; }

	IStiChart Chart { get; set; }

	StiSeriesLabelsCoreXF Core { get; set; }

	bool PreventIntersection { get; set; }

	bool WordWrap { get; set; }

	int Width { get; set; }

	StiFormatService FormatService { get; set; }
}
