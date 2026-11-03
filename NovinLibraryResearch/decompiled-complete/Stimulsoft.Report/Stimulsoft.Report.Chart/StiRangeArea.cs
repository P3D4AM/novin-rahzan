using System;
using System.ComponentModel;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public class StiRangeArea : StiClusteredColumnArea, IStiRangeArea, IStiClusteredColumnArea, IStiAxisArea, IStiArea, ICloneable, IStiJsonReportObject, IStiRoundValuesArea
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiRangeArea;

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiRangeSeries);
	}

	public override Type[] GetSeriesTypes()
	{
		return new Type[1] { typeof(StiRangeSeries) };
	}

	public override StiArea CreateNew()
	{
		return new StiRangeArea();
	}

	[StiUniversalConstructor("Area")]
	public StiRangeArea()
	{
		base.Core = new StiRangeAreaCoreXF(this);
	}
}
