using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiClusteredColumnSeries : StiSeries, IStiClusteredColumnSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiAllowApplyBrushNegative, IStiFontIconsSeries, IStiSeriesBorderThickness, IStiShowZerosSeries
{
	private bool showZeros;

	private float width = 0.9f;

	private Color borderColor = Color.Gray;

	private StiBrush brush = (StiBrush)new StiSolidBrush(Color.Gainsboro);

	private StiBrush brushNegative = (StiBrush)new StiSolidBrush(Color.Firebrick);

	private bool allowApplyBrushNegative;

	public override StiComponentId ComponentId => StiComponentId.StiClusteredColumnSeries;

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(false)]
	[Description("Gets or sets value which indicates whether it is necessary to show the series element, if the series value of this column is 0.")]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool ShowZeros
	{
		get
		{
			return showZeros;
		}
		set
		{
			showZeros = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(0.9f)]
	[Description("Gets or sets the width factor of one bar series. Value 1 is equal to 100%.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual float Width
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

	[StiSerializable]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets border color of series bar.")]
	[TypeConverter(typeof(StiColorConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public Color BorderColor
	{
		get
		{
			return borderColor;
		}
		set
		{
			borderColor = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets border thickness of series bar.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public int BorderThickness { get; set; } = 1;


	[RefreshProperties(RefreshProperties.All)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets brush which will used to fill bar area.")]
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
	public virtual StiBrush BrushNegative
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
	public virtual bool AllowApplyBrushNegative
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

	[DefaultValue(null)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Design.StiFontIconEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiFontIconConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiFontIcons? Icon { get; set; }

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyBool(val, "ShowZeros", ShowZeros, false, false);
		JsonExtensions.AddPropertyFloat(val, "Width", width, 0.9f);
		JsonExtensions.AddPropertyColor(val, "BorderColor", borderColor, Color.Gray);
		JsonExtensions.AddPropertyInt(val, "BorderThickness", BorderThickness, 1);
		JsonExtensions.AddPropertyBrush(val, "Brush", brush);
		JsonExtensions.AddPropertyBrush(val, "BrushNegative", BrushNegative);
		JsonExtensions.AddPropertyBool(val, "AllowApplyBrushNegative", AllowApplyBrushNegative, false, false);
		if (Icon.HasValue)
		{
			JsonExtensions.AddPropertyEnum(val, "Icon", (Enum)(object)Icon);
		}
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "ShowZeros":
				ShowZeros = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Width":
				width = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "BorderColor":
				borderColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "BorderThickness":
				BorderThickness = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "Brush":
				brush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "BrushNegative":
				BrushNegative = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "AllowApplyBrushNegative":
				AllowApplyBrushNegative = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Icon":
				Icon = StiJsonReportObjectHelper.DeserializeEnum<StiFontIcons>(item);
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

	public override object Clone()
	{
		IStiClusteredColumnSeries stiClusteredColumnSeries = base.Clone() as IStiClusteredColumnSeries;
		object obj = Brush.Clone();
		stiClusteredColumnSeries.Brush = (StiBrush)((obj is StiBrush) ? obj : null);
		return stiClusteredColumnSeries;
	}

	public override Type GetDefaultAreaType()
	{
		return typeof(StiClusteredColumnArea);
	}

	public override StiSeries CreateNew()
	{
		return new StiClusteredColumnSeries();
	}

	public StiClusteredColumnSeries()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		base.Core = new StiClusteredColumnSeriesCoreXF(this);
	}
}
