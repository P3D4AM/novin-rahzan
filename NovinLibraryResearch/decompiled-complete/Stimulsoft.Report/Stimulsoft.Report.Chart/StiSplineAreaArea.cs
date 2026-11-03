using System;
using System.ComponentModel;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public class StiSplineAreaArea : StiClusteredColumnArea, IStiSplineAreaArea, IStiClusteredColumnArea, IStiAxisArea, IStiArea, ICloneable, IStiJsonReportObject, IStiRoundValuesArea
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiSplineAreaArea;

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiSplineAreaSeries);
	}

	public override StiArea CreateNew()
	{
		return new StiSplineAreaArea();
	}

	[StiUniversalConstructor("Area")]
	public StiSplineAreaArea()
	{
		base.Core = new StiSplineAreaAreaCoreXF(this);
	}
}
