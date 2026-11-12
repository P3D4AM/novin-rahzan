using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

public class DataGridViewCheckBoxXCell : DataGridViewCell
{
	private bool _MouseEntered;

	private string _Text;

	public override Type EditType => null;

	public override Type FormattedValueType => typeof(string);

	private Rectangle GetAdjustedEditingControlBounds(Rectangle editingControlBounds, DataGridViewCellStyle cellStyle)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Invalid comparison between Unknown and I4
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Invalid comparison between Unknown and I4
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Invalid comparison between Unknown and I4
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Invalid comparison between Unknown and I4
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Invalid comparison between Unknown and I4
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Invalid comparison between Unknown and I4
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Invalid comparison between Unknown and I4
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Invalid comparison between Unknown and I4
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Invalid comparison between Unknown and I4
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Invalid comparison between Unknown and I4
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Invalid comparison between Unknown and I4
		editingControlBounds.X++;
		editingControlBounds.Y++;
		editingControlBounds.Width = Math.Max(0, editingControlBounds.Width - 2);
		editingControlBounds.Height = Math.Max(0, editingControlBounds.Height - 2);
		Rectangle contentBounds = GetContentBounds(editingControlBounds);
		if (cellStyle != null)
		{
			DataGridViewContentAlignment alignment = cellStyle.Alignment;
			if ((int)alignment <= 32)
			{
				if ((int)alignment <= 4)
				{
					if ((int)alignment != 2)
					{
						if ((int)alignment == 4)
						{
							editingControlBounds.X = editingControlBounds.Right - contentBounds.Width;
						}
					}
					else
					{
						editingControlBounds.X += (editingControlBounds.Width - contentBounds.Width) / 2;
					}
				}
				else if ((int)alignment != 16)
				{
					if ((int)alignment == 32)
					{
						editingControlBounds.X += (editingControlBounds.Width - contentBounds.Width) / 2;
						editingControlBounds.Y += (editingControlBounds.Height - contentBounds.Height) / 2;
					}
				}
				else
				{
					editingControlBounds.Y += (editingControlBounds.Height - contentBounds.Height) / 2;
				}
			}
			else if ((int)alignment <= 256)
			{
				if ((int)alignment != 64)
				{
					if ((int)alignment == 256)
					{
						editingControlBounds.Y += editingControlBounds.Height - contentBounds.Height;
					}
				}
				else
				{
					editingControlBounds.X = editingControlBounds.Right - contentBounds.Width;
					editingControlBounds.Y += (editingControlBounds.Height - contentBounds.Height) / 2;
				}
			}
			else if ((int)alignment != 512)
			{
				if ((int)alignment == 1024)
				{
					editingControlBounds.X = editingControlBounds.Right - contentBounds.Width;
					editingControlBounds.Y += editingControlBounds.Height - contentBounds.Height;
				}
			}
			else
			{
				editingControlBounds.X += (editingControlBounds.Width - contentBounds.Width) / 2;
				editingControlBounds.Y += editingControlBounds.Height - contentBounds.Height;
			}
		}
		if (editingControlBounds.Y < 1)
		{
			editingControlBounds.Y = 1;
		}
		editingControlBounds.Width = Math.Max(1, editingControlBounds.Width);
		editingControlBounds.Height = Math.Max(1, contentBounds.Height);
		return editingControlBounds;
	}

	protected override Size GetPreferredSize(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
	{
		if (((DataGridViewElement)this).DataGridView == null)
		{
			return new Size(-1, -1);
		}
		Size empty = Size.Empty;
		if (constraintSize.Width == 0 && ((DataGridViewCell)this).OwningColumn is DataGridViewCheckBoxXColumn dataGridViewCheckBoxXColumn)
		{
			((DataGridViewCell)this).GetFormattedValue(((DataGridViewCell)this).GetValue(rowIndex), rowIndex, ref cellStyle, (TypeConverter)null, (TypeConverter)null, (DataGridViewDataErrorContexts)0);
			if (rowIndex < ((DataGridViewElement)this).DataGridView.RowCount)
			{
				dataGridViewCheckBoxXColumn.OnBeforeCellPaint(rowIndex, ((DataGridViewCell)this).ColumnIndex);
			}
			CheckBoxX checkBoxX = dataGridViewCheckBoxXColumn.CheckBoxX;
			empty.Width = (int)graphics.MeasureString(((Control)checkBoxX).Text, cellStyle.Font).Width + 4;
			empty.Height = ((Control)checkBoxX).Height;
			empty.Width += checkBoxX.CheckBoxItem.CheckSignSize.Width;
		}
		return empty;
	}

	protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		return ((DataGridViewCell)this).GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
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
		DataGridViewCheckBoxXColumn dataGridViewCheckBoxXColumn = (DataGridViewCheckBoxXColumn)(object)((DataGridViewCell)this).OwningColumn;
		Bitmap cellBitmap = dataGridViewCheckBoxXColumn.GetCellBitmap(cellBounds);
		if (cellBitmap != null)
		{
			Graphics val = Graphics.FromImage((Image)(object)cellBitmap);
			try
			{
				PaintButtonBackground(val, cellStyle, backBounds);
				PaintButtonContent(cellBounds, rowIndex, value, cellStyle, paintParts, val);
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

	private void PaintButtonContent(Rectangle cellBounds, int rowIndex, object value, DataGridViewCellStyle cellStyle, DataGridViewPaintParts paintParts, Graphics g)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Expected O, but got Unknown
		DataGridViewCheckBoxXColumn dataGridViewCheckBoxXColumn = (DataGridViewCheckBoxXColumn)(object)((DataGridViewCell)this).OwningColumn;
		CheckBoxX checkBoxX = dataGridViewCheckBoxXColumn.CheckBoxX;
		string text = ((Control)checkBoxX).Text;
		bool @checked = checkBoxX.Checked;
		object checkValue = checkBoxX.CheckValue;
		CheckState checkState = checkBoxX.CheckState;
		bool isMouseOver = checkBoxX.CheckBoxItem.IsMouseOver;
		bool isMouseDown = checkBoxX.CheckBoxItem.IsMouseDown;
		GraphicsState val = g.Save();
		try
		{
			dataGridViewCheckBoxXColumn.InCellCallBack = true;
			cellBounds.X = 0;
			cellBounds.Y = 0;
			cellBounds.Width -= ((DataGridViewColumn)dataGridViewCheckBoxXColumn).DividerWidth + 1;
			cellBounds.Height--;
			checkBoxX.CheckValue = (dataGridViewCheckBoxXColumn.ThreeState ? value : ((object)GetCheckValue(dataGridViewCheckBoxXColumn, value)));
			((Control)checkBoxX).Font = cellStyle.Font;
			((Control)checkBoxX).ForeColor = cellStyle.ForeColor;
			((Control)checkBoxX).BackColor = (((DataGridViewCell)this).Selected ? Color.Transparent : cellStyle.BackColor);
			if (rowIndex != dataGridViewCheckBoxXColumn.ActiveRowIndex)
			{
				checkBoxX.CheckBoxItem.IsMouseOver = false;
				checkBoxX.CheckBoxItem.IsMouseDown = false;
			}
			if (rowIndex < ((DataGridViewElement)this).DataGridView.RowCount)
			{
				((Control)checkBoxX).Text = text;
				if (PartsSet(paintParts, (DataGridViewPaintParts)8))
				{
					dataGridViewCheckBoxXColumn.OnBeforeCellPaint(rowIndex, ((DataGridViewCell)this).ColumnIndex);
				}
			}
			else
			{
				((Control)checkBoxX).Text = "";
			}
			_Text = ((Control)checkBoxX).Text;
			Rectangle adjustedEditingControlBounds = GetAdjustedEditingControlBounds(cellBounds, cellStyle);
			checkBoxX.CallBasePaintBackground = false;
			g.TranslateTransform((float)adjustedEditingControlBounds.X, (float)adjustedEditingControlBounds.Y);
			((Control)checkBoxX).Bounds = adjustedEditingControlBounds;
			checkBoxX.RecalcLayout();
			checkBoxX.InternalPaint(new PaintEventArgs(g, Rectangle.Empty));
		}
		finally
		{
			((Control)checkBoxX).Text = text;
			if (checkBoxX.ThreeState)
			{
				checkBoxX.CheckValue = checkValue;
			}
			checkBoxX.Checked = @checked;
			checkBoxX.CheckState = checkState;
			checkBoxX.CheckBoxItem.IsMouseOver = isMouseOver;
			checkBoxX.CheckBoxItem.IsMouseDown = isMouseDown;
			g.Restore(val);
			dataGridViewCheckBoxXColumn.InCellCallBack = false;
		}
	}

	private bool GetCheckValue(DataGridViewCheckBoxXColumn oc, object value)
	{
		if (value == null || value == DBNull.Value)
		{
			return false;
		}
		Type type = value.GetType();
		if (type == typeof(bool))
		{
			return (bool)value;
		}
		if (type == typeof(string))
		{
			string text = (string)value;
			if (oc.CheckValueChecked is string && text.Equals(oc.CheckValueChecked))
			{
				return true;
			}
			if (oc.CheckValueUnchecked is string)
			{
				text.Equals(oc.CheckValueUnchecked);
				return false;
			}
		}
		else
		{
			int num = Convert.ToInt32(value);
			if (num == 1)
			{
				return true;
			}
		}
		return false;
	}

	protected override void OnMouseEnter(int rowIndex)
	{
		((DataGridViewCell)this).OnMouseEnter(rowIndex);
		if (((DataGridViewCell)this).OwningColumn is DataGridViewCheckBoxXColumn dataGridViewCheckBoxXColumn)
		{
			_MouseEntered = false;
			dataGridViewCheckBoxXColumn.ActiveRowIndex = rowIndex;
			((DataGridViewCell)this).ToolTipText = _Text;
			RefreshCell(((DataGridViewCell)this).ColumnIndex, rowIndex);
		}
	}

	protected override void OnMouseLeave(int rowIndex)
	{
		((DataGridViewCell)this).OnMouseLeave(rowIndex);
		if (((DataGridViewCell)this).OwningColumn is DataGridViewCheckBoxXColumn dataGridViewCheckBoxXColumn)
		{
			dataGridViewCheckBoxXColumn.ActiveRowIndex = -1;
			if (_MouseEntered)
			{
				_MouseEntered = false;
				dataGridViewCheckBoxXColumn.CheckBoxX.CheckBoxItem.InternalMouseLeave();
			}
			RefreshCell(((DataGridViewCell)this).ColumnIndex, rowIndex);
		}
	}

	protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
	{
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		((DataGridViewCell)this).OnMouseMove(e);
		if (!(((DataGridViewCell)this).OwningColumn is DataGridViewCheckBoxXColumn dataGridViewCheckBoxXColumn) || ((DataGridViewCell)this).ReadOnly || ((DataGridViewBand)dataGridViewCheckBoxXColumn).ReadOnly || ((DataGridViewElement)this).DataGridView.ReadOnly)
		{
			return;
		}
		Rectangle cellDisplayRectangle = ((DataGridViewElement)this).DataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
		cellDisplayRectangle.X = 0;
		cellDisplayRectangle.Y = 0;
		DataGridViewCellStyle defaultCellStyle = ((DataGridViewBand)((DataGridViewElement)this).DataGridView.Columns[e.ColumnIndex]).DefaultCellStyle;
		Rectangle adjustedEditingControlBounds = GetAdjustedEditingControlBounds(cellDisplayRectangle, defaultCellStyle);
		Point pt = new Point(((MouseEventArgs)e).X - adjustedEditingControlBounds.X, ((MouseEventArgs)e).Y - adjustedEditingControlBounds.Y);
		if (dataGridViewCheckBoxXColumn.CheckBoxX.CheckBoxItem.DisplayRectangle.Contains(pt))
		{
			if (!_MouseEntered)
			{
				_MouseEntered = true;
				dataGridViewCheckBoxXColumn.CheckBoxX.CheckBoxItem.InternalMouseEnter();
			}
		}
		else if (_MouseEntered)
		{
			_MouseEntered = false;
			dataGridViewCheckBoxXColumn.CheckBoxX.CheckBoxItem.InternalMouseLeave();
		}
		dataGridViewCheckBoxXColumn.CheckBoxX.CheckBoxItem.InternalMouseMove(new MouseEventArgs(((MouseEventArgs)e).Button, ((MouseEventArgs)e).Clicks, pt.X, pt.Y, ((MouseEventArgs)e).Delta));
		RefreshCell(e.ColumnIndex, e.RowIndex);
	}

	protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
	{
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Expected O, but got Unknown
		((DataGridViewCell)this).OnMouseDown(e);
		if (_MouseEntered && ((DataGridViewCell)this).OwningColumn is DataGridViewCheckBoxXColumn dataGridViewCheckBoxXColumn && !((DataGridViewBand)dataGridViewCheckBoxXColumn).ReadOnly)
		{
			dataGridViewCheckBoxXColumn.DownRowIndex = e.RowIndex;
			Rectangle cellDisplayRectangle = ((DataGridViewElement)this).DataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
			cellDisplayRectangle.X = 0;
			cellDisplayRectangle.Y = 0;
			DataGridViewCellStyle defaultCellStyle = ((DataGridViewBand)((DataGridViewElement)this).DataGridView.Columns[e.ColumnIndex]).DefaultCellStyle;
			Rectangle adjustedEditingControlBounds = GetAdjustedEditingControlBounds(cellDisplayRectangle, defaultCellStyle);
			Point pt = new Point(((MouseEventArgs)e).X - adjustedEditingControlBounds.X, ((MouseEventArgs)e).Y - adjustedEditingControlBounds.Y);
			if (dataGridViewCheckBoxXColumn.CheckBoxX.CheckBoxItem.DisplayRectangle.Contains(pt))
			{
				dataGridViewCheckBoxXColumn.CheckBoxX.CheckValue = ((DataGridViewCell)this).GetValue(e.RowIndex);
				dataGridViewCheckBoxXColumn.CheckBoxX.CheckBoxItem.InternalMouseDown(new MouseEventArgs(((MouseEventArgs)e).Button, ((MouseEventArgs)e).Clicks, pt.X, pt.Y, ((MouseEventArgs)e).Delta));
				RefreshCell(e.ColumnIndex, e.RowIndex);
			}
		}
	}

	protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)
	{
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		((DataGridViewCell)this).OnMouseUp(e);
		if (!(((DataGridViewCell)this).OwningColumn is DataGridViewCheckBoxXColumn dataGridViewCheckBoxXColumn) || ((DataGridViewBand)dataGridViewCheckBoxXColumn).ReadOnly)
		{
			return;
		}
		Rectangle cellDisplayRectangle = ((DataGridViewElement)this).DataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
		cellDisplayRectangle.X = 0;
		cellDisplayRectangle.Y = 0;
		DataGridViewCellStyle defaultCellStyle = ((DataGridViewBand)((DataGridViewElement)this).DataGridView.Columns[e.ColumnIndex]).DefaultCellStyle;
		Rectangle adjustedEditingControlBounds = GetAdjustedEditingControlBounds(cellDisplayRectangle, defaultCellStyle);
		Point point = new Point(((MouseEventArgs)e).X - adjustedEditingControlBounds.X, ((MouseEventArgs)e).Y - adjustedEditingControlBounds.Y);
		dataGridViewCheckBoxXColumn.CheckBoxX.CheckBoxItem.InternalMouseUp(new MouseEventArgs(((MouseEventArgs)e).Button, ((MouseEventArgs)e).Clicks, point.X, point.Y, ((MouseEventArgs)e).Delta));
		if (e.RowIndex == dataGridViewCheckBoxXColumn.DownRowIndex)
		{
			if (adjustedEditingControlBounds.Contains(((MouseEventArgs)e).Location))
			{
				((DataGridViewCell)this).Value = dataGridViewCheckBoxXColumn.CheckBoxX.CheckValue;
			}
			RefreshCell(e.ColumnIndex, e.RowIndex);
		}
		dataGridViewCheckBoxXColumn.DownRowIndex = -1;
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
		DataGridViewCheckBoxXColumn dataGridViewCheckBoxXColumn = (DataGridViewCheckBoxXColumn)(object)((DataGridViewCell)this).OwningColumn;
		Rectangle rectangle = ((DataGridViewCell)this).BorderWidths(advancedBorderStyle);
		cellBounds.Offset(rectangle.X, rectangle.Y);
		cellBounds.Width -= rectangle.Right;
		cellBounds.Height -= rectangle.Bottom;
		if (((DataGridViewCell)this).Selected)
		{
			cellBounds.Width += ((DataGridViewColumn)dataGridViewCheckBoxXColumn).DividerWidth;
		}
		return cellBounds;
	}

	private Rectangle GetContentBounds(Rectangle cellBounds)
	{
		if (((DataGridViewCell)this).OwningColumn is DataGridViewCheckBoxXColumn dataGridViewCheckBoxXColumn)
		{
			if (((Control)dataGridViewCheckBoxXColumn.CheckBoxX).Parent == null)
			{
				Form val = ((Control)((DataGridViewElement)dataGridViewCheckBoxXColumn).DataGridView).FindForm();
				if (val != null)
				{
					((Control)dataGridViewCheckBoxXColumn.CheckBoxX).Parent = (Control)(object)val;
				}
			}
			dataGridViewCheckBoxXColumn.CheckBoxX.CheckBoxItem.RecalcSize();
			cellBounds.Width = Math.Min(dataGridViewCheckBoxXColumn.CheckBoxX.CheckBoxItem.WidthInternal, cellBounds.Width);
			cellBounds.Width -= ((DataGridViewColumn)dataGridViewCheckBoxXColumn).DividerWidth + 3;
			cellBounds.Height = dataGridViewCheckBoxXColumn.CheckBoxX.CheckBoxItem.HeightInternal;
		}
		return cellBounds;
	}

	private void RefreshCell(int columnIndex, int rowIndex)
	{
		((DataGridViewElement)this).DataGridView.InvalidateCell(columnIndex, rowIndex);
	}
}
