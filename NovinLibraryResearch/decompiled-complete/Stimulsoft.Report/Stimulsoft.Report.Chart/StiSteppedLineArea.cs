using System;
using System.ComponentModel;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public class StiSteppedLineArea : StiClusteredColumnArea, IStiSteppedLineArea, IStiClusteredColumnArea, IStiAxisArea, IStiArea, ICloneable, IStiJsonReportObject, IStiRoundValuesArea
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiSteppedLineArea;

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiSteppedLineSeries);
	}

	public override StiArea CreateNew()
	{
		return new StiSteppedLineArea();
	}

	[StiUniversalConstructor("Area")]
	public StiSteppedLineArea()
	{
		base.Core = new StiSteppedLineAreaCoreXF(this);
	}
}
