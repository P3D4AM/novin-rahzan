using System;

namespace DevComponents.AdvTree;

public class ColumnEventArgs : EventArgs
{
	public readonly ColumnHeader Column;

	public ColumnEventArgs(ColumnHeader column)
	{
		Column = column;
	}
}
