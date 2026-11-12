using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Media;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.Editors;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

public class DataGridViewIntegerInputCell : DataGridViewTextBoxCell
{
	public override Type EditType => typeof(DataGridViewIntegerInputEditingControl);

	public override Type FormattedValueType => typeof(string);

	public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		((DataGridViewCell)this).DetachEditingControl();
		((DataGridViewTextBoxCell)this).InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
		DataGridViewIntegerInputEditingControl dataGridViewIntegerInputEditingControl = (DataGridViewIntegerInputEditingControl)(object)((DataGridViewElement)this).DataGridView.EditingControl;
		if (((DataGridViewCell)this).OwningColumn is DataGridViewIntegerInputColumn { IntegerInput: var integerInput })
		{
			dataGridViewIntegerInputEditingControl.Value = GetValue(initialFormattedValue);
			dataGridViewIntegerInputEditingControl.DisplayFormat = integerInput.DisplayFormat;
			((Control)dataGridViewIntegerInputEditingControl).Enabled = ((Control)integerInput).Enabled;
			dataGridViewIntegerInputEditingControl.Increment = integerInput.Increment;
			dataGridViewIntegerInputEditingControl.InputHorizontalAlignment = GetHorizontalAlignment(dataGridViewCellStyle.Alignment);
			dataGridViewIntegerInputEditingControl.LockUpdateChecked = integerInput.LockUpdateChecked;
			dataGridViewIntegerInputEditingControl.MaxValue = integerInput.MaxValue;
			dataGridViewIntegerInputEditingControl.MinValue = integerInput.MinValue;
			dataGridViewIntegerInputEditingControl.ShowCheckBox = integerInput.ShowCheckBox;
			dataGridViewIntegerInputEditingControl.ShowUpDown = integerInput.ShowUpDown;
			dataGridViewIntegerInputEditingControl.BackgroundStyle.ApplyStyle(integerInput.BackgroundStyle);
			dataGridViewIntegerInputEditingControl.BackgroundStyle.Class = integerInput.BackgroundStyle.Class;
			integerInput.ButtonClear.CopyToItem(dataGridViewIntegerInputEditingControl.ButtonClear);
			integerInput.ButtonCustom.CopyToItem(dataGridViewIntegerInputEditingControl.ButtonCustom);
			integerInput.ButtonCustom2.CopyToItem(dataGridViewIntegerInputEditingControl.ButtonCustom2);
			integerInput.ButtonDropDown.CopyToItem(dataGridViewIntegerInputEditingControl.ButtonDropDown);
			integerInput.ButtonFreeText.CopyToItem(dataGridViewIntegerInputEditingControl.ButtonFreeText);
			dataGridViewIntegerInputEditingControl.ButtonClearClick += ButtonClearClick;
			dataGridViewIntegerInputEditingControl.ButtonCustomClick += ButtonCustomClick;
			dataGridViewIntegerInputEditingControl.ButtonCustom2Click += ButtonCustom2Click;
			dataGridViewIntegerInputEditingControl.ButtonDropDownClick += ButtonDropDownClick;
			dataGridViewIntegerInputEditingControl.ButtonFreeTextClick += ButtonFreeTextClick;
			dataGridViewIntegerInputEditingControl.ConvertFreeTextEntry += ConvertFreeTextEntry;
			dataGridViewIntegerInputEditingControl.EditCancelled = false;
		}
	}

	public override void DetachEditingControl()
	{
		if (((DataGridViewElement)this).DataGridView != null && ((DataGridViewElement)this).DataGridView.EditingControl != null && ((DataGridViewElement)this).DataGridView.EditingControl is DataGridViewIntegerInputEditingControl dataGridViewIntegerInputEditingControl)
		{
			dataGridViewIntegerInputEditingControl.ButtonClearClick -= ButtonClearClick;
			dataGridViewIntegerInputEditingControl.ButtonDropDownClick -= ButtonDropDownClick;
			dataGridViewIntegerInputEditingControl.ButtonFreeTextClick -= ButtonFreeTextClick;
			dataGridViewIntegerInputEditingControl.ButtonCustomClick -= ButtonCustomClick;
			dataGridViewIntegerInputEditingControl.ButtonCustom2Click -= ButtonCustom2Click;
			dataGridViewIntegerInputEditingControl.ConvertFreeTextEntry -= ConvertFreeTextEntry;
			if (dataGridViewIntegerInputEditingControl.EditCancelled)
			{
				dataGridViewIntegerInputEditingControl.EditCancelled = false;
				SystemSounds.Beep.Play();
			}
		}
		((DataGridViewTextBoxCell)this).DetachEditingControl();
	}

	private void ButtonClearClick(object sender, CancelEventArgs e)
	{
		((DataGridViewIntegerInputColumn)(object)((DataGridViewCell)this).OwningColumn).DoButtonClearClick(sender, e);
	}

	private void ButtonCustomClick(object sender, EventArgs e)
	{
		((DataGridViewIntegerInputColumn)(object)((DataGridViewCell)this).OwningColumn).DoButtonCustomClick(sender, e);
	}

	private void ButtonCustom2Click(object sender, EventArgs e)
	{
		((DataGridViewIntegerInputColumn)(object)((DataGridViewCell)this).OwningColumn).DoButtonCustom2Click(sender, e);
	}

	private void ButtonDropDownClick(object sender, CancelEventArgs e)
	{
		((DataGridViewIntegerInputColumn)(object)((DataGridViewCell)this).OwningColumn).DoButtonDropDownClick(sender, e);
	}

	private void ButtonFreeTextClick(object sender, CancelEventArgs e)
	{
		((DataGridViewIntegerInputColumn)(object)((DataGridViewCell)this).OwningColumn).DoButtonFreeTextClick(sender, e);
	}

	private void ConvertFreeTextEntry(object sender, FreeTextEntryConversionEventArgs e)
	{
		((DataGridViewIntegerInputColumn)(object)((DataGridViewCell)this).OwningColumn).DoConvertFreeTextEntry(sender, e);
	}

	public override void PositionEditingControl(bool setLocation, bool setSize, Rectangle cellBounds, Rectangle cellClip, DataGridViewCellStyle cellStyle, bool singleVerticalBorderAdded, bool singleHorizontalBorderAdded, bool isFirstDisplayedColumn, bool isFirstDisplayedRow)
	{
		Rectangle editingControlBounds = ((DataGridViewCell)this).PositionEditingPanel(cellBounds, cellClip, cellStyle, singleVerticalBorderAdded, singleHorizontalBorderAdded, isFirstDisplayedColumn, isFirstDisplayedRow);
		editingControlBounds = GetAdjustedEditingControlBounds(editingControlBounds, cellStyle);
		((DataGridViewElement)this).DataGridView.EditingControl.Location = new Point(editingControlBounds.X, editingControlBounds.Y);
		((DataGridViewElement)this).DataGridView.EditingControl.Size = new Size(editingControlBounds.Width, editingControlBounds.Height);
	}

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
		Rectangle cellBounds = GetCellBounds(editingControlBounds);
		if (cellBounds.Height < editingControlBounds.Height)
		{
			DataGridViewContentAlignment alignment = cellStyle.Alignment;
			if ((int)alignment <= 64)
			{
				if ((int)alignment == 16 || (int)alignment == 32 || (int)alignment == 64)
				{
					editingControlBounds.Y += (editingControlBounds.Height - cellBounds.Height) / 2;
				}
			}
			else if ((int)alignment == 256 || (int)alignment == 512 || (int)alignment == 1024)
			{
				editingControlBounds.Y += editingControlBounds.Height - cellBounds.Height;
			}
		}
		editingControlBounds.Width = Math.Max(1, editingControlBounds.Width);
		editingControlBounds.Height = Math.Max(1, cellBounds.Height);
		return editingControlBounds;
	}

	protected override Size GetPreferredSize(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
	{
		if (((DataGridViewElement)this).DataGridView == null)
		{
			return new Size(-1, -1);
		}
		Size preferredSize = ((DataGridViewTextBoxCell)this).GetPreferredSize(graphics, cellStyle, rowIndex, constraintSize);
		if (((DataGridViewCell)this).OwningColumn is DataGridViewIntegerInputColumn { IntegerInput: var integerInput })
		{
			preferredSize.Height = ((Control)integerInput).Height;
			if (constraintSize.Width == 0)
			{
				preferredSize.Width += GetImageWidth(integerInput.ButtonClear);
				preferredSize.Width += GetImageWidth(integerInput.ButtonCustom);
				preferredSize.Width += GetImageWidth(integerInput.ButtonCustom2);
				preferredSize.Width += GetImageWidth(integerInput.ButtonDropDown);
				preferredSize.Width += GetImageWidth(integerInput.ButtonFreeText);
				if (integerInput.ShowUpDown)
				{
					preferredSize.Width += 16;
				}
				if (integerInput.ShowCheckBox)
				{
					preferredSize.Width += 16;
				}
			}
		}
		return preferredSize;
	}

	protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		if (((DataGridViewCell)this).OwningColumn is DataGridViewIntegerInputColumn { IntegerInput: var integerInput })
		{
			integerInput.Value = GetValue(value);
			return ((Control)integerInput).Text;
		}
		return ((DataGridViewCell)this).GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
	}

	private int GetImageWidth(InputButtonSettings ibs)
	{
		if (ibs.Visible)
		{
			if (ibs.Image == null)
			{
				return 16;
			}
			return ibs.Image.Width;
		}
		return 0;
	}

	protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
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
		DataGridViewIntegerInputColumn dataGridViewIntegerInputColumn = (DataGridViewIntegerInputColumn)(object)((DataGridViewCell)this).OwningColumn;
		Bitmap cellBitmap = dataGridViewIntegerInputColumn.GetCellBitmap(cellBounds);
		if (cellBitmap != null)
		{
			Graphics val = Graphics.FromImage((Image)(object)cellBitmap);
			try
			{
				PaintCellBackground(val, cellStyle, backBounds);
				PaintCellContent(val, cellBounds, rowIndex, formattedValue, cellStyle, paintParts);
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

	private void PaintCellBackground(Graphics g, DataGridViewCellStyle cellStyle, Rectangle rBack)
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

	private void PaintCellContent(Graphics g, Rectangle cellBounds, int rowIndex, object value, DataGridViewCellStyle cellStyle, DataGridViewPaintParts paintParts)
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		DataGridViewIntegerInputColumn dataGridViewIntegerInputColumn = (DataGridViewIntegerInputColumn)(object)((DataGridViewCell)this).OwningColumn;
		IntegerInput integerInput = dataGridViewIntegerInputColumn.IntegerInput;
		Point currentCellAddress = ((DataGridViewElement)this).DataGridView.CurrentCellAddress;
		if ((currentCellAddress.X != ((DataGridViewCell)this).ColumnIndex || currentCellAddress.Y != rowIndex || ((DataGridViewElement)this).DataGridView.EditingControl == null) && rowIndex < ((DataGridViewElement)this).DataGridView.RowCount && PartsSet(paintParts, (DataGridViewPaintParts)8))
		{
			cellBounds.X = 0;
			cellBounds.Y = 0;
			cellBounds.Width -= ((DataGridViewColumn)dataGridViewIntegerInputColumn).DividerWidth + 1;
			cellBounds.Height--;
			((Control)integerInput).Font = cellStyle.Font;
			((Control)integerInput).ForeColor = cellStyle.ForeColor;
			((Control)integerInput).BackColor = (((DataGridViewCell)this).Selected ? Color.Transparent : cellStyle.BackColor);
			integerInput.BackgroundStyle.TextColor = cellStyle.ForeColor;
			integerInput.BackgroundStyle.BackColor = (((DataGridViewCell)this).Selected ? Color.Transparent : cellStyle.BackColor);
			integerInput.InputHorizontalAlignment = GetHorizontalAlignment(cellStyle.Alignment);
			integerInput.Value = GetValue(value);
			dataGridViewIntegerInputColumn.OnBeforeCellPaint(rowIndex, ((DataGridViewCell)this).ColumnIndex);
			Rectangle adjustedEditingControlBounds = GetAdjustedEditingControlBounds(cellBounds, cellStyle);
			if (!dataGridViewIntegerInputColumn.DisplayControlForCurrentCellOnly)
			{
				DrawControl(integerInput, adjustedEditingControlBounds, g);
			}
			else
			{
				DrawText(integerInput, adjustedEditingControlBounds, g);
			}
		}
	}

	private void DrawControl(IntegerInput di, Rectangle r, Graphics g)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		GraphicsState val = g.Save();
		try
		{
			g.TranslateTransform((float)r.X, (float)r.Y);
			((Control)di).Width = r.Width;
			di.InternalPaint(new PaintEventArgs(g, Rectangle.Empty));
		}
		finally
		{
			g.Restore(val);
		}
	}

	private void DrawText(IntegerInput di, Rectangle r, Graphics g)
	{
		r.Inflate(-2, 0);
		eTextFormat eTextFormat = eTextFormat.VerticalCenter;
		switch (di.InputHorizontalAlignment)
		{
		case eHorizontalAlignment.Center:
			eTextFormat |= eTextFormat.HorizontalCenter;
			break;
		case eHorizontalAlignment.Right:
			eTextFormat |= eTextFormat.Right;
			break;
		}
		TextDrawing.DrawString(g, ((Control)di).Text, ((Control)di).Font, ((Control)di).ForeColor, r, eTextFormat);
	}

	private Rectangle GetBackBounds(Rectangle cellBounds, DataGridViewAdvancedBorderStyle advancedBorderStyle)
	{
		DataGridViewIntegerInputColumn dataGridViewIntegerInputColumn = (DataGridViewIntegerInputColumn)(object)((DataGridViewCell)this).OwningColumn;
		Rectangle rectangle = ((DataGridViewCell)this).BorderWidths(advancedBorderStyle);
		cellBounds.Offset(rectangle.X, rectangle.Y);
		cellBounds.Width -= rectangle.Right;
		cellBounds.Height -= rectangle.Bottom;
		if (((DataGridViewCell)this).Selected)
		{
			cellBounds.Width += ((DataGridViewColumn)dataGridViewIntegerInputColumn).DividerWidth;
		}
		return cellBounds;
	}

	private Rectangle GetCellBounds(Rectangle cellBounds)
	{
		DataGridViewIntegerInputColumn dataGridViewIntegerInputColumn = (DataGridViewIntegerInputColumn)(object)((DataGridViewCell)this).OwningColumn;
		Size size = ((Control)dataGridViewIntegerInputColumn.IntegerInput).Size;
		cellBounds.Location = new Point(1, 1);
		cellBounds.Width = Math.Min(size.Width, cellBounds.Width);
		cellBounds.Width -= ((DataGridViewColumn)dataGridViewIntegerInputColumn).DividerWidth;
		cellBounds.Height = size.Height;
		return cellBounds;
	}

	private int GetValue(object value)
	{
		if (value == Convert.DBNull || (value is string && string.IsNullOrEmpty((string)value)))
		{
			return 0;
		}
		return Convert.ToInt32(value);
	}

	private eHorizontalAlignment GetHorizontalAlignment(DataGridViewContentAlignment alignment)
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
		return eHorizontalAlignment.Right;
		IL_002b:
		return eHorizontalAlignment.Center;
		IL_002f:
		return eHorizontalAlignment.Left;
	}

	private bool PartsSet(DataGridViewPaintParts paintParts, DataGridViewPaintParts parts)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return (DataGridViewPaintParts)(paintParts & parts) == parts;
	}
}
