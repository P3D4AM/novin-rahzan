using System;
using System.ComponentModel;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public class StiSteppedRangeArea : StiClusteredColumnArea, IStiSteppedRangeArea, IStiClusteredColumnArea, IStiAxisArea, IStiArea, ICloneable, IStiJsonReportObject, IStiRoundValuesArea
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiSteppedRangeArea;

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiSteppedRangeSeries);
	}

	public override Type[] GetSeriesTypes()
	{
		return new Type[1] { typeof(StiSteppedRangeSeries) };
	}

	public override StiArea CreateNew()
	{
		return new StiSteppedRangeArea();
	}

	[StiUniversalConstructor("Area")]
	public StiSteppedRangeArea()
	{
		base.Core = new StiSteppedRangeAreaCoreXF(this);
	}
}
