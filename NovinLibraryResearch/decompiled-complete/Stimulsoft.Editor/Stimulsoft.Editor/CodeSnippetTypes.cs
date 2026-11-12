using System.Collections;

namespace Stimulsoft.Editor;

public class CodeSnippetTypes : ArrayList, ICodeSnippetTypes, IList, ICollection, IEnumerable
{
	public new ICodeSnippetType this[int index] => base[index] as ICodeSnippetType;

	public ICodeSnippetType AddSnippetType()
	{
		ICodeSnippetType codeSnippetType = new CodeSnippetType();
		base.Add(codeSnippetType);
		return codeSnippetType;
	}

	public ICodeSnippetType InsertSnippetType(int index)
	{
		ICodeSnippetType codeSnippetType = new CodeSnippetType();
		base.Insert(0, codeSnippetType);
		return codeSnippetType;
	}
}
