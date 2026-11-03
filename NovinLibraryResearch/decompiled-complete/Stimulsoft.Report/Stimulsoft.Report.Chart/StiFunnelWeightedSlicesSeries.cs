using System;
using System.ComponentModel;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public class StiFunnelWeightedSlicesSeries : StiFunnelSeries, IStiFunnelWeightedSlicesSeries, IStiFunnelSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiSeriesBorderThickness, IStiFontIconsSeries, IStiShowZerosSeries
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiFunnelWeightedSlicesSeries;

	public override object Clone()
	{
		return base.Clone() as IStiFunnelWeightedSlicesSeries;
	}

	public override Type GetDefaultAreaType()
	{
		return typeof(StiFunnelWeightedSlicesArea);
	}

	public override StiSeries CreateNew()
	{
		return new StiFunnelWeightedSlicesSeries();
	}

	public StiFunnelWeightedSlicesSeries()
	{
		base.Core = new StiFunnelWeightedSlicesSeriesCoreXF(this);
	}
}
