using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Media;
using System.Windows.Forms;
using DevComponents.Editors;

namespace DevComponents.DotNetBar.Controls;

public class DataGridViewDoubleInputCell : DataGridViewTextBoxCell
{
	public override Type EditType => typeof(DataGridViewDoubleInputEditingControl);

	public override Type FormattedValueType => typeof(string);

	public override Type ValueType => typeof(double);

	public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
	{
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		((DataGridViewCell)this).DetachEditingControl();
		((DataGridViewTextBoxCell)this).InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
		DataGridViewDoubleInputEditingControl dataGridViewDoubleInputEditingControl = (DataGridViewDoubleInputEditingControl)(object)((DataGridViewElement)this).DataGridView.EditingControl;
		if (((DataGridViewCell)this).OwningColumn is DataGridViewDoubleInputColumn { _2B37: var _2B37 })
		{
			dataGridViewDoubleInputEditingControl.Value = _239B(_2B37.Value);
			dataGridViewDoubleInputEditingControl.DisplayFormat = _2B37.DisplayFormat;
			((Control)dataGridViewDoubleInputEditingControl).Enabled = ((Control)_2B37).Enabled;
			dataGridViewDoubleInputEditingControl.Increment = _2B37.Increment;
			dataGridViewDoubleInputEditingControl.LockUpdateChecked = _2B37.LockUpdateChecked;
			dataGridViewDoubleInputEditingControl.MaxValue = _2B37.MaxValue;
			dataGridViewDoubleInputEditingControl.MinValue = _2B37.MinValue;
			dataGridViewDoubleInputEditingControl.ShowCheckBox = _2B37.ShowCheckBox;
			dataGridViewDoubleInputEditingControl.ShowUpDown = _2B37.ShowUpDown;
			dataGridViewDoubleInputEditingControl.BackgroundStyle.ApplyStyle(_2B37.BackgroundStyle);
			dataGridViewDoubleInputEditingControl.BackgroundStyle.Class = _2B37.BackgroundStyle.Class;
			dataGridViewDoubleInputEditingControl.InputHorizontalAlignment = _2B2B(dataGridViewCellStyle.Alignment);
			_2B37.ButtonClear.ᡄ(dataGridViewDoubleInputEditingControl.ButtonClear);
			_2B37.ButtonCustom.ᡄ(dataGridViewDoubleInputEditingControl.ButtonCustom);
			_2B37.ButtonCustom2.ᡄ(dataGridViewDoubleInputEditingControl.ButtonCustom2);
			_2B37.ButtonDropDown.ᡄ(dataGridViewDoubleInputEditingControl.ButtonDropDown);
			_2B37.ButtonFreeText.ᡄ(dataGridViewDoubleInputEditingControl.ButtonFreeText);
			dataGridViewDoubleInputEditingControl.ButtonClearClick += _៦;
			dataGridViewDoubleInputEditingControl.ButtonCustomClick += _181E;
			dataGridViewDoubleInputEditingControl.ButtonCustom2Click += _181F;
			dataGridViewDoubleInputEditingControl.ButtonDropDownClick += _៧;
			dataGridViewDoubleInputEditingControl.ButtonFreeTextClick += _17FC;
			dataGridViewDoubleInputEditingControl.ConvertFreeTextEntry += _17FB;
			dataGridViewDoubleInputEditingControl._2B2D = false;
		}
	}

	public override void DetachEditingControl()
	{
		if (((DataGridViewElement)this).DataGridView != null && ((DataGridViewElement)this).DataGridView.EditingControl != null && ((DataGridViewElement)this).DataGridView.EditingControl is DataGridViewDoubleInputEditingControl dataGridViewDoubleInputEditingControl)
		{
			dataGridViewDoubleInputEditingControl.ButtonClearClick -= _៦;
			dataGridViewDoubleInputEditingControl.ButtonDropDownClick -= _៧;
			dataGridViewDoubleInputEditingControl.ButtonFreeTextClick -= _17FC;
			dataGridViewDoubleInputEditingControl.ButtonCustomClick -= _181E;
			dataGridViewDoubleInputEditingControl.ButtonCustom2Click -= _181F;
			dataGridViewDoubleInputEditingControl.ConvertFreeTextEntry -= _17FB;
			if (dataGridViewDoubleInputEditingControl._2B2D)
			{
				dataGridViewDoubleInputEditingControl._2B2D = false;
				SystemSounds.Beep.Play();
			}
		}
		((DataGridViewTextBoxCell)this).DetachEditingControl();
	}

	private void _៦(object _٠, CancelEventArgs ؿ)
	{
		((DataGridViewDoubleInputColumn)(object)((DataGridViewCell)this).OwningColumn)._2B31(_٠, ؿ);
	}

	private void _181E(object _٠, EventArgs ؿ)
	{
		((DataGridViewDoubleInputColumn)(object)((DataGridViewCell)this).OwningColumn)._2B32(_٠, ؿ);
	}

	private void _181F(object _٠, EventArgs ؿ)
	{
		((DataGridViewDoubleInputColumn)(object)((DataGridViewCell)this).OwningColumn)._2B33(_٠, ؿ);
	}

	private void _៧(object _٠, CancelEventArgs ؿ)
	{
		((DataGridViewDoubleInputColumn)(object)((DataGridViewCell)this).OwningColumn)._2B34(_٠, ؿ);
	}

	private void _17FC(object _٠, CancelEventArgs ؿ)
	{
		((DataGridViewDoubleInputColumn)(object)((DataGridViewCell)this).OwningColumn)._2B35(_٠, ؿ);
	}

	private void _17FB(object _٠, FreeTextEntryConversionEventArgs ؿ)
	{
		((DataGridViewDoubleInputColumn)(object)((DataGridViewCell)this).OwningColumn)._2B36(_٠, ؿ);
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
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Invalid comparison between Unknown and I4
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Invalid comparison between Unknown and I4
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Invalid comparison between Unknown and I4
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Invalid comparison between Unknown and I4
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Invalid comparison between Unknown and I4
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Invalid comparison between Unknown and I4
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Invalid comparison between Unknown and I4
		_2B06.X++;
		_2B06.Y++;
		_2B06.Width = Math.Max(0, _2B06.Width - 2);
		_2B06.Height = Math.Max(0, _2B06.Height - 2);
		Rectangle rectangle = _2B1B(_2B06);
		if (_2AE6 != null && rectangle.Height < _2B06.Height)
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
		_2B06.Width = Math.Max(1, _2B06.Width);
		_2B06.Height = Math.Max(1, rectangle.Height);
		return _2B06;
	}

	protected override Size GetPreferredSize(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
	{
		if (((DataGridViewElement)this).DataGridView == null)
		{
			return new Size(-1, -1);
		}
		Size preferredSize = ((DataGridViewTextBoxCell)this).GetPreferredSize(graphics, cellStyle, rowIndex, constraintSize);
		if (((DataGridViewCell)this).OwningColumn is DataGridViewDoubleInputColumn { _2B37: var _2B37 })
		{
			preferredSize.Height = ((Control)_2B37).Height;
			if (constraintSize.Width == 0)
			{
				preferredSize.Width += _2B29(_2B37.ButtonClear);
				preferredSize.Width += _2B29(_2B37.ButtonCustom);
				preferredSize.Width += _2B29(_2B37.ButtonCustom2);
				preferredSize.Width += _2B29(_2B37.ButtonDropDown);
				preferredSize.Width += _2B29(_2B37.ButtonFreeText);
				if (_2B37.ShowUpDown)
				{
					preferredSize.Width += 16;
				}
				if (_2B37.ShowCheckBox)
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
		if (((DataGridViewCell)this).OwningColumn is DataGridViewDoubleInputColumn { _2B37: var _2B37 })
		{
			_2B37.Value = _239B(value);
			return ((Control)_2B37).Text;
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
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Invalid comparison between Unknown and I4
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
		DataGridViewDoubleInputColumn dataGridViewDoubleInputColumn = (DataGridViewDoubleInputColumn)(object)((DataGridViewCell)this).OwningColumn;
		Bitmap val = dataGridViewDoubleInputColumn._2B01(cellBounds);
		if (val != null)
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				_2B13(val2, cellStyle, rectangle);
				_2B14(val2, cellBounds, rowIndex, value, cellStyle, paintParts);
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

	private void _2B14(Graphics _٥, Rectangle _2AE9, int _2AEA, object ԝ, DataGridViewCellStyle _2AE6, DataGridViewPaintParts _2AEB)
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		DataGridViewDoubleInputColumn dataGridViewDoubleInputColumn = (DataGridViewDoubleInputColumn)(object)((DataGridViewCell)this).OwningColumn;
		DoubleInput _2B37 = dataGridViewDoubleInputColumn._2B37;
		Point currentCellAddress = ((DataGridViewElement)this).DataGridView.CurrentCellAddress;
		if ((currentCellAddress.X != ((DataGridViewCell)this).ColumnIndex || currentCellAddress.Y != _2AEA || ((DataGridViewElement)this).DataGridView.EditingControl == null) && _2AEA < ((DataGridViewElement)this).DataGridView.RowCount && _2AED(_2AEB, (DataGridViewPaintParts)8))
		{
			_2AE9.X = 0;
			_2AE9.Y = 0;
			_2AE9.Width -= ((DataGridViewColumn)dataGridViewDoubleInputColumn).DividerWidth + 1;
			_2AE9.Height--;
			((Control)_2B37).Font = _2AE6.Font;
			((Control)_2B37).ForeColor = _2AE6.ForeColor;
			((Control)_2B37).BackColor = (((DataGridViewCell)this).Selected ? Color.Transparent : _2AE6.BackColor);
			_2B37.BackgroundStyle.TextColor = _2AE6.ForeColor;
			_2B37.BackgroundStyle.BackColor = (((DataGridViewCell)this).Selected ? Color.Transparent : _2AE6.BackColor);
			_2B37.InputHorizontalAlignment = _2B2B(_2AE6.Alignment);
			_2B37.Value = _239B(ԝ);
			dataGridViewDoubleInputColumn._2B02(_2AEA, ((DataGridViewCell)this).ColumnIndex);
			Rectangle ल = _2B05(_2AE9, _2AE6);
			if (!dataGridViewDoubleInputColumn.DisplayControlForCurrentCellOnly)
			{
				_2B15(_2B37, ल, _٥);
			}
			else
			{
				_0EE6(_2B37, ल, _٥);
			}
		}
	}

	private void _2B15(DoubleInput ឡ, Rectangle ल, Graphics _٥)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		GraphicsState val = _٥.Save();
		try
		{
			_٥.TranslateTransform((float)ल.X, (float)ल.Y);
			((Control)ឡ).Width = ल.Width;
			ឡ.ઔ(new PaintEventArgs(_٥, Rectangle.Empty));
		}
		finally
		{
			_٥.Restore(val);
		}
	}

	private void _0EE6(DoubleInput ឡ, Rectangle ल, Graphics _٥)
	{
		ल.Inflate(-2, 0);
		eTextFormat eTextFormat = eTextFormat.VerticalCenter;
		switch (ឡ.InputHorizontalAlignment)
		{
		case eHorizontalAlignment.Center:
			eTextFormat |= eTextFormat.HorizontalCenter;
			break;
		case eHorizontalAlignment.Right:
			eTextFormat |= eTextFormat.Right;
			break;
		}
		TextDrawing.DrawString(_٥, ((Control)ឡ).Text, ((Control)ឡ).Font, ((Control)ឡ).ForeColor, ल, eTextFormat);
	}

	private Rectangle _2AEE(Rectangle _2AE9, DataGridViewAdvancedBorderStyle _2AEF)
	{
		DataGridViewDoubleInputColumn dataGridViewDoubleInputColumn = (DataGridViewDoubleInputColumn)(object)((DataGridViewCell)this).OwningColumn;
		Rectangle rectangle = ((DataGridViewCell)this).BorderWidths(_2AEF);
		_2AE9.Offset(rectangle.X, rectangle.Y);
		_2AE9.Width -= rectangle.Right;
		_2AE9.Height -= rectangle.Bottom;
		if (((DataGridViewCell)this).Selected)
		{
			_2AE9.Width += ((DataGridViewColumn)dataGridViewDoubleInputColumn).DividerWidth;
		}
		return _2AE9;
	}

	private Rectangle _2B1B(Rectangle _2AE9)
	{
		DataGridViewDoubleInputColumn dataGridViewDoubleInputColumn = (DataGridViewDoubleInputColumn)(object)((DataGridViewCell)this).OwningColumn;
		Size size = ((Control)dataGridViewDoubleInputColumn._2B37).Size;
		_2AE9.Location = new Point(1, 1);
		_2AE9.Width = Math.Min(size.Width, _2AE9.Width);
		_2AE9.Width -= ((DataGridViewColumn)dataGridViewDoubleInputColumn).DividerWidth;
		_2AE9.Height = size.Height;
		return _2AE9;
	}

	private double _239B(object ԝ)
	{
		if (ԝ == Convert.DBNull || (ԝ is string && string.IsNullOrEmpty((string)ԝ)))
		{
			return 0.0;
		}
		if (ԝ is string)
		{
			string text = ԝ.ToString();
			return Convert.ToDouble(text.Replace(" ", ""));
		}
		return Convert.ToDouble(ԝ);
	}

	private eHorizontalAlignment _2B2B(DataGridViewContentAlignment _2B2C)
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
		return eHorizontalAlignment.Right;
		IL_003d:
		return eHorizontalAlignment.Left;
		IL_0039:
		return eHorizontalAlignment.Center;
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
