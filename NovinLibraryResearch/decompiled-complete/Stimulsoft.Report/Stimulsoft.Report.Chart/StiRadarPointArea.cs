using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiRadarPointArea : StiRadarArea, IStiRadarPointArea, IStiRadarArea, IStiArea, ICloneable, IStiJsonReportObject
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiRadarPointArea;

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		stiPropertyCollection.Add(StiPropertyCategories.Main, new StiPropertyObject[1] { propertyGrid.PropertiesHelper.RadarPointArea() });
		return stiPropertyCollection;
	}

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiRadarPointSeries);
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
		return new StiRadarPointArea();
	}

	[StiUniversalConstructor("Area")]
	public StiRadarPointArea()
	{
		base.Core = new StiRadarPointAreaCoreXF(this);
	}
}
