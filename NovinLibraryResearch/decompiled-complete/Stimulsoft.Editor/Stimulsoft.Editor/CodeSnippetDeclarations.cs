using System.Collections;

namespace Stimulsoft.Editor;

public class CodeSnippetDeclarations : ArrayList, ICodeSnippetDeclarations, IList, ICollection, IEnumerable
{
	public new ICodeSnippetDeclaration this[int index] => (ICodeSnippetDeclaration)base[index];

	public ICodeSnippetDeclaration AddDeclaration()
	{
		ICodeSnippetDeclaration codeSnippetDeclaration = new CodeSnippetDeclaration();
		base.Add(codeSnippetDeclaration);
		return codeSnippetDeclaration;
	}

	public ICodeSnippetDeclaration InsertDeclaration(int index)
	{
		ICodeSnippetDeclaration codeSnippetDeclaration = new CodeSnippetDeclaration();
		base.Insert(index, codeSnippetDeclaration);
		return codeSnippetDeclaration;
	}
}
