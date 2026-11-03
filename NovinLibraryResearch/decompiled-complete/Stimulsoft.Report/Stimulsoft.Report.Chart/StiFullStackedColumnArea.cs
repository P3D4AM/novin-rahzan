using System;
using System.ComponentModel;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public class StiFullStackedColumnArea : StiStackedColumnArea, IStiFullStackedColumnArea, IStiStackedColumnArea, IStiAxisArea, IStiArea, ICloneable, IStiJsonReportObject
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiFullStackedColumnArea;

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiFullStackedColumnSeries);
	}

	public override Type[] GetSeriesTypes()
	{
		return new Type[5]
		{
			typeof(StiFullStackedColumnSeries),
			typeof(StiFullStackedLineSeries),
			typeof(StiFullStackedAreaSeries),
			typeof(StiFullStackedSplineSeries),
			typeof(StiFullStackedSplineAreaSeries)
		};
	}

	public override StiArea CreateNew()
	{
		return new StiFullStackedColumnArea();
	}

	[StiUniversalConstructor("Area")]
	public StiFullStackedColumnArea()
	{
		base.Core = new StiFullStackedColumnAreaCoreXF(this);
	}
}
