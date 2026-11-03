using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiClusteredBarSeries : StiClusteredColumnSeries, IStiClusteredBarSeries, IStiClusteredColumnSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiAllowApplyBrushNegative, IStiFontIconsSeries, IStiSeriesBorderThickness, IStiShowZerosSeries
{
	public override StiComponentId ComponentId => StiComponentId.StiClusteredBarSeries;

	[Browsable(false)]
	public override StiSeriesYAxis YAxis { get; set; }

	[StiSerializable]
	[DefaultValue(StiSeriesXAxis.BottomXAxis)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets X Axis for series on which will output string representation of arguments.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiSeriesXAxis XAxis
	{
		get
		{
			if (YAxis == StiSeriesYAxis.LeftYAxis)
			{
				return StiSeriesXAxis.BottomXAxis;
			}
			return StiSeriesXAxis.TopXAxis;
		}
		set
		{
			YAxis = ((value != 0) ? StiSeriesYAxis.RightYAxis : StiSeriesYAxis.LeftYAxis);
		}
	}

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
		value = new StiPropertyObject[4]
		{
			propertiesHelper.BorderColor(),
			propertiesHelper.Brush(),
			propertiesHelper.BrushNegative(),
			propertiesHelper.ShowShadow()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, value);
		value = new StiPropertyObject[8]
		{
			propertiesHelper.AllowApplyStyle(),
			propertiesHelper.AllowApplyBrushNegative(),
			propertiesHelper.ShowInLegend(),
			propertiesHelper.ShowSeriesLabels(),
			propertiesHelper.ShowZeros(),
			propertiesHelper.Title(),
			propertiesHelper.YAxis(),
			propertiesHelper.fWidth()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Behavior, value);
		return stiPropertyCollection;
	}

	public override Type GetDefaultAreaType()
	{
		return typeof(StiClusteredBarArea);
	}

	public override StiSeries CreateNew()
	{
		return new StiClusteredBarSeries();
	}

	public StiClusteredBarSeries()
	{
		base.Core = new StiClusteredBarSeriesCoreXF(this);
	}
}
