using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Components.PivotTable;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Dashboard.Design.Controls;
using Stimulsoft.Dashboard.Design.Editors.Helpers;
using Stimulsoft.Dashboard.Design.Editors.Menu;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.CrossTab.Core;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Design.Panels;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Dashboard.Design.Editors;

public class StiPivotTableElementDesignerForm : StiElementEditorForm
{
	private StiMeterListBox selectedListBox;

	private IContainer components;

	private StiPanelUX panelOptions;

	internal StiMeterListBox listBoxRows;

	internal StiMeterListBox listBoxColumns;

	private StiChartGroupLabel labelColumns;

	internal StiMeterListBox listBoxSummaries;

	private StiChartGroupLabel labelSummary;

	private ToolTip toolTip1;

	private ImageList ilDirection;

	private StiPanelBox panelBoxExpression;

	private HintTextBox textBoxExpression;

	private StiOperationButton buttonExpressionMenu;

	private StiLabelUX labelExpression;

	private ContextMenuBar contextMenuBar;

	private ButtonItem menuExpression;

	private StiOperationButton buttonSwap;

	private StiPanelUX panel1;

	private StiPanelUX panel2;

	private StiChartGroupLabel labelRows;

	private StiLabelUX labelTopN;

	private StiPanelBox panelTopN;

	private StiLabelUX labelTopNStatus;

	private StiOperationButton buttonClearTopN;

	private StiOperationButton buttonEditTopN;

	private StiOperationButton buttonExpressionEdit;

	private StiOperationButton buttonSummaryDirection;

	public override bool AllowResize => false;

	internal bool IsColumnsPresent => ((ListBox)listBoxColumns).Items.Count > 0;

	internal bool IsRowsPresent => ((ListBox)listBoxRows).Items.Count > 0;

	internal bool IsSummaryPresent => ((ListBox)listBoxSummaries).Items.Count > 0;

	public override bool IsAnyPresent => true;

	internal StiPivotColumn PivotColumn => listBoxColumns.SelectedMeterItem?.Meter as StiPivotColumn;

	internal StiPivotRow PivotRow => listBoxRows.SelectedMeterItem?.Meter as StiPivotRow;

	internal StiPivotSummary PivotSummary => listBoxSummaries.SelectedMeterItem?.Meter as StiPivotSummary;

	internal StiPivotTableElement PivotTableElement => base.Element as StiPivotTableElement;

	public StiPivotTableElementDesignerForm()
		: this(null)
	{
	}

	public StiPivotTableElementDesignerForm(StiPivotTableElement element)
		: base(element)
	{
		InitializeComponent();
		if (StiOptions.Appearance.ShowFormsHelp)
		{
			((Form)this).HelpButton = true;
		}
		selectedListBox = listBoxColumns;
		if (!((Control)this).MinimumSize.IsEmpty)
		{
			((Form)this).Size = ((Control)this).MinimumSize;
		}
		foreach (StiMeterListBox item in (from Control c in (IEnumerable)((Control)this).Controls
			where c is StiMeterListBox
			select c).Cast<StiMeterListBox>())
		{
			item.Designer = Designer;
			item.Editor = this;
			item.AllowImage = false;
			contextMenuBar.SetContextMenuEx((Control)(object)item, menuExpression);
		}
		LocalizeInternal();
		InitializeMenus();
		ActivateDictionaryPanel();
		DoScaling();
		((ListBox)listBoxColumns).SelectedIndexChanged += delegate
		{
			StiPropertiesPanelHelper.SetPropertyGrid(Designer, PivotColumn);
		};
		((ListBox)listBoxRows).SelectedIndexChanged += delegate
		{
			StiPropertiesPanelHelper.SetPropertyGrid(Designer, PivotRow);
		};
		((ListBox)listBoxSummaries).SelectedIndexChanged += delegate
		{
			StiPropertiesPanelHelper.SetPropertyGrid(Designer, PivotSummary);
		};
	}

	public override void Localize()
	{
		LocalizeInternal();
		((Control)listBoxColumns).Invalidate();
		((Control)listBoxRows).Invalidate();
		((Control)listBoxSummaries).Invalidate();
	}

	private void LocalizeInternal()
	{
		((Control)this).Text = Loc.Get("Components", "StiPivotTable");
		((Control)labelExpression).Text = Loc.Get("PropertyMain", "Expression");
		((Control)labelColumns).Text = StiLocalization.Get("FormCrossTabDesigner", "Columns").Replace(":", "");
		((Control)labelRows).Text = StiLocalization.Get("FormCrossTabDesigner", "Rows").Replace(":", "");
		((Control)labelSummary).Text = StiLocalization.Get("FormCrossTabDesigner", "Summary").Replace(":", "");
		((Control)labelTopN).Text = Loc.Get("PropertyMain", "TopN");
		toolTip1.SetToolTip((Control)(object)buttonSwap, StiLocalization.Get("FormCrossTabDesigner", "Swap"));
		toolTip1.SetToolTip((Control)(object)buttonSummaryDirection, StiLocalization.Get("PropertyMain", "Direction"));
	}

	private void InitializeMenus()
	{
		menuExpression.PopupOpen += delegate
		{
			new StiPivotTableExpressionMenu(menuExpression, (TextBox)(object)textBoxExpression, this, selectedListBox).CreateItems();
		};
	}

	internal void SwitchToPercentOfGrandTotal()
	{
		((Control)textBoxExpression).Focus();
		if (PivotSummary?.TextFormat != null && !(PivotSummary.TextFormat is StiPercentageFormatService))
		{
			PivotSummary.TextFormat = new StiPercentageFormatService();
			Designer.InvokeRefreshDesigner();
		}
	}

	private void DoScaling()
	{
		((ButtonBase)buttonClearTopN).Image = StiReportDesignImages.Actions.Remove();
		((ButtonBase)buttonEditTopN).Image = StiReportDesignImages.Actions.Edit();
		((ButtonBase)buttonExpressionEdit).Image = StiReportDesignImages.Actions.Edit();
		((ButtonBase)buttonSwap).Image = StiReportDesignImages.Actions.Swap();
		((ButtonBase)buttonSummaryDirection).Image = ((PivotTableElement.SummaryDirection == StiSummaryDirection.LeftToRight) ? StiReportDesignImages.Actions.HorizontalPlacement() : StiReportDesignImages.Actions.VerticalPlacement());
		StiButtonsLocator.ArrangeButtons((Control)(object)panelBoxExpression);
		StiButtonsLocator.ArrangeButtons((Control)(object)panelTopN);
	}

	public override void ReadPropertiesFromElement()
	{
		ReadSummariesPropertiesFromElement();
		ReadRowsPropertiesFromElement();
		ReadColumnsPropertiesFromElement();
	}

	private void ReadRowsPropertiesFromElement()
	{
		((ListBox)listBoxRows).BeginUpdate();
		((ListBox)listBoxRows).Items.Clear();
		listBoxRows.AddItems(PivotTableElement.Rows);
		listBoxRows.SelectFirstItem();
		((ListBox)listBoxRows).EndUpdate();
	}

	private void ReadColumnsPropertiesFromElement()
	{
		((ListBox)listBoxColumns).BeginUpdate();
		((ListBox)listBoxColumns).Items.Clear();
		listBoxColumns.AddItems(PivotTableElement.Columns);
		listBoxColumns.SelectFirstItem();
		((ListBox)listBoxColumns).EndUpdate();
	}

	private void ReadSummariesPropertiesFromElement()
	{
		((ListBox)listBoxSummaries).BeginUpdate();
		((ListBox)listBoxSummaries).Items.Clear();
		listBoxSummaries.AddItems(PivotTableElement.Summaries);
		listBoxSummaries.SelectFirstItem();
		((ListBox)listBoxSummaries).EndUpdate();
	}

	public override void WritePropertiesToElement()
	{
		WriteColumnsPropertiesToElement();
		WriteRowsPropertiesToElement();
		WriteSummaryPropertiesToElement();
		StiPivotTableToCrossTabCache.Remove(PivotTableElement);
		RefreshElementView();
	}

	private void WriteColumnsPropertiesToElement()
	{
		PivotTableElement.Columns = (from t in listBoxColumns.ToItems()
			select t.Meter).Cast<StiPivotColumn>().ToList();
	}

	private void WriteRowsPropertiesToElement()
	{
		PivotTableElement.Rows = (from t in listBoxRows.ToItems()
			select t.Meter).Cast<StiPivotRow>().ToList();
	}

	private void WriteSummaryPropertiesToElement()
	{
		PivotTableElement.Summaries = (from t in listBoxSummaries.ToItems()
			select t.Meter).Cast<StiPivotSummary>().ToList();
	}

	protected override void AttachEvents()
	{
		base.AttachEvents();
		AttachEventsToRows();
		AttachEventsToColumns();
		AttachEventsToSummary();
		ReadPropertiesFromElement();
		RefreshUiPanel();
	}

	private void AttachEventsToRows()
	{
		listBoxRows.ElementsConverting += ListBox_ElementsConverting;
		listBoxRows.ElementsChanged += ListBox_ElementsChanged;
		listBoxRows.ElementsSelectedChanged += ListBox_ElementsAfterSelect;
		listBoxRows.ElementsDragEnter += ListBox_ElementsDragEnter;
		listBoxRows.ElementsDragDrop += delegate
		{
			StiPivotTableElementSelection.ResetSelection(PivotTableElement);
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(Designer))
			{
				IStiAppDataCell draggedDataColumnOrVariable = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
				if (draggedDataColumnOrVariable == null)
				{
					return;
				}
				StiPivotRow row = StiMeterHelper.Pivot.GetRow(draggedDataColumnOrVariable);
				StiMeterItem stiMeterItem = new StiMeterItem(StiLabelHelper.GetLabel(row), row, StiMeterItemImage.Measure);
				StiMeterItem itemAtMouse = listBoxRows.GetItemAtMouse();
				if (itemAtMouse != null)
				{
					int index = ((ListBox)listBoxRows).Items.IndexOf((object)itemAtMouse);
					listBoxRows.InsertItem(index, stiMeterItem);
				}
				else
				{
					listBoxRows.AddItem(stiMeterItem);
				}
				Designer.PageView.ResetDragging();
				PivotTableElement.Select();
				WritePropertiesToElement();
				listBoxRows.SelectedMeterItem = stiMeterItem;
				((Control)listBoxRows).Focus();
				StiPivotTableElementSelection.Select(PivotTableElement, row);
			}
			else if (StiDragDropColumnHelper.IsTableElementDragged(Designer))
			{
				if (!(StiDragDropColumnHelper.GetDraggedTableElement(Designer) is StiTableElement stiTableElement))
				{
					return;
				}
				int count = ((ListBox)listBoxRows).Items.Count;
				foreach (StiTableColumn column in stiTableElement.Columns)
				{
					PivotTableElement.Rows.Add(StiMeterHelper.Pivot.GetRow(column));
				}
				RefreshElementView();
				ReadPropertiesFromElement();
				Designer.PageView.ResetDragging();
				((ListControl)listBoxRows).SelectedIndex = count;
			}
			ResetComponentHighlights();
			RefreshUiPanel();
		};
		listBoxRows.ElementsSelectedChanged += delegate
		{
			PivotTableElement.Select();
			StiPivotTableElementSelection.ResetSelection(PivotTableElement);
			if (((ListBox)listBoxRows).SelectedItem != null)
			{
				StiPivotTableElementSelection.Select(PivotTableElement, (((ListBox)listBoxRows).SelectedItem as StiMeterItem).Meter);
			}
			RefreshElementView();
		};
	}

	private void AttachEventsToColumns()
	{
		listBoxColumns.ElementsConverting += ListBox_ElementsConverting;
		listBoxColumns.ElementsChanged += ListBox_ElementsChanged;
		listBoxColumns.ElementsSelectedChanged += ListBox_ElementsAfterSelect;
		listBoxColumns.ElementsDragEnter += ListBox_ElementsDragEnter;
		listBoxColumns.ElementsDragDrop += delegate
		{
			StiPivotTableElementSelection.ResetSelection(PivotTableElement);
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(Designer))
			{
				IStiAppDataCell draggedDataColumnOrVariable = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
				if (draggedDataColumnOrVariable == null)
				{
					return;
				}
				StiPivotColumn column = StiMeterHelper.Pivot.GetColumn(draggedDataColumnOrVariable);
				StiMeterItem stiMeterItem = new StiMeterItem(StiLabelHelper.GetLabel(column), column, StiMeterItemImage.Measure);
				StiMeterItem itemAtMouse = listBoxColumns.GetItemAtMouse();
				if (itemAtMouse != null)
				{
					int index = ((ListBox)listBoxColumns).Items.IndexOf((object)itemAtMouse);
					listBoxColumns.InsertItem(index, stiMeterItem);
				}
				else
				{
					listBoxColumns.AddItem(stiMeterItem);
				}
				Designer.PageView.ResetDragging();
				PivotTableElement.Select();
				WritePropertiesToElement();
				listBoxColumns.SelectedMeterItem = stiMeterItem;
				((Control)listBoxColumns).Focus();
				StiPivotTableElementSelection.Select(PivotTableElement, column);
				RefreshUiPanel();
			}
			else if (StiDragDropColumnHelper.IsTableElementDragged(Designer))
			{
				if (!(StiDragDropColumnHelper.GetDraggedTableElement(Designer) is StiTableElement stiTableElement))
				{
					return;
				}
				int count = ((ListBox)listBoxColumns).Items.Count;
				foreach (StiTableColumn column2 in stiTableElement.Columns)
				{
					PivotTableElement.Columns.Add(StiMeterHelper.Pivot.GetColumn(column2));
				}
				RefreshElementView();
				ReadPropertiesFromElement();
				Designer.PageView.ResetDragging();
				((ListControl)listBoxColumns).SelectedIndex = count;
			}
			ResetComponentHighlights();
		};
		listBoxColumns.ElementsSelectedChanged += delegate
		{
			PivotTableElement.Select();
			StiPivotTableElementSelection.ResetSelection(PivotTableElement);
			if (((ListBox)listBoxColumns).SelectedItem != null)
			{
				StiPivotTableElementSelection.Select(PivotTableElement, (((ListBox)listBoxColumns).SelectedItem as StiMeterItem).Meter);
			}
			RefreshElementView();
		};
	}

	private void AttachEventsToSummary()
	{
		listBoxSummaries.ElementsConverting += ListBox_ElementsConverting;
		listBoxSummaries.ElementsChanged += ListBox_ElementsChanged;
		listBoxSummaries.ElementsSelectedChanged += ListBox_ElementsAfterSelect;
		listBoxSummaries.ElementsDragEnter += ListBox_ElementsDragEnter;
		listBoxSummaries.ElementsDragDrop += delegate
		{
			StiPivotTableElementSelection.ResetSelection(PivotTableElement);
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(Designer))
			{
				IStiAppDataCell draggedDataColumnOrVariable = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
				if (draggedDataColumnOrVariable == null)
				{
					return;
				}
				StiPivotSummary summary = StiMeterHelper.Pivot.GetSummary(draggedDataColumnOrVariable);
				StiMeterItem stiMeterItem = new StiMeterItem(StiLabelHelper.GetLabel(summary), summary, StiMeterItemImage.Measure);
				StiMeterItem itemAtMouse = listBoxSummaries.GetItemAtMouse();
				if (itemAtMouse != null)
				{
					int index = ((ListBox)listBoxSummaries).Items.IndexOf((object)itemAtMouse);
					listBoxSummaries.InsertItem(index, stiMeterItem);
				}
				else
				{
					listBoxSummaries.AddItem(stiMeterItem);
				}
				Designer.PageView.ResetDragging();
				PivotTableElement.Select();
				WritePropertiesToElement();
				listBoxSummaries.SelectedMeterItem = stiMeterItem;
				((Control)listBoxSummaries).Focus();
				StiPivotTableElementSelection.Select(PivotTableElement, summary);
				RefreshUiPanel();
			}
			else if (StiDragDropColumnHelper.IsTableElementDragged(Designer))
			{
				if (!(StiDragDropColumnHelper.GetDraggedTableElement(Designer) is StiTableElement stiTableElement))
				{
					return;
				}
				int count = ((ListBox)listBoxSummaries).Items.Count;
				foreach (StiTableColumn column in stiTableElement.Columns)
				{
					PivotTableElement.Summaries.Add(StiMeterHelper.Pivot.GetSummary(column));
				}
				RefreshElementView();
				ReadPropertiesFromElement();
				Designer.PageView.ResetDragging();
				((ListControl)listBoxSummaries).SelectedIndex = count;
			}
			ResetComponentHighlights();
		};
		listBoxSummaries.ElementsSelectedChanged += delegate
		{
			PivotTableElement.Select();
			StiPivotTableElementSelection.ResetSelection(PivotTableElement);
			if (((ListBox)listBoxSummaries).SelectedItem != null)
			{
				StiPivotTableElementSelection.Select(PivotTableElement, (((ListBox)listBoxSummaries).SelectedItem as StiMeterItem).Meter);
			}
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
			StiPanelUX stiPanelUX = panelOptions;
			StiLabelUX stiLabelUX = labelExpression;
			bool flag2 = (((Control)panelBoxExpression).Visible = IsColumnsPresent || IsRowsPresent || IsSummaryPresent);
			bool visible = (((Control)stiLabelUX).Visible = flag2);
			((Control)stiPanelUX).Visible = visible;
			((Control)buttonSwap).Visible = IsColumnsPresent || IsRowsPresent;
			((Control)buttonSummaryDirection).Visible = ((ListBox)listBoxSummaries).Items.Count > 1;
			SetListHeightFromCount(listBoxColumns, multiple: true);
			SetListHeightFromCount(listBoxRows, multiple: true);
			SetListHeightFromCount(listBoxSummaries, multiple: true);
			CalculateWindowHeight();
			CalculateWindowPosition();
		}
		finally
		{
			((Control)this).ResumeLayout();
			base.LockRefresh = false;
		}
	}

	private void RefreshExpressionHint()
	{
		if (selectedListBox == listBoxSummaries)
		{
			textBoxExpression.Hint = "Sum(" + Loc.GetMain("Field") + ")";
		}
		else if (selectedListBox == listBoxRows || selectedListBox == listBoxColumns)
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
		if (selectedListBox == listBoxColumns)
		{
			((Control)textBoxExpression).Text = ((PivotColumn != null) ? PivotColumn.Expression : string.Empty);
		}
		else if (selectedListBox == listBoxRows)
		{
			((Control)textBoxExpression).Text = ((PivotRow != null) ? PivotRow.Expression : string.Empty);
		}
		else if (selectedListBox == listBoxSummaries)
		{
			((Control)textBoxExpression).Text = ((PivotSummary != null) ? PivotSummary.Expression : string.Empty);
		}
		else
		{
			((Control)textBoxExpression).Text = string.Empty;
		}
		base.LockRefresh = false;
	}

	internal void RefreshTopN()
	{
		base.LockRefresh = true;
		IStiDataTopN stiDataTopN = null;
		if (selectedListBox == listBoxColumns)
		{
			stiDataTopN = PivotColumn;
		}
		else if (selectedListBox == listBoxRows)
		{
			stiDataTopN = PivotRow;
		}
		if (stiDataTopN != null)
		{
			((Control)labelTopNStatus).Text = stiDataTopN.TopN.ToString();
			((Control)buttonClearTopN).Enabled = stiDataTopN.TopN.Mode != StiDataTopNMode.None;
			((Control)panelTopN).Enabled = true;
			((Control)labelTopN).Enabled = true;
		}
		else
		{
			((Control)labelTopNStatus).Text = "";
			((Control)panelTopN).Enabled = false;
			((Control)labelTopN).Enabled = false;
		}
		base.LockRefresh = false;
	}

	private void CheckMeterItem(StiMeterItem meterItem)
	{
		if (meterItem.List == listBoxRows)
		{
			meterItem.Meter = StiMeterHelper.Pivot.GetRow(meterItem.Meter);
		}
		else if (meterItem.List == listBoxColumns)
		{
			meterItem.Meter = StiMeterHelper.Pivot.GetColumn(meterItem.Meter);
		}
		else if (meterItem.List == listBoxSummaries)
		{
			meterItem.Meter = StiMeterHelper.Pivot.GetSummary(meterItem.Meter, base.Element.Dashboard);
		}
	}

	private void CheckSelectedColumns()
	{
		StiMeter selectedMeter = StiPivotTableElementSelection.GetSelected(PivotTableElement)?.FirstOrDefault();
		if (selectedMeter is StiPivotColumn)
		{
			listBoxColumns.SelectedMeterItem = listBoxColumns.ToItems().FirstOrDefault((StiMeterItem t) => t.Meter == selectedMeter);
		}
		else if (selectedMeter is StiPivotRow)
		{
			listBoxRows.SelectedMeterItem = listBoxRows.ToItems().FirstOrDefault((StiMeterItem t) => t.Meter == selectedMeter);
		}
	}

	protected override void OnGlobalElementChanged(object sender, EventArgs e)
	{
		if (sender == base.Element)
		{
			IEnumerable<StiPivotColumn> second = from t in listBoxColumns.ToItems()
				select t.Meter as StiPivotColumn into c
				where c != null
				select c;
			IEnumerable<StiPivotRow> second2 = from t in listBoxRows.ToItems()
				select t.Meter as StiPivotRow into c
				where c != null
				select c;
			IEnumerable<StiPivotSummary> second3 = from t in listBoxSummaries.ToItems()
				select t.Meter as StiPivotSummary into c
				where c != null
				select c;
			IEnumerable<StiPivotColumn> difColumns = PivotTableElement.Columns.Except(second);
			IEnumerable<StiPivotRow> difRows = PivotTableElement.Rows.Except(second2);
			IEnumerable<StiPivotSummary> difSummaries = PivotTableElement.Summaries.Except(second3);
			ReadPropertiesFromElement();
			listBoxColumns.SelectedMeterItem = listBoxColumns.ToItems().FirstOrDefault((StiMeterItem t) => t.Meter == difColumns.FirstOrDefault());
			listBoxRows.SelectedMeterItem = listBoxRows.ToItems().FirstOrDefault((StiMeterItem t) => t.Meter == difRows.FirstOrDefault());
			listBoxSummaries.SelectedMeterItem = listBoxSummaries.ToItems().FirstOrDefault((StiMeterItem t) => t.Meter == difSummaries.FirstOrDefault());
		}
	}

	private void ButtonExpressionEdit_Click(object sender, EventArgs e)
	{
		if (StiExpressionEditorHelper.RunExpressionEditor(base.Element, ref textBoxExpression))
		{
			RefreshElementView();
		}
	}

	private void ListBox_ElementsChanged(object sender, StiElementChangedArgs changedArgs)
	{
		ProcessElementChanging(changedArgs);
		WritePropertiesToElement();
		RefreshUiPanel();
	}

	private void ListBox_ElementsAfterSelect(object sender, EventArgs e)
	{
		selectedListBox = sender as StiMeterListBox;
		RefreshExpression();
		RefreshTopN();
		RefreshUiPanel();
	}

	private void ListBox_ElementsConverting(object sender, EventArgs e)
	{
		StiMeterItem meterItem = sender as StiMeterItem;
		CheckMeterItem(meterItem);
	}

	private void ListBox_ElementsDragEnter(object sender, EventArgs e)
	{
		ResetComponentHighlights();
	}

	private void ListBox_Enter(object sender, EventArgs e)
	{
		selectedListBox = sender as StiMeterListBox;
		RefreshExpression();
		RefreshTopN();
	}

	private void ListBox_AfterSelect(object sender, EventArgs e)
	{
		selectedListBox = sender as StiMeterListBox;
		RefreshExpression();
		RefreshTopN();
	}

	private void ListBox_MouseDown(object sender, MouseEventArgs e)
	{
		selectedListBox = sender as StiMeterListBox;
		RefreshExpression();
		RefreshTopN();
	}

	private void ButtonClearTopN_Click(object sender, EventArgs e)
	{
		IStiDataTopN stiDataTopN;
		if (selectedListBox != listBoxColumns)
		{
			IStiDataTopN pivotRow = PivotRow;
			stiDataTopN = pivotRow;
		}
		else
		{
			IStiDataTopN pivotRow = PivotColumn;
			stiDataTopN = pivotRow;
		}
		IStiDataTopN stiDataTopN2 = stiDataTopN;
		if (stiDataTopN2 != null)
		{
			stiDataTopN2.TopN = new StiDataTopN();
		}
		RefreshElementView();
		RefreshTopN();
		RefreshUiPanel();
	}

	private void ButtonEditTopN_Click(object sender, EventArgs e)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		IStiDataTopN topNElement = null;
		if (selectedListBox == listBoxColumns)
		{
			topNElement = PivotColumn;
		}
		else if (selectedListBox == listBoxRows)
		{
			topNElement = PivotRow;
		}
		StiDataTopNEditorForm stiDataTopNEditorForm = new StiDataTopNEditorForm(base.Element, topNElement);
		Designer.AddOwnedForm((Form)(object)stiDataTopNEditorForm);
		((Form)stiDataTopNEditorForm).StartPosition = (FormStartPosition)1;
		((Form)stiDataTopNEditorForm).ShowDialog();
		RefreshElementView();
		RefreshTopN();
		RefreshUiPanel();
	}

	private void StiPivotTableElementClickHelper_TableHeaderClick(object sender, EventArgs e)
	{
		if (((sender is StiPivotTableElement stiPivotTableElement) ? stiPivotTableElement.Key : null) == PivotTableElement.Key)
		{
			CheckSelectedColumns();
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		StiPivotTableElementClickHelper.TableHeaderClick += StiPivotTableElementClickHelper_TableHeaderClick;
	}

	protected override void OnClosed(EventArgs e)
	{
		((Form)this).OnClosed(e);
		StiPivotTableElementClickHelper.TableHeaderClick -= StiPivotTableElementClickHelper_TableHeaderClick;
		StiPivotTableElementSelection.ResetSelection(PivotTableElement);
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

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/dashboards_pivot_table.htm");
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
			StiPivotTableToCrossTabCache.Remove(PivotTableElement);
			if (selectedListBox == listBoxColumns && PivotColumn != null)
			{
				PivotColumn.Expression = ((Control)textBoxExpression).Text;
				listBoxColumns.SetSelectedText(StiLabelHelper.GetLabel(PivotColumn));
			}
			if (selectedListBox == listBoxRows && PivotRow != null)
			{
				PivotRow.Expression = ((Control)textBoxExpression).Text;
				listBoxRows.SetSelectedText(StiLabelHelper.GetLabel(PivotRow));
			}
			if (selectedListBox == listBoxSummaries && PivotSummary != null)
			{
				PivotSummary.Expression = ((Control)textBoxExpression).Text;
				listBoxSummaries.SetSelectedText(StiLabelHelper.GetLabel(PivotSummary));
			}
			RefreshExpressionHint();
		}
	}

	private void ButtonExpressionMenu_Click(object sender, EventArgs e)
	{
		new StiPivotTableExpressionMenu(menuExpression, (TextBox)(object)textBoxExpression, this, selectedListBox).CreateItems();
		Point point = new Point(((Control)panelBoxExpression).Width, ((Control)panelBoxExpression).Height + StiScale.I1);
		if (StiLeftRightHandedHelper.IsLeftHanded)
		{
			point.X -= menuExpression.PopupSize.Width;
		}
		point = ((Control)panelBoxExpression).PointToScreen(point);
		menuExpression.Popup(point);
	}

	private void ButtonSwap_Click(object sender, EventArgs e)
	{
		StiMeterItem[] array = ((IEnumerable)((ListBox)listBoxRows).Items).Cast<StiMeterItem>().ToArray();
		StiMeterItem[] array2 = ((IEnumerable)((ListBox)listBoxColumns).Items).Cast<StiMeterItem>().ToArray();
		((ListBox)listBoxRows).Items.Clear();
		StiMeterItem[] array3 = array2;
		foreach (StiMeterItem stiMeterItem in array3)
		{
			listBoxRows.AddItem(stiMeterItem);
			CheckMeterItem(stiMeterItem);
		}
		if (((ListBox)listBoxRows).Items.Count > 0)
		{
			listBoxRows.SelectedMeterItem = ((ListBox)listBoxRows).Items[0] as StiMeterItem;
		}
		((ListBox)listBoxColumns).Items.Clear();
		StiMeterItem[] array4 = array;
		foreach (StiMeterItem stiMeterItem2 in array4)
		{
			listBoxColumns.AddItem(stiMeterItem2);
			CheckMeterItem(stiMeterItem2);
		}
		if (((ListBox)listBoxColumns).Items.Count > 0)
		{
			listBoxColumns.SelectedMeterItem = ((ListBox)listBoxColumns).Items[0] as StiMeterItem;
		}
		WritePropertiesToElement();
		RefreshUiPanel();
	}

	private void ButtonSummaryDirection_Click(object sender, EventArgs e)
	{
		PivotTableElement.SummaryDirection = ((PivotTableElement.SummaryDirection == StiSummaryDirection.LeftToRight) ? StiSummaryDirection.UpToDown : StiSummaryDirection.LeftToRight);
		WritePropertiesToElement();
		RefreshUiPanel();
		DoScaling();
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
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Expected O, but got Unknown
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Expected O, but got Unknown
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0480: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_054c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0556: Expected O, but got Unknown
		//IL_0573: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f8: Expected O, but got Unknown
		//IL_0615: Unknown result type (might be due to invalid IL or missing references)
		//IL_069f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0773: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d8: Expected O, but got Unknown
		//IL_07f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0918: Unknown result type (might be due to invalid IL or missing references)
		//IL_0922: Expected O, but got Unknown
		//IL_094c: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b04: Expected O, but got Unknown
		//IL_0b2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b94: Expected O, but got Unknown
		//IL_0c25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c2f: Expected O, but got Unknown
		//IL_0c6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d56: Expected O, but got Unknown
		//IL_0e78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f69: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f73: Expected O, but got Unknown
		//IL_10ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d2: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiPivotTableElementDesignerForm));
		listBoxRows = new StiMeterListBox();
		listBoxColumns = new StiMeterListBox();
		labelColumns = new StiChartGroupLabel();
		panelOptions = new StiPanelUX();
		panelTopN = new StiPanelBox();
		labelTopNStatus = new StiLabelUX();
		buttonClearTopN = new StiOperationButton();
		buttonEditTopN = new StiOperationButton();
		labelTopN = new StiLabelUX();
		panelBoxExpression = new StiPanelBox();
		buttonExpressionEdit = new StiOperationButton();
		textBoxExpression = new HintTextBox();
		buttonExpressionMenu = new StiOperationButton();
		labelExpression = new StiLabelUX();
		listBoxSummaries = new StiMeterListBox();
		labelSummary = new StiChartGroupLabel();
		ilDirection = new ImageList(components);
		toolTip1 = new ToolTip(components);
		contextMenuBar = new ContextMenuBar();
		menuExpression = new ButtonItem();
		buttonSwap = new StiOperationButton();
		panel1 = new StiPanelUX();
		labelRows = new StiChartGroupLabel();
		panel2 = new StiPanelUX();
		buttonSummaryDirection = new StiOperationButton();
		((Control)panelOptions).SuspendLayout();
		((Control)panelTopN).SuspendLayout();
		((Control)panelBoxExpression).SuspendLayout();
		((ISupportInitialize)contextMenuBar).BeginInit();
		((Control)panel1).SuspendLayout();
		((Control)panel2).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)listBoxRows).AllowDrop = true;
		((Control)listBoxRows).Dock = (DockStyle)1;
		((Control)listBoxRows).Location = new Point(12, 109);
		((Control)listBoxRows).Margin = new Padding(4);
		((Control)listBoxRows).Name = "listBoxRows";
		((Control)listBoxRows).Size = new Size(276, 33);
		((Control)listBoxRows).TabIndex = 2;
		((ListBox)listBoxRows).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxRows).Enter += ListBox_Enter;
		((Control)listBoxRows).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)listBoxColumns).AllowDrop = true;
		((Control)listBoxColumns).Dock = (DockStyle)1;
		((Control)listBoxColumns).Location = new Point(12, 44);
		((Control)listBoxColumns).Margin = new Padding(4);
		((Control)listBoxColumns).Name = "listBoxColumns";
		((Control)listBoxColumns).Size = new Size(276, 33);
		((Control)listBoxColumns).TabIndex = 1;
		((ListBox)listBoxColumns).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxColumns).Enter += ListBox_Enter;
		((Control)listBoxColumns).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)labelColumns).Dock = (DockStyle)1;
		((Control)labelColumns).Location = new Point(12, 12);
		((Control)labelColumns).Margin = new Padding(2, 0, 2, 0);
		((Control)labelColumns).Name = "labelColumns";
		((Control)labelColumns).Size = new Size(276, 32);
		((Control)labelColumns).TabIndex = 0;
		((Control)labelColumns).Text = "Columns";
		((Control)panelOptions).BackColor = Color.White;
		((Control)panelOptions).Controls.Add((Control)(object)panelTopN);
		((Control)panelOptions).Controls.Add((Control)(object)labelTopN);
		((Control)panelOptions).Dock = (DockStyle)2;
		((Control)panelOptions).Location = new Point(12, 299);
		((Control)panelOptions).Margin = new Padding(2);
		((Control)panelOptions).Name = "panelOptions";
		((Control)panelOptions).Size = new Size(276, 43);
		((Control)panelOptions).TabIndex = 6;
		((Control)panelTopN).Anchor = (AnchorStyles)13;
		((Control)panelTopN).BackColor = Color.White;
		((Control)panelTopN).Controls.Add((Control)(object)labelTopNStatus);
		((Control)panelTopN).Controls.Add((Control)(object)buttonClearTopN);
		((Control)panelTopN).Controls.Add((Control)(object)buttonEditTopN);
		((Control)panelTopN).Location = new Point(101, 12);
		((Control)panelTopN).Margin = new Padding(2);
		((Control)panelTopN).Name = "panelTopN";
		((Control)panelTopN).Size = new Size(175, 25);
		((Control)panelTopN).TabIndex = 13;
		((Control)labelTopNStatus).Anchor = (AnchorStyles)15;
		((Control)labelTopNStatus).Location = new Point(3, 6);
		((Control)labelTopNStatus).Margin = new Padding(2, 0, 2, 0);
		((Control)labelTopNStatus).Name = "labelTopNStatus";
		((Control)labelTopNStatus).Size = new Size(120, 13);
		((Control)labelTopNStatus).TabIndex = 0;
		((Label)labelTopNStatus).TextAlign = (ContentAlignment)16;
		((Control)buttonClearTopN).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonClearTopN).Image = (Image)componentResourceManager.GetObject("buttonClearTopN.Image");
		((Control)buttonClearTopN).Location = new Point(151, 1);
		((Control)buttonClearTopN).Margin = new Padding(2);
		((Control)buttonClearTopN).Name = "buttonClearTopN";
		((Control)buttonClearTopN).Size = new Size(23, 23);
		((Control)buttonClearTopN).TabIndex = 2;
		((ButtonBase)buttonClearTopN).UseVisualStyleBackColor = true;
		((Control)buttonClearTopN).Click += ButtonClearTopN_Click;
		((Control)buttonEditTopN).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonEditTopN).Image = (Image)componentResourceManager.GetObject("buttonEditTopN.Image");
		((Control)buttonEditTopN).Location = new Point(129, 1);
		((Control)buttonEditTopN).Margin = new Padding(2);
		((Control)buttonEditTopN).Name = "buttonEditTopN";
		((Control)buttonEditTopN).Size = new Size(23, 23);
		((Control)buttonEditTopN).TabIndex = 1;
		((ButtonBase)buttonEditTopN).UseVisualStyleBackColor = true;
		((Control)buttonEditTopN).Click += ButtonEditTopN_Click;
		((Control)labelTopN).ForeColor = Color.DimGray;
		((Control)labelTopN).Location = new Point(0, 12);
		((Control)labelTopN).Margin = new Padding(2, 0, 2, 0);
		((Control)labelTopN).Name = "labelTopN";
		((Control)labelTopN).Size = new Size(99, 25);
		((Control)labelTopN).TabIndex = 12;
		((Control)labelTopN).Text = "Top N";
		((Label)labelTopN).TextAlign = (ContentAlignment)16;
		((Control)panelBoxExpression).BackColor = Color.White;
		((Control)panelBoxExpression).Controls.Add((Control)(object)buttonExpressionEdit);
		((Control)panelBoxExpression).Controls.Add((Control)(object)textBoxExpression);
		((Control)panelBoxExpression).Controls.Add((Control)(object)buttonExpressionMenu);
		((Control)panelBoxExpression).Dock = (DockStyle)1;
		((Control)panelBoxExpression).Location = new Point(12, 239);
		((Control)panelBoxExpression).Margin = new Padding(2);
		((Control)panelBoxExpression).Name = "panelBoxExpression";
		((Control)panelBoxExpression).Size = new Size(276, 25);
		((Control)panelBoxExpression).TabIndex = 5;
		((Control)buttonExpressionEdit).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonExpressionEdit).Image = (Image)componentResourceManager.GetObject("buttonExpressionEdit.Image");
		((Control)buttonExpressionEdit).Location = new Point(230, 1);
		((Control)buttonExpressionEdit).Margin = new Padding(2);
		((Control)buttonExpressionEdit).Name = "buttonExpressionEdit";
		((Control)buttonExpressionEdit).Size = new Size(23, 23);
		((Control)buttonExpressionEdit).TabIndex = 1;
		((Control)buttonExpressionEdit).TabStop = false;
		((ButtonBase)buttonExpressionEdit).UseVisualStyleBackColor = true;
		((Control)buttonExpressionEdit).Click += ButtonExpressionEdit_Click;
		((Control)textBoxExpression).Anchor = (AnchorStyles)13;
		((Control)textBoxExpression).BackColor = SystemColors.Window;
		((TextBoxBase)textBoxExpression).BorderStyle = (BorderStyle)0;
		((Control)textBoxExpression).Location = new Point(4, 6);
		((Control)textBoxExpression).Margin = new Padding(2);
		((Control)textBoxExpression).Name = "textBoxExpression";
		((Control)textBoxExpression).Size = new Size(222, 13);
		((Control)textBoxExpression).TabIndex = 3;
		((Control)textBoxExpression).TextChanged += TextBoxExpression_TextChanged;
		((Control)textBoxExpression).Enter += TextBoxExpression_Leave;
		((Control)textBoxExpression).KeyDown += new KeyEventHandler(TextBoxExpression_KeyDown);
		((Control)buttonExpressionMenu).Anchor = (AnchorStyles)9;
		((Control)buttonExpressionMenu).Location = new Point(252, 1);
		((Control)buttonExpressionMenu).Margin = new Padding(2);
		((Control)buttonExpressionMenu).Name = "buttonExpressionMenu";
		((Control)buttonExpressionMenu).Size = new Size(23, 23);
		((Control)buttonExpressionMenu).TabIndex = 2;
		((Control)buttonExpressionMenu).TabStop = false;
		((ButtonBase)buttonExpressionMenu).UseVisualStyleBackColor = true;
		((Control)buttonExpressionMenu).Click += ButtonExpressionMenu_Click;
		((Control)labelExpression).Dock = (DockStyle)1;
		((Control)labelExpression).ForeColor = Color.DimGray;
		((Control)labelExpression).Location = new Point(12, 207);
		((Control)labelExpression).Margin = new Padding(2, 0, 2, 0);
		((Control)labelExpression).Name = "labelExpression";
		((Control)labelExpression).Size = new Size(276, 32);
		((Control)labelExpression).TabIndex = 4;
		((Control)labelExpression).Text = "Expression";
		((Label)labelExpression).TextAlign = (ContentAlignment)16;
		((Control)listBoxSummaries).AllowDrop = true;
		((Control)listBoxSummaries).Dock = (DockStyle)1;
		((Control)listBoxSummaries).Location = new Point(12, 174);
		((Control)listBoxSummaries).Margin = new Padding(4);
		((Control)listBoxSummaries).Name = "listBoxSummaries";
		((Control)listBoxSummaries).Size = new Size(276, 33);
		((Control)listBoxSummaries).TabIndex = 3;
		((ListBox)listBoxSummaries).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxSummaries).Enter += ListBox_Enter;
		((Control)listBoxSummaries).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)labelSummary).Dock = (DockStyle)1;
		((Control)labelSummary).Location = new Point(0, 0);
		((Control)labelSummary).Margin = new Padding(2, 0, 2, 0);
		((Control)labelSummary).Name = "labelSummary";
		((Control)labelSummary).Size = new Size(276, 32);
		((Control)labelSummary).TabIndex = 0;
		((Control)labelSummary).Text = "Summary";
		ilDirection.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("ilDirection.ImageStream");
		ilDirection.TransparentColor = Color.Magenta;
		ilDirection.Images.SetKeyName(0, "");
		ilDirection.Images.SetKeyName(1, "");
		ilDirection.Images.SetKeyName(2, "HorizontalDirection.png");
		ilDirection.Images.SetKeyName(3, "VerticalDirection.png");
		contextMenuBar.AntiAlias = true;
		contextMenuBar.DockTabStripHeight = 32;
		((Control)contextMenuBar).Font = new Font("Segoe UI", 9f);
		contextMenuBar.Items.AddRange(new BaseItem[1] { menuExpression });
		contextMenuBar.Location = new Point(208, 267);
		((Control)contextMenuBar).Margin = new Padding(2);
		contextMenuBar.Name = "contextMenuBar";
		contextMenuBar.Size = new Size(80, 27);
		contextMenuBar.Stretch = true;
		contextMenuBar.Style = eDotNetBarStyle.StyleManagerControlled;
		contextMenuBar.TabIndex = 12;
		contextMenuBar.TabStop = false;
		((Control)contextMenuBar).Text = "contextMenuBar";
		contextMenuBar.WrapItemsDock = true;
		menuExpression.AutoExpandOnClick = true;
		menuExpression.ImagePaddingHorizontal = 10;
		menuExpression.ImagePaddingVertical = 8;
		menuExpression.LockClosePopup = false;
		menuExpression.Name = "menuExpression";
		menuExpression.Text = "menuExpression";
		((ButtonBase)buttonSwap).Image = (Image)componentResourceManager.GetObject("buttonSwap.Image");
		((Control)buttonSwap).Location = new Point(249, 4);
		((Control)buttonSwap).Name = "buttonSwap";
		((Control)buttonSwap).Size = new Size(24, 24);
		((Control)buttonSwap).TabIndex = 46;
		((ButtonBase)buttonSwap).UseVisualStyleBackColor = true;
		((Control)buttonSwap).Click += ButtonSwap_Click;
		((Control)panel1).BackColor = Color.White;
		((Control)panel1).Controls.Add((Control)(object)buttonSwap);
		((Control)panel1).Controls.Add((Control)(object)labelRows);
		((Control)panel1).Dock = (DockStyle)1;
		((Control)panel1).Location = new Point(12, 77);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(276, 32);
		((Control)panel1).TabIndex = 47;
		((Control)labelRows).Dock = (DockStyle)5;
		((Control)labelRows).Location = new Point(0, 0);
		((Control)labelRows).Margin = new Padding(2, 0, 2, 0);
		((Control)labelRows).Name = "labelRows";
		((Control)labelRows).Size = new Size(276, 32);
		((Control)labelRows).TabIndex = 1;
		((Control)labelRows).Text = "Rows";
		((Control)panel2).BackColor = Color.White;
		((Control)panel2).Controls.Add((Control)(object)buttonSummaryDirection);
		((Control)panel2).Controls.Add((Control)(object)labelSummary);
		((Control)panel2).Dock = (DockStyle)1;
		((Control)panel2).Location = new Point(12, 142);
		((Control)panel2).Name = "panel2";
		((Control)panel2).Size = new Size(276, 32);
		((Control)panel2).TabIndex = 48;
		((ButtonBase)buttonSummaryDirection).Image = (Image)componentResourceManager.GetObject("buttonSummaryDirection.Image");
		((Control)buttonSummaryDirection).Location = new Point(249, 5);
		((Control)buttonSummaryDirection).Name = "buttonSummaryDirection";
		((Control)buttonSummaryDirection).Size = new Size(24, 24);
		((Control)buttonSummaryDirection).TabIndex = 48;
		((ButtonBase)buttonSummaryDirection).UseVisualStyleBackColor = true;
		((Control)buttonSummaryDirection).Click += ButtonSummaryDirection_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(300, 354);
		((Control)this).Controls.Add((Control)(object)panelBoxExpression);
		((Control)this).Controls.Add((Control)(object)labelExpression);
		((Control)this).Controls.Add((Control)(object)listBoxSummaries);
		((Control)this).Controls.Add((Control)(object)panel2);
		((Control)this).Controls.Add((Control)(object)listBoxRows);
		((Control)this).Controls.Add((Control)(object)panel1);
		((Control)this).Controls.Add((Control)(object)listBoxColumns);
		((Control)this).Controls.Add((Control)(object)labelColumns);
		((Control)this).Controls.Add((Control)(object)panelOptions);
		((Control)this).Controls.Add((Control)(object)contextMenuBar);
		((Form)this).Margin = new Padding(0);
		((Control)this).Name = "StiPivotTableElementDesignerForm";
		((Control)this).Padding = new Padding(12);
		((Control)this).Text = "Pivot";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Control)panelOptions).ResumeLayout(false);
		((Control)panelTopN).ResumeLayout(false);
		((Control)panelBoxExpression).ResumeLayout(false);
		((Control)panelBoxExpression).PerformLayout();
		((ISupportInitialize)contextMenuBar).EndInit();
		((Control)panel1).ResumeLayout(false);
		((Control)panel2).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
