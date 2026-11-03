using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public class StiStockArea : StiClusteredColumnArea, IStiStockArea, IStiClusteredColumnArea, IStiAxisArea, IStiArea, ICloneable, IStiJsonReportObject, IStiRoundValuesArea
{
	public override StiComponentId ComponentId => StiComponentId.StiStockArea;

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiStockSeries);
	}

	public override Type[] GetSeriesTypes()
	{
		return new Type[1] { typeof(StiStockSeries) };
	}

	public override Type[] GetSeriesLabelsTypes()
	{
		return new Type[1] { typeof(StiNoneLabels) };
	}

	public override StiArea CreateNew()
	{
		return new StiStockArea();
	}

	[StiUniversalConstructor("Area")]
	public StiStockArea()
	{
		base.Core = new StiStockAreaCoreXF(this);
	}
}
