using System;

namespace DevComponents.AdvTree;

public class CellEditEventArgs : EventArgs
{
	public eTreeAction Action = eTreeAction.Code;

	public Cell Cell;

	public string NewText = "";

	private ICellEditControl __17C0;

	public bool Cancel;

	public readonly bool IsUserCanceled;

	public ICellEditControl Editor => __17C0;

	public CellEditEventArgs(Cell cell, eTreeAction action, string newText)
	{
		Action = action;
		Cell = cell;
		NewText = newText;
		IsUserCanceled = false;
	}

	public CellEditEventArgs(Cell cell, eTreeAction action, string newText, ICellEditControl editor)
	{
		Action = action;
		Cell = cell;
		NewText = newText;
		IsUserCanceled = false;
		__17C0 = editor;
	}

	public CellEditEventArgs(Cell cell, eTreeAction action, string newText, bool isUserCanceled)
	{
		Action = action;
		Cell = cell;
		NewText = newText;
		IsUserCanceled = isUserCanceled;
	}

	public CellEditEventArgs(Cell cell, eTreeAction action, string newText, bool isUserCanceled, ICellEditControl editor)
	{
		Action = action;
		Cell = cell;
		NewText = newText;
		IsUserCanceled = isUserCanceled;
		__17C0 = editor;
	}
}
