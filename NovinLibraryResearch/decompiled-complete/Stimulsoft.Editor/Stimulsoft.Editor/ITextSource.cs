using System.Collections;
using System.Drawing;

namespace Stimulsoft.Editor;

public interface ITextSource : IEditEx, IEdit, INavigate, IUndo, ISourceNotify, INotifyEx, INotify, INotifier, IImport, IExportEx, IExport, IHyperText, ISpelling, IBraceMatching, ITrackChanges
{
	string FileName { get; set; }

	IList Edits { get; }

	ISyntaxStrings Lines { get; set; }

	string Text { get; set; }

	ILineStyles LineStyles { get; set; }

	IBookMarks BookMarks { get; set; }

	ISyntaxEdit ActiveEdit { get; set; }

	ILexer Lexer { get; set; }

	int ParserLine { get; }

	int TextPointToAbsolutePosition(Point position);

	Point AbsolutePositionToTextPoint(int position);

	int GetCharIndexFromPosition(Point position);

	Point GetPositionFromCharIndex(int charIndex);

	void ParseString(int index);

	void ParseStrings(int first, int last);

	void ParseToString(int index);

	bool NeedParse();

	void FormatText();

	void SetColorFlag(Point pos, int len, ColorFlags flag, bool setFlag);

	IStrItem CreateStrItem(string s);
}
