using System;
using System.ComponentModel;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public class StiSplineArea : StiClusteredColumnArea, IStiSplineArea, IStiClusteredColumnArea, IStiAxisArea, IStiArea, ICloneable, IStiJsonReportObject, IStiRoundValuesArea
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiSplineArea;

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiSplineSeries);
	}

	public override StiArea CreateNew()
	{
		return new StiSplineArea();
	}

	[StiUniversalConstructor("Area")]
	public StiSplineArea()
	{
		base.Core = new StiSplineAreaCoreXF(this);
	}
}
