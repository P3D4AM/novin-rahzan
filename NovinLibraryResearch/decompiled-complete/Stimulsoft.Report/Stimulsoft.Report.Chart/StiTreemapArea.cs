using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiTreemapArea : StiArea, IStiTreemapArea, IStiArea, ICloneable, IStiJsonReportObject
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiTreemapArea;

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		stiPropertyCollection.Add(StiPropertyCategories.Main, new StiPropertyObject[1] { propertyGrid.PropertiesHelper.TreemapArea() });
		return stiPropertyCollection;
	}

	public override Type GetDefaultSeriesLabelsType()
	{
		return typeof(StiNoneLabels);
	}

	public override Type[] GetSeriesLabelsTypes()
	{
		return new Type[2]
		{
			typeof(StiNoneLabels),
			typeof(StiCenterTreemapLabels)
		};
	}

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiTreemapSeries);
	}

	public override Type[] GetSeriesTypes()
	{
		return new Type[1] { typeof(StiTreemapSeries) };
	}

	public override StiArea CreateNew()
	{
		return new StiTreemapArea();
	}

	[StiUniversalConstructor("Area")]
	public StiTreemapArea()
	{
		base.Core = new StiTreemapAreaCoreXF(this);
	}
}
