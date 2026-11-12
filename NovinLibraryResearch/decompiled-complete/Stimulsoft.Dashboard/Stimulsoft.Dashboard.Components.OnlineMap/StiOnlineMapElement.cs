using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Gis;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Dashboard.Components.Design;
using Stimulsoft.Dashboard.Components.Helpers;
using Stimulsoft.Dashboard.Components.Image.Design;
using Stimulsoft.Dashboard.Components.RegionMap;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Dashboard.Interactions;
using Stimulsoft.Dashboard.Interactions.Design;
using Stimulsoft.Dashboard.Visualizers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Dashboard.Components.OnlineMap;

[StiServiceBitmap(typeof(StiElement), "Stimulsoft.Dashboard.Images.Toolbox.StiOnlineMap.png")]
[StiGdiPainter("Stimulsoft.Dashboard.Drawing.Painters.StiOnlineMapElementGdiPainter, Stimulsoft.Dashboard.Drawing, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfPainter("Stimulsoft.Dashboard.Drawing.Wpf.Painters.StiOnlineMapElementWpfPainter, Stimulsoft.Dashboard.Drawing.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiDesigner("Stimulsoft.Dashboard.Design.Designers.StiOnlineMapElementDesigner, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Dashboard.WpfDesign.Designers.StiOnlineMapElementWpfDesigner, Stimulsoft.Dashboard.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiVisualizer("Stimulsoft.Dashboard.Viewer.Visualizers.StiOnlineMapElementVisualizer, Stimulsoft.Dashboard.Viewer, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfVisualizer("Stimulsoft.Dashboard.Viewer.Wpf.Visualizers.StiOnlineMapElementWpfVisualizer, Stimulsoft.Dashboard.Viewer.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiContextTool(typeof(IStiComponentDesigner))]
[StiContextTool(typeof(IStiTitleElement))]
public class StiOnlineMapElement : StiElement, IStiOnlineMapElement, IStiElement, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiQueryObject, IStiRetrieval, IStiTransformActions, IStiTransformFilters, IStiTransformSorts, IStiDataTransformationElement, IStiGroupElement, IStiCrossFiltering, IStiDataFilters, IStiManuallyEnteredData, IStiSimpleShadow, IStiCornerRadius, IStiTitleElement, IStiElementLayout, IStiGlobalizationProvider, IStiElementInteraction, IStiConvertibleElement
{
	private List<StiDataFilterRule> dataFilters = new List<StiDataFilterRule>();

	private StiSimpleShadow shadow = new StiSimpleShadow();

	private StiCornerRadius cornerRadius = new StiCornerRadius(0f);

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiOnlineMapElement;

	[StiSerializable]
	[StiCategory("OnlineMap", StiCategorySource.PropertyCategory)]
	[DefaultValue("")]
	[StiOrder(103)]
	[StiPropertyLevel(StiLevel.Standard)]
	public string Group { get; set; } = "";


	[StiSerializable]
	[StiCategory("OnlineMap", StiCategorySource.PropertyCategory)]
	[DefaultValue(true)]
	[StiOrder(100)]
	[StiPropertyLevel(StiLevel.Standard)]
	[TypeConverter(typeof(StiBoolConverter))]
	public bool CrossFiltering { get; set; } = true;


	[StiSerializable(StiSerializationVisibility.Class)]
	[StiCategory("Behavior", StiCategorySource.PropertyCategory)]
	[StiOrder(490)]
	[TypeConverter(typeof(StiTitleConverter))]
	public IStiTitle Title { get; set; } = new StiTitle();


	[Browsable(false)]
	public StiElementLayout Layout { get; set; }

	[Browsable(false)]
	public override bool IsDefined
	{
		get
		{
			if (Latitude == null && Longitude == null && Location == null && LocationValue == null && LocationColorMeter == null && LocationArgument == null && Gis == null && GisColor == null && LineSize == null && Description == null)
			{
				return DataMode == StiDataMode.ManuallyEnteringData;
			}
			return true;
		}
	}

	[StiSerializable(StiSerializationVisibility.List)]
	[Browsable(false)]
	public List<StiDataActionRule> TransformActions { get; set; } = new List<StiDataActionRule>();


	[StiSerializable(StiSerializationVisibility.List)]
	[Browsable(false)]
	public virtual List<StiDataFilterRule> TransformFilters { get; set; } = new List<StiDataFilterRule>();


	[StiSerializable(StiSerializationVisibility.List)]
	[Browsable(false)]
	public List<StiDataSortRule> TransformSorts { get; set; } = new List<StiDataSortRule>();


	[Editor("Stimulsoft.Report.Components.Design.StiDataTransformationEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiDataTransformationConverter))]
	[StiCategory("OnlineMap", StiCategorySource.PropertyCategory)]
	[StiPropertyLevel(StiLevel.Standard)]
	[StiOrder(101)]
	public object DataTransformation { get; set; } = new object();


	[StiSerializable(StiSerializationVisibility.List)]
	[Browsable(false)]
	public virtual List<StiDataFilterRule> DataFilters
	{
		get
		{
			return dataFilters;
		}
		set
		{
			if (dataFilters != value)
			{
				dataFilters = value;
				InvokeRefreshControl("DataFilters");
			}
		}
	}

	[StiSerializable]
	[DefaultValue(StiDataMode.UsingDataFields)]
	[Browsable(false)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiDataMode DataMode { get; set; }

	[StiSerializable]
	[DefaultValue(null)]
	[Browsable(false)]
	public string ManuallyEnteredData { get; set; }

	public override int ToolboxPosition => 310;

	public override string LocalizedName => StiLocalization.Get("Components", "StiOnlineMap");

	public override string HelpUrl => "user-manual/dashboards_maps_online_map.htm";

	[StiEventHide]
	[StiNonSerialized]
	public override StiClickEvent ClickEvent
	{
		get
		{
			return base.ClickEvent;
		}
		set
		{
			base.ClickEvent = value;
		}
	}

	[StiSerializable(StiSerializationVisibility.Class)]
	[TypeConverter(typeof(StiDashboardInteractionConverter))]
	[Browsable(false)]
	public virtual IStiDashboardInteraction DashboardInteraction { get; set; }

	[Description("Shadow settings of the component.")]
	[StiSerializable]
	[StiPropertyLevel(StiLevel.Standard)]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[StiOrder(276)]
	public StiSimpleShadow Shadow
	{
		get
		{
			return shadow;
		}
		set
		{
			if (shadow != value)
			{
				shadow = value;
				InvokeRefreshControl("Shadow");
			}
		}
	}

	[Description("Represents the value to which the corners are rounded.")]
	[StiSerializable]
	[StiPropertyLevel(StiLevel.Standard)]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[StiOrder(150)]
	public StiCornerRadius CornerRadius
	{
		get
		{
			return cornerRadius;
		}
		set
		{
			if (cornerRadius != value)
			{
				cornerRadius = value;
				InvokeRefreshControl("CornerRadius");
			}
		}
	}

	[StiSerializable]
	[DefaultValue(StiOnlineMapMode.Location)]
	[Browsable(false)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiOnlineMapMode MapMode { get; set; }

	[Editor("Stimulsoft.Report.Components.Design.StiElementInteractionEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory("Behavior", StiCategorySource.PropertyCategory)]
	[StiPropertyLevel(StiLevel.Standard)]
	[StiOrder(205)]
	[StiSerializable(StiSerializationVisibility.Class)]
	[TypeConverter(typeof(StiDashboardInteractionConverter))]
	[Browsable(true)]
	public override StiInteraction Interaction
	{
		get
		{
			return base.Interaction;
		}
		set
		{
			base.Interaction = value;
		}
	}

	[StiSerializable(StiSerializationVisibility.Class)]
	[DefaultValue(null)]
	[Browsable(false)]
	public StiLatitudeMapMeter Latitude { get; set; }

	[StiSerializable(StiSerializationVisibility.Class)]
	[DefaultValue(null)]
	[Browsable(false)]
	public StiLongitudeMapMeter Longitude { get; set; }

	[StiSerializable(StiSerializationVisibility.Class)]
	[DefaultValue(null)]
	[Browsable(false)]
	public StiLocationMapMeter Location { get; set; }

	[StiSerializable(StiSerializationVisibility.Class)]
	[DefaultValue(null)]
	[Browsable(false)]
	public StiGisMapMeter Gis { get; set; }

	[StiSerializable(StiSerializationVisibility.Class)]
	[DefaultValue(null)]
	[Browsable(false)]
	public StiGisMapColorMeter GisColor { get; set; }

	[StiSerializable(StiSerializationVisibility.Class)]
	[DefaultValue(null)]
	[Browsable(false)]
	public StiGisMapLineSizeMeter LineSize { get; set; }

	[StiSerializable(StiSerializationVisibility.Class)]
	[DefaultValue(null)]
	[Browsable(false)]
	public StiGisMapDescriptionMeter Description { get; set; }

	[StiSerializable]
	[DefaultValue(StiGeoMapProviderType.Bing)]
	[Browsable(false)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiGeoMapProviderType Provider { get; set; } = StiGeoMapProviderType.Bing;


	[StiSerializable(StiSerializationVisibility.Class)]
	[DefaultValue(null)]
	[Browsable(false)]
	public StiLocationValueMapMeter LocationValue { get; set; }

	[StiSerializable(StiSerializationVisibility.Class)]
	[DefaultValue(null)]
	[Browsable(false)]
	public StiLocationArgumentMapMeter LocationArgument { get; set; }

	[StiSerializable]
	[DefaultValue(StiOnlineMapLocationColorType.Single)]
	[Browsable(false)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiOnlineMapLocationColorType LocationColorType { get; set; }

	[StiSerializable]
	[DefaultValue(StiOnlineMapHeatmapColorGradientType.VisibleSpectrum)]
	[Browsable(false)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiOnlineMapHeatmapColorGradientType HeatmapColorGradientType { get; set; } = StiOnlineMapHeatmapColorGradientType.Sunrise;


	[StiSerializable(StiSerializationVisibility.Class)]
	[DefaultValue(null)]
	[Browsable(false)]
	[Editor("Stimulsoft.Dashboard.Design.Components.StiElementEditor, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public StiLocationColorMapMeter LocationColorMeter { get; set; }

	[StiSerializable]
	[DefaultValue(StiOnlineMapLocationType.Auto)]
	[Browsable(false)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiOnlineMapLocationType LocationType { get; set; }

	[StiSerializable]
	[DefaultValue(StiOnlineMapValueViewMode.Bubble)]
	[Browsable(false)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiOnlineMapValueViewMode ValueViewMode { get; set; }

	[StiSerializable]
	[DefaultValue(StiFontIcons.MapMarker)]
	[Browsable(false)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiFontIcons Icon { get; set; } = StiFontIcons.MapMarker;


	[StiSerializable]
	[TypeConverter(typeof(StiStyleColorConverter))]
	[Browsable(false)]
	public Color IconColor { get; set; } = Color.Maroon;


	[StiSerializable]
	[DefaultValue(null)]
	[Browsable(false)]
	[TypeConverter(typeof(StiImageBytesConverter))]
	public byte[] CustomIcon { get; set; }

	[StiSerializable]
	[DefaultValue(StiOnlineMapCulture.en_US)]
	[Browsable(false)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiOnlineMapCulture Culture { get; set; } = StiOnlineMapCulture.en_US;


	[StiSerializable]
	[TypeConverter(typeof(StiStyleColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiStyleColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Browsable(false)]
	public Color LocationColor { get; set; } = Color.LimeGreen;


	[StiSerializable(StiSerializationVisibility.List)]
	[Browsable(false)]
	public virtual List<IStiHeatmapGradientRange> HeatmapGradientRanges { get; set; } = new List<IStiHeatmapGradientRange>();


	[StiSerializable]
	[DefaultValue(20)]
	[Browsable(false)]
	public int HeatmapRadius { get; set; } = 20;


	[Description("Gets or sets settings of the gis map.")]
	[StiSerializable(StiSerializationVisibility.Class)]
	[StiPropertyLevel(StiLevel.Standard)]
	[StiCategory("OnlineMap", StiCategorySource.PropertyCategory)]
	[StiOrder(102)]
	public StiOnlineMapGisSettings GisSettings { get; set; } = new StiOnlineMapGisSettings();


	public override object Clone(bool cloneProperties)
	{
		StiOnlineMapElement stiOnlineMapElement = ((StiElement)base.Clone(cloneProperties)) as StiOnlineMapElement;
		stiOnlineMapElement.Latitude = Latitude?.Clone() as StiLatitudeMapMeter;
		stiOnlineMapElement.Longitude = Longitude?.Clone() as StiLongitudeMapMeter;
		stiOnlineMapElement.Location = Location?.Clone() as StiLocationMapMeter;
		stiOnlineMapElement.Gis = Gis?.Clone() as StiGisMapMeter;
		stiOnlineMapElement.GisColor = GisColor?.Clone() as StiGisMapColorMeter;
		stiOnlineMapElement.LineSize = LineSize?.Clone() as StiGisMapLineSizeMeter;
		stiOnlineMapElement.Description = Description?.Clone() as StiGisMapDescriptionMeter;
		stiOnlineMapElement.Provider = Provider;
		stiOnlineMapElement.LocationColorMeter = LocationColorMeter?.Clone() as StiLocationColorMapMeter;
		stiOnlineMapElement.LocationValue = LocationValue?.Clone() as StiLocationValueMapMeter;
		stiOnlineMapElement.LocationArgument = LocationArgument?.Clone() as StiLocationArgumentMapMeter;
		stiOnlineMapElement.TransformActions = TransformActions.Select((StiDataActionRule c) => c.Clone() as StiDataActionRule).ToList();
		stiOnlineMapElement.TransformFilters = TransformFilters.Select((StiDataFilterRule c) => c.Clone() as StiDataFilterRule).ToList();
		stiOnlineMapElement.TransformSorts = TransformSorts.Select((StiDataSortRule c) => c.Clone() as StiDataSortRule).ToList();
		stiOnlineMapElement.DataFilters = DataFilters.Select((StiDataFilterRule c) => c.Clone() as StiDataFilterRule).ToList();
		stiOnlineMapElement.DashboardInteraction = DashboardInteraction?.Clone() as StiDashboardInteraction;
		stiOnlineMapElement.Title = ((StiTitle)Title)?.Clone() as StiTitle;
		stiOnlineMapElement.LocationType = LocationType;
		stiOnlineMapElement.Culture = Culture;
		stiOnlineMapElement.LocationColorType = LocationColorType;
		stiOnlineMapElement.LocationColor = LocationColor;
		stiOnlineMapElement.IconColor = IconColor;
		stiOnlineMapElement.Icon = Icon;
		stiOnlineMapElement.CustomIcon = ((CustomIcon != null) ? StiImageConverter.StringToByteArray(System.Convert.ToBase64String(CustomIcon)) : null);
		stiOnlineMapElement.Shadow = (StiSimpleShadow)(Shadow?.Clone());
		stiOnlineMapElement.CornerRadius = CornerRadius?.Clone() as StiCornerRadius;
		return stiOnlineMapElement;
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = base.SaveToJsonObject(mode);
		jObject.AddPropertyJObject("TransformActions", StiJsonReportObjectHelper.Serialize.JObjectCollection(TransformActions, mode));
		jObject.AddPropertyJObject("TransformFilters", StiJsonReportObjectHelper.Serialize.JObjectCollection(TransformFilters, mode));
		jObject.AddPropertyJObject("TransformSorts", StiJsonReportObjectHelper.Serialize.JObjectCollection(TransformSorts, mode));
		jObject.AddPropertyJObject("DataFilters", StiJsonReportObjectHelper.Serialize.JObjectCollection(DataFilters, mode));
		if (DashboardInteraction is StiOnlineMapDashboardInteraction stiOnlineMapDashboardInteraction)
		{
			jObject.AddPropertyJObject("DashboardInteraction", stiOnlineMapDashboardInteraction.SaveToJsonObject(mode));
		}
		if (Title != null)
		{
			jObject.AddPropertyJObject("Title", ((StiTitle)Title).SaveToJsonObject(mode));
		}
		if (Latitude != null)
		{
			jObject.AddPropertyJObject("Latitude", Latitude.SaveToJsonObject(mode));
		}
		if (Longitude != null)
		{
			jObject.AddPropertyJObject("Longitude", Longitude.SaveToJsonObject(mode));
		}
		if (Location != null)
		{
			jObject.AddPropertyJObject("Location", Location.SaveToJsonObject(mode));
		}
		if (Gis != null)
		{
			jObject.AddPropertyJObject("Gis", Gis.SaveToJsonObject(mode));
		}
		if (GisColor != null)
		{
			jObject.AddPropertyJObject("GisColor", GisColor.SaveToJsonObject(mode));
		}
		if (LineSize != null)
		{
			jObject.AddPropertyJObject("LineSize", LineSize.SaveToJsonObject(mode));
		}
		if (Description != null)
		{
			jObject.AddPropertyJObject("Description", Description.SaveToJsonObject(mode));
		}
		jObject.AddPropertyEnum("Provider", Provider);
		if (LocationColorMeter != null)
		{
			jObject.AddPropertyJObject("LocationColorMeter", LocationColorMeter.SaveToJsonObject(mode));
		}
		if (LocationArgument != null)
		{
			jObject.AddPropertyJObject("LocationArgument", LocationArgument.SaveToJsonObject(mode));
		}
		if (LocationValue != null)
		{
			jObject.AddPropertyJObject("LocationValue", LocationValue.SaveToJsonObject(mode));
		}
		jObject.AddPropertyStringNullOrEmpty("Group", Group);
		jObject.AddPropertyEnum("DataMode", DataMode, StiDataMode.UsingDataFields);
		jObject.AddPropertyEnum("MapMode", MapMode, StiOnlineMapMode.Location);
		jObject.AddPropertyStringNullOrEmpty("ManuallyEnteredData", ManuallyEnteredData);
		jObject.AddPropertyBool("CrossFiltering", CrossFiltering, defaultValue: true);
		jObject.AddPropertyEnum("Culture", Culture);
		jObject.AddPropertyEnum("LocationType", LocationType);
		jObject.AddPropertyColor("LocationColor", LocationColor, Color.LimeGreen);
		jObject.AddPropertyColor("IconColor", IconColor, Color.Maroon);
		jObject.AddPropertyEnum("LocationColorType", LocationColorType);
		jObject.AddPropertyEnum("ValueViewMode", ValueViewMode);
		jObject.AddPropertyEnum("Icon", Icon);
		jObject.AddPropertyCornerRadius("CornerRadius", CornerRadius);
		jObject.AddPropertyShadow("Shadow", Shadow);
		if (GisSettings != null)
		{
			jObject.AddPropertyJObject("GisSettings", GisSettings.SaveToJsonObject(mode));
		}
		if (CustomIcon != null)
		{
			jObject.Add("CustomIcon", System.Convert.ToBase64String(CustomIcon));
		}
		return jObject;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "DashboardInteraction":
			{
				StiOnlineMapDashboardInteraction stiOnlineMapDashboardInteraction = StiDashboardInteractionLoader.LoadInteractionFromJsonObject((JObject)item.Value) as StiOnlineMapDashboardInteraction;
				DashboardInteraction = stiOnlineMapDashboardInteraction ?? null;
				break;
			}
			case "TransformActions":
				TransformActions.AddRange(from p in ((JObject)item.Value).Properties()
					select StiDataActionRule.LoadFromJson((JObject)p.Value));
				break;
			case "TransformFilters":
				TransformFilters.AddRange(from p in ((JObject)item.Value).Properties()
					select StiDataFilterRule.LoadFromJson((JObject)p.Value));
				break;
			case "TransformSorts":
				TransformSorts.AddRange(from p in ((JObject)item.Value).Properties()
					select StiDataSortRule.LoadFromJson((JObject)p.Value));
				break;
			case "DataFilters":
				DataFilters.AddRange(from p in ((JObject)item.Value).Properties()
					select StiDataFilterRule.LoadFromJson((JObject)p.Value));
				break;
			case "Title":
				Title = StiTitle.CreateFromJsonObject((JObject)item.Value);
				break;
			case "Layout":
				Layout = StiElementLayout.CreateFromJsonObject((JObject)item.Value, this);
				break;
			case "Latitude":
			{
				StiLatitudeMapMeter stiLatitudeMapMeter = StiMeterLoader.LoadFromJson((JObject)item.Value) as StiLatitudeMapMeter;
				Latitude = stiLatitudeMapMeter ?? null;
				break;
			}
			case "Longitude":
			{
				StiLongitudeMapMeter stiLongitudeMapMeter = StiMeterLoader.LoadFromJson((JObject)item.Value) as StiLongitudeMapMeter;
				Longitude = stiLongitudeMapMeter ?? null;
				break;
			}
			case "Location":
			{
				StiLocationMapMeter stiLocationMapMeter = StiMeterLoader.LoadFromJson((JObject)item.Value) as StiLocationMapMeter;
				Location = stiLocationMapMeter ?? null;
				break;
			}
			case "Gis":
			{
				StiGisMapMeter stiGisMapMeter = StiMeterLoader.LoadFromJson((JObject)item.Value) as StiGisMapMeter;
				Gis = stiGisMapMeter ?? null;
				break;
			}
			case "GisColor":
			{
				StiGisMapColorMeter stiGisMapColorMeter = StiMeterLoader.LoadFromJson((JObject)item.Value) as StiGisMapColorMeter;
				GisColor = stiGisMapColorMeter ?? null;
				break;
			}
			case "LineSize":
			{
				StiGisMapLineSizeMeter stiGisMapLineSizeMeter = StiMeterLoader.LoadFromJson((JObject)item.Value) as StiGisMapLineSizeMeter;
				LineSize = stiGisMapLineSizeMeter ?? null;
				break;
			}
			case "Description":
			{
				StiGisMapDescriptionMeter stiGisMapDescriptionMeter = StiMeterLoader.LoadFromJson((JObject)item.Value) as StiGisMapDescriptionMeter;
				Description = stiGisMapDescriptionMeter ?? null;
				break;
			}
			case "Provider":
				Provider = item.DeserializeEnum<StiGeoMapProviderType>();
				break;
			case "LocationColorMeter":
			{
				StiLocationColorMapMeter stiLocationColorMapMeter = StiMeterLoader.LoadFromJson((JObject)item.Value) as StiLocationColorMapMeter;
				LocationColorMeter = stiLocationColorMapMeter ?? null;
				break;
			}
			case "LocationValue":
			{
				StiLocationValueMapMeter stiLocationValueMapMeter = StiMeterLoader.LoadFromJson((JObject)item.Value) as StiLocationValueMapMeter;
				LocationValue = stiLocationValueMapMeter ?? null;
				break;
			}
			case "LocationArgument":
			{
				StiLocationArgumentMapMeter stiLocationArgumentMapMeter = StiMeterLoader.LoadFromJson((JObject)item.Value) as StiLocationArgumentMapMeter;
				LocationArgument = stiLocationArgumentMapMeter ?? null;
				break;
			}
			case "Group":
				Group = item.DeserializeString();
				break;
			case "DataMode":
				DataMode = item.DeserializeEnum<StiDataMode>();
				break;
			case "MapMode":
				MapMode = item.DeserializeEnum<StiOnlineMapMode>();
				break;
			case "ManuallyEnteredData":
				ManuallyEnteredData = item.DeserializeString();
				break;
			case "CrossFiltering":
				CrossFiltering = item.DeserializeBool();
				break;
			case "LocationType":
				LocationType = item.DeserializeEnum<StiOnlineMapLocationType>();
				break;
			case "Culture":
				Culture = item.DeserializeEnum<StiOnlineMapCulture>();
				break;
			case "LocationColor":
				LocationColor = item.DeserializeColor();
				break;
			case "IconColor":
				IconColor = item.DeserializeColor();
				break;
			case "GisSettings":
				GisSettings.LoadFromJsonObject((JObject)item.Value);
				break;
			case "LocationColorType":
				LocationColorType = item.DeserializeEnum<StiOnlineMapLocationColorType>();
				break;
			case "ValueViewMode":
				ValueViewMode = item.DeserializeEnum<StiOnlineMapValueViewMode>();
				break;
			case "Icon":
				Icon = item.DeserializeEnum<StiFontIcons>();
				break;
			case "CustomIcon":
				CustomIcon = item.DeserializeByteArray();
				break;
			case "Shadow":
				Shadow = item.DeserializeSimpleShadow();
				break;
			case "CornerRadius":
				CornerRadius = item.DeserializeCornerRadius();
				break;
			}
		}
	}

	public override List<IStiMeter> FetchAllMeters()
	{
		List<IStiMeter> list = new List<IStiMeter>();
		if (Latitude != null)
		{
			list.Add(Latitude);
		}
		if (Longitude != null)
		{
			list.Add(Longitude);
		}
		if (Gis != null)
		{
			list.Add(Gis);
		}
		if (GisColor != null)
		{
			list.Add(GisColor);
		}
		if (LineSize != null)
		{
			list.Add(LineSize);
		}
		if (Description != null)
		{
			list.Add(Description);
		}
		if (Location != null)
		{
			list.Add(Location);
		}
		if (LocationValue != null)
		{
			list.Add(LocationValue);
		}
		if (LocationColorMeter != null)
		{
			list.Add(LocationColorMeter);
		}
		if (LocationArgument != null)
		{
			list.Add(LocationArgument);
		}
		return list;
	}

	public override List<IStiMeter> GetMeters()
	{
		List<IStiMeter> list = new List<IStiMeter>();
		if (DataMode == StiDataMode.UsingDataFields && IsDefined)
		{
			if (Latitude != null)
			{
				list.Add(Latitude);
			}
			if (Longitude != null)
			{
				list.Add(Longitude);
			}
			if (Gis != null)
			{
				list.Add(Gis);
			}
			if (GisColor != null)
			{
				list.Add(GisColor);
			}
			if (LineSize != null)
			{
				list.Add(LineSize);
			}
			if (Description != null)
			{
				list.Add(Description);
			}
			if (Location != null)
			{
				list.Add(Location);
			}
			if (LocationValue != null)
			{
				list.Add(LocationValue);
			}
			if (LocationColorMeter != null)
			{
				list.Add(LocationColorMeter);
			}
			if (LocationArgument != null)
			{
				list.Add(LocationArgument);
			}
		}
		return list;
	}

	public override List<string> RetrieveUsedDataNames()
	{
		if (DataMode == StiDataMode.UsingDataFields)
		{
			List<string> list = new List<string>();
			list.AddRange(StiUsedDataHelper.GetMany(Latitude));
			list.AddRange(StiUsedDataHelper.GetMany(Longitude));
			list.AddRange(StiUsedDataHelper.GetMany(Gis));
			list.AddRange(StiUsedDataHelper.GetMany(GisColor));
			list.AddRange(StiUsedDataHelper.GetMany(LineSize));
			list.AddRange(StiUsedDataHelper.GetMany(Description));
			list.AddRange(StiUsedDataHelper.GetMany(Location));
			list.AddRange(StiUsedDataHelper.GetMany(LocationValue));
			list.AddRange(StiUsedDataHelper.GetMany(LocationColorMeter));
			list.AddRange(StiUsedDataHelper.GetMany(LocationArgument));
			return list.Distinct().ToList();
		}
		return null;
	}

	private bool ShouldSerializeDataTransformation()
	{
		if (TransformActions != null && !TransformActions.Any() && TransformFilters != null && !TransformFilters.Any() && TransformSorts != null)
		{
			return TransformSorts.Any();
		}
		return true;
	}

	public bool IsDefaultDataTransformation()
	{
		return !ShouldSerializeDataTransformation();
	}

	public void CreateNextMeter(IStiAppDataCell cell)
	{
		Location = ((cell != null) ? StiMeterHelper.OnlineMap.GetLocation(cell) : null);
	}

	public void AddLatitudeMeter(IStiAppDataCell cell)
	{
		Latitude = ((cell != null) ? StiMeterHelper.OnlineMap.GetLatitude(cell) : null);
	}

	public void AddLatitudeMeter(IStiMeter meter)
	{
		Latitude = ((meter != null) ? StiMeterHelper.OnlineMap.GetLatitude(meter as StiMeter) : null);
	}

	public IStiMeter GetLatitudeMeter()
	{
		return Latitude;
	}

	public void RemoveLatitudeMeter()
	{
		Latitude = null;
	}

	public void CreateNewLatitudeMeter()
	{
		Latitude = new StiLatitudeMapMeter();
	}

	public void AddLongitudeMeter(IStiAppDataCell cell)
	{
		Longitude = ((cell != null) ? StiMeterHelper.OnlineMap.GetLongitude(cell) : null);
	}

	public void AddLongitudeMeter(IStiMeter meter)
	{
		Longitude = ((meter != null) ? StiMeterHelper.OnlineMap.GetLongitude(meter as StiMeter) : null);
	}

	public IStiMeter GetLongitudeMeter()
	{
		return Longitude;
	}

	public void RemoveLongitudeMeter()
	{
		Longitude = null;
	}

	public void CreateNewLongitudeMeter()
	{
		Longitude = new StiLongitudeMapMeter();
	}

	public void AddGisMeter(IStiAppDataCell cell)
	{
		Gis = ((cell != null) ? StiMeterHelper.OnlineMap.GetGis(cell) : null);
	}

	public void AddGisMeter(IStiMeter meter)
	{
		Gis = ((meter != null) ? StiMeterHelper.OnlineMap.GetGis(meter as StiMeter) : null);
	}

	public IStiMeter GetGisMeter()
	{
		return Gis;
	}

	public void RemoveGisMeter()
	{
		Gis = null;
	}

	public void CreateNewGisMeter()
	{
		Gis = new StiGisMapMeter();
	}

	public void AddGisColorMeter(IStiAppDataCell cell)
	{
		GisColor = ((cell != null) ? StiMeterHelper.OnlineMap.GetGisColor(cell) : null);
	}

	public void AddGisColorMeter(IStiMeter meter)
	{
		GisColor = ((meter != null) ? StiMeterHelper.OnlineMap.GetGisColor(meter as StiMeter) : null);
	}

	public IStiMeter GetGisColorMeter()
	{
		return GisColor;
	}

	public void RemoveGisColorMeter()
	{
		GisColor = null;
	}

	public void CreateNewGisColorMeter()
	{
		GisColor = new StiGisMapColorMeter();
	}

	public void AddLineSizeMeter(IStiAppDataCell cell)
	{
		LineSize = ((cell != null) ? StiMeterHelper.OnlineMap.GetGisLineSize(cell) : null);
	}

	public void AddLineSizeMeter(IStiMeter meter)
	{
		LineSize = ((meter != null) ? StiMeterHelper.OnlineMap.GetGisLineSize(meter as StiMeter) : null);
	}

	public IStiMeter GetLineSizeMeter()
	{
		return LineSize;
	}

	public void RemoveLineSizeMeter()
	{
		LineSize = null;
	}

	public void CreateNewLineSizeMeter()
	{
		LineSize = new StiGisMapLineSizeMeter();
	}

	public void AddDescriptionMeter(IStiAppDataCell cell)
	{
		Description = ((cell != null) ? StiMeterHelper.OnlineMap.GetGisDescription(cell) : null);
	}

	public void AddDescriptionMeter(IStiMeter meter)
	{
		Description = ((meter != null) ? StiMeterHelper.OnlineMap.GetGisDescription(meter as StiMeter) : null);
	}

	public IStiMeter GetDescriptionMeter()
	{
		return Description;
	}

	public void RemoveDescriptionMeter()
	{
		Description = null;
	}

	public void CreateNewDescriptionMeter()
	{
		Description = new StiGisMapDescriptionMeter();
	}

	public void AddLocationMeter(IStiAppDataCell cell)
	{
		Location = ((cell != null) ? StiMeterHelper.OnlineMap.GetLocation(cell) : null);
	}

	public void AddLocationMeter(IStiMeter meter)
	{
		Location = ((meter != null) ? StiMeterHelper.OnlineMap.GetLocation(meter as StiMeter) : null);
	}

	public IStiMeter GetLocationMeter()
	{
		return Location;
	}

	public void RemoveLocationMeter()
	{
		Location = null;
	}

	public void CreateNewLocationMeter()
	{
		Location = new StiLocationMapMeter();
	}

	public void AddLocationColorMeter(IStiAppDataCell cell)
	{
		LocationColorMeter = ((cell != null) ? StiMeterHelper.OnlineMap.GetLocationColorMeter(cell) : null);
	}

	public void AddLocationColorMeter(IStiMeter meter)
	{
		LocationColorMeter = ((meter != null) ? StiMeterHelper.OnlineMap.GetLocationColorMeter(meter as StiMeter) : null);
	}

	public IStiMeter GetLocationColorMeter()
	{
		return LocationColorMeter;
	}

	public void RemoveLocationColorMeter()
	{
		LocationColorMeter = null;
	}

	public void CreateNewLocationColorMeter()
	{
		LocationColorMeter = new StiLocationColorMapMeter();
	}

	public void AddLocationArgumentMeter(IStiAppDataCell cell)
	{
		LocationArgument = ((cell != null) ? StiMeterHelper.OnlineMap.GetLocationArgument(cell) : null);
	}

	public void AddLocationArgumentMeter(IStiMeter meter)
	{
		LocationArgument = ((meter != null) ? StiMeterHelper.OnlineMap.GetLocationArgument(meter as StiMeter) : null);
	}

	public IStiMeter GetLocationArgumentMeter()
	{
		return LocationArgument;
	}

	public void RemoveLocationArgumentMeter()
	{
		LocationArgument = null;
	}

	public void CreateNewLocationArgumentMeter()
	{
		LocationArgument = new StiLocationArgumentMapMeter();
	}

	public void AddLocationValueMeter(IStiAppDataCell cell)
	{
		LocationValue = ((cell != null) ? StiMeterHelper.OnlineMap.GetLocationValue(cell) : null);
	}

	public void AddLocationValueMeter(IStiMeter meter)
	{
		LocationValue = ((meter != null) ? StiMeterHelper.OnlineMap.GetLocationValue(meter as StiMeter) : null);
	}

	public IStiMeter GetLocationValueMeter()
	{
		return LocationValue;
	}

	public void RemoveLocationValueMeter()
	{
		LocationValue = null;
	}

	public void CreateNewLocationValueMeter()
	{
		LocationValue = new StiLocationValueMapMeter();
	}

	public StiDataTable GetManuallyEnteredDataTable()
	{
		if (DataMode != StiDataMode.ManuallyEnteringData)
		{
			return null;
		}
		return StiManuallyEnteredDataParser.GetOnlineMapDataTable(ManuallyEnteredData, this);
	}

	public void ConvertFrom(IStiElement element)
	{
		if (element is StiRegionMapElement { KeyMeter: not null } stiRegionMapElement && !stiRegionMapElement.KeyMeter.IsDefault)
		{
			Location = new StiLocationMapMeter();
			Location.Label = stiRegionMapElement.KeyMeter.Label;
			Location.Expression = stiRegionMapElement.KeyMeter.Expression;
		}
		if (DashboardInteraction == null)
		{
			DashboardInteraction = new StiOnlineMapDashboardInteraction();
		}
	}

	void IStiGlobalizationProvider.SetString(string propertyName, string value)
	{
		if (propertyName == "Title.Text")
		{
			Title.Text = value;
		}
	}

	string IStiGlobalizationProvider.GetString(string propertyName)
	{
		if (propertyName == "Title.Text")
		{
			return Title.Text;
		}
		throw new ArgumentException("Property with name " + propertyName);
	}

	string[] IStiGlobalizationProvider.GetAllStrings()
	{
		return new string[1] { "Title.Text" };
	}

	private bool ShouldSerializeDashboardInteraction()
	{
		if (DashboardInteraction != null)
		{
			return !DashboardInteraction.IsDefault;
		}
		return true;
	}

	public override List<StiFont> GetFonts()
	{
		List<StiFont> fonts = base.GetFonts();
		fonts.Add(new StiFont(Title.Font));
		if (ValueViewMode == StiOnlineMapValueViewMode.Icon && CustomIcon == null)
		{
			fonts.Add(StiFontIconsHelper.GetFont());
		}
		return fonts.Distinct().ToList();
	}

	private bool ShouldSerializeShadow()
	{
		if (Shadow != null)
		{
			return !Shadow.IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeCornerRadius()
	{
		if (CornerRadius != null)
		{
			return !CornerRadius.IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeInteraction()
	{
		if (DashboardInteraction != null)
		{
			return !DashboardInteraction.IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeGisSettings()
	{
		if (GisSettings != null)
		{
			return !GisSettings.IsDefault;
		}
		return true;
	}

	public virtual int GetUniqueCode()
	{
		int num = Latitude?.GetUniqueCode() ?? 1;
		num = (num * 397) ^ (Longitude?.GetUniqueCode() ?? 1);
		num = (num * 397) ^ (Gis?.GetUniqueCode() ?? 1);
		num = (num * 397) ^ (GisColor?.GetUniqueCode() ?? 1);
		num = (num * 397) ^ (LineSize?.GetUniqueCode() ?? 1);
		num = (num * 397) ^ (Description?.GetUniqueCode() ?? 1);
		num = (num * 397) ^ (Location?.GetUniqueCode() ?? 1);
		num = (num * 397) ^ (LocationColorMeter?.GetUniqueCode() ?? 1);
		num = (num * 397) ^ (LocationValue?.GetUniqueCode() ?? 1);
		num = (num * 397) ^ (LocationArgument?.GetUniqueCode() ?? 1);
		if (DataMode == StiDataMode.ManuallyEnteringData && ManuallyEnteredData != null && ManuallyEnteredData.Length > 0)
		{
			num = (num * 397) ^ ManuallyEnteredData.GetHashCode();
		}
		if (TransformActions != null)
		{
			foreach (StiDataActionRule transformAction in TransformActions)
			{
				num = (num * 397) ^ transformAction.GetUniqueCode();
			}
		}
		if (TransformFilters != null)
		{
			foreach (StiDataFilterRule transformFilter in TransformFilters)
			{
				num = (num * 397) ^ transformFilter.GetUniqueCode();
			}
		}
		if (TransformSorts != null)
		{
			foreach (StiDataSortRule transformSort in TransformSorts)
			{
				num = (num * 397) ^ transformSort.GetUniqueCode();
			}
		}
		if (DataFilters != null)
		{
			foreach (StiDataFilterRule dataFilter in DataFilters)
			{
				num = (num * 397) ^ dataFilter.GetUniqueCode();
			}
		}
		if (HeatmapGradientRanges != null)
		{
			foreach (IStiHeatmapGradientRange heatmapGradientRange in HeatmapGradientRanges)
			{
				num = (num * 397) ^ heatmapGradientRange.GetUniqueCode();
			}
		}
		num = (num * 397) ^ (DashboardInteraction?.GetHashCode() ?? 0);
		num = (num * 397) ^ ((int)LocationType * 10);
		num = (num * 397) ^ ((int)Culture * 100);
		num = (num * 397) ^ ((int)LocationColorType * 1000);
		num = (num * 397) ^ ((int)ValueViewMode * 10000);
		num = (num * 397) ^ ((int)Icon * 100000);
		num = (num * 397) ^ ((int)HeatmapColorGradientType * 1000000);
		num = (num * 397) ^ (HeatmapRadius * 10000000);
		num = (num * 397) ^ ((int)Provider * 100000000);
		_ = LocationColor;
		num = (num * 397) ^ LocationColor.GetHashCode();
		_ = IconColor;
		num = (num * 397) ^ IconColor.GetHashCode();
		if (CustomIcon != null)
		{
			num = (num * 397) ^ CustomIcon.GetHashCode();
		}
		if (Title != null)
		{
			num = (num * 397) ^ Title.GetHashCode();
		}
		return (num * 397) ^ StiDashboardStyleHelper.IsDarkStyle(base.Dashboard).GetHashCode();
	}

	public override StiComponent CreateNew()
	{
		return new StiOnlineMapElement();
	}

	public StiOnlineMapElement()
		: this(RectangleD.Empty)
	{
	}

	public StiOnlineMapElement(RectangleD rect)
		: base(rect)
	{
		Title.Text = LocalizedName;
		DashboardInteraction = new StiOnlineMapDashboardInteraction();
		Layout = new StiElementLayout(this);
	}
}
