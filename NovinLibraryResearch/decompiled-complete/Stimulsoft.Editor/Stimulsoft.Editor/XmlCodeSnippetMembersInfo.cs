using System.Xml.Serialization;

namespace Stimulsoft.Editor;

[XmlRoot("CodeSnippetCollection")]
public class XmlCodeSnippetMembersInfo
{
	private CodeSnippetMembers owner;

	private CodeSnippetMember[] codeSnippetMembers;

	[XmlArray]
	[XmlArrayItem("Member")]
	public CodeSnippetMember[] CodeSnippetMembers
	{
		get
		{
			if (owner == null)
			{
				return codeSnippetMembers;
			}
			return owner.Members;
		}
		set
		{
			codeSnippetMembers = value;
			if (owner != null)
			{
				owner.Members = value;
			}
		}
	}

	public XmlCodeSnippetMembersInfo()
	{
	}

	public XmlCodeSnippetMembersInfo(CodeSnippetMembers owner)
		: this()
	{
		this.owner = owner;
	}

	public virtual void FixupReferences(CodeSnippetMembers owner)
	{
		this.owner = owner;
		CodeSnippetMembers = codeSnippetMembers;
	}
}
