using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiHistogramArea : StiAxisArea
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiHistorgamArea;

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		stiPropertyCollection.Add(StiPropertyCategories.Main, new StiPropertyObject[1] { propertyGrid.PropertiesHelper.ClusteredColumnArea() });
		return stiPropertyCollection;
	}

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiHistogramSeries);
	}

	public override Type[] GetSeriesTypes()
	{
		return new Type[1] { typeof(StiHistogramSeries) };
	}

	public override StiArea CreateNew()
	{
		return new StiHistogramArea();
	}

	[StiUniversalConstructor("Area")]
	public StiHistogramArea()
	{
		base.Core = new StiHistogramAreaCoreXF(this);
	}
}
