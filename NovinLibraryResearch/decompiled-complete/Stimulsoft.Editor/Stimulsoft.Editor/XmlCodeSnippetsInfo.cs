using System.Xml.Serialization;

namespace Stimulsoft.Editor;

[XmlRoot("CodeSnippetCollection")]
public class XmlCodeSnippetsInfo
{
	private CodeSnippets owner;

	private CodeSnippet[] codeSnippets;

	[XmlArray]
	[XmlArrayItem("Snippet")]
	public CodeSnippet[] CodeSnippets
	{
		get
		{
			if (owner != null)
			{
				CodeSnippet[] array = new CodeSnippet[owner.Count];
				owner.CopyTo(array);
				return array;
			}
			return codeSnippets;
		}
		set
		{
			codeSnippets = value;
			if (owner != null)
			{
				owner.Clear();
				foreach (CodeSnippet codeSnippet in value)
				{
					owner.Add(codeSnippet);
					codeSnippet.Parent = owner;
				}
			}
		}
	}

	public XmlCodeSnippetsInfo()
	{
	}

	public XmlCodeSnippetsInfo(CodeSnippets owner)
		: this()
	{
		this.owner = owner;
	}

	public virtual void FixupReferences(CodeSnippets owner)
	{
		this.owner = owner;
		CodeSnippets = codeSnippets;
	}
}
