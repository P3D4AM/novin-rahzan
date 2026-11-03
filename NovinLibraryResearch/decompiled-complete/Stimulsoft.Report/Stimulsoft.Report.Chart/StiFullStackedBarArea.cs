using System;
using System.ComponentModel;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public class StiFullStackedBarArea : StiStackedBarArea, IStiFullStackedBarArea, IStiStackedBarArea, IStiClusteredBarArea, IStiClusteredColumnArea, IStiAxisArea, IStiArea, ICloneable, IStiJsonReportObject, IStiRoundValuesArea
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiFullStackedBarArea;

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiFullStackedBarSeries);
	}

	public override Type[] GetSeriesTypes()
	{
		return new Type[1] { typeof(StiFullStackedBarSeries) };
	}

	public override StiArea CreateNew()
	{
		return new StiFullStackedBarArea();
	}

	[StiUniversalConstructor("Area")]
	public StiFullStackedBarArea()
	{
		base.Core = new StiFullStackedBarAreaCoreXF(this);
	}
}
