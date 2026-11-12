using System;

namespace Stimulsoft.Editor;

public interface ICodeCompletionBox : ICodeCompletionWindow, IControl
{
	ICodeCompletionColumn[] Columns { get; }

	int DropDownCount { get; set; }

	bool Filtered { get; set; }

	string Filter { get; set; }

	bool Sorted { get; set; }

	event EventHandler SelectionChanged;

	void ClearColumns();

	ICodeCompletionColumn AddColumn();

	ICodeCompletionColumn InsertColumn(int index);

	void RemoveColumnAt(int index);

	void ResetDropDownCount();
}
