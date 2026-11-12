using System;
using System.Xml.Serialization;

namespace Stimulsoft.Editor;

public class CodeSnippetMember : ICodeSnippetMember, IComparable
{
	private string name = string.Empty;

	private string path = string.Empty;

	private int imageIndex = -1;

	private ICodeSnippetMembers parent;

	private ICodeSnippetMembers members;

	private ICodeSnippets snippets;

	[XmlIgnore]
	public ICodeSnippetMembers Parent
	{
		get
		{
			return parent;
		}
		set
		{
			parent = value;
		}
	}

	[XmlIgnore]
	public string EditPath
	{
		get
		{
			string text = string.Empty;
			for (ICodeSnippetMember codeSnippetMember = this; codeSnippetMember != null; codeSnippetMember = ((codeSnippetMember.Parent != null) ? codeSnippetMember.Parent.Parent : null))
			{
				text = ((text == string.Empty) ? codeSnippetMember.Name : (codeSnippetMember.Name + SyntaxConsts.DefaultSnippetPathSeparator + text));
			}
			if (text != string.Empty)
			{
				text += SyntaxConsts.DefaultSnippetPathSeparator;
			}
			return text;
		}
	}

	public string Name
	{
		get
		{
			return name;
		}
		set
		{
			name = value;
		}
	}

	public string Path
	{
		get
		{
			return path;
		}
		set
		{
			path = value;
		}
	}

	public int ImageIndex
	{
		get
		{
			return imageIndex;
		}
		set
		{
			imageIndex = value;
		}
	}

	[XmlIgnore]
	public ICodeSnippetsProvider SnippetsAndMembers
	{
		get
		{
			if (members.Count == 0)
			{
				return snippets;
			}
			if (snippets.Count == 0)
			{
				return members;
			}
			ICodeSnippetsProvider codeSnippetsProvider = new CodeSnippetsProvider(this);
			foreach (ICodeSnippet snippet in snippets)
			{
				codeSnippetsProvider.Add(snippet);
			}
			foreach (ICodeSnippetMember member in members)
			{
				codeSnippetsProvider.Add(member);
			}
			return codeSnippetsProvider;
		}
	}

	[XmlIgnore]
	public ICodeSnippetMembers Members => members;

	[XmlArray]
	[XmlArrayItem("Member")]
	public CodeSnippetMember[] CodeSnippetMembers
	{
		get
		{
			CodeSnippetMember[] array = new CodeSnippetMember[Members.Count];
			((CodeSnippetMembers)Members).CopyTo(array);
			return array;
		}
		set
		{
			Members.Clear();
			foreach (CodeSnippetMember value2 in value)
			{
				Members.Add(value2);
			}
			FixupMembers();
		}
	}

	[XmlIgnore]
	public ICodeSnippets Snippets => snippets;

	[XmlArray]
	[XmlArrayItem("Snippet")]
	public CodeSnippet[] CodeSnippets
	{
		get
		{
			CodeSnippet[] array = new CodeSnippet[Snippets.Count];
			((CodeSnippets)Snippets).CopyTo(array);
			return array;
		}
		set
		{
			Snippets.Clear();
			foreach (CodeSnippet value2 in value)
			{
				Snippets.Add(value2);
			}
			FixupSnippets();
		}
	}

	public CodeSnippetMember()
	{
		members = new CodeSnippetMembers(this);
		snippets = new CodeSnippets(this);
	}

	protected void FixupMembers()
	{
		foreach (ICodeSnippetMember member in members)
		{
			member.Parent = members;
		}
	}

	protected void FixupSnippets()
	{
		foreach (ICodeSnippet snippet in snippets)
		{
			snippet.Parent = snippets;
		}
	}

	public CodeSnippetMember(ICodeSnippetMembers parent)
		: this()
	{
		this.parent = parent;
	}

	public virtual int CompareTo(object value)
	{
		if (value is ICodeSnippetMember)
		{
			return string.Compare(name, ((ICodeSnippetMember)value).Name, ignoreCase: true);
		}
		throw new ArgumentException();
	}
}
