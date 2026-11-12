using System.Collections;

namespace Stimulsoft.Editor;

public interface ICodeSnippetDeclarations : IList, ICollection, IEnumerable
{
	new ICodeSnippetDeclaration this[int index] { get; }

	ICodeSnippetDeclaration AddDeclaration();

	ICodeSnippetDeclaration InsertDeclaration(int index);
}
