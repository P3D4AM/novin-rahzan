using System;

namespace DevComponents.AdvTree;

public class CustomCellEditorEventArgs : EventArgs
{
	public ICellEditControl EditControl;

	public readonly Cell Cell;

	public CustomCellEditorEventArgs(Cell cell)
	{
		Cell = cell;
	}
}
