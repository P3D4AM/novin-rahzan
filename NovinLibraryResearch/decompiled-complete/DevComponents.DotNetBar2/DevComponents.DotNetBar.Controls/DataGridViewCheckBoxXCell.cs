using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls;

public class DataGridViewCheckBoxXCell : DataGridViewCell
{
	private bool __2B04;

	private string _ኚ;

	public override Type EditType => null;

	public override Type FormattedValueType => typeof(string);

	private Rectangle _2B05(Rectangle _2B06, DataGridViewCellStyle _2AE6)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Invalid comparison between Unknown and I4
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Invalid comparison between Unknown and I4
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected I4, but got Unknown
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Invalid comparison between Unknown and I4
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Invalid comparison between Unknown and I4
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Invalid comparison between Unknown and I4
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Invalid comparison between Unknown and I4
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Invalid comparison between Unknown and I4
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Invalid comparison between Unknown and I4
		_2B06.X++;
		_2B06.Y++;
		_2B06.Width = Math.Max(0, _2B06.Width - 2);
		_2B06.Height = Math.Max(0, _2B06.Height - 2);
		Rectangle rectangle = _2B08(_2B06);
		if (_2AE6 != null)
		{
			DataGridViewContentAlignment alignment = _2AE6.Alignment;
			if ((int)alignment <= 32)
			{
				switch (alignment - 2)
				{
				default:
					if ((int)alignment != 16)
					{
						if ((int)alignment == 32)
						{
							_2B06.X += (_2B06.Width - rectangle.Width) / 2;
							_2B06.Y += (_2B06.Height - rectangle.Height) / 2;
						}
					}
					else
					{
						_2B06.Y += (_2B06.Height - rectangle.Height) / 2;
					}
					break;
				case 0:
					_2B06.X += (_2B06.Width - rectangle.Width) / 2;
					break;
				case 2:
					_2B06.X = _2B06.Right - rectangle.Width;
					break;
				case 1:
					break;
				}
			}
			else if ((int)alignment <= 256)
			{
				if ((int)alignment != 64)
				{
					if ((int)alignment == 256)
					{
						_2B06.Y += _2B06.Height - rectangle.Height;
					}
				}
				else
				{
					_2B06.X = _2B06.Right - rectangle.Width;
					_2B06.Y += (_2B06.Height - rectangle.Height) / 2;
				}
			}
			else if ((int)alignment != 512)
			{
				if ((int)alignment == 1024)
				{
					_2B06.X = _2B06.Right - rectangle.Width;
					_2B06.Y += _2B06.Height - rectangle.Height;
				}
			}
			else
			{
				_2B06.X += (_2B06.Width - rectangle.Width) / 2;
				_2B06.Y += _2B06.Height - rectangle.Height;
			}
		}
		if (_2B06.Y < 1)
		{
			_2B06.Y = 1;
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
		Size empty = Size.Empty;
		if (constraintSize.Width == 0 && ((DataGridViewCell)this).OwningColumn is DataGridViewCheckBoxXColumn dataGridViewCheckBoxXColumn)
		{
			((DataGridViewCell)this).GetFormattedValue(((DataGridViewCell)this).GetValue(rowIndex), rowIndex, ref cellStyle, (TypeConverter)null, (TypeConverter)null, (DataGridViewDataErrorContexts)0);
			if (rowIndex < ((DataGridViewElement)this).DataGridView.RowCount)
			{
				dataGridViewCheckBoxXColumn._2B02(rowIndex, ((DataGridViewCell)this).ColumnIndex);
			}
			CheckBoxX _2B0A = dataGridViewCheckBoxXColumn._2B0A;
			empty.Width = (int)graphics.MeasureString(((Control)_2B0A).Text, cellStyle.Font).Width + 4;
			empty.Height = ((Control)_2B0A).Height;
			empty.Width += _2B0A._23F5.CheckSignSize.Width;
		}
		return empty;
	}

	protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		return ((DataGridViewCell)this).GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
	}

	protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
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
		DataGridViewCheckBoxXColumn dataGridViewCheckBoxXColumn = (DataGridViewCheckBoxXColumn)(object)((DataGridViewCell)this).OwningColumn;
		Bitmap val = dataGridViewCheckBoxXColumn._2B01(cellBounds);
		if (val != null)
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				_2AE5(val2, cellStyle, rectangle);
				_2AE8(cellBounds, rowIndex, value, cellStyle, paintParts, val2);
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

	private void _2AE5(Graphics _٥, DataGridViewCellStyle _2AE6, Rectangle _2AE7)
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

	private void _2AE8(Rectangle _2AE9, int _2AEA, object ԝ, DataGridViewCellStyle _2AE6, DataGridViewPaintParts _2AEB, Graphics _٥)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Expected O, but got Unknown
		DataGridViewCheckBoxXColumn dataGridViewCheckBoxXColumn = (DataGridViewCheckBoxXColumn)(object)((DataGridViewCell)this).OwningColumn;
		CheckBoxX _2B0A = dataGridViewCheckBoxXColumn._2B0A;
		string text = ((Control)_2B0A).Text;
		bool @checked = _2B0A.Checked;
		object checkValue = _2B0A.CheckValue;
		CheckState checkState = _2B0A.CheckState;
		bool isMouseOver = _2B0A._23F5.IsMouseOver;
		bool isMouseDown = _2B0A._23F5.IsMouseDown;
		GraphicsState val = _٥.Save();
		try
		{
			dataGridViewCheckBoxXColumn._2AF7 = true;
			_2AE9.X = 0;
			_2AE9.Y = 0;
			_2AE9.Width -= ((DataGridViewColumn)dataGridViewCheckBoxXColumn).DividerWidth + 1;
			_2AE9.Height--;
			_2B0A.CheckValue = (dataGridViewCheckBoxXColumn.ThreeState ? ԝ : ((object)_2403(dataGridViewCheckBoxXColumn, ԝ)));
			((Control)_2B0A).Font = _2AE6.Font;
			((Control)_2B0A).ForeColor = _2AE6.ForeColor;
			((Control)_2B0A).BackColor = (((DataGridViewCell)this).Selected ? Color.Transparent : _2AE6.BackColor);
			if (_2AEA != dataGridViewCheckBoxXColumn._2AF3)
			{
				_2B0A._23F5.IsMouseOver = false;
				_2B0A._23F5.IsMouseDown = false;
			}
			if (_2AEA < ((DataGridViewElement)this).DataGridView.RowCount)
			{
				((Control)_2B0A).Text = text;
				if (_2AED(_2AEB, (DataGridViewPaintParts)8))
				{
					dataGridViewCheckBoxXColumn._2B02(_2AEA, ((DataGridViewCell)this).ColumnIndex);
				}
			}
			else
			{
				((Control)_2B0A).Text = "";
			}
			_ኚ = ((Control)_2B0A).Text;
			Rectangle bounds = _2B05(_2AE9, _2AE6);
			_2B0A.CallBasePaintBackground = false;
			_٥.TranslateTransform((float)bounds.X, (float)bounds.Y);
			((Control)_2B0A).Bounds = bounds;
			_2B0A.RecalcLayout();
			_2B0A.ઔ(new PaintEventArgs(_٥, Rectangle.Empty));
		}
		finally
		{
			((Control)_2B0A).Text = text;
			if (_2B0A.ThreeState)
			{
				_2B0A.CheckValue = checkValue;
			}
			_2B0A.Checked = @checked;
			_2B0A.CheckState = checkState;
			_2B0A._23F5.IsMouseOver = isMouseOver;
			_2B0A._23F5.IsMouseDown = isMouseDown;
			_٥.Restore(val);
			dataGridViewCheckBoxXColumn._2AF7 = false;
		}
	}

	private bool _2403(DataGridViewCheckBoxXColumn _2B07, object ԝ)
	{
		if (ԝ == null || ԝ == DBNull.Value)
		{
			return false;
		}
		Type type = ԝ.GetType();
		if ((object)type == typeof(bool))
		{
			return (bool)ԝ;
		}
		if ((object)type == typeof(string))
		{
			string text = (string)ԝ;
			if (_2B07.CheckValueChecked is string && text.Equals(_2B07.CheckValueChecked))
			{
				return true;
			}
			if (_2B07.CheckValueUnchecked is string && text.Equals(_2B07.CheckValueUnchecked))
			{
				return false;
			}
		}
		else
		{
			int num = Convert.ToInt32(ԝ);
			if (num == 1)
			{
				return true;
			}
		}
		return false;
	}

	protected override void OnMouseEnter(int rowIndex)
	{
		((DataGridViewCell)this).OnMouseEnter(rowIndex);
		if (((DataGridViewCell)this).OwningColumn is DataGridViewCheckBoxXColumn dataGridViewCheckBoxXColumn && ((DataGridViewElement)this).DataGridView != null)
		{
			__2B04 = false;
			dataGridViewCheckBoxXColumn._2AF3 = rowIndex;
			((DataGridViewCell)this).ToolTipText = _ኚ;
			_2B09(((DataGridViewCell)this).ColumnIndex, rowIndex);
		}
	}

	protected override void OnMouseLeave(int rowIndex)
	{
		((DataGridViewCell)this).OnMouseLeave(rowIndex);
		if (((DataGridViewCell)this).OwningColumn is DataGridViewCheckBoxXColumn dataGridViewCheckBoxXColumn && ((DataGridViewElement)this).DataGridView != null)
		{
			dataGridViewCheckBoxXColumn._2AF3 = -1;
			if (__2B04)
			{
				__2B04 = false;
				dataGridViewCheckBoxXColumn._2B0A._23F5.InternalMouseLeave();
			}
			_2B09(((DataGridViewCell)this).ColumnIndex, rowIndex);
		}
	}

	protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
	{
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Expected O, but got Unknown
		((DataGridViewCell)this).OnMouseMove(e);
		if (!(((DataGridViewCell)this).OwningColumn is DataGridViewCheckBoxXColumn dataGridViewCheckBoxXColumn) || ((DataGridViewElement)this).DataGridView == null || ((DataGridViewCell)this).ReadOnly || ((DataGridViewBand)dataGridViewCheckBoxXColumn).ReadOnly || ((DataGridViewElement)this).DataGridView.ReadOnly)
		{
			return;
		}
		Rectangle cellDisplayRectangle = ((DataGridViewElement)this).DataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
		cellDisplayRectangle.X = 0;
		cellDisplayRectangle.Y = 0;
		DataGridViewCellStyle inheritedStyle = ((DataGridViewBand)((DataGridViewElement)this).DataGridView.Columns[e.ColumnIndex]).InheritedStyle;
		Rectangle rectangle = _2B05(cellDisplayRectangle, inheritedStyle);
		Point pt = new Point(((MouseEventArgs)e).X - rectangle.X, ((MouseEventArgs)e).Y - rectangle.Y);
		if (dataGridViewCheckBoxXColumn._2B0A._23F5.DisplayRectangle.Contains(pt))
		{
			if (!__2B04)
			{
				__2B04 = true;
				dataGridViewCheckBoxXColumn._2B0A._23F5.InternalMouseEnter();
			}
		}
		else if (__2B04)
		{
			__2B04 = false;
			dataGridViewCheckBoxXColumn._2B0A._23F5.InternalMouseLeave();
		}
		dataGridViewCheckBoxXColumn._2B0A._23F5.InternalMouseMove(new MouseEventArgs(((MouseEventArgs)e).Button, ((MouseEventArgs)e).Clicks, pt.X, pt.Y, ((MouseEventArgs)e).Delta));
		_2B09(e.ColumnIndex, e.RowIndex);
	}

	protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
	{
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Expected O, but got Unknown
		((DataGridViewCell)this).OnMouseDown(e);
		if (__2B04 && ((DataGridViewCell)this).OwningColumn is DataGridViewCheckBoxXColumn dataGridViewCheckBoxXColumn && !((DataGridViewBand)dataGridViewCheckBoxXColumn).ReadOnly && ((DataGridViewElement)this).DataGridView != null)
		{
			dataGridViewCheckBoxXColumn._2B0B = e.RowIndex;
			Rectangle cellDisplayRectangle = ((DataGridViewElement)this).DataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
			cellDisplayRectangle.X = 0;
			cellDisplayRectangle.Y = 0;
			DataGridViewCellStyle inheritedStyle = ((DataGridViewBand)((DataGridViewElement)this).DataGridView.Columns[e.ColumnIndex]).InheritedStyle;
			Rectangle rectangle = _2B05(cellDisplayRectangle, inheritedStyle);
			Point pt = new Point(((MouseEventArgs)e).X - rectangle.X, ((MouseEventArgs)e).Y - rectangle.Y);
			if (dataGridViewCheckBoxXColumn._2B0A._23F5.DisplayRectangle.Contains(pt))
			{
				dataGridViewCheckBoxXColumn._2B0A.CheckValue = ((DataGridViewCell)this).GetValue(e.RowIndex);
				dataGridViewCheckBoxXColumn._2B0A._23F5.InternalMouseDown(new MouseEventArgs(((MouseEventArgs)e).Button, ((MouseEventArgs)e).Clicks, pt.X, pt.Y, ((MouseEventArgs)e).Delta));
				_2B09(e.ColumnIndex, e.RowIndex);
			}
		}
	}

	protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)
	{
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected O, but got Unknown
		((DataGridViewCell)this).OnMouseUp(e);
		if (!(((DataGridViewCell)this).OwningColumn is DataGridViewCheckBoxXColumn dataGridViewCheckBoxXColumn) || ((DataGridViewBand)dataGridViewCheckBoxXColumn).ReadOnly || ((DataGridViewElement)this).DataGridView == null)
		{
			return;
		}
		Rectangle cellDisplayRectangle = ((DataGridViewElement)this).DataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
		cellDisplayRectangle.X = 0;
		cellDisplayRectangle.Y = 0;
		DataGridViewCellStyle inheritedStyle = ((DataGridViewBand)((DataGridViewElement)this).DataGridView.Columns[e.ColumnIndex]).InheritedStyle;
		Rectangle rectangle = _2B05(cellDisplayRectangle, inheritedStyle);
		Point point = new Point(((MouseEventArgs)e).X - rectangle.X, ((MouseEventArgs)e).Y - rectangle.Y);
		dataGridViewCheckBoxXColumn._2B0A._23F5.InternalMouseUp(new MouseEventArgs(((MouseEventArgs)e).Button, ((MouseEventArgs)e).Clicks, point.X, point.Y, ((MouseEventArgs)e).Delta));
		if (e.RowIndex == dataGridViewCheckBoxXColumn._2B0B)
		{
			if (rectangle.Contains(((MouseEventArgs)e).Location))
			{
				((DataGridViewCell)this).Value = dataGridViewCheckBoxXColumn._2B0A.CheckValue;
			}
			_2B09(e.ColumnIndex, e.RowIndex);
		}
		dataGridViewCheckBoxXColumn._2B0B = -1;
	}

	private bool _2AED(DataGridViewPaintParts _2AEB, DataGridViewPaintParts ᝑ)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return (DataGridViewPaintParts)(_2AEB & ᝑ) == ᝑ;
	}

	private Rectangle _2AEE(Rectangle _2AE9, DataGridViewAdvancedBorderStyle _2AEF)
	{
		DataGridViewCheckBoxXColumn dataGridViewCheckBoxXColumn = (DataGridViewCheckBoxXColumn)(object)((DataGridViewCell)this).OwningColumn;
		Rectangle rectangle = ((DataGridViewCell)this).BorderWidths(_2AEF);
		_2AE9.Offset(rectangle.X, rectangle.Y);
		_2AE9.Width -= rectangle.Right;
		_2AE9.Height -= rectangle.Bottom;
		if (((DataGridViewCell)this).Selected)
		{
			_2AE9.Width += ((DataGridViewColumn)dataGridViewCheckBoxXColumn).DividerWidth;
		}
		return _2AE9;
	}

	private Rectangle _2B08(Rectangle _2AE9)
	{
		if (((DataGridViewCell)this).OwningColumn is DataGridViewCheckBoxXColumn dataGridViewCheckBoxXColumn)
		{
			if (((Control)dataGridViewCheckBoxXColumn._2B0A).Parent == null)
			{
				Form val = ((Control)((DataGridViewElement)dataGridViewCheckBoxXColumn).DataGridView).FindForm();
				if (val != null)
				{
					((Control)dataGridViewCheckBoxXColumn._2B0A).Parent = (Control)(object)val;
				}
			}
			dataGridViewCheckBoxXColumn._2B0A._23F5.RecalcSize();
			_2AE9.Width = Math.Min(dataGridViewCheckBoxXColumn._2B0A._23F5.WidthInternal, _2AE9.Width);
			_2AE9.Width -= ((DataGridViewColumn)dataGridViewCheckBoxXColumn).DividerWidth + 3;
			_2AE9.Height = dataGridViewCheckBoxXColumn._2B0A._23F5.HeightInternal;
		}
		return _2AE9;
	}

	private void _2B09(int _21A5, int _2AEA)
	{
		if (((DataGridViewElement)this).DataGridView != null)
		{
			((DataGridViewElement)this).DataGridView.InvalidateCell(_21A5, _2AEA);
		}
	}
}
