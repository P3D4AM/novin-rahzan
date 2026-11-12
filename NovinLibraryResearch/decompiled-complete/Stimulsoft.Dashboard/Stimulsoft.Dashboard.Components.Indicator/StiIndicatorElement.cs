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
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Dashboard.Components.Design;
using Stimulsoft.Dashboard.Components.Helpers;
using Stimulsoft.Dashboard.Components.Image.Design;
using Stimulsoft.Dashboard.Components.Indicator.Design;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Dashboard.Interactions;
using Stimulsoft.Dashboard.Interactions.Design;
using Stimulsoft.Dashboard.Visualizers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Dashboard.Components.Indicator;

[StiServiceBitmap(typeof(StiElement), "Stimulsoft.Dashboard.Images.Toolbox.StiIndicator.png")]
[StiGdiPainter("Stimulsoft.Dashboard.Drawing.Painters.StiIndicatorElementGdiPainter, Stimulsoft.Dashboard.Drawing, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfPainter("Stimulsoft.Dashboard.Drawing.Wpf.Painters.StiIndicatorElementWpfPainter, Stimulsoft.Dashboard.Drawing.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiDesigner("Stimulsoft.Dashboard.Design.Designers.StiIndicatorElementDesigner, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Dashboard.WpfDesign.Designers.StiIndicatorElementWpfDesigner, Stimulsoft.Dashboard.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiVisualizer("Stimulsoft.Dashboard.Viewer.Visualizers.StiIndicatorElementVisualizer, Stimulsoft.Dashboard.Viewer, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfVisualizer("Stimulsoft.Dashboard.Viewer.Wpf.Visualizers.StiIndicatorElementWpfVisualizer, Stimulsoft.Dashboard.Viewer.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiContextTool(typeof(IStiComponentDesigner))]
[StiContextTool(typeof(IStiTitleElement))]
public class StiIndicatorElement : StiElement, IStiTextFormat, IStiIndicatorElement, IStiElement, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiQueryObject, IStiRetrieval, IStiUserSorts, IStiDashboardElementStyle, IStiTransformActions, IStiTransformFilters, IStiTransformSorts, IStiDataTopN, IStiDataTransformationElement, IStiGroupElement, IStiCrossFiltering, IStiDataFilters, IStiConvertibleElement, IStiManuallyEnteredData, IStiSimpleShadow, IStiCornerRadius, IStiTitleElement, IStiFont, IStiForeColor, IStiElementLayout, IStiElementInteraction, IStiGlobalizationProvider, IStiAllowSortByVariation
{
	private Font font = new Font("Arial", 13f);

	private Color foreColor = Color.Transparent;

	private List<StiDataSortRule> userSorts = new List<StiDataSortRule>();

	private List<StiDataFilterRule> dataFilters = new List<StiDataFilterRule>();

	private StiElementStyleIdent style = StiElementStyleIdent.Auto;

	private string customStyleName = "";

	private StiSimpleShadow shadow = new StiSimpleShadow();

	private StiCornerRadius cornerRadius = new StiCornerRadius(0f);

	private StiIconAlignment iconAlignment = StiIconAlignment.Right;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiIndicatorElement;

	[StiSerializable]
	[StiCategory("Indicator", StiCategorySource.PropertyCategory)]
	[DefaultValue("")]
	[StiOrder(104)]
	[StiPropertyLevel(StiLevel.Standard)]
	public string Group { get; set; } = "";


	[StiSerializable]
	[StiCategory("Indicator", StiCategorySource.PropertyCategory)]
	[DefaultValue(true)]
	[StiOrder(102)]
	[StiPropertyLevel(StiLevel.Standard)]
	[TypeConverter(typeof(StiBoolConverter))]
	public bool CrossFiltering { get; set; } = true;


	[StiSerializable]
	[DefaultValue(StiDataMode.UsingDataFields)]
	[Browsable(false)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiDataMode DataMode { get; set; }

	[StiSerializable]
	[DefaultValue(null)]
	[Browsable(false)]
	public string ManuallyEnteredData { get; set; }

	[Editor("Stimulsoft.Report.Components.TextFormats.Design.StiTextFormatEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiSerializable(StiSerializationVisibility.Class, StiSerializeTypes.SerializeToAll)]
	[StiCategory("Behavior", StiCategorySource.PropertyCategory)]
	[StiOrder(480)]
	[StiPropertyLevel(StiLevel.Standard)]
	public StiFormatService TextFormat { get; set; } = new StiGeneralFormatService();


	[StiSerializable]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[Description("The font used to display text in the component.")]
	[StiOrder(180)]
	[Editor("Stimulsoft.Report.Design.Components.StiFontEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Font Font
	{
		get
		{
			return font;
		}
		set
		{
			if (font != value)
			{
				font = value;
				InvokeRefreshControl("Font");
			}
		}
	}

	[StiSerializable]
	[TypeConverter(typeof(StiStyleColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionStyleColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[Description("The foreground color of this component, which is used to display text.")]
	[StiOrder(210)]
	[StiExpressionAllowed]
	public Color ForeColor
	{
		get
		{
			return foreColor;
		}
		set
		{
			if (foreColor != value)
			{
				foreColor = value;
				InvokeRefreshControl("ForeColor");
			}
		}
	}

	[Browsable(false)]
	public override bool IsDefined
	{
		get
		{
			if (Value == null && Target == null && Series == null)
			{
				return DataMode == StiDataMode.ManuallyEnteringData;
			}
			return true;
		}
	}

	[StiSerializable(StiSerializationVisibility.Class)]
	[StiCategory("Behavior", StiCategorySource.PropertyCategory)]
	[StiOrder(490)]
	[TypeConverter(typeof(StiTitleConverter))]
	public IStiTitle Title { get; set; } = new StiTitle();


	[Browsable(false)]
	public StiElementLayout Layout { get; set; }

	[StiSerializable(StiSerializationVisibility.List)]
	[Browsable(false)]
	public List<StiDataSortRule> UserSorts
	{
		get
		{
			return userSorts;
		}
		set
		{
			if (userSorts != value)
			{
				userSorts = value;
				InvokeRefreshControl("UserSorts");
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


	[Browsable(false)]
	[StiSerializable(StiSerializationVisibility.Class)]
	public StiDataTopN TopN { get; set; } = new StiDataTopN();


	[Editor("Stimulsoft.Report.Components.Design.StiDataTransformationEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiDataTransformationConverter))]
	[StiCategory("Indicator", StiCategorySource.PropertyCategory)]
	[StiPropertyLevel(StiLevel.Standard)]
	[StiOrder(103)]
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

	public override int ToolboxPosition => 306;

	public override string LocalizedName => Loc.Get("Components", "StiIndicator");

	[Browsable(false)]
	public override RectangleD DefaultClientRectangle => new RectangleD(0.0, 0.0, 160.0, 160.0);

	public override string HelpUrl => "user-manual/dashboards_indicator.htm";

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

	[StiSerializable]
	[TypeConverter(typeof(StiStyleColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiStyleColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[Description("Gets or sets a glyph color for this element.")]
	[StiOrder(220)]
	public Color GlyphColor { get; set; } = Color.Transparent;


	[StiSerializable(StiSerializationVisibility.Class)]
	[DefaultValue(null)]
	[Browsable(false)]
	public StiValueIndicatorMeter Value { get; set; }

	[StiSerializable(StiSerializationVisibility.Class)]
	[DefaultValue(null)]
	[Browsable(false)]
	public StiTargetIndicatorMeter Target { get; set; }

	[StiSerializable(StiSerializationVisibility.Class)]
	[DefaultValue(null)]
	[Browsable(false)]
	public StiSeriesIndicatorMeter Series { get; set; }

	[StiSerializable]
	[DefaultValue(null)]
	[Browsable(false)]
	[TypeConverter(typeof(StiImageBytesConverter))]
	[Editor("Stimulsoft.Dashboard.Design.Components.StiElementEditor, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiPropertyLevel(StiLevel.Basic)]
	public byte[] CustomIcon { get; set; }

	[StiSerializable]
	[DefaultValue(StiFontIcons.Rating4)]
	[Browsable(false)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiFontIcons Icon { get; set; } = StiFontIcons.Rating4;


	[StiSerializable]
	[Browsable(false)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiFontIconSet IconSet { get; set; }

	[StiSerializable]
	[DefaultValue(StiIconAlignment.Right)]
	[StiCategory("Behavior", StiCategorySource.PropertyCategory)]
	[StiOrder(197)]
	[StiPropertyLevel(StiLevel.Standard)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Editor("Stimulsoft.Dashboard.Design.Components.StiIndicatorIconAlignmentEditor, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public StiIconAlignment IconAlignment
	{
		get
		{
			return iconAlignment;
		}
		set
		{
			if (value == StiIconAlignment.Center)
			{
				value = StiIconAlignment.None;
			}
			iconAlignment = value;
		}
	}

	[StiSerializable]
	[DefaultValue(StiTargetMode.Variation)]
	[StiCategory("Indicator", StiCategorySource.PropertyCategory)]
	[StiOrder(108)]
	[StiPropertyLevel(StiLevel.Standard)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiTargetMode TargetMode { get; set; } = StiTargetMode.Variation;


	[StiSerializable(StiSerializationVisibility.List)]
	[TypeConverter(typeof(StiIndicatorConditionConverter))]
	[Browsable(false)]
	public List<StiIndicatorElementCondition> IndicatorConditions { get; set; } = new List<StiIndicatorElementCondition>();


	[RefreshProperties(RefreshProperties.All)]
	[Editor("Stimulsoft.Dashboard.Design.Components.StiIndicatorConditionsEditor, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[StiPropertyLevel(StiLevel.Standard)]
	[StiOrder(140)]
	[StiSerializable(StiSerializationVisibility.List)]
	[TypeConverter(typeof(StiIndicatorConditionConverter))]
	[Browsable(true)]
	public override StiConditionsCollection Conditions
	{
		get
		{
			return base.Conditions;
		}
		set
		{
			base.Conditions = value;
		}
	}

	[StiSerializable]
	[DefaultValue(StiFontSizeMode.Auto)]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[StiOrder(185)]
	[StiPropertyLevel(StiLevel.Standard)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiFontSizeMode FontSizeMode { get; set; }

	[StiSerializable]
	[DefaultValue(StiIndicatorIconMode.Auto)]
	[Browsable(false)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiIndicatorIconMode IconMode { get; set; } = StiIndicatorIconMode.Auto;


	[StiSerializable]
	[DefaultValue(StiIndicatorIconMode.Auto)]
	[Browsable(false)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiIndicatorIconRangeMode IconRangeMode { get; set; } = StiIndicatorIconRangeMode.Percentage;


	[StiSerializable(StiSerializationVisibility.List)]
	[Browsable(false)]
	public virtual List<StiIndicatorIconRange> IconRanges { get; set; } = new List<StiIndicatorIconRange>();


	public override object Clone(bool cloneProperties)
	{
		StiIndicatorElement stiIndicatorElement = ((StiElement)base.Clone(cloneProperties)) as StiIndicatorElement;
		stiIndicatorElement.Value = Value?.Clone() as StiValueIndicatorMeter;
		stiIndicatorElement.Target = Target?.Clone() as StiTargetIndicatorMeter;
		stiIndicatorElement.Series = Series?.Clone() as StiSeriesIndicatorMeter;
		stiIndicatorElement.UserSorts = UserSorts.Select((StiDataSortRule c) => c.Clone() as StiDataSortRule).ToList();
		stiIndicatorElement.TransformActions = TransformActions.Select((StiDataActionRule c) => c.Clone() as StiDataActionRule).ToList();
		stiIndicatorElement.TransformFilters = TransformFilters.Select((StiDataFilterRule c) => c.Clone() as StiDataFilterRule).ToList();
		stiIndicatorElement.TransformSorts = TransformSorts.Select((StiDataSortRule c) => c.Clone() as StiDataSortRule).ToList();
		stiIndicatorElement.DataFilters = DataFilters.Select((StiDataFilterRule c) => c.Clone() as StiDataFilterRule).ToList();
		stiIndicatorElement.TopN = TopN.Clone() as StiDataTopN;
		stiIndicatorElement.TextFormat = TextFormat.Clone() as StiFormatService;
		stiIndicatorElement.Title = ((StiTitle)Title)?.Clone() as StiTitle;
		stiIndicatorElement.DashboardInteraction = DashboardInteraction?.Clone() as StiDashboardInteraction;
		stiIndicatorElement.IndicatorConditions = IndicatorConditions.Select((StiIndicatorElementCondition c) => c.Clone() as StiIndicatorElementCondition).ToList();
		stiIndicatorElement.Shadow = (StiSimpleShadow)(Shadow?.Clone());
		stiIndicatorElement.CornerRadius = CornerRadius?.Clone() as StiCornerRadius;
		return stiIndicatorElement;
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = base.SaveToJsonObject(mode);
		jObject.AddPropertyJObject("UserSorts", StiJsonReportObjectHelper.Serialize.JObjectCollection(UserSorts, mode));
		jObject.AddPropertyJObject("TransformActions", StiJsonReportObjectHelper.Serialize.JObjectCollection(TransformActions, mode));
		jObject.AddPropertyJObject("TransformFilters", StiJsonReportObjectHelper.Serialize.JObjectCollection(TransformFilters, mode));
		jObject.AddPropertyJObject("TransformSorts", StiJsonReportObjectHelper.Serialize.JObjectCollection(TransformSorts, mode));
		jObject.AddPropertyJObject("DataFilters", StiJsonReportObjectHelper.Serialize.JObjectCollection(DataFilters, mode));
		jObject.AddPropertyJObject("IconRanges", StiJsonReportObjectHelper.Serialize.JObjectCollection(IconRanges, mode));
		if (DashboardInteraction is StiIndicatorDashboardInteraction stiIndicatorDashboardInteraction)
		{
			jObject.AddPropertyJObject("DashboardInteraction", stiIndicatorDashboardInteraction.SaveToJsonObject(mode));
		}
		if (TopN != null)
		{
			jObject.AddPropertyJObject("TopN", TopN.SaveToJsonObject(mode));
		}
		if (!(TextFormat is StiGeneralFormatService) && TextFormat != null)
		{
			jObject.AddPropertyJObject("TextFormat", TextFormat.SaveToJsonObject(mode));
		}
		if (Value != null)
		{
			jObject.AddPropertyJObject("Value", Value.SaveToJsonObject(mode));
		}
		if (Target != null)
		{
			jObject.AddPropertyJObject("Target", Target.SaveToJsonObject(mode));
		}
		if (Series != null)
		{
			jObject.AddPropertyJObject("Series", Series.SaveToJsonObject(mode));
		}
		if (Title != null)
		{
			jObject.AddPropertyJObject("Title", ((StiTitle)Title).SaveToJsonObject(mode));
		}
		jObject.AddPropertyColor("ForeColor", ForeColor, Color.Transparent);
		jObject.AddPropertyColor("GlyphColor", GlyphColor, Color.Transparent);
		jObject.AddPropertyFontArial8("Font", Font);
		jObject.AddPropertyEnum("DataMode", DataMode, StiDataMode.UsingDataFields);
		jObject.AddPropertyStringNullOrEmpty("ManuallyEnteredData", ManuallyEnteredData);
		jObject.AddPropertyEnum("Style", Style, StiElementStyleIdent.Auto);
		jObject.AddPropertyStringNullOrEmpty("Group", Group);
		jObject.AddPropertyBool("CrossFiltering", CrossFiltering, defaultValue: true);
		jObject.AddPropertyStringNullOrEmpty("CustomStyleName", CustomStyleName);
		jObject.AddPropertyEnum("Icon", Icon, StiFontIcons.Rating4);
		jObject.AddPropertyEnum("IconSet", IconSet, StiFontIconSet.Rating);
		jObject.AddPropertyEnum("IconMode", IconMode, StiIndicatorIconMode.Auto);
		jObject.AddPropertyEnum("IconRangeMode", IconRangeMode, StiIndicatorIconRangeMode.Percentage);
		if (CustomIcon != null)
		{
			jObject.Add("CustomIcon", System.Convert.ToBase64String(CustomIcon));
		}
		jObject.AddPropertyEnum("IconAlignment", IconAlignment, StiIconAlignment.Right);
		jObject.AddPropertyEnum("TargetMode", TargetMode, StiTargetMode.Variation);
		jObject.AddPropertyEnum("FontSizeMode", FontSizeMode, StiFontSizeMode.Auto);
		jObject.AddPropertyJObject("IndicatorConditions", StiJsonReportObjectHelper.Serialize.JObjectCollection(IndicatorConditions, mode));
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
				StiIndicatorDashboardInteraction stiIndicatorDashboardInteraction = StiDashboardInteractionLoader.LoadInteractionFromJsonObject((JObject)item.Value) as StiIndicatorDashboardInteraction;
				DashboardInteraction = stiIndicatorDashboardInteraction ?? null;
				break;
			}
			case "UserSorts":
				UserSorts.AddRange(from p in ((JObject)item.Value).Properties()
					select StiDataSortRule.LoadFromJson((JObject)p.Value));
				break;
			case "TopN":
				TopN = StiDataTopN.CreateFromJsonObject((JObject)item.Value);
				break;
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
			case "TextFormat":
				TextFormat = StiFormatService.CreateFromJsonObject((JObject)item.Value);
				break;
			case "Value":
			{
				StiValueIndicatorMeter stiValueIndicatorMeter = StiMeterLoader.LoadFromJson((JObject)item.Value) as StiValueIndicatorMeter;
				Value = stiValueIndicatorMeter ?? null;
				break;
			}
			case "Target":
			{
				StiTargetIndicatorMeter stiTargetIndicatorMeter = StiMeterLoader.LoadFromJson((JObject)item.Value) as StiTargetIndicatorMeter;
				Target = stiTargetIndicatorMeter ?? null;
				break;
			}
			case "Series":
			{
				StiSeriesIndicatorMeter stiSeriesIndicatorMeter = StiMeterLoader.LoadFromJson((JObject)item.Value) as StiSeriesIndicatorMeter;
				Series = stiSeriesIndicatorMeter ?? null;
				break;
			}
			case "DataMode":
				DataMode = item.DeserializeEnum<StiDataMode>();
				break;
			case "ManuallyEnteredData":
				ManuallyEnteredData = item.DeserializeString();
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
			case "Icon":
				Icon = item.DeserializeEnum<StiFontIcons>();
				break;
			case "IconSet":
				IconSet = item.DeserializeEnum<StiFontIconSet>();
				break;
			case "IconMode":
				IconMode = item.DeserializeEnum<StiIndicatorIconMode>();
				break;
			case "IconRangeMode":
				IconRangeMode = item.DeserializeEnum<StiIndicatorIconRangeMode>();
				break;
			case "CustomIcon":
				CustomIcon = item.DeserializeByteArray();
				break;
			case "ForeColor":
				ForeColor = item.DeserializeColor();
				break;
			case "GlyphColor":
				GlyphColor = item.DeserializeColor();
				break;
			case "Font":
				Font = item.DeserializeFont(Font);
				break;
			case "IconAlignment":
				IconAlignment = item.DeserializeEnum<StiIconAlignment>();
				break;
			case "TargetMode":
				TargetMode = item.DeserializeEnum<StiTargetMode>();
				break;
			case "FontSizeMode":
				FontSizeMode = item.DeserializeEnum<StiFontSizeMode>();
				break;
			case "IndicatorConditions":
				IndicatorConditions.AddRange(from p in ((JObject)item.Value).Properties()
					select StiIndicatorElementCondition.LoadFromJson((JObject)p.Value));
				break;
			case "IconRanges":
				IconRanges.AddRange(from p in ((JObject)item.Value).Properties()
					select StiIndicatorIconRange.LoadFromJson((JObject)p.Value));
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

	public void AddValue(IStiAppDataCell cell)
	{
		Value = ((cell != null) ? StiMeterHelper.Indicator.GetValue(cell) : null);
	}

	public void AddValue(IStiMeter meter)
	{
		Value = StiMeterHelper.Indicator.GetValue(meter as StiMeter);
	}

	public void RemoveValue()
	{
		Value = null;
	}

	public IStiMeter GetValue()
	{
		return Value;
	}

	public IStiMeter GetValue(IStiMeter meter)
	{
		if (meter == null)
		{
			return null;
		}
		return StiMeterHelper.Indicator.GetValue(meter as StiMeter);
	}

	public void CreateNewValue()
	{
		Value = new StiValueIndicatorMeter
		{
			Expression = "Sum()"
		};
	}

	public void AddTarget(IStiAppDataCell cell)
	{
		Target = ((cell != null) ? StiMeterHelper.Indicator.GetTarget(cell) : null);
	}

	public void AddTarget(IStiMeter meter)
	{
		Target = StiMeterHelper.Indicator.GetTarget(meter as StiMeter);
	}

	public void RemoveTarget()
	{
		Target = null;
	}

	public IStiMeter GetTarget()
	{
		return Target;
	}

	public IStiMeter GetTarget(IStiMeter meter)
	{
		if (meter == null)
		{
			return null;
		}
		return StiMeterHelper.Indicator.GetTarget(meter as StiMeter);
	}

	public void CreateNewTarget()
	{
		Target = new StiTargetIndicatorMeter
		{
			Expression = "Sum()"
		};
	}

	public void AddSeries(IStiAppDataCell cell)
	{
		Series = ((cell != null) ? StiMeterHelper.Indicator.GetSeries(cell) : null);
	}

	public void AddSeries(IStiMeter meter)
	{
		Series = StiMeterHelper.Indicator.GetSeries(meter as StiMeter);
	}

	public void RemoveSeries()
	{
		Series = null;
	}

	public IStiMeter GetSeries()
	{
		return Series;
	}

	public IStiMeter GetSeries(IStiMeter meter)
	{
		if (meter == null)
		{
			return null;
		}
		return StiMeterHelper.Indicator.GetSeries(meter as StiMeter);
	}

	public void CreateNewSeries()
	{
		Series = new StiSeriesIndicatorMeter();
	}

	public List<IStiIndicatorIconRange> GetIconRanges()
	{
		List<IStiIndicatorIconRange> list = new List<IStiIndicatorIconRange>();
		list.AddRange(IconRanges);
		return list;
	}

	public IStiIndicatorIconRange AddRange()
	{
		StiIndicatorIconRange stiIndicatorIconRange = new StiIndicatorIconRange();
		IconRanges.Add(stiIndicatorIconRange);
		return stiIndicatorIconRange;
	}

	public void RemoveRange(int index)
	{
		if (index < IconRanges.Count)
		{
			IconRanges.RemoveAt(index);
		}
	}

	public void CreatedDefaultRanges()
	{
		IconRanges.Clear();
		IconRanges.Add(new StiIndicatorIconRange(StiFontIcons.Rating1, "0", "25"));
		IconRanges.Add(new StiIndicatorIconRange(StiFontIcons.Rating2, "25", "50"));
		IconRanges.Add(new StiIndicatorIconRange(StiFontIcons.Rating3, "50", "75"));
		IconRanges.Add(new StiIndicatorIconRange(StiFontIcons.Rating4, "75", "100"));
	}

	public StiDataTable GetManuallyEnteredDataTable()
	{
		if (DataMode != StiDataMode.ManuallyEnteringData)
		{
			return null;
		}
		return StiManuallyEnteredDataParser.GetIndicatorDataTable(ManuallyEnteredData, this);
	}

	public void ConvertFrom(IStiElement element)
	{
		List<IStiMeter> list = element.FetchAllMeters();
		StiMeter stiMeter = list?.FirstOrDefault((IStiMeter m) => m is IStiValueMeter) as StiMeter;
		stiMeter = stiMeter ?? (list?.FirstOrDefault((IStiMeter m) => m is IStiMeasureMeter) as StiMeter);
		if (stiMeter != null)
		{
			Value = Value ?? new StiValueIndicatorMeter();
			Value.LoadFromJsonObject(stiMeter.SaveToJsonObject(StiJsonSaveMode.Report));
		}
		if (list?.FirstOrDefault((IStiMeter m) => m is IStiTargetMeter) is StiMeter stiMeter2)
		{
			Target = Target ?? new StiTargetIndicatorMeter();
			Target.LoadFromJsonObject(stiMeter2.SaveToJsonObject(StiJsonSaveMode.Report));
		}
		StiMeter stiMeter3 = list?.FirstOrDefault((IStiMeter m) => m is IStiSeriesMeter) as StiMeter;
		stiMeter3 = stiMeter3 ?? (list?.FirstOrDefault((IStiMeter m) => m is IStiDimensionMeter) as StiMeter);
		if (stiMeter3 != null)
		{
			Series = Series ?? new StiSeriesIndicatorMeter();
			Series.LoadFromJsonObject(stiMeter3.SaveToJsonObject(StiJsonSaveMode.Report));
		}
		if (DashboardInteraction == null)
		{
			DashboardInteraction = new StiIndicatorDashboardInteraction();
		}
	}

	private bool ShouldSerializeTextFormat()
	{
		if (TextFormat != null)
		{
			return !(TextFormat is StiGeneralFormatService);
		}
		return true;
	}

	private bool ShouldSerializeFont()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Invalid comparison between Unknown and I4
		if (Font != null && Font.Name == "Arial" && Font.SizeInPoints == 13f)
		{
			return (int)Font.Style > 0;
		}
		return true;
	}

	private bool ShouldSerializeForeColor()
	{
		return ForeColor != Color.Transparent;
	}

	public override List<IStiMeter> FetchAllMeters()
	{
		return GetMeters();
	}

	public override List<IStiMeter> GetMeters()
	{
		List<IStiMeter> list = new List<IStiMeter>();
		if (DataMode == StiDataMode.UsingDataFields)
		{
			if (Value != null)
			{
				list.Add(Value);
			}
			if (Target != null)
			{
				list.Add(Target);
			}
			if (Series != null)
			{
				list.Add(Series);
			}
		}
		return list;
	}

	public override List<string> RetrieveUsedDataNames()
	{
		if (DataMode == StiDataMode.UsingDataFields)
		{
			return StiUsedDataHelper.GetMany(Value, Target, Series);
		}
		return null;
	}

	private bool ShouldSerializeTopN()
	{
		if (TopN != null)
		{
			return !TopN.IsDefault;
		}
		return true;
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

	public void AddIndicatorCondition(StiIndicatorFieldCondition field, Stimulsoft.Report.Components.StiFilterCondition condition, string value, StiFontIcons icon, Color iconColor, StiFontIcons targetIcon, Color targetIconColor, byte[] customIcon, StiIconAlignment iconAlignment, StiIconAlignment targetIconAlignment, StiIndicatorConditionPermissions permissions, Font font, Color textColor, Color backColor)
	{
		IndicatorConditions.Add(new StiIndicatorElementCondition(field, condition, value, icon, iconColor, targetIcon, targetIconColor, customIcon, iconAlignment, targetIconAlignment, permissions, font, textColor, backColor));
	}

	public List<IStiIndicatorElementCondition> FetchIndicatorConditions()
	{
		List<IStiIndicatorElementCondition> list = new List<IStiIndicatorElementCondition>();
		if (IndicatorConditions != null)
		{
			list.AddRange(IndicatorConditions);
		}
		return list;
	}

	public void ClearIndicatorConditions()
	{
		IndicatorConditions.Clear();
	}

	public override List<StiFont> GetFonts()
	{
		List<StiFont> fonts = base.GetFonts();
		fonts.Add(new StiFont(Font));
		fonts.Add(new StiFont(Title.Font));
		foreach (object condition in Conditions)
		{
			if (condition is IStiIndicatorElementCondition stiIndicatorElementCondition)
			{
				fonts.Add(new StiFont(stiIndicatorElementCondition.Font));
				if (stiIndicatorElementCondition.IconAlignment != 0 || stiIndicatorElementCondition.TargetIconAlignment != 0)
				{
					fonts.Add(StiFontIconsHelper.GetFont());
				}
			}
		}
		if (CustomIcon == null)
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

	private bool ShouldSerializeGlyphColor()
	{
		return GlyphColor != Color.Transparent;
	}

	private bool ShouldSerializeConditions()
	{
		if (IndicatorConditions != null)
		{
			return IndicatorConditions.Count > 0;
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

	public override StiComponent CreateNew()
	{
		return new StiIndicatorElement();
	}

	public StiIndicatorElement()
		: this(RectangleD.Empty)
	{
	}

	public StiIndicatorElement(RectangleD rect)
		: base(rect)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		Title.Text = LocalizedName;
		DashboardInteraction = new StiIndicatorDashboardInteraction();
		Layout = new StiElementLayout(this);
	}
}
