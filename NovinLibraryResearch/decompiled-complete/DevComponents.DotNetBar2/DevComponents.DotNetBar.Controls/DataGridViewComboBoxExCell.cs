using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls;

public class DataGridViewComboBoxExCell : DataGridViewTextBoxCell
{
	private BindingContext _૨ = new BindingContext();

	public override Type EditType => typeof(DataGridViewComboBoxExEditingControl);

	public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Expected O, but got Unknown
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Expected O, but got Unknown
		((DataGridViewCell)this).DetachEditingControl();
		((DataGridViewTextBoxCell)this).InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
		DataGridViewComboBoxExEditingControl dataGridViewComboBoxExEditingControl = (DataGridViewComboBoxExEditingControl)(object)((DataGridViewElement)this).DataGridView.EditingControl;
		DataGridViewComboBoxExColumn dataGridViewComboBoxExColumn = (DataGridViewComboBoxExColumn)(object)((DataGridViewCell)this).OwningColumn;
		ComboBoxEx _2B1F = dataGridViewComboBoxExColumn._2B1F;
		((ComboBox)dataGridViewComboBoxExEditingControl).DropDownStyle = ((ComboBox)_2B1F).DropDownStyle;
		((ComboBox)dataGridViewComboBoxExEditingControl).AutoCompleteCustomSource = ((ComboBox)_2B1F).AutoCompleteCustomSource;
		((ComboBox)dataGridViewComboBoxExEditingControl).AutoCompleteMode = ((ComboBox)_2B1F).AutoCompleteMode;
		((ComboBox)dataGridViewComboBoxExEditingControl).AutoCompleteSource = ((ComboBox)_2B1F).AutoCompleteSource;
		((ComboBox)dataGridViewComboBoxExEditingControl).DrawMode = ((ComboBox)_2B1F).DrawMode;
		((ComboBox)dataGridViewComboBoxExEditingControl).DropDownHeight = ((ComboBox)_2B1F).DropDownHeight;
		((Control)dataGridViewComboBoxExEditingControl).Enabled = ((Control)_2B1F).Enabled;
		((ComboBox)dataGridViewComboBoxExEditingControl).FlatStyle = ((ComboBox)_2B1F).FlatStyle;
		dataGridViewComboBoxExEditingControl.FocusCuesEnabled = _2B1F.FocusCuesEnabled;
		((ListControl)dataGridViewComboBoxExEditingControl).FormatString = ((ListControl)_2B1F).FormatString;
		((ListControl)dataGridViewComboBoxExEditingControl).FormattingEnabled = ((ListControl)_2B1F).FormattingEnabled;
		dataGridViewComboBoxExEditingControl.Images = _2B1F.Images;
		((Control)dataGridViewComboBoxExEditingControl).ImeMode = ((Control)_2B1F).ImeMode;
		((ComboBox)dataGridViewComboBoxExEditingControl).IntegralHeight = ((ComboBox)_2B1F).IntegralHeight;
		dataGridViewComboBoxExEditingControl.IsStandalone = _2B1F.IsStandalone;
		((ComboBox)dataGridViewComboBoxExEditingControl).ItemHeight = ((ComboBox)_2B1F).ItemHeight;
		((ComboBox)dataGridViewComboBoxExEditingControl).MaxDropDownItems = ((ComboBox)_2B1F).MaxDropDownItems;
		((ComboBox)dataGridViewComboBoxExEditingControl).MaxLength = ((ComboBox)_2B1F).MaxLength;
		dataGridViewComboBoxExEditingControl.ᚩ = _2B1F.ᚩ;
		((Control)dataGridViewComboBoxExEditingControl).RightToLeft = ((Control)_2B1F).RightToLeft;
		dataGridViewComboBoxExEditingControl.Style = _2B1F.Style;
		dataGridViewComboBoxExEditingControl.WatermarkBehavior = _2B1F.WatermarkBehavior;
		dataGridViewComboBoxExEditingControl.WatermarkColor = _2B1F.WatermarkColor;
		dataGridViewComboBoxExEditingControl.WatermarkEnabled = _2B1F.WatermarkEnabled;
		dataGridViewComboBoxExEditingControl.WatermarkFont = _2B1F.WatermarkFont;
		dataGridViewComboBoxExEditingControl.WatermarkText = _2B1F.WatermarkText;
		((ComboBox)dataGridViewComboBoxExEditingControl).DataSource = ((ComboBox)_2B1F).DataSource;
		if (!(((ComboBox)dataGridViewComboBoxExEditingControl).DataSource is List<string>))
		{
			((ListControl)dataGridViewComboBoxExEditingControl).ValueMember = ((ListControl)_2B1F).ValueMember;
		}
		((ListControl)dataGridViewComboBoxExEditingControl).DisplayMember = ((ListControl)_2B1F).DisplayMember;
		if (((ComboBox)dataGridViewComboBoxExEditingControl).DataSource == null)
		{
			object[] array = new object[_2B1F.Items.Count];
			_2B1F.Items.CopyTo(array, 0);
			dataGridViewComboBoxExEditingControl.Items.Clear();
			dataGridViewComboBoxExEditingControl.Items.AddRange(array);
			((Control)dataGridViewComboBoxExEditingControl).Text = Convert.ToString(initialFormattedValue);
		}
		else
		{
			object value = ((DataGridViewCell)this).GetValue(rowIndex);
			object obj = _2B17(dataGridViewComboBoxExColumn, _2B1F, value);
			if (obj == null)
			{
				((ListControl)dataGridViewComboBoxExEditingControl).SelectedIndex = -1;
				((ListControl)dataGridViewComboBoxExEditingControl).SelectedIndex = -1;
			}
			else
			{
				((ComboBox)dataGridViewComboBoxExEditingControl).SelectedItem = obj;
			}
		}
		((Control)dataGridViewComboBoxExEditingControl).Click += દ;
		((ComboBox)dataGridViewComboBoxExEditingControl).DrawItem += new DrawItemEventHandler(_2B11);
		dataGridViewComboBoxExEditingControl.DropDownChange += ᚮ;
		((ComboBox)dataGridViewComboBoxExEditingControl).MeasureItem += new MeasureItemEventHandler(_2B12);
	}

	public override void DetachEditingControl()
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		if (((DataGridViewElement)this).DataGridView != null && ((DataGridViewElement)this).DataGridView.EditingControl != null && ((DataGridViewElement)this).DataGridView.EditingControl is DataGridViewComboBoxExEditingControl dataGridViewComboBoxExEditingControl)
		{
			((Control)dataGridViewComboBoxExEditingControl).Click -= દ;
			((ComboBox)dataGridViewComboBoxExEditingControl).DrawItem -= new DrawItemEventHandler(_2B11);
			dataGridViewComboBoxExEditingControl.DropDownChange -= ᚮ;
			((ComboBox)dataGridViewComboBoxExEditingControl).MeasureItem -= new MeasureItemEventHandler(_2B12);
		}
		((DataGridViewTextBoxCell)this).DetachEditingControl();
	}

	private void દ(object _٠, EventArgs ؿ)
	{
		((DataGridViewComboBoxExColumn)(object)((DataGridViewCell)this).OwningColumn)._2B25(_٠, ؿ);
	}

	private void _2B11(object _٠, DrawItemEventArgs ؿ)
	{
		((DataGridViewComboBoxExColumn)(object)((DataGridViewCell)this).OwningColumn)._2B26(_٠, ؿ);
	}

	private void ᚮ(object _٠, bool _211F)
	{
		((DataGridViewComboBoxExColumn)(object)((DataGridViewCell)this).OwningColumn)._2B27(_٠, new DropDownChangeEventArgs(((DataGridViewCell)this).RowIndex, ((DataGridViewCell)this).ColumnIndex, _211F));
	}

	private void _2B12(object _٠, MeasureItemEventArgs ؿ)
	{
		((DataGridViewComboBoxExColumn)(object)((DataGridViewCell)this).OwningColumn)._2B28(_٠, ؿ);
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
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Invalid comparison between Unknown and I4
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Invalid comparison between Unknown and I4
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Invalid comparison between Unknown and I4
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Invalid comparison between Unknown and I4
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Invalid comparison between Unknown and I4
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Invalid comparison between Unknown and I4
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Invalid comparison between Unknown and I4
		_2B06.X++;
		_2B06.Y++;
		_2B06.Width = Math.Max(0, _2B06.Width - 2);
		_2B06.Height = Math.Max(0, _2B06.Height - 2);
		Rectangle rectangle = _2B1B(_2B06);
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
		if (((DataGridViewCell)this).OwningColumn is DataGridViewComboBoxExColumn { _2B1F: var _2B1F })
		{
			preferredSize.Height = ((Control)_2B1F).Height;
			if (constraintSize.Width == 0)
			{
				Rectangle cellDisplayRectangle = ((DataGridViewElement)this).DataGridView.GetCellDisplayRectangle(((DataGridViewCell)this).ColumnIndex, rowIndex, true);
				preferredSize.Width += _2B1F._16EB(cellDisplayRectangle).Width;
			}
		}
		return preferredSize;
	}

	public override object ParseFormattedValue(object formattedValue, DataGridViewCellStyle cellStyle, TypeConverter formattedValueTypeConverter, TypeConverter valueTypeConverter)
	{
		DataGridViewComboBoxExColumn dataGridViewComboBoxExColumn = (DataGridViewComboBoxExColumn)(object)((DataGridViewCell)this).OwningColumn;
		ComboBoxEx _2B1F = dataGridViewComboBoxExColumn._2B1F;
		if (((ComboBox)_2B1F).DataSource != null)
		{
			return _2B18(dataGridViewComboBoxExColumn, _2B1F, formattedValue);
		}
		return ((DataGridViewCell)this).ParseFormattedValue(formattedValue, cellStyle, formattedValueTypeConverter, valueTypeConverter);
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
		DataGridViewComboBoxExColumn dataGridViewComboBoxExColumn = (DataGridViewComboBoxExColumn)(object)((DataGridViewCell)this).OwningColumn;
		Bitmap val = dataGridViewComboBoxExColumn._2B01(cellBounds);
		if (val != null)
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				_2B13(val2, cellStyle, rectangle);
				_2B14(val2, cellBounds, rowIndex, value, cellStyle, paintParts, val);
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
		DataGridViewComboBoxExColumn dataGridViewComboBoxExColumn = (DataGridViewComboBoxExColumn)(object)((DataGridViewCell)this).OwningColumn;
		ComboBoxEx _2B1F = dataGridViewComboBoxExColumn._2B1F;
		Point currentCellAddress = ((DataGridViewElement)this).DataGridView.CurrentCellAddress;
		if ((currentCellAddress.X != ((DataGridViewCell)this).ColumnIndex || currentCellAddress.Y != _2AEA || ((DataGridViewElement)this).DataGridView.EditingControl == null) && _2AEA < ((DataGridViewElement)this).DataGridView.RowCount && _2AED(_2AEB, (DataGridViewPaintParts)8))
		{
			_2AE9.X = 0;
			_2AE9.Y = 0;
			_2AE9.Width -= ((DataGridViewColumn)dataGridViewComboBoxExColumn).DividerWidth + 1;
			_2AE9.Height--;
			if (((ComboBox)_2B1F).DataSource == null)
			{
				((ListControl)_2B1F).DisplayMember = "Text";
				((ListControl)_2B1F).ValueMember = null;
			}
			else
			{
				ԝ = _2B16(dataGridViewComboBoxExColumn, _2B1F, ԝ);
				((Control)_2B1F).BindingContext = _૨;
			}
			((Control)_2B1F).Font = _2AE6.Font;
			((Control)_2B1F).ForeColor = _2AE6.ForeColor;
			((Control)_2B1F).BackColor = _2AE6.BackColor;
			string ٮ = ((ԝ == Convert.DBNull || string.IsNullOrEmpty(Convert.ToString(ԝ))) ? null : Convert.ToString(ԝ));
			dataGridViewComboBoxExColumn._2B02(_2AEA, ((DataGridViewCell)this).ColumnIndex);
			Rectangle ल = _2B05(_2AE9, _2AE6);
			if (!dataGridViewComboBoxExColumn.DisplayControlForCurrentCellOnly)
			{
				_2B15(_2B1F, ल, _2ADA, _٥, dataGridViewComboBoxExColumn, ٮ);
			}
			else
			{
				_0EE6(_2B1F, ल, _٥, ٮ);
			}
		}
	}

	private void _2B15(ComboBoxEx ឡ, Rectangle ल, Bitmap _2ADA, Graphics _٥, DataGridViewComboBoxExColumn _2B07, string ٮ)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		Rectangle rectangle = ឡ._16EB(new Rectangle(0, 0, ल.Width, ल.Height));
		if (!rectangle.IsEmpty)
		{
			if (_2B1A(((ComboBox)ឡ).DropDownStyle))
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
				rectangle.X += ल.X;
				rectangle.Y += ल.Y;
				_٥.DrawImage((Image)(object)val2, rectangle, rectangle, (GraphicsUnit)2);
				if (rectangle.Left < ल.Right)
				{
					ल.Width -= ल.Right - rectangle.Left - 3;
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			((Control)ឡ).Visible = false;
		}
		_0EE6(ឡ, ल, _٥, ٮ);
	}

	private void _0EE6(ComboBoxEx ឡ, Rectangle ल, Graphics _٥, string ٮ)
	{
		ल.Inflate(-2, 0);
		TextDrawing.DrawString(_٥, ٮ, ((Control)ឡ).Font, ((Control)ឡ).ForeColor, ल, eTextFormat.EndEllipsis | eTextFormat.NoPrefix | eTextFormat.VerticalCenter);
	}

	private object _2B16(DataGridViewComboBoxExColumn _2B07, ComboBoxEx ឡ, object _੬)
	{
		if (_੬ != null && !string.IsNullOrEmpty(((ListControl)ឡ).ValueMember) && !string.IsNullOrEmpty(((ListControl)ឡ).DisplayMember))
		{
			CurrencyManager val = _2B19(_2B07, ឡ);
			if (val != null)
			{
				string text = _੬.ToString();
				for (int i = 0; i < val.List.Count; i++)
				{
					if (val.List[i] is DataRowView)
					{
						DataRowView dataRowView = (DataRowView)val.List[i];
						string value = dataRowView[((ListControl)ឡ).ValueMember].ToString();
						if (text.Equals(value))
						{
							return dataRowView[((ListControl)ឡ).DisplayMember];
						}
					}
					else if (val.List[i] is string)
					{
						if (text.Equals(val.List[i]))
						{
							return _੬;
						}
					}
					else
					{
						object obj = val.List[i];
						object value2 = obj.GetType().GetProperty(((ListControl)ឡ).ValueMember).GetValue(obj, null);
						if (_੬.Equals(value2))
						{
							return obj.GetType().GetProperty(((ListControl)ឡ).DisplayMember).GetValue(obj, null);
						}
					}
				}
			}
		}
		if (_੬ != Convert.DBNull)
		{
			return _੬;
		}
		return "";
	}

	private object _2B17(DataGridViewComboBoxExColumn _2B07, ComboBoxEx ឡ, object _੬)
	{
		if (_੬ != null && !string.IsNullOrEmpty(((ListControl)ឡ).ValueMember) && !string.IsNullOrEmpty(((ListControl)ឡ).DisplayMember))
		{
			CurrencyManager val = _2B19(_2B07, ឡ);
			if (val != null)
			{
				string text = _੬.ToString();
				for (int i = 0; i < val.List.Count; i++)
				{
					if (val.List[i] is DataRowView)
					{
						DataRowView dataRowView = (DataRowView)val.List[i];
						string value = dataRowView[((ListControl)ឡ).ValueMember].ToString();
						if (text.Equals(value))
						{
							return dataRowView;
						}
					}
					else if (val.List[i] is string)
					{
						if (text.Equals(val.List[i]))
						{
							return _੬;
						}
					}
					else
					{
						object obj = val.List[i];
						object value2 = obj.GetType().GetProperty(((ListControl)ឡ).ValueMember).GetValue(obj, null);
						if (_੬.Equals(value2))
						{
							return obj;
						}
					}
				}
			}
		}
		if (_੬ != Convert.DBNull)
		{
			return _੬;
		}
		return null;
	}

	private object _2B18(DataGridViewComboBoxExColumn _2B07, ComboBoxEx ឡ, object _੬)
	{
		if (_੬ != null && !string.IsNullOrEmpty(((ListControl)ឡ).ValueMember) && !string.IsNullOrEmpty(((ListControl)ឡ).DisplayMember))
		{
			CurrencyManager val = _2B19(_2B07, ឡ);
			if (val != null)
			{
				string text = _੬.ToString();
				for (int i = 0; i < val.List.Count; i++)
				{
					if (val.List[i] is DataRowView)
					{
						DataRowView dataRowView = (DataRowView)val.List[i];
						string value = dataRowView[((ListControl)ឡ).DisplayMember].ToString();
						if (text.Equals(value))
						{
							return dataRowView[((ListControl)ឡ).ValueMember];
						}
					}
					else if (val.List[i] is string)
					{
						if (text.Equals(val.List[i]))
						{
							return _੬;
						}
					}
					else
					{
						object obj = val.List[i];
						object value2 = obj.GetType().GetProperty(((ListControl)ឡ).DisplayMember).GetValue(obj, null);
						if (value2 is string && text.Equals(value2))
						{
							return obj.GetType().GetProperty(((ListControl)ឡ).ValueMember).GetValue(obj, null);
						}
					}
				}
			}
		}
		return _੬;
	}

	private CurrencyManager _2B19(DataGridViewComboBoxExColumn _2B07, ComboBoxEx ឡ)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		if (_2B07._2B20 == null && ((DataGridViewElement)this).DataGridView != null && ((Control)((DataGridViewElement)this).DataGridView).BindingContext != null && ((ComboBox)ឡ).DataSource != null && ((ComboBox)ឡ).DataSource != Convert.DBNull)
		{
			_2B07._2B20 = (CurrencyManager)((Control)((DataGridViewElement)this).DataGridView).BindingContext[((ComboBox)ឡ).DataSource];
		}
		return _2B07._2B20;
	}

	private bool _2B1A(ComboBoxStyle _0657)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Invalid comparison between Unknown and I4
		if ((int)_0657 == 2)
		{
			return true;
		}
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

	protected override void OnMouseEnter(int rowIndex)
	{
		DataGridViewComboBoxExColumn dataGridViewComboBoxExColumn = (DataGridViewComboBoxExColumn)(object)((DataGridViewCell)this).OwningColumn;
		ComboBoxEx _2B1F = dataGridViewComboBoxExColumn._2B1F;
		if (((DataGridViewElement)this).DataGridView != null && (uint)((DataGridViewCell)this).RowIndex < ((DataGridViewElement)this).DataGridView.Rows.Count)
		{
			if (((ComboBox)_2B1F).DataSource != null)
			{
				((DataGridViewCell)this).ToolTipText = Convert.ToString(_2B16(dataGridViewComboBoxExColumn, _2B1F, ((DataGridViewCell)this).Value));
			}
			else
			{
				((DataGridViewCell)this).ToolTipText = Convert.ToString(((DataGridViewCell)this).Value);
			}
		}
		((DataGridViewCell)this).OnMouseEnter(rowIndex);
	}

	private Rectangle _2AEE(Rectangle _2AE9, DataGridViewAdvancedBorderStyle _2AEF)
	{
		DataGridViewComboBoxExColumn dataGridViewComboBoxExColumn = (DataGridViewComboBoxExColumn)(object)((DataGridViewCell)this).OwningColumn;
		Rectangle rectangle = ((DataGridViewCell)this).BorderWidths(_2AEF);
		_2AE9.Offset(rectangle.X, rectangle.Y);
		_2AE9.Width -= rectangle.Right;
		_2AE9.Height -= rectangle.Bottom;
		if (((DataGridViewCell)this).Selected)
		{
			_2AE9.Width += ((DataGridViewColumn)dataGridViewComboBoxExColumn).DividerWidth;
		}
		return _2AE9;
	}

	private Rectangle _2B1B(Rectangle _2AE9)
	{
		DataGridViewComboBoxExColumn dataGridViewComboBoxExColumn = (DataGridViewComboBoxExColumn)(object)((DataGridViewCell)this).OwningColumn;
		Size size = ((Control)dataGridViewComboBoxExColumn._2B1F).Size;
		_2AE9.Location = new Point(1, 1);
		_2AE9.Width -= ((DataGridViewColumn)dataGridViewComboBoxExColumn).DividerWidth;
		_2AE9.Height = Math.Min(size.Height, _2AE9.Height);
		return _2AE9;
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
