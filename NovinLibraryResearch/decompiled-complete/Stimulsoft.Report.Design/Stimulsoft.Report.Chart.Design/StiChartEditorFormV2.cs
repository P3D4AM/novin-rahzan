using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Design.Inplace;
using Stimulsoft.Report.Design.Panels;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Helper;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Chart.Design;

public class StiChartEditorFormV2 : StiInplaceToolForm
{
	private bool lockRefresh;

	private bool isItemsInitialized;

	private StiChartDataColumnListBox selectedListBox;

	private string pathImage = "Stimulsoft.Report.Design.Resources.Office2013.Chart.";

	private StiPopup iconsPopup;

	private Panel panelIcons;

	private StiHintTextBox textBoxSearch;

	private IContainer components;

	private ButtonItem menuExpression;

	private StiChartDataColumnListBox listBoxValues;

	private ToolTip toolTip;

	private StiChartButton buttonSeriesType;

	private ButtonItem categoryClusteredColumn;

	private ButtonItem categoryTreemap;

	private ButtonItem categorySunburst;

	private ButtonItem categoryHistogram;

	private ButtonItem categoryBoxAndWhisker;

	private ButtonItem categoryWaterfall;

	private ButtonItem typeClusteredColumn;

	private ButtonItem typeStackedColumn;

	private ButtonItem typeFullStackedColumn;

	private ButtonItem typePareto;

	private ButtonItem typeHistogram;

	private ButtonItem categoryLine;

	private ButtonItem typeLine;

	private ButtonItem typeStackedLine;

	private ButtonItem typeFullStackedLine;

	private ButtonItem typeSpline;

	private ButtonItem typeStackedSpline;

	private ButtonItem typeFullStackedSpline;

	private ButtonItem typeSteppedLine;

	private ButtonItem categoryArea;

	private ButtonItem typeArea;

	private ButtonItem typeStackedArea;

	private ButtonItem typeFullStackedArea;

	private ButtonItem typeSplineArea;

	private ButtonItem typeStackedSplineArea;

	private ButtonItem typeFullStackedSplineArea;

	private ButtonItem typeSteppedArea;

	private ButtonItem categoryRange;

	private ButtonItem typeRange;

	private ButtonItem typeSplineRange;

	private ButtonItem typeSteppedRange;

	private ButtonItem typeRangeBar;

	private ButtonItem categoryClusteredBar;

	private ButtonItem typeClusteredBar;

	private ButtonItem typeStackedBar;

	private ButtonItem typeFullStackedBar;

	private ButtonItem categoryScatter;

	private ButtonItem typeScatter;

	private ButtonItem typeScatterLine;

	private ButtonItem typeScatterSpline;

	private ButtonItem categoryPie;

	private ButtonItem typePie;

	private ButtonItem typePie3d;

	private ButtonItem categoryRadar;

	private ButtonItem typeRadarPoint;

	private ButtonItem typeRadarLine;

	private ButtonItem typeRadarArea;

	private ButtonItem categoryFunnel;

	private ButtonItem typeFunnel;

	private ButtonItem typeFunnelWeightedSlices;

	private ButtonItem categoryFinancial;

	private ButtonItem typeCandlestick;

	private ButtonItem typeStock;

	private ButtonItem categoryPictorial;

	private ButtonItem typeTreemap;

	private ButtonItem typeGantt;

	private ButtonItem typeDoughnut;

	private ButtonItem typeBubble;

	private ButtonItem typePictorial;

	private ButtonItem typeSunburst;

	private ButtonItem typeWaterfall;

	private ButtonItem typeBoxAndWhisker;

	private StiChartGroupLabel labelValues;

	private Panel panelSeriesType;

	private ContextMenuBar contextMenuBar;

	private StiChartDataColumnListBox listBoxEndValues;

	private StiChartGroupLabel labelEndValues;

	private StiChartDataColumnListBox listBoxCloseValues;

	private StiChartGroupLabel labelCloseValues;

	private StiChartDataColumnListBox listBoxLowValues;

	private StiChartGroupLabel labelLowValues;

	private StiChartDataColumnListBox listBoxHighValues;

	private StiChartGroupLabel labelHighVaules;

	private StiChartDataColumnListBox listBoxWeights;

	private StiChartGroupLabel labelWeights;

	private StiChartDataColumnListBox listBoxArguments;

	private StiChartGroupLabel labelArguments;

	private Label labelIcon;

	private Panel panelBoxIcon;

	private StiOperationButton buttonIcon;

	private Panel panelIcon;

	private StiGroupLine panelFooter;

	private Button buttonOk;

	private Button buttonCancel;

	private Button buttonMoreOptions;

	private Panel panelOptions;

	private StiPanelBox panelBoxExpression;

	private HintTextBox textBoxExpression;

	private StiOperationButton buttonExpressionMenu;

	private Label labelField;

	private Panel panelListBoxes;

	private Button buttonSeries;

	private Label labelChartType;

	protected bool? UseAliases => Designer?.UseAliases;

	internal virtual StiDesignerControl Designer { get; set; }

	internal StiReport Report => Chart.Report;

	internal StiChart Chart { get; }

	private StiFontIcons PictorialIcon { get; set; } = StiFontIcons.QuarterFull;


	private StiPropertyGrid PropertyGrid { get; set; }

	internal StiChartSeriesType? OldSeriesType { get; set; }

	internal StiDataColumnMeter ChartValue => listBoxValues.SelectedChartItem?.Meter;

	internal StiDataColumnMeter ChartEndValue => listBoxEndValues.SelectedChartItem?.Meter;

	internal StiDataColumnMeter ChartCloseValue => listBoxCloseValues.SelectedChartItem?.Meter;

	internal StiDataColumnMeter ChartLowValue => listBoxLowValues.SelectedChartItem?.Meter;

	internal StiDataColumnMeter ChartHighValue => listBoxHighValues.SelectedChartItem?.Meter;

	internal StiDataColumnMeter ChartArgument => listBoxArguments.SelectedChartItem?.Meter;

	internal StiDataColumnMeter ChartWeight => listBoxWeights.SelectedChartItem?.Meter;

	internal bool IsBubble => Chart.Series.ToList().Any((StiSeries v) => v != null && v.ComponentId == StiComponentId.StiBubbleSeries);

	internal bool IsPictorial => Chart.Series.ToList().Any((StiSeries v) => v != null && v.ComponentId == StiComponentId.StiPictorialSeries);

	internal bool IsSunburst => Chart.Series.ToList().Any((StiSeries v) => v != null && v.ComponentId == StiComponentId.StiSunburstSeries);

	internal bool IsRange => Chart.Series.ToList().Any((StiSeries v) => v != null && (v.ComponentId == StiComponentId.StiRangeSeries || v.ComponentId == StiComponentId.StiRangeBarSeries || v.ComponentId == StiComponentId.StiSplineRangeSeries || v.ComponentId == StiComponentId.StiSteppedRangeSeries || v.ComponentId == StiComponentId.StiGanttSeries));

	internal bool IsFinancial => Chart.Series.ToList().Any((StiSeries v) => v != null && (v.ComponentId == StiComponentId.StiCandlestickSeries || v.ComponentId == StiComponentId.StiStockSeries));

	internal bool IsValuesPresent => ((ListBox)listBoxValues).Items.Count > 0;

	internal bool IsEndValuesPresent
	{
		get
		{
			if (((ListBox)listBoxEndValues).Items.Count > 0)
			{
				return IsRange;
			}
			return false;
		}
	}

	internal bool IsCloseValuesPresent
	{
		get
		{
			if (((ListBox)listBoxCloseValues).Items.Count > 0)
			{
				return IsFinancial;
			}
			return false;
		}
	}

	internal bool IsLowValuesPresent
	{
		get
		{
			if (((ListBox)listBoxLowValues).Items.Count > 0)
			{
				return IsFinancial;
			}
			return false;
		}
	}

	internal bool IsHighValuesPresent
	{
		get
		{
			if (((ListBox)listBoxHighValues).Items.Count > 0)
			{
				return IsFinancial;
			}
			return false;
		}
	}

	internal bool IsArgumentsPresent => ((ListBox)listBoxArguments).Items.Count > 0;

	internal bool IsWeightsPresent
	{
		get
		{
			if (((ListBox)listBoxWeights).Items.Count > 0)
			{
				return IsBubble;
			}
			return false;
		}
	}

	public override bool IsAnyPresent
	{
		get
		{
			if (!IsValuesPresent && !IsEndValuesPresent && !IsCloseValuesPresent && !IsLowValuesPresent && !IsHighValuesPresent && !IsArgumentsPresent)
			{
				return IsWeightsPresent;
			}
			return true;
		}
	}

	public override bool AllowResize => false;

	internal StiChartDataColumnListBox ValuesListBox => listBoxValues;

	internal StiChartDataColumnListBox EndValuesListBox => listBoxEndValues;

	internal StiChartDataColumnListBox CloseValuesListBox => listBoxCloseValues;

	internal StiChartDataColumnListBox LowValuesListBox => listBoxLowValues;

	internal StiChartDataColumnListBox HighValuesListBox => listBoxHighValues;

	internal StiChartDataColumnListBox ArgumentsListBox => listBoxArguments;

	internal StiChartDataColumnListBox WeightsListBox => listBoxWeights;

	private static string LocNotDefined => "[" + Loc.Get("Dashboard", "DataNotDefined") + "]";

	public StiChartEditorFormV2()
		: this(null)
	{
	}

	public StiChartEditorFormV2(StiChart chart)
	{
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Expected O, but got Unknown
		InitializeComponent();
		Chart = chart;
		Designer = Report?.Designer?.DesignerControl as StiDesignerControl;
		PropertyGrid = StiPropertiesPanelHelper.GetPropertyGrid(Designer);
		if (StiOptions.Appearance.ShowFormsHelp)
		{
			((Form)this).HelpButton = true;
		}
		if (!((Control)this).MinimumSize.IsEmpty)
		{
			((Form)this).Size = ((Control)this).MinimumSize;
		}
		foreach (StiChartDataColumnListBox item in (from Control c in (IEnumerable)((Control)panelListBoxes).Controls
			where c is StiChartDataColumnListBox
			select c).Cast<StiChartDataColumnListBox>())
		{
			item.Designer = Designer;
			item.Editor = this;
			item.AllowImage = false;
		}
		lockRefresh = true;
		listBoxValues.AllowImage = true;
		ReadPropertiesFromElement();
		if (Chart.Series.FirstOrDefault() != null)
		{
			OldSeriesType = ConvertSeriesToChartSeriesType(Chart.Series.FirstOrDefault());
		}
		InitSeriesTypes();
		InitSeriesIcons();
		InitSeriesEvents();
		LocalizeInternal();
		DoScaling();
		((Control)buttonMoreOptions).Visible = !StiOptions.Designer.IsBeginnerDesigner;
		((PropertyGrid)PropertyGrid).PropertyValueChanged += new PropertyValueChangedEventHandler(PropertyGrid_PropertyValueChanged);
		lockRefresh = false;
	}

	public override void Localize()
	{
		LocalizeInternal();
		((Control)listBoxValues).Invalidate();
		((Control)listBoxEndValues).Invalidate();
		((Control)listBoxCloseValues).Invalidate();
		((Control)listBoxLowValues).Invalidate();
		((Control)listBoxHighValues).Invalidate();
		((Control)listBoxArguments).Invalidate();
		((Control)listBoxWeights).Invalidate();
		InitButtonSeriesType();
		((Control)this).Invalidate();
	}

	private void LocalizeInternal()
	{
		((Control)this).Text = Loc.Get("Components", "StiChart");
		((Control)labelChartType).Text = Loc.Get("Chart", "ChartType");
		((Control)labelField).Text = Loc.Get("PropertyMain", "Field");
		((Control)buttonMoreOptions).Text = StiLocalization.Get("Buttons", "MoreOptions");
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)buttonMoreOptions).Width = TextRenderer.MeasureText(((Control)buttonMoreOptions).Text, ((Control)buttonMoreOptions).Font).Width + StiScale.I16;
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
		foreach (ButtonItem subItem in buttonSeriesType.SubItems)
		{
			foreach (ButtonItem subItem2 in subItem.SubItems)
			{
				StiChartSeriesType stiChartSeriesType = (StiChartSeriesType)subItem2.Tag;
				if (stiChartSeriesType == StiChartSeriesType.Pie3d)
				{
					subItem2.Text = "3D " + Loc.Get("Chart", "Pie");
				}
				else
				{
					subItem2.Text = Loc.Get("Chart", $"{stiChartSeriesType}");
				}
			}
		}
		toolTip.SetToolTip((Control)(object)buttonSeries, Loc.Get("Chart", "SeriesEditorForm"));
		LocalizeMetricLabels();
	}

	private void LocalizeMetricLabels()
	{
		((Control)labelValues).Text = Loc.Get("PropertyMain", "Values");
		if (IsBubble)
		{
			((Control)labelValues).Text = "Y";
		}
		else if (IsFinancial)
		{
			((Control)labelValues).Text = Loc.Get("PropertyMain", "OpenValues");
		}
		((Control)labelWeights).Text = Loc.Get("PropertyMain", "Weights");
		((Control)labelEndValues).Text = Loc.Get("PropertyMain", "EndValues");
		((Control)labelCloseValues).Text = Loc.Get("PropertyMain", "CloseValues");
		((Control)labelLowValues).Text = Loc.Get("PropertyMain", "LowValues");
		((Control)labelHighVaules).Text = Loc.Get("PropertyMain", "HighValues");
		((Control)labelArguments).Text = (IsBubble ? "X" : Loc.Get("PropertyMain", "Arguments"));
	}

	protected internal void ActivateDictionaryPanel()
	{
		StiDictionaryPanelHelper.ActivatePanel(Designer);
	}

	protected internal void ResetComponentHighlights()
	{
		if (Chart != null && Chart.Page != null)
		{
			Chart.Page.GetComponents().ToList().ForEach(delegate(StiComponent c)
			{
				c.HighlightState = StiHighlightState.Hide;
			});
			RefreshElementView();
		}
	}

	public void RefreshElementView()
	{
		StiDesignerControl designer = Designer;
		if (designer != null)
		{
			((Control)designer).Refresh();
		}
		Designer?.Builder.Redraw();
	}

	protected virtual void SetListHeightFromCount(StiChartDataColumnListBox list, bool multiple)
	{
		int num = Math.Min(10, ((ListBox)list).Items.Count);
		if (multiple)
		{
			((Control)list).Height = Math.Min(num + 1, 7) * ((ListBox)list).ItemHeight;
		}
		else
		{
			((Control)list).Height = Math.Max(num, 1) * ((ListBox)list).ItemHeight + StiScale.I1;
		}
	}

	public virtual void ProcessElementChanging(StiElementChangedArgs args)
	{
		StiElementChangedProcessor.ProcessElementChanging(Chart, args);
	}

	private void UpdateFormPosition()
	{
		if (Designer != null)
		{
			Size size = ((Form)this).Size;
			Rectangle rectangle = Stimulsoft.Report.Design.StiConvert.RectangleToScreen(Designer, Chart.GetDisplayRectangle());
			rectangle = ((Control)Designer.PageView.GetCurrentPageView().View).RectangleToScreen(rectangle);
			Point location = ((rectangle.Right + StiInplaceEditor.ButtonMargin + StiInplaceEditor.ButtonSize > Screen.PrimaryScreen.Bounds.Right) ? new Point(rectangle.Left - size.Width - StiScale.I2, rectangle.Top) : new Point(rectangle.Right + StiScale.I2, rectangle.Top));
			if (location.Y + size.Height > Screen.PrimaryScreen.Bounds.Bottom)
			{
				location.Y = Screen.PrimaryScreen.Bounds.Bottom - StiScale.I(40) - size.Height;
			}
			((Form)this).Location = location;
		}
	}

	private void DoScaling()
	{
		((ButtonBase)buttonSeries).Image = StiReportDesignImages.Actions.Series();
		buttonSeriesType.DropArrowShift = -4;
		((Control)buttonSeriesType).Left = ((Control)panelBoxExpression).Left;
		((Control)buttonSeriesType).Width = ((Control)panelBoxExpression).Width;
		StiButtonsLocator.ArrangeButtons((Control)(object)panelBoxIcon);
	}

	private void RefreshSampleIcon()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		Graphics val = ((Control)panelBoxIcon).CreateGraphics();
		FontFamily fontFamilyIcons = StiFontIconsHelper.GetFontFamilyIcons();
		Font val2 = new Font(fontFamilyIcons, 13f);
		try
		{
			SolidBrush val3 = new SolidBrush(Color.FromArgb(68, 114, 196));
			try
			{
				val.FillRectangle(Brushes.White, new Rectangle(StiScale.I2, StiScale.I4, ((Control)panelBoxIcon).Width - StiScale.I4, ((Control)panelBoxIcon).Height - StiScale.I6));
				val.DrawString(StiFontIconsHelper.GetContent(PictorialIcon), val2, (Brush)(object)val3, (RectangleF)new Rectangle(StiScale.I2, StiScale.I4, ((Control)panelBoxIcon).Width, ((Control)panelBoxIcon).Height));
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		val.DrawRectangle(SystemPens.ControlDark, 0, 0, ((Control)panelBoxIcon).Width - 1, ((Control)panelBoxIcon).Height - 1);
	}

	private Panel GetIconPanel()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Expected O, but got Unknown
		Panel val = new Panel
		{
			Width = StiScale.XXI(274.0),
			Height = StiScale.YYI(342.0),
			BackColor = StiUX.Background,
			Padding = new Padding(StiScale.I1)
		};
		((Control)val).Paint += new PaintEventHandler(StiIconControlHelper.PanelRootPainter);
		Panel val2 = new Panel
		{
			Dock = (DockStyle)1,
			Size = new Size(StiScale.XXI(150.0), StiScale.YYI(28.0)),
			Padding = new Padding(StiScale.I10, StiScale.I6, StiScale.I10, StiScale.I6),
			BackColor = StiUX.Background
		};
		StiHintTextBox stiHintTextBox = new StiHintTextBox();
		((Control)stiHintTextBox).Dock = (DockStyle)5;
		stiHintTextBox.AllowDrawBorder = false;
		stiHintTextBox.Hint = StiLocalization.Get("FormViewer", "Find");
		((Control)stiHintTextBox).Font = new Font("Arial", 10f);
		textBoxSearch = stiHintTextBox;
		((Control)textBoxSearch).TextChanged += TextBoxSearch_TextChanged;
		((Control)val2).Controls.Add((Control)(object)textBoxSearch);
		panelIcons = new Panel
		{
			AutoScroll = true,
			Dock = (DockStyle)5
		};
		((Control)val).Controls.Add((Control)(object)panelIcons);
		((Control)val).Controls.Add((Control)(object)val2);
		BuildIconPanel(null);
		return val;
	}

	private void BuildIconPanel(string filterName)
	{
		((Control)panelIcons).Controls.Clear();
		StiFontIconSelectionControl stiFontIconSelectionControl = new StiFontIconSelectionControl(PictorialIcon, Color.FromArgb(68, 114, 196));
		((Control)stiFontIconSelectionControl).Location = new Point(StiScale.I2, StiScale.I2);
		((Control)stiFontIconSelectionControl).Width = ((Control)panelIcons).Width;
		((Control)stiFontIconSelectionControl).Anchor = (AnchorStyles)13;
		stiFontIconSelectionControl.TextFilter = filterName;
		StiFontIconSelectionControl stiFontIconSelectionControl2 = stiFontIconSelectionControl;
		stiFontIconSelectionControl2.ValueChanged += FontIconSelectionControl_ValueChanged;
		((Control)panelIcons).Controls.Add((Control)(object)stiFontIconSelectionControl2);
	}

	private StiMeterItemImage GetNodeImageKey(StiChartSeriesType seriesType)
	{
		if (Enum.TryParse<StiMeterItemImage>($"Series{seriesType}", ignoreCase: true, out var result))
		{
			return result;
		}
		return StiMeterItemImage.SeriesClusteredColumn;
	}

	private void InitSeriesEvents()
	{
		foreach (ButtonItem subItem in buttonSeriesType.SubItems)
		{
			foreach (ButtonItem subItem2 in subItem.SubItems)
			{
				subItem2.Click += SeriesItem_Click;
			}
		}
	}

	private StiSeries ConvertValueToSeries(StiDataColumnMeter value)
	{
		StiSeries stiSeries = ((value.Series != null && ConvertSeriesToChartSeriesType(value.Series) == value.SeriesType) ? value.Series : StiChartSeriesCreator.New(value.SeriesType));
		if (value.Series != null)
		{
			stiSeries.ListOfValues = value.Series.ListOfValues;
			stiSeries.ListOfArguments = value.Series.ListOfArguments;
			if (stiSeries is IStiRangeSeries stiRangeSeries && value.Series is IStiRangeSeries stiRangeSeries2)
			{
				stiRangeSeries.ListOfValuesEnd = stiRangeSeries2.ListOfValuesEnd;
			}
		}
		if (stiSeries is IStiFinancialSeries)
		{
			((IStiFinancialSeries)stiSeries).ValueDataColumnOpen = value.DataColumn;
		}
		else
		{
			stiSeries.ValueDataColumn = value.DataColumn;
		}
		if (stiSeries is StiPictorialSeries stiPictorialSeries)
		{
			stiPictorialSeries.Icon = PictorialIcon;
		}
		value.Series = stiSeries;
		return stiSeries;
	}

	private StiDataColumnMeter ConvertSeriesToValue(StiSeries series)
	{
		string dataColumn = ((series is IStiFinancialSeries) ? ((IStiFinancialSeries)series).ValueDataColumnOpen : series.ValueDataColumn);
		return new StiDataColumnMeter
		{
			DataColumn = dataColumn,
			Series = series,
			SeriesType = ConvertSeriesToChartSeriesType(series)
		};
	}

	private static StiChartSeriesType ConvertSeriesToChartSeriesType(StiSeries series)
	{
		string text = series.GetType().ToString();
		int startIndex = text.LastIndexOf(".") + 1;
		text = text.Substring(startIndex).Substring(3).Replace("Series", "");
		return (StiChartSeriesType)Enum.Parse(typeof(StiChartSeriesType), text);
	}

	public void ReadPropertiesFromElement()
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		ReadValuesPropertiesFromElement();
		ReadEndValuesPropertiesFromElement();
		ReadCloseValuesPropertiesFromElement();
		ReadLowValuesPropertiesFromElement();
		ReadHighValuesPropertiesFromElement();
		ReadArgumentsPropertiesFromElement();
		ReadWeightsPropertiesFromElement();
		Timer timer = new Timer();
		timer.Interval = 100;
		timer.Tick += delegate
		{
			timer.Stop();
			((Control)listBoxValues).Focus();
			if (!ShowChartItemAtProperties(listBoxValues))
			{
				ActivateDictionaryPanel();
			}
		};
		timer.Start();
	}

	private void ReadValuesPropertiesFromElement()
	{
		int selectedIndex = ((ListControl)listBoxValues).SelectedIndex;
		((ListBox)listBoxValues).BeginUpdate();
		((ListBox)listBoxValues).Items.Clear();
		for (int i = 0; i < Chart.Series.Count; i++)
		{
			StiSeries stiSeries = Chart.Series[i] as StiSeries;
			StiDataColumnMeter stiDataColumnMeter = ConvertSeriesToValue(stiSeries);
			if (stiSeries is StiPictorialSeries stiPictorialSeries)
			{
				PictorialIcon = stiPictorialSeries.Icon.GetValueOrDefault();
			}
			listBoxValues.AddItem(stiDataColumnMeter, GetNodeImageKey(stiDataColumnMeter.SeriesType));
		}
		if (selectedIndex < 0)
		{
			listBoxValues.SelectFirstItem();
		}
		else
		{
			IEnumerable<StiChartItem> source = ((IEnumerable)((ListBox)listBoxValues).Items).Cast<StiChartItem>();
			if (source.Count() > selectedIndex)
			{
				listBoxValues.SelectedChartItem = source.ToList()[selectedIndex];
			}
		}
		((ListBox)listBoxValues).EndUpdate();
		ShowChartItemAtProperties(listBoxValues);
	}

	private void ReadEndValuesPropertiesFromElement()
	{
		((ListBox)listBoxEndValues).BeginUpdate();
		((ListBox)listBoxEndValues).Items.Clear();
		(from s in Chart.Series.ToList()
			where s is IStiRangeSeries
			select s).Cast<IStiRangeSeries>().ToList().ForEach(delegate(IStiRangeSeries s)
		{
			listBoxEndValues.AddItem(new StiDataColumnMeter(s.ValueDataColumnEnd)).Meter.Series = s as StiSeries;
		});
		listBoxEndValues.SelectFirstItem();
		((ListBox)listBoxEndValues).EndUpdate();
	}

	private void ReadCloseValuesPropertiesFromElement()
	{
		((ListBox)listBoxCloseValues).BeginUpdate();
		((ListBox)listBoxCloseValues).Items.Clear();
		(from s in Chart.Series.ToList()
			where s is IStiFinancialSeries
			select s).Cast<IStiFinancialSeries>().ToList().ForEach(delegate(IStiFinancialSeries s)
		{
			listBoxCloseValues.AddItem(new StiDataColumnMeter(s.ValueDataColumnClose)).Meter.Series = s as StiSeries;
		});
		listBoxCloseValues.SelectFirstItem();
		((ListBox)listBoxCloseValues).EndUpdate();
	}

	private void ReadLowValuesPropertiesFromElement()
	{
		((ListBox)listBoxLowValues).BeginUpdate();
		((ListBox)listBoxLowValues).Items.Clear();
		(from s in Chart.Series.ToList()
			where s is IStiFinancialSeries
			select s).Cast<IStiFinancialSeries>().ToList().ForEach(delegate(IStiFinancialSeries s)
		{
			listBoxLowValues.AddItem(new StiDataColumnMeter(s.ValueDataColumnLow)).Meter.Series = s as StiSeries;
		});
		listBoxLowValues.SelectFirstItem();
		((ListBox)listBoxLowValues).EndUpdate();
	}

	private void ReadHighValuesPropertiesFromElement()
	{
		((ListBox)listBoxHighValues).BeginUpdate();
		((ListBox)listBoxHighValues).Items.Clear();
		(from s in Chart.Series.ToList()
			where s is IStiFinancialSeries
			select s).Cast<IStiFinancialSeries>().ToList().ForEach(delegate(IStiFinancialSeries s)
		{
			listBoxHighValues.AddItem(new StiDataColumnMeter(s.ValueDataColumnHigh)).Meter.Series = s as StiSeries;
		});
		listBoxHighValues.SelectFirstItem();
		((ListBox)listBoxHighValues).EndUpdate();
	}

	private void ReadArgumentsPropertiesFromElement()
	{
		((ListBox)listBoxArguments).BeginUpdate();
		((ListBox)listBoxArguments).Items.Clear();
		(from s in Chart.Series.ToList()
			where !string.IsNullOrWhiteSpace(s.ArgumentDataColumn)
			select s).ToList().ForEach(delegate(StiSeries s)
		{
			listBoxArguments.AddItem(new StiDataColumnMeter(s.ArgumentDataColumn)).Meter.Series = s;
		});
		listBoxArguments.SelectFirstItem();
		((ListBox)listBoxArguments).EndUpdate();
	}

	private void ReadWeightsPropertiesFromElement()
	{
		((ListBox)listBoxWeights).BeginUpdate();
		((ListBox)listBoxWeights).Items.Clear();
		(from s in Chart.Series.ToList()
			where s is StiBubbleSeries
			select s).Cast<StiBubbleSeries>().ToList().ForEach(delegate(StiBubbleSeries s)
		{
			listBoxWeights.AddItem(new StiDataColumnMeter(s.WeightDataColumn)).Meter.Series = s;
		});
		listBoxWeights.SelectFirstItem();
		((ListBox)listBoxWeights).EndUpdate();
	}

	public void WritePropertiesToElement()
	{
		WriteValuesPropertiesToElement();
		WriteEndValuesPropertiesToElement();
		WriteCloseValuesPropertiesToElement();
		WriteLowValuesPropertiesToElement();
		WriteHighValuesPropertiesToElement();
		WriteArgumentsPropertiesToElement();
		WriteWeightsPropertiesToElement();
		RefreshElementView();
	}

	private void WriteValuesPropertiesToElement()
	{
		List<StiDataColumnMeter> list = (from t in listBoxValues.ToItems()
			select t.Meter).Cast<StiDataColumnMeter>().ToList();
		Chart.Series.Clear();
		foreach (StiDataColumnMeter item in list)
		{
			Chart.Series.Add(ConvertValueToSeries(item));
		}
	}

	private void WriteEndValuesPropertiesToElement()
	{
		List<StiDataColumnMeter> list = (from t in listBoxEndValues.ToItems()
			select t.Meter).Cast<StiDataColumnMeter>().ToList();
		int num = 0;
		foreach (StiSeries item in Chart.Series)
		{
			StiDataColumnMeter stiDataColumnMeter = ((num < list.Count) ? list[num] : null);
			if (item is IStiRangeSeries stiRangeSeries)
			{
				stiRangeSeries.ValueDataColumnEnd = ((stiDataColumnMeter != null) ? stiDataColumnMeter.DataColumn : "");
			}
			num++;
		}
	}

	private void WriteCloseValuesPropertiesToElement()
	{
		List<StiDataColumnMeter> list = (from t in listBoxCloseValues.ToItems()
			select t.Meter).Cast<StiDataColumnMeter>().ToList();
		int num = 0;
		foreach (StiSeries item in Chart.Series)
		{
			StiDataColumnMeter stiDataColumnMeter = ((num < list.Count) ? list[num] : null);
			if (item is IStiFinancialSeries stiFinancialSeries)
			{
				stiFinancialSeries.ValueDataColumnClose = ((stiDataColumnMeter != null) ? stiDataColumnMeter.DataColumn : "");
			}
			num++;
		}
	}

	private void WriteLowValuesPropertiesToElement()
	{
		List<StiDataColumnMeter> list = (from t in listBoxLowValues.ToItems()
			select t.Meter).Cast<StiDataColumnMeter>().ToList();
		int num = 0;
		foreach (StiSeries item in Chart.Series)
		{
			StiDataColumnMeter stiDataColumnMeter = ((num < list.Count) ? list[num] : null);
			if (item is IStiFinancialSeries stiFinancialSeries)
			{
				stiFinancialSeries.ValueDataColumnLow = ((stiDataColumnMeter != null) ? stiDataColumnMeter.DataColumn : "");
			}
			num++;
		}
	}

	private void WriteHighValuesPropertiesToElement()
	{
		List<StiDataColumnMeter> list = (from t in listBoxHighValues.ToItems()
			select t.Meter).Cast<StiDataColumnMeter>().ToList();
		int num = 0;
		foreach (StiSeries item in Chart.Series)
		{
			StiDataColumnMeter stiDataColumnMeter = ((num < list.Count) ? list[num] : null);
			if (item is IStiFinancialSeries stiFinancialSeries)
			{
				stiFinancialSeries.ValueDataColumnHigh = ((stiDataColumnMeter != null) ? stiDataColumnMeter.DataColumn : "");
			}
			num++;
		}
	}

	private void WriteArgumentsPropertiesToElement()
	{
		List<StiDataColumnMeter> list = (from t in listBoxArguments.ToItems()
			select t.Meter).Cast<StiDataColumnMeter>().ToList();
		Chart.Series.ToList().ForEach(delegate(StiSeries s)
		{
			s.ArgumentDataColumn = "";
		});
		int num = 0;
		foreach (StiDataColumnMeter item in list)
		{
			StiSeries stiSeries = ((num < Chart.Series.Count) ? (Chart.Series[num] as StiSeries) : null);
			if (stiSeries == null)
			{
				stiSeries = ((Chart.Series.Count > 0) ? (Chart.Series.ToList().FirstOrDefault().Clone() as StiSeries) : new StiClusteredColumnSeries());
				Chart.Series.Add(stiSeries);
			}
			stiSeries.ArgumentDataColumn = ((item != null) ? item.DataColumn : "");
			num++;
		}
	}

	private void WriteWeightsPropertiesToElement()
	{
		List<StiDataColumnMeter> list = (from t in listBoxWeights.ToItems()
			select t.Meter).Cast<StiDataColumnMeter>().ToList();
		int num = 0;
		foreach (StiSeries item in Chart.Series)
		{
			StiDataColumnMeter stiDataColumnMeter = ((num < list.Count) ? list[num] : null);
			if (item is StiBubbleSeries stiBubbleSeries)
			{
				stiBubbleSeries.WeightDataColumn = ((stiDataColumnMeter != null) ? stiDataColumnMeter.DataColumn : "");
			}
			num++;
		}
	}

	protected override void AttachEvents()
	{
		base.AttachEvents();
		AttachEventsToValues();
		AttachEventsToEndValues();
		AttachEventsToCloseValues();
		AttachEventsToLowValues();
		AttachEventsToHighValues();
		AttachEventsToWeights();
		AttachEventsToArguments();
	}

	private void AttachEventsToValues()
	{
		listBoxValues.ElementsConverting += ListBox_ElementsConverting;
		listBoxValues.ElementsChanged += ListBox_ElementsChanged;
		listBoxValues.ElementsSelectedChanged += ListBox_ElementsAfterSelect;
		listBoxValues.ElementsDragEnter += ListBox_ElementsDragEnter;
		listBoxValues.ElementsDragDrop += delegate
		{
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(Designer))
			{
				StiDataColumnMeter draggedDataColumnMeter = GetDraggedDataColumnMeter();
				if (draggedDataColumnMeter == null)
				{
					return;
				}
				if (Chart.Series.Count == 0 && OldSeriesType.HasValue && OldSeriesType != (StiChartSeriesType)0)
				{
					draggedDataColumnMeter.SeriesType = ((OldSeriesType == (StiChartSeriesType)0) ? StiChartSeriesType.ClusteredColumn : OldSeriesType.Value);
				}
				else
				{
					draggedDataColumnMeter.SeriesType = ((ChartValue == null) ? StiChartSeriesType.ClusteredColumn : ChartValue.SeriesType);
				}
				draggedDataColumnMeter.Series = ConvertValueToSeries(draggedDataColumnMeter);
				StiChartItem stiChartItem = new StiChartItem(draggedDataColumnMeter.DataColumn, draggedDataColumnMeter, GetNodeImageKey(draggedDataColumnMeter.SeriesType));
				if (((ListBox)listBoxValues).Items.Count == 1 && string.IsNullOrWhiteSpace(listBoxValues.SelectedChartItem?.Meter?.DataColumn))
				{
					StiChartItem selectedChartItem = listBoxValues.SelectedChartItem;
					selectedChartItem.Meter = draggedDataColumnMeter;
					selectedChartItem.Text = draggedDataColumnMeter.DataColumn;
					((Control)listBoxValues).Invalidate();
					stiChartItem = selectedChartItem;
				}
				else
				{
					StiChartItem itemAtMouse = listBoxValues.GetItemAtMouse();
					if (itemAtMouse != null)
					{
						int index = ((ListBox)listBoxValues).Items.IndexOf((object)itemAtMouse);
						listBoxValues.InsertItem(index, stiChartItem);
					}
					else
					{
						listBoxValues.AddItem(stiChartItem);
					}
				}
				Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxValues.SelectedChartItem = stiChartItem;
				((Control)listBoxValues).Focus();
				StiDesignerDragDrop.IgnoreAddSelectedToProperties = true;
			}
			ResetComponentHighlights();
			RefreshUiPanel();
		};
		listBoxValues.ElementsSelectedChanged += delegate
		{
			Chart.Select();
			RefreshElementView();
		};
	}

	private void AttachEventsToArguments()
	{
		listBoxArguments.ElementsChanged += ListBox_ElementsChanged;
		listBoxArguments.ElementsSelectedChanged += ListBox_ElementsAfterSelect;
		listBoxArguments.ElementsDragEnter += ListBox_ElementsDragEnter;
		listBoxArguments.ElementsDragDrop += delegate
		{
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(Designer))
			{
				StiDataColumnMeter draggedDataColumnMeter = GetDraggedDataColumnMeter();
				if (draggedDataColumnMeter == null)
				{
					return;
				}
				StiChartItem stiChartItem = new StiChartItem(draggedDataColumnMeter.DataColumn, draggedDataColumnMeter, StiMeterItemImage.Dimension);
				StiChartItem itemAtMouse = listBoxArguments.GetItemAtMouse();
				if (itemAtMouse != null)
				{
					int index = ((ListBox)listBoxArguments).Items.IndexOf((object)itemAtMouse);
					listBoxArguments.InsertItem(index, stiChartItem);
				}
				else
				{
					listBoxArguments.AddItem(stiChartItem);
				}
				Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxArguments.SelectedChartItem = stiChartItem;
				((Control)listBoxArguments).Focus();
			}
			ResetComponentHighlights();
		};
		listBoxArguments.ElementsSelectedChanged += delegate
		{
			Chart.Select();
			RefreshElementView();
		};
	}

	private void AttachEventsToWeights()
	{
		listBoxWeights.ElementsChanged += ListBox_ElementsChanged;
		listBoxWeights.ElementsSelectedChanged += ListBox_ElementsAfterSelect;
		listBoxWeights.ElementsDragEnter += ListBox_ElementsDragEnter;
		listBoxWeights.ElementsDragDrop += delegate
		{
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(Designer))
			{
				StiDataColumnMeter draggedDataColumnMeter = GetDraggedDataColumnMeter();
				if (draggedDataColumnMeter == null)
				{
					return;
				}
				StiChartItem stiChartItem = new StiChartItem(draggedDataColumnMeter.DataColumn, draggedDataColumnMeter, StiMeterItemImage.Measure);
				if (!string.IsNullOrWhiteSpace(listBoxWeights.SelectedChartItem?.Meter?.DataColumn))
				{
					foreach (StiChartItem item in ((ListBox)listBoxWeights).Items)
					{
						if (string.IsNullOrWhiteSpace(item.Meter?.DataColumn))
						{
							listBoxWeights.SelectedChartItem = item;
						}
					}
				}
				if (string.IsNullOrWhiteSpace(listBoxWeights.SelectedChartItem?.Meter?.DataColumn))
				{
					StiChartItem selectedChartItem = listBoxWeights.SelectedChartItem;
					selectedChartItem.Meter = draggedDataColumnMeter;
					selectedChartItem.Text = draggedDataColumnMeter.DataColumn;
					((Control)listBoxWeights).Invalidate();
					stiChartItem = selectedChartItem;
				}
				else
				{
					StiChartItem itemAtMouse = listBoxWeights.GetItemAtMouse();
					if (itemAtMouse != null)
					{
						int index = ((ListBox)listBoxWeights).Items.IndexOf((object)itemAtMouse);
						listBoxWeights.InsertItem(index, stiChartItem);
					}
					else
					{
						listBoxWeights.AddItem(stiChartItem);
					}
				}
				Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxWeights.SelectedChartItem = stiChartItem;
				((Control)listBoxWeights).Focus();
			}
			ResetComponentHighlights();
		};
		listBoxWeights.ElementsSelectedChanged += delegate
		{
			Chart.Select();
			RefreshElementView();
		};
	}

	private void AttachEventsToEndValues()
	{
		listBoxEndValues.ElementsChanged += ListBox_ElementsChanged;
		listBoxEndValues.ElementsSelectedChanged += ListBox_ElementsAfterSelect;
		listBoxEndValues.ElementsDragEnter += ListBox_ElementsDragEnter;
		listBoxEndValues.ElementsDragDrop += delegate
		{
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(Designer))
			{
				StiDataColumnMeter draggedDataColumnMeter = GetDraggedDataColumnMeter();
				if (draggedDataColumnMeter == null)
				{
					return;
				}
				StiChartItem stiChartItem = new StiChartItem(draggedDataColumnMeter.DataColumn, draggedDataColumnMeter, GetNodeImageKey(draggedDataColumnMeter.SeriesType));
				if (((ListBox)listBoxEndValues).Items.Count == 1 && string.IsNullOrWhiteSpace(listBoxEndValues.SelectedChartItem?.Meter?.DataColumn) && listBoxEndValues.SelectedChartItem != null)
				{
					StiChartItem selectedChartItem = listBoxEndValues.SelectedChartItem;
					selectedChartItem.Meter = draggedDataColumnMeter;
					selectedChartItem.Text = draggedDataColumnMeter.DataColumn;
					((Control)listBoxEndValues).Invalidate();
					stiChartItem = selectedChartItem;
				}
				else
				{
					StiChartItem itemAtMouse = listBoxEndValues.GetItemAtMouse();
					if (itemAtMouse != null)
					{
						int index = ((ListBox)listBoxEndValues).Items.IndexOf((object)itemAtMouse);
						listBoxEndValues.InsertItem(index, stiChartItem);
					}
					else
					{
						listBoxEndValues.AddItem(stiChartItem);
					}
				}
				Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxEndValues.SelectedChartItem = stiChartItem;
				((Control)listBoxEndValues).Focus();
			}
			ResetComponentHighlights();
		};
		listBoxEndValues.ElementsSelectedChanged += delegate
		{
			Chart.Select();
			RefreshElementView();
		};
	}

	private void AttachEventsToCloseValues()
	{
		listBoxCloseValues.ElementsChanged += ListBox_ElementsChanged;
		listBoxCloseValues.ElementsSelectedChanged += ListBox_ElementsAfterSelect;
		listBoxCloseValues.ElementsDragEnter += ListBox_ElementsDragEnter;
		listBoxCloseValues.ElementsDragDrop += delegate
		{
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(Designer))
			{
				StiDataColumnMeter draggedDataColumnMeter = GetDraggedDataColumnMeter();
				if (draggedDataColumnMeter == null)
				{
					return;
				}
				((ListBox)listBoxCloseValues).Items.Clear();
				listBoxCloseValues.AddItem(draggedDataColumnMeter);
				Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxCloseValues.SelectedChartItem = listBoxCloseValues.LastItem();
				((Control)listBoxCloseValues).Focus();
			}
			ResetComponentHighlights();
		};
		listBoxCloseValues.ElementsSelectedChanged += delegate
		{
			Chart.Select();
			RefreshElementView();
		};
	}

	private void AttachEventsToLowValues()
	{
		listBoxLowValues.ElementsChanged += ListBox_ElementsChanged;
		listBoxLowValues.ElementsSelectedChanged += ListBox_ElementsAfterSelect;
		listBoxLowValues.ElementsDragEnter += ListBox_ElementsDragEnter;
		listBoxLowValues.ElementsDragDrop += delegate
		{
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(Designer))
			{
				StiDataColumnMeter draggedDataColumnMeter = GetDraggedDataColumnMeter();
				if (draggedDataColumnMeter == null)
				{
					return;
				}
				((ListBox)listBoxLowValues).Items.Clear();
				listBoxLowValues.AddItem(draggedDataColumnMeter);
				Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxLowValues.SelectedChartItem = listBoxLowValues.LastItem();
				((Control)listBoxLowValues).Focus();
			}
			ResetComponentHighlights();
		};
		listBoxLowValues.ElementsSelectedChanged += delegate
		{
			Chart.Select();
			RefreshElementView();
		};
	}

	private void AttachEventsToHighValues()
	{
		listBoxHighValues.ElementsChanged += ListBox_ElementsChanged;
		listBoxHighValues.ElementsSelectedChanged += ListBox_ElementsAfterSelect;
		listBoxHighValues.ElementsDragEnter += ListBox_ElementsDragEnter;
		listBoxHighValues.ElementsDragDrop += delegate
		{
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(Designer))
			{
				StiDataColumnMeter draggedDataColumnMeter = GetDraggedDataColumnMeter();
				if (draggedDataColumnMeter == null)
				{
					return;
				}
				((ListBox)listBoxHighValues).Items.Clear();
				listBoxHighValues.AddItem(draggedDataColumnMeter);
				Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxHighValues.SelectedChartItem = listBoxHighValues.LastItem();
				((Control)listBoxHighValues).Focus();
			}
			ResetComponentHighlights();
		};
		listBoxHighValues.ElementsSelectedChanged += delegate
		{
			Chart.Select();
			RefreshElementView();
		};
	}

	private StiDataColumnMeter GetDraggedDataColumnMeter()
	{
		IStiAppDataCell draggedDataColumnOrVariable = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
		if (draggedDataColumnOrVariable == null)
		{
			return null;
		}
		if (draggedDataColumnOrVariable is StiDataColumn)
		{
			string draggedDataColumnPath = StiDragDropColumnHelper.GetDraggedDataColumnPath(Designer);
			if (!string.IsNullOrWhiteSpace(draggedDataColumnPath))
			{
				return new StiDataColumnMeter(draggedDataColumnPath);
			}
		}
		return new StiDataColumnMeter(draggedDataColumnOrVariable);
	}

	public override void RefreshUiPanel()
	{
		if (base.LockRefresh)
		{
			return;
		}
		base.LockRefresh = true;
		((Control)this).SuspendLayout();
		try
		{
			StiChartGroupLabel stiChartGroupLabel = labelWeights;
			bool visible = (((Control)listBoxWeights).Visible = IsValuesPresent && IsBubble);
			((Control)stiChartGroupLabel).Visible = visible;
			StiChartGroupLabel stiChartGroupLabel2 = labelEndValues;
			visible = (((Control)listBoxEndValues).Visible = IsValuesPresent && IsRange);
			((Control)stiChartGroupLabel2).Visible = visible;
			StiChartGroupLabel stiChartGroupLabel3 = labelCloseValues;
			StiChartDataColumnListBox stiChartDataColumnListBox = listBoxCloseValues;
			StiChartGroupLabel stiChartGroupLabel4 = labelLowValues;
			StiChartDataColumnListBox stiChartDataColumnListBox2 = listBoxLowValues;
			StiChartGroupLabel stiChartGroupLabel5 = labelHighVaules;
			bool flag3 = (((Control)listBoxHighValues).Visible = IsFinancial);
			bool flag5 = (((Control)stiChartGroupLabel5).Visible = flag3);
			bool flag7 = (((Control)stiChartDataColumnListBox2).Visible = flag5);
			bool flag9 = (((Control)stiChartGroupLabel4).Visible = flag7);
			visible = (((Control)stiChartDataColumnListBox).Visible = flag9);
			((Control)stiChartGroupLabel3).Visible = visible;
			Panel obj = panelOptions;
			visible = (((Control)textBoxExpression).Visible = (IsValuesPresent && listBoxValues.IsCurrentlySelected) || (IsEndValuesPresent && listBoxEndValues.IsCurrentlySelected) || (IsCloseValuesPresent && listBoxCloseValues.IsCurrentlySelected) || (IsLowValuesPresent && listBoxLowValues.IsCurrentlySelected) || (IsHighValuesPresent && listBoxHighValues.IsCurrentlySelected) || (IsArgumentsPresent && listBoxArguments.IsCurrentlySelected) || (IsWeightsPresent && listBoxWeights.IsCurrentlySelected));
			((Control)obj).Enabled = visible;
			listBoxArguments.ToItems().ToList().ForEach(delegate(StiChartItem n)
			{
				n.Image = (IsBubble ? StiMeterItemImage.Measure : StiMeterItemImage.Dimension);
			});
			Panel obj2 = panelSeriesType;
			StiChartGroupLabel stiChartGroupLabel6 = labelValues;
			StiChartGroupLabel stiChartGroupLabel7 = labelArguments;
			StiChartDataColumnListBox stiChartDataColumnListBox3 = listBoxArguments;
			flag5 = (((Control)panelOptions).Visible = IsAnyPresent);
			flag7 = (((Control)stiChartDataColumnListBox3).Visible = flag5);
			flag9 = (((Control)stiChartGroupLabel7).Visible = flag7);
			visible = (((Control)stiChartGroupLabel6).Visible = flag9);
			((Control)obj2).Visible = visible;
			((Control)panelIcon).Visible = IsPictorial;
			((Control)buttonSeries).Left = ((Control)listBoxValues).Right - ((Control)buttonSeries).Width - ((!IsAnyPresent) ? StiScaleUI.I3 : 0);
			if (ChartValue != null)
			{
				InitButtonSeriesType();
			}
			((Control)buttonSeriesType).Enabled = ChartValue != null;
			if (IsAnyPresent)
			{
				SetListHeightFromCount(listBoxValues, multiple: true);
			}
			else
			{
				((Control)listBoxValues).Height = ((Control)listBoxValues).Width;
			}
			SetListHeightFromCount(listBoxEndValues, multiple: true);
			SetListHeightFromCount(listBoxCloseValues, multiple: true);
			SetListHeightFromCount(listBoxLowValues, multiple: true);
			SetListHeightFromCount(listBoxHighValues, multiple: true);
			SetListHeightFromCount(listBoxArguments, multiple: true);
			SetListHeightFromCount(listBoxWeights, multiple: true);
			CalculateWindowHeight();
			CalculateWindowPosition();
			LocalizeMetricLabels();
		}
		finally
		{
			((Control)this).ResumeLayout();
			base.LockRefresh = false;
		}
	}

	protected override void CalculateWindowHeight()
	{
	}

	internal void RefreshExpression()
	{
		base.LockRefresh = true;
		if (selectedListBox == listBoxValues && ChartValue != null)
		{
			((Control)textBoxExpression).Text = GetExpression(ChartValue.DataColumn);
		}
		else if (selectedListBox == listBoxEndValues && ChartEndValue != null)
		{
			((Control)textBoxExpression).Text = GetExpression(ChartEndValue.DataColumn);
		}
		else if (selectedListBox == listBoxCloseValues && ChartCloseValue != null)
		{
			((Control)textBoxExpression).Text = GetExpression(ChartCloseValue.DataColumn);
		}
		else if (selectedListBox == listBoxLowValues && ChartLowValue != null)
		{
			((Control)textBoxExpression).Text = GetExpression(ChartLowValue.DataColumn);
		}
		else if (selectedListBox == listBoxHighValues && ChartHighValue != null)
		{
			((Control)textBoxExpression).Text = GetExpression(ChartHighValue.DataColumn);
		}
		else if (selectedListBox == listBoxWeights && ChartWeight != null)
		{
			((Control)textBoxExpression).Text = GetExpression(ChartWeight.DataColumn);
		}
		else if (selectedListBox == listBoxArguments && ChartArgument != null)
		{
			((Control)textBoxExpression).Text = GetExpression(ChartArgument.DataColumn);
		}
		else
		{
			((Control)textBoxExpression).Text = string.Empty;
		}
		base.LockRefresh = false;
	}

	private string GetExpression(string dataColumn)
	{
		if (!string.IsNullOrWhiteSpace(dataColumn))
		{
			return dataColumn;
		}
		return LocNotDefined;
	}

	private void InitButtonSeriesType()
	{
		if (ChartValue != null)
		{
			string arg = "";
			if (StiScaleUI.Factor >= 2.0)
			{
				arg = "_x2";
			}
			else if (StiScaleUI.Factor >= 1.5)
			{
				arg = "_x1_5";
			}
			StiResourcesHelper.SetImage((ButtonX)buttonSeriesType, $"{pathImage}{ChartValue.SeriesType}{arg}.png", scale: false);
			if (ChartValue.SeriesType == StiChartSeriesType.Pie3d)
			{
				((Control)buttonSeriesType).Text = "3D " + Loc.Get("Chart", "Pie");
			}
			else
			{
				((Control)buttonSeriesType).Text = Loc.Get("Chart", $"{ChartValue.SeriesType}");
			}
		}
	}

	private void InitSeriesTypes()
	{
		categoryClusteredColumn.Tag = "categoryClusteredColumn";
		categoryLine.Tag = "categoryLine";
		categoryArea.Tag = "categoryArea";
		categoryRange.Tag = "categoryRange";
		categoryClusteredBar.Tag = "categoryClusteredBar";
		categoryPie.Tag = "categoryPie";
		categoryRadar.Tag = "RadarArea";
		categoryScatter.Tag = "categoryScatter";
		categoryFunnel.Tag = "Funnel";
		categoryFinancial.Tag = "categoryFinancial";
		categoryTreemap.Tag = "Treemap";
		categorySunburst.Tag = "Sunburst";
		categoryHistogram.Tag = "Histogram";
		categoryBoxAndWhisker.Tag = "BoxAndWhisker";
		categoryWaterfall.Tag = "Waterfall";
		categoryPictorial.Tag = "Pictorial";
		typeArea.Tag = StiChartSeriesType.Area;
		typeBubble.Tag = StiChartSeriesType.Bubble;
		typeClusteredBar.Tag = StiChartSeriesType.ClusteredBar;
		typeClusteredColumn.Tag = StiChartSeriesType.ClusteredColumn;
		typePareto.Tag = StiChartSeriesType.Pareto;
		typeHistogram.Tag = StiChartSeriesType.Histogram;
		typeDoughnut.Tag = StiChartSeriesType.Doughnut;
		typeFullStackedArea.Tag = StiChartSeriesType.FullStackedArea;
		typeFullStackedBar.Tag = StiChartSeriesType.FullStackedBar;
		typeFullStackedColumn.Tag = StiChartSeriesType.FullStackedColumn;
		typeFullStackedLine.Tag = StiChartSeriesType.FullStackedLine;
		typeFullStackedSpline.Tag = StiChartSeriesType.FullStackedSpline;
		typeFullStackedSplineArea.Tag = StiChartSeriesType.FullStackedSplineArea;
		typeFunnel.Tag = StiChartSeriesType.Funnel;
		typeFunnelWeightedSlices.Tag = StiChartSeriesType.FunnelWeightedSlices;
		typeCandlestick.Tag = StiChartSeriesType.Candlestick;
		typeTreemap.Tag = StiChartSeriesType.Treemap;
		typeGantt.Tag = StiChartSeriesType.Gantt;
		typeLine.Tag = StiChartSeriesType.Line;
		typePie.Tag = StiChartSeriesType.Pie;
		typePie3d.Tag = StiChartSeriesType.Pie3d;
		typePictorial.Tag = StiChartSeriesType.Pictorial;
		typeRadarPoint.Tag = StiChartSeriesType.RadarPoint;
		typeRadarLine.Tag = StiChartSeriesType.RadarLine;
		typeRadarArea.Tag = StiChartSeriesType.RadarArea;
		typeRange.Tag = StiChartSeriesType.Range;
		typeRangeBar.Tag = StiChartSeriesType.RangeBar;
		typeScatter.Tag = StiChartSeriesType.Scatter;
		typeScatterLine.Tag = StiChartSeriesType.ScatterLine;
		typeScatterSpline.Tag = StiChartSeriesType.ScatterSpline;
		typeSpline.Tag = StiChartSeriesType.Spline;
		typeSplineArea.Tag = StiChartSeriesType.SplineArea;
		typeSplineRange.Tag = StiChartSeriesType.SplineRange;
		typeStackedArea.Tag = StiChartSeriesType.StackedArea;
		typeStackedBar.Tag = StiChartSeriesType.StackedBar;
		typeStackedColumn.Tag = StiChartSeriesType.StackedColumn;
		typeStackedLine.Tag = StiChartSeriesType.StackedLine;
		typeStackedSpline.Tag = StiChartSeriesType.StackedSpline;
		typeStackedSplineArea.Tag = StiChartSeriesType.StackedSplineArea;
		typeSteppedArea.Tag = StiChartSeriesType.SteppedArea;
		typeSteppedLine.Tag = StiChartSeriesType.SteppedLine;
		typeSteppedRange.Tag = StiChartSeriesType.SteppedRange;
		typeStock.Tag = StiChartSeriesType.Stock;
		typeSunburst.Tag = StiChartSeriesType.Sunburst;
		typeWaterfall.Tag = StiChartSeriesType.Waterfall;
		typeBoxAndWhisker.Tag = StiChartSeriesType.BoxAndWhisker;
	}

	private void InitSeriesIcons()
	{
		foreach (ButtonItem subItem in buttonSeriesType.SubItems)
		{
			StiResourcesHelper.SetImage(subItem, pathImage + (subItem.Tag as string) + ".png");
			foreach (ButtonItem subItem2 in subItem.SubItems)
			{
				StiResourcesHelper.SetImage(subItem2, $"{pathImage}{(StiChartSeriesType)subItem2.Tag}.png");
			}
		}
	}

	private void CreateMenuItems()
	{
		if (isItemsInitialized)
		{
			return;
		}
		menuExpression.SubItems.Clear();
		StiDataColumn stiDataColumn = ((((Control)textBoxExpression).Text == LocNotDefined) ? null : StiDataColumn.GetDataColumnFromColumnName(Chart.Report.Dictionary, ((Control)textBoxExpression).Text));
		foreach (StiDataSource dataSource in Chart.Report.Dictionary.DataSources)
		{
			bool flag = stiDataColumn != null && dataSource.Columns.Contains(stiDataColumn);
			ButtonItem buttonItem = new ButtonItem
			{
				Text = dataSource.ToString(StiOptions.Dictionary.ShowOnlyAliasForDataSource && UseAliases.GetValueOrDefault(false)),
				Image = StiDataBuilder.Images.Images["DataSource"],
				FontBold = flag,
				HotFontBold = flag,
				Tag = dataSource
			};
			menuExpression.SubItems.Add(buttonItem);
			buttonItem.SubItems.Add(new ButtonItem());
			buttonItem.PopupOpen += MenuItemDataSource_PopupOpen;
		}
		isItemsInitialized = true;
	}

	private bool ShowChartItemAtProperties(StiChartDataColumnListBox listBox)
	{
		StiSeries stiSeries = listBox.SelectedChartItem?.Meter?.Series;
		if (!lockRefresh && stiSeries != null)
		{
			StiPropertiesPanelHelper.ActivatePanel(Designer, stiSeries);
			return true;
		}
		return false;
	}

	private void SeriesItem_Click(object sender, EventArgs e)
	{
		ButtonItem buttonItem = sender as ButtonItem;
		if (ChartValue == null || buttonItem == null)
		{
			return;
		}
		StiChartSeriesType stiChartSeriesType = (StiChartSeriesType)buttonItem.Tag;
		ChartValue.SeriesType = stiChartSeriesType;
		if (((ListBox)listBoxValues).Items.Count > 1)
		{
			List<StiChartSeriesType> group = StiChartGroups.GetGroup(stiChartSeriesType);
			foreach (StiChartItem item in ((ListBox)listBoxValues).Items)
			{
				StiDataColumnMeter stiDataColumnMeter = item?.Meter;
				if (!group.Contains(stiDataColumnMeter.SeriesType))
				{
					stiDataColumnMeter.SeriesType = stiChartSeriesType;
					item.Image = GetNodeImageKey(stiDataColumnMeter.SeriesType);
				}
			}
		}
		listBoxValues.SetSelectedImage(GetNodeImageKey(ChartValue.SeriesType));
		WritePropertiesToElement();
		InitButtonSeriesType();
		RefreshUiPanel();
		StiSeries stiSeries = listBoxValues.SelectedChartItem?.Meter?.Series;
		if (stiSeries != null)
		{
			StiPropertiesPanelHelper.ActivatePanel(Designer, stiSeries);
		}
	}

	private void ButtonSeriesType_PopupOpen(object sender, EventArgs e)
	{
		foreach (ButtonItem subItem in buttonSeriesType.SubItems)
		{
			foreach (ButtonItem subItem2 in subItem.SubItems)
			{
				StiChartSeriesType stiChartSeriesType = (StiChartSeriesType)subItem2.Tag;
				subItem2.Checked = stiChartSeriesType == ChartValue.SeriesType;
			}
		}
	}

	private void ListBox_ElementsConverting(object sender, EventArgs e)
	{
		StiChartItem stiChartItem = sender as StiChartItem;
		if (stiChartItem.List == listBoxValues)
		{
			stiChartItem.Image = GetNodeImageKey(stiChartItem.Meter.SeriesType);
			stiChartItem.Meter.SeriesType = ((Chart.Series.Count == 0 && OldSeriesType.HasValue) ? OldSeriesType.Value : stiChartItem.Meter.SeriesType);
			stiChartItem.Meter.Series = null;
			stiChartItem.Meter.Series = ConvertValueToSeries(stiChartItem.Meter);
		}
	}

	private void ListBox_ElementsChanged(object sender, StiElementChangedArgs changedArgs)
	{
		ProcessElementChanging(changedArgs);
		WritePropertiesToElement();
		RefreshUiPanel();
		if (sender != null)
		{
			StiChartDataColumnListBox stiChartDataColumnListBox = sender as StiChartDataColumnListBox;
			StiSeries stiSeries = stiChartDataColumnListBox.SelectedChartItem?.Meter?.Series;
			if (stiSeries != null)
			{
				StiPropertiesPanelHelper.ActivatePanel(Designer, stiSeries);
			}
			else
			{
				StiDictionaryPanelHelper.ActivatePanel(Designer);
			}
		}
	}

	private void ListBox_ElementsDragEnter(object sender, EventArgs e)
	{
		ResetComponentHighlights();
	}

	private void ListBox_ElementsAfterSelect(object sender, EventArgs e)
	{
		selectedListBox = sender as StiChartDataColumnListBox;
		RefreshExpression();
		WritePropertiesToElement();
		RefreshUiPanel();
	}

	private void ListBox_Enter(object sender, EventArgs e)
	{
		selectedListBox = sender as StiChartDataColumnListBox;
		RefreshExpression();
	}

	private void ListBox_AfterSelect(object sender, EventArgs e)
	{
		selectedListBox = sender as StiChartDataColumnListBox;
		RefreshExpression();
		ShowChartItemAtProperties(selectedListBox);
	}

	private void ListBox_MouseDown(object sender, MouseEventArgs e)
	{
		selectedListBox = sender as StiChartDataColumnListBox;
		RefreshExpression();
		if (!lockRefresh)
		{
			if (((ListBox)selectedListBox).Items.Count == 0)
			{
				StiDictionaryPanelHelper.ActivatePanel(Designer);
			}
			else
			{
				ShowChartItemAtProperties(selectedListBox);
			}
		}
	}

	private void ButtonIcon_Click(object sender, EventArgs e)
	{
		((ToolStripDropDown)iconsPopup).Show(new Point(((Control)panelBoxIcon).RectangleToScreen(Rectangle.Empty).Left, ((Control)buttonIcon).RectangleToScreen(Rectangle.Empty).Bottom + ((Control)buttonIcon).Height + StiScale.I2));
		((Control)textBoxSearch).Focus();
	}

	private void TextBoxSearch_TextChanged(object sender, EventArgs e)
	{
		StiHintTextBox stiHintTextBox = sender as StiHintTextBox;
		string filterName = ((Control)stiHintTextBox).Text.ToLower();
		BuildIconPanel(filterName);
	}

	private void FontIconSelectionControl_ValueChanged(object sender, EventArgs e)
	{
		PictorialIcon = ((StiFontIconSelectionControl)sender).SelectedIcon.GetValueOrDefault();
		RefreshElementView();
		RefreshSampleIcon();
		((Control)iconsPopup).Hide();
	}

	private void PanelBoxIcon_Paint(object sender, PaintEventArgs e)
	{
		RefreshSampleIcon();
	}

	private void ButtonExpressionMenu_Click(object sender, EventArgs e)
	{
		CreateMenuItems();
		Point point = new Point(((Control)panelBoxExpression).Width, ((Control)panelBoxExpression).Height + StiScale.I1);
		if (StiLeftRightHandedHelper.IsLeftHanded)
		{
			point.X -= menuExpression.PopupSize.Width;
		}
		point = ((Control)panelBoxExpression).PointToScreen(point);
		menuExpression.Popup(point);
	}

	protected void MenuItemDataSource_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		ButtonItem buttonItem = sender as ButtonItem;
		StiDataSource stiDataSource = buttonItem.Tag as StiDataSource;
		StiDataColumn stiDataColumn = ((((Control)textBoxExpression).Text == LocNotDefined) ? null : StiDataColumn.GetDataColumnFromColumnName(Chart.Report.Dictionary, ((Control)textBoxExpression).Text));
		buttonItem.SubItems.Clear();
		foreach (StiDataColumn column in stiDataSource.Columns)
		{
			bool flag = stiDataColumn == column;
			ButtonItem buttonItem2 = new ButtonItem
			{
				Checked = flag,
				Image = StiDataBuilder.Images.Images[StiDataBuilder.GetImageKeyFromColumn(column)],
				FontBold = flag,
				HotFontBold = flag,
				Tag = column,
				Text = column.ToString(StiOptions.Dictionary.ShowOnlyAliasForDataColumn && UseAliases.GetValueOrDefault(false))
			};
			buttonItem2.Click += MenuItemDataColumn_Click;
			buttonItem.SubItems.Add(buttonItem2);
		}
	}

	private void MenuItemDataColumn_Click(object sender, EventArgs e)
	{
		ButtonItem item = (ButtonItem)sender;
		(from ButtonItem b in item.Parent.SubItems
			where b != item
			select b).ToList().ForEach(delegate(ButtonItem b)
		{
			bool flag3 = (b.Checked = false);
			bool fontBold2 = (b.HotFontBold = flag3);
			b.FontBold = fontBold2;
		});
		ButtonItem buttonItem = item.Parent as ButtonItem;
		item.Checked = true;
		ButtonItem buttonItem2 = item;
		bool fontBold = (item.HotFontBold = true);
		buttonItem2.FontBold = fontBold;
		StiDataSource stiDataSource = buttonItem.Tag as StiDataSource;
		StiDataColumn stiDataColumn = item.Tag as StiDataColumn;
		string text = stiDataSource.Name + "." + stiDataColumn.NameInSource;
		string text2 = StiCodeDomSerializator.ReplaceSymbols(stiDataSource.Name) + "." + StiCodeDomSerializator.ReplaceSymbols(stiDataColumn.NameInSource);
		((Control)textBoxExpression).Text = text;
		((TextBoxBase)textBoxExpression).Select(0, 0);
		selectedListBox?.SetSelectedItem(text, stiDataColumn);
		RefreshElementView();
		RefreshUiPanel();
	}

	private void ButtonSeries_Click(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)1;
		This_OkClick(sender, e);
		StiChartSeriesEditor.ProcessChart(Chart);
	}

	private void PropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
	{
		foreach (StiChartDataColumnListBox item in (from Control c in (IEnumerable)((Control)panelListBoxes).Controls
			where c is StiChartDataColumnListBox
			select c).Cast<StiChartDataColumnListBox>())
		{
			((Control)item).Invalidate();
		}
		RefreshElementView();
	}

	protected override void OnSizeChanged(EventArgs e)
	{
		((Control)this).OnSizeChanged(e);
		UpdateFormPosition();
	}

	protected override void OnFormClosed(FormClosedEventArgs e)
	{
		base.OnFormClosed(e);
		StiPropertyGrid propertyGrid = StiPropertiesPanelHelper.GetPropertyGrid(Designer);
		if (propertyGrid != null)
		{
			((PropertyGrid)propertyGrid).SelectedObject = Chart;
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		base.OnLoad(e);
		iconsPopup = new StiPopup((Control)(object)GetIconPanel());
		((Control)panelBoxIcon).Paint += new PaintEventHandler(PanelBoxIcon_Paint);
	}

	protected override void OnClosed(EventArgs e)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Invalid comparison between Unknown and I4
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		((Form)this).OnClosed(e);
		lock (StiInplaceEditorForm.FormClosingObject)
		{
			if (!base.IsFormClosed)
			{
				base.IsFormClosed = true;
				if ((int)((Form)this).DialogResult != 1)
				{
					((PropertyGrid)PropertyGrid).SelectedObject = Chart;
					StiActions.Clipboard.InvokeUndo(Designer, closeAllInplaceDesigners: false);
					Designer.UndoRedo.RedoStack.Clear();
					((Control)Designer).Refresh();
				}
				((PropertyGrid)PropertyGrid).PropertyValueChanged -= new PropertyValueChangedEventHandler(PropertyGrid_PropertyValueChanged);
			}
		}
	}

	protected override void OnHelpButtonClicked(CancelEventArgs e)
	{
		((Form)this).OnHelpButtonClicked(e);
		StiHelpProvider.ShowHelpViewer("user-manual/dashboards_chart.htm");
	}

	private void This_MoreOptionsClick(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)1;
		Chart.EditorType = StiChartEditorType.Advanced;
		This_OkClick(sender, e);
		new StiChartDesigner(Designer).OnDoubleClick(Chart);
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		WritePropertiesToElement();
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
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Expected O, but got Unknown
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Expected O, but got Unknown
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Expected O, but got Unknown
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Expected O, but got Unknown
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ce: Expected O, but got Unknown
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d9: Expected O, but got Unknown
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e4: Expected O, but got Unknown
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Expected O, but got Unknown
		//IL_0411: Unknown result type (might be due to invalid IL or missing references)
		//IL_041b: Expected O, but got Unknown
		//IL_041c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0426: Expected O, but got Unknown
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Expected O, but got Unknown
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Expected O, but got Unknown
		//IL_0fcc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1036: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b6: Expected O, but got Unknown
		//IL_10f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1220: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_1acd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b45: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b4f: Expected O, but got Unknown
		//IL_1b79: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c74: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c7e: Expected O, but got Unknown
		//IL_1cab: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1da6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1db0: Expected O, but got Unknown
		//IL_1ddd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e60: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ed9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ee3: Expected O, but got Unknown
		//IL_1f10: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f93: Unknown result type (might be due to invalid IL or missing references)
		//IL_200c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2016: Expected O, but got Unknown
		//IL_2043: Unknown result type (might be due to invalid IL or missing references)
		//IL_20c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2140: Unknown result type (might be due to invalid IL or missing references)
		//IL_214a: Expected O, but got Unknown
		//IL_2177: Unknown result type (might be due to invalid IL or missing references)
		//IL_21f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_229e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2305: Unknown result type (might be due to invalid IL or missing references)
		//IL_23d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_24a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_24fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_25a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_264c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2722: Unknown result type (might be due to invalid IL or missing references)
		//IL_27c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2847: Unknown result type (might be due to invalid IL or missing references)
		//IL_28ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_2950: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b74: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bf8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c83: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d6a: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		toolTip = new ToolTip(components);
		typeClusteredColumn = new ButtonItem();
		typeStackedColumn = new ButtonItem();
		typeFullStackedColumn = new ButtonItem();
		typePareto = new ButtonItem();
		typeHistogram = new ButtonItem();
		typeLine = new ButtonItem();
		typeStackedLine = new ButtonItem();
		typeFullStackedLine = new ButtonItem();
		typeSpline = new ButtonItem();
		typeStackedSpline = new ButtonItem();
		typeFullStackedSpline = new ButtonItem();
		typeSteppedLine = new ButtonItem();
		typeArea = new ButtonItem();
		typeStackedArea = new ButtonItem();
		typeFullStackedArea = new ButtonItem();
		typeSplineArea = new ButtonItem();
		typeStackedSplineArea = new ButtonItem();
		typeFullStackedSplineArea = new ButtonItem();
		typeSteppedArea = new ButtonItem();
		typeRange = new ButtonItem();
		typeSplineRange = new ButtonItem();
		typeSteppedRange = new ButtonItem();
		typeRangeBar = new ButtonItem();
		typeClusteredBar = new ButtonItem();
		typeStackedBar = new ButtonItem();
		typeFullStackedBar = new ButtonItem();
		typeScatter = new ButtonItem();
		typeScatterLine = new ButtonItem();
		typeScatterSpline = new ButtonItem();
		typePie = new ButtonItem();
		typePie3d = new ButtonItem();
		typeRadarPoint = new ButtonItem();
		typeRadarLine = new ButtonItem();
		typeRadarArea = new ButtonItem();
		typeFunnel = new ButtonItem();
		typeFunnelWeightedSlices = new ButtonItem();
		typeCandlestick = new ButtonItem();
		typeStock = new ButtonItem();
		typeTreemap = new ButtonItem();
		typeGantt = new ButtonItem();
		typeDoughnut = new ButtonItem();
		typeBubble = new ButtonItem();
		typePictorial = new ButtonItem();
		typeSunburst = new ButtonItem();
		typeWaterfall = new ButtonItem();
		typeBoxAndWhisker = new ButtonItem();
		listBoxEndValues = new StiChartDataColumnListBox();
		labelEndValues = new StiChartGroupLabel();
		contextMenuBar = new ContextMenuBar();
		menuExpression = new ButtonItem();
		panelSeriesType = new Panel();
		buttonSeriesType = new StiChartButton();
		categoryClusteredColumn = new ButtonItem();
		categoryLine = new ButtonItem();
		categoryPie = new ButtonItem();
		categoryClusteredBar = new ButtonItem();
		categoryArea = new ButtonItem();
		categoryRange = new ButtonItem();
		categoryScatter = new ButtonItem();
		categoryRadar = new ButtonItem();
		categoryFunnel = new ButtonItem();
		categoryFinancial = new ButtonItem();
		categoryTreemap = new ButtonItem();
		categorySunburst = new ButtonItem();
		categoryHistogram = new ButtonItem();
		categoryBoxAndWhisker = new ButtonItem();
		categoryWaterfall = new ButtonItem();
		categoryPictorial = new ButtonItem();
		listBoxValues = new StiChartDataColumnListBox();
		labelValues = new StiChartGroupLabel();
		listBoxCloseValues = new StiChartDataColumnListBox();
		labelCloseValues = new StiChartGroupLabel();
		listBoxLowValues = new StiChartDataColumnListBox();
		labelLowValues = new StiChartGroupLabel();
		listBoxHighValues = new StiChartDataColumnListBox();
		labelHighVaules = new StiChartGroupLabel();
		listBoxWeights = new StiChartDataColumnListBox();
		labelWeights = new StiChartGroupLabel();
		listBoxArguments = new StiChartDataColumnListBox();
		labelArguments = new StiChartGroupLabel();
		labelIcon = new Label();
		panelBoxIcon = new Panel();
		buttonIcon = new StiOperationButton();
		panelIcon = new Panel();
		panelFooter = new StiGroupLine();
		buttonMoreOptions = new Button();
		buttonOk = new Button();
		buttonCancel = new Button();
		panelOptions = new Panel();
		panelBoxExpression = new StiPanelBox();
		textBoxExpression = new HintTextBox();
		buttonExpressionMenu = new StiOperationButton();
		labelField = new Label();
		panelListBoxes = new Panel();
		buttonSeries = new Button();
		labelChartType = new Label();
		((ISupportInitialize)contextMenuBar).BeginInit();
		((Control)panelSeriesType).SuspendLayout();
		((Control)panelBoxIcon).SuspendLayout();
		((Control)panelIcon).SuspendLayout();
		((Control)panelFooter).SuspendLayout();
		((Control)panelOptions).SuspendLayout();
		((Control)panelBoxExpression).SuspendLayout();
		((Control)panelListBoxes).SuspendLayout();
		((Control)this).SuspendLayout();
		typeClusteredColumn.ButtonStyle = eButtonStyle.ImageAndText;
		typeClusteredColumn.ImagePaddingHorizontal = 16;
		typeClusteredColumn.ImagePaddingVertical = 12;
		typeClusteredColumn.Name = "typeClusteredColumn";
		typeClusteredColumn.Text = "Clustered Column";
		typeStackedColumn.ButtonStyle = eButtonStyle.ImageAndText;
		typeStackedColumn.ImagePaddingHorizontal = 16;
		typeStackedColumn.ImagePaddingVertical = 12;
		typeStackedColumn.Name = "typeStackedColumn";
		typeStackedColumn.Text = "Stacked Column";
		typeFullStackedColumn.ButtonStyle = eButtonStyle.ImageAndText;
		typeFullStackedColumn.ImagePaddingHorizontal = 16;
		typeFullStackedColumn.ImagePaddingVertical = 12;
		typeFullStackedColumn.Name = "typeFullStackedColumn";
		typeFullStackedColumn.Text = "Full Stacked Column";
		typePareto.ButtonStyle = eButtonStyle.ImageAndText;
		typePareto.ImagePaddingHorizontal = 16;
		typePareto.ImagePaddingVertical = 12;
		typePareto.Name = "typePareto";
		typePareto.Text = "Pareto";
		typeHistogram.ButtonStyle = eButtonStyle.ImageAndText;
		typeHistogram.ImagePaddingHorizontal = 16;
		typeHistogram.ImagePaddingVertical = 12;
		typeHistogram.Name = "typeHistogram";
		typeHistogram.Text = "Histogram";
		typeLine.ButtonStyle = eButtonStyle.ImageAndText;
		typeLine.ImagePaddingHorizontal = 16;
		typeLine.ImagePaddingVertical = 12;
		typeLine.Name = "typeLine";
		typeLine.Text = "Line";
		typeStackedLine.ButtonStyle = eButtonStyle.ImageAndText;
		typeStackedLine.ImagePaddingHorizontal = 16;
		typeStackedLine.ImagePaddingVertical = 12;
		typeStackedLine.Name = "typeStackedLine";
		typeStackedLine.Text = "Stacked Line";
		typeFullStackedLine.ButtonStyle = eButtonStyle.ImageAndText;
		typeFullStackedLine.ImagePaddingHorizontal = 16;
		typeFullStackedLine.ImagePaddingVertical = 12;
		typeFullStackedLine.Name = "typeFullStackedLine";
		typeFullStackedLine.Text = "Full Stacked Line";
		typeSpline.BeginGroup = true;
		typeSpline.ImagePaddingHorizontal = 16;
		typeSpline.ImagePaddingVertical = 12;
		typeSpline.Name = "typeSpline";
		typeSpline.Text = "Spline";
		typeStackedSpline.ImagePaddingHorizontal = 16;
		typeStackedSpline.ImagePaddingVertical = 12;
		typeStackedSpline.Name = "typeStackedSpline";
		typeStackedSpline.Text = "Stacked Spline";
		typeFullStackedSpline.ImagePaddingHorizontal = 16;
		typeFullStackedSpline.ImagePaddingVertical = 12;
		typeFullStackedSpline.Name = "typeFullStackedSpline";
		typeFullStackedSpline.Text = "Full-Stacked Spline";
		typeSteppedLine.BeginGroup = true;
		typeSteppedLine.ImagePaddingHorizontal = 16;
		typeSteppedLine.ImagePaddingVertical = 12;
		typeSteppedLine.Name = "typeSteppedLine";
		typeSteppedLine.Text = "Stepped Line";
		typeArea.ImagePaddingHorizontal = 16;
		typeArea.ImagePaddingVertical = 12;
		typeArea.Name = "typeArea";
		typeArea.Text = "Area";
		typeStackedArea.ImagePaddingHorizontal = 16;
		typeStackedArea.ImagePaddingVertical = 12;
		typeStackedArea.Name = "typeStackedArea";
		typeStackedArea.Text = "Stacked Area";
		typeFullStackedArea.ImagePaddingHorizontal = 16;
		typeFullStackedArea.ImagePaddingVertical = 12;
		typeFullStackedArea.Name = "typeFullStackedArea";
		typeFullStackedArea.Text = "Full-Stacked Area";
		typeSplineArea.BeginGroup = true;
		typeSplineArea.ImagePaddingHorizontal = 16;
		typeSplineArea.ImagePaddingVertical = 12;
		typeSplineArea.Name = "typeSplineArea";
		typeSplineArea.Text = "Spline Area";
		typeStackedSplineArea.ImagePaddingHorizontal = 16;
		typeStackedSplineArea.ImagePaddingVertical = 12;
		typeStackedSplineArea.Name = "typeStackedSplineArea";
		typeStackedSplineArea.Text = "Stacked Spline Area";
		typeFullStackedSplineArea.ImagePaddingHorizontal = 16;
		typeFullStackedSplineArea.ImagePaddingVertical = 12;
		typeFullStackedSplineArea.Name = "typeFullStackedSplineArea";
		typeFullStackedSplineArea.Text = "Full-Stacked Spline Area";
		typeSteppedArea.BeginGroup = true;
		typeSteppedArea.ImagePaddingHorizontal = 16;
		typeSteppedArea.ImagePaddingVertical = 12;
		typeSteppedArea.Name = "typeSteppedArea";
		typeSteppedArea.Text = "Stepped Area";
		typeRange.ImagePaddingHorizontal = 16;
		typeRange.ImagePaddingVertical = 12;
		typeRange.Name = "typeRange";
		typeRange.Text = "Line Range";
		typeSplineRange.ImagePaddingHorizontal = 16;
		typeSplineRange.ImagePaddingVertical = 12;
		typeSplineRange.Name = "typeSplineRange";
		typeSplineRange.Text = "Spline Range";
		typeSteppedRange.ImagePaddingHorizontal = 16;
		typeSteppedRange.ImagePaddingVertical = 12;
		typeSteppedRange.Name = "typeSteppedRange";
		typeSteppedRange.Text = "Stepped Range";
		typeRangeBar.ImagePaddingHorizontal = 16;
		typeRangeBar.ImagePaddingVertical = 12;
		typeRangeBar.Name = "typeRangeBar";
		typeRangeBar.Text = "Range Bar";
		typeClusteredBar.ImagePaddingHorizontal = 16;
		typeClusteredBar.ImagePaddingVertical = 12;
		typeClusteredBar.Name = "typeClusteredBar";
		typeClusteredBar.Text = "Clustered Bar";
		typeStackedBar.ImagePaddingHorizontal = 16;
		typeStackedBar.ImagePaddingVertical = 12;
		typeStackedBar.Name = "typeStackedBar";
		typeStackedBar.Text = "Stacked Bar";
		typeFullStackedBar.ImagePaddingHorizontal = 16;
		typeFullStackedBar.ImagePaddingVertical = 12;
		typeFullStackedBar.Name = "typeFullStackedBar";
		typeFullStackedBar.Text = "Full-Stacked Bar";
		typeScatter.ImagePaddingHorizontal = 16;
		typeScatter.ImagePaddingVertical = 12;
		typeScatter.Name = "typeScatter";
		typeScatter.Text = "Scatter";
		typeScatterLine.ImagePaddingHorizontal = 16;
		typeScatterLine.ImagePaddingVertical = 12;
		typeScatterLine.Name = "typeScatterLine";
		typeScatterLine.Text = "Scatter Line";
		typeScatterSpline.ImagePaddingHorizontal = 16;
		typeScatterSpline.ImagePaddingVertical = 12;
		typeScatterSpline.Name = "typeScatterSpline";
		typeScatterSpline.Text = "Scatter Spline";
		typePie.ImagePaddingHorizontal = 16;
		typePie.ImagePaddingVertical = 12;
		typePie.Name = "typePie";
		typePie.Text = "Pie";
		typePie3d.ImagePaddingHorizontal = 16;
		typePie3d.ImagePaddingVertical = 12;
		typePie3d.Name = "typePie3d";
		typePie3d.Text = "3D Pie";
		typeRadarPoint.ImagePaddingHorizontal = 16;
		typeRadarPoint.ImagePaddingVertical = 12;
		typeRadarPoint.Name = "typeRadarPoint";
		typeRadarPoint.Text = "Radar Point";
		typeRadarLine.ImagePaddingHorizontal = 16;
		typeRadarLine.ImagePaddingVertical = 12;
		typeRadarLine.Name = "typeRadarLine";
		typeRadarLine.Text = "Radar Line";
		typeRadarArea.ImagePaddingHorizontal = 16;
		typeRadarArea.ImagePaddingVertical = 12;
		typeRadarArea.Name = "typeRadarArea";
		typeRadarArea.Text = "Radar Area";
		typeFunnel.ImagePaddingHorizontal = 16;
		typeFunnel.ImagePaddingVertical = 12;
		typeFunnel.Name = "typeFunnel";
		typeFunnel.Text = "Funnel";
		typeFunnelWeightedSlices.ImagePaddingHorizontal = 16;
		typeFunnelWeightedSlices.ImagePaddingVertical = 12;
		typeFunnelWeightedSlices.Name = "typeFunnelWeightedSlices";
		typeFunnelWeightedSlices.Text = "Funnel Weighted Slices";
		typeCandlestick.ImagePaddingHorizontal = 16;
		typeCandlestick.ImagePaddingVertical = 12;
		typeCandlestick.Name = "typeCandlestick";
		typeCandlestick.Text = "Candlestick";
		typeStock.ImagePaddingHorizontal = 16;
		typeStock.ImagePaddingVertical = 12;
		typeStock.Name = "typeStock";
		typeStock.Text = "Stock";
		typeTreemap.ImagePaddingHorizontal = 16;
		typeTreemap.ImagePaddingVertical = 12;
		typeTreemap.Name = "typeTreemap";
		typeTreemap.Text = "Treemap";
		typeGantt.ImagePaddingHorizontal = 16;
		typeGantt.ImagePaddingVertical = 12;
		typeGantt.Name = "typeGantt";
		typeGantt.Text = "Gantt";
		typeDoughnut.ImagePaddingHorizontal = 16;
		typeDoughnut.ImagePaddingVertical = 12;
		typeDoughnut.Name = "typeDoughnut";
		typeDoughnut.Text = "Doughnut";
		typeBubble.ImagePaddingHorizontal = 16;
		typeBubble.ImagePaddingVertical = 12;
		typeBubble.Name = "typeBubble";
		typeBubble.Text = "Bubble";
		typePictorial.ImagePaddingHorizontal = 16;
		typePictorial.ImagePaddingVertical = 12;
		typePictorial.Name = "typePictorial";
		typePictorial.Text = "Pictorial";
		typeSunburst.ImagePaddingHorizontal = 16;
		typeSunburst.ImagePaddingVertical = 12;
		typeSunburst.Name = "typeSunburst";
		typeSunburst.Text = "Sunburst";
		typeWaterfall.ImagePaddingHorizontal = 16;
		typeWaterfall.ImagePaddingVertical = 12;
		typeWaterfall.Name = "typeWaterfall";
		typeWaterfall.Text = "Wwaterfall";
		typeBoxAndWhisker.ImagePaddingHorizontal = 16;
		typeBoxAndWhisker.ImagePaddingVertical = 12;
		typeBoxAndWhisker.Name = "typeBoxAndWhisker";
		typeBoxAndWhisker.Text = "BoxAndWhisker";
		((Control)listBoxEndValues).AllowDrop = true;
		((Control)listBoxEndValues).Dock = (DockStyle)1;
		((Control)listBoxEndValues).Location = new Point(24, 209);
		((Control)listBoxEndValues).Margin = new Padding(4);
		((Control)listBoxEndValues).Name = "listBoxEndValues";
		((Control)listBoxEndValues).Size = new Size(608, 62);
		((Control)listBoxEndValues).TabIndex = 3;
		((Control)labelEndValues).Dock = (DockStyle)1;
		((Control)labelEndValues).Location = new Point(24, 147);
		((Control)labelEndValues).Margin = new Padding(4, 0, 4, 0);
		((Control)labelEndValues).Name = "labelEndValues";
		((Control)labelEndValues).Size = new Size(608, 62);
		((Control)labelEndValues).TabIndex = 2;
		((Control)labelEndValues).Text = "End Values";
		contextMenuBar.AntiAlias = true;
		contextMenuBar.DockTabStripHeight = 50;
		((Control)contextMenuBar).Font = new Font("Segoe UI", 9f);
		contextMenuBar.Items.AddRange(new BaseItem[1] { menuExpression });
		contextMenuBar.Location = new Point(380, 898);
		((Control)contextMenuBar).Margin = new Padding(4);
		contextMenuBar.Name = "contextMenuBar";
		contextMenuBar.Size = new Size(160, 47);
		contextMenuBar.Stretch = true;
		contextMenuBar.Style = eDotNetBarStyle.StyleManagerControlled;
		contextMenuBar.TabIndex = 11;
		contextMenuBar.TabStop = false;
		((Control)contextMenuBar).Text = "contextMenuBar";
		contextMenuBar.WrapItemsDock = true;
		menuExpression.AutoExpandOnClick = true;
		menuExpression.ImagePaddingHorizontal = 16;
		menuExpression.ImagePaddingVertical = 12;
		menuExpression.Name = "menuExpression";
		menuExpression.Text = "menuExpression";
		((Control)panelSeriesType).BackColor = Color.White;
		((Control)panelSeriesType).Controls.Add((Control)(object)buttonSeriesType);
		((Control)panelSeriesType).Controls.Add((Control)(object)labelChartType);
		((Control)panelSeriesType).Dock = (DockStyle)2;
		((Control)panelSeriesType).Location = new Point(24, 891);
		((Control)panelSeriesType).Margin = new Padding(4);
		((Control)panelSeriesType).Name = "panelSeriesType";
		((Control)panelSeriesType).Size = new Size(608, 100);
		((Control)panelSeriesType).TabIndex = 14;
		((Control)buttonSeriesType).AccessibleRole = (AccessibleRole)43;
		((Control)buttonSeriesType).Anchor = (AnchorStyles)13;
		buttonSeriesType.AutoExpandOnClick = true;
		buttonSeriesType.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)buttonSeriesType).Location = new Point(252, 23);
		buttonSeriesType.LockClosePopup = false;
		((Control)buttonSeriesType).Margin = new Padding(4);
		((Control)buttonSeriesType).Name = "buttonSeriesType";
		buttonSeriesType.PopupSide = ePopupSide.Right;
		((Control)buttonSeriesType).Size = new Size(356, 74);
		buttonSeriesType.Style = eDotNetBarStyle.StyleManagerControlled;
		buttonSeriesType.SubItems.AddRange(new BaseItem[16]
		{
			categoryClusteredColumn, categoryLine, categoryPie, categoryClusteredBar, categoryArea, categoryRange, categoryScatter, categoryRadar, categoryFunnel, categoryFinancial,
			categoryTreemap, categorySunburst, categoryHistogram, categoryBoxAndWhisker, categoryWaterfall, categoryPictorial
		});
		((Control)buttonSeriesType).TabIndex = 0;
		((Control)buttonSeriesType).TabStop = false;
		((Control)buttonSeriesType).Text = "Series Type";
		buttonSeriesType.TextAlignment = eButtonTextAlignment.Left;
		buttonSeriesType.PopupOpen += ButtonSeriesType_PopupOpen;
		categoryClusteredColumn.ImagePaddingHorizontal = 16;
		categoryClusteredColumn.ImagePaddingVertical = 12;
		categoryClusteredColumn.Name = "categoryClusteredColumn";
		categoryClusteredColumn.SubItems.AddRange(new BaseItem[3] { typeClusteredColumn, typeStackedColumn, typeFullStackedColumn });
		categoryClusteredColumn.Text = "Clustered Column";
		categoryLine.ImagePaddingHorizontal = 16;
		categoryLine.ImagePaddingVertical = 12;
		categoryLine.Name = "categoryLine";
		categoryLine.SubItems.AddRange(new BaseItem[7] { typeLine, typeStackedLine, typeFullStackedLine, typeSpline, typeStackedSpline, typeFullStackedSpline, typeSteppedLine });
		categoryLine.Text = "Line";
		categoryPie.ImagePaddingHorizontal = 16;
		categoryPie.ImagePaddingVertical = 12;
		categoryPie.Name = "categoryPie";
		categoryPie.SubItems.AddRange(new BaseItem[3] { typePie, typePie3d, typeDoughnut });
		categoryPie.Text = "Pie";
		categoryClusteredBar.ImagePaddingHorizontal = 16;
		categoryClusteredBar.ImagePaddingVertical = 12;
		categoryClusteredBar.Name = "categoryClusteredBar";
		categoryClusteredBar.SubItems.AddRange(new BaseItem[4] { typeClusteredBar, typeStackedBar, typeFullStackedBar, typeGantt });
		categoryClusteredBar.Text = "Clustered Bar";
		categoryArea.ImagePaddingHorizontal = 16;
		categoryArea.ImagePaddingVertical = 12;
		categoryArea.Name = "categoryArea";
		categoryArea.SubItems.AddRange(new BaseItem[7] { typeArea, typeStackedArea, typeFullStackedArea, typeSplineArea, typeStackedSplineArea, typeFullStackedSplineArea, typeSteppedArea });
		categoryArea.Text = "Area";
		categoryRange.ImagePaddingHorizontal = 16;
		categoryRange.ImagePaddingVertical = 12;
		categoryRange.Name = "categoryRange";
		categoryRange.SubItems.AddRange(new BaseItem[4] { typeRange, typeSplineRange, typeSteppedRange, typeRangeBar });
		categoryRange.Text = "Range";
		categoryScatter.ImagePaddingHorizontal = 16;
		categoryScatter.ImagePaddingVertical = 12;
		categoryScatter.Name = "categoryScatter";
		categoryScatter.SubItems.AddRange(new BaseItem[4] { typeScatter, typeScatterLine, typeScatterSpline, typeBubble });
		categoryScatter.Text = "Scatter";
		categoryRadar.ImagePaddingHorizontal = 16;
		categoryRadar.ImagePaddingVertical = 12;
		categoryRadar.Name = "categoryRadar";
		categoryRadar.SubItems.AddRange(new BaseItem[3] { typeRadarPoint, typeRadarLine, typeRadarArea });
		categoryRadar.Text = "Radar";
		categoryFunnel.ImagePaddingHorizontal = 16;
		categoryFunnel.ImagePaddingVertical = 12;
		categoryFunnel.Name = "categoryFunnel";
		categoryFunnel.SubItems.AddRange(new BaseItem[2] { typeFunnel, typeFunnelWeightedSlices });
		categoryFunnel.Text = "Funnel";
		categoryFinancial.ImagePaddingHorizontal = 16;
		categoryFinancial.ImagePaddingVertical = 12;
		categoryFinancial.Name = "categoryFinancial";
		categoryFinancial.SubItems.AddRange(new BaseItem[2] { typeCandlestick, typeStock });
		categoryFinancial.Text = "Financial";
		categoryTreemap.ImagePaddingHorizontal = 16;
		categoryTreemap.ImagePaddingVertical = 12;
		categoryTreemap.Name = "categoryTreemap";
		categoryTreemap.SubItems.AddRange(new BaseItem[1] { typeTreemap });
		categoryTreemap.Text = "Treemap";
		categorySunburst.ImagePaddingHorizontal = 16;
		categorySunburst.ImagePaddingVertical = 12;
		categorySunburst.Name = "categorySunburst";
		categorySunburst.SubItems.AddRange(new BaseItem[1] { typeSunburst });
		categorySunburst.Text = "Sunburst";
		categoryHistogram.ImagePaddingHorizontal = 16;
		categoryHistogram.ImagePaddingVertical = 12;
		categoryHistogram.Name = "categoryHistogram";
		categoryHistogram.SubItems.AddRange(new BaseItem[2] { typeHistogram, typePareto });
		categoryHistogram.Text = "Histogram";
		categoryBoxAndWhisker.ImagePaddingHorizontal = 16;
		categoryBoxAndWhisker.ImagePaddingVertical = 12;
		categoryBoxAndWhisker.Name = "categoryBoxAndWhisker";
		categoryBoxAndWhisker.SubItems.AddRange(new BaseItem[1] { typeBoxAndWhisker });
		categoryBoxAndWhisker.Text = "Waterfall";
		categoryWaterfall.ImagePaddingHorizontal = 16;
		categoryWaterfall.ImagePaddingVertical = 12;
		categoryWaterfall.Name = "categoryWaterfall";
		categoryWaterfall.SubItems.AddRange(new BaseItem[1] { typeWaterfall });
		categoryWaterfall.Text = "Waterfall";
		categoryPictorial.ImagePaddingHorizontal = 16;
		categoryPictorial.ImagePaddingVertical = 12;
		categoryPictorial.Name = "categoryPictorial";
		categoryPictorial.SubItems.AddRange(new BaseItem[1] { typePictorial });
		categoryPictorial.Text = "Other";
		((Control)listBoxValues).AllowDrop = true;
		((Control)listBoxValues).Dock = (DockStyle)1;
		((Control)listBoxValues).Location = new Point(24, 85);
		((Control)listBoxValues).Margin = new Padding(4);
		((Control)listBoxValues).Name = "listBoxValues";
		((Control)listBoxValues).Size = new Size(608, 62);
		((Control)listBoxValues).TabIndex = 1;
		((ListBox)listBoxValues).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxValues).Enter += ListBox_Enter;
		((Control)listBoxValues).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)labelValues).Dock = (DockStyle)1;
		((Control)labelValues).Location = new Point(24, 23);
		((Control)labelValues).Margin = new Padding(4, 0, 4, 0);
		((Control)labelValues).Name = "labelValues";
		((Control)labelValues).Size = new Size(608, 62);
		((Control)labelValues).TabIndex = 0;
		((Control)labelValues).Text = "Values";
		((Control)listBoxCloseValues).AllowDrop = true;
		((Control)listBoxCloseValues).Dock = (DockStyle)1;
		((Control)listBoxCloseValues).Location = new Point(24, 333);
		((Control)listBoxCloseValues).Margin = new Padding(4);
		((Control)listBoxCloseValues).Name = "listBoxCloseValues";
		((Control)listBoxCloseValues).Size = new Size(608, 62);
		((Control)listBoxCloseValues).TabIndex = 5;
		((ListBox)listBoxCloseValues).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxCloseValues).Enter += ListBox_Enter;
		((Control)listBoxCloseValues).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)labelCloseValues).Dock = (DockStyle)1;
		((Control)labelCloseValues).Location = new Point(24, 271);
		((Control)labelCloseValues).Margin = new Padding(4, 0, 4, 0);
		((Control)labelCloseValues).Name = "labelCloseValues";
		((Control)labelCloseValues).Size = new Size(608, 62);
		((Control)labelCloseValues).TabIndex = 4;
		((Control)labelCloseValues).Text = "Close Values";
		((Control)listBoxLowValues).AllowDrop = true;
		((Control)listBoxLowValues).Dock = (DockStyle)1;
		((Control)listBoxLowValues).Location = new Point(24, 457);
		((Control)listBoxLowValues).Margin = new Padding(4);
		((Control)listBoxLowValues).Name = "listBoxLowValues";
		((Control)listBoxLowValues).Size = new Size(608, 62);
		((Control)listBoxLowValues).TabIndex = 7;
		((ListBox)listBoxLowValues).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxLowValues).Enter += ListBox_Enter;
		((Control)listBoxLowValues).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)labelLowValues).Dock = (DockStyle)1;
		((Control)labelLowValues).Location = new Point(24, 395);
		((Control)labelLowValues).Margin = new Padding(4, 0, 4, 0);
		((Control)labelLowValues).Name = "labelLowValues";
		((Control)labelLowValues).Size = new Size(608, 62);
		((Control)labelLowValues).TabIndex = 6;
		((Control)labelLowValues).Text = "Low Values";
		((Control)listBoxHighValues).AllowDrop = true;
		((Control)listBoxHighValues).Dock = (DockStyle)1;
		((Control)listBoxHighValues).Location = new Point(24, 581);
		((Control)listBoxHighValues).Margin = new Padding(4);
		((Control)listBoxHighValues).Name = "listBoxHighValues";
		((Control)listBoxHighValues).Size = new Size(608, 62);
		((Control)listBoxHighValues).TabIndex = 9;
		((ListBox)listBoxHighValues).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxHighValues).Enter += ListBox_Enter;
		((Control)listBoxHighValues).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)labelHighVaules).Dock = (DockStyle)1;
		((Control)labelHighVaules).Location = new Point(24, 519);
		((Control)labelHighVaules).Margin = new Padding(4, 0, 4, 0);
		((Control)labelHighVaules).Name = "labelHighVaules";
		((Control)labelHighVaules).Size = new Size(608, 62);
		((Control)labelHighVaules).TabIndex = 8;
		((Control)labelHighVaules).Text = "High Vaules";
		((Control)listBoxWeights).AllowDrop = true;
		((Control)listBoxWeights).Dock = (DockStyle)1;
		((Control)listBoxWeights).Location = new Point(24, 829);
		((Control)listBoxWeights).Margin = new Padding(4);
		((Control)listBoxWeights).Name = "listBoxWeights";
		((Control)listBoxWeights).Size = new Size(608, 62);
		((Control)listBoxWeights).TabIndex = 13;
		((ListBox)listBoxWeights).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxWeights).Enter += ListBox_Enter;
		((Control)listBoxWeights).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)labelWeights).Dock = (DockStyle)1;
		((Control)labelWeights).Location = new Point(24, 767);
		((Control)labelWeights).Margin = new Padding(4, 0, 4, 0);
		((Control)labelWeights).Name = "labelWeights";
		((Control)labelWeights).Size = new Size(608, 62);
		((Control)labelWeights).TabIndex = 12;
		((Control)labelWeights).Text = "Weights";
		((Control)listBoxArguments).AllowDrop = true;
		((Control)listBoxArguments).Dock = (DockStyle)1;
		((Control)listBoxArguments).Location = new Point(24, 705);
		((Control)listBoxArguments).Margin = new Padding(4);
		((Control)listBoxArguments).Name = "listBoxArguments";
		((Control)listBoxArguments).Size = new Size(608, 62);
		((Control)listBoxArguments).TabIndex = 11;
		((ListBox)listBoxArguments).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxArguments).Enter += ListBox_Enter;
		((Control)listBoxArguments).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)labelArguments).Dock = (DockStyle)1;
		((Control)labelArguments).Location = new Point(24, 643);
		((Control)labelArguments).Margin = new Padding(4, 0, 4, 0);
		((Control)labelArguments).Name = "labelArguments";
		((Control)labelArguments).Size = new Size(608, 62);
		((Control)labelArguments).TabIndex = 10;
		((Control)labelArguments).Text = "Arguments";
		((Control)labelIcon).ForeColor = Color.DimGray;
		((Control)labelIcon).Location = new Point(0, 25);
		((Control)labelIcon).Margin = new Padding(4, 0, 4, 0);
		((Control)labelIcon).Name = "labelIcon";
		((Control)labelIcon).Size = new Size(194, 42);
		((Control)labelIcon).TabIndex = 9;
		((Control)labelIcon).Text = "Icon";
		labelIcon.TextAlign = (ContentAlignment)16;
		((Control)panelBoxIcon).Anchor = (AnchorStyles)13;
		((Control)panelBoxIcon).BackColor = Color.White;
		((Control)panelBoxIcon).Controls.Add((Control)(object)buttonIcon);
		((Control)panelBoxIcon).Location = new Point(252, 23);
		((Control)panelBoxIcon).Margin = new Padding(4);
		((Control)panelBoxIcon).Name = "panelBoxIcon";
		((Control)panelBoxIcon).Size = new Size(252, 48);
		((Control)panelBoxIcon).TabIndex = 2;
		((Control)buttonIcon).Anchor = (AnchorStyles)9;
		((Control)buttonIcon).Location = new Point(204, 2);
		((Control)buttonIcon).Margin = new Padding(4);
		((Control)buttonIcon).Name = "buttonIcon";
		((Control)buttonIcon).Size = new Size(46, 44);
		((Control)buttonIcon).TabIndex = 1;
		((Control)buttonIcon).TabStop = false;
		((ButtonBase)buttonIcon).UseVisualStyleBackColor = true;
		((Control)buttonIcon).Click += ButtonIcon_Click;
		((Control)panelIcon).BackColor = Color.White;
		((Control)panelIcon).Controls.Add((Control)(object)panelBoxIcon);
		((Control)panelIcon).Controls.Add((Control)(object)labelIcon);
		((Control)panelIcon).Dock = (DockStyle)2;
		((Control)panelIcon).Location = new Point(24, 1066);
		((Control)panelIcon).Margin = new Padding(6);
		((Control)panelIcon).Name = "panelIcon";
		((Control)panelIcon).Size = new Size(608, 77);
		((Control)panelIcon).TabIndex = 16;
		((Control)panelFooter).BackColor = Color.White;
		((Control)panelFooter).Controls.Add((Control)(object)buttonMoreOptions);
		((Control)panelFooter).Controls.Add((Control)(object)buttonOk);
		((Control)panelFooter).Controls.Add((Control)(object)buttonCancel);
		((Control)panelFooter).Dock = (DockStyle)2;
		panelFooter.LinePosition = StiLinePosition.Top;
		panelFooter.LineStyle = StiLineStyle.Dot;
		((Control)panelFooter).Location = new Point(0, 1235);
		((Control)panelFooter).Margin = new Padding(6);
		((Control)panelFooter).Name = "panelFooter";
		((Control)panelFooter).Size = new Size(656, 88);
		((Control)panelFooter).TabIndex = 17;
		((Control)buttonMoreOptions).Location = new Point(24, 23);
		((Control)buttonMoreOptions).Margin = new Padding(6);
		((Control)buttonMoreOptions).Name = "buttonMoreOptions";
		((Control)buttonMoreOptions).Size = new Size(194, 46);
		((Control)buttonMoreOptions).TabIndex = 2;
		((Control)buttonMoreOptions).Text = "More Options";
		((ButtonBase)buttonMoreOptions).UseVisualStyleBackColor = true;
		((Control)buttonMoreOptions).Click += This_MoreOptionsClick;
		((Control)buttonOk).Anchor = (AnchorStyles)9;
		buttonOk.DialogResult = (DialogResult)1;
		((Control)buttonOk).Location = new Point(320, 23);
		((Control)buttonOk).Margin = new Padding(6);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(150, 46);
		((Control)buttonOk).TabIndex = 0;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += This_OkClick;
		((Control)buttonCancel).Anchor = (AnchorStyles)9;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(484, 23);
		((Control)buttonCancel).Margin = new Padding(6);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(150, 46);
		((Control)buttonCancel).TabIndex = 1;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)buttonCancel).Click += This_CancelClick;
		((Control)panelOptions).BackColor = Color.White;
		((Control)panelOptions).Controls.Add((Control)(object)panelBoxExpression);
		((Control)panelOptions).Controls.Add((Control)(object)labelField);
		((Control)panelOptions).Dock = (DockStyle)2;
		((Control)panelOptions).Location = new Point(24, 991);
		((Control)panelOptions).Margin = new Padding(4);
		((Control)panelOptions).Name = "panelOptions";
		((Control)panelOptions).Size = new Size(608, 75);
		((Control)panelOptions).TabIndex = 15;
		((Control)panelBoxExpression).Anchor = (AnchorStyles)13;
		((Control)panelBoxExpression).BackColor = Color.WhiteSmoke;
		((Control)panelBoxExpression).Controls.Add((Control)(object)textBoxExpression);
		((Control)panelBoxExpression).Controls.Add((Control)(object)buttonExpressionMenu);
		((Control)panelBoxExpression).Location = new Point(252, 23);
		((Control)panelBoxExpression).Margin = new Padding(4);
		((Control)panelBoxExpression).Name = "panelBoxExpression";
		((Control)panelBoxExpression).Size = new Size(356, 48);
		((Control)panelBoxExpression).TabIndex = 0;
		((Control)textBoxExpression).Anchor = (AnchorStyles)13;
		((Control)textBoxExpression).BackColor = Color.WhiteSmoke;
		((TextBoxBase)textBoxExpression).BorderStyle = (BorderStyle)0;
		((Control)textBoxExpression).Location = new Point(8, 12);
		((Control)textBoxExpression).Margin = new Padding(4);
		((Control)textBoxExpression).Name = "textBoxExpression";
		((TextBoxBase)textBoxExpression).ReadOnly = true;
		((Control)textBoxExpression).Size = new Size(292, 24);
		((Control)textBoxExpression).TabIndex = 0;
		((Control)buttonExpressionMenu).Anchor = (AnchorStyles)9;
		((Control)buttonExpressionMenu).Location = new Point(308, 2);
		((Control)buttonExpressionMenu).Margin = new Padding(4);
		((Control)buttonExpressionMenu).Name = "buttonExpressionMenu";
		((Control)buttonExpressionMenu).Size = new Size(46, 44);
		((Control)buttonExpressionMenu).TabIndex = 2;
		((Control)buttonExpressionMenu).TabStop = false;
		((ButtonBase)buttonExpressionMenu).UseVisualStyleBackColor = true;
		((Control)buttonExpressionMenu).Click += ButtonExpressionMenu_Click;
		((Control)labelField).ForeColor = Color.DimGray;
		((Control)labelField).Location = new Point(0, 25);
		((Control)labelField).Margin = new Padding(6, 0, 6, 0);
		((Control)labelField).Name = "labelField";
		((Control)labelField).Size = new Size(236, 44);
		((Control)labelField).TabIndex = 9;
		((Control)labelField).Text = "Field";
		labelField.TextAlign = (ContentAlignment)16;
		((Control)panelListBoxes).AutoSize = true;
		panelListBoxes.AutoSizeMode = (AutoSizeMode)0;
		((Control)panelListBoxes).Controls.Add((Control)(object)buttonSeries);
		((Control)panelListBoxes).Controls.Add((Control)(object)panelSeriesType);
		((Control)panelListBoxes).Controls.Add((Control)(object)panelOptions);
		((Control)panelListBoxes).Controls.Add((Control)(object)listBoxWeights);
		((Control)panelListBoxes).Controls.Add((Control)(object)labelWeights);
		((Control)panelListBoxes).Controls.Add((Control)(object)listBoxArguments);
		((Control)panelListBoxes).Controls.Add((Control)(object)labelArguments);
		((Control)panelListBoxes).Controls.Add((Control)(object)listBoxHighValues);
		((Control)panelListBoxes).Controls.Add((Control)(object)labelHighVaules);
		((Control)panelListBoxes).Controls.Add((Control)(object)listBoxLowValues);
		((Control)panelListBoxes).Controls.Add((Control)(object)labelLowValues);
		((Control)panelListBoxes).Controls.Add((Control)(object)listBoxCloseValues);
		((Control)panelListBoxes).Controls.Add((Control)(object)labelCloseValues);
		((Control)panelListBoxes).Controls.Add((Control)(object)listBoxEndValues);
		((Control)panelListBoxes).Controls.Add((Control)(object)labelEndValues);
		((Control)panelListBoxes).Controls.Add((Control)(object)listBoxValues);
		((Control)panelListBoxes).Controls.Add((Control)(object)labelValues);
		((Control)panelListBoxes).Controls.Add((Control)(object)panelIcon);
		((Control)panelListBoxes).Dock = (DockStyle)1;
		((Control)panelListBoxes).Location = new Point(0, 0);
		((Control)panelListBoxes).Margin = new Padding(4);
		((Control)panelListBoxes).Name = "panelListBoxes";
		((Control)panelListBoxes).Padding = new Padding(24, 23, 24, 23);
		((Control)panelListBoxes).Size = new Size(656, 1166);
		((Control)panelListBoxes).TabIndex = 18;
		((Control)buttonSeries).Anchor = (AnchorStyles)9;
		((Control)buttonSeries).Location = new Point(586, 29);
		((Control)buttonSeries).Margin = new Padding(6);
		((Control)buttonSeries).Name = "buttonSeries";
		((Control)buttonSeries).Size = new Size(46, 46);
		((Control)buttonSeries).TabIndex = 17;
		((ButtonBase)buttonSeries).UseVisualStyleBackColor = true;
		((Control)buttonSeries).Click += ButtonSeries_Click;
		((Control)labelChartType).ForeColor = Color.DimGray;
		((Control)labelChartType).Location = new Point(0, 23);
		((Control)labelChartType).Margin = new Padding(6, 0, 6, 0);
		((Control)labelChartType).Name = "labelChartType";
		((Control)labelChartType).Size = new Size(236, 74);
		((Control)labelChartType).TabIndex = 9;
		((Control)labelChartType).Text = "Chart Type";
		labelChartType.TextAlign = (ContentAlignment)16;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).AutoSize = true;
		((Form)this).AutoSizeMode = (AutoSizeMode)0;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(656, 1323);
		((Control)this).Controls.Add((Control)(object)panelListBoxes);
		((Control)this).Controls.Add((Control)(object)contextMenuBar);
		((Control)this).Controls.Add((Control)(object)panelFooter);
		((Form)this).Margin = new Padding(4);
		((Control)this).MinimumSize = new Size(652, 71);
		((Control)this).Name = "StiChartEditorFormV2";
		((Control)this).Text = "Chart";
		((ISupportInitialize)contextMenuBar).EndInit();
		((Control)panelSeriesType).ResumeLayout(false);
		((Control)panelBoxIcon).ResumeLayout(false);
		((Control)panelIcon).ResumeLayout(false);
		((Control)panelFooter).ResumeLayout(false);
		((Control)panelOptions).ResumeLayout(false);
		((Control)panelBoxExpression).ResumeLayout(false);
		((Control)panelBoxExpression).PerformLayout();
		((Control)panelListBoxes).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
