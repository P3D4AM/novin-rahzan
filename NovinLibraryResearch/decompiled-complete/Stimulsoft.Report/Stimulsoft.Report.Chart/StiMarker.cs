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

[TypeConverter(typeof(StiMarkerConverter))]
public class StiMarker : IStiMarker, ICloneable, IStiFontIconsSeries, IStiJsonReportObject, IStiDefault, IStiSerializeToCodeAsClass, IStiPropertyGridObject
{
	[Browsable(false)]
	public virtual StiComponentId ComponentId => StiComponentId.StiMarker;

	[Browsable(false)]
	public string PropName => string.Empty;

	[Browsable(false)]
	public virtual bool IsDefault
	{
		get
		{
			if (!ShouldSerializeVisible() && !ShouldSerializeBrush() && !ShouldSerializeBorderColor() && !ShouldSerializeSize() && ShowInLegend && ExtendedVisible == StiExtendedStyleBool.FromStyle && Angle == 0f)
			{
				return Type == StiMarkerType.Circle;
			}
			return false;
		}
	}

	[Browsable(false)]
	public StiMarkerCoreXF Core { get; set; }

	[DefaultValue(true)]
	[StiSerializable]
	[Browsable(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that marker will be visible in legend marker.")]
	public virtual bool ShowInLegend { get; set; } = true;


	[DefaultValue(true)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets visibility of marker.")]
	public virtual bool Visible { get; set; } = true;


	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	internal StiExtendedStyleBool ExtendedVisible { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets brush which will be used to fill marker area.")]
	public virtual StiBrush Brush { get; set; } = (StiBrush)new StiSolidBrush(Color.White);


	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets border color of marker.")]
	public Color BorderColor { get; set; } = Color.Black;


	[DefaultValue(7f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets size of the marker.")]
	public virtual float Size { get; set; } = 7f;


	[DefaultValue(0f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets rotation angle of the marker.")]
	public virtual float Angle { get; set; }

	[DefaultValue(StiMarkerType.Circle)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets type of the marker.")]
	public StiMarkerType Type { get; set; } = StiMarkerType.Circle;


	[DefaultValue(null)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Design.StiFontIconEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiFontIconConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiFontIcons? Icon { get; set; }

	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyBool(val, "ShowInLegend", ShowInLegend, true, false);
		JsonExtensions.AddPropertyBool(val, "Visible", Visible, true, false);
		JsonExtensions.AddPropertyBrush(val, "Brush", Brush);
		JsonExtensions.AddPropertyColor(val, "BorderColor", BorderColor, Color.Black);
		JsonExtensions.AddPropertyFloat(val, "Size", Size, 7f);
		JsonExtensions.AddPropertyFloat(val, "Angle", Angle, 0f);
		JsonExtensions.AddPropertyEnum(val, "Type", (Enum)Type, (Enum)StiMarkerType.Circle);
		if (Icon.HasValue)
		{
			JsonExtensions.AddPropertyEnum(val, "Icon", (Enum)(object)Icon);
		}
		return val;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "ShowInLegend":
				ShowInLegend = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Visible":
				Visible = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Brush":
				Brush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "BorderColor":
				BorderColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "Size":
				Size = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "Angle":
				Angle = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "Type":
				Type = StiJsonReportObjectHelper.DeserializeEnum<StiMarkerType>(item);
				break;
			case "Icon":
				Icon = StiJsonReportObjectHelper.DeserializeEnum<StiFontIcons>(item);
				break;
			}
		}
	}

	public virtual StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyObject[] value = new StiPropertyObject[1] { propertiesHelper.Marker() };
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	public StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		return null;
	}

	public object Clone()
	{
		IStiMarker stiMarker = MemberwiseClone() as IStiMarker;
		if (Core != null)
		{
			stiMarker.Core = Core.Clone() as StiMarkerCoreXF;
			stiMarker.Core.Marker = stiMarker;
		}
		return stiMarker;
	}

	protected virtual bool ShouldSerializeVisible()
	{
		return !Visible;
	}

	private bool ShouldSerializeBrush()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (Brush is StiSolidBrush)
		{
			return ((StiSolidBrush)Brush).Color != Color.White;
		}
		return true;
	}

	private bool ShouldSerializeBorderColor()
	{
		return BorderColor != Color.Black;
	}

	protected virtual bool ShouldSerializeSize()
	{
		return Size != 7f;
	}

	public StiMarker()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		Core = new StiMarkerCoreXF(this);
	}
}
