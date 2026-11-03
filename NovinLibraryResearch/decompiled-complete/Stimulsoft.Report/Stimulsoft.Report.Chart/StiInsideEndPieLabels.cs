using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public class StiInsideEndPieLabels : StiCenterPieLabels, IStiInsideEndPieLabels, IStiCenterPieLabels, IStiPieSeriesLabels, IStiSeriesLabels, ICloneable, IStiJsonReportObject
{
	public override StiComponentId ComponentId => StiComponentId.StiInsideEndPieLabels;

	public override StiSeriesLabels CreateNew()
	{
		return new StiInsideEndPieLabels();
	}

	public StiInsideEndPieLabels()
	{
		base.Core = new StiInsideEndPieLabelsCoreXF(this);
	}
}
