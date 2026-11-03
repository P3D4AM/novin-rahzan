using System;
using System.ComponentModel;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public class StiFunnelWeightedSlicesArea : StiFunnelArea
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiFunnelWeightedSlicesArea;

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiFunnelWeightedSlicesSeries);
	}

	public override Type[] GetSeriesTypes()
	{
		return new Type[1] { typeof(StiFunnelWeightedSlicesSeries) };
	}

	public override Type[] GetSeriesLabelsTypes()
	{
		return new Type[2]
		{
			typeof(StiNoneLabels),
			typeof(StiCenterFunnelLabels)
		};
	}

	public override StiArea CreateNew()
	{
		return new StiFunnelWeightedSlicesArea();
	}

	[StiUniversalConstructor("Area")]
	public StiFunnelWeightedSlicesArea()
	{
		base.Core = new StiFunnelAreaCoreXF(this);
		ColorEach = true;
	}
}
