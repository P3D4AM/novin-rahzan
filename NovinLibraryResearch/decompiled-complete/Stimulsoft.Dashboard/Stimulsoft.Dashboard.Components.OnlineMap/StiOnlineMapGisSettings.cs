using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Gis;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Dashboard.Components.HeatmapGradient.Design;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Dashboard.Components.OnlineMap;

[TypeConverter(typeof(StiOnlineMapGisSettingsConverter))]
public class StiOnlineMapGisSettings : IStiJsonReportObject, IStiOnlineMapGisSettings
{
	public enum Order
	{
		Color = 1,
		Icon,
		IconColor,
		Language,
		Size,
		ShowPlacemark
	}

	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Browsable(true)]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[StiOrder(1)]
	public virtual Color Color { get; set; } = Color.FromArgb(180, 4, 4);


	[StiSerializable]
	[DefaultValue(StiLanguageType.English)]
	[Browsable(true)]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[StiOrder(4)]
	[TypeConverter(typeof(StiEnumConverter))]
	public virtual StiLanguageType Language { get; set; } = StiLanguageType.English;


	[StiSerializable]
	[Browsable(true)]
	[DefaultValue(2.0)]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[StiOrder(5)]
	public virtual double Size { get; set; } = 2.0;


	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets visibility of placemark.")]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[StiOrder(6)]
	public virtual bool ShowPlacemark { get; set; } = true;


	[StiSerializable]
	[DefaultValue(StiFontIcons.MapMarker)]
	[Editor("Stimulsoft.Report.Design.StiFontIconEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiFontIconConverter))]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[StiOrder(2)]
	public virtual StiFontIcons Icon { get; set; } = StiFontIcons.MapMarker;


	[StiSerializable]
	[TypeConverter(typeof(StiStyleColorConverter))]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[StiOrder(3)]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color IconColor { get; set; } = Color.FromArgb(68, 114, 196);


	[Browsable(false)]
	public virtual bool IsDefault
	{
		get
		{
			if (Color == Color.FromArgb(180, 4, 4) && Language == StiLanguageType.English && Size == 2.0 && ShowPlacemark && Icon == StiFontIcons.MapMarker)
			{
				return IconColor == Color.FromArgb(68, 114, 196);
			}
			return false;
		}
	}

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = new JObject();
		jObject.AddPropertyColor("Color", Color, Color.FromArgb(180, 4, 4));
		jObject.AddPropertyDouble("Size", Size, 2.0);
		jObject.AddPropertyBool("ShowPlacemark", ShowPlacemark, defaultValue: true);
		jObject.AddPropertyEnum("Language", Language, StiLanguageType.English);
		jObject.AddPropertyEnum("Icon", Icon, StiFontIcons.MapMarker);
		jObject.AddPropertyColor("IconColor", IconColor, Color.FromArgb(68, 114, 196));
		return jObject;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Size":
				Size = item.DeserializeDouble();
				break;
			case "Color":
				Color = item.DeserializeColor();
				break;
			case "ShowPlacemark":
				ShowPlacemark = item.DeserializeBool();
				break;
			case "Language":
				Language = item.DeserializeEnum<StiLanguageType>();
				break;
			case "Icon":
				Icon = item.DeserializeEnum<StiFontIcons>();
				break;
			case "IconColor":
				IconColor = item.DeserializeColor();
				break;
			}
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public StiOnlineMapGisSettings()
	{
	}

	[StiUniversalConstructor("GisSettings")]
	public StiOnlineMapGisSettings(Color color, double size, bool showPlacemark)
		: this(color, size, showPlacemark, StiLanguageType.English)
	{
	}

	[StiUniversalConstructor("GisSettings")]
	public StiOnlineMapGisSettings(Color color, double size, bool showPlacemark, StiLanguageType language)
		: this(color, size, showPlacemark, language, StiFontIcons.MapMarker, Color.FromArgb(68, 114, 196))
	{
		Color = color;
		Size = size;
		ShowPlacemark = showPlacemark;
		Language = language;
	}

	[StiUniversalConstructor("GisSettings")]
	public StiOnlineMapGisSettings(Color color, double size, bool showPlacemark, StiLanguageType language, StiFontIcons icon, Color iconColor)
	{
		Color = color;
		Size = size;
		ShowPlacemark = showPlacemark;
		Language = language;
		Icon = icon;
		IconColor = iconColor;
	}
}
