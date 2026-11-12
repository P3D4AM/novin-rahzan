using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Controls;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Dashboard.Drawing.Painters;
using Stimulsoft.Dashboard.Drawing.Painters.Table;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Dashboard.Interactions;
using Stimulsoft.Dashboard.Viewer.Controls.Table;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Extensions;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Dashboard.Viewer.Elements.Table;

[ToolboxItem(false)]
public class StiTableElementControl : StiElementControl, IStiRefreshControl
{
	private bool lockWidth;

	private bool AllowFooter { get; set; }

	internal StiTableViewControl TableViewControl => base.Child as StiTableViewControl;

	private StiTableFooterControl FooterControl { get; set; }

	internal bool IsAllColumnsInvisible => ((IEnumerable)((DataGridView)TableViewControl).Columns).Cast<DataGridViewColumn>().All((DataGridViewColumn c) => !((DataGridViewBand)c).Visible);

	public virtual void RefreshControl(object sender, StiReportControlUpdateEventArgs e)
	{
		if (sender == base.Element)
		{
			((Control)this).Invalidate();
			StiTableElement stiTableElement = base.Element as StiTableElement;
			StiTableElementStyle tableStyle = StiDashboardStyleHelper.GetTableStyle(stiTableElement);
			((DataGridView)(base.Child as StiTableViewControl)).BackgroundColor = StiDashboardStyleHelper.GetBackColor(base.Element);
			if (AllowFooter)
			{
				FooterControl.Style = tableStyle;
				((Control)FooterControl).BackColor = StiDashboardStyleHelper.GetBackColor(base.Element);
				((Control)FooterControl).ForeColor = stiTableElement.FooterForeColor;
			}
		}
	}

	public override StiDataTable GetOrCreateData()
	{
		StiDataRequestOption option = StiDataRequestOption.AllowOpenConnections;
		return StiElementDataCache.GetOrCreate(base.Element, option);
	}

	public override void CreateChild()
	{
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Expected O, but got Unknown
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Expected O, but got Unknown
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Expected O, but got Unknown
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Expected O, but got Unknown
		lockWidth = true;
		StiTableElement stiTableElement = base.Element as StiTableElement;
		StiTableElementStyle tableStyle = StiDashboardStyleHelper.GetTableStyle(stiTableElement);
		AllowFooter = stiTableElement.Columns.FirstOrDefault((StiTableColumn x) => x.ShowTotalSummary) != null;
		StiTableViewControl obj = new StiTableViewControl(stiTableElement, null)
		{
			AllowBaseDrawing = false
		};
		((DataGridView)obj).AutoSizeRowsMode = (DataGridViewAutoSizeRowsMode)10;
		((DataGridView)obj).DataSource = null;
		((DataGridView)obj).ColumnHeadersHeight = StiElementConsts.Table.GetHeight(stiTableElement.HeaderFont, StiScale.Factor);
		((Control)obj).Tag = stiTableElement;
		((Control)obj).Font = StiElementConsts.Table.Font.GetGdiFont(1.0, null, stiTableElement.Font);
		obj.ColumnHeadersDefaultCellStyle.Font = stiTableElement.HeaderFont;
		((DataGridView)obj).RowTemplate.MinimumHeight = StiElementConsts.Table.GetHeight(stiTableElement.Font, StiScale.Factor);
		((Control)obj).Dock = (DockStyle)5;
		((DataGridView)obj).BackgroundColor = StiDashboardStyleHelper.GetBackColor(base.Element);
		((DataGridView)obj).SelectionMode = GetSelectionMode(stiTableElement);
		base.Child = (Control)(object)obj;
		if (AllowFooter)
		{
			StiTableFooterControl stiTableFooterControl = new StiTableFooterControl(TableViewControl, base.Element as StiTableElement, tableStyle);
			((Control)stiTableFooterControl).Height = (int)(28.0 * StiScale.Factor);
			((Control)stiTableFooterControl).BackColor = StiDashboardStyleHelper.GetBackColor(base.Element);
			((Control)stiTableFooterControl).Dock = (DockStyle)2;
			((Control)stiTableFooterControl).Font = StiElementConsts.Table.Font.GetGdiFont(1.0, null, stiTableElement.FooterFont);
			((Control)stiTableFooterControl).ForeColor = stiTableElement.FooterForeColor;
			FooterControl = stiTableFooterControl;
		}
		((DataGridView)TableViewControl).CellPainting += new DataGridViewCellPaintingEventHandler(TableViewControl_CellPainting);
		((DataGridView)TableViewControl).CellFormatting += new DataGridViewCellFormattingEventHandler(TableViewControl_CellFormatting);
		((DataGridView)TableViewControl).ColumnWidthChanged += new DataGridViewColumnEventHandler(TableViewControl_ColumnWidthChanged);
		base.Child.MouseEnter += Child_RefreshActionButtons;
		base.Child.MouseLeave += Child_RefreshActionButtons;
		base.Child.MouseMove += new MouseEventHandler(Child_RefreshActionButtons);
		((Control)this).Controls.Add(base.Child);
		if (FooterControl != null)
		{
			((Control)this).Controls.Add((Control)(object)FooterControl);
		}
		lockWidth = false;
	}

	private static DataGridViewSelectionMode GetSelectionMode(StiTableElement table)
	{
		if (((StiTableDashboardInteraction)table.DashboardInteraction).FullRowSelect)
		{
			return (DataGridViewSelectionMode)1;
		}
		return (DataGridViewSelectionMode)0;
	}

	private static Color GetForeColor(StiTableElement table, StiTableColumn column, Color baseColor)
	{
		if (column != null && column.ForeColor != Color.Transparent)
		{
			return column.ForeColor;
		}
		if (table.ForeColor != Color.Transparent)
		{
			return table.ForeColor;
		}
		return baseColor;
	}

	public override void RefreshChild(StiDataTable dataTable)
	{
		lockWidth = true;
		base.RefreshChild(dataTable);
		base.IsShowNoResult = dataTable == null;
		base.IsShowEmptyData = dataTable != null && dataTable.IsEmpty;
		if (base.IsShowNoResult)
		{
			return;
		}
		if (FooterControl != null)
		{
			((Control)FooterControl).Visible = dataTable != null && dataTable.Rows.Count > 0;
		}
		if (dataTable == null)
		{
			return;
		}
		StiTableElement tableElement = base.Element as StiTableElement;
		DataTable netTable = dataTable.ToNetTable();
		StiDataGridViewPosition stiDataGridViewPosition = TableViewControl.SavePosition();
		TableViewControl.DataTable = dataTable;
		((DataGridView)TableViewControl).DataSource = netTable;
		int columnIndex = 0;
		if (FooterControl != null)
		{
			FooterControl.BlockColumnWidthChanged = true;
		}
		((DataGridView)TableViewControl).Columns.Clear();
		if (tableElement.Columns.Any((StiTableColumn c) => StiTableColumnVisibilityHelper.GetVisible(c, tableElement.Report)))
		{
			tableElement.Columns.ForEach(delegate(StiTableColumn column)
			{
				//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
				bool visible = StiTableColumnVisibilityHelper.GetVisible(column, tableElement.Report);
				DataColumn dataColumn = ((netTable.Columns.Count > columnIndex) ? netTable.Columns[columnIndex] : null);
				if (dataColumn != null && visible)
				{
					StiTableColumnSizeHelper stiTableColumnSizeHelper = new StiTableColumnSizeHelper(tableElement.Font, column);
					Type dataType = dataColumn.DataType;
					DataGridViewColumn dataGridViewColumn = GetDataGridViewColumn(TableViewControl, dataTable, column, columnIndex, dataType);
					dataGridViewColumn.Name = dataColumn.ColumnName;
					dataGridViewColumn.HeaderText = StiLabelHelper.GetLabel(column);
					dataGridViewColumn.DataPropertyName = dataColumn.ColumnName;
					((DataGridViewBand)dataGridViewColumn).DefaultCellStyle.Alignment = ConvertAlignment(column);
					((DataGridViewBand)dataGridViewColumn).DefaultCellStyle.WrapMode = (DataGridViewTriState)(stiTableColumnSizeHelper.WordWrap ? 1 : 2);
					((DataGridViewBand)dataGridViewColumn).Frozen = columnIndex < tableElement.FrozenColumns && tableElement.SizeMode != StiTableSizeMode.Fit;
					((DataGridViewBand)dataGridViewColumn).Tag = column;
					if (stiTableColumnSizeHelper.MinWidth > 0)
					{
						dataGridViewColumn.MinimumWidth = StiScale.I(stiTableColumnSizeHelper.MinWidth);
					}
					if (stiTableColumnSizeHelper.Width > 0)
					{
						dataGridViewColumn.Width = StiScale.I(stiTableColumnSizeHelper.Width);
					}
					if (tableElement.SizeMode == StiTableSizeMode.Fit)
					{
						dataGridViewColumn.AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
					}
					TableViewControl.GridColumnToTable[dataGridViewColumn] = tableElement;
					TableViewControl.GridColumnToTableColumn[dataGridViewColumn] = column;
					((DataGridView)TableViewControl).Columns.Add(dataGridViewColumn);
				}
				columnIndex++;
			});
		}
		StiTableSizer.MakeAutoSize((DataGridView)(object)TableViewControl, skipLimit: false, isDashboardShowing: true);
		((DataGridView)TableViewControl).ColumnHeadersHeight = (int)StiTableElementGdiPainter.MeasureHeaders(tableElement, tableElement.HeaderFont).Height;
		if (FooterControl != null)
		{
			FooterControl.BlockColumnWidthChanged = false;
		}
		FooterControl?.UpdateData();
		TableViewControl.ApplySort();
		TableViewControl.RestorePosition(TableViewControl.PreFilterPosition ?? stiDataGridViewPosition);
		lockWidth = false;
	}

	protected override void SetChildVisibilityFromNoResult(StiDataTable dataTable)
	{
		SetChildVisibility(dataTable != null);
	}

	private static DataGridViewColumn GetDataGridViewColumn(StiTableViewControl grid, StiDataTable dataTable, StiTableColumn column, int columnIndex, Type columnType)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Expected O, but got Unknown
		if (column is StiDataBarsColumn || column is StiColorScaleColumn || column is StiSparklinesColumn || column is StiIndicatorColumn || column is StiBubbleColumn)
		{
			DataGridViewColumn val = new DataGridViewColumn
			{
				CellTemplate = (DataGridViewCell)(object)new StiTableViewTextBoxCell()
			};
			grid.GridColumnToMin[val] = StiTableElementHelper.MinDataColumnValue(column, dataTable, columnIndex);
			grid.GridColumnToMax[val] = StiTableElementHelper.MaxDataColumnValue(column, dataTable, columnIndex);
			return val;
		}
		if (!(columnType == typeof(bool)))
		{
			if (columnType == typeof(byte[]))
			{
				DataGridViewImageColumn val2 = new DataGridViewImageColumn();
				StiTableViewImageCell stiTableViewImageCell = new StiTableViewImageCell();
				((DataGridViewImageCell)stiTableViewImageCell).ImageLayout = (DataGridViewImageCellLayout)3;
				((DataGridViewColumn)val2).CellTemplate = (DataGridViewCell)(object)stiTableViewImageCell;
				return (DataGridViewColumn)val2;
			}
			bool flag = column is StiDimensionColumn && ((StiDimensionColumn)column).ShowHyperlink;
			return new DataGridViewColumn
			{
				CellTemplate = (DataGridViewCell)(flag ? ((object)new StiTableViewLinkCell(column)) : ((object)new StiTableViewTextBoxCell()))
			};
		}
		return (DataGridViewColumn)new DataGridViewCheckBoxColumn
		{
			CellTemplate = (DataGridViewCell)(object)new StiTableViewCheckBoxCell()
		};
	}

	private static DataGridViewContentAlignment ConvertAlignment(object component)
	{
		IStiHorAlignment stiHorAlignment = component as IStiHorAlignment;
		return (DataGridViewContentAlignment)(stiHorAlignment.HorAlignment switch
		{
			StiHorAlignment.Left => 16, 
			StiHorAlignment.Center => 32, 
			StiHorAlignment.Right => 64, 
			_ => throw new NotSupportedException(), 
		});
	}

	internal void HideAllColumns()
	{
		((IEnumerable)((DataGridView)TableViewControl).Columns).Cast<DataGridViewColumn>().ToList().ForEach(delegate(DataGridViewColumn c)
		{
			((DataGridViewBand)c).Visible = false;
		});
	}

	protected override void Dispose(bool disposing)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		if (disposing)
		{
			if (TableViewControl != null)
			{
				((DataGridView)TableViewControl).ColumnWidthChanged -= new DataGridViewColumnEventHandler(TableViewControl_ColumnWidthChanged);
				((DataGridView)TableViewControl).CellFormatting -= new DataGridViewCellFormattingEventHandler(TableViewControl_CellFormatting);
				((DataGridView)TableViewControl).CellPainting -= new DataGridViewCellPaintingEventHandler(TableViewControl_CellPainting);
			}
			if (base.Child != null)
			{
				base.Child.MouseEnter -= Child_RefreshActionButtons;
				base.Child.MouseLeave -= Child_RefreshActionButtons;
				base.Child.MouseMove -= new MouseEventHandler(Child_RefreshActionButtons);
			}
		}
		base.Dispose(disposing);
	}

	private void TableViewControl_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		if ((int)e.Column.AutoSizeMode != 16)
		{
			StiTableColumn column = TableViewControl.GridColumnToTableColumn[e.Column] as StiTableColumn;
			StiTableColumnSizeHelper stiTableColumnSizeHelper = new StiTableColumnSizeHelper(((Control)TableViewControl).Font, column);
			e.Column.Width = (int)stiTableColumnSizeHelper.GetColumnWidth(e.Column.Width, !lockWidth);
		}
	}

	private void TableViewControl_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
	{
		StiTableElement stiTableElement = base.Element as StiTableElement;
		StiTableElementStyle tableStyle = StiDashboardStyleHelper.GetTableStyle(stiTableElement);
		DataGridViewColumn key = ((DataGridView)TableViewControl).Columns[e.ColumnIndex];
		StiTableColumn column = TableViewControl.GridColumnToTableColumn[key] as StiTableColumn;
		new StiTablePartDrawer(e, (DataGridView)(object)(base.Child as StiTableViewControl), stiTableElement, tableStyle, stiTableElement.HeaderForeColor, stiTableElement.HeaderFont, -1, StiTableColumnSizeHelper.IsWordWrap(column)).DrawCell();
	}

	private void TableViewControl_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
	{
		StiTableElement stiTableElement = base.Element as StiTableElement;
		StiTableElementStyle tableStyle = StiDashboardStyleHelper.GetTableStyle(stiTableElement);
		StiTableColumn column = TableViewControl.GridColumnToTableColumn[((DataGridView)TableViewControl).Columns[e.ColumnIndex]] as StiTableColumn;
		if ((e.RowIndex & 1) == 1)
		{
			e.CellStyle.BackColor = tableStyle.AlternatingCellBackColor;
			e.CellStyle.ForeColor = GetForeColor(stiTableElement, column, tableStyle.AlternatingCellForeColor);
			e.CellStyle.SelectionBackColor = tableStyle.SelectedCellBackColor;
			e.CellStyle.SelectionForeColor = GetForeColor(stiTableElement, column, tableStyle.SelectedCellForeColor);
		}
		else
		{
			e.CellStyle.BackColor = tableStyle.CellBackColor;
			e.CellStyle.ForeColor = GetForeColor(stiTableElement, column, tableStyle.CellForeColor);
			e.CellStyle.SelectionBackColor = tableStyle.SelectedCellBackColor;
			e.CellStyle.SelectionForeColor = GetForeColor(stiTableElement, column, tableStyle.SelectedCellForeColor);
		}
	}

	private void Child_RefreshActionButtons(object sender, EventArgs e)
	{
		RefreshActionButtons();
	}

	public StiTableElementControl(IStiElement element)
		: base(element)
	{
	}
}
