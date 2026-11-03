using System;
using System.ComponentModel;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public class StiBubbleArea : StiScatterArea, IStiBubbleArea, IStiScatterArea, IStiClusteredColumnArea, IStiAxisArea, IStiArea, ICloneable, IStiJsonReportObject, IStiRoundValuesArea
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiBubbleArea;

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiBubbleSeries);
	}

	public override Type[] GetSeriesTypes()
	{
		return new Type[1] { typeof(StiBubbleSeries) };
	}

	public override StiArea CreateNew()
	{
		return new StiBubbleArea();
	}

	[StiUniversalConstructor("Area")]
	public StiBubbleArea()
	{
		base.Core = new StiBubbleAreaCoreXF(this);
		base.InterlacingHor = new StiInterlacingHor();
		base.InterlacingVert = new StiInterlacingVert();
	}
}
