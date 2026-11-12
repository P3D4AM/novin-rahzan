using System.Collections;

namespace Stimulsoft.Editor;

public class CodeSnippetLiterals : ArrayList, ICodeSnippetLiterals, IList, ICollection, IEnumerable
{
	public new ICodeSnippetLiteral this[int index] => (ICodeSnippetLiteral)base[index];

	public ICodeSnippetLiteral AddLiteral()
	{
		ICodeSnippetLiteral codeSnippetLiteral = new CodeSnippetLiteral();
		base.Add(codeSnippetLiteral);
		return codeSnippetLiteral;
	}

	public ICodeSnippetLiteral InsertLiteral(int index)
	{
		ICodeSnippetLiteral codeSnippetLiteral = new CodeSnippetLiteral();
		base.Insert(index, codeSnippetLiteral);
		return codeSnippetLiteral;
	}
}
