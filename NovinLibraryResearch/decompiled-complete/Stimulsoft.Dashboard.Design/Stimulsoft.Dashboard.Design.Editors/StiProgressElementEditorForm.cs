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
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Components.Progress;
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
using Stimulsoft.Report.Design.Helpers;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Helper;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Dashboard.Design.Editors;

public class StiProgressElementEditorForm : StiElementEditorForm
{
	private const string ColumnValue = "Value";

	private const string ColumnTarget = "Target";

	private const string ColumnSeries = "Series";

	private StiMeterListBox selectedListBox;

	private Timer timerRefreshGrid;

	private IContainer components;

	private StiMeterListBox listBoxValues;

	private StiPanelUX panelUi;

	private ToolTip toolTip;

	private StiLabelPanel labelValue;

	private StiMeterListBox listBoxSeries;

	private StiChartGroupLabel labelSeries;

	private StiLabelUX labelMode;

	private StiOperationRadioButton buttonDataBars;

	private StiOperationRadioButton buttonPie;

	private StiOperationRadioButton buttonCircle;

	private StiMeterListBox listBoxTargets;

	private StiChartGroupLabel labelTarget;

	private StiPanelBox panelBoxExpression;

	private HintTextBox textBoxExpression;

	private StiOperationButton buttonExpressionMenu;

	private StiLabelUX labelExpression;

	private ContextMenuBar contextMenuBar;

	private ButtonItem menuExpression;

	private StiOperationButton buttonExpressionEdit;

	private StiOperationButton buttonUseDataFields;

	private StiOperationButton buttonEnterDataManually;

	private StiExcelGrid gridData;

	private StiLabelPanel labelGridData;

	internal StiProgressElement ProgressElement => base.Element as StiProgressElement;

	internal StiValueProgressMeter ProgressValue => ProgressElement?.Value;

	internal StiTargetProgressMeter ProgressTarget => ProgressElement?.Target;

	internal StiSeriesProgressMeter ProgressSeries => ProgressElement?.Series;

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
				return gridData.DataTable.Rows.Cast<DataRow>().Any((DataRow r) => !string.IsNullOrWhiteSpace(r["Series"]?.ToString()));
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

	internal StiMeterListBox ValuesListBox => listBoxValues;

	internal StiMeterListBox TargetsListBox => listBoxTargets;

	internal StiMeterListBox SeriesListBox => listBoxSeries;

	private bool IsDataEnterMode => ProgressElement.DataMode == StiDataMode.ManuallyEnteringData;

	private bool IsDataFieldsMode => ProgressElement.DataMode == StiDataMode.UsingDataFields;

	private bool IsShowMore { get; set; }

	public StiProgressElementEditorForm()
		: this(null)
	{
	}

	public StiProgressElementEditorForm(StiElement element)
		: base(element)
	{
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
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
		((Control)listBoxTargets).Invalidate();
		((Control)listBoxSeries).Invalidate();
		((Control)gridData).Invalidate();
	}

	private void LocalizeInternal()
	{
		((Control)this).Text = Loc.Get("Components", "StiProgress");
		((Control)labelValue).Text = Loc.GetMain("Value");
		((Control)labelTarget).Text = Loc.GetMain("Target");
		((Control)labelSeries).Text = Loc.Get("Chart", "Series");
		((Control)labelMode).Text = Loc.GetMain("Mode");
		((Control)labelExpression).Text = Loc.GetMain("Expression");
		((Control)labelGridData).Text = Loc.GetMain("Data");
		toolTip.SetToolTip((Control)(object)buttonCircle, Loc.GetEnum("StiProgressElementModeCircle"));
		toolTip.SetToolTip((Control)(object)buttonPie, Loc.GetEnum("StiProgressElementModePie"));
		toolTip.SetToolTip((Control)(object)buttonDataBars, Loc.GetEnum("StiProgressElementModeDataBars"));
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
			new StiProgressExpressionMenu(menuExpression, (TextBox)(object)textBoxExpression, this, selectedListBox).CreateItems();
		};
	}

	private void DoScaling()
	{
		((ButtonBase)buttonExpressionEdit).Image = StiReportDesignImages.Actions.Edit();
		((ButtonBase)buttonCircle).Image = StiReportDesignImages.Progress.Circle();
		((ButtonBase)buttonDataBars).Image = StiReportDesignImages.Progress.DataBars();
		((ButtonBase)buttonPie).Image = StiReportDesignImages.Progress.Pie();
		((ButtonBase)buttonEnterDataManually).Image = StiReportDesignImages.Actions.EditTable();
		((ButtonBase)buttonUseDataFields).Image = StiReportDesignImages.Actions.RetrieveColumnsDown();
		StiButtonsLocator.ArrangeButtons((Control)(object)panelBoxExpression);
	}

	public override void ReadPropertiesFromElement()
	{
		ReadValuesPropertiesFromElement();
		ReadTargetPropertiesFromElement();
		ReadSeriesPropertiesFromElement();
	}

	private void ReadValuesPropertiesFromElement()
	{
		if (!listBoxValues.ItemsEqualTo(ProgressValue))
		{
			((ListBox)listBoxValues).BeginUpdate();
			((ListBox)listBoxValues).Items.Clear();
			listBoxValues.AddItem(ProgressValue);
			listBoxValues.SelectFirstItem();
			((ListBox)listBoxValues).EndUpdate();
		}
	}

	private void ReadTargetPropertiesFromElement()
	{
		if (!listBoxTargets.ItemsEqualTo(ProgressTarget))
		{
			((ListBox)listBoxTargets).BeginUpdate();
			((ListBox)listBoxTargets).Items.Clear();
			listBoxTargets.AddItem(ProgressTarget);
			listBoxTargets.SelectFirstItem();
			((ListBox)listBoxTargets).EndUpdate();
		}
	}

	private void ReadSeriesPropertiesFromElement()
	{
		if (!listBoxSeries.ItemsEqualTo(ProgressSeries))
		{
			((ListBox)listBoxSeries).BeginUpdate();
			((ListBox)listBoxSeries).Items.Clear();
			listBoxSeries.AddItem(ProgressSeries);
			listBoxSeries.SelectFirstItem();
			((ListBox)listBoxSeries).EndUpdate();
		}
	}

	public override void WritePropertiesToElement()
	{
		WriteValuesPropertiesToElement();
		WriteTargetPropertiesToElement();
		WriteSeriesPropertiesToElement();
		RefreshElementView();
	}

	private void WriteValuesPropertiesToElement()
	{
		StiMeterItem stiMeterItem = listBoxValues.FirstItem();
		ProgressElement.Value = stiMeterItem?.Meter as StiValueProgressMeter;
	}

	private void WriteTargetPropertiesToElement()
	{
		StiMeterItem stiMeterItem = listBoxTargets.FirstItem();
		ProgressElement.Target = stiMeterItem?.Meter as StiTargetProgressMeter;
	}

	private void WriteSeriesPropertiesToElement()
	{
		StiMeterItem stiMeterItem = listBoxSeries.FirstItem();
		ProgressElement.Series = stiMeterItem?.Meter as StiSeriesProgressMeter;
	}

	protected override void AttachEvents()
	{
		base.AttachEvents();
		AttachEventsToValues();
		AttachEventsToTargets();
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
				listBoxValues.AddItem(StiMeterHelper.Progress.GetValue(draggedDataColumnOrVariable));
				Designer.PageView.ResetDragging();
				ProgressElement.Select();
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
				listBoxValues.AddItem(StiMeterHelper.Progress.GetValue(stiTableColumn));
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
			ProgressElement.Select();
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
				listBoxTargets.AddItem(StiMeterHelper.Progress.GetTarget(draggedDataColumnOrVariable));
				Designer.PageView.ResetDragging();
				ProgressElement.Select();
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
				listBoxTargets.AddItem(StiMeterHelper.Progress.GetValue(stiTableColumn));
				Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxTargets.SelectedMeterItem = listBoxTargets.LastItem();
				((Control)listBoxTargets).Focus();
			}
			ResetComponentHighlights();
		};
		listBoxTargets.ElementsSelectedChanged += delegate
		{
			ProgressElement.Select();
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
				listBoxSeries.AddItem(StiMeterHelper.Progress.GetSeries(draggedDataColumnOrVariable));
				Designer.PageView.ResetDragging();
				ProgressElement.Select();
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
				listBoxSeries.AddItem(StiMeterHelper.Progress.GetSeries(stiTableColumn));
				Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxSeries.SelectedMeterItem = listBoxSeries.LastItem();
				((Control)listBoxSeries).Focus();
			}
			ResetComponentHighlights();
		};
		listBoxSeries.ElementsSelectedChanged += delegate
		{
			ProgressElement.Select();
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
			((RadioButton)buttonCircle).Checked = ProgressElement != null && ProgressElement.Mode == StiProgressElementMode.Circle;
			((RadioButton)buttonPie).Checked = ProgressElement != null && ProgressElement.Mode == StiProgressElementMode.Pie;
			((RadioButton)buttonDataBars).Checked = ProgressElement != null && ProgressElement.Mode == StiProgressElementMode.DataBars;
			StiLabelPanel stiLabelPanel = labelValue;
			StiChartGroupLabel stiChartGroupLabel = labelTarget;
			StiChartGroupLabel stiChartGroupLabel2 = labelSeries;
			StiMeterListBox stiMeterListBox = listBoxTargets;
			StiMeterListBox stiMeterListBox2 = listBoxSeries;
			StiLabelUX stiLabelUX = labelExpression;
			StiPanelBox stiPanelBox = panelBoxExpression;
			bool flag2 = (((Control)buttonEnterDataManually).Visible = IsAnyPresent && IsDataFieldsMode);
			bool flag4 = (((Control)stiPanelBox).Visible = flag2);
			bool flag6 = (((Control)stiLabelUX).Visible = flag4);
			bool flag8 = (((Control)stiMeterListBox2).Visible = flag6);
			bool flag10 = (((Control)stiMeterListBox).Visible = flag8);
			bool flag12 = (((Control)stiChartGroupLabel2).Visible = flag10);
			bool visible = (((Control)stiChartGroupLabel).Visible = flag12);
			((Control)stiLabelPanel).Visible = visible;
			((Control)panelUi).Visible = (IsAnyPresent && IsDataFieldsMode) || IsDataEnterMode;
			StiMeterListBox stiMeterListBox3 = listBoxValues;
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
		if (selectedListBox == listBoxValues && ProgressValue != null)
		{
			((Control)textBoxExpression).Text = ProgressValue.Expression;
		}
		else if (selectedListBox == listBoxTargets && ProgressTarget != null)
		{
			((Control)textBoxExpression).Text = ProgressTarget.Expression;
		}
		else if (selectedListBox == listBoxSeries && ProgressSeries != null)
		{
			((Control)textBoxExpression).Text = ProgressSeries.Expression;
		}
		else
		{
			((Control)textBoxExpression).Text = string.Empty;
		}
		base.LockRefresh = false;
	}

	private Task BuildDataGridAsync()
	{
		return Task.Factory.StartNew(delegate
		{
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Expected O, but got Unknown
			string[] columns = new string[3] { "Value", "Target", "Series" };
			List<string[]> rows = StiStringsTableHelper.LoadStringRowsFromPackedString(ProgressElement.ManuallyEnteredData, columns);
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
		ProgressElement.ManuallyEnteredData = StiStringsTableHelper.SaveDataTableToPackedString(gridData.DataTable);
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
			stiMeterItem.Meter = StiMeterHelper.Progress.GetValue(stiMeterItem.Meter);
		}
		if (stiMeterItem.List == listBoxTargets)
		{
			stiMeterItem.Meter = StiMeterHelper.Progress.GetTarget(stiMeterItem.Meter);
		}
		if (stiMeterItem.List == listBoxSeries)
		{
			stiMeterItem.Meter = StiMeterHelper.Progress.GetSeries(stiMeterItem.Meter);
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

	protected override void OnGlobalElementChanged(object sender, EventArgs e)
	{
		if (sender == base.Element)
		{
			ReadPropertiesFromElement();
			listBoxValues.SelectedMeterItem = listBoxValues.FirstItem();
		}
	}

	private void ButtonProgressMode_CheckedChanged(object sender, EventArgs e)
	{
		if (((RadioButton)buttonCircle).Checked)
		{
			ProgressElement.Mode = StiProgressElementMode.Circle;
			WritePropertiesToElement();
			RefreshUiPanel();
		}
		else if (((RadioButton)buttonPie).Checked)
		{
			ProgressElement.Mode = StiProgressElementMode.Pie;
			WritePropertiesToElement();
			RefreshUiPanel();
		}
		else if (((RadioButton)buttonDataBars).Checked)
		{
			ProgressElement.Mode = StiProgressElementMode.DataBars;
			WritePropertiesToElement();
			RefreshUiPanel();
		}
		RefreshElementView();
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
		ProgressElement.DataMode = StiDataMode.ManuallyEnteringData;
		RefreshElementView();
		RefreshUiPanel();
		((Control)gridData).Focus();
	}

	private void ButtonUseDataFields_Click(object sender, EventArgs e)
	{
		IsShowMore = true;
		ProgressElement.DataMode = StiDataMode.UsingDataFields;
		RefreshElementView();
		RefreshUiPanel();
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/dashboards_progress.htm");
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
			if (selectedListBox == listBoxValues && ProgressValue != null)
			{
				ProgressValue.Expression = ((Control)textBoxExpression).Text;
				listBoxValues.SetSelectedText(StiLabelHelper.GetLabel(ProgressValue));
			}
			if (selectedListBox == listBoxTargets && ProgressTarget != null)
			{
				ProgressTarget.Expression = ((Control)textBoxExpression).Text;
				listBoxTargets.SetSelectedText(StiLabelHelper.GetLabel(ProgressTarget));
			}
			if (selectedListBox == listBoxSeries && ProgressSeries != null)
			{
				ProgressSeries.Expression = ((Control)textBoxExpression).Text;
				listBoxSeries.SetSelectedText(StiLabelHelper.GetLabel(ProgressSeries));
			}
			RefreshExpressionHint();
		}
	}

	private void ButtonExpressionMenu_Click(object sender, EventArgs e)
	{
		new StiProgressExpressionMenu(menuExpression, (TextBox)(object)textBoxExpression, this, selectedListBox).CreateItems();
		Point point = new Point(((Control)panelBoxExpression).Width, ((Control)panelBoxExpression).Height + StiScale.I1);
		if (StiLeftRightHandedHelper.IsLeftHanded)
		{
			point.X -= menuExpression.PopupSize.Width;
		}
		point = ((Control)panelBoxExpression).PointToScreen(point);
		menuExpression.Popup(point);
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
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Expected O, but got Unknown
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Expected O, but got Unknown
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_045e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0468: Expected O, but got Unknown
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_0520: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Expected O, but got Unknown
		//IL_054b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0633: Unknown result type (might be due to invalid IL or missing references)
		//IL_068e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0698: Expected O, but got Unknown
		//IL_06b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0767: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d2: Expected O, but got Unknown
		//IL_0816: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_093f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0949: Expected O, but got Unknown
		//IL_098c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b12: Expected O, but got Unknown
		//IL_0b33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c58: Expected O, but got Unknown
		//IL_0c85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d83: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d8d: Expected O, but got Unknown
		//IL_0dba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2f: Expected O, but got Unknown
		//IL_0e4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f36: Expected O, but got Unknown
		//IL_0fc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fcf: Expected O, but got Unknown
		//IL_1049: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1230: Unknown result type (might be due to invalid IL or missing references)
		//IL_124e: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiProgressElementEditorForm));
		listBoxValues = new StiMeterListBox();
		panelUi = new StiPanelUX();
		labelMode = new StiLabelUX();
		buttonDataBars = new StiOperationRadioButton();
		buttonPie = new StiOperationRadioButton();
		buttonCircle = new StiOperationRadioButton();
		panelBoxExpression = new StiPanelBox();
		buttonExpressionEdit = new StiOperationButton();
		textBoxExpression = new HintTextBox();
		buttonExpressionMenu = new StiOperationButton();
		labelExpression = new StiLabelUX();
		contextMenuBar = new ContextMenuBar();
		menuExpression = new ButtonItem();
		toolTip = new ToolTip(components);
		labelValue = new StiLabelPanel();
		buttonEnterDataManually = new StiOperationButton();
		listBoxSeries = new StiMeterListBox();
		labelSeries = new StiChartGroupLabel();
		listBoxTargets = new StiMeterListBox();
		labelTarget = new StiChartGroupLabel();
		buttonUseDataFields = new StiOperationButton();
		gridData = new StiExcelGrid();
		labelGridData = new StiLabelPanel();
		((Control)panelUi).SuspendLayout();
		((Control)panelBoxExpression).SuspendLayout();
		((ISupportInitialize)contextMenuBar).BeginInit();
		((Control)labelValue).SuspendLayout();
		((ISupportInitialize)gridData).BeginInit();
		((Control)labelGridData).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)listBoxValues).AllowDrop = true;
		((Control)listBoxValues).Dock = (DockStyle)1;
		((Control)listBoxValues).Location = new Point(24, 186);
		((Control)listBoxValues).Margin = new Padding(4, 4, 4, 4);
		((Control)listBoxValues).Name = "listBoxValues";
		((Control)listBoxValues).Size = new Size(530, 95);
		((Control)listBoxValues).TabIndex = 1;
		((ListBox)listBoxValues).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxValues).Enter += ListBox_Enter;
		((Control)listBoxValues).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)panelUi).BackColor = Color.White;
		((Control)panelUi).Controls.Add((Control)(object)labelMode);
		((Control)panelUi).Controls.Add((Control)(object)buttonDataBars);
		((Control)panelUi).Controls.Add((Control)(object)buttonPie);
		((Control)panelUi).Controls.Add((Control)(object)buttonCircle);
		((Control)panelUi).Dock = (DockStyle)2;
		((Control)panelUi).Location = new Point(24, 763);
		((Control)panelUi).Margin = new Padding(4, 4, 4, 4);
		((Control)panelUi).Name = "panelUi";
		((Control)panelUi).Size = new Size(530, 81);
		((Control)panelUi).TabIndex = 8;
		((Control)labelMode).ForeColor = Color.DimGray;
		((Control)labelMode).Location = new Point(0, 21);
		((Control)labelMode).Margin = new Padding(4, 0, 4, 0);
		((Control)labelMode).Name = "labelMode";
		((Control)labelMode).Size = new Size(164, 50);
		((Control)labelMode).TabIndex = 4;
		((Control)labelMode).Text = "Mode";
		((Label)labelMode).TextAlign = (ContentAlignment)16;
		((RadioButton)buttonDataBars).Appearance = (Appearance)1;
		((ButtonBase)buttonDataBars).FlatAppearance.BorderSize = 0;
		((ButtonBase)buttonDataBars).Image = (Image)componentResourceManager.GetObject("buttonDataBars.Image");
		((Control)buttonDataBars).Location = new Point(300, 21);
		((Control)buttonDataBars).Margin = new Padding(4, 4, 4, 4);
		((Control)buttonDataBars).Name = "buttonDataBars";
		((Control)buttonDataBars).Size = new Size(52, 50);
		((Control)buttonDataBars).TabIndex = 3;
		((ButtonBase)buttonDataBars).UseVisualStyleBackColor = true;
		((RadioButton)buttonDataBars).CheckedChanged += ButtonProgressMode_CheckedChanged;
		((RadioButton)buttonPie).Appearance = (Appearance)1;
		((ButtonBase)buttonPie).FlatAppearance.BorderSize = 0;
		((ButtonBase)buttonPie).Image = (Image)componentResourceManager.GetObject("buttonPie.Image");
		((Control)buttonPie).Location = new Point(238, 21);
		((Control)buttonPie).Margin = new Padding(4, 4, 4, 4);
		((Control)buttonPie).Name = "buttonPie";
		((Control)buttonPie).Size = new Size(52, 50);
		((Control)buttonPie).TabIndex = 2;
		((ButtonBase)buttonPie).UseVisualStyleBackColor = true;
		((RadioButton)buttonPie).CheckedChanged += ButtonProgressMode_CheckedChanged;
		((RadioButton)buttonCircle).Appearance = (Appearance)1;
		((RadioButton)buttonCircle).Checked = true;
		((ButtonBase)buttonCircle).FlatAppearance.BorderSize = 0;
		((ButtonBase)buttonCircle).Image = (Image)componentResourceManager.GetObject("buttonCircle.Image");
		((Control)buttonCircle).Location = new Point(178, 21);
		((Control)buttonCircle).Margin = new Padding(4, 4, 4, 4);
		((Control)buttonCircle).Name = "buttonCircle";
		((Control)buttonCircle).Size = new Size(52, 50);
		((Control)buttonCircle).TabIndex = 1;
		((RadioButton)buttonCircle).TabStop = true;
		((ButtonBase)buttonCircle).UseVisualStyleBackColor = true;
		((RadioButton)buttonCircle).CheckedChanged += ButtonProgressMode_CheckedChanged;
		((Control)panelBoxExpression).BackColor = Color.White;
		((Control)panelBoxExpression).Controls.Add((Control)(object)buttonExpressionEdit);
		((Control)panelBoxExpression).Controls.Add((Control)(object)textBoxExpression);
		((Control)panelBoxExpression).Controls.Add((Control)(object)buttonExpressionMenu);
		((Control)panelBoxExpression).Dock = (DockStyle)1;
		((Control)panelBoxExpression).Location = new Point(24, 657);
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
		((Control)textBoxExpression).Location = new Point(8, 12);
		((Control)textBoxExpression).Margin = new Padding(4, 4, 4, 4);
		((Control)textBoxExpression).Name = "textBoxExpression";
		((Control)textBoxExpression).Size = new Size(422, 24);
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
		((Control)labelExpression).Dock = (DockStyle)1;
		((Control)labelExpression).ForeColor = Color.DimGray;
		((Control)labelExpression).Location = new Point(24, 595);
		((Control)labelExpression).Margin = new Padding(4, 0, 4, 0);
		((Control)labelExpression).Name = "labelExpression";
		((Control)labelExpression).Size = new Size(530, 62);
		((Control)labelExpression).TabIndex = 6;
		((Control)labelExpression).Text = "Expression";
		((Label)labelExpression).TextAlign = (ContentAlignment)16;
		contextMenuBar.AntiAlias = true;
		contextMenuBar.DockTabStripHeight = 50;
		((Control)contextMenuBar).Font = new Font("Segoe UI", 9f);
		contextMenuBar.Items.AddRange(new BaseItem[1] { menuExpression });
		contextMenuBar.Location = new Point(412, 681);
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
		((Control)labelValue).Controls.Add((Control)(object)buttonEnterDataManually);
		((Control)labelValue).Dock = (DockStyle)1;
		((Control)labelValue).Location = new Point(24, 124);
		((Control)labelValue).Margin = new Padding(4, 0, 4, 0);
		((Control)labelValue).Name = "labelValue";
		((Control)labelValue).Size = new Size(530, 62);
		((Control)labelValue).TabIndex = 0;
		((Control)labelValue).Text = "Value";
		labelValue.TextAlign = (ContentAlignment)16;
		((Control)buttonEnterDataManually).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonEnterDataManually).Image = (Image)componentResourceManager.GetObject("buttonEnterDataManually.Image");
		((Control)buttonEnterDataManually).Location = new Point(483, 9);
		((Control)buttonEnterDataManually).Margin = new Padding(4, 4, 4, 4);
		((Control)buttonEnterDataManually).Name = "buttonEnterDataManually";
		((Control)buttonEnterDataManually).Size = new Size(48, 44);
		((Control)buttonEnterDataManually).TabIndex = 22;
		((Control)buttonEnterDataManually).TabStop = false;
		((ButtonBase)buttonEnterDataManually).UseVisualStyleBackColor = true;
		((Control)buttonEnterDataManually).Click += ButtonEnterDataManually_Click;
		((Control)listBoxSeries).AllowDrop = true;
		((Control)listBoxSeries).Dock = (DockStyle)1;
		((Control)listBoxSeries).Location = new Point(24, 500);
		((Control)listBoxSeries).Margin = new Padding(4, 4, 4, 4);
		((Control)listBoxSeries).Name = "listBoxSeries";
		((Control)listBoxSeries).Size = new Size(530, 95);
		((Control)listBoxSeries).TabIndex = 5;
		((ListBox)listBoxSeries).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxSeries).Enter += ListBox_Enter;
		((Control)listBoxSeries).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)labelSeries).Dock = (DockStyle)1;
		((Control)labelSeries).Location = new Point(24, 438);
		((Control)labelSeries).Margin = new Padding(4, 0, 4, 0);
		((Control)labelSeries).Name = "labelSeries";
		((Control)labelSeries).Size = new Size(530, 62);
		((Control)labelSeries).TabIndex = 4;
		((Control)labelSeries).Text = "Series";
		((Control)listBoxTargets).AllowDrop = true;
		((Control)listBoxTargets).Dock = (DockStyle)1;
		((Control)listBoxTargets).Location = new Point(24, 343);
		((Control)listBoxTargets).Margin = new Padding(4, 4, 4, 4);
		((Control)listBoxTargets).Name = "listBoxTargets";
		((Control)listBoxTargets).Size = new Size(530, 95);
		((Control)listBoxTargets).TabIndex = 3;
		((ListBox)listBoxTargets).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxTargets).Enter += ListBox_Enter;
		((Control)listBoxTargets).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)labelTarget).Dock = (DockStyle)1;
		((Control)labelTarget).Location = new Point(24, 281);
		((Control)labelTarget).Margin = new Padding(4, 0, 4, 0);
		((Control)labelTarget).Name = "labelTarget";
		((Control)labelTarget).Size = new Size(530, 62);
		((Control)labelTarget).TabIndex = 2;
		((Control)labelTarget).Text = "Target";
		((Control)buttonUseDataFields).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonUseDataFields).Image = (Image)componentResourceManager.GetObject("buttonUseDataFields.Image");
		((Control)buttonUseDataFields).Location = new Point(483, 8);
		((Control)buttonUseDataFields).Margin = new Padding(4, 4, 4, 4);
		((Control)buttonUseDataFields).Name = "buttonUseDataFields";
		((Control)buttonUseDataFields).Size = new Size(48, 44);
		((Control)buttonUseDataFields).TabIndex = 21;
		((Control)buttonUseDataFields).TabStop = false;
		((ButtonBase)buttonUseDataFields).UseVisualStyleBackColor = true;
		((Control)buttonUseDataFields).Click += ButtonUseDataFields_Click;
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
		((Control)gridData).TabIndex = 20;
		((Control)labelGridData).Controls.Add((Control)(object)buttonUseDataFields);
		((Control)labelGridData).Dock = (DockStyle)1;
		((Control)labelGridData).Location = new Point(24, 23);
		((Control)labelGridData).Margin = new Padding(4, 0, 4, 0);
		((Control)labelGridData).Name = "labelGridData";
		((Control)labelGridData).Size = new Size(530, 61);
		((Control)labelGridData).TabIndex = 23;
		((Control)labelGridData).Text = "Data";
		labelGridData.TextAlign = (ContentAlignment)16;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(578, 867);
		((Control)this).Controls.Add((Control)(object)panelBoxExpression);
		((Control)this).Controls.Add((Control)(object)contextMenuBar);
		((Control)this).Controls.Add((Control)(object)labelExpression);
		((Control)this).Controls.Add((Control)(object)listBoxSeries);
		((Control)this).Controls.Add((Control)(object)labelSeries);
		((Control)this).Controls.Add((Control)(object)listBoxTargets);
		((Control)this).Controls.Add((Control)(object)labelTarget);
		((Control)this).Controls.Add((Control)(object)listBoxValues);
		((Control)this).Controls.Add((Control)(object)panelUi);
		((Control)this).Controls.Add((Control)(object)labelValue);
		((Control)this).Controls.Add((Control)(object)gridData);
		((Control)this).Controls.Add((Control)(object)labelGridData);
		((Form)this).Margin = new Padding(2, 2, 2, 2);
		((Control)this).Name = "StiProgressElementEditorForm";
		((Control)this).Padding = new Padding(24, 23, 24, 23);
		((Control)this).Text = "Progress";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Control)panelUi).ResumeLayout(false);
		((Control)panelBoxExpression).ResumeLayout(false);
		((Control)panelBoxExpression).PerformLayout();
		((ISupportInitialize)contextMenuBar).EndInit();
		((Control)labelValue).ResumeLayout(false);
		((ISupportInitialize)gridData).EndInit();
		((Control)labelGridData).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
