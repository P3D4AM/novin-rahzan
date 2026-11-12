using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Components.Indicator;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Dashboard.Design.Controls;
using Stimulsoft.Dashboard.Design.Editors.Helpers;
using Stimulsoft.Dashboard.Design.Editors.Menu;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Components.FontIcons;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Editors;
using Stimulsoft.Report.Design.Helpers;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Helper;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Dashboard.Design.Editors;

public class StiIndicatorElementEditorForm : StiElementEditorForm
{
	private const string ColumnValue = "Value";

	private const string ColumnTarget = "Target";

	private const string ColumnSeries = "Series";

	private Stimulsoft.Dashboard.Design.Controls.StiMeterListBox selectedListBox;

	private StiPopup iconSetsPopup;

	private Timer timerRefreshGrid;

	private IContainer components;

	private Stimulsoft.Dashboard.Design.Controls.StiMeterListBox listBoxValues;

	private ToolTip toolTip;

	private StiLabelPanel labelValue;

	private Stimulsoft.Dashboard.Design.Controls.StiMeterListBox listBoxSeries;

	private StiChartGroupLabel labelSeries;

	private Stimulsoft.Dashboard.Design.Controls.StiMeterListBox listBoxTargets;

	private StiChartGroupLabel labelTarget;

	private StiPanelBox panelBoxExpression;

	private HintTextBox textBoxExpression;

	private StiOperationButton buttonExpressionMenu;

	private StiLabelUX labelExpression;

	private ContextMenuBar contextMenuBar;

	private ButtonItem menuExpression;

	private StiLabelUX labelIcon;

	private StiPanelUX panelIcon;

	private StiPanelUX panelIconSet;

	private StiLabelUX labelIconSet;

	private StiPanelUX panelBoxIconSet;

	private StiOperationButton buttonIconSet;

	private StiPanelUX panelCustomIcon;

	private StiPanelUX panelImage;

	private StiOperationButton buttonOpen;

	private StiOperationButton buttonRemove;

	private StiOperationButton buttonExpressionEdit;

	private StiFontIconSelectionButton fontIconSelectionButton;

	private StiPanelUX panelModeUi;

	private StiLabelUX labelCalculationMode;

	private StiComboBox comboBoxCalculationMode;

	private StiIndicatorIconRangeListBox listBoxRanges;

	private StiPanelUX panelRangeOption;

	private StiPanelBox panelBoxRangeEndExpression;

	private HintTextBox textBoxRangeEndExpression;

	private StiOperationButton buttonRangeEndExpression;

	private StiPanelBox panelBoxRangeStartExpression;

	private HintTextBox textBoxRangeStartExpression;

	private StiOperationButton buttonRangeStartExpression;

	private StiLabelUX label1;

	private StiFontIconSelectionButton fontIconSelectionRangeIcon;

	private StiPanelUX panelRangeUi;

	private StiButtonUX buttonAddRange;

	private StiComboBox comboBoxRangeMode;

	private StiExcelGrid gridData;

	private StiOperationButton buttonEnterDataManually;

	private StiLabelPanel labelGridData;

	private StiOperationButton buttonUseDataFields;

	internal StiIndicatorElement IndicatorElement => base.Element as StiIndicatorElement;

	internal StiValueIndicatorMeter IndicatorValue => IndicatorElement?.Value;

	internal StiTargetIndicatorMeter IndicatorTarget => IndicatorElement?.Target;

	internal StiSeriesIndicatorMeter IndicatorSeries => IndicatorElement?.Series;

	internal List<StiIndicatorIconRange> IconRanges => IndicatorElement?.IconRanges;

	internal StiIndicatorIconRange IconRange => listBoxRanges.SelectedRangeItem?.Range;

	internal bool IsValuePresent
	{
		get
		{
			if (IsDataFieldsMode)
			{
				return ((ListBox)listBoxValues).Items.Count > 0;
			}
			if (gridData.DataTable != null)
			{
				return gridData.DataTable.Rows.Cast<DataRow>().Any((DataRow r) => !string.IsNullOrWhiteSpace(r["Value"]?.ToString()));
			}
			return false;
		}
	}

	internal bool IsTargetPresent
	{
		get
		{
			if (IsDataFieldsMode)
			{
				return ((ListBox)listBoxTargets).Items.Count > 0;
			}
			if (gridData.DataTable != null)
			{
				return gridData.DataTable.Rows.Cast<DataRow>().Any((DataRow r) => !string.IsNullOrWhiteSpace(r["Target"]?.ToString()));
			}
			return false;
		}
	}

	internal bool IsSeriesPresent
	{
		get
		{
			if (IsDataFieldsMode)
			{
				return ((ListBox)listBoxSeries).Items.Count > 0;
			}
			if (gridData.DataTable != null)
			{
				return (from DataRow r in gridData.DataTable.Rows
					select r["Series"]?.ToString() into c
					where !string.IsNullOrEmpty(c)
					select c).Distinct().Count() > 1;
			}
			return false;
		}
	}

	public override bool IsAnyPresent
	{
		get
		{
			if (!IsValuePresent && !IsTargetPresent && !IsSeriesPresent && !IsShowMore)
			{
				return IsDataEnterMode;
			}
			return true;
		}
	}

	public override bool AllowResize => false;

	internal Stimulsoft.Dashboard.Design.Controls.StiMeterListBox ValuesListBox => listBoxValues;

	internal Stimulsoft.Dashboard.Design.Controls.StiMeterListBox TargetsListBox => listBoxTargets;

	internal Stimulsoft.Dashboard.Design.Controls.StiMeterListBox SeriesListBox => listBoxSeries;

	internal bool UseCustomIcon => IndicatorElement.CustomIcon != null;

	private bool IsDataEnterMode => IndicatorElement.DataMode == StiDataMode.ManuallyEnteringData;

	private bool IsDataFieldsMode => IndicatorElement.DataMode == StiDataMode.UsingDataFields;

	private bool IsShowMore { get; set; }

	private string LocCalculationModeCustom => Loc.GetEnum("StiGaugeCalculationModeCustom");

	private string LocCalculationModeAuto => Loc.GetEnum("StiGaugeCalculationModeAuto");

	private string LocRangeModePercentage => Loc.GetEnum("StiGaugeRangeModePercentage");

	private string LocRangeModeValue => Loc.GetEnum("StiGaugeRangeModeValue");

	public StiIndicatorElementEditorForm()
		: this(null)
	{
	}

	public StiIndicatorElementEditorForm(StiElement element)
		: base(element)
	{
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Expected O, but got Unknown
		InitializeComponent();
		if (StiOptions.Appearance.ShowFormsHelp)
		{
			((Form)this).HelpButton = true;
		}
		selectedListBox = listBoxValues;
		foreach (Stimulsoft.Dashboard.Design.Controls.StiMeterListBox item in (from Control c in (IEnumerable)((Control)this).Controls
			where c is Stimulsoft.Dashboard.Design.Controls.StiMeterListBox
			select c).Cast<Stimulsoft.Dashboard.Design.Controls.StiMeterListBox>())
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
		CreateIconControls();
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
		((Control)listBoxTargets).Invalidate();
		((Control)listBoxSeries).Invalidate();
		((Control)gridData).Invalidate();
	}

	private void LocalizeInternal()
	{
		((Control)this).Text = Loc.Get("Components", "StiIndicator");
		((Control)labelValue).Text = Loc.GetMain("Value");
		((Control)labelTarget).Text = Loc.GetMain("Target");
		((Control)labelSeries).Text = Loc.Get("Chart", "Series");
		((Control)labelIcon).Text = Loc.GetMain("Icon");
		((Control)labelIconSet).Text = Loc.GetMain("IconSet");
		((Control)labelExpression).Text = Loc.GetMain("Expression");
		((Control)labelCalculationMode).Text = Loc.GetMain("Mode");
		((Control)labelGridData).Text = Loc.GetMain("Data");
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
		toolTip.SetToolTip((Control)(object)buttonOpen, Loc.Get("Buttons", "Open"));
		toolTip.SetToolTip((Control)(object)buttonRemove, Loc.Get("Buttons", "Remove"));
		toolTip.SetToolTip((Control)(object)buttonUseDataFields, Loc.Get("Report", "UseDataFields"));
		toolTip.SetToolTip((Control)(object)buttonEnterDataManually, Loc.Get("Report", "EnterDataManually"));
		StiExcelGridLocalizer.Localize(gridData);
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
			new StiIndicatorExpressionMenu(menuExpression, (TextBox)(object)textBoxExpression, this, selectedListBox).CreateItems();
		};
	}

	private void DoScaling()
	{
		((ButtonBase)buttonOpen).Image = StiReportDesignImages.Actions.OpenFile();
		((ButtonBase)buttonRemove).Image = StiReportDesignImages.Actions.Remove();
		((ButtonBase)buttonExpressionEdit).Image = StiReportDesignImages.Actions.Edit();
		((ButtonBase)buttonEnterDataManually).Image = StiReportDesignImages.Actions.EditTable();
		((ButtonBase)buttonUseDataFields).Image = StiReportDesignImages.Actions.RetrieveColumnsDown();
		StiButtonsLocator.ArrangeButtons((Control)(object)panelBoxExpression);
		((Control)buttonAddRange).Height = ((Control)comboBoxRangeMode).Height + 2;
		((Control)buttonAddRange).Top = ((Control)comboBoxRangeMode).Top - 1;
		((Control)comboBoxCalculationMode).Width = ((Control)panelBoxIconSet).Width;
	}

	private void CreateIconControls()
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		fontIconSelectionButton.Icon = IndicatorElement.Icon;
		fontIconSelectionButton.IconColor = StiDashboardStyleHelper.GetGlyphColor(IndicatorElement);
		iconSetsPopup = new StiPopup((Control)(object)GetIconSetPanel());
		((Control)panelBoxIconSet).Paint += new PaintEventHandler(PanelBoxIconSet_Paint);
	}

	private void RefreshImageAndStatus()
	{
		((Control)panelImage).Invalidate();
		RefreshElementView();
		CheckStatus();
	}

	private void CheckStatus()
	{
		((Control)buttonRemove).Enabled = IndicatorElement.CustomIcon != null;
	}

	public override void ReadPropertiesFromElement()
	{
		ReadValuesPropertiesFromElement();
		ReadTargetPropertiesFromElement();
		ReadSeriesPropertiesFromElement();
		ReadRangesPropertiesFromElement();
		CheckStatus();
	}

	private void ReadValuesPropertiesFromElement()
	{
		if (!listBoxValues.ItemsEqualTo(IndicatorValue))
		{
			base.LockRefresh = true;
			((ListBox)listBoxValues).BeginUpdate();
			((ListBox)listBoxValues).Items.Clear();
			listBoxValues.AddItem(IndicatorValue);
			listBoxValues.SelectFirstItem();
			((ListBox)listBoxValues).EndUpdate();
			base.LockRefresh = false;
		}
	}

	private void ReadTargetPropertiesFromElement()
	{
		if (!listBoxTargets.ItemsEqualTo(IndicatorTarget))
		{
			((ListBox)listBoxTargets).BeginUpdate();
			((ListBox)listBoxTargets).Items.Clear();
			listBoxTargets.AddItem(IndicatorTarget);
			listBoxTargets.SelectFirstItem();
			((ListBox)listBoxTargets).EndUpdate();
		}
	}

	private void ReadSeriesPropertiesFromElement()
	{
		if (!listBoxSeries.ItemsEqualTo(IndicatorSeries))
		{
			((ListBox)listBoxSeries).BeginUpdate();
			((ListBox)listBoxSeries).Items.Clear();
			listBoxSeries.AddItem(IndicatorSeries);
			listBoxSeries.SelectFirstItem();
			((ListBox)listBoxSeries).EndUpdate();
		}
	}

	private void ReadRangesPropertiesFromElement()
	{
		((ListBox)listBoxRanges).BeginUpdate();
		((ListBox)listBoxRanges).Items.Clear();
		listBoxRanges.AddItems(IconRanges);
		listBoxRanges.SelectFirstItem();
		((ListBox)listBoxRanges).EndUpdate();
	}

	public override void WritePropertiesToElement()
	{
		WriteValuesPropertiesToElement();
		WriteTargetsPropertiesToElement();
		WriteSeriesPropertiesToElement();
		WriteRangesPropertiesToElement();
		RefreshElementView();
	}

	private void WriteValuesPropertiesToElement()
	{
		Stimulsoft.Dashboard.Design.Controls.StiMeterItem stiMeterItem = listBoxValues.FirstItem();
		IndicatorElement.Value = stiMeterItem?.Meter as StiValueIndicatorMeter;
	}

	private void WriteTargetsPropertiesToElement()
	{
		Stimulsoft.Dashboard.Design.Controls.StiMeterItem stiMeterItem = listBoxTargets.FirstItem();
		IndicatorElement.Target = stiMeterItem?.Meter as StiTargetIndicatorMeter;
	}

	private void WriteSeriesPropertiesToElement()
	{
		Stimulsoft.Dashboard.Design.Controls.StiMeterItem stiMeterItem = listBoxSeries.FirstItem();
		IndicatorElement.Series = stiMeterItem?.Meter as StiSeriesIndicatorMeter;
	}

	private void WriteRangesPropertiesToElement()
	{
		IndicatorElement.IconRanges = (from t in listBoxRanges.ToItems()
			select t.Range).ToList();
	}

	protected override void AttachEvents()
	{
		base.AttachEvents();
		AttachEventsToValues();
		AttachEventsToTargets();
		AttachEventsToSeries();
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
				listBoxValues.AddItem(StiMeterHelper.Indicator.GetValue(draggedDataColumnOrVariable));
				Designer.PageView.ResetDragging();
				IndicatorElement.Select();
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
				listBoxValues.AddItem(StiMeterHelper.Indicator.GetValue(stiTableColumn));
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
			IndicatorElement.Select();
			RefreshUiPanel();
		};
	}

	private void AttachEventsToTargets()
	{
		listBoxTargets.ElementsConverting += ListBox_ElementsConverting;
		listBoxTargets.ElementsChanged += delegate(object sender, StiElementChangedArgs changedArgs)
		{
			ProcessElementChanging(changedArgs);
			WritePropertiesToElement();
			RefreshUiPanel();
			Designer.AddSelectedToPropertyGrid();
		};
		listBoxTargets.ElementsDragEnter += delegate
		{
			ResetComponentHighlights();
		};
		listBoxTargets.ElementsDragDrop += delegate
		{
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(Designer))
			{
				IStiAppDataCell draggedDataColumnOrVariable = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
				if (draggedDataColumnOrVariable == null)
				{
					return;
				}
				((ListBox)listBoxTargets).Items.Clear();
				listBoxTargets.AddItem(StiMeterHelper.Indicator.GetTarget(draggedDataColumnOrVariable));
				Designer.PageView.ResetDragging();
				IndicatorElement.Select();
				WritePropertiesToElement();
				listBoxTargets.SelectedMeterItem = listBoxTargets.LastItem();
				((Control)listBoxTargets).Focus();
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
				((ListBox)listBoxTargets).Items.Clear();
				listBoxTargets.AddItem(StiMeterHelper.Indicator.GetValue(stiTableColumn));
				Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxTargets.SelectedMeterItem = listBoxTargets.LastItem();
				((Control)listBoxTargets).Focus();
			}
			ResetComponentHighlights();
		};
		listBoxTargets.ElementsSelectedChanged += delegate
		{
			IndicatorElement.Select();
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
			Designer.AddSelectedToPropertyGrid();
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
				listBoxSeries.AddItem(StiMeterHelper.Indicator.GetSeries(draggedDataColumnOrVariable));
				Designer.PageView.ResetDragging();
				IndicatorElement.Select();
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
				listBoxSeries.AddItem(StiMeterHelper.Indicator.GetSeries(stiTableColumn));
				Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxSeries.SelectedMeterItem = listBoxSeries.LastItem();
				((Control)listBoxSeries).Focus();
			}
			ResetComponentHighlights();
		};
		listBoxSeries.ElementsSelectedChanged += delegate
		{
			IndicatorElement.Select();
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
			IndicatorElement.Select();
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
			((ComboBox)comboBoxCalculationMode).SelectedItem = ((IndicatorElement.IconMode == StiIndicatorIconMode.Auto) ? LocCalculationModeAuto : LocCalculationModeCustom);
			((ComboBox)comboBoxRangeMode).SelectedItem = ((IndicatorElement.IconRangeMode == StiIndicatorIconRangeMode.Percentage) ? LocRangeModePercentage : LocRangeModeValue);
			if (IsValuePresent && IsSeriesPresent && !IsTargetPresent)
			{
				((Control)panelIcon).Visible = false;
				((Control)panelCustomIcon).Visible = false;
				((Control)panelModeUi).Visible = true;
				if (IndicatorElement.IconMode == StiIndicatorIconMode.Custom)
				{
					((Control)panelRangeUi).Visible = true;
					((Control)panelRangeOption).Visible = ((ListBox)listBoxRanges).Items.Count > 0;
					((Control)listBoxRanges).Visible = true;
					((Control)panelIconSet).Visible = false;
				}
				else
				{
					((Control)panelRangeOption).Visible = false;
					((Control)listBoxRanges).Visible = false;
					((Control)panelRangeUi).Visible = false;
					((Control)panelIconSet).Visible = true;
				}
			}
			else
			{
				((Control)panelIcon).Visible = IsValuePresent && !IsSeriesPresent && !UseCustomIcon;
				((Control)panelCustomIcon).Visible = IsValuePresent && !IsSeriesPresent && UseCustomIcon;
				((Control)panelIconSet).Visible = false;
				((Control)panelModeUi).Visible = false;
				((Control)panelRangeUi).Visible = false;
				((Control)listBoxRanges).Visible = false;
				((Control)panelRangeOption).Visible = false;
			}
			StiLabelPanel stiLabelPanel = labelValue;
			StiChartGroupLabel stiChartGroupLabel = labelTarget;
			StiChartGroupLabel stiChartGroupLabel2 = labelSeries;
			StiLabelUX stiLabelUX = labelExpression;
			StiPanelBox stiPanelBox = panelBoxExpression;
			Stimulsoft.Dashboard.Design.Controls.StiMeterListBox stiMeterListBox = listBoxTargets;
			Stimulsoft.Dashboard.Design.Controls.StiMeterListBox stiMeterListBox2 = listBoxSeries;
			bool flag2 = (((Control)buttonEnterDataManually).Visible = IsAnyPresent && IsDataFieldsMode);
			bool flag4 = (((Control)stiMeterListBox2).Visible = flag2);
			bool flag6 = (((Control)stiMeterListBox).Visible = flag4);
			bool flag8 = (((Control)stiPanelBox).Visible = flag6);
			bool flag10 = (((Control)stiLabelUX).Visible = flag8);
			bool flag12 = (((Control)stiChartGroupLabel2).Visible = flag10);
			bool visible = (((Control)stiChartGroupLabel).Visible = flag12);
			((Control)stiLabelPanel).Visible = visible;
			Stimulsoft.Dashboard.Design.Controls.StiMeterListBox stiMeterListBox3 = listBoxValues;
			visible = (listBoxValues.ShowEnterDataManuallyLabel = !IsAnyPresent);
			stiMeterListBox3.ShowShowMoreLabel = visible;
			((Control)listBoxValues).Visible = IsDataFieldsMode;
			StiLabelPanel stiLabelPanel2 = labelGridData;
			StiExcelGrid stiExcelGrid = gridData;
			flag12 = (((Control)buttonUseDataFields).Visible = IsDataEnterMode);
			visible = (((Control)stiExcelGrid).Visible = flag12);
			((Control)stiLabelPanel2).Visible = visible;
			((Control)listBoxValues).Dock = (DockStyle)(IsAnyPresent ? 1 : 5);
			SetListHeightFromCount(listBoxValues, multiple: false);
			SetListHeightFromCount(listBoxTargets, multiple: false);
			SetListHeightFromCount(listBoxSeries, multiple: false);
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

	protected virtual void SetListHeightFromCount(StiIndicatorIconRangeListBox list)
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
		if (selectedListBox == listBoxValues || selectedListBox == listBoxTargets)
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
		if (selectedListBox == listBoxValues && IndicatorValue != null)
		{
			((Control)textBoxExpression).Text = IndicatorValue.Expression;
		}
		else if (selectedListBox == listBoxTargets && IndicatorTarget != null)
		{
			((Control)textBoxExpression).Text = IndicatorTarget.Expression;
		}
		else if (selectedListBox == listBoxSeries && IndicatorSeries != null)
		{
			((Control)textBoxExpression).Text = IndicatorSeries.Expression;
		}
		else
		{
			((Control)textBoxExpression).Text = string.Empty;
		}
		base.LockRefresh = false;
	}

	internal void RefreshRange()
	{
		if (IconRange != null)
		{
			base.LockRefresh = true;
			fontIconSelectionRangeIcon.Icon = IconRange.Icon;
			((Control)textBoxRangeStartExpression).Text = IconRange.StartExpression;
			((Control)textBoxRangeEndExpression).Text = IconRange.EndExpression;
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

	private Panel GetIconSetPanel()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Expected O, but got Unknown
		Array values = Enum.GetValues(typeof(StiFontIconSet));
		Panel val = new Panel
		{
			Width = StiScale.XXI(140.0),
			Height = StiScale.YYI(28 * values.Length + 4)
		};
		((Control)val).Paint += new PaintEventHandler(StiIconControlHelper.PanelRootPainter);
		FontFamily fontFamilyIcons = StiFontIconsHelper.GetFontFamilyIcons();
		Font font = new Font(fontFamilyIcons, 13f);
		int i = StiScale.I2;
		int num = StiScale.I2;
		for (int j = 0; j < Enum.GetValues(typeof(StiFontIconSet)).Length; j++)
		{
			StiFontIconSet stiFontIconSet = (StiFontIconSet)values.GetValue(j);
			StiFontIconButton stiFontIconButton = new StiFontIconButton();
			((Control)stiFontIconButton).Location = new Point(i, num);
			((Control)stiFontIconButton).Width = ((Control)val).Width - StiScale.I4;
			((Control)stiFontIconButton).Height = StiScale.YYI(28.0);
			((Control)stiFontIconButton).Font = font;
			((Control)stiFontIconButton).Text = StiFontIconsHelper.GetIsonSetContent(stiFontIconSet);
			((Control)stiFontIconButton).ForeColor = StiDashboardStyleHelper.GetGlyphColor(IndicatorElement);
			((RadioButton)stiFontIconButton).Checked = IndicatorElement.IconSet.Equals(stiFontIconSet);
			((Control)stiFontIconButton).Tag = stiFontIconSet;
			StiFontIconButton stiFontIconButton2 = stiFontIconButton;
			((Control)stiFontIconButton2).MouseClick += new MouseEventHandler(ButtonItemIconSet_Click);
			num += StiScale.YYI(28.0);
			((Control)val).Controls.Add((Control)(object)stiFontIconButton2);
		}
		return val;
	}

	private void RefreshSampleIconSet()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		Graphics val = ((Control)panelBoxIconSet).CreateGraphics();
		FontFamily fontFamilyIcons = StiFontIconsHelper.GetFontFamilyIcons();
		Font val2 = new Font(fontFamilyIcons, 13f);
		try
		{
			Color glyphColor = StiDashboardStyleHelper.GetGlyphColor(IndicatorElement);
			SolidBrush val3 = new SolidBrush(glyphColor);
			try
			{
				StiDrawing.FillRectangle(val, StiUX.Background, new Rectangle(StiScale.I2, StiScale.I4, ((Control)panelBoxIconSet).Width - StiScale.I4, ((Control)panelBoxIconSet).Height - StiScale.I6));
				val.DrawString(StiFontIconsHelper.GetIsonSetContent(IndicatorElement.IconSet), val2, (Brush)(object)val3, (RectangleF)new Rectangle(StiScale.I2, StiScale.I4, ((Control)panelBoxIconSet).Width, ((Control)panelBoxIconSet).Height));
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			StiDrawing.DrawRectangle(val, StiUX.InputBorder, new Rectangle(((Control)panelBoxIconSet).ClientRectangle.X, ((Control)panelBoxIconSet).ClientRectangle.Y, ((Control)panelBoxIconSet).ClientRectangle.Width - 1, ((Control)panelBoxIconSet).ClientRectangle.Height - 1));
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	private void CheckColorIconsSet()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		foreach (object item in (ArrangedElementCollection)((Control)(Panel)iconSetsPopup.Content).Controls)
		{
			if (item is StiFontIconButton stiFontIconButton)
			{
				((Control)stiFontIconButton).ForeColor = StiDashboardStyleHelper.GetGlyphColor(IndicatorElement);
			}
		}
	}

	private Task BuildDataGridAsync()
	{
		return Task.Factory.StartNew(delegate
		{
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Expected O, but got Unknown
			string[] columns = new string[3] { "Value", "Target", "Series" };
			List<string[]> rows = StiStringsTableHelper.LoadStringRowsFromPackedString(IndicatorElement.ManuallyEnteredData, columns);
			StiStringsTableHelper.SaveStringRowsToCache(base.Element, rows);
			((Control)this).BeginInvoke((Delegate)(MethodInvoker)delegate
			{
				((DataGridView)gridData).AutoGenerateColumns = false;
				((DataGridView)gridData).DataSource = StiStringsTableHelper.LoadDataTableFromStringRows(rows, columns);
				gridData.AddNewEmptyRowsUpTo(10);
				gridData.AddTextBoxColumn("Value", Loc.GetMain("Value"), 0, (DataGridViewContentAlignment)64);
				gridData.AddTextBoxColumn("Target", Loc.GetMain("Target"), 0, (DataGridViewContentAlignment)64);
				gridData.AddTextBoxColumn("Series", Loc.Get("Chart", "Series"), 0, (DataGridViewContentAlignment)16);
				RefreshUiPanel();
				if (IsDataEnterMode)
				{
					((Control)gridData).Focus();
				}
			});
		});
	}

	private void SaveManuallyEnteredData()
	{
		IndicatorElement.ManuallyEnteredData = StiStringsTableHelper.SaveDataTableToPackedString(gridData.DataTable);
	}

	private void ComboBoxCalculationMode_SelectedIndexChanged(object sender, EventArgs e)
	{
		StiComboBox stiComboBox = comboBoxCalculationMode;
		bool flag = ((stiComboBox != null) ? ((ComboBox)stiComboBox).SelectedItem : null) as string == LocCalculationModeAuto;
		IndicatorElement.IconMode = (flag ? StiIndicatorIconMode.Auto : StiIndicatorIconMode.Custom);
		RefreshElementView();
		RefreshUiPanel();
	}

	private void ComboBoxRangeMode_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			IndicatorElement.IconRangeMode = ((((ComboBox)comboBoxRangeMode).SelectedItem as string == LocRangeModePercentage) ? StiIndicatorIconRangeMode.Percentage : StiIndicatorIconRangeMode.Value);
			RefreshElementView();
			RefreshUiPanel();
		}
	}

	private void FontIconSelectionButton_IconChanged(object sender, EventArgs e)
	{
		IndicatorElement.Icon = fontIconSelectionButton.Icon;
		RefreshElementView();
	}

	private void ButtonCustomIcon_CustomIconClick(object sender, EventArgs e)
	{
		ButtonOpen_Click(sender, e);
	}

	private void ButtonExpressionEdit_Click(object sender, EventArgs e)
	{
		if (StiExpressionEditorHelper.RunExpressionEditor(base.Element, ref textBoxExpression))
		{
			RefreshElementView();
		}
	}

	private void PanelImage_DragDrop(object sender, DragEventArgs e)
	{
		string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
		if (array == null)
		{
			return;
		}
		foreach (string item in array.Where((string f) => f != null))
		{
			string ext = Path.GetExtension(item).ToLowerInvariant();
			if (StiResourceTypeHelper.IsImageType(ext))
			{
				if (StiMaximumResourceSizeMessageBox.Check((Form)(object)this, item))
				{
					IndicatorElement.CustomIcon = StiImageHelper.TryBytesFromFile(item);
					RefreshImageAndStatus();
				}
				break;
			}
		}
	}

	private void PanelImage_DragOver(object sender, DragEventArgs e)
	{
		if (!e.Data.GetDataPresent(DataFormats.FileDrop, false))
		{
			return;
		}
		string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
		if (array != null)
		{
			foreach (string item in array.Where((string f) => f != null))
			{
				string ext = Path.GetExtension(item).ToLowerInvariant();
				if (StiResourceTypeHelper.IsImageType(ext))
				{
					e.Effect = (DragDropEffects)(-2147483645);
					return;
				}
			}
		}
		e.Effect = (DragDropEffects)0;
	}

	private void PanelImage_Paint(object sender, PaintEventArgs e)
	{
		StiImageDialogPainter.Paint((Control)(object)panelImage, e.Graphics, IndicatorElement.CustomIcon, drawDragDrop: true, drawBorder: true, isImage: true);
	}

	private void PanelImage_DoubleClick(object sender, EventArgs e)
	{
		ButtonOpen_Click(sender, e);
	}

	private void ButtonRemove_Click(object sender, EventArgs e)
	{
		IndicatorElement.CustomIcon = null;
		RefreshUiPanel();
		RefreshImageAndStatus();
	}

	private void ButtonOpen_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Invalid comparison between Unknown and I4
		OpenFileDialog val = new OpenFileDialog();
		try
		{
			((FileDialog)val).Filter = StiFileDialogHelper.GetImageFilter();
			((FileDialog)val).RestoreDirectory = true;
			if ((int)((CommonDialog)val).ShowDialog() != 1)
			{
				return;
			}
			try
			{
				if (StiMaximumResourceSizeMessageBox.Check((Form)(object)this, ((FileDialog)val).FileName))
				{
					IndicatorElement.CustomIcon = StiImageHelper.TryBytesFromFile(((FileDialog)val).FileName);
					RefreshUiPanel();
					RefreshImageAndStatus();
				}
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void ButtonIconSet_MouseClick(object sender, EventArgs e)
	{
		CheckColorIconsSet();
		((ToolStripDropDown)iconSetsPopup).Show(new Point(((Control)buttonIconSet).RectangleToScreen(Rectangle.Empty).Left + ((Control)buttonIconSet).Width - ((Control)iconSetsPopup).Width, ((Control)buttonIconSet).RectangleToScreen(Rectangle.Empty).Bottom + ((Control)buttonIconSet).Height + StiScale.I2));
	}

	private void ButtonItemIconSet_Click(object sender, EventArgs e)
	{
		Control val = (Control)((sender is Control) ? sender : null);
		if (val != null)
		{
			IndicatorElement.IconSet = (StiFontIconSet)val.Tag;
			RefreshElementView();
			RefreshSampleIconSet();
		}
		((Control)iconSetsPopup).Hide();
	}

	private void PanelBoxIconSet_Paint(object sender, PaintEventArgs e)
	{
		RefreshSampleIconSet();
	}

	private void ListBox_ElementsConverting(object sender, EventArgs e)
	{
		Stimulsoft.Dashboard.Design.Controls.StiMeterItem stiMeterItem = sender as Stimulsoft.Dashboard.Design.Controls.StiMeterItem;
		if (stiMeterItem.List == listBoxValues)
		{
			stiMeterItem.Meter = StiMeterHelper.Indicator.GetValue(stiMeterItem.Meter);
		}
		if (stiMeterItem.List == listBoxTargets)
		{
			stiMeterItem.Meter = StiMeterHelper.Indicator.GetTarget(stiMeterItem.Meter);
		}
		if (stiMeterItem.List == listBoxSeries)
		{
			stiMeterItem.Meter = StiMeterHelper.Indicator.GetSeries(stiMeterItem.Meter);
		}
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

	protected override void OnGlobalElementChanged(object sender, EventArgs e)
	{
		if (sender == base.Element)
		{
			ReadPropertiesFromElement();
			listBoxValues.SelectedMeterItem = listBoxValues.FirstItem();
		}
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

	private void FontIconSelectionRangeIcon_IconChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			IconRange.Icon = fontIconSelectionRangeIcon.Icon;
			RefreshElementView();
			listBoxRanges.InvalidateSelectedItem();
		}
	}

	private void ButtonAddRange_Click(object sender, EventArgs e)
	{
		IndicatorElement.IconRanges.Add(new StiIndicatorIconRange());
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

	private async void This_LoadAsync(object sender, EventArgs e)
	{
		await BuildDataGridAsync();
	}

	protected override void OnFormClosed(FormClosedEventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		base.OnFormClosed(e);
		SaveManuallyEnteredData();
		StiStringsTableHelper.RemoveFromCache(base.Element);
		((Control)panelBoxIconSet).Paint -= new PaintEventHandler(PanelBoxIconSet_Paint);
		((DataGridView)gridData).CellValueChanged -= new DataGridViewCellEventHandler(GridData_CellValueChanged);
		gridData.CellRowsChanged -= GridData_CellRowsChanged;
		((Form)this).Load -= This_LoadAsync;
		((Component)(object)timerRefreshGrid)?.Dispose();
		timerRefreshGrid = null;
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/dashboards_indicator.htm");
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
			if (selectedListBox == listBoxValues && IndicatorValue != null)
			{
				IndicatorValue.Expression = ((Control)textBoxExpression).Text;
				listBoxValues.SetSelectedText(StiLabelHelper.GetLabel(IndicatorValue));
			}
			if (selectedListBox == listBoxTargets && IndicatorTarget != null)
			{
				IndicatorTarget.Expression = ((Control)textBoxExpression).Text;
				listBoxTargets.SetSelectedText(StiLabelHelper.GetLabel(IndicatorTarget));
			}
			if (selectedListBox == listBoxSeries && IndicatorSeries != null)
			{
				IndicatorSeries.Expression = ((Control)textBoxExpression).Text;
				listBoxSeries.SetSelectedText(StiLabelHelper.GetLabel(IndicatorSeries));
			}
			RefreshExpressionHint();
		}
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

	private void ButtonExpressionMenu_Click(object sender, EventArgs e)
	{
		new StiIndicatorExpressionMenu(menuExpression, (TextBox)(object)textBoxExpression, this, selectedListBox).CreateItems();
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
			IconRange.StartExpression = ((Control)textBoxRangeStartExpression).Text;
			RefreshElementView();
			listBoxRanges.InvalidateSelectedItem();
		}
	}

	private void TextBoxRangeEndExpression_TextChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			IconRange.EndExpression = ((Control)textBoxRangeEndExpression).Text;
			RefreshElementView();
			listBoxRanges.InvalidateSelectedItem();
		}
	}

	private void ButtonEnterDataManually_Click(object sender, EventArgs e)
	{
		IndicatorElement.DataMode = StiDataMode.ManuallyEnteringData;
		RefreshElementView();
		RefreshUiPanel();
		((Control)gridData).Focus();
	}

	private void ButtonUseDataFields_Click(object sender, EventArgs e)
	{
		IsShowMore = true;
		IndicatorElement.DataMode = StiDataMode.UsingDataFields;
		RefreshElementView();
		RefreshUiPanel();
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
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Expected O, but got Unknown
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b0: Expected O, but got Unknown
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0563: Unknown result type (might be due to invalid IL or missing references)
		//IL_05db: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e5: Expected O, but got Unknown
		//IL_0612: Unknown result type (might be due to invalid IL or missing references)
		//IL_0698: Unknown result type (might be due to invalid IL or missing references)
		//IL_0710: Unknown result type (might be due to invalid IL or missing references)
		//IL_071a: Expected O, but got Unknown
		//IL_0757: Unknown result type (might be due to invalid IL or missing references)
		//IL_0830: Unknown result type (might be due to invalid IL or missing references)
		//IL_088b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0895: Expected O, but got Unknown
		//IL_08b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0964: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09cf: Expected O, but got Unknown
		//IL_0a13: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3e: Expected O, but got Unknown
		//IL_0b5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c31: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d65: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e29: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ebb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1081: Unknown result type (might be due to invalid IL or missing references)
		//IL_1116: Unknown result type (might be due to invalid IL or missing references)
		//IL_1163: Unknown result type (might be due to invalid IL or missing references)
		//IL_116d: Expected O, but got Unknown
		//IL_117a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1184: Expected O, but got Unknown
		//IL_1191: Unknown result type (might be due to invalid IL or missing references)
		//IL_119b: Expected O, but got Unknown
		//IL_11d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_11da: Expected O, but got Unknown
		//IL_11fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1282: Unknown result type (might be due to invalid IL or missing references)
		//IL_128c: Expected O, but got Unknown
		//IL_12ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_137f: Unknown result type (might be due to invalid IL or missing references)
		//IL_13ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_148a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1517: Unknown result type (might be due to invalid IL or missing references)
		//IL_1593: Unknown result type (might be due to invalid IL or missing references)
		//IL_159d: Expected O, but got Unknown
		//IL_1632: Unknown result type (might be due to invalid IL or missing references)
		//IL_16b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_178d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1810: Unknown result type (might be due to invalid IL or missing references)
		//IL_186e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1878: Expected O, but got Unknown
		//IL_18ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_18b7: Expected O, but got Unknown
		//IL_18d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_19a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a88: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a92: Expected O, but got Unknown
		//IL_1ac7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ad1: Expected O, but got Unknown
		//IL_1aee: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b78: Expected O, but got Unknown
		//IL_1b99: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ce6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e59: Expected O, but got Unknown
		//IL_1ee8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ef2: Expected O, but got Unknown
		//IL_1f6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ff6: Unknown result type (might be due to invalid IL or missing references)
		//IL_206f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2079: Expected O, but got Unknown
		//IL_2099: Unknown result type (might be due to invalid IL or missing references)
		//IL_226b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2289: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiIndicatorElementEditorForm));
		toolTip = new ToolTip(components);
		contextMenuBar = new ContextMenuBar();
		menuExpression = new ButtonItem();
		listBoxSeries = new Stimulsoft.Dashboard.Design.Controls.StiMeterListBox();
		labelSeries = new StiChartGroupLabel();
		listBoxTargets = new Stimulsoft.Dashboard.Design.Controls.StiMeterListBox();
		labelTarget = new StiChartGroupLabel();
		listBoxValues = new Stimulsoft.Dashboard.Design.Controls.StiMeterListBox();
		labelExpression = new StiLabelUX();
		panelBoxExpression = new StiPanelBox();
		buttonExpressionEdit = new StiOperationButton();
		textBoxExpression = new HintTextBox();
		buttonExpressionMenu = new StiOperationButton();
		labelValue = new StiLabelPanel();
		buttonEnterDataManually = new StiOperationButton();
		panelIcon = new StiPanelUX();
		fontIconSelectionButton = new StiFontIconSelectionButton();
		labelIcon = new StiLabelUX();
		panelIconSet = new StiPanelUX();
		panelBoxIconSet = new StiPanelUX();
		buttonIconSet = new StiOperationButton();
		labelIconSet = new StiLabelUX();
		panelCustomIcon = new StiPanelUX();
		panelImage = new StiPanelUX();
		buttonOpen = new StiOperationButton();
		buttonRemove = new StiOperationButton();
		panelModeUi = new StiPanelUX();
		labelCalculationMode = new StiLabelUX();
		comboBoxCalculationMode = new StiComboBox();
		listBoxRanges = new StiIndicatorIconRangeListBox();
		panelRangeOption = new StiPanelUX();
		fontIconSelectionRangeIcon = new StiFontIconSelectionButton();
		panelBoxRangeEndExpression = new StiPanelBox();
		textBoxRangeEndExpression = new HintTextBox();
		buttonRangeEndExpression = new StiOperationButton();
		panelBoxRangeStartExpression = new StiPanelBox();
		textBoxRangeStartExpression = new HintTextBox();
		buttonRangeStartExpression = new StiOperationButton();
		label1 = new StiLabelUX();
		panelRangeUi = new StiPanelUX();
		comboBoxRangeMode = new StiComboBox();
		buttonAddRange = new StiButtonUX();
		gridData = new StiExcelGrid();
		labelGridData = new StiLabelPanel();
		buttonUseDataFields = new StiOperationButton();
		((ISupportInitialize)contextMenuBar).BeginInit();
		((Control)panelBoxExpression).SuspendLayout();
		((Control)labelValue).SuspendLayout();
		((Control)panelIcon).SuspendLayout();
		((Control)panelIconSet).SuspendLayout();
		((Control)panelBoxIconSet).SuspendLayout();
		((Control)panelCustomIcon).SuspendLayout();
		((Control)panelImage).SuspendLayout();
		((Control)panelModeUi).SuspendLayout();
		((Control)panelRangeOption).SuspendLayout();
		((Control)panelBoxRangeEndExpression).SuspendLayout();
		((Control)panelBoxRangeStartExpression).SuspendLayout();
		((Control)panelRangeUi).SuspendLayout();
		((ISupportInitialize)gridData).BeginInit();
		((Control)labelGridData).SuspendLayout();
		((Control)this).SuspendLayout();
		contextMenuBar.AntiAlias = true;
		contextMenuBar.DockTabStripHeight = 50;
		((Control)contextMenuBar).Font = new Font("Segoe UI", 9f);
		contextMenuBar.Items.AddRange(new BaseItem[1] { menuExpression });
		contextMenuBar.Location = new Point(394, 600);
		((Control)contextMenuBar).Margin = new Padding(4, 4, 4, 4);
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
		((Control)listBoxSeries).AllowDrop = true;
		((Control)listBoxSeries).Dock = (DockStyle)1;
		((Control)listBoxSeries).Location = new Point(24, 482);
		((Control)listBoxSeries).Margin = new Padding(4, 4, 4, 4);
		((Control)listBoxSeries).Name = "listBoxSeries";
		((Control)listBoxSeries).Size = new Size(530, 96);
		((Control)listBoxSeries).TabIndex = 5;
		((ListBox)listBoxSeries).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxSeries).Enter += ListBox_Enter;
		((Control)listBoxSeries).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)labelSeries).Dock = (DockStyle)1;
		((Control)labelSeries).Location = new Point(24, 421);
		((Control)labelSeries).Margin = new Padding(4, 0, 4, 0);
		((Control)labelSeries).Name = "labelSeries";
		((Control)labelSeries).Size = new Size(530, 61);
		((Control)labelSeries).TabIndex = 4;
		((Control)labelSeries).Text = "Series";
		((Control)listBoxTargets).AllowDrop = true;
		((Control)listBoxTargets).Dock = (DockStyle)1;
		((Control)listBoxTargets).Location = new Point(24, 325);
		((Control)listBoxTargets).Margin = new Padding(4, 4, 4, 4);
		((Control)listBoxTargets).Name = "listBoxTargets";
		((Control)listBoxTargets).Size = new Size(530, 96);
		((Control)listBoxTargets).TabIndex = 3;
		((ListBox)listBoxTargets).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxTargets).Enter += ListBox_Enter;
		((Control)listBoxTargets).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)labelTarget).Dock = (DockStyle)1;
		((Control)labelTarget).Location = new Point(24, 264);
		((Control)labelTarget).Margin = new Padding(4, 0, 4, 0);
		((Control)labelTarget).Name = "labelTarget";
		((Control)labelTarget).Size = new Size(530, 61);
		((Control)labelTarget).TabIndex = 2;
		((Control)labelTarget).Text = "Target";
		((Control)listBoxValues).AllowDrop = true;
		((Control)listBoxValues).Dock = (DockStyle)1;
		((Control)listBoxValues).Location = new Point(24, 185);
		((Control)listBoxValues).Margin = new Padding(4, 4, 4, 4);
		((Control)listBoxValues).Name = "listBoxValues";
		((Control)listBoxValues).Size = new Size(530, 79);
		((Control)listBoxValues).TabIndex = 1;
		((ListBox)listBoxValues).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxValues).Enter += ListBox_Enter;
		((Control)listBoxValues).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)labelExpression).Dock = (DockStyle)1;
		((Control)labelExpression).ForeColor = Color.DimGray;
		((Control)labelExpression).Location = new Point(24, 578);
		((Control)labelExpression).Margin = new Padding(4, 0, 4, 0);
		((Control)labelExpression).Name = "labelExpression";
		((Control)labelExpression).Size = new Size(530, 61);
		((Control)labelExpression).TabIndex = 6;
		((Control)labelExpression).Text = "Expression";
		((Label)labelExpression).TextAlign = (ContentAlignment)16;
		((Control)panelBoxExpression).BackColor = Color.White;
		((Control)panelBoxExpression).Controls.Add((Control)(object)buttonExpressionEdit);
		((Control)panelBoxExpression).Controls.Add((Control)(object)textBoxExpression);
		((Control)panelBoxExpression).Controls.Add((Control)(object)buttonExpressionMenu);
		((Control)panelBoxExpression).Dock = (DockStyle)1;
		((Control)panelBoxExpression).Location = new Point(24, 639);
		((Control)panelBoxExpression).Margin = new Padding(4, 4, 4, 4);
		((Control)panelBoxExpression).Name = "panelBoxExpression";
		((Control)panelBoxExpression).Size = new Size(530, 48);
		((Control)panelBoxExpression).TabIndex = 7;
		((Control)buttonExpressionEdit).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonExpressionEdit).Image = (Image)componentResourceManager.GetObject("buttonExpressionEdit.Image");
		((Control)buttonExpressionEdit).Location = new Point(438, 2);
		((Control)buttonExpressionEdit).Margin = new Padding(4, 4, 4, 4);
		((Control)buttonExpressionEdit).Name = "buttonExpressionEdit";
		((Control)buttonExpressionEdit).Size = new Size(46, 44);
		((Control)buttonExpressionEdit).TabIndex = 1;
		((Control)buttonExpressionEdit).TabStop = false;
		((ButtonBase)buttonExpressionEdit).UseVisualStyleBackColor = true;
		((Control)buttonExpressionEdit).Click += ButtonExpressionEdit_Click;
		((Control)textBoxExpression).Anchor = (AnchorStyles)13;
		((Control)textBoxExpression).BackColor = SystemColors.Window;
		((TextBoxBase)textBoxExpression).BorderStyle = (BorderStyle)0;
		((Control)textBoxExpression).Location = new Point(8, 11);
		((Control)textBoxExpression).Margin = new Padding(4, 4, 4, 4);
		((Control)textBoxExpression).Name = "textBoxExpression";
		((Control)textBoxExpression).Size = new Size(424, 24);
		((Control)textBoxExpression).TabIndex = 0;
		((Control)textBoxExpression).TextChanged += TextBoxExpression_TextChanged;
		((Control)textBoxExpression).KeyDown += new KeyEventHandler(TextBoxExpression_KeyDown);
		((Control)textBoxExpression).Leave += TextBoxExpression_Leave;
		((Control)buttonExpressionMenu).Anchor = (AnchorStyles)9;
		((Control)buttonExpressionMenu).Location = new Point(482, 2);
		((Control)buttonExpressionMenu).Margin = new Padding(4, 4, 4, 4);
		((Control)buttonExpressionMenu).Name = "buttonExpressionMenu";
		((Control)buttonExpressionMenu).Size = new Size(46, 44);
		((Control)buttonExpressionMenu).TabIndex = 2;
		((Control)buttonExpressionMenu).TabStop = false;
		((ButtonBase)buttonExpressionMenu).UseVisualStyleBackColor = true;
		((Control)buttonExpressionMenu).Click += ButtonExpressionMenu_Click;
		((Control)labelValue).Controls.Add((Control)(object)buttonEnterDataManually);
		((Control)labelValue).Dock = (DockStyle)1;
		((Control)labelValue).Location = new Point(24, 124);
		((Control)labelValue).Margin = new Padding(4, 0, 4, 0);
		((Control)labelValue).Name = "labelValue";
		((Control)labelValue).Size = new Size(530, 61);
		((Control)labelValue).TabIndex = 0;
		((Control)labelValue).Text = "Value";
		labelValue.TextAlign = (ContentAlignment)16;
		((Control)buttonEnterDataManually).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonEnterDataManually).Image = (Image)componentResourceManager.GetObject("buttonEnterDataManually.Image");
		((Control)buttonEnterDataManually).Location = new Point(483, 8);
		((Control)buttonEnterDataManually).Margin = new Padding(4, 4, 4, 4);
		((Control)buttonEnterDataManually).Name = "buttonEnterDataManually";
		((Control)buttonEnterDataManually).Size = new Size(48, 44);
		((Control)buttonEnterDataManually).TabIndex = 18;
		((Control)buttonEnterDataManually).TabStop = false;
		((ButtonBase)buttonEnterDataManually).UseVisualStyleBackColor = true;
		((Control)buttonEnterDataManually).Click += ButtonEnterDataManually_Click;
		((Control)panelIcon).BackColor = Color.White;
		((Control)panelIcon).Controls.Add((Control)(object)fontIconSelectionButton);
		((Control)panelIcon).Controls.Add((Control)(object)labelIcon);
		((Control)panelIcon).Dock = (DockStyle)1;
		((Control)panelIcon).Location = new Point(24, 687);
		((Control)panelIcon).Margin = new Padding(6, 6, 6, 6);
		((Control)panelIcon).Name = "panelIcon";
		((Control)panelIcon).Size = new Size(530, 73);
		((Control)panelIcon).TabIndex = 8;
		fontIconSelectionButton.Icon = StiFontIcons.Latin5;
		fontIconSelectionButton.IconColor = Color.FromArgb(68, 114, 196);
		((Control)fontIconSelectionButton).Location = new Point(176, 23);
		((Control)fontIconSelectionButton).Margin = new Padding(6, 6, 6, 6);
		((Control)fontIconSelectionButton).Name = "fontIconSelectionButton";
		fontIconSelectionButton.ShowCustomIcon = true;
		fontIconSelectionButton.ShowEmptyIcon = false;
		((Control)fontIconSelectionButton).Size = new Size(162, 48);
		((Control)fontIconSelectionButton).TabIndex = 10;
		fontIconSelectionButton.CustomIconClick += ButtonCustomIcon_CustomIconClick;
		fontIconSelectionButton.IconChanged += FontIconSelectionButton_IconChanged;
		((Control)labelIcon).ForeColor = Color.DimGray;
		((Control)labelIcon).Location = new Point(0, 23);
		((Control)labelIcon).Margin = new Padding(4, 0, 4, 0);
		((Control)labelIcon).Name = "labelIcon";
		((Control)labelIcon).Size = new Size(172, 48);
		((Control)labelIcon).TabIndex = 9;
		((Control)labelIcon).Text = "Icon";
		((Label)labelIcon).TextAlign = (ContentAlignment)16;
		((Control)panelIconSet).BackColor = Color.White;
		((Control)panelIconSet).Controls.Add((Control)(object)panelBoxIconSet);
		((Control)panelIconSet).Controls.Add((Control)(object)labelIconSet);
		((Control)panelIconSet).Dock = (DockStyle)1;
		((Control)panelIconSet).Location = new Point(24, 825);
		((Control)panelIconSet).Margin = new Padding(6, 6, 6, 6);
		((Control)panelIconSet).Name = "panelIconSet";
		((Control)panelIconSet).Size = new Size(530, 73);
		((Control)panelIconSet).TabIndex = 9;
		((Control)panelBoxIconSet).Anchor = (AnchorStyles)13;
		((Control)panelBoxIconSet).BackColor = Color.White;
		((Control)panelBoxIconSet).Controls.Add((Control)(object)buttonIconSet);
		((Control)panelBoxIconSet).Location = new Point(176, 23);
		((Control)panelBoxIconSet).Margin = new Padding(4, 4, 4, 4);
		((Control)panelBoxIconSet).Name = "panelBoxIconSet";
		((Control)panelBoxIconSet).Size = new Size(354, 48);
		((Control)panelBoxIconSet).TabIndex = 11;
		((Control)panelBoxIconSet).Click += ButtonIconSet_MouseClick;
		((Control)buttonIconSet).Anchor = (AnchorStyles)9;
		((Control)buttonIconSet).Location = new Point(306, 2);
		((Control)buttonIconSet).Margin = new Padding(4, 4, 4, 4);
		((Control)buttonIconSet).Name = "buttonIconSet";
		((Control)buttonIconSet).Size = new Size(46, 44);
		((Control)buttonIconSet).TabIndex = 1;
		((Control)buttonIconSet).TabStop = false;
		((ButtonBase)buttonIconSet).UseVisualStyleBackColor = true;
		((Control)buttonIconSet).Click += ButtonIconSet_MouseClick;
		((Control)labelIconSet).ForeColor = Color.DimGray;
		((Control)labelIconSet).Location = new Point(0, 23);
		((Control)labelIconSet).Margin = new Padding(4, 0, 4, 0);
		((Control)labelIconSet).Name = "labelIconSet";
		((Control)labelIconSet).Size = new Size(172, 48);
		((Control)labelIconSet).TabIndex = 10;
		((Control)labelIconSet).Text = "Icon Set";
		((Label)labelIconSet).TextAlign = (ContentAlignment)16;
		((Control)panelCustomIcon).BackColor = Color.White;
		((Control)panelCustomIcon).Controls.Add((Control)(object)panelImage);
		((Control)panelCustomIcon).Dock = (DockStyle)1;
		((Control)panelCustomIcon).Location = new Point(24, 898);
		((Control)panelCustomIcon).Margin = new Padding(4, 4, 4, 4);
		((Control)panelCustomIcon).Name = "panelCustomIcon";
		((Control)panelCustomIcon).Size = new Size(530, 283);
		((Control)panelCustomIcon).TabIndex = 5;
		((Control)panelImage).AllowDrop = true;
		((Control)panelImage).Controls.Add((Control)(object)buttonOpen);
		((Control)panelImage).Controls.Add((Control)(object)buttonRemove);
		((Control)panelImage).Location = new Point(0, 23);
		((Control)panelImage).Margin = new Padding(4, 4, 4, 4);
		((Control)panelImage).Name = "panelImage";
		((Control)panelImage).Size = new Size(530, 260);
		((Control)panelImage).TabIndex = 0;
		((Control)panelImage).DragDrop += new DragEventHandler(PanelImage_DragDrop);
		((Control)panelImage).DragOver += new DragEventHandler(PanelImage_DragOver);
		((Control)panelImage).Paint += new PaintEventHandler(PanelImage_Paint);
		((Control)panelImage).DoubleClick += PanelImage_DoubleClick;
		((Control)buttonOpen).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonOpen).Image = (Image)componentResourceManager.GetObject("buttonOpen.Image");
		((Control)buttonOpen).Location = new Point(470, 10);
		((Control)buttonOpen).Margin = new Padding(4, 4, 4, 4);
		((Control)buttonOpen).Name = "buttonOpen";
		((Control)buttonOpen).Size = new Size(48, 44);
		((Control)buttonOpen).TabIndex = 0;
		((Control)buttonOpen).TabStop = false;
		((ButtonBase)buttonOpen).UseVisualStyleBackColor = true;
		((Control)buttonOpen).Click += ButtonOpen_Click;
		((Control)buttonRemove).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonRemove).Image = (Image)componentResourceManager.GetObject("buttonRemove.Image");
		((Control)buttonRemove).Location = new Point(470, 58);
		((Control)buttonRemove).Margin = new Padding(4, 4, 4, 4);
		((Control)buttonRemove).Name = "buttonRemove";
		((Control)buttonRemove).Size = new Size(48, 44);
		((Control)buttonRemove).TabIndex = 1;
		((Control)buttonRemove).TabStop = false;
		((ButtonBase)buttonRemove).UseVisualStyleBackColor = true;
		((Control)buttonRemove).Click += ButtonRemove_Click;
		((Control)panelModeUi).BackColor = Color.White;
		((Control)panelModeUi).Controls.Add((Control)(object)labelCalculationMode);
		((Control)panelModeUi).Controls.Add((Control)(object)comboBoxCalculationMode);
		((Control)panelModeUi).Dock = (DockStyle)1;
		((Control)panelModeUi).Location = new Point(24, 760);
		((Control)panelModeUi).Margin = new Padding(4, 4, 4, 4);
		((Control)panelModeUi).Name = "panelModeUi";
		((Control)panelModeUi).Size = new Size(530, 65);
		((Control)panelModeUi).TabIndex = 13;
		((Control)labelCalculationMode).ForeColor = Color.DimGray;
		((Control)labelCalculationMode).Location = new Point(0, 23);
		((Control)labelCalculationMode).Margin = new Padding(4, 0, 4, 0);
		((Control)labelCalculationMode).Name = "labelCalculationMode";
		((Control)labelCalculationMode).Size = new Size(170, 39);
		((Control)labelCalculationMode).TabIndex = 1;
		((Control)labelCalculationMode).Text = "Mode";
		((Label)labelCalculationMode).TextAlign = (ContentAlignment)16;
		((Control)comboBoxCalculationMode).Anchor = (AnchorStyles)13;
		comboBoxCalculationMode.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxCalculationMode).FormattingEnabled = true;
		((Control)comboBoxCalculationMode).Location = new Point(176, 23);
		((Control)comboBoxCalculationMode).Margin = new Padding(4, 4, 4, 4);
		((Control)comboBoxCalculationMode).Name = "comboBoxCalculationMode";
		((Control)comboBoxCalculationMode).Size = new Size(350, 42);
		((Control)comboBoxCalculationMode).TabIndex = 1;
		((ComboBox)comboBoxCalculationMode).SelectedIndexChanged += ComboBoxCalculationMode_SelectedIndexChanged;
		((Control)listBoxRanges).AllowDrop = true;
		((Control)listBoxRanges).Dock = (DockStyle)1;
		((Control)listBoxRanges).Location = new Point(24, 1264);
		((Control)listBoxRanges).Margin = new Padding(4, 4, 4, 4);
		((Control)listBoxRanges).Name = "listBoxRanges";
		((Control)listBoxRanges).Size = new Size(530, 139);
		((Control)listBoxRanges).TabIndex = 15;
		((ListBox)listBoxRanges).SelectedIndexChanged += ListBoxRanges_SelectedIndexChanged;
		((Control)listBoxRanges).Enter += ListBoxRanges_Enter;
		((Control)listBoxRanges).MouseDown += new MouseEventHandler(ListBoxRanges_MouseDown);
		((Control)panelRangeOption).BackColor = Color.White;
		((Control)panelRangeOption).Controls.Add((Control)(object)fontIconSelectionRangeIcon);
		((Control)panelRangeOption).Controls.Add((Control)(object)panelBoxRangeEndExpression);
		((Control)panelRangeOption).Controls.Add((Control)(object)panelBoxRangeStartExpression);
		((Control)panelRangeOption).Controls.Add((Control)(object)label1);
		((Control)panelRangeOption).Dock = (DockStyle)1;
		((Control)panelRangeOption).Location = new Point(24, 1403);
		((Control)panelRangeOption).Margin = new Padding(6, 6, 6, 6);
		((Control)panelRangeOption).Name = "panelRangeOption";
		((Control)panelRangeOption).Size = new Size(530, 77);
		((Control)panelRangeOption).TabIndex = 16;
		((Control)fontIconSelectionRangeIcon).Anchor = (AnchorStyles)13;
		fontIconSelectionRangeIcon.Icon = StiFontIcons.Latin5;
		fontIconSelectionRangeIcon.IconColor = Color.DimGray;
		((Control)fontIconSelectionRangeIcon).Location = new Point(0, 23);
		((Control)fontIconSelectionRangeIcon).Margin = new Padding(6, 6, 6, 6);
		((Control)fontIconSelectionRangeIcon).Name = "fontIconSelectionRangeIcon";
		fontIconSelectionRangeIcon.ShowCustomIcon = false;
		fontIconSelectionRangeIcon.ShowEmptyIcon = false;
		((Control)fontIconSelectionRangeIcon).Size = new Size(156, 48);
		((Control)fontIconSelectionRangeIcon).TabIndex = 17;
		fontIconSelectionRangeIcon.IconChanged += FontIconSelectionRangeIcon_IconChanged;
		((Control)panelBoxRangeEndExpression).Anchor = (AnchorStyles)13;
		((Control)panelBoxRangeEndExpression).BackColor = Color.White;
		((Control)panelBoxRangeEndExpression).Controls.Add((Control)(object)textBoxRangeEndExpression);
		((Control)panelBoxRangeEndExpression).Controls.Add((Control)(object)buttonRangeEndExpression);
		((Control)panelBoxRangeEndExpression).Location = new Point(376, 23);
		((Control)panelBoxRangeEndExpression).Margin = new Padding(4, 4, 4, 4);
		((Control)panelBoxRangeEndExpression).Name = "panelBoxRangeEndExpression";
		((Control)panelBoxRangeEndExpression).Size = new Size(154, 48);
		((Control)panelBoxRangeEndExpression).TabIndex = 1;
		((Control)textBoxRangeEndExpression).Anchor = (AnchorStyles)13;
		((Control)textBoxRangeEndExpression).BackColor = SystemColors.Window;
		((TextBoxBase)textBoxRangeEndExpression).BorderStyle = (BorderStyle)0;
		((Control)textBoxRangeEndExpression).Location = new Point(8, 11);
		((Control)textBoxRangeEndExpression).Margin = new Padding(4, 4, 4, 4);
		((Control)textBoxRangeEndExpression).Name = "textBoxRangeEndExpression";
		((Control)textBoxRangeEndExpression).Size = new Size(94, 24);
		((Control)textBoxRangeEndExpression).TabIndex = 0;
		((Control)textBoxRangeEndExpression).TextChanged += TextBoxRangeEndExpression_TextChanged;
		((Control)textBoxRangeEndExpression).KeyDown += new KeyEventHandler(TextBoxExpression_KeyDown);
		((Control)textBoxRangeEndExpression).Leave += TextBoxExpression_Leave;
		((Control)buttonRangeEndExpression).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonRangeEndExpression).Image = (Image)componentResourceManager.GetObject("buttonRangeEndExpression.Image");
		((Control)buttonRangeEndExpression).Location = new Point(106, 2);
		((Control)buttonRangeEndExpression).Margin = new Padding(4, 4, 4, 4);
		((Control)buttonRangeEndExpression).Name = "buttonRangeEndExpression";
		((Control)buttonRangeEndExpression).Size = new Size(46, 44);
		((Control)buttonRangeEndExpression).TabIndex = 1;
		((Control)buttonRangeEndExpression).TabStop = false;
		((ButtonBase)buttonRangeEndExpression).UseVisualStyleBackColor = true;
		((Control)buttonRangeEndExpression).Click += ButtonRangeExpression_Click;
		((Control)panelBoxRangeStartExpression).Anchor = (AnchorStyles)13;
		((Control)panelBoxRangeStartExpression).BackColor = Color.White;
		((Control)panelBoxRangeStartExpression).Controls.Add((Control)(object)textBoxRangeStartExpression);
		((Control)panelBoxRangeStartExpression).Controls.Add((Control)(object)buttonRangeStartExpression);
		((Control)panelBoxRangeStartExpression).Location = new Point(180, 23);
		((Control)panelBoxRangeStartExpression).Margin = new Padding(4, 4, 4, 4);
		((Control)panelBoxRangeStartExpression).Name = "panelBoxRangeStartExpression";
		((Control)panelBoxRangeStartExpression).Size = new Size(154, 48);
		((Control)panelBoxRangeStartExpression).TabIndex = 0;
		((Control)textBoxRangeStartExpression).Anchor = (AnchorStyles)13;
		((Control)textBoxRangeStartExpression).BackColor = SystemColors.Window;
		((TextBoxBase)textBoxRangeStartExpression).BorderStyle = (BorderStyle)0;
		((Control)textBoxRangeStartExpression).Location = new Point(8, 11);
		((Control)textBoxRangeStartExpression).Margin = new Padding(4, 4, 4, 4);
		((Control)textBoxRangeStartExpression).Name = "textBoxRangeStartExpression";
		((Control)textBoxRangeStartExpression).Size = new Size(94, 24);
		((Control)textBoxRangeStartExpression).TabIndex = 0;
		((Control)textBoxRangeStartExpression).TextChanged += TextBoxRangeStartExpression_TextChanged;
		((Control)textBoxRangeStartExpression).KeyDown += new KeyEventHandler(TextBoxExpression_KeyDown);
		((Control)textBoxRangeStartExpression).Leave += TextBoxExpression_Leave;
		((Control)buttonRangeStartExpression).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonRangeStartExpression).Image = (Image)componentResourceManager.GetObject("buttonRangeStartExpression.Image");
		((Control)buttonRangeStartExpression).Location = new Point(106, 2);
		((Control)buttonRangeStartExpression).Margin = new Padding(4, 4, 4, 4);
		((Control)buttonRangeStartExpression).Name = "buttonRangeStartExpression";
		((Control)buttonRangeStartExpression).Size = new Size(46, 44);
		((Control)buttonRangeStartExpression).TabIndex = 1;
		((Control)buttonRangeStartExpression).TabStop = false;
		((ButtonBase)buttonRangeStartExpression).UseVisualStyleBackColor = true;
		((Control)buttonRangeStartExpression).Click += ButtonRangeExpression_Click;
		((Control)label1).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)label1).Location = new Point(342, 27);
		((Control)label1).Margin = new Padding(4, 0, 4, 0);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(26, 39);
		((Control)label1).TabIndex = 16;
		((Control)label1).Text = "-";
		((Label)label1).TextAlign = (ContentAlignment)32;
		((Control)panelRangeUi).BackColor = Color.White;
		((Control)panelRangeUi).Controls.Add((Control)(object)comboBoxRangeMode);
		((Control)panelRangeUi).Controls.Add((Control)(object)buttonAddRange);
		((Control)panelRangeUi).Dock = (DockStyle)1;
		((Control)panelRangeUi).Location = new Point(24, 1181);
		((Control)panelRangeUi).Margin = new Padding(4, 4, 4, 4);
		((Control)panelRangeUi).Name = "panelRangeUi";
		((Control)panelRangeUi).Size = new Size(530, 83);
		((Control)panelRangeUi).TabIndex = 14;
		((Control)panelRangeUi).Visible = false;
		((Control)comboBoxRangeMode).Anchor = (AnchorStyles)13;
		comboBoxRangeMode.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxRangeMode).FormattingEnabled = true;
		((Control)comboBoxRangeMode).Location = new Point(0, 21);
		((Control)comboBoxRangeMode).Margin = new Padding(4, 4, 4, 4);
		((Control)comboBoxRangeMode).Name = "comboBoxRangeMode";
		((Control)comboBoxRangeMode).Size = new Size(218, 42);
		((Control)comboBoxRangeMode).TabIndex = 3;
		((ComboBox)comboBoxRangeMode).SelectedIndexChanged += ComboBoxRangeMode_SelectedIndexChanged;
		((ButtonBase)buttonAddRange).FlatAppearance.BorderSize = 0;
		((Control)buttonAddRange).Location = new Point(308, 21);
		((Control)buttonAddRange).Margin = new Padding(4, 4, 4, 4);
		((Control)buttonAddRange).Name = "buttonAddRange";
		((Control)buttonAddRange).Size = new Size(222, 40);
		((Control)buttonAddRange).TabIndex = 2;
		((Control)buttonAddRange).Text = "Add Range";
		((ButtonBase)buttonAddRange).UseVisualStyleBackColor = true;
		((Control)buttonAddRange).Click += ButtonAddRange_Click;
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
		((Control)gridData).Margin = new Padding(6, 6, 6, 6);
		((Control)gridData).Name = "gridData";
		((DataGridView)gridData).SelectionMode = (DataGridViewSelectionMode)0;
		((Control)gridData).Size = new Size(530, 40);
		((Control)gridData).TabIndex = 17;
		((Control)labelGridData).Controls.Add((Control)(object)buttonUseDataFields);
		((Control)labelGridData).Dock = (DockStyle)1;
		((Control)labelGridData).Location = new Point(24, 23);
		((Control)labelGridData).Margin = new Padding(4, 0, 4, 0);
		((Control)labelGridData).Name = "labelGridData";
		((Control)labelGridData).Size = new Size(530, 61);
		((Control)labelGridData).TabIndex = 19;
		((Control)labelGridData).Text = "Data";
		labelGridData.TextAlign = (ContentAlignment)16;
		((Control)buttonUseDataFields).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonUseDataFields).Image = (Image)componentResourceManager.GetObject("buttonUseDataFields.Image");
		((Control)buttonUseDataFields).Location = new Point(483, 8);
		((Control)buttonUseDataFields).Margin = new Padding(4, 4, 4, 4);
		((Control)buttonUseDataFields).Name = "buttonUseDataFields";
		((Control)buttonUseDataFields).Size = new Size(48, 44);
		((Control)buttonUseDataFields).TabIndex = 18;
		((Control)buttonUseDataFields).TabStop = false;
		((ButtonBase)buttonUseDataFields).UseVisualStyleBackColor = true;
		((Control)buttonUseDataFields).Click += ButtonUseDataFields_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(578, 1511);
		((Control)this).Controls.Add((Control)(object)panelRangeOption);
		((Control)this).Controls.Add((Control)(object)listBoxRanges);
		((Control)this).Controls.Add((Control)(object)panelRangeUi);
		((Control)this).Controls.Add((Control)(object)panelCustomIcon);
		((Control)this).Controls.Add((Control)(object)panelIconSet);
		((Control)this).Controls.Add((Control)(object)panelModeUi);
		((Control)this).Controls.Add((Control)(object)panelIcon);
		((Control)this).Controls.Add((Control)(object)panelBoxExpression);
		((Control)this).Controls.Add((Control)(object)labelExpression);
		((Control)this).Controls.Add((Control)(object)contextMenuBar);
		((Control)this).Controls.Add((Control)(object)listBoxSeries);
		((Control)this).Controls.Add((Control)(object)labelSeries);
		((Control)this).Controls.Add((Control)(object)listBoxTargets);
		((Control)this).Controls.Add((Control)(object)labelTarget);
		((Control)this).Controls.Add((Control)(object)listBoxValues);
		((Control)this).Controls.Add((Control)(object)labelValue);
		((Control)this).Controls.Add((Control)(object)gridData);
		((Control)this).Controls.Add((Control)(object)labelGridData);
		((Form)this).Margin = new Padding(2, 2, 2, 2);
		((Control)this).Name = "StiIndicatorElementEditorForm";
		((Control)this).Padding = new Padding(24, 23, 24, 23);
		((Control)this).Text = "Indicator";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((ISupportInitialize)contextMenuBar).EndInit();
		((Control)panelBoxExpression).ResumeLayout(false);
		((Control)panelBoxExpression).PerformLayout();
		((Control)labelValue).ResumeLayout(false);
		((Control)panelIcon).ResumeLayout(false);
		((Control)panelIconSet).ResumeLayout(false);
		((Control)panelBoxIconSet).ResumeLayout(false);
		((Control)panelCustomIcon).ResumeLayout(false);
		((Control)panelImage).ResumeLayout(false);
		((Control)panelModeUi).ResumeLayout(false);
		((Control)panelRangeOption).ResumeLayout(false);
		((Control)panelBoxRangeEndExpression).ResumeLayout(false);
		((Control)panelBoxRangeEndExpression).PerformLayout();
		((Control)panelBoxRangeStartExpression).ResumeLayout(false);
		((Control)panelBoxRangeStartExpression).PerformLayout();
		((Control)panelRangeUi).ResumeLayout(false);
		((ISupportInitialize)gridData).EndInit();
		((Control)labelGridData).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
