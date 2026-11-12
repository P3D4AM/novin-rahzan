using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.Editors;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

public class DataGridViewTextBoxDropDownCell : DataGridViewTextBoxCell
{
	public override Type EditType => typeof(DataGridViewTextBoxDropDownEditingControl);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	private static extern IntPtr PostMessage(IntPtr hWnd, int msg, int wParam, int lParam);

	public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Invalid comparison between Unknown and I4
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Expected O, but got Unknown
		((DataGridViewCell)this).DetachEditingControl();
		((DataGridViewTextBoxCell)this).InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
		DataGridViewTextBoxDropDownEditingControl dataGridViewTextBoxDropDownEditingControl = (DataGridViewTextBoxDropDownEditingControl)(object)((DataGridViewElement)this).DataGridView.EditingControl;
		DataGridViewTextBoxDropDownColumn dataGridViewTextBoxDropDownColumn = (DataGridViewTextBoxDropDownColumn)(object)((DataGridViewCell)this).OwningColumn;
		TextBoxDropDown textBoxDropDown = dataGridViewTextBoxDropDownColumn.TextBoxDropDown;
		dataGridViewTextBoxDropDownEditingControl.AutoCompleteCustomSource = textBoxDropDown.AutoCompleteCustomSource;
		dataGridViewTextBoxDropDownEditingControl.AutoCompleteMode = textBoxDropDown.AutoCompleteMode;
		dataGridViewTextBoxDropDownEditingControl.AutoCompleteSource = textBoxDropDown.AutoCompleteSource;
		((Control)dataGridViewTextBoxDropDownEditingControl).BackColor = ((Control)textBoxDropDown).BackColor;
		dataGridViewTextBoxDropDownEditingControl.CharacterCasing = textBoxDropDown.CharacterCasing;
		dataGridViewTextBoxDropDownEditingControl.DropDownControl = textBoxDropDown.DropDownControl;
		((Control)dataGridViewTextBoxDropDownEditingControl).Enabled = ((Control)textBoxDropDown).Enabled;
		dataGridViewTextBoxDropDownEditingControl.FocusHighlightColor = textBoxDropDown.FocusHighlightColor;
		dataGridViewTextBoxDropDownEditingControl.FocusHighlightEnabled = textBoxDropDown.FocusHighlightEnabled;
		((Control)dataGridViewTextBoxDropDownEditingControl).ForeColor = ((Control)textBoxDropDown).ForeColor;
		dataGridViewTextBoxDropDownEditingControl.HideSelection = textBoxDropDown.HideSelection;
		((Control)dataGridViewTextBoxDropDownEditingControl).ImeMode = ((Control)textBoxDropDown).ImeMode;
		dataGridViewTextBoxDropDownEditingControl.MaxLength = textBoxDropDown.MaxLength;
		dataGridViewTextBoxDropDownEditingControl.PasswordChar = textBoxDropDown.PasswordChar;
		((Control)dataGridViewTextBoxDropDownEditingControl).RightToLeft = ((Control)textBoxDropDown).RightToLeft;
		dataGridViewTextBoxDropDownEditingControl.TextAlign = GetTextAlignment(dataGridViewCellStyle.Alignment);
		dataGridViewTextBoxDropDownEditingControl.UseSystemPasswordChar = textBoxDropDown.UseSystemPasswordChar;
		dataGridViewTextBoxDropDownEditingControl.WatermarkBehavior = textBoxDropDown.WatermarkBehavior;
		dataGridViewTextBoxDropDownEditingControl.WatermarkColor = textBoxDropDown.WatermarkColor;
		dataGridViewTextBoxDropDownEditingControl.WatermarkEnabled = textBoxDropDown.WatermarkEnabled;
		dataGridViewTextBoxDropDownEditingControl.WatermarkFont = textBoxDropDown.WatermarkFont;
		dataGridViewTextBoxDropDownEditingControl.WatermarkText = textBoxDropDown.WatermarkText;
		((TextBoxBase)dataGridViewTextBoxDropDownEditingControl.TextBox).Multiline = (int)dataGridViewCellStyle.WrapMode == 1;
		dataGridViewTextBoxDropDownEditingControl.BackgroundStyle.ApplyStyle(textBoxDropDown.BackgroundStyle);
		dataGridViewTextBoxDropDownEditingControl.BackgroundStyle.Class = textBoxDropDown.BackgroundStyle.Class;
		textBoxDropDown.ButtonClear.CopyToItem(dataGridViewTextBoxDropDownEditingControl.ButtonClear);
		textBoxDropDown.ButtonCustom.CopyToItem(dataGridViewTextBoxDropDownEditingControl.ButtonCustom);
		textBoxDropDown.ButtonCustom2.CopyToItem(dataGridViewTextBoxDropDownEditingControl.ButtonCustom2);
		textBoxDropDown.ButtonDropDown.CopyToItem(dataGridViewTextBoxDropDownEditingControl.ButtonDropDown);
		dataGridViewTextBoxDropDownEditingControl.ButtonClearClick += ButtonClearClick;
		dataGridViewTextBoxDropDownEditingControl.ButtonCustomClick += ButtonCustomClick;
		dataGridViewTextBoxDropDownEditingControl.ButtonCustom2Click += ButtonCustom2Click;
		dataGridViewTextBoxDropDownEditingControl.ButtonDropDownClick += ButtonDropDownClick;
		((Control)dataGridViewTextBoxDropDownEditingControl).KeyDown += new KeyEventHandler(KeyDown);
		((Control)dataGridViewTextBoxDropDownEditingControl).Text = ((initialFormattedValue != null && initialFormattedValue != Convert.DBNull) ? Convert.ToString(initialFormattedValue) : "");
	}

	public override void DetachEditingControl()
	{
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected O, but got Unknown
		if (((DataGridViewElement)this).DataGridView != null && ((DataGridViewElement)this).DataGridView.EditingControl != null && ((DataGridViewElement)this).DataGridView.EditingControl is DataGridViewTextBoxDropDownEditingControl dataGridViewTextBoxDropDownEditingControl)
		{
			dataGridViewTextBoxDropDownEditingControl.ButtonClearClick -= ButtonClearClick;
			dataGridViewTextBoxDropDownEditingControl.ButtonCustomClick -= ButtonCustomClick;
			dataGridViewTextBoxDropDownEditingControl.ButtonCustom2Click -= ButtonCustom2Click;
			dataGridViewTextBoxDropDownEditingControl.ButtonDropDownClick -= ButtonDropDownClick;
			((Control)dataGridViewTextBoxDropDownEditingControl).KeyDown -= new KeyEventHandler(KeyDown);
		}
		((DataGridViewTextBoxCell)this).DetachEditingControl();
	}

	private void ButtonClearClick(object sender, CancelEventArgs e)
	{
		((DataGridViewTextBoxDropDownColumn)(object)((DataGridViewCell)this).OwningColumn).DoButtonClearClick(sender, e);
	}

	private void ButtonCustomClick(object sender, EventArgs e)
	{
		((DataGridViewTextBoxDropDownColumn)(object)((DataGridViewCell)this).OwningColumn).DoButtonCustomClick(sender, e);
	}

	private void ButtonCustom2Click(object sender, EventArgs e)
	{
		((DataGridViewTextBoxDropDownColumn)(object)((DataGridViewCell)this).OwningColumn).DoButtonCustom2Click(sender, e);
	}

	private void ButtonDropDownClick(object sender, CancelEventArgs e)
	{
		((DataGridViewTextBoxDropDownColumn)(object)((DataGridViewCell)this).OwningColumn).DoButtonDropDownClick(sender, e);
	}

	private void KeyDown(object sender, KeyEventArgs e)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected I4, but got Unknown
		if (((DataGridViewElement)this).DataGridView != null && ((DataGridViewElement)this).DataGridView.EditingControl != null && ((DataGridViewElement)this).DataGridView.EditingControl is DataGridViewTextBoxDropDownEditingControl dataGridViewTextBoxDropDownEditingControl)
		{
			PostMessage(((Control)dataGridViewTextBoxDropDownEditingControl.TextBox).Handle, 256, (int)e.KeyCode, 1);
		}
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
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Invalid comparison between Unknown and I4
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Invalid comparison between Unknown and I4
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Invalid comparison between Unknown and I4
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Invalid comparison between Unknown and I4
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Invalid comparison between Unknown and I4
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Invalid comparison between Unknown and I4
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Invalid comparison between Unknown and I4
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Invalid comparison between Unknown and I4
		editingControlBounds.X++;
		editingControlBounds.Y++;
		editingControlBounds.Width = Math.Max(0, editingControlBounds.Width - 2);
		editingControlBounds.Height = Math.Max(0, editingControlBounds.Height - 2);
		Rectangle cellBounds = GetCellBounds(editingControlBounds);
		if ((int)cellStyle.WrapMode != 1)
		{
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
			editingControlBounds.Height = Math.Max(1, cellBounds.Height);
		}
		editingControlBounds.Width = Math.Max(1, editingControlBounds.Width);
		return editingControlBounds;
	}

	protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Invalid comparison between Unknown and I4
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
		DataGridViewTextBoxDropDownColumn dataGridViewTextBoxDropDownColumn = (DataGridViewTextBoxDropDownColumn)(object)((DataGridViewCell)this).OwningColumn;
		Bitmap cellBitmap = dataGridViewTextBoxDropDownColumn.GetCellBitmap(cellBounds);
		if (cellBitmap != null)
		{
			Graphics val = Graphics.FromImage((Image)(object)cellBitmap);
			try
			{
				PaintCellBackground(val, cellStyle, backBounds);
				PaintCellContent(val, cellBounds, rowIndex, formattedValue, cellStyle, paintParts, cellBitmap);
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

	private void PaintCellContent(Graphics g, Rectangle cellBounds, int rowIndex, object value, DataGridViewCellStyle cellStyle, DataGridViewPaintParts paintParts, Bitmap bm)
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		DataGridViewTextBoxDropDownColumn dataGridViewTextBoxDropDownColumn = (DataGridViewTextBoxDropDownColumn)(object)((DataGridViewCell)this).OwningColumn;
		TextBoxDropDown textBoxDropDown = dataGridViewTextBoxDropDownColumn.TextBoxDropDown;
		Point currentCellAddress = ((DataGridViewElement)this).DataGridView.CurrentCellAddress;
		if ((currentCellAddress.X != ((DataGridViewCell)this).ColumnIndex || currentCellAddress.Y != rowIndex || ((DataGridViewElement)this).DataGridView.EditingControl == null) && rowIndex < ((DataGridViewElement)this).DataGridView.RowCount && PartsSet(paintParts, (DataGridViewPaintParts)8))
		{
			cellBounds.X = 0;
			cellBounds.Y = 0;
			cellBounds.Width -= ((DataGridViewColumn)dataGridViewTextBoxDropDownColumn).DividerWidth + 1;
			cellBounds.Height--;
			((Control)textBoxDropDown).Font = cellStyle.Font;
			((Control)textBoxDropDown).ForeColor = cellStyle.ForeColor;
			((Control)textBoxDropDown).BackColor = cellStyle.BackColor;
			textBoxDropDown.TextAlign = GetTextAlignment(cellStyle.Alignment);
			((Control)textBoxDropDown).Text = GetValue(value);
			dataGridViewTextBoxDropDownColumn.OnBeforeCellPaint(rowIndex, ((DataGridViewCell)this).ColumnIndex);
			Rectangle adjustedEditingControlBounds = GetAdjustedEditingControlBounds(cellBounds, cellStyle);
			if (!dataGridViewTextBoxDropDownColumn.DisplayControlForCurrentCellOnly)
			{
				DrawControl(textBoxDropDown, cellStyle, adjustedEditingControlBounds, bm, g);
			}
			else
			{
				DrawText(textBoxDropDown, cellStyle, adjustedEditingControlBounds, g);
			}
		}
	}

	private void DrawControl(TextBoxDropDown di, DataGridViewCellStyle cellStyle, Rectangle r, Bitmap bm, Graphics g)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		if (di.ButtonGroup.Items.Count > 0)
		{
			Bitmap val = new Bitmap((Image)(object)bm);
			try
			{
				((Control)di).Bounds = r;
				((Control)di).DrawToBitmap(val, r);
				foreach (VisualItem item in di.ButtonGroup.Items)
				{
					if (item.Visible)
					{
						Rectangle renderBounds = item.RenderBounds;
						renderBounds.X += r.X;
						renderBounds.Y += r.Y;
						g.DrawImage((Image)(object)val, renderBounds, renderBounds, (GraphicsUnit)2);
						if (renderBounds.Left < r.Right)
						{
							r.Width -= r.Right - renderBounds.Left - 1;
						}
					}
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		DrawText(di, cellStyle, r, g);
	}

	private void DrawText(TextBoxDropDown di, DataGridViewCellStyle cellStyle, Rectangle r, Graphics g)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Invalid comparison between Unknown and I4
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Invalid comparison between Unknown and I4
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Invalid comparison between Unknown and I4
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Invalid comparison between Unknown and I4
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Invalid comparison between Unknown and I4
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Invalid comparison between Unknown and I4
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Invalid comparison between Unknown and I4
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Invalid comparison between Unknown and I4
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Invalid comparison between Unknown and I4
		r.Inflate(-2, 0);
		eTextFormat eTextFormat = eTextFormat.NoPrefix;
		HorizontalAlignment textAlign = di.TextAlign;
		if ((int)textAlign != 1)
		{
			if ((int)textAlign == 2)
			{
				eTextFormat |= eTextFormat.HorizontalCenter;
			}
		}
		else
		{
			eTextFormat |= eTextFormat.Right;
		}
		if ((int)cellStyle.WrapMode == 1)
		{
			eTextFormat |= eTextFormat.WordBreak;
		}
		DataGridViewContentAlignment alignment = cellStyle.Alignment;
		if ((int)alignment <= 4)
		{
			if (alignment - 1 > 1 && (int)alignment != 4)
			{
				goto IL_0074;
			}
			eTextFormat |= eTextFormat.Default;
		}
		else
		{
			if ((int)alignment != 256 && (int)alignment != 512 && (int)alignment != 1024)
			{
				goto IL_0074;
			}
			eTextFormat |= eTextFormat.Bottom;
		}
		goto IL_0078;
		IL_0074:
		eTextFormat |= eTextFormat.VerticalCenter;
		goto IL_0078;
		IL_0078:
		TextDrawing.DrawString(g, ((Control)di).Text, ((Control)di).Font, ((Control)di).ForeColor, r, eTextFormat);
	}

	private Rectangle GetBackBounds(Rectangle cellBounds, DataGridViewAdvancedBorderStyle advancedBorderStyle)
	{
		DataGridViewTextBoxDropDownColumn dataGridViewTextBoxDropDownColumn = (DataGridViewTextBoxDropDownColumn)(object)((DataGridViewCell)this).OwningColumn;
		Rectangle rectangle = ((DataGridViewCell)this).BorderWidths(advancedBorderStyle);
		cellBounds.Offset(rectangle.X, rectangle.Y);
		cellBounds.Width -= rectangle.Right;
		cellBounds.Height -= rectangle.Bottom;
		if (((DataGridViewCell)this).Selected)
		{
			cellBounds.Width += ((DataGridViewColumn)dataGridViewTextBoxDropDownColumn).DividerWidth;
		}
		return cellBounds;
	}

	private Rectangle GetCellBounds(Rectangle cellBounds)
	{
		DataGridViewTextBoxDropDownColumn dataGridViewTextBoxDropDownColumn = (DataGridViewTextBoxDropDownColumn)(object)((DataGridViewCell)this).OwningColumn;
		Size preferredSize = ((Control)dataGridViewTextBoxDropDownColumn.TextBoxDropDown).PreferredSize;
		cellBounds.Location = new Point(1, 1);
		cellBounds.Width -= ((DataGridViewColumn)dataGridViewTextBoxDropDownColumn).DividerWidth;
		cellBounds.Height = preferredSize.Height - 1;
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

	private HorizontalAlignment GetTextAlignment(DataGridViewContentAlignment alignment)
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
		return (HorizontalAlignment)1;
		IL_002b:
		return (HorizontalAlignment)2;
		IL_002f:
		return (HorizontalAlignment)0;
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
