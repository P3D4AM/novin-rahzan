using System;
using System.ComponentModel;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public class StiParetoArea : StiClusteredColumnArea, IStiParetoArea, IStiClusteredColumnArea, IStiAxisArea, IStiArea, ICloneable, IStiJsonReportObject, IStiRoundValuesArea
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiParetoArea;

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiParetoSeries);
	}

	public override Type[] GetSeriesTypes()
	{
		return new Type[1] { typeof(StiParetoSeries) };
	}

	public override StiArea CreateNew()
	{
		return new StiParetoArea();
	}

	[StiUniversalConstructor("Area")]
	public StiParetoArea()
	{
		base.YRightAxis = new StiYRightAxis
		{
			Visible = true
		};
		base.YRightAxis.Range.Auto = false;
		base.YRightAxis.Range.Maximum = 100.0;
		base.YRightAxis.Labels.TextAfter = "%";
		base.Core = new StiParetoAreaCoreXF(this);
	}
}
