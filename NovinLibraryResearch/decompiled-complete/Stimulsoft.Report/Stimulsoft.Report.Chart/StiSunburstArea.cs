using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiSunburstArea : StiArea, IStiSunburstArea, IStiArea, ICloneable, IStiJsonReportObject
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiSunburstArea;

	[Browsable(false)]
	public override bool ColorEach => false;

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		return new StiPropertyCollection();
	}

	public override Type GetDefaultSeriesLabelsType()
	{
		return typeof(StiCenterPieLabels);
	}

	public override Type[] GetSeriesLabelsTypes()
	{
		return new Type[2]
		{
			typeof(StiNoneLabels),
			typeof(StiCenterPieLabels)
		};
	}

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiSunburstSeries);
	}

	public override Type[] GetSeriesTypes()
	{
		return new Type[1] { typeof(StiSunburstSeries) };
	}

	public override StiArea CreateNew()
	{
		return new StiSunburstArea();
	}

	[StiUniversalConstructor("Area")]
	public StiSunburstArea()
	{
		base.Core = new StiSunburstAreaCoreXF(this);
	}
}
