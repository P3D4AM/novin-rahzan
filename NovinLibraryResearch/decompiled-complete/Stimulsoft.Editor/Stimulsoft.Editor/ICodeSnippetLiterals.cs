using System.Collections;

namespace Stimulsoft.Editor;

public interface ICodeSnippetLiterals : IList, ICollection, IEnumerable
{
	new ICodeSnippetLiteral this[int index] { get; }

	ICodeSnippetLiteral AddLiteral();

	ICodeSnippetLiteral InsertLiteral(int index);
}
