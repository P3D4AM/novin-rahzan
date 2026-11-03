using System;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls;

public class DataGridViewButtonXCell : DataGridViewButtonCell
{
	public override Type EditType => null;

	public override Type FormattedValueType => typeof(string);

	protected override Rectangle GetContentBounds(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex)
	{
		Rectangle cellDisplayRectangle = ((DataGridViewElement)this).DataGridView.GetCellDisplayRectangle(((DataGridViewCell)this).ColumnIndex, rowIndex, false);
		cellDisplayRectangle.Location = new Point(0, 0);
		DataGridViewButtonXColumn dataGridViewButtonXColumn = (DataGridViewButtonXColumn)(object)((DataGridViewCell)this).OwningColumn;
		ButtonX _2AF4 = dataGridViewButtonXColumn._2AF4;
		Rectangle result = _2AF0(cellDisplayRectangle, _2AF1: true);
		if (_2AF4.ButtonItem.SubItems.Count > 0)
		{
			result.Width -= _2AF4.ButtonItem.SubItemsExpandWidth;
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
		DataGridViewButtonXColumn dataGridViewButtonXColumn = (DataGridViewButtonXColumn)(object)((DataGridViewCell)this).OwningColumn;
		Bitmap val = dataGridViewButtonXColumn._2B01(cellBounds);
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
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		DataGridViewButtonXColumn dataGridViewButtonXColumn = (DataGridViewButtonXColumn)(object)((DataGridViewCell)this).OwningColumn;
		ButtonX _2AF4 = dataGridViewButtonXColumn._2AF4;
		Rectangle rectangle = _2AF0(_2AE9, _2AF1: true);
		if (rectangle.Width <= 0 || rectangle.Height <= 0)
		{
			return;
		}
		bool _0E5B = _2AF4.ButtonItem._0E5B;
		bool flag = _2AF4.ButtonItem._0E5C;
		bool flag2 = _2AF4.ButtonItem._0E5D;
		bool flag3 = _2AF4.ButtonItem._0E5E;
		try
		{
			string text = dataGridViewButtonXColumn.Text;
			dataGridViewButtonXColumn._2AF7 = true;
			if (_2AEA != dataGridViewButtonXColumn._2AF3)
			{
				_2AF4.ButtonItem._0E5B = false;
				_2AF4.ButtonItem._0E5C = false;
				_2AF4.ButtonItem._0E5D = false;
				_2AF4.ButtonItem._0E5E = false;
			}
			((Control)_2AF4).Font = _2AE6.Font;
			((Control)_2AF4).ForeColor = _2AE6.ForeColor;
			((Control)_2AF4).BackColor = _2AE6.BackColor;
			if (_2AEA < ((DataGridViewElement)this).DataGridView.RowCount)
			{
				((Control)_2AF4).Text = (((DataGridViewButtonCell)this).UseColumnTextForButtonValue ? text : _239B(ԝ));
				if (_2AED(_2AEB, (DataGridViewPaintParts)8))
				{
					dataGridViewButtonXColumn._2B02(_2AEA, ((DataGridViewCell)this).ColumnIndex);
				}
			}
			else
			{
				((Control)_2AF4).Text = "";
			}
			((Control)_2AF4).Bounds = rectangle;
			_2AF4.RecalcLayout();
			((Control)_2AF4).DrawToBitmap(_2ADA, rectangle);
		}
		finally
		{
			_2AF4.ButtonItem._0E5B = _0E5B;
			_2AF4.ButtonItem._0E5C = flag;
			_2AF4.ButtonItem._0E5D = flag2;
			_2AF4.ButtonItem._0E5E = flag3;
			dataGridViewButtonXColumn._2AF7 = false;
		}
	}

	protected override void OnMouseEnter(int rowIndex)
	{
		((DataGridViewCell)this).OnMouseEnter(rowIndex);
		_2AEC(rowIndex);
	}

	internal void _2AEC(int _2AEA)
	{
		DataGridViewButtonXColumn dataGridViewButtonXColumn = (DataGridViewButtonXColumn)(object)((DataGridViewCell)this).OwningColumn;
		ButtonX _2AF4 = dataGridViewButtonXColumn._2AF4;
		dataGridViewButtonXColumn._2AF5 = _2AEA;
		if (!_2AF4.Expanded)
		{
			dataGridViewButtonXColumn._2AF3 = _2AEA;
			_2AF4.ButtonItem.InternalMouseEnter();
			_2AF2(((DataGridViewCell)this).ColumnIndex, _2AEA);
		}
	}

	protected override void OnMouseLeave(int rowIndex)
	{
		((DataGridViewButtonCell)this).OnMouseLeave(rowIndex);
		DataGridViewButtonXColumn dataGridViewButtonXColumn = (DataGridViewButtonXColumn)(object)((DataGridViewCell)this).OwningColumn;
		ButtonX _2AF4 = dataGridViewButtonXColumn._2AF4;
		dataGridViewButtonXColumn._2AF5 = -1;
		if (!_2AF4.Expanded)
		{
			dataGridViewButtonXColumn._2AF3 = -1;
			_2AF4.ButtonItem.InternalMouseLeave();
			_2AF2(((DataGridViewCell)this).ColumnIndex, rowIndex);
		}
	}

	protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		((DataGridViewButtonCell)this).OnMouseMove(e);
		DataGridViewButtonXColumn dataGridViewButtonXColumn = (DataGridViewButtonXColumn)(object)((DataGridViewCell)this).OwningColumn;
		ButtonX _2AF4 = dataGridViewButtonXColumn._2AF4;
		if (dataGridViewButtonXColumn._2AF3 == e.RowIndex || !_2AF4.Expanded)
		{
			_2AF4.ButtonItem.InternalMouseMove(new MouseEventArgs(((MouseEventArgs)e).Button, ((MouseEventArgs)e).Clicks, ((MouseEventArgs)e).X, ((MouseEventArgs)e).Y, ((MouseEventArgs)e).Delta));
			_2AF2(e.ColumnIndex, e.RowIndex);
		}
	}

	protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
	{
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		((DataGridViewButtonCell)this).OnMouseDown(e);
		DataGridViewButtonXColumn dataGridViewButtonXColumn = (DataGridViewButtonXColumn)(object)((DataGridViewCell)this).OwningColumn;
		ButtonX _2AF4 = dataGridViewButtonXColumn._2AF4;
		if (!((Control)((DataGridViewElement)this).DataGridView).Controls.Contains((Control)(object)_2AF4))
		{
			((Control)((DataGridViewElement)this).DataGridView).Controls.Add((Control)(object)_2AF4);
		}
		if (!dataGridViewButtonXColumn._2AF6 || !_2AF4.ButtonItem._0E5C)
		{
			Rectangle cellDisplayRectangle = ((DataGridViewElement)this).DataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
			Rectangle rectangle = _2AF0(cellDisplayRectangle, _2AF1: false);
			Point location = ((MouseEventArgs)e).Location;
			location.Offset(rectangle.Location);
			if (rectangle.Contains(location))
			{
				((Control)_2AF4).Bounds = rectangle;
				_2AF4.ButtonItem.InternalMouseDown(new MouseEventArgs(((MouseEventArgs)e).Button, ((MouseEventArgs)e).Clicks, ((MouseEventArgs)e).X, ((MouseEventArgs)e).Y, ((MouseEventArgs)e).Delta));
				((Control)((DataGridViewElement)this).DataGridView).Invalidate(rectangle);
			}
		}
		dataGridViewButtonXColumn._2AF6 = false;
	}

	protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		((DataGridViewButtonCell)this).OnMouseUp(e);
		DataGridViewButtonXColumn dataGridViewButtonXColumn = (DataGridViewButtonXColumn)(object)((DataGridViewCell)this).OwningColumn;
		ButtonX _2AF4 = dataGridViewButtonXColumn._2AF4;
		Rectangle cellDisplayRectangle = ((DataGridViewElement)this).DataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
		Rectangle rectangle = _2AF0(cellDisplayRectangle, _2AF1: false);
		((Control)((DataGridViewElement)this).DataGridView).Invalidate(rectangle);
		((Control)_2AF4).Bounds = rectangle;
		_2AF4.ButtonItem.InternalMouseUp(new MouseEventArgs(((MouseEventArgs)e).Button, ((MouseEventArgs)e).Clicks, ((MouseEventArgs)e).X, ((MouseEventArgs)e).Y, ((MouseEventArgs)e).Delta));
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
		DataGridViewButtonXColumn dataGridViewButtonXColumn = (DataGridViewButtonXColumn)(object)((DataGridViewCell)this).OwningColumn;
		Rectangle rectangle = ((DataGridViewCell)this).BorderWidths(_2AEF);
		_2AE9.Offset(rectangle.X, rectangle.Y);
		_2AE9.Width -= rectangle.Right;
		_2AE9.Height -= rectangle.Bottom;
		if (((DataGridViewCell)this).Selected)
		{
			_2AE9.Width += ((DataGridViewColumn)dataGridViewButtonXColumn).DividerWidth;
		}
		return _2AE9;
	}

	private Rectangle _2AF0(Rectangle _2AE9, bool _2AF1)
	{
		DataGridViewButtonXColumn dataGridViewButtonXColumn = (DataGridViewButtonXColumn)(object)((DataGridViewCell)this).OwningColumn;
		_2AE9.Width -= ((DataGridViewColumn)dataGridViewButtonXColumn).DividerWidth + 3;
		_2AE9.Height -= 3;
		if (_2AF1)
		{
			_2AE9.Location = new Point(0, 0);
		}
		_2AE9.X++;
		_2AE9.Y++;
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

	private void _2AF2(int _21A5, int _2AEA)
	{
		if (((DataGridViewElement)this).DataGridView != null)
		{
			Rectangle cellDisplayRectangle = ((DataGridViewElement)this).DataGridView.GetCellDisplayRectangle(_21A5, _2AEA, false);
			Rectangle rectangle = _2AF0(cellDisplayRectangle, _2AF1: false);
			((Control)((DataGridViewElement)this).DataGridView).Invalidate(rectangle);
		}
	}
}
