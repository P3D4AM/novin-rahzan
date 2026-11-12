using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Chart.Design;

[ToolboxItem(false)]
public class StiCloudSeriesControl : UserControl
{
	private class StiMouseScrollableControl : StiScrollableControl
	{
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			if (e.Delta > 0)
			{
				base.VScrollBar.DoUp();
			}
			else
			{
				base.VScrollBar.DoDown();
			}
		}
	}

	public struct StiSeriesImageInfo
	{
		public Bitmap Bitmap;

		public object Tag;

		public int ImageWidth;

		public int ImageHeight;

		public int X;

		public int Y;

		public int Width;

		public int Height;

		public StiSeriesImageInfo(Bitmap bitmap, object obj, int x, int y, int width, int height, int imageWidth, int imageHeight)
		{
			Bitmap = bitmap;
			Tag = obj;
			X = x;
			Y = y;
			Width = width;
			Height = height;
			ImageWidth = imageWidth;
			ImageHeight = imageHeight;
		}
	}

	private StiCloudChartForm chartWindow;

	private Timer timerSeriesLabels;

	private IStiSeriesLabels lastSeriesLabels;

	private StiCloudChartConditionsCollectionControl conditionsControl;

	private StiSeries lastConditionsSeries;

	private StiCloudChartFiltersCollectionControl filtersControl;

	private StiSeries lastFiltersSeries;

	private StiCloudChartTrendLinesCollectionControl trendLinesControl;

	private StiSeries lastTrendLinesSeries;

	private bool isInitSeriesIcons;

	private bool isSeriesChanged = true;

	private StiSeries lastSeries;

	private Type lastSeriesLabelsStyle;

	private bool lockRadioShowSeriesLabels;

	private List<StiSeriesImageInfo> listSeriesLabelsImages;

	private int seriesLabelsImagesSelectedIndex;

	private int seriesLabelsImagesMouseOverIndex;

	private IContainer components;

	private StiCloudTabulator TabControl;

	private StiCloudTabulatorPage pageCommon;

	private StiCloudTabulatorPage pageConditions;

	private StiCloudTabulatorPage pageFilters;

	private StiCloudTabulatorPage pageMarker;

	private StiCloudTabulatorPage pageLineMarker;

	private StiCloudTabulatorPage pageInteraction;

	private StiCloudTabulatorPage pageTrendLine;

	private StiCloudTabulatorPage pageTopN;

	private StiCloudTabulatorPage pageSeriesLabels;

	private Panel panelAdditionalCommon;

	private Bar toolBar;

	private ButtonItem buttonRemoveSeries;

	private ButtonItem buttonDownSeries;

	private ButtonItem buttonUpSeries;

	private ButtonItem menuAddSeries;

	private ButtonItem categoryClusteredColumn;

	private ButtonItem categoryTreemap;

	private ButtonItem categorySunburst;

	private ButtonItem categoryHistogram;

	private ButtonItem categoryBoxAndWhisker;

	private ButtonItem categoryLine;

	private ButtonItem typeClusteredColumn;

	private ButtonItem typeHistogram;

	private ButtonItem typeBoxAndWhisker;

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

	private ButtonItem type3dPie;

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

	private StiGroupLine toolBarPanel;

	private Panel tvChartSeriesPanel;

	public StiTreeView tvChartSeries;

	private Panel pageConditionsPanel;

	private Panel pageFiltersPanel;

	private StiGroupLine footerPanelSeriesLabels;

	private Label labelShowSeriesLabels;

	private Label labelSeriesLabel;

	public RadioButton radioFromSeries;

	public RadioButton radioFromChart;

	private Panel pageSeriesLabelsLeftPanel;

	private StiMouseScrollableControl panelSeriesLabels;

	private Panel pageTrendLinePanel;

	public StiCloudSeriesControl(StiCloudChartForm chartWindow)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			this.chartWindow = chartWindow;
			timerSeriesLabels = new Timer
			{
				Interval = 70
			};
			timerSeriesLabels.Tick += timerSeriesLabelsTick;
			InitSeriesTypes();
			pageConditions.Invisible = true;
			pageFilters.Invisible = true;
			pageInteraction.Invisible = true;
			pageLineMarker.Invisible = true;
			pageMarker.Invisible = true;
			pageSeriesLabels.Invisible = true;
			pageTopN.Invisible = true;
			pageTrendLine.Invisible = true;
			Localize();
			TabControl.SelectedIndex = 0;
			TabControl_SelectedIndexChanged(null, null);
			BuildSeries();
			UpdateStateSeries();
			DoScaling();
		}
	}

	private void Localize()
	{
		menuAddSeries.Text = StiHint.CreateHint(StiLocalization.Get("Chart", "AddSeries"));
		menuAddSeries.Tooltip = StiHint.CreateHint(StiLocalization.Get("Chart", "AddSeries"));
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
		categoryBoxAndWhisker.Text = StiLocalization.Get("Chart", "BoxAndWhisker");
		categoryWaterfall.Text = StiLocalization.Get("Chart", "Waterfall");
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
		type3dPie.Text = "3D " + StiLocalization.Get("Chart", "Pie");
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
		((Control)pageCommon).Text = StiLocalization.Get("Chart", "Common");
		((Control)pageConditions).Text = StiLocalization.Get("Chart", "ChartConditionsCollectionForm");
		((Control)pageFilters).Text = StiLocalization.Get("Chart", "ChartFiltersCollectionForm");
		((Control)pageTopN).Text = StiLocalization.Get("PropertyMain", "TopN");
		((Control)pageInteraction).Text = StiLocalization.Get("PropertyMain", "Interaction");
		((Control)pageSeriesLabels).Text = StiLocalization.Get("PropertyMain", "SeriesLabels");
		((Control)pageTrendLine).Text = StiLocalization.Get("PropertyMain", "TrendLine");
		((Control)pageLineMarker).Text = StiLocalization.Get("PropertyMain", "LineMarker");
		((Control)pageMarker).Text = StiLocalization.Get("PropertyMain", "Marker");
		((Control)labelShowSeriesLabels).Text = StiLocalization.Get("PropertyMain", "ShowSeriesLabels");
		((Control)radioFromChart).Text = StiLocalization.Get("PropertyEnum", "StiShowSeriesLabelsFromChart");
		((Control)radioFromSeries).Text = StiLocalization.Get("PropertyEnum", "StiShowSeriesLabelsFromSeries");
		buttonRemoveSeries.Tooltip = StiHint.CreateHint(StiLocalization.Get("Chart", "RemoveSeries"));
		buttonUpSeries.Tooltip = StiHint.CreateHint(StiLocalization.Get("Chart", "MoveSeriesUp"));
		buttonDownSeries.Tooltip = StiHint.CreateHint(StiLocalization.Get("Chart", "MoveSeriesDown"));
	}

	private void DoScaling()
	{
		DockPaddingEdges dockPadding = TabControl.DockPadding;
		int right = (TabControl.TitleWidth = StiScale.XXI(110.0));
		dockPadding.Right = right;
		pageCommon.Image = StiReportDesignImages.Tasks.ChartSeriesCommon(StiImageSize.Double);
		pageConditions.Image = StiReportDesignImages.Tasks.ChartConditions(StiImageSize.Double);
		pageFilters.Image = StiReportDesignImages.Tasks.ChartFilters(StiImageSize.Double);
		pageInteraction.Image = StiReportDesignImages.Tasks.ChartInteraction(StiImageSize.Double);
		pageSeriesLabels.Image = StiReportDesignImages.Tasks.ChartSeriesLabels(StiImageSize.Double);
		pageTopN.Image = StiReportDesignImages.Tasks.ChartTopN(StiImageSize.Double);
		pageTrendLine.Image = StiReportDesignImages.Tasks.ChartTrendLine(StiImageSize.Double);
		pageLineMarker.Image = StiReportDesignImages.Tasks.ChartLineMarker(StiImageSize.Double);
		pageMarker.Image = StiReportDesignImages.Tasks.ChartMarker(StiImageSize.Double);
		menuAddSeries.Image = StiReportDesignImages.Actions.NewSeries();
		buttonRemoveSeries.Image = StiReportDesignImages.Actions.Remove();
		buttonUpSeries.Image = StiReportDesignImages.Actions.MoveUp();
		buttonDownSeries.Image = StiReportDesignImages.Actions.MoveDown();
		((Control)labelShowSeriesLabels).Width = TextRenderer.MeasureText(((Control)labelShowSeriesLabels).Text, ((Control)labelShowSeriesLabels).Font).Width;
		Label obj = labelShowSeriesLabels;
		right = (((Control)radioFromChart).Top = ((Control)radioFromSeries).Top);
		((Control)obj).Top = right;
		Label obj2 = labelShowSeriesLabels;
		right = (((Control)radioFromChart).Height = ((Control)radioFromSeries).Height);
		((Control)obj2).Height = right;
		((Control)radioFromChart).Left = ((Control)labelShowSeriesLabels).Right + StiScaleUI.I12;
		((Control)radioFromSeries).Left = ((Control)radioFromChart).Right + StiScaleUI.I12;
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
		typeBoxAndWhisker.Tag = typeof(StiBoxAndWhiskerSeries);
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
		type3dPie.Tag = typeof(StiPie3dSeries);
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
	}

	internal void CheckSamplePosition()
	{
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		if (TabControl.SelectedTab == pageSeriesLabels)
		{
			chartWindow.UpdateSamplePosition(1, 1, 1, 1, showRightSeparator: true, showSample: true, skipFooter: true, StiScale.I20);
			((Control)panelAdditionalCommon).Visible = false;
			TreeNode selectedNode = ((TreeView)tvChartSeries).SelectedNode;
			if (selectedNode != null)
			{
				StiSeries stiSeries = (StiSeries)selectedNode.Tag;
				((Control)labelSeriesLabel).Visible = true;
				((Control)labelSeriesLabel).Text = $"{stiSeries.Title} {stiSeries}";
			}
			else
			{
				((Control)labelSeriesLabel).Visible = false;
			}
			Timer val = new Timer
			{
				Interval = 10
			};
			val.Tick += TimerLabel_Tick;
			val.Start();
		}
		else if (TabControl.SelectedTab == pageConditions || TabControl.SelectedTab == pageFilters || TabControl.SelectedTab == pageTrendLine)
		{
			chartWindow.UpdateSamplePosition(0, 0, 2, 2, showRightSeparator: true, showSample: false);
			((Control)panelAdditionalCommon).Visible = false;
		}
		else
		{
			chartWindow.UpdateSamplePosition(1, 1, 1, 1, showRightSeparator: true, showSample: true);
			((Control)panelAdditionalCommon).Visible = true;
		}
		UpdatePropertyGridState();
	}

	private void TimerLabel_Tick(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		Timer val = (Timer)sender;
		val.Stop();
		BuildSeriesLabels();
	}

	private void UpdatePropertyGridState()
	{
		if (((TreeView)tvChartSeries).SelectedNode == null)
		{
			((PropertyGrid)chartWindow.PropertyGrid).SelectedObject = null;
		}
		else if (TabControl.SelectedTab == pageCommon)
		{
			((PropertyGrid)chartWindow.PropertyGrid).SelectedObject = ((TreeView)tvChartSeries).SelectedNode.Tag;
		}
		else if (TabControl.SelectedTab == pageConditions || TabControl.SelectedTab == pageFilters || TabControl.SelectedTab == pageTrendLine)
		{
			((PropertyGrid)chartWindow.PropertyGrid).SelectedObject = null;
		}
		else if (TabControl.SelectedTab == null)
		{
			((PropertyGrid)chartWindow.PropertyGrid).SelectedObject = null;
		}
		else
		{
			((PropertyGrid)chartWindow.PropertyGrid).SelectedObject = ((Control)TabControl.SelectedTab).Tag;
		}
	}

	public void SaveConditions()
	{
		if (lastConditionsSeries != null)
		{
			lastConditionsSeries = null;
			if (conditionsControl != null)
			{
				conditionsControl.AddConditions();
			}
		}
	}

	public void SaveFilters()
	{
		if (lastFiltersSeries != null)
		{
			lastFiltersSeries = null;
			if (filtersControl != null)
			{
				filtersControl.AddFilters();
			}
		}
	}

	public void SaveTrendLines()
	{
		if (lastTrendLinesSeries != null)
		{
			lastTrendLinesSeries = null;
			if (trendLinesControl != null)
			{
				trendLinesControl.AddTrendLines();
			}
		}
	}

	private void Panels_Paint(object sender, PaintEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		Panel val = (Panel)sender;
		Pen val2 = new Pen(Color.FromArgb(170, 170, 170));
		try
		{
			val2.DashPattern = new float[2] { 1f, 2f };
			Point point = new Point(StiScale.I(204), 0);
			Point point2 = new Point(StiScale.I(204), ((Control)val).Height);
			e.Graphics.DrawLine(val2, point, point2);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	private void pageSeriesLabelsLeftPanel_Paint(object sender, PaintEventArgs e)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		Pen val = new Pen(Color.FromArgb(170, 170, 170));
		try
		{
			val.DashPattern = new float[2] { 1f, 2f };
			Point point = new Point(((Control)pageSeriesLabelsLeftPanel).Width - 1, 0);
			Point point2 = new Point(((Control)pageSeriesLabelsLeftPanel).Width - 1, ((Control)pageSeriesLabelsLeftPanel).Height);
			e.Graphics.DrawLine(val, point, point2);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
	{
		CheckSamplePosition();
		if (TabControl.SelectedTab == pageConditions)
		{
			BuildConditions();
		}
		else if (TabControl.SelectedTab == pageFilters)
		{
			BuildFilters();
		}
		else if (TabControl.SelectedTab == pageTrendLine)
		{
			BuildTrendLines();
		}
		else if (TabControl.SelectedTab == pageSeriesLabels)
		{
			CheckShowSeriesLabels();
		}
	}

	private void BuildConditions()
	{
		if (conditionsControl == null)
		{
			StiCloudChartConditionsCollectionControl stiCloudChartConditionsCollectionControl = new StiCloudChartConditionsCollectionControl();
			((Control)stiCloudChartConditionsCollectionControl).Dock = (DockStyle)5;
			conditionsControl = stiCloudChartConditionsCollectionControl;
			((Control)pageConditionsPanel).Controls.Add((Control)(object)conditionsControl);
		}
		StiSeries stiSeries = ((((TreeView)tvChartSeries).SelectedNode != null) ? ((StiSeries)((TreeView)tvChartSeries).SelectedNode.Tag) : null);
		if (lastConditionsSeries != stiSeries)
		{
			lastConditionsSeries = stiSeries;
			if (stiSeries == null)
			{
				((Control)conditionsControl).Enabled = false;
				conditionsControl.Clean();
			}
			else
			{
				conditionsControl.Init(stiSeries.Conditions, stiSeries);
				((Control)conditionsControl).Enabled = true;
			}
		}
	}

	private void BuildFilters()
	{
		if (filtersControl == null)
		{
			StiCloudChartFiltersCollectionControl stiCloudChartFiltersCollectionControl = new StiCloudChartFiltersCollectionControl();
			((Control)stiCloudChartFiltersCollectionControl).Dock = (DockStyle)5;
			filtersControl = stiCloudChartFiltersCollectionControl;
			((Control)pageFiltersPanel).Controls.Add((Control)(object)filtersControl);
		}
		StiSeries stiSeries = ((((TreeView)tvChartSeries).SelectedNode != null) ? ((StiSeries)((TreeView)tvChartSeries).SelectedNode.Tag) : null);
		if (lastFiltersSeries != stiSeries)
		{
			lastFiltersSeries = stiSeries;
			filtersControl.Clean();
			if (stiSeries == null)
			{
				((Control)filtersControl).Enabled = false;
				return;
			}
			filtersControl.Init(stiSeries.Filters, stiSeries);
			((Control)filtersControl).Enabled = true;
		}
	}

	private void BuildTrendLines()
	{
		if (trendLinesControl == null)
		{
			StiCloudChartTrendLinesCollectionControl stiCloudChartTrendLinesCollectionControl = new StiCloudChartTrendLinesCollectionControl();
			((Control)stiCloudChartTrendLinesCollectionControl).Dock = (DockStyle)5;
			trendLinesControl = stiCloudChartTrendLinesCollectionControl;
			((Control)pageTrendLinePanel).Controls.Add((Control)(object)trendLinesControl);
		}
		StiSeries stiSeries = ((((TreeView)tvChartSeries).SelectedNode != null) ? ((StiSeries)((TreeView)tvChartSeries).SelectedNode.Tag) : null);
		if (lastTrendLinesSeries != stiSeries)
		{
			lastTrendLinesSeries = stiSeries;
			if (stiSeries == null)
			{
				((Control)trendLinesControl).Enabled = false;
				trendLinesControl.Clean();
			}
			else
			{
				trendLinesControl.Init(stiSeries.TrendLines, stiSeries);
				((Control)trendLinesControl).Enabled = true;
			}
		}
	}

	private void BuildSeries()
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		((TreeView)tvChartSeries).Nodes.Clear();
		if (chartWindow.Chart.Series.Count <= 0)
		{
			return;
		}
		foreach (StiSeries item in chartWindow.Chart.Series)
		{
			TreeNode val = new TreeNode
			{
				Tag = item,
				Text = string.Concat(item.Title, " ", item?.ToString())
			};
			((TreeView)tvChartSeries).Nodes.Add(val);
		}
		((TreeView)tvChartSeries).SelectedNode = ((TreeView)tvChartSeries).Nodes[0];
	}

	private void UpdateStateSeries()
	{
		if (((TreeView)tvChartSeries).Nodes.Count > 0 && ((TreeView)tvChartSeries).SelectedNode != null)
		{
			TreeNode selectedNode = ((TreeView)tvChartSeries).SelectedNode;
			int num = ((TreeView)tvChartSeries).Nodes.IndexOf(selectedNode);
			if (num == 0)
			{
				buttonUpSeries.Enabled = false;
			}
			else
			{
				buttonUpSeries.Enabled = true;
			}
			if (num == ((TreeView)tvChartSeries).Nodes.Count - 1)
			{
				buttonDownSeries.Enabled = false;
			}
			else
			{
				buttonDownSeries.Enabled = true;
			}
		}
		else
		{
			buttonUpSeries.Enabled = false;
			buttonDownSeries.Enabled = false;
		}
		buttonRemoveSeries.Enabled = ((TreeView)tvChartSeries).Nodes.Count > 0;
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
			StiResourcesHelper.SetImage(typeDoughnut, "Stimulsoft.Report.Design.Resources.Office2013.Chart.categoryOther.png");
			StiResourcesHelper.SetImage(type3dPie, "Stimulsoft.Report.Design.Resources.Office2013.Chart.Pie3d.png");
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
			StiResourcesHelper.SetImage(categoryBoxAndWhisker, "Stimulsoft.Report.Design.Resources.Office2013.Chart.BoxAndWhisker.png");
			StiResourcesHelper.SetImage(typeBoxAndWhisker, "Stimulsoft.Report.Design.Resources.Office2013.Chart.BoxAndWhisker.png");
			StiResourcesHelper.SetImage(categoryWaterfall, "Stimulsoft.Report.Design.Resources.Office2013.Chart.Waterfall.png");
			StiResourcesHelper.SetImage(typeWaterfall, "Stimulsoft.Report.Design.Resources.Office2013.Chart.Waterfall.png");
			StiResourcesHelper.SetImage(categoryPictorial, "Stimulsoft.Report.Design.Resources.Office2013.Chart.Pictorial.png");
			StiResourcesHelper.SetImage(typePictorial, "Stimulsoft.Report.Design.Resources.Office2013.Chart.Pictorial.png");
		}
	}

	private void BuildTabItemsSeries()
	{
		TabControl.SelectedIndex = 0;
		pageConditions.Invisible = true;
		pageFilters.Invisible = true;
		pageInteraction.Invisible = true;
		pageLineMarker.Invisible = true;
		pageMarker.Invisible = true;
		pageSeriesLabels.Invisible = true;
		pageTopN.Invisible = true;
		pageTrendLine.Invisible = true;
		if (((TreeView)tvChartSeries).SelectedNode != null)
		{
			StiSeries stiSeries = (StiSeries)((TreeView)tvChartSeries).SelectedNode.Tag;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = false;
			switch (stiSeries.ComponentId)
			{
			case StiComponentId.StiWaterfallSeries:
				flag = true;
				flag2 = true;
				flag3 = true;
				break;
			case StiComponentId.StiClusteredColumnSeries:
			case StiComponentId.StiClusteredBarSeries:
				flag = true;
				flag2 = true;
				flag3 = true;
				flag4 = true;
				break;
			case StiComponentId.StiStackedColumnSeries:
			case StiComponentId.StiFullStackedColumnSeries:
			case StiComponentId.StiStackedBarSeries:
			case StiComponentId.StiFullStackedBarSeries:
			case StiComponentId.StiPieSeries:
			case StiComponentId.StiDoughnutSeries:
			case StiComponentId.StiFunnelSeries:
			case StiComponentId.StiFunnelWeightedSlicesSeries:
				flag = true;
				flag2 = true;
				flag3 = true;
				break;
			case StiComponentId.StiLineSeries:
			case StiComponentId.StiSteppedLineSeries:
			case StiComponentId.StiAreaSeries:
			case StiComponentId.StiSteppedAreaSeries:
				flag = true;
				flag5 = true;
				flag6 = true;
				flag2 = true;
				flag3 = true;
				flag4 = true;
				break;
			case StiComponentId.StiStackedLineSeries:
			case StiComponentId.StiStackedAreaSeries:
			case StiComponentId.StiFullStackedLineSeries:
			case StiComponentId.StiFullStackedAreaSeries:
				flag = true;
				flag5 = true;
				flag6 = true;
				flag2 = true;
				flag3 = true;
				break;
			case StiComponentId.StiSplineSeries:
			case StiComponentId.StiSplineAreaSeries:
				flag = true;
				flag6 = true;
				flag2 = true;
				flag3 = true;
				flag4 = true;
				break;
			case StiComponentId.StiStackedSplineSeries:
			case StiComponentId.StiStackedSplineAreaSeries:
			case StiComponentId.StiFullStackedSplineSeries:
			case StiComponentId.StiFullStackedSplineAreaSeries:
			case StiComponentId.StiRadarAreaSeries:
			case StiComponentId.StiRadarLineSeries:
			case StiComponentId.StiRadarPointSeries:
				flag = true;
				flag6 = true;
				flag2 = true;
				flag3 = true;
				break;
			case StiComponentId.StiRangeSeries:
			case StiComponentId.StiSteppedRangeSeries:
				flag5 = true;
				flag6 = true;
				flag2 = true;
				flag3 = true;
				break;
			case StiComponentId.StiSplineRangeSeries:
				flag6 = true;
				flag2 = true;
				flag3 = true;
				break;
			case StiComponentId.StiParetoSeries:
			case StiComponentId.StiGanttSeries:
			case StiComponentId.StiRangeBarSeries:
			case StiComponentId.StiCandlestickSeries:
			case StiComponentId.StiStockSeries:
				flag2 = true;
				flag3 = true;
				break;
			case StiComponentId.StiScatterLineSeries:
			case StiComponentId.StiScatterSplineSeries:
				flag = true;
				flag5 = true;
				flag6 = true;
				flag2 = true;
				flag3 = true;
				flag4 = true;
				break;
			case StiComponentId.StiScatterSeries:
				flag = true;
				flag6 = true;
				flag2 = true;
				flag3 = true;
				flag4 = true;
				break;
			case StiComponentId.StiBubbleSeries:
				flag2 = true;
				flag3 = true;
				flag4 = true;
				break;
			case StiComponentId.StiTreemapSeries:
				flag = true;
				flag2 = true;
				flag3 = true;
				break;
			case StiComponentId.StiPictorialSeries:
				flag2 = true;
				break;
			}
			if (stiSeries.ComponentId != StiComponentId.StiBoxAndWhiskerSeries)
			{
				pageConditions.Invisible = false;
				pageFilters.Invisible = false;
			}
			if (flag)
			{
				((Control)pageTopN).Tag = stiSeries.TopN;
				pageTopN.Invisible = false;
			}
			if (flag2)
			{
				((Control)pageInteraction).Tag = stiSeries.Interaction;
				pageInteraction.Invisible = false;
			}
			if (flag3)
			{
				((Control)pageSeriesLabels).Tag = stiSeries.SeriesLabels;
				pageSeriesLabels.Invisible = false;
			}
			if (flag4)
			{
				((Control)pageTrendLine).Tag = stiSeries.TrendLines;
				pageTrendLine.Invisible = false;
			}
			if (flag5)
			{
				PropertyInfo property = stiSeries.GetType().GetProperty("LineMarker");
				((Control)pageLineMarker).Tag = property.GetValue(stiSeries, null);
				pageLineMarker.Invisible = false;
			}
			if (flag6)
			{
				PropertyInfo property2 = stiSeries.GetType().GetProperty("Marker");
				((Control)pageMarker).Tag = property2.GetValue(stiSeries, null);
				pageMarker.Invisible = false;
			}
		}
	}

	private void tvChartSeries_AfterSelect(object sender, TreeViewEventArgs e)
	{
		if (((TreeView)tvChartSeries).SelectedNode == null)
		{
			((PropertyGrid)chartWindow.PropertyGrid).SelectedObject = null;
		}
		else
		{
			((PropertyGrid)chartWindow.PropertyGrid).SelectedObject = ((TreeView)tvChartSeries).SelectedNode.Tag;
		}
		UpdateStateSeries();
		BuildTabItemsSeries();
		SaveFilters();
		SaveConditions();
		SaveTrendLines();
		if (TabControl.SelectedTab == pageConditions)
		{
			BuildConditions();
		}
	}

	private void menuAddSeries_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		if (!isSeriesChanged)
		{
			return;
		}
		InitSeriesIcons();
		Type[] array = ((chartWindow.Chart.Area != null) ? chartWindow.Chart.Area.GetSeriesTypes() : new Type[0]);
		foreach (ButtonItem subItem in menuAddSeries.SubItems)
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
				if (chartWindow.Chart.Series.Count == 0)
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
		categoryPie.Enabled = typePie.Enabled || type3dPie.Enabled || typeDoughnut.Enabled;
		categoryRadar.Enabled = typeRadarPoint.Enabled || typeRadarLine.Enabled || typeRadarArea.Enabled;
		categoryFunnel.Enabled = typeFunnel.Enabled || typeFunnelWeightedSlices.Enabled;
		categoryFinancial.Enabled = typeCandlestick.Enabled || typeStock.Enabled;
		categoryTreemap.Enabled = typeTreemap.Enabled;
		categorySunburst.Enabled = typeSunburst.Enabled;
		categoryHistogram.Enabled = typeHistogram.Enabled || typePareto.Enabled;
		categoryBoxAndWhisker.Enabled = typeBoxAndWhisker.Enabled;
		categoryWaterfall.Enabled = typeWaterfall.Enabled;
		categoryPictorial.Enabled = typePictorial.Enabled;
	}

	private void typeAddSetiesClick(object sender, EventArgs e)
	{
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		isSeriesChanged = true;
		ButtonItem buttonItem = (ButtonItem)sender;
		StiSeries stiSeries = (StiSeries)StiActivator.CreateObject((Type)buttonItem.Tag);
		chartWindow.Chart.Series.Add(stiSeries);
		stiSeries.Core?.ApplyStyle(chartWindow.Chart.Style, chartWindow.Chart.Style.Core.GetColorBySeries(stiSeries, stiSeries.Core?.SeriesColors));
		TreeNode val = new TreeNode
		{
			Tag = stiSeries,
			Text = string.Concat(stiSeries.Title, " ", stiSeries?.ToString())
		};
		((TreeView)tvChartSeries).Nodes.Add(val);
		((TreeView)tvChartSeries).SelectedNode = val;
		chartWindow.UpdateSample();
		((Control)tvChartSeries).Focus();
	}

	private void buttonRemoveSeriesClick(object sender, EventArgs e)
	{
		isSeriesChanged = true;
		StiSeries value = (StiSeries)((TreeView)tvChartSeries).SelectedNode.Tag;
		int num = chartWindow.Chart.Series.IndexOf(value);
		chartWindow.Chart.Series.RemoveAt(num);
		((TreeView)tvChartSeries).Nodes.RemoveAt(num);
		if (((TreeView)tvChartSeries).Nodes.Count > 1)
		{
			if (num == ((TreeView)tvChartSeries).Nodes.Count - 1)
			{
				num--;
			}
		}
		else
		{
			num = -1;
		}
		if (num > 0)
		{
			((TreeView)tvChartSeries).SelectedNode = ((TreeView)tvChartSeries).Nodes[num - 1];
		}
		else
		{
			tvChartSeries_AfterSelect(null, null);
		}
		chartWindow.UpdateSample();
		((Control)tvChartSeries).Focus();
	}

	private void buttonUpSeriesClick(object sender, EventArgs e)
	{
		TreeNode selectedNode = ((TreeView)tvChartSeries).SelectedNode;
		StiSeries value = (StiSeries)selectedNode.Tag;
		int num = ((TreeView)tvChartSeries).Nodes.IndexOf(selectedNode);
		((TreeView)tvChartSeries).Nodes.RemoveAt(num);
		chartWindow.Chart.Series.RemoveAt(num);
		num--;
		((TreeView)tvChartSeries).Nodes.Insert(num, selectedNode);
		chartWindow.Chart.Series.Insert(num, value);
		((TreeView)tvChartSeries).SelectedNode = selectedNode;
		chartWindow.UpdateSample();
		((Control)tvChartSeries).Focus();
	}

	private void buttonDownSeriesClick(object sender, EventArgs e)
	{
		TreeNode selectedNode = ((TreeView)tvChartSeries).SelectedNode;
		StiSeries value = (StiSeries)selectedNode.Tag;
		int num = ((TreeView)tvChartSeries).Nodes.IndexOf(selectedNode);
		((TreeView)tvChartSeries).Nodes.RemoveAt(num);
		chartWindow.Chart.Series.RemoveAt(num);
		num++;
		((TreeView)tvChartSeries).Nodes.Insert(num, selectedNode);
		chartWindow.Chart.Series.Insert(num, value);
		((TreeView)tvChartSeries).SelectedNode = selectedNode;
		chartWindow.UpdateSample();
		((Control)tvChartSeries).Focus();
	}

	internal void Collect()
	{
		if (timerSeriesLabels != null)
		{
			timerSeriesLabels.Tick -= timerSeriesLabelsTick;
			((Component)(object)timerSeriesLabels).Dispose();
			timerSeriesLabels = null;
		}
		if (listSeriesLabelsImages == null)
		{
			return;
		}
		foreach (StiSeriesImageInfo listSeriesLabelsImage in listSeriesLabelsImages)
		{
			((Image)listSeriesLabelsImage.Bitmap).Dispose();
		}
		listSeriesLabelsImages.Clear();
		listSeriesLabelsImages = null;
	}

	private void CheckShowSeriesLabels()
	{
		if (((TreeView)tvChartSeries).SelectedNode == null)
		{
			((Control)footerPanelSeriesLabels).Visible = false;
			return;
		}
		lockRadioShowSeriesLabels = true;
		((Control)footerPanelSeriesLabels).Visible = true;
		StiSeries stiSeries = (StiSeries)((TreeView)tvChartSeries).SelectedNode.Tag;
		if (stiSeries.ShowSeriesLabels == StiShowSeriesLabels.FromChart)
		{
			radioFromChart.Checked = true;
		}
		else
		{
			radioFromSeries.Checked = true;
		}
		lockRadioShowSeriesLabels = false;
	}

	private void BuildSeriesLabels()
	{
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Expected O, but got Unknown
		seriesLabelsImagesMouseOverIndex = -1;
		if (listSeriesLabelsImages == null)
		{
			listSeriesLabelsImages = new List<StiSeriesImageInfo>();
		}
		StiSeries stiSeries = null;
		if (((TreeView)tvChartSeries).SelectedNode != null)
		{
			stiSeries = (StiSeries)((TreeView)tvChartSeries).SelectedNode.Tag;
		}
		if (listSeriesLabelsImages.Count > 0 && lastSeries == stiSeries && lastSeriesLabelsStyle == chartWindow.Chart.Style.GetType())
		{
			((Control)panelSeriesLabels).Invalidate();
			return;
		}
		seriesLabelsImagesSelectedIndex = -1;
		foreach (StiSeriesImageInfo listSeriesLabelsImage in listSeriesLabelsImages)
		{
			((Image)listSeriesLabelsImage.Bitmap).Dispose();
		}
		listSeriesLabelsImages.Clear();
		lastSeries = stiSeries;
		lastSeriesLabelsStyle = chartWindow.Chart.Style.GetType();
		StiChart simpleChart = StiSimpleChartHelper.GetSimpleChart(chartWindow.Chart);
		simpleChart.Series[0].ShowSeriesLabels = StiShowSeriesLabels.FromSeries;
		simpleChart.Series.Remove(simpleChart.Series[1]);
		List<StiSeriesLabels> list = new List<StiSeriesLabels>();
		StiAreaCoreXF core = chartWindow.Chart.Area.Core;
		foreach (StiSeriesLabels item in StiOptions.Services.ChartSerieLabels.Where((StiSeriesLabels s) => s.ServiceEnabled))
		{
			if (core.CheckInLabelsTypes(item.GetType()))
			{
				list.Add(item);
			}
		}
		int num = StiScale.I4;
		int num2 = StiScale.I2;
		int num3 = StiScale.I(80);
		int num4 = StiScale.I(80);
		int width = StiScale.I(86);
		int height = StiScale.I(116);
		bool flag = false;
		if (list.Count < 6)
		{
			num3 = StiScale.I(160);
			num4 = StiScale.I(110);
			width = StiScale.I(164);
			height = StiScale.I(146);
			flag = true;
		}
		Type type = ((stiSeries != null && stiSeries.ShowSeriesLabels == StiShowSeriesLabels.FromChart) ? chartWindow.Chart.SeriesLabels.GetType() : stiSeries.SeriesLabels.GetType());
		int num5 = 0;
		foreach (StiSeriesLabels item2 in list)
		{
			if (chartWindow.Chart.Area.Core.CheckInLabelsTypes(item2.GetType()))
			{
				simpleChart.Series[0].SeriesLabels = (StiSeriesLabels)item2.Clone();
				RectangleF rect = new RectangleF(0f, 0f, num3, num4);
				Bitmap val = new Bitmap(num3, num4);
				Graphics val2 = Graphics.FromImage((Image)(object)val);
				try
				{
					double zoom = simpleChart.Report.Info.Zoom;
					simpleChart.Report.Info.Zoom = 1.0;
					simpleChart.DrawChart(val2, rect, useMargins: false, useBackground: false);
					simpleChart.Report.Info.Zoom = zoom;
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
				listSeriesLabelsImages.Add(new StiSeriesImageInfo(val, item2, num, num2, width, height, num3, num4));
				if (type == item2.GetType())
				{
					seriesLabelsImagesSelectedIndex = num5;
				}
				if (flag)
				{
					num2 += StiScale.I(151);
				}
				else if (num5 % 2 == 0)
				{
					num += StiScale.I(94);
				}
				else
				{
					num = StiScale.I4;
					num2 += StiScale.I(121);
				}
				num5++;
			}
		}
		panelSeriesLabels.ScrollTop = 0;
		if (flag)
		{
			panelSeriesLabels.ScrollHeight = StiScale.I(146) * listSeriesLabelsImages.Count;
		}
		else
		{
			panelSeriesLabels.ScrollHeight = StiScale.I(125) * (listSeriesLabelsImages.Count / 2);
		}
		((Control)panelSeriesLabels).Invalidate();
	}

	private void RadioShowSeriesLabelsChecked_CheckedChanged(object sender, EventArgs e)
	{
		if (!lockRadioShowSeriesLabels)
		{
			StiSeries stiSeries = (StiSeries)((TreeView)tvChartSeries).SelectedNode.Tag;
			stiSeries.ShowSeriesLabels = ((sender == radioFromChart) ? StiShowSeriesLabels.FromChart : StiShowSeriesLabels.FromSeries);
			chartWindow.UpdateSample();
		}
	}

	private void panelSeriesLabels_Paint(object sender, PaintEventArgs e)
	{
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		if (listSeriesLabelsImages == null || listSeriesLabelsImages.Count == 0)
		{
			return;
		}
		Graphics graphics = e.Graphics;
		int num = 0;
		foreach (StiSeriesImageInfo listSeriesLabelsImage in listSeriesLabelsImages)
		{
			Rectangle rectangle = new Rectangle(listSeriesLabelsImage.X, listSeriesLabelsImage.Y - ((ScrollBar)panelSeriesLabels.VScrollBar).Value, listSeriesLabelsImage.Width, listSeriesLabelsImage.Height);
			if (num == seriesLabelsImagesSelectedIndex)
			{
				SolidBrush val = new SolidBrush(Color.FromArgb(212, 224, 241));
				try
				{
					graphics.FillRectangle((Brush)(object)val, rectangle);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
				Pen val2 = new Pen(Color.FromArgb(43, 87, 154));
				try
				{
					graphics.DrawRectangle(val2, rectangle);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			else if (num == seriesLabelsImagesMouseOverIndex)
			{
				SolidBrush val3 = new SolidBrush(Color.FromArgb(212, 224, 241));
				try
				{
					graphics.FillRectangle((Brush)(object)val3, rectangle);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			Rectangle rectangle2 = new Rectangle(rectangle.X + 3, rectangle.Y + 6, listSeriesLabelsImage.ImageWidth, listSeriesLabelsImage.ImageHeight);
			graphics.DrawImage((Image)(object)listSeriesLabelsImage.Bitmap, rectangle2);
			SolidBrush val4 = new SolidBrush(((Control)panelSeriesLabels).ForeColor);
			try
			{
				StringFormat val5 = new StringFormat();
				try
				{
					val5.Alignment = (StringAlignment)1;
					val5.LineAlignment = (StringAlignment)1;
					rectangle2.Y += listSeriesLabelsImage.ImageHeight + 1;
					rectangle2.Height = 28;
					graphics.DrawString(listSeriesLabelsImage.Tag.ToString(), ((Control)panelSeriesLabels).Font, (Brush)(object)val4, (RectangleF)rectangle2, val5);
				}
				finally
				{
					((IDisposable)val5)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
			num++;
		}
	}

	private void panelSeriesLabels_MouseDown(object sender, MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button != 1048576)
		{
			return;
		}
		int num = 0;
		foreach (StiSeriesImageInfo listSeriesLabelsImage in listSeriesLabelsImages)
		{
			if (new Rectangle(listSeriesLabelsImage.X, listSeriesLabelsImage.Y - ((ScrollBar)panelSeriesLabels.VScrollBar).Value, listSeriesLabelsImage.Width, listSeriesLabelsImage.Height).Contains(e.X, e.Y))
			{
				timerSeriesLabels.Stop();
				lastConditionsSeries = null;
				seriesLabelsImagesSelectedIndex = num;
				((Control)panelSeriesLabels).Invalidate();
				StiSeries stiSeries = (StiSeries)((TreeView)tvChartSeries).SelectedNode.Tag;
				StiSeriesLabels stiSeriesLabels = StiActivator.CreateObject(listSeriesLabelsImage.Tag.GetType()) as StiSeriesLabels;
				JObject jObject = ((StiSeriesLabels)((PropertyGrid)chartWindow.PropertyGrid).SelectedObject).SaveToJsonObject(StiJsonSaveMode.Report);
				lastSeriesLabels = null;
				stiSeriesLabels.LoadFromJsonObject(jObject);
				stiSeries.SeriesLabels = stiSeriesLabels;
				stiSeries.SeriesLabels.Core.ApplyStyle(chartWindow.Chart.Style);
				((Control)pageSeriesLabels).Tag = stiSeriesLabels;
				((PropertyGrid)chartWindow.PropertyGrid).SelectedObject = stiSeriesLabels;
				chartWindow.UpdateSample();
				break;
			}
			num++;
		}
	}

	private void panelSeriesLabels_MouseMove(object sender, MouseEventArgs e)
	{
		int num = 0;
		foreach (StiSeriesImageInfo listSeriesLabelsImage in listSeriesLabelsImages)
		{
			if (new Rectangle(listSeriesLabelsImage.X, listSeriesLabelsImage.Y - ((ScrollBar)panelSeriesLabels.VScrollBar).Value, listSeriesLabelsImage.Width, listSeriesLabelsImage.Height).Contains(e.X, e.Y))
			{
				if (seriesLabelsImagesMouseOverIndex != num)
				{
					buttonSeriesLabelsMouseEnter(listSeriesLabelsImage);
					seriesLabelsImagesMouseOverIndex = num;
					((Control)panelSeriesLabels).Invalidate();
				}
				return;
			}
			num++;
		}
		if (seriesLabelsImagesMouseOverIndex != -1)
		{
			buttonSeriesLabelsMouseLeave();
			seriesLabelsImagesMouseOverIndex = -1;
			((Control)panelSeriesLabels).Invalidate();
		}
	}

	private void panelSeriesLabels_MouseLeave(object sender, EventArgs e)
	{
		if (seriesLabelsImagesMouseOverIndex != -1)
		{
			buttonSeriesLabelsMouseLeave();
			seriesLabelsImagesMouseOverIndex = -1;
			((Control)panelSeriesLabels).Invalidate();
		}
	}

	private void timerSeriesLabelsTick(object sender, EventArgs e)
	{
		timerSeriesLabels.Stop();
		if (lastSeriesLabels != null)
		{
			StiSeries stiSeries = (StiSeries)((TreeView)tvChartSeries).SelectedNode.Tag;
			StiSeriesLabels stiSeriesLabels = StiActivator.CreateObject(lastSeriesLabels.GetType()) as StiSeriesLabels;
			JObject jObject = ((StiSeriesLabels)((PropertyGrid)chartWindow.PropertyGrid).SelectedObject).SaveToJsonObject(StiJsonSaveMode.Report);
			stiSeriesLabels.LoadFromJsonObject(jObject);
			lastSeriesLabels = null;
			stiSeries.SeriesLabels = stiSeriesLabels;
			stiSeries.SeriesLabels.Core.ApplyStyle(chartWindow.Chart.Style);
			((PropertyGrid)chartWindow.PropertyGrid).SelectedObject = stiSeriesLabels;
			chartWindow.UpdateSample();
		}
	}

	private void buttonSeriesLabelsMouseLeave()
	{
		if (timerSeriesLabels != null)
		{
			timerSeriesLabels.Stop();
			timerSeriesLabels.Start();
		}
	}

	private void buttonSeriesLabelsMouseEnter(StiSeriesImageInfo info)
	{
		timerSeriesLabels.Stop();
		StiSeries stiSeries = (StiSeries)((TreeView)tvChartSeries).SelectedNode.Tag;
		if (lastSeriesLabels == null)
		{
			lastSeriesLabels = stiSeries.SeriesLabels;
		}
		StiSeriesLabels stiSeriesLabels = ((ICloneable)info.Tag).Clone() as StiSeriesLabels;
		JObject jObject = ((StiSeriesLabels)((PropertyGrid)chartWindow.PropertyGrid).SelectedObject).SaveToJsonObject(StiJsonSaveMode.Report);
		stiSeriesLabels.LoadFromJsonObject(jObject);
		stiSeries.SeriesLabels = stiSeriesLabels;
		stiSeries.SeriesLabels.Core.ApplyStyle(chartWindow.Chart.Style);
		chartWindow.UpdateSample();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0349: Expected O, but got Unknown
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Expected O, but got Unknown
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0396: Expected O, but got Unknown
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Expected O, but got Unknown
		//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Expected O, but got Unknown
		//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d8: Expected O, but got Unknown
		//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e3: Expected O, but got Unknown
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Expected O, but got Unknown
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_056c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c4: Expected O, but got Unknown
		//IL_05fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0649: Unknown result type (might be due to invalid IL or missing references)
		//IL_0653: Expected O, but got Unknown
		//IL_06c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0774: Unknown result type (might be due to invalid IL or missing references)
		//IL_077e: Expected O, but got Unknown
		//IL_07e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0806: Unknown result type (might be due to invalid IL or missing references)
		//IL_088d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0897: Expected O, but got Unknown
		//IL_2263: Unknown result type (might be due to invalid IL or missing references)
		//IL_226d: Expected O, but got Unknown
		//IL_22d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_22e1: Expected O, but got Unknown
		//IL_233f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2349: Expected O, but got Unknown
		//IL_2484: Unknown result type (might be due to invalid IL or missing references)
		//IL_24ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_255e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2568: Expected O, but got Unknown
		//IL_2590: Unknown result type (might be due to invalid IL or missing references)
		//IL_25b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2633: Unknown result type (might be due to invalid IL or missing references)
		//IL_263d: Expected O, but got Unknown
		//IL_2665: Unknown result type (might be due to invalid IL or missing references)
		//IL_2689: Unknown result type (might be due to invalid IL or missing references)
		//IL_26fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_2771: Unknown result type (might be due to invalid IL or missing references)
		//IL_277b: Expected O, but got Unknown
		//IL_27a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_27c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_284c: Unknown result type (might be due to invalid IL or missing references)
		//IL_28a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_28b3: Expected O, but got Unknown
		//IL_28d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_28fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2965: Unknown result type (might be due to invalid IL or missing references)
		//IL_296f: Expected O, but got Unknown
		//IL_2994: Unknown result type (might be due to invalid IL or missing references)
		//IL_29b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a21: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a2b: Expected O, but got Unknown
		//IL_2a53: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a77: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b06: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b10: Expected O, but got Unknown
		//IL_2b38: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c38: Expected O, but got Unknown
		//IL_2c60: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c84: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d39: Expected O, but got Unknown
		//IL_2d61: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d85: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ed5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f72: Unknown result type (might be due to invalid IL or missing references)
		//IL_301b: Unknown result type (might be due to invalid IL or missing references)
		//IL_30cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_311c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3126: Expected O, but got Unknown
		//IL_315e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3168: Expected O, but got Unknown
		//IL_3194: Unknown result type (might be due to invalid IL or missing references)
		//IL_31f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_3203: Expected O, but got Unknown
		//IL_3210: Unknown result type (might be due to invalid IL or missing references)
		//IL_321a: Expected O, but got Unknown
		//IL_323e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3248: Expected O, but got Unknown
		//IL_3278: Unknown result type (might be due to invalid IL or missing references)
		//IL_3315: Unknown result type (might be due to invalid IL or missing references)
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiCloudSeriesControl));
		panelAdditionalCommon = new Panel();
		tvChartSeriesPanel = new Panel();
		tvChartSeries = new StiTreeView();
		toolBarPanel = new StiGroupLine();
		toolBar = new Bar();
		menuAddSeries = new ButtonItem();
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
		type3dPie = new ButtonItem();
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
		buttonRemoveSeries = new ButtonItem();
		buttonUpSeries = new ButtonItem();
		buttonDownSeries = new ButtonItem();
		TabControl = new StiCloudTabulator();
		pageCommon = new StiCloudTabulatorPage();
		pageConditions = new StiCloudTabulatorPage();
		pageConditionsPanel = new Panel();
		pageFilters = new StiCloudTabulatorPage();
		pageFiltersPanel = new Panel();
		pageMarker = new StiCloudTabulatorPage();
		pageLineMarker = new StiCloudTabulatorPage();
		pageInteraction = new StiCloudTabulatorPage();
		pageTrendLine = new StiCloudTabulatorPage();
		pageTrendLinePanel = new Panel();
		pageTopN = new StiCloudTabulatorPage();
		pageSeriesLabels = new StiCloudTabulatorPage();
		footerPanelSeriesLabels = new StiGroupLine();
		radioFromSeries = new RadioButton();
		radioFromChart = new RadioButton();
		labelShowSeriesLabels = new Label();
		pageSeriesLabelsLeftPanel = new Panel();
		panelSeriesLabels = new StiMouseScrollableControl();
		labelSeriesLabel = new Label();
		((Control)panelAdditionalCommon).SuspendLayout();
		((Control)tvChartSeriesPanel).SuspendLayout();
		((Control)toolBarPanel).SuspendLayout();
		((ISupportInitialize)toolBar).BeginInit();
		((Control)TabControl).SuspendLayout();
		((Control)pageConditions).SuspendLayout();
		((Control)pageFilters).SuspendLayout();
		((Control)pageTrendLine).SuspendLayout();
		((Control)pageSeriesLabels).SuspendLayout();
		((Control)footerPanelSeriesLabels).SuspendLayout();
		((Control)pageSeriesLabelsLeftPanel).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panelAdditionalCommon).Anchor = (AnchorStyles)15;
		((Control)panelAdditionalCommon).BackColor = Color.White;
		((Control)panelAdditionalCommon).Controls.Add((Control)(object)tvChartSeriesPanel);
		((Control)panelAdditionalCommon).Controls.Add((Control)(object)toolBarPanel);
		((Control)panelAdditionalCommon).Location = new Point(2, 2);
		((Control)panelAdditionalCommon).Margin = new Padding(6, 6, 6, 6);
		((Control)panelAdditionalCommon).Name = "panelAdditionalCommon";
		((Control)panelAdditionalCommon).Size = new Size(1154, 1344);
		((Control)panelAdditionalCommon).TabIndex = 0;
		((Control)tvChartSeriesPanel).Anchor = (AnchorStyles)7;
		((Control)tvChartSeriesPanel).BackColor = Color.White;
		((Control)tvChartSeriesPanel).Controls.Add((Control)(object)tvChartSeries);
		((Control)tvChartSeriesPanel).Location = new Point(0, 52);
		((Control)tvChartSeriesPanel).Margin = new Padding(6, 6, 6, 6);
		((Control)tvChartSeriesPanel).Name = "tvChartSeriesPanel";
		((Control)tvChartSeriesPanel).Size = new Size(410, 1292);
		((Control)tvChartSeriesPanel).TabIndex = 32;
		((Control)tvChartSeriesPanel).Paint += new PaintEventHandler(Panels_Paint);
		((Control)tvChartSeries).BackColor = Color.White;
		((TreeView)tvChartSeries).BorderStyle = (BorderStyle)0;
		((Control)tvChartSeries).Location = new Point(2, 8);
		((Control)tvChartSeries).Margin = new Padding(6, 6, 6, 6);
		((Control)tvChartSeries).Name = "tvChartSeries";
		((Control)tvChartSeries).Size = new Size(400, 1277);
		((Control)tvChartSeries).TabIndex = 0;
		((TreeView)tvChartSeries).AfterSelect += new TreeViewEventHandler(tvChartSeries_AfterSelect);
		((Control)toolBarPanel).BackColor = Color.FromArgb(250, 250, 250);
		((Control)toolBarPanel).Controls.Add((Control)(object)toolBar);
		((Control)toolBarPanel).Dock = (DockStyle)1;
		toolBarPanel.LinePosition = StiLinePosition.Bottom;
		toolBarPanel.LineStyle = StiLineStyle.Dot;
		((Control)toolBarPanel).Location = new Point(0, 0);
		((Control)toolBarPanel).Margin = new Padding(6, 6, 6, 6);
		((Control)toolBarPanel).Name = "toolBarPanel";
		((Control)toolBarPanel).Size = new Size(1154, 52);
		((Control)toolBarPanel).TabIndex = 31;
		((Control)toolBar).AccessibleDescription = "DotNetBar Bar (toolBar)";
		((Control)toolBar).AccessibleName = "DotNetBar Bar";
		((Control)toolBar).AccessibleRole = (AccessibleRole)22;
		toolBar.BarType = eBarType.MenuBar;
		((Control)toolBar).Dock = (DockStyle)1;
		toolBar.DockSide = eDockSide.Top;
		toolBar.DockTabStripHeight = 28;
		((Control)toolBar).Font = new Font("Segoe UI", 9f);
		toolBar.GrabHandleStyle = eGrabHandleStyle.Office2003;
		toolBar.Items.AddRange(new BaseItem[4] { menuAddSeries, buttonRemoveSeries, buttonUpSeries, buttonDownSeries });
		toolBar.Location = new Point(0, 0);
		((Control)toolBar).Margin = new Padding(6, 6, 6, 6);
		toolBar.Name = "toolBar";
		toolBar.Padding = new Padding(24, 6, 6, 6);
		toolBar.RoundCorners = false;
		toolBar.Size = new Size(1154, 47);
		toolBar.Stretch = true;
		toolBar.Style = eDotNetBarStyle.StyleManagerControlled;
		toolBar.TabIndex = 30;
		toolBar.TabStop = false;
		menuAddSeries.AutoExpandOnClick = true;
		menuAddSeries.ButtonStyle = eButtonStyle.ImageAndText;
		menuAddSeries.Image = (Image)componentResourceManager.GetObject("menuAddSeries.Image");
		menuAddSeries.ImagePaddingHorizontal = 16;
		menuAddSeries.ImagePaddingVertical = 12;
		menuAddSeries.LockClosePopup = false;
		menuAddSeries.Name = "menuAddSeries";
		menuAddSeries.SubItems.AddRange(new BaseItem[16]
		{
			categoryClusteredColumn, categoryLine, categoryPie, categoryClusteredBar, categoryArea, categoryRange, categoryScatter, categoryRadar, categoryFunnel, categoryFinancial,
			categoryTreemap, categorySunburst, categoryHistogram, categoryBoxAndWhisker, categoryWaterfall, categoryPictorial
		});
		menuAddSeries.Text = "Add Series";
		menuAddSeries.PopupOpen += menuAddSeries_PopupOpen;
		categoryClusteredColumn.ImagePaddingHorizontal = 16;
		categoryClusteredColumn.ImagePaddingVertical = 12;
		categoryClusteredColumn.LockClosePopup = false;
		categoryClusteredColumn.Name = "categoryClusteredColumn";
		categoryClusteredColumn.SubItems.AddRange(new BaseItem[3] { typeClusteredColumn, typeStackedColumn, typeFullStackedColumn });
		categoryClusteredColumn.Text = "Clustered Column";
		typeClusteredColumn.ButtonStyle = eButtonStyle.ImageAndText;
		typeClusteredColumn.ImagePaddingHorizontal = 16;
		typeClusteredColumn.ImagePaddingVertical = 12;
		typeClusteredColumn.LockClosePopup = false;
		typeClusteredColumn.Name = "typeClusteredColumn";
		typeClusteredColumn.Text = "Clustered Column";
		typeClusteredColumn.Click += typeAddSetiesClick;
		typeStackedColumn.ButtonStyle = eButtonStyle.ImageAndText;
		typeStackedColumn.ImagePaddingHorizontal = 16;
		typeStackedColumn.ImagePaddingVertical = 12;
		typeStackedColumn.LockClosePopup = false;
		typeStackedColumn.Name = "typeStackedColumn";
		typeStackedColumn.Text = "Stacked Column";
		typeStackedColumn.Click += typeAddSetiesClick;
		typeFullStackedColumn.ButtonStyle = eButtonStyle.ImageAndText;
		typeFullStackedColumn.ImagePaddingHorizontal = 16;
		typeFullStackedColumn.ImagePaddingVertical = 12;
		typeFullStackedColumn.LockClosePopup = false;
		typeFullStackedColumn.Name = "typeFullStackedColumn";
		typeFullStackedColumn.Text = "Full Stacked Column";
		typeFullStackedColumn.Click += typeAddSetiesClick;
		categoryLine.ImagePaddingHorizontal = 16;
		categoryLine.ImagePaddingVertical = 12;
		categoryLine.LockClosePopup = false;
		categoryLine.Name = "categoryLine";
		categoryLine.SubItems.AddRange(new BaseItem[7] { typeLine, typeStackedLine, typeFullStackedLine, typeSpline, typeStackedSpline, typeFullStackedSpline, typeSteppedLine });
		categoryLine.Text = "Line";
		typeLine.ButtonStyle = eButtonStyle.ImageAndText;
		typeLine.ImagePaddingHorizontal = 16;
		typeLine.ImagePaddingVertical = 12;
		typeLine.LockClosePopup = false;
		typeLine.Name = "typeLine";
		typeLine.Text = "Line";
		typeLine.Click += typeAddSetiesClick;
		typeStackedLine.ButtonStyle = eButtonStyle.ImageAndText;
		typeStackedLine.ImagePaddingHorizontal = 16;
		typeStackedLine.ImagePaddingVertical = 12;
		typeStackedLine.LockClosePopup = false;
		typeStackedLine.Name = "typeStackedLine";
		typeStackedLine.Text = "Stacked Line";
		typeStackedLine.Click += typeAddSetiesClick;
		typeFullStackedLine.ButtonStyle = eButtonStyle.ImageAndText;
		typeFullStackedLine.ImagePaddingHorizontal = 16;
		typeFullStackedLine.ImagePaddingVertical = 12;
		typeFullStackedLine.LockClosePopup = false;
		typeFullStackedLine.Name = "typeFullStackedLine";
		typeFullStackedLine.Text = "Full Stacked Line";
		typeFullStackedLine.Click += typeAddSetiesClick;
		typeSpline.BeginGroup = true;
		typeSpline.ImagePaddingHorizontal = 16;
		typeSpline.ImagePaddingVertical = 12;
		typeSpline.LockClosePopup = false;
		typeSpline.Name = "typeSpline";
		typeSpline.Text = "Spline";
		typeSpline.Click += typeAddSetiesClick;
		typeStackedSpline.ImagePaddingHorizontal = 16;
		typeStackedSpline.ImagePaddingVertical = 12;
		typeStackedSpline.LockClosePopup = false;
		typeStackedSpline.Name = "typeStackedSpline";
		typeStackedSpline.Text = "Stacked Spline";
		typeStackedSpline.Click += typeAddSetiesClick;
		typeFullStackedSpline.ImagePaddingHorizontal = 16;
		typeFullStackedSpline.ImagePaddingVertical = 12;
		typeFullStackedSpline.LockClosePopup = false;
		typeFullStackedSpline.Name = "typeFullStackedSpline";
		typeFullStackedSpline.Text = "Full-Stacked Spline";
		typeFullStackedSpline.Click += typeAddSetiesClick;
		typeSteppedLine.BeginGroup = true;
		typeSteppedLine.ImagePaddingHorizontal = 16;
		typeSteppedLine.ImagePaddingVertical = 12;
		typeSteppedLine.LockClosePopup = false;
		typeSteppedLine.Name = "typeSteppedLine";
		typeSteppedLine.Text = "Stepped Line";
		typeSteppedLine.Click += typeAddSetiesClick;
		categoryPie.ImagePaddingHorizontal = 16;
		categoryPie.ImagePaddingVertical = 12;
		categoryPie.LockClosePopup = false;
		categoryPie.Name = "categoryPie";
		categoryPie.SubItems.AddRange(new BaseItem[3] { typePie, type3dPie, typeDoughnut });
		categoryPie.Text = "Pie";
		typePie.ImagePaddingHorizontal = 16;
		typePie.ImagePaddingVertical = 12;
		typePie.LockClosePopup = false;
		typePie.Name = "typePie";
		typePie.Text = "Pie";
		typePie.Click += typeAddSetiesClick;
		type3dPie.ImagePaddingHorizontal = 16;
		type3dPie.ImagePaddingVertical = 12;
		type3dPie.LockClosePopup = false;
		type3dPie.Name = "type3dPie";
		type3dPie.Text = "3D Pie";
		type3dPie.Click += typeAddSetiesClick;
		typeDoughnut.ImagePaddingHorizontal = 16;
		typeDoughnut.ImagePaddingVertical = 12;
		typeDoughnut.LockClosePopup = false;
		typeDoughnut.Name = "typeDoughnut";
		typeDoughnut.Text = "Doughnut";
		typeDoughnut.Click += typeAddSetiesClick;
		categoryClusteredBar.ImagePaddingHorizontal = 16;
		categoryClusteredBar.ImagePaddingVertical = 12;
		categoryClusteredBar.LockClosePopup = false;
		categoryClusteredBar.Name = "categoryClusteredBar";
		categoryClusteredBar.SubItems.AddRange(new BaseItem[4] { typeClusteredBar, typeStackedBar, typeFullStackedBar, typeGantt });
		categoryClusteredBar.Text = "Clustered Bar";
		typeClusteredBar.ImagePaddingHorizontal = 16;
		typeClusteredBar.ImagePaddingVertical = 12;
		typeClusteredBar.LockClosePopup = false;
		typeClusteredBar.Name = "typeClusteredBar";
		typeClusteredBar.Text = "Clustered Bar";
		typeClusteredBar.Click += typeAddSetiesClick;
		typeStackedBar.ImagePaddingHorizontal = 16;
		typeStackedBar.ImagePaddingVertical = 12;
		typeStackedBar.LockClosePopup = false;
		typeStackedBar.Name = "typeStackedBar";
		typeStackedBar.Text = "Stacked Bar";
		typeStackedBar.Click += typeAddSetiesClick;
		typeFullStackedBar.ImagePaddingHorizontal = 16;
		typeFullStackedBar.ImagePaddingVertical = 12;
		typeFullStackedBar.LockClosePopup = false;
		typeFullStackedBar.Name = "typeFullStackedBar";
		typeFullStackedBar.Text = "Full-Stacked Bar";
		typeFullStackedBar.Click += typeAddSetiesClick;
		typeGantt.ImagePaddingHorizontal = 16;
		typeGantt.ImagePaddingVertical = 12;
		typeGantt.LockClosePopup = false;
		typeGantt.Name = "typeGantt";
		typeGantt.Text = "Gantt";
		typeGantt.Click += typeAddSetiesClick;
		categoryArea.ImagePaddingHorizontal = 16;
		categoryArea.ImagePaddingVertical = 12;
		categoryArea.LockClosePopup = false;
		categoryArea.Name = "categoryArea";
		categoryArea.SubItems.AddRange(new BaseItem[7] { typeArea, typeStackedArea, typeFullStackedArea, typeSplineArea, typeStackedSplineArea, typeFullStackedSplineArea, typeSteppedArea });
		categoryArea.Text = "Area";
		typeArea.ImagePaddingHorizontal = 16;
		typeArea.ImagePaddingVertical = 12;
		typeArea.LockClosePopup = false;
		typeArea.Name = "typeArea";
		typeArea.Text = "Area";
		typeArea.Click += typeAddSetiesClick;
		typeStackedArea.ImagePaddingHorizontal = 16;
		typeStackedArea.ImagePaddingVertical = 12;
		typeStackedArea.LockClosePopup = false;
		typeStackedArea.Name = "typeStackedArea";
		typeStackedArea.Text = "Stacked Area";
		typeStackedArea.Click += typeAddSetiesClick;
		typeFullStackedArea.ImagePaddingHorizontal = 16;
		typeFullStackedArea.ImagePaddingVertical = 12;
		typeFullStackedArea.LockClosePopup = false;
		typeFullStackedArea.Name = "typeFullStackedArea";
		typeFullStackedArea.Text = "Full-Stacked Area";
		typeFullStackedArea.Click += typeAddSetiesClick;
		typeSplineArea.BeginGroup = true;
		typeSplineArea.ImagePaddingHorizontal = 16;
		typeSplineArea.ImagePaddingVertical = 12;
		typeSplineArea.LockClosePopup = false;
		typeSplineArea.Name = "typeSplineArea";
		typeSplineArea.Text = "Spline Area";
		typeSplineArea.Click += typeAddSetiesClick;
		typeStackedSplineArea.ImagePaddingHorizontal = 16;
		typeStackedSplineArea.ImagePaddingVertical = 12;
		typeStackedSplineArea.LockClosePopup = false;
		typeStackedSplineArea.Name = "typeStackedSplineArea";
		typeStackedSplineArea.Text = "Stacked Spline Area";
		typeStackedSplineArea.Click += typeAddSetiesClick;
		typeFullStackedSplineArea.ImagePaddingHorizontal = 16;
		typeFullStackedSplineArea.ImagePaddingVertical = 12;
		typeFullStackedSplineArea.LockClosePopup = false;
		typeFullStackedSplineArea.Name = "typeFullStackedSplineArea";
		typeFullStackedSplineArea.Text = "Full-Stacked Spline Area";
		typeFullStackedSplineArea.Click += typeAddSetiesClick;
		typeSteppedArea.BeginGroup = true;
		typeSteppedArea.ImagePaddingHorizontal = 16;
		typeSteppedArea.ImagePaddingVertical = 12;
		typeSteppedArea.LockClosePopup = false;
		typeSteppedArea.Name = "typeSteppedArea";
		typeSteppedArea.Text = "Stepped Area";
		typeSteppedArea.Click += typeAddSetiesClick;
		categoryRange.ImagePaddingHorizontal = 16;
		categoryRange.ImagePaddingVertical = 12;
		categoryRange.LockClosePopup = false;
		categoryRange.Name = "categoryRange";
		categoryRange.SubItems.AddRange(new BaseItem[4] { typeRange, typeSplineRange, typeSteppedRange, typeRangeBar });
		categoryRange.Text = "Range";
		typeRange.ImagePaddingHorizontal = 16;
		typeRange.ImagePaddingVertical = 12;
		typeRange.LockClosePopup = false;
		typeRange.Name = "typeRange";
		typeRange.Text = "Line Range";
		typeRange.Click += typeAddSetiesClick;
		typeSplineRange.ImagePaddingHorizontal = 16;
		typeSplineRange.ImagePaddingVertical = 12;
		typeSplineRange.LockClosePopup = false;
		typeSplineRange.Name = "typeSplineRange";
		typeSplineRange.Text = "Spline Range";
		typeSplineRange.Click += typeAddSetiesClick;
		typeSteppedRange.ImagePaddingHorizontal = 16;
		typeSteppedRange.ImagePaddingVertical = 12;
		typeSteppedRange.LockClosePopup = false;
		typeSteppedRange.Name = "typeSteppedRange";
		typeSteppedRange.Text = "Stepped Range";
		typeSteppedRange.Click += typeAddSetiesClick;
		typeRangeBar.ImagePaddingHorizontal = 16;
		typeRangeBar.ImagePaddingVertical = 12;
		typeRangeBar.LockClosePopup = false;
		typeRangeBar.Name = "typeRangeBar";
		typeRangeBar.Text = "Range Bar";
		typeRangeBar.Click += typeAddSetiesClick;
		categoryScatter.ImagePaddingHorizontal = 16;
		categoryScatter.ImagePaddingVertical = 12;
		categoryScatter.LockClosePopup = false;
		categoryScatter.Name = "categoryScatter";
		categoryScatter.SubItems.AddRange(new BaseItem[4] { typeScatter, typeScatterLine, typeScatterSpline, typeBubble });
		categoryScatter.Text = "Scatter";
		typeScatter.ImagePaddingHorizontal = 16;
		typeScatter.ImagePaddingVertical = 12;
		typeScatter.LockClosePopup = false;
		typeScatter.Name = "typeScatter";
		typeScatter.Text = "Scatter";
		typeScatter.Click += typeAddSetiesClick;
		typeScatterLine.ImagePaddingHorizontal = 16;
		typeScatterLine.ImagePaddingVertical = 12;
		typeScatterLine.LockClosePopup = false;
		typeScatterLine.Name = "typeScatterLine";
		typeScatterLine.Text = "Scatter Line";
		typeScatterLine.Click += typeAddSetiesClick;
		typeScatterSpline.ImagePaddingHorizontal = 16;
		typeScatterSpline.ImagePaddingVertical = 12;
		typeScatterSpline.LockClosePopup = false;
		typeScatterSpline.Name = "typeScatterSpline";
		typeScatterSpline.Text = "Scatter Spline";
		typeScatterSpline.Click += typeAddSetiesClick;
		typeBubble.ImagePaddingHorizontal = 16;
		typeBubble.ImagePaddingVertical = 12;
		typeBubble.LockClosePopup = false;
		typeBubble.Name = "typeBubble";
		typeBubble.Text = "Bubble";
		typeBubble.Click += typeAddSetiesClick;
		categoryRadar.ImagePaddingHorizontal = 16;
		categoryRadar.ImagePaddingVertical = 12;
		categoryRadar.LockClosePopup = false;
		categoryRadar.Name = "categoryRadar";
		categoryRadar.SubItems.AddRange(new BaseItem[3] { typeRadarPoint, typeRadarLine, typeRadarArea });
		categoryRadar.Text = "Radar";
		typeRadarPoint.ImagePaddingHorizontal = 16;
		typeRadarPoint.ImagePaddingVertical = 12;
		typeRadarPoint.LockClosePopup = false;
		typeRadarPoint.Name = "typeRadarPoint";
		typeRadarPoint.Text = "Radar Point";
		typeRadarPoint.Click += typeAddSetiesClick;
		typeRadarLine.ImagePaddingHorizontal = 16;
		typeRadarLine.ImagePaddingVertical = 12;
		typeRadarLine.LockClosePopup = false;
		typeRadarLine.Name = "typeRadarLine";
		typeRadarLine.Text = "Radar Line";
		typeRadarLine.Click += typeAddSetiesClick;
		typeRadarArea.ImagePaddingHorizontal = 16;
		typeRadarArea.ImagePaddingVertical = 12;
		typeRadarArea.LockClosePopup = false;
		typeRadarArea.Name = "typeRadarArea";
		typeRadarArea.Text = "Radar Area";
		typeRadarArea.Click += typeAddSetiesClick;
		categoryFunnel.ImagePaddingHorizontal = 16;
		categoryFunnel.ImagePaddingVertical = 12;
		categoryFunnel.LockClosePopup = false;
		categoryFunnel.Name = "categoryFunnel";
		categoryFunnel.SubItems.AddRange(new BaseItem[2] { typeFunnel, typeFunnelWeightedSlices });
		categoryFunnel.Text = "Funnel";
		typeFunnel.ImagePaddingHorizontal = 16;
		typeFunnel.ImagePaddingVertical = 12;
		typeFunnel.LockClosePopup = false;
		typeFunnel.Name = "typeFunnel";
		typeFunnel.Text = "Funnel";
		typeFunnel.Click += typeAddSetiesClick;
		typeFunnelWeightedSlices.ImagePaddingHorizontal = 16;
		typeFunnelWeightedSlices.ImagePaddingVertical = 12;
		typeFunnelWeightedSlices.LockClosePopup = false;
		typeFunnelWeightedSlices.Name = "typeFunnelWeightedSlices";
		typeFunnelWeightedSlices.Text = "Funnel Weighted Slices";
		typeFunnelWeightedSlices.Click += typeAddSetiesClick;
		categoryFinancial.ImagePaddingHorizontal = 16;
		categoryFinancial.ImagePaddingVertical = 12;
		categoryFinancial.LockClosePopup = false;
		categoryFinancial.Name = "categoryFinancial";
		categoryFinancial.SubItems.AddRange(new BaseItem[2] { typeCandlestick, typeStock });
		categoryFinancial.Text = "Financial";
		typeCandlestick.ImagePaddingHorizontal = 16;
		typeCandlestick.ImagePaddingVertical = 12;
		typeCandlestick.LockClosePopup = false;
		typeCandlestick.Name = "typeCandlestick";
		typeCandlestick.Text = "Candlestick";
		typeCandlestick.Click += typeAddSetiesClick;
		typeStock.ImagePaddingHorizontal = 16;
		typeStock.ImagePaddingVertical = 12;
		typeStock.LockClosePopup = false;
		typeStock.Name = "typeStock";
		typeStock.Text = "Stock";
		typeStock.Click += typeAddSetiesClick;
		categoryTreemap.ImagePaddingHorizontal = 16;
		categoryTreemap.ImagePaddingVertical = 12;
		categoryTreemap.LockClosePopup = false;
		categoryTreemap.Name = "categoryTreemap";
		categoryTreemap.SubItems.AddRange(new BaseItem[1] { typeTreemap });
		categoryTreemap.Text = "Treemap";
		typeTreemap.ImagePaddingHorizontal = 16;
		typeTreemap.ImagePaddingVertical = 12;
		typeTreemap.LockClosePopup = false;
		typeTreemap.Name = "typeTreemap";
		typeTreemap.Text = "Treemap";
		typeTreemap.Click += typeAddSetiesClick;
		categorySunburst.ImagePaddingHorizontal = 16;
		categorySunburst.ImagePaddingVertical = 12;
		categorySunburst.LockClosePopup = false;
		categorySunburst.Name = "categorySunburst";
		categorySunburst.SubItems.AddRange(new BaseItem[1] { typeSunburst });
		categorySunburst.Text = "Sunburst";
		typeSunburst.ImagePaddingHorizontal = 16;
		typeSunburst.ImagePaddingVertical = 12;
		typeSunburst.LockClosePopup = false;
		typeSunburst.Name = "typeSunburst";
		typeSunburst.Text = "Sunburst";
		typeSunburst.Click += typeAddSetiesClick;
		categoryHistogram.ImagePaddingHorizontal = 16;
		categoryHistogram.ImagePaddingVertical = 12;
		categoryHistogram.LockClosePopup = false;
		categoryHistogram.Name = "categoryHistogram";
		categoryHistogram.SubItems.AddRange(new BaseItem[2] { typeHistogram, typePareto });
		categoryHistogram.Text = "Histogram";
		typeHistogram.ButtonStyle = eButtonStyle.ImageAndText;
		typeHistogram.ImagePaddingHorizontal = 16;
		typeHistogram.ImagePaddingVertical = 12;
		typeHistogram.LockClosePopup = false;
		typeHistogram.Name = "typeHistogram";
		typeHistogram.Text = "Histogram";
		typeHistogram.Click += typeAddSetiesClick;
		typePareto.ButtonStyle = eButtonStyle.ImageAndText;
		typePareto.ImagePaddingHorizontal = 16;
		typePareto.ImagePaddingVertical = 12;
		typePareto.LockClosePopup = false;
		typePareto.Name = "typePareto";
		typePareto.Text = "Pareto";
		typePareto.Click += typeAddSetiesClick;
		categoryBoxAndWhisker.ImagePaddingHorizontal = 16;
		categoryBoxAndWhisker.ImagePaddingVertical = 12;
		categoryBoxAndWhisker.LockClosePopup = false;
		categoryBoxAndWhisker.Name = "categoryBoxAndWhisker";
		categoryBoxAndWhisker.SubItems.AddRange(new BaseItem[1] { typeBoxAndWhisker });
		categoryBoxAndWhisker.Text = "BoxAndWhisker";
		typeBoxAndWhisker.ButtonStyle = eButtonStyle.ImageAndText;
		typeBoxAndWhisker.ImagePaddingHorizontal = 16;
		typeBoxAndWhisker.ImagePaddingVertical = 12;
		typeBoxAndWhisker.LockClosePopup = false;
		typeBoxAndWhisker.Name = "typeBoxAndWhisker";
		typeBoxAndWhisker.Text = "BoxAndWhisker";
		typeBoxAndWhisker.Click += typeAddSetiesClick;
		categoryWaterfall.ImagePaddingHorizontal = 16;
		categoryWaterfall.ImagePaddingVertical = 12;
		categoryWaterfall.LockClosePopup = false;
		categoryWaterfall.Name = "categoryWaterfall";
		categoryWaterfall.SubItems.AddRange(new BaseItem[1] { typeWaterfall });
		categoryWaterfall.Text = "Waterfall";
		typeWaterfall.ImagePaddingHorizontal = 16;
		typeWaterfall.ImagePaddingVertical = 12;
		typeWaterfall.LockClosePopup = false;
		typeWaterfall.Name = "typeWaterfall";
		typeWaterfall.Text = "Waterfall";
		typeWaterfall.Click += typeAddSetiesClick;
		categoryPictorial.ImagePaddingHorizontal = 16;
		categoryPictorial.ImagePaddingVertical = 12;
		categoryPictorial.LockClosePopup = false;
		categoryPictorial.Name = "categoryPictorial";
		categoryPictorial.SubItems.AddRange(new BaseItem[1] { typePictorial });
		categoryPictorial.Text = "Other";
		typePictorial.ImagePaddingHorizontal = 16;
		typePictorial.ImagePaddingVertical = 12;
		typePictorial.LockClosePopup = false;
		typePictorial.Name = "typePictorial";
		typePictorial.Text = "Pictorial";
		typePictorial.Click += typeAddSetiesClick;
		buttonRemoveSeries.Image = (Image)componentResourceManager.GetObject("buttonRemoveSeries.Image");
		buttonRemoveSeries.ImagePaddingHorizontal = 16;
		buttonRemoveSeries.ImagePaddingVertical = 12;
		buttonRemoveSeries.LockClosePopup = false;
		buttonRemoveSeries.Name = "buttonRemoveSeries";
		buttonRemoveSeries.Click += buttonRemoveSeriesClick;
		buttonUpSeries.BeginGroup = true;
		buttonUpSeries.Image = (Image)componentResourceManager.GetObject("buttonUpSeries.Image");
		buttonUpSeries.ImagePaddingHorizontal = 16;
		buttonUpSeries.ImagePaddingVertical = 12;
		buttonUpSeries.LockClosePopup = false;
		buttonUpSeries.Name = "buttonUpSeries";
		buttonUpSeries.Click += buttonUpSeriesClick;
		buttonDownSeries.Image = (Image)componentResourceManager.GetObject("buttonDownSeries.Image");
		buttonDownSeries.ImagePaddingHorizontal = 16;
		buttonDownSeries.ImagePaddingVertical = 12;
		buttonDownSeries.LockClosePopup = false;
		buttonDownSeries.Name = "buttonDownSeries";
		buttonDownSeries.Click += buttonDownSeriesClick;
		((Control)TabControl).Controls.Add((Control)(object)pageCommon);
		((Control)TabControl).Controls.Add((Control)(object)pageConditions);
		((Control)TabControl).Controls.Add((Control)(object)pageFilters);
		((Control)TabControl).Controls.Add((Control)(object)pageMarker);
		((Control)TabControl).Controls.Add((Control)(object)pageLineMarker);
		((Control)TabControl).Controls.Add((Control)(object)pageInteraction);
		((Control)TabControl).Controls.Add((Control)(object)pageTrendLine);
		((Control)TabControl).Controls.Add((Control)(object)pageTopN);
		((Control)TabControl).Controls.Add((Control)(object)pageSeriesLabels);
		((Control)TabControl).Dock = (DockStyle)5;
		((Control)TabControl).Location = new Point(0, 0);
		((Control)TabControl).Margin = new Padding(6, 6, 6, 6);
		((Control)TabControl).Name = "TabControl";
		((Control)TabControl).Padding = new Padding(0, 0, 128, 0);
		TabControl.ScrollHeight = 0;
		TabControl.ScrollWidth = 100;
		((Control)TabControl).Size = new Size(1400, 1346);
		((Control)TabControl).TabIndex = 1;
		TabControl.TitleBackColor = SystemColors.Control;
		TabControl.TitleInLine = false;
		TabControl.TitlePosition = StiTabTitlePosition.RightHorizontal;
		TabControl.TitleWidth = 110;
		TabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
		pageCommon.Dock = (DockStyle)5;
		pageCommon.Image = (Image)componentResourceManager.GetObject("pageCommon.Image");
		pageCommon.InvisibleVisible = false;
		((Control)pageCommon).Location = new Point(0, 0);
		((Control)pageCommon).Margin = new Padding(6, 6, 6, 6);
		((Control)pageCommon).Name = "pageCommon";
		((Control)pageCommon).Padding = new Padding(0, 0, 2, 0);
		((Control)pageCommon).Size = new Size(1272, 1346);
		((Control)pageCommon).TabIndex = 0;
		((Control)pageCommon).Text = "Common";
		((Control)pageCommon).Visible = false;
		((Control)pageConditions).Controls.Add((Control)(object)pageConditionsPanel);
		pageConditions.Dock = (DockStyle)5;
		pageConditions.Image = (Image)componentResourceManager.GetObject("pageConditions.Image");
		pageConditions.InvisibleVisible = false;
		((Control)pageConditions).Location = new Point(0, 0);
		((Control)pageConditions).Margin = new Padding(6, 6, 6, 6);
		((Control)pageConditions).Name = "pageConditions";
		((Control)pageConditions).Padding = new Padding(0, 0, 2, 0);
		((Control)pageConditions).Size = new Size(1162, 1346);
		((Control)pageConditions).TabIndex = 1;
		((Control)pageConditions).Text = "Conditions";
		((Control)pageConditions).Visible = false;
		((Control)pageConditionsPanel).Anchor = (AnchorStyles)15;
		((Control)pageConditionsPanel).Location = new Point(6, 6);
		((Control)pageConditionsPanel).Margin = new Padding(6, 6, 6, 6);
		((Control)pageConditionsPanel).Name = "pageConditionsPanel";
		((Control)pageConditionsPanel).Size = new Size(1150, 1335);
		((Control)pageConditionsPanel).TabIndex = 0;
		((Control)pageFilters).Controls.Add((Control)(object)pageFiltersPanel);
		pageFilters.Dock = (DockStyle)5;
		pageFilters.Image = (Image)componentResourceManager.GetObject("pageFilters.Image");
		pageFilters.InvisibleVisible = false;
		((Control)pageFilters).Location = new Point(0, 0);
		((Control)pageFilters).Margin = new Padding(6, 6, 6, 6);
		((Control)pageFilters).Name = "pageFilters";
		((Control)pageFilters).Padding = new Padding(0, 0, 2, 0);
		((Control)pageFilters).Size = new Size(1162, 1346);
		((Control)pageFilters).TabIndex = 2;
		((Control)pageFilters).Text = "Filters";
		((Control)pageFilters).Visible = false;
		((Control)pageFiltersPanel).Anchor = (AnchorStyles)15;
		((Control)pageFiltersPanel).BackColor = Color.White;
		((Control)pageFiltersPanel).Location = new Point(6, 6);
		((Control)pageFiltersPanel).Margin = new Padding(6, 6, 6, 6);
		((Control)pageFiltersPanel).Name = "pageFiltersPanel";
		((Control)pageFiltersPanel).Size = new Size(1150, 1335);
		((Control)pageFiltersPanel).TabIndex = 0;
		pageMarker.Dock = (DockStyle)5;
		pageMarker.Image = (Image)componentResourceManager.GetObject("pageMarker.Image");
		pageMarker.InvisibleVisible = false;
		((Control)pageMarker).Location = new Point(0, 0);
		((Control)pageMarker).Margin = new Padding(0);
		((Control)pageMarker).Name = "pageMarker";
		((Control)pageMarker).Padding = new Padding(0, 0, 2, 0);
		((Control)pageMarker).Size = new Size(1162, 1346);
		((Control)pageMarker).TabIndex = 3;
		((Control)pageMarker).Text = "Marker";
		((Control)pageMarker).Visible = false;
		pageLineMarker.Dock = (DockStyle)5;
		pageLineMarker.Image = (Image)componentResourceManager.GetObject("pageLineMarker.Image");
		pageLineMarker.InvisibleVisible = false;
		((Control)pageLineMarker).Location = new Point(0, 0);
		((Control)pageLineMarker).Margin = new Padding(0);
		((Control)pageLineMarker).Name = "pageLineMarker";
		((Control)pageLineMarker).Padding = new Padding(0, 0, 2, 0);
		((Control)pageLineMarker).Size = new Size(1162, 1346);
		((Control)pageLineMarker).TabIndex = 4;
		((Control)pageLineMarker).Text = "Line Marker";
		((Control)pageLineMarker).Visible = false;
		pageInteraction.Dock = (DockStyle)5;
		pageInteraction.Image = (Image)componentResourceManager.GetObject("pageInteraction.Image");
		pageInteraction.InvisibleVisible = false;
		((Control)pageInteraction).Location = new Point(0, 0);
		((Control)pageInteraction).Margin = new Padding(6, 6, 6, 6);
		((Control)pageInteraction).Name = "pageInteraction";
		((Control)pageInteraction).Padding = new Padding(0, 8, 2, 8);
		((Control)pageInteraction).Size = new Size(1162, 1346);
		((Control)pageInteraction).TabIndex = 3;
		((Control)pageInteraction).Text = "Interaction";
		((Control)pageInteraction).Visible = false;
		((Control)pageTrendLine).BackColor = Color.Red;
		((Control)pageTrendLine).Controls.Add((Control)(object)pageTrendLinePanel);
		pageTrendLine.Dock = (DockStyle)5;
		pageTrendLine.Image = (Image)componentResourceManager.GetObject("pageTrendLine.Image");
		pageTrendLine.InvisibleVisible = true;
		((Control)pageTrendLine).Location = new Point(0, 0);
		((Control)pageTrendLine).Margin = new Padding(6, 6, 6, 6);
		((Control)pageTrendLine).Name = "pageTrendLine";
		((Control)pageTrendLine).Padding = new Padding(0, 8, 2, 8);
		((Control)pageTrendLine).Size = new Size(1272, 1346);
		((Control)pageTrendLine).TabIndex = 4;
		((Control)pageTrendLine).Text = "TrendLine";
		((Control)pageTrendLine).Visible = false;
		((Control)pageTrendLinePanel).Anchor = (AnchorStyles)15;
		((Control)pageTrendLinePanel).Location = new Point(6, 6);
		((Control)pageTrendLinePanel).Margin = new Padding(6, 6, 6, 6);
		((Control)pageTrendLinePanel).Name = "pageTrendLinePanel";
		((Control)pageTrendLinePanel).Size = new Size(1260, 1335);
		((Control)pageTrendLinePanel).TabIndex = 0;
		pageTopN.Dock = (DockStyle)5;
		pageTopN.Image = (Image)componentResourceManager.GetObject("pageTopN.Image");
		pageTopN.InvisibleVisible = false;
		((Control)pageTopN).Location = new Point(0, 0);
		((Control)pageTopN).Margin = new Padding(6, 6, 6, 6);
		((Control)pageTopN).Name = "pageTopN";
		((Control)pageTopN).Padding = new Padding(0, 8, 2, 8);
		((Control)pageTopN).Size = new Size(1162, 1346);
		((Control)pageTopN).TabIndex = 4;
		((Control)pageTopN).Text = "TopN";
		((Control)pageTopN).Visible = false;
		((Control)pageSeriesLabels).Controls.Add((Control)(object)footerPanelSeriesLabels);
		((Control)pageSeriesLabels).Controls.Add((Control)(object)pageSeriesLabelsLeftPanel);
		((Control)pageSeriesLabels).Controls.Add((Control)(object)labelSeriesLabel);
		pageSeriesLabels.Dock = (DockStyle)5;
		pageSeriesLabels.Image = (Image)componentResourceManager.GetObject("pageSeriesLabels.Image");
		pageSeriesLabels.InvisibleVisible = false;
		((Control)pageSeriesLabels).Location = new Point(0, 0);
		((Control)pageSeriesLabels).Margin = new Padding(6, 6, 6, 6);
		((Control)pageSeriesLabels).Name = "pageSeriesLabels";
		((Control)pageSeriesLabels).Padding = new Padding(0, 8, 2, 8);
		((Control)pageSeriesLabels).Size = new Size(1272, 1346);
		((Control)pageSeriesLabels).TabIndex = 4;
		((Control)pageSeriesLabels).Text = "SeriesLabels";
		((Control)pageSeriesLabels).Visible = false;
		((Control)footerPanelSeriesLabels).BackColor = Color.White;
		((Control)footerPanelSeriesLabels).Controls.Add((Control)(object)radioFromSeries);
		((Control)footerPanelSeriesLabels).Controls.Add((Control)(object)radioFromChart);
		((Control)footerPanelSeriesLabels).Controls.Add((Control)(object)labelShowSeriesLabels);
		((Control)footerPanelSeriesLabels).Dock = (DockStyle)2;
		footerPanelSeriesLabels.LinePosition = StiLinePosition.Top;
		footerPanelSeriesLabels.LineStyle = StiLineStyle.Dot;
		((Control)footerPanelSeriesLabels).Location = new Point(410, 1265);
		((Control)footerPanelSeriesLabels).Margin = new Padding(6, 6, 6, 6);
		((Control)footerPanelSeriesLabels).Name = "footerPanelSeriesLabels";
		((Control)footerPanelSeriesLabels).Size = new Size(860, 73);
		((Control)footerPanelSeriesLabels).TabIndex = 1;
		((Control)radioFromSeries).AutoSize = true;
		((Control)radioFromSeries).Location = new Point(466, 25);
		((Control)radioFromSeries).Margin = new Padding(6, 6, 6, 6);
		((Control)radioFromSeries).Name = "radioFromSeries";
		((Control)radioFromSeries).Size = new Size(159, 29);
		((Control)radioFromSeries).TabIndex = 3;
		((Control)radioFromSeries).Text = "From Series";
		radioFromSeries.CheckedChanged += RadioShowSeriesLabelsChecked_CheckedChanged;
		((Control)radioFromChart).AutoSize = true;
		radioFromChart.Checked = true;
		((Control)radioFromChart).Location = new Point(248, 25);
		((Control)radioFromChart).Margin = new Padding(6, 6, 6, 6);
		((Control)radioFromChart).Name = "radioFromChart";
		((Control)radioFromChart).Size = new Size(150, 29);
		((Control)radioFromChart).TabIndex = 2;
		radioFromChart.TabStop = true;
		((Control)radioFromChart).Text = "From Chart";
		radioFromChart.CheckedChanged += RadioShowSeriesLabelsChecked_CheckedChanged;
		((Control)labelShowSeriesLabels).AutoSize = true;
		((Control)labelShowSeriesLabels).ForeColor = Color.DimGray;
		((Control)labelShowSeriesLabels).Location = new Point(8, 25);
		((Control)labelShowSeriesLabels).Margin = new Padding(6, 0, 6, 0);
		((Control)labelShowSeriesLabels).Name = "labelShowSeriesLabels";
		((Control)labelShowSeriesLabels).Size = new Size(194, 30);
		((Control)labelShowSeriesLabels).TabIndex = 0;
		((Control)labelShowSeriesLabels).Text = "Show Series Label:";
		labelShowSeriesLabels.TextAlign = (ContentAlignment)16;
		labelShowSeriesLabels.UseCompatibleTextRendering = true;
		((Control)pageSeriesLabelsLeftPanel).BackColor = Color.White;
		((Control)pageSeriesLabelsLeftPanel).Controls.Add((Control)(object)panelSeriesLabels);
		((Control)pageSeriesLabelsLeftPanel).Dock = (DockStyle)3;
		((Control)pageSeriesLabelsLeftPanel).Location = new Point(0, 8);
		((Control)pageSeriesLabelsLeftPanel).Margin = new Padding(6, 6, 6, 6);
		((Control)pageSeriesLabelsLeftPanel).Name = "pageSeriesLabelsLeftPanel";
		((Control)pageSeriesLabelsLeftPanel).Size = new Size(410, 1330);
		((Control)pageSeriesLabelsLeftPanel).TabIndex = 3;
		((Control)pageSeriesLabelsLeftPanel).Paint += new PaintEventHandler(pageSeriesLabelsLeftPanel_Paint);
		((ScrollableControl)panelSeriesLabels).AutoScroll = true;
		((Control)panelSeriesLabels).BackColor = SystemColors.Window;
		((Control)panelSeriesLabels).Dock = (DockStyle)5;
		((Control)panelSeriesLabels).Font = new Font("Arial", 8f);
		((Control)panelSeriesLabels).ForeColor = Color.Black;
		((Control)panelSeriesLabels).Location = new Point(0, 0);
		((Control)panelSeriesLabels).Margin = new Padding(6, 6, 6, 6);
		((Control)panelSeriesLabels).Name = "panelSeriesLabels";
		panelSeriesLabels.ScrollHeight = 0;
		panelSeriesLabels.ScrollWidth = 0;
		((Control)panelSeriesLabels).Size = new Size(410, 1330);
		((Control)panelSeriesLabels).TabIndex = 0;
		((Control)panelSeriesLabels).Paint += new PaintEventHandler(panelSeriesLabels_Paint);
		((Control)panelSeriesLabels).MouseDown += new MouseEventHandler(panelSeriesLabels_MouseDown);
		((Control)panelSeriesLabels).MouseLeave += panelSeriesLabels_MouseLeave;
		((Control)panelSeriesLabels).MouseMove += new MouseEventHandler(panelSeriesLabels_MouseMove);
		((Control)labelSeriesLabel).BackColor = Color.White;
		((Control)labelSeriesLabel).Location = new Point(416, 0);
		((Control)labelSeriesLabel).Margin = new Padding(6, 0, 6, 0);
		((Control)labelSeriesLabel).Name = "labelSeriesLabel";
		((Control)labelSeriesLabel).Size = new Size(736, 38);
		((Control)labelSeriesLabel).TabIndex = 2;
		((Control)labelSeriesLabel).Text = "Series Label";
		labelSeriesLabel.TextAlign = (ContentAlignment)32;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)panelAdditionalCommon);
		((Control)this).Controls.Add((Control)(object)TabControl);
		((Control)this).Margin = new Padding(6, 6, 6, 6);
		((Control)this).Name = "StiCloudSeriesControl";
		((Control)this).Size = new Size(1400, 1346);
		((Control)panelAdditionalCommon).ResumeLayout(false);
		((Control)tvChartSeriesPanel).ResumeLayout(false);
		((Control)toolBarPanel).ResumeLayout(false);
		((ISupportInitialize)toolBar).EndInit();
		((Control)TabControl).ResumeLayout(false);
		((Control)pageConditions).ResumeLayout(false);
		((Control)pageFilters).ResumeLayout(false);
		((Control)pageTrendLine).ResumeLayout(false);
		((Control)pageSeriesLabels).ResumeLayout(false);
		((Control)footerPanelSeriesLabels).ResumeLayout(false);
		((Control)footerPanelSeriesLabels).PerformLayout();
		((Control)pageSeriesLabelsLeftPanel).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
