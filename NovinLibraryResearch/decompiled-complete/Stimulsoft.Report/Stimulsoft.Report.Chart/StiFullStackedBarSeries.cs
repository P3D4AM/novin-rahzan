using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiFullStackedBarSeries : StiStackedBarSeries, IStiFullStackedBarSeries, IStiStackedBarSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiSeriesBorderThickness, IStiFontIconsSeries, IStiAllowApplyBrushNegative, IStiShowZerosSeries
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiFullStackedBarSeries;

	[Browsable(false)]
	public override bool ShowZeros
	{
		get
		{
			return base.ShowZeros;
		}
		set
		{
			base.ShowZeros = value;
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[8]
		{
			propertiesHelper.Conditions(),
			propertiesHelper.Filters(),
			propertiesHelper.Format(),
			propertiesHelper.SortBy(),
			propertiesHelper.SortDirection(),
			propertiesHelper.AutoSeriesKeyDataColumn(),
			propertiesHelper.AutoSeriesColorDataColumn(),
			propertiesHelper.AutoSeriesTitleDataColumn()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Data, value);
		value = new StiPropertyObject[3]
		{
			propertiesHelper.BorderColor(),
			propertiesHelper.Brush(),
			propertiesHelper.ShowShadow()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, value);
		value = new StiPropertyObject[6]
		{
			propertiesHelper.AllowApplyStyle(),
			propertiesHelper.ShowInLegend(),
			propertiesHelper.ShowSeriesLabels(),
			propertiesHelper.Title(),
			propertiesHelper.YAxis(),
			propertiesHelper.fWidth()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Behavior, value);
		value = new StiPropertyObject[3]
		{
			propertiesHelper.ArgumentDataColumn(),
			propertiesHelper.Argument(),
			propertiesHelper.ListOfArguments()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Argument, value);
		value = new StiPropertyObject[3]
		{
			propertiesHelper.ValueDataColumn(),
			propertiesHelper.Value(),
			propertiesHelper.ListOfValues()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Value, value);
		return stiPropertyCollection;
	}

	public override Type GetDefaultAreaType()
	{
		return typeof(StiFullStackedBarArea);
	}

	public override StiSeries CreateNew()
	{
		return new StiFullStackedBarSeries();
	}

	public StiFullStackedBarSeries()
	{
		base.Core = new StiFullStackedBarSeriesCoreXF(this);
	}
}
