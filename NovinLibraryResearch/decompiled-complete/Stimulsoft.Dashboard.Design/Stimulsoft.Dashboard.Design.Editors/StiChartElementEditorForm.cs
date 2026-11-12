using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Components.Chart;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Dashboard.Design.Controls;
using Stimulsoft.Dashboard.Design.Editors.Helpers;
using Stimulsoft.Dashboard.Design.Editors.Menu;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Data.Extensions;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.Components.Controls;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Helpers;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Design.Panels;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Helper;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Dashboard.Design.Editors;

public class StiChartElementEditorForm : StiElementEditorForm
{
	private const string ColumnValue = "Value";

	private const string ColumnEndValue = "EndValue";

	private const string ColumnCloseValue = "CloseValue";

	private const string ColumnLowValue = "LowValue";

	private const string ColumnHighValue = "HighValue";

	private const string ColumnArgument = "Argument";

	private const string ColumnWeight = "Weight";

	private const string ColumnSeries = "Series";

	private string pathImage = "Stimulsoft.Report.Design.Resources.Office2013.Chart.";

	private Stimulsoft.Dashboard.Design.Controls.StiMeterListBox selectedListBox;

	private StiPopup iconsPopup;

	private Panel panelIcons;

	private StiHintTextBox textBoxSearch;

	private bool isShowMore;

	private Timer timerExpressionChanged;

	private Timer timerRefreshGrid;

	private IContainer components;

	private ButtonItem menuExpression;

	private ButtonItem menuViewStates;

	private Stimulsoft.Dashboard.Design.Controls.StiMeterListBox listBoxValues;

	private ToolTip toolTip;

	private Stimulsoft.Dashboard.Design.Controls.StiChartButton buttonSeriesType;

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

	private ButtonItem typeBoxAndWhisker;

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

	private StiLabelUX labelExpression;

	private StiPanelUX panelSeriesType;

	private StiPanelUX panelOptions;

	private ContextMenuBar contextMenuBar;

	private StiPanelBox panelBoxExpression;

	private HintTextBox textBoxExpression;

	private StiOperationButton buttonExpressionMenu;

	private Stimulsoft.Dashboard.Design.Controls.StiMeterListBox listBoxEndValues;

	private StiChartGroupLabel labelEndValues;

	private Stimulsoft.Dashboard.Design.Controls.StiMeterListBox listBoxCloseValues;

	private StiChartGroupLabel labelCloseValues;

	private Stimulsoft.Dashboard.Design.Controls.StiMeterListBox listBoxLowValues;

	private StiChartGroupLabel labelLowValues;

	private Stimulsoft.Dashboard.Design.Controls.StiMeterListBox listBoxHighValues;

	private StiChartGroupLabel labelHighValues;

	private Stimulsoft.Dashboard.Design.Controls.StiMeterListBox listBoxSeries;

	private StiChartGroupLabel labelSeries;

	private Stimulsoft.Dashboard.Design.Controls.StiMeterListBox listBoxWeights;

	private StiChartGroupLabel labelWeights;

	private Stimulsoft.Dashboard.Design.Controls.StiMeterListBox listBoxArguments;

	private StiChartGroupLabel labelArguments;

	private StiOperationButton buttonExpressionEdit;

	private StiPanelUX panelIcon;

	private StiPanelUX panelBoxIcon;

	private StiOperationButton buttonIcon;

	private StiLabelUX labelIcon;

	private StiPanelUX panelYAxis;

	private StiLabelUX labelYAxis;

	private StiComboBox comboBoxYAxis;

	private StiPanelUX panelLineSettings;

	private StiLabelUX labelLineStyle;

	private StiTypesButton buttonLineStyle;

	private ButtonItem buttonSolid;

	private ButtonItem buttonDash;

	private ButtonItem buttonDashDot;

	private ButtonItem buttonDashDotDot;

	private ButtonItem buttonDot;

	private ButtonItem buttonNone;

	private ImageList imageListPenStyle;

	private StiLabelUX labelLineWidth;

	private StiNumericUpDown numericUpDownLineWidth;

	private StiPanelUX panelMoreOptions;

	private StiButtonUX buttonShowMore;

	private StiPanelUX panelShowZeros;

	private StiPanelUX panelShowNulls;

	private StiLabelUX labelShowZeros;

	private StiComboBox comboBoxShowZeros;

	private StiLabelUX labelShowNulls;

	private StiComboBox comboBoxShowNulls;

	private StiLabelUX labelChartType;

	private StiComboBox comboBoxRoundValues;

	private StiLabelUX labelRoundValues;

	private StiPanelUX panelViewStates;

	private StiLabelUX labelViewStates;

	private StiPanelBox panelBoxViewStates;

	private HintTextBox textBoxViewStates;

	private StiOperationButton buttonViewStates;

	private StiOperationButton buttonDeleteViewState;

	private StiLabelPanel labelValues;

	private StiOperationButton buttonEnterDataManually;

	private StiExcelGrid gridData;

	private StiLabelPanel labelGridData;

	private StiOperationButton buttonUseDataFields;

	internal StiChartSeriesType CurrentSeriesType
	{
		get
		{
			if (IsDataFieldsMode)
			{
				if (ChartValue == null)
				{
					return StiChartSeriesType.ClusteredColumn;
				}
				return ChartValue.SeriesType;
			}
			if (ChartElement.ManuallyEnteredChartMeter != null)
			{
				return ChartElement.ManuallyEnteredChartMeter.SeriesType;
			}
			return StiChartSeriesType.ClusteredColumn;
		}
		set
		{
			if (IsDataFieldsMode)
			{
				ChartValue.SeriesType = value;
			}
			else if (ChartElement.ManuallyEnteredChartMeter != null)
			{
				ChartElement.ManuallyEnteredChartMeter.SeriesType = value;
			}
		}
	}

	internal StiChartSeriesType? OldSeriesType { get; private set; }

	internal StiChartElement ChartElement => base.Element as StiChartElement;

	internal StiValueChartMeter ChartValue
	{
		get
		{
			if (IsDataFieldsMode)
			{
				return listBoxValues.SelectedMeterItem?.Meter as StiValueChartMeter;
			}
			return ChartElement.ManuallyEnteredChartMeter;
		}
	}

	internal StiEndValueChartMeter ChartEndValue => listBoxEndValues.SelectedMeterItem?.Meter as StiEndValueChartMeter;

	internal StiCloseValueChartMeter ChartCloseValue => listBoxCloseValues.SelectedMeterItem?.Meter as StiCloseValueChartMeter;

	internal StiLowValueChartMeter ChartLowValue => listBoxLowValues.SelectedMeterItem?.Meter as StiLowValueChartMeter;

	internal StiHighValueChartMeter ChartHighValue => listBoxHighValues.SelectedMeterItem?.Meter as StiHighValueChartMeter;

	internal StiArgumentChartMeter ChartArgument => listBoxArguments.SelectedMeterItem?.Meter as StiArgumentChartMeter;

	internal StiWeightChartMeter ChartWeight => listBoxWeights.SelectedMeterItem?.Meter as StiWeightChartMeter;

	internal StiSeriesChartMeter ChartSeries => listBoxSeries.SelectedMeterItem?.Meter as StiSeriesChartMeter;

	private StiUserViewState CurrentViewState => ChartElement.UserViewStates.FirstOrDefault((StiUserViewState s) => s.Key == ChartElement.SelectedViewStateKey);

	internal bool IsBubble
	{
		get
		{
			if (IsDataFieldsMode)
			{
				return ChartElement.Values.Any((StiValueChartMeter v) => v != null && v.SeriesType == StiChartSeriesType.Bubble);
			}
			return ChartValue.SeriesType == StiChartSeriesType.Bubble;
		}
	}

	internal bool IsUsedNullableIcon
	{
		get
		{
			if (IsDataFieldsMode)
			{
				return ChartElement.Values.Any((StiValueChartMeter v) => v != null && GetIsUsedNUllableIcon(v.SeriesType));
			}
			return GetIsUsedNUllableIcon(ChartValue.SeriesType);
		}
	}

	internal bool IsPictorial
	{
		get
		{
			if (IsDataFieldsMode)
			{
				return ChartElement.Values.Any((StiValueChartMeter v) => v != null && StiChartTypes.IsPictorial(v.SeriesType));
			}
			return StiChartTypes.IsPictorial(ChartValue.SeriesType);
		}
	}

	internal bool IsSunburst => ChartElement.Values.Any((StiValueChartMeter v) => v != null && StiChartTypes.IsSunburst(v.SeriesType));

	internal bool IsRange
	{
		get
		{
			if (IsDataFieldsMode)
			{
				return ChartElement.Values.Any((StiValueChartMeter v) => v != null && StiChartTypes.IsRange(v.SeriesType));
			}
			return StiChartTypes.IsRange(ChartValue.SeriesType);
		}
	}

	internal bool IsGantt => ChartElement.Values.Any((StiValueChartMeter v) => v != null && v.SeriesType == StiChartSeriesType.Gantt);

	internal bool IsFinancial
	{
		get
		{
			if (IsDataFieldsMode)
			{
				return ChartElement.Values.Any((StiValueChartMeter v) => v != null && StiChartTypes.IsFinancial(v.SeriesType));
			}
			return StiChartTypes.IsFinancial(ChartValue.SeriesType);
		}
	}

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

	internal bool IsSeriesPresent => ((ListBox)listBoxSeries).Items.Count > 0;

	public override bool IsAnyPresent
	{
		get
		{
			if (!IsValuesPresent && !IsEndValuesPresent && !IsCloseValuesPresent && !IsLowValuesPresent && !IsHighValuesPresent && !IsArgumentsPresent && !IsWeightsPresent && !IsSeriesPresent && !IsShowMore)
			{
				return IsDataEnterMode;
			}
			return true;
		}
	}

	public override bool AllowResize => false;

	internal Stimulsoft.Dashboard.Design.Controls.StiMeterListBox ValuesListBox => listBoxValues;

	internal Stimulsoft.Dashboard.Design.Controls.StiMeterListBox EndValuesListBox => listBoxEndValues;

	internal Stimulsoft.Dashboard.Design.Controls.StiMeterListBox CloseValuesListBox => listBoxCloseValues;

	internal Stimulsoft.Dashboard.Design.Controls.StiMeterListBox LowValuesListBox => listBoxLowValues;

	internal Stimulsoft.Dashboard.Design.Controls.StiMeterListBox HighValuesListBox => listBoxHighValues;

	internal Stimulsoft.Dashboard.Design.Controls.StiMeterListBox ArgumentsListBox => listBoxArguments;

	internal Stimulsoft.Dashboard.Design.Controls.StiMeterListBox WeightsListBox => listBoxWeights;

	internal Stimulsoft.Dashboard.Design.Controls.StiMeterListBox SeriesListBox => listBoxSeries;

	private string LocLeftYAxis => Loc.GetEnum("StiSeriesYAxisLeftYAxis");

	private string LocRightYAxis => Loc.GetEnum("StiSeriesYAxisRightYAxis");

	private string LocEmptyCellsAsGap => Loc.GetEnum("StiEmptyCellsAsGap");

	private string LocEmptyCellsAsZero => Loc.GetEnum("StiEmptyCellsAsZero");

	private string LocEmptyCellsAsConnectPointsWithLine => Loc.GetEnum("StiEmptyCellsAsConnectPointsWithLine");

	private string LocEmptyCellsAsTrue => Loc.GetEnum("boolTrue");

	private string LocEmptyCellsAsFalse => Loc.GetEnum("boolFalse");

	private string LocValues
	{
		get
		{
			if (IsBubble)
			{
				return "Y";
			}
			if (IsFinancial)
			{
				return Loc.GetMain("OpenValues");
			}
			return Loc.GetMain("Values");
		}
	}

	private string LocArguments
	{
		get
		{
			if (!IsBubble)
			{
				return Loc.GetMain("Arguments");
			}
			return "X";
		}
	}

	private bool ShowEmptyValuesInSimpleWay
	{
		get
		{
			if (ChartValue != null)
			{
				return StiEmptyValuesHelper.IsSimple(CurrentSeriesType);
			}
			return true;
		}
	}

	public StiEmptyCellsAs? ShowZeros
	{
		get
		{
			if (ChartValue != null)
			{
				return ChartValue.ShowZeros;
			}
			return null;
		}
		set
		{
			if (ChartValue != null && value.HasValue)
			{
				ChartValue.ShowZeros = value.GetValueOrDefault();
			}
		}
	}

	public StiEmptyCellsAs? ShowNulls
	{
		get
		{
			if (ChartValue != null)
			{
				return ChartValue.ShowNulls;
			}
			return null;
		}
		set
		{
			if (ChartValue != null && value.HasValue)
			{
				ChartValue.ShowNulls = value.GetValueOrDefault();
			}
		}
	}

	public StiPenStyle? LineStyle
	{
		get
		{
			if (ChartElement.IsLinesChart && ChartValue != null)
			{
				return ChartValue.LineStyle;
			}
			return null;
		}
		set
		{
			if (ChartElement.IsLinesChart && ChartValue != null && value.HasValue)
			{
				ChartValue.LineStyle = value.GetValueOrDefault();
			}
		}
	}

	public float? LineWidth
	{
		get
		{
			if (ChartElement.IsLinesChart && ChartValue != null)
			{
				return ChartValue.LineWidth;
			}
			return null;
		}
		set
		{
			if (ChartElement.IsLinesChart && ChartValue != null && value.HasValue)
			{
				ChartValue.LineWidth = value.GetValueOrDefault();
			}
		}
	}

	private bool IsDataEnterMode => ChartElement.DataMode == StiDataMode.ManuallyEnteringData;

	private bool IsDataFieldsMode => ChartElement.DataMode == StiDataMode.UsingDataFields;

	private bool IsShowMore { get; set; }

	public StiChartElementEditorForm()
		: this(null)
	{
	}

	public StiChartElementEditorForm(StiElement element)
		: base(element)
	{
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Expected O, but got Unknown
		InitializeComponent();
		if (StiOptions.Appearance.ShowFormsHelp)
		{
			((Form)this).HelpButton = true;
		}
		selectedListBox = listBoxValues;
		if (!((Control)this).MinimumSize.IsEmpty)
		{
			((Form)this).Size = ((Control)this).MinimumSize;
		}
		foreach (Stimulsoft.Dashboard.Design.Controls.StiMeterListBox item in (from Control c in (IEnumerable)((Control)this).Controls
			where c is Stimulsoft.Dashboard.Design.Controls.StiMeterListBox
			select c).Cast<Stimulsoft.Dashboard.Design.Controls.StiMeterListBox>())
		{
			item.Designer = Designer;
			item.Editor = this;
			item.AllowImage = false;
			contextMenuBar.SetContextMenuEx((Control)(object)item, menuExpression);
			((ListBox)item).SelectedIndexChanged += ListBox_SelectedIndexChangedAsync;
		}
		listBoxValues.AllowImage = true;
		listBoxSeries.AllowMultipleElements = false;
		ReadPropertiesFromElement();
		if (ChartElement.Values.FirstOrDefault() != null)
		{
			OldSeriesType = ChartElement.Values.FirstOrDefault().SeriesType;
		}
		InitSeriesTypes();
		InitSeriesIcons();
		InitSeriesEvents();
		LocalizeInternal();
		InitializeMenus();
		DoScaling();
		ActivateDictionaryPanel();
		CreateIconControls();
		((DataGridView)gridData).BorderStyle = (BorderStyle)1;
		((DataGridView)gridData).CellValueChanged += new DataGridViewCellEventHandler(GridData_CellValueChanged);
		gridData.CellRowsChanged += GridData_CellRowsChanged;
		((Control)gridData).Height = StiScale.I(300);
		((Form)this).Load += This_LoadAsync;
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
		((Control)listBoxSeries).Invalidate();
		InitButtonSeriesType();
		((Control)this).Invalidate();
	}

	private void LocalizeInternal()
	{
		((Control)this).Text = Loc.Get("Components", "StiChart");
		((Control)labelGridData).Text = Loc.GetMain("Data");
		((Control)labelSeries).Text = Loc.Get("PropertyMain", "Series");
		((Control)labelExpression).Text = Loc.Get("PropertyMain", "Expression");
		((Control)labelYAxis).Text = Loc.GetMain("YAxis");
		((Control)labelShowZeros).Text = Loc.GetMain("ShowZeros");
		((Control)labelShowNulls).Text = Loc.GetMain("ShowNulls");
		((Control)labelLineWidth).Text = Loc.GetMain("LineWidth");
		((Control)labelLineStyle).Text = Loc.GetMain("LineStyle");
		((Control)labelChartType).Text = Loc.Get("Chart", "ChartType");
		((Control)labelRoundValues).Text = Loc.Get("PropertyMain", "RoundValues");
		((Control)labelViewStates).Text = Loc.Get("QueryBuilder", "Views");
		((Control)labelIcon).Text = Loc.GetMain("Icon");
		toolTip.SetToolTip((Control)(object)buttonUseDataFields, Loc.Get("Report", "UseDataFields"));
		toolTip.SetToolTip((Control)(object)buttonEnterDataManually, Loc.Get("Report", "EnterDataManually"));
		toolTip.SetToolTip((Control)(object)labelShowZeros, Loc.GetMain("ShowZeros"));
		toolTip.SetToolTip((Control)(object)labelShowNulls, Loc.GetMain("ShowNulls"));
		StiExcelGridLocalizer.Localize(gridData);
		LocalizeGridDataColumns();
		LocalizeYAxis();
		LocalizeShowZeros();
		LocalizeShowNulls();
		LocalizeLineStyle();
		LocalizeChartTypes();
		LocalizeRoundValues();
		Size size = TextRenderer.MeasureText(Loc.Get("Buttons", "LessOptions"), ((Control)buttonShowMore).Font);
		Size size2 = TextRenderer.MeasureText(Loc.Get("Buttons", "MoreOptions"), ((Control)buttonShowMore).Font);
		((Control)buttonShowMore).Width = Math.Max(size.Width, size2.Width) + StiScale.I8;
		((Control)buttonShowMore).Left = ((Control)panelMoreOptions).Width - ((Control)buttonShowMore).Width;
	}

	private void LocalizeYAxis()
	{
		((ComboBox)comboBoxYAxis).BeginUpdate();
		int selectedIndex = ((ListControl)comboBoxYAxis).SelectedIndex;
		((ComboBox)comboBoxYAxis).Items.Clear();
		((ComboBox)comboBoxYAxis).Items.Add((object)LocLeftYAxis);
		((ComboBox)comboBoxYAxis).Items.Add((object)LocRightYAxis);
		((ListControl)comboBoxYAxis).SelectedIndex = selectedIndex;
		((ComboBox)comboBoxYAxis).EndUpdate();
	}

	private void LocalizeShowZeros()
	{
		((ComboBox)comboBoxShowZeros).BeginUpdate();
		int selectedIndex = ((ListControl)comboBoxShowZeros).SelectedIndex;
		if (ShowEmptyValuesInSimpleWay)
		{
			if (((IEnumerable)((ComboBox)comboBoxShowZeros).Items).Cast<string>().FirstOrDefault() != LocEmptyCellsAsTrue)
			{
				((ComboBox)comboBoxShowZeros).Items.Clear();
				((ComboBox)comboBoxShowZeros).Items.Add((object)LocEmptyCellsAsTrue);
				((ComboBox)comboBoxShowZeros).Items.Add((object)LocEmptyCellsAsFalse);
			}
		}
		else if (((IEnumerable)((ComboBox)comboBoxShowZeros).Items).Cast<string>().FirstOrDefault() != LocEmptyCellsAsZero)
		{
			((ComboBox)comboBoxShowZeros).Items.Clear();
			((ComboBox)comboBoxShowZeros).Items.Add((object)LocEmptyCellsAsZero);
			((ComboBox)comboBoxShowZeros).Items.Add((object)LocEmptyCellsAsGap);
			((ComboBox)comboBoxShowZeros).Items.Add((object)LocEmptyCellsAsConnectPointsWithLine);
		}
		if (selectedIndex < ((ComboBox)comboBoxShowZeros).Items.Count)
		{
			((ListControl)comboBoxShowZeros).SelectedIndex = selectedIndex;
		}
		((ComboBox)comboBoxShowZeros).EndUpdate();
	}

	private void LocalizeShowNulls()
	{
		((ComboBox)comboBoxShowNulls).BeginUpdate();
		int selectedIndex = ((ListControl)comboBoxShowNulls).SelectedIndex;
		if (ShowEmptyValuesInSimpleWay)
		{
			if (((IEnumerable)((ComboBox)comboBoxShowNulls).Items).Cast<string>().FirstOrDefault() != LocEmptyCellsAsTrue)
			{
				((ComboBox)comboBoxShowNulls).Items.Clear();
				((ComboBox)comboBoxShowNulls).Items.Add((object)LocEmptyCellsAsTrue);
				((ComboBox)comboBoxShowNulls).Items.Add((object)LocEmptyCellsAsFalse);
			}
		}
		else if (((IEnumerable)((ComboBox)comboBoxShowNulls).Items).Cast<string>().FirstOrDefault() != LocEmptyCellsAsZero)
		{
			((ComboBox)comboBoxShowNulls).Items.Clear();
			((ComboBox)comboBoxShowNulls).Items.Add((object)LocEmptyCellsAsZero);
			((ComboBox)comboBoxShowNulls).Items.Add((object)LocEmptyCellsAsGap);
			((ComboBox)comboBoxShowNulls).Items.Add((object)LocEmptyCellsAsConnectPointsWithLine);
		}
		if (selectedIndex < ((ComboBox)comboBoxShowNulls).Items.Count)
		{
			((ListControl)comboBoxShowNulls).SelectedIndex = selectedIndex;
		}
		((ComboBox)comboBoxShowNulls).EndUpdate();
	}

	private void LocalizeRoundValues()
	{
		((ComboBox)comboBoxRoundValues).BeginUpdate();
		int selectedIndex = ((ListControl)comboBoxRoundValues).SelectedIndex;
		((ComboBox)comboBoxRoundValues).Items.Clear();
		((ComboBox)comboBoxRoundValues).Items.Add((object)LocEmptyCellsAsTrue);
		((ComboBox)comboBoxRoundValues).Items.Add((object)LocEmptyCellsAsFalse);
		((ListControl)comboBoxRoundValues).SelectedIndex = selectedIndex;
		((ComboBox)comboBoxRoundValues).EndUpdate();
	}

	private void LocalizeGridDataColumns()
	{
		DataGridViewColumn val = ((DataGridView)gridData).Columns["Value"];
		if (val != null)
		{
			val.HeaderText = LocValues;
		}
		val = ((DataGridView)gridData).Columns["EndValue"];
		if (val != null)
		{
			val.HeaderText = Loc.GetMain("EndValues");
			((DataGridViewBand)val).Visible = IsRange;
		}
		val = ((DataGridView)gridData).Columns["CloseValue"];
		if (val != null)
		{
			val.HeaderText = Loc.GetMain("CloseValues");
			((DataGridViewBand)val).Visible = IsFinancial;
		}
		val = ((DataGridView)gridData).Columns["LowValue"];
		if (val != null)
		{
			val.HeaderText = Loc.GetMain("LowValues");
			((DataGridViewBand)val).Visible = IsFinancial;
		}
		val = ((DataGridView)gridData).Columns["HighValue"];
		if (val != null)
		{
			val.HeaderText = Loc.GetMain("HighValues");
			((DataGridViewBand)val).Visible = IsFinancial;
		}
		val = ((DataGridView)gridData).Columns["Argument"];
		if (val != null)
		{
			val.HeaderText = LocArguments;
			((DataGridViewBand)val).DefaultCellStyle.Alignment = (DataGridViewContentAlignment)(IsBubble ? 64 : 16);
		}
		val = ((DataGridView)gridData).Columns["Weight"];
		if (val != null)
		{
			val.HeaderText = Loc.GetMain("Weights");
			((DataGridViewBand)val).Visible = IsBubble;
		}
		val = ((DataGridView)gridData).Columns["Series"];
		if (val != null)
		{
			val.HeaderText = Loc.GetMain("Series");
		}
	}

	private void LocalizeLineStyle()
	{
		buttonSolid.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleSolid");
		buttonDash.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleDash");
		buttonDashDot.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleDashDot");
		buttonDashDotDot.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleDashDotDot");
		buttonDot.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleDot");
		buttonNone.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleNone");
	}

	private void LocalizeChartTypes()
	{
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
	}

	private void LocalizeMetricLabels()
	{
		((Control)labelValues).Text = LocValues;
		((Control)labelArguments).Text = LocArguments;
		((Control)labelWeights).Text = Loc.Get("PropertyMain", "Weights");
		((Control)labelEndValues).Text = Loc.Get("PropertyMain", "EndValues");
		((Control)labelCloseValues).Text = Loc.Get("PropertyMain", "CloseValues");
		((Control)labelLowValues).Text = Loc.Get("PropertyMain", "LowValues");
		((Control)labelHighValues).Text = Loc.Get("PropertyMain", "HighValues");
	}

	private void DoScaling()
	{
		((ButtonBase)buttonEnterDataManually).Image = StiReportDesignImages.Actions.EditTable();
		((ButtonBase)buttonUseDataFields).Image = StiReportDesignImages.Actions.RetrieveColumnsDown();
		((ButtonBase)buttonExpressionEdit).Image = StiReportDesignImages.Actions.Edit();
		StiComboBox stiComboBox = comboBoxYAxis;
		StiComboBox stiComboBox2 = comboBoxShowZeros;
		StiComboBox stiComboBox3 = comboBoxShowNulls;
		StiComboBox stiComboBox4 = comboBoxRoundValues;
		int num = (((Control)buttonLineStyle).Width = ((Control)buttonSeriesType).Width);
		int num3 = (((Control)stiComboBox4).Width = num);
		int num5 = (((Control)stiComboBox3).Width = num3);
		int width2 = (((Control)stiComboBox2).Width = num5);
		((Control)stiComboBox).Width = width2;
		((Control)buttonShowMore).Left = ((Control)panelBoxExpression).Right - ((Control)buttonShowMore).Width + 1;
		StiButtonsLocator.ArrangeButtons((Control)(object)panelBoxExpression);
		StiButtonsLocator.ArrangeButtons((Control)(object)panelBoxViewStates);
		StiButtonsLocator.ArrangeButtons((Control)(object)panelBoxIcon);
		((Control)panelBoxViewStates).Width = ((Control)buttonSeriesType).Width;
		((ButtonBase)buttonDeleteViewState).Image = StiReportDesignImages.Actions.Remove();
		buttonSeriesType.DropArrowShift = -4;
		((Control)buttonSeriesType).Width = ((Control)buttonShowMore).Right - ((Control)buttonSeriesType).Left - 1;
		buttonSolid.Image = StiImageResizer.GetImage(imageListPenStyle.Images["Solid.png"], allowDispose: false);
		buttonDash.Image = StiImageResizer.GetImage(imageListPenStyle.Images["Dash.png"], allowDispose: false);
		buttonDashDot.Image = StiImageResizer.GetImage(imageListPenStyle.Images["DashDot.png"], allowDispose: false);
		buttonDashDotDot.Image = StiImageResizer.GetImage(imageListPenStyle.Images["DashDotDot.png"], allowDispose: false);
		buttonDot.Image = StiImageResizer.GetImage(imageListPenStyle.Images["Dot.png"], allowDispose: false);
		buttonNone.Image = StiImageResizer.GetImage(imageListPenStyle.Images["None.png"], allowDispose: false);
	}

	private void InitializeMenus()
	{
		menuExpression.PopupOpen += delegate
		{
			new StiChartExpressionMenu(menuExpression, (TextBox)(object)textBoxExpression, this, selectedListBox).CreateItems();
		};
		menuViewStates.PopupOpen += delegate
		{
			new StiChartViewStatesMenu(menuViewStates, (TextBox)(object)textBoxViewStates, this, selectedListBox).CreateItems();
		};
	}

	private void CreateIconControls()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		iconsPopup = new StiPopup((Control)(object)GetIconPanel());
		((Control)panelBoxIcon).Paint += new PaintEventHandler(PanelBoxIcon_Paint);
	}

	private void RefreshButtonBorderStyle()
	{
		if (LineStyle.HasValue)
		{
			Image image = null;
			string text = string.Empty;
			switch (LineStyle)
			{
			case StiPenStyle.Dash:
				image = imageListPenStyle.Images["Dash.png"];
				text = StiLocalization.Get("PropertyEnum", "StiPenStyleDash");
				break;
			case StiPenStyle.DashDot:
				image = imageListPenStyle.Images["DashDot.png"];
				text = StiLocalization.Get("PropertyEnum", "StiPenStyleDashDot");
				break;
			case StiPenStyle.DashDotDot:
				image = imageListPenStyle.Images["DashDotDot.png"];
				text = StiLocalization.Get("PropertyEnum", "StiPenStyleDashDotDot");
				break;
			case StiPenStyle.Dot:
				image = imageListPenStyle.Images["Dot.png"];
				text = StiLocalization.Get("PropertyEnum", "StiPenStyleDot");
				break;
			case StiPenStyle.None:
				image = imageListPenStyle.Images["None.png"];
				text = StiLocalization.Get("PropertyEnum", "StiPenStyleNone");
				break;
			case StiPenStyle.Solid:
				image = imageListPenStyle.Images["Solid.png"];
				text = StiLocalization.Get("PropertyEnum", "StiPenStyleSolid");
				break;
			case StiPenStyle.Double:
				image = imageListPenStyle.Images["Double.png"];
				text = StiLocalization.Get("PropertyEnum", "StiPenStyleDouble");
				break;
			}
			Size size = TextRenderer.MeasureText(text, ((Control)buttonLineStyle).Font);
			if (size.Width > StiScale.I(100))
			{
				float num = (float)size.Width / (float)text.Length;
				int length = (int)((float)StiScale.I(100) / num);
				((Control)buttonLineStyle).Text = text.Substring(0, length) + "...";
			}
			else
			{
				((Control)buttonLineStyle).Text = text;
			}
			buttonLineStyle.Image = StiImageResizer.GetImage(image);
		}
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
				if (ChartElement.Icon.HasValue)
				{
					val.DrawString(StiFontIconsHelper.GetContent(ChartElement.Icon), val2, (Brush)(object)val3, (RectangleF)new Rectangle(StiScale.I2, StiScale.I4, ((Control)panelBoxIcon).Width, ((Control)panelBoxIcon).Height));
				}
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
		StiFontIconSelectionControl stiFontIconSelectionControl = new StiFontIconSelectionControl(ChartElement.Icon, Color.FromArgb(68, 114, 196), showCustomIcon: false, !IsPictorial);
		((Control)stiFontIconSelectionControl).Location = new Point(StiScale.I2, StiScale.I2);
		((Control)stiFontIconSelectionControl).Width = ((Control)panelIcons).Width;
		((Control)stiFontIconSelectionControl).Anchor = (AnchorStyles)13;
		stiFontIconSelectionControl.TextFilter = filterName;
		StiFontIconSelectionControl stiFontIconSelectionControl2 = stiFontIconSelectionControl;
		stiFontIconSelectionControl2.ValueChanged += FontIconSelectionControl_ValueChanged;
		((Control)panelIcons).Controls.Add((Control)(object)stiFontIconSelectionControl2);
	}

	private static bool GetIsUsedNUllableIcon(StiChartSeriesType type)
	{
		if (type != StiChartSeriesType.Pie && type != StiChartSeriesType.ClusteredColumn && type != StiChartSeriesType.StackedColumn && type != StiChartSeriesType.FullStackedColumn && type != StiChartSeriesType.ClusteredBar && type != StiChartSeriesType.StackedBar && type != StiChartSeriesType.FullStackedBar && type != StiChartSeriesType.Bubble && type != StiChartSeriesType.Histogram && type != StiChartSeriesType.Pareto && type != StiChartSeriesType.Doughnut && type != StiChartSeriesType.Funnel && type != StiChartSeriesType.FunnelWeightedSlices && type != StiChartSeriesType.Gantt && type != StiChartSeriesType.RangeBar && type != StiChartSeriesType.Treemap)
		{
			return type == StiChartSeriesType.Waterfall;
		}
		return true;
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

	public override void ReadPropertiesFromElement()
	{
		ReadValuesPropertiesFromElement();
		ReadEndValuesPropertiesFromElement();
		ReadCloseValuesPropertiesFromElement();
		ReadLowValuesPropertiesFromElement();
		ReadHighValuesPropertiesFromElement();
		ReadArgumentsPropertiesFromElement();
		ReadWeightsPropertiesFromElement();
		ReadSeriesPropertiesFromElement();
	}

	private void ReadValuesPropertiesFromElement()
	{
		int selectedIndex = ((ListControl)listBoxValues).SelectedIndex;
		((ListBox)listBoxValues).BeginUpdate();
		((ListBox)listBoxValues).Items.Clear();
		for (int i = 0; i < ChartElement.Values.Count; i++)
		{
			listBoxValues.AddItem(ChartElement.Values[i], GetNodeImageKey(ChartElement.Values[i].SeriesType));
		}
		if (selectedIndex < 0)
		{
			listBoxValues.SelectFirstItem();
		}
		else
		{
			IEnumerable<Stimulsoft.Dashboard.Design.Controls.StiMeterItem> source = ((IEnumerable)((ListBox)listBoxValues).Items).Cast<Stimulsoft.Dashboard.Design.Controls.StiMeterItem>();
			if (source.Count() > selectedIndex)
			{
				listBoxValues.SelectedMeterItem = source.ToList()[selectedIndex];
			}
		}
		((ListBox)listBoxValues).EndUpdate();
	}

	private void ReadEndValuesPropertiesFromElement()
	{
		if (!listBoxEndValues.ItemsEqualTo(ChartElement.EndValues))
		{
			((ListBox)listBoxEndValues).BeginUpdate();
			((ListBox)listBoxEndValues).Items.Clear();
			listBoxEndValues.AddItems(ChartElement.EndValues);
			listBoxEndValues.SelectFirstItem();
			((ListBox)listBoxEndValues).EndUpdate();
		}
	}

	private void ReadCloseValuesPropertiesFromElement()
	{
		if (!listBoxCloseValues.ItemsEqualTo(ChartElement.CloseValues))
		{
			((ListBox)listBoxCloseValues).BeginUpdate();
			((ListBox)listBoxCloseValues).Items.Clear();
			listBoxCloseValues.AddItems(ChartElement.CloseValues);
			listBoxCloseValues.SelectFirstItem();
			((ListBox)listBoxCloseValues).EndUpdate();
		}
	}

	private void ReadLowValuesPropertiesFromElement()
	{
		if (!listBoxLowValues.ItemsEqualTo(ChartElement.LowValues))
		{
			((ListBox)listBoxLowValues).BeginUpdate();
			((ListBox)listBoxLowValues).Items.Clear();
			listBoxLowValues.AddItems(ChartElement.LowValues);
			listBoxLowValues.SelectFirstItem();
			((ListBox)listBoxLowValues).EndUpdate();
		}
	}

	private void ReadHighValuesPropertiesFromElement()
	{
		if (!listBoxHighValues.ItemsEqualTo(ChartElement.HighValues))
		{
			((ListBox)listBoxHighValues).BeginUpdate();
			((ListBox)listBoxHighValues).Items.Clear();
			listBoxHighValues.AddItems(ChartElement.HighValues);
			listBoxHighValues.SelectFirstItem();
			((ListBox)listBoxHighValues).EndUpdate();
		}
	}

	private void ReadArgumentsPropertiesFromElement()
	{
		if (!listBoxArguments.ItemsEqualTo(ChartElement.Arguments))
		{
			((ListBox)listBoxArguments).BeginUpdate();
			((ListBox)listBoxArguments).Items.Clear();
			listBoxArguments.AddItems(ChartElement.Arguments, StiMeterItemImage.Dimension);
			listBoxArguments.SelectFirstItem();
			((ListBox)listBoxArguments).EndUpdate();
		}
	}

	private void ReadWeightsPropertiesFromElement()
	{
		if (!listBoxWeights.ItemsEqualTo(ChartElement.Weights))
		{
			((ListBox)listBoxWeights).BeginUpdate();
			((ListBox)listBoxWeights).Items.Clear();
			listBoxWeights.AddItems(ChartElement.Weights);
			listBoxWeights.SelectFirstItem();
			((ListBox)listBoxWeights).EndUpdate();
		}
	}

	private void ReadSeriesPropertiesFromElement()
	{
		if (!listBoxSeries.ItemsEqualTo(ChartElement.Series))
		{
			((ListBox)listBoxSeries).BeginUpdate();
			((ListBox)listBoxSeries).Items.Clear();
			listBoxSeries.AddItem(ChartElement.Series);
			listBoxSeries.SelectedMeterItem = listBoxSeries.FirstItem();
			((ListBox)listBoxSeries).EndUpdate();
		}
	}

	public override void WritePropertiesToElement()
	{
		WriteValuesPropertiesToElement();
		WriteEndValuesPropertiesToElement();
		WriteCloseValuesPropertiesToElement();
		WriteLowValuesPropertiesToElement();
		WriteHighValuesPropertiesToElement();
		WriteArgumentsPropertiesToElement();
		WriteWeightsPropertiesToElement();
		WriteSeriesPropertiesToElement();
		RefreshElementView();
	}

	private void WriteValuesPropertiesToElement()
	{
		ChartElement.Values = (from t in listBoxValues.ToItems()
			select t.Meter).Cast<StiValueChartMeter>().ToList();
	}

	private void WriteEndValuesPropertiesToElement()
	{
		ChartElement.EndValues = (from t in listBoxEndValues.ToItems()
			select t.Meter).Cast<StiEndValueChartMeter>().ToList();
	}

	private void WriteCloseValuesPropertiesToElement()
	{
		ChartElement.CloseValues = (from t in listBoxCloseValues.ToItems()
			select t.Meter).Cast<StiCloseValueChartMeter>().ToList();
	}

	private void WriteLowValuesPropertiesToElement()
	{
		ChartElement.LowValues = (from t in listBoxLowValues.ToItems()
			select t.Meter).Cast<StiLowValueChartMeter>().ToList();
	}

	private void WriteHighValuesPropertiesToElement()
	{
		ChartElement.HighValues = (from t in listBoxHighValues.ToItems()
			select t.Meter).Cast<StiHighValueChartMeter>().ToList();
	}

	private void WriteArgumentsPropertiesToElement()
	{
		ChartElement.Arguments = (from t in listBoxArguments.ToItems()
			select t.Meter).Cast<StiArgumentChartMeter>().ToList();
	}

	private void WriteWeightsPropertiesToElement()
	{
		if (IsBubble)
		{
			ChartElement.Weights = (from t in listBoxWeights.ToItems()
				select t.Meter).Cast<StiWeightChartMeter>().ToList();
		}
		else
		{
			ChartElement.Weights = new List<StiWeightChartMeter>();
		}
	}

	private void WriteSeriesPropertiesToElement()
	{
		ChartElement.Series = listBoxSeries.FirstItem()?.Meter as StiSeriesChartMeter;
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
		AttachEventsToSeries();
	}

	private void AttachEventsToValues()
	{
		listBoxValues.ShowMoreLabelClicked += delegate
		{
			IsShowMore = true;
			RefreshUiPanel();
		};
		listBoxValues.EnterDataManuallyLabelClicked += delegate
		{
			ButtonEnterDataManually_Click(null, null);
		};
		listBoxValues.ElementsConverting += ListBox_ElementsConverting;
		listBoxValues.ElementsChanged += ListBox_ElementsChanged;
		listBoxValues.ElementsSelectedChanged += ListBox_ElementsAfterSelect;
		listBoxValues.ElementsDragEnter += ListBox_ElementsDragEnter;
		listBoxValues.ElementsDragDrop += delegate
		{
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(Designer))
			{
				IStiAppDataCell draggedDataColumnOrVariable = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
				if (draggedDataColumnOrVariable == null)
				{
					return;
				}
				StiValueChartMeter stiValueChartMeter = null;
				if (!IsGantt)
				{
					stiValueChartMeter = ((!IsBubble) ? StiMeterHelper.Chart.GetValue(draggedDataColumnOrVariable, ChartElement) : StiMeterHelper.Chart.GetY(draggedDataColumnOrVariable));
				}
				else
				{
					stiValueChartMeter = StiMeterHelper.Chart.GetGanttStartValue(draggedDataColumnOrVariable);
					stiValueChartMeter.SeriesType = StiChartSeriesType.Gantt;
				}
				stiValueChartMeter.SeriesType = ((ChartElement.Values.Count == 0 && OldSeriesType.HasValue) ? OldSeriesType.Value : stiValueChartMeter.SeriesType);
				if (stiValueChartMeter.SeriesType == StiChartSeriesType.Bubble)
				{
					stiValueChartMeter = StiMeterHelper.Chart.GetY(draggedDataColumnOrVariable);
					stiValueChartMeter.SeriesType = StiChartSeriesType.Bubble;
				}
				else if (stiValueChartMeter.SeriesType == StiChartSeriesType.Gantt)
				{
					stiValueChartMeter = StiMeterHelper.Chart.GetGanttStartValue(draggedDataColumnOrVariable);
					stiValueChartMeter.SeriesType = StiChartSeriesType.Gantt;
				}
				Stimulsoft.Dashboard.Design.Controls.StiMeterItem stiMeterItem = new Stimulsoft.Dashboard.Design.Controls.StiMeterItem(StiLabelHelper.GetLabel(stiValueChartMeter), stiValueChartMeter, GetNodeImageKey(stiValueChartMeter.SeriesType));
				Stimulsoft.Dashboard.Design.Controls.StiMeterItem itemAtMouse = listBoxValues.GetItemAtMouse();
				if (itemAtMouse != null)
				{
					int index = ((ListBox)listBoxValues).Items.IndexOf((object)itemAtMouse);
					listBoxValues.InsertItem(index, stiMeterItem);
				}
				else
				{
					listBoxValues.AddItem(stiMeterItem);
				}
				if (CurrentViewState != null)
				{
					CurrentViewState.SeriesType = stiValueChartMeter.SeriesType;
				}
				Designer.PageView.ResetDragging();
				ChartElement.Select();
				WritePropertiesToElement();
				listBoxValues.SelectedMeterItem = stiMeterItem;
				((Control)listBoxValues).Focus();
			}
			else if (StiDragDropColumnHelper.IsTableElementDragged(Designer))
			{
				if (!(StiDragDropColumnHelper.GetDraggedTableElement(Designer) is StiTableElement stiTableElement))
				{
					return;
				}
				int count = ((ListBox)listBoxValues).Items.Count;
				foreach (StiTableColumn column in stiTableElement.Columns)
				{
					ChartElement.Values.Add(IsBubble ? StiMeterHelper.Chart.GetY(column) : StiMeterHelper.Chart.GetValue(column));
				}
				if (CurrentViewState != null && ChartElement.Values.Count > 0)
				{
					CurrentViewState.SeriesType = ChartElement.Values[0].SeriesType;
				}
				RefreshElementView();
				ReadPropertiesFromElement();
				Designer.PageView.ResetDragging();
				((ListControl)listBoxValues).SelectedIndex = count;
			}
			ResetComponentHighlights();
			RefreshUiPanel();
		};
		listBoxValues.ElementsSelectedChanged += delegate
		{
			ChartElement.Select();
			RefreshElementView();
		};
	}

	private void AttachEventsToArguments()
	{
		listBoxArguments.ElementsConverting += ListBox_ElementsConverting;
		listBoxArguments.ElementsChanged += ListBox_ElementsChanged;
		listBoxArguments.ElementsSelectedChanged += ListBox_ElementsAfterSelect;
		listBoxArguments.ElementsDragEnter += ListBox_ElementsDragEnter;
		listBoxArguments.ElementsDragDrop += delegate
		{
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(Designer))
			{
				IStiAppDataCell draggedDataColumnOrVariable = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
				if (draggedDataColumnOrVariable == null)
				{
					return;
				}
				StiArgumentChartMeter meter = (IsBubble ? StiMeterHelper.Chart.GetX(draggedDataColumnOrVariable) : StiMeterHelper.Chart.GetArgument(draggedDataColumnOrVariable));
				Stimulsoft.Dashboard.Design.Controls.StiMeterItem stiMeterItem = new Stimulsoft.Dashboard.Design.Controls.StiMeterItem(StiLabelHelper.GetLabel(meter), meter, StiMeterItemImage.Dimension);
				Stimulsoft.Dashboard.Design.Controls.StiMeterItem itemAtMouse = listBoxArguments.GetItemAtMouse();
				if (itemAtMouse != null)
				{
					int index = ((ListBox)listBoxArguments).Items.IndexOf((object)itemAtMouse);
					listBoxArguments.InsertItem(index, stiMeterItem);
				}
				else
				{
					listBoxArguments.AddItem(stiMeterItem);
				}
				Designer.PageView.ResetDragging();
				ChartElement.Select();
				WritePropertiesToElement();
				listBoxArguments.SelectedMeterItem = stiMeterItem;
				((Control)listBoxArguments).Focus();
			}
			else if (StiDragDropColumnHelper.IsTableElementDragged(Designer))
			{
				if (!(StiDragDropColumnHelper.GetDraggedTableElement(Designer) is StiTableElement stiTableElement))
				{
					return;
				}
				int count = ((ListBox)listBoxArguments).Items.Count;
				foreach (StiTableColumn column in stiTableElement.Columns)
				{
					ChartElement.Arguments.Add(IsBubble ? StiMeterHelper.Chart.GetX(column) : StiMeterHelper.Chart.GetArgument(column));
				}
				RefreshElementView();
				ReadPropertiesFromElement();
				Designer.PageView.ResetDragging();
				((ListControl)listBoxArguments).SelectedIndex = count;
			}
			ResetComponentHighlights();
		};
		listBoxArguments.ElementsSelectedChanged += delegate
		{
			ChartElement.Select();
			RefreshElementView();
		};
	}

	private void AttachEventsToWeights()
	{
		listBoxWeights.ElementsConverting += ListBox_ElementsConverting;
		listBoxWeights.ElementsChanged += ListBox_ElementsChanged;
		listBoxWeights.ElementsSelectedChanged += ListBox_ElementsAfterSelect;
		listBoxWeights.ElementsDragEnter += ListBox_ElementsDragEnter;
		listBoxWeights.ElementsDragDrop += delegate
		{
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(Designer))
			{
				IStiAppDataCell draggedDataColumnOrVariable = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
				if (draggedDataColumnOrVariable == null)
				{
					return;
				}
				StiWeightChartMeter weight = StiMeterHelper.Chart.GetWeight(draggedDataColumnOrVariable);
				Stimulsoft.Dashboard.Design.Controls.StiMeterItem stiMeterItem = new Stimulsoft.Dashboard.Design.Controls.StiMeterItem(StiLabelHelper.GetLabel(weight), weight, StiMeterItemImage.Measure);
				Stimulsoft.Dashboard.Design.Controls.StiMeterItem itemAtMouse = listBoxWeights.GetItemAtMouse();
				if (itemAtMouse != null)
				{
					int index = ((ListBox)listBoxWeights).Items.IndexOf((object)itemAtMouse);
					listBoxWeights.InsertItem(index, stiMeterItem);
				}
				else
				{
					listBoxWeights.AddItem(stiMeterItem);
				}
				Designer.PageView.ResetDragging();
				ChartElement.Select();
				WritePropertiesToElement();
				listBoxWeights.SelectedMeterItem = stiMeterItem;
				((Control)listBoxWeights).Focus();
			}
			else if (StiDragDropColumnHelper.IsTableElementDragged(Designer))
			{
				if (!(StiDragDropColumnHelper.GetDraggedTableElement(Designer) is StiTableElement stiTableElement))
				{
					return;
				}
				int count = ((ListBox)listBoxWeights).Items.Count;
				foreach (StiTableColumn column in stiTableElement.Columns)
				{
					ChartElement.Weights.Add(StiMeterHelper.Chart.GetWeight(column));
				}
				RefreshElementView();
				ReadPropertiesFromElement();
				Designer.PageView.ResetDragging();
				((ListControl)listBoxWeights).SelectedIndex = count;
			}
			ResetComponentHighlights();
		};
		listBoxWeights.ElementsSelectedChanged += delegate
		{
			ChartElement.Select();
			RefreshElementView();
		};
	}

	private void AttachEventsToSeries()
	{
		listBoxSeries.ElementsConverting += ListBox_ElementsConverting;
		listBoxSeries.ElementsChanged += ListBox_ElementsChanged;
		listBoxSeries.ElementsSelectedChanged += ListBox_ElementsAfterSelect;
		listBoxSeries.ElementsDragEnter += ListBox_ElementsDragEnter;
		listBoxSeries.ElementsDragDrop += delegate
		{
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(Designer))
			{
				IStiAppDataCell draggedDataColumnOrVariable = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
				if (draggedDataColumnOrVariable == null)
				{
					return;
				}
				((ListBox)listBoxSeries).Items.Clear();
				listBoxSeries.AddItem(StiMeterHelper.Chart.GetSeries(draggedDataColumnOrVariable));
				Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxSeries.SelectedMeterItem = listBoxSeries.LastItem();
				((Control)listBoxSeries).Focus();
			}
			else if (StiDragDropColumnHelper.IsTableElementDragged(Designer))
			{
				if (!(StiDragDropColumnHelper.GetDraggedTableElement(Designer) is StiTableElement stiTableElement))
				{
					return;
				}
				StiTableColumn stiTableColumn = stiTableElement.Columns.FirstOrDefault();
				if (stiTableColumn == null)
				{
					return;
				}
				((ListBox)listBoxSeries).Items.Clear();
				listBoxSeries.AddItem(StiMeterHelper.Chart.GetSeries(stiTableColumn));
				Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxSeries.SelectedMeterItem = listBoxSeries.LastItem();
				((Control)listBoxSeries).Focus();
			}
			ResetComponentHighlights();
		};
		listBoxSeries.ElementsSelectedChanged += delegate
		{
			ChartElement.Select();
			RefreshElementView();
		};
	}

	private void AttachEventsToEndValues()
	{
		listBoxEndValues.ElementsConverting += ListBox_ElementsConverting;
		listBoxEndValues.ElementsChanged += ListBox_ElementsChanged;
		listBoxEndValues.ElementsSelectedChanged += ListBox_ElementsAfterSelect;
		listBoxEndValues.ElementsDragEnter += ListBox_ElementsDragEnter;
		listBoxEndValues.ElementsDragDrop += delegate
		{
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(Designer))
			{
				IStiAppDataCell draggedDataColumnOrVariable = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
				if (draggedDataColumnOrVariable == null)
				{
					return;
				}
				StiEndValueChartMeter meter = (IsGantt ? StiMeterHelper.Chart.GetGanttEndValue(draggedDataColumnOrVariable) : StiMeterHelper.Chart.GetEndValue(draggedDataColumnOrVariable));
				((ListBox)listBoxEndValues).Items.Clear();
				listBoxEndValues.AddItem(meter);
				Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxEndValues.SelectedMeterItem = listBoxEndValues.LastItem();
				((Control)listBoxEndValues).Focus();
			}
			else if (StiDragDropColumnHelper.IsTableElementDragged(Designer))
			{
				if (!(StiDragDropColumnHelper.GetDraggedTableElement(Designer) is StiTableElement stiTableElement))
				{
					return;
				}
				StiTableColumn stiTableColumn = stiTableElement.Columns.FirstOrDefault();
				if (stiTableColumn == null)
				{
					return;
				}
				((ListBox)listBoxEndValues).Items.Clear();
				listBoxEndValues.AddItem(StiMeterHelper.Chart.GetEndValue(stiTableColumn));
				Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxEndValues.SelectedMeterItem = listBoxEndValues.LastItem();
				((Control)listBoxEndValues).Focus();
			}
			ResetComponentHighlights();
		};
		listBoxEndValues.ElementsSelectedChanged += delegate
		{
			ChartElement.Select();
			RefreshElementView();
		};
	}

	private void AttachEventsToCloseValues()
	{
		listBoxCloseValues.ElementsConverting += ListBox_ElementsConverting;
		listBoxCloseValues.ElementsChanged += ListBox_ElementsChanged;
		listBoxCloseValues.ElementsSelectedChanged += ListBox_ElementsAfterSelect;
		listBoxCloseValues.ElementsDragEnter += ListBox_ElementsDragEnter;
		listBoxCloseValues.ElementsDragDrop += delegate
		{
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(Designer))
			{
				IStiAppDataCell draggedDataColumnOrVariable = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
				if (draggedDataColumnOrVariable == null)
				{
					return;
				}
				((ListBox)listBoxCloseValues).Items.Clear();
				listBoxCloseValues.AddItem(StiMeterHelper.Chart.GetCloseValue(draggedDataColumnOrVariable));
				Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxCloseValues.SelectedMeterItem = listBoxCloseValues.LastItem();
				((Control)listBoxCloseValues).Focus();
			}
			else if (StiDragDropColumnHelper.IsTableElementDragged(Designer))
			{
				if (!(StiDragDropColumnHelper.GetDraggedTableElement(Designer) is StiTableElement stiTableElement))
				{
					return;
				}
				StiTableColumn stiTableColumn = stiTableElement.Columns.FirstOrDefault();
				if (stiTableColumn == null)
				{
					return;
				}
				((ListBox)listBoxCloseValues).Items.Clear();
				listBoxCloseValues.AddItem(StiMeterHelper.Chart.GetCloseValue(stiTableColumn));
				Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxCloseValues.SelectedMeterItem = listBoxCloseValues.LastItem();
				((Control)listBoxCloseValues).Focus();
			}
			ResetComponentHighlights();
		};
		listBoxCloseValues.ElementsSelectedChanged += delegate
		{
			ChartElement.Select();
			RefreshElementView();
		};
	}

	private void AttachEventsToLowValues()
	{
		listBoxLowValues.ElementsConverting += ListBox_ElementsConverting;
		listBoxLowValues.ElementsChanged += ListBox_ElementsChanged;
		listBoxLowValues.ElementsSelectedChanged += ListBox_ElementsAfterSelect;
		listBoxLowValues.ElementsDragEnter += ListBox_ElementsDragEnter;
		listBoxLowValues.ElementsDragDrop += delegate
		{
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(Designer))
			{
				IStiAppDataCell draggedDataColumnOrVariable = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
				if (draggedDataColumnOrVariable == null)
				{
					return;
				}
				((ListBox)listBoxLowValues).Items.Clear();
				listBoxLowValues.AddItem(StiMeterHelper.Chart.GetLowValue(draggedDataColumnOrVariable));
				Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxLowValues.SelectedMeterItem = listBoxLowValues.LastItem();
				((Control)listBoxLowValues).Focus();
			}
			else if (StiDragDropColumnHelper.IsTableElementDragged(Designer))
			{
				if (!(StiDragDropColumnHelper.GetDraggedTableElement(Designer) is StiTableElement stiTableElement))
				{
					return;
				}
				StiTableColumn stiTableColumn = stiTableElement.Columns.FirstOrDefault();
				if (stiTableColumn == null)
				{
					return;
				}
				((ListBox)listBoxLowValues).Items.Clear();
				listBoxLowValues.AddItem(StiMeterHelper.Chart.GetLowValue(stiTableColumn));
				Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxLowValues.SelectedMeterItem = listBoxLowValues.LastItem();
				((Control)listBoxLowValues).Focus();
			}
			ResetComponentHighlights();
		};
		listBoxLowValues.ElementsSelectedChanged += delegate
		{
			ChartElement.Select();
			RefreshElementView();
		};
	}

	private void AttachEventsToHighValues()
	{
		listBoxHighValues.ElementsConverting += ListBox_ElementsConverting;
		listBoxHighValues.ElementsChanged += ListBox_ElementsChanged;
		listBoxHighValues.ElementsSelectedChanged += ListBox_ElementsAfterSelect;
		listBoxHighValues.ElementsDragEnter += ListBox_ElementsDragEnter;
		listBoxHighValues.ElementsDragDrop += delegate
		{
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(Designer))
			{
				IStiAppDataCell draggedDataColumnOrVariable = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
				if (draggedDataColumnOrVariable == null)
				{
					return;
				}
				((ListBox)listBoxHighValues).Items.Clear();
				listBoxHighValues.AddItem(StiMeterHelper.Chart.GetHighValue(draggedDataColumnOrVariable));
				Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxHighValues.SelectedMeterItem = listBoxHighValues.LastItem();
				((Control)listBoxHighValues).Focus();
			}
			else if (StiDragDropColumnHelper.IsTableElementDragged(Designer))
			{
				if (!(StiDragDropColumnHelper.GetDraggedTableElement(Designer) is StiTableElement stiTableElement))
				{
					return;
				}
				StiTableColumn stiTableColumn = stiTableElement.Columns.FirstOrDefault();
				if (stiTableColumn == null)
				{
					return;
				}
				((ListBox)listBoxHighValues).Items.Clear();
				listBoxHighValues.AddItem(StiMeterHelper.Chart.GetHighValue(stiTableColumn));
				Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxHighValues.SelectedMeterItem = listBoxHighValues.LastItem();
				((Control)listBoxHighValues).Focus();
			}
			ResetComponentHighlights();
		};
		listBoxHighValues.ElementsSelectedChanged += delegate
		{
			ChartElement.Select();
			RefreshElementView();
		};
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
			bool visible = (((Control)listBoxWeights).Visible = IsValuesPresent && IsBubble && IsDataFieldsMode);
			((Control)stiChartGroupLabel).Visible = visible;
			StiChartGroupLabel stiChartGroupLabel2 = labelEndValues;
			visible = (((Control)listBoxEndValues).Visible = IsValuesPresent && IsRange && IsDataFieldsMode);
			((Control)stiChartGroupLabel2).Visible = visible;
			StiChartGroupLabel stiChartGroupLabel3 = labelCloseValues;
			Stimulsoft.Dashboard.Design.Controls.StiMeterListBox stiMeterListBox = listBoxCloseValues;
			StiChartGroupLabel stiChartGroupLabel4 = labelLowValues;
			Stimulsoft.Dashboard.Design.Controls.StiMeterListBox stiMeterListBox2 = listBoxLowValues;
			StiChartGroupLabel stiChartGroupLabel5 = labelHighValues;
			bool flag4 = (((Control)listBoxHighValues).Visible = IsFinancial && IsDataFieldsMode);
			bool flag6 = (((Control)stiChartGroupLabel5).Visible = flag4);
			bool flag8 = (((Control)stiMeterListBox2).Visible = flag6);
			bool flag10 = (((Control)stiChartGroupLabel4).Visible = flag8);
			visible = (((Control)stiMeterListBox).Visible = flag10);
			((Control)stiChartGroupLabel3).Visible = visible;
			listBoxArguments.ToItems().ForEach(delegate(Stimulsoft.Dashboard.Design.Controls.StiMeterItem n)
			{
				n.Image = (IsBubble ? StiMeterItemImage.Measure : StiMeterItemImage.Dimension);
			});
			StiLabelPanel stiLabelPanel = labelValues;
			StiChartGroupLabel stiChartGroupLabel6 = labelArguments;
			StiChartGroupLabel stiChartGroupLabel7 = labelSeries;
			StiPanelUX stiPanelUX = panelOptions;
			Stimulsoft.Dashboard.Design.Controls.StiMeterListBox stiMeterListBox3 = listBoxArguments;
			flag4 = (((Control)listBoxSeries).Visible = IsAnyPresent && IsDataFieldsMode);
			flag6 = (((Control)stiMeterListBox3).Visible = flag4);
			flag8 = (((Control)stiPanelUX).Visible = flag6);
			flag10 = (((Control)stiChartGroupLabel7).Visible = flag8);
			visible = (((Control)stiChartGroupLabel6).Visible = flag10);
			((Control)stiLabelPanel).Visible = visible;
			((Control)panelSeriesType).Visible = (IsAnyPresent && IsDataFieldsMode) || IsDataEnterMode;
			StiChartGroupLabel stiChartGroupLabel8 = labelSeries;
			visible = (((Control)listBoxSeries).Visible = IsAnyPresent && IsDataFieldsMode && !IsSunburst);
			((Control)stiChartGroupLabel8).Visible = visible;
			StiPanelBox stiPanelBox = panelBoxExpression;
			visible = (((Control)textBoxExpression).Visible = (IsValuesPresent && listBoxValues.IsCurrentlySelected) || (IsEndValuesPresent && listBoxEndValues.IsCurrentlySelected) || (IsCloseValuesPresent && listBoxCloseValues.IsCurrentlySelected) || (IsLowValuesPresent && listBoxLowValues.IsCurrentlySelected) || (IsHighValuesPresent && listBoxHighValues.IsCurrentlySelected) || (IsArgumentsPresent && listBoxArguments.IsCurrentlySelected) || (IsSeriesPresent && listBoxSeries.IsCurrentlySelected) || (IsWeightsPresent && listBoxWeights.IsCurrentlySelected));
			((Control)stiPanelBox).Enabled = visible;
			bool flag19 = ((ListBox)ValuesListBox).Items.Count > 1 && ChartElement.YRightAxis.Visible && IsValuesPresent && IsAnyPresent && ChartElement.IsAxisAreaChart;
			bool flag20 = IsValuesPresent && IsAnyPresent && ChartValue != null && StiEmptyValuesHelper.IsShowZeros(CurrentSeriesType) && IsDataFieldsMode;
			bool flag21 = IsValuesPresent && IsAnyPresent && ChartValue != null && StiEmptyValuesHelper.IsShowNulls(CurrentSeriesType) && IsDataFieldsMode;
			bool flag22 = IsValuesPresent && IsAnyPresent && ChartElement.IsLinesChart;
			((Control)panelYAxis).Visible = flag19 && isShowMore;
			((Control)panelShowZeros).Visible = flag20 && isShowMore;
			((Control)panelShowNulls).Visible = flag21 && isShowMore;
			((Control)panelViewStates).Visible = isShowMore && (IsAnyPresent || ChartElement.UserViewStates.Count > 1) && IsDataFieldsMode;
			((Control)panelLineSettings).Visible = flag22 && isShowMore;
			((Control)panelMoreOptions).Visible = IsAnyPresent || IsDataEnterMode;
			((Control)buttonShowMore).Text = (isShowMore ? Loc.Get("Buttons", "LessOptions") : Loc.Get("Buttons", "MoreOptions"));
			((Control)panelIcon).Visible = (IsUsedNullableIcon && isShowMore) || IsPictorial;
			((Control)comboBoxRoundValues).Enabled = ChartElement.Icon.HasValue;
			Stimulsoft.Dashboard.Design.Controls.StiMeterListBox stiMeterListBox4 = listBoxValues;
			visible = (listBoxValues.ShowEnterDataManuallyLabel = !IsAnyPresent);
			stiMeterListBox4.ShowShowMoreLabel = visible;
			((Control)listBoxValues).Visible = IsDataFieldsMode;
			StiLabelPanel stiLabelPanel2 = labelGridData;
			StiExcelGrid stiExcelGrid = gridData;
			flag10 = (((Control)buttonUseDataFields).Visible = IsDataEnterMode);
			visible = (((Control)stiExcelGrid).Visible = flag10);
			((Control)stiLabelPanel2).Visible = visible;
			((Control)listBoxValues).Dock = (DockStyle)(IsAnyPresent ? 1 : 5);
			((Control)panelMoreOptions).Visible = flag21 || flag20 || flag22 || flag19 || IsUsedNullableIcon || IsPictorial;
			InitButtonSeriesType();
			((Control)buttonSeriesType).Enabled = ChartValue != null || IsDataEnterMode;
			((ComboBox)comboBoxYAxis).SelectedItem = ((ChartValue == null || ChartValue.YAxis == StiSeriesYAxis.LeftYAxis) ? LocLeftYAxis : LocRightYAxis);
			numericUpDownLineWidth.Value = (LineWidth.HasValue ? ((decimal)LineWidth.Value) : 2m);
			LocalizeShowNulls();
			LocalizeShowZeros();
			LocalizeRoundValues();
			LocalizeGridDataColumns();
			ReadShowZeros();
			ReadShowNulls();
			ReadRoundValues();
			bool flag25 = CurrentViewState != null && ChartElement.UserViewStates.Count > 1;
			((TextBoxBase)textBoxViewStates).ReadOnly = !flag25;
			((Control)textBoxViewStates).Text = (flag25 ? CurrentViewState.Name : ("(" + Loc.Get("FormStyleDesigner", "NotSpecified") + ")"));
			((Control)buttonDeleteViewState).Enabled = flag25;
			SetListHeightFromCount(listBoxValues, multiple: true);
			SetListHeightFromCount(listBoxEndValues, multiple: true);
			SetListHeightFromCount(listBoxCloseValues, multiple: true);
			SetListHeightFromCount(listBoxLowValues, multiple: true);
			SetListHeightFromCount(listBoxHighValues, multiple: true);
			SetListHeightFromCount(listBoxArguments, multiple: true);
			SetListHeightFromCount(listBoxWeights, multiple: true);
			SetListHeightFromCount(listBoxSeries, multiple: false);
			RefreshButtonBorderStyle();
			RefreshSampleIcon();
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

	private void ReadShowZeros()
	{
		if (ShowEmptyValuesInSimpleWay)
		{
			StiEmptyCellsAs? showZeros = ShowZeros;
			if (showZeros.HasValue)
			{
				StiEmptyCellsAs valueOrDefault = showZeros.GetValueOrDefault();
				if (valueOrDefault == StiEmptyCellsAs.Zero)
				{
					((ComboBox)comboBoxShowZeros).SelectedItem = LocEmptyCellsAsTrue;
					return;
				}
			}
			((ComboBox)comboBoxShowZeros).SelectedItem = LocEmptyCellsAsFalse;
			return;
		}
		StiEmptyCellsAs? showZeros2 = ShowZeros;
		if (showZeros2.HasValue)
		{
			switch (showZeros2.GetValueOrDefault())
			{
			case StiEmptyCellsAs.Zero:
				((ComboBox)comboBoxShowZeros).SelectedItem = LocEmptyCellsAsZero;
				break;
			case StiEmptyCellsAs.Gap:
				((ComboBox)comboBoxShowZeros).SelectedItem = LocEmptyCellsAsGap;
				break;
			case StiEmptyCellsAs.ConnectPointsWithLine:
				((ComboBox)comboBoxShowZeros).SelectedItem = LocEmptyCellsAsConnectPointsWithLine;
				break;
			}
		}
	}

	private void ReadShowNulls()
	{
		if (ShowEmptyValuesInSimpleWay)
		{
			StiEmptyCellsAs? showZeros = ShowZeros;
			if (showZeros.HasValue)
			{
				StiEmptyCellsAs valueOrDefault = showZeros.GetValueOrDefault();
				if (valueOrDefault == StiEmptyCellsAs.Zero)
				{
					((ComboBox)comboBoxShowNulls).SelectedItem = LocEmptyCellsAsTrue;
					return;
				}
			}
			((ComboBox)comboBoxShowNulls).SelectedItem = LocEmptyCellsAsFalse;
			return;
		}
		StiEmptyCellsAs? showNulls = ShowNulls;
		if (showNulls.HasValue)
		{
			switch (showNulls.GetValueOrDefault())
			{
			case StiEmptyCellsAs.Zero:
				((ComboBox)comboBoxShowNulls).SelectedItem = LocEmptyCellsAsZero;
				break;
			case StiEmptyCellsAs.Gap:
				((ComboBox)comboBoxShowNulls).SelectedItem = LocEmptyCellsAsGap;
				break;
			case StiEmptyCellsAs.ConnectPointsWithLine:
				((ComboBox)comboBoxShowNulls).SelectedItem = LocEmptyCellsAsConnectPointsWithLine;
				break;
			}
		}
	}

	private void ReadRoundValues()
	{
		if (ChartElement.RoundValues)
		{
			((ComboBox)comboBoxRoundValues).SelectedItem = LocEmptyCellsAsTrue;
		}
		else
		{
			((ComboBox)comboBoxRoundValues).SelectedItem = LocEmptyCellsAsFalse;
		}
	}

	protected override void CalculateWindowHeight()
	{
		if (IsAnyPresent)
		{
			base.CalculateWindowHeight();
		}
		else
		{
			((Form)this).ClientSize = new Size(((Form)this).ClientSize.Width, base.PaddingCorrection + ((Control)listBoxValues).Width);
		}
	}

	private void InitButtonSeriesType()
	{
		if (ChartValue != null || IsDataEnterMode)
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
			StiResourcesHelper.SetImage((ButtonX)buttonSeriesType, $"{pathImage}{CurrentSeriesType}{arg}.png", scale: false);
			if (CurrentSeriesType == StiChartSeriesType.Pie3d)
			{
				((Control)buttonSeriesType).Text = "3D " + Loc.Get("Chart", "Pie");
			}
			else
			{
				((Control)buttonSeriesType).Text = Loc.Get("Chart", $"{CurrentSeriesType}");
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
		typeBoxAndWhisker.Tag = StiChartSeriesType.BoxAndWhisker;
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

	private void RefreshExpressionHint()
	{
		if (selectedListBox == listBoxValues || selectedListBox == listBoxEndValues || selectedListBox == listBoxCloseValues || selectedListBox == listBoxLowValues || selectedListBox == listBoxHighValues || selectedListBox == listBoxWeights || (selectedListBox == listBoxArguments && IsBubble))
		{
			textBoxExpression.Hint = "Sum(" + Loc.GetMain("Field") + ")";
		}
		else if ((selectedListBox == listBoxArguments && !IsBubble) || selectedListBox == listBoxSeries)
		{
			textBoxExpression.Hint = Loc.GetMain("Field");
		}
		else
		{
			textBoxExpression.Hint = "";
		}
	}

	internal void RefreshExpression()
	{
		base.LockRefresh = true;
		if (selectedListBox == listBoxValues && ChartValue != null)
		{
			((Control)textBoxExpression).Text = ChartValue.Expression;
		}
		else if (selectedListBox == listBoxEndValues && ChartEndValue != null)
		{
			((Control)textBoxExpression).Text = ChartEndValue.Expression;
		}
		else if (selectedListBox == listBoxCloseValues && ChartCloseValue != null)
		{
			((Control)textBoxExpression).Text = ChartCloseValue.Expression;
		}
		else if (selectedListBox == listBoxLowValues && ChartLowValue != null)
		{
			((Control)textBoxExpression).Text = ChartLowValue.Expression;
		}
		else if (selectedListBox == listBoxHighValues && ChartHighValue != null)
		{
			((Control)textBoxExpression).Text = ChartHighValue.Expression;
		}
		else if (selectedListBox == listBoxWeights && ChartWeight != null)
		{
			((Control)textBoxExpression).Text = ChartWeight.Expression;
		}
		else if (selectedListBox == listBoxArguments && ChartArgument != null)
		{
			((Control)textBoxExpression).Text = ChartArgument.Expression;
		}
		else if (selectedListBox == listBoxSeries && ChartSeries != null)
		{
			((Control)textBoxExpression).Text = ChartSeries.Expression;
		}
		else
		{
			((Control)textBoxExpression).Text = string.Empty;
		}
		base.LockRefresh = false;
	}

	private void ChangeSeriesType(StiValueChartMeter valueChartMeter, StiChartSeriesType chartSeriesType)
	{
		bool isBubble = IsBubble;
		bool isGantt = IsGantt;
		if (IsDataFieldsMode)
		{
			valueChartMeter.SeriesType = chartSeriesType;
		}
		else
		{
			ChartElement.ManuallyEnteredChartMeter.SeriesType = chartSeriesType;
		}
		OldSeriesType = chartSeriesType;
		if (CurrentViewState != null)
		{
			CurrentViewState.SeriesType = chartSeriesType;
		}
		WritePropertiesToElement();
		if (isBubble && !IsBubble)
		{
			StiChartConverter.ConvertFromBubble(ChartElement);
		}
		if (!isBubble && IsBubble)
		{
			StiChartConverter.ConvertToBubble(ChartElement);
		}
		if (!isGantt && IsGantt)
		{
			StiChartConverter.ConvertToGantt(ChartElement);
		}
		ReadPropertiesFromElement();
		ChartElement.CheckBrowsableProperties();
		Designer.InvokeSelectedObjectsChanged();
	}

	private void RefreshGridData()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		if (timerRefreshGrid == null)
		{
			timerRefreshGrid = new Timer();
			timerRefreshGrid.Interval = 100;
			timerRefreshGrid.Tick += delegate
			{
				timerRefreshGrid.Stop();
				StiStringsTableHelper.SaveDataTableToCache(base.Element, gridData.DataTable, saveHeaders: true);
				RefreshElementView();
				RefreshUiPanel();
			};
		}
		timerRefreshGrid.Stop();
		timerRefreshGrid.Start();
	}

	private Task BuildDataGridAsync()
	{
		return Task.Factory.StartNew(delegate
		{
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Expected O, but got Unknown
			string[] columns;
			List<string[]> rows = StiStringsTableHelper.LoadColumnsAndStringRowsFromPackedString(ChartElement.ManuallyEnteredData, out columns);
			StiStringsTableHelper.SaveColumnsAndStringRowsToCache(base.Element, columns, rows);
			DataTable dataTable = StiStringsTableHelper.LoadDataTableFromStringRows(rows, columns);
			dataTable = AddAndRemoveRequiredColumnsToDataTable(dataTable, "Value", "EndValue", "CloseValue", "LowValue", "HighValue", "Argument", "Weight", "Series");
			int numWidth = Math.Max(gridData.MeasureHeader(LocValues), gridData.MeasureHeader(LocArguments)) + StiScale.I4;
			numWidth = Math.Max(numWidth, StiScale.I(40));
			int seriesWidth = Math.Max(gridData.MeasureHeader(Loc.GetMain("Series")), StiScaleUI.I(70)) + StiScale.I4;
			((Control)this).BeginInvoke((Delegate)(MethodInvoker)delegate
			{
				((DataGridView)gridData).AutoGenerateColumns = false;
				((DataGridView)gridData).DataSource = dataTable;
				gridData.AddNewEmptyRowsUpTo(10);
				gridData.AddTextBoxColumn("Value", LocValues, numWidth, (DataGridViewContentAlignment)64);
				gridData.AddTextBoxColumn("EndValue", Loc.GetMain("EndValues"), gridData.MeasureHeader(Loc.GetMain("EndValues")), (DataGridViewContentAlignment)64);
				gridData.AddTextBoxColumn("CloseValue", Loc.GetMain("CloseValues"), gridData.MeasureHeader(Loc.GetMain("CloseValues")), (DataGridViewContentAlignment)64);
				gridData.AddTextBoxColumn("LowValue", Loc.GetMain("LowValues"), gridData.MeasureHeader(Loc.GetMain("LowValues")), (DataGridViewContentAlignment)64);
				gridData.AddTextBoxColumn("HighValue", Loc.GetMain("HighValues"), gridData.MeasureHeader(Loc.GetMain("HighValues")), (DataGridViewContentAlignment)64);
				gridData.AddTextBoxColumn("Argument", LocArguments, numWidth, (DataGridViewContentAlignment)(IsBubble ? 64 : 16));
				gridData.AddTextBoxColumn("Weight", Loc.GetMain("Weights"), gridData.MeasureHeader(Loc.GetMain("Weights")), (DataGridViewContentAlignment)64);
				gridData.AddTextBoxColumn("Series", Loc.GetMain("Series"), seriesWidth, (DataGridViewContentAlignment)16);
				RefreshUiPanel();
				if (IsDataEnterMode)
				{
					((Control)gridData).Focus();
				}
			});
		});
	}

	private DataTable AddAndRemoveRequiredColumnsToDataTable(DataTable table, params string[] columns)
	{
		table.Remove(from tableColumn in table.ColumnsList()
			where !columns.Any((string c) => tableColumn.ColumnName == c)
			select tableColumn);
		columns.Where((string column) => !table.ColumnsList().Any((DataColumn c) => c.ColumnName == column)).ToList().ForEach(delegate(string c)
		{
			switch (c)
			{
			case "Value":
				table.AddColumn(new DataColumn("Value"), 0);
				break;
			case "EndValue":
				table.AddColumn(new DataColumn("EndValue"), ColumnStartWithInvariantLastIndexOf(table, "Value") + 1);
				break;
			case "CloseValue":
				table.AddColumn(new DataColumn("CloseValue"), ColumnStartWithInvariantLastIndexOf(table, "EndValue") + 1);
				break;
			case "LowValue":
				table.AddColumn(new DataColumn("LowValue"), ColumnStartWithInvariantLastIndexOf(table, "CloseValue") + 1);
				break;
			case "HighValue":
				table.AddColumn(new DataColumn("HighValue"), ColumnStartWithInvariantLastIndexOf(table, "LowValue") + 1);
				break;
			case "Argument":
				table.AddColumn(new DataColumn("Argument"), ColumnStartWithInvariantLastIndexOf(table, "HighValue") + 1);
				break;
			case "Weight":
				table.AddColumn(new DataColumn("Weight"), ColumnStartWithInvariantLastIndexOf(table, "Argument") + 1);
				break;
			case "Series":
				table.AddColumn(new DataColumn("Series"));
				break;
			}
		});
		return table;
	}

	public int ColumnStartWithInvariantLastIndexOf(DataTable table, string name)
	{
		return table.ColumnsList().LastOrDefault((DataColumn cl) => cl.ColumnName.StartsWithInvariant(name))?.Ordinal ?? (-1);
	}

	private void SaveManuallyEnteredData()
	{
		ChartElement.ManuallyEnteredData = StiStringsTableHelper.SaveDataTableToPackedString(gridData.DataTable);
	}

	private void SeriesItem_Click(object sender, EventArgs e)
	{
		if (!(sender is ButtonItem buttonItem))
		{
			return;
		}
		StiChartSeriesType currentSeriesType = CurrentSeriesType;
		StiChartSeriesType stiChartSeriesType = (StiChartSeriesType)buttonItem.Tag;
		ChangeSeriesType(ChartValue, stiChartSeriesType);
		if (IsDataFieldsMode && ((ListBox)listBoxValues).Items.Count > 1)
		{
			List<StiChartSeriesType> group = StiChartGroups.GetGroup(stiChartSeriesType);
			List<StiValueChartMeter> list = new List<StiValueChartMeter>();
			foreach (Stimulsoft.Dashboard.Design.Controls.StiMeterItem item in ((ListBox)listBoxValues).Items)
			{
				StiValueChartMeter stiValueChartMeter = item?.Meter as StiValueChartMeter;
				if (!group.Contains(stiValueChartMeter.SeriesType))
				{
					list.Add(stiValueChartMeter);
				}
			}
			foreach (StiValueChartMeter item2 in list)
			{
				ChangeSeriesType(item2, stiChartSeriesType);
			}
		}
		InitButtonSeriesType();
		StiSeries stiSeries = StiChartSeriesCreator.New(currentSeriesType);
		StiSeries stiSeries2 = StiChartSeriesCreator.New(stiChartSeriesType);
		if (!(stiSeries is IStiBaseLineSeries) || !(stiSeries2 is IStiBaseLineSeries))
		{
			if (StiEmptyValuesHelper.IsShowZeros(stiChartSeriesType))
			{
				ShowZeros = ((stiSeries2 as IStiShowZerosSeries).ShowZeros ? StiEmptyCellsAs.Zero : StiEmptyCellsAs.Gap);
			}
			if (StiEmptyValuesHelper.IsShowNulls(stiChartSeriesType))
			{
				ShowNulls = ((stiSeries2 as IStiShowNullsSeries).ShowNulls ? StiEmptyCellsAs.Zero : StiEmptyCellsAs.Gap);
			}
		}
		RefreshUiPanel();
		if (stiSeries2 is IStiPictorialSeries && !(stiSeries is IStiPictorialSeries))
		{
			ChartElement.Icon = StiFontIcons.QuarterFull;
		}
		else if (!(stiSeries2 is IStiPictorialSeries) && stiSeries is IStiPictorialSeries)
		{
			ChartElement.Icon = null;
		}
		BuildIconPanel(null);
	}

	protected override void OnGlobalElementChanged(object sender, EventArgs e)
	{
		if (sender == base.Element)
		{
			IEnumerable<StiValueChartMeter> second = from t in listBoxValues.ToItems()
				select t.Meter as StiValueChartMeter into c
				where c != null
				select c;
			IEnumerable<StiValueChartMeter> dif = ChartElement.Values.Except(second);
			ReadPropertiesFromElement();
			listBoxValues.SelectedMeterItem = listBoxValues.ToItems().FirstOrDefault((Stimulsoft.Dashboard.Design.Controls.StiMeterItem t) => t.Meter == dif.FirstOrDefault());
		}
	}

	private void ButtonSeriesType_PopupOpen(object sender, EventArgs e)
	{
		listBoxValues.EndEdit();
		foreach (ButtonItem subItem in buttonSeriesType.SubItems)
		{
			foreach (ButtonItem subItem2 in subItem.SubItems)
			{
				StiChartSeriesType stiChartSeriesType = (StiChartSeriesType)subItem2.Tag;
				subItem2.Checked = stiChartSeriesType == CurrentSeriesType;
			}
		}
	}

	private void ListBox_ElementsConverting(object sender, EventArgs e)
	{
		Stimulsoft.Dashboard.Design.Controls.StiMeterItem stiMeterItem = sender as Stimulsoft.Dashboard.Design.Controls.StiMeterItem;
		if (stiMeterItem.List == listBoxValues)
		{
			StiValueChartMeter stiValueChartMeter = (StiValueChartMeter)(stiMeterItem.Meter = StiMeterHelper.Chart.GetValue(stiMeterItem.Meter, OldSeriesType));
			stiMeterItem.Image = GetNodeImageKey(stiValueChartMeter.SeriesType);
		}
		if (stiMeterItem.List == listBoxEndValues)
		{
			stiMeterItem.Meter = StiMeterHelper.Chart.GetEndValue(stiMeterItem.Meter);
		}
		if (stiMeterItem.List == listBoxCloseValues)
		{
			stiMeterItem.Meter = StiMeterHelper.Chart.GetCloseValue(stiMeterItem.Meter);
		}
		if (stiMeterItem.List == listBoxLowValues)
		{
			stiMeterItem.Meter = StiMeterHelper.Chart.GetLowValue(stiMeterItem.Meter);
		}
		if (stiMeterItem.List == listBoxHighValues)
		{
			stiMeterItem.Meter = StiMeterHelper.Chart.GetHighValue(stiMeterItem.Meter);
		}
		if (stiMeterItem.List == listBoxArguments)
		{
			stiMeterItem.Meter = StiMeterHelper.Chart.GetArgument(stiMeterItem.Meter);
		}
		if (stiMeterItem.List == listBoxWeights)
		{
			stiMeterItem.Meter = StiMeterHelper.Chart.GetWeight(stiMeterItem.Meter);
		}
		if (stiMeterItem.List == listBoxSeries)
		{
			stiMeterItem.Meter = StiMeterHelper.Chart.GetSeries(stiMeterItem.Meter);
		}
	}

	private void ListBox_ElementsChanged(object sender, StiElementChangedArgs changedArgs)
	{
		ProcessElementChanging(changedArgs);
		WritePropertiesToElement();
		RefreshUiPanel();
	}

	private void ListBox_ElementsDragEnter(object sender, EventArgs e)
	{
		ResetComponentHighlights();
	}

	private void ListBox_ElementsAfterSelect(object sender, EventArgs e)
	{
		selectedListBox = sender as Stimulsoft.Dashboard.Design.Controls.StiMeterListBox;
		RefreshExpression();
		RefreshUiPanel();
	}

	private void ListBox_Enter(object sender, EventArgs e)
	{
		selectedListBox = sender as Stimulsoft.Dashboard.Design.Controls.StiMeterListBox;
		RefreshExpression();
	}

	private void ListBox_AfterSelect(object sender, EventArgs e)
	{
		selectedListBox = sender as Stimulsoft.Dashboard.Design.Controls.StiMeterListBox;
		RefreshExpression();
	}

	private void ListBox_MouseDown(object sender, MouseEventArgs e)
	{
		selectedListBox = sender as Stimulsoft.Dashboard.Design.Controls.StiMeterListBox;
		RefreshExpression();
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
		ChartElement.Icon = ((StiFontIconSelectionControl)sender).SelectedIcon;
		RefreshElementView();
		RefreshUiPanel();
		((Control)iconsPopup).Hide();
	}

	private void PanelBoxIcon_Paint(object sender, PaintEventArgs e)
	{
		RefreshSampleIcon();
	}

	private void ComboBoxYAxis_SelectedIndexChanged(object sender, EventArgs e)
	{
		StiComboBox stiComboBox = comboBoxYAxis;
		bool flag = ((stiComboBox != null) ? ((ComboBox)stiComboBox).SelectedItem : null) as string == LocLeftYAxis;
		if (ChartValue != null)
		{
			ChartValue.YAxis = ((!flag) ? StiSeriesYAxis.RightYAxis : StiSeriesYAxis.LeftYAxis);
		}
		RefreshElementView();
		RefreshUiPanel();
	}

	private void ComboBoxShowZeros_SelectedIndexChanged(object sender, EventArgs e)
	{
		StiComboBox stiComboBox = comboBoxShowZeros;
		string text = ((stiComboBox != null) ? ((ComboBox)stiComboBox).SelectedItem : null) as string;
		if (ShowEmptyValuesInSimpleWay)
		{
			if (text == LocEmptyCellsAsTrue)
			{
				ShowZeros = StiEmptyCellsAs.Zero;
			}
			else
			{
				ShowZeros = StiEmptyCellsAs.Gap;
			}
		}
		else if (text == LocEmptyCellsAsZero)
		{
			ShowZeros = StiEmptyCellsAs.Zero;
		}
		else if (text == LocEmptyCellsAsGap)
		{
			ShowZeros = StiEmptyCellsAs.Gap;
		}
		else if (text == LocEmptyCellsAsConnectPointsWithLine)
		{
			ShowZeros = StiEmptyCellsAs.ConnectPointsWithLine;
		}
		RefreshElementView();
		RefreshUiPanel();
	}

	private void ComboBoxShowNulls_SelectedIndexChanged(object sender, EventArgs e)
	{
		StiComboBox stiComboBox = comboBoxShowNulls;
		string text = ((stiComboBox != null) ? ((ComboBox)stiComboBox).SelectedItem : null) as string;
		if (ShowEmptyValuesInSimpleWay)
		{
			if (text == LocEmptyCellsAsTrue)
			{
				ShowNulls = StiEmptyCellsAs.Zero;
			}
			else
			{
				ShowNulls = StiEmptyCellsAs.Gap;
			}
		}
		else if (text == LocEmptyCellsAsZero)
		{
			ShowNulls = StiEmptyCellsAs.Zero;
		}
		else if (text == LocEmptyCellsAsGap)
		{
			ShowNulls = StiEmptyCellsAs.Gap;
		}
		else if (text == LocEmptyCellsAsConnectPointsWithLine)
		{
			ShowNulls = StiEmptyCellsAs.ConnectPointsWithLine;
		}
		RefreshElementView();
		RefreshUiPanel();
	}

	private void ComboBoxRoundValues_SelectedIndexChanged(object sender, EventArgs e)
	{
		StiComboBox stiComboBox = comboBoxRoundValues;
		string text = ((stiComboBox != null) ? ((ComboBox)stiComboBox).SelectedItem : null) as string;
		ChartElement.RoundValues = text == LocEmptyCellsAsTrue;
		RefreshElementView();
		RefreshUiPanel();
	}

	private void ButtonLineStyle_Click(object sender, EventArgs e)
	{
		ButtonItem buttonItem = sender as ButtonItem;
		if (buttonItem == buttonSolid)
		{
			LineStyle = StiPenStyle.Solid;
		}
		else if (buttonItem == buttonDash)
		{
			LineStyle = StiPenStyle.Dash;
		}
		else if (buttonItem == buttonDashDot)
		{
			LineStyle = StiPenStyle.DashDot;
		}
		else if (buttonItem == buttonDashDotDot)
		{
			LineStyle = StiPenStyle.DashDotDot;
		}
		else if (buttonItem == buttonDot)
		{
			LineStyle = StiPenStyle.Dot;
		}
		else if (buttonItem == buttonNone)
		{
			LineStyle = StiPenStyle.None;
		}
		RefreshButtonBorderStyle();
		RefreshElementView();
		RefreshUiPanel();
	}

	private void NumericUpDownLineWidth_ValueChanged(object sender, EventArgs e)
	{
		LineWidth = (float)numericUpDownLineWidth.Value;
		RefreshElementView();
		RefreshUiPanel();
	}

	private void ButtonShowMore_Click(object sender, EventArgs e)
	{
		isShowMore = !isShowMore;
		RefreshUiPanel();
	}

	private void ButtonViewStates_Click(object sender, EventArgs e)
	{
		new StiChartViewStatesMenu(menuViewStates, (TextBox)(object)textBoxViewStates, this, selectedListBox).CreateItems();
		Point point = new Point(((Control)panelBoxViewStates).Width, ((Control)panelBoxViewStates).Height + StiScale.I1);
		if (StiLeftRightHandedHelper.IsLeftHanded)
		{
			point.X -= menuViewStates.PopupSize.Width;
		}
		point = ((Control)panelBoxViewStates).PointToScreen(point);
		menuViewStates.Popup(point);
	}

	private void TextBoxViewStates_TextChanged(object sender, EventArgs e)
	{
		if (CurrentViewState != null && !((TextBoxBase)textBoxViewStates).ReadOnly)
		{
			CurrentViewState.Name = ((Control)textBoxViewStates).Text;
		}
	}

	private void ButtonDeleteViewState_Click(object sender, EventArgs e)
	{
		if (CurrentViewState == null)
		{
			return;
		}
		int num = ChartElement.UserViewStates.IndexOf(CurrentViewState);
		ChartElement.UserViewStates.Remove(CurrentViewState);
		if (ChartElement.UserViewStates.Count > 0)
		{
			if (num == ChartElement.UserViewStates.Count)
			{
				num--;
			}
			StiUserViewState stiUserViewState = ChartElement.UserViewStates[num];
			ChartElement.SwitchSelectedViewState(stiUserViewState.Key);
		}
		else
		{
			ChartElement.SwitchSelectedViewState(null);
		}
		ReadPropertiesFromElement();
		RefreshUiPanel();
	}

	private void GridData_CellValueChanged(object sender, DataGridViewCellEventArgs e)
	{
		if (e.RowIndex != -1)
		{
			RefreshGridData();
		}
	}

	private void GridData_CellRowsChanged(object sender, EventArgs e)
	{
		RefreshGridData();
	}

	private void ButtonEnterDataManually_Click(object sender, EventArgs e)
	{
		StiChartSeriesType? stiChartSeriesType = ((ChartValue != null) ? new StiChartSeriesType?(ChartValue.SeriesType) : null);
		ChartElement.DataMode = StiDataMode.ManuallyEnteringData;
		if (!IsValuesPresent && stiChartSeriesType.HasValue && ChartValue.SeriesType == StiChartSeriesType.ClusteredColumn)
		{
			ChartValue.SeriesType = stiChartSeriesType.Value;
		}
		RefreshElementView();
		RefreshUiPanel();
		((Control)gridData).Focus();
	}

	private void ButtonUseDataFields_Click(object sender, EventArgs e)
	{
		IsShowMore = true;
		ChartElement.DataMode = StiDataMode.UsingDataFields;
		RefreshElementView();
		RefreshUiPanel();
	}

	private async void This_LoadAsync(object sender, EventArgs e)
	{
		await BuildDataGridAsync();
	}

	protected override void OnClosed(EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		((Form)this).OnClosed(e);
		SaveManuallyEnteredData();
		StiStringsTableHelper.RemoveFromCache(base.Element);
		((Control)panelBoxIcon).Paint -= new PaintEventHandler(PanelBoxIcon_Paint);
		((DataGridView)gridData).CellValueChanged -= new DataGridViewCellEventHandler(GridData_CellValueChanged);
		gridData.CellRowsChanged -= GridData_CellRowsChanged;
		((Form)this).Load -= This_LoadAsync;
		foreach (Stimulsoft.Dashboard.Design.Controls.StiMeterListBox item in (from Control c in (IEnumerable)((Control)this).Controls
			where c is Stimulsoft.Dashboard.Design.Controls.StiMeterListBox
			select c).Cast<Stimulsoft.Dashboard.Design.Controls.StiMeterListBox>())
		{
			((ListBox)item).SelectedIndexChanged -= ListBox_SelectedIndexChangedAsync;
		}
		((Component)(object)timerRefreshGrid)?.Dispose();
		timerRefreshGrid = null;
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/dashboards_chart.htm");
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		((Control)this).OnKeyDown(e);
		if ((int)e.KeyCode == 115)
		{
			StiPropertiesPanelHelper.ActivatePanel(Designer);
		}
	}

	private void TextBoxExpression_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 13)
		{
			RefreshElementView();
		}
	}

	private void TextBoxExpression_TextChanged(object sender, EventArgs e)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		if (base.LockRefresh)
		{
			return;
		}
		if (timerExpressionChanged == null)
		{
			timerExpressionChanged = new Timer
			{
				Interval = 500
			};
			timerExpressionChanged.Tick += delegate
			{
				if (selectedListBox == listBoxValues && ChartValue != null)
				{
					ChartValue.Expression = ((Control)textBoxExpression).Text;
					listBoxValues.SetSelectedText(StiLabelHelper.GetLabel(ChartValue));
				}
				if (selectedListBox == listBoxEndValues && ChartEndValue != null)
				{
					ChartEndValue.Expression = ((Control)textBoxExpression).Text;
					listBoxEndValues.SetSelectedText(StiLabelHelper.GetLabel(ChartEndValue));
				}
				if (selectedListBox == listBoxCloseValues && ChartCloseValue != null)
				{
					ChartCloseValue.Expression = ((Control)textBoxExpression).Text;
					listBoxCloseValues.SetSelectedText(StiLabelHelper.GetLabel(ChartCloseValue));
				}
				if (selectedListBox == listBoxLowValues && ChartLowValue != null)
				{
					ChartLowValue.Expression = ((Control)textBoxExpression).Text;
					listBoxLowValues.SetSelectedText(StiLabelHelper.GetLabel(ChartLowValue));
				}
				if (selectedListBox == listBoxHighValues && ChartHighValue != null)
				{
					ChartHighValue.Expression = ((Control)textBoxExpression).Text;
					listBoxHighValues.SetSelectedText(StiLabelHelper.GetLabel(ChartHighValue));
				}
				if (selectedListBox == listBoxWeights && ChartWeight != null)
				{
					ChartWeight.Expression = ((Control)textBoxExpression).Text;
					listBoxWeights.SetSelectedText(StiLabelHelper.GetLabel(ChartWeight));
				}
				if (selectedListBox == listBoxArguments && ChartArgument != null)
				{
					ChartArgument.Expression = ((Control)textBoxExpression).Text;
					listBoxArguments.SetSelectedText(StiLabelHelper.GetLabel(ChartArgument));
				}
				if (selectedListBox == listBoxSeries && ChartSeries != null)
				{
					ChartSeries.Expression = ((Control)textBoxExpression).Text;
					listBoxSeries.SetSelectedText(StiLabelHelper.GetLabel(ChartSeries));
				}
				RefreshExpressionHint();
			};
		}
		timerExpressionChanged.Stop();
		timerExpressionChanged.Start();
	}

	private void ButtonExpressionMenu_Click(object sender, EventArgs e)
	{
		new StiChartExpressionMenu(menuExpression, (TextBox)(object)textBoxExpression, this, selectedListBox).CreateItems();
		Point point = new Point(((Control)panelBoxExpression).Width, ((Control)panelBoxExpression).Height + StiScale.I1);
		if (StiLeftRightHandedHelper.IsLeftHanded)
		{
			point.X -= menuExpression.PopupSize.Width;
		}
		point = ((Control)panelBoxExpression).PointToScreen(point);
		menuExpression.Popup(point);
	}

	private void ButtonExpressionEdit_Click(object sender, EventArgs e)
	{
		if (StiExpressionEditorHelper.RunExpressionEditor(base.Element, ref textBoxExpression))
		{
			RefreshElementView();
		}
	}

	private async void ListBox_SelectedIndexChangedAsync(object sender, EventArgs e)
	{
		await StiPropertiesPanelHelper.SetPropertyGridAsync(Designer, (sender as Stimulsoft.Dashboard.Design.Controls.StiMeterListBox)?.SelectedMeterItem?.Meter);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		Timer obj = timerExpressionChanged;
		if (obj != null)
		{
			obj.Stop();
		}
		((Component)(object)timerExpressionChanged)?.Dispose();
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_04e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ec: Expected O, but got Unknown
		//IL_11bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1234: Unknown result type (might be due to invalid IL or missing references)
		//IL_123e: Expected O, but got Unknown
		//IL_126b: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_12eb: Expected O, but got Unknown
		//IL_1334: Unknown result type (might be due to invalid IL or missing references)
		//IL_14a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_150f: Unknown result type (might be due to invalid IL or missing references)
		//IL_15d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1de8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e60: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e6a: Expected O, but got Unknown
		//IL_1ed0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f86: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fe1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1feb: Expected O, but got Unknown
		//IL_2008: Unknown result type (might be due to invalid IL or missing references)
		//IL_20b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2115: Unknown result type (might be due to invalid IL or missing references)
		//IL_211f: Expected O, but got Unknown
		//IL_2149: Unknown result type (might be due to invalid IL or missing references)
		//IL_21ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_2286: Unknown result type (might be due to invalid IL or missing references)
		//IL_22fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_2308: Expected O, but got Unknown
		//IL_2335: Unknown result type (might be due to invalid IL or missing references)
		//IL_23b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2430: Unknown result type (might be due to invalid IL or missing references)
		//IL_243a: Expected O, but got Unknown
		//IL_2467: Unknown result type (might be due to invalid IL or missing references)
		//IL_24ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_2563: Unknown result type (might be due to invalid IL or missing references)
		//IL_256d: Expected O, but got Unknown
		//IL_259a: Unknown result type (might be due to invalid IL or missing references)
		//IL_261d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2696: Unknown result type (might be due to invalid IL or missing references)
		//IL_26a0: Expected O, but got Unknown
		//IL_26cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2751: Unknown result type (might be due to invalid IL or missing references)
		//IL_27ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_27d4: Expected O, but got Unknown
		//IL_2801: Unknown result type (might be due to invalid IL or missing references)
		//IL_2885: Unknown result type (might be due to invalid IL or missing references)
		//IL_28fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_2908: Expected O, but got Unknown
		//IL_2935: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a15: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a99: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b27: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bdf: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c43: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2da6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e11: Unknown result type (might be due to invalid IL or missing references)
		//IL_2eba: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fa0: Unknown result type (might be due to invalid IL or missing references)
		//IL_302b: Unknown result type (might be due to invalid IL or missing references)
		//IL_30bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_3171: Unknown result type (might be due to invalid IL or missing references)
		//IL_3495: Unknown result type (might be due to invalid IL or missing references)
		//IL_350c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3516: Expected O, but got Unknown
		//IL_3610: Unknown result type (might be due to invalid IL or missing references)
		//IL_3696: Unknown result type (might be due to invalid IL or missing references)
		//IL_376d: Unknown result type (might be due to invalid IL or missing references)
		//IL_37ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_386f: Unknown result type (might be due to invalid IL or missing references)
		//IL_393b: Unknown result type (might be due to invalid IL or missing references)
		//IL_39bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a49: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b15: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c28: Expected O, but got Unknown
		//IL_3c45: Unknown result type (might be due to invalid IL or missing references)
		//IL_3cf1: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e11: Unknown result type (might be due to invalid IL or missing references)
		//IL_3eb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f30: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f3a: Expected O, but got Unknown
		//IL_3f58: Unknown result type (might be due to invalid IL or missing references)
		//IL_4035: Unknown result type (might be due to invalid IL or missing references)
		//IL_403f: Expected O, but got Unknown
		//IL_40ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_40d8: Expected O, but got Unknown
		//IL_414f: Unknown result type (might be due to invalid IL or missing references)
		//IL_41d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_4252: Unknown result type (might be due to invalid IL or missing references)
		//IL_425c: Expected O, but got Unknown
		//IL_4279: Unknown result type (might be due to invalid IL or missing references)
		//IL_44f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_4510: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiChartElementEditorForm));
		toolTip = new ToolTip(components);
		typeClusteredColumn = new ButtonItem();
		typeStackedColumn = new ButtonItem();
		typeFullStackedColumn = new ButtonItem();
		typePareto = new ButtonItem();
		typeHistogram = new ButtonItem();
		typeBoxAndWhisker = new ButtonItem();
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
		listBoxEndValues = new Stimulsoft.Dashboard.Design.Controls.StiMeterListBox();
		labelEndValues = new StiChartGroupLabel();
		contextMenuBar = new ContextMenuBar();
		menuExpression = new ButtonItem();
		menuViewStates = new ButtonItem();
		panelSeriesType = new StiPanelUX();
		labelChartType = new StiLabelUX();
		buttonSeriesType = new Stimulsoft.Dashboard.Design.Controls.StiChartButton();
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
		listBoxValues = new Stimulsoft.Dashboard.Design.Controls.StiMeterListBox();
		panelOptions = new StiPanelUX();
		panelBoxExpression = new StiPanelBox();
		buttonExpressionEdit = new StiOperationButton();
		textBoxExpression = new HintTextBox();
		buttonExpressionMenu = new StiOperationButton();
		labelExpression = new StiLabelUX();
		listBoxCloseValues = new Stimulsoft.Dashboard.Design.Controls.StiMeterListBox();
		labelCloseValues = new StiChartGroupLabel();
		listBoxLowValues = new Stimulsoft.Dashboard.Design.Controls.StiMeterListBox();
		labelLowValues = new StiChartGroupLabel();
		listBoxHighValues = new Stimulsoft.Dashboard.Design.Controls.StiMeterListBox();
		labelHighValues = new StiChartGroupLabel();
		listBoxSeries = new Stimulsoft.Dashboard.Design.Controls.StiMeterListBox();
		labelSeries = new StiChartGroupLabel();
		listBoxWeights = new Stimulsoft.Dashboard.Design.Controls.StiMeterListBox();
		labelWeights = new StiChartGroupLabel();
		listBoxArguments = new Stimulsoft.Dashboard.Design.Controls.StiMeterListBox();
		labelArguments = new StiChartGroupLabel();
		panelIcon = new StiPanelUX();
		comboBoxRoundValues = new StiComboBox();
		labelRoundValues = new StiLabelUX();
		panelBoxIcon = new StiPanelUX();
		buttonIcon = new StiOperationButton();
		labelIcon = new StiLabelUX();
		panelYAxis = new StiPanelUX();
		labelYAxis = new StiLabelUX();
		comboBoxYAxis = new StiComboBox();
		panelLineSettings = new StiPanelUX();
		numericUpDownLineWidth = new StiNumericUpDown();
		labelLineWidth = new StiLabelUX();
		buttonLineStyle = new StiTypesButton();
		buttonSolid = new ButtonItem();
		buttonDash = new ButtonItem();
		buttonDashDot = new ButtonItem();
		buttonDashDotDot = new ButtonItem();
		buttonDot = new ButtonItem();
		buttonNone = new ButtonItem();
		labelLineStyle = new StiLabelUX();
		imageListPenStyle = new ImageList(components);
		panelMoreOptions = new StiPanelUX();
		buttonShowMore = new StiButtonUX();
		panelShowZeros = new StiPanelUX();
		comboBoxShowZeros = new StiComboBox();
		labelShowZeros = new StiLabelUX();
		panelShowNulls = new StiPanelUX();
		comboBoxShowNulls = new StiComboBox();
		labelShowNulls = new StiLabelUX();
		panelViewStates = new StiPanelUX();
		panelBoxViewStates = new StiPanelBox();
		buttonDeleteViewState = new StiOperationButton();
		textBoxViewStates = new HintTextBox();
		buttonViewStates = new StiOperationButton();
		labelViewStates = new StiLabelUX();
		labelValues = new StiLabelPanel();
		buttonEnterDataManually = new StiOperationButton();
		gridData = new StiExcelGrid();
		labelGridData = new StiLabelPanel();
		buttonUseDataFields = new StiOperationButton();
		((ISupportInitialize)contextMenuBar).BeginInit();
		((Control)panelSeriesType).SuspendLayout();
		((Control)panelOptions).SuspendLayout();
		((Control)panelBoxExpression).SuspendLayout();
		((Control)panelIcon).SuspendLayout();
		((Control)panelBoxIcon).SuspendLayout();
		((Control)panelYAxis).SuspendLayout();
		((Control)panelLineSettings).SuspendLayout();
		((ISupportInitialize)numericUpDownLineWidth).BeginInit();
		((Control)panelMoreOptions).SuspendLayout();
		((Control)panelShowZeros).SuspendLayout();
		((Control)panelShowNulls).SuspendLayout();
		((Control)panelViewStates).SuspendLayout();
		((Control)panelBoxViewStates).SuspendLayout();
		((Control)labelValues).SuspendLayout();
		((ISupportInitialize)gridData).BeginInit();
		((Control)labelGridData).SuspendLayout();
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
		typeBoxAndWhisker.ButtonStyle = eButtonStyle.ImageAndText;
		typeBoxAndWhisker.ImagePaddingHorizontal = 16;
		typeBoxAndWhisker.ImagePaddingVertical = 12;
		typeBoxAndWhisker.Name = "typeBoxAndWhisker";
		typeBoxAndWhisker.Text = "BoxAndWhisker";
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
		((Control)listBoxEndValues).AllowDrop = true;
		((Control)listBoxEndValues).Dock = (DockStyle)1;
		((Control)listBoxEndValues).Location = new Point(24, 309);
		((Control)listBoxEndValues).Margin = new Padding(4);
		((Control)listBoxEndValues).Name = "listBoxEndValues";
		((Control)listBoxEndValues).Size = new Size(540, 62);
		((Control)listBoxEndValues).TabIndex = 3;
		((ListBox)listBoxEndValues).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxEndValues).Enter += ListBox_Enter;
		((Control)listBoxEndValues).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)labelEndValues).Dock = (DockStyle)1;
		((Control)labelEndValues).Location = new Point(24, 247);
		((Control)labelEndValues).Margin = new Padding(4, 0, 4, 0);
		((Control)labelEndValues).Name = "labelEndValues";
		((Control)labelEndValues).Size = new Size(540, 62);
		((Control)labelEndValues).TabIndex = 2;
		((Control)labelEndValues).Text = "End Values";
		contextMenuBar.AntiAlias = true;
		contextMenuBar.DockTabStripHeight = 50;
		((Control)contextMenuBar).Font = new Font("Segoe UI", 9f);
		contextMenuBar.Items.AddRange(new BaseItem[2] { menuExpression, menuViewStates });
		contextMenuBar.Location = new Point(316, 1008);
		((Control)contextMenuBar).Margin = new Padding(4);
		contextMenuBar.Name = "contextMenuBar";
		contextMenuBar.Size = new Size(224, 91);
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
		menuViewStates.AutoExpandOnClick = true;
		menuViewStates.ImagePaddingHorizontal = 16;
		menuViewStates.ImagePaddingVertical = 12;
		menuViewStates.Name = "menuViewStates";
		menuViewStates.Text = "menuViewStates";
		((Control)panelSeriesType).BackColor = Color.White;
		((Control)panelSeriesType).Controls.Add((Control)(object)labelChartType);
		((Control)panelSeriesType).Controls.Add((Control)(object)buttonSeriesType);
		((Control)panelSeriesType).Dock = (DockStyle)2;
		((Control)panelSeriesType).Location = new Point(24, 1305);
		((Control)panelSeriesType).Margin = new Padding(4);
		((Control)panelSeriesType).Name = "panelSeriesType";
		((Control)panelSeriesType).Size = new Size(540, 100);
		((Control)panelSeriesType).TabIndex = 16;
		((Control)labelChartType).ForeColor = Color.DimGray;
		((Control)labelChartType).Location = new Point(0, 28);
		((Control)labelChartType).Margin = new Padding(4, 0, 4, 0);
		((Control)labelChartType).Name = "labelChartType";
		((Control)labelChartType).Size = new Size(202, 63);
		((Control)labelChartType).TabIndex = 10;
		((Control)labelChartType).Text = "Chart Type";
		((Label)labelChartType).TextAlign = (ContentAlignment)16;
		((Label)labelChartType).UseCompatibleTextRendering = true;
		((Control)buttonSeriesType).AccessibleRole = (AccessibleRole)43;
		((Control)buttonSeriesType).Anchor = (AnchorStyles)13;
		buttonSeriesType.AutoExpandOnClick = true;
		buttonSeriesType.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)buttonSeriesType).Location = new Point(214, 23);
		buttonSeriesType.LockClosePopup = false;
		((Control)buttonSeriesType).Margin = new Padding(4);
		((Control)buttonSeriesType).Name = "buttonSeriesType";
		buttonSeriesType.PopupSide = ePopupSide.Right;
		((Control)buttonSeriesType).Size = new Size(326, 74);
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
		categoryBoxAndWhisker.Text = "BoxAndWhisker";
		categoryWaterfall.ImagePaddingHorizontal = 16;
		categoryWaterfall.ImagePaddingVertical = 12;
		categoryWaterfall.Name = "categoryWaterfall";
		categoryWaterfall.SubItems.AddRange(new BaseItem[1] { typeWaterfall });
		categoryWaterfall.Text = "Waterfall";
		categoryPictorial.ImagePaddingHorizontal = 16;
		categoryPictorial.ImagePaddingVertical = 12;
		categoryPictorial.Name = "categoryPictorial";
		categoryPictorial.SubItems.AddRange(new BaseItem[1] { typePictorial });
		categoryPictorial.Text = "Pictorial";
		((Control)listBoxValues).AllowDrop = true;
		((Control)listBoxValues).Dock = (DockStyle)1;
		((Control)listBoxValues).Location = new Point(24, 185);
		((Control)listBoxValues).Margin = new Padding(4);
		((Control)listBoxValues).Name = "listBoxValues";
		((Control)listBoxValues).Size = new Size(540, 62);
		((Control)listBoxValues).TabIndex = 1;
		((ListBox)listBoxValues).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxValues).Enter += ListBox_Enter;
		((Control)listBoxValues).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)panelOptions).BackColor = Color.White;
		((Control)panelOptions).Controls.Add((Control)(object)panelBoxExpression);
		((Control)panelOptions).Controls.Add((Control)(object)labelExpression);
		((Control)panelOptions).Dock = (DockStyle)2;
		((Control)panelOptions).Location = new Point(24, 1188);
		((Control)panelOptions).Margin = new Padding(4);
		((Control)panelOptions).Name = "panelOptions";
		((Control)panelOptions).Size = new Size(540, 117);
		((Control)panelOptions).TabIndex = 17;
		((Control)panelBoxExpression).BackColor = Color.White;
		((Control)panelBoxExpression).Controls.Add((Control)(object)buttonExpressionEdit);
		((Control)panelBoxExpression).Controls.Add((Control)(object)textBoxExpression);
		((Control)panelBoxExpression).Controls.Add((Control)(object)buttonExpressionMenu);
		((Control)panelBoxExpression).Dock = (DockStyle)1;
		((Control)panelBoxExpression).Location = new Point(0, 62);
		((Control)panelBoxExpression).Margin = new Padding(4);
		((Control)panelBoxExpression).Name = "panelBoxExpression";
		((Control)panelBoxExpression).Size = new Size(540, 48);
		((Control)panelBoxExpression).TabIndex = 1;
		((Control)buttonExpressionEdit).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonExpressionEdit).Image = (Image)componentResourceManager.GetObject("buttonExpressionEdit.Image");
		((Control)buttonExpressionEdit).Location = new Point(448, 2);
		((Control)buttonExpressionEdit).Margin = new Padding(4);
		((Control)buttonExpressionEdit).Name = "buttonExpressionEdit";
		((Control)buttonExpressionEdit).Size = new Size(46, 44);
		((Control)buttonExpressionEdit).TabIndex = 2;
		((Control)buttonExpressionEdit).TabStop = false;
		((ButtonBase)buttonExpressionEdit).UseVisualStyleBackColor = true;
		((Control)buttonExpressionEdit).Click += ButtonExpressionEdit_Click;
		((Control)textBoxExpression).Anchor = (AnchorStyles)13;
		((Control)textBoxExpression).BackColor = SystemColors.Window;
		((TextBoxBase)textBoxExpression).BorderStyle = (BorderStyle)0;
		((Control)textBoxExpression).Location = new Point(8, 12);
		((Control)textBoxExpression).Margin = new Padding(4);
		((Control)textBoxExpression).Name = "textBoxExpression";
		((Control)textBoxExpression).Size = new Size(432, 24);
		((Control)textBoxExpression).TabIndex = 0;
		((Control)textBoxExpression).TextChanged += TextBoxExpression_TextChanged;
		((Control)textBoxExpression).KeyDown += new KeyEventHandler(TextBoxExpression_KeyDown);
		((Control)buttonExpressionMenu).Anchor = (AnchorStyles)9;
		((Control)buttonExpressionMenu).Location = new Point(492, 2);
		((Control)buttonExpressionMenu).Margin = new Padding(4);
		((Control)buttonExpressionMenu).Name = "buttonExpressionMenu";
		((Control)buttonExpressionMenu).Size = new Size(46, 44);
		((Control)buttonExpressionMenu).TabIndex = 1;
		((Control)buttonExpressionMenu).TabStop = false;
		((ButtonBase)buttonExpressionMenu).UseVisualStyleBackColor = true;
		((Control)buttonExpressionMenu).Click += ButtonExpressionMenu_Click;
		((Control)labelExpression).Dock = (DockStyle)1;
		((Control)labelExpression).ForeColor = Color.DimGray;
		((Control)labelExpression).Location = new Point(0, 0);
		((Control)labelExpression).Margin = new Padding(4, 0, 4, 0);
		((Control)labelExpression).Name = "labelExpression";
		((Control)labelExpression).Size = new Size(540, 62);
		((Control)labelExpression).TabIndex = 1;
		((Control)labelExpression).Text = "Expression";
		((Label)labelExpression).TextAlign = (ContentAlignment)16;
		((Label)labelExpression).UseCompatibleTextRendering = true;
		((Control)listBoxCloseValues).AllowDrop = true;
		((Control)listBoxCloseValues).Dock = (DockStyle)1;
		((Control)listBoxCloseValues).Location = new Point(24, 433);
		((Control)listBoxCloseValues).Margin = new Padding(4);
		((Control)listBoxCloseValues).Name = "listBoxCloseValues";
		((Control)listBoxCloseValues).Size = new Size(540, 62);
		((Control)listBoxCloseValues).TabIndex = 5;
		((ListBox)listBoxCloseValues).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxCloseValues).Enter += ListBox_Enter;
		((Control)listBoxCloseValues).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)labelCloseValues).Dock = (DockStyle)1;
		((Control)labelCloseValues).Location = new Point(24, 371);
		((Control)labelCloseValues).Margin = new Padding(4, 0, 4, 0);
		((Control)labelCloseValues).Name = "labelCloseValues";
		((Control)labelCloseValues).Size = new Size(540, 62);
		((Control)labelCloseValues).TabIndex = 4;
		((Control)labelCloseValues).Text = "Close Values";
		((Control)listBoxLowValues).AllowDrop = true;
		((Control)listBoxLowValues).Dock = (DockStyle)1;
		((Control)listBoxLowValues).Location = new Point(24, 557);
		((Control)listBoxLowValues).Margin = new Padding(4);
		((Control)listBoxLowValues).Name = "listBoxLowValues";
		((Control)listBoxLowValues).Size = new Size(540, 62);
		((Control)listBoxLowValues).TabIndex = 7;
		((ListBox)listBoxLowValues).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxLowValues).Enter += ListBox_Enter;
		((Control)listBoxLowValues).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)labelLowValues).Dock = (DockStyle)1;
		((Control)labelLowValues).Location = new Point(24, 495);
		((Control)labelLowValues).Margin = new Padding(4, 0, 4, 0);
		((Control)labelLowValues).Name = "labelLowValues";
		((Control)labelLowValues).Size = new Size(540, 62);
		((Control)labelLowValues).TabIndex = 6;
		((Control)labelLowValues).Text = "Low Values";
		((Control)listBoxHighValues).AllowDrop = true;
		((Control)listBoxHighValues).Dock = (DockStyle)1;
		((Control)listBoxHighValues).Location = new Point(24, 681);
		((Control)listBoxHighValues).Margin = new Padding(4);
		((Control)listBoxHighValues).Name = "listBoxHighValues";
		((Control)listBoxHighValues).Size = new Size(540, 62);
		((Control)listBoxHighValues).TabIndex = 9;
		((ListBox)listBoxHighValues).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxHighValues).Enter += ListBox_Enter;
		((Control)listBoxHighValues).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)labelHighValues).Dock = (DockStyle)1;
		((Control)labelHighValues).Location = new Point(24, 619);
		((Control)labelHighValues).Margin = new Padding(4, 0, 4, 0);
		((Control)labelHighValues).Name = "labelHighValues";
		((Control)labelHighValues).Size = new Size(540, 62);
		((Control)labelHighValues).TabIndex = 8;
		((Control)labelHighValues).Text = "High Vaules";
		((Control)listBoxSeries).AllowDrop = true;
		((Control)listBoxSeries).Dock = (DockStyle)1;
		((Control)listBoxSeries).Location = new Point(24, 1053);
		((Control)listBoxSeries).Margin = new Padding(4);
		((Control)listBoxSeries).Name = "listBoxSeries";
		((Control)listBoxSeries).Size = new Size(540, 62);
		((Control)listBoxSeries).TabIndex = 15;
		((ListBox)listBoxSeries).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxSeries).Enter += ListBox_Enter;
		((Control)listBoxSeries).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)labelSeries).Dock = (DockStyle)1;
		((Control)labelSeries).Location = new Point(24, 991);
		((Control)labelSeries).Margin = new Padding(4, 0, 4, 0);
		((Control)labelSeries).Name = "labelSeries";
		((Control)labelSeries).Size = new Size(540, 62);
		((Control)labelSeries).TabIndex = 14;
		((Control)labelSeries).Text = "Series";
		((Control)listBoxWeights).AllowDrop = true;
		((Control)listBoxWeights).Dock = (DockStyle)1;
		((Control)listBoxWeights).Location = new Point(24, 929);
		((Control)listBoxWeights).Margin = new Padding(4);
		((Control)listBoxWeights).Name = "listBoxWeights";
		((Control)listBoxWeights).Size = new Size(540, 62);
		((Control)listBoxWeights).TabIndex = 13;
		((ListBox)listBoxWeights).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxWeights).Enter += ListBox_Enter;
		((Control)listBoxWeights).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)labelWeights).Dock = (DockStyle)1;
		((Control)labelWeights).Location = new Point(24, 867);
		((Control)labelWeights).Margin = new Padding(4, 0, 4, 0);
		((Control)labelWeights).Name = "labelWeights";
		((Control)labelWeights).Size = new Size(540, 62);
		((Control)labelWeights).TabIndex = 12;
		((Control)labelWeights).Text = "Weights";
		((Control)listBoxArguments).AllowDrop = true;
		((Control)listBoxArguments).Dock = (DockStyle)1;
		((Control)listBoxArguments).Location = new Point(24, 805);
		((Control)listBoxArguments).Margin = new Padding(4);
		((Control)listBoxArguments).Name = "listBoxArguments";
		((Control)listBoxArguments).Size = new Size(540, 62);
		((Control)listBoxArguments).TabIndex = 11;
		((ListBox)listBoxArguments).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxArguments).Enter += ListBox_Enter;
		((Control)listBoxArguments).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)labelArguments).Dock = (DockStyle)1;
		((Control)labelArguments).Location = new Point(24, 743);
		((Control)labelArguments).Margin = new Padding(4, 0, 4, 0);
		((Control)labelArguments).Name = "labelArguments";
		((Control)labelArguments).Size = new Size(540, 62);
		((Control)labelArguments).TabIndex = 10;
		((Control)labelArguments).Text = "Arguments";
		((Control)panelIcon).BackColor = Color.White;
		((Control)panelIcon).Controls.Add((Control)(object)comboBoxRoundValues);
		((Control)panelIcon).Controls.Add((Control)(object)labelRoundValues);
		((Control)panelIcon).Controls.Add((Control)(object)panelBoxIcon);
		((Control)panelIcon).Controls.Add((Control)(object)labelIcon);
		((Control)panelIcon).Dock = (DockStyle)2;
		((Control)panelIcon).Location = new Point(24, 1405);
		((Control)panelIcon).Margin = new Padding(6);
		((Control)panelIcon).Name = "panelIcon";
		((Control)panelIcon).Size = new Size(540, 146);
		((Control)panelIcon).TabIndex = 17;
		((Control)comboBoxRoundValues).Anchor = (AnchorStyles)13;
		comboBoxRoundValues.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxRoundValues).FormattingEnabled = true;
		((Control)comboBoxRoundValues).Location = new Point(214, 94);
		((Control)comboBoxRoundValues).Margin = new Padding(4);
		((Control)comboBoxRoundValues).Name = "comboBoxRoundValues";
		((Control)comboBoxRoundValues).Size = new Size(320, 42);
		((Control)comboBoxRoundValues).TabIndex = 11;
		((ComboBox)comboBoxRoundValues).SelectedIndexChanged += ComboBoxRoundValues_SelectedIndexChanged;
		((Label)labelRoundValues).AutoEllipsis = true;
		((Control)labelRoundValues).ForeColor = Color.DimGray;
		((Control)labelRoundValues).Location = new Point(2, 94);
		((Control)labelRoundValues).Margin = new Padding(6, 0, 6, 0);
		((Control)labelRoundValues).Name = "labelRoundValues";
		((Control)labelRoundValues).Size = new Size(210, 40);
		((Control)labelRoundValues).TabIndex = 10;
		((Control)labelRoundValues).Text = "Round Values";
		((Label)labelRoundValues).TextAlign = (ContentAlignment)16;
		((Label)labelRoundValues).UseCompatibleTextRendering = true;
		((Control)panelBoxIcon).Anchor = (AnchorStyles)13;
		((Control)panelBoxIcon).BackColor = Color.White;
		((Control)panelBoxIcon).Controls.Add((Control)(object)buttonIcon);
		((Control)panelBoxIcon).Location = new Point(214, 23);
		((Control)panelBoxIcon).Margin = new Padding(4);
		((Control)panelBoxIcon).Name = "panelBoxIcon";
		((Control)panelBoxIcon).Size = new Size(158, 48);
		((Control)panelBoxIcon).TabIndex = 2;
		((Control)buttonIcon).Anchor = (AnchorStyles)9;
		((Control)buttonIcon).Location = new Point(110, 2);
		((Control)buttonIcon).Margin = new Padding(4);
		((Control)buttonIcon).Name = "buttonIcon";
		((Control)buttonIcon).Size = new Size(46, 44);
		((Control)buttonIcon).TabIndex = 1;
		((Control)buttonIcon).TabStop = false;
		((ButtonBase)buttonIcon).UseVisualStyleBackColor = true;
		((Control)buttonIcon).Click += ButtonIcon_Click;
		((Control)labelIcon).ForeColor = Color.DimGray;
		((Control)labelIcon).Location = new Point(0, 23);
		((Control)labelIcon).Margin = new Padding(4, 0, 4, 0);
		((Control)labelIcon).Name = "labelIcon";
		((Control)labelIcon).Size = new Size(202, 48);
		((Control)labelIcon).TabIndex = 9;
		((Control)labelIcon).Text = "Icon";
		((Label)labelIcon).TextAlign = (ContentAlignment)16;
		((Label)labelIcon).UseCompatibleTextRendering = true;
		((Control)panelYAxis).BackColor = Color.White;
		((Control)panelYAxis).Controls.Add((Control)(object)labelYAxis);
		((Control)panelYAxis).Controls.Add((Control)(object)comboBoxYAxis);
		((Control)panelYAxis).Dock = (DockStyle)2;
		((Control)panelYAxis).Location = new Point(24, 1551);
		((Control)panelYAxis).Margin = new Padding(6);
		((Control)panelYAxis).Name = "panelYAxis";
		((Control)panelYAxis).Size = new Size(540, 73);
		((Control)panelYAxis).TabIndex = 18;
		((Control)labelYAxis).ForeColor = Color.DimGray;
		((Control)labelYAxis).Location = new Point(0, 23);
		((Control)labelYAxis).Margin = new Padding(6, 0, 6, 0);
		((Control)labelYAxis).Name = "labelYAxis";
		((Control)labelYAxis).Size = new Size(200, 42);
		((Control)labelYAxis).TabIndex = 2;
		((Control)labelYAxis).Text = "Y Axis";
		((Label)labelYAxis).TextAlign = (ContentAlignment)16;
		((Label)labelYAxis).UseCompatibleTextRendering = true;
		((Control)comboBoxYAxis).Anchor = (AnchorStyles)13;
		comboBoxYAxis.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxYAxis).FormattingEnabled = true;
		((Control)comboBoxYAxis).Location = new Point(214, 23);
		((Control)comboBoxYAxis).Margin = new Padding(4);
		((Control)comboBoxYAxis).Name = "comboBoxYAxis";
		((Control)comboBoxYAxis).Size = new Size(320, 42);
		((Control)comboBoxYAxis).TabIndex = 3;
		((ComboBox)comboBoxYAxis).SelectedIndexChanged += ComboBoxYAxis_SelectedIndexChanged;
		((Control)panelLineSettings).BackColor = Color.White;
		((Control)panelLineSettings).Controls.Add((Control)(object)numericUpDownLineWidth);
		((Control)panelLineSettings).Controls.Add((Control)(object)labelLineWidth);
		((Control)panelLineSettings).Controls.Add((Control)(object)buttonLineStyle);
		((Control)panelLineSettings).Controls.Add((Control)(object)labelLineStyle);
		((Control)panelLineSettings).Dock = (DockStyle)2;
		((Control)panelLineSettings).Location = new Point(24, 1624);
		((Control)panelLineSettings).Margin = new Padding(6);
		((Control)panelLineSettings).Name = "panelLineSettings";
		((Control)panelLineSettings).Size = new Size(540, 137);
		((Control)panelLineSettings).TabIndex = 19;
		((Control)numericUpDownLineWidth).BackColor = Color.White;
		numericUpDownLineWidth.DecimalPlaces = 0;
		((Control)numericUpDownLineWidth).ForeColor = SystemColors.ControlText;
		((Control)numericUpDownLineWidth).Location = new Point(214, 90);
		((Control)numericUpDownLineWidth).Margin = new Padding(6);
		((Control)numericUpDownLineWidth).Name = "numericUpDownLineWidth";
		((Control)numericUpDownLineWidth).Size = new Size(158, 42);
		((Control)numericUpDownLineWidth).TabIndex = 7;
		numericUpDownLineWidth.ValueChanged += NumericUpDownLineWidth_ValueChanged;
		((Control)labelLineWidth).BackColor = Color.White;
		((Control)labelLineWidth).ForeColor = Color.DimGray;
		((Control)labelLineWidth).Location = new Point(0, 90);
		((Control)labelLineWidth).Margin = new Padding(6, 0, 6, 0);
		((Control)labelLineWidth).Name = "labelLineWidth";
		((Control)labelLineWidth).Size = new Size(200, 42);
		((Control)labelLineWidth).TabIndex = 6;
		((Control)labelLineWidth).Text = "Line Width";
		((Label)labelLineWidth).TextAlign = (ContentAlignment)16;
		((Label)labelLineWidth).UseCompatibleTextRendering = true;
		((Control)buttonLineStyle).AccessibleRole = (AccessibleRole)43;
		buttonLineStyle.AutoExpandOnClick = true;
		buttonLineStyle.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)buttonLineStyle).Location = new Point(214, 23);
		buttonLineStyle.LockClosePopup = false;
		((Control)buttonLineStyle).Margin = new Padding(8);
		((Control)buttonLineStyle).Name = "buttonLineStyle";
		buttonLineStyle.PopupSide = ePopupSide.Bottom;
		buttonLineStyle.ShowArrow = false;
		((Control)buttonLineStyle).Size = new Size(324, 42);
		buttonLineStyle.Style = eDotNetBarStyle.StyleManagerControlled;
		buttonLineStyle.SubItems.AddRange(new BaseItem[6] { buttonSolid, buttonDash, buttonDashDot, buttonDashDotDot, buttonDot, buttonNone });
		((Control)buttonLineStyle).TabIndex = 4;
		((Control)buttonLineStyle).Text = "buttonX1";
		buttonLineStyle.TextAlignment = eButtonTextAlignment.Left;
		buttonSolid.GlobalItem = false;
		buttonSolid.ImagePaddingHorizontal = 16;
		buttonSolid.ImagePaddingVertical = 12;
		buttonSolid.Name = "buttonSolid";
		buttonSolid.Text = "Solid";
		buttonSolid.Click += ButtonLineStyle_Click;
		buttonDash.GlobalItem = false;
		buttonDash.ImagePaddingHorizontal = 16;
		buttonDash.ImagePaddingVertical = 12;
		buttonDash.Name = "buttonDash";
		buttonDash.Text = "Dash";
		buttonDash.Click += ButtonLineStyle_Click;
		buttonDashDot.GlobalItem = false;
		buttonDashDot.ImagePaddingHorizontal = 16;
		buttonDashDot.ImagePaddingVertical = 12;
		buttonDashDot.Name = "buttonDashDot";
		buttonDashDot.Text = "Dash Dot";
		buttonDashDot.Click += ButtonLineStyle_Click;
		buttonDashDotDot.GlobalItem = false;
		buttonDashDotDot.ImagePaddingHorizontal = 16;
		buttonDashDotDot.ImagePaddingVertical = 12;
		buttonDashDotDot.Name = "buttonDashDotDot";
		buttonDashDotDot.Text = "Dash Dot Dot";
		buttonDashDotDot.Click += ButtonLineStyle_Click;
		buttonDot.GlobalItem = false;
		buttonDot.ImagePaddingHorizontal = 16;
		buttonDot.ImagePaddingVertical = 12;
		buttonDot.Name = "buttonDot";
		buttonDot.Text = "Dot";
		buttonDot.Click += ButtonLineStyle_Click;
		buttonNone.GlobalItem = false;
		buttonNone.ImagePaddingHorizontal = 16;
		buttonNone.ImagePaddingVertical = 12;
		buttonNone.Name = "buttonNone";
		buttonNone.Text = "None";
		buttonNone.Click += ButtonLineStyle_Click;
		((Control)labelLineStyle).ForeColor = Color.DimGray;
		((Control)labelLineStyle).Location = new Point(0, 27);
		((Control)labelLineStyle).Margin = new Padding(6, 0, 6, 0);
		((Control)labelLineStyle).Name = "labelLineStyle";
		((Control)labelLineStyle).Size = new Size(200, 38);
		((Control)labelLineStyle).TabIndex = 2;
		((Control)labelLineStyle).Text = "Line Style";
		((Label)labelLineStyle).TextAlign = (ContentAlignment)16;
		((Label)labelLineStyle).UseCompatibleTextRendering = true;
		imageListPenStyle.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imageListPenStyle.ImageStream");
		imageListPenStyle.TransparentColor = Color.Transparent;
		imageListPenStyle.Images.SetKeyName(0, "Dash.png");
		imageListPenStyle.Images.SetKeyName(1, "DashDot.png");
		imageListPenStyle.Images.SetKeyName(2, "DashDotDot.png");
		imageListPenStyle.Images.SetKeyName(3, "Dot.png");
		imageListPenStyle.Images.SetKeyName(4, "Double.png");
		imageListPenStyle.Images.SetKeyName(5, "None.png");
		imageListPenStyle.Images.SetKeyName(6, "Solid.png");
		((Control)panelMoreOptions).BackColor = Color.White;
		((Control)panelMoreOptions).Controls.Add((Control)(object)buttonShowMore);
		((Control)panelMoreOptions).Dock = (DockStyle)2;
		((Control)panelMoreOptions).Location = new Point(24, 1986);
		((Control)panelMoreOptions).Margin = new Padding(2);
		((Control)panelMoreOptions).Name = "panelMoreOptions";
		((Control)panelMoreOptions).Size = new Size(540, 67);
		((Control)panelMoreOptions).TabIndex = 22;
		((Control)panelMoreOptions).Visible = false;
		((Control)buttonShowMore).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonShowMore).FlatAppearance.BorderSize = 0;
		((Control)buttonShowMore).Location = new Point(312, 19);
		((Control)buttonShowMore).Margin = new Padding(2);
		((Control)buttonShowMore).Name = "buttonShowMore";
		((Control)buttonShowMore).Size = new Size(228, 46);
		((Control)buttonShowMore).TabIndex = 30;
		((Control)buttonShowMore).Text = "More options";
		((ButtonBase)buttonShowMore).UseVisualStyleBackColor = true;
		((Control)buttonShowMore).Click += ButtonShowMore_Click;
		((Control)panelShowZeros).BackColor = Color.White;
		((Control)panelShowZeros).Controls.Add((Control)(object)comboBoxShowZeros);
		((Control)panelShowZeros).Controls.Add((Control)(object)labelShowZeros);
		((Control)panelShowZeros).Dock = (DockStyle)2;
		((Control)panelShowZeros).Location = new Point(24, 1761);
		((Control)panelShowZeros).Margin = new Padding(6);
		((Control)panelShowZeros).Name = "panelShowZeros";
		((Control)panelShowZeros).Size = new Size(540, 73);
		((Control)panelShowZeros).TabIndex = 20;
		((Control)comboBoxShowZeros).Anchor = (AnchorStyles)13;
		comboBoxShowZeros.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxShowZeros).FormattingEnabled = true;
		((Control)comboBoxShowZeros).Location = new Point(214, 23);
		((Control)comboBoxShowZeros).Margin = new Padding(4);
		((Control)comboBoxShowZeros).Name = "comboBoxShowZeros";
		((Control)comboBoxShowZeros).Size = new Size(322, 42);
		((Control)comboBoxShowZeros).TabIndex = 5;
		((ComboBox)comboBoxShowZeros).SelectedIndexChanged += ComboBoxShowZeros_SelectedIndexChanged;
		((Control)labelShowZeros).ForeColor = Color.DimGray;
		((Control)labelShowZeros).Location = new Point(0, 23);
		((Control)labelShowZeros).Margin = new Padding(6, 0, 6, 0);
		((Control)labelShowZeros).Name = "labelShowZeros";
		((Control)labelShowZeros).Size = new Size(210, 40);
		((Control)labelShowZeros).TabIndex = 4;
		((Control)labelShowZeros).Text = "Show Zeros";
		((Label)labelShowZeros).TextAlign = (ContentAlignment)16;
		((Label)labelShowZeros).UseCompatibleTextRendering = true;
		((Control)panelShowNulls).BackColor = Color.White;
		((Control)panelShowNulls).Controls.Add((Control)(object)comboBoxShowNulls);
		((Control)panelShowNulls).Controls.Add((Control)(object)labelShowNulls);
		((Control)panelShowNulls).Dock = (DockStyle)2;
		((Control)panelShowNulls).Location = new Point(24, 1834);
		((Control)panelShowNulls).Margin = new Padding(6);
		((Control)panelShowNulls).Name = "panelShowNulls";
		((Control)panelShowNulls).Size = new Size(540, 73);
		((Control)panelShowNulls).TabIndex = 21;
		((Control)comboBoxShowNulls).Anchor = (AnchorStyles)13;
		comboBoxShowNulls.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxShowNulls).FormattingEnabled = true;
		((Control)comboBoxShowNulls).Location = new Point(214, 23);
		((Control)comboBoxShowNulls).Margin = new Padding(4);
		((Control)comboBoxShowNulls).Name = "comboBoxShowNulls";
		((Control)comboBoxShowNulls).Size = new Size(320, 42);
		((Control)comboBoxShowNulls).TabIndex = 5;
		((ComboBox)comboBoxShowNulls).SelectedIndexChanged += ComboBoxShowNulls_SelectedIndexChanged;
		((Label)labelShowNulls).AutoEllipsis = true;
		((Control)labelShowNulls).ForeColor = Color.DimGray;
		((Control)labelShowNulls).Location = new Point(0, 23);
		((Control)labelShowNulls).Margin = new Padding(6, 0, 6, 0);
		((Control)labelShowNulls).Name = "labelShowNulls";
		((Control)labelShowNulls).Size = new Size(210, 40);
		((Control)labelShowNulls).TabIndex = 4;
		((Control)labelShowNulls).Text = "Show Nulls";
		((Label)labelShowNulls).TextAlign = (ContentAlignment)16;
		((Label)labelShowNulls).UseCompatibleTextRendering = true;
		((Control)panelViewStates).BackColor = Color.White;
		((Control)panelViewStates).Controls.Add((Control)(object)panelBoxViewStates);
		((Control)panelViewStates).Controls.Add((Control)(object)labelViewStates);
		((Control)panelViewStates).Dock = (DockStyle)2;
		((Control)panelViewStates).Location = new Point(24, 1907);
		((Control)panelViewStates).Margin = new Padding(6);
		((Control)panelViewStates).Name = "panelViewStates";
		((Control)panelViewStates).Size = new Size(540, 79);
		((Control)panelViewStates).TabIndex = 23;
		((Control)panelBoxViewStates).BackColor = Color.White;
		((Control)panelBoxViewStates).Controls.Add((Control)(object)buttonDeleteViewState);
		((Control)panelBoxViewStates).Controls.Add((Control)(object)textBoxViewStates);
		((Control)panelBoxViewStates).Controls.Add((Control)(object)buttonViewStates);
		((Control)panelBoxViewStates).Location = new Point(214, 23);
		((Control)panelBoxViewStates).Margin = new Padding(4);
		((Control)panelBoxViewStates).Name = "panelBoxViewStates";
		((Control)panelBoxViewStates).Size = new Size(320, 48);
		((Control)panelBoxViewStates).TabIndex = 6;
		((Control)buttonDeleteViewState).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonDeleteViewState).Image = (Image)componentResourceManager.GetObject("buttonDeleteViewState.Image");
		((Control)buttonDeleteViewState).Location = new Point(227, 2);
		((Control)buttonDeleteViewState).Margin = new Padding(4);
		((Control)buttonDeleteViewState).Name = "buttonDeleteViewState";
		((Control)buttonDeleteViewState).Size = new Size(46, 44);
		((Control)buttonDeleteViewState).TabIndex = 3;
		((Control)buttonDeleteViewState).TabStop = false;
		((ButtonBase)buttonDeleteViewState).UseVisualStyleBackColor = true;
		((Control)buttonDeleteViewState).Click += ButtonDeleteViewState_Click;
		((Control)textBoxViewStates).Anchor = (AnchorStyles)13;
		((Control)textBoxViewStates).BackColor = SystemColors.Window;
		((TextBoxBase)textBoxViewStates).BorderStyle = (BorderStyle)0;
		((Control)textBoxViewStates).Location = new Point(8, 12);
		((Control)textBoxViewStates).Margin = new Padding(4);
		((Control)textBoxViewStates).Name = "textBoxViewStates";
		((Control)textBoxViewStates).Size = new Size(213, 24);
		((Control)textBoxViewStates).TabIndex = 0;
		((Control)textBoxViewStates).TextChanged += TextBoxViewStates_TextChanged;
		((Control)buttonViewStates).Anchor = (AnchorStyles)9;
		((Control)buttonViewStates).Location = new Point(272, 2);
		((Control)buttonViewStates).Margin = new Padding(4);
		((Control)buttonViewStates).Name = "buttonViewStates";
		((Control)buttonViewStates).Size = new Size(46, 44);
		((Control)buttonViewStates).TabIndex = 1;
		((Control)buttonViewStates).TabStop = false;
		((ButtonBase)buttonViewStates).UseVisualStyleBackColor = true;
		((Control)buttonViewStates).Click += ButtonViewStates_Click;
		((Label)labelViewStates).AutoEllipsis = true;
		((Control)labelViewStates).ForeColor = Color.DimGray;
		((Control)labelViewStates).Location = new Point(0, 30);
		((Control)labelViewStates).Margin = new Padding(6, 0, 6, 0);
		((Control)labelViewStates).Name = "labelViewStates";
		((Control)labelViewStates).Size = new Size(210, 40);
		((Control)labelViewStates).TabIndex = 4;
		((Control)labelViewStates).Text = "View States";
		((Label)labelViewStates).TextAlign = (ContentAlignment)16;
		((Label)labelViewStates).UseCompatibleTextRendering = true;
		((Control)labelValues).Controls.Add((Control)(object)buttonEnterDataManually);
		((Control)labelValues).Dock = (DockStyle)1;
		((Control)labelValues).Location = new Point(24, 124);
		((Control)labelValues).Margin = new Padding(4, 0, 4, 0);
		((Control)labelValues).Name = "labelValues";
		((Control)labelValues).Size = new Size(540, 61);
		((Control)labelValues).TabIndex = 24;
		((Control)labelValues).Text = "Value";
		labelValues.TextAlign = (ContentAlignment)16;
		((Control)buttonEnterDataManually).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonEnterDataManually).Image = (Image)componentResourceManager.GetObject("buttonEnterDataManually.Image");
		((Control)buttonEnterDataManually).Location = new Point(493, 9);
		((Control)buttonEnterDataManually).Margin = new Padding(4);
		((Control)buttonEnterDataManually).Name = "buttonEnterDataManually";
		((Control)buttonEnterDataManually).Size = new Size(48, 44);
		((Control)buttonEnterDataManually).TabIndex = 18;
		((Control)buttonEnterDataManually).TabStop = false;
		((ButtonBase)buttonEnterDataManually).UseVisualStyleBackColor = true;
		((Control)buttonEnterDataManually).Click += ButtonEnterDataManually_Click;
		((DataGridView)gridData).AllowUserToAddRows = false;
		((DataGridView)gridData).AllowUserToDeleteRows = false;
		((DataGridView)gridData).BackgroundColor = Color.White;
		((DataGridView)gridData).BorderStyle = (BorderStyle)0;
		gridData.ColumnHeadersDefaultCellStyle.Alignment = (DataGridViewContentAlignment)16;
		gridData.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Control;
		gridData.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.1f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		gridData.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.WindowText;
		gridData.ColumnHeadersDefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
		gridData.ColumnHeadersDefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
		gridData.ColumnHeadersDefaultCellStyle.WrapMode = (DataGridViewTriState)1;
		gridData.DefaultCellStyle.Alignment = (DataGridViewContentAlignment)16;
		gridData.DefaultCellStyle.BackColor = Color.White;
		gridData.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.1f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		gridData.DefaultCellStyle.ForeColor = SystemColors.ControlText;
		gridData.DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
		gridData.DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
		gridData.DefaultCellStyle.WrapMode = (DataGridViewTriState)2;
		((Control)gridData).Dock = (DockStyle)1;
		((Control)gridData).Location = new Point(24, 84);
		((Control)gridData).Margin = new Padding(6);
		((Control)gridData).Name = "gridData";
		((DataGridView)gridData).SelectionMode = (DataGridViewSelectionMode)0;
		((Control)gridData).Size = new Size(540, 40);
		((Control)gridData).TabIndex = 25;
		((Control)labelGridData).Controls.Add((Control)(object)buttonUseDataFields);
		((Control)labelGridData).Dock = (DockStyle)1;
		((Control)labelGridData).Location = new Point(24, 23);
		((Control)labelGridData).Margin = new Padding(4, 0, 4, 0);
		((Control)labelGridData).Name = "labelGridData";
		((Control)labelGridData).Size = new Size(540, 61);
		((Control)labelGridData).TabIndex = 26;
		((Control)labelGridData).Text = "Data";
		labelGridData.TextAlign = (ContentAlignment)16;
		((Control)buttonUseDataFields).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonUseDataFields).Image = (Image)componentResourceManager.GetObject("buttonUseDataFields.Image");
		((Control)buttonUseDataFields).Location = new Point(493, 7);
		((Control)buttonUseDataFields).Margin = new Padding(4);
		((Control)buttonUseDataFields).Name = "buttonUseDataFields";
		((Control)buttonUseDataFields).Size = new Size(48, 44);
		((Control)buttonUseDataFields).TabIndex = 18;
		((Control)buttonUseDataFields).TabStop = false;
		((ButtonBase)buttonUseDataFields).UseVisualStyleBackColor = true;
		((Control)buttonUseDataFields).Click += ButtonUseDataFields_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(588, 2076);
		((Control)this).Controls.Add((Control)(object)panelOptions);
		((Control)this).Controls.Add((Control)(object)listBoxSeries);
		((Control)this).Controls.Add((Control)(object)labelSeries);
		((Control)this).Controls.Add((Control)(object)listBoxWeights);
		((Control)this).Controls.Add((Control)(object)labelWeights);
		((Control)this).Controls.Add((Control)(object)listBoxArguments);
		((Control)this).Controls.Add((Control)(object)labelArguments);
		((Control)this).Controls.Add((Control)(object)listBoxHighValues);
		((Control)this).Controls.Add((Control)(object)labelHighValues);
		((Control)this).Controls.Add((Control)(object)listBoxLowValues);
		((Control)this).Controls.Add((Control)(object)labelLowValues);
		((Control)this).Controls.Add((Control)(object)listBoxCloseValues);
		((Control)this).Controls.Add((Control)(object)labelCloseValues);
		((Control)this).Controls.Add((Control)(object)listBoxEndValues);
		((Control)this).Controls.Add((Control)(object)labelEndValues);
		((Control)this).Controls.Add((Control)(object)contextMenuBar);
		((Control)this).Controls.Add((Control)(object)panelSeriesType);
		((Control)this).Controls.Add((Control)(object)listBoxValues);
		((Control)this).Controls.Add((Control)(object)panelIcon);
		((Control)this).Controls.Add((Control)(object)panelYAxis);
		((Control)this).Controls.Add((Control)(object)panelLineSettings);
		((Control)this).Controls.Add((Control)(object)panelShowZeros);
		((Control)this).Controls.Add((Control)(object)panelShowNulls);
		((Control)this).Controls.Add((Control)(object)panelViewStates);
		((Control)this).Controls.Add((Control)(object)panelMoreOptions);
		((Control)this).Controls.Add((Control)(object)labelValues);
		((Control)this).Controls.Add((Control)(object)gridData);
		((Control)this).Controls.Add((Control)(object)labelGridData);
		((Form)this).Margin = new Padding(4);
		((Control)this).Name = "StiChartElementEditorForm";
		((Control)this).Padding = new Padding(24, 23, 24, 23);
		((Control)this).Text = "Chart";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((ISupportInitialize)contextMenuBar).EndInit();
		((Control)panelSeriesType).ResumeLayout(false);
		((Control)panelOptions).ResumeLayout(false);
		((Control)panelBoxExpression).ResumeLayout(false);
		((Control)panelBoxExpression).PerformLayout();
		((Control)panelIcon).ResumeLayout(false);
		((Control)panelBoxIcon).ResumeLayout(false);
		((Control)panelYAxis).ResumeLayout(false);
		((Control)panelLineSettings).ResumeLayout(false);
		((ISupportInitialize)numericUpDownLineWidth).EndInit();
		((Control)panelMoreOptions).ResumeLayout(false);
		((Control)panelShowZeros).ResumeLayout(false);
		((Control)panelShowNulls).ResumeLayout(false);
		((Control)panelViewStates).ResumeLayout(false);
		((Control)panelBoxViewStates).ResumeLayout(false);
		((Control)panelBoxViewStates).PerformLayout();
		((Control)labelValues).ResumeLayout(false);
		((ISupportInitialize)gridData).EndInit();
		((Control)labelGridData).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
