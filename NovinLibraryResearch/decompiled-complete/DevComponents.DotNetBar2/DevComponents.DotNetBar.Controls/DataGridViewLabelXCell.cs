using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls;

public class DataGridViewLabelXCell : DataGridViewButtonCell
{
	private enum _2B50
	{
		_2B51,
		_0711,
		ܐ
	}

	private DataGridViewCellStyle __2B44;

	public override Type EditType => null;

	public override Type FormattedValueType => typeof(string);

	private Rectangle _2B05(Rectangle _2B06, DataGridViewCellStyle _2AE6, int _2AEA)
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
		Rectangle contentBounds = ((DataGridViewCell)this).GetContentBounds(_2AEA);
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
							_2B06.X += (_2B06.Width - contentBounds.Width) / 2;
							_2B06.Y += (_2B06.Height - contentBounds.Height) / 2;
						}
					}
					else
					{
						_2B06.Y += (_2B06.Height - contentBounds.Height) / 2;
					}
					break;
				case 0:
					_2B06.X += (_2B06.Width - contentBounds.Width) / 2;
					break;
				case 2:
					_2B06.X = _2B06.Right - contentBounds.Width;
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
						_2B06.Y += _2B06.Height - contentBounds.Height;
					}
				}
				else
				{
					_2B06.X = _2B06.Right - contentBounds.Width;
					_2B06.Y += (_2B06.Height - contentBounds.Height) / 2;
				}
			}
			else if ((int)alignment != 512)
			{
				if ((int)alignment == 1024)
				{
					_2B06.X = _2B06.Right - contentBounds.Width;
					_2B06.Y += _2B06.Height - contentBounds.Height;
				}
			}
			else
			{
				_2B06.X += (_2B06.Width - contentBounds.Width) / 2;
				_2B06.Y += _2B06.Height - contentBounds.Height;
			}
		}
		if (_2B06.Y < 1)
		{
			_2B06.Y = 1;
		}
		_2B06.Width = Math.Max(1, contentBounds.Width);
		_2B06.Height = Math.Max(1, contentBounds.Height);
		return _2B06;
	}

	protected override Size GetPreferredSize(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
	{
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Expected O, but got Unknown
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		Size result = Size.Empty;
		if (((DataGridViewElement)this).DataGridView == null)
		{
			return new Size(-1, -1);
		}
		if (((DataGridViewCell)this).OwningColumn is DataGridViewLabelXColumn { _2B4D: var _2B4D } dataGridViewLabelXColumn)
		{
			if (!((Control)dataGridViewLabelXColumn._2B4D).IsHandleCreated)
			{
				if (((Control)dataGridViewLabelXColumn._2B4D).Parent == null)
				{
					Form val = ((Control)((DataGridViewElement)dataGridViewLabelXColumn).DataGridView).FindForm();
					if (val != null)
					{
						((Control)dataGridViewLabelXColumn._2B4D).Parent = (Control)(object)val;
					}
				}
				((Control)dataGridViewLabelXColumn._2B4D).Visible = true;
				((Control)dataGridViewLabelXColumn._2B4D).Visible = false;
			}
			_2B50 _2B50 = _2B45(constraintSize);
			if (_2B50 != _2B50.ܐ)
			{
				constraintSize.Width -= 2;
			}
			((Control)_2B4D).Font = cellStyle.Font;
			_2B4D.WordWrap = _2B49(dataGridViewLabelXColumn, cellStyle);
			_2B4D.TextAlignment = _2B4B(cellStyle.Alignment);
			((Control)_2B4D).MaximumSize = Size.Empty;
			_2B4D.Image = dataGridViewLabelXColumn.Image;
			string text2 = (((Control)_2B4D).Text = " ");
			string text3 = text2;
			if (rowIndex < ((DataGridViewElement)this).DataGridView.RowCount)
			{
				text3 = _239B(((DataGridViewElement)this).DataGridView.Rows[rowIndex].Cells[((DataGridViewCell)this).ColumnIndex].Value);
			}
			if (string.IsNullOrEmpty(text3))
			{
				text3 = " ";
			}
			((Control)_2B4D).Text = text3;
			result = ((Control)_2B4D).GetPreferredSize(constraintSize);
			Rectangle rectangle = ((DataGridViewCell)this).BorderWidths(new DataGridViewAdvancedBorderStyle());
			int num = rectangle.Left + rectangle.Width;
			Padding padding = cellStyle.Padding;
			int num2 = num + ((Padding)(ref padding)).Horizontal;
			int num3 = rectangle.Top + rectangle.Height;
			Padding padding2 = cellStyle.Padding;
			int num4 = num3 + ((Padding)(ref padding2)).Vertical;
			switch (_2B50)
			{
			case _2B50._0711:
				result.Width = 0;
				break;
			case _2B50.ܐ:
				result.Height = 0;
				break;
			}
			if (_2B50 != _2B50._0711)
			{
				result.Width += num2 + 4;
			}
			if (_2B50 != _2B50.ܐ)
			{
				result.Height += num4 + 2;
			}
		}
		return result;
	}

	private _2B50 _2B45(Size _2B46)
	{
		if (_2B46.Width == 0)
		{
			if (_2B46.Height == 0)
			{
				return _2B50._2B51;
			}
			return _2B50.ܐ;
		}
		return _2B50._0711;
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
		DataGridViewLabelXColumn dataGridViewLabelXColumn = (DataGridViewLabelXColumn)(object)((DataGridViewCell)this).OwningColumn;
		Bitmap val = dataGridViewLabelXColumn._2B01(cellBounds);
		if (val != null)
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				_2B47(val2, cellStyle, rectangle);
				_2B48(cellBounds, rowIndex, formattedValue, cellStyle, paintParts, val2);
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

	private void _2B47(Graphics _٥, DataGridViewCellStyle _2AE6, Rectangle _2AE7)
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

	private void _2B48(Rectangle _2AE9, int _2AEA, object ԝ, DataGridViewCellStyle _2AE6, DataGridViewPaintParts _2AEB, Graphics _٥)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		DataGridViewLabelXColumn dataGridViewLabelXColumn = (DataGridViewLabelXColumn)(object)((DataGridViewCell)this).OwningColumn;
		LabelX _2B4D = dataGridViewLabelXColumn._2B4D;
		__2B44 = _2AE6;
		string text = _239B(ԝ);
		dataGridViewLabelXColumn._2B4C = true;
		bool wordWrap = _2B4D.WordWrap;
		StringAlignment textAlignment = _2B4D.TextAlignment;
		GraphicsState val = _٥.Save();
		try
		{
			_2AE9.X = 0;
			_2AE9.Y = 0;
			_2AE9.Width -= ((DataGridViewColumn)dataGridViewLabelXColumn).DividerWidth + 1;
			_2AE9.Height--;
			((Control)_2B4D).Font = _2AE6.Font;
			((Control)_2B4D).ForeColor = _2AE6.ForeColor;
			((Control)_2B4D).BackColor = (((DataGridViewCell)this).Selected ? Color.Transparent : _2AE6.BackColor);
			_2B4D.WordWrap = _2B49(dataGridViewLabelXColumn, _2AE6);
			_2B4D.TextAlignment = _2B4B(_2AE6.Alignment);
			Rectangle bounds = _2AE9;
			if (_2AEA < ((DataGridViewElement)this).DataGridView.RowCount)
			{
				((Control)_2B4D).Text = text;
				if (_2AED(_2AEB, (DataGridViewPaintParts)8))
				{
					dataGridViewLabelXColumn._2B02(_2AEA, ((DataGridViewCell)this).ColumnIndex);
				}
				bounds = _2B05(_2AE9, _2AE6, _2AEA);
			}
			else
			{
				((Control)_2B4D).Text = " ";
			}
			_2B4D.CallBasePaintBackground = false;
			_٥.TranslateTransform((float)bounds.X, (float)bounds.Y);
			((Control)_2B4D).Bounds = bounds;
			_2B4D.RecalcLayout();
			_2B4D.ઔ(new PaintEventArgs(_٥, Rectangle.Empty));
		}
		finally
		{
			_2B4D.WordWrap = wordWrap;
			_2B4D.TextAlignment = textAlignment;
			dataGridViewLabelXColumn._2B4C = false;
			_٥.Restore(val);
		}
	}

	private bool _2B49(DataGridViewLabelXColumn _2B07, DataGridViewCellStyle _2AE6)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected I4, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Invalid comparison between Unknown and I4
		if (_2B07.WordWrap)
		{
			return true;
		}
		DataGridViewTriState wrapMode = _2AE6.WrapMode;
		return (wrapMode - 1) switch
		{
			0 => true, 
			1 => false, 
			_ => (int)((DataGridViewElement)this).DataGridView.DefaultCellStyle.WrapMode == 1, 
		};
	}

	protected override void OnMouseEnter(int rowIndex)
	{
		((DataGridViewCell)this).OnMouseEnter(rowIndex);
		DataGridViewLabelXColumn dataGridViewLabelXColumn = (DataGridViewLabelXColumn)(object)((DataGridViewCell)this).OwningColumn;
		LabelX _2B4D = dataGridViewLabelXColumn._2B4D;
		_2B4D._2288.InternalMouseEnter();
	}

	protected override void OnMouseLeave(int rowIndex)
	{
		((DataGridViewButtonCell)this).OnMouseLeave(rowIndex);
		DataGridViewLabelXColumn dataGridViewLabelXColumn = (DataGridViewLabelXColumn)(object)((DataGridViewCell)this).OwningColumn;
		LabelX _2B4D = dataGridViewLabelXColumn._2B4D;
		_2B4D._2288.InternalMouseLeave();
	}

	protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		((DataGridViewButtonCell)this).OnMouseMove(e);
		if (((DataGridViewCell)this).OwningColumn is DataGridViewLabelXColumn dataGridViewLabelXColumn && (uint)e.RowIndex < ((DataGridViewElement)this).DataGridView.Rows.Count)
		{
			Point point = _2B4A(e);
			dataGridViewLabelXColumn._2B4D._2288.InternalMouseMove(new MouseEventArgs(((MouseEventArgs)e).Button, ((MouseEventArgs)e).Clicks, point.X, point.Y, ((MouseEventArgs)e).Delta));
		}
	}

	protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		((DataGridViewButtonCell)this).OnMouseDown(e);
		if (((DataGridViewCell)this).OwningColumn is DataGridViewLabelXColumn dataGridViewLabelXColumn)
		{
			Point point = _2B4A(e);
			dataGridViewLabelXColumn._2B4D._2288.InternalMouseDown(new MouseEventArgs(((MouseEventArgs)e).Button, ((MouseEventArgs)e).Clicks, point.X, point.Y, ((MouseEventArgs)e).Delta));
			_2B09(e.ColumnIndex, e.RowIndex);
		}
	}

	protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		((DataGridViewButtonCell)this).OnMouseUp(e);
		if (((DataGridViewCell)this).OwningColumn is DataGridViewLabelXColumn dataGridViewLabelXColumn)
		{
			Point point = _2B4A(e);
			dataGridViewLabelXColumn._2B4D._2288.InternalMouseUp(new MouseEventArgs(((MouseEventArgs)e).Button, ((MouseEventArgs)e).Clicks, point.X, point.Y, ((MouseEventArgs)e).Delta));
			_2B09(e.ColumnIndex, e.RowIndex);
		}
	}

	private Point _2B4A(DataGridViewCellMouseEventArgs ؿ)
	{
		Rectangle cellDisplayRectangle = ((DataGridViewElement)this).DataGridView.GetCellDisplayRectangle(ؿ.ColumnIndex, ؿ.RowIndex, true);
		cellDisplayRectangle.Location = new Point(0, 0);
		Rectangle rectangle = _2B05(cellDisplayRectangle, __2B44, ؿ.RowIndex);
		return new Point(((MouseEventArgs)ؿ).X - rectangle.X, ((MouseEventArgs)ؿ).Y - rectangle.Y);
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
		DataGridViewLabelXColumn dataGridViewLabelXColumn = (DataGridViewLabelXColumn)(object)((DataGridViewCell)this).OwningColumn;
		Rectangle rectangle = ((DataGridViewCell)this).BorderWidths(_2AEF);
		_2AE9.Offset(rectangle.X, rectangle.Y);
		_2AE9.Width -= rectangle.Right;
		_2AE9.Height -= rectangle.Bottom;
		if (((DataGridViewCell)this).Selected)
		{
			_2AE9.Width += ((DataGridViewColumn)dataGridViewLabelXColumn).DividerWidth;
		}
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

	private StringAlignment _2B4B(DataGridViewContentAlignment _2B2C)
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
		return (StringAlignment)2;
		IL_003d:
		return (StringAlignment)0;
		IL_0039:
		return (StringAlignment)1;
	}

	internal void _2B09(int _21A5, int _2AEA)
	{
		((DataGridViewElement)this).DataGridView.InvalidateCell(_21A5, _2AEA);
	}
}
