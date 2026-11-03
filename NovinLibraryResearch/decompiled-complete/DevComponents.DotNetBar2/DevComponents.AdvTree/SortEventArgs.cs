using System;
using System.Collections;

namespace DevComponents.AdvTree;

public class SortEventArgs : EventArgs
{
	public bool ReverseSort;

	public bool Cancel;

	public ColumnHeader ColumnHeader;

	public IComparer Comparer;

	public SortEventArgs(bool reverse, ColumnHeader columnHeader)
	{
		ReverseSort = reverse;
		ColumnHeader = columnHeader;
	}
}
