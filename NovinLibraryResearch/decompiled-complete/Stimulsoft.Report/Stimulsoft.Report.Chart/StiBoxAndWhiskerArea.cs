using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiBoxAndWhiskerArea : StiAxisArea, IStiBoxAndWhiskerArea, IStiAxisArea, IStiArea, ICloneable, IStiJsonReportObject
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiBoxAndWhiskerArea;

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiBoxAndWhiskerSeries);
	}

	public override Type[] GetSeriesTypes()
	{
		return new Type[1] { typeof(StiBoxAndWhiskerSeries) };
	}

	public override Type[] GetSeriesLabelsTypes()
	{
		return new Type[4]
		{
			typeof(StiNoneLabels),
			typeof(StiCenterAxisLabels),
			typeof(StiLeftAxisLabels),
			typeof(StiRightAxisLabels)
		};
	}

	public override StiArea CreateNew()
	{
		return new StiBoxAndWhiskerArea();
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		stiPropertyCollection.Add(StiPropertyCategories.Main, new StiPropertyObject[1] { propertyGrid.PropertiesHelper.BoxAndWhiskerArea() });
		return stiPropertyCollection;
	}

	[StiUniversalConstructor("Area")]
	public StiBoxAndWhiskerArea()
	{
		base.Core = new StiBoxAndWhiskerAreaCoreXF(this);
	}
}
