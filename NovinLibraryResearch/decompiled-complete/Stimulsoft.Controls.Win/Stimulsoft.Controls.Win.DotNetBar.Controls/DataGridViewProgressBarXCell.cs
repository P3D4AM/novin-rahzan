using System;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

public class DataGridViewProgressBarXCell : DataGridViewButtonCell
{
	public override Type EditType => null;

	public override Type FormattedValueType => typeof(string);

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
		DataGridViewProgressBarXColumn dataGridViewProgressBarXColumn = (DataGridViewProgressBarXColumn)(object)((DataGridViewCell)this).OwningColumn;
		Bitmap cellBitmap = dataGridViewProgressBarXColumn.GetCellBitmap(cellBounds);
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
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		DataGridViewProgressBarXColumn dataGridViewProgressBarXColumn = (DataGridViewProgressBarXColumn)(object)((DataGridViewCell)this).OwningColumn;
		ProgressBarX progressBarX = dataGridViewProgressBarXColumn.ProgressBarX;
		Rectangle contentBounds = GetContentBounds(cellBounds, localize: true);
		if (contentBounds.Width <= 0 || contentBounds.Height <= 0)
		{
			return;
		}
		string text = dataGridViewProgressBarXColumn.Text;
		dataGridViewProgressBarXColumn.InCallBack = true;
		try
		{
			((Control)progressBarX).Font = cellStyle.Font;
			((Control)progressBarX).ForeColor = cellStyle.ForeColor;
			((Control)progressBarX).BackColor = cellStyle.BackColor;
			progressBarX.Value = GetValue(value, progressBarX.Minimum);
			if (rowIndex < ((DataGridViewElement)this).DataGridView.RowCount)
			{
				((Control)progressBarX).Text = text;
				if (PartsSet(paintParts, (DataGridViewPaintParts)8))
				{
					dataGridViewProgressBarXColumn.OnBeforeCellPaint(rowIndex, ((DataGridViewCell)this).ColumnIndex);
				}
			}
			else
			{
				((Control)progressBarX).Text = "";
			}
			((Control)progressBarX).Bounds = contentBounds;
			progressBarX.RecalcLayout();
			((Control)progressBarX).DrawToBitmap(bm, contentBounds);
		}
		finally
		{
			dataGridViewProgressBarXColumn.InCallBack = false;
		}
	}

	protected override void OnMouseEnter(int rowIndex)
	{
		((DataGridViewCell)this).OnMouseEnter(rowIndex);
		DataGridViewProgressBarXColumn dataGridViewProgressBarXColumn = (DataGridViewProgressBarXColumn)(object)((DataGridViewCell)this).OwningColumn;
		ProgressBarX progressBarX = dataGridViewProgressBarXColumn.ProgressBarX;
		progressBarX.ProgressBarItem.InternalMouseEnter();
	}

	protected override void OnMouseLeave(int rowIndex)
	{
		((DataGridViewButtonCell)this).OnMouseLeave(rowIndex);
		DataGridViewProgressBarXColumn dataGridViewProgressBarXColumn = (DataGridViewProgressBarXColumn)(object)((DataGridViewCell)this).OwningColumn;
		ProgressBarX progressBarX = dataGridViewProgressBarXColumn.ProgressBarX;
		progressBarX.ProgressBarItem.InternalMouseLeave();
	}

	protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		((DataGridViewButtonCell)this).OnMouseMove(e);
		DataGridViewProgressBarXColumn dataGridViewProgressBarXColumn = (DataGridViewProgressBarXColumn)(object)((DataGridViewCell)this).OwningColumn;
		ProgressBarX progressBarX = dataGridViewProgressBarXColumn.ProgressBarX;
		progressBarX.ProgressBarItem.InternalMouseMove(new MouseEventArgs(((MouseEventArgs)e).Button, ((MouseEventArgs)e).Clicks, ((MouseEventArgs)e).X, ((MouseEventArgs)e).Y, ((MouseEventArgs)e).Delta));
	}

	protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		((DataGridViewButtonCell)this).OnMouseDown(e);
		DataGridViewProgressBarXColumn dataGridViewProgressBarXColumn = (DataGridViewProgressBarXColumn)(object)((DataGridViewCell)this).OwningColumn;
		ProgressBarX progressBarX = dataGridViewProgressBarXColumn.ProgressBarX;
		Rectangle cellDisplayRectangle = ((DataGridViewElement)this).DataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
		Rectangle contentBounds = GetContentBounds(cellDisplayRectangle, localize: false);
		Point location = ((MouseEventArgs)e).Location;
		location.Offset(contentBounds.Location);
		if (contentBounds.Contains(location))
		{
			((Control)progressBarX).Bounds = contentBounds;
			progressBarX.ProgressBarItem.InternalMouseDown(new MouseEventArgs(((MouseEventArgs)e).Button, ((MouseEventArgs)e).Clicks, ((MouseEventArgs)e).X, ((MouseEventArgs)e).Y, ((MouseEventArgs)e).Delta));
			((Control)((DataGridViewElement)this).DataGridView).Invalidate(contentBounds);
		}
	}

	protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		((DataGridViewButtonCell)this).OnMouseUp(e);
		DataGridViewProgressBarXColumn dataGridViewProgressBarXColumn = (DataGridViewProgressBarXColumn)(object)((DataGridViewCell)this).OwningColumn;
		ProgressBarX progressBarX = dataGridViewProgressBarXColumn.ProgressBarX;
		Rectangle cellDisplayRectangle = ((DataGridViewElement)this).DataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
		Rectangle rectangle = (((Control)progressBarX).Bounds = GetContentBounds(cellDisplayRectangle, localize: false));
		progressBarX.ProgressBarItem.InternalMouseUp(new MouseEventArgs(((MouseEventArgs)e).Button, ((MouseEventArgs)e).Clicks, ((MouseEventArgs)e).X, ((MouseEventArgs)e).Y, ((MouseEventArgs)e).Delta));
		((Control)((DataGridViewElement)this).DataGridView).Invalidate(rectangle);
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
		DataGridViewProgressBarXColumn dataGridViewProgressBarXColumn = (DataGridViewProgressBarXColumn)(object)((DataGridViewCell)this).OwningColumn;
		Rectangle rectangle = ((DataGridViewCell)this).BorderWidths(advancedBorderStyle);
		cellBounds.Offset(rectangle.X, rectangle.Y);
		cellBounds.Width -= rectangle.Right;
		cellBounds.Height -= rectangle.Bottom;
		if (((DataGridViewCell)this).Selected)
		{
			cellBounds.Width += ((DataGridViewColumn)dataGridViewProgressBarXColumn).DividerWidth;
		}
		return cellBounds;
	}

	private Rectangle GetContentBounds(Rectangle cellBounds, bool localize)
	{
		DataGridViewProgressBarXColumn dataGridViewProgressBarXColumn = (DataGridViewProgressBarXColumn)(object)((DataGridViewCell)this).OwningColumn;
		cellBounds.Width -= ((DataGridViewColumn)dataGridViewProgressBarXColumn).DividerWidth + 3;
		cellBounds.Height -= 3;
		if (localize)
		{
			cellBounds.Location = new Point(0, 0);
		}
		cellBounds.X++;
		cellBounds.Y++;
		return cellBounds;
	}

	private int GetValue(object value, int minimum)
	{
		if (value == Convert.DBNull || (value is string && string.IsNullOrEmpty((string)value)))
		{
			return minimum;
		}
		return Convert.ToInt32(value);
	}
}
