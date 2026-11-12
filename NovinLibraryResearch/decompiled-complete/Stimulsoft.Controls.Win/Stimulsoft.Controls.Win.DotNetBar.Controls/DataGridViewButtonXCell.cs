using System;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

public class DataGridViewButtonXCell : DataGridViewButtonCell
{
	public override Type EditType => null;

	public override Type FormattedValueType => typeof(string);

	protected override Rectangle GetContentBounds(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex)
	{
		Rectangle cellDisplayRectangle = ((DataGridViewElement)this).DataGridView.GetCellDisplayRectangle(((DataGridViewCell)this).ColumnIndex, rowIndex, false);
		cellDisplayRectangle.Location = new Point(0, 0);
		DataGridViewButtonXColumn dataGridViewButtonXColumn = (DataGridViewButtonXColumn)(object)((DataGridViewCell)this).OwningColumn;
		ButtonX buttonX = dataGridViewButtonXColumn.ButtonX;
		Rectangle buttonBounds = GetButtonBounds(cellDisplayRectangle, localize: true);
		if (buttonX.ButtonItem.SubItems.Count > 0)
		{
			buttonBounds.Width -= buttonX.ButtonItem.SubItemsExpandWidth;
		}
		return buttonBounds;
	}

	protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Invalid comparison between Unknown and I4
		if (((DataGridViewElement)this).DataGridView == null)
		{
			return;
		}
		if (PartsSet(paintParts, (DataGridViewPaintParts)2))
		{
			((DataGridViewCell)this).PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
		}
		if (!PartsSet(paintParts, (DataGridViewPaintParts)1))
		{
			return;
		}
		Rectangle backBounds = GetBackBounds(cellBounds, advancedBorderStyle);
		if (backBounds.Height <= 0 || backBounds.Width <= 0)
		{
			return;
		}
		DataGridViewButtonXColumn dataGridViewButtonXColumn = (DataGridViewButtonXColumn)(object)((DataGridViewCell)this).OwningColumn;
		Bitmap cellBitmap = dataGridViewButtonXColumn.GetCellBitmap(cellBounds);
		if (cellBitmap != null)
		{
			Graphics val = Graphics.FromImage((Image)(object)cellBitmap);
			try
			{
				PaintButtonBackground(val, cellStyle, backBounds);
				PaintButtonContent(cellBounds, rowIndex, formattedValue, cellStyle, paintParts, cellBitmap);
				graphics.DrawImageUnscaledAndClipped((Image)(object)cellBitmap, backBounds);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			if (((DataGridViewElement)this).DataGridView.ShowCellErrors && (paintParts & 0x10) == 16)
			{
				((DataGridViewCell)this).PaintErrorIcon(graphics, clipBounds, cellBounds, errorText);
			}
		}
	}

	private void PaintButtonBackground(Graphics g, DataGridViewCellStyle cellStyle, Rectangle rBack)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		Rectangle rectangle = rBack;
		rectangle.Location = new Point(0, 0);
		if (((DataGridViewElement)this).DataGridView is DataGridViewX dataGridViewX && ((Control)dataGridViewX).Enabled && ((DataGridViewCell)this).Selected)
		{
			Office2007ButtonItemPainter.PaintBackground(g, dataGridViewX.ButtonStateColorTable, rectangle, RoundRectangleShapeDescriptor.RectangleShape, isDefault: false, paintBorder: false);
			return;
		}
		Color color = (((DataGridViewCell)this).Selected ? cellStyle.SelectionBackColor : cellStyle.BackColor);
		Brush val = (Brush)new SolidBrush(color);
		try
		{
			g.FillRectangle(val, rectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void PaintButtonContent(Rectangle cellBounds, int rowIndex, object value, DataGridViewCellStyle cellStyle, DataGridViewPaintParts paintParts, Bitmap bm)
	{
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		DataGridViewButtonXColumn dataGridViewButtonXColumn = (DataGridViewButtonXColumn)(object)((DataGridViewCell)this).OwningColumn;
		ButtonX buttonX = dataGridViewButtonXColumn.ButtonX;
		Rectangle buttonBounds = GetButtonBounds(cellBounds, localize: true);
		if (buttonBounds.Width <= 0 || buttonBounds.Height <= 0)
		{
			return;
		}
		bool mouseIsOver = buttonX.ButtonItem.MouseIsOver;
		bool mouseIsOverExpand = buttonX.ButtonItem.MouseIsOverExpand;
		bool mouseIsDown = buttonX.ButtonItem.MouseIsDown;
		bool buttonIsExpanded = buttonX.ButtonItem.ButtonIsExpanded;
		try
		{
			string text = dataGridViewButtonXColumn.Text;
			dataGridViewButtonXColumn.InCellCallBack = true;
			if (rowIndex != dataGridViewButtonXColumn.ActiveRowIndex)
			{
				buttonX.ButtonItem.MouseIsOver = false;
				buttonX.ButtonItem.MouseIsOverExpand = false;
				buttonX.ButtonItem.MouseIsDown = false;
				buttonX.ButtonItem.ButtonIsExpanded = false;
			}
			((Control)buttonX).Font = cellStyle.Font;
			((Control)buttonX).ForeColor = cellStyle.ForeColor;
			((Control)buttonX).BackColor = cellStyle.BackColor;
			if (rowIndex < ((DataGridViewElement)this).DataGridView.RowCount)
			{
				((Control)buttonX).Text = (((DataGridViewButtonCell)this).UseColumnTextForButtonValue ? text : GetValue(value));
				if (PartsSet(paintParts, (DataGridViewPaintParts)8))
				{
					dataGridViewButtonXColumn.OnBeforeCellPaint(rowIndex, ((DataGridViewCell)this).ColumnIndex);
				}
			}
			else
			{
				((Control)buttonX).Text = "";
			}
			((Control)buttonX).Bounds = buttonBounds;
			buttonX.RecalcLayout();
			((Control)buttonX).DrawToBitmap(bm, buttonBounds);
		}
		finally
		{
			buttonX.ButtonItem.MouseIsOver = mouseIsOver;
			buttonX.ButtonItem.MouseIsOverExpand = mouseIsOverExpand;
			buttonX.ButtonItem.MouseIsDown = mouseIsDown;
			buttonX.ButtonItem.ButtonIsExpanded = buttonIsExpanded;
			dataGridViewButtonXColumn.InCellCallBack = false;
		}
	}

	protected override void OnMouseEnter(int rowIndex)
	{
		((DataGridViewCell)this).OnMouseEnter(rowIndex);
		DoMouseEnter(rowIndex);
	}

	internal void DoMouseEnter(int rowIndex)
	{
		DataGridViewButtonXColumn dataGridViewButtonXColumn = (DataGridViewButtonXColumn)(object)((DataGridViewCell)this).OwningColumn;
		ButtonX buttonX = dataGridViewButtonXColumn.ButtonX;
		dataGridViewButtonXColumn.CurrentRowIndex = rowIndex;
		if (!buttonX.Expanded)
		{
			dataGridViewButtonXColumn.ActiveRowIndex = rowIndex;
			buttonX.ButtonItem.InternalMouseEnter();
			RefreshButton(((DataGridViewCell)this).ColumnIndex, rowIndex);
		}
	}

	protected override void OnMouseLeave(int rowIndex)
	{
		((DataGridViewButtonCell)this).OnMouseLeave(rowIndex);
		DataGridViewButtonXColumn dataGridViewButtonXColumn = (DataGridViewButtonXColumn)(object)((DataGridViewCell)this).OwningColumn;
		ButtonX buttonX = dataGridViewButtonXColumn.ButtonX;
		dataGridViewButtonXColumn.CurrentRowIndex = -1;
		if (!buttonX.Expanded)
		{
			dataGridViewButtonXColumn.ActiveRowIndex = -1;
			buttonX.ButtonItem.InternalMouseLeave();
			RefreshButton(((DataGridViewCell)this).ColumnIndex, rowIndex);
		}
	}

	protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		((DataGridViewButtonCell)this).OnMouseMove(e);
		DataGridViewButtonXColumn dataGridViewButtonXColumn = (DataGridViewButtonXColumn)(object)((DataGridViewCell)this).OwningColumn;
		ButtonX buttonX = dataGridViewButtonXColumn.ButtonX;
		if (dataGridViewButtonXColumn.ActiveRowIndex == e.RowIndex || !buttonX.Expanded)
		{
			buttonX.ButtonItem.InternalMouseMove(new MouseEventArgs(((MouseEventArgs)e).Button, ((MouseEventArgs)e).Clicks, ((MouseEventArgs)e).X, ((MouseEventArgs)e).Y, ((MouseEventArgs)e).Delta));
			RefreshButton(e.ColumnIndex, e.RowIndex);
		}
	}

	protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
	{
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		((DataGridViewButtonCell)this).OnMouseDown(e);
		DataGridViewButtonXColumn dataGridViewButtonXColumn = (DataGridViewButtonXColumn)(object)((DataGridViewCell)this).OwningColumn;
		ButtonX buttonX = dataGridViewButtonXColumn.ButtonX;
		if (!((Control)((DataGridViewElement)this).DataGridView).Controls.Contains((Control)(object)buttonX))
		{
			((Control)((DataGridViewElement)this).DataGridView).Controls.Add((Control)(object)buttonX);
		}
		if (!dataGridViewButtonXColumn.ExpandClosed || !buttonX.ButtonItem.MouseIsOverExpand)
		{
			Rectangle cellDisplayRectangle = ((DataGridViewElement)this).DataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
			Rectangle buttonBounds = GetButtonBounds(cellDisplayRectangle, localize: false);
			Point location = ((MouseEventArgs)e).Location;
			location.Offset(buttonBounds.Location);
			if (buttonBounds.Contains(location))
			{
				((Control)buttonX).Bounds = buttonBounds;
				buttonX.ButtonItem.InternalMouseDown(new MouseEventArgs(((MouseEventArgs)e).Button, ((MouseEventArgs)e).Clicks, ((MouseEventArgs)e).X, ((MouseEventArgs)e).Y, ((MouseEventArgs)e).Delta));
				((Control)((DataGridViewElement)this).DataGridView).Invalidate(buttonBounds);
			}
		}
		dataGridViewButtonXColumn.ExpandClosed = false;
	}

	protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		((DataGridViewButtonCell)this).OnMouseUp(e);
		DataGridViewButtonXColumn dataGridViewButtonXColumn = (DataGridViewButtonXColumn)(object)((DataGridViewCell)this).OwningColumn;
		ButtonX buttonX = dataGridViewButtonXColumn.ButtonX;
		Rectangle cellDisplayRectangle = ((DataGridViewElement)this).DataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
		Rectangle buttonBounds = GetButtonBounds(cellDisplayRectangle, localize: false);
		((Control)((DataGridViewElement)this).DataGridView).Invalidate(buttonBounds);
		((Control)buttonX).Bounds = buttonBounds;
		buttonX.ButtonItem.InternalMouseUp(new MouseEventArgs(((MouseEventArgs)e).Button, ((MouseEventArgs)e).Clicks, ((MouseEventArgs)e).X, ((MouseEventArgs)e).Y, ((MouseEventArgs)e).Delta));
	}

	private bool PartsSet(DataGridViewPaintParts paintParts, DataGridViewPaintParts parts)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return (DataGridViewPaintParts)(paintParts & parts) == parts;
	}

	private Rectangle GetBackBounds(Rectangle cellBounds, DataGridViewAdvancedBorderStyle advancedBorderStyle)
	{
		DataGridViewButtonXColumn dataGridViewButtonXColumn = (DataGridViewButtonXColumn)(object)((DataGridViewCell)this).OwningColumn;
		Rectangle rectangle = ((DataGridViewCell)this).BorderWidths(advancedBorderStyle);
		cellBounds.Offset(rectangle.X, rectangle.Y);
		cellBounds.Width -= rectangle.Right;
		cellBounds.Height -= rectangle.Bottom;
		if (((DataGridViewCell)this).Selected)
		{
			cellBounds.Width += ((DataGridViewColumn)dataGridViewButtonXColumn).DividerWidth;
		}
		return cellBounds;
	}

	private Rectangle GetButtonBounds(Rectangle cellBounds, bool localize)
	{
		DataGridViewButtonXColumn dataGridViewButtonXColumn = (DataGridViewButtonXColumn)(object)((DataGridViewCell)this).OwningColumn;
		cellBounds.Width -= ((DataGridViewColumn)dataGridViewButtonXColumn).DividerWidth + 3;
		cellBounds.Height -= 3;
		if (localize)
		{
			cellBounds.Location = new Point(0, 0);
		}
		cellBounds.X++;
		cellBounds.Y++;
		return cellBounds;
	}

	private string GetValue(object value)
	{
		if (value == Convert.DBNull)
		{
			return "";
		}
		return Convert.ToString(value);
	}

	private void RefreshButton(int columnIndex, int rowIndex)
	{
		Rectangle cellDisplayRectangle = ((DataGridViewElement)this).DataGridView.GetCellDisplayRectangle(columnIndex, rowIndex, false);
		Rectangle buttonBounds = GetButtonBounds(cellDisplayRectangle, localize: false);
		((Control)((DataGridViewElement)this).DataGridView).Invalidate(buttonBounds);
	}
}
