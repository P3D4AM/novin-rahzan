using System;
using System.ComponentModel;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public class StiScatterArea : StiClusteredColumnArea, IStiScatterArea, IStiClusteredColumnArea, IStiAxisArea, IStiArea, ICloneable, IStiJsonReportObject, IStiRoundValuesArea
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiScatterArea;

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiScatterLineSeries);
	}

	public override Type[] GetSeriesTypes()
	{
		return new Type[3]
		{
			typeof(StiScatterSeries),
			typeof(StiScatterLineSeries),
			typeof(StiScatterSplineSeries)
		};
	}

	public override StiArea CreateNew()
	{
		return new StiScatterArea();
	}

	[StiUniversalConstructor("Area")]
	public StiScatterArea()
	{
		base.Core = new StiScatterAreaCoreXF(this);
	}
}
