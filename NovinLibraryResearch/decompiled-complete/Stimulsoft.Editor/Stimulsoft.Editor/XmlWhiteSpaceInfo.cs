namespace Stimulsoft.Editor;

public class XmlWhiteSpaceInfo
{
	private WhiteSpace owner;

	private bool visible;

	private char tabSymbol = EditConsts.TabSymbol;

	private char spaceSymbol = EditConsts.SpaceSymbol;

	private char eolSymbol = EditConsts.EolSymbol;

	private char eofSymbol = EditConsts.EofSymbol;

	private char wordWrapSymbol = EditConsts.WordWrapSymbol;

	private string symbolColor = EditConsts.DefaultWhiteSpaceForeColor.Name;

	public bool Visible
	{
		get
		{
			if (owner == null)
			{
				return visible;
			}
			return owner.Visible;
		}
		set
		{
			visible = value;
			if (owner != null)
			{
				owner.Visible = value;
			}
		}
	}

	public char TabSymbol
	{
		get
		{
			if (owner == null)
			{
				return tabSymbol;
			}
			return owner.TabSymbol;
		}
		set
		{
			tabSymbol = value;
			if (owner != null)
			{
				owner.TabSymbol = value;
			}
		}
	}

	public char SpaceSymbol
	{
		get
		{
			if (owner == null)
			{
				return spaceSymbol;
			}
			return owner.SpaceSymbol;
		}
		set
		{
			spaceSymbol = value;
			if (owner != null)
			{
				owner.SpaceSymbol = value;
			}
		}
	}

	public char EolSymbol
	{
		get
		{
			if (owner == null)
			{
				return eolSymbol;
			}
			return owner.EolSymbol;
		}
		set
		{
			eolSymbol = value;
			if (owner != null)
			{
				owner.EolSymbol = value;
			}
		}
	}

	public char EofSymbol
	{
		get
		{
			if (owner == null)
			{
				return eofSymbol;
			}
			return owner.EofSymbol;
		}
		set
		{
			eofSymbol = value;
			if (owner != null)
			{
				owner.EofSymbol = value;
			}
		}
	}

	public char WordWrapSymbol
	{
		get
		{
			if (owner == null)
			{
				return wordWrapSymbol;
			}
			return owner.WordWrapSymbol;
		}
		set
		{
			wordWrapSymbol = value;
			if (owner != null)
			{
				owner.WordWrapSymbol = value;
			}
		}
	}

	public string SymbolColor
	{
		get
		{
			if (owner == null)
			{
				return symbolColor;
			}
			return XmlHelper.SerializeColor(owner.SymbolColor);
		}
		set
		{
			symbolColor = value;
			if (owner != null)
			{
				owner.SymbolColor = XmlHelper.DeserializeColor(value);
			}
		}
	}

	public XmlWhiteSpaceInfo()
	{
	}

	public XmlWhiteSpaceInfo(WhiteSpace owner)
		: this()
	{
		this.owner = owner;
	}

	public virtual void Load()
	{
		if (owner != null)
		{
			visible = Visible;
			tabSymbol = TabSymbol;
			spaceSymbol = SpaceSymbol;
			eolSymbol = EolSymbol;
			eofSymbol = EofSymbol;
			symbolColor = SymbolColor;
			wordWrapSymbol = WordWrapSymbol;
		}
	}

	public virtual void FixupReferences(WhiteSpace owner)
	{
		this.owner = owner;
		Visible = visible;
		TabSymbol = tabSymbol;
		SpaceSymbol = spaceSymbol;
		EolSymbol = eolSymbol;
		EofSymbol = eofSymbol;
		SymbolColor = symbolColor;
		WordWrapSymbol = wordWrapSymbol;
	}
}
