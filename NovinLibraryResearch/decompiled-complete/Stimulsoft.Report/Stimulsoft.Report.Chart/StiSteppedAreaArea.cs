using System;
using System.ComponentModel;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public class StiSteppedAreaArea : StiClusteredColumnArea, IStiSteppedAreaArea, IStiClusteredColumnArea, IStiAxisArea, IStiArea, ICloneable, IStiJsonReportObject, IStiRoundValuesArea
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiSteppedAreaArea;

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiSteppedAreaSeries);
	}

	public override StiArea CreateNew()
	{
		return new StiSteppedAreaArea();
	}

	[StiUniversalConstructor("Area")]
	public StiSteppedAreaArea()
	{
		base.Core = new StiSteppedAreaAreaCoreXF(this);
	}
}
