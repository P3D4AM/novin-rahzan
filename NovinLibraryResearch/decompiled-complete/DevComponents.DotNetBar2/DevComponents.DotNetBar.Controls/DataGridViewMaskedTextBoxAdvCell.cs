using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DevComponents.Editors;

namespace DevComponents.DotNetBar.Controls;

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
		if (((DataGridViewCell)this).OwningColumn is DataGridViewMaskedTextBoxAdvColumn { _28CE: var _28CE })
		{
			dataGridViewMaskedTextBoxAdvEditingControl.AllowPromptAsInput = _28CE.AllowPromptAsInput;
			dataGridViewMaskedTextBoxAdvEditingControl.AsciiOnly = _28CE.AsciiOnly;
			dataGridViewMaskedTextBoxAdvEditingControl.BeepOnError = false;
			dataGridViewMaskedTextBoxAdvEditingControl.Culture = _28CE.Culture;
			dataGridViewMaskedTextBoxAdvEditingControl.CutCopyMaskFormat = _28CE.CutCopyMaskFormat;
			dataGridViewMaskedTextBoxAdvEditingControl.DropDownControl = _28CE.DropDownControl;
			((Control)dataGridViewMaskedTextBoxAdvEditingControl).Enabled = ((Control)_28CE).Enabled;
			dataGridViewMaskedTextBoxAdvEditingControl.FocusHighlightColor = _28CE.FocusHighlightColor;
			dataGridViewMaskedTextBoxAdvEditingControl.FocusHighlightEnabled = _28CE.FocusHighlightEnabled;
			dataGridViewMaskedTextBoxAdvEditingControl.HidePromptOnLeave = _28CE.HidePromptOnLeave;
			((Control)dataGridViewMaskedTextBoxAdvEditingControl).ImeMode = ((Control)_28CE).ImeMode;
			dataGridViewMaskedTextBoxAdvEditingControl.InsertKeyMode = _28CE.InsertKeyMode;
			dataGridViewMaskedTextBoxAdvEditingControl.Mask = _28CE.Mask;
			dataGridViewMaskedTextBoxAdvEditingControl.PasswordChar = _28CE.PasswordChar;
			dataGridViewMaskedTextBoxAdvEditingControl.PromptChar = _28CE.PromptChar;
			dataGridViewMaskedTextBoxAdvEditingControl.RejectInputOnFirstFailure = _28CE.RejectInputOnFirstFailure;
			dataGridViewMaskedTextBoxAdvEditingControl.ResetOnPrompt = _28CE.ResetOnPrompt;
			dataGridViewMaskedTextBoxAdvEditingControl.ResetOnSpace = _28CE.ResetOnSpace;
			((Control)dataGridViewMaskedTextBoxAdvEditingControl).RightToLeft = ((Control)_28CE).RightToLeft;
			dataGridViewMaskedTextBoxAdvEditingControl.SkipLiterals = _28CE.SkipLiterals;
			dataGridViewMaskedTextBoxAdvEditingControl.TextAlign = _2B4B(dataGridViewCellStyle.Alignment);
			dataGridViewMaskedTextBoxAdvEditingControl.TextMaskFormat = _28CE.TextMaskFormat;
			dataGridViewMaskedTextBoxAdvEditingControl.UseSystemPasswordChar = _28CE.UseSystemPasswordChar;
			dataGridViewMaskedTextBoxAdvEditingControl.ValidatingType = _28CE.ValidatingType;
			dataGridViewMaskedTextBoxAdvEditingControl.WatermarkBehavior = _28CE.WatermarkBehavior;
			dataGridViewMaskedTextBoxAdvEditingControl.WatermarkColor = _28CE.WatermarkColor;
			dataGridViewMaskedTextBoxAdvEditingControl.WatermarkEnabled = _28CE.WatermarkEnabled;
			dataGridViewMaskedTextBoxAdvEditingControl.WatermarkFont = _28CE.WatermarkFont;
			dataGridViewMaskedTextBoxAdvEditingControl.WatermarkText = _28CE.WatermarkText;
			((TextBoxBase)dataGridViewMaskedTextBoxAdvEditingControl.MaskedTextBox).Multiline = (int)dataGridViewCellStyle.WrapMode == 1;
			dataGridViewMaskedTextBoxAdvEditingControl.BackgroundStyle.ApplyStyle(_28CE.BackgroundStyle);
			dataGridViewMaskedTextBoxAdvEditingControl.BackgroundStyle.Class = _28CE.BackgroundStyle.Class;
			_28CE.ButtonClear.ᡄ(dataGridViewMaskedTextBoxAdvEditingControl.ButtonClear);
			_28CE.ButtonCustom.ᡄ(dataGridViewMaskedTextBoxAdvEditingControl.ButtonCustom);
			_28CE.ButtonCustom2.ᡄ(dataGridViewMaskedTextBoxAdvEditingControl.ButtonCustom2);
			_28CE.ButtonDropDown.ᡄ(dataGridViewMaskedTextBoxAdvEditingControl.ButtonDropDown);
			dataGridViewMaskedTextBoxAdvEditingControl.ButtonClearClick += _៦;
			dataGridViewMaskedTextBoxAdvEditingControl.ButtonCustomClick += _181E;
			dataGridViewMaskedTextBoxAdvEditingControl.ButtonCustom2Click += _181F;
			dataGridViewMaskedTextBoxAdvEditingControl.ButtonDropDownClick += _៧;
			((Control)dataGridViewMaskedTextBoxAdvEditingControl).KeyDown += new KeyEventHandler(ᵢ);
			((Control)dataGridViewMaskedTextBoxAdvEditingControl).Text = _239B(initialFormattedValue);
			dataGridViewMaskedTextBoxAdvEditingControl.BeepOnError = _28CE.BeepOnError;
		}
	}

	public override void DetachEditingControl()
	{
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		if (((DataGridViewElement)this).DataGridView != null && ((DataGridViewElement)this).DataGridView.EditingControl != null && ((DataGridViewElement)this).DataGridView.EditingControl is DataGridViewMaskedTextBoxAdvEditingControl dataGridViewMaskedTextBoxAdvEditingControl)
		{
			dataGridViewMaskedTextBoxAdvEditingControl.ButtonClearClick -= _៦;
			dataGridViewMaskedTextBoxAdvEditingControl.ButtonCustomClick -= _181E;
			dataGridViewMaskedTextBoxAdvEditingControl.ButtonCustom2Click -= _181F;
			dataGridViewMaskedTextBoxAdvEditingControl.ButtonDropDownClick -= _៧;
			((Control)dataGridViewMaskedTextBoxAdvEditingControl).KeyDown -= new KeyEventHandler(ᵢ);
		}
		((DataGridViewTextBoxCell)this).DetachEditingControl();
	}

	private void _៦(object _٠, CancelEventArgs ؿ)
	{
		((DataGridViewMaskedTextBoxAdvColumn)(object)((DataGridViewCell)this).OwningColumn)._2B31(_٠, ؿ);
	}

	private void _181E(object _٠, EventArgs ؿ)
	{
		((DataGridViewMaskedTextBoxAdvColumn)(object)((DataGridViewCell)this).OwningColumn)._2B32(_٠, ؿ);
	}

	private void _181F(object _٠, EventArgs ؿ)
	{
		((DataGridViewMaskedTextBoxAdvColumn)(object)((DataGridViewCell)this).OwningColumn)._2B33(_٠, ؿ);
	}

	private void _៧(object _٠, CancelEventArgs ؿ)
	{
		((DataGridViewMaskedTextBoxAdvColumn)(object)((DataGridViewCell)this).OwningColumn)._2B34(_٠, ؿ);
	}

	private void ᵢ(object _٠, KeyEventArgs ؿ)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected I4, but got Unknown
		if (((DataGridViewElement)this).DataGridView != null && ((DataGridViewElement)this).DataGridView.EditingControl != null && ((DataGridViewElement)this).DataGridView.EditingControl is DataGridViewMaskedTextBoxAdvEditingControl dataGridViewMaskedTextBoxAdvEditingControl)
		{
			((TextBoxBase)dataGridViewMaskedTextBoxAdvEditingControl.MaskedTextBox).SelectAll();
			PostMessage(((Control)dataGridViewMaskedTextBoxAdvEditingControl.MaskedTextBox).Handle, 256, (int)ؿ.KeyCode, 1);
		}
	}

	public override void PositionEditingControl(bool setLocation, bool setSize, Rectangle cellBounds, Rectangle cellClip, DataGridViewCellStyle cellStyle, bool singleVerticalBorderAdded, bool singleHorizontalBorderAdded, bool isFirstDisplayedColumn, bool isFirstDisplayedRow)
	{
		Rectangle _2B06 = ((DataGridViewCell)this).PositionEditingPanel(cellBounds, cellClip, cellStyle, singleVerticalBorderAdded, singleHorizontalBorderAdded, isFirstDisplayedColumn, isFirstDisplayedRow);
		_2B06 = _2B05(_2B06, cellStyle);
		((DataGridViewElement)this).DataGridView.EditingControl.Location = new Point(_2B06.X, _2B06.Y);
		((DataGridViewElement)this).DataGridView.EditingControl.Size = new Size(_2B06.Width, _2B06.Height);
	}

	private Rectangle _2B05(Rectangle _2B06, DataGridViewCellStyle _2AE6)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Invalid comparison between Unknown and I4
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Invalid comparison between Unknown and I4
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Invalid comparison between Unknown and I4
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Invalid comparison between Unknown and I4
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Invalid comparison between Unknown and I4
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Invalid comparison between Unknown and I4
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Invalid comparison between Unknown and I4
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Invalid comparison between Unknown and I4
		_2B06.X++;
		_2B06.Y++;
		_2B06.Width = Math.Max(0, _2B06.Width - 2);
		_2B06.Height = Math.Max(0, _2B06.Height - 2);
		Rectangle rectangle = _2B1B(_2B06);
		if ((int)_2AE6.WrapMode != 1)
		{
			if (rectangle.Height < _2B06.Height)
			{
				DataGridViewContentAlignment alignment = _2AE6.Alignment;
				if ((int)alignment <= 64)
				{
					if ((int)alignment == 16 || (int)alignment == 32 || (int)alignment == 64)
					{
						_2B06.Y += (_2B06.Height - rectangle.Height) / 2;
					}
				}
				else if ((int)alignment == 256 || (int)alignment == 512 || (int)alignment == 1024)
				{
					_2B06.Y += _2B06.Height - rectangle.Height;
				}
			}
			_2B06.Height = Math.Max(1, rectangle.Height);
		}
		_2B06.Width = Math.Max(1, _2B06.Width);
		return _2B06;
	}

	protected override Size GetPreferredSize(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
	{
		if (((DataGridViewElement)this).DataGridView == null)
		{
			return new Size(-1, -1);
		}
		Size preferredSize = ((DataGridViewTextBoxCell)this).GetPreferredSize(graphics, cellStyle, rowIndex, constraintSize);
		if (((DataGridViewCell)this).OwningColumn is DataGridViewMaskedTextBoxAdvColumn { _28CE: var _28CE })
		{
			preferredSize.Height = ((Control)_28CE).Height;
			if (constraintSize.Width == 0)
			{
				preferredSize.Width += _2B29(_28CE.ButtonClear);
				preferredSize.Width += _2B29(_28CE.ButtonCustom);
				preferredSize.Width += _2B29(_28CE.ButtonCustom2);
				preferredSize.Width += _2B29(_28CE.ButtonDropDown);
			}
		}
		return preferredSize;
	}

	protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		if (((DataGridViewCell)this).OwningColumn is DataGridViewMaskedTextBoxAdvColumn { _28CE: var _28CE })
		{
			((Control)_28CE).Text = _239B(value);
			return ((Control)_28CE).Text;
		}
		return ((DataGridViewCell)this).GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
	}

	private int _2B29(InputButtonSettings _2B2A)
	{
		if (_2B2A.Visible)
		{
			if (_2B2A.Image == null)
			{
				return 16;
			}
			return _2B2A.Image.Width;
		}
		return 0;
	}

	protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Invalid comparison between Unknown and I4
		if (((DataGridViewElement)this).DataGridView == null)
		{
			return;
		}
		if (_2AED(paintParts, (DataGridViewPaintParts)2))
		{
			((DataGridViewCell)this).PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
		}
		if (!_2AED(paintParts, (DataGridViewPaintParts)1))
		{
			return;
		}
		Rectangle rectangle = _2AEE(cellBounds, advancedBorderStyle);
		if (rectangle.Height <= 0 || rectangle.Width <= 0)
		{
			return;
		}
		DataGridViewMaskedTextBoxAdvColumn dataGridViewMaskedTextBoxAdvColumn = (DataGridViewMaskedTextBoxAdvColumn)(object)((DataGridViewCell)this).OwningColumn;
		Bitmap val = dataGridViewMaskedTextBoxAdvColumn._2B01(cellBounds);
		if (val != null)
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				_2B13(val2, cellStyle, rectangle);
				_2B14(val2, cellBounds, rowIndex, formattedValue, cellStyle, paintParts, val);
				graphics.DrawImageUnscaledAndClipped((Image)(object)val, rectangle);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			if (((DataGridViewElement)this).DataGridView.ShowCellErrors && (paintParts & 0x10) == 16)
			{
				((DataGridViewCell)this).PaintErrorIcon(graphics, clipBounds, cellBounds, errorText);
			}
		}
	}

	private void _2B13(Graphics _٥, DataGridViewCellStyle _2AE6, Rectangle _2AE7)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		Rectangle rectangle = _2AE7;
		rectangle.Location = new Point(0, 0);
		if (((DataGridViewElement)this).DataGridView is DataGridViewX dataGridViewX && ((Control)dataGridViewX).Enabled && ((DataGridViewCell)this).Selected && dataGridViewX.PaintEnhancedSelection)
		{
			ᯊ.ᯓ(_٥, dataGridViewX._2242, rectangle, RoundRectangleShapeDescriptor.RectangleShape, ᯕ: false, ᯖ: false);
			return;
		}
		Color color = (((DataGridViewCell)this).Selected ? _2AE6.SelectionBackColor : _2AE6.BackColor);
		Brush val = (Brush)new SolidBrush(color);
		try
		{
			_٥.FillRectangle(val, rectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void _2B14(Graphics _٥, Rectangle _2AE9, int _2AEA, object ԝ, DataGridViewCellStyle _2AE6, DataGridViewPaintParts _2AEB, Bitmap _2ADA)
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		DataGridViewMaskedTextBoxAdvColumn dataGridViewMaskedTextBoxAdvColumn = (DataGridViewMaskedTextBoxAdvColumn)(object)((DataGridViewCell)this).OwningColumn;
		MaskedTextBoxAdv _28CE = dataGridViewMaskedTextBoxAdvColumn._28CE;
		Point currentCellAddress = ((DataGridViewElement)this).DataGridView.CurrentCellAddress;
		if ((currentCellAddress.X != ((DataGridViewCell)this).ColumnIndex || currentCellAddress.Y != _2AEA || ((DataGridViewElement)this).DataGridView.EditingControl == null) && _2AEA < ((DataGridViewElement)this).DataGridView.RowCount && _2AED(_2AEB, (DataGridViewPaintParts)8))
		{
			_2AE9.X = 0;
			_2AE9.Y = 0;
			_2AE9.Width -= ((DataGridViewColumn)dataGridViewMaskedTextBoxAdvColumn).DividerWidth + 1;
			_2AE9.Height--;
			((Control)_28CE).Font = _2AE6.Font;
			((Control)_28CE).ForeColor = _2AE6.ForeColor;
			((Control)_28CE).BackColor = _2AE6.BackColor;
			_28CE.TextAlign = _2B4B(_2AE6.Alignment);
			((Control)_28CE).Text = _239B(ԝ);
			Rectangle ल = _2B05(_2AE9, _2AE6);
			dataGridViewMaskedTextBoxAdvColumn._2B02(_2AEA, ((DataGridViewCell)this).ColumnIndex);
			if (!dataGridViewMaskedTextBoxAdvColumn.DisplayControlForCurrentCellOnly)
			{
				_2B15(_28CE, _2AE6, ल, _2ADA, _٥, dataGridViewMaskedTextBoxAdvColumn);
			}
			else
			{
				_0EE6(_28CE, _2AE6, ल, _٥);
			}
		}
	}

	private void _2B15(MaskedTextBoxAdv ឡ, DataGridViewCellStyle _2AE6, Rectangle ल, Bitmap _2ADA, Graphics _٥, DataGridViewMaskedTextBoxAdvColumn _2B07)
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		if (ឡ._28C0.Items.Count > 0)
		{
			if (_2B1A())
			{
				((Control)ឡ).Location = ((Control)((DataGridViewElement)_2B07).DataGridView).Location;
				if (((Control)ឡ).Parent == null)
				{
					Form val = ((Control)((DataGridViewElement)_2B07).DataGridView).FindForm();
					if (val != null)
					{
						((Control)ឡ).Parent = (Control)(object)val;
					}
				}
				((Control)ឡ).SendToBack();
				((Control)ឡ).Visible = true;
			}
			Bitmap val2 = new Bitmap((Image)(object)_2ADA);
			try
			{
				((Control)ឡ).Bounds = ल;
				((Control)ឡ).DrawToBitmap(val2, ल);
				foreach (VisualItem item in ឡ._28C0.Items)
				{
					if (item.Visible)
					{
						Rectangle renderBounds = item.RenderBounds;
						renderBounds.X += ल.X;
						renderBounds.Y += ल.Y;
						_٥.DrawImage((Image)(object)val2, renderBounds, renderBounds, (GraphicsUnit)2);
						if (renderBounds.Left < ल.Right)
						{
							ल.Width -= ल.Right - renderBounds.Left - 1;
						}
					}
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			((Control)ឡ).Visible = false;
		}
		_0EE6(ឡ, _2AE6, ल, _٥);
	}

	private void _0EE6(MaskedTextBoxAdv ឡ, DataGridViewCellStyle _2AE6, Rectangle ल, Graphics _٥)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected I4, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Invalid comparison between Unknown and I4
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Invalid comparison between Unknown and I4
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Invalid comparison between Unknown and I4
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected I4, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Invalid comparison between Unknown and I4
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Invalid comparison between Unknown and I4
		ल.Inflate(-2, 0);
		eTextFormat eTextFormat = eTextFormat.NoPrefix;
		HorizontalAlignment textAlign = ឡ.TextAlign;
		switch (textAlign - 1)
		{
		case 1:
			eTextFormat |= eTextFormat.HorizontalCenter;
			break;
		case 0:
			eTextFormat |= eTextFormat.Right;
			break;
		}
		if ((int)_2AE6.WrapMode == 1)
		{
			eTextFormat |= eTextFormat.WordBreak;
		}
		DataGridViewContentAlignment alignment = _2AE6.Alignment;
		if ((int)alignment <= 256)
		{
			switch (alignment - 1)
			{
			case 0:
			case 1:
			case 3:
				goto IL_008a;
			case 2:
				goto IL_0094;
			}
			if ((int)alignment == 256)
			{
				goto IL_008e;
			}
		}
		else if ((int)alignment == 512 || (int)alignment == 1024)
		{
			goto IL_008e;
		}
		goto IL_0094;
		IL_0094:
		eTextFormat |= eTextFormat.VerticalCenter;
		goto IL_0098;
		IL_0098:
		DataGridViewMaskedTextBoxAdvColumn dataGridViewMaskedTextBoxAdvColumn = (DataGridViewMaskedTextBoxAdvColumn)(object)((DataGridViewCell)this).OwningColumn;
		string text = ((Control)ឡ).Text;
		if (dataGridViewMaskedTextBoxAdvColumn.PasswordChar != 0)
		{
			text = "".PadRight(text.Length, dataGridViewMaskedTextBoxAdvColumn.PasswordChar);
		}
		TextDrawing.DrawString(_٥, text, ((Control)ឡ).Font, ((Control)ឡ).ForeColor, ल, eTextFormat);
		return;
		IL_008a:
		eTextFormat = eTextFormat;
		goto IL_0098;
		IL_008e:
		eTextFormat |= eTextFormat.Bottom;
		goto IL_0098;
	}

	private bool _2B1A()
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

	private Rectangle _2AEE(Rectangle _2AE9, DataGridViewAdvancedBorderStyle _2AEF)
	{
		DataGridViewMaskedTextBoxAdvColumn dataGridViewMaskedTextBoxAdvColumn = (DataGridViewMaskedTextBoxAdvColumn)(object)((DataGridViewCell)this).OwningColumn;
		Rectangle rectangle = ((DataGridViewCell)this).BorderWidths(_2AEF);
		_2AE9.Offset(rectangle.X, rectangle.Y);
		_2AE9.Width -= rectangle.Right;
		_2AE9.Height -= rectangle.Bottom;
		if (((DataGridViewCell)this).Selected)
		{
			_2AE9.Width += ((DataGridViewColumn)dataGridViewMaskedTextBoxAdvColumn).DividerWidth;
		}
		return _2AE9;
	}

	private Rectangle _2B1B(Rectangle _2AE9)
	{
		DataGridViewMaskedTextBoxAdvColumn dataGridViewMaskedTextBoxAdvColumn = (DataGridViewMaskedTextBoxAdvColumn)(object)((DataGridViewCell)this).OwningColumn;
		Size preferredSize = ((Control)dataGridViewMaskedTextBoxAdvColumn._28CE).PreferredSize;
		_2AE9.Location = new Point(1, 1);
		_2AE9.Width -= ((DataGridViewColumn)dataGridViewMaskedTextBoxAdvColumn).DividerWidth;
		_2AE9.Height = preferredSize.Height - 1;
		return _2AE9;
	}

	private string _239B(object ԝ)
	{
		if (ԝ == Convert.DBNull)
		{
			return "";
		}
		return Convert.ToString(ԝ);
	}

	private HorizontalAlignment _2B4B(DataGridViewContentAlignment _2B2C)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Invalid comparison between Unknown and I4
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Invalid comparison between Unknown and I4
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected I4, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Invalid comparison between Unknown and I4
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Invalid comparison between Unknown and I4
		if ((int)_2B2C <= 32)
		{
			switch (_2B2C - 2)
			{
			default:
				if ((int)_2B2C == 32)
				{
					break;
				}
				goto IL_003d;
			case 0:
				break;
			case 2:
				goto IL_003b;
			case 1:
				goto IL_003d;
			}
			goto IL_0039;
		}
		if ((int)_2B2C != 64)
		{
			if ((int)_2B2C == 512)
			{
				goto IL_0039;
			}
			if ((int)_2B2C != 1024)
			{
				goto IL_003d;
			}
		}
		goto IL_003b;
		IL_003b:
		return (HorizontalAlignment)1;
		IL_003d:
		return (HorizontalAlignment)0;
		IL_0039:
		return (HorizontalAlignment)2;
	}

	private bool _2AED(DataGridViewPaintParts _2AEB, DataGridViewPaintParts ᝑ)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return (DataGridViewPaintParts)(_2AEB & ᝑ) == ᝑ;
	}
}
