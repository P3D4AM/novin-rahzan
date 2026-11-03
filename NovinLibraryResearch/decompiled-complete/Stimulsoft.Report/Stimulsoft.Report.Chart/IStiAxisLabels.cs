using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components.TextFormats;

namespace Stimulsoft.Report.Chart;

public interface IStiAxisLabels : ICloneable, IStiJsonReportObject, IStiDefault
{
	StiAxisLabelsCoreXF Core { get; set; }

	bool AllowApplyStyle { get; set; }

	string Format { get; set; }

	float Angle { get; set; }

	float Width { get; set; }

	string TextBefore { get; set; }

	string TextAfter { get; set; }

	Font Font { get; set; }

	bool Antialiasing { get; set; }

	StiLabelsPlacement Placement { get; set; }

	Color Color { get; set; }

	StiHorAlignment TextAlignment { get; set; }

	float Step { get; set; }

	float CalculatedStep { get; set; }

	bool WordWrap { get; set; }

	StiFormatService FormatService { get; set; }
}
