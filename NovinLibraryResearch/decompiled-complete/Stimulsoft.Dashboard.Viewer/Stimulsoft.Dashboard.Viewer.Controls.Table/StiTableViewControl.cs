using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Menu;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Dashboard.Drawing.Helpers;
using Stimulsoft.Dashboard.Interactions;
using Stimulsoft.Dashboard.Viewer.Controls.Helpers;
using Stimulsoft.Dashboard.Viewer.Controls.Table.Menu;
using Stimulsoft.Dashboard.Viewer.Elements;
using Stimulsoft.Dashboard.Viewer.Elements.Table;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Dashboard.Viewer.Controls.Table;

[ToolboxItem(false)]
public class StiTableViewControl : StiDataGridView
{
	private StiTableElementStyle style;

	private StiContextMenu menuView;

	private StiTableElementSortFilterMenu sortFilterMenu;

	private int cellColumnIndex = -1;

	private int cellRowIndex = -1;

	private StiHyperlinkToolTip toolTipHyperlink;

	private StiHtmlToolTip toolTipHtml;

	private bool isColumnDragging;

	private int dropOverColumnIndex = -1;

	private int dragColumnIndex = -1;

	private Rectangle dragBoxFromMouseDown;

	private ToolTip toolTipDragColumn;

	private DataTable storedTable;

	internal bool WordWrapHeader { get; set; } = true;


	internal Hashtable GridColumnToTable { get; set; } = new Hashtable();


	internal Hashtable GridColumnToTableColumn { get; set; } = new Hashtable();


	internal Hashtable GridColumnToMin { get; set; } = new Hashtable();


	internal Hashtable GridColumnToMax { get; set; } = new Hashtable();


	internal StiTableElement Element { get; }

	internal StiDataTable DataTable { get; set; }

	internal DataTable NetTable => ((DataGridView)this).DataSource as DataTable;

	internal List<string> ColumnKeys => (from c in Element.Columns
		select c?.Key into c
		where c != null
		select c).ToList();

	internal StiDataGridViewPosition PreFilterPosition { get; set; }

	internal bool AllowBaseDrawing { get; set; } = true;


	private bool IsOverClickableCell(int columnIndex, int rowIndex)
	{
		if (DataTable == null || (rowIndex == -1 && columnIndex > -1))
		{
			return false;
		}
		StiTableColumn column = GridColumnToTableColumn[((DataGridView)this).Columns[columnIndex]] as StiTableColumn;
		StiTableColumnDashboardInteraction interaction = GetInteraction(column);
		if (interaction == null)
		{
			return false;
		}
		if (interaction.OnClick == StiInteractionOnClick.ShowDashboard || interaction.OnClick == StiInteractionOnClick.DrillDown || interaction.OnClick == StiInteractionOnClick.OpenHyperlink)
		{
			return true;
		}
		return false;
	}

	public void NewTab(StiPage page, string value, List<DictionaryEntry> parameters)
	{
		StiDashboardViewerHelper.FindViewer((Control)(object)this)?.NewTab(page, value, parameters);
	}

	private StiHolderElementControl GetHolderControl()
	{
		Control parent = ((Control)this).Parent;
		while (parent != null && !(parent is StiHolderElementControl))
		{
			parent = parent.Parent;
		}
		return parent as StiHolderElementControl;
	}

	protected override DataGridViewCellStyle CreateAlternatingCellStyle()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		return new DataGridViewCellStyle
		{
			ForeColor = style.AlternatingCellForeColor,
			BackColor = style.AlternatingCellBackColor
		};
	}

	internal void ApplySort()
	{
		List<string> columnKeys = DataTable.Meters.Select((IStiMeter m) => m.Key).ToList();
		List<string> columnNames = (from DataColumn c in NetTable.Columns
			select c.ColumnName).ToList();
		if (StiDataSortRuleHelper.IsCommaInColumnNames(NetTable) && Element.UserSorts != null && Element.UserSorts.Count > 0)
		{
			if (storedTable == null)
			{
				storedTable = ((DataGridView)this).DataSource as DataTable;
			}
			else
			{
				((DataGridView)this).DataSource = storedTable;
			}
			((DataGridView)this).DataSource = StiDataSortRuleHelper.OrderDataTableWithCommainColumnNames(NetTable.Copy(), Element.UserSorts, columnKeys, columnNames);
			return;
		}
		if (storedTable != null)
		{
			((DataGridView)this).DataSource = storedTable;
			storedTable = null;
		}
		NetTable.DefaultView.Sort = StiDataSortRuleHelper.GetDataTableSortQuery(Element.UserSorts, columnKeys, columnNames);
	}

	internal void RefreshStatus(bool checkProgressStatus)
	{
		StiHolderElementControl holderControl = GetHolderControl();
		if (holderControl != null)
		{
			holderControl.RefreshStatus(checkProgressStatus);
			StiActionButtonsHelper.RefreshButtonsVisibility(holderControl, holderControl.Element);
		}
	}

	internal void RefreshViewerData()
	{
		(((Control)this).Parent as StiTableElementControl)?.RefreshViewerData();
	}

	internal void RefreshActionButtons()
	{
		(((Control)this).Parent as StiTableElementControl)?.RefreshActionButtons();
	}

	private void CreateSortMenu(StiTableElement element)
	{
		sortFilterMenu = new StiTableElementSortFilterMenu((ContextMenuStrip)(object)menuView, element);
		sortFilterMenu.SortChanged += SortFilterMenu_SortChanged;
		sortFilterMenu.FiltersChanged += SortFilterMenu_FiltersChanged;
	}

	private void CreateViewMenu()
	{
		StiContextMenu stiContextMenu = new StiContextMenu();
		((Control)stiContextMenu).Name = "menuView";
		((Control)stiContextMenu).Text = "menuView";
		menuView = stiContextMenu;
		((ToolStripDropDown)menuView).Opening += delegate
		{
			menuView.IsDarkMode = StiDashboardStyleHelper.IsDarkStyle(Element);
		};
	}

	private void CreateContextMenu()
	{
		StiContextMenu stiContextMenu = new StiContextMenu();
		((Control)stiContextMenu).Name = "menuCopy";
		((Control)stiContextMenu).Text = "menuCopy";
		stiContextMenu.CloseOnClick = true;
		StiContextMenu menuCopy = stiContextMenu;
		StiContextMenuItem stiContextMenuItem = new StiContextMenuItem(Loc.GetEnum("PictureBoxSizeModeAutoSize"));
		((ToolStripItem)stiContextMenuItem).Click += delegate
		{
			try
			{
				StiTableSizer.MakeAutoSize((DataGridView)(object)this, skipLimit: true);
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		};
		((ToolStrip)menuCopy).Items.Add((ToolStripItem)(object)stiContextMenuItem);
		StiContextMenuItem stiContextMenuItem2 = new StiContextMenuItem(Loc.Get("MainMenu", "menuEditCopy").Replace("&", ""));
		((ToolStripItem)stiContextMenuItem2).Click += delegate
		{
			try
			{
				string text = ((DataGridView)this).CurrentCell.FormattedValue?.ToString();
				if (string.IsNullOrEmpty(text))
				{
					Clipboard.SetText(text);
				}
			}
			catch
			{
			}
		};
		((ToolStrip)menuCopy).Items.Add((ToolStripItem)(object)stiContextMenuItem2);
		((ToolStripDropDown)menuCopy).Opening += delegate
		{
			menuCopy.IsDarkMode = StiDashboardStyleHelper.IsDarkStyle(Element);
		};
		((Control)this).ContextMenuStrip = (ContextMenuStrip)(object)menuCopy;
	}

	private void CreateHyperlinkToolTip()
	{
		if (toolTipHyperlink == null)
		{
			StiHyperlinkToolTip stiHyperlinkToolTip = new StiHyperlinkToolTip();
			((ToolTip)stiHyperlinkToolTip).AutomaticDelay = 0;
			((ToolTip)stiHyperlinkToolTip).OwnerDraw = true;
			((ToolTip)stiHyperlinkToolTip).ShowAlways = true;
			((ToolTip)stiHyperlinkToolTip).UseAnimation = false;
			((ToolTip)stiHyperlinkToolTip).UseFading = false;
			toolTipHyperlink = stiHyperlinkToolTip;
		}
	}

	private void CreateHtmlToolTip()
	{
		if (toolTipHtml == null)
		{
			StiHtmlToolTip stiHtmlToolTip = new StiHtmlToolTip();
			((ToolTip)stiHtmlToolTip).AutomaticDelay = 0;
			((ToolTip)stiHtmlToolTip).OwnerDraw = true;
			((ToolTip)stiHtmlToolTip).ShowAlways = true;
			((ToolTip)stiHtmlToolTip).UseAnimation = false;
			((ToolTip)stiHtmlToolTip).UseFading = false;
			toolTipHtml = stiHtmlToolTip;
		}
	}

	private void HideToolTips()
	{
		HideHyperlinkToolTip();
		HideHtmlToolTip();
		HideDragToolTip();
		cellColumnIndex = -1;
		cellRowIndex = -1;
	}

	private void HideDragToolTip()
	{
		ToolTip obj = toolTipDragColumn;
		if (obj != null)
		{
			obj.Hide((IWin32Window)(object)this);
		}
	}

	private void HideHtmlToolTip()
	{
		StiHtmlToolTip stiHtmlToolTip = toolTipHtml;
		if (stiHtmlToolTip != null)
		{
			((ToolTip)stiHtmlToolTip).Hide((IWin32Window)(object)this);
		}
	}

	private void HideHyperlinkToolTip()
	{
		StiHyperlinkToolTip stiHyperlinkToolTip = toolTipHyperlink;
		if (stiHyperlinkToolTip != null)
		{
			((ToolTip)stiHyperlinkToolTip).Hide((IWin32Window)(object)this);
		}
	}

	private string GetHyperlink(int columnIndex, int rowIndex, StiTableColumn column)
	{
		if (!(column is StiDimensionColumn stiDimensionColumn))
		{
			return null;
		}
		string text = ((DataGridView)this).Rows[rowIndex].Cells[columnIndex].Value?.ToString();
		if (string.IsNullOrWhiteSpace(stiDimensionColumn.HyperlinkPattern))
		{
			return text;
		}
		Dictionary<string, object> constants = GetConstants(text, ((DataGridView)this).Rows[rowIndex].Cells);
		return StiReportParser.Parse(stiDimensionColumn.HyperlinkPattern, Element, allowCache: false, constants);
	}

	private string GetToolTip(int columnIndex, int rowIndex, StiTableColumn column)
	{
		string value = ((DataGridView)this).Rows[rowIndex].Cells[columnIndex].Value?.ToString();
		Dictionary<string, object> constants = GetConstants(value, ((DataGridView)this).Rows[rowIndex].Cells);
		return StiReportParser.Parse(GetInteraction(column).ToolTip, Element, allowCache: false, constants);
	}

	protected StiTableColumnDashboardInteraction GetInteraction(StiTableColumn column)
	{
		return column?.DashboardInteraction as StiTableColumnDashboardInteraction;
	}

	private bool IsToolTipActive(StiTableColumn column)
	{
		if (column == null)
		{
			return false;
		}
		if (GetInteraction(column).OnHover == StiInteractionOnHover.ShowHyperlink)
		{
			if (toolTipHyperlink != null)
			{
				return ((ToolTip)toolTipHyperlink).Active;
			}
			return false;
		}
		if (toolTipHtml != null)
		{
			return ((ToolTip)toolTipHtml).Active;
		}
		return false;
	}

	public string GetColumnPath(StiTableColumn tableColumn)
	{
		string expression = StiExpressionHelper.RemoveFunction(tableColumn.Expression);
		StiDataColumn stiDataColumn = StiDataExpressionHelper.GetDataColumnFromExpression(Element.Page as StiDashboard, expression) as StiDataColumn;
		if (stiDataColumn?.DataSource == null)
		{
			return null;
		}
		return stiDataColumn.DataSource.Name + "." + stiDataColumn.Name;
	}

	private Dictionary<string, object> GetConstants(string value, DataGridViewCellCollection cells)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("Value", value);
		int num = 0;
		foreach (DataGridViewCell item in (BaseCollection)cells)
		{
			DataGridViewCell val = item;
			string key = "Row." + StiCodeDomSerializator.ReplaceSymbols(val.OwningColumn.Name);
			dictionary.Add(key, val.Value?.ToString());
			num++;
		}
		return dictionary;
	}

	protected override void Dispose(bool disposing)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		if (disposing)
		{
			((Component)(object)toolTipHtml)?.Dispose();
			((Component)(object)toolTipHyperlink)?.Dispose();
			((DataGridView)this).CellPainting -= new DataGridViewCellPaintingEventHandler(This_CellPainting);
		}
		((DataGridView)this).Dispose(disposing);
	}

	private void This_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
	{
		if (AllowBaseDrawing && e.RowIndex == -1)
		{
			int num = ((isColumnDragging && dragColumnIndex >= 0) ? dragColumnIndex : (-1));
			new StiTablePartDrawer(e, (DataGridView)(object)this, Element, style, Element.HeaderForeColor, Element.HeaderFont, num, WordWrapHeader).DrawCell();
		}
	}

	protected override void OnPaintBackground(PaintEventArgs e)
	{
		StiElementControlPainter.PaintBackground(rect: new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height), g: e.Graphics, element: Element);
	}

	private int GetDisplayIndex(int index)
	{
		return ((DataGridView)this).Columns[index].DisplayIndex;
	}

	protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
	{
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		((DataGridView)this).OnCellPainting(e);
		if (e.RowIndex != -1 || dropOverColumnIndex != e.ColumnIndex || dragColumnIndex == e.ColumnIndex || dragColumnIndex < 0)
		{
			return;
		}
		Color color = ((style == null) ? StiElementConsts.Table.BorderColor : style.LineColor);
		Pen val = new Pen(color, (float)StiScale.I2);
		try
		{
			int i = StiScale.I2;
			if (GetDisplayIndex(e.ColumnIndex) > GetDisplayIndex(dragColumnIndex))
			{
				StiDrawing.DrawLine(e.Graphics, val, e.CellBounds.Right - i, e.CellBounds.Top, e.CellBounds.Right - i, e.CellBounds.Bottom);
			}
			else
			{
				StiDrawing.DrawLine(e.Graphics, val, e.CellBounds.Left + i, e.CellBounds.Top, e.CellBounds.Left + i, e.CellBounds.Bottom);
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected override void OnCellMouseDown(DataGridViewCellMouseEventArgs e)
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Invalid comparison between Unknown and I4
		((DataGridView)this).OnCellMouseDown(e);
		dragColumnIndex = e.ColumnIndex;
		Point location = new Point(((MouseEventArgs)e).X - SystemInformation.DragSize.Width / 2, ((MouseEventArgs)e).Y - SystemInformation.DragSize.Height / 2);
		dragBoxFromMouseDown = new Rectangle(location, SystemInformation.DragSize);
		try
		{
			if (e.ColumnIndex != -1 && e.RowIndex != -1 && (int)((MouseEventArgs)e).Button == 2097152)
			{
				DataGridViewCell val = ((DataGridView)this)[e.ColumnIndex, e.RowIndex];
				if (!val.Selected)
				{
					((DataGridViewElement)val).DataGridView.ClearSelection();
					((DataGridViewElement)val).DataGridView.CurrentCell = val;
					val.Selected = true;
				}
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	protected override void OnCellMouseUp(DataGridViewCellMouseEventArgs e)
	{
		((DataGridView)this).OnCellMouseUp(e);
		DataGridViewColumn val = ((IEnumerable)((DataGridView)this).Columns).Cast<DataGridViewColumn>().LastOrDefault((Func<DataGridViewColumn, bool>)((DataGridViewColumn c) => ((DataGridViewBand)c).Frozen));
		int num = ((val != null) ? val.DisplayIndex : (-1));
		if (isColumnDragging && dropOverColumnIndex >= 0 && num < dropOverColumnIndex)
		{
			((DataGridView)this).Columns[dragColumnIndex].DisplayIndex = GetDisplayIndex(dropOverColumnIndex);
		}
	}

	protected override void OnCellMouseEnter(DataGridViewCellEventArgs e)
	{
		((DataGridView)this).OnCellMouseEnter(e);
		if (isColumnDragging)
		{
			dropOverColumnIndex = e.ColumnIndex;
			((DataGridView)this).InvalidateCell(e.ColumnIndex, e.RowIndex);
		}
		((Control)this).Cursor = (IsOverClickableCell(e.ColumnIndex, e.RowIndex) ? Cursors.Hand : Cursors.Default);
	}

	protected override void OnCellMouseLeave(DataGridViewCellEventArgs e)
	{
		((DataGridView)this).OnCellMouseLeave(e);
		if (isColumnDragging)
		{
			dropOverColumnIndex = -1;
			((DataGridView)this).InvalidateCell(e.ColumnIndex, e.RowIndex);
		}
		((Control)this).Cursor = Cursors.Default;
	}

	protected override void OnCellMouseMove(DataGridViewCellMouseEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Invalid comparison between Unknown and I4
		((DataGridView)this).OnCellMouseMove(e);
		try
		{
			if ((((MouseEventArgs)e).Button & 0x100000) == 1048576)
			{
				if (dragColumnIndex != -1 && !((DataGridViewBand)((DataGridView)this).Columns[dragColumnIndex]).Frozen && ((e.ColumnIndex == dragColumnIndex && !dragBoxFromMouseDown.Contains(((MouseEventArgs)e).X, ((MouseEventArgs)e).Y)) || e.ColumnIndex != dragColumnIndex))
				{
					isColumnDragging = true;
				}
				else
				{
					isColumnDragging = false;
				}
			}
			else
			{
				dragColumnIndex = -1;
				dropOverColumnIndex = -1;
				isColumnDragging = false;
				dragBoxFromMouseDown = Rectangle.Empty;
			}
			StiTableColumn column = ((e.ColumnIndex != -1) ? (GridColumnToTableColumn[((DataGridView)this).Columns[e.ColumnIndex]] as StiTableColumn) : null);
			if (e.ColumnIndex != cellColumnIndex || e.RowIndex != cellRowIndex || !IsToolTipActive(column))
			{
				cellColumnIndex = e.ColumnIndex;
				cellRowIndex = e.RowIndex;
				if (cellColumnIndex >= 0 && cellRowIndex >= 0)
				{
					Point mousePos = ((Control)this).PointToClient(Control.MousePosition);
					StiTableColumnDashboardInteraction interaction = GetInteraction(column);
					if (interaction.OnHover == StiInteractionOnHover.ShowHyperlink)
					{
						OnCellMouseMove_ShowHyperlink(e, column, mousePos);
					}
					else if (interaction.OnHover == StiInteractionOnHover.ShowToolTip)
					{
						OnCellMouseMove_ShowToolTip(e, column, mousePos);
					}
					else
					{
						HideToolTips();
					}
					return;
				}
			}
			if (e.ColumnIndex == -1 || e.RowIndex == -1)
			{
				HideToolTips();
			}
		}
		catch
		{
		}
	}

	private void OnCellMouseMove_ShowHyperlink(DataGridViewCellMouseEventArgs e, StiTableColumn column, Point mousePos)
	{
		try
		{
			string hyperlink = GetHyperlink(e.ColumnIndex, e.RowIndex, column);
			if (hyperlink != null)
			{
				CreateHyperlinkToolTip();
				((ToolTip)toolTipHyperlink).Show(hyperlink, (IWin32Window)(object)this, mousePos);
				HideHtmlToolTip();
			}
			else
			{
				HideToolTips();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void OnCellMouseMove_ShowToolTip(DataGridViewCellMouseEventArgs e, StiTableColumn column, Point mousePos)
	{
		try
		{
			CreateHtmlToolTip();
			string toolTip = GetToolTip(e.ColumnIndex, e.RowIndex, column);
			toolTipHtml.ShowHtml(toolTip, (IWin32Window)(object)this, mousePos);
			HideHyperlinkToolTip();
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((DataGridView)this).OnMouseLeave(e);
		HideToolTips();
		ToolTip obj = toolTipDragColumn;
		if (obj != null)
		{
			obj.Hide((IWin32Window)(object)this);
		}
		((Control)this).Cursor = Cursors.Default;
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		((DataGridView)this).OnMouseMove(e);
		if (isColumnDragging)
		{
			if (toolTipDragColumn == null)
			{
				toolTipDragColumn = new ToolTip();
			}
			toolTipDragColumn.Show(((DataGridView)this).Columns[dragColumnIndex].HeaderText, (IWin32Window)(object)this, new Point(e.X + StiScale.I10, e.Y + StiScale.I10));
		}
		else
		{
			HideDragToolTip();
		}
	}

	protected override void OnCellMouseClick(DataGridViewCellMouseEventArgs e)
	{
		((DataGridView)this).OnCellMouseClick(e);
		try
		{
			if (DataTable != null)
			{
				if (e.RowIndex != -1 || e.ColumnIndex <= -1)
				{
					OnDataCellMouseClick(e);
				}
				else
				{
					OnHeaderCellMouseClick(e);
				}
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void OnHeaderCellMouseClick(DataGridViewCellMouseEventArgs e)
	{
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Invalid comparison between Unknown and I4
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Invalid comparison between Unknown and I4
		try
		{
			StiTableColumn item = GridColumnToTableColumn[((DataGridView)this).Columns[e.ColumnIndex]] as StiTableColumn;
			int num = Element.Columns.IndexOf(item);
			string key = DataTable.Meters[num].Key;
			StiTableDashboardInteraction stiTableDashboardInteraction = Element.DashboardInteraction as StiTableDashboardInteraction;
			if (stiTableDashboardInteraction.AllowUserSorting)
			{
				Rectangle contentBounds = ((DataGridViewCell)((DataGridView)this).Columns[e.ColumnIndex].HeaderCell).ContentBounds;
				bool flag = StiTablePartDrawer.GetSortRectangle(contentBounds).Contains(((MouseEventArgs)e).Location);
				bool flag2 = (Control.ModifierKeys & 0x20000) > 0;
				bool flag3 = (Control.ModifierKeys & 0x10000) > 0;
				if (flag2 || flag3 || flag)
				{
					OnHeaderCellClick_ApplySorting(key, flag3);
					return;
				}
			}
			if (((DataGridView)this).CurrentCell != null)
			{
				((DataGridView)this).CurrentCell = ((DataGridView)this).Rows[((DataGridView)this).CurrentCell.RowIndex].Cells[e.ColumnIndex];
			}
			if (stiTableDashboardInteraction.AllowUserFiltering || stiTableDashboardInteraction.AllowUserSorting)
			{
				OnHeaderCellClick_ShowMenu(e, num);
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void OnHeaderCellClick_ApplySorting(string columnKey, bool isShiftKey)
	{
		StiDataSortDirection sortDirection = StiDataSortRuleHelper.GetSortDirection(Element.UserSorts, columnKey);
		sortDirection = ((sortDirection != StiDataSortDirection.Ascending) ? StiDataSortDirection.Ascending : StiDataSortDirection.Descending);
		if (isShiftKey)
		{
			Element.UserSorts = new List<StiDataSortRule>();
		}
		Element.UserSorts = StiDataSortRuleHelper.SetSortDirection(Element.UserSorts, ColumnKeys, columnKey, sortDirection);
		ApplySort();
	}

	private void OnHeaderCellClick_ShowMenu(DataGridViewCellMouseEventArgs e, int columnIndex)
	{
		Rectangle cellDisplayRectangle = ((DataGridView)this).GetCellDisplayRectangle(e.ColumnIndex, -1, false);
		sortFilterMenu.CreateItems(DataTable.Meters, columnIndex, Element);
		Point point = ((Control)this).PointToScreen(new Point(cellDisplayRectangle.Left, cellDisplayRectangle.Bottom));
		((ToolStripDropDown)menuView).Show(point);
	}

	private void OnDataCellMouseClick(DataGridViewCellMouseEventArgs e)
	{
		StiTableColumn stiTableColumn = GridColumnToTableColumn[((DataGridView)this).Columns[e.ColumnIndex]] as StiTableColumn;
		object value = ((DataGridView)this).Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
		StiTableColumnDashboardInteraction interaction = GetInteraction(stiTableColumn);
		DataColumn column = NetTable.Columns[e.ColumnIndex];
		if (interaction.OnClick == StiInteractionOnClick.ApplyFilter)
		{
			OnDataCellMouseClick_ApplyFilter(column, stiTableColumn, value);
		}
		else if (interaction.OnClick == StiInteractionOnClick.ShowDashboard)
		{
			OnDataCellMouseClick_ShowDashboard(stiTableColumn, value?.ToString(), ((DataGridView)this).Rows[e.RowIndex].Cells);
		}
		if (interaction.OnClick == StiInteractionOnClick.DrillDown)
		{
			OnDataCellMouseClick_DrillDown(stiTableColumn, value?.ToString());
		}
	}

	private void OnDataCellMouseClick_ApplyFilter(DataColumn column, StiTableColumn tableColumn, object value)
	{
		PreFilterPosition = SavePosition();
		string value2 = (column.DataType.IsEnum ? Enum.ToObject(column.DataType, StiValueHelper.TryToInt(value)).ToString() : value?.ToString());
		if (StiActionButtonsHelper.GetFilterMultiOption(Element))
		{
			if (Element.UserFilters == null)
			{
				Element.UserFilters = new List<StiDataFilterRule>();
			}
			Element.UserFilters.Add(new StiDataFilterRule(GetColumnPath(tableColumn), StiDataFilterCondition.EqualTo, value2));
		}
		else
		{
			Element.UserFilters = new StiDataFilterRule(GetColumnPath(tableColumn), StiDataFilterCondition.EqualTo, value2).ToList();
		}
		RefreshViewerData();
		RefreshActionButtons();
	}

	private void OnDataCellMouseClick_ShowDashboard(StiTableColumn tableColumn, string value, DataGridViewCellCollection cells)
	{
		StiTableColumnDashboardInteraction tableColumnInteraction = tableColumn.DashboardInteraction as StiTableColumnDashboardInteraction;
		if (string.IsNullOrWhiteSpace(tableColumnInteraction.DrillDownPageKey))
		{
			return;
		}
		StiPage stiPage = Element.Report.Pages.ToList().FirstOrDefault((StiPage p) => p.Guid == tableColumnInteraction.DrillDownPageKey);
		if (stiPage != null)
		{
			Dictionary<string, object> constants = GetConstants(value, cells);
			List<DictionaryEntry> parameters = tableColumnInteraction.DrillDownParameters.Select((StiDashboardDrillDownParameter d) => new DictionaryEntry(d.Name, StiReportParser.Parse(d.Expression, Element, allowCache: false, constants))).ToList();
			NewTab(stiPage, value, parameters);
		}
	}

	private void OnDataCellMouseClick_DrillDown(StiTableColumn tableColumn, string value)
	{
	}

	private void SortFilterMenu_SortChanged(object sender, EventArgs e)
	{
		ApplySort();
	}

	private void SortFilterMenu_FiltersChanged(object sender, EventArgs e)
	{
		RefreshViewerData();
		RefreshActionButtons();
	}

	public StiTableViewControl(StiTableElement element, StiDataTable dataTable)
	{
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		((DataGridView)this).ShowCellToolTips = false;
		style = StiDashboardStyleHelper.GetTableStyle(element);
		InitGridProperties();
		((Control)this).DoubleBuffered = true;
		Element = element;
		DataTable = dataTable;
		((DataGridView)this).AutoGenerateColumns = false;
		((DataGridView)this).ReadOnly = true;
		((DataGridView)this).CellPainting += new DataGridViewCellPaintingEventHandler(This_CellPainting);
		CreateContextMenu();
		CreateViewMenu();
		CreateSortMenu(element);
	}
}
