using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.Editors;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

public class DataGridViewMaskedTextBoxAdvCell : DataGridViewTextBoxCell
{
	public override Type EditType => typeof(DataGridViewMaskedTextBoxAdvEditingControl);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	private static extern IntPtr PostMessage(IntPtr hWnd, int msg, int wParam, int lParam);

	public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Invalid comparison between Unknown and I4
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Expected O, but got Unknown
		((DataGridViewCell)this).DetachEditingControl();
		((DataGridViewTextBoxCell)this).InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
		DataGridViewMaskedTextBoxAdvEditingControl dataGridViewMaskedTextBoxAdvEditingControl = (DataGridViewMaskedTextBoxAdvEditingControl)(object)((DataGridViewElement)this).DataGridView.EditingControl;
		if (((DataGridViewCell)this).OwningColumn is DataGridViewMaskedTextBoxAdvColumn { MaskedTextBoxAdv: var maskedTextBoxAdv })
		{
			dataGridViewMaskedTextBoxAdvEditingControl.AllowPromptAsInput = maskedTextBoxAdv.AllowPromptAsInput;
			dataGridViewMaskedTextBoxAdvEditingControl.AsciiOnly = maskedTextBoxAdv.AsciiOnly;
			dataGridViewMaskedTextBoxAdvEditingControl.BeepOnError = false;
			dataGridViewMaskedTextBoxAdvEditingControl.Culture = maskedTextBoxAdv.Culture;
			dataGridViewMaskedTextBoxAdvEditingControl.CutCopyMaskFormat = maskedTextBoxAdv.CutCopyMaskFormat;
			dataGridViewMaskedTextBoxAdvEditingControl.DropDownControl = maskedTextBoxAdv.DropDownControl;
			((Control)dataGridViewMaskedTextBoxAdvEditingControl).Enabled = ((Control)maskedTextBoxAdv).Enabled;
			dataGridViewMaskedTextBoxAdvEditingControl.FocusHighlightColor = maskedTextBoxAdv.FocusHighlightColor;
			dataGridViewMaskedTextBoxAdvEditingControl.FocusHighlightEnabled = maskedTextBoxAdv.FocusHighlightEnabled;
			dataGridViewMaskedTextBoxAdvEditingControl.HidePromptOnLeave = maskedTextBoxAdv.HidePromptOnLeave;
			((Control)dataGridViewMaskedTextBoxAdvEditingControl).ImeMode = ((Control)maskedTextBoxAdv).ImeMode;
			dataGridViewMaskedTextBoxAdvEditingControl.InsertKeyMode = maskedTextBoxAdv.InsertKeyMode;
			dataGridViewMaskedTextBoxAdvEditingControl.Mask = maskedTextBoxAdv.Mask;
			dataGridViewMaskedTextBoxAdvEditingControl.PasswordChar = maskedTextBoxAdv.PasswordChar;
			dataGridViewMaskedTextBoxAdvEditingControl.PromptChar = maskedTextBoxAdv.PromptChar;
			dataGridViewMaskedTextBoxAdvEditingControl.RejectInputOnFirstFailure = maskedTextBoxAdv.RejectInputOnFirstFailure;
			dataGridViewMaskedTextBoxAdvEditingControl.ResetOnPrompt = maskedTextBoxAdv.ResetOnPrompt;
			dataGridViewMaskedTextBoxAdvEditingControl.ResetOnSpace = maskedTextBoxAdv.ResetOnSpace;
			((Control)dataGridViewMaskedTextBoxAdvEditingControl).RightToLeft = ((Control)maskedTextBoxAdv).RightToLeft;
			dataGridViewMaskedTextBoxAdvEditingControl.SkipLiterals = maskedTextBoxAdv.SkipLiterals;
			dataGridViewMaskedTextBoxAdvEditingControl.TextAlign = GetTextAlignment(dataGridViewCellStyle.Alignment);
			dataGridViewMaskedTextBoxAdvEditingControl.TextMaskFormat = maskedTextBoxAdv.TextMaskFormat;
			dataGridViewMaskedTextBoxAdvEditingControl.UseSystemPasswordChar = maskedTextBoxAdv.UseSystemPasswordChar;
			dataGridViewMaskedTextBoxAdvEditingControl.ValidatingType = maskedTextBoxAdv.ValidatingType;
			dataGridViewMaskedTextBoxAdvEditingControl.WatermarkBehavior = maskedTextBoxAdv.WatermarkBehavior;
			dataGridViewMaskedTextBoxAdvEditingControl.WatermarkColor = maskedTextBoxAdv.WatermarkColor;
			dataGridViewMaskedTextBoxAdvEditingControl.WatermarkEnabled = maskedTextBoxAdv.WatermarkEnabled;
			dataGridViewMaskedTextBoxAdvEditingControl.WatermarkFont = maskedTextBoxAdv.WatermarkFont;
			dataGridViewMaskedTextBoxAdvEditingControl.WatermarkText = maskedTextBoxAdv.WatermarkText;
			((TextBoxBase)dataGridViewMaskedTextBoxAdvEditingControl.MaskedTextBox).Multiline = (int)dataGridViewCellStyle.WrapMode == 1;
			dataGridViewMaskedTextBoxAdvEditingControl.BackgroundStyle.ApplyStyle(maskedTextBoxAdv.BackgroundStyle);
			dataGridViewMaskedTextBoxAdvEditingControl.BackgroundStyle.Class = maskedTextBoxAdv.BackgroundStyle.Class;
			maskedTextBoxAdv.ButtonClear.CopyToItem(dataGridViewMaskedTextBoxAdvEditingControl.ButtonClear);
			maskedTextBoxAdv.ButtonCustom.CopyToItem(dataGridViewMaskedTextBoxAdvEditingControl.ButtonCustom);
			maskedTextBoxAdv.ButtonCustom2.CopyToItem(dataGridViewMaskedTextBoxAdvEditingControl.ButtonCustom2);
			maskedTextBoxAdv.ButtonDropDown.CopyToItem(dataGridViewMaskedTextBoxAdvEditingControl.ButtonDropDown);
			dataGridViewMaskedTextBoxAdvEditingControl.ButtonClearClick += ButtonClearClick;
			dataGridViewMaskedTextBoxAdvEditingControl.ButtonCustomClick += ButtonCustomClick;
			dataGridViewMaskedTextBoxAdvEditingControl.ButtonCustom2Click += ButtonCustom2Click;
			dataGridViewMaskedTextBoxAdvEditingControl.ButtonDropDownClick += ButtonDropDownClick;
			((Control)dataGridViewMaskedTextBoxAdvEditingControl).KeyDown += new KeyEventHandler(KeyDown);
			((Control)dataGridViewMaskedTextBoxAdvEditingControl).Text = GetValue(initialFormattedValue);
			dataGridViewMaskedTextBoxAdvEditingControl.BeepOnError = maskedTextBoxAdv.BeepOnError;
		}
	}

	public override void DetachEditingControl()
	{
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected O, but got Unknown
		if (((DataGridViewElement)this).DataGridView != null && ((DataGridViewElement)this).DataGridView.EditingControl != null && ((DataGridViewElement)this).DataGridView.EditingControl is DataGridViewMaskedTextBoxAdvEditingControl dataGridViewMaskedTextBoxAdvEditingControl)
		{
			dataGridViewMaskedTextBoxAdvEditingControl.ButtonClearClick -= ButtonClearClick;
			dataGridViewMaskedTextBoxAdvEditingControl.ButtonCustomClick -= ButtonCustomClick;
			dataGridViewMaskedTextBoxAdvEditingControl.ButtonCustom2Click -= ButtonCustom2Click;
			dataGridViewMaskedTextBoxAdvEditingControl.ButtonDropDownClick -= ButtonDropDownClick;
			((Control)dataGridViewMaskedTextBoxAdvEditingControl).KeyDown -= new KeyEventHandler(KeyDown);
		}
		((DataGridViewTextBoxCell)this).DetachEditingControl();
	}

	private void ButtonClearClick(object sender, CancelEventArgs e)
	{
		((DataGridViewMaskedTextBoxAdvColumn)(object)((DataGridViewCell)this).OwningColumn).DoButtonClearClick(sender, e);
	}

	private void ButtonCustomClick(object sender, EventArgs e)
	{
		((DataGridViewMaskedTextBoxAdvColumn)(object)((DataGridViewCell)this).OwningColumn).DoButtonCustomClick(sender, e);
	}

	private void ButtonCustom2Click(object sender, EventArgs e)
	{
		((DataGridViewMaskedTextBoxAdvColumn)(object)((DataGridViewCell)this).OwningColumn).DoButtonCustom2Click(sender, e);
	}

	private void ButtonDropDownClick(object sender, CancelEventArgs e)
	{
		((DataGridViewMaskedTextBoxAdvColumn)(object)((DataGridViewCell)this).OwningColumn).DoButtonDropDownClick(sender, e);
	}

	private void KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected I4, but got Unknown
		if (((DataGridViewElement)this).DataGridView != null && ((DataGridViewElement)this).DataGridView.EditingControl != null && ((DataGridViewElement)this).DataGridView.EditingControl is DataGridViewMaskedTextBoxAdvEditingControl dataGridViewMaskedTextBoxAdvEditingControl)
		{
			((TextBoxBase)dataGridViewMaskedTextBoxAdvEditingControl.MaskedTextBox).SelectAll();
			PostMessage(((Control)dataGridViewMaskedTextBoxAdvEditingControl.MaskedTextBox).Handle, 256, (int)e.KeyCode, 1);
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

	protected override Size GetPreferredSize(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
	{
		if (((DataGridViewElement)this).DataGridView == null)
		{
			return new Size(-1, -1);
		}
		Size preferredSize = ((DataGridViewTextBoxCell)this).GetPreferredSize(graphics, cellStyle, rowIndex, constraintSize);
		if (((DataGridViewCell)this).OwningColumn is DataGridViewMaskedTextBoxAdvColumn { MaskedTextBoxAdv: var maskedTextBoxAdv })
		{
			preferredSize.Height = ((Control)maskedTextBoxAdv).Height;
			if (constraintSize.Width == 0)
			{
				preferredSize.Width += GetImageWidth(maskedTextBoxAdv.ButtonClear);
				preferredSize.Width += GetImageWidth(maskedTextBoxAdv.ButtonCustom);
				preferredSize.Width += GetImageWidth(maskedTextBoxAdv.ButtonCustom2);
				preferredSize.Width += GetImageWidth(maskedTextBoxAdv.ButtonDropDown);
			}
		}
		return preferredSize;
	}

	protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		if (((DataGridViewCell)this).OwningColumn is DataGridViewMaskedTextBoxAdvColumn { MaskedTextBoxAdv: var maskedTextBoxAdv })
		{
			((Control)maskedTextBoxAdv).Text = GetValue(value);
			return ((Control)maskedTextBoxAdv).Text;
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
		DataGridViewMaskedTextBoxAdvColumn dataGridViewMaskedTextBoxAdvColumn = (DataGridViewMaskedTextBoxAdvColumn)(object)((DataGridViewCell)this).OwningColumn;
		Bitmap cellBitmap = dataGridViewMaskedTextBoxAdvColumn.GetCellBitmap(cellBounds);
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
		DataGridViewMaskedTextBoxAdvColumn dataGridViewMaskedTextBoxAdvColumn = (DataGridViewMaskedTextBoxAdvColumn)(object)((DataGridViewCell)this).OwningColumn;
		MaskedTextBoxAdv maskedTextBoxAdv = dataGridViewMaskedTextBoxAdvColumn.MaskedTextBoxAdv;
		Point currentCellAddress = ((DataGridViewElement)this).DataGridView.CurrentCellAddress;
		if ((currentCellAddress.X != ((DataGridViewCell)this).ColumnIndex || currentCellAddress.Y != rowIndex || ((DataGridViewElement)this).DataGridView.EditingControl == null) && rowIndex < ((DataGridViewElement)this).DataGridView.RowCount && PartsSet(paintParts, (DataGridViewPaintParts)8))
		{
			cellBounds.X = 0;
			cellBounds.Y = 0;
			cellBounds.Width -= ((DataGridViewColumn)dataGridViewMaskedTextBoxAdvColumn).DividerWidth + 1;
			cellBounds.Height--;
			((Control)maskedTextBoxAdv).Font = cellStyle.Font;
			((Control)maskedTextBoxAdv).ForeColor = cellStyle.ForeColor;
			((Control)maskedTextBoxAdv).BackColor = cellStyle.BackColor;
			maskedTextBoxAdv.TextAlign = GetTextAlignment(cellStyle.Alignment);
			((Control)maskedTextBoxAdv).Text = GetValue(value);
			Rectangle adjustedEditingControlBounds = GetAdjustedEditingControlBounds(cellBounds, cellStyle);
			dataGridViewMaskedTextBoxAdvColumn.OnBeforeCellPaint(rowIndex, ((DataGridViewCell)this).ColumnIndex);
			if (!dataGridViewMaskedTextBoxAdvColumn.DisplayControlForCurrentCellOnly)
			{
				DrawControl(maskedTextBoxAdv, cellStyle, adjustedEditingControlBounds, bm, g, dataGridViewMaskedTextBoxAdvColumn);
			}
			else
			{
				DrawText(maskedTextBoxAdv, cellStyle, adjustedEditingControlBounds, g);
			}
		}
	}

	private void DrawControl(MaskedTextBoxAdv di, DataGridViewCellStyle cellStyle, Rectangle r, Bitmap bm, Graphics g, DataGridViewMaskedTextBoxAdvColumn oc)
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		if (di.ButtonGroup.Items.Count > 0)
		{
			if (MustRenderVisibleControl())
			{
				((Control)di).Location = ((Control)((DataGridViewElement)oc).DataGridView).Location;
				if (((Control)di).Parent == null)
				{
					Form val = ((Control)((DataGridViewElement)oc).DataGridView).FindForm();
					if (val != null)
					{
						((Control)di).Parent = (Control)(object)val;
					}
				}
				((Control)di).SendToBack();
				((Control)di).Visible = true;
			}
			Bitmap val2 = new Bitmap((Image)(object)bm);
			try
			{
				((Control)di).Bounds = r;
				((Control)di).DrawToBitmap(val2, r);
				foreach (VisualItem item in di.ButtonGroup.Items)
				{
					if (item.Visible)
					{
						Rectangle renderBounds = item.RenderBounds;
						renderBounds.X += r.X;
						renderBounds.Y += r.Y;
						g.DrawImage((Image)(object)val2, renderBounds, renderBounds, (GraphicsUnit)2);
						if (renderBounds.Left < r.Right)
						{
							r.Width -= r.Right - renderBounds.Left - 1;
						}
					}
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			((Control)di).Visible = false;
		}
		DrawText(di, cellStyle, r, g);
	}

	private void DrawText(MaskedTextBoxAdv di, DataGridViewCellStyle cellStyle, Rectangle r, Graphics g)
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

	private bool MustRenderVisibleControl()
	{
		OperatingSystem oSVersion = Environment.OSVersion;
		if (oSVersion.Platform != PlatformID.Win32Windows)
		{
			if (oSVersion.Platform == PlatformID.Win32NT)
			{
				return oSVersion.Version.Major < 6;
			}
			return false;
		}
		return true;
	}

	private Rectangle GetBackBounds(Rectangle cellBounds, DataGridViewAdvancedBorderStyle advancedBorderStyle)
	{
		DataGridViewMaskedTextBoxAdvColumn dataGridViewMaskedTextBoxAdvColumn = (DataGridViewMaskedTextBoxAdvColumn)(object)((DataGridViewCell)this).OwningColumn;
		Rectangle rectangle = ((DataGridViewCell)this).BorderWidths(advancedBorderStyle);
		cellBounds.Offset(rectangle.X, rectangle.Y);
		cellBounds.Width -= rectangle.Right;
		cellBounds.Height -= rectangle.Bottom;
		if (((DataGridViewCell)this).Selected)
		{
			cellBounds.Width += ((DataGridViewColumn)dataGridViewMaskedTextBoxAdvColumn).DividerWidth;
		}
		return cellBounds;
	}

	private Rectangle GetCellBounds(Rectangle cellBounds)
	{
		DataGridViewMaskedTextBoxAdvColumn dataGridViewMaskedTextBoxAdvColumn = (DataGridViewMaskedTextBoxAdvColumn)(object)((DataGridViewCell)this).OwningColumn;
		Size preferredSize = ((Control)dataGridViewMaskedTextBoxAdvColumn.MaskedTextBoxAdv).PreferredSize;
		cellBounds.Location = new Point(1, 1);
		cellBounds.Width -= ((DataGridViewColumn)dataGridViewMaskedTextBoxAdvColumn).DividerWidth;
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
