using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls;

public class DataGridViewSliderCell : DataGridViewTextBoxCell
{
	private DataGridViewCellStyle __2B44;

	public override Type EditType => null;

	public override Type ValueType => ((DataGridViewTextBoxCell)this).ValueType ?? typeof(int);

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
		Rectangle rectangle = _2B08(_2B06);
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
		Size result = new Size(-1, -1);
		if (((DataGridViewElement)this).DataGridView != null && ((DataGridViewCell)this).OwningColumn is DataGridViewSliderColumn { _2B5F: var _2B5F })
		{
			result.Height = ((Control)_2B5F).Height;
			if (constraintSize.Width == 0)
			{
				result.Width += 80;
				if (_2B5F.LabelVisible)
				{
					result.Width += _2B5F.LabelWidth;
				}
			}
		}
		return result;
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
		DataGridViewSliderColumn dataGridViewSliderColumn = (DataGridViewSliderColumn)(object)((DataGridViewCell)this).OwningColumn;
		Bitmap val = dataGridViewSliderColumn._2B01(cellBounds);
		if (val != null)
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				_2B59(val2, cellStyle, rectangle);
				_2B5A(cellBounds, rowIndex, formattedValue, cellStyle, paintParts, val2);
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

	private void _2B59(Graphics _٥, DataGridViewCellStyle _2AE6, Rectangle _2AE7)
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

	private void _2B5A(Rectangle _2AE9, int _2AEA, object ԝ, DataGridViewCellStyle _2AE6, DataGridViewPaintParts _2AEB, Graphics _٥)
	{
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Expected O, but got Unknown
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		DataGridViewSliderColumn dataGridViewSliderColumn = (DataGridViewSliderColumn)(object)((DataGridViewCell)this).OwningColumn;
		Slider _2B5F = dataGridViewSliderColumn._2B5F;
		__2B44 = _2AE6;
		int value = _2B5F.Value;
		eSliderPart ᘅ = _2B5F._251F.ᘅ;
		eSliderPart mouseDownPart = _2B5F._251F.MouseDownPart;
		GraphicsState val = _٥.Save();
		try
		{
			_2AE9.X = 0;
			_2AE9.Y = 0;
			_2AE9.Width -= ((DataGridViewColumn)dataGridViewSliderColumn).DividerWidth + 1;
			_2AE9.Height--;
			if (_2AEA != dataGridViewSliderColumn._2AF3)
			{
				_2B5F._251F.ᘅ = eSliderPart.None;
				_2B5F._251F.MouseDownPart = eSliderPart.None;
			}
			((Control)_2B5F).Font = _2AE6.Font;
			((Control)_2B5F).ForeColor = _2AE6.ForeColor;
			((Control)_2B5F).BackColor = (((DataGridViewCell)this).Selected ? Color.Transparent : _2AE6.BackColor);
			if (_2AEA < ((DataGridViewElement)this).DataGridView.RowCount)
			{
				((Control)_2B5F).Text = dataGridViewSliderColumn.Text;
				if (_2AEA != dataGridViewSliderColumn._2AF3)
				{
					_2B5F.Value = _2B5C(ԝ);
				}
				if (_2AED(_2AEB, (DataGridViewPaintParts)8))
				{
					dataGridViewSliderColumn._2B02(_2AEA, ((DataGridViewCell)this).ColumnIndex);
				}
			}
			else
			{
				((Control)_2B5F).Text = "";
				_2B5F.Value = _2B5C(ԝ);
			}
			Rectangle bounds = _2B05(_2AE9, _2AE6);
			_٥.TranslateTransform((float)bounds.X, (float)bounds.Y);
			_2B5F.CallBasePaintBackground = false;
			((Control)_2B5F).Bounds = bounds;
			_2B5F.ઔ(new PaintEventArgs(_٥, Rectangle.Empty));
		}
		finally
		{
			_٥.Restore(val);
			_2B5F.Value = value;
			_2B5F._251F.ᘅ = ᘅ;
			_2B5F._251F.MouseDownPart = mouseDownPart;
		}
	}

	protected override void OnMouseEnter(int rowIndex)
	{
		((DataGridViewCell)this).OnMouseEnter(rowIndex);
		_2AEC(rowIndex, _2B5B: true);
	}

	private void _2AEC(int _2AEA, bool _2B5B)
	{
		DataGridViewSliderColumn dataGridViewSliderColumn = (DataGridViewSliderColumn)(object)((DataGridViewCell)this).OwningColumn;
		Slider _2B5F = dataGridViewSliderColumn._2B5F;
		if (((DataGridViewCell)this).Visible)
		{
			dataGridViewSliderColumn._2AF3 = _2AEA;
			_2B5F.Value = _2B5C(((DataGridViewCell)this).Value);
			_2B5F._251F.InternalMouseEnter();
			if (_2B5B)
			{
				_2B5D(((DataGridViewCell)this).ColumnIndex, _2AEA);
			}
		}
	}

	protected override void OnMouseLeave(int rowIndex)
	{
		((DataGridViewCell)this).OnMouseLeave(rowIndex);
		DataGridViewSliderColumn dataGridViewSliderColumn = (DataGridViewSliderColumn)(object)((DataGridViewCell)this).OwningColumn;
		Slider _2B5F = dataGridViewSliderColumn._2B5F;
		if (dataGridViewSliderColumn._2AF3 >= 0)
		{
			dataGridViewSliderColumn._2AF3 = -1;
			_2B5F._251F.InternalMouseLeave();
			((DataGridViewCell)this).Value = _2B5F.Value;
			_2B5D(((DataGridViewCell)this).ColumnIndex, rowIndex);
		}
	}

	protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		((DataGridViewCell)this).OnMouseMove(e);
		if (((DataGridViewCell)this).OwningColumn is DataGridViewSliderColumn dataGridViewSliderColumn)
		{
			if (dataGridViewSliderColumn._2AF3 == -1)
			{
				_2AEC(e.RowIndex, _2B5B: false);
			}
			if (dataGridViewSliderColumn._2AF3 >= 0)
			{
				Point point = _2B4A(((MouseEventArgs)e).X, ((MouseEventArgs)e).Y);
				dataGridViewSliderColumn._2B5F._251F.InternalMouseMove(new MouseEventArgs(((MouseEventArgs)e).Button, ((MouseEventArgs)e).Clicks, point.X, point.Y, ((MouseEventArgs)e).Delta));
				_2B5D(e.ColumnIndex, e.RowIndex);
			}
		}
	}

	protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		((DataGridViewCell)this).OnMouseDown(e);
		if (((DataGridViewCell)this).OwningColumn is DataGridViewSliderColumn dataGridViewSliderColumn)
		{
			Point point = _2B4A(((MouseEventArgs)e).X, ((MouseEventArgs)e).Y);
			dataGridViewSliderColumn._2B5F._251F.InternalMouseDown(new MouseEventArgs(((MouseEventArgs)e).Button, ((MouseEventArgs)e).Clicks, point.X, point.Y, ((MouseEventArgs)e).Delta));
			_2B5D(e.ColumnIndex, e.RowIndex);
		}
	}

	protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		((DataGridViewCell)this).OnMouseUp(e);
		if (((DataGridViewCell)this).OwningColumn is DataGridViewSliderColumn dataGridViewSliderColumn)
		{
			Point point = _2B4A(((MouseEventArgs)e).X, ((MouseEventArgs)e).Y);
			dataGridViewSliderColumn._2B5F._251F.InternalMouseUp(new MouseEventArgs(((MouseEventArgs)e).Button, ((MouseEventArgs)e).Clicks, point.X, point.Y, ((MouseEventArgs)e).Delta));
			((DataGridViewCell)this).Value = dataGridViewSliderColumn._2B5F.Value;
			_2B5D(e.ColumnIndex, e.RowIndex);
		}
	}

	private Point _2B4A(int و, int ى)
	{
		return new Point(و, ى - _2B05(((DataGridViewCell)this).ContentBounds, __2B44).Y);
	}

	internal int _2B5C(object ԝ)
	{
		if (ԝ == Convert.DBNull || (ԝ is string && string.IsNullOrEmpty((string)ԝ)))
		{
			return 0;
		}
		return Convert.ToInt32(ԝ);
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
		DataGridViewSliderColumn dataGridViewSliderColumn = (DataGridViewSliderColumn)(object)((DataGridViewCell)this).OwningColumn;
		Rectangle rectangle = ((DataGridViewCell)this).BorderWidths(_2AEF);
		_2AE9.Offset(rectangle.X, rectangle.Y);
		_2AE9.Width -= rectangle.Right;
		_2AE9.Height -= rectangle.Bottom;
		if (((DataGridViewCell)this).Selected)
		{
			_2AE9.Width += ((DataGridViewColumn)dataGridViewSliderColumn).DividerWidth;
		}
		return _2AE9;
	}

	private Rectangle _2B08(Rectangle _2AE9)
	{
		DataGridViewSliderColumn dataGridViewSliderColumn = (DataGridViewSliderColumn)(object)((DataGridViewCell)this).OwningColumn;
		if (((Control)dataGridViewSliderColumn._2B5F).Parent == null)
		{
			Form val = ((Control)((DataGridViewElement)dataGridViewSliderColumn).DataGridView).FindForm();
			if (val != null)
			{
				((Control)dataGridViewSliderColumn._2B5F).Parent = (Control)(object)val;
			}
		}
		dataGridViewSliderColumn._2B5F._251F.RecalcSize();
		_2AE9.Width -= ((DataGridViewColumn)dataGridViewSliderColumn).DividerWidth + 3;
		_2AE9.Height = dataGridViewSliderColumn._2B5F._251F.HeightInternal;
		return _2AE9;
	}

	internal void _2B5D(int _21A5, int _2AEA)
	{
		if (((DataGridViewElement)this).DataGridView != null)
		{
			((DataGridViewElement)this).DataGridView.InvalidateCell(_21A5, _2AEA);
		}
	}
}
