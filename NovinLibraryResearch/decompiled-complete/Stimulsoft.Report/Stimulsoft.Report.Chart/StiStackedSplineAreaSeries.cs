using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiStackedSplineAreaSeries : StiStackedSplineSeries, IStiStackedSplineAreaSeries, IStiStackedSplineSeries, IStiStackedBaseLineSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiShowNullsSeries, IStiAllowApplyColorNegative, IStiAllowApplyBrushNegative
{
	private StiBrush brush = (StiBrush)new StiSolidBrush(Color.Gainsboro);

	private StiBrush brushNegative = (StiBrush)new StiSolidBrush(Color.Firebrick);

	private bool allowApplyBrushNegative;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiStackedSplineAreaSeries;

	[RefreshProperties(RefreshProperties.All)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiBrush Brush
	{
		get
		{
			return brush;
		}
		set
		{
			brush = value;
		}
	}

	[RefreshProperties(RefreshProperties.All)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a brush which will be used to fill negative values.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiBrush BrushNegative
	{
		get
		{
			return brushNegative;
		}
		set
		{
			brushNegative = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value which indicates that the specific brush for filling negative values will be used.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public bool AllowApplyBrushNegative
	{
		get
		{
			return allowApplyBrushNegative;
		}
		set
		{
			allowApplyBrushNegative = value;
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyBrush(val, "Brush", brush);
		JsonExtensions.AddPropertyBrush(val, "BrushNegative", brushNegative);
		JsonExtensions.AddPropertyBool(val, "AllowApplyBrushNegative", allowApplyBrushNegative, false, false);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Brush":
				brush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "BrushNegative":
				brushNegative = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "AllowApplyBrushNegative":
				allowApplyBrushNegative = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
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
		value = new StiPropertyObject[8]
		{
			propertiesHelper.Brush(),
			propertiesHelper.Lighting(),
			propertiesHelper.LineColor(),
			propertiesHelper.LineColorNegative(),
			propertiesHelper.LineStyle(),
			propertiesHelper.LineWidth(),
			propertiesHelper.BrushNegative(),
			propertiesHelper.ShowShadow()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, value);
		value = new StiPropertyObject[9]
		{
			propertiesHelper.AllowApplyStyle(),
			propertiesHelper.AllowApplyBrushNegative(),
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

	public override object Clone()
	{
		IStiStackedSplineAreaSeries stiStackedSplineAreaSeries = base.Clone() as IStiStackedSplineAreaSeries;
		object obj = Brush.Clone();
		stiStackedSplineAreaSeries.Brush = (StiBrush)((obj is StiBrush) ? obj : null);
		return stiStackedSplineAreaSeries;
	}

	public override Type GetDefaultAreaType()
	{
		return typeof(StiStackedColumnArea);
	}

	public override StiSeries CreateNew()
	{
		return new StiStackedSplineAreaSeries();
	}

	public StiStackedSplineAreaSeries()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Expected O, but got Unknown
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		base.Core = new StiStackedSplineAreaSeriesCoreXF(this);
	}
}
