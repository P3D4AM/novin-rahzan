using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DevComponents.Editors;

namespace DevComponents.DotNetBar.Controls;

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
		TextBoxDropDown _2B64 = dataGridViewTextBoxDropDownColumn._2B64;
		dataGridViewTextBoxDropDownEditingControl.AutoCompleteCustomSource = _2B64.AutoCompleteCustomSource;
		dataGridViewTextBoxDropDownEditingControl.AutoCompleteMode = _2B64.AutoCompleteMode;
		dataGridViewTextBoxDropDownEditingControl.AutoCompleteSource = _2B64.AutoCompleteSource;
		((Control)dataGridViewTextBoxDropDownEditingControl).BackColor = ((Control)_2B64).BackColor;
		dataGridViewTextBoxDropDownEditingControl.CharacterCasing = _2B64.CharacterCasing;
		dataGridViewTextBoxDropDownEditingControl.DropDownControl = _2B64.DropDownControl;
		((Control)dataGridViewTextBoxDropDownEditingControl).Enabled = ((Control)_2B64).Enabled;
		dataGridViewTextBoxDropDownEditingControl.FocusHighlightColor = _2B64.FocusHighlightColor;
		dataGridViewTextBoxDropDownEditingControl.FocusHighlightEnabled = _2B64.FocusHighlightEnabled;
		((Control)dataGridViewTextBoxDropDownEditingControl).ForeColor = ((Control)_2B64).ForeColor;
		dataGridViewTextBoxDropDownEditingControl.HideSelection = _2B64.HideSelection;
		((Control)dataGridViewTextBoxDropDownEditingControl).ImeMode = ((Control)_2B64).ImeMode;
		dataGridViewTextBoxDropDownEditingControl.MaxLength = _2B64.MaxLength;
		dataGridViewTextBoxDropDownEditingControl.PasswordChar = _2B64.PasswordChar;
		((Control)dataGridViewTextBoxDropDownEditingControl).RightToLeft = ((Control)_2B64).RightToLeft;
		dataGridViewTextBoxDropDownEditingControl.TextAlign = _2B4B(dataGridViewCellStyle.Alignment);
		dataGridViewTextBoxDropDownEditingControl.UseSystemPasswordChar = _2B64.UseSystemPasswordChar;
		dataGridViewTextBoxDropDownEditingControl.WatermarkBehavior = _2B64.WatermarkBehavior;
		dataGridViewTextBoxDropDownEditingControl.WatermarkColor = _2B64.WatermarkColor;
		dataGridViewTextBoxDropDownEditingControl.WatermarkEnabled = _2B64.WatermarkEnabled;
		dataGridViewTextBoxDropDownEditingControl.WatermarkFont = _2B64.WatermarkFont;
		dataGridViewTextBoxDropDownEditingControl.WatermarkText = _2B64.WatermarkText;
		((TextBoxBase)dataGridViewTextBoxDropDownEditingControl.TextBox).Multiline = (int)dataGridViewCellStyle.WrapMode == 1;
		dataGridViewTextBoxDropDownEditingControl.BackgroundStyle.ApplyStyle(_2B64.BackgroundStyle);
		dataGridViewTextBoxDropDownEditingControl.BackgroundStyle.Class = _2B64.BackgroundStyle.Class;
		_2B64.ButtonClear.ᡄ(dataGridViewTextBoxDropDownEditingControl.ButtonClear);
		_2B64.ButtonCustom.ᡄ(dataGridViewTextBoxDropDownEditingControl.ButtonCustom);
		_2B64.ButtonCustom2.ᡄ(dataGridViewTextBoxDropDownEditingControl.ButtonCustom2);
		_2B64.ButtonDropDown.ᡄ(dataGridViewTextBoxDropDownEditingControl.ButtonDropDown);
		dataGridViewTextBoxDropDownEditingControl.ButtonClearClick += _៦;
		dataGridViewTextBoxDropDownEditingControl.ButtonCustomClick += _181E;
		dataGridViewTextBoxDropDownEditingControl.ButtonCustom2Click += _181F;
		dataGridViewTextBoxDropDownEditingControl.ButtonDropDownClick += _៧;
		((Control)dataGridViewTextBoxDropDownEditingControl).KeyDown += new KeyEventHandler(ᵢ);
		((Control)dataGridViewTextBoxDropDownEditingControl).Text = ((initialFormattedValue != null && initialFormattedValue != Convert.DBNull) ? Convert.ToString(initialFormattedValue) : "");
	}

	public override void DetachEditingControl()
	{
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		if (((DataGridViewElement)this).DataGridView != null && ((DataGridViewElement)this).DataGridView.EditingControl != null && ((DataGridViewElement)this).DataGridView.EditingControl is DataGridViewTextBoxDropDownEditingControl dataGridViewTextBoxDropDownEditingControl)
		{
			dataGridViewTextBoxDropDownEditingControl.ButtonClearClick -= _៦;
			dataGridViewTextBoxDropDownEditingControl.ButtonCustomClick -= _181E;
			dataGridViewTextBoxDropDownEditingControl.ButtonCustom2Click -= _181F;
			dataGridViewTextBoxDropDownEditingControl.ButtonDropDownClick -= _៧;
			((Control)dataGridViewTextBoxDropDownEditingControl).KeyDown -= new KeyEventHandler(ᵢ);
		}
		((DataGridViewTextBoxCell)this).DetachEditingControl();
	}

	private void _៦(object _٠, CancelEventArgs ؿ)
	{
		((DataGridViewTextBoxDropDownColumn)(object)((DataGridViewCell)this).OwningColumn)._2B31(_٠, ؿ);
	}

	private void _181E(object _٠, EventArgs ؿ)
	{
		((DataGridViewTextBoxDropDownColumn)(object)((DataGridViewCell)this).OwningColumn)._2B32(_٠, ؿ);
	}

	private void _181F(object _٠, EventArgs ؿ)
	{
		((DataGridViewTextBoxDropDownColumn)(object)((DataGridViewCell)this).OwningColumn)._2B33(_٠, ؿ);
	}

	private void _៧(object _٠, CancelEventArgs ؿ)
	{
		((DataGridViewTextBoxDropDownColumn)(object)((DataGridViewCell)this).OwningColumn)._2B34(_٠, ؿ);
	}

	private void ᵢ(object _٠, KeyEventArgs ؿ)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected I4, but got Unknown
		if (((DataGridViewElement)this).DataGridView != null && ((DataGridViewElement)this).DataGridView.EditingControl != null && ((DataGridViewElement)this).DataGridView.EditingControl is DataGridViewTextBoxDropDownEditingControl dataGridViewTextBoxDropDownEditingControl)
		{
			PostMessage(((Control)dataGridViewTextBoxDropDownEditingControl.TextBox).Handle, 256, (int)ؿ.KeyCode, 1);
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
		DataGridViewTextBoxDropDownColumn dataGridViewTextBoxDropDownColumn = (DataGridViewTextBoxDropDownColumn)(object)((DataGridViewCell)this).OwningColumn;
		Bitmap val = dataGridViewTextBoxDropDownColumn._2B01(cellBounds);
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
		DataGridViewTextBoxDropDownColumn dataGridViewTextBoxDropDownColumn = (DataGridViewTextBoxDropDownColumn)(object)((DataGridViewCell)this).OwningColumn;
		TextBoxDropDown _2B64 = dataGridViewTextBoxDropDownColumn._2B64;
		Point currentCellAddress = ((DataGridViewElement)this).DataGridView.CurrentCellAddress;
		if ((currentCellAddress.X != ((DataGridViewCell)this).ColumnIndex || currentCellAddress.Y != _2AEA || ((DataGridViewElement)this).DataGridView.EditingControl == null) && _2AEA < ((DataGridViewElement)this).DataGridView.RowCount && _2AED(_2AEB, (DataGridViewPaintParts)8))
		{
			_2AE9.X = 0;
			_2AE9.Y = 0;
			_2AE9.Width -= ((DataGridViewColumn)dataGridViewTextBoxDropDownColumn).DividerWidth + 1;
			_2AE9.Height--;
			((Control)_2B64).Font = _2AE6.Font;
			((Control)_2B64).ForeColor = _2AE6.ForeColor;
			((Control)_2B64).BackColor = _2AE6.BackColor;
			_2B64.TextAlign = _2B4B(_2AE6.Alignment);
			((Control)_2B64).Text = _239B(ԝ);
			dataGridViewTextBoxDropDownColumn._2B02(_2AEA, ((DataGridViewCell)this).ColumnIndex);
			Rectangle ल = _2B05(_2AE9, _2AE6);
			if (!dataGridViewTextBoxDropDownColumn.DisplayControlForCurrentCellOnly)
			{
				_2B15(_2B64, _2AE6, ल, _2ADA, _٥);
			}
			else
			{
				_0EE6(_2B64, _2AE6, ल, _٥);
			}
		}
	}

	private void _2B15(TextBoxDropDown ឡ, DataGridViewCellStyle _2AE6, Rectangle ल, Bitmap _2ADA, Graphics _٥)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		if (ឡ._28C0.Items.Count > 0)
		{
			Bitmap val = new Bitmap((Image)(object)_2ADA);
			try
			{
				((Control)ឡ).Bounds = ल;
				((Control)ឡ).DrawToBitmap(val, ल);
				foreach (VisualItem item in ឡ._28C0.Items)
				{
					if (item.Visible)
					{
						Rectangle renderBounds = item.RenderBounds;
						renderBounds.X += ल.X;
						renderBounds.Y += ल.Y;
						_٥.DrawImage((Image)(object)val, renderBounds, renderBounds, (GraphicsUnit)2);
						if (renderBounds.Left < ल.Right)
						{
							ल.Width -= ल.Right - renderBounds.Left - 1;
						}
					}
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		_0EE6(ឡ, _2AE6, ल, _٥);
	}

	private void _0EE6(TextBoxDropDown ឡ, DataGridViewCellStyle _2AE6, Rectangle ल, Graphics _٥)
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
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Invalid comparison between Unknown and I4
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Invalid comparison between Unknown and I4
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected I4, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Invalid comparison between Unknown and I4
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Invalid comparison between Unknown and I4
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
				goto IL_0084;
			case 2:
				goto IL_008e;
			}
			if ((int)alignment == 256)
			{
				goto IL_0088;
			}
		}
		else if ((int)alignment == 512 || (int)alignment == 1024)
		{
			goto IL_0088;
		}
		goto IL_008e;
		IL_0092:
		TextDrawing.DrawString(_٥, ((Control)ឡ).Text, ((Control)ឡ).Font, ((Control)ឡ).ForeColor, ल, eTextFormat);
		return;
		IL_008e:
		eTextFormat |= eTextFormat.VerticalCenter;
		goto IL_0092;
		IL_0084:
		eTextFormat = eTextFormat;
		goto IL_0092;
		IL_0088:
		eTextFormat |= eTextFormat.Bottom;
		goto IL_0092;
	}

	private Rectangle _2AEE(Rectangle _2AE9, DataGridViewAdvancedBorderStyle _2AEF)
	{
		DataGridViewTextBoxDropDownColumn dataGridViewTextBoxDropDownColumn = (DataGridViewTextBoxDropDownColumn)(object)((DataGridViewCell)this).OwningColumn;
		Rectangle rectangle = ((DataGridViewCell)this).BorderWidths(_2AEF);
		_2AE9.Offset(rectangle.X, rectangle.Y);
		_2AE9.Width -= rectangle.Right;
		_2AE9.Height -= rectangle.Bottom;
		if (((DataGridViewCell)this).Selected)
		{
			_2AE9.Width += ((DataGridViewColumn)dataGridViewTextBoxDropDownColumn).DividerWidth;
		}
		return _2AE9;
	}

	private Rectangle _2B1B(Rectangle _2AE9)
	{
		DataGridViewTextBoxDropDownColumn dataGridViewTextBoxDropDownColumn = (DataGridViewTextBoxDropDownColumn)(object)((DataGridViewCell)this).OwningColumn;
		Size preferredSize = ((Control)dataGridViewTextBoxDropDownColumn._2B64).PreferredSize;
		_2AE9.Location = new Point(1, 1);
		_2AE9.Width -= ((DataGridViewColumn)dataGridViewTextBoxDropDownColumn).DividerWidth;
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
