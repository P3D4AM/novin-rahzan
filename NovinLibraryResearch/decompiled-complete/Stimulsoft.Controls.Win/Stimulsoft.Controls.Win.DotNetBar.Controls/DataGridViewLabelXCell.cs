using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

public class DataGridViewLabelXCell : DataGridViewButtonCell
{
	private enum FreeDimension
	{
		Both,
		Height,
		Width
	}

	private DataGridViewCellStyle _CellStyle;

	public override Type EditType => null;

	public override Type FormattedValueType => typeof(string);

	private Rectangle GetAdjustedEditingControlBounds(Rectangle editingControlBounds, DataGridViewCellStyle cellStyle, int rowIndex)
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
		Rectangle contentBounds = ((DataGridViewCell)this).GetContentBounds(rowIndex);
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
		editingControlBounds.Width = Math.Max(1, contentBounds.Width);
		editingControlBounds.Height = Math.Max(1, contentBounds.Height);
		return editingControlBounds;
	}

	protected override Size GetPreferredSize(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
	{
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Expected O, but got Unknown
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		Size result = Size.Empty;
		if (((DataGridViewElement)this).DataGridView == null)
		{
			return new Size(-1, -1);
		}
		if (((DataGridViewCell)this).OwningColumn is DataGridViewLabelXColumn { LabelX: var labelX } dataGridViewLabelXColumn)
		{
			if (!((Control)dataGridViewLabelXColumn.LabelX).IsHandleCreated)
			{
				if (((Control)dataGridViewLabelXColumn.LabelX).Parent == null)
				{
					Form val = ((Control)((DataGridViewElement)dataGridViewLabelXColumn).DataGridView).FindForm();
					if (val != null)
					{
						((Control)dataGridViewLabelXColumn.LabelX).Parent = (Control)(object)val;
					}
				}
				((Control)dataGridViewLabelXColumn.LabelX).Visible = true;
				((Control)dataGridViewLabelXColumn.LabelX).Visible = false;
			}
			FreeDimension dFromConstraint = GetDFromConstraint(constraintSize);
			if (dFromConstraint != FreeDimension.Width)
			{
				constraintSize.Width -= 2;
			}
			((Control)labelX).Font = cellStyle.Font;
			labelX.WordWrap = GetWordWrap(dataGridViewLabelXColumn, cellStyle);
			labelX.TextAlignment = GetTextAlignment(cellStyle.Alignment);
			((Control)labelX).MaximumSize = Size.Empty;
			labelX.Image = dataGridViewLabelXColumn.Image;
			string text2 = (((Control)labelX).Text = " ");
			string text3 = text2;
			if (rowIndex < ((DataGridViewElement)this).DataGridView.RowCount)
			{
				text3 = GetValue(((DataGridViewElement)this).DataGridView.Rows[rowIndex].Cells[((DataGridViewCell)this).ColumnIndex].Value);
			}
			if (string.IsNullOrEmpty(text3))
			{
				text3 = " ";
			}
			((Control)labelX).Text = text3;
			result = ((Control)labelX).GetPreferredSize(constraintSize);
			Rectangle rectangle = ((DataGridViewCell)this).BorderWidths(new DataGridViewAdvancedBorderStyle());
			int num = rectangle.Left + rectangle.Width;
			Padding padding = cellStyle.Padding;
			int num2 = num + ((Padding)(ref padding)).Horizontal;
			int num3 = rectangle.Top + rectangle.Height;
			padding = cellStyle.Padding;
			int num4 = num3 + ((Padding)(ref padding)).Vertical;
			switch (dFromConstraint)
			{
			case FreeDimension.Height:
				result.Width = 0;
				break;
			case FreeDimension.Width:
				result.Height = 0;
				break;
			}
			if (dFromConstraint != FreeDimension.Height)
			{
				result.Width += num2 + 4;
			}
			if (dFromConstraint != FreeDimension.Width)
			{
				result.Height += num4 + 2;
			}
		}
		return result;
	}

	private FreeDimension GetDFromConstraint(Size constraintSize)
	{
		if (constraintSize.Width == 0)
		{
			if (constraintSize.Height == 0)
			{
				return FreeDimension.Both;
			}
			return FreeDimension.Width;
		}
		return FreeDimension.Height;
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
		DataGridViewLabelXColumn dataGridViewLabelXColumn = (DataGridViewLabelXColumn)(object)((DataGridViewCell)this).OwningColumn;
		Bitmap cellBitmap = dataGridViewLabelXColumn.GetCellBitmap(cellBounds);
		if (cellBitmap != null)
		{
			Graphics val = Graphics.FromImage((Image)(object)cellBitmap);
			try
			{
				PaintLabelBackground(val, cellStyle, backBounds);
				PaintLabelContent(cellBounds, rowIndex, formattedValue, cellStyle, paintParts, val);
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

	private void PaintLabelBackground(Graphics g, DataGridViewCellStyle cellStyle, Rectangle rBack)
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

	private void PaintLabelContent(Rectangle cellBounds, int rowIndex, object value, DataGridViewCellStyle cellStyle, DataGridViewPaintParts paintParts, Graphics g)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		DataGridViewLabelXColumn dataGridViewLabelXColumn = (DataGridViewLabelXColumn)(object)((DataGridViewCell)this).OwningColumn;
		LabelX labelX = dataGridViewLabelXColumn.LabelX;
		_CellStyle = cellStyle;
		string value2 = GetValue(value);
		dataGridViewLabelXColumn.InCallBack = true;
		bool wordWrap = labelX.WordWrap;
		StringAlignment textAlignment = labelX.TextAlignment;
		GraphicsState val = g.Save();
		try
		{
			cellBounds.X = 0;
			cellBounds.Y = 0;
			cellBounds.Width -= ((DataGridViewColumn)dataGridViewLabelXColumn).DividerWidth + 1;
			cellBounds.Height--;
			((Control)labelX).Font = cellStyle.Font;
			((Control)labelX).ForeColor = cellStyle.ForeColor;
			((Control)labelX).BackColor = (((DataGridViewCell)this).Selected ? Color.Transparent : cellStyle.BackColor);
			labelX.WordWrap = GetWordWrap(dataGridViewLabelXColumn, cellStyle);
			labelX.TextAlignment = GetTextAlignment(cellStyle.Alignment);
			Rectangle bounds = cellBounds;
			if (rowIndex < ((DataGridViewElement)this).DataGridView.RowCount)
			{
				((Control)labelX).Text = value2;
				if (PartsSet(paintParts, (DataGridViewPaintParts)8))
				{
					dataGridViewLabelXColumn.OnBeforeCellPaint(rowIndex, ((DataGridViewCell)this).ColumnIndex);
				}
				bounds = GetAdjustedEditingControlBounds(cellBounds, cellStyle, rowIndex);
			}
			else
			{
				((Control)labelX).Text = " ";
			}
			labelX.CallBasePaintBackground = false;
			g.TranslateTransform((float)bounds.X, (float)bounds.Y);
			((Control)labelX).Bounds = bounds;
			labelX.RecalcLayout();
			labelX.InternalPaint(new PaintEventArgs(g, Rectangle.Empty));
		}
		finally
		{
			labelX.WordWrap = wordWrap;
			labelX.TextAlignment = textAlignment;
			dataGridViewLabelXColumn.InCallBack = false;
			g.Restore(val);
		}
	}

	private bool GetWordWrap(DataGridViewLabelXColumn oc, DataGridViewCellStyle cellStyle)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Invalid comparison between Unknown and I4
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Invalid comparison between Unknown and I4
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Invalid comparison between Unknown and I4
		if (oc.WordWrap)
		{
			return true;
		}
		DataGridViewTriState wrapMode = cellStyle.WrapMode;
		if ((int)wrapMode != 1)
		{
			if ((int)wrapMode == 2)
			{
				return false;
			}
			return (int)((DataGridViewElement)this).DataGridView.DefaultCellStyle.WrapMode == 1;
		}
		return true;
	}

	protected override void OnMouseEnter(int rowIndex)
	{
		((DataGridViewCell)this).OnMouseEnter(rowIndex);
		DataGridViewLabelXColumn dataGridViewLabelXColumn = (DataGridViewLabelXColumn)(object)((DataGridViewCell)this).OwningColumn;
		LabelX labelX = dataGridViewLabelXColumn.LabelX;
		labelX.LabelItem.InternalMouseEnter();
	}

	protected override void OnMouseLeave(int rowIndex)
	{
		((DataGridViewButtonCell)this).OnMouseLeave(rowIndex);
		DataGridViewLabelXColumn dataGridViewLabelXColumn = (DataGridViewLabelXColumn)(object)((DataGridViewCell)this).OwningColumn;
		LabelX labelX = dataGridViewLabelXColumn.LabelX;
		labelX.LabelItem.InternalMouseLeave();
	}

	protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		((DataGridViewButtonCell)this).OnMouseMove(e);
		if (((DataGridViewCell)this).OwningColumn is DataGridViewLabelXColumn dataGridViewLabelXColumn && (uint)e.RowIndex < ((DataGridViewElement)this).DataGridView.Rows.Count)
		{
			Point point = CellAlignPoint(e);
			dataGridViewLabelXColumn.LabelX.LabelItem.InternalMouseMove(new MouseEventArgs(((MouseEventArgs)e).Button, ((MouseEventArgs)e).Clicks, point.X, point.Y, ((MouseEventArgs)e).Delta));
		}
	}

	protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		((DataGridViewButtonCell)this).OnMouseDown(e);
		if (((DataGridViewCell)this).OwningColumn is DataGridViewLabelXColumn dataGridViewLabelXColumn)
		{
			Point point = CellAlignPoint(e);
			dataGridViewLabelXColumn.LabelX.LabelItem.InternalMouseDown(new MouseEventArgs(((MouseEventArgs)e).Button, ((MouseEventArgs)e).Clicks, point.X, point.Y, ((MouseEventArgs)e).Delta));
			RefreshCell(e.ColumnIndex, e.RowIndex);
		}
	}

	protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		((DataGridViewButtonCell)this).OnMouseUp(e);
		if (((DataGridViewCell)this).OwningColumn is DataGridViewLabelXColumn dataGridViewLabelXColumn)
		{
			Point point = CellAlignPoint(e);
			dataGridViewLabelXColumn.LabelX.LabelItem.InternalMouseUp(new MouseEventArgs(((MouseEventArgs)e).Button, ((MouseEventArgs)e).Clicks, point.X, point.Y, ((MouseEventArgs)e).Delta));
			RefreshCell(e.ColumnIndex, e.RowIndex);
		}
	}

	private Point CellAlignPoint(DataGridViewCellMouseEventArgs e)
	{
		Rectangle cellDisplayRectangle = ((DataGridViewElement)this).DataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
		cellDisplayRectangle.Location = new Point(0, 0);
		Rectangle adjustedEditingControlBounds = GetAdjustedEditingControlBounds(cellDisplayRectangle, _CellStyle, e.RowIndex);
		return new Point(((MouseEventArgs)e).X - adjustedEditingControlBounds.X, ((MouseEventArgs)e).Y - adjustedEditingControlBounds.Y);
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
		DataGridViewLabelXColumn dataGridViewLabelXColumn = (DataGridViewLabelXColumn)(object)((DataGridViewCell)this).OwningColumn;
		Rectangle rectangle = ((DataGridViewCell)this).BorderWidths(advancedBorderStyle);
		cellBounds.Offset(rectangle.X, rectangle.Y);
		cellBounds.Width -= rectangle.Right;
		cellBounds.Height -= rectangle.Bottom;
		if (((DataGridViewCell)this).Selected)
		{
			cellBounds.Width += ((DataGridViewColumn)dataGridViewLabelXColumn).DividerWidth;
		}
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

	private StringAlignment GetTextAlignment(DataGridViewContentAlignment alignment)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Invalid comparison between Unknown and I4
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Invalid comparison between Unknown and I4
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Invalid comparison between Unknown and I4
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Invalid comparison between Unknown and I4
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		if ((int)alignment <= 32)
		{
			if ((int)alignment != 2)
			{
				if ((int)alignment == 4)
				{
					goto IL_002d;
				}
				if ((int)alignment != 32)
				{
					goto IL_002f;
				}
			}
			goto IL_002b;
		}
		if ((int)alignment != 64)
		{
			if ((int)alignment == 512)
			{
				goto IL_002b;
			}
			if ((int)alignment != 1024)
			{
				goto IL_002f;
			}
		}
		goto IL_002d;
		IL_002d:
		return (StringAlignment)2;
		IL_002b:
		return (StringAlignment)1;
		IL_002f:
		return (StringAlignment)0;
	}

	internal void RefreshCell(int columnIndex, int rowIndex)
	{
		((DataGridViewElement)this).DataGridView.InvalidateCell(columnIndex, rowIndex);
	}
}
