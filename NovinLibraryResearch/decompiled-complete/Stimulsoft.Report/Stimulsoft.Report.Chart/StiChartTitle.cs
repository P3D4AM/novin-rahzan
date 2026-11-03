using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

[TypeConverter(typeof(StiUniversalConverter))]
public class StiChartTitle : IStiChartTitle, ICloneable, IStiJsonReportObject, IStiDefault, IStiPropertyGridObject, IStiFont
{
	private bool allowApplyStyle = true;

	[Browsable(false)]
	public StiComponentId ComponentId => StiComponentId.StiChartTitle;

	[Browsable(false)]
	public string PropName => string.Empty;

	[Browsable(false)]
	public bool IsDefault
	{
		get
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Invalid comparison between Unknown and I4
			if (AllowApplyStyle && Text != null && Text.Length == 0 && (int)Alignment == 1 && Dock == StiChartTitleDock.Top && Spacing == 2)
			{
				return !Visible;
			}
			return false;
		}
	}

	[Browsable(false)]
	public StiChartTitleCoreXF Core { get; set; }

	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that chart style will be used.")]
	[DefaultValue(true)]
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

	[StiSerializable]
	[Description("Gets or sets font of the chart title.")]
	[Editor("Stimulsoft.Report.Design.Components.StiFontEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Font Font { get; set; } = new Font("Tahoma", 12f, (FontStyle)1);


	[DefaultValue("")]
	[StiSerializable]
	[Description("Gets or sets text of the chart title.")]
	public string Text { get; set; } = "";


	[StiSerializable]
	[Description("Gets or sets text brush of the chart title.")]
	public StiBrush Brush { get; set; } = (StiBrush)new StiSolidBrush(Color.SaddleBrown);


	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which control antialiasing drawing mode of chart title.")]
	public bool Antialiasing { get; set; } = true;


	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets os sets alignment of chart title.")]
	public StringAlignment Alignment { get; set; } = (StringAlignment)1;


	[StiSerializable]
	[DefaultValue(StiChartTitleDock.Top)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets docking ot chart title.")]
	public StiChartTitleDock Dock { get; set; }

	[StiSerializable]
	[DefaultValue(2)]
	[Description("Gets or sets spacing between chart title and chart area.")]
	public int Spacing { get; set; } = 2;


	[StiSerializable]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets visibility of chart title.")]
	public bool Visible { get; set; }

	[Browsable(false)]
	public IStiChart Chart { get; set; }

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		JObject val = new JObject();
		JsonExtensions.AddPropertyBool(val, "AllowApplyStyle", allowApplyStyle, true, false);
		JsonExtensions.AddPropertyFontTahoma12Bold(val, "Font", Font);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Text", Text);
		JsonExtensions.AddPropertyBrush(val, "Brush", Brush);
		JsonExtensions.AddPropertyBool(val, "Antialiasing", Antialiasing, true, false);
		JsonExtensions.AddPropertyEnum(val, "Alignment", (Enum)(object)Alignment, (Enum)(object)(StringAlignment)1);
		JsonExtensions.AddPropertyEnum(val, "Dock", (Enum)Dock, (Enum)StiChartTitleDock.Top);
		JsonExtensions.AddPropertyInt(val, "Spacing", Spacing, 2);
		JsonExtensions.AddPropertyBool(val, "Visible", Visible, false, false);
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "AllowApplyStyle":
				AllowApplyStyle = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Font":
				Font = StiJsonReportObjectHelper.DeserializeFont(item, Font);
				break;
			case "Text":
				Text = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Brush":
				Brush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "Antialiasing":
				Antialiasing = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Alignment":
				Alignment = StiJsonReportObjectHelper.DeserializeEnum<StringAlignment>(item);
				break;
			case "Dock":
				Dock = StiJsonReportObjectHelper.DeserializeEnum<StiChartTitleDock>(item);
				break;
			case "Spacing":
				Spacing = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "Visible":
				Visible = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			}
		}
	}

	public StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyObject[] value = new StiPropertyObject[1] { propertiesHelper.ChartTitle() };
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	public StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		return null;
	}

	public object Clone()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		IStiChartTitle stiChartTitle = MemberwiseClone() as IStiChartTitle;
		stiChartTitle.Alignment = Alignment;
		object obj = Font.Clone();
		stiChartTitle.Font = (Font)((obj is Font) ? obj : null);
		object obj2 = Brush.Clone();
		stiChartTitle.Brush = (StiBrush)((obj2 is StiBrush) ? obj2 : null);
		if (Core != null)
		{
			stiChartTitle.Core = Core.Clone() as StiChartTitleCoreXF;
			stiChartTitle.Core.ChartTitle = stiChartTitle;
		}
		return stiChartTitle;
	}

	protected bool ShouldSerializeFont()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Invalid comparison between Unknown and I4
		if (Font != null && Font.Name == "Tahoma" && Font.SizeInPoints == 12f)
		{
			return (int)Font.Style != 1;
		}
		return true;
	}

	private bool ShouldSerializeBrush()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (Brush is StiSolidBrush)
		{
			return !(((StiSolidBrush)Brush).Color == Color.SaddleBrown);
		}
		return true;
	}

	public StiChartTitle()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		Core = new StiChartTitleCoreXF(this);
	}

	[StiUniversalConstructor("Title")]
	public StiChartTitle(Font font, string text, StiBrush brush, bool antialiasing, StringAlignment alignment, StiChartTitleDock dock, int spacing, bool visible, bool allowApplyStyle)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		Core = new StiChartTitleCoreXF(this);
		Font = font;
		Text = text;
		Brush = brush;
		Antialiasing = antialiasing;
		Alignment = alignment;
		Dock = dock;
		Spacing = spacing;
		Visible = visible;
		this.allowApplyStyle = allowApplyStyle;
	}
}
