using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public class StiStackedSplineArea : StiStackedColumnArea, IStiStackedSplineArea, IStiStackedColumnArea, IStiAxisArea, IStiArea, ICloneable, IStiJsonReportObject
{
	public override StiComponentId ComponentId => StiComponentId.StiStackedSplineArea;

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiStackedSplineSeries);
	}

	public override StiArea CreateNew()
	{
		return new StiStackedSplineArea();
	}

	[StiUniversalConstructor("Area")]
	public StiStackedSplineArea()
	{
		base.Core = new StiStackedSplineAreaCoreXF(this);
	}
}
