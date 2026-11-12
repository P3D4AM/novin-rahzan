using System.Xml.Serialization;

namespace Stimulsoft.Editor;

public class CodeSnippetHeader : ICodeSnippetHeader
{
	private string title = string.Empty;

	private string description = string.Empty;

	private string author = string.Empty;

	private string shortcut = string.Empty;

	private CodeSnippetTypes types;

	public string Title
	{
		get
		{
			return title;
		}
		set
		{
			title = value;
		}
	}

	public string Description
	{
		get
		{
			return description;
		}
		set
		{
			description = value;
		}
	}

	public string Author
	{
		get
		{
			return author;
		}
		set
		{
			author = value;
		}
	}

	public string Shortcut
	{
		get
		{
			return shortcut;
		}
		set
		{
			shortcut = value;
		}
	}

	[XmlIgnore]
	public ICodeSnippetTypes Types => types;

	public CodeSnippetType[] SnippetTypes
	{
		get
		{
			CodeSnippetType[] array = new CodeSnippetType[Types.Count];
			((CodeSnippetTypes)Types).CopyTo(array);
			return array;
		}
		set
		{
			types.Clear();
			foreach (CodeSnippetType value2 in value)
			{
				types.Add(value2);
			}
		}
	}

	public CodeSnippetHeader()
	{
		types = new CodeSnippetTypes();
	}

	public void Assign(ICodeSnippetHeader source)
	{
		Title = source.Title;
		Description = source.Description;
		Author = source.Author;
		Shortcut = source.Shortcut;
		types.Clear();
		foreach (ICodeSnippetType type in source.Types)
		{
			types.AddSnippetType().SnippetType = type.SnippetType;
		}
	}
}
