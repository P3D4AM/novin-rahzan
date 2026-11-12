using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Chart;

namespace Stimulsoft.Dashboard.Components.Chart;

[TypeConverter(typeof(StiUniversalConverter))]
public class StiChartMarker : IStiJsonReportObject, ICloneable
{
	[DefaultValue(7f)]
	[StiSerializable]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[Description("Gets or sets size of the marker.")]
	public virtual float Size { get; set; } = 7f;


	[DefaultValue(0f)]
	[StiSerializable]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[Description("Gets or sets rotation angle of the marker.")]
	public virtual float Angle { get; set; }

	[DefaultValue(StiMarkerType.Circle)]
	[StiSerializable]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets type of the marker.")]
	public StiMarkerType Type { get; set; } = StiMarkerType.Circle;


	[DefaultValue(StiExtendedStyleBool.FromStyle)]
	[StiSerializable]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets visibility of marker.")]
	public virtual StiExtendedStyleBool Visible { get; set; }

	[Browsable(false)]
	public bool IsDefault
	{
		get
		{
			if (Size == 7f && Angle == 0f && Type == StiMarkerType.Circle)
			{
				return Visible == StiExtendedStyleBool.FromStyle;
			}
			return false;
		}
	}

	public virtual object Clone()
	{
		return MemberwiseClone() as StiChartMarker;
	}

	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = new JObject();
		jObject.AddPropertyFloat("Size", Size, 7f);
		jObject.AddPropertyFloat("Angle", Angle);
		jObject.AddPropertyEnum("Type", Type, StiMarkerType.Circle);
		jObject.AddPropertyEnum("Visible", Visible, StiExtendedStyleBool.FromStyle);
		return jObject;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Size":
				Size = item.DeserializeFloat();
				break;
			case "Angle":
				Angle = item.DeserializeFloat();
				break;
			case "Type":
				Type = item.DeserializeEnum<StiMarkerType>();
				break;
			case "Visible":
				Visible = item.DeserializeEnum<StiExtendedStyleBool>();
				break;
			}
		}
	}

	public StiChartMarker()
	{
	}

	[StiUniversalConstructor("Marker")]
	public StiChartMarker(float size, float angle, StiMarkerType type, StiExtendedStyleBool visible)
	{
		Size = size;
		Angle = angle;
		Type = type;
		Visible = visible;
	}
}
