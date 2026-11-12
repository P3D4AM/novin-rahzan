using System.Collections;

namespace Stimulsoft.Editor;

public interface ICodeSnippetTypes : IList, ICollection, IEnumerable
{
	new ICodeSnippetType this[int index] { get; }

	ICodeSnippetType AddSnippetType();

	ICodeSnippetType InsertSnippetType(int index);
}
