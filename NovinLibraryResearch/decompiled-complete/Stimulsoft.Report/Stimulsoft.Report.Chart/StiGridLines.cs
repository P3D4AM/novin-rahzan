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
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

[TypeConverter(typeof(StiUniversalConverter))]
public abstract class StiGridLines : IStiGridLines, ICloneable, IStiJsonReportObject, IStiDefault, IStiPropertyGridObject
{
	internal bool needSetAreaJsonPropertyInternal;

	private bool allowApplyStyle = true;

	private int minorCount;

	[Browsable(false)]
	public StiComponentId ComponentId => StiComponentId.StiGridLines;

	[Browsable(false)]
	public string PropName => string.Empty;

	[Browsable(false)]
	public bool IsDefault
	{
		get
		{
			if (AllowApplyStyle && Visible && !MinorVisible)
			{
				return MinorCount == 0;
			}
			return false;
		}
	}

	[Browsable(false)]
	public StiGridLinesCoreXF Core { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
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
				if (value && Area != null && Area.Chart != null)
				{
					Core.ApplyStyle(Area.Chart.Style);
				}
			}
		}
	}

	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets color which will be used for drawing major grid lines.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public Color Color { get; set; } = Color.Silver;


	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets color which will be used for drawing minor grid lines.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public Color MinorColor { get; set; } = Color.Gainsboro;


	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets style which will be used for drawing major grid lines.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public StiPenStyle Style { get; set; } = (StiPenStyle)4;


	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets style which will be used for drawing minor grid lines.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public StiPenStyle MinorStyle { get; set; } = (StiPenStyle)4;


	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets visibility of major grid lines.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public bool Visible { get; set; } = true;


	[StiSerializable]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets visibility of minor grid lines.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public bool MinorVisible { get; set; }

	[DefaultValue(0)]
	[StiSerializable]
	[Description("Gets or sets count of minor grid lines per each major grid line.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public int MinorCount
	{
		get
		{
			return minorCount;
		}
		set
		{
			if (value >= 0)
			{
				minorCount = value;
			}
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	public IStiArea Area { get; set; }

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		JObject val = new JObject();
		JsonExtensions.AddPropertyBool(val, "AllowApplyStyle", allowApplyStyle, true, false);
		JsonExtensions.AddPropertyColor(val, "Color", Color, Color.Silver);
		JsonExtensions.AddPropertyColor(val, "MinorColor", MinorColor, Color.Gainsboro);
		JsonExtensions.AddPropertyEnum(val, "Style", (Enum)(object)Style, (Enum)(object)(StiPenStyle)4);
		JsonExtensions.AddPropertyEnum(val, "MinorStyle", (Enum)(object)MinorStyle, (Enum)(object)(StiPenStyle)4);
		JsonExtensions.AddPropertyBool(val, "Visible", Visible, true, false);
		JsonExtensions.AddPropertyBool(val, "MinorVisible", MinorVisible, false, false);
		JsonExtensions.AddPropertyInt(val, "MinorCount", minorCount, 0);
		if (Area != null)
		{
			JsonExtensions.AddPropertyBool(val, "Area", true, false, false);
		}
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "AllowApplyStyle":
				allowApplyStyle = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Color":
				Color = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "MinorColor":
				MinorColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "Style":
				Style = StiJsonReportObjectHelper.DeserializeEnum<StiPenStyle>(item);
				break;
			case "MinorStyle":
				MinorStyle = StiJsonReportObjectHelper.DeserializeEnum<StiPenStyle>(item);
				break;
			case "Visible":
				Visible = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "MinorVisible":
				MinorVisible = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "MinorCount":
				minorCount = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "Area":
				needSetAreaJsonPropertyInternal = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			}
		}
	}

	public StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyObject[] value = new StiPropertyObject[1] { propertiesHelper.GridLines() };
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
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		IStiGridLines stiGridLines = MemberwiseClone() as IStiGridLines;
		stiGridLines.Style = Style;
		stiGridLines.MinorStyle = MinorStyle;
		if (Core != null)
		{
			stiGridLines.Core = Core.Clone() as StiGridLinesCoreXF;
			stiGridLines.Core.GridLines = stiGridLines;
		}
		return stiGridLines;
	}

	private bool ShouldSerializeColor()
	{
		return Color != Color.Silver;
	}

	private bool ShouldSerializeMinorColor()
	{
		return Color != Color.Gainsboro;
	}

	public StiGridLines()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		Core = new StiGridLinesCoreXF(this);
	}

	public StiGridLines(Color color, StiPenStyle style, bool visible, Color minorColor, StiPenStyle minorStyle, bool minorVisible, int minorCount, bool allowApplyStyle)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		Color = color;
		Style = style;
		Visible = visible;
		MinorColor = minorColor;
		MinorStyle = minorStyle;
		MinorVisible = minorVisible;
		this.minorCount = minorCount;
		this.allowApplyStyle = allowApplyStyle;
		Core = new StiGridLinesCoreXF(this);
	}
}
