using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public interface IStiAxis : ICloneable, IStiJsonReportObject, IStiDefault
{
	bool LogarithmicScale { get; set; }

	StiAxisCoreXF Core { get; set; }

	bool AllowApplyStyle { get; set; }

	bool StartFromZero { get; set; }

	float Step { get; set; }

	IStiAxisInteraction Interaction { get; set; }

	IStiAxisLabels Labels { get; set; }

	IStiAxisRange Range { get; set; }

	IStiAxisTitle Title { get; set; }

	IStiAxisTicks Ticks { get; set; }

	StiArrowStyle ArrowStyle { get; set; }

	StiPenStyle LineStyle { get; set; }

	Color LineColor { get; set; }

	float LineWidth { get; set; }

	bool Visible { get; set; }

	IStiAxisArea Area { get; set; }

	StiAxisInfoXF Info { get; set; }
}
