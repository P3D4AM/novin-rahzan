using System;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

public class DropDownChangeEventArgs : EventArgs
{
	private int _RowIndex;

	private int _ColumnIndex;

	private bool _Expanded;

	public int ColumnIndex => _ColumnIndex;

	public bool Expanded => _Expanded;

	public int RowIndex => _RowIndex;

	public DropDownChangeEventArgs(int rowIndex, int columnIndex, bool expanded)
	{
		_RowIndex = rowIndex;
		_ColumnIndex = columnIndex;
		_Expanded = expanded;
	}
}
