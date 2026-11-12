using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Media;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.Editors;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

public class DataGridViewDoubleInputCell : DataGridViewTextBoxCell
{
	public override Type EditType => typeof(DataGridViewDoubleInputEditingControl);

	public override Type FormattedValueType => typeof(string);

	public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
	{
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		((DataGridViewCell)this).DetachEditingControl();
		((DataGridViewTextBoxCell)this).InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
		DataGridViewDoubleInputEditingControl dataGridViewDoubleInputEditingControl = (DataGridViewDoubleInputEditingControl)(object)((DataGridViewElement)this).DataGridView.EditingControl;
		if (((DataGridViewCell)this).OwningColumn is DataGridViewDoubleInputColumn { DoubleInput: var doubleInput })
		{
			dataGridViewDoubleInputEditingControl.Value = GetValue(initialFormattedValue);
			dataGridViewDoubleInputEditingControl.DisplayFormat = doubleInput.DisplayFormat;
			((Control)dataGridViewDoubleInputEditingControl).Enabled = ((Control)doubleInput).Enabled;
			dataGridViewDoubleInputEditingControl.Increment = doubleInput.Increment;
			dataGridViewDoubleInputEditingControl.LockUpdateChecked = doubleInput.LockUpdateChecked;
			dataGridViewDoubleInputEditingControl.MaxValue = doubleInput.MaxValue;
			dataGridViewDoubleInputEditingControl.MinValue = doubleInput.MinValue;
			dataGridViewDoubleInputEditingControl.ShowCheckBox = doubleInput.ShowCheckBox;
			dataGridViewDoubleInputEditingControl.ShowUpDown = doubleInput.ShowUpDown;
			dataGridViewDoubleInputEditingControl.BackgroundStyle.ApplyStyle(doubleInput.BackgroundStyle);
			dataGridViewDoubleInputEditingControl.BackgroundStyle.Class = doubleInput.BackgroundStyle.Class;
			dataGridViewDoubleInputEditingControl.InputHorizontalAlignment = GetHorizontalAlignment(dataGridViewCellStyle.Alignment);
			doubleInput.ButtonClear.CopyToItem(dataGridViewDoubleInputEditingControl.ButtonClear);
			doubleInput.ButtonCustom.CopyToItem(dataGridViewDoubleInputEditingControl.ButtonCustom);
			doubleInput.ButtonCustom2.CopyToItem(dataGridViewDoubleInputEditingControl.ButtonCustom2);
			doubleInput.ButtonDropDown.CopyToItem(dataGridViewDoubleInputEditingControl.ButtonDropDown);
			doubleInput.ButtonFreeText.CopyToItem(dataGridViewDoubleInputEditingControl.ButtonFreeText);
			dataGridViewDoubleInputEditingControl.ButtonClearClick += ButtonClearClick;
			dataGridViewDoubleInputEditingControl.ButtonCustomClick += ButtonCustomClick;
			dataGridViewDoubleInputEditingControl.ButtonCustom2Click += ButtonCustom2Click;
			dataGridViewDoubleInputEditingControl.ButtonDropDownClick += ButtonDropDownClick;
			dataGridViewDoubleInputEditingControl.ButtonFreeTextClick += ButtonFreeTextClick;
			dataGridViewDoubleInputEditingControl.ConvertFreeTextEntry += ConvertFreeTextEntry;
			dataGridViewDoubleInputEditingControl.EditCancelled = false;
		}
	}

	public override void DetachEditingControl()
	{
		if (((DataGridViewElement)this).DataGridView != null && ((DataGridViewElement)this).DataGridView.EditingControl != null && ((DataGridViewElement)this).DataGridView.EditingControl is DataGridViewDoubleInputEditingControl dataGridViewDoubleInputEditingControl)
		{
			dataGridViewDoubleInputEditingControl.ButtonClearClick -= ButtonClearClick;
			dataGridViewDoubleInputEditingControl.ButtonDropDownClick -= ButtonDropDownClick;
			dataGridViewDoubleInputEditingControl.ButtonFreeTextClick -= ButtonFreeTextClick;
			dataGridViewDoubleInputEditingControl.ButtonCustomClick -= ButtonCustomClick;
			dataGridViewDoubleInputEditingControl.ButtonCustom2Click -= ButtonCustom2Click;
			dataGridViewDoubleInputEditingControl.ConvertFreeTextEntry -= ConvertFreeTextEntry;
			if (dataGridViewDoubleInputEditingControl.EditCancelled)
			{
				dataGridViewDoubleInputEditingControl.EditCancelled = false;
				SystemSounds.Beep.Play();
			}
		}
		((DataGridViewTextBoxCell)this).DetachEditingControl();
	}

	private void ButtonClearClick(object sender, CancelEventArgs e)
	{
		((DataGridViewDoubleInputColumn)(object)((DataGridViewCell)this).OwningColumn).DoButtonClearClick(sender, e);
	}

	private void ButtonCustomClick(object sender, EventArgs e)
	{
		((DataGridViewDoubleInputColumn)(object)((DataGridViewCell)this).OwningColumn).DoButtonCustomClick(sender, e);
	}

	private void ButtonCustom2Click(object sender, EventArgs e)
	{
		((DataGridViewDoubleInputColumn)(object)((DataGridViewCell)this).OwningColumn).DoButtonCustom2Click(sender, e);
	}

	private void ButtonDropDownClick(object sender, CancelEventArgs e)
	{
		((DataGridViewDoubleInputColumn)(object)((DataGridViewCell)this).OwningColumn).DoButtonDropDownClick(sender, e);
	}

	private void ButtonFreeTextClick(object sender, CancelEventArgs e)
	{
		((DataGridViewDoubleInputColumn)(object)((DataGridViewCell)this).OwningColumn).DoButtonFreeTextClick(sender, e);
	}

	private void ConvertFreeTextEntry(object sender, FreeTextEntryConversionEventArgs e)
	{
		((DataGridViewDoubleInputColumn)(object)((DataGridViewCell)this).OwningColumn).DoConvertFreeTextEntry(sender, e);
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
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Invalid comparison between Unknown and I4
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Invalid comparison between Unknown and I4
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Invalid comparison between Unknown and I4
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Invalid comparison between Unknown and I4
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Invalid comparison between Unknown and I4
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Invalid comparison between Unknown and I4
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Invalid comparison between Unknown and I4
		editingControlBounds.X++;
		editingControlBounds.Y++;
		editingControlBounds.Width = Math.Max(0, editingControlBounds.Width - 2);
		editingControlBounds.Height = Math.Max(0, editingControlBounds.Height - 2);
		Rectangle cellBounds = GetCellBounds(editingControlBounds);
		if (cellStyle != null && cellBounds.Height < editingControlBounds.Height)
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
		if (((DataGridViewCell)this).OwningColumn is DataGridViewDoubleInputColumn { DoubleInput: var doubleInput })
		{
			preferredSize.Height = ((Control)doubleInput).Height;
			if (constraintSize.Width == 0)
			{
				preferredSize.Width += GetImageWidth(doubleInput.ButtonClear);
				preferredSize.Width += GetImageWidth(doubleInput.ButtonCustom);
				preferredSize.Width += GetImageWidth(doubleInput.ButtonCustom2);
				preferredSize.Width += GetImageWidth(doubleInput.ButtonDropDown);
				preferredSize.Width += GetImageWidth(doubleInput.ButtonFreeText);
				if (doubleInput.ShowUpDown)
				{
					preferredSize.Width += 16;
				}
				if (doubleInput.ShowCheckBox)
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
		if (((DataGridViewCell)this).OwningColumn is DataGridViewDoubleInputColumn { DoubleInput: var doubleInput })
		{
			doubleInput.Value = GetValue(value);
			return ((Control)doubleInput).Text;
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
		DataGridViewDoubleInputColumn dataGridViewDoubleInputColumn = (DataGridViewDoubleInputColumn)(object)((DataGridViewCell)this).OwningColumn;
		Bitmap cellBitmap = dataGridViewDoubleInputColumn.GetCellBitmap(cellBounds);
		if (cellBitmap != null)
		{
			Graphics val = Graphics.FromImage((Image)(object)cellBitmap);
			try
			{
				PaintCellBackground(val, cellStyle, backBounds);
				PaintCellContent(val, cellBounds, rowIndex, value, cellStyle, paintParts);
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
		DataGridViewDoubleInputColumn dataGridViewDoubleInputColumn = (DataGridViewDoubleInputColumn)(object)((DataGridViewCell)this).OwningColumn;
		DoubleInput doubleInput = dataGridViewDoubleInputColumn.DoubleInput;
		Point currentCellAddress = ((DataGridViewElement)this).DataGridView.CurrentCellAddress;
		if ((currentCellAddress.X != ((DataGridViewCell)this).ColumnIndex || currentCellAddress.Y != rowIndex || ((DataGridViewElement)this).DataGridView.EditingControl == null) && rowIndex < ((DataGridViewElement)this).DataGridView.RowCount && PartsSet(paintParts, (DataGridViewPaintParts)8))
		{
			cellBounds.X = 0;
			cellBounds.Y = 0;
			cellBounds.Width -= ((DataGridViewColumn)dataGridViewDoubleInputColumn).DividerWidth + 1;
			cellBounds.Height--;
			((Control)doubleInput).Font = cellStyle.Font;
			((Control)doubleInput).ForeColor = cellStyle.ForeColor;
			((Control)doubleInput).BackColor = (((DataGridViewCell)this).Selected ? Color.Transparent : cellStyle.BackColor);
			doubleInput.BackgroundStyle.TextColor = cellStyle.ForeColor;
			doubleInput.BackgroundStyle.BackColor = (((DataGridViewCell)this).Selected ? Color.Transparent : cellStyle.BackColor);
			doubleInput.InputHorizontalAlignment = GetHorizontalAlignment(cellStyle.Alignment);
			doubleInput.Value = GetValue(value);
			dataGridViewDoubleInputColumn.OnBeforeCellPaint(rowIndex, ((DataGridViewCell)this).ColumnIndex);
			Rectangle adjustedEditingControlBounds = GetAdjustedEditingControlBounds(cellBounds, cellStyle);
			if (!dataGridViewDoubleInputColumn.DisplayControlForCurrentCellOnly)
			{
				DrawControl(doubleInput, adjustedEditingControlBounds, g);
			}
			else
			{
				DrawText(doubleInput, adjustedEditingControlBounds, g);
			}
		}
	}

	private void DrawControl(DoubleInput di, Rectangle r, Graphics g)
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

	private void DrawText(DoubleInput di, Rectangle r, Graphics g)
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
		DataGridViewDoubleInputColumn dataGridViewDoubleInputColumn = (DataGridViewDoubleInputColumn)(object)((DataGridViewCell)this).OwningColumn;
		Rectangle rectangle = ((DataGridViewCell)this).BorderWidths(advancedBorderStyle);
		cellBounds.Offset(rectangle.X, rectangle.Y);
		cellBounds.Width -= rectangle.Right;
		cellBounds.Height -= rectangle.Bottom;
		if (((DataGridViewCell)this).Selected)
		{
			cellBounds.Width += ((DataGridViewColumn)dataGridViewDoubleInputColumn).DividerWidth;
		}
		return cellBounds;
	}

	private Rectangle GetCellBounds(Rectangle cellBounds)
	{
		DataGridViewDoubleInputColumn dataGridViewDoubleInputColumn = (DataGridViewDoubleInputColumn)(object)((DataGridViewCell)this).OwningColumn;
		Size size = ((Control)dataGridViewDoubleInputColumn.DoubleInput).Size;
		cellBounds.Location = new Point(1, 1);
		cellBounds.Width = Math.Min(size.Width, cellBounds.Width);
		cellBounds.Width -= ((DataGridViewColumn)dataGridViewDoubleInputColumn).DividerWidth;
		cellBounds.Height = size.Height;
		return cellBounds;
	}

	private double GetValue(object value)
	{
		if (value == Convert.DBNull || (value is string && string.IsNullOrEmpty((string)value)))
		{
			return 0.0;
		}
		if (value is string)
		{
			string text = value.ToString();
			return Convert.ToDouble(text.Replace(" ", ""));
		}
		return Convert.ToDouble(value);
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
