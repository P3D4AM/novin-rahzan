using System;
using System.ComponentModel;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public class StiStackedBarArea : StiClusteredBarArea, IStiStackedBarArea, IStiClusteredBarArea, IStiClusteredColumnArea, IStiAxisArea, IStiArea, ICloneable, IStiJsonReportObject, IStiRoundValuesArea
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiStackedBarArea;

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiStackedBarSeries);
	}

	public override Type[] GetSeriesTypes()
	{
		return new Type[1] { typeof(StiStackedBarSeries) };
	}

	public override StiArea CreateNew()
	{
		return new StiStackedBarArea();
	}

	[StiUniversalConstructor("Area")]
	public StiStackedBarArea()
	{
		base.Core = new StiStackedBarAreaCoreXF(this);
	}
}
