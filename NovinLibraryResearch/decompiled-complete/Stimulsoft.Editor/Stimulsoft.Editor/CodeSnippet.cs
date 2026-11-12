using System;
using System.Xml.Serialization;

namespace Stimulsoft.Editor;

public class CodeSnippet : ICodeSnippet, IComparable
{
	private ICodeSnippetHeader header = new CodeSnippetHeader();

	private ICodeSnippetDeclarations declarations = new CodeSnippetDeclarations();

	private ICodeSnippetImports imports = new CodeSnippetImports();

	private ICodeSnippetReferences references = new CodeSnippetReferences();

	private ICodeSnippetCode code = new CodeSnippetCode();

	private int imageIndex = -1;

	private object customData;

	private ICodeSnippets parent;

	[XmlIgnore]
	public ICodeSnippets Parent
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
	public ICodeSnippetHeader Header
	{
		get
		{
			return header;
		}
		set
		{
			header.Assign(value);
		}
	}

	[XmlElement("Header")]
	public CodeSnippetHeader SnippetHeader
	{
		get
		{
			return (CodeSnippetHeader)header;
		}
		set
		{
			header.Assign(value);
		}
	}

	[XmlIgnore]
	public ICodeSnippetDeclarations Declarations => declarations;

	[XmlArray("Declarations")]
	[XmlArrayItem("Declaration")]
	public CodeSnippetDeclaration[] SnippetDeclarations
	{
		get
		{
			CodeSnippetDeclaration[] array = new CodeSnippetDeclaration[Declarations.Count];
			((CodeSnippetDeclarations)Declarations).CopyTo(array);
			return array;
		}
		set
		{
			declarations.Clear();
			foreach (CodeSnippetDeclaration value2 in value)
			{
				Declarations.Add(value2);
			}
		}
	}

	[XmlIgnore]
	public ICodeSnippetImports Imports => imports;

	[XmlArray("Imports")]
	[XmlArrayItem("Import")]
	public CodeSnippetImport[] SnippetImports
	{
		get
		{
			CodeSnippetImport[] array = new CodeSnippetImport[Imports.Count];
			((CodeSnippetImports)Imports).CopyTo(array);
			return array;
		}
		set
		{
			imports.Clear();
			foreach (CodeSnippetImport value2 in value)
			{
				Imports.Add(value2);
			}
		}
	}

	[XmlIgnore]
	public ICodeSnippetReferences References => references;

	[XmlArray("References")]
	[XmlArrayItem("Reference")]
	public CodeSnippetReference[] SnippetReferences
	{
		get
		{
			CodeSnippetReference[] array = new CodeSnippetReference[References.Count];
			((CodeSnippetReferences)References).CopyTo(array);
			return array;
		}
		set
		{
			references.Clear();
			foreach (CodeSnippetReference value2 in value)
			{
				References.Add(value2);
			}
		}
	}

	[XmlIgnore]
	public ICodeSnippetCode Code
	{
		get
		{
			return code;
		}
		set
		{
			code.Assign(value);
		}
	}

	[XmlElement("Code")]
	public CodeSnippetCode SnippetCode
	{
		get
		{
			return (CodeSnippetCode)code;
		}
		set
		{
			code.Assign(value);
		}
	}

	[XmlElement("Code")]
	public string Description
	{
		get
		{
			string text = header.Description;
			if (header.Shortcut != null && header.Shortcut != string.Empty)
			{
				text = text + ((parent != null && parent.UseHtmlFormatting) ? SyntaxConsts.DefaultBrTag : "\r\n") + string.Format(StringConsts.CodeSnippetTooltipHeader, header.Shortcut);
			}
			return text.Trim();
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
	public object CustomData
	{
		get
		{
			return customData;
		}
		set
		{
			customData = value;
		}
	}

	public CodeSnippet()
	{
	}

	public CodeSnippet(ICodeSnippets parent)
		: this()
	{
		this.parent = parent;
	}

	public virtual int CompareTo(object value)
	{
		if (value is ICodeSnippet)
		{
			return string.Compare(Header.Title, ((ICodeSnippet)value).Header.Title, ignoreCase: true);
		}
		throw new ArgumentException();
	}
}
