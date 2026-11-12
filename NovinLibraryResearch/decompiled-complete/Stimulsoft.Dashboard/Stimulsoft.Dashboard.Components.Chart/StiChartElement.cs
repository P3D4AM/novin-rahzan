using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Reflection;
using Stimulsoft.Base;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Dashboard.Components.Chart.Design;
using Stimulsoft.Dashboard.Components.Design;
using Stimulsoft.Dashboard.Components.Helpers;
using Stimulsoft.Dashboard.Export;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Dashboard.Interactions;
using Stimulsoft.Dashboard.Interactions.Design;
using Stimulsoft.Dashboard.Visualizers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Dashboard.Components.Chart;

[StiServiceBitmap(typeof(StiElement), "Stimulsoft.Dashboard.Images.Toolbox.StiChart.png")]
[StiExportTool("Stimulsoft.Dashboard.Export.Tools.StiChartElementExportTool, Stimulsoft.Dashboard.Export, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiPainter("Stimulsoft.Dashboard.Drawing.Painters.StiChartElementGdiPainter, Stimulsoft.Dashboard.Drawing, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfPainter("Stimulsoft.Dashboard.Drawing.Wpf.Painters.StiChartElementWpfPainter, Stimulsoft.Dashboard.Drawing.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiDesigner("Stimulsoft.Dashboard.Design.Designers.StiChartElementDesigner, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Dashboard.WpfDesign.Designers.StiChartElementWpfDesigner, Stimulsoft.Dashboard.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiVisualizer("Stimulsoft.Dashboard.Viewer.Visualizers.StiChartElementVisualizer, Stimulsoft.Dashboard.Viewer, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfVisualizer("Stimulsoft.Dashboard.Viewer.Wpf.Visualizers.StiChartElementWpfVisualizer, Stimulsoft.Dashboard.Viewer.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiContextTool(typeof(IStiComponentDesigner))]
[StiContextTool(typeof(IStiTitleElement))]
public class StiChartElement : StiElement, IStiChartElement, IStiElement, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiQueryObject, IStiRetrieval, IStiUserFilters, IStiUserSorts, IStiDashboardElementStyle, IStiTransformActions, IStiTransformFilters, IStiTransformSorts, IStiDataTopN, IStiDataTransformationElement, IStiGroupElement, IStiCrossFiltering, IStiDataFilters, IStiConvertibleElement, IStiManuallyEnteredData, IStiSimpleShadow, IStiCornerRadius, IStiUserViewStates, IStiSkipOwnFilter, IStiTitleElement, IStiElementLayout, IStiGlobalizationProvider, IStiSeriesColors, IStiNegativeSeriesColors, IStiParetoSeriesColors, IStiElementInteraction, IStiDrillDownElement
{
	private StiElementStyleIdent style = StiElementStyleIdent.Auto;

	private string customStyleName = "";

	private List<StiDataFilterRule> userFilters = new List<StiDataFilterRule>();

	private List<StiDataSortRule> userSorts = new List<StiDataSortRule>();

	private List<StiDataFilterRule> dataFilters = new List<StiDataFilterRule>();

	private Color[] seriesColors = new Color[0];

	private StiSimpleShadow shadow = new StiSimpleShadow();

	private StiCornerRadius cornerRadius = new StiCornerRadius(0f);

	private StiChartLegend legend;

	private StiChartLabels labels;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiChartElement;

	[Browsable(false)]
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

	[Browsable(false)]
	public override bool IsDefined
	{
		get
		{
			if ((Values == null || Values.Count <= 0) && (Weights == null || Weights.Count <= 0 || !IsBubbleChart) && (Arguments == null || Arguments.Count <= 0) && Series == null)
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

	[StiSerializable]
	[StiCategory("Chart", StiCategorySource.PropertyCategory)]
	[DefaultValue("")]
	[StiOrder(250)]
	[StiPropertyLevel(StiLevel.Standard)]
	public string Group { get; set; } = "";


	[StiSerializable]
	[StiCategory("Chart", StiCategorySource.PropertyCategory)]
	[DefaultValue(true)]
	[StiOrder(220)]
	[StiPropertyLevel(StiLevel.Standard)]
	[TypeConverter(typeof(StiBoolConverter))]
	public bool CrossFiltering { get; set; } = true;


	[Browsable(false)]
	bool IStiSkipOwnFilter.AllowSkipOwnFilter => true;

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
	public List<StiUserViewState> UserViewStates { get; set; } = new List<StiUserViewState>();


	[StiSerializable]
	[Browsable(false)]
	public string SelectedViewStateKey { get; set; }

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
	public virtual List<List<StiDataFilterRule>> DrillDownFiltersList { get; set; } = new List<List<StiDataFilterRule>>();


	[Browsable(false)]
	public virtual List<StiDataFilterRule> DrillDownFilters { get; set; } = new List<StiDataFilterRule>();


	[Browsable(false)]
	public int DrillDownCurrentLevel { get; set; }

	[Browsable(false)]
	public int DrillDownLevelCount
	{
		get
		{
			if (Arguments == null)
			{
				return 0;
			}
			return Arguments.Count;
		}
	}

	[Browsable(false)]
	[StiSerializable(StiSerializationVisibility.Class)]
	public StiDataTopN TopN { get; set; } = new StiDataTopN();


	[Editor("Stimulsoft.Report.Components.Design.StiDataTransformationEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiDataTransformationConverter))]
	[StiCategory("Chart", StiCategorySource.PropertyCategory)]
	[StiPropertyLevel(StiLevel.Standard)]
	[StiOrder(240)]
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

	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[StiSerializable]
	[Description("Gets or sets a list of colors which will be used for drawing series.")]
	[TypeConverter(typeof(StiColorsConverter))]
	[StiOrder(270)]
	[Editor("Stimulsoft.Report.Design.Components.StiElementColorsCollectionEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color[] SeriesColors
	{
		get
		{
			return seriesColors;
		}
		set
		{
			if (seriesColors != value)
			{
				seriesColors = value;
				InvokeRefreshControl("SeriesColors");
			}
		}
	}

	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[StiSerializable]
	[Description("Gets or sets a list of colors which will be used for drawing series with negative values.")]
	[TypeConverter(typeof(StiColorsConverter))]
	[StiOrder(260)]
	[Editor("Stimulsoft.Report.Design.Components.StiElementNegativeColorsCollectionEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color[] NegativeSeriesColors { get; set; } = new Color[0];


	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[StiSerializable]
	[Description("Gets or sets a list of colors which will be used for drawing lines pareto series.")]
	[TypeConverter(typeof(StiColorsConverter))]
	[StiOrder(272)]
	[Editor("Stimulsoft.Report.Design.Components.StiElementParetoColorsCollectionEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color[] ParetoSeriesColors { get; set; } = new Color[0];


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

	public override int ToolboxPosition => 304;

	public override string LocalizedName => StiLocalization.Get("Components", "StiChart");

	public override string HelpUrl => "user-manual/dashboards_chart.htm";

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
	[Browsable(false)]
	public StiValueChartMeter ManuallyEnteredChartMeter { get; set; } = new StiValueChartMeter();


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

	[DefaultValue(false)]
	[StiCategory("Chart", StiCategorySource.PropertyCategory)]
	[StiOrder(210)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiNonSerialized]
	[Browsable(false)]
	[Obsolete("'ColorEach' property of the chart element is obsolete! Please use 'Area.ColorEach' property instead.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ColorEach
	{
		get
		{
			if (Area != null)
			{
				return Area.ColorEach;
			}
			return false;
		}
		set
		{
			if (Area != null)
			{
				Area.ColorEach = value;
			}
		}
	}

	[StiSerializable(StiSerializationVisibility.List)]
	[TypeConverter(typeof(StiMeterListConverter))]
	[Browsable(false)]
	public List<StiValueChartMeter> Values { get; set; } = new List<StiValueChartMeter>();


	[StiSerializable(StiSerializationVisibility.List)]
	[TypeConverter(typeof(StiMeterListConverter))]
	[Browsable(false)]
	public List<StiEndValueChartMeter> EndValues { get; set; } = new List<StiEndValueChartMeter>();


	[StiSerializable(StiSerializationVisibility.List)]
	[TypeConverter(typeof(StiMeterListConverter))]
	[Browsable(false)]
	public List<StiCloseValueChartMeter> CloseValues { get; set; } = new List<StiCloseValueChartMeter>();


	[StiSerializable(StiSerializationVisibility.List)]
	[TypeConverter(typeof(StiMeterListConverter))]
	[Browsable(false)]
	public List<StiLowValueChartMeter> LowValues { get; set; } = new List<StiLowValueChartMeter>();


	[StiSerializable(StiSerializationVisibility.List)]
	[TypeConverter(typeof(StiMeterListConverter))]
	[Browsable(false)]
	public List<StiHighValueChartMeter> HighValues { get; set; } = new List<StiHighValueChartMeter>();


	[StiSerializable(StiSerializationVisibility.List)]
	[TypeConverter(typeof(StiMeterListConverter))]
	[Browsable(false)]
	[Editor("Stimulsoft.Dashboard.Design.Components.StiElementEditor, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public List<StiArgumentChartMeter> Arguments { get; set; } = new List<StiArgumentChartMeter>();


	[StiSerializable(StiSerializationVisibility.List)]
	[TypeConverter(typeof(StiMeterListConverter))]
	[Browsable(false)]
	public List<StiWeightChartMeter> Weights { get; set; } = new List<StiWeightChartMeter>();


	[StiSerializable(StiSerializationVisibility.Class)]
	[DefaultValue(null)]
	[Browsable(false)]
	public StiSeriesChartMeter Series { get; set; }

	[StiNonSerialized]
	[Browsable(false)]
	public StiXChartAxis XAxis
	{
		get
		{
			return Area?.XAxis;
		}
		set
		{
			if (Area != null)
			{
				Area.XAxis = value;
			}
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public StiXTopChartAxis XTopAxis
	{
		get
		{
			return Area?.XTopAxis;
		}
		set
		{
			if (Area != null)
			{
				Area.XTopAxis = value;
			}
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public StiYChartAxis YAxis
	{
		get
		{
			return Area?.YAxis;
		}
		set
		{
			if (Area != null)
			{
				Area.YAxis = value;
			}
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public StiYRightChartAxis YRightAxis
	{
		get
		{
			return Area?.YRightAxis;
		}
		set
		{
			if (Area != null)
			{
				Area.YRightAxis = value;
			}
		}
	}

	[Description("Gets or sets settings of the legend.")]
	[StiSerializable(StiSerializationVisibility.Class)]
	[StiPropertyLevel(StiLevel.Standard)]
	[StiCategory("Chart", StiCategorySource.PropertyCategory)]
	[StiOrder(270)]
	public StiChartLegend Legend
	{
		get
		{
			return legend;
		}
		set
		{
			legend = value;
			if (legend != null)
			{
				legend.ParentComponent = this;
				legend.Title.ParentComponent = this;
			}
		}
	}

	[Description("Gets or sets settings of the chart area.")]
	[StiSerializable(StiSerializationVisibility.Class)]
	[StiPropertyLevel(StiLevel.Standard)]
	[StiCategory("Chart", StiCategorySource.PropertyCategory)]
	[StiOrder(190)]
	public StiChartArea Area { get; set; } = new StiChartArea();


	[Description("Gets or sets settings of the chart labels.")]
	[StiSerializable(StiSerializationVisibility.Class)]
	[StiPropertyLevel(StiLevel.Standard)]
	[StiCategory("Chart", StiCategorySource.PropertyCategory)]
	[StiOrder(260)]
	public StiChartLabels Labels
	{
		get
		{
			return labels;
		}
		set
		{
			if (value != null)
			{
				value.element = this;
			}
			labels = value;
		}
	}

	[Editor("Stimulsoft.Report.Components.TextFormats.Design.StiArgumentFormatEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiSerializable(StiSerializationVisibility.Class)]
	[StiCategory("Behavior", StiCategorySource.PropertyCategory)]
	[StiOrder(105)]
	[StiPropertyLevel(StiLevel.Standard)]
	public StiFormatService ArgumentFormat { get; set; } = new StiGeneralFormatService();


	[Editor("Stimulsoft.Report.Components.TextFormats.Design.StiValueFormatEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiSerializable(StiSerializationVisibility.Class)]
	[StiCategory("Behavior", StiCategorySource.PropertyCategory)]
	[StiOrder(495)]
	[StiPropertyLevel(StiLevel.Standard)]
	public StiFormatService ValueFormat { get; set; } = GetValueFormatDefault();


	[StiCategory("Chart", StiCategorySource.PropertyCategory)]
	[StiSerializable(StiSerializationVisibility.Class)]
	[StiOrder(310)]
	[StiPropertyLevel(StiLevel.Standard)]
	public Sti3dOptions Options3D { get; set; }

	[RefreshProperties(RefreshProperties.All)]
	[StiSerializable(StiSerializationVisibility.List)]
	[TypeConverter(typeof(StiChartConstantLinesConverter))]
	[StiCategory("Chart", StiCategorySource.PropertyCategory)]
	[StiPropertyLevel(StiLevel.Standard)]
	[Description("Gets os sets constant lines settings of the chart.")]
	[StiOrder(230)]
	[Editor("Stimulsoft.Dashboard.Design.Components.StiChartConstantLinesEditor, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public List<StiChartConstantLines> ConstantLines { get; set; } = new List<StiChartConstantLines>();


	[RefreshProperties(RefreshProperties.All)]
	[StiSerializable(StiSerializationVisibility.List)]
	[TypeConverter(typeof(StiChartTrendLinesConverter))]
	[StiCategory("Chart", StiCategorySource.PropertyCategory)]
	[StiPropertyLevel(StiLevel.Standard)]
	[Description("Gets os sets trend lines settings of the chart.")]
	[StiOrder(290)]
	[Editor("Stimulsoft.Dashboard.Design.Components.StiChartTrendLinesEditor, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public List<StiChartTrendLine> TrendLines { get; set; } = new List<StiChartTrendLine>();


	[StiSerializable(StiSerializationVisibility.List)]
	[TypeConverter(typeof(StiChartConditionConverter))]
	[Browsable(false)]
	public List<StiChartElementCondition> ChartConditions { get; set; } = new List<StiChartElementCondition>();


	[RefreshProperties(RefreshProperties.All)]
	[Editor("Stimulsoft.Dashboard.Design.Components.StiChartConditionsEditor, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[StiPropertyLevel(StiLevel.Standard)]
	[StiOrder(140)]
	[StiSerializable(StiSerializationVisibility.List)]
	[TypeConverter(typeof(StiChartConditionConverter))]
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

	[Description("Gets or sets size of the marker.")]
	[StiSerializable(StiSerializationVisibility.Class)]
	[StiPropertyLevel(StiLevel.Standard)]
	[StiCategory("Chart", StiCategorySource.PropertyCategory)]
	[StiOrder(280)]
	public StiChartMarker Marker { get; set; } = new StiChartMarker();


	[StiSerializable]
	[DefaultValue(null)]
	[Browsable(false)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiFontIcons? Icon { get; set; }

	[StiSerializable]
	[DefaultValue(null)]
	[Browsable(false)]
	[TypeConverter(typeof(StiEnumConverter))]
	public bool RoundValues { get; set; } = true;


	[Browsable(false)]
	public bool IsAxisAreaChart => ChartValuesList().Any((StiValueChartMeter v) => v != null && StiChartTypes.IsAxisArea(v.SeriesType));

	[Browsable(false)]
	public bool IsStackedChart => ChartValuesList().Any((StiValueChartMeter v) => v != null && StiChartTypes.IsStacked(v.SeriesType));

	[Browsable(false)]
	public bool IsScatterChart => ChartValuesList().Any((StiValueChartMeter v) => v != null && StiChartTypes.IsScatter(v.SeriesType));

	[Browsable(false)]
	public bool IsLinesChart => ChartValuesList().Any((StiValueChartMeter v) => v != null && StiChartTypes.IsLine(v.SeriesType));

	[Browsable(false)]
	internal bool IsBubbleChart => ChartValuesList().Any((StiValueChartMeter v) => v != null && v.SeriesType == StiChartSeriesType.Bubble);

	[Browsable(false)]
	internal bool IsBarChart => ChartValuesList().Any((StiValueChartMeter v) => v != null && StiChartTypes.IsBar(v.SeriesType));

	[Browsable(false)]
	public bool IsRange => ChartValuesList().Any((StiValueChartMeter v) => v != null && StiChartTypes.IsRange(v.SeriesType));

	[Browsable(false)]
	internal bool IsFinancial => ChartValuesList().Any((StiValueChartMeter v) => v != null && StiChartTypes.IsFinancial(v.SeriesType));

	[Browsable(false)]
	public bool IsPieChart => ChartValuesList().Any((StiValueChartMeter v) => v != null && StiChartTypes.IsPie(v.SeriesType));

	[Browsable(false)]
	public bool IsPie3dChart => ChartValuesList().Any((StiValueChartMeter v) => v != null && StiChartTypes.IsPie3d(v.SeriesType));

	[Browsable(false)]
	public bool IsRadarChart => ChartValuesList().Any((StiValueChartMeter v) => v != null && StiChartTypes.IsRadar(v.SeriesType));

	[Browsable(false)]
	public bool IsDoughnutChart => ChartValuesList().Any((StiValueChartMeter v) => v != null && StiChartTypes.IsDoughnut(v.SeriesType));

	[Browsable(false)]
	public bool IsFunnelChart => ChartValuesList().Any((StiValueChartMeter v) => v != null && StiChartTypes.IsFunnel(v.SeriesType));

	[Browsable(false)]
	public bool IsTreemapChart => ChartValuesList().Any((StiValueChartMeter v) => v != null && StiChartTypes.IsTreemap(v.SeriesType));

	[Browsable(false)]
	public bool IsParetoChart => ChartValuesList().Any((StiValueChartMeter v) => v != null && StiChartTypes.IsPareto(v.SeriesType));

	[Browsable(false)]
	public bool IsSunburstChart => ChartValuesList().Any((StiValueChartMeter v) => v != null && StiChartTypes.IsSunburst(v.SeriesType));

	[Browsable(false)]
	public bool IsWaterfallChart => ChartValuesList().Any((StiValueChartMeter v) => v != null && StiChartTypes.IsWaterfall(v.SeriesType));

	[Browsable(false)]
	internal bool IsFullStackedChart => ChartValuesList().Any((StiValueChartMeter v) => v != null && StiChartTypes.IsFullStacked(v.SeriesType));

	[Browsable(false)]
	public bool IsPictorialChart => ChartValuesList().Any((StiValueChartMeter v) => v != null && StiChartTypes.IsPictorial(v.SeriesType));

	[Browsable(false)]
	public bool IsBoxAndWhisker => ChartValuesList().Any((StiValueChartMeter v) => v != null && StiChartTypes.IsBoxAndWhisker(v.SeriesType));

	public override object Clone(bool cloneProperties)
	{
		StiChartElement stiChartElement = ((StiElement)base.Clone(cloneProperties)) as StiChartElement;
		stiChartElement.Values = Values.Select((StiValueChartMeter c) => c.Clone() as StiValueChartMeter).ToList();
		stiChartElement.EndValues = EndValues.Select((StiEndValueChartMeter c) => c.Clone() as StiEndValueChartMeter).ToList();
		stiChartElement.CloseValues = CloseValues.Select((StiCloseValueChartMeter c) => c.Clone() as StiCloseValueChartMeter).ToList();
		stiChartElement.LowValues = LowValues.Select((StiLowValueChartMeter c) => c.Clone() as StiLowValueChartMeter).ToList();
		stiChartElement.HighValues = HighValues.Select((StiHighValueChartMeter c) => c.Clone() as StiHighValueChartMeter).ToList();
		stiChartElement.Arguments = Arguments.Select((StiArgumentChartMeter c) => c.Clone() as StiArgumentChartMeter).ToList();
		stiChartElement.Weights = Weights.Select((StiWeightChartMeter c) => c.Clone() as StiWeightChartMeter).ToList();
		stiChartElement.Series = Series?.Clone() as StiSeriesChartMeter;
		stiChartElement.ManuallyEnteredChartMeter = ManuallyEnteredChartMeter?.Clone() as StiValueChartMeter;
		stiChartElement.UserFilters = UserFilters.Select((StiDataFilterRule c) => c.Clone() as StiDataFilterRule).ToList();
		stiChartElement.UserSorts = UserSorts.Select((StiDataSortRule c) => c.Clone() as StiDataSortRule).ToList();
		stiChartElement.UserViewStates = UserViewStates.Select((StiUserViewState c) => c.Clone() as StiUserViewState).ToList();
		stiChartElement.TransformActions = TransformActions.Select((StiDataActionRule c) => c.Clone() as StiDataActionRule).ToList();
		stiChartElement.TransformFilters = TransformFilters.Select((StiDataFilterRule c) => c.Clone() as StiDataFilterRule).ToList();
		stiChartElement.TransformSorts = TransformSorts.Select((StiDataSortRule c) => c.Clone() as StiDataSortRule).ToList();
		stiChartElement.DataFilters = DataFilters.Select((StiDataFilterRule c) => c.Clone() as StiDataFilterRule).ToList();
		stiChartElement.TopN = TopN.Clone() as StiDataTopN;
		stiChartElement.ArgumentFormat = ArgumentFormat?.Clone() as StiFormatService;
		stiChartElement.ValueFormat = ValueFormat?.Clone() as StiFormatService;
		stiChartElement.Title = ((StiTitle)Title)?.Clone() as StiTitle;
		stiChartElement.DashboardInteraction = DashboardInteraction?.Clone() as StiDashboardInteraction;
		stiChartElement.XAxis = XAxis?.Clone() as StiXChartAxis;
		stiChartElement.XTopAxis = XTopAxis?.Clone() as StiXTopChartAxis;
		stiChartElement.YAxis = YAxis?.Clone() as StiYChartAxis;
		stiChartElement.YRightAxis = YRightAxis?.Clone() as StiYRightChartAxis;
		stiChartElement.Legend = Legend?.Clone() as StiChartLegend;
		stiChartElement.Area = Area?.Clone() as StiChartArea;
		stiChartElement.SeriesColors = SeriesColors?.Clone() as Color[];
		stiChartElement.NegativeSeriesColors = NegativeSeriesColors?.Clone() as Color[];
		stiChartElement.ParetoSeriesColors = ParetoSeriesColors?.Clone() as Color[];
		stiChartElement.Marker = Marker?.Clone() as StiChartMarker;
		stiChartElement.Labels = Labels?.Clone() as StiChartLabels;
		stiChartElement.ConstantLines = ConstantLines.Select((StiChartConstantLines c) => c.Clone() as StiChartConstantLines).ToList();
		stiChartElement.TrendLines = TrendLines.Select((StiChartTrendLine c) => c.Clone() as StiChartTrendLine).ToList();
		stiChartElement.ChartConditions = ChartConditions.Select((StiChartElementCondition c) => c.Clone() as StiChartElementCondition).ToList();
		stiChartElement.Shadow = (StiSimpleShadow)(Shadow?.Clone());
		stiChartElement.CornerRadius = CornerRadius?.Clone() as StiCornerRadius;
		return stiChartElement;
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = base.SaveToJsonObject(mode);
		jObject.AddPropertyJObject("UserFilters", StiJsonReportObjectHelper.Serialize.JObjectCollection(UserFilters, mode));
		jObject.AddPropertyJObject("UserSorts", StiJsonReportObjectHelper.Serialize.JObjectCollection(UserSorts, mode));
		jObject.AddPropertyJObject("UserViewStates", StiJsonReportObjectHelper.Serialize.JObjectCollection(UserViewStates, mode));
		jObject.AddPropertyJObject("TransformActions", StiJsonReportObjectHelper.Serialize.JObjectCollection(TransformActions, mode));
		jObject.AddPropertyJObject("TransformFilters", StiJsonReportObjectHelper.Serialize.JObjectCollection(TransformFilters, mode));
		jObject.AddPropertyJObject("TransformSorts", StiJsonReportObjectHelper.Serialize.JObjectCollection(TransformSorts, mode));
		jObject.AddPropertyJObject("DataFilters", StiJsonReportObjectHelper.Serialize.JObjectCollection(DataFilters, mode));
		jObject.AddPropertyJObject("Values", StiJsonReportObjectHelper.Serialize.JObjectCollection(Values, mode));
		jObject.AddPropertyJObject("EndValues", StiJsonReportObjectHelper.Serialize.JObjectCollection(EndValues, mode));
		jObject.AddPropertyJObject("CloseValues", StiJsonReportObjectHelper.Serialize.JObjectCollection(CloseValues, mode));
		jObject.AddPropertyJObject("LowValues", StiJsonReportObjectHelper.Serialize.JObjectCollection(LowValues, mode));
		jObject.AddPropertyJObject("HighValues", StiJsonReportObjectHelper.Serialize.JObjectCollection(HighValues, mode));
		jObject.AddPropertyJObject("Arguments", StiJsonReportObjectHelper.Serialize.JObjectCollection(Arguments, mode));
		jObject.AddPropertyJObject("Weights", StiJsonReportObjectHelper.Serialize.JObjectCollection(Weights, mode));
		if (TopN != null)
		{
			jObject.AddPropertyJObject("TopN", TopN.SaveToJsonObject(mode));
		}
		if (DashboardInteraction is StiChartDashboardInteraction stiChartDashboardInteraction)
		{
			jObject.AddPropertyJObject("DashboardInteraction", stiChartDashboardInteraction.SaveToJsonObject(mode));
		}
		if (Title != null)
		{
			jObject.AddPropertyJObject("Title", ((StiTitle)Title).SaveToJsonObject(mode));
		}
		if (ManuallyEnteredChartMeter != null)
		{
			jObject.AddPropertyJObject("ManuallyEnteredChartMeter", ManuallyEnteredChartMeter.SaveToJsonObject(mode));
		}
		if (Series != null)
		{
			jObject.AddPropertyJObject("Series", Series.SaveToJsonObject(mode));
		}
		if (ArgumentFormat != null)
		{
			jObject.AddPropertyJObject("ArgumentFormat", ArgumentFormat.SaveToJsonObject(mode));
		}
		if (ValueFormat != null)
		{
			jObject.AddPropertyJObject("ValueFormat", ValueFormat.SaveToJsonObject(mode));
		}
		if (XAxis != null)
		{
			jObject.AddPropertyJObject("XAxis", XAxis.SaveToJsonObject(mode));
		}
		if (XTopAxis != null)
		{
			jObject.AddPropertyJObject("XTopAxis", XTopAxis.SaveToJsonObject(mode));
		}
		if (YAxis != null)
		{
			jObject.AddPropertyJObject("YAxis", YAxis.SaveToJsonObject(mode));
		}
		if (YRightAxis != null)
		{
			jObject.AddPropertyJObject("YRightAxis", YRightAxis.SaveToJsonObject(mode));
		}
		if (Legend != null)
		{
			jObject.AddPropertyJObject("Legend", Legend.SaveToJsonObject(mode));
		}
		if (Area != null)
		{
			jObject.AddPropertyJObject("Area", Area.SaveToJsonObject(mode));
		}
		if (Marker != null)
		{
			jObject.AddPropertyJObject("Marker", Marker.SaveToJsonObject(mode));
		}
		if (Labels != null)
		{
			jObject.AddPropertyJObject("Labels", Labels.SaveToJsonObject(mode));
		}
		if (Options3D != null)
		{
			jObject.AddPropertyJObject("Options3D", Options3D.SaveToJsonObject(mode));
		}
		jObject.AddPropertyArrayColor("SeriesColors", SeriesColors);
		jObject.AddPropertyArrayColor("NegativeSeriesColors", NegativeSeriesColors);
		jObject.AddPropertyArrayColor("ParetoSeriesColors", ParetoSeriesColors);
		jObject.AddPropertyEnum("DataMode", DataMode, StiDataMode.UsingDataFields);
		jObject.AddPropertyStringNullOrEmpty("ManuallyEnteredData", ManuallyEnteredData);
		jObject.AddPropertyEnum("Style", Style, StiElementStyleIdent.Auto);
		jObject.AddPropertyStringNullOrEmpty("Group", Group);
		jObject.AddPropertyBool("CrossFiltering", CrossFiltering, defaultValue: true);
		jObject.AddPropertyStringNullOrEmpty("CustomStyleName", CustomStyleName);
		jObject.AddPropertyJObject("ConstantLines", StiJsonReportObjectHelper.Serialize.JObjectCollection(ConstantLines, mode));
		jObject.AddPropertyJObject("TrendLines", StiJsonReportObjectHelper.Serialize.JObjectCollection(TrendLines, mode));
		jObject.AddPropertyJObject("ChartConditions", StiJsonReportObjectHelper.Serialize.JObjectCollection(ChartConditions, mode));
		jObject.AddPropertyCornerRadius("CornerRadius", CornerRadius);
		jObject.AddPropertyShadow("Shadow", Shadow);
		if (Icon.HasValue)
		{
			jObject.AddPropertyEnum("Icon", Icon);
		}
		jObject.AddPropertyBool("RoundValues", RoundValues, defaultValue: true);
		if (SelectedViewStateKey != null)
		{
			jObject.AddPropertyStringNullOrEmpty("SelectedViewStateKey", SelectedViewStateKey);
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
				StiChartDashboardInteraction stiChartDashboardInteraction = StiDashboardInteractionLoader.LoadInteractionFromJsonObject((JObject)item.Value) as StiChartDashboardInteraction;
				DashboardInteraction = stiChartDashboardInteraction ?? null;
				break;
			}
			case "TopN":
				TopN = StiDataTopN.CreateFromJsonObject((JObject)item.Value);
				break;
			case "Title":
				Title = StiTitle.CreateFromJsonObject((JObject)item.Value);
				break;
			case "Layout":
				Layout = StiElementLayout.CreateFromJsonObject((JObject)item.Value, this);
				break;
			case "UserSorts":
				UserSorts.AddRange(from p in ((JObject)item.Value).Properties()
					select StiDataSortRule.LoadFromJson((JObject)p.Value));
				break;
			case "UserFilters":
				UserFilters.AddRange(from p in ((JObject)item.Value).Properties()
					select StiDataFilterRule.LoadFromJson((JObject)p.Value));
				break;
			case "UserViewStates":
				UserViewStates.AddRange(from p in ((JObject)item.Value).Properties()
					select StiUserViewState.LoadFromJson((JObject)p.Value));
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
			case "Values":
				Values.AddRange(from p in ((JObject)item.Value).Properties()
					select (StiValueChartMeter)StiMeterLoader.LoadFromJson((JObject)p.Value));
				break;
			case "EndValues":
				EndValues.AddRange(from p in ((JObject)item.Value).Properties()
					select (StiEndValueChartMeter)StiMeterLoader.LoadFromJson((JObject)p.Value));
				break;
			case "CloseValues":
				CloseValues.AddRange(from p in ((JObject)item.Value).Properties()
					select (StiCloseValueChartMeter)StiMeterLoader.LoadFromJson((JObject)p.Value));
				break;
			case "LowValues":
				LowValues.AddRange(from p in ((JObject)item.Value).Properties()
					select (StiLowValueChartMeter)StiMeterLoader.LoadFromJson((JObject)p.Value));
				break;
			case "HighValues":
				HighValues.AddRange(from p in ((JObject)item.Value).Properties()
					select (StiHighValueChartMeter)StiMeterLoader.LoadFromJson((JObject)p.Value));
				break;
			case "Arguments":
				Arguments.AddRange(from p in ((JObject)item.Value).Properties()
					select (StiArgumentChartMeter)StiMeterLoader.LoadFromJson((JObject)p.Value));
				break;
			case "Weights":
				Weights.AddRange(from p in ((JObject)item.Value).Properties()
					select (StiWeightChartMeter)StiMeterLoader.LoadFromJson((JObject)p.Value));
				break;
			case "Series":
			{
				StiSeriesChartMeter stiSeriesChartMeter = StiMeterLoader.LoadFromJson((JObject)item.Value) as StiSeriesChartMeter;
				Series = stiSeriesChartMeter ?? null;
				break;
			}
			case "ManuallyEnteredChartMeter":
			{
				StiValueChartMeter stiValueChartMeter = StiMeterLoader.LoadFromJson((JObject)item.Value) as StiValueChartMeter;
				ManuallyEnteredChartMeter = stiValueChartMeter ?? null;
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
			case "ColorEach":
				if (Area != null)
				{
					Area.ColorEach = item.DeserializeBool();
				}
				break;
			case "ArgumentFormat":
				ArgumentFormat = StiFormatService.CreateFromJsonObject((JObject)item.Value);
				break;
			case "ValueFormat":
				ValueFormat = StiFormatService.CreateFromJsonObject((JObject)item.Value);
				break;
			case "XAxis":
				XAxis.LoadFromJsonObject((JObject)item.Value);
				break;
			case "XTopAxis":
				XTopAxis.LoadFromJsonObject((JObject)item.Value);
				break;
			case "YAxis":
				YAxis.LoadFromJsonObject((JObject)item.Value);
				break;
			case "YRightAxis":
				YRightAxis.LoadFromJsonObject((JObject)item.Value);
				break;
			case "Legend":
				Legend.LoadFromJsonObject((JObject)item.Value);
				break;
			case "Area":
				Area.LoadFromJsonObject((JObject)item.Value);
				break;
			case "Marker":
				Marker.LoadFromJsonObject((JObject)item.Value);
				break;
			case "Labels":
				Labels.LoadFromJsonObject((JObject)item.Value);
				break;
			case "Options3D":
				Options3D.LoadFromJsonObject((JObject)item.Value);
				break;
			case "SeriesColors":
				SeriesColors = item.DeserializeArrayColor();
				break;
			case "NegativeSeriesColors":
				NegativeSeriesColors = item.DeserializeArrayColor();
				break;
			case "ParetoSeriesColors":
				ParetoSeriesColors = item.DeserializeArrayColor();
				break;
			case "ConstantLines":
				ConstantLines.AddRange(from p in ((JObject)item.Value).Properties()
					select StiChartConstantLines.LoadFromJson((JObject)p.Value));
				break;
			case "TrendLines":
				TrendLines.AddRange(from p in ((JObject)item.Value).Properties()
					select StiChartTrendLine.LoadFromJson((JObject)p.Value));
				break;
			case "ChartConditions":
				ChartConditions.AddRange(from p in ((JObject)item.Value).Properties()
					select StiChartElementCondition.LoadFromJson((JObject)p.Value));
				break;
			case "Icon":
				Icon = item.DeserializeEnum<StiFontIcons>();
				break;
			case "RoundValues":
				RoundValues = item.DeserializeBool();
				break;
			case "SelectedViewStateKey":
				SelectedViewStateKey = item.DeserializeString();
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

	public void ConvertToBubble()
	{
		Arguments?.Where((StiArgumentChartMeter a) => a != null).ToList().ForEach(delegate(StiArgumentChartMeter a)
		{
			a.Expression = Stimulsoft.Data.Helpers.StiExpressionHelper.ReplaceFunction(a.Expression, "Sum");
		});
	}

	public void ConvertFromBubble()
	{
		Arguments?.Where((StiArgumentChartMeter a) => a != null).ToList().ForEach(delegate(StiArgumentChartMeter a)
		{
			a.Expression = Stimulsoft.Data.Helpers.StiExpressionHelper.RemoveFunction(a.Expression);
		});
		Weights?.Clear();
	}

	public void ConvertToGantt()
	{
		Values?.Where((StiValueChartMeter a) => a != null).ToList().ForEach(delegate(StiValueChartMeter a)
		{
			a.Expression = Stimulsoft.Data.Helpers.StiExpressionHelper.RemoveFunction(a.Expression);
		});
	}

	public List<string> GetChartSeriesTypes(string seriesTypeStr)
	{
		List<string> list = new List<string>();
		if (Values.Count > 1)
		{
			StiChartSeriesType type = (StiChartSeriesType)Enum.Parse(typeof(StiChartSeriesType), seriesTypeStr);
			List<StiChartSeriesType> group = StiChartGroups.GetGroup(type);
			if (group != null)
			{
				foreach (StiChartSeriesType item in group)
				{
					list.Add(item.ToString());
				}
			}
		}
		return list;
	}

	public IStiMeter GetManuallyEnteredChartMeter()
	{
		return ManuallyEnteredChartMeter;
	}

	public void AddValue(IStiAppDataCell cell)
	{
		if (cell != null)
		{
			Values.Add(StiMeterHelper.Chart.GetValue(cell, this));
		}
	}

	public IStiMeter GetValue(IStiAppDataCell cell)
	{
		if (cell == null)
		{
			return null;
		}
		return StiMeterHelper.Chart.GetValue(cell, this);
	}

	public IStiMeter GetValue(IStiMeter meter)
	{
		if (meter == null)
		{
			return null;
		}
		return StiMeterHelper.Chart.GetValue(meter as StiMeter);
	}

	public IStiMeter GetGanttStartValue(IStiAppDataCell cell)
	{
		if (cell == null)
		{
			return null;
		}
		return StiMeterHelper.Chart.GetGanttStartValue(cell);
	}

	public IStiMeter GetGanttEndValue(IStiAppDataCell cell)
	{
		if (cell == null)
		{
			return null;
		}
		return StiMeterHelper.Chart.GetGanttEndValue(cell);
	}

	public IStiMeter GetY(IStiAppDataCell cell)
	{
		if (cell == null)
		{
			return null;
		}
		return StiMeterHelper.Chart.GetY(cell);
	}

	public IStiMeter GetX(IStiAppDataCell cell)
	{
		if (cell == null)
		{
			return null;
		}
		return StiMeterHelper.Chart.GetX(cell);
	}

	public List<IStiMeter> FetchAllValues()
	{
		return Values.Cast<IStiMeter>().ToList();
	}

	public IStiMeter GetValueByIndex(int index)
	{
		if (index >= Values.Count)
		{
			return null;
		}
		return Values[index];
	}

	public void InsertValue(int index, IStiMeter meter)
	{
		if (meter is StiValueChartMeter item)
		{
			if (index >= 0 && index < Values.Count)
			{
				Values.Insert(index, item);
			}
			else
			{
				Values.Add(item);
			}
		}
	}

	public void RemoveValue(int index)
	{
		if (index < Values.Count)
		{
			Values.RemoveAt(index);
		}
	}

	public void RemoveAllValues()
	{
		Values.Clear();
	}

	public IStiMeter CreateNewValue()
	{
		StiValueChartMeter stiValueChartMeter = new StiValueChartMeter
		{
			Expression = "Sum()"
		};
		Values.Add(stiValueChartMeter);
		return stiValueChartMeter;
	}

	public void AddEndValue(IStiAppDataCell cell)
	{
		if (cell != null)
		{
			EndValues.Add(StiMeterHelper.Chart.GetEndValue(cell));
		}
	}

	public IStiMeter GetEndValue(IStiAppDataCell cell)
	{
		if (cell == null)
		{
			return null;
		}
		return StiMeterHelper.Chart.GetEndValue(cell);
	}

	public IStiMeter GetEndValue(IStiMeter meter)
	{
		if (meter == null)
		{
			return null;
		}
		return StiMeterHelper.Chart.GetEndValue(meter as StiMeter);
	}

	public IStiMeter GetEndValueByIndex(int index)
	{
		if (index >= EndValues.Count)
		{
			return null;
		}
		return EndValues[index];
	}

	public void InsertEndValue(int index, IStiMeter meter)
	{
		if (meter is StiEndValueChartMeter item)
		{
			if (index >= 0 && index < EndValues.Count)
			{
				EndValues.Insert(index, item);
			}
			else
			{
				EndValues.Add(item);
			}
		}
	}

	public void RemoveEndValue(int index)
	{
		if (index < EndValues.Count)
		{
			EndValues.RemoveAt(index);
		}
	}

	public void RemoveAllEndValues()
	{
		EndValues.Clear();
	}

	public IStiMeter CreateNewEndValue()
	{
		StiEndValueChartMeter stiEndValueChartMeter = new StiEndValueChartMeter
		{
			Expression = "Sum()"
		};
		EndValues.Add(stiEndValueChartMeter);
		return stiEndValueChartMeter;
	}

	public void AddCloseValue(IStiAppDataCell cell)
	{
		if (cell != null)
		{
			CloseValues.Add(StiMeterHelper.Chart.GetCloseValue(cell));
		}
	}

	public IStiMeter GetCloseValue(IStiAppDataCell cell)
	{
		if (cell == null)
		{
			return null;
		}
		return StiMeterHelper.Chart.GetCloseValue(cell);
	}

	public IStiMeter GetCloseValue(IStiMeter meter)
	{
		if (meter == null)
		{
			return null;
		}
		return StiMeterHelper.Chart.GetCloseValue(meter as StiMeter);
	}

	public IStiMeter GetCloseValueByIndex(int index)
	{
		if (index >= CloseValues.Count)
		{
			return null;
		}
		return CloseValues[index];
	}

	public void InsertCloseValue(int index, IStiMeter meter)
	{
		if (meter is StiCloseValueChartMeter item)
		{
			if (index >= 0 && index < CloseValues.Count)
			{
				CloseValues.Insert(index, item);
			}
			else
			{
				CloseValues.Add(item);
			}
		}
	}

	public void RemoveCloseValue(int index)
	{
		if (index < CloseValues.Count)
		{
			CloseValues.RemoveAt(index);
		}
	}

	public void RemoveAllCloseValues()
	{
		CloseValues.Clear();
	}

	public IStiMeter CreateNewCloseValue()
	{
		StiCloseValueChartMeter stiCloseValueChartMeter = new StiCloseValueChartMeter
		{
			Expression = "Sum()"
		};
		CloseValues.Add(stiCloseValueChartMeter);
		return stiCloseValueChartMeter;
	}

	public void AddLowValue(IStiAppDataCell cell)
	{
		if (cell != null)
		{
			LowValues.Add(StiMeterHelper.Chart.GetLowValue(cell));
		}
	}

	public IStiMeter GetLowValue(IStiAppDataCell cell)
	{
		if (cell == null)
		{
			return null;
		}
		return StiMeterHelper.Chart.GetLowValue(cell);
	}

	public IStiMeter GetLowValue(IStiMeter meter)
	{
		if (meter == null)
		{
			return null;
		}
		return StiMeterHelper.Chart.GetLowValue(meter as StiMeter);
	}

	public IStiMeter GetLowValueByIndex(int index)
	{
		if (index >= LowValues.Count)
		{
			return null;
		}
		return LowValues[index];
	}

	public void InsertLowValue(int index, IStiMeter meter)
	{
		if (meter is StiLowValueChartMeter item)
		{
			if (index >= 0 && index < LowValues.Count)
			{
				LowValues.Insert(index, item);
			}
			else
			{
				LowValues.Add(item);
			}
		}
	}

	public void RemoveLowValue(int index)
	{
		if (index < LowValues.Count)
		{
			LowValues.RemoveAt(index);
		}
	}

	public void RemoveAllLowValues()
	{
		LowValues.Clear();
	}

	public IStiMeter CreateNewLowValue()
	{
		StiLowValueChartMeter stiLowValueChartMeter = new StiLowValueChartMeter
		{
			Expression = "Sum()"
		};
		LowValues.Add(stiLowValueChartMeter);
		return stiLowValueChartMeter;
	}

	public void AddHighalue(IStiAppDataCell cell)
	{
		if (cell != null)
		{
			HighValues.Add(StiMeterHelper.Chart.GetHighValue(cell));
		}
	}

	public IStiMeter GetHighValue(IStiAppDataCell cell)
	{
		if (cell == null)
		{
			return null;
		}
		return StiMeterHelper.Chart.GetHighValue(cell);
	}

	public IStiMeter GetHighValue(IStiMeter meter)
	{
		if (meter == null)
		{
			return null;
		}
		return StiMeterHelper.Chart.GetHighValue(meter as StiMeter);
	}

	public IStiMeter GetHighValueByIndex(int index)
	{
		if (index >= HighValues.Count)
		{
			return null;
		}
		return HighValues[index];
	}

	public void InsertHighValue(int index, IStiMeter meter)
	{
		if (meter is StiHighValueChartMeter item)
		{
			if (index >= 0 && index < HighValues.Count)
			{
				HighValues.Insert(index, item);
			}
			else
			{
				HighValues.Add(item);
			}
		}
	}

	public void RemoveHighValue(int index)
	{
		if (index < HighValues.Count)
		{
			HighValues.RemoveAt(index);
		}
	}

	public void RemoveAllHighValues()
	{
		HighValues.Clear();
	}

	public IStiMeter CreateNewHighValue()
	{
		StiHighValueChartMeter stiHighValueChartMeter = new StiHighValueChartMeter
		{
			Expression = "Sum()"
		};
		HighValues.Add(stiHighValueChartMeter);
		return stiHighValueChartMeter;
	}

	public void AddArgument(IStiAppDataCell cell)
	{
		if (cell != null)
		{
			Arguments.Add(StiMeterHelper.Chart.GetArgument(cell));
		}
	}

	public IStiMeter GetArgument(IStiAppDataCell cell)
	{
		if (cell == null)
		{
			return null;
		}
		return StiMeterHelper.Chart.GetArgument(cell);
	}

	public IStiMeter GetArgument(IStiMeter meter)
	{
		if (meter == null)
		{
			return null;
		}
		return StiMeterHelper.Chart.GetArgument(meter as StiMeter);
	}

	public List<IStiMeter> FetchAllArguments()
	{
		return Arguments.Cast<IStiMeter>().ToList();
	}

	public IStiMeter GetArgumentByIndex(int index)
	{
		if (index >= Arguments.Count)
		{
			return null;
		}
		return Arguments[index];
	}

	public void InsertArgument(int index, IStiMeter meter)
	{
		if (meter is StiArgumentChartMeter item)
		{
			if (index >= 0 && index < Arguments.Count)
			{
				Arguments.Insert(index, item);
			}
			else
			{
				Arguments.Add(item);
			}
		}
	}

	public void RemoveArgument(int index)
	{
		if (index < Arguments.Count)
		{
			Arguments.RemoveAt(index);
		}
	}

	public void RemoveAllArguments()
	{
		Arguments.Clear();
	}

	public void CreateNewArgument()
	{
		Arguments.Add(new StiArgumentChartMeter());
	}

	public void AddWeight(IStiAppDataCell cell)
	{
		if (cell != null)
		{
			Weights.Add(StiMeterHelper.Chart.GetWeight(cell));
		}
	}

	public IStiMeter GetWeight(IStiAppDataCell cell)
	{
		if (cell == null)
		{
			return null;
		}
		return StiMeterHelper.Chart.GetWeight(cell);
	}

	public IStiMeter GetWeight(IStiMeter meter)
	{
		if (meter == null)
		{
			return null;
		}
		return StiMeterHelper.Chart.GetWeight(meter as StiMeter);
	}

	public IStiMeter GetWeightByIndex(int index)
	{
		if (index >= Weights.Count)
		{
			return null;
		}
		return Weights[index];
	}

	public void InsertWeight(int index, IStiMeter meter)
	{
		if (meter is StiWeightChartMeter item)
		{
			if (index >= 0 && index < Weights.Count)
			{
				Weights.Insert(index, item);
			}
			else
			{
				Weights.Add(item);
			}
		}
	}

	public void RemoveWeight(int index)
	{
		if (index < Weights.Count)
		{
			Weights.RemoveAt(index);
		}
	}

	public void RemoveAllWeights()
	{
		Weights.Clear();
	}

	public void CreateNewWeight()
	{
		Weights.Add(new StiWeightChartMeter
		{
			Expression = "Sum()"
		});
	}

	public void AddSeries(IStiAppDataCell cell)
	{
		Series = ((cell != null) ? StiMeterHelper.Chart.GetSeries(cell) : null);
	}

	public IStiMeter GetSeries(IStiAppDataCell cell)
	{
		if (cell == null)
		{
			return null;
		}
		return StiMeterHelper.Chart.GetSeries(cell);
	}

	public IStiMeter GetSeries(IStiMeter meter)
	{
		if (meter == null)
		{
			return null;
		}
		return StiMeterHelper.Chart.GetSeries(meter as StiMeter);
	}

	public IStiMeter GetSeries()
	{
		return Series;
	}

	public void InsertSeries(IStiMeter meter)
	{
		Series = meter as StiSeriesChartMeter;
	}

	public void RemoveSeries()
	{
		Series = null;
	}

	public void CreateNewSeries()
	{
		Series = new StiSeriesChartMeter();
	}

	public List<IStiChartConstantLines> FetchConstantLines()
	{
		List<IStiChartConstantLines> list = new List<IStiChartConstantLines>();
		if (ConstantLines != null)
		{
			list.AddRange(ConstantLines);
		}
		return list;
	}

	public void AddConstantLine()
	{
		ConstantLines.Add(new StiChartConstantLines());
	}

	public void RemoveConstantLine(int index)
	{
		if (index >= 0 && index < ConstantLines.Count)
		{
			ConstantLines.RemoveAt(index);
		}
	}

	public void MoveConstantLine(int fromIndex, int toIndex)
	{
		if (fromIndex >= 0 && fromIndex < ConstantLines.Count)
		{
			StiChartConstantLines item = ConstantLines[fromIndex];
			ConstantLines.RemoveAt(fromIndex);
			ConstantLines.Insert(toIndex, item);
		}
	}

	public void AddTrendLines(string keyValueMeter, StiChartTrendLineType type, Color lineColor, StiPenStyle lineStyle, float lineWidth)
	{
		TrendLines.Add(new StiChartTrendLine(keyValueMeter, type, lineColor, lineStyle, lineWidth));
	}

	public List<IStiChartTrendLine> FetchTrendLines()
	{
		List<IStiChartTrendLine> list = new List<IStiChartTrendLine>();
		if (TrendLines != null)
		{
			list.AddRange(TrendLines);
		}
		return list;
	}

	public void ClearTrendLines()
	{
		TrendLines.Clear();
	}

	public void AddChartCondition(string keyValueMeter, Stimulsoft.Report.Components.StiFilterDataType dataType, Stimulsoft.Report.Components.StiFilterCondition condition, string value, Color color, StiMarkerType markerType, float markerAngle, StiChartConditionalField field, bool isExpression)
	{
		ChartConditions.Add(new StiChartElementCondition(keyValueMeter, color, dataType, condition, value, markerType, markerAngle, field, isExpression));
	}

	public List<IStiChartElementCondition> FetchChartConditions()
	{
		List<IStiChartElementCondition> list = new List<IStiChartElementCondition>();
		if (ChartConditions != null)
		{
			list.AddRange(ChartConditions);
		}
		return list;
	}

	public void ClearChartConditions()
	{
		ChartConditions.Clear();
	}

	public StiDataTable GetManuallyEnteredDataTable()
	{
		if (DataMode != StiDataMode.ManuallyEnteringData)
		{
			return null;
		}
		return StiManuallyEnteredDataParser.GetChartDataTable(ManuallyEnteredData, this);
	}

	public void ConvertFrom(IStiElement element)
	{
		List<IStiMeter> list = element.FetchAllMeters();
		Values.Clear();
		Arguments.Clear();
		list?.Where((IStiMeter m) => (m is IStiValueMeter || m is IStiMeasureMeter) && !(m is IStiSeriesMeter)).Cast<StiMeter>().ToList()
			.ForEach(delegate(StiMeter m)
			{
				StiValueChartMeter stiValueChartMeter = new StiValueChartMeter();
				stiValueChartMeter.LoadFromJsonObject(m.SaveToJsonObject(StiJsonSaveMode.Report));
				Values.Add(stiValueChartMeter);
			});
		list?.Where((IStiMeter m) => m is IStiSeriesMeter).Cast<StiMeter>().ToList()
			.ForEach(delegate(StiMeter m)
			{
				StiArgumentChartMeter stiArgumentChartMeter = new StiArgumentChartMeter();
				stiArgumentChartMeter.LoadFromJsonObject(m.SaveToJsonObject(StiJsonSaveMode.Report));
				Arguments.Add(stiArgumentChartMeter);
			});
		if (DashboardInteraction == null)
		{
			DashboardInteraction = new StiChartDashboardInteraction();
		}
	}

	public override List<IStiMeter> FetchAllMeters()
	{
		List<IStiMeter> list = new List<IStiMeter>();
		if (DataMode == StiDataMode.UsingDataFields)
		{
			if (Values != null)
			{
				list.AddRange(Values);
			}
			if (EndValues != null)
			{
				list.AddRange(EndValues);
			}
			if (CloseValues != null)
			{
				list.AddRange(CloseValues);
			}
			if (LowValues != null)
			{
				list.AddRange(LowValues);
			}
			if (HighValues != null)
			{
				list.AddRange(HighValues);
			}
			if (Arguments != null)
			{
				list.AddRange(Arguments);
			}
			if (Weights != null)
			{
				list.AddRange(Weights);
			}
			if (Series != null)
			{
				list.Add(Series);
			}
		}
		return list;
	}

	public override List<IStiMeter> GetMeters()
	{
		List<IStiMeter> list = new List<IStiMeter>();
		if (DataMode == StiDataMode.UsingDataFields)
		{
			foreach (StiValueChartMeter value in Values)
			{
				if (IsBubbleChart)
				{
					list.Add(new StiYChartMeter
					{
						Key = value.Key,
						Expression = value.Expression,
						Label = value.Label
					});
				}
				else if (IsFinancial)
				{
					list.Add(new StiOpenValueChartMeter
					{
						SeriesType = value.SeriesType,
						Key = value.Key,
						Expression = value.Expression,
						Label = value.Label
					});
				}
				else if (IsRange)
				{
					list.Add(new StiStartValueChartMeter
					{
						SeriesType = value.SeriesType,
						Key = value.Key,
						Expression = value.Expression,
						Label = value.Label
					});
				}
				else if ((value.ShowZeros != StiEmptyCellsAs.Zero || value.ShowNulls != StiEmptyCellsAs.Zero) && StiChartSeriesCreator.New(value.SeriesType) is IStiShowNullsSeries)
				{
					string text = Stimulsoft.Data.Helpers.StiExpressionHelper.GetFunction(value.Expression)?.ToLowerInvariant();
					string text2 = null;
					if (text == "sum")
					{
						text2 = "sumnulls";
					}
					else if (text == "avg")
					{
						text2 = "avgnulls";
					}
					if (text2 != null)
					{
						StiValueChartMeter stiValueChartMeter = value.Clone() as StiValueChartMeter;
						stiValueChartMeter.Expression = Stimulsoft.Data.Helpers.StiExpressionHelper.ReplaceFunction(value.Expression, text2);
						list.Add(stiValueChartMeter);
					}
					else
					{
						list.Add(value);
					}
				}
				else
				{
					list.Add(value);
				}
			}
			if (IsRange)
			{
				foreach (StiEndValueChartMeter endValue in EndValues)
				{
					list.Add(endValue);
				}
			}
			if (IsFinancial)
			{
				foreach (StiCloseValueChartMeter closeValue in CloseValues)
				{
					list.Add(closeValue);
				}
			}
			if (IsFinancial)
			{
				foreach (StiLowValueChartMeter lowValue in LowValues)
				{
					list.Add(lowValue);
				}
			}
			if (IsFinancial)
			{
				foreach (StiHighValueChartMeter highValue in HighValues)
				{
					list.Add(highValue);
				}
			}
			List<StiArgumentChartMeter> list2 = ((StiDrillDownHelper.IsDrillAvailable(this) && DrillDownCurrentLevel < DrillDownLevelCount) ? Arguments.GetRange(DrillDownCurrentLevel, 1) : Arguments);
			foreach (StiArgumentChartMeter item in list2)
			{
				if (IsBubbleChart)
				{
					list.Add(new StiXChartMeter
					{
						Key = item.Key,
						Expression = item.Expression,
						Label = item.Label
					});
				}
				else
				{
					list.Add(item);
				}
			}
			if (IsBubbleChart)
			{
				foreach (StiWeightChartMeter weight in Weights)
				{
					list.Add(weight);
				}
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
		List<string> list = new List<string>();
		if (DataMode == StiDataMode.UsingDataFields)
		{
			list.AddRange(StiUsedDataHelper.GetMany(Values));
			list.AddRange(StiUsedDataHelper.GetMany(Arguments));
			list.AddRange(StiUsedDataHelper.GetMany(Weights));
			list.AddRange(StiUsedDataHelper.GetMany(Series));
		}
		return list.Distinct().ToList();
	}

	public void SwitchSelectedViewState(string newKey)
	{
		StiChartUserViewStatesHelper.SwitchSelectedViewState(this, newKey);
	}

	public JObject SaveToJsonForViewState()
	{
		return StiChartUserViewStatesHelper.SaveToJsonObject(this);
	}

	public void LoadFromJsonForViewState(JObject jObject)
	{
		StiChartUserViewStatesHelper.LoadFromJsonObject(this, jObject);
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

	private bool ShouldSerializeSeriesColors()
	{
		if (SeriesColors != null)
		{
			return SeriesColors.Length != 0;
		}
		return true;
	}

	private bool ShouldSerializeNegativeSeriesColors()
	{
		if (NegativeSeriesColors != null)
		{
			return NegativeSeriesColors.Length != 0;
		}
		return true;
	}

	private bool ShouldSerializeParetoSeriesColors()
	{
		if (ParetoSeriesColors != null)
		{
			return ParetoSeriesColors.Length != 0;
		}
		return true;
	}

	void IStiGlobalizationProvider.SetString(string propertyName, string value)
	{
		switch (propertyName)
		{
		case "Title.Text":
			Title.Text = value;
			break;
		case "Legend.Title.Text":
			Legend.Title.Text = value;
			break;
		case "XAxis.Labels.TextAfter":
			XAxis.Labels.TextAfter = value;
			break;
		case "XAxis.Labels.TextBefore":
			XAxis.Labels.TextBefore = value;
			break;
		case "XAxis.Title.Text":
			XAxis.Title.Text = value;
			break;
		case "XTopAxis.Labels.TextAfter":
			XTopAxis.Labels.TextAfter = value;
			break;
		case "XTopAxis.Labels.TextBefore":
			XTopAxis.Labels.TextBefore = value;
			break;
		case "XTopAxis.Title.Text":
			XTopAxis.Title.Text = value;
			break;
		case "YAxis.Labels.TextAfter":
			YAxis.Labels.TextAfter = value;
			break;
		case "YAxis.Labels.TextBefore":
			YAxis.Labels.TextBefore = value;
			break;
		case "YAxis.Title.Text":
			YAxis.Title.Text = value;
			break;
		case "YRightAxis.Labels.TextAfter":
			YRightAxis.Labels.TextAfter = value;
			break;
		case "YRightAxis.Labels.TextBefore":
			YRightAxis.Labels.TextBefore = value;
			break;
		case "YRightAxis.Title.Text":
			YRightAxis.Title.Text = value;
			break;
		}
		int num = 0;
		foreach (StiValueChartMeter value2 in Values)
		{
			string text = $"Value{num++}.Label";
			if (propertyName == text)
			{
				value2.Label = value;
			}
		}
		num = 0;
		foreach (StiArgumentChartMeter argument in Arguments)
		{
			string text2 = $"Argument{num++}.Label";
			if (propertyName == text2)
			{
				argument.Label = value;
			}
		}
	}

	string IStiGlobalizationProvider.GetString(string propertyName)
	{
		switch (propertyName)
		{
		case "Title.Text":
			return Title.Text;
		case "Legend.Title.Text":
			return Legend.Title.Text;
		case "XAxis.Labels.TextAfter":
			return XAxis.Labels.TextAfter;
		case "XAxis.Labels.TextBefore":
			return XAxis.Labels.TextBefore;
		case "XAxis.Title.Text":
			return XAxis.Title.Text;
		case "XTopAxis.Labels.TextAfter":
			return XTopAxis.Labels.TextAfter;
		case "XTopAxis.Labels.TextBefore":
			return XTopAxis.Labels.TextBefore;
		case "XTopAxis.Title.Text":
			return XTopAxis.Title.Text;
		case "YAxis.Labels.TextAfter":
			return YAxis.Labels.TextAfter;
		case "YAxis.Labels.TextBefore":
			return YAxis.Labels.TextBefore;
		case "YAxis.Title.Text":
			return YAxis.Title.Text;
		case "YRightAxis.Labels.TextAfter":
			return YRightAxis.Labels.TextAfter;
		case "YRightAxis.Labels.TextBefore":
			return YRightAxis.Labels.TextBefore;
		case "YRightAxis.Title.Text":
			return YRightAxis.Title.Text;
		default:
		{
			int num = 0;
			foreach (StiValueChartMeter value in Values)
			{
				string text = $"Value{num++}.Label";
				if (propertyName == text)
				{
					return value.Label;
				}
			}
			num = 0;
			foreach (StiArgumentChartMeter argument in Arguments)
			{
				string text2 = $"Argument{num++}.Label";
				if (propertyName == text2)
				{
					return argument.Label;
				}
			}
			throw new ArgumentException("Property with name " + propertyName);
		}
		}
	}

	string[] IStiGlobalizationProvider.GetAllStrings()
	{
		List<string> list = new List<string> { "Title.Text", "Legend.Title.Text" };
		if (IsAxisAreaChart)
		{
			list.Add("XAxis.Labels.TextAfter");
			list.Add("XAxis.Labels.TextBefore");
			list.Add("XAxis.Title.Text");
			list.Add("XTopAxis.Labels.TextAfter");
			list.Add("XTopAxis.Labels.TextBefore");
			list.Add("XTopAxis.Title.Text");
			list.Add("YAxis.Labels.TextAfter");
			list.Add("YAxis.Labels.TextBefore");
			list.Add("YAxis.Title.Text");
			list.Add("YRightAxis.Labels.TextAfter");
			list.Add("YRightAxis.Labels.TextBefore");
			list.Add("YRightAxis.Title.Text");
		}
		int num = 0;
		foreach (StiValueChartMeter value in Values)
		{
			list.Add($"Value{num++}.Label");
		}
		num = 0;
		foreach (StiArgumentChartMeter argument in Arguments)
		{
			list.Add($"Argument{num++}.Label");
		}
		return list.ToArray();
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
		fonts.Add(new StiFont(Labels.Font));
		fonts.Add(new StiFont(Legend.Labels.Font));
		fonts.Add(new StiFont(Legend.Title.Font));
		fonts.Add(new StiFont(XAxis.Labels.Font));
		fonts.Add(new StiFont(XAxis.Title.Font));
		fonts.Add(new StiFont(XTopAxis.Labels.Font));
		fonts.Add(new StiFont(XTopAxis.Title.Font));
		fonts.Add(new StiFont(YAxis.Labels.Font));
		fonts.Add(new StiFont(YAxis.Title.Font));
		fonts.Add(new StiFont(YRightAxis.Labels.Font));
		fonts.Add(new StiFont(YRightAxis.Title.Font));
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

	private bool ShouldSerializeLegend()
	{
		if (Legend != null)
		{
			return !Legend.IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeArea()
	{
		if (Area != null)
		{
			return !Area.IsDefault;
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

	private bool ShouldSerializeArgumentFormat()
	{
		return !(ArgumentFormat is StiGeneralFormatService);
	}

	private static StiFormatService GetValueFormatDefault()
	{
		return new StiNumberFormatService
		{
			DecimalDigits = 0,
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

	private bool ShouldSerializeOptions3d()
	{
		if (Options3D != null)
		{
			return !Options3D.IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeConstantLines()
	{
		if (ConstantLines != null)
		{
			return ConstantLines.Count > 0;
		}
		return true;
	}

	private bool ShouldSerializeTrendLines()
	{
		if (TrendLines != null)
		{
			return TrendLines.Count > 0;
		}
		return true;
	}

	private bool ShouldSerializeConditions()
	{
		if (ChartConditions != null)
		{
			return ChartConditions.Count > 0;
		}
		return true;
	}

	private bool ShouldSerializeMarker()
	{
		if (Marker != null)
		{
			return !Marker.IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeIcon()
	{
		return Icon.HasValue;
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
		return new StiChartElement();
	}

	public void CheckBrowsableProperties()
	{
		if (IsRange)
		{
			SetBrowsableProperty("Weights", browsableValue: false);
			SetBrowsableProperty("EndValues", browsableValue: true);
			SetBrowsableProperty("CloseValues", browsableValue: false);
			SetBrowsableProperty("LowValues", browsableValue: false);
			SetBrowsableProperty("HighValues", browsableValue: false);
		}
		else if (IsFinancial)
		{
			SetBrowsableProperty("Weights", browsableValue: false);
			SetBrowsableProperty("EndValues", browsableValue: false);
			SetBrowsableProperty("CloseValues", browsableValue: true);
			SetBrowsableProperty("LowValues", browsableValue: true);
			SetBrowsableProperty("HighValues", browsableValue: true);
		}
		else if (IsBubbleChart)
		{
			SetBrowsableProperty("Weights", browsableValue: true);
			SetBrowsableProperty("EndValues", browsableValue: false);
			SetBrowsableProperty("CloseValues", browsableValue: false);
			SetBrowsableProperty("LowValues", browsableValue: false);
			SetBrowsableProperty("HighValues", browsableValue: false);
		}
		else
		{
			SetBrowsableProperty("Weights", browsableValue: false);
			SetBrowsableProperty("EndValues", browsableValue: false);
			SetBrowsableProperty("CloseValues", browsableValue: false);
			SetBrowsableProperty("LowValues", browsableValue: false);
			SetBrowsableProperty("HighValues", browsableValue: false);
		}
	}

	private void SetBrowsableProperty(string strPropertyName, bool browsableValue)
	{
		PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(GetType())[strPropertyName];
		BrowsableAttribute browsableAttribute = (BrowsableAttribute)propertyDescriptor.Attributes[typeof(BrowsableAttribute)];
		FieldInfo field = browsableAttribute.GetType().GetField("browsable", BindingFlags.Instance | BindingFlags.NonPublic);
		if (field == null)
		{
			field = browsableAttribute.GetType().GetField("_browsable", BindingFlags.Instance | BindingFlags.NonPublic);
		}
		if (field == null)
		{
			field = browsableAttribute.GetType().GetField("<Browsable>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
		}
		if (field != null)
		{
			field.SetValue(browsableAttribute, browsableValue);
		}
	}

	public StiSeries GetChartSeries()
	{
		StiChartSeriesType type = StiChartSeriesType.ClusteredColumn;
		if (ChartValuesList().Any())
		{
			type = ChartValuesList().First().SeriesType;
		}
		return StiChartSeriesCreator.New(type);
	}

	internal List<StiValueChartMeter> ChartValuesList()
	{
		if (DataMode == StiDataMode.UsingDataFields)
		{
			if (Values == null)
			{
				return new List<StiValueChartMeter>();
			}
			return Values.Where((StiValueChartMeter v) => v != null).ToList();
		}
		if (ManuallyEnteredChartMeter != null)
		{
			return new List<StiValueChartMeter> { ManuallyEnteredChartMeter };
		}
		return new List<StiValueChartMeter>();
	}

	public StiChartElement()
		: this(RectangleD.Empty)
	{
	}

	public StiChartElement(RectangleD rect)
		: base(rect)
	{
		Title.Text = LocalizedName;
		DashboardInteraction = new StiChartDashboardInteraction();
		Layout = new StiElementLayout(this);
		Labels = new StiChartLabels();
		Legend = new StiChartLegend();
		Options3D = new StiPie3dOptions();
		PreviousAnimations = new List<StiAnimation>();
	}
}
