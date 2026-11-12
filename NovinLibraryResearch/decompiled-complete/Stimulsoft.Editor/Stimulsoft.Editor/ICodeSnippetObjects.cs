using System.Collections;

namespace Stimulsoft.Editor;

public interface ICodeSnippetObjects : IList, ICollection, IEnumerable
{
	new ICodeSnippetObject this[int index] { get; }

	ICodeSnippetObject AddObject();

	ICodeSnippetObject InsertObject(int index);
}
