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
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

[TypeConverter(typeof(StiUniversalConverter))]
public abstract class StiRadarGridLines : IStiRadarGridLines, ICloneable, IStiJsonReportObject, IStiPropertyGridObject
{
	internal bool needSetAreaJsonPropertyInternal;

	private bool allowApplyStyle = true;

	[Browsable(false)]
	public StiComponentId ComponentId => StiComponentId.StiRadarGridLines;

	[Browsable(false)]
	public string PropName => string.Empty;

	[Browsable(false)]
	public StiRadarGridLinesCoreXF Core { get; set; }

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
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets color which will be used for drawing major grid lines.")]
	public Color Color { get; set; } = Color.Silver;


	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets style which will be used for drawing major grid lines.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public StiPenStyle Style { get; set; }

	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets visibility of major grid lines.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public bool Visible { get; set; } = true;


	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	public IStiArea Area { get; set; }

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		JObject val = new JObject();
		JsonExtensions.AddPropertyBool(val, "AllowApplyStyle", allowApplyStyle, true, false);
		JsonExtensions.AddPropertyColor(val, "Color", Color, Color.Silver);
		JsonExtensions.AddPropertyEnum(val, "Style", (Enum)(object)Style, (Enum)(object)(StiPenStyle)0);
		JsonExtensions.AddPropertyBool(val, "Visible", Visible, true, false);
		if (Area != null)
		{
			JsonExtensions.AddPropertyBool(val, "Area", true, false, false);
		}
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
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
			case "Style":
				Style = StiJsonReportObjectHelper.DeserializeEnum<StiPenStyle>(item);
				break;
			case "Visible":
				Visible = StiJsonReportObjectHelper.DeserializeBool(item);
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
		StiPropertyObject[] value = new StiPropertyObject[1] { propertiesHelper.RadarGridLines() };
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
		IStiRadarGridLines stiRadarGridLines = MemberwiseClone() as IStiRadarGridLines;
		stiRadarGridLines.Style = Style;
		if (Core != null)
		{
			stiRadarGridLines.Core = Core.Clone() as StiRadarGridLinesCoreXF;
			stiRadarGridLines.Core.GridLines = stiRadarGridLines;
		}
		return stiRadarGridLines;
	}

	public StiRadarGridLines()
	{
		Core = new StiRadarGridLinesCoreXF(this);
	}

	public StiRadarGridLines(Color color, StiPenStyle style, bool visible, bool allowApplyStyle)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		Color = color;
		Style = style;
		Visible = visible;
		this.allowApplyStyle = allowApplyStyle;
		Core = new StiRadarGridLinesCoreXF(this);
	}
}
