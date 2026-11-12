using System.Collections;

namespace Stimulsoft.Editor;

public interface IUndo
{
	UndoOptions UndoOptions { get; set; }

	int UndoLimit { get; set; }

	int UndoUpdateCount { get; }

	IList UndoList { get; }

	IList RedoList { get; }

	void Undo();

	void Undo(UndoData undoData);

	void Redo();

	bool CanUndo();

	bool CanRedo();

	void ClearUndo();

	void ClearRedo();

	int DisableUndo();

	int EnableUndo();

	int BeginUndoUpdate();

	int EndUndoUpdate();

	void ResetUndoOptions();

	void ResetUndoLimit();
}
