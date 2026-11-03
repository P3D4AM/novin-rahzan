using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Images;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report;

public class StiChartStyle : StiBaseStyle
{
	private Color[] DefaultColors = new Color[6]
	{
		Color.FromArgb(112, 173, 71),
		Color.FromArgb(68, 114, 196),
		Color.FromArgb(255, 192, 0),
		Color.FromArgb(67, 104, 43),
		Color.FromArgb(253, 106, 55),
		Color.FromArgb(153, 115, 0)
	};

	public override StiComponentId ComponentId => StiComponentId.StiChartStyle;

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[Description("The appearance and behavior of the component border.")]
	public StiBorder Border { get; set; } = new StiBorder();


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[Description("Gets or sets a brush to fill the style.")]
	public StiBrush Brush { get; set; } = (StiBrush)new StiSolidBrush(Color.White);


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	public StiBrush ChartAreaBrush { get; set; } = (StiBrush)new StiSolidBrush(Color.White);


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color ChartAreaBorderColor { get; set; } = Color.FromArgb(171, 172, 173);


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[DefaultValue(false)]
	public bool ChartAreaShowShadow { get; set; }

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	public StiBrush InterlacingHorBrush { get; set; } = (StiBrush)new StiSolidBrush(Color.FromArgb(10, 155, 155, 155));


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	public StiBrush InterlacingVertBrush { get; set; } = (StiBrush)new StiSolidBrush(Color.FromArgb(10, 155, 155, 155));


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color GridLinesHorColor { get; set; } = Color.FromArgb(100, 105, 105, 105);


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color GridLinesVertColor { get; set; } = Color.FromArgb(100, 105, 105, 105);


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[DefaultValue(false)]
	public bool SeriesLighting { get; set; }

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[DefaultValue(false)]
	public bool SeriesShowShadow { get; set; }

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[DefaultValue(false)]
	public bool SeriesShowBorder { get; set; }

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[DefaultValue(1)]
	public int SeriesBorderThickness { get; set; } = 1;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	public StiBrush SeriesLabelsBrush { get; set; } = (StiBrush)new StiSolidBrush(Color.White);


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color SeriesLabelsColor { get; set; } = Color.FromArgb(90, 90, 90);


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color SeriesLabelsBorderColor { get; set; } = Color.FromArgb(140, 140, 140);


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color SeriesLabelsLineColor { get; set; } = Color.FromArgb(140, 140, 140);


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color TrendLineColor { get; set; } = Color.FromArgb(140, 140, 140);


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[DefaultValue(false)]
	public bool TrendLineShowShadow { get; set; }

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	public StiBrush LegendBrush { get; set; } = (StiBrush)new StiSolidBrush(Color.White);


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color LegendLabelsColor { get; set; } = Color.FromArgb(140, 140, 140);


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color LegendBorderColor { get; set; } = Color.DimGray;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color LegendTitleColor { get; set; } = Color.DimGray;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color AxisTitleColor { get; set; } = Color.FromArgb(140, 140, 140);


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color AxisLineColor { get; set; } = Color.FromArgb(140, 140, 140);


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color AxisLabelsColor { get; set; } = Color.FromArgb(140, 140, 140);


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[DefaultValue(true)]
	public bool MarkerVisible { get; set; } = true;


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(StiBrushType.Solid)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets a value which indicates which type of brush report engine will be used to draw this style.")]
	public StiBrushType BrushType { get; set; }

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[Description("Gets or sets a list of colors which will be used for drawing chart series.")]
	[TypeConverter(typeof(StiStyleColorsConverter))]
	[Editor("Stimulsoft.Report.Components.Design.StiColorsCollectionEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color[] StyleColors { get; set; }

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets a base color for drawing this style.")]
	public Color BasicStyleColor { get; set; } = Color.WhiteSmoke;


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value which indicates whether a report engine can use Border formatting or not.")]
	public bool AllowUseBorderFormatting { get; set; } = true;


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value which indicates whether a report engine can use Border Sides or not.")]
	public bool AllowUseBorderSides { get; set; } = true;


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value which indicates whether a report engine can use Brush formatting or not.")]
	public bool AllowUseBrush { get; set; } = true;


	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyBorder(val, "Border", Border);
		JsonExtensions.AddPropertyBrush(val, "Brush", Brush);
		JsonExtensions.AddPropertyBrush(val, "ChartAreaBrush", ChartAreaBrush);
		JsonExtensions.AddPropertyColor(val, "ChartAreaBorderColor", ChartAreaBorderColor, Color.FromArgb(171, 172, 173));
		JsonExtensions.AddPropertyBool(val, "ChartAreaShowShadow", ChartAreaShowShadow, false, false);
		JsonExtensions.AddPropertyBool(val, "SeriesLighting", SeriesLighting, false, false);
		JsonExtensions.AddPropertyBool(val, "SeriesShowShadow", SeriesShowShadow, false, false);
		JsonExtensions.AddPropertyBool(val, "SeriesShowBorder", SeriesShowBorder, false, false);
		JsonExtensions.AddPropertyInt(val, "SeriesBorderThickness", SeriesBorderThickness, 1);
		JsonExtensions.AddPropertyBrush(val, "SeriesLabelsBrush", SeriesLabelsBrush);
		JsonExtensions.AddPropertyColor(val, "SeriesLabelsColor", SeriesLabelsColor, Color.FromArgb(90, 90, 90));
		JsonExtensions.AddPropertyColor(val, "SeriesLabelsLineColor", SeriesLabelsLineColor, Color.FromArgb(140, 140, 140));
		JsonExtensions.AddPropertyColor(val, "SeriesLabelsBorderColor", SeriesLabelsBorderColor, Color.FromArgb(90, 90, 90));
		JsonExtensions.AddPropertyColor(val, "TrendLineColor", TrendLineColor, Color.FromArgb(140, 140, 140));
		JsonExtensions.AddPropertyBool(val, "TrendLineShowShadow", TrendLineShowShadow, false, false);
		JsonExtensions.AddPropertyBrush(val, "LegendBrush", LegendBrush);
		JsonExtensions.AddPropertyColor(val, "LegendLabelsColor", LegendLabelsColor, Color.FromArgb(140, 140, 140));
		JsonExtensions.AddPropertyColor(val, "LegendBorderColor", LegendBorderColor, Color.DimGray);
		JsonExtensions.AddPropertyColor(val, "LegendTitleColor", LegendTitleColor, Color.DimGray);
		JsonExtensions.AddPropertyColor(val, "AxisTitleColor", AxisTitleColor, Color.FromArgb(140, 140, 140));
		JsonExtensions.AddPropertyColor(val, "AxisLineColor", AxisLineColor, Color.FromArgb(140, 140, 140));
		JsonExtensions.AddPropertyColor(val, "AxisLabelsColor", AxisLabelsColor, Color.FromArgb(140, 140, 140));
		JsonExtensions.AddPropertyBool(val, "MarkerVisible", MarkerVisible, true, false);
		JsonExtensions.AddPropertyBrush(val, "InterlacingHorBrush", InterlacingHorBrush);
		JsonExtensions.AddPropertyBrush(val, "InterlacingVertBrush", InterlacingVertBrush);
		JsonExtensions.AddPropertyColor(val, "GridLinesHorColor", GridLinesHorColor, Color.FromArgb(100, 105, 105, 105));
		JsonExtensions.AddPropertyColor(val, "GridLinesVertColor", GridLinesVertColor, Color.FromArgb(100, 105, 105, 105));
		JsonExtensions.AddPropertyEnum(val, "BrushType", (Enum)BrushType, (Enum)StiBrushType.Solid);
		JsonExtensions.AddPropertyArrayColor(val, "StyleColors", StyleColors);
		JsonExtensions.AddPropertyColor(val, "BasicStyleColor", BasicStyleColor, Color.WhiteSmoke);
		JsonExtensions.AddPropertyBool(val, "AllowUseBorderFormatting", AllowUseBorderFormatting, true, false);
		JsonExtensions.AddPropertyBool(val, "AllowUseBorderSides", AllowUseBorderSides, true, false);
		JsonExtensions.AddPropertyBool(val, "AllowUseBrush", AllowUseBrush, true, false);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Border":
				Border = StiJsonReportObjectHelper.DeserializeBorder(item);
				break;
			case "Brush":
				Brush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "ChartAreaBrush":
				ChartAreaBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "ChartAreaBorderColor":
				ChartAreaBorderColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "ChartAreaShowShadow":
				ChartAreaShowShadow = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "SeriesLighting":
				SeriesLighting = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "SeriesShowShadow":
				SeriesShowShadow = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "SeriesShowBorder":
				SeriesShowBorder = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "SeriesBorderThickness":
				SeriesBorderThickness = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "SeriesLabelsBrush":
				SeriesLabelsBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "SeriesLabelsColor":
				SeriesLabelsColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "SeriesLabelsBorderColor":
				SeriesLabelsBorderColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "SeriesLabelsLineColor":
				SeriesLabelsLineColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "TrendLineColor":
				TrendLineColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "TrendLineShowShadow":
				TrendLineShowShadow = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "LegendBrush":
				LegendBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "LegendLabelsColor":
				LegendLabelsColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "LegendBorderColor":
				LegendBorderColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "LegendTitleColor":
				LegendTitleColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "AxisTitleColor":
				AxisTitleColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "AxisLineColor":
				AxisLineColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "AxisLabelsColor":
				AxisLabelsColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "MarkerVisible":
				MarkerVisible = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "InterlacingHorBrush":
				InterlacingHorBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "InterlacingVertBrush":
				InterlacingVertBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "GridLinesHorColor":
				GridLinesHorColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "GridLinesVertColor":
				GridLinesVertColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "BrushType":
				BrushType = StiJsonReportObjectHelper.DeserializeEnum<StiBrushType>(item);
				break;
			case "StyleColors":
				StyleColors = StiJsonReportObjectHelper.DeserializeArrayColor(item);
				break;
			case "BasicStyleColor":
				BasicStyleColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "AllowUseBorderFormatting":
				AllowUseBorderFormatting = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "AllowUseBorderSides":
				AllowUseBorderSides = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "AllowUseBrush":
				AllowUseBrush = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[4]
		{
			propertiesHelper.StyleName(),
			propertiesHelper.Description(),
			propertiesHelper.StyleCollectionName(),
			propertiesHelper.StyleConditions()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		value = new StiPropertyObject[2]
		{
			propertiesHelper.Brush(),
			propertiesHelper.Border()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, value);
		value = new StiPropertyObject[6]
		{
			propertiesHelper.BasicStyleColor(),
			propertiesHelper.StyleColors(),
			propertiesHelper.AllowUseBorderFormatting(),
			propertiesHelper.AllowUseBorderSides(),
			propertiesHelper.AllowUseBrush(),
			propertiesHelper.BrushType()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Parameters, value);
		value = new StiPropertyObject[3]
		{
			propertiesHelper.ChartAreaShowShadow(),
			propertiesHelper.ChartAreaBorderColor(),
			propertiesHelper.ChartAreaBrush()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Area, value);
		value = new StiPropertyObject[2]
		{
			propertiesHelper.SeriesLighting(),
			propertiesHelper.SeriesShowShadow()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Series, value);
		value = new StiPropertyObject[3]
		{
			propertiesHelper.SeriesLabelsBorderColor(),
			propertiesHelper.SeriesLabelsColor(),
			propertiesHelper.SeriesLabelsBrush()
		};
		stiPropertyCollection.Add(StiPropertyCategories.SeriesLabels, value);
		value = new StiPropertyObject[2]
		{
			propertiesHelper.TrendLineShowShadow(),
			propertiesHelper.TrendLineColor()
		};
		stiPropertyCollection.Add(StiPropertyCategories.TrendLine, value);
		value = new StiPropertyObject[4]
		{
			propertiesHelper.LegendBorderColor(),
			propertiesHelper.LegendLabelsColor(),
			propertiesHelper.LegendTitleColor(),
			propertiesHelper.LegendBrush()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Legend, value);
		value = new StiPropertyObject[3]
		{
			propertiesHelper.AxisLabelsColor(),
			propertiesHelper.AxisLineColor(),
			propertiesHelper.AxisTitleColor()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Axis, value);
		value = new StiPropertyObject[2]
		{
			propertiesHelper.InterlacingHorBrush(),
			propertiesHelper.InterlacingVertBrush()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Interlacing, value);
		value = new StiPropertyObject[2]
		{
			propertiesHelper.GridLinesHorColor(),
			propertiesHelper.GridLinesVertColor()
		};
		stiPropertyCollection.Add(StiPropertyCategories.GridLines, value);
		return stiPropertyCollection;
	}

	public override object Clone()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		StiChartStyle stiChartStyle = base.Clone() as StiChartStyle;
		stiChartStyle.Border = (StiBorder)Border.Clone();
		stiChartStyle.Brush = (StiBrush)Brush.Clone();
		stiChartStyle.ChartAreaBrush = (StiBrush)ChartAreaBrush.Clone();
		stiChartStyle.InterlacingHorBrush = (StiBrush)InterlacingHorBrush.Clone();
		stiChartStyle.InterlacingVertBrush = (StiBrush)InterlacingVertBrush.Clone();
		stiChartStyle.SeriesLabelsBrush = (StiBrush)SeriesLabelsBrush.Clone();
		stiChartStyle.LegendBrush = (StiBrush)LegendBrush.Clone();
		stiChartStyle.StyleColors = StyleColors?.ToArray();
		return stiChartStyle;
	}

	private bool ShouldSerializeBorder()
	{
		if (Border != null)
		{
			return !Border.IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeBrush()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (Brush is StiSolidBrush)
		{
			return !(((StiSolidBrush)Brush).Color == Color.White);
		}
		return true;
	}

	private bool ShouldSerializeChartAreaBrush()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (ChartAreaBrush is StiSolidBrush)
		{
			return !(((StiSolidBrush)ChartAreaBrush).Color == Color.White);
		}
		return true;
	}

	private bool ShouldSerializeChartAreaBorderColor()
	{
		return ChartAreaBorderColor != Color.FromArgb(171, 172, 173);
	}

	private bool ShouldSerializeChartAreaShowShadow()
	{
		return ChartAreaShowShadow;
	}

	private bool ShouldSerializeInterlacingHorBrush()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (InterlacingHorBrush is StiSolidBrush)
		{
			return !(((StiSolidBrush)InterlacingHorBrush).Color == Color.FromArgb(10, 155, 155, 155));
		}
		return true;
	}

	private bool ShouldSerializeInterlacingVertBrush()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (InterlacingVertBrush is StiSolidBrush)
		{
			return !(((StiSolidBrush)InterlacingVertBrush).Color == Color.FromArgb(10, 155, 155, 155));
		}
		return true;
	}

	private bool ShouldSerializeGridLinesHorColor()
	{
		return GridLinesHorColor != Color.FromArgb(100, 105, 105, 105);
	}

	private bool ShouldSerializeGridLinesVertColor()
	{
		return GridLinesVertColor != Color.FromArgb(100, 105, 105, 105);
	}

	private bool ShouldSerializeSeriesLighting()
	{
		return SeriesLighting;
	}

	private bool ShouldSerializeSeriesShowShadow()
	{
		return SeriesShowShadow;
	}

	private bool ShouldSeriesShowBorder()
	{
		return SeriesShowBorder;
	}

	private bool ShouldSeriesBorderThickness()
	{
		return SeriesBorderThickness != 1;
	}

	private bool ShouldSerializeSeriesLabelsBrush()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (SeriesLabelsBrush is StiSolidBrush)
		{
			return !(((StiSolidBrush)SeriesLabelsBrush).Color == Color.White);
		}
		return true;
	}

	private bool ShouldSerializeSeriesLabelsColor()
	{
		return SeriesLabelsColor != Color.FromArgb(90, 90, 90);
	}

	private bool ShouldSerializeSeriesLabelsBorderColor()
	{
		return SeriesLabelsBorderColor != Color.FromArgb(140, 140, 140);
	}

	private bool ShouldSerializeSeriesLabelsLineColor()
	{
		return SeriesLabelsLineColor != Color.FromArgb(140, 140, 140);
	}

	private bool ShouldSerializeTrendLineColor()
	{
		return TrendLineColor != Color.FromArgb(140, 140, 140);
	}

	private bool ShouldSerializeTrendLineShowShadow()
	{
		return TrendLineShowShadow;
	}

	private bool ShouldSerializeLegendBrush()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (LegendBrush is StiSolidBrush)
		{
			return !(((StiSolidBrush)LegendBrush).Color == Color.White);
		}
		return true;
	}

	private bool ShouldSerializeLegendLabelsColor()
	{
		return LegendLabelsColor != Color.FromArgb(140, 140, 140);
	}

	private bool ShouldSerializeLegendBorderColor()
	{
		return LegendBorderColor != Color.DimGray;
	}

	private bool ShouldSerializeLegendTitleColor()
	{
		return LegendTitleColor != Color.DimGray;
	}

	private bool ShouldSerializeAxisTitleColor()
	{
		return AxisTitleColor != Color.FromArgb(140, 140, 140);
	}

	private bool ShouldSerializeAxisLineColor()
	{
		return AxisLineColor != Color.FromArgb(140, 140, 140);
	}

	private bool ShouldSerializeAxisLabelsColor()
	{
		return AxisLabelsColor != Color.FromArgb(140, 140, 140);
	}

	private bool ShouldSerializeMarkerVisible()
	{
		return !MarkerVisible;
	}

	private bool ShouldSerializeBrushType()
	{
		return BrushType != StiBrushType.Solid;
	}

	private bool ShouldSerializeStyleColors()
	{
		if (StyleColors != null && StyleColors.Length == DefaultColors.Length)
		{
			return !StyleColors.ToList().SequenceEqual(DefaultColors);
		}
		return true;
	}

	private bool ShouldSerializeBasicStyleColor()
	{
		return BasicStyleColor != Color.WhiteSmoke;
	}

	private bool ShouldAllowUseBorderFormatting()
	{
		return !AllowUseBorderFormatting;
	}

	private bool ShouldAllowAllowUseBorderSides()
	{
		return !AllowUseBorderSides;
	}

	private bool ShouldAllowUseBrush()
	{
		return !AllowUseBrush;
	}

	public override void DrawStyle(Graphics g, Rectangle rect, bool paintValue, bool paintImage)
	{
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Expected O, but got Unknown
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected O, but got Unknown
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		Image val = StiReportImages.Styles.Chart((StiImageSize)2);
		Rectangle rectangle = new Rectangle(rect.X + StiScaleUI.I10 + (StiScaleUI.I(32) - val.Width) / 2, rect.Y + (rect.Height - val.Width) / 2, val.Width, val.Height);
		g.DrawImage(val, rectangle);
		if (rect.Height < StiScaleUI.I(32))
		{
			rect.Width -= StiScaleUI.I(36);
			rect.X += StiScaleUI.I(36);
		}
		else
		{
			rect.Width -= StiScaleUI.I(56);
			rect.X += StiScaleUI.I(56);
		}
		SmoothingMode smoothingMode = g.SmoothingMode;
		g.SmoothingMode = (SmoothingMode)4;
		SolidBrush val2 = new SolidBrush(Color.Black);
		try
		{
			Font val3 = new Font("Arial", 8f);
			try
			{
				StringFormat val4 = new StringFormat();
				try
				{
					val4.Alignment = (StringAlignment)0;
					val4.LineAlignment = (StringAlignment)1;
					g.DrawString(Name, val3, (Brush)(object)val2, (RectangleF)rect, val4);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		g.SmoothingMode = smoothingMode;
	}

	public override void DrawBox(Graphics g, Rectangle rect, bool paintValue, bool paintImage)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		rect.X++;
		rect.Y++;
		rect.Width -= 2;
		rect.Height -= 3;
		DrawStyle(g, rect, paintValue, paintImage);
		Pen val = new Pen(Color.FromArgb(150, Color.Gray));
		try
		{
			val.DashStyle = (DashStyle)1;
			g.DrawRectangle(val, rect);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public override void GetStyleFromComponent(StiComponent component, StiStyleElements styleElements)
	{
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Expected O, but got Unknown
		if (styleElements != StiStyleElements.All)
		{
			throw new Exception("StiCrossTabStyle support only StiStyleElements.All.");
		}
		if (!(component is StiChart stiChart))
		{
			return;
		}
		if (stiChart.Style is StiCustomStyle)
		{
			string customStyleName = stiChart.CustomStyleName;
			if (!string.IsNullOrEmpty(customStyleName) && stiChart.Report != null && stiChart.Report.Styles[customStyleName] is StiChartStyle)
			{
				BasicStyleColor = ((StiChartStyle)stiChart.Report.Styles[customStyleName]).BasicStyleColor;
				StyleColors = ((StiChartStyle)stiChart.Report.Styles[customStyleName]).StyleColors;
			}
		}
		else
		{
			BasicStyleColor = stiChart.Style.Core.BasicStyleColor;
			StyleColors = stiChart.Style.Core.StyleColors;
		}
		if ((styleElements & StiStyleElements.Border) > (StiStyleElements)0)
		{
			IStiBorder stiBorder = component as IStiBorder;
			object obj = stiBorder.Border.Clone();
			Border = (StiBorder)((obj is StiBorder) ? obj : null);
			AllowUseBorderFormatting = true;
			AllowUseBorderSides = true;
		}
		if ((styleElements & StiStyleElements.Brush) > (StiStyleElements)0)
		{
			IStiBrush stiBrush = component as IStiBrush;
			object obj2 = stiBrush.Brush.Clone();
			Brush = (StiBrush)((obj2 is StiBrush) ? obj2 : null);
			AllowUseBrush = true;
		}
		if (component is IStiBackColor && (styleElements & StiStyleElements.Brush) > (StiStyleElements)0)
		{
			IStiBackColor stiBackColor = component as IStiBackColor;
			Brush = (StiBrush)new StiSolidBrush(stiBackColor.BackColor);
			AllowUseBrush = true;
		}
	}

	public override void SetStyleToComponent(StiComponent component)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		if (!(component is StiChart stiChart) || !StiStyleConditionHelper.IsAllowStyle(component, this))
		{
			return;
		}
		stiChart.CustomStyleName = Name;
		stiChart.Style = new StiCustomStyle(Name);
		if (AllowUseBorderFormatting || AllowUseBorderSides)
		{
			IStiBorder stiBorder = component as IStiBorder;
			StiBorderSides side = stiBorder.Border.Side;
			if (AllowUseBorderFormatting)
			{
				object obj = Border.Clone();
				stiBorder.Border = (StiBorder)((obj is StiBorder) ? obj : null);
				stiBorder.Border.Side = side;
			}
			if (AllowUseBorderSides)
			{
				stiBorder.Border.Side = Border.Side;
			}
		}
		if (AllowUseBrush)
		{
			IStiBrush stiBrush = component as IStiBrush;
			object obj2 = Brush.Clone();
			stiBrush.Brush = (StiBrush)((obj2 is StiBrush) ? obj2 : null);
		}
		if (component is IStiBackColor && AllowUseBrush)
		{
			IStiBackColor stiBackColor = component as IStiBackColor;
			stiBackColor.BackColor = StiBrush.ToColor(Brush);
		}
	}

	public override void Save(Stream stream)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		StiSerializing val = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter());
		val.Serialize((object)this, stream, "StiChartStyle");
	}

	public override void Load(Stream stream)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		StiSerializing val = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter());
		val.Deserialize((object)this, stream, "StiChartStyle");
	}

	internal StiChartStyle(string name, string description, StiReport report)
		: base(name, description, report)
	{
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Expected O, but got Unknown
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Expected O, but got Unknown
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Expected O, but got Unknown
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Expected O, but got Unknown
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Expected O, but got Unknown
		StyleColors = DefaultColors.Clone() as Color[];
	}

	public StiChartStyle(string name, string description)
		: this(name, description, null)
	{
	}

	public StiChartStyle(string name)
		: this(name, "")
	{
	}

	public StiChartStyle()
		: this("")
	{
	}
}
