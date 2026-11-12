using System.Drawing;

namespace Stimulsoft.Editor;

public interface IParser : ILexer
{
	int Token { get; }

	int State { get; }

	string TokenString { get; }

	Point TokenPosition { get; }

	Point CurrentPosition { get; }

	bool Eof { get; }

	IStringList Strings { get; set; }

	string[] Lines { get; set; }

	void Reset();

	void Reset(int line, int pos, int state);

	int NextToken();

	int NextToken(out string str);

	int PeekToken();

	int PeekToken(out string str);

	int PeekValidToken();

	int PeekValidToken(out string str);

	int NextValidToken();

	int NextValidToken(out string str);

	void SaveState();

	void RestoreState();

	void RestoreState(bool restore);
}
