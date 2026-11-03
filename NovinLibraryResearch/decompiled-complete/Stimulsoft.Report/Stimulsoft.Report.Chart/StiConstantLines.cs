using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiConstantLines : StiService, IStiConstantLines, ICloneable, IStiJsonReportObject, IStiPropertyGridObject
{
	public enum StiOrientation
	{
		Horizontal,
		Vertical,
		HorizontalRight
	}

	public enum StiTextPosition
	{
		LeftTop,
		LeftBottom,
		CenterTop,
		CenterBottom,
		RightTop,
		RightBottom
	}

	private bool allowApplyStyle = true;

	[Browsable(false)]
	public StiComponentId ComponentId => StiComponentId.StiConstantLines;

	[Browsable(false)]
	public string PropName => string.Empty;

	[Browsable(false)]
	public sealed override string ServiceCategory => "Chart";

	[Browsable(false)]
	public sealed override Type ServiceType => typeof(StiConstantLines);

	[Browsable(false)]
	public StiConstantLinesCoreXF Core { get; set; }

	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that chart style will be used.")]
	[DefaultValue(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
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
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which control antialiasing drawing mode.")]
	public bool Antialiasing { get; set; } = true;


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(StiTextPosition.LeftTop)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets text position at constant line.")]
	public StiTextPosition Position { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets font which used for drawing constant line text.")]
	public virtual Font Font { get; set; } = new Font("Arial", 7f);


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue("")]
	[Description("Gets or sets constant line text.")]
	public string Text { get; set; } = "";


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets visibility of constant lines title.")]
	public bool TitleVisible { get; set; } = true;


	[DefaultValue(StiOrientation.Horizontal)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets horizontal or vertical orientation of constant line.")]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiOrientation Orientation { get; set; }

	[DefaultValue(1f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets constant line width.")]
	public float LineWidth { get; set; } = 1f;


	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets constant line style.")]
	public StiPenStyle LineStyle { get; set; }

	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets color which will be used for drawing constant line.")]
	public virtual Color LineColor { get; set; } = Color.Black;


	[Browsable(false)]
	[DefaultValue(true)]
	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets constant lines in chart legend.")]
	public virtual bool ShowInLegend { get; set; } = true;


	[DefaultValue(false)]
	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets value which indicates that constant lines will be shown behind chart series or in front of chart series.")]
	public virtual bool ShowBehind { get; set; }

	[DefaultValue("1")]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets Y axis value through what the constant line is drawn.")]
	public virtual string AxisValue { get; set; } = "1";


	[DefaultValue(true)]
	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets visibility of constant line.")]
	public virtual bool Visible { get; set; } = true;


	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	public IStiChart Chart { get; set; }

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		JObject val = new JObject();
		JsonExtensions.AddPropertyBool(val, "AllowApplyStyle", AllowApplyStyle, true, false);
		JsonExtensions.AddPropertyBool(val, "Antialiasing", Antialiasing, true, false);
		JsonExtensions.AddPropertyEnum(val, "Position", (Enum)Position, (Enum)StiTextPosition.LeftTop);
		JsonExtensions.AddPropertyFontArial7(val, "Font", Font);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Text", Text);
		JsonExtensions.AddPropertyBool(val, "TitleVisible", TitleVisible, true, false);
		JsonExtensions.AddPropertyEnum(val, "Orientation", (Enum)Orientation, (Enum)StiOrientation.Horizontal);
		JsonExtensions.AddPropertyFloat(val, "LineWidth", LineWidth, 1f);
		JsonExtensions.AddPropertyEnum(val, "LineStyle", (Enum)(object)LineStyle, (Enum)(object)(StiPenStyle)0);
		JsonExtensions.AddPropertyColor(val, "LineColor", LineColor, Color.Black);
		JsonExtensions.AddPropertyBool(val, "ShowInLegend", ShowInLegend, true, false);
		JsonExtensions.AddPropertyBool(val, "ShowBehind", ShowBehind, false, false);
		JsonExtensions.AddPropertyString(val, "AxisValue", AxisValue, "1");
		JsonExtensions.AddPropertyBool(val, "Visible", Visible, true, false);
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "AllowApplyStyle":
				AllowApplyStyle = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Antialiasing":
				Antialiasing = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Position":
				Position = StiJsonReportObjectHelper.DeserializeEnum<StiTextPosition>(item);
				break;
			case "Font":
				Font = StiJsonReportObjectHelper.DeserializeFont(item, Font);
				break;
			case "Text":
				Text = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "TitleVisible":
				TitleVisible = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Orientation":
				Orientation = StiJsonReportObjectHelper.DeserializeEnum<StiOrientation>(item);
				break;
			case "LineWidth":
				LineWidth = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "LineStyle":
				LineStyle = StiJsonReportObjectHelper.DeserializeEnum<StiPenStyle>(item);
				break;
			case "LineColor":
				LineColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "ShowInLegend":
				ShowInLegend = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ShowBehind":
				ShowBehind = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "AxisValue":
				AxisValue = StiJsonReportObjectHelper.DeserializeString(item);
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
		StiPropertyObject[] value = new StiPropertyObject[7]
		{
			propertiesHelper.AxisValue(),
			propertiesHelper.LineColor(),
			propertiesHelper.LineStyle(),
			propertiesHelper.LineWidth(),
			propertiesHelper.Orientation(),
			propertiesHelper.ShowBehind(),
			propertiesHelper.Visible()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Behavior, value);
		value = new StiPropertyObject[1] { propertiesHelper.AllowApplyStyle() };
		stiPropertyCollection.Add(StiPropertyCategories.Misc, value);
		value = new StiPropertyObject[5]
		{
			propertiesHelper.Antialiasing(),
			propertiesHelper.Font(),
			propertiesHelper.Position(),
			propertiesHelper.TextNotEdit(),
			propertiesHelper.TitleVisible()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Title, value);
		return stiPropertyCollection;
	}

	public StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		return null;
	}

	public override object Clone()
	{
		IStiConstantLines stiConstantLines = MemberwiseClone() as IStiConstantLines;
		if (Core != null)
		{
			stiConstantLines.Core = Core.Clone() as StiConstantLinesCoreXF;
			stiConstantLines.Core.ConstantLines = stiConstantLines;
		}
		return stiConstantLines;
	}

	public override string ToString()
	{
		if (!string.IsNullOrWhiteSpace(Text))
		{
			return Text;
		}
		return "(" + Loc.Get("Chart", "ConstantLine") + ")";
	}

	public StiConstantLines()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		Core = new StiConstantLinesCoreXF(this);
	}
}
