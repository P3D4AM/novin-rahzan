using System;

namespace DevComponents.AdvTree;

public class PrepareCellEditorEventArgs : EventArgs
{
	public readonly Cell EditedCell;

	public readonly ICellEditControl Editor;

	public PrepareCellEditorEventArgs(Cell editedCell, ICellEditControl editor)
	{
		EditedCell = editedCell;
		Editor = editor;
	}
}
