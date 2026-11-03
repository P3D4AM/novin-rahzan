using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiStackedColumnArea : StiAxisArea, IStiStackedColumnArea, IStiAxisArea, IStiArea, ICloneable, IStiJsonReportObject
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiStackedColumnArea;

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		stiPropertyCollection.Add(StiPropertyCategories.Main, new StiPropertyObject[1] { propertyGrid.PropertiesHelper.StackedColumnArea() });
		return stiPropertyCollection;
	}

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiStackedColumnSeries);
	}

	public override Type[] GetSeriesTypes()
	{
		return new Type[5]
		{
			typeof(StiStackedColumnSeries),
			typeof(StiStackedLineSeries),
			typeof(StiStackedSplineSeries),
			typeof(StiStackedAreaSeries),
			typeof(StiStackedSplineAreaSeries)
		};
	}

	public override StiArea CreateNew()
	{
		return new StiStackedColumnArea();
	}

	[StiUniversalConstructor("Area")]
	public StiStackedColumnArea()
	{
		base.Core = new StiStackedColumnAreaCoreXF(this);
	}
}
