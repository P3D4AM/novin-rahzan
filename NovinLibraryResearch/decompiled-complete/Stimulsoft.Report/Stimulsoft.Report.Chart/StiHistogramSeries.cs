using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiHistogramSeries : StiClusteredColumnSeries, IStiHistogramSeries, IStiClusteredColumnSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiAllowApplyBrushNegative, IStiFontIconsSeries, IStiSeriesBorderThickness, IStiShowZerosSeries
{
	private float width = 0.98f;

	public override StiComponentId ComponentId => StiComponentId.StiHistogramSeries;

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(0.98f)]
	[Description("Gets or sets the width factor of one column series. Value 1 is equal to 100%.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public override float Width
	{
		get
		{
			return width;
		}
		set
		{
			if (value >= 0.01f && value <= 1f)
			{
				width = value;
			}
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
		return stiPropertyCollection;
	}

	public override Type GetDefaultAreaType()
	{
		return typeof(StiHistogramArea);
	}

	public override StiSeries CreateNew()
	{
		return new StiHistogramSeries();
	}

	public StiHistogramSeries()
	{
		base.Core = new StiHistogramSeriesCoreXF(this);
	}
}
