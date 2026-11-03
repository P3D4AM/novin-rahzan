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
using Stimulsoft.Report.Chart.Design;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiParetoSeries : StiSeries, IStiParetoSeries, IStiClusteredColumnSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiAllowApplyBrushNegative, IStiFontIconsSeries, IStiSeriesBorderThickness, IStiShowZerosSeries, IStiBaseLineSeries, IStiAllowApplyColorNegative, IStiShowNullsSeries
{
	private float width = 0.9f;

	private Color borderColor = Color.Gray;

	private StiBrush brush = (StiBrush)new StiSolidBrush(Color.Gainsboro);

	private IStiMarker marker = new StiMarker();

	private IStiLineMarker lineMarker = new StiLineMarker();

	private Color lineColor = Color.Black;

	private StiPenStyle lineStyle;

	private bool lighting = true;

	private float lineWidth = 2f;

	private StiSeriesSortType sortBy;

	private StiSeriesSortDirection sortDirection = StiSeriesSortDirection.Descending;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiParetoSeries;

	[StiNonSerialized]
	[Browsable(false)]
	public bool ShowZeros { get; set; }

	[StiNonSerialized]
	[Browsable(false)]
	public virtual StiBrush BrushNegative { get; set; }

	[StiNonSerialized]
	[Browsable(false)]
	public virtual bool AllowApplyBrushNegative { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(0.9f)]
	[Description("Gets or sets the width factor of one bar series. Value 1 is equal to 100%.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public float Width
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
	public int BorderThickness { get; set; }

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

	[StiNonSerialized]
	[Browsable(false)]
	public virtual bool ShowNulls { get; set; }

	[StiNonSerialized]
	[Browsable(false)]
	public virtual Color LineColorNegative { get; set; }

	[StiNonSerialized]
	[Browsable(false)]
	public virtual bool AllowApplyColorNegative { get; set; }

	[StiNonSerialized]
	[Browsable(false)]
	public int LabelsOffset { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets marker settings.")]
	[Browsable(false)]
	[TypeConverter(typeof(StiMarkerConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual IStiMarker Marker
	{
		get
		{
			return marker;
		}
		set
		{
			marker = value;
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets line marker settings.")]
	[Browsable(false)]
	[TypeConverter(typeof(StiLineMarkerConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual IStiLineMarker LineMarker
	{
		get
		{
			return lineMarker;
		}
		set
		{
			lineMarker = value;
		}
	}

	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets line color of series.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual Color LineColor
	{
		get
		{
			return lineColor;
		}
		set
		{
			lineColor = value;
		}
	}

	[Editor("Stimulsoft.Base.Drawing.Design.StiPenStyleEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a line style of series.")]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiPenStyle LineStyle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return lineStyle;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			lineStyle = value;
		}
	}

	[DefaultValue(true)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that light effect will be shown.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool Lighting
	{
		get
		{
			return lighting;
		}
		set
		{
			lighting = value;
		}
	}

	[DefaultValue(2f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets line width of series.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual float LineWidth
	{
		get
		{
			return lineWidth;
		}
		set
		{
			if (value > 0f)
			{
				lineWidth = value;
			}
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that pareto line color will be used.")]
	[DefaultValue(false)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool AllowApplyLineColor { get; set; }

	[DefaultValue(StiSeriesSortType.Value)]
	[StiSerializable]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets mode of series values sorting.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public override StiSeriesSortType SortBy
	{
		get
		{
			return sortBy;
		}
		set
		{
			sortBy = value;
		}
	}

	[DefaultValue(StiSeriesSortDirection.Descending)]
	[StiSerializable]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets sort direction.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public override StiSeriesSortDirection SortDirection
	{
		get
		{
			return sortDirection;
		}
		set
		{
			sortDirection = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public StiShowEmptyCellsAs ShowNullsAs { get; set; }

	[StiNonSerialized]
	[Browsable(false)]
	public StiShowEmptyCellsAs ShowZerosAs { get; set; }

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
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyFloat(val, "Width", width, 0.9f);
		JsonExtensions.AddPropertyColor(val, "BorderColor", borderColor, Color.Gray);
		JsonExtensions.AddPropertyBrush(val, "Brush", brush);
		JsonExtensions.RemoveProperty(val, "Conditions");
		JsonExtensions.AddPropertyJObject(val, "Marker", ((IStiJsonReportObject)Marker).SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "LineMarker", ((IStiJsonReportObject)LineMarker).SaveToJsonObject(mode));
		JsonExtensions.AddPropertyColor(val, "LineColor", LineColor, Color.Black);
		JsonExtensions.AddPropertyEnum(val, "LineStyle", (Enum)(object)LineStyle, (Enum)(object)(StiPenStyle)0);
		JsonExtensions.AddPropertyBool(val, "Lighting", Lighting, true, false);
		JsonExtensions.AddPropertyFloat(val, "LineWidth", LineWidth, 2f);
		JsonExtensions.AddPropertyBool(val, "AllowApplyLineColor", AllowApplyLineColor, false, false);
		if (Icon.HasValue)
		{
			JsonExtensions.AddPropertyEnum(val, "Icon", (Enum)(object)Icon);
		}
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Expected O, but got Unknown
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Expected O, but got Unknown
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Width":
				width = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "BorderColor":
				borderColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "Brush":
				brush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "Marker":
				((IStiJsonReportObject)Marker).LoadFromJsonObject((JObject)item.Value);
				break;
			case "LineMarker":
				((IStiJsonReportObject)LineMarker).LoadFromJsonObject((JObject)item.Value);
				break;
			case "LineColor":
				LineColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "LineStyle":
				LineStyle = StiJsonReportObjectHelper.DeserializeEnum<StiPenStyle>(item);
				break;
			case "Lighting":
				Lighting = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "LineWidth":
				LineWidth = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "AllowApplyLineColor":
				AllowApplyLineColor = StiJsonReportObjectHelper.DeserializeBool(item);
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
		value = new StiPropertyObject[4]
		{
			propertiesHelper.Format(),
			propertiesHelper.AutoSeriesKeyDataColumn(),
			propertiesHelper.AutoSeriesColorDataColumn(),
			propertiesHelper.AutoSeriesTitleDataColumn()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Data, value);
		value = new StiPropertyObject[8]
		{
			propertiesHelper.BorderColor(),
			propertiesHelper.Brush(),
			propertiesHelper.ShowShadow(),
			propertiesHelper.LabelsOffset(),
			propertiesHelper.Lighting(),
			propertiesHelper.LineColor(),
			propertiesHelper.LineStyle(),
			propertiesHelper.LineWidth()
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
		return stiPropertyCollection;
	}

	public override object Clone()
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		IStiParetoSeries stiParetoSeries = base.Clone() as IStiParetoSeries;
		object obj = Brush.Clone();
		stiParetoSeries.Brush = (StiBrush)((obj is StiBrush) ? obj : null);
		stiParetoSeries.Marker = Marker.Clone() as IStiMarker;
		stiParetoSeries.LineStyle = LineStyle;
		return stiParetoSeries;
	}

	public override Type GetDefaultAreaType()
	{
		return typeof(StiParetoArea);
	}

	public override StiSeries CreateNew()
	{
		return new StiParetoSeries();
	}

	public StiParetoSeries()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		base.Core = new StiParetoSeriesCoreXF(this);
	}
}
