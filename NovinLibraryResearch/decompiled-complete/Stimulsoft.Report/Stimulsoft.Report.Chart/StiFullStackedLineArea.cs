using System;
using System.ComponentModel;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public class StiFullStackedLineArea : StiFullStackedColumnArea, IStiFullStackedLineArea, IStiFullStackedColumnArea, IStiStackedColumnArea, IStiAxisArea, IStiArea, ICloneable, IStiJsonReportObject
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiFullStackedLineArea;

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiFullStackedLineSeries);
	}

	public override StiArea CreateNew()
	{
		return new StiFullStackedLineArea();
	}

	[StiUniversalConstructor("Area")]
	public StiFullStackedLineArea()
	{
		base.Core = new StiFullStackedLineAreaCoreXF(this);
	}
}
