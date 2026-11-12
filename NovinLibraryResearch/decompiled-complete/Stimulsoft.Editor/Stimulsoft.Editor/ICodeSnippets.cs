using System.Collections;

namespace Stimulsoft.Editor;

public interface ICodeSnippets : ICodeSnippetsProvider, ICodeCompletionProvider, IList, ICollection, IEnumerable
{
	new ICodeSnippet this[int index] { get; }

	ICodeSnippet AddSnippet();

	ICodeSnippet InsertSnippet(int index);
}
