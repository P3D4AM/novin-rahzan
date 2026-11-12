using System.Text.RegularExpressions;

namespace Stimulsoft.Editor;

public interface ILexState
{
	string Name { get; set; }

	string Desc { get; set; }

	bool CaseSensitive { get; set; }

	string Expression { get; }

	ILexSyntaxBlocks LexSyntaxBlocks { get; }

	Regex Regex { get; }

	int Index { get; }

	void ResetCaseSensitive();
}
