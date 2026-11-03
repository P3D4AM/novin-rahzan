using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public class StiStackedSplineAreaArea : StiStackedColumnArea, IStiStackedSplineAreaArea, IStiStackedColumnArea, IStiAxisArea, IStiArea, ICloneable, IStiJsonReportObject
{
	public override StiComponentId ComponentId => StiComponentId.StiStackedSplineAreaArea;

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiStackedSplineAreaSeries);
	}

	public override StiArea CreateNew()
	{
		return new StiStackedSplineAreaArea();
	}

	[StiUniversalConstructor("Area")]
	public StiStackedSplineAreaArea()
	{
		base.Core = new StiStackedSplineAreaAreaCoreXF(this);
	}
}
