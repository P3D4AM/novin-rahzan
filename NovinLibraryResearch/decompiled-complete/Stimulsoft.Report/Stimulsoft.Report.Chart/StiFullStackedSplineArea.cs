using System;
using System.ComponentModel;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public class StiFullStackedSplineArea : StiFullStackedColumnArea, IStiFullStackedSplineArea, IStiFullStackedColumnArea, IStiStackedColumnArea, IStiAxisArea, IStiArea, ICloneable, IStiJsonReportObject
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiFullStackedSplineArea;

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiFullStackedSplineSeries);
	}

	public override StiArea CreateNew()
	{
		return new StiFullStackedSplineArea();
	}

	[StiUniversalConstructor("Area")]
	public StiFullStackedSplineArea()
	{
		base.Core = new StiFullStackedSplineAreaCoreXF(this);
	}
}
