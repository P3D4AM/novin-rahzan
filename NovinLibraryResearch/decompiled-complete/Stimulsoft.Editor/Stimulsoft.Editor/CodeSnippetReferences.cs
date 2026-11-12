using System.Collections;

namespace Stimulsoft.Editor;

public class CodeSnippetReferences : ArrayList, ICodeSnippetReferences, IList, ICollection, IEnumerable
{
	public new ICodeSnippetReference this[int index] => (ICodeSnippetReference)base[index];

	public ICodeSnippetReference AddReference()
	{
		ICodeSnippetReference codeSnippetReference = new CodeSnippetReference();
		base.Add(codeSnippetReference);
		return codeSnippetReference;
	}

	public ICodeSnippetReference InsertReference(int index)
	{
		ICodeSnippetReference codeSnippetReference = new CodeSnippetReference();
		base.Insert(index, codeSnippetReference);
		return codeSnippetReference;
	}
}
