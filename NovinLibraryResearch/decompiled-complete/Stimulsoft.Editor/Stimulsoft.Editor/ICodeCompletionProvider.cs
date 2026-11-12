using System.Collections;
using System.IO;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

public interface ICodeCompletionProvider : IList, ICollection, IEnumerable
{
	int ColumnCount { get; }

	string[] Strings { get; }

	int[] ImageIndexes { get; }

	string[] Descriptions { get; }

	bool ShowDescriptions { get; set; }

	int SelIndex { get; set; }

	ImageList Images { get; set; }

	bool UseIndent { get; set; }

	bool UseHtmlFormatting { get; set; }

	string EditField { get; set; }

	string EditPath { get; }

	event ClosePopupEvent ClosePopup;

	event ShowPopupEvent ShowPopup;

	bool ColumnVisible(int column);

	string GetColumnText(int index, int column);

	string GetText(int index);

	string GetName(int index);

	int IndexOfName(string name, bool caseSensitive);

	ICodeCompletionProvider GetParent();

	bool SaveFile(string fileName);

	bool SaveStream(TextWriter writer);

	bool SaveStream(Stream stream);

	bool LoadFile(string fileName);

	bool LoadStream(TextReader reader);

	bool LoadStream(Stream stream);

	void Sort();

	void Sort(IComparer comparer);
}
