using System;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

public class BeforeCellPaintEventArgs : EventArgs
{
	private int _RowIndex;

	private int _ColumnIndex;

	public int RowIndex => _RowIndex;

	public int ColumnIndex => _ColumnIndex;

	public BeforeCellPaintEventArgs(int rowIndex, int columnIndex)
	{
		_RowIndex = rowIndex;
		_ColumnIndex = columnIndex;
	}
}
