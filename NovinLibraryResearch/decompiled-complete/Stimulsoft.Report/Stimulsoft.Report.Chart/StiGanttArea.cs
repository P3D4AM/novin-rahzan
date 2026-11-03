using System;
using System.ComponentModel;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public class StiGanttArea : StiClusteredBarArea, IStiGanttArea, IStiClusteredBarArea, IStiClusteredColumnArea, IStiAxisArea, IStiArea, ICloneable, IStiJsonReportObject, IStiRoundValuesArea
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiGanttArea;

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiGanttSeries);
	}

	public override Type[] GetSeriesTypes()
	{
		return new Type[1] { typeof(StiGanttSeries) };
	}

	public override Type[] GetSeriesLabelsTypes()
	{
		return new Type[4]
		{
			typeof(StiNoneLabels),
			typeof(StiValueAxisLabels),
			typeof(StiCenterAxisLabels),
			typeof(StiOutsideAxisLabels)
		};
	}

	public override StiArea CreateNew()
	{
		return new StiGanttArea();
	}

	[StiUniversalConstructor("Area")]
	public StiGanttArea()
	{
		base.Core = new StiGanttAreaCoreXF(this);
	}
}
