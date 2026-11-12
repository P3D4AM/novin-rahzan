using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

public class DataGridViewSliderCell : DataGridViewTextBoxCell
{
	private DataGridViewCellStyle _CellStyle;

	public override Type EditType => null;

	public override Type ValueType => ((DataGridViewTextBoxCell)this).ValueType ?? typeof(int);

	private Rectangle GetAdjustedEditingControlBounds(Rectangle editingControlBounds, DataGridViewCellStyle cellStyle)
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Invalid comparison between Unknown and I4
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Invalid comparison between Unknown and I4
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Invalid comparison between Unknown and I4
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Invalid comparison between Unknown and I4
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Invalid comparison between Unknown and I4
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Invalid comparison between Unknown and I4
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Invalid comparison between Unknown and I4
		editingControlBounds.X++;
		editingControlBounds.Y++;
		editingControlBounds.Width = Math.Max(0, editingControlBounds.Width - 2);
		editingControlBounds.Height = Math.Max(0, editingControlBounds.Height - 2);
		Rectangle contentBounds = GetContentBounds(editingControlBounds);
		if (contentBounds.Height < editingControlBounds.Height)
		{
			DataGridViewContentAlignment alignment = cellStyle.Alignment;
			if ((int)alignment <= 64)
			{
				if ((int)alignment == 16 || (int)alignment == 32 || (int)alignment == 64)
				{
					editingControlBounds.Y += (editingControlBounds.Height - contentBounds.Height) / 2;
				}
			}
			else if ((int)alignment == 256 || (int)alignment == 512 || (int)alignment == 1024)
			{
				editingControlBounds.Y += editingControlBounds.Height - contentBounds.Height;
			}
		}
		editingControlBounds.Width = Math.Max(1, editingControlBounds.Width);
		editingControlBounds.Height = Math.Max(1, contentBounds.Height);
		return editingControlBounds;
	}

	protected override Size GetPreferredSize(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
	{
		Size result = new Size(-1, -1);
		if (((DataGridViewElement)this).DataGridView != null && ((DataGridViewCell)this).OwningColumn is DataGridViewSliderColumn { Slider: var slider })
		{
			result.Height = ((Control)slider).Height;
			if (constraintSize.Width == 0)
			{
				result.Width += 80;
				if (slider.LabelVisible)
				{
					result.Width += slider.LabelWidth;
				}
			}
		}
		return result;
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
		DataGridViewSliderColumn dataGridViewSliderColumn = (DataGridViewSliderColumn)(object)((DataGridViewCell)this).OwningColumn;
		Bitmap cellBitmap = dataGridViewSliderColumn.GetCellBitmap(cellBounds);
		if (cellBitmap != null)
		{
			Graphics val = Graphics.FromImage((Image)(object)cellBitmap);
			try
			{
				PaintSliderBackground(val, cellStyle, backBounds);
				PaintSliderContent(cellBounds, rowIndex, formattedValue, cellStyle, paintParts, val);
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

	private void PaintSliderBackground(Graphics g, DataGridViewCellStyle cellStyle, Rectangle rBack)
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

	private void PaintSliderContent(Rectangle cellBounds, int rowIndex, object value, DataGridViewCellStyle cellStyle, DataGridViewPaintParts paintParts, Graphics g)
	{
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Expected O, but got Unknown
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		DataGridViewSliderColumn dataGridViewSliderColumn = (DataGridViewSliderColumn)(object)((DataGridViewCell)this).OwningColumn;
		Slider slider = dataGridViewSliderColumn.Slider;
		_CellStyle = cellStyle;
		int value2 = slider.Value;
		eSliderPart mouseOverPart = slider.SliderItem.MouseOverPart;
		eSliderPart mouseDownPart = slider.SliderItem.MouseDownPart;
		GraphicsState val = g.Save();
		try
		{
			cellBounds.X = 0;
			cellBounds.Y = 0;
			cellBounds.Width -= ((DataGridViewColumn)dataGridViewSliderColumn).DividerWidth + 1;
			cellBounds.Height--;
			if (rowIndex != dataGridViewSliderColumn.ActiveRowIndex)
			{
				slider.SliderItem.MouseOverPart = eSliderPart.None;
				slider.SliderItem.MouseDownPart = eSliderPart.None;
			}
			((Control)slider).Font = cellStyle.Font;
			((Control)slider).ForeColor = cellStyle.ForeColor;
			((Control)slider).BackColor = (((DataGridViewCell)this).Selected ? Color.Transparent : cellStyle.BackColor);
			if (rowIndex < ((DataGridViewElement)this).DataGridView.RowCount)
			{
				((Control)slider).Text = dataGridViewSliderColumn.Text;
				if (rowIndex != dataGridViewSliderColumn.ActiveRowIndex)
				{
					slider.Value = GetSliderValue(value);
				}
				if (PartsSet(paintParts, (DataGridViewPaintParts)8))
				{
					dataGridViewSliderColumn.OnBeforeCellPaint(rowIndex, ((DataGridViewCell)this).ColumnIndex);
				}
			}
			else
			{
				((Control)slider).Text = "";
				slider.Value = GetSliderValue(value);
			}
			Rectangle adjustedEditingControlBounds = GetAdjustedEditingControlBounds(cellBounds, cellStyle);
			g.TranslateTransform((float)adjustedEditingControlBounds.X, (float)adjustedEditingControlBounds.Y);
			slider.CallBasePaintBackground = false;
			((Control)slider).Bounds = adjustedEditingControlBounds;
			slider.InternalPaint(new PaintEventArgs(g, Rectangle.Empty));
		}
		finally
		{
			g.Restore(val);
			slider.Value = value2;
			slider.SliderItem.MouseOverPart = mouseOverPart;
			slider.SliderItem.MouseDownPart = mouseDownPart;
		}
	}

	protected override void OnMouseEnter(int rowIndex)
	{
		((DataGridViewCell)this).OnMouseEnter(rowIndex);
		DoMouseEnter(rowIndex, refresh: true);
	}

	private void DoMouseEnter(int rowIndex, bool refresh)
	{
		DataGridViewSliderColumn dataGridViewSliderColumn = (DataGridViewSliderColumn)(object)((DataGridViewCell)this).OwningColumn;
		Slider slider = dataGridViewSliderColumn.Slider;
		if (((DataGridViewCell)this).Visible)
		{
			dataGridViewSliderColumn.ActiveRowIndex = rowIndex;
			slider.Value = GetSliderValue(((DataGridViewCell)this).Value);
			slider.SliderItem.InternalMouseEnter();
			if (refresh)
			{
				RefreshSlider(((DataGridViewCell)this).ColumnIndex, rowIndex);
			}
		}
	}

	protected override void OnMouseLeave(int rowIndex)
	{
		((DataGridViewCell)this).OnMouseLeave(rowIndex);
		DataGridViewSliderColumn dataGridViewSliderColumn = (DataGridViewSliderColumn)(object)((DataGridViewCell)this).OwningColumn;
		Slider slider = dataGridViewSliderColumn.Slider;
		if (dataGridViewSliderColumn.ActiveRowIndex >= 0)
		{
			dataGridViewSliderColumn.ActiveRowIndex = -1;
			slider.SliderItem.InternalMouseLeave();
			((DataGridViewCell)this).Value = slider.Value;
			RefreshSlider(((DataGridViewCell)this).ColumnIndex, rowIndex);
		}
	}

	protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		((DataGridViewCell)this).OnMouseMove(e);
		if (((DataGridViewCell)this).OwningColumn is DataGridViewSliderColumn dataGridViewSliderColumn)
		{
			if (dataGridViewSliderColumn.ActiveRowIndex == -1)
			{
				DoMouseEnter(e.RowIndex, refresh: false);
			}
			if (dataGridViewSliderColumn.ActiveRowIndex >= 0)
			{
				Point point = CellAlignPoint(((MouseEventArgs)e).X, ((MouseEventArgs)e).Y);
				dataGridViewSliderColumn.Slider.SliderItem.InternalMouseMove(new MouseEventArgs(((MouseEventArgs)e).Button, ((MouseEventArgs)e).Clicks, point.X, point.Y, ((MouseEventArgs)e).Delta));
				RefreshSlider(e.ColumnIndex, e.RowIndex);
			}
		}
	}

	protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		((DataGridViewCell)this).OnMouseDown(e);
		if (((DataGridViewCell)this).OwningColumn is DataGridViewSliderColumn dataGridViewSliderColumn)
		{
			Point point = CellAlignPoint(((MouseEventArgs)e).X, ((MouseEventArgs)e).Y);
			dataGridViewSliderColumn.Slider.SliderItem.InternalMouseDown(new MouseEventArgs(((MouseEventArgs)e).Button, ((MouseEventArgs)e).Clicks, point.X, point.Y, ((MouseEventArgs)e).Delta));
			RefreshSlider(e.ColumnIndex, e.RowIndex);
		}
	}

	protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		((DataGridViewCell)this).OnMouseUp(e);
		if (((DataGridViewCell)this).OwningColumn is DataGridViewSliderColumn dataGridViewSliderColumn)
		{
			Point point = CellAlignPoint(((MouseEventArgs)e).X, ((MouseEventArgs)e).Y);
			dataGridViewSliderColumn.Slider.SliderItem.InternalMouseUp(new MouseEventArgs(((MouseEventArgs)e).Button, ((MouseEventArgs)e).Clicks, point.X, point.Y, ((MouseEventArgs)e).Delta));
			((DataGridViewCell)this).Value = dataGridViewSliderColumn.Slider.Value;
			RefreshSlider(e.ColumnIndex, e.RowIndex);
		}
	}

	private Point CellAlignPoint(int x, int y)
	{
		return new Point(x, y - GetAdjustedEditingControlBounds(((DataGridViewCell)this).ContentBounds, _CellStyle).Y);
	}

	internal int GetSliderValue(object value)
	{
		if (value == Convert.DBNull || (value is string && string.IsNullOrEmpty((string)value)))
		{
			return 0;
		}
		return Convert.ToInt32(value);
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
		DataGridViewSliderColumn dataGridViewSliderColumn = (DataGridViewSliderColumn)(object)((DataGridViewCell)this).OwningColumn;
		Rectangle rectangle = ((DataGridViewCell)this).BorderWidths(advancedBorderStyle);
		cellBounds.Offset(rectangle.X, rectangle.Y);
		cellBounds.Width -= rectangle.Right;
		cellBounds.Height -= rectangle.Bottom;
		if (((DataGridViewCell)this).Selected)
		{
			cellBounds.Width += ((DataGridViewColumn)dataGridViewSliderColumn).DividerWidth;
		}
		return cellBounds;
	}

	private Rectangle GetContentBounds(Rectangle cellBounds)
	{
		DataGridViewSliderColumn dataGridViewSliderColumn = (DataGridViewSliderColumn)(object)((DataGridViewCell)this).OwningColumn;
		if (((Control)dataGridViewSliderColumn.Slider).Parent == null)
		{
			Form val = ((Control)((DataGridViewElement)dataGridViewSliderColumn).DataGridView).FindForm();
			if (val != null)
			{
				((Control)dataGridViewSliderColumn.Slider).Parent = (Control)(object)val;
			}
		}
		dataGridViewSliderColumn.Slider.SliderItem.RecalcSize();
		cellBounds.Width -= ((DataGridViewColumn)dataGridViewSliderColumn).DividerWidth + 3;
		cellBounds.Height = dataGridViewSliderColumn.Slider.SliderItem.HeightInternal;
		return cellBounds;
	}

	internal void RefreshSlider(int columnIndex, int rowIndex)
	{
		((DataGridViewElement)this).DataGridView.InvalidateCell(columnIndex, rowIndex);
	}
}
