using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Media;
using System.Windows.Forms;
using DevComponents.Editors;
using DevComponents.Editors.DateTimeAdv;

namespace DevComponents.DotNetBar.Controls;

public class DataGridViewDateTimeInputCell : DataGridViewTextBoxCell
{
	public override Type EditType => typeof(DataGridViewDateTimeInputEditingControl);

	public override Type FormattedValueType => typeof(string);

	public override Type ValueType => typeof(DateTime);

	public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
	{
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		((DataGridViewCell)this).DetachEditingControl();
		((DataGridViewTextBoxCell)this).InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
		DataGridViewDateTimeInputEditingControl dataGridViewDateTimeInputEditingControl = (DataGridViewDateTimeInputEditingControl)(object)((DataGridViewElement)this).DataGridView.EditingControl;
		if (((DataGridViewCell)this).OwningColumn is DataGridViewDateTimeInputColumn { _2B2F: var _2B2F })
		{
			dataGridViewDateTimeInputEditingControl.AllowEmptyState = _2B2F.AllowEmptyState;
			dataGridViewDateTimeInputEditingControl.AutoAdvance = _2B2F.AutoAdvance;
			dataGridViewDateTimeInputEditingControl.AutoOverwrite = _2B2F.AutoOverwrite;
			dataGridViewDateTimeInputEditingControl.AutoSelectDate = _2B2F.AutoSelectDate;
			dataGridViewDateTimeInputEditingControl.CustomFormat = _2B2F.CustomFormat;
			dataGridViewDateTimeInputEditingControl.DefaultInputValues = _2B2F.DefaultInputValues;
			((Control)dataGridViewDateTimeInputEditingControl).Enabled = ((Control)_2B2F).Enabled;
			dataGridViewDateTimeInputEditingControl.Format = _2B2F.Format;
			dataGridViewDateTimeInputEditingControl.InputHorizontalAlignment = _2B2B(dataGridViewCellStyle.Alignment);
			dataGridViewDateTimeInputEditingControl.IsInputReadOnly = _2B2F.IsInputReadOnly;
			dataGridViewDateTimeInputEditingControl.LockUpdateChecked = _2B2F.LockUpdateChecked;
			dataGridViewDateTimeInputEditingControl.MinDate = _2B2F.MinDate;
			dataGridViewDateTimeInputEditingControl.MaxDate = _2B2F.MaxDate;
			dataGridViewDateTimeInputEditingControl.SelectNextInputCharacters = _2B2F.SelectNextInputCharacters;
			dataGridViewDateTimeInputEditingControl.ShowCheckBox = _2B2F.ShowCheckBox;
			dataGridViewDateTimeInputEditingControl.ShowUpDown = _2B2F.ShowUpDown;
			dataGridViewDateTimeInputEditingControl.BackgroundStyle.ApplyStyle(_2B2F.BackgroundStyle);
			dataGridViewDateTimeInputEditingControl.BackgroundStyle.Class = _2B2F.BackgroundStyle.Class;
			_2B2F.ButtonClear.ᡄ(dataGridViewDateTimeInputEditingControl.ButtonClear);
			_2B2F.ButtonDropDown.ᡄ(dataGridViewDateTimeInputEditingControl.ButtonDropDown);
			_2B2F.ButtonFreeText.ᡄ(dataGridViewDateTimeInputEditingControl.ButtonFreeText);
			_2B2F.ButtonCustom.ᡄ(dataGridViewDateTimeInputEditingControl.ButtonCustom);
			_2B2F.ButtonCustom2.ᡄ(dataGridViewDateTimeInputEditingControl.ButtonCustom2);
			_2B2F.MonthCalendar._0EBA(dataGridViewDateTimeInputEditingControl.MonthCalendar);
			dataGridViewDateTimeInputEditingControl.ButtonClearClick += _៦;
			dataGridViewDateTimeInputEditingControl.ButtonCustomClick += _181E;
			dataGridViewDateTimeInputEditingControl.ButtonCustom2Click += _181F;
			dataGridViewDateTimeInputEditingControl.ButtonDropDownClick += _៧;
			dataGridViewDateTimeInputEditingControl.ButtonFreeTextClick += _17FC;
			dataGridViewDateTimeInputEditingControl.ConvertFreeTextEntry += _17FB;
			dataGridViewDateTimeInputEditingControl.ValueObject = ((DataGridViewCell)this).Value;
			dataGridViewDateTimeInputEditingControl._2B2D = false;
		}
	}

	public override void DetachEditingControl()
	{
		if (((DataGridViewElement)this).DataGridView != null && ((DataGridViewElement)this).DataGridView.EditingControl != null && ((DataGridViewElement)this).DataGridView.EditingControl is DataGridViewDateTimeInputEditingControl dataGridViewDateTimeInputEditingControl)
		{
			dataGridViewDateTimeInputEditingControl.ButtonClearClick -= _៦;
			dataGridViewDateTimeInputEditingControl.ButtonCustomClick -= _181E;
			dataGridViewDateTimeInputEditingControl.ButtonCustom2Click -= _181F;
			dataGridViewDateTimeInputEditingControl.ButtonDropDownClick -= _៧;
			dataGridViewDateTimeInputEditingControl.ButtonFreeTextClick -= _17FC;
			dataGridViewDateTimeInputEditingControl.ConvertFreeTextEntry -= _17FB;
			if (dataGridViewDateTimeInputEditingControl._2B2D)
			{
				dataGridViewDateTimeInputEditingControl._2B2D = false;
				SystemSounds.Beep.Play();
			}
		}
		((DataGridViewTextBoxCell)this).DetachEditingControl();
	}

	private void _៦(object _٠, CancelEventArgs ؿ)
	{
		((DataGridViewDateTimeInputColumn)(object)((DataGridViewCell)this).OwningColumn)._2B31(_٠, ؿ);
	}

	private void _181E(object _٠, EventArgs ؿ)
	{
		((DataGridViewDateTimeInputColumn)(object)((DataGridViewCell)this).OwningColumn)._2B32(_٠, ؿ);
	}

	private void _181F(object _٠, EventArgs ؿ)
	{
		((DataGridViewDateTimeInputColumn)(object)((DataGridViewCell)this).OwningColumn)._2B33(_٠, ؿ);
	}

	private void _៧(object _٠, CancelEventArgs ؿ)
	{
		((DataGridViewDateTimeInputColumn)(object)((DataGridViewCell)this).OwningColumn)._2B34(_٠, ؿ);
	}

	private void _17FC(object _٠, CancelEventArgs ؿ)
	{
		((DataGridViewDateTimeInputColumn)(object)((DataGridViewCell)this).OwningColumn)._2B35(_٠, ؿ);
	}

	private void _17FB(object _٠, FreeTextEntryConversionEventArgs ؿ)
	{
		((DataGridViewDateTimeInputColumn)(object)((DataGridViewCell)this).OwningColumn)._2B36(_٠, ؿ);
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
		if (((DataGridViewCell)this).OwningColumn is DataGridViewDateTimeInputColumn { _2B2F: var _2B2F })
		{
			preferredSize.Width += 6;
			preferredSize.Height = ((Control)_2B2F).Height;
			if (constraintSize.Width == 0)
			{
				preferredSize.Width += _2B29(_2B2F.ButtonClear);
				preferredSize.Width += _2B29(_2B2F.ButtonCustom);
				preferredSize.Width += _2B29(_2B2F.ButtonCustom2);
				preferredSize.Width += _2B29(_2B2F.ButtonDropDown);
				preferredSize.Width += _2B29(_2B2F.ButtonFreeText);
				if (_2B2F.ShowUpDown)
				{
					preferredSize.Width += 16;
				}
				if (_2B2F.ShowCheckBox)
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
		if (((DataGridViewCell)this).OwningColumn is DataGridViewDateTimeInputColumn { _2B2F: var _2B2F })
		{
			_2B2F.Value = _239B(value);
			return ((Control)_2B2F).Text;
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
		DataGridViewDateTimeInputColumn dataGridViewDateTimeInputColumn = (DataGridViewDateTimeInputColumn)(object)((DataGridViewCell)this).OwningColumn;
		Bitmap val = dataGridViewDateTimeInputColumn._2B01(cellBounds);
		if (val != null)
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				_2B13(val2, cellStyle, rectangle);
				_2B14(val2, cellBounds, rowIndex, formattedValue, cellStyle, paintParts);
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
		DataGridViewDateTimeInputColumn dataGridViewDateTimeInputColumn = (DataGridViewDateTimeInputColumn)(object)((DataGridViewCell)this).OwningColumn;
		DateTimeInput _2B2F = dataGridViewDateTimeInputColumn._2B2F;
		Point currentCellAddress = ((DataGridViewElement)this).DataGridView.CurrentCellAddress;
		if ((currentCellAddress.X != ((DataGridViewCell)this).ColumnIndex || currentCellAddress.Y != _2AEA || ((DataGridViewElement)this).DataGridView.EditingControl == null) && _2AEA < ((DataGridViewElement)this).DataGridView.RowCount && _2AED(_2AEB, (DataGridViewPaintParts)8))
		{
			_2AE9.X = 0;
			_2AE9.Y = 0;
			_2AE9.Width -= ((DataGridViewColumn)dataGridViewDateTimeInputColumn).DividerWidth + 1;
			_2AE9.Height--;
			((Control)_2B2F).Font = _2AE6.Font;
			((Control)_2B2F).ForeColor = _2AE6.ForeColor;
			((Control)_2B2F).BackColor = (((DataGridViewCell)this).Selected ? Color.Transparent : _2AE6.BackColor);
			_2B2F.BackgroundStyle.TextColor = _2AE6.ForeColor;
			_2B2F.BackgroundStyle.BackColor = (((DataGridViewCell)this).Selected ? Color.Transparent : _2AE6.BackColor);
			_2B2F.InputHorizontalAlignment = _2B2B(_2AE6.Alignment);
			_2B2F.Value = _239B(ԝ);
			dataGridViewDateTimeInputColumn._2B02(_2AEA, ((DataGridViewCell)this).ColumnIndex);
			Rectangle ल = _2B05(_2AE9, _2AE6);
			if (!dataGridViewDateTimeInputColumn.DisplayControlForCurrentCellOnly)
			{
				_2B15(_2B2F, ल, _٥);
			}
			else
			{
				_0EE6(_2B2F, ल, _٥);
			}
		}
	}

	private void _2B15(DateTimeInput ឡ, Rectangle ल, Graphics _٥)
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

	private void _0EE6(DateTimeInput ឡ, Rectangle ल, Graphics _٥)
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
		DataGridViewDateTimeInputColumn dataGridViewDateTimeInputColumn = (DataGridViewDateTimeInputColumn)(object)((DataGridViewCell)this).OwningColumn;
		Rectangle rectangle = ((DataGridViewCell)this).BorderWidths(_2AEF);
		_2AE9.Offset(rectangle.X, rectangle.Y);
		_2AE9.Width -= rectangle.Right;
		_2AE9.Height -= rectangle.Bottom;
		if (((DataGridViewCell)this).Selected)
		{
			_2AE9.Width += ((DataGridViewColumn)dataGridViewDateTimeInputColumn).DividerWidth;
		}
		return _2AE9;
	}

	private Rectangle _2B1B(Rectangle _2AE9)
	{
		DataGridViewDateTimeInputColumn dataGridViewDateTimeInputColumn = (DataGridViewDateTimeInputColumn)(object)((DataGridViewCell)this).OwningColumn;
		Size size = ((Control)dataGridViewDateTimeInputColumn._2B2F).Size;
		_2AE9.Location = new Point(1, 1);
		_2AE9.Width = Math.Min(size.Width, _2AE9.Width);
		_2AE9.Width -= ((DataGridViewColumn)dataGridViewDateTimeInputColumn).DividerWidth;
		_2AE9.Height = size.Height;
		return _2AE9;
	}

	private DateTime _239B(object ԝ)
	{
		if (ԝ == Convert.DBNull || (ԝ is string && string.IsNullOrEmpty((string)ԝ)))
		{
			return DateTime.MinValue;
		}
		if (ԝ is DateTime)
		{
			return Convert.ToDateTime(ԝ);
		}
		if (ԝ is string && DateTimeInput.CurrentCulture != null)
		{
			DateTime result = default(DateTime);
			if (DateTime.TryParse(ԝ.ToString(), DateTimeInput.GetActiveCulture().DateTimeFormat, DateTimeStyles.None, out result))
			{
				return result;
			}
		}
		return Convert.ToDateTime(ԝ);
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
