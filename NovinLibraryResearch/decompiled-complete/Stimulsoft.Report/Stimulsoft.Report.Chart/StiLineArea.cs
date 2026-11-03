using System;
using System.ComponentModel;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public class StiLineArea : StiClusteredColumnArea, IStiLineArea, IStiClusteredColumnArea, IStiAxisArea, IStiArea, ICloneable, IStiJsonReportObject, IStiRoundValuesArea
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiLineArea;

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiLineSeries);
	}

	public override StiArea CreateNew()
	{
		return new StiLineArea();
	}

	[StiUniversalConstructor("Area")]
	public StiLineArea()
	{
		base.Core = new StiLineAreaCoreXF(this);
	}
}
