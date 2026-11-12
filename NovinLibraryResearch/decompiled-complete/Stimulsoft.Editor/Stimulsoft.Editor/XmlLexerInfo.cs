using System.Xml.Serialization;

namespace Stimulsoft.Editor;

public class XmlLexerInfo
{
	private Lexer owner;

	private ILexScheme scheme;

	private int defaultState;

	[XmlIgnore]
	public ILexScheme Scheme
	{
		get
		{
			if (owner == null)
			{
				return scheme;
			}
			return owner.Scheme;
		}
		set
		{
			scheme = value;
			if (owner != null)
			{
				owner.Scheme = value;
			}
		}
	}

	[XmlElement("Scheme")]
	public LexScheme LexScheme
	{
		get
		{
			return Scheme as LexScheme;
		}
		set
		{
			Scheme = value;
		}
	}

	public int DefaultState
	{
		get
		{
			if (owner == null)
			{
				return defaultState;
			}
			return owner.DefaultState;
		}
		set
		{
			defaultState = value;
			if (owner != null)
			{
				owner.DefaultState = value;
			}
		}
	}

	public XmlLexerInfo()
	{
		scheme = new LexScheme();
	}

	public XmlLexerInfo(Lexer owner)
		: this()
	{
		this.owner = owner;
	}

	public virtual void Load()
	{
		if (owner != null)
		{
			scheme = Scheme;
			defaultState = DefaultState;
		}
	}

	public virtual void FixupReferences(Lexer owner)
	{
		this.owner = owner;
		owner.BeginUpdate();
		try
		{
			Scheme = scheme;
			DefaultState = defaultState;
		}
		finally
		{
			owner.EndUpdate();
		}
	}
}
