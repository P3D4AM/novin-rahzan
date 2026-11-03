using System;

namespace DevComponents.DotNetBar.Controls;

public class DropDownChangeEventArgs : EventArgs
{
	private int __2B03;

	private int __17CD;

	private bool __223A;

	public int ColumnIndex => __17CD;

	public bool Expanded => __223A;

	public int RowIndex => __2B03;

	public DropDownChangeEventArgs(int rowIndex, int columnIndex, bool expanded)
	{
		__2B03 = rowIndex;
		__17CD = columnIndex;
		__223A = expanded;
	}
}
