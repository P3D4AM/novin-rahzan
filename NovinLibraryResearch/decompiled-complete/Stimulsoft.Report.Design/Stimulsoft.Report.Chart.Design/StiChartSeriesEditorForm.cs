using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Inplace;
using Stimulsoft.Report.Design.Panels;

namespace Stimulsoft.Report.Chart.Design;

public class StiChartSeriesEditorForm : StiInplaceToolForm
{
	private StiChart chart;

	private bool isInitSeriesIcons;

	private bool isSeriesChanged = true;

	private IContainer components;

	private StiGroupLine stiGroupLine1;

	private Button buttonCancel;

	private Button buttonOk;

	private StiSeriesListBox listBoxItems;

	private ButtonX buttonAddSeries;

	private ButtonItem categoryClusteredColumn;

	private ButtonItem categoryTreemap;

	private ButtonItem categorySunburst;

	private ButtonItem categoryHistogram;

	private ButtonItem categoryLine;

	private ButtonItem typeClusteredColumn;

	private ButtonItem typeHistogram;

	private ButtonItem typeStackedColumn;

	private ButtonItem typeFullStackedColumn;

	private ButtonItem typePareto;

	private ButtonItem typeLine;

	private ButtonItem typeStackedLine;

	private ButtonItem typeFullStackedLine;

	private ButtonItem typeSteppedLine;

	private ButtonItem typeSpline;

	private ButtonItem typeStackedSpline;

	private ButtonItem typeFullStackedSpline;

	private ButtonItem categoryArea;

	private ButtonItem typeArea;

	private ButtonItem typeStackedArea;

	private ButtonItem typeFullStackedArea;

	private ButtonItem typeTreemap;

	private ButtonItem typePictorial;

	private ButtonItem typeWaterfall;

	private ButtonItem typeBoxAndWhisker;

	private ButtonItem typeSunburst;

	private ButtonItem typeGantt;

	private ButtonItem typeSteppedArea;

	private ButtonItem categoryPie;

	private ButtonItem typeDoughnut;

	private ButtonItem categoryClusteredBar;

	private ButtonItem typeClusteredBar;

	private ButtonItem typeStackedBar;

	private ButtonItem typeFullStackedBar;

	private ButtonItem categoryScatter;

	private ButtonItem typeSplineArea;

	private ButtonItem typeStackedSplineArea;

	private ButtonItem typeFullStackedSplineArea;

	private ButtonItem typeScatter;

	private ButtonItem typeScatterLine;

	private ButtonItem typeScatterSpline;

	private ButtonItem typePie;

	private ButtonItem typePie3d;

	private ButtonItem categoryFunnel;

	private ButtonItem categoryPictorial;

	private ButtonItem typeBubble;

	private ButtonItem typeFunnel;

	private ButtonItem typeFunnelWeightedSlices;

	private ButtonItem categoryRadar;

	private ButtonItem typeRadarPoint;

	private ButtonItem typeRadarLine;

	private ButtonItem typeRadarArea;

	private ButtonItem categoryRange;

	private ButtonItem typeRange;

	private ButtonItem typeSplineRange;

	private ButtonItem typeSteppedRange;

	private ButtonItem typeRangeBar;

	private ButtonItem categoryFinancial;

	private ButtonItem typeCandlestick;

	private ButtonItem typeStock;

	private ButtonItem categoryWaterfall;

	private ButtonItem categoryBoxAndWhisker;

	public override bool AllowResize => false;

	private StiReport Report => chart?.Report;

	private StiDesignerControl Designer { get; set; }

	private StiPropertyGrid PropertyGrid { get; set; }

	public StiChartSeriesEditorForm(StiChart chart)
	{
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			this.chart = chart;
			Designer = Report?.Designer?.DesignerControl as StiDesignerControl;
			PropertyGrid = StiPropertiesPanelHelper.GetPropertyGrid(Designer);
			InitSeriesTypes();
			DoScaling();
			Build();
			Localize();
			((Form)this).ShowIcon = false;
			listBoxItems.Editor = this;
			StiPropertiesPanelHelper.ActivatePanel(Designer, chart.Series.ToList().FirstOrDefault());
			((PropertyGrid)PropertyGrid).PropertyValueChanged += new PropertyValueChangedEventHandler(PropertyGrid_PropertyValueChanged);
			StiPropertiesPanelService.GetService(Designer).RefreshLock = true;
		}
	}

	private void Build()
	{
		if (chart.Series == null)
		{
			return;
		}
		int num = 1;
		foreach (StiSeries item in chart.Series)
		{
			((ListBox)listBoxItems).Items.Add((object)item);
			num++;
		}
		if (((ListBox)listBoxItems).Items.Count > 0)
		{
			((ListControl)listBoxItems).SelectedIndex = 0;
		}
	}

	private void DoScaling()
	{
		((ListBox)listBoxItems).ItemHeight = StiScale.I(24);
	}

	private void InitSeriesTypes()
	{
		typeArea.Tag = typeof(StiAreaSeries);
		typeBubble.Tag = typeof(StiBubbleSeries);
		typeClusteredBar.Tag = typeof(StiClusteredBarSeries);
		typeClusteredColumn.Tag = typeof(StiClusteredColumnSeries);
		typeDoughnut.Tag = typeof(StiDoughnutSeries);
		typeFullStackedArea.Tag = typeof(StiFullStackedAreaSeries);
		typeFullStackedBar.Tag = typeof(StiFullStackedBarSeries);
		typePareto.Tag = typeof(StiParetoSeries);
		typeHistogram.Tag = typeof(StiHistogramSeries);
		typeFullStackedColumn.Tag = typeof(StiFullStackedColumnSeries);
		typeFullStackedLine.Tag = typeof(StiFullStackedLineSeries);
		typeFullStackedSpline.Tag = typeof(StiFullStackedSplineSeries);
		typeFullStackedSplineArea.Tag = typeof(StiFullStackedSplineAreaSeries);
		typeFunnel.Tag = typeof(StiFunnelSeries);
		typeFunnelWeightedSlices.Tag = typeof(StiFunnelWeightedSlicesSeries);
		typeCandlestick.Tag = typeof(StiCandlestickSeries);
		typeTreemap.Tag = typeof(StiTreemapSeries);
		typeSunburst.Tag = typeof(StiSunburstSeries);
		typeWaterfall.Tag = typeof(StiWaterfallSeries);
		typePictorial.Tag = typeof(StiPictorialSeries);
		typeGantt.Tag = typeof(StiGanttSeries);
		typeLine.Tag = typeof(StiLineSeries);
		typePie.Tag = typeof(StiPieSeries);
		typePie3d.Tag = typeof(StiPie3dSeries);
		typeRadarPoint.Tag = typeof(StiRadarPointSeries);
		typeRadarLine.Tag = typeof(StiRadarLineSeries);
		typeRadarArea.Tag = typeof(StiRadarAreaSeries);
		typeRange.Tag = typeof(StiRangeSeries);
		typeRangeBar.Tag = typeof(StiRangeBarSeries);
		typeScatter.Tag = typeof(StiScatterSeries);
		typeScatterLine.Tag = typeof(StiScatterLineSeries);
		typeScatterSpline.Tag = typeof(StiScatterSplineSeries);
		typeSpline.Tag = typeof(StiSplineSeries);
		typeSplineArea.Tag = typeof(StiSplineAreaSeries);
		typeSplineRange.Tag = typeof(StiSplineRangeSeries);
		typeStackedArea.Tag = typeof(StiStackedAreaSeries);
		typeStackedBar.Tag = typeof(StiStackedBarSeries);
		typeStackedColumn.Tag = typeof(StiStackedColumnSeries);
		typeStackedLine.Tag = typeof(StiStackedLineSeries);
		typeStackedSpline.Tag = typeof(StiStackedSplineSeries);
		typeStackedSplineArea.Tag = typeof(StiStackedSplineAreaSeries);
		typeSteppedArea.Tag = typeof(StiSteppedAreaSeries);
		typeSteppedLine.Tag = typeof(StiSteppedLineSeries);
		typeSteppedRange.Tag = typeof(StiSteppedRangeSeries);
		typeStock.Tag = typeof(StiStockSeries);
		typeBoxAndWhisker.Tag = typeof(StiBoxAndWhiskerSeries);
	}

	private void InitSeriesIcons()
	{
		if (!isInitSeriesIcons)
		{
			isInitSeriesIcons = true;
			StiResourcesHelper.SetImage(categoryClusteredColumn, "Stimulsoft.Report.Design.Resources.Office2013.Chart.categoryClusteredColumn.png");
			StiResourcesHelper.SetImage(typeClusteredColumn, "Stimulsoft.Report.Design.Resources.Office2013.Chart.ClusteredColumn.png");
			StiResourcesHelper.SetImage(typeStackedColumn, "Stimulsoft.Report.Design.Resources.Office2013.Chart.StackedColumn.png");
			StiResourcesHelper.SetImage(typeFullStackedColumn, "Stimulsoft.Report.Design.Resources.Office2013.Chart.FullStackedColumn.png");
			StiResourcesHelper.SetImage(categoryLine, "Stimulsoft.Report.Design.Resources.Office2013.Chart.categoryLine.png");
			StiResourcesHelper.SetImage(typeLine, "Stimulsoft.Report.Design.Resources.Office2013.Chart.Line.png");
			StiResourcesHelper.SetImage(typeStackedLine, "Stimulsoft.Report.Design.Resources.Office2013.Chart.StackedLine.png");
			StiResourcesHelper.SetImage(typeFullStackedLine, "Stimulsoft.Report.Design.Resources.Office2013.Chart.FullStackedLine.png");
			StiResourcesHelper.SetImage(typeSpline, "Stimulsoft.Report.Design.Resources.Office2013.Chart.Spline.png");
			StiResourcesHelper.SetImage(typeStackedSpline, "Stimulsoft.Report.Design.Resources.Office2013.Chart.StackedSpline.png");
			StiResourcesHelper.SetImage(typeFullStackedSpline, "Stimulsoft.Report.Design.Resources.Office2013.Chart.FullStackedSpline.png");
			StiResourcesHelper.SetImage(typeSteppedLine, "Stimulsoft.Report.Design.Resources.Office2013.Chart.SteppedLine.png");
			StiResourcesHelper.SetImage(categoryArea, "Stimulsoft.Report.Design.Resources.Office2013.Chart.categoryArea.png");
			StiResourcesHelper.SetImage(typeArea, "Stimulsoft.Report.Design.Resources.Office2013.Chart.Area.png");
			StiResourcesHelper.SetImage(typeStackedArea, "Stimulsoft.Report.Design.Resources.Office2013.Chart.StackedArea.png");
			StiResourcesHelper.SetImage(typeFullStackedArea, "Stimulsoft.Report.Design.Resources.Office2013.Chart.FullStackedArea.png");
			StiResourcesHelper.SetImage(typeSplineArea, "Stimulsoft.Report.Design.Resources.Office2013.Chart.SplineArea.png");
			StiResourcesHelper.SetImage(typeStackedSplineArea, "Stimulsoft.Report.Design.Resources.Office2013.Chart.StackedSplineArea.png");
			StiResourcesHelper.SetImage(typeFullStackedSplineArea, "Stimulsoft.Report.Design.Resources.Office2013.Chart.FullStackedSplineArea.png");
			StiResourcesHelper.SetImage(typeSteppedArea, "Stimulsoft.Report.Design.Resources.Office2013.Chart.SteppedArea.png");
			StiResourcesHelper.SetImage(categoryRange, "Stimulsoft.Report.Design.Resources.Office2013.Chart.categoryRange.png");
			StiResourcesHelper.SetImage(typeRange, "Stimulsoft.Report.Design.Resources.Office2013.Chart.Range.png");
			StiResourcesHelper.SetImage(typeSplineRange, "Stimulsoft.Report.Design.Resources.Office2013.Chart.SplineRange.png");
			StiResourcesHelper.SetImage(typeSteppedRange, "Stimulsoft.Report.Design.Resources.Office2013.Chart.SteppedRange.png");
			StiResourcesHelper.SetImage(typeRangeBar, "Stimulsoft.Report.Design.Resources.Office2013.Chart.RangeBar.png");
			StiResourcesHelper.SetImage(categoryClusteredBar, "Stimulsoft.Report.Design.Resources.Office2013.Chart.categoryClusteredBar.png");
			StiResourcesHelper.SetImage(typeClusteredBar, "Stimulsoft.Report.Design.Resources.Office2013.Chart.ClusteredBar.png");
			StiResourcesHelper.SetImage(typeStackedBar, "Stimulsoft.Report.Design.Resources.Office2013.Chart.StackedBar.png");
			StiResourcesHelper.SetImage(typeFullStackedBar, "Stimulsoft.Report.Design.Resources.Office2013.Chart.FullStackedBar.png");
			StiResourcesHelper.SetImage(categoryScatter, "Stimulsoft.Report.Design.Resources.Office2013.Chart.categoryScatter.png");
			StiResourcesHelper.SetImage(typeScatter, "Stimulsoft.Report.Design.Resources.Office2013.Chart.Scatter.png");
			StiResourcesHelper.SetImage(typeScatterLine, "Stimulsoft.Report.Design.Resources.Office2013.Chart.ScatterLine.png");
			StiResourcesHelper.SetImage(typeScatterSpline, "Stimulsoft.Report.Design.Resources.Office2013.Chart.ScatterSpline.png");
			StiResourcesHelper.SetImage(categoryPie, "Stimulsoft.Report.Design.Resources.Office2013.Chart.categoryPie.png");
			StiResourcesHelper.SetImage(typePie, "Stimulsoft.Report.Design.Resources.Office2013.Chart.categoryPie.png");
			StiResourcesHelper.SetImage(typePie3d, "Stimulsoft.Report.Design.Resources.Office2013.Chart.Pie3d.png");
			StiResourcesHelper.SetImage(typeDoughnut, "Stimulsoft.Report.Design.Resources.Office2013.Chart.categoryOther.png");
			StiResourcesHelper.SetImage(categoryRadar, "Stimulsoft.Report.Design.Resources.Office2013.Chart.RadarArea.png");
			StiResourcesHelper.SetImage(typeRadarPoint, "Stimulsoft.Report.Design.Resources.Office2013.Chart.RadarPoint.png");
			StiResourcesHelper.SetImage(typeRadarLine, "Stimulsoft.Report.Design.Resources.Office2013.Chart.RadarLine.png");
			StiResourcesHelper.SetImage(typeRadarArea, "Stimulsoft.Report.Design.Resources.Office2013.Chart.RadarArea.png");
			StiResourcesHelper.SetImage(categoryFunnel, "Stimulsoft.Report.Design.Resources.Office2013.Chart.Funnel.png");
			StiResourcesHelper.SetImage(typeFunnel, "Stimulsoft.Report.Design.Resources.Office2013.Chart.Funnel.png");
			StiResourcesHelper.SetImage(typeFunnelWeightedSlices, "Stimulsoft.Report.Design.Resources.Office2013.Chart.FunnelWeightedSlices.png");
			StiResourcesHelper.SetImage(categoryFinancial, "Stimulsoft.Report.Design.Resources.Office2013.Chart.categoryFinancial.png");
			StiResourcesHelper.SetImage(typeStock, "Stimulsoft.Report.Design.Resources.Office2013.Chart.Stock.png");
			StiResourcesHelper.SetImage(typeCandlestick, "Stimulsoft.Report.Design.Resources.Office2013.Chart.Candlestick.png");
			StiResourcesHelper.SetImage(typeGantt, "Stimulsoft.Report.Design.Resources.Office2013.Chart.Gantt.png");
			StiResourcesHelper.SetImage(typeBubble, "Stimulsoft.Report.Design.Resources.Office2013.Chart.Bubble.png");
			StiResourcesHelper.SetImage(categoryTreemap, "Stimulsoft.Report.Design.Resources.Office2013.Chart.Treemap.png");
			StiResourcesHelper.SetImage(typeTreemap, "Stimulsoft.Report.Design.Resources.Office2013.Chart.Treemap.png");
			StiResourcesHelper.SetImage(categorySunburst, "Stimulsoft.Report.Design.Resources.Office2013.Chart.Sunburst.png");
			StiResourcesHelper.SetImage(typeSunburst, "Stimulsoft.Report.Design.Resources.Office2013.Chart.Sunburst.png");
			StiResourcesHelper.SetImage(categoryHistogram, "Stimulsoft.Report.Design.Resources.Office2013.Chart.Histogram.png");
			StiResourcesHelper.SetImage(typePareto, "Stimulsoft.Report.Design.Resources.Office2013.Chart.Pareto.png");
			StiResourcesHelper.SetImage(typeHistogram, "Stimulsoft.Report.Design.Resources.Office2013.Chart.Histogram.png");
			StiResourcesHelper.SetImage(categoryWaterfall, "Stimulsoft.Report.Design.Resources.Office2013.Chart.Waterfall.png");
			StiResourcesHelper.SetImage(typeWaterfall, "Stimulsoft.Report.Design.Resources.Office2013.Chart.Waterfall.png");
			StiResourcesHelper.SetImage(categoryBoxAndWhisker, "Stimulsoft.Report.Design.Resources.Office2013.Chart.BoxAndWhisker.png");
			StiResourcesHelper.SetImage(typeBoxAndWhisker, "Stimulsoft.Report.Design.Resources.Office2013.Chart.BoxAndWhisker.png");
			StiResourcesHelper.SetImage(categoryPictorial, "Stimulsoft.Report.Design.Resources.Office2013.Chart.Pictorial.png");
			StiResourcesHelper.SetImage(typePictorial, "Stimulsoft.Report.Design.Resources.Office2013.Chart.Pictorial.png");
		}
	}

	public override void Localize()
	{
		((Control)this).Text = StiLocalization.Get("Chart", "SeriesEditorForm");
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)buttonAddSeries).Text = StiLocalization.Get("Chart", "AddSeries");
		((Control)buttonAddSeries).Width = TextRenderer.MeasureText(((Control)buttonAddSeries).Text, ((Control)buttonAddSeries).Font).Width + StiScaleUI.I(28);
		categoryClusteredColumn.Text = StiLocalization.Get("Chart", "ClusteredColumn");
		categoryFinancial.Text = StiLocalization.Get("Chart", "Financial");
		categoryLine.Text = StiLocalization.Get("Chart", "Line");
		categoryArea.Text = StiLocalization.Get("Chart", "Area");
		categoryClusteredBar.Text = StiLocalization.Get("Chart", "ClusteredBar");
		categoryScatter.Text = StiLocalization.Get("Chart", "Scatter");
		categoryRadar.Text = StiLocalization.Get("Chart", "Radar");
		categoryRange.Text = StiLocalization.Get("Chart", "Range");
		categoryPie.Text = StiLocalization.Get("Chart", "Pie");
		categoryFunnel.Text = StiLocalization.Get("Chart", "Funnel");
		categoryPictorial.Text = StiLocalization.Get("Chart", "Pictorial");
		categoryTreemap.Text = StiLocalization.Get("Chart", "Treemap");
		categorySunburst.Text = StiLocalization.Get("Chart", "Sunburst");
		categoryHistogram.Text = StiLocalization.Get("Chart", "Histogram");
		categoryWaterfall.Text = StiLocalization.Get("Chart", "Waterfall");
		categoryBoxAndWhisker.Text = StiLocalization.Get("Chart", "BoxAndWhisker");
		typeScatterLine.Text = StiLocalization.Get("Chart", "ScatterLine");
		typeScatterSpline.Text = StiLocalization.Get("Chart", "ScatterSpline");
		typeSpline.Text = StiLocalization.Get("Chart", "Spline");
		typeSplineArea.Text = StiLocalization.Get("Chart", "SplineArea");
		typeStackedArea.Text = StiLocalization.Get("Chart", "StackedArea");
		typeStackedBar.Text = StiLocalization.Get("Chart", "StackedBar");
		typeStackedColumn.Text = StiLocalization.Get("Chart", "StackedColumn");
		typeStackedLine.Text = StiLocalization.Get("Chart", "StackedLine");
		typeStackedSpline.Text = StiLocalization.Get("Chart", "StackedSpline");
		typeStackedSplineArea.Text = StiLocalization.Get("Chart", "StackedSplineArea");
		typeSteppedArea.Text = StiLocalization.Get("Chart", "SteppedArea");
		typeSteppedLine.Text = StiLocalization.Get("Chart", "SteppedLine");
		typeArea.Text = StiLocalization.Get("Chart", "Area");
		typeClusteredBar.Text = StiLocalization.Get("Chart", "ClusteredBar");
		typeClusteredColumn.Text = StiLocalization.Get("Chart", "ClusteredColumn");
		typeDoughnut.Text = StiLocalization.Get("Chart", "Doughnut");
		typeFullStackedArea.Text = StiLocalization.Get("Chart", "FullStackedArea");
		typeFullStackedBar.Text = StiLocalization.Get("Chart", "FullStackedBar");
		typeFullStackedColumn.Text = StiLocalization.Get("Chart", "FullStackedColumn");
		typePareto.Text = StiLocalization.Get("Chart", "Pareto");
		typeHistogram.Text = StiLocalization.Get("Chart", "Histogram");
		typeBoxAndWhisker.Text = StiLocalization.Get("Chart", "BoxAndWhisker");
		typeFullStackedLine.Text = StiLocalization.Get("Chart", "FullStackedLine");
		typeFullStackedSpline.Text = StiLocalization.Get("Chart", "FullStackedSpline");
		typeFullStackedSplineArea.Text = StiLocalization.Get("Chart", "FullStackedSplineArea");
		typeTreemap.Text = StiLocalization.Get("Chart", "Treemap");
		typePictorial.Text = StiLocalization.Get("Chart", "Pictorial");
		typeWaterfall.Text = StiLocalization.Get("Chart", "Waterfall");
		typeSunburst.Text = StiLocalization.Get("Chart", "Sunburst");
		typeGantt.Text = StiLocalization.Get("Chart", "Gantt");
		typeLine.Text = StiLocalization.Get("Chart", "Line");
		typePie.Text = StiLocalization.Get("Chart", "Pie");
		typePie3d.Text = "3D " + StiLocalization.Get("Chart", "Pie");
		typeRadarPoint.Text = StiLocalization.Get("Chart", "RadarPoint");
		typeRadarLine.Text = StiLocalization.Get("Chart", "RadarLine");
		typeRadarArea.Text = StiLocalization.Get("Chart", "RadarArea");
		typeScatter.Text = StiLocalization.Get("Chart", "Scatter");
		typeScatterLine.Text = StiLocalization.Get("Chart", "ScatterLine");
		typeScatterSpline.Text = StiLocalization.Get("Chart", "ScatterSpline");
		typeSpline.Text = StiLocalization.Get("Chart", "Spline");
		typeSplineArea.Text = StiLocalization.Get("Chart", "SplineArea");
		typeStackedArea.Text = StiLocalization.Get("Chart", "StackedArea");
		typeStackedBar.Text = StiLocalization.Get("Chart", "StackedBar");
		typeStackedColumn.Text = StiLocalization.Get("Chart", "StackedColumn");
		typeStackedLine.Text = StiLocalization.Get("Chart", "StackedLine");
		typeStackedSpline.Text = StiLocalization.Get("Chart", "StackedSpline");
		typeStackedSplineArea.Text = StiLocalization.Get("Chart", "StackedSplineArea");
		typeSteppedArea.Text = StiLocalization.Get("Chart", "SteppedArea");
		typeSteppedLine.Text = StiLocalization.Get("Chart", "SteppedLine");
		typeBubble.Text = StiLocalization.Get("Chart", "Bubble");
		typeFunnel.Text = StiLocalization.Get("Chart", "Funnel");
		typeFunnelWeightedSlices.Text = StiLocalization.Get("Chart", "FunnelWeightedSlices");
		typeCandlestick.Text = StiLocalization.Get("Chart", "Candlestick");
		typeRange.Text = StiLocalization.Get("Chart", "Range");
		typeSplineRange.Text = StiLocalization.Get("Chart", "SplineRange");
		typeSteppedRange.Text = StiLocalization.Get("Chart", "SteppedRange");
		typeRangeBar.Text = StiLocalization.Get("Chart", "RangeBar");
		((Control)listBoxItems).Invalidate();
	}

	public void UpdateChartOnDesign()
	{
		Designer.Invalidate(chart);
	}

	private void MenuItemSeries_Click(object sender, EventArgs e)
	{
		isSeriesChanged = true;
		ButtonItem buttonItem = (ButtonItem)sender;
		StiSeries stiSeries = (StiSeries)StiActivator.CreateObject((Type)buttonItem.Tag);
		chart.Series.Add(stiSeries);
		stiSeries.Core?.ApplyStyle(chart.Style, chart.Style.Core.GetColorBySeries(stiSeries, stiSeries.Core?.SeriesColors));
		((ListBox)listBoxItems).Items.Add((object)stiSeries);
		((ListBox)listBoxItems).SelectedItem = stiSeries;
		((Control)listBoxItems).Focus();
		UpdateChartOnDesign();
	}

	private void ListBoxItems_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (((ListBox)listBoxItems).SelectedItem != null)
		{
			StiPropertiesPanelHelper.ActivatePanel(Designer, ((ListBox)listBoxItems).SelectedItem);
		}
	}

	private void PropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
	{
		((Control)listBoxItems).Invalidate();
		UpdateChartOnDesign();
	}

	private void ButtonAddSeries_PopupOpen(object sender, EventArgs e)
	{
		if (!isSeriesChanged)
		{
			return;
		}
		InitSeriesIcons();
		Type[] array = ((chart.Area != null) ? chart.Area.GetSeriesTypes() : new Type[0]);
		foreach (ButtonItem subItem in buttonAddSeries.SubItems)
		{
			foreach (ButtonItem subItem2 in subItem.SubItems)
			{
				Type type = (Type)subItem2.Tag;
				if (type == null)
				{
					continue;
				}
				bool enabled = false;
				Type[] array2 = array;
				foreach (Type type2 in array2)
				{
					if (type2 == type)
					{
						enabled = true;
						break;
					}
				}
				if (chart.Series.Count == 0)
				{
					subItem2.Enabled = true;
				}
				else
				{
					subItem2.Enabled = enabled;
				}
			}
		}
		categoryClusteredColumn.Enabled = typeClusteredColumn.Enabled || typeStackedColumn.Enabled || typeFullStackedColumn.Enabled;
		categoryLine.Enabled = typeLine.Enabled || typeStackedLine.Enabled || typeFullStackedLine.Enabled || typeSpline.Enabled || typeStackedSpline.Enabled || typeFullStackedSpline.Enabled || typeSteppedLine.Enabled;
		categoryArea.Enabled = typeArea.Enabled || typeStackedArea.Enabled || typeFullStackedArea.Enabled || typeSplineArea.Enabled || typeStackedSplineArea.Enabled || typeFullStackedSplineArea.Enabled || typeSteppedArea.Enabled;
		categoryRange.Enabled = typeRangeBar.Enabled || typeSplineRange.Enabled || typeRange.Enabled || typeSteppedRange.Enabled;
		categoryClusteredBar.Enabled = typeGantt.Enabled || typeClusteredBar.Enabled || typeStackedBar.Enabled || typeFullStackedBar.Enabled;
		categoryScatter.Enabled = typeScatter.Enabled || typeScatterLine.Enabled || typeScatterSpline.Enabled || typeBubble.Enabled;
		categoryPie.Enabled = typePie.Enabled || typePie3d.Enabled || typeDoughnut.Enabled;
		categoryRadar.Enabled = typeRadarPoint.Enabled || typeRadarLine.Enabled || typeRadarArea.Enabled;
		categoryFunnel.Enabled = typeFunnel.Enabled || typeFunnelWeightedSlices.Enabled;
		categoryFinancial.Enabled = typeCandlestick.Enabled || typeStock.Enabled;
		categoryTreemap.Enabled = typeTreemap.Enabled;
		categorySunburst.Enabled = typeSunburst.Enabled;
		categoryHistogram.Enabled = typePareto.Enabled;
		categoryBoxAndWhisker.Enabled = typeBoxAndWhisker.Enabled;
		categoryWaterfall.Enabled = typeWaterfall.Enabled;
		categoryPictorial.Enabled = typePictorial.Enabled;
	}

	protected override void OnLoad(EventArgs e)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		base.OnLoad(e);
		Timer timer = new Timer
		{
			Interval = 200
		};
		timer.Tick += delegate
		{
			timer.Stop();
			((Component)(object)timer).Dispose();
			StiPropertiesPanelService.GetService(Designer).RefreshLock = false;
			((Control)listBoxItems).Focus();
		};
		timer.Start();
	}

	protected override void OnFormClosed(FormClosedEventArgs e)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Invalid comparison between Unknown and I4
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		base.OnFormClosed(e);
		lock (StiInplaceEditorForm.FormClosingObject)
		{
			if (!base.IsFormClosed)
			{
				base.IsFormClosed = true;
				if ((int)((Form)this).DialogResult != 1)
				{
					((PropertyGrid)PropertyGrid).SelectedObject = chart;
					StiActions.Clipboard.InvokeUndo(Designer, closeAllInplaceDesigners: false);
					Designer.UndoRedo.RedoStack.Clear();
					((Control)Designer).Refresh();
				}
				else
				{
					((PropertyGrid)PropertyGrid).SelectedObject = chart;
				}
				((PropertyGrid)PropertyGrid).PropertyValueChanged -= new PropertyValueChangedEventHandler(PropertyGrid_PropertyValueChanged);
			}
		}
	}

	private void This_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			e.Handled = true;
			((Form)this).Close();
		}
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)1;
		((Form)this).Close();
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_036f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_052e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f33: Unknown result type (might be due to invalid IL or missing references)
		//IL_2021: Unknown result type (might be due to invalid IL or missing references)
		//IL_205e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2068: Expected O, but got Unknown
		stiGroupLine1 = new StiGroupLine();
		buttonCancel = new Button();
		buttonOk = new Button();
		buttonAddSeries = new ButtonX();
		categoryClusteredColumn = new ButtonItem();
		typeClusteredColumn = new ButtonItem();
		typeStackedColumn = new ButtonItem();
		typeFullStackedColumn = new ButtonItem();
		categoryLine = new ButtonItem();
		typeLine = new ButtonItem();
		typeStackedLine = new ButtonItem();
		typeFullStackedLine = new ButtonItem();
		typeSpline = new ButtonItem();
		typeStackedSpline = new ButtonItem();
		typeFullStackedSpline = new ButtonItem();
		typeSteppedLine = new ButtonItem();
		categoryPie = new ButtonItem();
		typePie = new ButtonItem();
		typePie3d = new ButtonItem();
		typeDoughnut = new ButtonItem();
		categoryClusteredBar = new ButtonItem();
		typeClusteredBar = new ButtonItem();
		typeStackedBar = new ButtonItem();
		typeFullStackedBar = new ButtonItem();
		typeGantt = new ButtonItem();
		categoryArea = new ButtonItem();
		typeArea = new ButtonItem();
		typeStackedArea = new ButtonItem();
		typeFullStackedArea = new ButtonItem();
		typeSplineArea = new ButtonItem();
		typeStackedSplineArea = new ButtonItem();
		typeFullStackedSplineArea = new ButtonItem();
		typeSteppedArea = new ButtonItem();
		categoryRange = new ButtonItem();
		typeRange = new ButtonItem();
		typeSplineRange = new ButtonItem();
		typeSteppedRange = new ButtonItem();
		typeRangeBar = new ButtonItem();
		categoryScatter = new ButtonItem();
		typeScatter = new ButtonItem();
		typeScatterLine = new ButtonItem();
		typeScatterSpline = new ButtonItem();
		typeBubble = new ButtonItem();
		categoryRadar = new ButtonItem();
		typeRadarPoint = new ButtonItem();
		typeRadarLine = new ButtonItem();
		typeRadarArea = new ButtonItem();
		categoryFunnel = new ButtonItem();
		typeFunnel = new ButtonItem();
		typeFunnelWeightedSlices = new ButtonItem();
		categoryFinancial = new ButtonItem();
		typeCandlestick = new ButtonItem();
		typeStock = new ButtonItem();
		categoryTreemap = new ButtonItem();
		typeTreemap = new ButtonItem();
		categorySunburst = new ButtonItem();
		typeSunburst = new ButtonItem();
		categoryHistogram = new ButtonItem();
		typeHistogram = new ButtonItem();
		typePareto = new ButtonItem();
		categoryBoxAndWhisker = new ButtonItem();
		typeBoxAndWhisker = new ButtonItem();
		categoryWaterfall = new ButtonItem();
		typeWaterfall = new ButtonItem();
		categoryPictorial = new ButtonItem();
		typePictorial = new ButtonItem();
		listBoxItems = new StiSeriesListBox();
		((Control)stiGroupLine1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Controls.Add((Control)(object)buttonCancel);
		((Control)stiGroupLine1).Controls.Add((Control)(object)buttonOk);
		((Control)stiGroupLine1).Dock = (DockStyle)2;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 816);
		((Control)stiGroupLine1).Margin = new Padding(6);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(729, 88);
		((Control)stiGroupLine1).TabIndex = 2;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(555, 23);
		((Control)buttonCancel).Margin = new Padding(6);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(150, 46);
		((Control)buttonCancel).TabIndex = 1;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)buttonCancel).Click += This_CancelClick;
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		((Control)buttonOk).Location = new Point(393, 23);
		((Control)buttonOk).Margin = new Padding(6);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(150, 46);
		((Control)buttonOk).TabIndex = 0;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += This_OkClick;
		((Control)buttonAddSeries).AccessibleRole = (AccessibleRole)43;
		buttonAddSeries.AutoExpandOnClick = true;
		buttonAddSeries.Is20Button = false;
		((Control)buttonAddSeries).Location = new Point(24, 23);
		((Control)buttonAddSeries).Margin = new Padding(6);
		((Control)buttonAddSeries).Name = "buttonAddSeries";
		((Control)buttonAddSeries).Size = new Size(226, 46);
		buttonAddSeries.Style = eDotNetBarStyle.StyleManagerControlled;
		buttonAddSeries.SubItems.AddRange(new BaseItem[16]
		{
			categoryClusteredColumn, categoryLine, categoryPie, categoryClusteredBar, categoryArea, categoryRange, categoryScatter, categoryRadar, categoryFunnel, categoryFinancial,
			categoryTreemap, categorySunburst, categoryHistogram, categoryBoxAndWhisker, categoryWaterfall, categoryPictorial
		});
		((Control)buttonAddSeries).TabIndex = 0;
		((Control)buttonAddSeries).Text = "Add Series";
		buttonAddSeries.ThemeAware = true;
		buttonAddSeries.PopupOpen += ButtonAddSeries_PopupOpen;
		categoryClusteredColumn.ImagePaddingHorizontal = 16;
		categoryClusteredColumn.ImagePaddingVertical = 12;
		categoryClusteredColumn.Name = "categoryClusteredColumn";
		categoryClusteredColumn.SubItems.AddRange(new BaseItem[3] { typeClusteredColumn, typeStackedColumn, typeFullStackedColumn });
		categoryClusteredColumn.Text = "Clustered Column";
		categoryClusteredColumn.ThemeAware = true;
		typeClusteredColumn.ButtonStyle = eButtonStyle.ImageAndText;
		typeClusteredColumn.ImagePaddingHorizontal = 16;
		typeClusteredColumn.ImagePaddingVertical = 12;
		typeClusteredColumn.Name = "typeClusteredColumn";
		typeClusteredColumn.Text = "Clustered Column";
		typeClusteredColumn.ThemeAware = true;
		typeClusteredColumn.Click += MenuItemSeries_Click;
		typeStackedColumn.ButtonStyle = eButtonStyle.ImageAndText;
		typeStackedColumn.ImagePaddingHorizontal = 16;
		typeStackedColumn.ImagePaddingVertical = 12;
		typeStackedColumn.Name = "typeStackedColumn";
		typeStackedColumn.Text = "Stacked Column";
		typeStackedColumn.ThemeAware = true;
		typeStackedColumn.Click += MenuItemSeries_Click;
		typeFullStackedColumn.ButtonStyle = eButtonStyle.ImageAndText;
		typeFullStackedColumn.ImagePaddingHorizontal = 16;
		typeFullStackedColumn.ImagePaddingVertical = 12;
		typeFullStackedColumn.Name = "typeFullStackedColumn";
		typeFullStackedColumn.Text = "Full Stacked Column";
		typeFullStackedColumn.ThemeAware = true;
		typeFullStackedColumn.Click += MenuItemSeries_Click;
		categoryLine.ImagePaddingHorizontal = 16;
		categoryLine.ImagePaddingVertical = 12;
		categoryLine.Name = "categoryLine";
		categoryLine.SubItems.AddRange(new BaseItem[7] { typeLine, typeStackedLine, typeFullStackedLine, typeSpline, typeStackedSpline, typeFullStackedSpline, typeSteppedLine });
		categoryLine.Text = "Line";
		categoryLine.ThemeAware = true;
		typeLine.ButtonStyle = eButtonStyle.ImageAndText;
		typeLine.ImagePaddingHorizontal = 16;
		typeLine.ImagePaddingVertical = 12;
		typeLine.Name = "typeLine";
		typeLine.Text = "Line";
		typeLine.ThemeAware = true;
		typeLine.Click += MenuItemSeries_Click;
		typeStackedLine.ButtonStyle = eButtonStyle.ImageAndText;
		typeStackedLine.ImagePaddingHorizontal = 16;
		typeStackedLine.ImagePaddingVertical = 12;
		typeStackedLine.Name = "typeStackedLine";
		typeStackedLine.Text = "Stacked Line";
		typeStackedLine.ThemeAware = true;
		typeStackedLine.Click += MenuItemSeries_Click;
		typeFullStackedLine.ButtonStyle = eButtonStyle.ImageAndText;
		typeFullStackedLine.ImagePaddingHorizontal = 16;
		typeFullStackedLine.ImagePaddingVertical = 12;
		typeFullStackedLine.Name = "typeFullStackedLine";
		typeFullStackedLine.Text = "Full Stacked Line";
		typeFullStackedLine.ThemeAware = true;
		typeFullStackedLine.Click += MenuItemSeries_Click;
		typeSpline.BeginGroup = true;
		typeSpline.ImagePaddingHorizontal = 16;
		typeSpline.ImagePaddingVertical = 12;
		typeSpline.Name = "typeSpline";
		typeSpline.Text = "Spline";
		typeSpline.ThemeAware = true;
		typeSpline.Click += MenuItemSeries_Click;
		typeStackedSpline.ImagePaddingHorizontal = 16;
		typeStackedSpline.ImagePaddingVertical = 12;
		typeStackedSpline.Name = "typeStackedSpline";
		typeStackedSpline.Text = "Stacked Spline";
		typeStackedSpline.ThemeAware = true;
		typeStackedSpline.Click += MenuItemSeries_Click;
		typeFullStackedSpline.ImagePaddingHorizontal = 16;
		typeFullStackedSpline.ImagePaddingVertical = 12;
		typeFullStackedSpline.Name = "typeFullStackedSpline";
		typeFullStackedSpline.Text = "Full-Stacked Spline";
		typeFullStackedSpline.ThemeAware = true;
		typeFullStackedSpline.Click += MenuItemSeries_Click;
		typeSteppedLine.BeginGroup = true;
		typeSteppedLine.ImagePaddingHorizontal = 16;
		typeSteppedLine.ImagePaddingVertical = 12;
		typeSteppedLine.Name = "typeSteppedLine";
		typeSteppedLine.Text = "Stepped Line";
		typeSteppedLine.ThemeAware = true;
		typeSteppedLine.Click += MenuItemSeries_Click;
		categoryPie.ImagePaddingHorizontal = 16;
		categoryPie.ImagePaddingVertical = 12;
		categoryPie.Name = "categoryPie";
		categoryPie.SubItems.AddRange(new BaseItem[3] { typePie, typePie3d, typeDoughnut });
		categoryPie.Text = "Pie";
		categoryPie.ThemeAware = true;
		typePie.ImagePaddingHorizontal = 16;
		typePie.ImagePaddingVertical = 12;
		typePie.Name = "typePie";
		typePie.Text = "Pie";
		typePie.ThemeAware = true;
		typePie.Click += MenuItemSeries_Click;
		typePie3d.ImagePaddingHorizontal = 16;
		typePie3d.ImagePaddingVertical = 12;
		typePie3d.Name = "typePie3d";
		typePie3d.Text = "3D Pie";
		typePie3d.ThemeAware = true;
		typePie3d.Click += MenuItemSeries_Click;
		typeDoughnut.ImagePaddingHorizontal = 16;
		typeDoughnut.ImagePaddingVertical = 12;
		typeDoughnut.Name = "typeDoughnut";
		typeDoughnut.Text = "Doughnut";
		typeDoughnut.ThemeAware = true;
		typeDoughnut.Click += MenuItemSeries_Click;
		categoryClusteredBar.ImagePaddingHorizontal = 16;
		categoryClusteredBar.ImagePaddingVertical = 12;
		categoryClusteredBar.Name = "categoryClusteredBar";
		categoryClusteredBar.SubItems.AddRange(new BaseItem[4] { typeClusteredBar, typeStackedBar, typeFullStackedBar, typeGantt });
		categoryClusteredBar.Text = "Clustered Bar";
		categoryClusteredBar.ThemeAware = true;
		typeClusteredBar.ImagePaddingHorizontal = 16;
		typeClusteredBar.ImagePaddingVertical = 12;
		typeClusteredBar.Name = "typeClusteredBar";
		typeClusteredBar.Text = "Clustered Bar";
		typeClusteredBar.ThemeAware = true;
		typeClusteredBar.Click += MenuItemSeries_Click;
		typeStackedBar.ImagePaddingHorizontal = 16;
		typeStackedBar.ImagePaddingVertical = 12;
		typeStackedBar.Name = "typeStackedBar";
		typeStackedBar.Text = "Stacked Bar";
		typeStackedBar.ThemeAware = true;
		typeStackedBar.Click += MenuItemSeries_Click;
		typeFullStackedBar.ImagePaddingHorizontal = 16;
		typeFullStackedBar.ImagePaddingVertical = 12;
		typeFullStackedBar.Name = "typeFullStackedBar";
		typeFullStackedBar.Text = "Full-Stacked Bar";
		typeFullStackedBar.ThemeAware = true;
		typeFullStackedBar.Click += MenuItemSeries_Click;
		typeGantt.ImagePaddingHorizontal = 16;
		typeGantt.ImagePaddingVertical = 12;
		typeGantt.Name = "typeGantt";
		typeGantt.Text = "Gantt";
		typeGantt.ThemeAware = true;
		typeGantt.Click += MenuItemSeries_Click;
		categoryArea.ImagePaddingHorizontal = 16;
		categoryArea.ImagePaddingVertical = 12;
		categoryArea.Name = "categoryArea";
		categoryArea.SubItems.AddRange(new BaseItem[7] { typeArea, typeStackedArea, typeFullStackedArea, typeSplineArea, typeStackedSplineArea, typeFullStackedSplineArea, typeSteppedArea });
		categoryArea.Text = "Area";
		categoryArea.ThemeAware = true;
		typeArea.ImagePaddingHorizontal = 16;
		typeArea.ImagePaddingVertical = 12;
		typeArea.Name = "typeArea";
		typeArea.Text = "Area";
		typeArea.ThemeAware = true;
		typeArea.Click += MenuItemSeries_Click;
		typeStackedArea.ImagePaddingHorizontal = 16;
		typeStackedArea.ImagePaddingVertical = 12;
		typeStackedArea.Name = "typeStackedArea";
		typeStackedArea.Text = "Stacked Area";
		typeStackedArea.ThemeAware = true;
		typeStackedArea.Click += MenuItemSeries_Click;
		typeFullStackedArea.ImagePaddingHorizontal = 16;
		typeFullStackedArea.ImagePaddingVertical = 12;
		typeFullStackedArea.Name = "typeFullStackedArea";
		typeFullStackedArea.Text = "Full-Stacked Area";
		typeFullStackedArea.ThemeAware = true;
		typeFullStackedArea.Click += MenuItemSeries_Click;
		typeSplineArea.BeginGroup = true;
		typeSplineArea.ImagePaddingHorizontal = 16;
		typeSplineArea.ImagePaddingVertical = 12;
		typeSplineArea.Name = "typeSplineArea";
		typeSplineArea.Text = "Spline Area";
		typeSplineArea.ThemeAware = true;
		typeSplineArea.Click += MenuItemSeries_Click;
		typeStackedSplineArea.ImagePaddingHorizontal = 16;
		typeStackedSplineArea.ImagePaddingVertical = 12;
		typeStackedSplineArea.Name = "typeStackedSplineArea";
		typeStackedSplineArea.Text = "Stacked Spline Area";
		typeStackedSplineArea.ThemeAware = true;
		typeStackedSplineArea.Click += MenuItemSeries_Click;
		typeFullStackedSplineArea.ImagePaddingHorizontal = 16;
		typeFullStackedSplineArea.ImagePaddingVertical = 12;
		typeFullStackedSplineArea.Name = "typeFullStackedSplineArea";
		typeFullStackedSplineArea.Text = "Full-Stacked Spline Area";
		typeFullStackedSplineArea.ThemeAware = true;
		typeFullStackedSplineArea.Click += MenuItemSeries_Click;
		typeSteppedArea.BeginGroup = true;
		typeSteppedArea.ImagePaddingHorizontal = 16;
		typeSteppedArea.ImagePaddingVertical = 12;
		typeSteppedArea.Name = "typeSteppedArea";
		typeSteppedArea.Text = "Stepped Area";
		typeSteppedArea.ThemeAware = true;
		typeSteppedArea.Click += MenuItemSeries_Click;
		categoryRange.ImagePaddingHorizontal = 16;
		categoryRange.ImagePaddingVertical = 12;
		categoryRange.Name = "categoryRange";
		categoryRange.SubItems.AddRange(new BaseItem[4] { typeRange, typeSplineRange, typeSteppedRange, typeRangeBar });
		categoryRange.Text = "Range";
		categoryRange.ThemeAware = true;
		typeRange.ImagePaddingHorizontal = 16;
		typeRange.ImagePaddingVertical = 12;
		typeRange.Name = "typeRange";
		typeRange.Text = "Line Range";
		typeRange.ThemeAware = true;
		typeRange.Click += MenuItemSeries_Click;
		typeSplineRange.ImagePaddingHorizontal = 16;
		typeSplineRange.ImagePaddingVertical = 12;
		typeSplineRange.Name = "typeSplineRange";
		typeSplineRange.Text = "Spline Range";
		typeSplineRange.ThemeAware = true;
		typeSplineRange.Click += MenuItemSeries_Click;
		typeSteppedRange.ImagePaddingHorizontal = 16;
		typeSteppedRange.ImagePaddingVertical = 12;
		typeSteppedRange.Name = "typeSteppedRange";
		typeSteppedRange.Text = "Stepped Range";
		typeSteppedRange.ThemeAware = true;
		typeSteppedRange.Click += MenuItemSeries_Click;
		typeRangeBar.ImagePaddingHorizontal = 16;
		typeRangeBar.ImagePaddingVertical = 12;
		typeRangeBar.Name = "typeRangeBar";
		typeRangeBar.Text = "Range Bar";
		typeRangeBar.ThemeAware = true;
		typeRangeBar.Click += MenuItemSeries_Click;
		categoryScatter.ImagePaddingHorizontal = 16;
		categoryScatter.ImagePaddingVertical = 12;
		categoryScatter.Name = "categoryScatter";
		categoryScatter.SubItems.AddRange(new BaseItem[4] { typeScatter, typeScatterLine, typeScatterSpline, typeBubble });
		categoryScatter.Text = "Scatter";
		categoryScatter.ThemeAware = true;
		typeScatter.ImagePaddingHorizontal = 16;
		typeScatter.ImagePaddingVertical = 12;
		typeScatter.Name = "typeScatter";
		typeScatter.Text = "Scatter";
		typeScatter.ThemeAware = true;
		typeScatter.Click += MenuItemSeries_Click;
		typeScatterLine.ImagePaddingHorizontal = 16;
		typeScatterLine.ImagePaddingVertical = 12;
		typeScatterLine.Name = "typeScatterLine";
		typeScatterLine.Text = "Scatter Line";
		typeScatterLine.ThemeAware = true;
		typeScatterLine.Click += MenuItemSeries_Click;
		typeScatterSpline.ImagePaddingHorizontal = 16;
		typeScatterSpline.ImagePaddingVertical = 12;
		typeScatterSpline.Name = "typeScatterSpline";
		typeScatterSpline.Text = "Scatter Spline";
		typeScatterSpline.ThemeAware = true;
		typeScatterSpline.Click += MenuItemSeries_Click;
		typeBubble.ImagePaddingHorizontal = 16;
		typeBubble.ImagePaddingVertical = 12;
		typeBubble.Name = "typeBubble";
		typeBubble.Text = "Bubble";
		typeBubble.ThemeAware = true;
		typeBubble.Click += MenuItemSeries_Click;
		categoryRadar.ImagePaddingHorizontal = 16;
		categoryRadar.ImagePaddingVertical = 12;
		categoryRadar.Name = "categoryRadar";
		categoryRadar.SubItems.AddRange(new BaseItem[3] { typeRadarPoint, typeRadarLine, typeRadarArea });
		categoryRadar.Text = "Radar";
		categoryRadar.ThemeAware = true;
		typeRadarPoint.ImagePaddingHorizontal = 16;
		typeRadarPoint.ImagePaddingVertical = 12;
		typeRadarPoint.Name = "typeRadarPoint";
		typeRadarPoint.Text = "Radar Point";
		typeRadarPoint.ThemeAware = true;
		typeRadarPoint.Click += MenuItemSeries_Click;
		typeRadarLine.ImagePaddingHorizontal = 16;
		typeRadarLine.ImagePaddingVertical = 12;
		typeRadarLine.Name = "typeRadarLine";
		typeRadarLine.Text = "Radar Line";
		typeRadarLine.ThemeAware = true;
		typeRadarLine.Click += MenuItemSeries_Click;
		typeRadarArea.ImagePaddingHorizontal = 16;
		typeRadarArea.ImagePaddingVertical = 12;
		typeRadarArea.Name = "typeRadarArea";
		typeRadarArea.Text = "Radar Area";
		typeRadarArea.ThemeAware = true;
		typeRadarArea.Click += MenuItemSeries_Click;
		categoryFunnel.ImagePaddingHorizontal = 16;
		categoryFunnel.ImagePaddingVertical = 12;
		categoryFunnel.Name = "categoryFunnel";
		categoryFunnel.SubItems.AddRange(new BaseItem[2] { typeFunnel, typeFunnelWeightedSlices });
		categoryFunnel.Text = "Funnel";
		categoryFunnel.ThemeAware = true;
		typeFunnel.ImagePaddingHorizontal = 16;
		typeFunnel.ImagePaddingVertical = 12;
		typeFunnel.Name = "typeFunnel";
		typeFunnel.Text = "Funnel";
		typeFunnel.ThemeAware = true;
		typeFunnel.Click += MenuItemSeries_Click;
		typeFunnelWeightedSlices.ImagePaddingHorizontal = 16;
		typeFunnelWeightedSlices.ImagePaddingVertical = 12;
		typeFunnelWeightedSlices.Name = "typeFunnelWeightedSlices";
		typeFunnelWeightedSlices.Text = "Funnel Weighted Slices";
		typeFunnelWeightedSlices.ThemeAware = true;
		typeFunnelWeightedSlices.Click += MenuItemSeries_Click;
		categoryFinancial.ImagePaddingHorizontal = 16;
		categoryFinancial.ImagePaddingVertical = 12;
		categoryFinancial.Name = "categoryFinancial";
		categoryFinancial.SubItems.AddRange(new BaseItem[2] { typeCandlestick, typeStock });
		categoryFinancial.Text = "Financial";
		categoryFinancial.ThemeAware = true;
		typeCandlestick.ImagePaddingHorizontal = 16;
		typeCandlestick.ImagePaddingVertical = 12;
		typeCandlestick.Name = "typeCandlestick";
		typeCandlestick.Text = "Candlestick";
		typeCandlestick.ThemeAware = true;
		typeCandlestick.Click += MenuItemSeries_Click;
		typeStock.ImagePaddingHorizontal = 16;
		typeStock.ImagePaddingVertical = 12;
		typeStock.Name = "typeStock";
		typeStock.Text = "Stock";
		typeStock.ThemeAware = true;
		typeStock.Click += MenuItemSeries_Click;
		categoryTreemap.ImagePaddingHorizontal = 16;
		categoryTreemap.ImagePaddingVertical = 12;
		categoryTreemap.Name = "categoryTreemap";
		categoryTreemap.SubItems.AddRange(new BaseItem[1] { typeTreemap });
		categoryTreemap.Text = "Treemap";
		categoryTreemap.ThemeAware = true;
		typeTreemap.ImagePaddingHorizontal = 16;
		typeTreemap.ImagePaddingVertical = 12;
		typeTreemap.Name = "typeTreemap";
		typeTreemap.Text = "Treemap";
		typeTreemap.ThemeAware = true;
		typeTreemap.Click += MenuItemSeries_Click;
		categorySunburst.ImagePaddingHorizontal = 16;
		categorySunburst.ImagePaddingVertical = 12;
		categorySunburst.Name = "categorySunburst";
		categorySunburst.SubItems.AddRange(new BaseItem[1] { typeSunburst });
		categorySunburst.Text = "Sunburst";
		categorySunburst.ThemeAware = true;
		typeSunburst.ImagePaddingHorizontal = 16;
		typeSunburst.ImagePaddingVertical = 12;
		typeSunburst.Name = "typeSunburst";
		typeSunburst.Text = "Sunburst";
		typeSunburst.ThemeAware = true;
		typeSunburst.Click += MenuItemSeries_Click;
		categoryHistogram.ImagePaddingHorizontal = 16;
		categoryHistogram.ImagePaddingVertical = 12;
		categoryHistogram.Name = "categoryHistogram";
		categoryHistogram.SubItems.AddRange(new BaseItem[2] { typeHistogram, typePareto });
		categoryHistogram.Text = "Histogram";
		categoryHistogram.ThemeAware = true;
		typeHistogram.ButtonStyle = eButtonStyle.ImageAndText;
		typeHistogram.ImagePaddingHorizontal = 16;
		typeHistogram.ImagePaddingVertical = 12;
		typeHistogram.Name = "typeHistogram";
		typeHistogram.Text = "Histogram";
		typeHistogram.ThemeAware = true;
		typeHistogram.Click += MenuItemSeries_Click;
		typePareto.ButtonStyle = eButtonStyle.ImageAndText;
		typePareto.ImagePaddingHorizontal = 16;
		typePareto.ImagePaddingVertical = 12;
		typePareto.Name = "typePareto";
		typePareto.Text = "Pareto";
		typePareto.ThemeAware = true;
		typePareto.Click += MenuItemSeries_Click;
		categoryBoxAndWhisker.ImagePaddingHorizontal = 16;
		categoryBoxAndWhisker.ImagePaddingVertical = 12;
		categoryBoxAndWhisker.Name = "categoryBoxAndWhisker";
		categoryBoxAndWhisker.SubItems.AddRange(new BaseItem[1] { typeBoxAndWhisker });
		categoryBoxAndWhisker.Text = "BoxAndWhisker";
		categoryBoxAndWhisker.ThemeAware = true;
		typeBoxAndWhisker.ImagePaddingHorizontal = 16;
		typeBoxAndWhisker.ImagePaddingVertical = 12;
		typeBoxAndWhisker.Name = "typeBoxAndWhisker";
		typeBoxAndWhisker.Text = "Box And Whisker";
		typeBoxAndWhisker.ThemeAware = true;
		typeBoxAndWhisker.Click += MenuItemSeries_Click;
		categoryWaterfall.ImagePaddingHorizontal = 16;
		categoryWaterfall.ImagePaddingVertical = 12;
		categoryWaterfall.Name = "categoryWaterfall";
		categoryWaterfall.SubItems.AddRange(new BaseItem[1] { typeWaterfall });
		categoryWaterfall.Text = "Waterfall";
		categoryWaterfall.ThemeAware = true;
		typeWaterfall.ImagePaddingHorizontal = 16;
		typeWaterfall.ImagePaddingVertical = 12;
		typeWaterfall.Name = "typeWaterfall";
		typeWaterfall.Text = "Waterfall";
		typeWaterfall.ThemeAware = true;
		typeWaterfall.Click += MenuItemSeries_Click;
		categoryPictorial.ImagePaddingHorizontal = 16;
		categoryPictorial.ImagePaddingVertical = 12;
		categoryPictorial.Name = "categoryPictorial";
		categoryPictorial.SubItems.AddRange(new BaseItem[1] { typePictorial });
		categoryPictorial.Text = "Other";
		categoryPictorial.ThemeAware = true;
		typePictorial.ImagePaddingHorizontal = 16;
		typePictorial.ImagePaddingVertical = 12;
		typePictorial.Name = "typePictorial";
		typePictorial.Text = "Pictorial";
		typePictorial.ThemeAware = true;
		typePictorial.Click += MenuItemSeries_Click;
		((Control)listBoxItems).AllowDrop = true;
		((Control)listBoxItems).Anchor = (AnchorStyles)15;
		((Control)listBoxItems).Location = new Point(24, 94);
		((Control)listBoxItems).Margin = new Padding(6, 6, 6, 0);
		((Control)listBoxItems).Name = "listBoxItems";
		((Control)listBoxItems).Size = new Size(681, 690);
		((Control)listBoxItems).TabIndex = 1;
		((ListBox)listBoxItems).SelectedIndexChanged += ListBoxItems_SelectedIndexChanged;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((Control)this).AllowDrop = true;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(729, 904);
		((Control)this).Controls.Add((Control)(object)buttonAddSeries);
		((Control)this).Controls.Add((Control)(object)listBoxItems);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).Margin = new Padding(6);
		((Control)this).MinimumSize = new Size(554, 656);
		((Control)this).Name = "StiChartSeriesEditorForm";
		((Control)this).Text = "Series";
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
