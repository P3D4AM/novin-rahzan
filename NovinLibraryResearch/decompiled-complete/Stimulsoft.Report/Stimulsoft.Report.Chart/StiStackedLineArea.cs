using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public class StiStackedLineArea : StiStackedColumnArea, IStiStackedLineArea, IStiStackedColumnArea, IStiAxisArea, IStiArea, ICloneable, IStiJsonReportObject
{
	public override StiComponentId ComponentId => StiComponentId.StiStackedLineArea;

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiStackedLineSeries);
	}

	public override StiArea CreateNew()
	{
		return new StiStackedLineArea();
	}

	[StiUniversalConstructor("Area")]
	public StiStackedLineArea()
	{
		base.Core = new StiStackedLineAreaCoreXF(this);
	}
}
