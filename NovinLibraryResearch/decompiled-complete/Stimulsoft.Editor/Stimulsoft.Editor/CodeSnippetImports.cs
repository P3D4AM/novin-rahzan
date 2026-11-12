using System.Collections;

namespace Stimulsoft.Editor;

public class CodeSnippetImports : ArrayList, ICodeSnippetImports, IList, ICollection, IEnumerable
{
	public new ICodeSnippetImport this[int index] => (ICodeSnippetImport)base[index];

	public ICodeSnippetImport AddImport()
	{
		ICodeSnippetImport codeSnippetImport = new CodeSnippetImport();
		base.Add(codeSnippetImport);
		return codeSnippetImport;
	}

	public ICodeSnippetImport InsertImport(int index)
	{
		ICodeSnippetImport codeSnippetImport = new CodeSnippetImport();
		base.Insert(index, codeSnippetImport);
		return codeSnippetImport;
	}
}
