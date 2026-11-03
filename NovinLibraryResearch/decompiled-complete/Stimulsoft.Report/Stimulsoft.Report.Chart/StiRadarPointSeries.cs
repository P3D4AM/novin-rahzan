using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiRadarPointSeries : StiRadarSeries, IStiRadarPointSeries, IStiRadarSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiShowNullsSeries
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiRadarPointSeries;

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[3]
		{
			propertiesHelper.ValueDataColumn(),
			propertiesHelper.Value(),
			propertiesHelper.ListOfValues()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Value, value);
		value = new StiPropertyObject[3]
		{
			propertiesHelper.ArgumentDataColumn(),
			propertiesHelper.Argument(),
			propertiesHelper.ListOfArguments()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Argument, value);
		value = new StiPropertyObject[6]
		{
			propertiesHelper.Format(),
			propertiesHelper.SortBy(),
			propertiesHelper.SortDirection(),
			propertiesHelper.AutoSeriesKeyDataColumn(),
			propertiesHelper.AutoSeriesColorDataColumn(),
			propertiesHelper.AutoSeriesTitleDataColumn()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Data, value);
		value = new StiPropertyObject[1] { propertiesHelper.ShowShadow() };
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, value);
		value = new StiPropertyObject[6]
		{
			propertiesHelper.AllowApplyStyle(),
			propertiesHelper.ShowInLegend(),
			propertiesHelper.ShowNulls(),
			propertiesHelper.ShowSeriesLabels(),
			propertiesHelper.Title(),
			propertiesHelper.YAxis()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Behavior, value);
		return stiPropertyCollection;
	}

	public override Type GetDefaultAreaType()
	{
		return typeof(StiRadarPointArea);
	}

	public override StiSeries CreateNew()
	{
		return new StiRadarPointSeries();
	}

	public StiRadarPointSeries()
	{
		base.Core = new StiRadarPointSeriesCoreXF(this);
	}
}
