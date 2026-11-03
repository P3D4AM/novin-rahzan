using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Chart.Design;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

[StiServiceBitmap(typeof(StiChartStyle), "Stimulsoft.Report.Images.Components.StiChart.png")]
[StiServiceCategoryBitmap(typeof(StiArea), "Stimulsoft.Report.Images.Components.StiChart.png")]
public abstract class StiSeries : StiService, IStiSeriesParent, IStiSeries, ICloneable, IStiJsonReportObject, IStiPropertyGridObject
{
	private StiShowSeriesLabels showSeriesLabels = StiShowSeriesLabels.FromChart;

	private IStiSeriesLabels seriesLabels;

	private IStiChart chart;

	private double?[] values = new double?[3] { 1.0, 3.0, 2.0 };

	private object[] arguments = new object[0];

	private string[] toolTips = new string[0];

	private object[] tags = new object[0];

	private string[] hyperlinks = new string[0];

	private IStiSeriesInteraction interaction;

	[Browsable(false)]
	public virtual StiComponentId ComponentId => StiComponentId.StiSeries;

	[Browsable(false)]
	public string PropName => string.Empty;

	[Browsable(false)]
	public StiComponent Parent => Chart as StiComponent;

	public override string ServiceName => Core.LocalizedName;

	[Browsable(false)]
	public sealed override string ServiceCategory => "Chart";

	[Browsable(false)]
	public sealed override Type ServiceType => typeof(StiSeries);

	[Browsable(false)]
	public StiSeriesCoreXF Core { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that chart style will be used.")]
	[DefaultValue(true)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool AllowApplyStyle { get; set; } = true;


	[DefaultValue("")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[Editor("Stimulsoft.Report.Chart.Design.StiFormatEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets string that is used to format series values.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public string Format { get; set; } = "";


	[DefaultValue(StiSeriesSortType.None)]
	[StiSerializable]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets mode of series values sorting.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiSeriesSortType SortBy { get; set; } = StiSeriesSortType.None;


	[DefaultValue(StiSeriesSortDirection.Ascending)]
	[StiSerializable]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets sort direction.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiSeriesSortDirection SortDirection { get; set; }

	[DefaultValue(true)]
	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets value which indicates that series must be shown in legend.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public bool ShowInLegend { get; set; } = true;


	[DefaultValue(true)]
	[Browsable(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Obsolete("Please use ShowSeriesLabels property instead ShowLabels property.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShowLabels
	{
		get
		{
			return true;
		}
		set
		{
			if (value)
			{
				ShowSeriesLabels = StiShowSeriesLabels.FromChart;
			}
		}
	}

	[DefaultValue(StiShowSeriesLabels.FromChart)]
	[StiSerializable]
	[TypeConverter(typeof(StiShowSeriesLabelsEnumConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets series labels output mode.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiShowSeriesLabels ShowSeriesLabels
	{
		get
		{
			return showSeriesLabels;
		}
		set
		{
			showSeriesLabels = ((value == StiShowSeriesLabels.None) ? StiShowSeriesLabels.FromChart : value);
		}
	}

	[DefaultValue(true)]
	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets value which indicates draw shadow or no.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public bool ShowShadow { get; set; } = true;


	[DefaultValue(StiFilterMode.And)]
	[StiSerializable]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets filter mode of series.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiFilterMode FilterMode { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets filters which used to filter series values.")]
	[Editor("Stimulsoft.Report.Chart.Design.StiChartFiltersEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiChartFiltersCollection Filters { get; set; } = new StiChartFiltersCollection();


	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiChartConditionsCollectionConverter))]
	[Description("Gets or sets collection of conditions which can be used to change behavior of series.")]
	[Editor("Stimulsoft.Report.Chart.Design.StiChartConditionsEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiChartConditionsCollection Conditions { get; set; } = new StiChartConditionsCollection();


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiSeriesTopNConverter))]
	[Description("Gets or sets parameters of displaying top results.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual IStiSeriesTopN TopN { get; set; } = new StiSeriesTopN();


	[StiNonSerialized]
	[Browsable(false)]
	public virtual bool TopNAllowed => true;

	[DefaultValue(StiSeriesYAxis.LeftYAxis)]
	[StiSerializable]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets Y Axis for series on which will output string representation of arguments.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiSeriesYAxis YAxis { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets series labels settings.")]
	[TypeConverter(typeof(StiSeriesLabelsConverter))]
	[Editor("Stimulsoft.Report.Chart.Design.StiChartLabelsEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public IStiSeriesLabels SeriesLabels
	{
		get
		{
			return seriesLabels;
		}
		set
		{
			seriesLabels = value;
			seriesLabels.Chart = Chart;
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets trend lines settings.")]
	[TypeConverter(typeof(StiTrendLinesCollectionConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Chart.Design.StiChartTrendLinesEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual StiTrendLinesCollection TrendLines { get; set; } = new StiTrendLinesCollection();


	[Browsable(false)]
	[StiNonSerialized]
	[Obsolete("TrendLine property is obsolete. Please use TrendLines property.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual IStiTrendLine TrendLine
	{
		get
		{
			if (TrendLines != null && TrendLines.Count > 0)
			{
				return TrendLines[0];
			}
			return new StiTrendLineNone();
		}
		set
		{
			TrendLines.Insert(0, value as StiTrendLine);
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public virtual bool TrendLineAllowed => true;

	[StiNonSerialized]
	[Browsable(false)]
	internal bool IsTotalLabel { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	public IStiChart Chart
	{
		get
		{
			return chart;
		}
		set
		{
			if (chart != value)
			{
				chart = value;
				if (value != null)
				{
					SeriesLabels.Chart = value;
				}
			}
		}
	}

	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	internal Color? ColorLegend { get; set; }

	[Browsable(false)]
	public double?[] ValuesStart { get; set; } = new double?[0];


	[Browsable(false)]
	public double?[] Values
	{
		get
		{
			if (Chart == null || !Chart.IsDesigning || IsDashboard)
			{
				return values;
			}
			int num = Chart.Series.IndexOf(this);
			double?[] array;
			if (!(this is IStiWaterfallSeries))
			{
				array = ((this is IStiBoxAndWhiskerSeries) ? new double?[5] { 8.0, 2.0, 3.0, 9.0, 10.0 } : ((this is IStiHistogramSeries) ? new double?[3] { 4.0, 2.0, 1.0 } : ((!string.IsNullOrEmpty(ListOfValues.Value) && !ListOfValues.Value.Contains("{")) ? GetNullableValuesFromString(this, ListOfValues.Value) : ((this is IStiSteppedRangeSeries) ? new double?[5]
				{
					1 + num * 3,
					3 + num * 4,
					2 + num * 3,
					3 + num * 3,
					3 + num * 3
				} : ((this is IStiFunnelSeries) ? new double?[3] { 3.0, 2.0, 1.0 } : ((this is IStiGanttSeries) ? new double?[3]
				{
					1 + num * 3,
					3 + num * 4,
					7 + num * 3
				} : ((this is IStiScatterSeries) ? new double?[3]
				{
					1 + num,
					6 + num,
					2 + num
				} : ((this is IStiPieSeries) ? ((num != 0) ? new double?[3]
				{
					1 + num,
					3 + num,
					4 + num
				} : new double?[3] { 1.0, 3.0, 2.0 }) : ((this is StiRadarSeries) ? new double?[5]
				{
					1 + num,
					2 + num,
					3 + num,
					4 + num,
					5 + num
				} : ((num != 0) ? new double?[3]
				{
					1 + num,
					3 + num,
					2.5 + (double)num
				} : values))))))))));
			}
			else
			{
				IStiWaterfallSeries stiWaterfallSeries = this as IStiWaterfallSeries;
				array = ((!stiWaterfallSeries.Total.Visible) ? new double?[3] { 3.0, -1.0, 2.0 } : new double?[4] { 3.0, -1.0, 2.0, 4.0 });
			}
			double offsetForValues = GetOffsetForValues();
			if (offsetForValues != 0.0)
			{
				double?[] array2 = new double?[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array2[i] = array[i] + offsetForValues;
				}
				return array2;
			}
			return array;
		}
		set
		{
			double?[] array = new double?[value.Length];
			for (int i = 0; i < value.Length; i++)
			{
				if (i < values.Length)
				{
					double?[] valuesStart = ValuesStart;
					if (valuesStart != null && valuesStart.Length != 0)
					{
						array[i] = values[i];
						continue;
					}
				}
				array[i] = 0.0;
			}
			ValuesStart = array;
			values = value;
		}
	}

	[DefaultValue("")]
	[StiSerializable]
	[StiOrder(100)]
	[Editor("Stimulsoft.Report.Components.Design.StiDataColumnEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a name of the column that contains the value.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual string ValueDataColumn { get; set; } = string.Empty;


	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public string ValuesString
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			double?[] array = values;
			foreach (double? num in array)
			{
				if (flag)
				{
					stringBuilder.AppendFormat("{0}", num ?? new double?(0.0));
				}
				else
				{
					stringBuilder.AppendFormat(";{0}", num ?? new double?(0.0));
				}
				flag = false;
			}
			return stringBuilder.ToString();
		}
		set
		{
			if (value == null || value.Trim().Length == 0)
			{
				values = new double?[0];
				return;
			}
			string[] array = value.Split(new char[1] { ';' });
			values = new double?[array.Length];
			int num = 0;
			string[] array2 = array;
			foreach (string s in array2)
			{
				values[num++] = double.Parse(s);
			}
		}
	}

	[Browsable(false)]
	public object[] OriginalArguments { get; set; }

	[Browsable(false)]
	public virtual object[] Arguments
	{
		get
		{
			if (Chart == null || !Chart.IsDesigning || IsDashboard)
			{
				return arguments;
			}
			if (this is IStiWaterfallSeries)
			{
				IStiWaterfallSeries stiWaterfallSeries = this as IStiWaterfallSeries;
				if (!stiWaterfallSeries.Total.Visible)
				{
					return new object[3] { "A", "B", "C" };
				}
				return new object[4]
				{
					"A",
					"B",
					"C",
					stiWaterfallSeries.Total.Text
				};
			}
			if (this is IStiHistogramSeries)
			{
				return new object[3] { "[1, 2]", "(2, 3]", "(3, 4]" };
			}
			if (!string.IsNullOrEmpty(ListOfArguments.Value))
			{
				return GetArgumentsFromString(ListOfArguments.Value);
			}
			if ((this is IStiTreemapSeries || this is IStiGanttSeries || this is IStiRangeBarSeries || this is IStiCandlestickSeries || this is StiSunburstSeries) && (arguments == null || arguments.Length == 0))
			{
				return new object[3] { "A", "B", "C" };
			}
			if ((this is IStiScatterSeries || this is IStiScatterLineSeries || this is IStiScatterSplineSeries) && (arguments == null || arguments.Length == 0))
			{
				return new object[3] { "1", "5", "4" };
			}
			return arguments;
		}
		set
		{
			arguments = value;
		}
	}

	[StiSerializable]
	[StiOrder(130)]
	[Editor("Stimulsoft.Report.Components.Design.StiDataColumnEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue("")]
	[Description("Gets or sets a name of the column that contains the argument value.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual string ArgumentDataColumn { get; set; } = string.Empty;


	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public string ArgumentsString
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			object[] array = arguments;
			foreach (object obj in array)
			{
				if (obj != null)
				{
					if (flag)
					{
						stringBuilder.AppendFormat("{0}", XmlConvert.EncodeName(obj.ToString()));
					}
					else
					{
						stringBuilder.AppendFormat(";{0}", XmlConvert.EncodeName(obj.ToString()));
					}
					flag = false;
				}
			}
			return stringBuilder.ToString();
		}
		set
		{
			if (value == null || value.Trim().Length == 0)
			{
				arguments = new object[0];
				return;
			}
			string[] array = value.Split(new char[1] { ';' });
			arguments = new object[array.Length];
			int num = 0;
			string[] array2 = array;
			foreach (string name in array2)
			{
				arguments[num++] = XmlConvert.DecodeName(name);
			}
		}
	}

	[StiSerializable]
	[StiOrder(290)]
	[DefaultValue("")]
	[Editor("Stimulsoft.Report.Components.Design.StiDataColumnEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a name of the column that contains the title of auto created series.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public string AutoSeriesTitleDataColumn { get; set; } = string.Empty;


	[StiSerializable]
	[Editor("Stimulsoft.Report.Components.Design.StiDataColumnEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(270)]
	[DefaultValue("")]
	[Description("Gets or sets a name of the column that contains the key of auto created series.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public string AutoSeriesKeyDataColumn { get; set; } = string.Empty;


	[StiSerializable]
	[Editor("Stimulsoft.Report.Components.Design.StiDataColumnEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(280)]
	[DefaultValue("")]
	[Description("Gets or sets a name of the column that contains the color of auto created series. Color must be presented as string.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public string AutoSeriesColorDataColumn { get; set; } = string.Empty;


	[Browsable(false)]
	public virtual string[] ToolTips
	{
		get
		{
			if (Chart == null || !Chart.IsDesigning || IsDashboard)
			{
				return toolTips;
			}
			if (!string.IsNullOrEmpty(ListOfToolTips.Value))
			{
				return GetStringsFromString(ListOfToolTips.Value);
			}
			return toolTips;
		}
		set
		{
			toolTips = value;
		}
	}

	[StiSerializable]
	[Browsable(false)]
	[StiOrder(150)]
	[Editor("Stimulsoft.Report.Components.Design.StiDataColumnEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue("")]
	[Description("Gets or sets a name of the column that contains the tool tip value.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public string ToolTipDataColumn { get; set; } = string.Empty;


	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public string ToolTipsString
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			string[] array = toolTips;
			foreach (object obj in array)
			{
				if (obj != null)
				{
					if (flag)
					{
						stringBuilder.AppendFormat("{0}", XmlConvert.EncodeName(obj.ToString()));
					}
					else
					{
						stringBuilder.AppendFormat(";{0}", XmlConvert.EncodeName(obj.ToString()));
					}
					flag = false;
				}
			}
			return stringBuilder.ToString();
		}
		set
		{
			if (value == null || value.Trim().Length == 0)
			{
				toolTips = new string[0];
				return;
			}
			string[] array = value.Split(new char[1] { ';' });
			toolTips = new string[array.Length];
			int num = 0;
			string[] array2 = array;
			foreach (string name in array2)
			{
				toolTips[num++] = XmlConvert.DecodeName(name);
			}
		}
	}

	[Browsable(false)]
	public virtual object[] Tags
	{
		get
		{
			if (Chart == null || !Chart.IsDesigning || IsDashboard)
			{
				return tags;
			}
			if (!string.IsNullOrEmpty(ListOfTags.Value))
			{
				return GetArgumentsFromString(ListOfTags.Value);
			}
			return tags;
		}
		set
		{
			tags = value;
		}
	}

	[StiSerializable]
	[Browsable(false)]
	[StiOrder(140)]
	[Editor("Stimulsoft.Report.Components.Design.StiDataColumnEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue("")]
	[Description("Gets or sets a name of the column that contains the tag value.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public string TagDataColumn { get; set; } = string.Empty;


	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public string TagString
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			object[] array = tags;
			foreach (object obj in array)
			{
				if (obj != null)
				{
					if (flag)
					{
						stringBuilder.AppendFormat("{0}", XmlConvert.EncodeName(obj.ToString()));
					}
					else
					{
						stringBuilder.AppendFormat(";{0}", XmlConvert.EncodeName(obj.ToString()));
					}
					flag = false;
				}
			}
			return stringBuilder.ToString();
		}
		set
		{
			if (value == null || value.Trim().Length == 0)
			{
				tags = new object[0];
				return;
			}
			string[] array = value.Split(new char[1] { ';' });
			tags = new object[array.Length];
			int num = 0;
			string[] array2 = array;
			foreach (string name in array2)
			{
				tags[num++] = XmlConvert.DecodeName(name);
			}
		}
	}

	[Browsable(false)]
	public virtual string[] Hyperlinks
	{
		get
		{
			if (Chart == null || !Chart.IsDesigning || IsDashboard)
			{
				return hyperlinks;
			}
			if (!string.IsNullOrEmpty(ListOfHyperlinks.Value))
			{
				return GetStringsFromString(ListOfHyperlinks.Value);
			}
			return hyperlinks;
		}
		set
		{
			hyperlinks = value;
		}
	}

	[StiSerializable]
	[Browsable(false)]
	[StiOrder(130)]
	[Editor("Stimulsoft.Report.Components.Design.StiDataColumnEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue("")]
	[Description("Gets or sets a name of the column that contains the hyperlink value.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public string HyperlinkDataColumn { get; set; } = string.Empty;


	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public string HyperlinkString
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			string[] array = hyperlinks;
			foreach (object obj in array)
			{
				if (obj != null)
				{
					if (flag)
					{
						stringBuilder.AppendFormat("{0}", XmlConvert.EncodeName(obj.ToString()));
					}
					else
					{
						stringBuilder.AppendFormat(";{0}", XmlConvert.EncodeName(obj.ToString()));
					}
					flag = false;
				}
			}
			return stringBuilder.ToString();
		}
		set
		{
			if (value == null || value.Trim().Length == 0)
			{
				hyperlinks = new string[0];
				return;
			}
			string[] array = value.Split(new char[1] { ';' });
			hyperlinks = new string[array.Length];
			int num = 0;
			string[] array2 = array;
			foreach (string name in array2)
			{
				hyperlinks[num++] = XmlConvert.DecodeName(name);
			}
		}
	}

	[StiSerializable]
	[Browsable(false)]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates whether the Drill-Down operation can be executed.")]
	[StiOrder(100)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool DrillDownEnabled { get; set; }

	[StiSerializable]
	[Browsable(false)]
	[DefaultValue("")]
	[Description("Gets or sets a path to a report for the Drill-Down operation.")]
	[StiOrder(120)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual string DrillDownReport { get; set; } = string.Empty;


	[StiNonSerialized]
	[Browsable(false)]
	[Editor("Stimulsoft.Report.Components.Design.StiInteractionDrillDownPageEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[DefaultValue(null)]
	[Description("Gets or sets a page for the Drill-Down operation.")]
	[StiOrder(110)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiPage DrillDownPage
	{
		get
		{
			if (Chart == null || ((StiChart)Chart).Report == null)
			{
				return null;
			}
			foreach (StiPage page in ((StiChart)Chart).Report.Pages)
			{
				if (page.Guid == DrillDownPageGuid)
				{
					return page;
				}
			}
			return null;
		}
		set
		{
			if (value == null)
			{
				DrillDownPageGuid = null;
				return;
			}
			if (value.Guid == null)
			{
				value.Guid = StiGuidUtils.NewGuid();
			}
			DrillDownPageGuid = value.Guid;
		}
	}

	[Browsable(false)]
	[StiSerializable]
	[DefaultValue(null)]
	public string DrillDownPageGuid { get; set; }

	[Browsable(false)]
	[StiSerializable]
	[DefaultValue(true)]
	public bool AllowSeries { get; set; } = true;


	[Browsable(false)]
	[StiSerializable]
	[DefaultValue(true)]
	public bool AllowSeriesElements { get; set; } = true;


	[Browsable(false)]
	public string CoreTitle
	{
		get
		{
			string text = ((Chart != null && Chart.IsDesigning) ? Title.Value : TitleValue);
			if (text == null)
			{
				text = string.Empty;
			}
			return text;
		}
		set
		{
			Title.Value = value;
			TitleValue = value;
		}
	}

	[Browsable(false)]
	internal bool IsDashboard { get; set; }

	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public bool IsDateTimeValues
	{
		get
		{
			return Core.IsDateTimeValues;
		}
		set
		{
			Core.IsDateTimeValues = value;
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets interaction options of this series.")]
	[TypeConverter(typeof(StiSeriesInteractionConverter))]
	[RefreshProperties(RefreshProperties.All)]
	public virtual IStiSeriesInteraction Interaction
	{
		get
		{
			return interaction;
		}
		set
		{
			if (interaction != value)
			{
				interaction = value;
				if (value != null)
				{
					((StiSeriesInteraction)interaction).ParentSeries = this;
				}
			}
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when new auto series is created.")]
	public StiNewAutoSeriesEvent NewAutoSeriesEvent { get; set; } = new StiNewAutoSeriesEvent();


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when getting the property Value.")]
	public StiGetValueEvent GetValueEvent { get; set; } = new StiGetValueEvent();


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when getting the list of values.")]
	public StiGetListOfValuesEvent GetListOfValuesEvent { get; set; } = new StiGetListOfValuesEvent();


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when getting the property Argument.")]
	public StiGetArgumentEvent GetArgumentEvent { get; set; } = new StiGetArgumentEvent();


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when getting the list of arguments.")]
	public StiGetListOfArgumentsEvent GetListOfArgumentsEvent { get; set; } = new StiGetListOfArgumentsEvent();


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when getting the property Title.")]
	public StiGetTitleEvent GetTitleEvent { get; set; } = new StiGetTitleEvent();


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when getting the ToolTip for the series.")]
	public virtual StiGetToolTipEvent GetToolTipEvent { get; set; } = new StiGetToolTipEvent();


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when getting the list of ToolTips.")]
	public StiGetListOfToolTipsEvent GetListOfToolTipsEvent { get; set; } = new StiGetListOfToolTipsEvent();


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when getting the Tag for the series.")]
	public virtual StiGetTagEvent GetTagEvent { get; set; } = new StiGetTagEvent();


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when getting the list of Tags.")]
	public StiGetListOfTagsEvent GetListOfTagsEvent { get; set; } = new StiGetListOfTagsEvent();


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when getting the Hyperlink for the series.")]
	public virtual StiGetHyperlinkEvent GetHyperlinkEvent { get; set; } = new StiGetHyperlinkEvent();


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when getting the list of Hyperlinks.")]
	public StiGetListOfHyperlinksEvent GetListOfHyperlinksEvent { get; set; } = new StiGetListOfHyperlinksEvent();


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(110)]
	[StiSerializable]
	[Description("Gets or sets point value expression. Example: {Order.Value}")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiExpression Value { get; set; } = new StiExpression();


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the expression to fill a list of values. Example: 1;2;3")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiListOfValuesExpression ListOfValues { get; set; } = new StiListOfValuesExpression();


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[StiOrder(140)]
	[Description("Gets or sets argument expression. Example: {Order.Argument}")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiArgumentExpression Argument { get; set; } = new StiArgumentExpression();


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(150)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the expression to fill a list of arguments.  Example: 1;2;3")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiListOfArgumentsExpression ListOfArguments { get; set; } = new StiListOfArgumentsExpression();


	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	public virtual string TitleValue { get; set; }

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets title of series.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiTitleExpression Title { get; set; } = new StiTitleExpression();


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[StiOrder(120)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the expression to fill a series tooltip.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiToolTipExpression ToolTip { get; set; } = new StiToolTipExpression();


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[StiOrder(230)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the expression to fill a list of tool tips. Example: 1;2;3")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiListOfToolTipsExpression ListOfToolTips { get; set; } = new StiListOfToolTipsExpression();


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[StiOrder(180)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the expression to fill a series tag.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiTagExpression Tag { get; set; } = new StiTagExpression();


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[StiOrder(220)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the expression to fill a list of tags. Example: 1;2;3")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiListOfTagsExpression ListOfTags { get; set; } = new StiListOfTagsExpression();


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[StiOrder(170)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the expression to fill a series hyperlink.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiHyperlinkExpression Hyperlink { get; set; } = new StiHyperlinkExpression();


	[Browsable(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(210)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the expression to fill a list of hyperlinks. Example: 1;2;3")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiListOfHyperlinksExpression ListOfHyperlinks { get; set; } = new StiListOfHyperlinksExpression();


	public event StiNewAutoSeriesEventHandler NewAutoSeries;

	public event StiGetValueEventHandler GetValue;

	public event StiGetValueEventHandler GetListOfValues;

	public event StiValueEventHandler GetArgument;

	public event StiGetValueEventHandler GetListOfArguments;

	public event StiGetTitleEventHandler GetTitle;

	public event StiValueEventHandler GetToolTip;

	public event StiGetValueEventHandler GetListOfToolTips;

	public event StiValueEventHandler GetTag;

	public event StiGetValueEventHandler GetListOfTags;

	public event StiValueEventHandler GetHyperlink;

	public event StiGetValueEventHandler GetListOfHyperlinks;

	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_0368: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0396: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0409: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_0437: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Unknown result type (might be due to invalid IL or missing references)
		//IL_0470: Unknown result type (might be due to invalid IL or missing references)
		//IL_0472: Invalid comparison between Unknown and I4
		JObject val = new JObject();
		JsonExtensions.AddPropertyIdent(val, "Ident", ((object)this).GetType().Name);
		JsonExtensions.AddPropertyBool(val, "AllowApplyStyle", AllowApplyStyle, true, false);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Format", Format);
		JsonExtensions.AddPropertyEnum(val, "SortBy", (Enum)SortBy, (Enum)StiSeriesSortType.None);
		JsonExtensions.AddPropertyEnum(val, "SortDirection", (Enum)SortDirection, (Enum)StiSeriesSortDirection.Ascending);
		JsonExtensions.AddPropertyBool(val, "ShowInLegend", ShowInLegend, true, false);
		JsonExtensions.AddPropertyEnum(val, "ShowSeriesLabels", (Enum)showSeriesLabels, (Enum)StiShowSeriesLabels.FromChart);
		JsonExtensions.AddPropertyBool(val, "ShowShadow", ShowShadow, true, false);
		JsonExtensions.AddPropertyEnum(val, "FilterMode", (Enum)FilterMode, (Enum)StiFilterMode.And);
		JsonExtensions.AddPropertyJObject(val, "Filters", Filters.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "Conditions", Conditions.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "TopN", ((IStiJsonReportObject)TopN).SaveToJsonObject(mode));
		JsonExtensions.AddPropertyEnum(val, "YAxis", (Enum)YAxis, (Enum)StiSeriesYAxis.LeftYAxis);
		JsonExtensions.AddPropertyJObject(val, "SeriesLabels", ((IStiJsonReportObject)seriesLabels).SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "TrendLines", TrendLines.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "ValueDataColumn", ValueDataColumn);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "ArgumentDataColumn", ArgumentDataColumn);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "AutoSeriesTitleDataColumn", AutoSeriesTitleDataColumn);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "AutoSeriesKeyDataColumn", AutoSeriesKeyDataColumn);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "AutoSeriesColorDataColumn", AutoSeriesColorDataColumn);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "ToolTipDataColumn", ToolTipDataColumn);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "TagDataColumn", TagDataColumn);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "HyperlinkDataColumn", HyperlinkDataColumn);
		JsonExtensions.AddPropertyBool(val, "DrillDownEnabled", DrillDownEnabled, false, false);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "DrillDownReport", DrillDownReport);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "DrillDownPageGuid", DrillDownPageGuid);
		JsonExtensions.AddPropertyBool(val, "AllowSeries", AllowSeries, true, false);
		JsonExtensions.AddPropertyBool(val, "AllowSeriesElements", AllowSeriesElements, true, false);
		JsonExtensions.AddPropertyJObject(val, "Interaction", ((IStiJsonReportObject)Interaction).SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "NewAutoSeriesEvent", NewAutoSeriesEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GetValueEvent", GetValueEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GetListOfValuesEvent", GetListOfValuesEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GetArgumentEvent", GetArgumentEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GetListOfArgumentsEvent", GetListOfArgumentsEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GetTitleEvent", GetTitleEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GetToolTipEvent", GetToolTipEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GetListOfToolTipsEvent", GetListOfToolTipsEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GetTagEvent", GetTagEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GetListOfTagsEvent", GetListOfTagsEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GetHyperlinkEvent", GetHyperlinkEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GetListOfHyperlinksEvent", GetListOfHyperlinksEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "Value", Value.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "ListOfValues", ListOfValues.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "Argument", Argument.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "ListOfArguments", ListOfArguments.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "Title", Title.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "ToolTip", ToolTip.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "ListOfToolTips", ListOfToolTips.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "Tag", Tag.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "ListOfTags", ListOfTags.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "Hyperlink", Hyperlink.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "ListOfHyperlinks", ListOfHyperlinks.SaveToJsonObject(mode));
		if ((int)mode == 1)
		{
			JsonExtensions.AddPropertyStringNullOrEmpty(val, "ValuesString", ValuesString);
			JsonExtensions.AddPropertyStringNullOrEmpty(val, "ArgumentsString", ArgumentsString);
			JsonExtensions.AddPropertyStringNullOrEmpty(val, "ToolTipsString", ToolTipsString);
			JsonExtensions.AddPropertyStringNullOrEmpty(val, "TagString", TagString);
			JsonExtensions.AddPropertyStringNullOrEmpty(val, "HyperlinkString", HyperlinkString);
			JsonExtensions.AddPropertyStringNullOrEmpty(val, "TitleValue", TitleValue);
		}
		return val;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
		//IL_0c0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c17: Expected O, but got Unknown
		//IL_0dd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ddf: Expected O, but got Unknown
		//IL_0b9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba5: Expected O, but got Unknown
		//IL_0b75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7f: Expected O, but got Unknown
		//IL_0c33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3d: Expected O, but got Unknown
		//IL_0d89: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d93: Expected O, but got Unknown
		//IL_0c7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c89: Expected O, but got Unknown
		//IL_09a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bc: Expected O, but got Unknown
		//IL_0ca5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0caf: Expected O, but got Unknown
		//IL_0e21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2b: Expected O, but got Unknown
		//IL_0be7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf1: Expected O, but got Unknown
		//IL_0b4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b59: Expected O, but got Unknown
		//IL_0b29: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b33: Expected O, but got Unknown
		//IL_0d63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d6d: Expected O, but got Unknown
		//IL_0b03: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0d: Expected O, but got Unknown
		//IL_094a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0954: Expected O, but got Unknown
		//IL_09ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f7: Expected O, but got Unknown
		//IL_0dfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e05: Expected O, but got Unknown
		//IL_0ccb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd5: Expected O, but got Unknown
		//IL_0bc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bcb: Expected O, but got Unknown
		//IL_0cf1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cfb: Expected O, but got Unknown
		//IL_0980: Unknown result type (might be due to invalid IL or missing references)
		//IL_098a: Expected O, but got Unknown
		//IL_09cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d7: Expected O, but got Unknown
		//IL_0e44: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4e: Expected O, but got Unknown
		//IL_0c59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c63: Expected O, but got Unknown
		//IL_0965: Unknown result type (might be due to invalid IL or missing references)
		//IL_096f: Expected O, but got Unknown
		//IL_0d17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d21: Expected O, but got Unknown
		//IL_0d3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d47: Expected O, but got Unknown
		//IL_0ae5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aef: Expected O, but got Unknown
		//IL_0daf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db9: Expected O, but got Unknown
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "AllowApplyStyle":
				AllowApplyStyle = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Format":
				Format = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "SortBy":
				SortBy = StiJsonReportObjectHelper.DeserializeEnum<StiSeriesSortType>(item);
				break;
			case "SortDirection":
				SortDirection = StiJsonReportObjectHelper.DeserializeEnum<StiSeriesSortDirection>(item);
				break;
			case "ShowInLegend":
				ShowInLegend = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ShowSeriesLabels":
				showSeriesLabels = StiJsonReportObjectHelper.DeserializeEnum<StiShowSeriesLabels>(item);
				break;
			case "ShowShadow":
				ShowShadow = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "FilterMode":
				FilterMode = StiJsonReportObjectHelper.DeserializeEnum<StiFilterMode>(item);
				break;
			case "Filters":
				Filters.LoadFromJsonObject((JObject)item.Value);
				break;
			case "Conditions":
				Conditions.LoadFromJsonObject((JObject)item.Value);
				break;
			case "TopN":
				((IStiJsonReportObject)TopN).LoadFromJsonObject((JObject)item.Value);
				break;
			case "YAxis":
				YAxis = StiJsonReportObjectHelper.DeserializeEnum<StiSeriesYAxis>(item);
				break;
			case "SeriesLabels":
				seriesLabels = StiSeriesLabels.LoadFromJsonObjectInternal((JObject)item.Value, (StiChart)chart);
				break;
			case "TrendLine":
				TrendLine = StiTrendLine.CreateFromJsonObject((JObject)item.Value);
				break;
			case "TrendLines":
				TrendLines.LoadFromJsonObject((JObject)item.Value);
				break;
			case "ValueDataColumn":
				ValueDataColumn = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "ArgumentDataColumn":
				ArgumentDataColumn = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "AutoSeriesTitleDataColumn":
				AutoSeriesTitleDataColumn = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "AutoSeriesKeyDataColumn":
				AutoSeriesKeyDataColumn = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "AutoSeriesColorDataColumn":
				AutoSeriesColorDataColumn = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "ToolTipDataColumn":
				ToolTipDataColumn = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "TagDataColumn":
				TagDataColumn = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "HyperlinkDataColumn":
				HyperlinkDataColumn = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "DrillDownEnabled":
				DrillDownEnabled = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "DrillDownReport":
				DrillDownReport = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "DrillDownPageGuid":
				DrillDownPageGuid = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "AllowSeries":
				AllowSeries = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "AllowSeriesElements":
				AllowSeriesElements = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Interaction":
				((IStiJsonReportObject)Interaction).LoadFromJsonObject((JObject)item.Value);
				break;
			case "NewAutoSeriesEvent":
			{
				StiNewAutoSeriesEvent stiNewAutoSeriesEvent = new StiNewAutoSeriesEvent();
				stiNewAutoSeriesEvent.LoadFromJsonObject((JObject)item.Value);
				NewAutoSeriesEvent = stiNewAutoSeriesEvent;
				break;
			}
			case "GetValueEvent":
			{
				StiGetValueEvent stiGetValueEvent = new StiGetValueEvent();
				stiGetValueEvent.LoadFromJsonObject((JObject)item.Value);
				GetValueEvent = stiGetValueEvent;
				break;
			}
			case "GetListOfValuesEvent":
			{
				StiGetListOfValuesEvent stiGetListOfValuesEvent = new StiGetListOfValuesEvent();
				stiGetListOfValuesEvent.LoadFromJsonObject((JObject)item.Value);
				GetListOfValuesEvent = stiGetListOfValuesEvent;
				break;
			}
			case "GetArgumentEvent":
			{
				StiGetArgumentEvent stiGetArgumentEvent = new StiGetArgumentEvent();
				stiGetArgumentEvent.LoadFromJsonObject((JObject)item.Value);
				GetArgumentEvent = stiGetArgumentEvent;
				break;
			}
			case "GetListOfArgumentsEvent":
			{
				StiGetListOfArgumentsEvent stiGetListOfArgumentsEvent = new StiGetListOfArgumentsEvent();
				stiGetListOfArgumentsEvent.LoadFromJsonObject((JObject)item.Value);
				GetListOfArgumentsEvent = stiGetListOfArgumentsEvent;
				break;
			}
			case "GetTitleEvent":
			{
				StiGetTitleEvent stiGetTitleEvent = new StiGetTitleEvent();
				stiGetTitleEvent.LoadFromJsonObject((JObject)item.Value);
				GetTitleEvent = stiGetTitleEvent;
				break;
			}
			case "GetToolTipEvent":
			{
				StiGetToolTipEvent stiGetToolTipEvent = new StiGetToolTipEvent();
				stiGetToolTipEvent.LoadFromJsonObject((JObject)item.Value);
				GetToolTipEvent = stiGetToolTipEvent;
				break;
			}
			case "GetListOfToolTipsEvent":
			{
				StiGetListOfToolTipsEvent stiGetListOfToolTipsEvent = new StiGetListOfToolTipsEvent();
				stiGetListOfToolTipsEvent.LoadFromJsonObject((JObject)item.Value);
				GetListOfToolTipsEvent = stiGetListOfToolTipsEvent;
				break;
			}
			case "GetTagEvent":
			{
				StiGetTagEvent stiGetTagEvent = new StiGetTagEvent();
				stiGetTagEvent.LoadFromJsonObject((JObject)item.Value);
				GetTagEvent = stiGetTagEvent;
				break;
			}
			case "GetListOfTagsEvent":
			{
				StiGetListOfTagsEvent stiGetListOfTagsEvent = new StiGetListOfTagsEvent();
				stiGetListOfTagsEvent.LoadFromJsonObject((JObject)item.Value);
				GetListOfTagsEvent = stiGetListOfTagsEvent;
				break;
			}
			case "GetHyperlinkEvent":
			{
				StiGetHyperlinkEvent stiGetHyperlinkEvent = new StiGetHyperlinkEvent();
				stiGetHyperlinkEvent.LoadFromJsonObject((JObject)item.Value);
				GetHyperlinkEvent = stiGetHyperlinkEvent;
				break;
			}
			case "GetListOfHyperlinksEvent":
			{
				StiGetListOfHyperlinksEvent stiGetListOfHyperlinksEvent = new StiGetListOfHyperlinksEvent();
				stiGetListOfHyperlinksEvent.LoadFromJsonObject((JObject)item.Value);
				GetListOfHyperlinksEvent = stiGetListOfHyperlinksEvent;
				break;
			}
			case "Value":
			{
				StiExpression stiExpression = new StiExpression();
				stiExpression.LoadFromJsonObject((JObject)item.Value);
				Value = stiExpression;
				break;
			}
			case "ListOfValues":
			{
				StiListOfValuesExpression stiListOfValuesExpression = new StiListOfValuesExpression();
				stiListOfValuesExpression.LoadFromJsonObject((JObject)item.Value);
				ListOfValues = stiListOfValuesExpression;
				break;
			}
			case "Argument":
			{
				StiArgumentExpression stiArgumentExpression = new StiArgumentExpression();
				stiArgumentExpression.LoadFromJsonObject((JObject)item.Value);
				Argument = stiArgumentExpression;
				break;
			}
			case "ListOfArguments":
			{
				StiListOfArgumentsExpression stiListOfArgumentsExpression = new StiListOfArgumentsExpression();
				stiListOfArgumentsExpression.LoadFromJsonObject((JObject)item.Value);
				ListOfArguments = stiListOfArgumentsExpression;
				break;
			}
			case "Title":
			{
				StiTitleExpression stiTitleExpression = new StiTitleExpression();
				stiTitleExpression.LoadFromJsonObject((JObject)item.Value);
				Title = stiTitleExpression;
				break;
			}
			case "ToolTip":
			{
				StiToolTipExpression stiToolTipExpression = new StiToolTipExpression();
				stiToolTipExpression.LoadFromJsonObject((JObject)item.Value);
				ToolTip = stiToolTipExpression;
				break;
			}
			case "ListOfToolTips":
			{
				StiListOfToolTipsExpression stiListOfToolTipsExpression = new StiListOfToolTipsExpression();
				stiListOfToolTipsExpression.LoadFromJsonObject((JObject)item.Value);
				ListOfToolTips = stiListOfToolTipsExpression;
				break;
			}
			case "Tag":
			{
				StiTagExpression stiTagExpression = new StiTagExpression();
				stiTagExpression.LoadFromJsonObject((JObject)item.Value);
				Tag = stiTagExpression;
				break;
			}
			case "ListOfTags":
			{
				StiListOfTagsExpression stiListOfTagsExpression = new StiListOfTagsExpression();
				stiListOfTagsExpression.LoadFromJsonObject((JObject)item.Value);
				ListOfTags = stiListOfTagsExpression;
				break;
			}
			case "Hyperlink":
			{
				StiHyperlinkExpression stiHyperlinkExpression = new StiHyperlinkExpression();
				stiHyperlinkExpression.LoadFromJsonObject((JObject)item.Value);
				Hyperlink = stiHyperlinkExpression;
				break;
			}
			case "ListOfHyperlinks":
			{
				StiListOfHyperlinksExpression stiListOfHyperlinksExpression = new StiListOfHyperlinksExpression();
				stiListOfHyperlinksExpression.LoadFromJsonObject((JObject)item.Value);
				ListOfHyperlinks = stiListOfHyperlinksExpression;
				break;
			}
			case "ValuesString":
				ValuesString = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "ArgumentsString":
				ArgumentsString = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "ToolTipsString":
				ToolTipsString = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "TagString":
				TagString = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "HyperlinkString":
				HyperlinkString = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "TitleValue":
				TitleValue = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			}
		}
	}

	public abstract StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level);

	public StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		StiEventCollection stiEventCollection = new StiEventCollection();
		StiPropertyEventId[] value = new StiPropertyEventId[1] { StiPropertyEventId.NewAutoSeriesEvent };
		stiEventCollection.Add(StiPropertyCategories.RenderEvents, value);
		value = new StiPropertyEventId[11]
		{
			StiPropertyEventId.GetValueEvent,
			StiPropertyEventId.GetListOfValuesEvent,
			StiPropertyEventId.GetArgumentEvent,
			StiPropertyEventId.GetListOfArgumentsEvent,
			StiPropertyEventId.GetTitleEvent,
			StiPropertyEventId.GetToolTipEvent,
			StiPropertyEventId.GetListOfToolTipsEvent,
			StiPropertyEventId.GetTagEvent,
			StiPropertyEventId.GetListOfTagsEvent,
			StiPropertyEventId.GetHyperlinkEvent,
			StiPropertyEventId.GetListOfHyperlinksEvent
		};
		stiEventCollection.Add(StiPropertyCategories.ValueEvents, value);
		return stiEventCollection;
	}

	public override object Clone()
	{
		StiSeries stiSeries = ((StiService)this).Clone() as StiSeries;
		stiSeries.Title = Title.Clone() as StiTitleExpression;
		stiSeries.Values = Values.Clone() as double?[];
		stiSeries.Arguments = Arguments.Clone() as object[];
		stiSeries.Tags = Tags.Clone() as object[];
		stiSeries.ToolTips = ToolTips.Clone() as string[];
		stiSeries.Hyperlinks = Hyperlinks.Clone() as string[];
		stiSeries.TopN = TopN.Clone() as StiSeriesTopN;
		stiSeries.Interaction = Interaction.Clone() as IStiSeriesInteraction;
		stiSeries.SeriesLabels = SeriesLabels.Clone() as IStiSeriesLabels;
		if (Core != null)
		{
			stiSeries.Core = Core.Clone() as StiSeriesCoreXF;
			stiSeries.Core.Series = stiSeries;
		}
		return stiSeries;
	}

	protected virtual void BaseTransform(object context, float x, float y, float angle, float dx, float dy)
	{
	}

	private bool ShouldSerializeFilters()
	{
		if (Filters != null)
		{
			return Filters.Count > 0;
		}
		return true;
	}

	private bool ShouldSerializeConditions()
	{
		if (Conditions != null)
		{
			return Conditions.Count > 0;
		}
		return true;
	}

	private bool ShouldSerializeTopN()
	{
		if (TopN != null)
		{
			return !((IStiDefault)TopN).IsDefault;
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

	protected double GetOffsetForValues()
	{
		double result = 0.0;
		if (Chart.Area is StiAxisArea stiAxisArea && !stiAxisArea.YAxis.Range.Auto)
		{
			result = stiAxisArea.YAxis.Range.Minimum;
		}
		return result;
	}

	internal double?[] GetApproximationValuesStart()
	{
		if (Chart.IsDesigning)
		{
			return ValuesStart;
		}
		if (Values == null || ValuesStart == null)
		{
			return ValuesStart;
		}
		double maximum = ((IStiAxisArea)chart.Area).YAxis.Info.Maximum;
		double min = 0.0;
		foreach (StiSeries item in chart.Series)
		{
			min = Math.Min(min, item.ValuesStart.ToList().Min().GetValueOrDefault());
		}
		double max = 0.0;
		foreach (StiSeries item2 in chart.Series)
		{
			max = Math.Max(max, item2.ValuesStart.ToList().Max().GetValueOrDefault());
		}
		((IStiAxisArea)chart.Area).AxisCore.CalculateMinimumAndMaximumYAxis(ref min, ref max);
		double num = ((maximum == 0.0 || max == 0.0) ? 0.0 : (maximum / max));
		double?[] array = new double?[ValuesStart.Length];
		for (int i = 0; i < ValuesStart.Length; i++)
		{
			double? num2 = ValuesStart[i];
			array[i] = num2.GetValueOrDefault() * num;
		}
		return array;
	}

	public Color ProcessSeriesColors(int pointIndex, Color seriesColor)
	{
		foreach (StiChartCondition condition in Conditions)
		{
			if (GetConditionResult(pointIndex, condition))
			{
				return condition.Color;
			}
		}
		return seriesColor;
	}

	public StiMarkerType ProcessSeriesMarkerType(int pointIndex, StiMarkerType markerType)
	{
		foreach (StiChartCondition condition in Conditions)
		{
			if (GetConditionResult(pointIndex, condition))
			{
				return condition.MarkerType;
			}
		}
		return markerType;
	}

	public float ProcessSeriesMarkerAngle(int pointIndex, float markerAngle)
	{
		foreach (StiChartCondition condition in Conditions)
		{
			if (GetConditionResult(pointIndex, condition))
			{
				return condition.MarkerAngle;
			}
		}
		return markerAngle;
	}

	public bool ProcessSeriesMarkerVisible(int pointIndex)
	{
		foreach (StiChartCondition condition in Conditions)
		{
			if (GetConditionResult(pointIndex, condition))
			{
				return true;
			}
		}
		return false;
	}

	public StiBrush ProcessSeriesBrushes(int pointIndex, StiBrush seriesBrush)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		foreach (StiChartCondition condition in Conditions)
		{
			if (!GetConditionResult(pointIndex, condition))
			{
				continue;
			}
			seriesBrush = (StiBrush)((!AllowApplyStyle || Chart.Style == null) ? ((object)new StiSolidBrush(condition.Color)) : ((object)Chart.Style.Core.GetColumnBrush(condition.Color)));
			if (Chart.Area is IStiClusteredBarArea)
			{
				if (seriesBrush is StiGradientBrush)
				{
					StiGradientBrush val = (StiGradientBrush)seriesBrush;
					val.Angle += 90.0;
				}
				if (seriesBrush is StiGlareBrush)
				{
					StiGlareBrush val2 = (StiGlareBrush)seriesBrush;
					val2.Angle += 90.0;
				}
			}
			return seriesBrush;
		}
		return seriesBrush;
	}

	private bool GetConditionResult(int pointIndex, StiChartCondition condition)
	{
		if ((Values == null || Values.Length <= pointIndex) && !(this is IStiFinancialSeries))
		{
			return false;
		}
		object itemValue = null;
		object itemValueEnd = null;
		object itemTooltip = null;
		object itemValueOpen = null;
		object itemValueClose = null;
		object itemValueLow = null;
		object itemValueHigh = null;
		object itemArgument = null;
		if ((chart.Area is IStiAxisArea && ((IStiAxisArea)chart.Area).ReverseHor && !(Chart.Area is IStiClusteredBarArea)) || (Chart.Area is IStiClusteredBarArea && ((IStiAxisArea)chart.Area).ReverseVert))
		{
			if (Values != null && pointIndex < Values.Length)
			{
				itemValue = Values[Values.Length - pointIndex - 1];
			}
			if (this is IStiRangeSeries && pointIndex < ((IStiRangeSeries)this).ValuesEnd.Length)
			{
				itemValueEnd = ((IStiRangeSeries)this).ValuesEnd[((IStiRangeSeries)this).ValuesEnd.Length - pointIndex - 1];
			}
			if (this is IStiFinancialSeries)
			{
				if (pointIndex < ((IStiFinancialSeries)this).ValuesOpen.Length)
				{
					itemValueOpen = ((IStiFinancialSeries)this).ValuesOpen[((IStiFinancialSeries)this).ValuesOpen.Length - pointIndex - 1];
				}
				if (pointIndex < ((IStiFinancialSeries)this).ValuesClose.Length)
				{
					itemValueClose = ((IStiFinancialSeries)this).ValuesClose[((IStiFinancialSeries)this).ValuesClose.Length - pointIndex - 1];
				}
				if (pointIndex < ((IStiFinancialSeries)this).ValuesLow.Length)
				{
					itemValueLow = ((IStiFinancialSeries)this).ValuesLow[((IStiFinancialSeries)this).ValuesLow.Length - pointIndex - 1];
				}
				if (pointIndex < ((IStiFinancialSeries)this).ValuesHigh.Length)
				{
					itemValueHigh = ((IStiFinancialSeries)this).ValuesHigh[((IStiFinancialSeries)this).ValuesHigh.Length - pointIndex - 1];
				}
			}
			if (Arguments != null && pointIndex < Arguments.Length)
			{
				itemArgument = Arguments[Arguments.Length - pointIndex - 1];
			}
			if (ToolTips != null && pointIndex < ToolTips.Length)
			{
				itemTooltip = ToolTips[ToolTips.Length - pointIndex - 1];
			}
		}
		else
		{
			if (Values != null && pointIndex < Values.Length)
			{
				itemValue = Values[pointIndex];
			}
			if (this is IStiRangeSeries && pointIndex < ((IStiRangeSeries)this).ValuesEnd.Length)
			{
				itemValueEnd = ((IStiRangeSeries)this).ValuesEnd[pointIndex];
			}
			if (this is IStiFinancialSeries)
			{
				if (pointIndex < ((IStiFinancialSeries)this).ValuesOpen.Length)
				{
					itemValueOpen = ((IStiFinancialSeries)this).ValuesOpen[pointIndex];
				}
				if (pointIndex < ((IStiFinancialSeries)this).ValuesClose.Length)
				{
					itemValueClose = ((IStiFinancialSeries)this).ValuesClose[pointIndex];
				}
				if (pointIndex < ((IStiFinancialSeries)this).ValuesLow.Length)
				{
					itemValueLow = ((IStiFinancialSeries)this).ValuesLow[pointIndex];
				}
				if (pointIndex < ((IStiFinancialSeries)this).ValuesHigh.Length)
				{
					itemValueHigh = ((IStiFinancialSeries)this).ValuesHigh[pointIndex];
				}
			}
			if (Arguments != null && pointIndex < Arguments.Length)
			{
				itemArgument = Arguments[pointIndex];
			}
			if (ToolTips != null && pointIndex < ToolTips.Length)
			{
				itemTooltip = ToolTips[pointIndex];
			}
		}
		object filterData = StiChartHelper.GetFilterData(null, condition, null);
		return StiChartHelper.GetFilterResult(condition, itemArgument, itemValue, itemValueEnd, itemValueOpen, itemValueClose, itemValueLow, itemValueHigh, itemTooltip, filterData);
	}

	public override string ToString()
	{
		return ((StiService)this).ServiceName;
	}

	internal static bool TryParseValue(string value, string culture, out double result)
	{
		CultureInfo cultureInfo = null;
		try
		{
			if (!string.IsNullOrEmpty(culture))
			{
				cultureInfo = Thread.CurrentThread.CurrentCulture;
				Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
			}
			return double.TryParse(value, out result);
		}
		finally
		{
			if (cultureInfo != null)
			{
				Thread.CurrentThread.CurrentCulture = cultureInfo;
			}
		}
	}

	public static double?[] GetNullableValuesFromString(StiSeries series, string list)
	{
		List<double?> list2 = new List<double?>();
		string[] array = list.Split(new char[1] { ';' });
		string[] array2 = array;
		foreach (string text in array2)
		{
			if (text == null || text.Trim().Length == 0)
			{
				continue;
			}
			double? item = 0.0;
			DateTime result2;
			if (TryParseValue(text, ((StiChart)series.Chart).Report.Culture, out var result))
			{
				item = result;
			}
			else if (!string.IsNullOrEmpty(((StiChart)series.Chart).Report.Culture))
			{
				try
				{
					CultureInfo provider = new CultureInfo(((StiChart)series.Chart).Report.Culture);
					if (DateTime.TryParse(text, provider, DateTimeStyles.None, out result2))
					{
						series.Core.IsDateTimeValues = true;
						item = result2.ToOADate();
					}
				}
				catch
				{
				}
			}
			else if (DateTime.TryParse(text, out result2))
			{
				series.Core.IsDateTimeValues = true;
				item = result2.ToOADate();
			}
			else if (DateTime.TryParse(text, new CultureInfo("en-US", useUserOverride: false), DateTimeStyles.None, out result2))
			{
				series.Core.IsDateTimeValues = true;
				item = result2.ToOADate();
			}
			list2.Add(item);
		}
		return list2.ToArray();
	}

	public static double[] GetValuesFromString(string list)
	{
		List<double> list2 = new List<double>();
		string[] array = list.Split(new char[1] { ';' });
		string[] array2 = array;
		foreach (string text in array2)
		{
			if (text != null && text.Trim().Length > 0)
			{
				double item = 0.0;
				try
				{
					item = (double)StiReport.ChangeType(text, typeof(double));
				}
				catch
				{
				}
				list2.Add(item);
			}
		}
		return list2.ToArray();
	}

	public static string[] GetStringsFromString(string list)
	{
		List<string> list2 = new List<string>();
		string[] array = list.Split(new char[1] { ';' });
		string[] array2 = array;
		foreach (string text in array2)
		{
			if (text != null && text.Trim().Length > 0)
			{
				list2.Add(text);
			}
		}
		return list2.ToArray();
	}

	public static object[] GetArgumentsFromString(string list)
	{
		return list.Split(new char[1] { ';' });
	}

	public virtual StiSeries CreateNew()
	{
		throw new NotImplementedException();
	}

	public abstract Type GetDefaultAreaType();

	public void InvokeNewAutoSeries(StiNewAutoSeriesEventArgs e)
	{
		this.NewAutoSeries?.Invoke(e.Series, e);
	}

	protected virtual void OnGetValue(StiGetValueEventArgs e)
	{
	}

	public virtual void InvokeGetValue(StiComponent sender, StiGetValueEventArgs e)
	{
		try
		{
			OnGetValue(e);
			if (sender.Report.CalculationMode == StiCalculationMode.Interpretation)
			{
				StiText component = new StiText
				{
					Name = "**ChartSeriesValue**",
					Page = sender.Report.Pages[0]
				};
				object obj = StiParser.ParseTextValue(Value.Value, component);
				e.Value = sender.Report.ToString(obj);
			}
			this.GetValue?.Invoke(sender, e);
			StiBlocklyHelper.InvokeBlockly(((StiChart)Chart).Report, sender, GetValueEvent, e);
		}
		catch (Exception ex)
		{
			string text = "Expression in GetValue property of '" + ((StiService)this).ServiceName + "' series from '" + ((StiChart)Chart).Name + "' chart can't be evaluated!";
			StiLogService.Write(((object)this).GetType(), text);
			StiLogService.Write(((object)this).GetType(), ex.Message);
			((StiChart)Chart).Report.WriteToReportRenderingMessages(text);
		}
	}

	protected virtual void OnGetListOfValues(StiGetValueEventArgs e)
	{
	}

	public void InvokeGetListOfValues(StiComponent sender, StiGetValueEventArgs e)
	{
		try
		{
			OnGetListOfValues(e);
			if (sender.Report.CalculationMode == StiCalculationMode.Interpretation)
			{
				StiText component = new StiText
				{
					Name = "**ChartSeriesListOfValues**",
					Page = sender.Report.Pages[0]
				};
				object obj = StiParser.ParseTextValue(ListOfValues, component);
				e.Value = sender.Report.ToString(obj);
			}
			this.GetListOfValues?.Invoke(sender, e);
		}
		catch (Exception ex)
		{
			string text = "Expression in GetListOfValues property of '" + ((StiService)this).ServiceName + "' series from '" + ((StiChart)Chart).Name + "' chart can't be evaluated!";
			StiLogService.Write(((object)this).GetType(), text);
			StiLogService.Write(((object)this).GetType(), ex.Message);
			((StiChart)Chart).Report.WriteToReportRenderingMessages(text);
		}
	}

	protected virtual void OnGetArgument(StiValueEventArgs e)
	{
	}

	public virtual void InvokeGetArgument(StiComponent sender, StiValueEventArgs e)
	{
		try
		{
			OnGetArgument(e);
			if (sender.Report.CalculationMode == StiCalculationMode.Interpretation)
			{
				StiText component = new StiText
				{
					Name = "**ChartSeriesArgument**",
					Page = sender.Report.Pages[0]
				};
				object obj = StiParser.ParseTextValue(Argument.Value, component);
				e.Value = (string.IsNullOrEmpty(Argument.Value) ? null : sender.Report.ToString(obj));
			}
			this.GetArgument?.Invoke(sender, e);
			StiBlocklyHelper.InvokeBlockly(((StiChart)Chart).Report, this, GetArgumentEvent, e);
		}
		catch (Exception ex)
		{
			string text = "Expression in GetArgument property of '" + ((StiService)this).ServiceName + "' series from '" + ((StiChart)Chart).Name + "' chart can't be evaluated!";
			StiLogService.Write(((object)this).GetType(), text);
			StiLogService.Write(((object)this).GetType(), ex.Message);
			((StiChart)Chart).Report.WriteToReportRenderingMessages(text);
		}
	}

	protected virtual void OnGetListOfArguments(StiGetValueEventArgs e)
	{
	}

	public void InvokeGetListOfArguments(StiComponent sender, StiGetValueEventArgs e)
	{
		try
		{
			OnGetListOfArguments(e);
			if (sender.Report.CalculationMode == StiCalculationMode.Interpretation)
			{
				StiText component = new StiText
				{
					Name = "**ChartSeriesListOfArguments**",
					Page = sender.Report.Pages[0]
				};
				object obj = StiParser.ParseTextValue(ListOfArguments, component);
				e.Value = sender.Report.ToString(obj);
			}
			this.GetListOfArguments?.Invoke(sender, e);
		}
		catch (Exception ex)
		{
			string text = "Expression in GetListOfArguments property of '" + ((StiService)this).ServiceName + "' series from '" + ((StiChart)Chart).Name + "' chart can't be evaluated!";
			StiLogService.Write(((object)this).GetType(), text);
			StiLogService.Write(((object)this).GetType(), ex.Message);
			((StiChart)Chart).Report.WriteToReportRenderingMessages(text);
		}
	}

	protected virtual void OnGetTitle(StiGetTitleEventArgs e)
	{
	}

	public virtual void InvokeGetTitle(StiComponent sender, StiGetTitleEventArgs e)
	{
		try
		{
			OnGetTitle(e);
			if (sender.Report.CalculationMode == StiCalculationMode.Interpretation)
			{
				StiText component = new StiText
				{
					Name = "**ChartSeriesTitle**",
					Page = sender.Report.Pages[0]
				};
				object obj = StiParser.ParseTextValue(Title.Value, component);
				e.Value = sender.Report.ToString(obj);
			}
			this.GetTitle?.Invoke(sender, e);
		}
		catch (Exception ex)
		{
			string text = "Expression in Title property of '" + ((StiService)this).ServiceName + "' series from '" + ((StiChart)Chart).Name + "' chart can't be evaluated!";
			StiLogService.Write(((object)this).GetType(), text);
			StiLogService.Write(((object)this).GetType(), ex.Message);
			((StiChart)Chart).Report.WriteToReportRenderingMessages(text);
		}
	}

	protected virtual void OnGetToolTip(StiValueEventArgs e)
	{
	}

	public void InvokeGetToolTip(object sender, StiValueEventArgs e)
	{
		try
		{
			OnGetToolTip(e);
			this.GetToolTip?.Invoke(sender, e);
			StiBlocklyHelper.InvokeBlockly(((StiChart)Chart).Report, sender, GetToolTipEvent, e);
		}
		catch (Exception ex)
		{
			string text = "Expression in GetToolTip property of '" + ((StiService)this).ServiceName + "' series from '" + ((StiChart)Chart).Name + "' chart can't be evaluated!";
			StiLogService.Write(((object)this).GetType(), text);
			StiLogService.Write(((object)this).GetType(), ex.Message);
			if (Chart != null && ((StiChart)Chart).Report != null)
			{
				((StiChart)Chart).Report.WriteToReportRenderingMessages(text);
			}
		}
	}

	protected virtual void OnGetListOfToolTips(StiGetValueEventArgs e)
	{
	}

	public void InvokeGetListOfToolTips(StiComponent sender, StiGetValueEventArgs e)
	{
		try
		{
			OnGetListOfToolTips(e);
			if (sender.Report.CalculationMode == StiCalculationMode.Interpretation)
			{
				StiText component = new StiText
				{
					Name = "**ChartSeriesListOfToolTips**",
					Page = sender.Report.Pages[0]
				};
				object obj = StiParser.ParseTextValue(ListOfToolTips, component);
				e.Value = sender.Report.ToString(obj);
			}
			this.GetListOfToolTips?.Invoke(sender, e);
		}
		catch (Exception ex)
		{
			string text = "Expression in GetListOfToolTips property of '" + ((StiService)this).ServiceName + "' series from '" + ((StiChart)Chart).Name + "' chart can't be evaluated!";
			StiLogService.Write(((object)this).GetType(), text);
			StiLogService.Write(((object)this).GetType(), ex.Message);
			((StiChart)Chart).Report.WriteToReportRenderingMessages(text);
		}
	}

	protected virtual void OnGetTag(StiValueEventArgs e)
	{
	}

	public void InvokeGetTag(StiComponent sender, StiValueEventArgs e)
	{
		try
		{
			OnGetTag(e);
			if (sender.Report.CalculationMode == StiCalculationMode.Interpretation)
			{
				StiText component = new StiText
				{
					Name = "**ChartSeriesTag**",
					Page = sender.Report.Pages[0]
				};
				object obj = StiParser.ParseTextValue(Tag.Value, component);
				e.Value = sender.Report.ToString(obj);
			}
			this.GetTag?.Invoke(sender, e);
			StiBlocklyHelper.InvokeBlockly(((StiChart)Chart).Report, sender, GetTagEvent, e);
		}
		catch (Exception ex)
		{
			string text = "Expression in GetTag property of '" + ((StiService)this).ServiceName + "' series from '" + ((StiChart)Chart).Name + "' chart can't be evaluated!";
			StiLogService.Write(((object)this).GetType(), text);
			StiLogService.Write(((object)this).GetType(), ex.Message);
			if (Chart != null && ((StiChart)Chart).Report != null)
			{
				((StiChart)Chart).Report.WriteToReportRenderingMessages(text);
			}
		}
	}

	protected virtual void OnGetListOfTags(StiGetValueEventArgs e)
	{
	}

	public void InvokeGetListOfTags(StiComponent sender, StiGetValueEventArgs e)
	{
		try
		{
			OnGetListOfTags(e);
			if (sender.Report.CalculationMode == StiCalculationMode.Interpretation)
			{
				StiText component = new StiText
				{
					Name = "**ChartSeriesListOfTag**",
					Page = sender.Report.Pages[0]
				};
				object obj = StiParser.ParseTextValue(ListOfTags, component);
				e.Value = sender.Report.ToString(obj);
			}
			this.GetListOfTags?.Invoke(sender, e);
		}
		catch (Exception ex)
		{
			string text = "Expression in GetListOfTags property of '" + ((StiService)this).ServiceName + "' series from '" + ((StiChart)Chart).Name + "' chart can't be evaluated!";
			StiLogService.Write(((object)this).GetType(), text);
			StiLogService.Write(((object)this).GetType(), ex.Message);
			((StiChart)Chart).Report.WriteToReportRenderingMessages(text);
		}
	}

	protected virtual void OnGetHyperlink(StiValueEventArgs e)
	{
	}

	public void InvokeGetHyperlink(object sender, StiValueEventArgs e)
	{
		try
		{
			OnGetHyperlink(e);
			this.GetHyperlink?.Invoke(sender, e);
			StiBlocklyHelper.InvokeBlockly(((StiChart)Chart).Report, sender, GetHyperlinkEvent, e);
		}
		catch (Exception ex)
		{
			string text = "Expression in GetHyperlink property of '" + ((StiService)this).ServiceName + "' series from '" + ((StiChart)Chart).Name + "' chart can't be evaluated!";
			StiLogService.Write(((object)this).GetType(), text);
			StiLogService.Write(((object)this).GetType(), ex.Message);
			if (Chart != null && ((StiChart)Chart).Report != null)
			{
				((StiChart)Chart).Report.WriteToReportRenderingMessages(text);
			}
		}
	}

	protected virtual void OnGetListOfHyperlinks(StiGetValueEventArgs e)
	{
	}

	public void InvokeGetListOfHyperlinks(StiComponent sender, StiGetValueEventArgs e)
	{
		try
		{
			OnGetListOfHyperlinks(e);
			this.GetListOfHyperlinks?.Invoke(sender, e);
		}
		catch (Exception ex)
		{
			string text = "Expression in GetListOfHyperlinks property of '" + ((StiService)this).ServiceName + "' series from '" + ((StiChart)Chart).Name + "' chart can't be evaluated!";
			StiLogService.Write(((object)this).GetType(), text);
			StiLogService.Write(((object)this).GetType(), ex.Message);
			((StiChart)Chart).Report.WriteToReportRenderingMessages(text);
		}
	}

	public StiSeries()
	{
		SeriesLabels = new StiCenterAxisLabels();
		Interaction = new StiSeriesInteraction();
	}
}
