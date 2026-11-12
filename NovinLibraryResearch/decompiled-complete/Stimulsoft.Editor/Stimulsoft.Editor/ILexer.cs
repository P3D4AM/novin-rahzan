using System.IO;

namespace Stimulsoft.Editor;

public interface ILexer
{
	ILexScheme Scheme { get; set; }

	string XmlScheme { get; set; }

	int DefaultState { get; set; }

	event TextParsedEvent TextParsed;

	int ParseText(int state, int line, string str, ref short[] colorData);

	int ParseText(int state, int line, string str, ref int pos, ref int len, ref int style);

	void SaveScheme(string fileName);

	void LoadScheme(string fileName);

	void SaveScheme(TextWriter writer);

	void LoadScheme(TextReader reader);

	void ResetDefaultState();
}
