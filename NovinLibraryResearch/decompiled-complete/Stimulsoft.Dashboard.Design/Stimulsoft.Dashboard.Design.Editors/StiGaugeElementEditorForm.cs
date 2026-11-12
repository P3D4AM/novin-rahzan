using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Components.Gauge;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Dashboard.Design.Controls;
using Stimulsoft.Dashboard.Design.Editors.Helpers;
using Stimulsoft.Dashboard.Design.Editors.Menu;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Editors;
using Stimulsoft.Report.Design.Helpers;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Helper;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Dashboard.Design.Editors;

public class StiGaugeElementEditorForm : StiElementEditorForm
{
	private const string ColumnValue = "Value";

	private const string ColumnTarget = "Target";

	private const string ColumnSeries = "Series";

	private StiMeterListBox selectedListBox;

	private Timer timerRefreshGrid;

	private IContainer components;

	private StiMeterListBox listBoxValues;

	private Panel panelUi;

	private ToolTip toolTip;

	private Label labelCalculationMode;

	private StiNumericUpDown numericMaximum;

	private StiNumericUpDown numericMinimum;

	private Label labelMaximum;

	private Label labelMinimum;

	private Label labelFromTo;

	private StiComboBox comboBoxCalculationMode;

	private StiMeterListBox listBoxSeries;

	private StiChartGroupLabel labelSeries;

	private StiChartGroupLabel labelTarget;

	private StiMeterListBox listBoxTarget;

	private Panel panelType;

	private Label labelType;

	private StiOperationRadioButton buttonHalfCircular;

	private StiOperationRadioButton buttonFullCircular;

	private StiOperationRadioButton buttonVerticalLinear;

	private StiOperationRadioButton buttonHorizontalLinear;

	private StiOperationRadioButton buttonBullet;

	private StiPanelBox panelBoxExpression;

	private HintTextBox textBoxExpression;

	private StiOperationButton buttonExpressionMenu;

	private Label labelExpression;

	private ContextMenuBar contextMenuBar;

	private ButtonItem menuExpression;

	private Panel panelRangeType;

	private StiGaugeRangeListBox listBoxRanges;

	private Button buttonAddRange;

	private Panel panelRangeUi;

	private Label labelRangeType;

	private StiComboBox comboBoxRangeType;

	private StiComboBox comboBoxRangeMode;

	private Panel panelRangeOption;

	private ColorPickerButton colorBoxRangeColor;

	private Label label1;

	private StiGroupLine stiGroupLine2;

	private StiPanelBox panelBoxRangeEndExpression;

	private HintTextBox textBoxRangeEndExpression;

	private StiOperationButton buttonRangeEndExpression;

	private StiPanelBox panelBoxRangeStartExpression;

	private HintTextBox textBoxRangeStartExpression;

	private StiOperationButton buttonRangeStartExpression;

	private StiOperationButton buttonExpressionEdit;

	private Panel panelExpression;

	private StiLabelPanel labelValue;

	private StiOperationButton buttonEnterDataManually;

	private StiExcelGrid gridData;

	private StiLabelPanel labelGridData;

	private StiOperationButton buttonUseDataFields;

	internal StiGaugeElement GaugeElement => base.Element as StiGaugeElement;

	internal StiValueGaugeMeter GaugeValue => GaugeElement?.Value;

	internal StiSeriesGaugeMeter GaugeSeries => GaugeElement?.Series;

	internal StiTargetGaugeMeter GaugeTarget => GaugeElement?.Target;

	internal List<StiGaugeRange> GaugeRanges => GaugeElement?.Ranges;

	internal StiGaugeRange GaugeRange => listBoxRanges.SelectedRangeItem?.Range;

	private string LocCalculationModeCustom => Loc.GetEnum("StiGaugeCalculationModeCustom");

	private string LocCalculationModeAuto => Loc.GetEnum("StiGaugeCalculationModeAuto");

	private string LocRangeModePercentage => Loc.GetEnum("StiGaugeRangeModePercentage");

	private string LocRangeModeValue => Loc.GetEnum("StiGaugeRangeModeValue");

	private string LocRangeTypeNone => Loc.GetEnum("StiGaugeRangeTypeNone");

	private string LocRangeTypeColor => Loc.GetEnum("StiGaugeRangeTypeColor");

	internal bool IsValuePresent => ((ListBox)listBoxValues).Items.Count > 0;

	internal bool IsSeriesPresent => ((ListBox)listBoxSeries).Items.Count > 0;

	internal bool IsTargetPresent => ((ListBox)listBoxTarget).Items.Count > 0;

	public override bool IsAnyPresent
	{
		get
		{
			if (!IsValuePresent && !IsSeriesPresent && !IsTargetPresent && !IsShowMore)
			{
				return IsDataEnterMode;
			}
			return true;
		}
	}

	public override bool AllowResize => false;

	internal StiMeterListBox ValuesListBox => listBoxValues;

	internal StiMeterListBox SeriesListBox => listBoxSeries;

	internal StiMeterListBox TargetListBox => listBoxTarget;

	internal StiGaugeRangeListBox RangesListBox => listBoxRanges;

	private bool IsDataEnterMode => GaugeElement.DataMode == StiDataMode.ManuallyEnteringData;

	private bool IsDataFieldsMode => GaugeElement.DataMode == StiDataMode.UsingDataFields;

	private bool IsShowMore { get; set; }

	public StiGaugeElementEditorForm()
		: this(null)
	{
	}

	public StiGaugeElementEditorForm(StiElement element)
		: base(element)
	{
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Expected O, but got Unknown
		InitializeComponent();
		if (StiOptions.Appearance.ShowFormsHelp)
		{
			((Form)this).HelpButton = true;
		}
		selectedListBox = listBoxValues;
		foreach (StiMeterListBox item in (from Control c in (IEnumerable)((Control)this).Controls
			where c is StiMeterListBox
			select c).Cast<StiMeterListBox>())
		{
			item.Designer = Designer;
			item.Editor = this;
			item.AllowMultipleElements = false;
			item.AllowImage = false;
			contextMenuBar.SetContextMenuEx((Control)(object)item, menuExpression);
		}
		listBoxRanges.Designer = Designer;
		listBoxRanges.Editor = this;
		listBoxRanges.AllowImage = false;
		ReadPropertiesFromElement();
		LocalizeInternal();
		InitializeMenus();
		ActivateDictionaryPanel();
		DoScaling();
		((DataGridView)gridData).BorderStyle = (BorderStyle)1;
		((DataGridView)gridData).CellValueChanged += new DataGridViewCellEventHandler(GridData_CellValueChanged);
		gridData.CellRowsChanged += GridData_CellRowsChanged;
		((Control)gridData).Height = StiScale.I(300);
		((DataGridView)gridData).AutoSizeColumnsMode = (DataGridViewAutoSizeColumnsMode)16;
		((Form)this).Load += This_LoadAsync;
	}

	public override void Localize()
	{
		LocalizeInternal();
		((Control)listBoxValues).Invalidate();
		((Control)listBoxSeries).Invalidate();
		((Control)listBoxTarget).Invalidate();
	}

	private void LocalizeInternal()
	{
		((Control)this).Text = Loc.Get("Components", "StiGauge");
		((Control)labelGridData).Text = Loc.GetMain("Data");
		((Control)labelValue).Text = Loc.GetMain("Value");
		((Control)labelSeries).Text = Loc.Get("Chart", "Series");
		((Control)labelTarget).Text = Loc.GetMain("Target");
		((Control)labelType).Text = Loc.GetMain("Type");
		((Control)labelExpression).Text = Loc.GetMain("Expression");
		((Control)labelCalculationMode).Text = Loc.GetMain("Mode");
		((Control)labelMinimum).Text = (Loc.IsCyrillic ? Loc.GetEnum("StiFilterItemValueLow") : Loc.GetMain("Minimum"));
		((Control)labelMaximum).Text = (Loc.IsCyrillic ? Loc.GetEnum("StiFilterItemValueHigh") : Loc.GetMain("Maximum"));
		((Control)labelRangeType).Text = Loc.Get("Dashboard", "RangeType");
		toolTip.SetToolTip((Control)(object)buttonUseDataFields, Loc.Get("Report", "UseDataFields"));
		toolTip.SetToolTip((Control)(object)buttonEnterDataManually, Loc.Get("Report", "EnterDataManually"));
		toolTip.SetToolTip((Control)(object)buttonFullCircular, Loc.GetEnum("StiGaugeTypeFullCircular"));
		toolTip.SetToolTip((Control)(object)buttonHalfCircular, Loc.GetEnum("StiGaugeTypeHalfCircular"));
		toolTip.SetToolTip((Control)(object)buttonVerticalLinear, Loc.GetEnum("StiGaugeTypeLinear"));
		toolTip.SetToolTip((Control)(object)buttonHorizontalLinear, Loc.GetEnum("StiGaugeTypeHorizontalLinear"));
		toolTip.SetToolTip((Control)(object)buttonBullet, Loc.GetEnum("StiGaugeTypeBullet"));
		StiExcelGridLocalizer.Localize(gridData);
		((Control)buttonAddRange).Text = Loc.Get("Dashboard", "AddRange");
		((ComboBox)comboBoxCalculationMode).BeginUpdate();
		int selectedIndex = ((ListControl)comboBoxCalculationMode).SelectedIndex;
		((ComboBox)comboBoxCalculationMode).Items.Clear();
		((ComboBox)comboBoxCalculationMode).Items.Add((object)LocCalculationModeAuto);
		((ComboBox)comboBoxCalculationMode).Items.Add((object)LocCalculationModeCustom);
		((ListControl)comboBoxCalculationMode).SelectedIndex = selectedIndex;
		((ComboBox)comboBoxCalculationMode).EndUpdate();
		((ComboBox)comboBoxRangeMode).BeginUpdate();
		selectedIndex = ((ListControl)comboBoxRangeMode).SelectedIndex;
		((ComboBox)comboBoxRangeMode).Items.Clear();
		((ComboBox)comboBoxRangeMode).Items.Add((object)LocRangeModePercentage);
		((ComboBox)comboBoxRangeMode).Items.Add((object)LocRangeModeValue);
		((ListControl)comboBoxRangeMode).SelectedIndex = selectedIndex;
		((ComboBox)comboBoxRangeMode).EndUpdate();
		((ComboBox)comboBoxRangeType).BeginUpdate();
		selectedIndex = ((ListControl)comboBoxRangeType).SelectedIndex;
		((ComboBox)comboBoxRangeType).Items.Clear();
		((ComboBox)comboBoxRangeType).Items.Add((object)LocRangeTypeNone);
		((ComboBox)comboBoxRangeType).Items.Add((object)LocRangeTypeColor);
		((ListControl)comboBoxRangeType).SelectedIndex = selectedIndex;
		((ComboBox)comboBoxRangeType).EndUpdate();
		DataGridViewColumn val = ((DataGridView)gridData).Columns["Value"];
		if (val != null)
		{
			val.HeaderText = Loc.GetMain("Value");
		}
		val = ((DataGridView)gridData).Columns["Target"];
		if (val != null)
		{
			val.HeaderText = Loc.GetMain("Target");
		}
		val = ((DataGridView)gridData).Columns["Series"];
		if (val != null)
		{
			val.HeaderText = Loc.Get("Chart", "Series");
		}
	}

	private void InitializeMenus()
	{
		menuExpression.PopupOpen += delegate
		{
			new StiGaugeExpressionMenu(menuExpression, (TextBox)(object)textBoxExpression, this, selectedListBox).CreateItems();
		};
	}

	private void DoScaling()
	{
		((ButtonBase)buttonEnterDataManually).Image = StiReportDesignImages.Actions.EditTable();
		((ButtonBase)buttonUseDataFields).Image = StiReportDesignImages.Actions.RetrieveColumnsDown();
		((ButtonBase)buttonRangeStartExpression).Image = StiReportDesignImages.Actions.Edit();
		((ButtonBase)buttonRangeEndExpression).Image = StiReportDesignImages.Actions.Edit();
		((ButtonBase)buttonExpressionEdit).Image = StiReportDesignImages.Actions.Edit();
		((ButtonBase)buttonFullCircular).Image = StiReportDesignImages.Gauge.RadialBar();
		((ButtonBase)buttonHalfCircular).Image = StiReportDesignImages.Gauge.HalfDonuts();
		((ButtonBase)buttonVerticalLinear).Image = StiReportDesignImages.Gauge.VerticalLinear();
		((ButtonBase)buttonHorizontalLinear).Image = StiReportDesignImages.Gauge.HorizontalLinear();
		((ButtonBase)buttonBullet).Image = StiReportDesignImages.Gauge.Bullet();
		StiComboBox stiComboBox = comboBoxCalculationMode;
		int width = (((Control)comboBoxRangeType).Width = ((Control)panelUi).Width - ((Control)comboBoxCalculationMode).Left);
		((Control)stiComboBox).Width = width;
		((Control)numericMaximum).Left = ((Control)comboBoxCalculationMode).Right - ((Control)numericMaximum).Width;
		((Control)buttonAddRange).Height = ((Control)comboBoxRangeMode).Height + 2;
		((Control)buttonAddRange).Top = ((Control)comboBoxRangeMode).Top - 1;
		StiButtonsLocator.ArrangeButtons((Control)(object)panelBoxExpression);
		StiButtonsLocator.ArrangeButtons((Control)(object)panelBoxRangeStartExpression);
		StiButtonsLocator.ArrangeButtons((Control)(object)panelBoxRangeEndExpression);
	}

	protected override void OnGlobalElementChanged(object sender, EventArgs e)
	{
		if (sender == base.Element)
		{
			ReadPropertiesFromElement();
			listBoxValues.SelectedMeterItem = listBoxValues.FirstItem();
		}
	}

	public override void ReadPropertiesFromElement()
	{
		ReadValuesPropertiesFromElement();
		ReadSeriesPropertiesFromElement();
		ReadTargetPropertiesFromElement();
		ReadRangesPropertiesFromElement();
	}

	private void ReadValuesPropertiesFromElement()
	{
		if (!listBoxValues.ItemsEqualTo(GaugeValue))
		{
			((ListBox)listBoxValues).BeginUpdate();
			((ListBox)listBoxValues).Items.Clear();
			listBoxValues.AddItem(GaugeValue);
			listBoxValues.SelectFirstItem();
			((ListBox)listBoxValues).EndUpdate();
		}
	}

	private void ReadSeriesPropertiesFromElement()
	{
		if (!listBoxSeries.ItemsEqualTo(GaugeSeries))
		{
			((ListBox)listBoxSeries).BeginUpdate();
			((ListBox)listBoxSeries).Items.Clear();
			listBoxSeries.AddItem(GaugeSeries);
			listBoxSeries.SelectFirstItem();
			((ListBox)listBoxSeries).EndUpdate();
		}
	}

	private void ReadTargetPropertiesFromElement()
	{
		if (!listBoxTarget.ItemsEqualTo(GaugeTarget))
		{
			((ListBox)listBoxTarget).BeginUpdate();
			((ListBox)listBoxTarget).Items.Clear();
			listBoxTarget.AddItem(GaugeTarget);
			listBoxTarget.SelectFirstItem();
			((ListBox)listBoxTarget).EndUpdate();
		}
	}

	private void ReadRangesPropertiesFromElement()
	{
		((ListBox)listBoxRanges).BeginUpdate();
		((ListBox)listBoxRanges).Items.Clear();
		listBoxRanges.AddItems(GaugeRanges);
		listBoxRanges.SelectFirstItem();
		((ListBox)listBoxRanges).EndUpdate();
	}

	public override void WritePropertiesToElement()
	{
		WriteValuesPropertiesToElement();
		WriteSeriesPropertiesToElement();
		WriteTargetPropertiesToElement();
		WriteRangesPropertiesToElement();
		RefreshElementView();
	}

	private void WriteValuesPropertiesToElement()
	{
		StiMeterItem stiMeterItem = listBoxValues.FirstItem();
		GaugeElement.Value = stiMeterItem?.Meter as StiValueGaugeMeter;
	}

	private void WriteSeriesPropertiesToElement()
	{
		StiMeterItem stiMeterItem = listBoxSeries.FirstItem();
		GaugeElement.Series = stiMeterItem?.Meter as StiSeriesGaugeMeter;
	}

	private void WriteTargetPropertiesToElement()
	{
		StiMeterItem stiMeterItem = listBoxTarget.FirstItem();
		GaugeElement.Target = stiMeterItem?.Meter as StiTargetGaugeMeter;
	}

	private void WriteRangesPropertiesToElement()
	{
		GaugeElement.Ranges = (from t in listBoxRanges.ToItems()
			select t.Range).ToList();
	}

	protected override void AttachEvents()
	{
		base.AttachEvents();
		AttachEventsToValues();
		AttachEventsToSeries();
		AttachEventsToTarget();
		AttachEventsToRanges();
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
		listBoxValues.ElementsChanged += delegate(object sender, StiElementChangedArgs changedArgs)
		{
			ProcessElementChanging(changedArgs);
			WritePropertiesToElement();
			RefreshUiPanel();
		};
		listBoxValues.ElementsDragEnter += delegate
		{
			ResetComponentHighlights();
		};
		listBoxValues.ElementsDragDrop += delegate
		{
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(Designer))
			{
				IStiAppDataCell draggedDataColumnOrVariable = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
				if (draggedDataColumnOrVariable == null)
				{
					return;
				}
				((ListBox)listBoxValues).Items.Clear();
				listBoxValues.AddItem(StiMeterHelper.Gauge.GetValue(draggedDataColumnOrVariable));
				Designer.PageView.ResetDragging();
				GaugeElement.Select();
				WritePropertiesToElement();
				listBoxValues.SelectedMeterItem = listBoxValues.LastItem();
				((Control)listBoxValues).Focus();
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
				((ListBox)listBoxValues).Items.Clear();
				listBoxValues.AddItem(StiMeterHelper.Gauge.GetValue(stiTableColumn));
				Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxValues.SelectedMeterItem = listBoxValues.LastItem();
				((Control)listBoxValues).Focus();
			}
			ResetComponentHighlights();
			RefreshUiPanel();
		};
		listBoxValues.ElementsSelectedChanged += delegate
		{
			GaugeElement.Select();
			RefreshUiPanel();
		};
	}

	private void AttachEventsToSeries()
	{
		listBoxSeries.ElementsConverting += ListBox_ElementsConverting;
		listBoxSeries.ElementsChanged += delegate(object sender, StiElementChangedArgs changedArgs)
		{
			ProcessElementChanging(changedArgs);
			WritePropertiesToElement();
			RefreshUiPanel();
		};
		listBoxSeries.ElementsDragEnter += delegate
		{
			ResetComponentHighlights();
		};
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
				listBoxSeries.AddItem(StiMeterHelper.Gauge.GetSeries(draggedDataColumnOrVariable));
				Designer.PageView.ResetDragging();
				GaugeElement.Select();
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
				listBoxSeries.AddItem(StiMeterHelper.Gauge.GetSeries(stiTableColumn));
				Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxSeries.SelectedMeterItem = listBoxSeries.LastItem();
				((Control)listBoxSeries).Focus();
			}
			ResetComponentHighlights();
		};
		listBoxSeries.ElementsSelectedChanged += delegate
		{
			GaugeElement.Select();
			RefreshUiPanel();
		};
	}

	private void AttachEventsToTarget()
	{
		listBoxTarget.ElementsConverting += ListBox_ElementsConverting;
		listBoxTarget.ElementsChanged += delegate(object sender, StiElementChangedArgs changedArgs)
		{
			ProcessElementChanging(changedArgs);
			WritePropertiesToElement();
			RefreshUiPanel();
			Designer.AddSelectedToPropertyGrid();
		};
		listBoxTarget.ElementsDragEnter += delegate
		{
			ResetComponentHighlights();
		};
		listBoxTarget.ElementsDragDrop += delegate
		{
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(Designer))
			{
				IStiAppDataCell draggedDataColumnOrVariable = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
				if (draggedDataColumnOrVariable == null)
				{
					return;
				}
				((ListBox)listBoxTarget).Items.Clear();
				listBoxTarget.AddItem(StiMeterHelper.Gauge.GetTarget(draggedDataColumnOrVariable));
				Designer.PageView.ResetDragging();
				GaugeElement.Select();
				WritePropertiesToElement();
				listBoxTarget.SelectedMeterItem = listBoxTarget.LastItem();
				((Control)listBoxTarget).Focus();
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
				((ListBox)listBoxTarget).Items.Clear();
				listBoxTarget.AddItem(StiMeterHelper.Gauge.GetTarget(stiTableColumn));
				Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxTarget.SelectedMeterItem = listBoxTarget.LastItem();
				((Control)listBoxTarget).Focus();
			}
			ResetComponentHighlights();
		};
		listBoxTarget.ElementsSelectedChanged += delegate
		{
			GaugeElement.Select();
			RefreshUiPanel();
		};
	}

	private void AttachEventsToRanges()
	{
		listBoxRanges.ElementsChanged += delegate
		{
			WritePropertiesToElement();
			RefreshUiPanel();
		};
		listBoxRanges.ElementsSelectedChanged += delegate
		{
			GaugeElement.Select();
			RefreshUiPanel();
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
			((Control)buttonAddRange).Top = ((Control)comboBoxRangeMode).Top - StiScale.I1;
			((Control)buttonAddRange).Height = ((Control)comboBoxRangeMode).Height + StiScale.I2;
			Panel obj = panelExpression;
			bool visible = (((Control)panelUi).Visible = GaugeValue != null);
			((Control)obj).Visible = visible;
			((ComboBox)comboBoxCalculationMode).SelectedItem = ((GaugeElement.CalculationMode == StiGaugeCalculationMode.Auto) ? LocCalculationModeAuto : LocCalculationModeCustom);
			((ComboBox)comboBoxRangeMode).SelectedItem = ((GaugeElement.RangeMode == StiGaugeRangeMode.Percentage) ? LocRangeModePercentage : LocRangeModeValue);
			((ComboBox)comboBoxRangeType).SelectedItem = ((GaugeElement.RangeType == StiGaugeRangeType.None) ? LocRangeTypeNone : LocRangeTypeColor);
			((RadioButton)buttonFullCircular).Checked = GaugeElement.Type == StiGaugeType.FullCircular;
			((RadioButton)buttonHalfCircular).Checked = GaugeElement.Type == StiGaugeType.HalfCircular;
			((RadioButton)buttonVerticalLinear).Checked = GaugeElement.Type == StiGaugeType.Linear;
			((RadioButton)buttonHorizontalLinear).Checked = GaugeElement.Type == StiGaugeType.HorizontalLinear;
			((RadioButton)buttonBullet).Checked = GaugeElement.Type == StiGaugeType.Bullet;
			numericMinimum.Value = GaugeElement.Minimum;
			numericMaximum.Value = GaugeElement.Maximum;
			bool flag2 = GaugeElement.RangeType != 0 && IsAnyPresent;
			((Control)panelRangeOption).Visible = flag2 && ((ListBox)listBoxRanges).Items.Count > 0;
			((Control)comboBoxRangeMode).Visible = flag2;
			((Control)listBoxRanges).Visible = flag2;
			((Control)panelRangeUi).Visible = flag2;
			StiChartGroupLabel stiChartGroupLabel = labelTarget;
			StiMeterListBox stiMeterListBox = listBoxTarget;
			StiLabelPanel stiLabelPanel = labelValue;
			StiChartGroupLabel stiChartGroupLabel2 = labelSeries;
			Panel obj2 = panelExpression;
			bool flag4 = (((Control)listBoxSeries).Visible = IsAnyPresent && IsDataFieldsMode);
			bool flag6 = (((Control)obj2).Visible = flag4);
			bool flag8 = (((Control)stiChartGroupLabel2).Visible = flag6);
			bool flag10 = (((Control)stiLabelPanel).Visible = flag8);
			visible = (((Control)stiMeterListBox).Visible = flag10);
			((Control)stiChartGroupLabel).Visible = visible;
			Panel obj3 = panelUi;
			Panel obj4 = panelType;
			flag10 = (((Control)panelRangeType).Visible = (IsAnyPresent && IsDataFieldsMode) || IsDataEnterMode);
			visible = (((Control)obj4).Visible = flag10);
			((Control)obj3).Visible = visible;
			StiMeterListBox stiMeterListBox2 = listBoxValues;
			visible = (listBoxValues.ShowEnterDataManuallyLabel = !IsAnyPresent);
			stiMeterListBox2.ShowShowMoreLabel = visible;
			((Control)listBoxValues).Visible = IsDataFieldsMode;
			StiLabelPanel stiLabelPanel2 = labelGridData;
			StiExcelGrid stiExcelGrid = gridData;
			flag10 = (((Control)buttonUseDataFields).Visible = IsDataEnterMode);
			visible = (((Control)stiExcelGrid).Visible = flag10);
			((Control)stiLabelPanel2).Visible = visible;
			((Control)listBoxValues).Dock = (DockStyle)(IsAnyPresent ? 1 : 5);
			CalculatePanelUiHeight();
			SetListHeightFromCount(listBoxValues, multiple: false);
			SetListHeightFromCount(listBoxSeries, multiple: false);
			SetListHeightFromCount(listBoxTarget, multiple: false);
			SetListHeightFromCount(listBoxRanges);
			CalculateWindowHeight();
			CalculateWindowPosition();
		}
		finally
		{
			((Control)this).ResumeLayout();
			base.LockRefresh = false;
		}
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
				StiStringsTableHelper.SaveDataTableToCache(base.Element, gridData.DataTable);
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
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Expected O, but got Unknown
			string[] columns = new string[3] { "Value", "Target", "Series" };
			List<string[]> rows = StiStringsTableHelper.LoadStringRowsFromPackedString(GaugeElement.ManuallyEnteredData, columns);
			StiStringsTableHelper.SaveStringRowsToCache(base.Element, rows);
			((Control)this).BeginInvoke((Delegate)(MethodInvoker)delegate
			{
				((DataGridView)gridData).AutoGenerateColumns = false;
				((DataGridView)gridData).DataSource = StiStringsTableHelper.LoadDataTableFromStringRows(rows, columns);
				gridData.AddNewEmptyRowsUpTo(10);
				gridData.AddTextBoxColumn("Value", Loc.GetMain("Value"), 0, (DataGridViewContentAlignment)64);
				gridData.AddTextBoxColumn("Target", Loc.GetMain("Target"), 0, (DataGridViewContentAlignment)64);
				gridData.AddTextBoxColumn("Series", Loc.Get("Chart", "Series"), 0, (DataGridViewContentAlignment)16);
				if (IsDataEnterMode)
				{
					((Control)gridData).Focus();
				}
			});
		});
	}

	private void SaveManuallyEnteredData()
	{
		GaugeElement.ManuallyEnteredData = StiStringsTableHelper.SaveDataTableToPackedString(gridData.DataTable);
	}

	private void CalculatePanelUiHeight()
	{
		StiComboBox stiComboBox = comboBoxCalculationMode;
		bool flag = ((stiComboBox != null) ? ((ComboBox)stiComboBox).SelectedItem : null) as string == LocCalculationModeAuto;
		((Control)panelUi).Height = ((!flag) ? ((Control)numericMaximum).Bottom : ((Control)comboBoxCalculationMode).Bottom);
		((Control)numericMaximum).Visible = !flag;
		((Control)numericMinimum).Visible = !flag;
	}

	protected virtual void SetListHeightFromCount(StiGaugeRangeListBox list)
	{
		int num = Math.Min(10, ((ListBox)list).Items.Count);
		((Control)list).Height = Math.Min(num + 1, 7) * ((ListBox)list).ItemHeight;
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

	private void RefreshExpressionHint()
	{
		if (selectedListBox == listBoxValues || selectedListBox == listBoxTarget)
		{
			textBoxExpression.Hint = "Sum(" + Loc.GetMain("Field") + ")";
		}
		else if (selectedListBox == listBoxSeries)
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
		if (selectedListBox == listBoxValues && GaugeValue != null)
		{
			((Control)textBoxExpression).Text = GaugeValue.Expression;
		}
		else if (selectedListBox == listBoxSeries && GaugeSeries != null)
		{
			((Control)textBoxExpression).Text = GaugeSeries.Expression;
		}
		else if (selectedListBox == listBoxTarget && GaugeTarget != null)
		{
			((Control)textBoxExpression).Text = GaugeTarget.Expression;
		}
		else
		{
			((Control)textBoxExpression).Text = string.Empty;
		}
		base.LockRefresh = false;
	}

	internal void RefreshRange()
	{
		if (GaugeRange != null)
		{
			base.LockRefresh = true;
			colorBoxRangeColor.SelectedColor = GaugeRange.Color;
			((Control)textBoxRangeStartExpression).Text = GaugeRange.StartExpression;
			((Control)textBoxRangeEndExpression).Text = GaugeRange.EndExpression;
			((Control)colorBoxRangeColor).Invalidate();
			base.LockRefresh = false;
		}
	}

	private Color GetPreferedColor()
	{
		if (GaugeRanges.All((StiGaugeRange r) => r.Color != Color.Green))
		{
			return Color.Green;
		}
		if (GaugeRanges.All((StiGaugeRange r) => r.Color != Color.Yellow))
		{
			return Color.Yellow;
		}
		if (GaugeRanges.All((StiGaugeRange r) => r.Color != Color.Red))
		{
			return Color.Red;
		}
		return Color.Orange;
	}

	private void CreatedDefaultRanges()
	{
		GaugeRanges.Add(new StiGaugeRange(Color.Red, "0", "33"));
		GaugeRanges.Add(new StiGaugeRange(Color.Yellow, "33", "66"));
		GaugeRanges.Add(new StiGaugeRange(Color.Green, "66", "100"));
	}

	private void ButtonExpressionEdit_Click(object sender, EventArgs e)
	{
		if (StiExpressionEditorHelper.RunExpressionEditor(base.Element, ref textBoxExpression))
		{
			RefreshElementView();
		}
	}

	private void ListBox_ElementsConverting(object sender, EventArgs e)
	{
		StiMeterItem stiMeterItem = sender as StiMeterItem;
		if (stiMeterItem.List == listBoxValues)
		{
			stiMeterItem.Meter = StiMeterHelper.Gauge.GetValue(stiMeterItem.Meter);
		}
		if (stiMeterItem.List == listBoxSeries)
		{
			stiMeterItem.Meter = StiMeterHelper.Gauge.GetSeries(stiMeterItem.Meter);
		}
		if (stiMeterItem.List == listBoxTarget)
		{
			stiMeterItem.Meter = StiMeterHelper.Gauge.GetTarget(stiMeterItem.Meter);
		}
	}

	private void ListBox_Enter(object sender, EventArgs e)
	{
		selectedListBox = sender as StiMeterListBox;
		RefreshExpression();
	}

	private void ListBox_AfterSelect(object sender, EventArgs e)
	{
		selectedListBox = sender as StiMeterListBox;
		RefreshExpression();
	}

	private void ListBox_MouseDown(object sender, MouseEventArgs e)
	{
		selectedListBox = sender as StiMeterListBox;
		RefreshExpression();
	}

	private void ListBoxRanges_Enter(object sender, EventArgs e)
	{
		RefreshRange();
	}

	private void ListBoxRanges_MouseDown(object sender, MouseEventArgs e)
	{
		RefreshRange();
	}

	private void ListBoxRanges_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			RefreshRange();
		}
	}

	private void ColorBoxRangeColor_SelectedColorChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			GaugeRange.Color = colorBoxRangeColor.SelectedColor;
			RefreshElementView();
			listBoxRanges.InvalidateSelectedItem();
		}
	}

	private void NumericMinimum_ValueChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			GaugeElement.Minimum = numericMinimum.Value;
			RefreshElementView();
		}
	}

	private void NumericMaximum_ValueChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			GaugeElement.Maximum = numericMaximum.Value;
			RefreshElementView();
		}
	}

	private void ComboBoxCalculationMode_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			StiComboBox stiComboBox = comboBoxCalculationMode;
			bool flag = ((stiComboBox != null) ? ((ComboBox)stiComboBox).SelectedItem : null) as string == LocCalculationModeAuto;
			GaugeElement.CalculationMode = (flag ? StiGaugeCalculationMode.Auto : StiGaugeCalculationMode.Custom);
			CalculatePanelUiHeight();
			RefreshElementView();
			RefreshUiPanel();
		}
	}

	private void ComboBoxRangeType_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			GaugeElement.RangeType = ((!(((ComboBox)comboBoxRangeType).SelectedItem as string == LocRangeTypeNone)) ? StiGaugeRangeType.Color : StiGaugeRangeType.None);
			if (GaugeElement.RangeType == StiGaugeRangeType.Color && GaugeRanges.Count == 0)
			{
				CreatedDefaultRanges();
				ReadRangesPropertiesFromElement();
			}
			RefreshElementView();
			RefreshUiPanel();
		}
	}

	private void ComboBoxRangeMode_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			GaugeElement.RangeMode = ((((ComboBox)comboBoxRangeMode).SelectedItem as string == LocRangeModePercentage) ? StiGaugeRangeMode.Percentage : StiGaugeRangeMode.Value);
			RefreshElementView();
			RefreshUiPanel();
		}
	}

	private void ButtonGaugeType_CheckedChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			if (((RadioButton)buttonFullCircular).Checked)
			{
				GaugeElement.Type = StiGaugeType.FullCircular;
			}
			else if (((RadioButton)buttonHalfCircular).Checked)
			{
				GaugeElement.Type = StiGaugeType.HalfCircular;
			}
			else if (((RadioButton)buttonVerticalLinear).Checked)
			{
				GaugeElement.Type = StiGaugeType.Linear;
			}
			else if (((RadioButton)buttonHorizontalLinear).Checked)
			{
				GaugeElement.Type = StiGaugeType.HorizontalLinear;
			}
			else if (((RadioButton)buttonBullet).Checked)
			{
				GaugeElement.Type = StiGaugeType.Bullet;
			}
			RefreshElementView();
			RefreshUiPanel();
		}
	}

	private void ButtonAddRange_Click(object sender, EventArgs e)
	{
		GaugeElement.Ranges.Add(new StiGaugeRange
		{
			Color = GetPreferedColor()
		});
		ReadRangesPropertiesFromElement();
		RefreshElementView();
		RefreshUiPanel();
		listBoxRanges.SelectLastItem();
	}

	private void ButtonRangeExpression_Click(object sender, EventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Invalid comparison between Unknown and I4
		HintTextBox hintTextBox = ((sender == buttonRangeStartExpression) ? textBoxRangeStartExpression : textBoxRangeEndExpression);
		string text = ((Control)hintTextBox).Text;
		DialogResult val = StiTextEditor.EditText(base.Element.Report, ref text, StiCodeEditorMode.EditorDashboardExpression, Loc.Get("FormTitles", "ExpressionEditorForm"));
		if ((int)val == 1)
		{
			((Control)hintTextBox).Text = text;
		}
		((Control)hintTextBox).Focus();
		((TextBoxBase)hintTextBox).Select(0, 0);
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
		GaugeElement.DataMode = StiDataMode.ManuallyEnteringData;
		RefreshElementView();
		RefreshUiPanel();
		((Control)gridData).Focus();
	}

	private void ButtonUseDataFields_Click(object sender, EventArgs e)
	{
		IsShowMore = true;
		GaugeElement.DataMode = StiDataMode.UsingDataFields;
		RefreshElementView();
		RefreshUiPanel();
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/dashboards_gauge.htm");
	}

	private async void This_LoadAsync(object sender, EventArgs e)
	{
		await BuildDataGridAsync();
	}

	protected override void OnClosed(EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		((Form)this).OnClosed(e);
		SaveManuallyEnteredData();
		StiStringsTableHelper.RemoveFromCache(base.Element);
		((DataGridView)gridData).CellValueChanged -= new DataGridViewCellEventHandler(GridData_CellValueChanged);
		gridData.CellRowsChanged -= GridData_CellRowsChanged;
		((Form)this).Load -= This_LoadAsync;
		((Component)(object)timerRefreshGrid)?.Dispose();
		timerRefreshGrid = null;
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

	private void TextBoxExpression_Leave(object sender, EventArgs e)
	{
		RefreshElementView();
	}

	private void TextBoxExpression_TextChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			if (selectedListBox == listBoxValues && GaugeValue != null)
			{
				GaugeValue.Expression = ((Control)textBoxExpression).Text;
				listBoxValues.SetSelectedText(StiLabelHelper.GetLabel(GaugeValue));
			}
			if (selectedListBox == listBoxSeries && GaugeSeries != null)
			{
				GaugeSeries.Expression = ((Control)textBoxExpression).Text;
				listBoxSeries.SetSelectedText(StiLabelHelper.GetLabel(GaugeSeries));
			}
			if (selectedListBox == listBoxTarget && GaugeTarget != null)
			{
				GaugeTarget.Expression = ((Control)textBoxExpression).Text;
				listBoxTarget.SetSelectedText(StiLabelHelper.GetLabel(GaugeTarget));
			}
			RefreshExpressionHint();
		}
	}

	private void ButtonExpressionMenu_Click(object sender, EventArgs e)
	{
		new StiGaugeExpressionMenu(menuExpression, (TextBox)(object)textBoxExpression, this, selectedListBox).CreateItems();
		Point point = new Point(((Control)panelBoxExpression).Width, ((Control)panelBoxExpression).Height + StiScale.I1);
		if (StiLeftRightHandedHelper.IsLeftHanded)
		{
			point.X -= menuExpression.PopupSize.Width;
		}
		point = ((Control)panelBoxExpression).PointToScreen(point);
		menuExpression.Popup(point);
	}

	private void TextBoxRangeStartExpression_TextChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			GaugeRange.StartExpression = ((Control)textBoxRangeStartExpression).Text;
			RefreshElementView();
			listBoxRanges.InvalidateSelectedItem();
		}
	}

	private void TextBoxRangeEndExpression_TextChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			GaugeRange.EndExpression = ((Control)textBoxRangeEndExpression).Text;
			RefreshElementView();
			listBoxRanges.InvalidateSelectedItem();
		}
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
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Expected O, but got Unknown
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Expected O, but got Unknown
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Expected O, but got Unknown
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_042a: Unknown result type (might be due to invalid IL or missing references)
		//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0508: Unknown result type (might be due to invalid IL or missing references)
		//IL_0512: Expected O, but got Unknown
		//IL_0547: Unknown result type (might be due to invalid IL or missing references)
		//IL_0551: Expected O, but got Unknown
		//IL_056b: Unknown result type (might be due to invalid IL or missing references)
		//IL_062e: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_070c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0716: Expected O, but got Unknown
		//IL_074b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0755: Expected O, but got Unknown
		//IL_076f: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f9: Expected O, but got Unknown
		//IL_081a: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a41: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c97: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d18: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e46: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea7: Expected O, but got Unknown
		//IL_0ec8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff7: Unknown result type (might be due to invalid IL or missing references)
		//IL_108c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1117: Unknown result type (might be due to invalid IL or missing references)
		//IL_119e: Unknown result type (might be due to invalid IL or missing references)
		//IL_123b: Unknown result type (might be due to invalid IL or missing references)
		//IL_134d: Unknown result type (might be due to invalid IL or missing references)
		//IL_13b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_143f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1449: Expected O, but got Unknown
		//IL_1467: Unknown result type (might be due to invalid IL or missing references)
		//IL_14f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_14fc: Expected O, but got Unknown
		//IL_151a: Unknown result type (might be due to invalid IL or missing references)
		//IL_15a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_15af: Expected O, but got Unknown
		//IL_15cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1658: Unknown result type (might be due to invalid IL or missing references)
		//IL_1662: Expected O, but got Unknown
		//IL_1680: Unknown result type (might be due to invalid IL or missing references)
		//IL_1717: Unknown result type (might be due to invalid IL or missing references)
		//IL_1721: Expected O, but got Unknown
		//IL_173f: Unknown result type (might be due to invalid IL or missing references)
		//IL_17fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_18b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_190e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1918: Expected O, but got Unknown
		//IL_1935: Unknown result type (might be due to invalid IL or missing references)
		//IL_19e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a59: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a63: Expected O, but got Unknown
		//IL_1a8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bbb: Expected O, but got Unknown
		//IL_1bfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d32: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d3c: Expected O, but got Unknown
		//IL_1dcb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dd5: Expected O, but got Unknown
		//IL_1e4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ecc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f48: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f52: Expected O, but got Unknown
		//IL_1f88: Unknown result type (might be due to invalid IL or missing references)
		//IL_2000: Unknown result type (might be due to invalid IL or missing references)
		//IL_200a: Expected O, but got Unknown
		//IL_2037: Unknown result type (might be due to invalid IL or missing references)
		//IL_20ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_2132: Unknown result type (might be due to invalid IL or missing references)
		//IL_213c: Expected O, but got Unknown
		//IL_2169: Unknown result type (might be due to invalid IL or missing references)
		//IL_21ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_2264: Unknown result type (might be due to invalid IL or missing references)
		//IL_226e: Expected O, but got Unknown
		//IL_22ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_2327: Unknown result type (might be due to invalid IL or missing references)
		//IL_2331: Expected O, but got Unknown
		//IL_234e: Unknown result type (might be due to invalid IL or missing references)
		//IL_23f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2471: Unknown result type (might be due to invalid IL or missing references)
		//IL_247b: Expected O, but got Unknown
		//IL_2498: Unknown result type (might be due to invalid IL or missing references)
		//IL_2645: Unknown result type (might be due to invalid IL or missing references)
		//IL_2663: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiGaugeElementEditorForm));
		toolTip = new ToolTip(components);
		panelRangeOption = new Panel();
		panelBoxRangeEndExpression = new StiPanelBox();
		textBoxRangeEndExpression = new HintTextBox();
		buttonRangeEndExpression = new StiOperationButton();
		panelBoxRangeStartExpression = new StiPanelBox();
		textBoxRangeStartExpression = new HintTextBox();
		buttonRangeStartExpression = new StiOperationButton();
		label1 = new Label();
		colorBoxRangeColor = new ColorPickerButton();
		panelRangeUi = new Panel();
		stiGroupLine2 = new StiGroupLine();
		comboBoxRangeMode = new StiComboBox();
		buttonAddRange = new Button();
		panelRangeType = new Panel();
		comboBoxRangeType = new StiComboBox();
		labelRangeType = new Label();
		panelUi = new Panel();
		labelFromTo = new Label();
		numericMaximum = new StiNumericUpDown();
		numericMinimum = new StiNumericUpDown();
		labelMaximum = new Label();
		labelMinimum = new Label();
		labelCalculationMode = new Label();
		comboBoxCalculationMode = new StiComboBox();
		panelType = new Panel();
		labelType = new Label();
		buttonBullet = new StiOperationRadioButton();
		buttonHorizontalLinear = new StiOperationRadioButton();
		buttonVerticalLinear = new StiOperationRadioButton();
		buttonHalfCircular = new StiOperationRadioButton();
		buttonFullCircular = new StiOperationRadioButton();
		panelExpression = new Panel();
		panelBoxExpression = new StiPanelBox();
		buttonExpressionEdit = new StiOperationButton();
		textBoxExpression = new HintTextBox();
		buttonExpressionMenu = new StiOperationButton();
		labelExpression = new Label();
		contextMenuBar = new ContextMenuBar();
		menuExpression = new ButtonItem();
		gridData = new StiExcelGrid();
		listBoxRanges = new StiGaugeRangeListBox();
		listBoxSeries = new StiMeterListBox();
		labelSeries = new StiChartGroupLabel();
		listBoxTarget = new StiMeterListBox();
		labelTarget = new StiChartGroupLabel();
		listBoxValues = new StiMeterListBox();
		labelValue = new StiLabelPanel();
		buttonEnterDataManually = new StiOperationButton();
		labelGridData = new StiLabelPanel();
		buttonUseDataFields = new StiOperationButton();
		((Control)panelRangeOption).SuspendLayout();
		((Control)panelBoxRangeEndExpression).SuspendLayout();
		((Control)panelBoxRangeStartExpression).SuspendLayout();
		((Control)panelRangeUi).SuspendLayout();
		((Control)panelRangeType).SuspendLayout();
		((Control)panelUi).SuspendLayout();
		((ISupportInitialize)numericMaximum).BeginInit();
		((ISupportInitialize)numericMinimum).BeginInit();
		((Control)panelType).SuspendLayout();
		((Control)panelExpression).SuspendLayout();
		((Control)panelBoxExpression).SuspendLayout();
		((ISupportInitialize)contextMenuBar).BeginInit();
		((ISupportInitialize)gridData).BeginInit();
		((Control)labelValue).SuspendLayout();
		((Control)labelGridData).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panelRangeOption).BackColor = Color.White;
		((Control)panelRangeOption).Controls.Add((Control)(object)panelBoxRangeEndExpression);
		((Control)panelRangeOption).Controls.Add((Control)(object)panelBoxRangeStartExpression);
		((Control)panelRangeOption).Controls.Add((Control)(object)label1);
		((Control)panelRangeOption).Controls.Add((Control)(object)colorBoxRangeColor);
		((Control)panelRangeOption).Dock = (DockStyle)1;
		((Control)panelRangeOption).Location = new Point(24, 1275);
		((Control)panelRangeOption).Margin = new Padding(6);
		((Control)panelRangeOption).Name = "panelRangeOption";
		((Control)panelRangeOption).Size = new Size(534, 142);
		((Control)panelRangeOption).TabIndex = 12;
		((Control)panelBoxRangeEndExpression).BackColor = Color.White;
		((Control)panelBoxRangeEndExpression).Controls.Add((Control)(object)textBoxRangeEndExpression);
		((Control)panelBoxRangeEndExpression).Controls.Add((Control)(object)buttonRangeEndExpression);
		((Control)panelBoxRangeEndExpression).Location = new Point(376, 23);
		((Control)panelBoxRangeEndExpression).Margin = new Padding(4);
		((Control)panelBoxRangeEndExpression).Name = "panelBoxRangeEndExpression";
		((Control)panelBoxRangeEndExpression).Size = new Size(158, 48);
		((Control)panelBoxRangeEndExpression).TabIndex = 1;
		((Control)textBoxRangeEndExpression).Anchor = (AnchorStyles)13;
		((Control)textBoxRangeEndExpression).BackColor = SystemColors.Window;
		((TextBoxBase)textBoxRangeEndExpression).BorderStyle = (BorderStyle)0;
		((Control)textBoxRangeEndExpression).Location = new Point(8, 12);
		((Control)textBoxRangeEndExpression).Margin = new Padding(4);
		((Control)textBoxRangeEndExpression).Name = "textBoxRangeEndExpression";
		((Control)textBoxRangeEndExpression).Size = new Size(98, 24);
		((Control)textBoxRangeEndExpression).TabIndex = 0;
		((Control)textBoxRangeEndExpression).TextChanged += TextBoxRangeEndExpression_TextChanged;
		((Control)textBoxRangeEndExpression).KeyDown += new KeyEventHandler(TextBoxExpression_KeyDown);
		((Control)textBoxRangeEndExpression).Leave += TextBoxExpression_Leave;
		((Control)buttonRangeEndExpression).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonRangeEndExpression).Image = (Image)componentResourceManager.GetObject("buttonRangeEndExpression.Image");
		((Control)buttonRangeEndExpression).Location = new Point(110, 2);
		((Control)buttonRangeEndExpression).Margin = new Padding(4);
		((Control)buttonRangeEndExpression).Name = "buttonRangeEndExpression";
		((Control)buttonRangeEndExpression).Size = new Size(46, 44);
		((Control)buttonRangeEndExpression).TabIndex = 1;
		((Control)buttonRangeEndExpression).TabStop = false;
		((ButtonBase)buttonRangeEndExpression).UseVisualStyleBackColor = true;
		((Control)buttonRangeEndExpression).Click += ButtonRangeExpression_Click;
		((Control)panelBoxRangeStartExpression).BackColor = Color.White;
		((Control)panelBoxRangeStartExpression).Controls.Add((Control)(object)textBoxRangeStartExpression);
		((Control)panelBoxRangeStartExpression).Controls.Add((Control)(object)buttonRangeStartExpression);
		((Control)panelBoxRangeStartExpression).Location = new Point(180, 23);
		((Control)panelBoxRangeStartExpression).Margin = new Padding(4);
		((Control)panelBoxRangeStartExpression).Name = "panelBoxRangeStartExpression";
		((Control)panelBoxRangeStartExpression).Size = new Size(158, 48);
		((Control)panelBoxRangeStartExpression).TabIndex = 0;
		((Control)textBoxRangeStartExpression).Anchor = (AnchorStyles)13;
		((Control)textBoxRangeStartExpression).BackColor = SystemColors.Window;
		((TextBoxBase)textBoxRangeStartExpression).BorderStyle = (BorderStyle)0;
		((Control)textBoxRangeStartExpression).Location = new Point(8, 12);
		((Control)textBoxRangeStartExpression).Margin = new Padding(4);
		((Control)textBoxRangeStartExpression).Name = "textBoxRangeStartExpression";
		((Control)textBoxRangeStartExpression).Size = new Size(98, 24);
		((Control)textBoxRangeStartExpression).TabIndex = 0;
		((Control)textBoxRangeStartExpression).TextChanged += TextBoxRangeStartExpression_TextChanged;
		((Control)textBoxRangeStartExpression).KeyDown += new KeyEventHandler(TextBoxExpression_KeyDown);
		((Control)textBoxRangeStartExpression).Leave += TextBoxExpression_Leave;
		((Control)buttonRangeStartExpression).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonRangeStartExpression).Image = (Image)componentResourceManager.GetObject("buttonRangeStartExpression.Image");
		((Control)buttonRangeStartExpression).Location = new Point(110, 2);
		((Control)buttonRangeStartExpression).Margin = new Padding(4);
		((Control)buttonRangeStartExpression).Name = "buttonRangeStartExpression";
		((Control)buttonRangeStartExpression).Size = new Size(46, 44);
		((Control)buttonRangeStartExpression).TabIndex = 1;
		((Control)buttonRangeStartExpression).TabStop = false;
		((ButtonBase)buttonRangeStartExpression).UseVisualStyleBackColor = true;
		((Control)buttonRangeStartExpression).Click += ButtonRangeExpression_Click;
		((Control)label1).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)label1).Location = new Point(346, 29);
		((Control)label1).Margin = new Padding(4, 0, 4, 0);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(26, 38);
		((Control)label1).TabIndex = 16;
		((Control)label1).Text = "-";
		label1.TextAlign = (ContentAlignment)32;
		((Control)colorBoxRangeColor).AccessibleRole = (AccessibleRole)43;
		colorBoxRangeColor.AutoExpandOnClick = true;
		colorBoxRangeColor.ColorTable = eButtonColor.OrangeWithBackground;
		colorBoxRangeColor.EnableMarkup = false;
		((Control)colorBoxRangeColor).Location = new Point(180, 88);
		colorBoxRangeColor.LockClosePopup = false;
		((Control)colorBoxRangeColor).Margin = new Padding(6);
		((Control)colorBoxRangeColor).Name = "colorBoxRangeColor";
		((Control)colorBoxRangeColor).Size = new Size(268, 46);
		((Control)colorBoxRangeColor).TabIndex = 2;
		((Control)colorBoxRangeColor).TabStop = false;
		colorBoxRangeColor.TextAlignment = eButtonTextAlignment.Right;
		colorBoxRangeColor.SelectedColorChanged += ColorBoxRangeColor_SelectedColorChanged;
		((Control)panelRangeUi).BackColor = Color.White;
		((Control)panelRangeUi).Controls.Add((Control)(object)stiGroupLine2);
		((Control)panelRangeUi).Controls.Add((Control)(object)comboBoxRangeMode);
		((Control)panelRangeUi).Controls.Add((Control)(object)buttonAddRange);
		((Control)panelRangeUi).Dock = (DockStyle)1;
		((Control)panelRangeUi).Location = new Point(24, 1029);
		((Control)panelRangeUi).Margin = new Padding(4);
		((Control)panelRangeUi).Name = "panelRangeUi";
		((Control)panelRangeUi).Size = new Size(534, 108);
		((Control)panelRangeUi).TabIndex = 10;
		((Control)panelRangeUi).Visible = false;
		((Control)stiGroupLine2).BackColor = Color.White;
		stiGroupLine2.LinePosition = StiLinePosition.Bottom;
		stiGroupLine2.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine2).Location = new Point(0, 12);
		((Control)stiGroupLine2).Margin = new Padding(4);
		((Control)stiGroupLine2).Name = "stiGroupLine2";
		((Control)stiGroupLine2).Size = new Size(528, 10);
		((Control)stiGroupLine2).TabIndex = 0;
		comboBoxRangeMode.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxRangeMode).FormattingEnabled = true;
		((Control)comboBoxRangeMode).Location = new Point(0, 46);
		((Control)comboBoxRangeMode).Margin = new Padding(4);
		((Control)comboBoxRangeMode).Name = "comboBoxRangeMode";
		((Control)comboBoxRangeMode).Size = new Size(218, 42);
		((Control)comboBoxRangeMode).TabIndex = 1;
		((ComboBox)comboBoxRangeMode).SelectedIndexChanged += ComboBoxRangeMode_SelectedIndexChanged;
		((Control)buttonAddRange).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonAddRange).FlatAppearance.BorderSize = 0;
		((Control)buttonAddRange).Location = new Point(312, 46);
		((Control)buttonAddRange).Margin = new Padding(4);
		((Control)buttonAddRange).Name = "buttonAddRange";
		((Control)buttonAddRange).Size = new Size(222, 40);
		((Control)buttonAddRange).TabIndex = 2;
		((Control)buttonAddRange).Text = "Add Range";
		((ButtonBase)buttonAddRange).UseVisualStyleBackColor = true;
		((Control)buttonAddRange).Click += ButtonAddRange_Click;
		((Control)panelRangeType).BackColor = Color.White;
		((Control)panelRangeType).Controls.Add((Control)(object)comboBoxRangeType);
		((Control)panelRangeType).Controls.Add((Control)(object)labelRangeType);
		((Control)panelRangeType).Dock = (DockStyle)1;
		((Control)panelRangeType).Location = new Point(24, 956);
		((Control)panelRangeType).Margin = new Padding(6);
		((Control)panelRangeType).Name = "panelRangeType";
		((Control)panelRangeType).Size = new Size(534, 73);
		((Control)panelRangeType).TabIndex = 9;
		((Control)comboBoxRangeType).Anchor = (AnchorStyles)13;
		comboBoxRangeType.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxRangeType).FormattingEnabled = true;
		((Control)comboBoxRangeType).Location = new Point(180, 23);
		((Control)comboBoxRangeType).Margin = new Padding(4);
		((Control)comboBoxRangeType).Name = "comboBoxRangeType";
		((Control)comboBoxRangeType).Size = new Size(350, 42);
		((Control)comboBoxRangeType).TabIndex = 1;
		((ComboBox)comboBoxRangeType).SelectedIndexChanged += ComboBoxRangeType_SelectedIndexChanged;
		((Control)labelRangeType).ForeColor = Color.DimGray;
		((Control)labelRangeType).Location = new Point(0, 23);
		((Control)labelRangeType).Margin = new Padding(4, 0, 4, 0);
		((Control)labelRangeType).Name = "labelRangeType";
		((Control)labelRangeType).Size = new Size(170, 38);
		((Control)labelRangeType).TabIndex = 0;
		((Control)labelRangeType).Text = "Range Type";
		labelRangeType.TextAlign = (ContentAlignment)16;
		((Control)panelUi).BackColor = Color.White;
		((Control)panelUi).Controls.Add((Control)(object)labelFromTo);
		((Control)panelUi).Controls.Add((Control)(object)numericMaximum);
		((Control)panelUi).Controls.Add((Control)(object)numericMinimum);
		((Control)panelUi).Controls.Add((Control)(object)labelMaximum);
		((Control)panelUi).Controls.Add((Control)(object)labelMinimum);
		((Control)panelUi).Controls.Add((Control)(object)labelCalculationMode);
		((Control)panelUi).Controls.Add((Control)(object)comboBoxCalculationMode);
		((Control)panelUi).Dock = (DockStyle)1;
		((Control)panelUi).Location = new Point(24, 783);
		((Control)panelUi).Margin = new Padding(4);
		((Control)panelUi).Name = "panelUi";
		((Control)panelUi).Size = new Size(534, 173);
		((Control)panelUi).TabIndex = 8;
		((Control)labelFromTo).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)labelFromTo).Location = new Point(344, 121);
		((Control)labelFromTo).Margin = new Padding(4, 0, 4, 0);
		((Control)labelFromTo).Name = "labelFromTo";
		((Control)labelFromTo).Size = new Size(24, 38);
		((Control)labelFromTo).TabIndex = 6;
		((Control)labelFromTo).Text = "-";
		labelFromTo.TextAlign = (ContentAlignment)32;
		((Control)numericMaximum).BackColor = Color.White;
		numericMaximum.DecimalPlaces = 2;
		((Control)numericMaximum).Location = new Point(374, 123);
		((Control)numericMaximum).Margin = new Padding(4);
		((Control)numericMaximum).Name = "numericMaximum";
		((Control)numericMaximum).Size = new Size(158, 42);
		((Control)numericMaximum).TabIndex = 5;
		((Control)numericMaximum).Text = "0,00";
		numericMaximum.ValueChanged += NumericMaximum_ValueChanged;
		((Control)numericMinimum).BackColor = Color.White;
		numericMinimum.DecimalPlaces = 2;
		((Control)numericMinimum).Location = new Point(180, 123);
		((Control)numericMinimum).Margin = new Padding(4);
		((Control)numericMinimum).Name = "numericMinimum";
		((Control)numericMinimum).Size = new Size(158, 42);
		((Control)numericMinimum).TabIndex = 4;
		((Control)numericMinimum).Text = "0,00";
		numericMinimum.ValueChanged += NumericMinimum_ValueChanged;
		((Control)labelMaximum).ForeColor = Color.DimGray;
		((Control)labelMaximum).Location = new Point(366, 81);
		((Control)labelMaximum).Margin = new Padding(4, 0, 4, 0);
		((Control)labelMaximum).Name = "labelMaximum";
		((Control)labelMaximum).Size = new Size(166, 33);
		((Control)labelMaximum).TabIndex = 3;
		((Control)labelMaximum).Text = "Maximum";
		labelMaximum.TextAlign = (ContentAlignment)16;
		((Control)labelMinimum).ForeColor = Color.DimGray;
		((Control)labelMinimum).Location = new Point(172, 79);
		((Control)labelMinimum).Margin = new Padding(4, 0, 4, 0);
		((Control)labelMinimum).Name = "labelMinimum";
		((Control)labelMinimum).Size = new Size(166, 33);
		((Control)labelMinimum).TabIndex = 2;
		((Control)labelMinimum).Text = "Minimum";
		labelMinimum.TextAlign = (ContentAlignment)16;
		((Control)labelCalculationMode).ForeColor = Color.DimGray;
		((Control)labelCalculationMode).Location = new Point(0, 23);
		((Control)labelCalculationMode).Margin = new Padding(4, 0, 4, 0);
		((Control)labelCalculationMode).Name = "labelCalculationMode";
		((Control)labelCalculationMode).Size = new Size(170, 38);
		((Control)labelCalculationMode).TabIndex = 1;
		((Control)labelCalculationMode).Text = "Mode";
		labelCalculationMode.TextAlign = (ContentAlignment)16;
		((Control)comboBoxCalculationMode).Anchor = (AnchorStyles)13;
		comboBoxCalculationMode.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxCalculationMode).FormattingEnabled = true;
		((Control)comboBoxCalculationMode).Location = new Point(180, 23);
		((Control)comboBoxCalculationMode).Margin = new Padding(4);
		((Control)comboBoxCalculationMode).Name = "comboBoxCalculationMode";
		((Control)comboBoxCalculationMode).Size = new Size(350, 42);
		((Control)comboBoxCalculationMode).TabIndex = 1;
		((ComboBox)comboBoxCalculationMode).SelectedIndexChanged += ComboBoxCalculationMode_SelectedIndexChanged;
		((Control)panelType).BackColor = Color.White;
		((Control)panelType).Controls.Add((Control)(object)labelType);
		((Control)panelType).Controls.Add((Control)(object)buttonBullet);
		((Control)panelType).Controls.Add((Control)(object)buttonHorizontalLinear);
		((Control)panelType).Controls.Add((Control)(object)buttonVerticalLinear);
		((Control)panelType).Controls.Add((Control)(object)buttonHalfCircular);
		((Control)panelType).Controls.Add((Control)(object)buttonFullCircular);
		((Control)panelType).Dock = (DockStyle)1;
		((Control)panelType).Location = new Point(24, 710);
		((Control)panelType).Margin = new Padding(4);
		((Control)panelType).Name = "panelType";
		((Control)panelType).Size = new Size(534, 73);
		((Control)panelType).TabIndex = 7;
		((Control)labelType).ForeColor = Color.DimGray;
		((Control)labelType).Location = new Point(0, 23);
		((Control)labelType).Margin = new Padding(4, 0, 4, 0);
		((Control)labelType).Name = "labelType";
		((Control)labelType).Size = new Size(164, 48);
		((Control)labelType).TabIndex = 1;
		((Control)labelType).Text = "Type";
		labelType.TextAlign = (ContentAlignment)16;
		((RadioButton)buttonBullet).Appearance = (Appearance)1;
		((ButtonBase)buttonBullet).FlatAppearance.BorderSize = 0;
		((ButtonBase)buttonBullet).Image = (Image)componentResourceManager.GetObject("buttonBullet.Image");
		((Control)buttonBullet).Location = new Point(418, 23);
		((Control)buttonBullet).Margin = new Padding(4);
		((Control)buttonBullet).Name = "buttonBullet";
		((Control)buttonBullet).Size = new Size(52, 50);
		((Control)buttonBullet).TabIndex = 1;
		((ButtonBase)buttonBullet).UseVisualStyleBackColor = true;
		((RadioButton)buttonBullet).CheckedChanged += ButtonGaugeType_CheckedChanged;
		((RadioButton)buttonHorizontalLinear).Appearance = (Appearance)1;
		((ButtonBase)buttonHorizontalLinear).FlatAppearance.BorderSize = 0;
		((ButtonBase)buttonHorizontalLinear).Image = (Image)componentResourceManager.GetObject("buttonHorizontalLinear.Image");
		((Control)buttonHorizontalLinear).Location = new Point(358, 23);
		((Control)buttonHorizontalLinear).Margin = new Padding(4);
		((Control)buttonHorizontalLinear).Name = "buttonHorizontalLinear";
		((Control)buttonHorizontalLinear).Size = new Size(52, 50);
		((Control)buttonHorizontalLinear).TabIndex = 1;
		((ButtonBase)buttonHorizontalLinear).UseVisualStyleBackColor = true;
		((RadioButton)buttonHorizontalLinear).CheckedChanged += ButtonGaugeType_CheckedChanged;
		((RadioButton)buttonVerticalLinear).Appearance = (Appearance)1;
		((ButtonBase)buttonVerticalLinear).FlatAppearance.BorderSize = 0;
		((ButtonBase)buttonVerticalLinear).Image = (Image)componentResourceManager.GetObject("buttonVerticalLinear.Image");
		((Control)buttonVerticalLinear).Location = new Point(298, 23);
		((Control)buttonVerticalLinear).Margin = new Padding(4);
		((Control)buttonVerticalLinear).Name = "buttonVerticalLinear";
		((Control)buttonVerticalLinear).Size = new Size(52, 50);
		((Control)buttonVerticalLinear).TabIndex = 1;
		((ButtonBase)buttonVerticalLinear).UseVisualStyleBackColor = true;
		((RadioButton)buttonVerticalLinear).CheckedChanged += ButtonGaugeType_CheckedChanged;
		((RadioButton)buttonHalfCircular).Appearance = (Appearance)1;
		((ButtonBase)buttonHalfCircular).FlatAppearance.BorderSize = 0;
		((ButtonBase)buttonHalfCircular).Image = (Image)componentResourceManager.GetObject("buttonHalfCircular.Image");
		((Control)buttonHalfCircular).Location = new Point(238, 23);
		((Control)buttonHalfCircular).Margin = new Padding(4);
		((Control)buttonHalfCircular).Name = "buttonHalfCircular";
		((Control)buttonHalfCircular).Size = new Size(52, 50);
		((Control)buttonHalfCircular).TabIndex = 1;
		((ButtonBase)buttonHalfCircular).UseVisualStyleBackColor = true;
		((RadioButton)buttonHalfCircular).CheckedChanged += ButtonGaugeType_CheckedChanged;
		((RadioButton)buttonFullCircular).Appearance = (Appearance)1;
		((RadioButton)buttonFullCircular).Checked = true;
		((ButtonBase)buttonFullCircular).FlatAppearance.BorderSize = 0;
		((ButtonBase)buttonFullCircular).Image = (Image)componentResourceManager.GetObject("buttonFullCircular.Image");
		((Control)buttonFullCircular).Location = new Point(178, 23);
		((Control)buttonFullCircular).Margin = new Padding(4);
		((Control)buttonFullCircular).Name = "buttonFullCircular";
		((Control)buttonFullCircular).Size = new Size(52, 50);
		((Control)buttonFullCircular).TabIndex = 0;
		((RadioButton)buttonFullCircular).TabStop = true;
		((ButtonBase)buttonFullCircular).UseVisualStyleBackColor = true;
		((RadioButton)buttonFullCircular).CheckedChanged += ButtonGaugeType_CheckedChanged;
		((Control)panelExpression).Controls.Add((Control)(object)panelBoxExpression);
		((Control)panelExpression).Controls.Add((Control)(object)labelExpression);
		((Control)panelExpression).Dock = (DockStyle)1;
		((Control)panelExpression).Location = new Point(24, 597);
		((Control)panelExpression).Margin = new Padding(6);
		((Control)panelExpression).Name = "panelExpression";
		((Control)panelExpression).Size = new Size(534, 113);
		((Control)panelExpression).TabIndex = 6;
		((Control)panelBoxExpression).BackColor = Color.White;
		((Control)panelBoxExpression).Controls.Add((Control)(object)buttonExpressionEdit);
		((Control)panelBoxExpression).Controls.Add((Control)(object)textBoxExpression);
		((Control)panelBoxExpression).Controls.Add((Control)(object)buttonExpressionMenu);
		((Control)panelBoxExpression).Dock = (DockStyle)1;
		((Control)panelBoxExpression).Location = new Point(0, 62);
		((Control)panelBoxExpression).Margin = new Padding(4);
		((Control)panelBoxExpression).Name = "panelBoxExpression";
		((Control)panelBoxExpression).Size = new Size(534, 48);
		((Control)panelBoxExpression).TabIndex = 0;
		((Control)buttonExpressionEdit).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonExpressionEdit).Image = (Image)componentResourceManager.GetObject("buttonExpressionEdit.Image");
		((Control)buttonExpressionEdit).Location = new Point(442, 2);
		((Control)buttonExpressionEdit).Margin = new Padding(4);
		((Control)buttonExpressionEdit).Name = "buttonExpressionEdit";
		((Control)buttonExpressionEdit).Size = new Size(46, 44);
		((Control)buttonExpressionEdit).TabIndex = 1;
		((Control)buttonExpressionEdit).TabStop = false;
		((ButtonBase)buttonExpressionEdit).UseVisualStyleBackColor = true;
		((Control)buttonExpressionEdit).Click += ButtonExpressionEdit_Click;
		((Control)textBoxExpression).Anchor = (AnchorStyles)13;
		((Control)textBoxExpression).BackColor = SystemColors.Window;
		((TextBoxBase)textBoxExpression).BorderStyle = (BorderStyle)0;
		((Control)textBoxExpression).Location = new Point(8, 12);
		((Control)textBoxExpression).Margin = new Padding(4);
		((Control)textBoxExpression).Name = "textBoxExpression";
		((Control)textBoxExpression).Size = new Size(426, 24);
		((Control)textBoxExpression).TabIndex = 0;
		((Control)textBoxExpression).TextChanged += TextBoxExpression_TextChanged;
		((Control)textBoxExpression).Enter += TextBoxExpression_Leave;
		((Control)textBoxExpression).KeyDown += new KeyEventHandler(TextBoxExpression_KeyDown);
		((Control)buttonExpressionMenu).Anchor = (AnchorStyles)9;
		((Control)buttonExpressionMenu).Location = new Point(486, 2);
		((Control)buttonExpressionMenu).Margin = new Padding(4);
		((Control)buttonExpressionMenu).Name = "buttonExpressionMenu";
		((Control)buttonExpressionMenu).Size = new Size(46, 44);
		((Control)buttonExpressionMenu).TabIndex = 2;
		((Control)buttonExpressionMenu).TabStop = false;
		((ButtonBase)buttonExpressionMenu).UseVisualStyleBackColor = true;
		((Control)buttonExpressionMenu).Click += ButtonExpressionMenu_Click;
		((Control)labelExpression).Dock = (DockStyle)1;
		((Control)labelExpression).ForeColor = Color.DimGray;
		((Control)labelExpression).Location = new Point(0, 0);
		((Control)labelExpression).Margin = new Padding(4, 0, 4, 0);
		((Control)labelExpression).Name = "labelExpression";
		((Control)labelExpression).Size = new Size(534, 62);
		((Control)labelExpression).TabIndex = 8;
		((Control)labelExpression).Text = "Expression";
		labelExpression.TextAlign = (ContentAlignment)16;
		contextMenuBar.AntiAlias = true;
		contextMenuBar.DockTabStripHeight = 50;
		((Control)contextMenuBar).Font = new Font("Segoe UI", 9f);
		contextMenuBar.Items.AddRange(new BaseItem[1] { menuExpression });
		contextMenuBar.Location = new Point(394, 1146);
		((Control)contextMenuBar).Margin = new Padding(4);
		contextMenuBar.Name = "contextMenuBar";
		contextMenuBar.Size = new Size(160, 47);
		contextMenuBar.Stretch = true;
		contextMenuBar.Style = eDotNetBarStyle.StyleManagerControlled;
		contextMenuBar.TabIndex = 12;
		contextMenuBar.TabStop = false;
		((Control)contextMenuBar).Text = "contextMenuBar";
		contextMenuBar.WrapItemsDock = true;
		menuExpression.AutoExpandOnClick = true;
		menuExpression.ImagePaddingHorizontal = 16;
		menuExpression.ImagePaddingVertical = 12;
		menuExpression.Name = "menuExpression";
		menuExpression.Text = "menuExpression";
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
		((Control)gridData).Size = new Size(534, 40);
		((Control)gridData).TabIndex = 21;
		((Control)listBoxRanges).AllowDrop = true;
		((Control)listBoxRanges).Dock = (DockStyle)1;
		((Control)listBoxRanges).Location = new Point(24, 1137);
		((Control)listBoxRanges).Margin = new Padding(4);
		((Control)listBoxRanges).Name = "listBoxRanges";
		((Control)listBoxRanges).Size = new Size(534, 138);
		((Control)listBoxRanges).TabIndex = 11;
		((ListBox)listBoxRanges).SelectedIndexChanged += ListBoxRanges_SelectedIndexChanged;
		((Control)listBoxRanges).Enter += ListBoxRanges_Enter;
		((Control)listBoxRanges).MouseDown += new MouseEventHandler(ListBoxRanges_MouseDown);
		((Control)listBoxSeries).AllowDrop = true;
		((Control)listBoxSeries).Dock = (DockStyle)1;
		((Control)listBoxSeries).Location = new Point(24, 501);
		((Control)listBoxSeries).Margin = new Padding(4);
		((Control)listBoxSeries).Name = "listBoxSeries";
		((Control)listBoxSeries).Size = new Size(534, 96);
		((Control)listBoxSeries).TabIndex = 5;
		((ListBox)listBoxSeries).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxSeries).Enter += ListBox_Enter;
		((Control)listBoxSeries).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)labelSeries).Dock = (DockStyle)1;
		((Control)labelSeries).Location = new Point(24, 439);
		((Control)labelSeries).Margin = new Padding(4, 0, 4, 0);
		((Control)labelSeries).Name = "labelSeries";
		((Control)labelSeries).Size = new Size(534, 62);
		((Control)labelSeries).TabIndex = 4;
		((Control)labelSeries).Text = "Series";
		((Control)listBoxTarget).AllowDrop = true;
		((Control)listBoxTarget).Dock = (DockStyle)1;
		((Control)listBoxTarget).Location = new Point(24, 343);
		((Control)listBoxTarget).Margin = new Padding(4);
		((Control)listBoxTarget).Name = "listBoxTarget";
		((Control)listBoxTarget).Size = new Size(534, 96);
		((Control)listBoxTarget).TabIndex = 3;
		((ListBox)listBoxTarget).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxTarget).Enter += ListBox_Enter;
		((Control)listBoxTarget).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)labelTarget).Dock = (DockStyle)1;
		((Control)labelTarget).Location = new Point(24, 281);
		((Control)labelTarget).Margin = new Padding(4, 0, 4, 0);
		((Control)labelTarget).Name = "labelTarget";
		((Control)labelTarget).Size = new Size(534, 62);
		((Control)labelTarget).TabIndex = 2;
		((Control)labelTarget).Text = "Target";
		((Control)listBoxValues).AllowDrop = true;
		((Control)listBoxValues).Dock = (DockStyle)1;
		((Control)listBoxValues).Location = new Point(24, 185);
		((Control)listBoxValues).Margin = new Padding(4);
		((Control)listBoxValues).Name = "listBoxValues";
		((Control)listBoxValues).Size = new Size(534, 96);
		((Control)listBoxValues).TabIndex = 1;
		((ListBox)listBoxValues).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxValues).Enter += ListBox_Enter;
		((Control)listBoxValues).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)labelValue).Controls.Add((Control)(object)buttonEnterDataManually);
		((Control)labelValue).Dock = (DockStyle)1;
		((Control)labelValue).Location = new Point(24, 124);
		((Control)labelValue).Margin = new Padding(4, 0, 4, 0);
		((Control)labelValue).Name = "labelValue";
		((Control)labelValue).Size = new Size(534, 61);
		((Control)labelValue).TabIndex = 20;
		((Control)labelValue).Text = "Value";
		labelValue.TextAlign = (ContentAlignment)16;
		((Control)buttonEnterDataManually).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonEnterDataManually).Image = (Image)componentResourceManager.GetObject("buttonEnterDataManually.Image");
		((Control)buttonEnterDataManually).Location = new Point(487, 8);
		((Control)buttonEnterDataManually).Margin = new Padding(4);
		((Control)buttonEnterDataManually).Name = "buttonEnterDataManually";
		((Control)buttonEnterDataManually).Size = new Size(48, 44);
		((Control)buttonEnterDataManually).TabIndex = 18;
		((Control)buttonEnterDataManually).TabStop = false;
		((ButtonBase)buttonEnterDataManually).UseVisualStyleBackColor = true;
		((Control)buttonEnterDataManually).Click += ButtonEnterDataManually_Click;
		((Control)labelGridData).Controls.Add((Control)(object)buttonUseDataFields);
		((Control)labelGridData).Dock = (DockStyle)1;
		((Control)labelGridData).Location = new Point(24, 23);
		((Control)labelGridData).Margin = new Padding(4, 0, 4, 0);
		((Control)labelGridData).Name = "labelGridData";
		((Control)labelGridData).Size = new Size(534, 61);
		((Control)labelGridData).TabIndex = 22;
		((Control)labelGridData).Text = "Data";
		labelGridData.TextAlign = (ContentAlignment)16;
		((Control)buttonUseDataFields).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonUseDataFields).Image = (Image)componentResourceManager.GetObject("buttonUseDataFields.Image");
		((Control)buttonUseDataFields).Location = new Point(487, 8);
		((Control)buttonUseDataFields).Margin = new Padding(4);
		((Control)buttonUseDataFields).Name = "buttonUseDataFields";
		((Control)buttonUseDataFields).Size = new Size(48, 44);
		((Control)buttonUseDataFields).TabIndex = 18;
		((Control)buttonUseDataFields).TabStop = false;
		((ButtonBase)buttonUseDataFields).UseVisualStyleBackColor = true;
		((Control)buttonUseDataFields).Click += ButtonUseDataFields_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(582, 1472);
		((Control)this).Controls.Add((Control)(object)panelRangeOption);
		((Control)this).Controls.Add((Control)(object)listBoxRanges);
		((Control)this).Controls.Add((Control)(object)panelRangeUi);
		((Control)this).Controls.Add((Control)(object)panelRangeType);
		((Control)this).Controls.Add((Control)(object)panelUi);
		((Control)this).Controls.Add((Control)(object)panelType);
		((Control)this).Controls.Add((Control)(object)panelExpression);
		((Control)this).Controls.Add((Control)(object)contextMenuBar);
		((Control)this).Controls.Add((Control)(object)listBoxSeries);
		((Control)this).Controls.Add((Control)(object)labelSeries);
		((Control)this).Controls.Add((Control)(object)listBoxTarget);
		((Control)this).Controls.Add((Control)(object)labelTarget);
		((Control)this).Controls.Add((Control)(object)listBoxValues);
		((Control)this).Controls.Add((Control)(object)labelValue);
		((Control)this).Controls.Add((Control)(object)gridData);
		((Control)this).Controls.Add((Control)(object)labelGridData);
		((Form)this).Margin = new Padding(4);
		((Control)this).Name = "StiGaugeElementEditorForm";
		((Control)this).Padding = new Padding(24, 23, 24, 23);
		((Control)this).Text = "Gauge";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Control)panelRangeOption).ResumeLayout(false);
		((Control)panelBoxRangeEndExpression).ResumeLayout(false);
		((Control)panelBoxRangeEndExpression).PerformLayout();
		((Control)panelBoxRangeStartExpression).ResumeLayout(false);
		((Control)panelBoxRangeStartExpression).PerformLayout();
		((Control)panelRangeUi).ResumeLayout(false);
		((Control)panelRangeType).ResumeLayout(false);
		((Control)panelUi).ResumeLayout(false);
		((ISupportInitialize)numericMaximum).EndInit();
		((ISupportInitialize)numericMinimum).EndInit();
		((Control)panelType).ResumeLayout(false);
		((Control)panelExpression).ResumeLayout(false);
		((Control)panelBoxExpression).ResumeLayout(false);
		((Control)panelBoxExpression).PerformLayout();
		((ISupportInitialize)contextMenuBar).EndInit();
		((ISupportInitialize)gridData).EndInit();
		((Control)labelValue).ResumeLayout(false);
		((Control)labelGridData).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
