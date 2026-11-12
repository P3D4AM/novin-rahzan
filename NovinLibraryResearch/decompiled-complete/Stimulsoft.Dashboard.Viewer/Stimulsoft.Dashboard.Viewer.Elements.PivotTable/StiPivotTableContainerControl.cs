using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.PivotTable;
using Stimulsoft.Dashboard.Drawing.Painters.PivotTable;
using Stimulsoft.Report.CrossTab.Core;
using Stimulsoft.Report.Dashboard.Styles;

namespace Stimulsoft.Dashboard.Viewer.Elements.PivotTable;

[ToolboxItem(false)]
public class StiPivotTableContainerControl : StiBaseElementControl
{
	private StiPivotTableContainer container;

	private Control grid;

	public void Clear()
	{
		container.Clear();
		((Control)this).Invalidate();
	}

	public void MakeGrid(StiCross cross, StiPivotTableElement pivot)
	{
		container.MakeGrid(cross, pivot);
		((Control)this).Invalidate();
	}

	private void MoveSelection(Keys key)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Invalid comparison between Unknown and I4
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Invalid comparison between Unknown and I4
		if (container.SelectedCell == null)
		{
			return;
		}
		bool flag = false;
		StiPivotTableCell stiPivotTableCell = container.FindSelectableCell(container.SelectedCell, key);
		if (stiPivotTableCell != container.SelectedCell)
		{
			container.SelectedCell.IsSelected = false;
			container.SelectedCell = stiPivotTableCell;
			container.SelectedCell.IsSelected = true;
		}
		else
		{
			flag = true;
		}
		if (grid == null)
		{
			return;
		}
		StiScrollableControl panel = GetPanel(grid);
		Rectangle cellRect = container.SelectedCell.GetCellRect();
		Rectangle rectangle = new Rectangle(((ScrollProperties)((ScrollableControl)panel).HorizontalScroll).Value, ((ScrollProperties)((ScrollableControl)panel).VerticalScroll).Value, ((Control)panel).Width - (((ScrollProperties)((ScrollableControl)panel).VerticalScroll).Visible ? SystemInformation.VerticalScrollBarWidth : 0), ((Control)panel).Height - (((ScrollProperties)((ScrollableControl)panel).HorizontalScroll).Visible ? SystemInformation.HorizontalScrollBarHeight : 0));
		if (cellRect.Left < rectangle.Left)
		{
			SetScrollValue((ScrollProperties)(object)((ScrollableControl)panel).HorizontalScroll, ((ScrollProperties)((ScrollableControl)panel).HorizontalScroll).Value + cellRect.Left - rectangle.Left - 10);
		}
		else if (cellRect.Right > rectangle.Right)
		{
			if (rectangle.Width > cellRect.Width)
			{
				SetScrollValue((ScrollProperties)(object)((ScrollableControl)panel).HorizontalScroll, cellRect.Right - rectangle.Width + 10);
			}
			else
			{
				SetScrollValue((ScrollProperties)(object)((ScrollableControl)panel).HorizontalScroll, cellRect.Left - rectangle.Width);
			}
		}
		else if ((int)key == 37 && flag)
		{
			SetScrollValue((ScrollProperties)(object)((ScrollableControl)panel).HorizontalScroll, 0);
		}
		if (cellRect.Top < rectangle.Top)
		{
			SetScrollValue((ScrollProperties)(object)((ScrollableControl)panel).VerticalScroll, ((ScrollProperties)((ScrollableControl)panel).VerticalScroll).Value + cellRect.Top - rectangle.Top - 10);
		}
		else if (cellRect.Bottom > rectangle.Bottom)
		{
			if (rectangle.Height > cellRect.Height)
			{
				SetScrollValue((ScrollProperties)(object)((ScrollableControl)panel).VerticalScroll, cellRect.Bottom - rectangle.Height + 10);
			}
			else
			{
				SetScrollValue((ScrollProperties)(object)((ScrollableControl)panel).VerticalScroll, cellRect.Top - rectangle.Height);
			}
		}
		else if ((int)key == 38 && flag)
		{
			SetScrollValue((ScrollProperties)(object)((ScrollableControl)panel).VerticalScroll, 0);
		}
	}

	private StiScrollableControl GetPanel(Control control)
	{
		if (control == null)
		{
			return null;
		}
		foreach (object item in (ArrangedElementCollection)control.Controls)
		{
			if (item is StiScrollableControl)
			{
				return item as StiScrollableControl;
			}
		}
		return null;
	}

	private void SetScrollValue(ScrollProperties scroll, int value)
	{
		int num = Math.Max(scroll.Minimum, Math.Min(scroll.Maximum, value));
		if (scroll.Value != num)
		{
			scroll.Value = num;
			if (grid != null)
			{
				((Control)GetPanel(grid)).PerformLayout();
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && container != null)
		{
			container.AfterSizeUpdate -= Container_AfterSizeUpdate;
		}
		((Control)this).Dispose(disposing);
	}

	public void Container_AfterSizeUpdate(object sender, EventArgs e)
	{
		((Control)this).Invalidate();
		grid.PerformLayout();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		base.OnPaint(e);
		StiPivotElementStyle pivotTableStyle = StiDashboardStyleHelper.GetPivotTableStyle(container.PivotTable);
		container.GridPaint(e, pivotTableStyle);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		container.OnMouseMove(e);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		((Control)this).Focus();
		((Control)this).OnMouseDown(e);
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		((Control)this).OnMouseClick(e);
		foreach (StiPivotTableRow row in container.Rows)
		{
			for (int i = 0; i < row.Cells.Count; i++)
			{
				RectangleD rectangleD = new RectangleD(container.Columns[i].Left, row.Top, container.Columns[i].Width, row.Height);
				if (row.Visible && container.Columns[i].Visible && rectangleD.Contains(e.X, e.Y))
				{
					container.GetParent(row.Cells[i]).OnMouseClick(e);
					MoveSelection((Keys)0);
					return;
				}
			}
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		if (container.MouseEnterCell != null)
		{
			container.MouseEnterCell.OnMouseLeave(e);
			container.MouseEnterCell = null;
		}
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		MoveSelection(keyData);
		return true;
	}

	public StiPivotTableContainerControl(Control grid, StiPivotTableElement element)
		: base(element)
	{
		this.grid = grid;
		container = new StiPivotTableContainer((Control)(object)this, element);
		container.AfterSizeUpdate += Container_AfterSizeUpdate;
	}
}
