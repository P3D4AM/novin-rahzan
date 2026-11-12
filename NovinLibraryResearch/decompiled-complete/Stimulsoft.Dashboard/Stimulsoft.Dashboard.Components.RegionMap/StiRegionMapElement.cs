using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Dashboard.Components.Design;
using Stimulsoft.Dashboard.Components.Helpers;
using Stimulsoft.Dashboard.Components.OnlineMap;
using Stimulsoft.Dashboard.Export;
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
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Maps;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Dashboard.Components.RegionMap;

[StiServiceBitmap(typeof(StiElement), "Stimulsoft.Dashboard.Images.Toolbox.StiRegionMap.png")]
[StiExportTool("Stimulsoft.Dashboard.Export.Tools.StiRegionMapElementExportTool, Stimulsoft.Dashboard.Export, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiPainter("Stimulsoft.Dashboard.Drawing.Painters.StiRegionMapElementGdiPainter, Stimulsoft.Dashboard.Drawing, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfPainter("Stimulsoft.Dashboard.Drawing.Wpf.Painters.StiRegionMapElementWpfPainter, Stimulsoft.Dashboard.Drawing.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiDesigner("Stimulsoft.Dashboard.Design.Designers.StiRegionMapElementDesigner, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Dashboard.WpfDesign.Designers.StiRegionMapElementWpfDesigner, Stimulsoft.Dashboard.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiVisualizer("Stimulsoft.Dashboard.Viewer.Visualizers.StiRegionMapElementVisualizer, Stimulsoft.Dashboard.Viewer, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfVisualizer("Stimulsoft.Dashboard.Viewer.Wpf.Visualizers.StiRegionMapElementWpfVisualizer, Stimulsoft.Dashboard.Viewer.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiContextTool(typeof(IStiComponentDesigner))]
[StiContextTool(typeof(IStiTitleElement))]
public class StiRegionMapElement : StiElement, IStiRegionMapElement, IStiElement, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiQueryObject, IStiRetrieval, IStiDashboardElementStyle, IStiUserFilters, IStiTransformActions, IStiTransformFilters, IStiTransformSorts, IStiDataTransformationElement, IStiGroupElement, IStiCrossFiltering, IStiDataFilters, IStiSimpleShadow, IStiCornerRadius, IStiSkipOwnFilter, IStiTitleElement, IStiElementLayout, IStiGlobalizationProvider, IStiElementInteraction, IStiConvertibleElement
{
	private StiElementStyleIdent style = StiElementStyleIdent.Auto;

	private string customStyleName = "";

	private List<StiDataFilterRule> userFilters = new List<StiDataFilterRule>();

	private List<StiDataFilterRule> dataFilters = new List<StiDataFilterRule>();

	private StiSimpleShadow shadow = new StiSimpleShadow();

	private StiCornerRadius cornerRadius = new StiCornerRadius(0f);

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiRegionMapElement;

	[StiSerializable]
	[StiCategory("RegionMap", StiCategorySource.PropertyCategory)]
	[DefaultValue("")]
	[StiOrder(109)]
	[StiPropertyLevel(StiLevel.Standard)]
	public string Group { get; set; } = "";


	[StiSerializable]
	[StiCategory("RegionMap", StiCategorySource.PropertyCategory)]
	[DefaultValue(true)]
	[StiOrder(107)]
	[StiPropertyLevel(StiLevel.Standard)]
	[TypeConverter(typeof(StiBoolConverter))]
	public bool CrossFiltering { get; set; } = true;


	[Browsable(false)]
	bool IStiSkipOwnFilter.AllowSkipOwnFilter => true;

	[Browsable(false)]
	public override bool IsDefined => true;

	[Browsable(false)]
	public override bool IsQuerable => DataFrom == StiMapSource.DataColumns;

	[StiSerializable(StiSerializationVisibility.Class)]
	[StiCategory("Behavior", StiCategorySource.PropertyCategory)]
	[StiOrder(490)]
	[TypeConverter(typeof(StiTitleConverter))]
	public IStiTitle Title { get; set; } = new StiTitle();


	[Browsable(false)]
	public StiElementLayout Layout { get; set; }

	[StiSerializable]
	[DefaultValue(StiElementStyleIdent.Auto)]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[StiOrder(280)]
	[StiPropertyLevel(StiLevel.Basic)]
	[TypeConverter(typeof(StiElementStyleConverter))]
	[Description("Specifies the visual style used to draw this component.")]
	[Editor("Stimulsoft.Dashboard.Design.Components.StiElementStyleEditor, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public StiElementStyleIdent Style
	{
		get
		{
			return style;
		}
		set
		{
			if (style != value)
			{
				style = value;
				if (value != StiElementStyleIdent.Custom)
				{
					CustomStyleName = string.Empty;
				}
				InvokeRefreshControl("Style");
			}
		}
	}

	[StiSerializable]
	[DefaultValue("")]
	[Browsable(false)]
	public string CustomStyleName
	{
		get
		{
			return customStyleName;
		}
		set
		{
			if (customStyleName != value)
			{
				customStyleName = value;
				if (!string.IsNullOrWhiteSpace(value))
				{
					style = StiElementStyleIdent.Custom;
				}
				InvokeRefreshControl("CustomStyleName");
			}
		}
	}

	[Browsable(false)]
	public virtual List<StiDataFilterRule> UserFilters
	{
		get
		{
			return userFilters;
		}
		set
		{
			if (userFilters != value)
			{
				userFilters = value;
				InvokeRefreshControl("UserFilters");
			}
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
	[StiCategory("RegionMap", StiCategorySource.PropertyCategory)]
	[StiPropertyLevel(StiLevel.Standard)]
	[StiOrder(108)]
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

	public override int ToolboxPosition => 308;

	public override string LocalizedName => StiLocalization.Get("Components", "StiRegionMap");

	public override string HelpUrl => "user-manual/dashboards_maps_region_map.htm";

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

	[Browsable(false)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Obsolete("MapID property is obsolete. Please use MapIdent property.")]
	public StiMapID MapID
	{
		get
		{
			throw new NotSupportedException();
		}
		set
		{
			MapIdent = value.ToString();
		}
	}

	[Browsable(false)]
	[StiSerializable]
	public string MapIdent { get; set; } = "USA";


	[Browsable(false)]
	[StiSerializable]
	public string Language { get; set; }

	[StiSerializable]
	[DefaultValue(StiMapSource.Manual)]
	[Browsable(false)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiMapSource DataFrom { get; set; }

	[Browsable(false)]
	[StiSerializable]
	[DefaultValue(null)]
	public string MapData { get; set; }

	[Browsable(false)]
	[StiSerializable]
	[DefaultValue(StiMapType.Individual)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiMapType MapType { get; set; } = StiMapType.Individual;


	[Browsable(false)]
	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiOrder(111)]
	public bool ShowValue { get; set; } = true;


	[Browsable(false)]
	[StiSerializable]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiOrder(112)]
	public bool ShowBubble { get; set; }

	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiCategory("RegionMap", StiCategorySource.PropertyCategory)]
	[StiPropertyLevel(StiLevel.Standard)]
	[StiOrder(110)]
	public bool ShortValue { get; set; } = true;


	[StiSerializable]
	[DefaultValue(false)]
	[Browsable(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	public bool ColorEach { get; set; }

	[Browsable(false)]
	[StiSerializable]
	[DefaultValue(StiDisplayNameType.Full)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiDisplayNameType ShowName { get; set; } = StiDisplayNameType.Full;


	[StiSerializable(StiSerializationVisibility.Class)]
	[DefaultValue(null)]
	[Browsable(false)]
	public StiKeyMapMeter KeyMeter { get; set; }

	[StiSerializable(StiSerializationVisibility.Class)]
	[DefaultValue(null)]
	[Browsable(false)]
	public StiNameMapMeter NameMeter { get; set; }

	[StiSerializable(StiSerializationVisibility.Class)]
	[DefaultValue(null)]
	[Browsable(false)]
	public StiValueMapMeter ValueMeter { get; set; }

	[StiSerializable(StiSerializationVisibility.Class)]
	[DefaultValue(null)]
	[Browsable(false)]
	public StiGroupMapMeter GroupMeter { get; set; }

	[StiSerializable(StiSerializationVisibility.Class)]
	[DefaultValue(null)]
	[Browsable(false)]
	public StiColorMapMeter ColorMeter { get; set; }

	public override object Clone(bool cloneProperties)
	{
		StiRegionMapElement stiRegionMapElement = ((StiElement)base.Clone(cloneProperties)) as StiRegionMapElement;
		stiRegionMapElement.KeyMeter = KeyMeter?.Clone() as StiKeyMapMeter;
		stiRegionMapElement.NameMeter = NameMeter?.Clone() as StiNameMapMeter;
		stiRegionMapElement.ValueMeter = ValueMeter?.Clone() as StiValueMapMeter;
		stiRegionMapElement.GroupMeter = GroupMeter?.Clone() as StiGroupMapMeter;
		stiRegionMapElement.ColorMeter = ColorMeter?.Clone() as StiColorMapMeter;
		stiRegionMapElement.UserFilters = UserFilters.Select((StiDataFilterRule c) => c.Clone() as StiDataFilterRule).ToList();
		stiRegionMapElement.TransformActions = TransformActions.Select((StiDataActionRule c) => c.Clone() as StiDataActionRule).ToList();
		stiRegionMapElement.TransformFilters = TransformFilters.Select((StiDataFilterRule c) => c.Clone() as StiDataFilterRule).ToList();
		stiRegionMapElement.TransformSorts = TransformSorts.Select((StiDataSortRule c) => c.Clone() as StiDataSortRule).ToList();
		stiRegionMapElement.DataFilters = DataFilters.Select((StiDataFilterRule c) => c.Clone() as StiDataFilterRule).ToList();
		stiRegionMapElement.Title = ((StiTitle)Title)?.Clone() as StiTitle;
		stiRegionMapElement.DashboardInteraction = DashboardInteraction?.Clone() as StiDashboardInteraction;
		stiRegionMapElement.Shadow = (StiSimpleShadow)(Shadow?.Clone());
		stiRegionMapElement.CornerRadius = CornerRadius?.Clone() as StiCornerRadius;
		return stiRegionMapElement;
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = base.SaveToJsonObject(mode);
		jObject.AddPropertyJObject("TransformActions", StiJsonReportObjectHelper.Serialize.JObjectCollection(TransformActions, mode));
		jObject.AddPropertyJObject("TransformFilters", StiJsonReportObjectHelper.Serialize.JObjectCollection(TransformFilters, mode));
		jObject.AddPropertyJObject("TransformSorts", StiJsonReportObjectHelper.Serialize.JObjectCollection(TransformSorts, mode));
		jObject.AddPropertyJObject("DataFilters", StiJsonReportObjectHelper.Serialize.JObjectCollection(DataFilters, mode));
		if (DashboardInteraction is StiRegionMapDashboardInteraction)
		{
			jObject.AddPropertyJObject("DashboardInteraction", (DashboardInteraction as StiRegionMapDashboardInteraction).SaveToJsonObject(mode));
		}
		if (Title != null)
		{
			jObject.AddPropertyJObject("Title", ((StiTitle)Title).SaveToJsonObject(mode));
		}
		if (KeyMeter != null)
		{
			jObject.AddPropertyJObject("KeyMeter", KeyMeter.SaveToJsonObject(mode));
		}
		if (NameMeter != null)
		{
			jObject.AddPropertyJObject("NameMeter", NameMeter.SaveToJsonObject(mode));
		}
		if (ValueMeter != null)
		{
			jObject.AddPropertyJObject("ValueMeter", ValueMeter.SaveToJsonObject(mode));
		}
		if (GroupMeter != null)
		{
			jObject.AddPropertyJObject("GroupMeter", GroupMeter.SaveToJsonObject(mode));
		}
		if (ColorMeter != null)
		{
			jObject.AddPropertyJObject("ColorMeter", ColorMeter.SaveToJsonObject(mode));
		}
		jObject.AddPropertyStringNullOrEmpty("MapIdent", MapIdent);
		jObject.AddPropertyStringNullOrEmpty("Language", Language);
		jObject.AddPropertyEnum("DataFrom", DataFrom, StiMapSource.Manual);
		jObject.AddPropertyEnum("MapType", MapType, StiMapType.Individual);
		jObject.AddPropertyStringNullOrEmpty("MapData", MapData);
		jObject.AddPropertyBool("ShowValue", ShowValue, defaultValue: true);
		jObject.AddPropertyBool("ShowBubble", ShowBubble);
		jObject.AddPropertyBool("ShortValue", ShortValue, defaultValue: true);
		jObject.AddPropertyBool("ColorEach", ColorEach);
		jObject.AddPropertyEnum("ShowName", ShowName, StiDisplayNameType.Full);
		jObject.AddPropertyEnum("Style", Style, StiElementStyleIdent.Auto);
		jObject.AddPropertyStringNullOrEmpty("CustomStyleName", CustomStyleName);
		jObject.AddPropertyStringNullOrEmpty("Group", Group);
		jObject.AddPropertyBool("CrossFiltering", CrossFiltering, defaultValue: true);
		jObject.AddPropertyCornerRadius("CornerRadius", CornerRadius);
		jObject.AddPropertyShadow("Shadow", Shadow);
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
				StiRegionMapDashboardInteraction stiRegionMapDashboardInteraction = StiDashboardInteractionLoader.LoadInteractionFromJsonObject((JObject)item.Value) as StiRegionMapDashboardInteraction;
				DashboardInteraction = stiRegionMapDashboardInteraction ?? null;
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
			case "KeyMeter":
			{
				StiKeyMapMeter stiKeyMapMeter = StiMeterLoader.LoadFromJson((JObject)item.Value) as StiKeyMapMeter;
				KeyMeter = stiKeyMapMeter ?? null;
				break;
			}
			case "NameMeter":
			{
				StiNameMapMeter stiNameMapMeter = StiMeterLoader.LoadFromJson((JObject)item.Value) as StiNameMapMeter;
				NameMeter = stiNameMapMeter ?? null;
				break;
			}
			case "ValueMeter":
			{
				StiValueMapMeter stiValueMapMeter = StiMeterLoader.LoadFromJson((JObject)item.Value) as StiValueMapMeter;
				ValueMeter = stiValueMapMeter ?? null;
				break;
			}
			case "GroupMeter":
			{
				StiGroupMapMeter stiGroupMapMeter = StiMeterLoader.LoadFromJson((JObject)item.Value) as StiGroupMapMeter;
				GroupMeter = stiGroupMapMeter ?? null;
				break;
			}
			case "ColorMeter":
			{
				StiColorMapMeter stiColorMapMeter = StiMeterLoader.LoadFromJson((JObject)item.Value) as StiColorMapMeter;
				ColorMeter = stiColorMapMeter ?? null;
				break;
			}
			case "MapID":
			case "MapIdent":
				MapIdent = item.DeserializeString();
				break;
			case "Language":
				Language = item.DeserializeString();
				break;
			case "MapType":
				MapType = item.DeserializeEnum<StiMapType>();
				break;
			case "DataFrom":
				DataFrom = item.DeserializeEnum<StiMapSource>();
				break;
			case "MapData":
				MapData = item.DeserializeString();
				break;
			case "ShowValue":
				ShowValue = item.DeserializeBool();
				break;
			case "ShowBubble":
				ShowBubble = item.DeserializeBool();
				break;
			case "ShortValue":
				ShortValue = item.DeserializeBool();
				break;
			case "ColorEach":
				ColorEach = item.DeserializeBool();
				break;
			case "ShowName":
				ShowName = item.DeserializeEnum<StiDisplayNameType>();
				break;
			case "Style":
				Style = item.DeserializeEnum<StiElementStyleIdent>();
				break;
			case "Group":
				Group = item.DeserializeString();
				break;
			case "CrossFiltering":
				CrossFiltering = item.DeserializeBool();
				break;
			case "CustomStyleName":
				CustomStyleName = item.DeserializeString();
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
		if (KeyMeter != null)
		{
			list.Add(KeyMeter);
		}
		if (NameMeter != null)
		{
			list.Add(NameMeter);
		}
		if (ValueMeter != null)
		{
			list.Add(ValueMeter);
		}
		if (GroupMeter != null)
		{
			list.Add(GroupMeter);
		}
		if (ColorMeter != null)
		{
			list.Add(ColorMeter);
		}
		return list;
	}

	public override List<IStiMeter> GetMeters()
	{
		List<IStiMeter> list = new List<IStiMeter>();
		if (IsDefined && DataFrom == StiMapSource.DataColumns)
		{
			if (KeyMeter != null)
			{
				if (StiExpressionHelper.IsFunctionPresent(KeyMeter.Expression))
				{
					list.Add(KeyMeter);
				}
				else
				{
					list.Add(new StiKeyMapMeter
					{
						Key = KeyMeter.Key,
						Expression = "Iso2(" + KeyMeter.Expression + ", \"" + MapIdent + "\")"
					});
				}
			}
			if (NameMeter != null)
			{
				list.Add(NameMeter);
			}
			if (ValueMeter != null)
			{
				if (StiExpressionHelper.IsAggregationFunctionPresent(ValueMeter.Expression))
				{
					list.Add(ValueMeter);
				}
				else
				{
					list.Add(new StiValueMapMeter
					{
						Key = ValueMeter.Key,
						Expression = "First(" + ValueMeter.Expression + ")"
					});
				}
			}
			if (GroupMeter != null)
			{
				list.Add(GroupMeter);
			}
			if (ColorMeter != null)
			{
				list.Add(ColorMeter);
			}
		}
		return list;
	}

	public override List<string> RetrieveUsedDataNames()
	{
		List<string> list = new List<string>();
		if (DataFrom == StiMapSource.DataColumns)
		{
			list.AddRange(StiUsedDataHelper.GetMany(KeyMeter));
			list.AddRange(StiUsedDataHelper.GetMany(NameMeter));
			list.AddRange(StiUsedDataHelper.GetMany(ValueMeter));
			list.AddRange(StiUsedDataHelper.GetMany(GroupMeter));
			list.AddRange(StiUsedDataHelper.GetMany(ColorMeter));
		}
		return list.Distinct().ToList();
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
		if (KeyMeter == null)
		{
			KeyMeter = StiMeterHelper.RegionMap.GetKey(cell);
		}
		else if (NameMeter == null)
		{
			NameMeter = StiMeterHelper.RegionMap.GetName(cell);
		}
		else if (ValueMeter == null)
		{
			ValueMeter = StiMeterHelper.RegionMap.GetValue(cell);
		}
		else if (GroupMeter == null)
		{
			GroupMeter = StiMeterHelper.RegionMap.GetGroup(cell);
		}
		else if (ColorMeter == null)
		{
			ColorMeter = StiMeterHelper.RegionMap.GetColor(cell);
		}
		else
		{
			KeyMeter = StiMeterHelper.RegionMap.GetKey(cell);
		}
	}

	public void AddKeyMeter(IStiAppDataCell cell)
	{
		KeyMeter = ((cell != null) ? StiMeterHelper.RegionMap.GetKey(cell) : null);
	}

	public void AddKeyMeter(IStiMeter meter)
	{
		KeyMeter = ((meter != null) ? StiMeterHelper.RegionMap.GetKey(meter as StiMeter) : null);
	}

	public IStiMeter GetKeyMeter()
	{
		return KeyMeter;
	}

	public void RemoveKeyMeter()
	{
		KeyMeter = null;
	}

	public void CreateNewKeyMeter()
	{
		KeyMeter = new StiKeyMapMeter();
	}

	public void AddNameMeter(IStiAppDataCell cell)
	{
		NameMeter = ((cell != null) ? StiMeterHelper.RegionMap.GetName(cell) : null);
	}

	public void AddNameMeter(IStiMeter meter)
	{
		NameMeter = ((meter != null) ? StiMeterHelper.RegionMap.GetName(meter as StiMeter) : null);
	}

	public IStiMeter GetNameMeter()
	{
		return NameMeter;
	}

	public void RemoveNameMeter()
	{
		NameMeter = null;
	}

	public void CreateNewNameMeter()
	{
		NameMeter = new StiNameMapMeter();
	}

	public void AddValueMeter(IStiAppDataCell cell)
	{
		ValueMeter = ((cell != null) ? StiMeterHelper.RegionMap.GetValue(cell) : null);
	}

	public void AddValueMeter(IStiMeter meter)
	{
		ValueMeter = ((meter != null) ? StiMeterHelper.RegionMap.GetValue(meter as StiMeter, base.Dashboard) : null);
	}

	public IStiMeter GetValueMeter()
	{
		return ValueMeter;
	}

	public void RemoveValueMeter()
	{
		ValueMeter = null;
	}

	public void CreateNewValueMeter()
	{
		ValueMeter = new StiValueMapMeter();
	}

	public void AddGroupMeter(IStiAppDataCell cell)
	{
		GroupMeter = ((cell != null) ? StiMeterHelper.RegionMap.GetGroup(cell) : null);
	}

	public void AddGroupMeter(IStiMeter meter)
	{
		GroupMeter = ((meter != null) ? StiMeterHelper.RegionMap.GetGroup(meter as StiMeter) : null);
	}

	public IStiMeter GetGroupMeter()
	{
		return GroupMeter;
	}

	public void RemoveGroupMeter()
	{
		GroupMeter = null;
	}

	public void CreateNewGroupMeter()
	{
		GroupMeter = new StiGroupMapMeter();
	}

	public void AddColorMeter(IStiAppDataCell cell)
	{
		ColorMeter = ((cell != null) ? StiMeterHelper.RegionMap.GetColor(cell) : null);
	}

	public void AddColorMeter(IStiMeter meter)
	{
		ColorMeter = ((meter != null) ? StiMeterHelper.RegionMap.GetColor(meter as StiMeter) : null);
	}

	public IStiMeter GetColorMeter()
	{
		return ColorMeter;
	}

	public void RemoveColorMeter()
	{
		ColorMeter = null;
	}

	public void CreateNewColorMeter()
	{
		ColorMeter = new StiColorMapMeter();
	}

	public void ConvertFrom(IStiElement element)
	{
		if (element is StiOnlineMapElement { Location: not null } stiOnlineMapElement && !stiOnlineMapElement.Location.IsDefault)
		{
			KeyMeter = new StiKeyMapMeter();
			KeyMeter.Label = stiOnlineMapElement.Location.Label;
			KeyMeter.Expression = stiOnlineMapElement.Location.Expression;
		}
		if (DashboardInteraction == null)
		{
			DashboardInteraction = new StiRegionMapDashboardInteraction();
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

	public override List<StiPage> GetNestedPages()
	{
		if (DashboardInteraction == null || DashboardInteraction.OnClick == StiInteractionOnClick.None)
		{
			return null;
		}
		string pageKey = DashboardInteraction?.DrillDownPageKey;
		if (StiKeyHelper.IsEmptyKey(pageKey))
		{
			return null;
		}
		StiPage stiPage = Report.Pages.ToList().FirstOrDefault((StiPage p) => p.Guid == pageKey);
		if (stiPage == null)
		{
			return null;
		}
		return new List<StiPage> { stiPage };
	}

	public List<StiMapData> GetMapData()
	{
		return StiMap.GetMapDataInternal(Report, MapData, MapIdent, Language);
	}

	public override StiComponent CreateNew()
	{
		return new StiRegionMapElement();
	}

	public StiRegionMapElement()
		: this(RectangleD.Empty)
	{
	}

	public StiRegionMapElement(RectangleD rect)
		: base(rect)
	{
		Title.Text = LocalizedName;
		DashboardInteraction = new StiRegionMapDashboardInteraction();
		Layout = new StiElementLayout(this);
	}
}
