using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public interface IStiBaseLineSeries : IStiSeries, ICloneable, IStiJsonReportObject, IStiAllowApplyColorNegative, IStiShowNullsSeries, IStiShowZerosSeries
{
	IStiMarker Marker { get; set; }

	IStiLineMarker LineMarker { get; set; }

	Color LineColor { get; set; }

	StiPenStyle LineStyle { get; set; }

	bool Lighting { get; set; }

	float LineWidth { get; set; }

	int LabelsOffset { get; set; }

	StiShowEmptyCellsAs ShowNullsAs { get; set; }

	StiShowEmptyCellsAs ShowZerosAs { get; set; }
}
