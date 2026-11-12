using System.Collections;

namespace Stimulsoft.Editor;

public interface ICodeSnippetMembers : ICodeSnippetsProvider, ICodeCompletionProvider, IList, ICollection, IEnumerable
{
	new ICodeSnippetMember this[int index] { get; }

	ICodeSnippetMember Parent { get; set; }

	ICodeSnippetMember AddSnippetMember();

	ICodeSnippetMember InsertSnippetMember(int index);
}
