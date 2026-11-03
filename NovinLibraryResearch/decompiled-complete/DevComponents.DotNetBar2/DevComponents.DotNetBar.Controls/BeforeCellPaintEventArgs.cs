using System;

namespace DevComponents.DotNetBar.Controls;

public class BeforeCellPaintEventArgs : EventArgs
{
	private int __2B03;

	private int __17CD;

	public int RowIndex => __2B03;

	public int ColumnIndex => __17CD;

	public BeforeCellPaintEventArgs(int rowIndex, int columnIndex)
	{
		__2B03 = rowIndex;
		__17CD = columnIndex;
	}
}
