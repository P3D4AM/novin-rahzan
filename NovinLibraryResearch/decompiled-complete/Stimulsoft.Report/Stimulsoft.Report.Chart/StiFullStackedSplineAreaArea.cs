using System;
using System.ComponentModel;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public class StiFullStackedSplineAreaArea : StiFullStackedColumnArea, IStiFullStackedSplineAreaArea, IStiFullStackedColumnArea, IStiStackedColumnArea, IStiAxisArea, IStiArea, ICloneable, IStiJsonReportObject
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiFullStackedSplineAreaArea;

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiFullStackedSplineAreaSeries);
	}

	public override StiArea CreateNew()
	{
		return new StiFullStackedSplineAreaArea();
	}

	[StiUniversalConstructor("Area")]
	public StiFullStackedSplineAreaArea()
	{
		base.Core = new StiFullStackedSplineAreaAreaCoreXF(this);
	}
}
