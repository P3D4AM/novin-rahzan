using System.Collections;

namespace Stimulsoft.Editor;

public interface ICodeSnippetReferences : IList, ICollection, IEnumerable
{
	new ICodeSnippetReference this[int index] { get; }

	ICodeSnippetReference AddReference();

	ICodeSnippetReference InsertReference(int index);
}
