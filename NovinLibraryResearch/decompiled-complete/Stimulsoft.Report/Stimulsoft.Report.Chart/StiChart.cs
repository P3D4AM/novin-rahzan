using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;
using System.Threading;
using Stimulsoft.Base;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Chart.Design;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Chart;

[StiServiceBitmap(typeof(StiComponent), "Stimulsoft.Report.Images.Components.StiChart.png")]
[StiServiceCategoryBitmap(typeof(StiComponent), "Stimulsoft.Report.Images.Components.catInfographics.png")]
[StiToolbox(true)]
[StiDesigner("Stimulsoft.Report.Chart.Design.StiChartDesigner, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Report.WpfDesign.StiWpfChartDesigner, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfPainter("Stimulsoft.Report.Painters.StiChartWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiPainter(typeof(StiChartGdiPainter))]
[StiV1Builder(typeof(StiChartV1Builder))]
[StiV2Builder(typeof(StiChartV2Builder))]
[StiContextTool(typeof(IStiComponentDesigner))]
public class StiChart : StiComponent, IStiBorder, IStiBusinessObject, IStiEnumerator, IStiBrush, IStiDataSource, IStiDataRelation, IStiMasterComponent, IStiSort, IStiFilter, IStiExportImageExtended, IStiExportImage, IStiIgnoryStyle, IStiGlobalizationProvider, IStiProcessAtEnd, IStiChartComponent, IStiChart
{
	internal string jsonMasterComponentTemp;

	private string dataSourceName = string.Empty;

	private string businessObjectGuid = string.Empty;

	internal bool isEofValue;

	internal bool isBofValue;

	internal int positionValue;

	private bool isCacheValues;

	private int cachedCount;

	private bool cachedIsBusinessObjectEmpty;

	private bool cachedIsDataSourceEmpty;

	private StiDataSource cachedDataSource;

	private StiBusinessObject cachedBusinessObject;

	private StiSeriesCollection series;

	private IStiArea area;

	private IStiChartTable table;

	private IStiChartStyle style = new StiStyle29();

	private bool allowApplyStyle = true;

	private int horSpacing = 10;

	private int vertSpacing = 10;

	private IStiSeriesLabels seriesLabels;

	private IStiLegend legend;

	private IStiChartTitle title;

	private StiStripsCollection strips;

	private StiConstantLinesCollection constantLines;

	private bool isAnimationChangingValues = true;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiChart;

	public override string HelpUrl => "user-manual/index.html?report_internals_charts.htm";

	[Browsable(false)]
	public StiFilterEventHandler FilterMethodHandler { get; set; }

	[DefaultValue(StiFilterMode.And)]
	[StiSerializable]
	[TypeConverter(typeof(StiEnumConverter))]
	[Browsable(false)]
	public StiFilterMode FilterMode { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiFiltersCollectionConverter))]
	[StiOrder(170)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Components.Design.StiFiltersCollectionEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets the collection of data filters.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiFiltersCollection Filters { get; set; } = new StiFiltersCollection();


	[Browsable(false)]
	[StiNonSerialized]
	[Obsolete("Filter property is obsolete. Please use Filters property.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual StiExpression Filter
	{
		get
		{
			if (Filters.Count == 0)
			{
				Filters.Add(new StiFilter());
			}
			Filters[0].Item = StiFilterItem.Expression;
			return Filters[0].Expression;
		}
		set
		{
			if (Filters.Count == 0)
			{
				Filters.Add(new StiFilter());
			}
			Filters[0].Item = StiFilterItem.Expression;
			Filters[0].Expression = value;
		}
	}

	[TypeConverter(typeof(StiBoolConverter))]
	[StiSerializable]
	[DefaultValue(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(160)]
	[Description("Gets or sets value indicates, that filter is turn on.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool FilterOn { get; set; } = true;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[StiSerializable]
	[Description("The appearance and behavior of the component border.")]
	public StiBorder Border { get; set; } = new StiBorder();


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(130)]
	[StiSerializable]
	[Description("The brush, which is used to draw background.")]
	public StiBrush Brush { get; set; } = (StiBrush)new StiSolidBrush(Color.White);


	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiSortConverter))]
	[Editor("Stimulsoft.Report.Components.Design.StiSortEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(210)]
	[Description("Gets or sets the array of strings that describes rules of sorting.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual string[] Sort { get; set; } = new string[0];


	[TypeConverter(typeof(StiDataSourceConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(100)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Components.Design.StiDataSourceEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Get data source that is used for getting data.")]
	public virtual StiDataSource DataSource
	{
		get
		{
			if (isCacheValues)
			{
				return cachedDataSource;
			}
			if (Page == null || Report == null || Report.DataSources == null || DataSourceName == null || DataSourceName.Length == 0)
			{
				return null;
			}
			return Report.DataSources[DataSourceName];
		}
	}

	[Browsable(false)]
	[StiSerializable]
	[DefaultValue("")]
	public string DataSourceName
	{
		get
		{
			return dataSourceName;
		}
		set
		{
			if (dataSourceName != value)
			{
				dataSourceName = value;
				StiOptions.Engine.GlobalEvents.InvokeDataSourceAssigned(this, EventArgs.Empty);
			}
		}
	}

	[Browsable(false)]
	public bool IsDataSourceEmpty
	{
		get
		{
			if (isCacheValues)
			{
				return cachedIsDataSourceEmpty;
			}
			if (!string.IsNullOrEmpty(DataSourceName))
			{
				return DataSource == null;
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool IsBusinessObjectEmpty
	{
		get
		{
			if (isCacheValues)
			{
				return cachedIsBusinessObjectEmpty;
			}
			if (!string.IsNullOrEmpty(BusinessObjectGuid))
			{
				return BusinessObject == null;
			}
			return true;
		}
	}

	[TypeConverter(typeof(StiBusinessObjectConverter))]
	[Description("Get Business Object that is used for getting data.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(125)]
	[RefreshProperties(RefreshProperties.All)]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Components.Design.StiDataSourceEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual StiBusinessObject BusinessObject
	{
		get
		{
			if (isCacheValues)
			{
				return cachedBusinessObject;
			}
			if (Page == null || Report == null || BusinessObjectGuid == null || BusinessObjectGuid.Length == 0)
			{
				return null;
			}
			return StiBusinessObjectHelper.GetBusinessObjectFromGuid(Report, BusinessObjectGuid);
		}
	}

	[Browsable(false)]
	[StiSerializable]
	[RefreshProperties(RefreshProperties.All)]
	[DefaultValue("")]
	public string BusinessObjectGuid
	{
		get
		{
			return businessObjectGuid;
		}
		set
		{
			if (businessObjectGuid != value)
			{
				businessObjectGuid = value;
				if (!string.IsNullOrEmpty(value))
				{
					DataSourceName = null;
				}
				StiOptions.Engine.GlobalEvents.InvokeBusinessObjectAssigned(this, EventArgs.Empty);
			}
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiMasterComponentConverter))]
	[DefaultValue(null)]
	[Description("Gets or sets the master-component.")]
	[StiOrder(130)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Components.Design.StiMasterComponentEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual StiComponent MasterComponent { get; set; }

	[StiSerializable]
	[DefaultValue(0)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[StiOrder(140)]
	[Description("Gets or sets the count of rows for virtual data.")]
	public virtual int CountData { get; set; }

	[Browsable(false)]
	public virtual bool IsEof
	{
		get
		{
			if (!IsBusinessObjectEmpty)
			{
				return BusinessObject.IsEof;
			}
			if (!IsDataSourceEmpty)
			{
				return DataSource.IsEof;
			}
			return isEofValue;
		}
		set
		{
			if (!IsBusinessObjectEmpty)
			{
				BusinessObject.IsEof = value;
			}
			else if (!IsDataSourceEmpty)
			{
				DataSource.IsEof = value;
			}
			else
			{
				isEofValue = value;
			}
		}
	}

	[Browsable(false)]
	public virtual bool IsBof
	{
		get
		{
			if (!IsBusinessObjectEmpty)
			{
				return BusinessObject.IsBof;
			}
			if (!IsDataSourceEmpty)
			{
				return DataSource.IsBof;
			}
			return isBofValue;
		}
		set
		{
			if (!IsBusinessObjectEmpty)
			{
				BusinessObject.IsBof = value;
			}
			else if (!IsDataSourceEmpty)
			{
				DataSource.IsBof = value;
			}
			else
			{
				isBofValue = value;
			}
		}
	}

	[Browsable(false)]
	public virtual bool IsEmpty
	{
		get
		{
			if (!IsBusinessObjectEmpty)
			{
				return BusinessObject.IsEmpty;
			}
			if (!IsDataSourceEmpty)
			{
				return DataSource.IsEmpty;
			}
			return CountData == 0;
		}
	}

	[Browsable(false)]
	public virtual int Position
	{
		get
		{
			if (!IsBusinessObjectEmpty)
			{
				return BusinessObject.Position;
			}
			if (!IsDataSourceEmpty)
			{
				return DataSource.Position;
			}
			return positionValue;
		}
		set
		{
			if (!IsBusinessObjectEmpty)
			{
				BusinessObject.Position = value;
			}
			else if (!IsDataSourceEmpty)
			{
				DataSource.Position = value;
			}
			else
			{
				positionValue = value;
			}
		}
	}

	[Browsable(false)]
	public virtual int Count
	{
		get
		{
			if (isCacheValues)
			{
				return cachedCount;
			}
			if (!IsBusinessObjectEmpty)
			{
				return BusinessObject.Count;
			}
			if (!IsDataSourceEmpty)
			{
				return DataSource.Count;
			}
			return CountData;
		}
	}

	[TypeConverter(typeof(StiDataRelationConverter))]
	[Editor("Stimulsoft.Report.Components.Design.StiDataRelationEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[Description("Get link that is used for master-detail reports rendering.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiDataRelation DataRelation
	{
		get
		{
			if (Page == null || Report == null || Report.Dictionary == null || Report.Dictionary.Relations == null || DataRelationName == null || DataRelationName.Length == 0)
			{
				return null;
			}
			return Report.Dictionary.Relations[DataRelationName];
		}
	}

	[Browsable(false)]
	[StiSerializable]
	[DefaultValue("")]
	public string DataRelationName { get; set; } = "";


	[Browsable(false)]
	[StiNonSerialized]
	public override bool CanShrink
	{
		get
		{
			return base.CanShrink;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override bool CanGrow
	{
		get
		{
			return base.CanGrow;
		}
		set
		{
		}
	}

	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(160)]
	[Description("Gets or sets value indicates that a chart is processed at the end of the report execution.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool ProcessAtEnd { get; set; }

	public override int Priority => 0;

	public override string LocalizedCategory => StiLocalization.Get("Components", "StiChart");

	[Browsable(false)]
	public override RectangleD DefaultClientRectangle => new RectangleD(0.0, 0.0, 200.0, 200.0);

	public override StiComponentType ComponentType => StiComponentType.Simple;

	public override int ToolboxPosition => 200;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Components;

	public override string LocalizedName => StiLocalization.Get("Components", "StiChart");

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when getting the ProcessChart.")]
	public virtual StiProcessChartEvent ProcessChartEvent { get; set; } = new StiProcessChartEvent();


	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets collection of conditions which can be used to change behavior of series labels.")]
	[Browsable(false)]
	public virtual StiChartConditionsCollection SeriesLabelsConditions { get; set; } = new StiChartConditionsCollection();


	[StiNonSerialized]
	[Description("'ChartType' property is obsolete. Now chart type detected from first series in series collection.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(110)]
	[TypeConverter(typeof(StiAreaConverter))]
	[Browsable(false)]
	[Obsolete("'ChartType' property is obsolete. Now chart type detected from first series in series collection.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public IStiArea ChartType
	{
		get
		{
			return Area;
		}
		set
		{
			Area = value;
		}
	}

	[Browsable(false)]
	public override string ComponentStyle
	{
		get
		{
			return base.ComponentStyle;
		}
		set
		{
			base.ComponentStyle = value;
		}
	}

	[Browsable(false)]
	public bool IsDashboard { get; set; }

	[Browsable(false)]
	public StiChartCoreXF Core { get; set; }

	[Browsable(true)]
	[DefaultValue(StiImageRotation.None)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(170)]
	[Description("Gets or sets value which indicates how to rotate an chart before output.")]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiImageRotation Rotation { get; set; }

	[Browsable(false)]
	[DefaultValue(StiChartEditorType.Advanced)]
	[StiSerializable]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiChartEditorType EditorType { get; set; } = StiChartEditorType.Advanced;


	[Description("Gets list of series.")]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(180)]
	[Editor("Stimulsoft.Report.Chart.Design.StiChartSeriesEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiSeriesConverter))]
	public StiSeriesCollection Series
	{
		get
		{
			return series;
		}
		set
		{
			series = value;
			series.Chart = this;
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(100)]
	[Description("Gets area of the chart.")]
	[TypeConverter(typeof(StiUniversalConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public IStiArea Area
	{
		get
		{
			return area;
		}
		set
		{
			if (area != value)
			{
				if (value is StiRadarArea)
				{
					area = value;
				}
				area = value;
				if (value != null)
				{
					area.Chart = this;
				}
				else
				{
					area = null;
				}
			}
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(210)]
	[Description("Gets table of the chart.")]
	[TypeConverter(typeof(StiUniversalConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public IStiChartTable Table
	{
		get
		{
			return table;
		}
		set
		{
			if (table != value)
			{
				table = value;
				table.Chart = this;
			}
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(200)]
	[TypeConverter(typeof(StiChartStyleConverter))]
	[Browsable(false)]
	[Description("Gets or sets style of the chart.")]
	public IStiChartStyle Style
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
				if (value != null)
				{
					value.Core.Chart = this;
				}
			}
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(90)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that chart style will be used.")]
	[DefaultValue(true)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
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
				if (value)
				{
					Core.ApplyStyle(Style);
				}
			}
		}
	}

	[StiSerializable]
	[Browsable(false)]
	public string CustomStyleName { get; set; } = "";


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(130)]
	[DefaultValue(10)]
	[Description("Gets os sets horizontal space between border of the chart and the chart.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public int HorSpacing
	{
		get
		{
			return horSpacing;
		}
		set
		{
			if (value >= 0)
			{
				horSpacing = value;
			}
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(230)]
	[DefaultValue(10)]
	[Description("Gets os sets vertical space between border of the chart and the chart.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public int VertSpacing
	{
		get
		{
			return vertSpacing;
		}
		set
		{
			vertSpacing = value;
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiSeriesLabelsConverter))]
	[Browsable(false)]
	public IStiSeriesLabels SeriesLabels
	{
		get
		{
			return seriesLabels;
		}
		set
		{
			seriesLabels = value;
			if (value != null)
			{
				seriesLabels.Chart = this;
			}
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiNonSerialized]
	[StiOrder(140)]
	[Description("Gets or sets series labels.")]
	[TypeConverter(typeof(StiSeriesLabelsConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Chart.Design.StiChartLabelsEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public IStiSeriesLabels Labels
	{
		get
		{
			return SeriesLabels;
		}
		set
		{
			SeriesLabels = value;
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(150)]
	[Description("Gets or sets legend settings.")]
	[TypeConverter(typeof(StiUniversalConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public IStiLegend Legend
	{
		get
		{
			return legend;
		}
		set
		{
			legend = value;
			legend.Chart = this;
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(220)]
	[Description("Gets or sets chart title settings.")]
	[TypeConverter(typeof(StiUniversalConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public IStiChartTitle Title
	{
		get
		{
			return title;
		}
		set
		{
			if (title != value)
			{
				title = value;
				title.Chart = this;
			}
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiOrder(190)]
	[Description("Gets os sets strips settings of the chart.")]
	[Editor("Stimulsoft.Report.Chart.Design.StiChartStripsEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiStripsConverter))]
	public virtual StiStripsCollection Strips
	{
		get
		{
			return strips;
		}
		set
		{
			strips = value;
			strips.Chart = this;
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[Description("Gets os sets constant lines settings of the chart.")]
	[Editor("Stimulsoft.Report.Chart.Design.StiChartConstantLinesEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiConstantLinesConverter))]
	public virtual StiConstantLinesCollection ConstantLines
	{
		get
		{
			return constantLines;
		}
		set
		{
			constantLines = value;
			constantLines.Chart = this;
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value which indicates that the chart will be animated.")]
	[DefaultValue(false)]
	[Browsable(false)]
	public bool IsAnimation { get; set; }

	[StiSerializable]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value which indicates that the chart will be animated changing values.")]
	[DefaultValue(true)]
	[Browsable(false)]
	public bool IsAnimationChangingValues
	{
		get
		{
			return isAnimationChangingValues;
		}
		set
		{
			if (isAnimationChangingValues != value)
			{
				isAnimationChangingValues = value;
				IsAnimation = value;
			}
		}
	}

	[Browsable(false)]
	public List<StiAnimation> PreviousAnimations { get; set; }

	[Browsable(false)]
	public StiChartInfoV1 ChartInfoV1 { get; private set; } = new StiChartInfoV1();


	[Browsable(false)]
	public StiChartInfoV2 ChartInfoV2 { get; private set; } = new StiChartInfoV2();


	public event EventHandler ProcessChart;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "CanShrink");
		JsonExtensions.RemoveProperty(val, "CanGrow");
		JsonExtensions.AddPropertyBool(val, "AllowApplyStyle", AllowApplyStyle, true, false);
		JsonExtensions.AddPropertyEnum(val, "FilterMode", (Enum)FilterMode, (Enum)StiFilterMode.And);
		JsonExtensions.AddPropertyBool(val, "FilterOn", FilterOn, true, false);
		JsonExtensions.AddPropertyBorder(val, "Border", Border);
		JsonExtensions.AddPropertyBrush(val, "Brush", Brush);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "DataSourceName", DataSourceName);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "BusinessObjectGuid", BusinessObjectGuid);
		JsonExtensions.AddPropertyInt(val, "CountData", CountData, 0);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "DataRelationName", DataRelationName);
		JsonExtensions.AddPropertyBool(val, "ProcessAtEnd", ProcessAtEnd, false, false);
		JsonExtensions.AddPropertyJObject(val, "ProcessChartEvent", ProcessChartEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyEnum(val, "Rotation", (Enum)Rotation, (Enum)StiImageRotation.None);
		JsonExtensions.AddPropertyEnum(val, "EditorType", (Enum)EditorType, (Enum)StiChartEditorType.Advanced);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "CustomStyleName", CustomStyleName);
		JsonExtensions.AddPropertyInt(val, "HorSpacing", HorSpacing, 10);
		JsonExtensions.AddPropertyInt(val, "VertSpacing", VertSpacing, 10);
		if (MasterComponent != null)
		{
			JsonExtensions.AddPropertyStringNullOrEmpty(val, "MasterComponent", MasterComponent.Name);
		}
		JsonExtensions.AddPropertyJObject(val, "Series", series.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "Area", ((IStiJsonReportObject)area).SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "Table", ((IStiJsonReportObject)Table).SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "SeriesLabels", ((IStiJsonReportObject)SeriesLabels).SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "Legend", ((IStiJsonReportObject)Legend).SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "Title", ((IStiJsonReportObject)title).SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "Strips", Strips.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "ConstantLines", ConstantLines.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "Style", ((IStiJsonReportObject)Style).SaveToJsonObject(mode));
		if ((int)mode == 0)
		{
			JsonExtensions.AddPropertyJObject(val, "Filters", Filters.SaveToJsonObject(mode));
			JsonExtensions.AddPropertyStringArray(val, "Sort", Sort);
			JsonExtensions.AddPropertyJObject(val, "SeriesLabelsConditions", SeriesLabelsConditions.SaveToJsonObject(mode));
		}
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_067c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0686: Expected O, but got Unknown
		//IL_0661: Unknown result type (might be due to invalid IL or missing references)
		//IL_066b: Expected O, but got Unknown
		//IL_06ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b7: Expected O, but got Unknown
		//IL_062b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0635: Expected O, but got Unknown
		//IL_060b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0615: Expected O, but got Unknown
		//IL_0568: Unknown result type (might be due to invalid IL or missing references)
		//IL_0572: Expected O, but got Unknown
		//IL_0545: Unknown result type (might be due to invalid IL or missing references)
		//IL_054f: Expected O, but got Unknown
		//IL_049f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Expected O, but got Unknown
		//IL_06e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f3: Expected O, but got Unknown
		//IL_068f: Unknown result type (might be due to invalid IL or missing references)
		//IL_069a: Expected O, but got Unknown
		//IL_0646: Unknown result type (might be due to invalid IL or missing references)
		//IL_0650: Expected O, but got Unknown
		//IL_06c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ca: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "FilterMode":
				FilterMode = StiJsonReportObjectHelper.DeserializeEnum<StiFilterMode>(item);
				break;
			case "Sort":
				Sort = StiJsonReportObjectHelper.DeserializeStringArray(item);
				break;
			case "Filters":
				Filters.LoadFromJsonObject((JObject)item.Value);
				break;
			case "FilterOn":
				FilterOn = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Border":
				Border = StiJsonReportObjectHelper.DeserializeBorder(item);
				break;
			case "Brush":
				Brush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "DataSourceName":
				dataSourceName = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "BusinessObjectGuid":
				businessObjectGuid = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "CountData":
				CountData = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "DataRelationName":
				DataRelationName = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "ProcessAtEnd":
				ProcessAtEnd = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ProcessChartEvent":
			{
				StiProcessChartEvent stiProcessChartEvent = new StiProcessChartEvent();
				stiProcessChartEvent.LoadFromJsonObject((JObject)item.Value);
				ProcessChartEvent = stiProcessChartEvent;
				break;
			}
			case "SeriesLabelsConditions":
				SeriesLabelsConditions.LoadFromJsonObject((JObject)item.Value);
				break;
			case "Rotation":
				Rotation = StiJsonReportObjectHelper.DeserializeEnum<StiImageRotation>(item);
				break;
			case "EditorType":
				EditorType = StiJsonReportObjectHelper.DeserializeEnum<StiChartEditorType>(item);
				break;
			case "AllowApplyStyle":
				AllowApplyStyle = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "CustomStyleName":
				CustomStyleName = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "HorSpacing":
				horSpacing = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "VertSpacing":
				vertSpacing = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "MasterComponent":
				jsonMasterComponentTemp = StiJsonReportObjectHelper.DeserializeString(item);
				Report.jsonLoaderHelper.MasterComponents.Add(this);
				break;
			case "Style":
				Style = StiChartStyle.CreateFromJsonObject((JObject)item.Value);
				break;
			case "ConstantLines":
				ConstantLines.LoadFromJsonObject((JObject)item.Value);
				break;
			case "Strips":
				Strips.LoadFromJsonObject((JObject)item.Value);
				break;
			case "Title":
				((IStiJsonReportObject)Title).LoadFromJsonObject((JObject)item.Value);
				break;
			case "Legend":
				((IStiJsonReportObject)Legend).LoadFromJsonObject((JObject)item.Value);
				break;
			case "SeriesLabels":
				SeriesLabels = StiSeriesLabels.LoadFromJsonObjectInternal((JObject)item.Value, this);
				break;
			case "Table":
				((IStiJsonReportObject)table).LoadFromJsonObject((JObject)item.Value);
				break;
			case "Area":
				area = StiArea.CreateFromJsonObject((JObject)item.Value);
				area.Chart = this;
				break;
			case "Series":
				series.LoadFromJsonObject((JObject)item.Value);
				break;
			}
		}
	}

	public string SaveToString(StiJsonSaveMode mode)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			return ((JToken)SaveToJsonObject(mode)).ToString(new CultureInfo("en-us"));
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
	}

	internal void LoadFromString(string text)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			JObject jObject = (JObject)JsonConvert.DeserializeObject(text);
			LoadFromJsonObject(jObject);
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Invalid comparison between Unknown and I4
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Invalid comparison between Unknown and I4
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		stiPropertyCollection.Add(StiPropertyCategories.ComponentEditor, new StiPropertyObject[2]
		{
			propertiesHelper.ChartEditor(),
			propertiesHelper.DataSourceEditor()
		});
		stiPropertyCollection.Add(StiPropertyCategories.Chart, new StiPropertyObject[1] { propertiesHelper.AllowApplyStyle() });
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Position, new StiPropertyObject[4]
			{
				propertiesHelper.Left(),
				propertiesHelper.Top(),
				propertiesHelper.Width(),
				propertiesHelper.Height()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Position, new StiPropertyObject[6]
			{
				propertiesHelper.Left(),
				propertiesHelper.Top(),
				propertiesHelper.Width(),
				propertiesHelper.Height(),
				propertiesHelper.MinSize(),
				propertiesHelper.MaxSize()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[3]
			{
				propertiesHelper.Brush(),
				propertiesHelper.Border(),
				propertiesHelper.Conditions()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[4]
			{
				propertiesHelper.Brush(),
				propertiesHelper.Border(),
				propertiesHelper.Conditions(),
				propertiesHelper.UseParentStyles()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[2]
			{
				propertiesHelper.GrowToHeight(),
				propertiesHelper.Enabled()
			});
		}
		else if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[7]
			{
				propertiesHelper.InteractionEditor(),
				propertiesHelper.AnchorMode(),
				propertiesHelper.GrowToHeight(),
				propertiesHelper.DockStyle(),
				propertiesHelper.Enabled(),
				propertiesHelper.PrintOn(),
				propertiesHelper.ShiftMode()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[8]
			{
				propertiesHelper.InteractionEditor(),
				propertiesHelper.AnchorMode(),
				propertiesHelper.GrowToHeight(),
				propertiesHelper.DockStyle(),
				propertiesHelper.Enabled(),
				propertiesHelper.Printable(),
				propertiesHelper.PrintOn(),
				propertiesHelper.ShiftMode()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Design, new StiPropertyObject[1] { propertiesHelper.Name() });
		}
		else if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Design, new StiPropertyObject[2]
			{
				propertiesHelper.Name(),
				propertiesHelper.Alias()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Design, new StiPropertyObject[5]
			{
				propertiesHelper.Name(),
				propertiesHelper.Alias(),
				propertiesHelper.Restrictions(),
				propertiesHelper.Locked(),
				propertiesHelper.Linked()
			});
		}
		return stiPropertyCollection;
	}

	public override StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		StiEventCollection stiEventCollection = new StiEventCollection();
		stiEventCollection.Add(StiPropertyCategories.MouseEvents, new StiPropertyEventId[4]
		{
			StiPropertyEventId.ClickEvent,
			StiPropertyEventId.DoubleClickEvent,
			StiPropertyEventId.MouseEnterEvent,
			StiPropertyEventId.MouseLeaveEvent
		});
		stiEventCollection.Add(StiPropertyCategories.NavigationEvents, new StiPropertyEventId[4]
		{
			StiPropertyEventId.GetBookmarkEvent,
			StiPropertyEventId.GetDrillDownReportEvent,
			StiPropertyEventId.GetHyperlinkEvent,
			StiPropertyEventId.GetPointerEvent
		});
		stiEventCollection.Add(StiPropertyCategories.PrintEvents, new StiPropertyEventId[2]
		{
			StiPropertyEventId.AfterPrintEvent,
			StiPropertyEventId.BeforePrintEvent
		});
		stiEventCollection.Add(StiPropertyCategories.ValueEvents, new StiPropertyEventId[3]
		{
			StiPropertyEventId.GetTagEvent,
			StiPropertyEventId.GetToolTipEvent,
			StiPropertyEventId.ProcessChartEvent
		});
		return stiEventCollection;
	}

	public override void Convert(StiUnit oldUnit, StiUnit newUnit, bool isReportSnapshot = false)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		base.Convert(oldUnit, newUnit, isReportSnapshot);
		Legend.Size = newUnit.ConvertFromHInches(oldUnit.ConvertToHInches(Legend.Size));
	}

	public double ConvertToHInches(double value)
	{
		if (Report != null)
		{
			return Report.Unit.ConvertToHInches(value);
		}
		return value;
	}

	void IStiGlobalizationProvider.SetString(string propertyName, string value)
	{
		switch (propertyName)
		{
		case "Legend.Title":
			Legend.Title = value;
			break;
		case "SeriesLabels.TextBefore":
			SeriesLabels.TextBefore = value;
			break;
		case "SeriesLabels.TextAfter":
			SeriesLabels.TextAfter = value;
			break;
		}
		if (area is IStiAxisArea stiAxisArea)
		{
			switch (propertyName)
			{
			case "Area.XAxis.Labels.TextBefore":
				stiAxisArea.XAxis.Labels.TextBefore = value;
				break;
			case "Area.XAxis.Labels.TextAfter":
				stiAxisArea.XAxis.Labels.TextAfter = value;
				break;
			case "Area.XAxis.Title.Text":
				stiAxisArea.XAxis.Title.Text = value;
				break;
			case "Area.YAxis.Labels.TextBefore":
				stiAxisArea.YAxis.Labels.TextBefore = value;
				break;
			case "Area.YAxis.Labels.TextAfter":
				stiAxisArea.YAxis.Labels.TextAfter = value;
				break;
			case "Area.YAxis.Title.Text":
				stiAxisArea.YAxis.Title.Text = value;
				break;
			case "Area.XTopAxis.Labels.TextBefore":
				stiAxisArea.XTopAxis.Labels.TextBefore = value;
				break;
			case "Area.XTopAxis.Labels.TextAfter":
				stiAxisArea.XTopAxis.Labels.TextAfter = value;
				break;
			case "Area.XTopAxis.Title.Text":
				stiAxisArea.XTopAxis.Title.Text = value;
				break;
			case "Area.YRightAxis.Labels.TextBefore":
				stiAxisArea.YRightAxis.Labels.TextBefore = value;
				break;
			case "Area.YRightAxis.Labels.TextAfter":
				stiAxisArea.YRightAxis.Labels.TextAfter = value;
				break;
			case "Area.YRightAxis.Title.Text":
				stiAxisArea.YRightAxis.Title.Text = value;
				break;
			}
		}
		int num = 0;
		foreach (IStiSeries item in Series)
		{
			string text = $"Series{num}.Title";
			if (propertyName == text && item.CoreTitle != value)
			{
				item.CoreTitle = value;
			}
			string text2 = $"Series{num}.TopN.OtherText";
			if (propertyName == text2)
			{
				item.TopN.OthersText = value;
			}
			num++;
		}
	}

	string IStiGlobalizationProvider.GetString(string propertyName)
	{
		switch (propertyName)
		{
		case "Legend.Title":
			return Legend.Title;
		case "SeriesLabels.TextBefore":
			return SeriesLabels.TextBefore;
		case "SeriesLabels.TextAfter":
			return SeriesLabels.TextAfter;
		default:
		{
			if (area is IStiAxisArea stiAxisArea)
			{
				switch (propertyName)
				{
				case "Area.XAxis.Labels.TextBefore":
					return stiAxisArea.XAxis.Labels.TextBefore;
				case "Area.XAxis.Labels.TextAfter":
					return stiAxisArea.XAxis.Labels.TextAfter;
				case "Area.XAxis.Title.Text":
					return stiAxisArea.XAxis.Title.Text;
				case "Area.YAxis.Labels.TextBefore":
					return stiAxisArea.YAxis.Labels.TextBefore;
				case "Area.YAxis.Labels.TextAfter":
					return stiAxisArea.YAxis.Labels.TextAfter;
				case "Area.YAxis.Title.Text":
					return stiAxisArea.YAxis.Title.Text;
				case "Area.XTopAxis.Labels.TextBefore":
					return stiAxisArea.XTopAxis.Labels.TextBefore;
				case "Area.XTopAxis.Labels.TextAfter":
					return stiAxisArea.XTopAxis.Labels.TextAfter;
				case "Area.XTopAxis.Title.Text":
					return stiAxisArea.XTopAxis.Title.Text;
				case "Area.YRightAxis.Labels.TextBefore":
					return stiAxisArea.YRightAxis.Labels.TextBefore;
				case "Area.YRightAxis.Labels.TextAfter":
					return stiAxisArea.YRightAxis.Labels.TextAfter;
				case "Area.YRightAxis.Title.Text":
					return stiAxisArea.YRightAxis.Title.Text;
				}
			}
			int num = 0;
			foreach (IStiSeries item in Series)
			{
				string text = $"Series{num}.Title";
				if (propertyName == text)
				{
					return item.CoreTitle;
				}
				string text2 = $"Series{num}.TopN.OtherText";
				if (propertyName == text2)
				{
					return item.TopN.OthersText;
				}
				num++;
			}
			throw new ArgumentException("Property with name " + propertyName);
		}
		}
	}

	string[] IStiGlobalizationProvider.GetAllStrings()
	{
		List<string> list = new List<string> { "Legend.Title", "SeriesLabels.TextBefore", "SeriesLabels.TextAfter" };
		if (Area is IStiAxisArea)
		{
			list.Add("Area.XAxis.Labels.TextBefore");
			list.Add("Area.XAxis.Labels.TextAfter");
			list.Add("Area.XAxis.Title.Text");
			list.Add("Area.YAxis.Labels.TextBefore");
			list.Add("Area.YAxis.Labels.TextAfter");
			list.Add("Area.YAxis.Title.Text");
			list.Add("Area.XTopAxis.Labels.TextBefore");
			list.Add("Area.XTopAxis.Labels.TextAfter");
			list.Add("Area.XTopAxis.Title.Text");
			list.Add("Area.YRightAxis.Labels.TextBefore");
			list.Add("Area.YRightAxis.Labels.TextAfter");
			list.Add("Area.YRightAxis.Title.Text");
		}
		for (int i = 0; i < series.Count; i++)
		{
			list.Add($"Series{i}.Title");
			list.Add($"Series{i}.TopN.OtherText");
		}
		return list.ToArray();
	}

	public override object Clone()
	{
		StiChart chart = base.Clone() as StiChart;
		return AdditionalClone(chart);
	}

	public override object Clone(bool cloneProperties)
	{
		StiChart chart = (StiChart)base.Clone(cloneProperties);
		return AdditionalClone(chart);
	}

	private StiChart AdditionalClone(StiChart chart)
	{
		chart.ChartInfoV1 = ChartInfoV1.Clone() as StiChartInfoV1;
		chart.ChartInfoV2 = ChartInfoV2.Clone() as StiChartInfoV2;
		object obj = Brush.Clone();
		chart.Brush = (StiBrush)((obj is StiBrush) ? obj : null);
		object obj2 = Border.Clone();
		chart.Border = (StiBorder)((obj2 is StiBorder) ? obj2 : null);
		if (Sort != null)
		{
			chart.Sort = (string[])Sort.Clone();
		}
		else
		{
			chart.Sort = null;
		}
		if (Filters != null)
		{
			chart.Filters = Filters.Clone() as StiFiltersCollection;
		}
		else
		{
			chart.Filters = null;
		}
		if (chart.Title != null)
		{
			chart.Title = Title.Clone() as IStiChartTitle;
		}
		if (chart.Area != null)
		{
			chart.Area = Area.Clone() as IStiArea;
		}
		if (chart.SeriesLabels != null)
		{
			chart.SeriesLabels = SeriesLabels.Clone() as IStiSeriesLabels;
		}
		if (chart.Legend != null)
		{
			chart.Legend = Legend.Clone() as IStiLegend;
		}
		if (chart.Table != null)
		{
			chart.Table = Table.Clone() as IStiChartTable;
		}
		chart.Series = new StiSeriesCollection();
		chart.Series.SeriesAdded += Series_SeriesAdded;
		chart.Series.SeriesRemoved += Series_SeriesRemoved;
		foreach (IStiSeries item in series)
		{
			IStiSeries stiSeries2 = (IStiSeries)item.Clone();
			string value = (stiSeries2 as StiSeries).Title.Value;
			chart.Series.Add(stiSeries2);
			((StiSeries)stiSeries2).Title.Value = value;
		}
		chart.ConstantLines = new StiConstantLinesCollection();
		foreach (IStiConstantLines constantLine in constantLines)
		{
			chart.ConstantLines.Add((IStiConstantLines)constantLine.Clone());
		}
		chart.Strips = new StiStripsCollection();
		foreach (IStiStrips strip in strips)
		{
			chart.Strips.Add((IStiStrips)strip.Clone());
		}
		if (chart.Core != null)
		{
			chart.Core = Core.Clone() as StiChartCoreXF;
			chart.Core.Chart = chart;
		}
		return chart;
	}

	public override void SaveState(string stateName)
	{
		base.SaveState(stateName);
		base.States.PushInt(stateName, this, "positionValue", positionValue);
		base.States.PushBool(stateName, this, "isEofValue", isEofValue);
		base.States.PushBool(stateName, this, "isBofValue", isBofValue);
		if (DataSource != null)
		{
			DataSource.SaveState(stateName);
		}
		if (BusinessObject != null)
		{
			BusinessObject.SaveState(stateName);
			BusinessObject.CreateEnumerator();
			BusinessObject.specTotalsCalculation = true;
		}
	}

	public override void RestoreState(string stateName)
	{
		base.RestoreState(stateName);
		if (base.States.IsExist(stateName, this))
		{
			isBofValue = base.States.PopBool(stateName, this, "isBofValue");
			isEofValue = base.States.PopBool(stateName, this, "isEofValue");
			positionValue = base.States.PopInt(stateName, this, "positionValue");
		}
		if (DataSource != null)
		{
			DataSource.RestoreState(stateName);
		}
		if (BusinessObject != null)
		{
			BusinessObject.RestoreState(stateName);
		}
	}

	public virtual Image GetImage(ref float zoom)
	{
		return GetImage(ref zoom, StiExportFormat.None);
	}

	public virtual Image GetImage(ref float zoom, StiExportFormat format)
	{
		StiPainter painter = StiPainter.GetPainter(((object)this).GetType(), (StiGuiMode)0);
		return painter.GetImage(this, ref zoom, format);
	}

	[Browsable(false)]
	public override bool IsExportAsImage(StiExportFormat format)
	{
		return format switch
		{
			StiExportFormat.Pdf => false, 
			StiExportFormat.ImageSvg => false, 
			_ => true, 
		};
	}

	private bool ShouldSerializeFilters()
	{
		if (Filters != null)
		{
			return Filters.Count > 0;
		}
		return true;
	}

	private bool ShouldSerializeBorder()
	{
		if (Border != null)
		{
			return !Border.IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeSort()
	{
		if (Sort != null)
		{
			return Sort.Length != 0;
		}
		return true;
	}

	public virtual void First()
	{
		if (!IsBusinessObjectEmpty)
		{
			BusinessObject.First();
			return;
		}
		if (!IsDataSourceEmpty)
		{
			DataSource.First();
			return;
		}
		isEofValue = false;
		isBofValue = true;
		positionValue = 0;
	}

	public virtual void Prior()
	{
		if (!IsBusinessObjectEmpty)
		{
			BusinessObject.Prior();
			return;
		}
		if (!IsDataSourceEmpty)
		{
			DataSource.Prior();
			return;
		}
		isBofValue = false;
		isEofValue = false;
		if (positionValue <= 0)
		{
			isBofValue = true;
		}
		else
		{
			positionValue--;
		}
	}

	public virtual void Next()
	{
		if (!IsBusinessObjectEmpty)
		{
			BusinessObject.Next();
			return;
		}
		if (!IsDataSourceEmpty)
		{
			DataSource.Next();
			return;
		}
		isBofValue = false;
		isEofValue = false;
		if (positionValue >= CountData - 1)
		{
			isEofValue = true;
		}
		else
		{
			positionValue++;
		}
	}

	public virtual void Last()
	{
		if (!IsBusinessObjectEmpty)
		{
			BusinessObject.Last();
			return;
		}
		if (!IsDataSourceEmpty)
		{
			DataSource.Last();
			return;
		}
		isEofValue = true;
		isBofValue = false;
		positionValue = CountData - 1;
	}

	internal void CacheValues(bool cache)
	{
		if (cache)
		{
			cachedCount = Count;
			cachedIsBusinessObjectEmpty = IsBusinessObjectEmpty;
			cachedIsDataSourceEmpty = IsDataSourceEmpty;
			cachedDataSource = DataSource;
			cachedBusinessObject = BusinessObject;
		}
		else
		{
			cachedDataSource = null;
			cachedBusinessObject = null;
		}
		isCacheValues = cache;
	}

	public override List<StiFont> GetFonts()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		List<StiFont> fonts = base.GetFonts();
		fonts.Add(new StiFont(Title.Font));
		fonts.Add(new StiFont(Legend.Font));
		fonts.Add(new StiFont(Table.DataCells.Font));
		fonts.Add(new StiFont(Table.Header.Font));
		foreach (object constantLine in ConstantLines)
		{
			fonts.Add(new StiFont((constantLine as IStiConstantLines).Font));
		}
		foreach (object strip in Strips)
		{
			fonts.Add(new StiFont((strip as IStiStrips).Font));
		}
		if (Area is StiAxisArea stiAxisArea)
		{
			fonts.Add(new StiFont(stiAxisArea.XAxis.Labels.Font));
			fonts.Add(new StiFont(stiAxisArea.XAxis.Title.Font));
			fonts.Add(new StiFont(stiAxisArea.YAxis.Labels.Font));
			fonts.Add(new StiFont(stiAxisArea.YAxis.Title.Font));
			fonts.Add(new StiFont(stiAxisArea.XTopAxis.Labels.Font));
			fonts.Add(new StiFont(stiAxisArea.XTopAxis.Title.Font));
			fonts.Add(new StiFont(stiAxisArea.YRightAxis.Labels.Font));
			fonts.Add(new StiFont(stiAxisArea.YRightAxis.Title.Font));
		}
		if (Area is StiRadarArea stiRadarArea)
		{
			fonts.Add(new StiFont(stiRadarArea.XAxis.Labels.Font));
			fonts.Add(new StiFont(stiRadarArea.YAxis.Labels.Font));
		}
		foreach (object item in Series)
		{
			if (item is StiSeries stiSeries)
			{
				foreach (StiTrendLine trendLine in stiSeries.TrendLines)
				{
					fonts.Add(new StiFont(trendLine.Font));
				}
				if (stiSeries.SeriesLabels != null)
				{
					fonts.Add(new StiFont(stiSeries.SeriesLabels.Font));
				}
			}
			if (item is StiPictorialSeries)
			{
				fonts.Add(StiFontIconsHelper.GetFont());
			}
		}
		return fonts.Distinct().ToList();
	}

	public override StiEventsCollection GetEvents()
	{
		StiEventsCollection stiEventsCollection = base.GetEvents();
		if (ProcessChartEvent != null)
		{
			stiEventsCollection.Add(ProcessChartEvent);
		}
		return stiEventsCollection;
	}

	public override void InvokeEvents()
	{
		try
		{
			base.InvokeEvents();
			InvokeProcessChart(this, EventArgs.Empty);
		}
		catch (Exception ex)
		{
			StiLogService.Write(((object)this).GetType(), "DoEvents...ERROR");
			StiLogService.Write(((object)this).GetType(), ex);
			if (Report != null)
			{
				Report.WriteToReportRenderingMessages(Name + " " + ex.Message);
			}
		}
	}

	protected virtual void OnProcessChart(EventArgs e)
	{
	}

	public void InvokeProcessChart(object sender, EventArgs e)
	{
		try
		{
			OnProcessChart(e);
			this.ProcessChart?.Invoke(sender, e);
		}
		catch (Exception ex)
		{
			StiLogService.Write(((object)this).GetType(), "InvokeProcessChart...Warning");
			StiLogService.Write(((object)this).GetType(), ex.Message);
			if (Report != null)
			{
				Report.WriteToReportRenderingMessages(Name + " " + ex.Message);
			}
		}
	}

	private void Series_SeriesAdded(object sender, EventArgs e)
	{
		IStiSeries stiSeries = sender as IStiSeries;
		StiChart stiChart = stiSeries.Chart as StiChart;
		if (stiChart.Area == null || !stiChart.Area.Core.IsAcceptableSeries(stiSeries.GetType()))
		{
			stiChart.Area = StiActivator.CreateObject(stiSeries.GetDefaultAreaType()) as IStiArea;
			if (stiChart.SeriesLabels == null || !stiChart.Area.Core.IsAcceptableSeriesLabels(stiChart.SeriesLabels.GetType()))
			{
				stiChart.SeriesLabels = StiActivator.CreateObject(Area.GetDefaultSeriesLabelsType()) as IStiSeriesLabels;
			}
		}
		if (stiChart.Series.Count == 1 && stiChart.Series[0] is StiTreemapSeries)
		{
			stiChart.Legend.VertAlignment = StiLegendVertAlignment.TopOutside;
		}
		else if (stiChart.Series.Count == 1 && stiChart.Series[0] is StiPictorialSeries)
		{
			stiChart.Legend.VertAlignment = StiLegendVertAlignment.TopOutside;
			stiChart.Legend.HorAlignment = StiLegendHorAlignment.Center;
			stiChart.Legend.Columns = 1;
		}
		if (AllowApplyStyle)
		{
			stiChart.Core.ApplyStyle(stiChart.Style);
		}
	}

	private void Series_SeriesRemoved(object sender, EventArgs e)
	{
		IStiSeries stiSeries = sender as IStiSeries;
		StiChart stiChart = stiSeries.Chart as StiChart;
		if (stiChart.Series.Count == 0)
		{
			if (!(stiChart.Area is IStiClusteredColumnArea))
			{
				stiChart.Area = new StiClusteredColumnArea();
				stiChart.Area.Core.ApplyStyle(Style);
			}
			return;
		}
		if (stiChart.Area != null)
		{
			Type type = stiChart.Area.GetType();
			foreach (IStiSeries item in stiChart.Series)
			{
				Type defaultAreaType = item.GetDefaultAreaType();
				if (defaultAreaType == type)
				{
					return;
				}
			}
		}
		stiChart.Area = StiActivator.CreateObject(((StiSeries)stiChart.Series[0]).GetDefaultAreaType()) as IStiArea;
		if (AllowApplyStyle)
		{
			Core.ApplyStyle(stiChart.Style);
		}
	}

	public override StiComponent CreateNew()
	{
		return new StiChart();
	}

	public void ApplyStyle()
	{
		if (AllowApplyStyle)
		{
			Core.ApplyStyle(Style);
		}
	}

	public void SimplifyValues()
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < Series.Count; i++)
		{
			StiSeries stiSeries = Series[i] as StiSeries;
			if (stiSeries.Values.Length > 800)
			{
				List<PointD> shorterListPoints = StiChartHelper.GetShorterListPoints(stiSeries);
				stiSeries.Values = new double?[shorterListPoints.Count];
				stiSeries.Arguments = new object[shorterListPoints.Count];
				for (int j = 0; j < shorterListPoints.Count; j++)
				{
					double?[] values = stiSeries.Values;
					int num = j;
					PointD val = shorterListPoints[j];
					values[num] = ((PointD)(ref val)).X;
					object[] arguments = stiSeries.Arguments;
					int num2 = j;
					val = shorterListPoints[j];
					arguments[num2] = ((PointD)(ref val)).Y;
				}
			}
		}
	}

	public StiChart()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiChart(RectangleD rect)
		: base(rect)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		allowApplyStyle = StiOptions.Engine.DefaultValueOfAllowApplyStyleProperty;
		PlaceOnToolbox = true;
		Legend = new StiLegend();
		Series = new StiSeriesCollection();
		Series.SeriesAdded += Series_SeriesAdded;
		Series.SeriesRemoved += Series_SeriesRemoved;
		Area = new StiClusteredColumnArea();
		SeriesLabels = new StiCenterAxisLabels();
		ConstantLines = new StiConstantLinesCollection();
		Strips = new StiStripsCollection();
		Title = new StiChartTitle();
		Table = new StiChartTable();
		PreviousAnimations = new List<StiAnimation>();
		Core = new StiChartCoreXF(this);
		Core.ApplyStyle(Style);
	}

	public void DrawChart(Graphics g, RectangleF rect, bool useMargins, bool useBackground)
	{
		StiChartGdiPainter stiChartGdiPainter = StiPainter.GetPainter(typeof(StiChart), (StiGuiMode)0) as StiChartGdiPainter;
		stiChartGdiPainter.DrawChart(this, g, rect, useMargins, useBackground);
	}

	public void DrawChart(Graphics g, RectangleF rect, bool useMargins, bool useBackground, double zoom)
	{
		StiChartGdiPainter stiChartGdiPainter = StiPainter.GetPainter(typeof(StiChart), (StiGuiMode)0) as StiChartGdiPainter;
		stiChartGdiPainter.DrawChart(this, g, rect, useMargins, useBackground, zoom);
	}

	private bool ShouldSerializeSeries()
	{
		if (Series != null)
		{
			return Series.Count > 0;
		}
		return true;
	}

	private bool ShouldSerializeTable()
	{
		if (Table != null)
		{
			return !((IStiDefault)Table).IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeLegend()
	{
		if (Legend != null)
		{
			return !((IStiDefault)Legend).IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeTitle()
	{
		if (Title != null)
		{
			return !((IStiDefault)Title).IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeStrips()
	{
		if (Strips != null)
		{
			return Strips.Count > 0;
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

	public void RenderAtEnd()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		if (Report != null && (int)Report.EngineVersion == 1)
		{
			StiChartV1Builder.RenderAtEnd(this);
		}
		else
		{
			StiChartV2Builder.RenderAtEnd(this);
		}
	}
}
