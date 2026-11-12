using System.Collections;

namespace Stimulsoft.Editor;

public class CodeSnippetObjects : ArrayList, ICodeSnippetObjects, IList, ICollection, IEnumerable
{
	public new ICodeSnippetObject this[int index] => (ICodeSnippetObject)base[index];

	public ICodeSnippetObject AddObject()
	{
		ICodeSnippetObject codeSnippetObject = new CodeSnippetObject();
		base.Add(codeSnippetObject);
		return codeSnippetObject;
	}

	public ICodeSnippetObject InsertObject(int index)
	{
		ICodeSnippetObject codeSnippetObject = new CodeSnippetObject();
		base.Insert(index, codeSnippetObject);
		return codeSnippetObject;
	}
}
