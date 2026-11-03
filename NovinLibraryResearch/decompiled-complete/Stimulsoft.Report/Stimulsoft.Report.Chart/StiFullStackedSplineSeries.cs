using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiFullStackedSplineSeries : StiStackedSplineSeries, IStiFullStackedSplineSeries, IStiStackedSplineSeries, IStiStackedBaseLineSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiShowNullsSeries, IStiAllowApplyColorNegative
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiFullStackedSplineSeries;

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
		value = new StiPropertyObject[6]
		{
			propertiesHelper.Lighting(),
			propertiesHelper.LineColor(),
			propertiesHelper.LineColorNegative(),
			propertiesHelper.LineStyle(),
			propertiesHelper.LineWidth(),
			propertiesHelper.ShowShadow()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, value);
		value = new StiPropertyObject[8]
		{
			propertiesHelper.AllowApplyStyle(),
			propertiesHelper.AllowApplyColorNegative(),
			propertiesHelper.ShowInLegend(),
			propertiesHelper.ShowNulls(),
			propertiesHelper.ShowSeriesLabels(),
			propertiesHelper.Tension(),
			propertiesHelper.Title(),
			propertiesHelper.YAxis()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Behavior, value);
		return stiPropertyCollection;
	}

	public override Type GetDefaultAreaType()
	{
		return typeof(StiFullStackedColumnArea);
	}

	public override StiSeries CreateNew()
	{
		return new StiFullStackedSplineSeries();
	}

	public StiFullStackedSplineSeries()
	{
		base.Core = new StiFullStackedSplineSeriesCoreXF(this);
	}
}
