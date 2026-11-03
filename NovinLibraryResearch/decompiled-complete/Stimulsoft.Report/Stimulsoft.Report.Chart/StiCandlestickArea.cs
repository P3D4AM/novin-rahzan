using System;
using System.ComponentModel;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public class StiCandlestickArea : StiClusteredColumnArea, IStiCandlestickArea, IStiClusteredColumnArea, IStiAxisArea, IStiArea, ICloneable, IStiJsonReportObject, IStiRoundValuesArea
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiCandlestickArea;

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiCandlestickSeries);
	}

	public override Type[] GetSeriesTypes()
	{
		return new Type[1] { typeof(StiCandlestickSeries) };
	}

	public override Type[] GetSeriesLabelsTypes()
	{
		return new Type[1] { typeof(StiNoneLabels) };
	}

	public override StiArea CreateNew()
	{
		return new StiCandlestickArea();
	}

	[StiUniversalConstructor("Area")]
	public StiCandlestickArea()
	{
		base.Core = new StiCandlestickAreaCoreXF(this);
	}
}
