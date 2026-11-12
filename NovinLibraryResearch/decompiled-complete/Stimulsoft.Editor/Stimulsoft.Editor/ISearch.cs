using System.Collections;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Stimulsoft.Editor;

public interface ISearch : ITextSearch
{
	bool InIncrementalSearch { get; }

	string IncrementalSearchString { get; }

	Point SearchPos { get; set; }

	SearchOptions SearchOptions { get; set; }

	int SearchLen { get; }

	bool FirstSearch { get; set; }

	ISearchDialog SearchDialog { get; set; }

	IGotoLineDialog GotoLineDialog { get; set; }

	bool Find(string text);

	bool Find(string text, SearchOptions options);

	bool Find(string text, SearchOptions options, Regex expression);

	bool Find(string text, SearchOptions options, Regex expression, IList ranges);

	bool FindNext();

	bool FindPrevious();

	bool FindNextSelected();

	bool FindPreviousSelected();

	bool CanFindNext();

	bool CanFindPrevious();

	bool CanFindNextSelected();

	bool CanFindPreviousSelected();

	bool Replace(string text, string replaceWith);

	bool Replace(string text, string replaceWith, SearchOptions options);

	bool Replace(string text, string replaceWith, SearchOptions options, Regex expression);

	bool ReplaceAll(string text, string replaceWith, out int count);

	bool ReplaceAll(string text, string replaceWith, SearchOptions options, out int count);

	bool ReplaceAll(string text, string replaceWith, SearchOptions options, Regex expression, out int count);

	int MarkAll(string text, bool clearPrevious);

	int MarkAll(string text, SearchOptions options, bool clearPrevious);

	int MarkAll(string text, SearchOptions options, Regex expression, bool clearPrevious);

	bool NeedReplaceCurrent(out Match match);

	bool ReplaceCurrent(string replaceWith, SearchOptions options, Match match);

	void StartIncrementalSearch();

	void StartIncrementalSearch(bool backwardSearch);

	void FinishIncrementalSearch();

	bool IncrementalSearch(string key, bool deleteLast);

	string GetTextToSearchAtCursor();

	bool CanSearchSelection(out string selectedText);

	void ShowNotFound(string caption);
}
