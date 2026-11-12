using System.Collections;
using System.Drawing;

namespace Stimulsoft.Editor;

public interface IWordBreak
{
	char[] Delimiters { get; set; }

	string DelimiterString { get; set; }

	Hashtable DelimTable { get; }

	bool IsDelimiter(string s, int pos);

	bool IsDelimiter(int index, int pos);

	bool IsDelimiter(char ch);

	bool GetWord(string s, int pos, out int left, out int right);

	bool GetWord(int index, int pos, out int left, out int right);

	string GetTextAt(int pos, int line);

	string GetTextAt(Point position);

	void ResetDelimiters();
}
