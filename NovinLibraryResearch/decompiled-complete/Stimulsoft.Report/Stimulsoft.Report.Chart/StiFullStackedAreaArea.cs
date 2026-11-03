using System;
using System.ComponentModel;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public class StiFullStackedAreaArea : StiFullStackedColumnArea, IStiFullStackedAreaArea, IStiFullStackedColumnArea, IStiStackedColumnArea, IStiAxisArea, IStiArea, ICloneable, IStiJsonReportObject
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiFullStackedAreaArea;

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiFullStackedAreaSeries);
	}

	public override StiArea CreateNew()
	{
		return new StiFullStackedAreaArea();
	}

	[StiUniversalConstructor("Area")]
	public StiFullStackedAreaArea()
	{
		base.Core = new StiFullStackedAreaAreaCoreXF(this);
	}
}
