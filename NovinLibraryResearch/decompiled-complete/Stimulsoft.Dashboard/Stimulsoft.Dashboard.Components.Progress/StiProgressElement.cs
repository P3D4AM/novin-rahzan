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
using Stimulsoft.Dashboard.Components.Chart.Design;
using Stimulsoft.Dashboard.Components.Design;
using Stimulsoft.Dashboard.Components.Helpers;
using Stimulsoft.Dashboard.Components.Progress.Design;
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
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Dashboard.Components.Progress;

[StiServiceBitmap(typeof(StiElement), "Stimulsoft.Dashboard.Images.Toolbox.StiProgress.png")]
[StiGdiPainter("Stimulsoft.Dashboard.Drawing.Painters.StiProgressElementGdiPainter, Stimulsoft.Dashboard.Drawing, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfPainter("Stimulsoft.Dashboard.Drawing.Wpf.Painters.StiProgressElementWpfPainter, Stimulsoft.Dashboard.Drawing.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiDesigner("Stimulsoft.Dashboard.Design.Designers.StiProgressElementDesigner, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Dashboard.WpfDesign.Designers.StiProgressElementWpfDesigner, Stimulsoft.Dashboard.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiVisualizer("Stimulsoft.Dashboard.Viewer.Visualizers.StiProgressElementVisualizer, Stimulsoft.Dashboard.Viewer, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfVisualizer("Stimulsoft.Dashboard.Viewer.Wpf.Visualizers.StiProgressElementWpfVisualizer, Stimulsoft.Dashboard.Viewer.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiContextTool(typeof(IStiComponentDesigner))]
[StiContextTool(typeof(IStiTitleElement))]
public class StiProgressElement : StiElement, IStiTextFormat, IStiProgressElement, IStiElement, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiQueryObject, IStiRetrieval, IStiUserSorts, IStiDashboardElementStyle, IStiTransformActions, IStiTransformFilters, IStiTransformSorts, IStiDataTopN, IStiDataTransformationElement, IStiGroupElement, IStiCrossFiltering, IStiDataFilters, IStiConvertibleElement, IStiManuallyEnteredData, IStiSimpleShadow, IStiCornerRadius, IStiTitleElement, IStiFont, IStiForeColor, IStiElementLayout, IStiElementInteraction, IStiGlobalizationProvider, IStiSeriesColors, IStiAllowSortByVariation
{
	private Font font = new Font("Arial", 13f);

	private Color foreColor = Color.Transparent;

	private List<StiDataSortRule> userSorts = new List<StiDataSortRule>();

	private List<StiDataFilterRule> dataFilters = new List<StiDataFilterRule>();

	private StiElementStyleIdent style = StiElementStyleIdent.Auto;

	private string customStyleName = "";

	private StiSimpleShadow shadow = new StiSimpleShadow();

	private StiCornerRadius cornerRadius = new StiCornerRadius(0f);

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiProgressElement;

	[StiSerializable]
	[StiCategory("Progress", StiCategorySource.PropertyCategory)]
	[DefaultValue("")]
	[StiOrder(104)]
	[StiPropertyLevel(StiLevel.Standard)]
	public string Group { get; set; } = "";


	[StiSerializable]
	[StiCategory("Progress", StiCategorySource.PropertyCategory)]
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
			if (Value == null && Target == null && Series == null)
			{
				return DataMode == StiDataMode.ManuallyEnteringData;
			}
			return true;
		}
	}

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
	[StiCategory("Progress", StiCategorySource.PropertyCategory)]
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

	[StiSerializable(StiSerializationVisibility.Class)]
	[TypeConverter(typeof(StiDashboardInteractionConverter))]
	[Browsable(false)]
	public virtual IStiDashboardInteraction DashboardInteraction { get; set; }

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

	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[StiSerializable]
	[Description("Gets or sets a list of colors which will be used for drawing series.")]
	[TypeConverter(typeof(StiColorsConverter))]
	[StiOrder(270)]
	[Editor("Stimulsoft.Report.Design.Components.StiElementColorsCollectionEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color[] SeriesColors { get; set; } = new Color[0];


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

	public override int ToolboxPosition => 307;

	public override string LocalizedName => StiLocalization.Get("Components", "StiProgress");

	[Browsable(false)]
	public override RectangleD DefaultClientRectangle => new RectangleD(0.0, 0.0, 160.0, 160.0);

	public override string HelpUrl => "user-manual/dashboards_progress.htm";

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

	[StiSerializable(StiSerializationVisibility.List)]
	[TypeConverter(typeof(StiProgressConditionConverter))]
	[Browsable(false)]
	public List<StiProgressElementCondition> ProgressConditions { get; set; } = new List<StiProgressElementCondition>();


	[RefreshProperties(RefreshProperties.All)]
	[Editor("Stimulsoft.Dashboard.Design.Components.StiProgressConditionsEditor, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[StiPropertyLevel(StiLevel.Standard)]
	[StiOrder(140)]
	[StiSerializable(StiSerializationVisibility.List)]
	[TypeConverter(typeof(StiProgressConditionConverter))]
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
	[DefaultValue(true)]
	[StiCategory("Progress", StiCategorySource.PropertyCategory)]
	[StiOrder(108)]
	[TypeConverter(typeof(StiBoolConverter))]
	public bool ColorEach { get; set; } = true;


	[StiSerializable(StiSerializationVisibility.Class)]
	[DefaultValue(null)]
	[Browsable(false)]
	public StiValueProgressMeter Value { get; set; }

	[StiSerializable(StiSerializationVisibility.Class)]
	[DefaultValue(null)]
	[Browsable(false)]
	public StiTargetProgressMeter Target { get; set; }

	[StiSerializable(StiSerializationVisibility.Class)]
	[DefaultValue(null)]
	[Browsable(false)]
	public StiSeriesProgressMeter Series { get; set; }

	[DefaultValue(StiProgressElementMode.Circle)]
	[StiSerializable]
	[Browsable(false)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiProgressElementMode Mode { get; set; } = StiProgressElementMode.Circle;


	public override object Clone(bool cloneProperties)
	{
		StiProgressElement stiProgressElement = ((StiElement)base.Clone(cloneProperties)) as StiProgressElement;
		stiProgressElement.Value = Value?.Clone() as StiValueProgressMeter;
		stiProgressElement.Target = Target?.Clone() as StiTargetProgressMeter;
		stiProgressElement.Series = Series?.Clone() as StiSeriesProgressMeter;
		stiProgressElement.UserSorts = UserSorts.Select((StiDataSortRule c) => c.Clone() as StiDataSortRule).ToList();
		stiProgressElement.TransformActions = TransformActions.Select((StiDataActionRule c) => c.Clone() as StiDataActionRule).ToList();
		stiProgressElement.TransformFilters = TransformFilters.Select((StiDataFilterRule c) => c.Clone() as StiDataFilterRule).ToList();
		stiProgressElement.TransformSorts = TransformSorts.Select((StiDataSortRule c) => c.Clone() as StiDataSortRule).ToList();
		stiProgressElement.DataFilters = DataFilters.Select((StiDataFilterRule c) => c.Clone() as StiDataFilterRule).ToList();
		stiProgressElement.TopN = TopN.Clone() as StiDataTopN;
		stiProgressElement.ProgressConditions = ProgressConditions.Select((StiProgressElementCondition c) => c.Clone() as StiProgressElementCondition).ToList();
		stiProgressElement.DashboardInteraction = DashboardInteraction?.Clone() as StiDashboardInteraction;
		stiProgressElement.TextFormat = TextFormat.Clone() as StiFormatService;
		stiProgressElement.Title = ((StiTitle)Title)?.Clone() as StiTitle;
		stiProgressElement.SeriesColors = SeriesColors?.Clone() as Color[];
		stiProgressElement.Shadow = (StiSimpleShadow)(Shadow?.Clone());
		stiProgressElement.CornerRadius = CornerRadius?.Clone() as StiCornerRadius;
		return stiProgressElement;
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = base.SaveToJsonObject(mode);
		jObject.AddPropertyJObject("UserSorts", StiJsonReportObjectHelper.Serialize.JObjectCollection(UserSorts, mode));
		jObject.AddPropertyJObject("TransformActions", StiJsonReportObjectHelper.Serialize.JObjectCollection(TransformActions, mode));
		jObject.AddPropertyJObject("TransformFilters", StiJsonReportObjectHelper.Serialize.JObjectCollection(TransformFilters, mode));
		jObject.AddPropertyJObject("TransformSorts", StiJsonReportObjectHelper.Serialize.JObjectCollection(TransformSorts, mode));
		jObject.AddPropertyJObject("DataFilters", StiJsonReportObjectHelper.Serialize.JObjectCollection(DataFilters, mode));
		if (DashboardInteraction is StiProgressDashboardInteraction stiProgressDashboardInteraction)
		{
			jObject.AddPropertyJObject("DashboardInteraction", stiProgressDashboardInteraction.SaveToJsonObject(mode));
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
		jObject.AddPropertyFontArial8("Font", Font);
		jObject.AddPropertyArrayColor("SeriesColors", SeriesColors);
		jObject.AddPropertyEnum("Mode", Mode, StiProgressElementMode.Circle);
		jObject.AddPropertyEnum("DataMode", DataMode, StiDataMode.UsingDataFields);
		jObject.AddPropertyStringNullOrEmpty("ManuallyEnteredData", ManuallyEnteredData);
		jObject.AddPropertyEnum("Style", Style, StiElementStyleIdent.Auto);
		jObject.AddPropertyStringNullOrEmpty("CustomStyleName", CustomStyleName);
		jObject.AddPropertyStringNullOrEmpty("Group", Group);
		jObject.AddPropertyBool("CrossFiltering", CrossFiltering, defaultValue: true);
		jObject.AddPropertyBool("ColorEach", ColorEach);
		jObject.AddPropertyJObject("ProgressConditions", StiJsonReportObjectHelper.Serialize.JObjectCollection(ProgressConditions, mode));
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
				StiProgressDashboardInteraction stiProgressDashboardInteraction = StiDashboardInteractionLoader.LoadInteractionFromJsonObject((JObject)item.Value) as StiProgressDashboardInteraction;
				DashboardInteraction = stiProgressDashboardInteraction ?? null;
				break;
			}
			case "UserSorts":
				UserSorts.AddRange(from p in ((JObject)item.Value).Properties()
					select StiDataSortRule.LoadFromJson((JObject)p.Value));
				break;
			case "ColorEach":
				ColorEach = item.DeserializeBool();
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
			case "Layout":
				Layout = StiElementLayout.CreateFromJsonObject((JObject)item.Value, this);
				break;
			case "TextFormat":
				TextFormat = StiFormatService.CreateFromJsonObject((JObject)item.Value);
				break;
			case "Value":
			{
				StiValueProgressMeter stiValueProgressMeter = StiMeterLoader.LoadFromJson((JObject)item.Value) as StiValueProgressMeter;
				Value = stiValueProgressMeter ?? null;
				break;
			}
			case "Target":
			{
				StiTargetProgressMeter stiTargetProgressMeter = StiMeterLoader.LoadFromJson((JObject)item.Value) as StiTargetProgressMeter;
				Target = stiTargetProgressMeter ?? null;
				break;
			}
			case "Series":
			{
				StiSeriesProgressMeter stiSeriesProgressMeter = StiMeterLoader.LoadFromJson((JObject)item.Value) as StiSeriesProgressMeter;
				Series = stiSeriesProgressMeter ?? null;
				break;
			}
			case "Mode":
				Mode = item.DeserializeEnum<StiProgressElementMode>();
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
			case "ForeColor":
				ForeColor = item.DeserializeColor();
				break;
			case "Font":
				Font = item.DeserializeFont(Font);
				break;
			case "SeriesColors":
				SeriesColors = item.DeserializeArrayColor();
				break;
			case "ProgressConditions":
				ProgressConditions.AddRange(from p in ((JObject)item.Value).Properties()
					select StiProgressElementCondition.LoadFromJson((JObject)p.Value));
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
		Value = ((cell != null) ? StiMeterHelper.Progress.GetValue(cell) : null);
	}

	public void AddValue(IStiMeter meter)
	{
		Value = StiMeterHelper.Progress.GetValue(meter as StiMeter);
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
		return StiMeterHelper.Progress.GetValue(meter as StiMeter);
	}

	public void CreateNewValue()
	{
		Value = new StiValueProgressMeter
		{
			Expression = "Sum()"
		};
	}

	public void AddTarget(IStiAppDataCell cell)
	{
		Target = ((cell != null) ? StiMeterHelper.Progress.GetTarget(cell) : null);
	}

	public void AddTarget(IStiMeter meter)
	{
		Target = StiMeterHelper.Progress.GetTarget(meter as StiMeter);
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
		return StiMeterHelper.Progress.GetTarget(meter as StiMeter);
	}

	public void CreateNewTarget()
	{
		Target = new StiTargetProgressMeter
		{
			Expression = "Sum()"
		};
	}

	public void AddSeries(IStiAppDataCell cell)
	{
		Series = ((cell != null) ? StiMeterHelper.Progress.GetSeries(cell) : null);
	}

	public void AddSeries(IStiMeter meter)
	{
		Series = StiMeterHelper.Progress.GetSeries(meter as StiMeter);
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
		return StiMeterHelper.Progress.GetSeries(meter as StiMeter);
	}

	public void CreateNewSeries()
	{
		Series = new StiSeriesProgressMeter();
	}

	public StiDataTable GetManuallyEnteredDataTable()
	{
		if (DataMode != StiDataMode.ManuallyEnteringData)
		{
			return null;
		}
		return StiManuallyEnteredDataParser.GetProgressDataTable(ManuallyEnteredData, this);
	}

	public void ConvertFrom(IStiElement element)
	{
		List<IStiMeter> list = element.FetchAllMeters();
		StiMeter stiMeter = list?.FirstOrDefault((IStiMeter m) => m is IStiValueMeter) as StiMeter;
		stiMeter = stiMeter ?? (list?.FirstOrDefault((IStiMeter m) => m is IStiMeasureMeter) as StiMeter);
		if (stiMeter != null)
		{
			Value = Value ?? new StiValueProgressMeter();
			Value.LoadFromJsonObject(stiMeter.SaveToJsonObject(StiJsonSaveMode.Report));
		}
		if (list?.FirstOrDefault((IStiMeter m) => m is IStiTargetMeter) is StiMeter stiMeter2)
		{
			Target = Target ?? new StiTargetProgressMeter();
			Target.LoadFromJsonObject(stiMeter2.SaveToJsonObject(StiJsonSaveMode.Report));
		}
		StiMeter stiMeter3 = list?.FirstOrDefault((IStiMeter m) => m is IStiSeriesMeter) as StiMeter;
		stiMeter3 = stiMeter3 ?? (list?.FirstOrDefault((IStiMeter m) => m is IStiDimensionMeter) as StiMeter);
		if (stiMeter3 != null)
		{
			Series = Series ?? new StiSeriesProgressMeter();
			Series.LoadFromJsonObject(stiMeter3.SaveToJsonObject(StiJsonSaveMode.Report));
		}
		if (DashboardInteraction == null)
		{
			DashboardInteraction = new StiProgressDashboardInteraction();
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

	private bool ShouldSerializeDashboardInteraction()
	{
		if (DashboardInteraction != null)
		{
			return !DashboardInteraction.IsDefault;
		}
		return true;
	}

	public void AddProgressCondition(StiProgressFieldCondition field, Stimulsoft.Report.Components.StiFilterCondition condition, string value, StiProgressConditionPermissions permissions, Font font, Color textColor, Color color, Color trackColor)
	{
		ProgressConditions.Add(new StiProgressElementCondition(field, condition, value, permissions, font, textColor, color, trackColor));
	}

	public List<IStiProgressElementCondition> FetchProgressConditions()
	{
		List<IStiProgressElementCondition> list = new List<IStiProgressElementCondition>();
		if (ProgressConditions != null)
		{
			list.AddRange(ProgressConditions);
		}
		return list;
	}

	public void ClearProgressConditions()
	{
		ProgressConditions.Clear();
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

	private bool ShouldSerializeSeriesColors()
	{
		if (SeriesColors != null)
		{
			return SeriesColors.Length != 0;
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

	private bool ShouldSerializeConditions()
	{
		if (ProgressConditions != null)
		{
			return ProgressConditions.Count > 0;
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

	public override StiComponent CreateNew()
	{
		return new StiProgressElement();
	}

	public StiProgressElement()
		: this(RectangleD.Empty)
	{
	}

	public StiProgressElement(RectangleD rect)
		: base(rect)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		Title.Text = LocalizedName;
		DashboardInteraction = new StiProgressDashboardInteraction();
		Layout = new StiElementLayout(this);
	}
}
