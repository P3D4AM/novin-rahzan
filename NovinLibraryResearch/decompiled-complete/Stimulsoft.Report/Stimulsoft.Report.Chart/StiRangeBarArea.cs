using System;
using System.ComponentModel;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public class StiRangeBarArea : StiClusteredColumnArea, IStiRangeBarArea, IStiClusteredColumnArea, IStiAxisArea, IStiArea, ICloneable, IStiJsonReportObject, IStiRoundValuesArea
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiRangeBarArea;

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiRangeBarSeries);
	}

	public override Type[] GetSeriesTypes()
	{
		return new Type[1] { typeof(StiRangeBarSeries) };
	}

	public override Type[] GetSeriesLabelsTypes()
	{
		return new Type[7]
		{
			typeof(StiNoneLabels),
			typeof(StiInsideBaseAxisLabels),
			typeof(StiOutsideBaseAxisLabels),
			typeof(StiOutsideAxisLabels),
			typeof(StiLeftAxisLabels),
			typeof(StiValueAxisLabels),
			typeof(StiRightAxisLabels)
		};
	}

	public override StiArea CreateNew()
	{
		return new StiRangeBarArea();
	}

	[StiUniversalConstructor("Area")]
	public StiRangeBarArea()
	{
		base.Core = new StiRangeBarAreaCoreXF(this);
	}
}
