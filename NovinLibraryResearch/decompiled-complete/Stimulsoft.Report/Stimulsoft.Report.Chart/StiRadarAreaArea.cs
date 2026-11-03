using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiRadarAreaArea : StiRadarArea, IStiRadarAreaArea, IStiRadarArea, IStiArea, ICloneable, IStiJsonReportObject
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiRadarAreaArea;

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		stiPropertyCollection.Add(StiPropertyCategories.Main, new StiPropertyObject[1] { propertyGrid.PropertiesHelper.RadarAreaArea() });
		return stiPropertyCollection;
	}

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiRadarAreaSeries);
	}

	public override Type[] GetSeriesTypes()
	{
		return new Type[3]
		{
			typeof(StiRadarPointSeries),
			typeof(StiRadarLineSeries),
			typeof(StiRadarAreaSeries)
		};
	}

	public override StiArea CreateNew()
	{
		return new StiRadarAreaArea();
	}

	[StiUniversalConstructor("Area")]
	public StiRadarAreaArea()
	{
		base.Core = new StiRadarAreaAreaCoreXF(this);
	}
}
