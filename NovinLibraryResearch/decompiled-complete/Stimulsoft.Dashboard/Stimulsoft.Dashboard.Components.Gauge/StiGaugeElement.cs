using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Context.Animation;
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
using Stimulsoft.Dashboard.Export;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Dashboard.Interactions;
using Stimulsoft.Dashboard.Interactions.Design;
using Stimulsoft.Dashboard.Render;
using Stimulsoft.Dashboard.Visualizers;
using Stimulsoft.Dashboard.Visuals.Gauge;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Dashboard.Components.Gauge;

[StiServiceBitmap(typeof(StiElement), "Stimulsoft.Dashboard.Images.Toolbox.StiGauge.png")]
[StiExportTool("Stimulsoft.Dashboard.Export.Tools.StiGaugeElementExportTool, Stimulsoft.Dashboard.Export, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiPainter("Stimulsoft.Dashboard.Drawing.Painters.StiGaugeElementGdiPainter, Stimulsoft.Dashboard.Drawing, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfPainter("Stimulsoft.Dashboard.Drawing.Wpf.Painters.StiGaugeElementWpfPainter, Stimulsoft.Dashboard.Drawing.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiDesigner("Stimulsoft.Dashboard.Design.Designers.StiGaugeElementDesigner, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Dashboard.WpfDesign.Designers.StiGaugeElementWpfDesigner, Stimulsoft.Dashboard.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiVisualizer("Stimulsoft.Dashboard.Viewer.Visualizers.StiGaugeElementVisualizer, Stimulsoft.Dashboard.Viewer, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfVisualizer("Stimulsoft.Dashboard.Viewer.Wpf.Visualizers.StiGaugeElementWpfVisualizer, Stimulsoft.Dashboard.Viewer.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiContextTool(typeof(IStiComponentDesigner))]
[StiContextTool(typeof(IStiTitleElement))]
public class StiGaugeElement : StiElement, IStiGaugeElement, IStiElement, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiQueryObject, IStiRetrieval, IStiUserSorts, IStiDashboardElementStyle, IStiTransformActions, IStiTransformFilters, IStiTransformSorts, IStiDataTransformationElement, IStiGroupElement, IStiCrossFiltering, IStiDataFilters, IStiConvertibleElement, IStiManuallyEnteredData, IStiSimpleShadow, IStiCornerRadius, IStiTitleElement, IStiFont, IStiForeColor, IStiElementLayout, IStiElementInteraction, IStiGlobalizationProvider
{
	private List<StiDataSortRule> userSorts = new List<StiDataSortRule>();

	private List<StiDataFilterRule> dataFilters = new List<StiDataFilterRule>();

	private StiElementStyleIdent style = StiElementStyleIdent.Auto;

	private string customStyleName = "";

	private StiSimpleShadow shadow = new StiSimpleShadow();

	private StiCornerRadius cornerRadius = new StiCornerRadius(0f);

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiGaugeElement;

	public List<StiAnimation> PreviousAnimations { get; set; }

	[StiSerializable]
	[DefaultValue(StiDataMode.UsingDataFields)]
	[Browsable(false)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiDataMode DataMode { get; set; }

	[StiSerializable]
	[DefaultValue(null)]
	[Browsable(false)]
	public string ManuallyEnteredData { get; set; }

	[StiSerializable]
	[StiCategory("Gauge", StiCategorySource.PropertyCategory)]
	[DefaultValue("")]
	[StiOrder(106)]
	[StiPropertyLevel(StiLevel.Standard)]
	public string Group { get; set; } = "";


	[StiSerializable]
	[StiCategory("Gauge", StiCategorySource.PropertyCategory)]
	[DefaultValue(true)]
	[StiOrder(104)]
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
			if (Value == null && Series == null && Target == null)
			{
				return DataMode == StiDataMode.ManuallyEnteringData;
			}
			return true;
		}
	}

	[StiSerializable]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[Description("The font used to display text in the component.")]
	[StiOrder(180)]
	[Editor("Stimulsoft.Report.Design.Components.StiFontEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Font Font { get; set; } = new Font("Arial", 13f);


	[Editor("Stimulsoft.Report.Components.TextFormats.Design.StiTextFormatGaugeElementEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiSerializable(StiSerializationVisibility.Class)]
	[StiCategory("Behavior", StiCategorySource.PropertyCategory)]
	[StiOrder(495)]
	[Description("Gets or sets the format of the text.")]
	[StiPropertyLevel(StiLevel.Standard)]
	public virtual StiFormatService ValueFormat { get; set; } = GetValueFormatDefault();


	[StiSerializable]
	[TypeConverter(typeof(StiStyleColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionStyleColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[Description("The foreground color of this component, which is used to display text.")]
	[StiOrder(210)]
	[StiExpressionAllowed]
	public Color ForeColor { get; set; } = Color.Transparent;


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


	[Editor("Stimulsoft.Report.Components.Design.StiDataTransformationEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiDataTransformationConverter))]
	[StiCategory("Gauge", StiCategorySource.PropertyCategory)]
	[StiPropertyLevel(StiLevel.Standard)]
	[StiOrder(105)]
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

	public override int ToolboxPosition => 305;

	public override string LocalizedName => StiLocalization.Get("Components", "StiGauge");

	public override string HelpUrl => "user-manual/dashboards_gauge.htm";

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
	[Browsable(false)]
	[StiBrowsable(false)]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Obsolete("ShortValue property is obsolete. Please use ValueFormat property.")]
	public bool ShortValue { get; set; } = true;


	[Description("Gets or sets settings of the gauge labels.")]
	[StiSerializable(StiSerializationVisibility.Class)]
	[StiPropertyLevel(StiLevel.Standard)]
	[StiCategory("Gauge", StiCategorySource.PropertyCategory)]
	[StiOrder(108)]
	public StiGaugeLabels Labels { get; set; } = new StiGaugeLabels();


	[Description("Gets or sets settings of the gauge labels.")]
	[StiSerializable(StiSerializationVisibility.Class)]
	[StiPropertyLevel(StiLevel.Standard)]
	[StiCategory("Gauge", StiCategorySource.PropertyCategory)]
	[StiOrder(108)]
	public StiGaugeTarget TargetSettings { get; set; } = new StiGaugeTarget();


	[StiSerializable(StiSerializationVisibility.Class)]
	[DefaultValue(null)]
	[Browsable(false)]
	[Editor("Stimulsoft.Dashboard.Design.Components.StiElementEditor, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public StiValueGaugeMeter Value { get; set; }

	[StiSerializable(StiSerializationVisibility.Class)]
	[DefaultValue(null)]
	[Browsable(false)]
	public StiSeriesGaugeMeter Series { get; set; }

	[StiSerializable(StiSerializationVisibility.Class)]
	[DefaultValue(null)]
	[Browsable(false)]
	public StiTargetGaugeMeter Target { get; set; }

	[StiSerializable]
	[DefaultValue(0.0)]
	[Browsable(false)]
	public decimal Minimum { get; set; }

	[StiSerializable]
	[DefaultValue(100.0)]
	[Browsable(false)]
	public decimal Maximum { get; set; } = 100m;


	[StiSerializable]
	[DefaultValue(StiGaugeCalculationMode.Auto)]
	[Browsable(false)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiGaugeCalculationMode CalculationMode { get; set; } = StiGaugeCalculationMode.Auto;


	[StiSerializable]
	[DefaultValue(StiGaugeRangeMode.Percentage)]
	[Browsable(false)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiGaugeRangeMode RangeMode { get; set; } = StiGaugeRangeMode.Percentage;


	[StiSerializable]
	[DefaultValue(StiGaugeRangeType.None)]
	[Browsable(false)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiGaugeRangeType RangeType { get; set; }

	[StiSerializable]
	[DefaultValue(StiGaugeType.FullCircular)]
	[Browsable(false)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiGaugeType Type { get; set; }

	[StiSerializable(StiSerializationVisibility.List)]
	[Browsable(false)]
	public virtual List<StiGaugeRange> Ranges { get; set; } = new List<StiGaugeRange>();


	public override object Clone(bool cloneProperties)
	{
		StiGaugeElement stiGaugeElement = ((StiElement)base.Clone(cloneProperties)) as StiGaugeElement;
		stiGaugeElement.Value = Value?.Clone() as StiValueGaugeMeter;
		stiGaugeElement.Series = Series?.Clone() as StiSeriesGaugeMeter;
		stiGaugeElement.Target = Target?.Clone() as StiTargetGaugeMeter;
		stiGaugeElement.UserSorts = UserSorts.Select((StiDataSortRule c) => c.Clone() as StiDataSortRule).ToList();
		stiGaugeElement.TransformActions = TransformActions.Select((StiDataActionRule c) => c.Clone() as StiDataActionRule).ToList();
		stiGaugeElement.TransformFilters = TransformFilters.Select((StiDataFilterRule c) => c.Clone() as StiDataFilterRule).ToList();
		stiGaugeElement.TransformSorts = TransformSorts.Select((StiDataSortRule c) => c.Clone() as StiDataSortRule).ToList();
		stiGaugeElement.DataFilters = DataFilters.Select((StiDataFilterRule c) => c.Clone() as StiDataFilterRule).ToList();
		stiGaugeElement.ValueFormat = ValueFormat?.Clone() as StiFormatService;
		stiGaugeElement.Title = ((StiTitle)Title)?.Clone() as StiTitle;
		stiGaugeElement.DashboardInteraction = DashboardInteraction?.Clone() as StiDashboardInteraction;
		stiGaugeElement.Shadow = (StiSimpleShadow)(Shadow?.Clone());
		stiGaugeElement.CornerRadius = CornerRadius?.Clone() as StiCornerRadius;
		stiGaugeElement.Labels = Labels?.Clone() as StiGaugeLabels;
		stiGaugeElement.TargetSettings = TargetSettings.Clone() as StiGaugeTarget;
		return stiGaugeElement;
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = base.SaveToJsonObject(mode);
		jObject.AddPropertyJObject("UserSorts", StiJsonReportObjectHelper.Serialize.JObjectCollection(UserSorts, mode));
		jObject.AddPropertyJObject("TransformActions", StiJsonReportObjectHelper.Serialize.JObjectCollection(TransformActions, mode));
		jObject.AddPropertyJObject("TransformFilters", StiJsonReportObjectHelper.Serialize.JObjectCollection(TransformFilters, mode));
		jObject.AddPropertyJObject("TransformSorts", StiJsonReportObjectHelper.Serialize.JObjectCollection(TransformSorts, mode));
		jObject.AddPropertyJObject("DataFilters", StiJsonReportObjectHelper.Serialize.JObjectCollection(DataFilters, mode));
		jObject.AddPropertyJObject("Ranges", StiJsonReportObjectHelper.Serialize.JObjectCollection(Ranges, mode));
		jObject.AddPropertyEnum("RangeType", RangeType, StiGaugeRangeType.None);
		jObject.AddPropertyBool("ShortValue", ShortValue, defaultValue: true);
		if (Labels != null)
		{
			jObject.AddPropertyJObject("Labels", Labels.SaveToJsonObject(mode));
		}
		if (TargetSettings != null)
		{
			jObject.AddPropertyJObject("TargetSettings", TargetSettings.SaveToJsonObject(mode));
		}
		jObject.AddPropertyDecimal("Minimum", Minimum, 0m);
		jObject.AddPropertyDecimal("Maximum", Maximum, 100m);
		jObject.AddPropertyEnum("CalculationMode", CalculationMode, StiGaugeCalculationMode.Auto);
		jObject.AddPropertyEnum("RangeMode", RangeMode, StiGaugeRangeMode.Percentage);
		jObject.AddPropertyEnum("Type", Type, StiGaugeType.FullCircular);
		if (DashboardInteraction is StiGaugeDashboardInteraction stiGaugeDashboardInteraction)
		{
			jObject.AddPropertyJObject("DashboardInteraction", stiGaugeDashboardInteraction.SaveToJsonObject(mode));
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
		jObject.AddPropertyFontArial13("Font", Font);
		jObject.AddPropertyEnum("DataMode", DataMode, StiDataMode.UsingDataFields);
		jObject.AddPropertyStringNullOrEmpty("ManuallyEnteredData", ManuallyEnteredData);
		jObject.AddPropertyEnum("Style", Style, StiElementStyleIdent.Auto);
		jObject.AddPropertyStringNullOrEmpty("Group", Group);
		jObject.AddPropertyBool("CrossFiltering", CrossFiltering, defaultValue: true);
		jObject.AddPropertyStringNullOrEmpty("CustomStyleName", CustomStyleName);
		jObject.AddPropertyCornerRadius("CornerRadius", CornerRadius);
		jObject.AddPropertyShadow("Shadow", Shadow);
		if (ValueFormat != null)
		{
			jObject.AddPropertyJObject("ValueFormat", ValueFormat.SaveToJsonObject(mode));
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
				StiGaugeDashboardInteraction stiGaugeDashboardInteraction = StiDashboardInteractionLoader.LoadInteractionFromJsonObject((JObject)item.Value) as StiGaugeDashboardInteraction;
				DashboardInteraction = stiGaugeDashboardInteraction ?? null;
				break;
			}
			case "Labels":
				Labels.LoadFromJsonObject((JObject)item.Value);
				break;
			case "TargetSettings":
				TargetSettings.LoadFromJsonObject((JObject)item.Value);
				break;
			case "UserSorts":
				UserSorts.AddRange(from p in ((JObject)item.Value).Properties()
					select StiDataSortRule.LoadFromJson((JObject)p.Value));
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
			case "Ranges":
				Ranges.AddRange(from p in ((JObject)item.Value).Properties()
					select StiGaugeRange.LoadFromJson((JObject)p.Value));
				break;
			case "RangeType":
				RangeType = item.DeserializeEnum<StiGaugeRangeType>();
				break;
			case "ShortValue":
				ShortValue = item.DeserializeBool();
				break;
			case "Title":
				Title = StiTitle.CreateFromJsonObject((JObject)item.Value);
				break;
			case "Layout":
				Layout = StiElementLayout.CreateFromJsonObject((JObject)item.Value, this);
				break;
			case "Value":
			{
				StiValueGaugeMeter stiValueGaugeMeter = StiMeterLoader.LoadFromJson((JObject)item.Value) as StiValueGaugeMeter;
				Value = stiValueGaugeMeter ?? null;
				break;
			}
			case "Target":
			{
				StiTargetGaugeMeter stiTargetGaugeMeter = StiMeterLoader.LoadFromJson((JObject)item.Value) as StiTargetGaugeMeter;
				Target = stiTargetGaugeMeter ?? null;
				break;
			}
			case "Minimum":
				Minimum = item.DeserializeDecimal();
				break;
			case "Maximum":
				Maximum = item.DeserializeDecimal();
				break;
			case "CalculationMode":
				CalculationMode = item.DeserializeEnum<StiGaugeCalculationMode>();
				break;
			case "RangeMode":
				RangeMode = item.DeserializeEnum<StiGaugeRangeMode>();
				break;
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
			case "Type":
				Type = item.DeserializeEnum<StiGaugeType>();
				break;
			case "Series":
			{
				StiSeriesGaugeMeter stiSeriesGaugeMeter = StiMeterLoader.LoadFromJson((JObject)item.Value) as StiSeriesGaugeMeter;
				Series = stiSeriesGaugeMeter ?? null;
				break;
			}
			case "ForeColor":
				ForeColor = item.DeserializeColor();
				break;
			case "Font":
				Font = item.DeserializeFont(Font);
				break;
			case "ValueFormat":
				ValueFormat = StiFormatService.CreateFromJsonObject((JObject)item.Value);
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
		Value = ((cell != null) ? StiMeterHelper.Gauge.GetValue(cell) : null);
	}

	public void AddValue(IStiMeter meter)
	{
		Value = StiMeterHelper.Gauge.GetValue(meter as StiMeter);
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
		return StiMeterHelper.Gauge.GetValue(meter as StiMeter);
	}

	public void CreateNewValue()
	{
		Value = new StiValueGaugeMeter
		{
			Expression = "Sum()"
		};
	}

	public void AddSeries(IStiAppDataCell cell)
	{
		Series = ((cell != null) ? StiMeterHelper.Gauge.GetSeries(cell) : null);
	}

	public void AddSeries(IStiMeter meter)
	{
		Series = StiMeterHelper.Gauge.GetSeries(meter as StiMeter);
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
		return StiMeterHelper.Gauge.GetSeries(meter as StiMeter);
	}

	public void CreateNewSeries()
	{
		Series = new StiSeriesGaugeMeter();
	}

	public void AddTarget(IStiAppDataCell cell)
	{
		Target = ((cell != null) ? StiMeterHelper.Gauge.GetTarget(cell) : null);
	}

	public void AddTarget(IStiMeter meter)
	{
		Target = StiMeterHelper.Gauge.GetTarget(meter as StiMeter);
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
		return StiMeterHelper.Gauge.GetValue(meter as StiMeter);
	}

	public void CreateNewTarget()
	{
		Target = new StiTargetGaugeMeter
		{
			Expression = "Sum()"
		};
	}

	public List<IStiGaugeRange> GetRanges()
	{
		List<IStiGaugeRange> list = new List<IStiGaugeRange>();
		list.AddRange(Ranges);
		return list;
	}

	private Color GetPreferedColor()
	{
		if (Ranges.All((StiGaugeRange r) => r.Color != Color.Green))
		{
			return Color.Green;
		}
		if (Ranges.All((StiGaugeRange r) => r.Color != Color.Yellow))
		{
			return Color.Yellow;
		}
		if (Ranges.All((StiGaugeRange r) => r.Color != Color.Red))
		{
			return Color.Red;
		}
		return Color.Orange;
	}

	public IStiGaugeRange AddRange()
	{
		StiGaugeRange stiGaugeRange = new StiGaugeRange
		{
			Color = GetPreferedColor()
		};
		Ranges.Add(stiGaugeRange);
		return stiGaugeRange;
	}

	public void RemoveRange(int index)
	{
		if (index < Ranges.Count)
		{
			Ranges.RemoveAt(index);
		}
	}

	public void CreatedDefaultRanges()
	{
		Ranges.Clear();
		Ranges.Add(new StiGaugeRange(Color.Red, "0", "33"));
		Ranges.Add(new StiGaugeRange(Color.Yellow, "33", "66"));
		Ranges.Add(new StiGaugeRange(Color.Green, "66", "100"));
	}

	public StiGauge GetGaugeComponent(SizeD? size = null)
	{
		StiDataTable orCreate = StiElementDataCache.GetOrCreate(this);
		IEnumerable<StiGaugeIteration> source = new StiGaugeElementBuilder().Render(this, size.HasValue ? size.Value : ClientRectangle.Size, orCreate);
		StiGauge stiGauge = new StiGauge();
		if (source.Count() == 1)
		{
			stiGauge = source.FirstOrDefault().Gauge;
			stiGauge.Report = Report;
			stiGauge.Page = Page;
			stiGauge.Name = Name + "_Content";
			stiGauge.ClientRectangle = (size.HasValue ? new RectangleD(new PointD(0.0, 0.0), size.Value) : ClientRectangle);
			stiGauge.Brush = new StiSolidBrush(BackColor);
		}
		return stiGauge;
	}

	public StiDataTable GetManuallyEnteredDataTable()
	{
		if (DataMode != StiDataMode.ManuallyEnteringData)
		{
			return null;
		}
		return StiManuallyEnteredDataParser.GetGaugeDataTable(ManuallyEnteredData, this);
	}

	public void ConvertFrom(IStiElement element)
	{
		List<IStiMeter> list = element.FetchAllMeters();
		StiMeter stiMeter = list?.FirstOrDefault((IStiMeter m) => m is IStiValueMeter) as StiMeter;
		stiMeter = stiMeter ?? (list?.FirstOrDefault((IStiMeter m) => m is IStiMeasureMeter) as StiMeter);
		if (stiMeter != null)
		{
			Value = Value ?? new StiValueGaugeMeter();
			Value.LoadFromJsonObject(stiMeter.SaveToJsonObject(StiJsonSaveMode.Report));
		}
		StiMeter stiMeter2 = list?.FirstOrDefault((IStiMeter m) => m is IStiSeriesMeter) as StiMeter;
		stiMeter2 = stiMeter2 ?? (list?.FirstOrDefault((IStiMeter m) => m is IStiDimensionMeter) as StiMeter);
		if (stiMeter2 != null)
		{
			Series = Series ?? new StiSeriesGaugeMeter();
			Series.LoadFromJsonObject(stiMeter2.SaveToJsonObject(StiJsonSaveMode.Report));
		}
		if (list?.FirstOrDefault((IStiMeter m) => m is IStiTargetMeter) is StiMeter stiMeter3)
		{
			Target = Target ?? new StiTargetGaugeMeter();
			Target.LoadFromJsonObject(stiMeter3.SaveToJsonObject(StiJsonSaveMode.Report));
		}
		if (DashboardInteraction == null)
		{
			DashboardInteraction = new StiGaugeDashboardInteraction();
		}
	}

	public override List<IStiMeter> FetchAllMeters()
	{
		List<IStiMeter> list = new List<IStiMeter>();
		if (DataMode == StiDataMode.UsingDataFields)
		{
			if (Value != null)
			{
				list.Add(Value);
			}
			if (Series != null)
			{
				list.Add(Series);
			}
			if (Target != null)
			{
				list.Add(Target);
			}
		}
		return list;
	}

	public override List<IStiMeter> GetMeters()
	{
		List<IStiMeter> list = new List<IStiMeter>();
		if (DataMode == StiDataMode.UsingDataFields)
		{
			if (Value != null)
			{
				Value.Key = StiKeyHelper.GetOrGeneratedKey(Value.Key);
				list.Add(Value);
				if (CalculationMode == StiGaugeCalculationMode.Auto)
				{
					list.Add(new StiMinGaugeMeter
					{
						Key = Value.Key,
						Expression = StiExpressionHelper.ReplaceFunction(Value.Expression, "Min")
					});
					list.Add(new StiMaxGaugeMeter
					{
						Key = Value.Key,
						Expression = StiExpressionHelper.ReplaceFunction(Value.Expression, "Max")
					});
				}
			}
			if (Series != null)
			{
				list.Add(Series);
			}
			if (Target != null)
			{
				list.Add(Target);
			}
		}
		return list;
	}

	public override List<string> RetrieveUsedDataNames()
	{
		if (DataMode == StiDataMode.UsingDataFields)
		{
			return StiUsedDataHelper.GetMany(Value, Series, Target);
		}
		return null;
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

	private static StiFormatService GetValueFormatDefault()
	{
		return new StiNumberFormatService
		{
			DecimalDigits = 1,
			State = (Stimulsoft.Report.Components.StiTextFormatState.DecimalDigits | Stimulsoft.Report.Components.StiTextFormatState.Abbreviation)
		};
	}

	private bool ShouldSerializeValueFormat()
	{
		if (ValueFormat != null)
		{
			return !ValueFormat.Equals(GetValueFormatDefault());
		}
		return true;
	}

	private bool ShouldSerializeForeColor()
	{
		return ForeColor != Color.Transparent;
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

	public override List<StiFont> GetFonts()
	{
		List<StiFont> fonts = base.GetFonts();
		fonts.Add(new StiFont(Font));
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

	private bool ShouldSerializeLabels()
	{
		if (Labels != null)
		{
			return !Labels.IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeTargetSettings()
	{
		if (TargetSettings != null)
		{
			return !TargetSettings.IsDefault;
		}
		return true;
	}

	public override StiComponent CreateNew()
	{
		return new StiGaugeElement();
	}

	public StiGaugeElement()
		: this(RectangleD.Empty)
	{
	}

	public StiGaugeElement(RectangleD rect)
		: base(rect)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		Title.Text = LocalizedName;
		DashboardInteraction = new StiGaugeDashboardInteraction();
		Layout = new StiElementLayout(this);
		PreviousAnimations = new List<StiAnimation>();
	}
}
