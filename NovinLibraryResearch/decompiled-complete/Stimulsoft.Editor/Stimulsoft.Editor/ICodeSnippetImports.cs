using System.Collections;

namespace Stimulsoft.Editor;

public interface ICodeSnippetImports : IList, ICollection, IEnumerable
{
	new ICodeSnippetImport this[int index] { get; }

	ICodeSnippetImport AddImport();

	ICodeSnippetImport InsertImport(int index);
}
