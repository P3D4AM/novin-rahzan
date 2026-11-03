using System;
using System.ComponentModel;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public class StiClusteredBarArea : StiClusteredColumnArea, IStiClusteredBarArea, IStiClusteredColumnArea, IStiAxisArea, IStiArea, ICloneable, IStiJsonReportObject, IStiRoundValuesArea
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiClusteredBarArea;

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiClusteredBarSeries);
	}

	public override Type[] GetSeriesTypes()
	{
		return new Type[1] { typeof(StiClusteredBarSeries) };
	}

	public override StiArea CreateNew()
	{
		return new StiClusteredBarArea();
	}

	public override Type[] GetSeriesLabelsTypes()
	{
		return new Type[9]
		{
			typeof(StiNoneLabels),
			typeof(StiInsideBaseAxisLabels),
			typeof(StiInsideEndAxisLabels),
			typeof(StiCenterAxisLabels),
			typeof(StiOutsideBaseAxisLabels),
			typeof(StiOutsideEndAxisLabels),
			typeof(StiLeftAxisLabels),
			typeof(StiValueAxisLabels),
			typeof(StiRightAxisLabels)
		};
	}

	[StiUniversalConstructor("Area")]
	public StiClusteredBarArea()
	{
		base.Core = new StiClusteredBarAreaCoreXF(this);
	}
}
