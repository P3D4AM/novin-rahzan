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
using Stimulsoft.Report.Components;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

[TypeConverter(typeof(StiUniversalConverter))]
public class StiLegend : IStiLegend, ICloneable, IStiJsonReportObject, IStiDefault, IStiSerializeToCodeAsClass, IStiPropertyGridObject, IStiFont
{
	private bool allowApplyStyle = true;

	private int columns;

	[Browsable(false)]
	public StiComponentId ComponentId => StiComponentId.StiLegend;

	[Browsable(false)]
	public string PropName => string.Empty;

	[Browsable(false)]
	public bool IsDefault
	{
		get
		{
			if (AllowApplyStyle && !HideSeriesWithEmptyTitle && Direction == StiLegendDirection.TopToBottom && HorAlignment == StiLegendHorAlignment.Left && VertAlignment == StiLegendVertAlignment.Top && Visible && MarkerVisible && MarkerBorder && !ShouldSerializeMarkerSize() && MarkerAlignment == StiMarkerAlignment.Left && Columns == 0 && HorSpacing == 4 && VertSpacing == 2 && !ShouldSerializeSize())
			{
				if (Title != null)
				{
					return Title.Length == 0;
				}
				return false;
			}
			return false;
		}
	}

	[Browsable(false)]
	public StiLegendCoreXF Core { get; set; }

	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that chart style will be used.")]
	[DefaultValue(true)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public bool AllowApplyStyle
	{
		get
		{
			return allowApplyStyle;
		}
		set
		{
			if (allowApplyStyle != value)
			{
				allowApplyStyle = value;
				if (value && Chart != null)
				{
					Core.ApplyStyle(Chart.Style);
				}
			}
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	public IStiChart Chart { get; set; }

	[DefaultValue(false)]
	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which shows/hides series with empty title.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public bool HideSeriesWithEmptyTitle { get; set; }

	[DefaultValue(true)]
	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates draw shadow or no.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public bool ShowShadow { get; set; } = true;


	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets border color.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public Color BorderColor { get; set; } = Color.Gray;


	[StiSerializable]
	[Description("Gets or sets background brush of legend.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiBrush Brush { get; set; } = (StiBrush)new StiSolidBrush(Color.White);


	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets title color of legend.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public Color TitleColor { get; set; } = Color.Gray;


	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets color of the labels.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public Color LabelsColor { get; set; } = Color.Gray;


	[StiSerializable]
	[DefaultValue(StiLegendDirection.TopToBottom)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets direction which used for series drawing in legend.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiLegendDirection Direction { get; set; } = StiLegendDirection.TopToBottom;


	[StiSerializable]
	[DefaultValue(StiLegendHorAlignment.Left)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets horizontal alignment of legend placement.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiLegendHorAlignment HorAlignment { get; set; } = StiLegendHorAlignment.Left;


	[StiSerializable]
	[DefaultValue(StiLegendVertAlignment.Top)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets vertical alignment of legend placement.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiLegendVertAlignment VertAlignment { get; set; } = StiLegendVertAlignment.Top;


	[StiSerializable]
	[Description("Gets or sets title font of the chart legend.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public Font TitleFont { get; set; } = new Font("Arial", 14f, (FontStyle)1);


	[StiSerializable]
	[Description("Gets or sets font which used for series title drawing in chart legend.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Design.Components.StiFontEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Font Font { get; set; } = new Font("Arial", 8f);


	[DefaultValue(true)]
	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets visibility of chart legend.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public bool Visible { get; set; } = true;


	[DefaultValue(true)]
	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets visibility of markers.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public bool MarkerVisible { get; set; } = true;


	[DefaultValue(true)]
	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets show a border marker.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public bool MarkerBorder { get; set; } = true;


	[StiSerializable]
	[Description("Gets or sets marker size.")]
	[TypeConverter(typeof(StiSizeConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public Size MarkerSize { get; set; } = new Size(10, 10);


	[StiSerializable]
	[DefaultValue(StiMarkerAlignment.Left)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets alignment of markers related to series title.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiMarkerAlignment MarkerAlignment { get; set; }

	[StiSerializable]
	[DefaultValue(0)]
	[Description("Gets or sets amount of columns.")]
	[Editor("Stimulsoft.Report.Design.Components.StiLegendColumnsEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public int Columns
	{
		get
		{
			return columns;
		}
		set
		{
			if (value >= 0)
			{
				columns = value;
			}
		}
	}

	[StiSerializable]
	[DefaultValue(4)]
	[Description("Gets or sets horizontal spacing between items in legend.")]
	public int HorSpacing { get; set; } = 4;


	[StiSerializable]
	[DefaultValue(2)]
	[Description("Gets or sets vertical spacing between items in legend.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public int VertSpacing { get; set; } = 2;


	[StiSerializable]
	[Description("Gets or sets size of legend.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public SizeD Size { get; set; }

	[DefaultValue("")]
	[StiSerializable]
	[Description("Gets or sets title of the legend.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public string Title { get; set; } = "";


	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		JObject val = new JObject();
		JsonExtensions.AddPropertyBool(val, "AllowApplyStyle", allowApplyStyle, true, false);
		JsonExtensions.AddPropertyBool(val, "HideSeriesWithEmptyTitle", HideSeriesWithEmptyTitle, false, false);
		JsonExtensions.AddPropertyBool(val, "ShowShadow", ShowShadow, true, false);
		JsonExtensions.AddPropertyColor(val, "BorderColor", BorderColor, Color.Gray);
		JsonExtensions.AddPropertyBrush(val, "Brush", Brush);
		JsonExtensions.AddPropertyColor(val, "TitleColor", TitleColor, Color.Gray);
		JsonExtensions.AddPropertyColor(val, "LabelsColor", LabelsColor, Color.Gray);
		JsonExtensions.AddPropertyEnum(val, "Direction", (Enum)Direction, (Enum)StiLegendDirection.TopToBottom);
		JsonExtensions.AddPropertyEnum(val, "HorAlignment", (Enum)HorAlignment, (Enum)StiLegendHorAlignment.Left);
		JsonExtensions.AddPropertyEnum(val, "VertAlignment", (Enum)VertAlignment, (Enum)StiLegendVertAlignment.Top);
		JsonExtensions.AddPropertyFontArial14Bold(val, "TitleFont", TitleFont);
		JsonExtensions.AddPropertyFontArial8(val, "Font", Font);
		JsonExtensions.AddPropertyBool(val, "Visible", Visible, true, false);
		JsonExtensions.AddPropertyBool(val, "MarkerVisible", MarkerVisible, true, false);
		JsonExtensions.AddPropertyBool(val, "MarkerBorder", MarkerBorder, true, false);
		JsonExtensions.AddPropertySize(val, "MarkerSize", MarkerSize);
		JsonExtensions.AddPropertyEnum(val, "MarkerAlignment", (Enum)MarkerAlignment, (Enum)StiMarkerAlignment.Left);
		JsonExtensions.AddPropertyInt(val, "Columns", columns, 0);
		JsonExtensions.AddPropertyInt(val, "HorSpacing", HorSpacing, 4);
		JsonExtensions.AddPropertyInt(val, "VertSpacing", VertSpacing, 2);
		JsonExtensions.AddPropertySizeD(val, "Size", Size, "0,0");
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Title", Title);
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "AllowApplyStyle":
				AllowApplyStyle = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "HideSeriesWithEmptyTitle":
				HideSeriesWithEmptyTitle = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ShowShadow":
				ShowShadow = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "BorderColor":
				BorderColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "Brush":
				Brush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "TitleColor":
				TitleColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "LabelsColor":
				LabelsColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "Direction":
				Direction = StiJsonReportObjectHelper.DeserializeEnum<StiLegendDirection>(item);
				break;
			case "HorAlignment":
				HorAlignment = StiJsonReportObjectHelper.DeserializeEnum<StiLegendHorAlignment>(item);
				break;
			case "VertAlignment":
				VertAlignment = StiJsonReportObjectHelper.DeserializeEnum<StiLegendVertAlignment>(item);
				break;
			case "TitleFont":
				TitleFont = StiJsonReportObjectHelper.DeserializeFont(item, TitleFont);
				break;
			case "Font":
				Font = StiJsonReportObjectHelper.DeserializeFont(item, Font);
				break;
			case "Visible":
				Visible = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "MarkerVisible":
				MarkerVisible = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "MarkerBorder":
				MarkerBorder = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "MarkerSize":
				MarkerSize = StiJsonReportObjectHelper.DeserializeSize(item);
				break;
			case "MarkerAlignment":
				MarkerAlignment = StiJsonReportObjectHelper.DeserializeEnum<StiMarkerAlignment>(item);
				break;
			case "Columns":
				columns = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "HorSpacing":
				HorSpacing = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "VertSpacing":
				VertSpacing = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "Size":
				Size = StiJsonReportObjectHelper.DeserializeSizeD(item);
				break;
			case "Title":
				Title = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			}
		}
	}

	public StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[1] { propertiesHelper.Legend() };
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	public StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		return null;
	}

	public object Clone()
	{
		IStiLegend stiLegend = MemberwiseClone() as IStiLegend;
		object obj = Brush.Clone();
		stiLegend.Brush = (StiBrush)((obj is StiBrush) ? obj : null);
		object obj2 = Font.Clone();
		stiLegend.Font = (Font)((obj2 is Font) ? obj2 : null);
		object obj3 = TitleFont.Clone();
		stiLegend.TitleFont = (Font)((obj3 is Font) ? obj3 : null);
		stiLegend.Direction = Direction;
		stiLegend.HorAlignment = HorAlignment;
		stiLegend.VertAlignment = VertAlignment;
		stiLegend.MarkerAlignment = MarkerAlignment;
		if (Core != null)
		{
			stiLegend.Core = Core.Clone() as StiLegendCoreXF;
			stiLegend.Core.Legend = stiLegend;
		}
		return stiLegend;
	}

	private bool ShouldSerializeBorderColor()
	{
		return BorderColor != Color.Gray;
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

	private bool ShouldSerializeTitleColor()
	{
		return TitleColor != Color.Gray;
	}

	private bool ShouldSerializeLabelsColor()
	{
		return LabelsColor != Color.Gray;
	}

	private bool ShouldSerializeTitleFont()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Invalid comparison between Unknown and I4
		if (TitleFont != null && TitleFont.Name == "Arial" && TitleFont.SizeInPoints == 14f)
		{
			return (int)TitleFont.Style != 1;
		}
		return true;
	}

	private bool ShouldSerializeFont()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Invalid comparison between Unknown and I4
		if (Font != null && Font.Name == "Arial" && Font.SizeInPoints == 14f)
		{
			return (int)Font.Style != 1;
		}
		return true;
	}

	private bool ShouldSerializeMarkerSize()
	{
		if (MarkerSize.Width == 10)
		{
			return MarkerSize.Height != 10;
		}
		return true;
	}

	private bool ShouldSerializeSize()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		SizeD size = Size;
		if (((SizeD)(ref size)).Width == 0.0)
		{
			size = Size;
			return ((SizeD)(ref size)).Height != 0.0;
		}
		return true;
	}

	[StiUniversalConstructor("Legend")]
	public StiLegend()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		Core = new StiLegendCoreXF(this);
	}
}
