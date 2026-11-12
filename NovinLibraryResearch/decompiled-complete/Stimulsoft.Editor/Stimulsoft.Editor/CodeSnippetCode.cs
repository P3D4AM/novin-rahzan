using System.Xml.Serialization;

namespace Stimulsoft.Editor;

public class CodeSnippetCode : ICodeSnippetCode
{
	private string delimiter = SyntaxConsts.DefaultSnippetDelimiter;

	private string kind = string.Empty;

	private string language = string.Empty;

	private string code = string.Empty;

	[XmlAttribute("Delimiter")]
	public string Delimiter
	{
		get
		{
			return delimiter;
		}
		set
		{
			delimiter = value;
		}
	}

	[XmlAttribute("Kind")]
	public string Kind
	{
		get
		{
			return kind;
		}
		set
		{
			kind = value;
		}
	}

	[XmlAttribute("Language")]
	public string Language
	{
		get
		{
			return language;
		}
		set
		{
			language = value;
		}
	}

	[XmlText(typeof(string))]
	public string Code
	{
		get
		{
			return code;
		}
		set
		{
			code = value;
		}
	}

	public void Assign(ICodeSnippetCode source)
	{
		Delimiter = source.Delimiter;
		Kind = source.Kind;
		Language = source.Language;
		Code = source.Code;
	}
}
