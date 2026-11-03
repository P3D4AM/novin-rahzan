using System;
using System.ComponentModel;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public class StiAreaArea : StiClusteredColumnArea, IStiAreaArea, IStiClusteredColumnArea, IStiAxisArea, IStiArea, ICloneable, IStiJsonReportObject, IStiRoundValuesArea
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiAreaArea;

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiAreaSeries);
	}

	public override StiArea CreateNew()
	{
		return new StiAreaArea();
	}

	[StiUniversalConstructor("Area")]
	public StiAreaArea()
	{
		base.Core = new StiAreaAreaCoreXF(this);
	}
}
