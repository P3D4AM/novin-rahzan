using System;
using System.ComponentModel;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public class StiSplineRangeArea : StiClusteredColumnArea, IStiSplineRangeArea, IStiClusteredColumnArea, IStiAxisArea, IStiArea, ICloneable, IStiJsonReportObject, IStiRoundValuesArea
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiSplineRangeArea;

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiSplineRangeSeries);
	}

	public override Type[] GetSeriesTypes()
	{
		return new Type[1] { typeof(StiSplineRangeSeries) };
	}

	public override StiArea CreateNew()
	{
		return new StiSplineRangeArea();
	}

	[StiUniversalConstructor("Area")]
	public StiSplineRangeArea()
	{
		base.Core = new StiSplineRangeAreaCoreXF(this);
	}
}
