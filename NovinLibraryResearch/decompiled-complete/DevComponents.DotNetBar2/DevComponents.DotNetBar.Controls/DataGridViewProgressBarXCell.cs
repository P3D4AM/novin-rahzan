using System;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls;

public class DataGridViewProgressBarXCell : DataGridViewButtonCell
{
	public override Type EditType => null;

	public override Type FormattedValueType => typeof(string);

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
		DataGridViewProgressBarXColumn dataGridViewProgressBarXColumn = (DataGridViewProgressBarXColumn)(object)((DataGridViewCell)this).OwningColumn;
		Bitmap val = dataGridViewProgressBarXColumn._2B01(cellBounds);
		if (val != null)
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				_2AE5(val2, cellStyle, rectangle);
				_2AE8(cellBounds, rowIndex, formattedValue, cellStyle, paintParts, val);
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

	private void _2AE8(Rectangle _2AE9, int _2AEA, object ԝ, DataGridViewCellStyle _2AE6, DataGridViewPaintParts _2AEB, Bitmap _2ADA)
	{
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		DataGridViewProgressBarXColumn dataGridViewProgressBarXColumn = (DataGridViewProgressBarXColumn)(object)((DataGridViewCell)this).OwningColumn;
		ProgressBarX _2B54 = dataGridViewProgressBarXColumn._2B54;
		Rectangle rectangle = _2B08(_2AE9, _2AF1: true);
		if (rectangle.Width <= 0 || rectangle.Height <= 0)
		{
			return;
		}
		string text = dataGridViewProgressBarXColumn.Text;
		dataGridViewProgressBarXColumn._2B4C = true;
		try
		{
			((Control)_2B54).Font = _2AE6.Font;
			((Control)_2B54).ForeColor = _2AE6.ForeColor;
			((Control)_2B54).BackColor = _2AE6.BackColor;
			_2B54.Value = _239B(ԝ, _2B54.Minimum);
			if (_2AEA < ((DataGridViewElement)this).DataGridView.RowCount)
			{
				((Control)_2B54).Text = text;
				if (_2AED(_2AEB, (DataGridViewPaintParts)8))
				{
					dataGridViewProgressBarXColumn._2B02(_2AEA, ((DataGridViewCell)this).ColumnIndex);
				}
			}
			else
			{
				((Control)_2B54).Text = "";
			}
			((Control)_2B54).Bounds = rectangle;
			_2B54.RecalcLayout();
			((Control)_2B54).DrawToBitmap(_2ADA, rectangle);
		}
		finally
		{
			dataGridViewProgressBarXColumn._2B4C = false;
		}
	}

	protected override void OnMouseEnter(int rowIndex)
	{
		((DataGridViewCell)this).OnMouseEnter(rowIndex);
		DataGridViewProgressBarXColumn dataGridViewProgressBarXColumn = (DataGridViewProgressBarXColumn)(object)((DataGridViewCell)this).OwningColumn;
		ProgressBarX _2B54 = dataGridViewProgressBarXColumn._2B54;
		_2B54._2B57.InternalMouseEnter();
	}

	protected override void OnMouseLeave(int rowIndex)
	{
		((DataGridViewButtonCell)this).OnMouseLeave(rowIndex);
		DataGridViewProgressBarXColumn dataGridViewProgressBarXColumn = (DataGridViewProgressBarXColumn)(object)((DataGridViewCell)this).OwningColumn;
		ProgressBarX _2B54 = dataGridViewProgressBarXColumn._2B54;
		_2B54._2B57.InternalMouseLeave();
	}

	protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		((DataGridViewButtonCell)this).OnMouseMove(e);
		DataGridViewProgressBarXColumn dataGridViewProgressBarXColumn = (DataGridViewProgressBarXColumn)(object)((DataGridViewCell)this).OwningColumn;
		ProgressBarX _2B54 = dataGridViewProgressBarXColumn._2B54;
		_2B54._2B57.InternalMouseMove(new MouseEventArgs(((MouseEventArgs)e).Button, ((MouseEventArgs)e).Clicks, ((MouseEventArgs)e).X, ((MouseEventArgs)e).Y, ((MouseEventArgs)e).Delta));
	}

	protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		((DataGridViewButtonCell)this).OnMouseDown(e);
		DataGridViewProgressBarXColumn dataGridViewProgressBarXColumn = (DataGridViewProgressBarXColumn)(object)((DataGridViewCell)this).OwningColumn;
		ProgressBarX _2B54 = dataGridViewProgressBarXColumn._2B54;
		Rectangle cellDisplayRectangle = ((DataGridViewElement)this).DataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
		Rectangle rectangle = _2B08(cellDisplayRectangle, _2AF1: false);
		Point location = ((MouseEventArgs)e).Location;
		location.Offset(rectangle.Location);
		if (rectangle.Contains(location))
		{
			((Control)_2B54).Bounds = rectangle;
			_2B54._2B57.InternalMouseDown(new MouseEventArgs(((MouseEventArgs)e).Button, ((MouseEventArgs)e).Clicks, ((MouseEventArgs)e).X, ((MouseEventArgs)e).Y, ((MouseEventArgs)e).Delta));
			((Control)((DataGridViewElement)this).DataGridView).Invalidate(rectangle);
		}
	}

	protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		((DataGridViewButtonCell)this).OnMouseUp(e);
		DataGridViewProgressBarXColumn dataGridViewProgressBarXColumn = (DataGridViewProgressBarXColumn)(object)((DataGridViewCell)this).OwningColumn;
		ProgressBarX _2B54 = dataGridViewProgressBarXColumn._2B54;
		Rectangle cellDisplayRectangle = ((DataGridViewElement)this).DataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
		Rectangle rectangle2 = (((Control)_2B54).Bounds = _2B08(cellDisplayRectangle, _2AF1: false));
		_2B54._2B57.InternalMouseUp(new MouseEventArgs(((MouseEventArgs)e).Button, ((MouseEventArgs)e).Clicks, ((MouseEventArgs)e).X, ((MouseEventArgs)e).Y, ((MouseEventArgs)e).Delta));
		((Control)((DataGridViewElement)this).DataGridView).Invalidate(rectangle2);
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
		DataGridViewProgressBarXColumn dataGridViewProgressBarXColumn = (DataGridViewProgressBarXColumn)(object)((DataGridViewCell)this).OwningColumn;
		Rectangle rectangle = ((DataGridViewCell)this).BorderWidths(_2AEF);
		_2AE9.Offset(rectangle.X, rectangle.Y);
		_2AE9.Width -= rectangle.Right;
		_2AE9.Height -= rectangle.Bottom;
		if (((DataGridViewCell)this).Selected)
		{
			_2AE9.Width += ((DataGridViewColumn)dataGridViewProgressBarXColumn).DividerWidth;
		}
		return _2AE9;
	}

	private Rectangle _2B08(Rectangle _2AE9, bool _2AF1)
	{
		DataGridViewProgressBarXColumn dataGridViewProgressBarXColumn = (DataGridViewProgressBarXColumn)(object)((DataGridViewCell)this).OwningColumn;
		_2AE9.Width -= ((DataGridViewColumn)dataGridViewProgressBarXColumn).DividerWidth + 3;
		_2AE9.Height -= 3;
		if (_2AF1)
		{
			_2AE9.Location = new Point(0, 0);
		}
		_2AE9.X++;
		_2AE9.Y++;
		return _2AE9;
	}

	private int _239B(object ԝ, int _2B53)
	{
		if (ԝ == Convert.DBNull || (ԝ is string && string.IsNullOrEmpty((string)ԝ)))
		{
			return _2B53;
		}
		return Convert.ToInt32(ԝ);
	}
}
