using System.Collections;
using System.Drawing;

namespace Stimulsoft.Editor;

public interface ISyntaxStrings : IStringList, ICollection, IEnumerable, IImport, IExport, ITextSearch, IFmtExport, ITabulationEx, ITabulation, IWordBreak, INotifyEx, INotify
{
	ITextSource Source { get; }

	new string this[int index] { get; set; }

	bool RemoveTrailingSpaces { get; set; }

	int Add(string value);

	void Clear();

	bool Contains(string value);

	int IndexOf(string value);

	void Insert(int index, string value);

	void Remove(string value);

	void RemoveAt(int index);

	IStrItem GetItem(int index);

	int GetLength(int index);

	void Changed(int index);

	void Changed(int first, int last);

	IList ToArrayList();

	string[] ToStringArray();

	char GetCharAt(int x, int y);

	char GetCharAt(Point position);

	int GetLexStyle(Point position);

	void AfterSave();

	int TextPointToAbsolutePosition(Point position);

	Point AbsolutePositionToTextPoint(int position);
}
