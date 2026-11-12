using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Editor;

public class EventHandlers
{
	private ISyntaxEdit owner;

	public KeyEvent tabifyEvent;

	public KeyEvent unTabifyEvent;

	public KeyEvent lowerCaseEvent;

	public KeyEvent upperCaseEvent;

	public KeyEvent capitalizeEvent;

	public KeyEvent deleteWhiteSpaceEvent;

	public KeyEvent commentSelectionEvent;

	public KeyEvent uncommentSelectionEvent;

	public KeyEvent moveCharLeftEvent;

	public KeyEvent moveCharRightEvent;

	public KeyEvent moveLineUpEvent;

	public KeyEvent moveLineDownEvent;

	public KeyEvent moveWordLeftEvent;

	public KeyEvent moveWordRightEvent;

	public KeyEvent movePageUpEvent;

	public KeyEvent movePageDownEvent;

	public KeyEvent moveScreenTopEvent;

	public KeyEvent moveScreenBottomEvent;

	public KeyEvent moveLineBeginEvent;

	public KeyEvent moveLineEndEvent;

	public KeyEvent moveFileBeginEvent;

	public KeyEvent moveFileEndEvent;

	public KeyEventEx selectCharLeftEvent;

	public KeyEventEx selectCharRightEvent;

	public KeyEventEx selectLineUpEvent;

	public KeyEventEx selectLineDownEvent;

	public KeyEventEx selectWordLeftEvent;

	public KeyEventEx selectWordRightEvent;

	public KeyEventEx selectPageUpEvent;

	public KeyEventEx selectPageDownEvent;

	public KeyEventEx selectScreenTopEvent;

	public KeyEventEx selectScreenBottomEvent;

	public KeyEventEx selectLineBeginEvent;

	public KeyEventEx selectLineEndEvent;

	public KeyEventEx selectFileBeginEvent;

	public KeyEventEx selectFileEndEvent;

	public KeyEvent selectAllEvent;

	public KeyEvent toggleBookMarkEvent;

	public KeyEvent nextBookMarkEvent;

	public KeyEvent prevBookMarkEvent;

	public KeyEvent clearBookMarkEvent;

	public KeyEventEx toggleBookMarkEventEx;

	public KeyEventEx moveBookMarkEvent;

	public KeyEvent toggleOutliningEvent;

	public KeyEvent toggleOutliningSelectionEvent;

	public KeyEvent collapseToDefinitionsEvent;

	public KeyEvent swapAnchorEvent;

	public KeyEvent deleteLeftEvent;

	public KeyEvent deleteWordLeftEvent;

	public KeyEvent deleteWordRightEvent;

	public KeyEvent deleteRightEvent;

	public KeyEvent newLineEvent;

	public KeyEvent processEscapeEvent;

	public KeyEvent processTabEvent;

	public KeyEvent processShiftTabEvent;

	public KeyEvent toggleOverwriteEvent;

	public KeyEvent undoEvent;

	public KeyEvent redoEvent;

	public KeyEvent findEvent;

	public KeyEvent findNextEvent;

	public KeyEvent findPreviousEvent;

	public KeyEvent findNextSelectedEvent;

	public KeyEvent findPreviousSelectedEvent;

	public KeyEvent replaceEvent;

	public KeyEvent gotolineEvent;

	public KeyEvent initIncrementalSearchEvent;

	public KeyEvent initReverseIncrementalSearchEvent;

	public KeyEvent cutEvent;

	public KeyEvent copyEvent;

	public KeyEvent pasteEvent;

	public KeyEvent completeWordEvent;

	public KeyEvent listMembersEvent;

	public KeyEvent quickInfoEvent;

	public KeyEvent codeSnippetsEvent;

	public KeyEvent parameterInfoEvent;

	public KeyEvent formatSelectionEvent;

	public KeyEvent formatDocumentEvent;

	public KeyEvent charTransposeEvent;

	public KeyEvent wordTransposeEvent;

	public KeyEvent lineTransposeEvent;

	public KeyEvent toggleHiddenTextEvent;

	public KeyEvent toggleMatchCaseEvent;

	public KeyEvent toggleRegularExpressionsEvent;

	public KeyEvent toggleSearchUpEvent;

	public KeyEvent toggleWholeWordEvent;

	public KeyEvent scrollLineUpEvent;

	public KeyEvent scrollLineDownEvent;

	public KeyEvent moveToOpenBraceEvent;

	public KeyEvent moveToOpenBraceExtendEvent;

	public KeyEvent moveToCloseBraceEvent;

	public KeyEvent moveToCloseBraceExtendEvent;

	public KeyEvent moveToBraceEvent;

	public KeyEvent moveToBraceExtendEvent;

	public KeyEvent newLineAboveEvent;

	public KeyEvent newLineBelowEvent;

	public KeyEvent selectWordEvent;

	public KeyEvent lineCutEvent;

	public KeyEvent lineDeleteEvent;

	public KeyEvent toggleMacroRecording;

	public KeyEvent playBackMacro;

	public KeyEventEx processKey;

	public string[] EventNames
	{
		get
		{
			FieldInfo[] fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
			string[] array = new string[fields.Length];
			for (int i = 0; i < fields.Length; i++)
			{
				string text = fields[i].Name;
				if (text != string.Empty)
				{
					char c = text[0].ToString().ToUpper(CultureInfo.InvariantCulture)[0];
					text = text.Remove(0, 1).Insert(0, c.ToString());
					int num = text.LastIndexOf("Event", text.Length);
					if (num >= 0)
					{
						text = text.Remove(num, text.Length - num);
					}
				}
				array[i] = text;
			}
			return array;
		}
	}

	public EventHandlers(ISyntaxEdit owner)
	{
		this.owner = owner;
		ISelection selection = owner.Selection;
		tabifyEvent = selection.Tabify;
		unTabifyEvent = selection.UnTabify;
		lowerCaseEvent = selection.LowerCase;
		upperCaseEvent = selection.UpperCase;
		capitalizeEvent = selection.Capitalize;
		deleteWhiteSpaceEvent = selection.DeleteWhiteSpace;
		commentSelectionEvent = selection.CommentSelection;
		uncommentSelectionEvent = selection.UncommentSelection;
		moveCharLeftEvent = owner.MoveCharLeft;
		moveCharRightEvent = owner.MoveCharRight;
		moveLineUpEvent = owner.MoveLineUp;
		moveLineDownEvent = owner.MoveLineDown;
		moveWordLeftEvent = owner.MoveWordLeft;
		moveWordRightEvent = owner.MoveWordRight;
		movePageUpEvent = owner.MovePageUp;
		movePageDownEvent = owner.MovePageDown;
		moveScreenTopEvent = owner.MoveScreenTop;
		moveScreenBottomEvent = owner.MoveScreenBottom;
		moveLineBeginEvent = owner.MoveLineBeginCycled;
		moveLineEndEvent = owner.MoveLineEnd;
		moveFileBeginEvent = owner.MoveFileBegin;
		moveFileEndEvent = owner.MoveFileEnd;
		selectCharLeftEvent = SelectCharLeft;
		selectCharRightEvent = SelectCharRight;
		selectLineUpEvent = SelectLineUp;
		selectLineDownEvent = SelectLineDown;
		selectWordLeftEvent = SelectWordLeft;
		selectWordRightEvent = SelectWordRight;
		selectPageUpEvent = SelectPageUp;
		selectPageDownEvent = SelectPageDown;
		selectScreenTopEvent = SelectScreenTop;
		selectScreenBottomEvent = SelectScreenBottom;
		selectLineBeginEvent = SelectLineBegin;
		selectLineEndEvent = SelectLineEnd;
		selectFileBeginEvent = SelectFileBegin;
		selectFileEndEvent = SelectFileEnd;
		selectAllEvent = SelectAll;
		toggleBookMarkEvent = ToggleBookMark;
		nextBookMarkEvent = GotoNextBookMark;
		prevBookMarkEvent = GotoPrevBookMark;
		clearBookMarkEvent = ClearAllBookMarks;
		toggleBookMarkEventEx = ToggleBookMark;
		moveBookMarkEvent = GotoBookMark;
		deleteLeftEvent = DeleteLeft;
		deleteWordLeftEvent = selection.DeleteWordLeft;
		deleteWordRightEvent = selection.DeleteWordRight;
		deleteRightEvent = selection.DeleteRight;
		newLineEvent = selection.NewLine;
		processEscapeEvent = ProcessEscape;
		processTabEvent = selection.ProcessTab;
		processShiftTabEvent = selection.ProcessShiftTab;
		toggleOverwriteEvent = selection.ToggleOverWrite;
		toggleOutliningEvent = owner.Outlining.ToggleOutlining;
		toggleOutliningSelectionEvent = selection.ToggleOutlining;
		collapseToDefinitionsEvent = owner.Outlining.CollapseToDefinitions;
		swapAnchorEvent = selection.SwapAnchor;
		undoEvent = Undo;
		redoEvent = Redo;
		findEvent = Find;
		findNextEvent = FindNext;
		findPreviousEvent = FindPrevious;
		findNextSelectedEvent = FindNextSelected;
		findPreviousSelectedEvent = FindPreviousSelected;
		replaceEvent = Replace;
		gotolineEvent = GotoLine;
		initIncrementalSearchEvent = owner.StartIncrementalSearch;
		initReverseIncrementalSearchEvent = StartReverseIncrementalSearch;
		cutEvent = selection.Cut;
		copyEvent = selection.Copy;
		pasteEvent = selection.Paste;
		completeWordEvent = owner.CompleteWord;
		listMembersEvent = owner.ListMembers;
		quickInfoEvent = owner.QuickInfo;
		parameterInfoEvent = owner.ParameterInfo;
		codeSnippetsEvent = owner.CodeSnippets;
		formatSelectionEvent = selection.SmartFormat;
		formatDocumentEvent = selection.SmartFormatDocument;
		charTransposeEvent = selection.CharTransponse;
		wordTransposeEvent = selection.WordTransponse;
		lineTransposeEvent = selection.LineTransponse;
		toggleHiddenTextEvent = ToggleHiddenText;
		toggleMatchCaseEvent = ToggleMatchCase;
		toggleRegularExpressionsEvent = ToggleRegularExpressions;
		toggleSearchUpEvent = ToggleSearchUp;
		toggleWholeWordEvent = ToggleWholeWord;
		scrollLineUpEvent = owner.ScrollLineUp;
		scrollLineDownEvent = owner.ScrollLineDown;
		moveToOpenBraceEvent = owner.MoveToOpenBrace;
		moveToOpenBraceExtendEvent = selection.SelectToOpenBrace;
		moveToCloseBraceEvent = owner.MoveToCloseBrace;
		moveToCloseBraceExtendEvent = selection.SelectToCloseBrace;
		moveToBraceEvent = owner.MoveToBrace;
		moveToBraceExtendEvent = selection.SelectToBrace;
		newLineAboveEvent = selection.NewLineAbove;
		newLineBelowEvent = selection.NewLineBelow;
		selectWordEvent = selection.SelectWord;
		lineCutEvent = selection.CutLine;
		lineDeleteEvent = selection.DeleteLine;
		toggleMacroRecording = owner.ToggleMacroRecording;
		playBackMacro = owner.PlayBackMacro;
		processKey = PressKey;
	}

	public void ToggleBookMark()
	{
		owner.Source.BookMarks.ToggleBookMark();
	}

	public void ToggleBookMark(object param)
	{
		owner.Source.BookMarks.ToggleBookMark((int)param);
	}

	public void GotoNextBookMark()
	{
		owner.Source.BookMarks.GotoNextBookMark();
	}

	public void GotoPrevBookMark()
	{
		owner.Source.BookMarks.GotoPrevBookMark();
	}

	public void ClearAllBookMarks()
	{
		owner.Source.BookMarks.ClearAllBookMarks();
	}

	public void GotoBookMark(object param)
	{
		owner.Source.BookMarks.GotoBookMark((int)param);
	}

	public void DeleteLeft()
	{
		if (owner.InIncrementalSearch && owner.IncrementalSearchString != string.Empty)
		{
			owner.IncrementalSearch(string.Empty, deleteLast: true);
		}
		else
		{
			owner.Selection.DeleteLeft();
		}
	}

	public void Undo()
	{
		owner.Source.Undo();
	}

	public void Redo()
	{
		owner.Source.Redo();
	}

	public void Find()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		owner.DisplaySearchDialog();
	}

	public void FindNext()
	{
		if (!owner.FindNext())
		{
			owner.ShowNotFound(StringConsts.DlgSearchCaption);
		}
	}

	public void FindPrevious()
	{
		if (!owner.FindPrevious())
		{
			owner.ShowNotFound(StringConsts.DlgSearchCaption);
		}
	}

	public void FindNextSelected()
	{
		if (owner.CanFindNextSelected() && !owner.FindNextSelected())
		{
			owner.ShowNotFound(StringConsts.DlgSearchCaption);
		}
	}

	public void FindPreviousSelected()
	{
		if (owner.CanFindPreviousSelected() && !owner.FindPreviousSelected())
		{
			owner.ShowNotFound(StringConsts.DlgSearchCaption);
		}
	}

	public void Replace()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		owner.DisplayReplaceDialog();
	}

	public void GotoLine()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		owner.DisplayGotoLineDialog();
	}

	public void ProcessEscape()
	{
		if (owner.InIncrementalSearch)
		{
			owner.FinishIncrementalSearch();
		}
		else
		{
			owner.Selection.ProcessEscape();
		}
	}

	public void ToggleHiddenText()
	{
		if (owner.SearchDialog != null)
		{
			owner.SearchDialog.ToggleHiddenText();
		}
	}

	public void ToggleMatchCase()
	{
		if (owner.SearchDialog != null)
		{
			owner.SearchDialog.ToggleMatchCase();
		}
	}

	public void ToggleRegularExpressions()
	{
		if (owner.SearchDialog != null)
		{
			owner.SearchDialog.ToggleRegularExpressions();
		}
	}

	public void ToggleSearchUp()
	{
		if (owner.SearchDialog != null)
		{
			owner.SearchDialog.ToggleSearchUp();
		}
	}

	public void ToggleWholeWord()
	{
		if (owner.SearchDialog != null)
		{
			owner.SearchDialog.ToggleWholeWord();
		}
	}

	public void SelectCharLeft(object selectionType)
	{
		owner.Selection.SelectCharLeft((SelectionType)selectionType);
	}

	public void SelectCharRight(object selectionType)
	{
		owner.Selection.SelectCharRight((SelectionType)selectionType);
	}

	public void SelectLineUp(object selectionType)
	{
		owner.Selection.SelectLineUp((SelectionType)selectionType);
	}

	public void SelectLineDown(object selectionType)
	{
		owner.Selection.SelectLineDown((SelectionType)selectionType);
	}

	public void SelectWordLeft(object selectionType)
	{
		owner.Selection.SelectWordLeft((SelectionType)selectionType);
	}

	public void SelectWordRight(object selectionType)
	{
		owner.Selection.SelectWordRight((SelectionType)selectionType);
	}

	public void SelectPageUp(object selectionType)
	{
		owner.Selection.SelectPageUp((SelectionType)selectionType);
	}

	public void SelectPageDown(object selectionType)
	{
		owner.Selection.SelectPageDown((SelectionType)selectionType);
	}

	public void SelectScreenTop(object selectionType)
	{
		owner.Selection.SelectScreenTop((SelectionType)selectionType);
	}

	public void SelectScreenBottom(object selectionType)
	{
		owner.Selection.SelectScreenBottom((SelectionType)selectionType);
	}

	public void SelectLineBegin(object selectionType)
	{
		owner.Selection.SelectLineBegin((SelectionType)selectionType);
	}

	public void SelectLineEnd(object selectionType)
	{
		owner.Selection.SelectLineEnd((SelectionType)selectionType);
	}

	public void SelectFileBegin(object selectionType)
	{
		owner.Selection.SelectFileBegin((SelectionType)selectionType);
	}

	public void SelectFileEnd(object selectionType)
	{
		owner.Selection.SelectFileEnd((SelectionType)selectionType);
	}

	public void SelectAll()
	{
		owner.MoveFileEnd();
		owner.Selection.SelectAll();
	}

	public void StartReverseIncrementalSearch()
	{
		owner.StartIncrementalSearch(backwardSearch: true);
	}

	public void PressKey(object param)
	{
		owner.ProcessKeyPress((char)param);
	}
}
