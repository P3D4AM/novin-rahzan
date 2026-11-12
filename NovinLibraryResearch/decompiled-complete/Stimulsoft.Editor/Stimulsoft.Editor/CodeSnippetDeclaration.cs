using System.Xml.Serialization;

namespace Stimulsoft.Editor;

public class CodeSnippetDeclaration : ICodeSnippetDeclaration
{
	private ICodeSnippetLiterals literals = new CodeSnippetLiterals();

	private ICodeSnippetObjects objects = new CodeSnippetObjects();

	[XmlIgnore]
	public ICodeSnippetLiterals Literals => literals;

	[XmlArray("Literals")]
	[XmlArrayItem("Literal")]
	public CodeSnippetLiteral[] SnippetLiterals
	{
		get
		{
			CodeSnippetLiteral[] array = new CodeSnippetLiteral[Literals.Count];
			((CodeSnippetLiterals)Literals).CopyTo(array);
			return array;
		}
		set
		{
			literals.Clear();
			foreach (CodeSnippetLiteral value2 in value)
			{
				literals.Add(value2);
			}
		}
	}

	[XmlIgnore]
	public ICodeSnippetObjects Objects => objects;

	[XmlArray("Objects")]
	[XmlArrayItem("Objects")]
	public CodeSnippetObject[] SnippetObjects
	{
		get
		{
			CodeSnippetObject[] array = new CodeSnippetObject[Objects.Count];
			((CodeSnippetObjects)Objects).CopyTo(array);
			return array;
		}
		set
		{
			objects.Clear();
			foreach (CodeSnippetObject value2 in value)
			{
				objects.Add(value2);
			}
		}
	}
}
