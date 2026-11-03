using System;
using System.ComponentModel;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public class StiStackedAreaArea : StiStackedColumnArea, IStiStackedAreaArea, IStiStackedColumnArea, IStiAxisArea, IStiArea, ICloneable, IStiJsonReportObject
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiStackedAreaArea;

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiStackedAreaSeries);
	}

	public override StiArea CreateNew()
	{
		return new StiStackedAreaArea();
	}

	[StiUniversalConstructor("Area")]
	public StiStackedAreaArea()
	{
		base.Core = new StiStackedAreaAreaCoreXF(this);
	}
}
