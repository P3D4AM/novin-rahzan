using System;
using System.Drawing;

namespace Stimulsoft.Editor;

public interface ISelection
{
	int UpdateCount { get; }

	bool IsEmpty { get; }

	int SelectionStart { get; set; }

	int SelectionLength { get; set; }

	Rectangle SelectionRect { get; set; }

	SelectionType SelectionType { get; set; }

	string SelectedText { get; set; }

	SelectionOptions Options { get; set; }

	Color ForeColor { get; set; }

	Color BackColor { get; set; }

	Color InActiveForeColor { get; set; }

	Color BorderColor { get; set; }

	Color InActiveBackColor { get; set; }

	AllowedSelectionMode AllowedSelectionMode { get; set; }

	event EventHandler SelectionChanged;

	void Assign(ISelection source);

	void SetSelection(SelectionType selectionType, Rectangle selectionRect);

	void SetSelection(SelectionType selectionType, Point selectionStart, Point selectionEnd);

	void SelectCharLeft();

	void SelectCharLeft(SelectionType selectionType);

	void SelectCharRight();

	void SelectCharRight(SelectionType selectionType);

	void SelectWordLeft();

	void SelectWordLeft(SelectionType selectionType);

	void SelectWordRight();

	void SelectWordRight(SelectionType selectionType);

	void SelectLineUp();

	void SelectLineUp(SelectionType selectionType);

	void SelectLineDown();

	void SelectLineDown(SelectionType selectionType);

	void SelectPageUp();

	void SelectPageUp(SelectionType selectionType);

	void SelectPageDown();

	void SelectPageDown(SelectionType selectionType);

	void SelectScreenTop();

	void SelectScreenTop(SelectionType selectionType);

	void SelectScreenBottom();

	void SelectScreenBottom(SelectionType selectionType);

	void SelectLineBegin();

	void SelectLineBegin(SelectionType selectionType);

	void SelectLineEnd();

	void SelectLineEnd(SelectionType selectionType);

	void SelectFileBegin();

	void SelectFileBegin(SelectionType selectionType);

	void SelectFileEnd();

	void SelectFileEnd(SelectionType selectionType);

	void SelectLine();

	void SelectWord();

	void SelectAll();

	void SelectToOpenBrace();

	void SelectToCloseBrace();

	void SelectToBrace();

	void CommentSelection();

	void UncommentSelection();

	void SmartFormat();

	void SmartFormat(int line);

	void SmartIndent();

	void SmartFormatBlock();

	void SmartFormatDocument();

	void Tabify();

	void UnTabify();

	void Indent();

	void UnIndent();

	void LowerCase();

	void UpperCase();

	void Capitalize();

	void DeleteWhiteSpace();

	void ToggleOutlining();

	void CollapseToDefinitions();

	void SwapAnchor();

	bool CanCopy();

	bool CanCut();

	bool CanPaste();

	void Copy();

	void Cut();

	void Paste();

	bool Delete();

	void Clear();

	bool Move(Point position, bool deleteOrigin);

	int BeginUpdate();

	int EndUpdate();

	void DeleteLeft();

	void DeleteRight();

	void DeleteWordLeft();

	void DeleteWordRight();

	void DeleteLine();

	void CutLine();

	void NewLine();

	void NewLineAbove();

	void NewLineBelow();

	void ProcessEscape();

	void ProcessTab();

	void ProcessShiftTab();

	void ToggleOverWrite();

	void InsertString(string s);

	void CharTransponse();

	void WordTransponse();

	void LineTransponse();

	bool CanDrag(Point position);

	void DragTo(Point position, bool deleteOrigin);

	void ChangeBlock(StringEvent action);

	void ChangeBlock(StringEvent action, bool changeIfEmpty, bool extendFirstLine);

	Point SelectionToTextPoint(Point position);

	Point TextToSelectionPoint(Point position);

	bool IsPosInSelection(Point position);

	bool IsPosInSelection(int x, int y);

	bool GetSelectionForLine(int index, out int left, out int right);

	int SelectedCount();

	string SelectedString(int index);

	void ResetOptions();

	void ResetAllowedSelectionMode();

	void ResetForeColor();

	void ResetBackColor();

	void ResetInActiveForeColor();

	void ResetInActiveBackColor();

	void ResetBorderColor();
}
