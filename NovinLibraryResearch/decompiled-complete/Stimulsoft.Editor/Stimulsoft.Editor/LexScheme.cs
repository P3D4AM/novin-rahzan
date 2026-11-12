using System.ComponentModel;
using System.Xml.Serialization;

namespace Stimulsoft.Editor;

public class LexScheme : ILexScheme
{
	private string author = string.Empty;

	private string name = string.Empty;

	private string desc = string.Empty;

	private string copyright = string.Empty;

	private string version = SyntaxConsts.DefaultLexSchemeVersion;

	private string fileExtension = string.Empty;

	private string fileType = string.Empty;

	private ILexStates lexStates;

	private ILexStyles lexStyles;

	[Description("Gets or sets author of the \"LexScheme\".")]
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

	[Description("Gets or sets \"LexScheme\" name.")]
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

	[Description("Gets or sets description of the \"LexScheme\".")]
	public string Desc
	{
		get
		{
			return desc;
		}
		set
		{
			desc = value;
		}
	}

	[Description("Gets or sets copyright of the \"LexScheme\".")]
	public string Copyright
	{
		get
		{
			return copyright;
		}
		set
		{
			copyright = value;
		}
	}

	[Description("Gets or sets string value indicating associated file extension of the \"ILexScheme\".")]
	public string FileExtension
	{
		get
		{
			return fileExtension;
		}
		set
		{
			fileExtension = value;
		}
	}

	[Description("Gets or sets string value indicating associated file type of the \"ILexScheme\".")]
	public string FileType
	{
		get
		{
			return fileType;
		}
		set
		{
			fileType = value;
		}
	}

	[Description("Gets or sets version of the \"LexScheme\".")]
	public string Version
	{
		get
		{
			return version;
		}
		set
		{
			version = value;
		}
	}

	[XmlIgnore]
	[Description("Gets or sets collection of lexical styles.")]
	public ILexStyles LexStyles
	{
		get
		{
			return lexStyles;
		}
		set
		{
			lexStyles.Clear();
			foreach (ILexStyle item in value)
			{
				lexStyles.Add(item);
			}
			FixupStyles();
		}
	}

	[XmlIgnore]
	[Description("Gets or sets collection of lexical states.")]
	public ILexStates LexStates
	{
		get
		{
			return lexStates;
		}
		set
		{
			lexStates.Clear();
			foreach (ILexState item in value)
			{
				lexStates.Add(item);
			}
			FixupStates();
		}
	}

	[XmlArray]
	[XmlArrayItem("Style")]
	[Description("Gets or sets \"LexStyles\" property in the form of LexStyle array.")]
	public LexStyle[] Styles
	{
		get
		{
			LexStyle[] array = new LexStyle[lexStyles.Count];
			((LexStyles)lexStyles).CopyTo(array);
			return array;
		}
		set
		{
			lexStyles.Clear();
			foreach (ILexStyle value2 in value)
			{
				lexStyles.Add(value2);
			}
			FixupStyles();
		}
	}

	[XmlArray]
	[XmlArrayItem("State")]
	[Description("Gets or sets \"LexStates\" property in the form of LexState array.")]
	public LexState[] States
	{
		get
		{
			LexState[] array = new LexState[lexStates.Count];
			((LexStates)lexStates).CopyTo(array);
			return array;
		}
		set
		{
			lexStates.Clear();
			foreach (ILexState value2 in value)
			{
				lexStates.Add(value2);
			}
			FixupStates();
		}
	}

	public LexScheme()
	{
		lexStates = new LexStates(this);
		lexStyles = new LexStyles(this);
	}

	public void ClearScheme()
	{
		lexStyles.Clear();
		lexStates.Clear();
		author = string.Empty;
		name = string.Empty;
		desc = string.Empty;
		copyright = string.Empty;
		version = SyntaxConsts.DefaultLexSchemeVersion;
		fileExtension = string.Empty;
		fileType = string.Empty;
	}

	private void FixupStyles()
	{
		foreach (LexStyle lexStyle in lexStyles)
		{
			lexStyle.Scheme = this;
		}
	}

	private void FixupStates()
	{
		foreach (LexState lexState in lexStates)
		{
			lexState.FixupBlocks(this);
		}
	}

	protected internal void Init()
	{
		FixupStates();
	}

	public ILexState GetLexState(int index)
	{
		if (index < 0 || index >= lexStates.Count)
		{
			return null;
		}
		return (LexState)lexStates[index];
	}

	public ILexStyle GetLexStyle(int index)
	{
		if (index < 0 || index >= lexStyles.Count)
		{
			return null;
		}
		return (LexStyle)lexStyles[index];
	}

	public bool IsPlainText(int style)
	{
		if (style >= 0 && style < lexStyles.Count)
		{
			return GetLexStyle(style).PlainText;
		}
		return false;
	}

	public bool IsEmpty()
	{
		if (lexStyles.Count == 0 && lexStates.Count == 0 && author == string.Empty && name == string.Empty && desc == string.Empty && copyright == string.Empty && fileType == string.Empty)
		{
			return fileExtension == string.Empty;
		}
		return false;
	}
}
