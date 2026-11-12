using System.ComponentModel;
using System.Drawing;

namespace Stimulsoft.Editor;

public class WhiteSpace : IWhiteSpace
{
	private ISyntaxEdit owner;

	private char tabSymbol = EditConsts.TabSymbol;

	private char spaceSymbol = EditConsts.SpaceSymbol;

	private char eolSymbol = EditConsts.EolSymbol;

	private char eofSymbol = EditConsts.EofSymbol;

	private char wordWrapSymbol = EditConsts.WordWrapSymbol;

	private Color symbolColor = EditConsts.DefaultWhiteSpaceForeColor;

	private string spaceString;

	private string tabString;

	private string eolString;

	private string eofString;

	private string wordWrapString;

	private int updateCount;

	private bool visible;

	[DefaultValue(false)]
	[Description("Gets or sets a value indicating whether white space symbols are visible in the contol's text content.")]
	public bool Visible
	{
		get
		{
			return visible;
		}
		set
		{
			if (visible != value)
			{
				visible = value;
				Update();
			}
		}
	}

	[Description("Gets or sets character that introduces special symbol to paint instead of the tab character.")]
	public char TabSymbol
	{
		get
		{
			return tabSymbol;
		}
		set
		{
			if (tabSymbol != value)
			{
				tabSymbol = value;
				tabString = new string(tabSymbol, 1);
				Update();
			}
		}
	}

	[Description("Gets or sets character that introduces special symbol to paint instead of the space character.")]
	public char SpaceSymbol
	{
		get
		{
			return spaceSymbol;
		}
		set
		{
			if (spaceSymbol != value)
			{
				spaceSymbol = value;
				spaceString = new string(spaceSymbol, 1);
				Update();
			}
		}
	}

	[Description("Gets or sets character that specifies end of line.")]
	public char EolSymbol
	{
		get
		{
			return eolSymbol;
		}
		set
		{
			if (eolSymbol != value)
			{
				eolSymbol = value;
				eolString = new string(eolSymbol, 1);
				Update();
			}
		}
	}

	[Description("Gets or sets character that specifies end of file.")]
	public char EofSymbol
	{
		get
		{
			return eofSymbol;
		}
		set
		{
			if (eofSymbol != value)
			{
				eofSymbol = value;
				eofString = new string(eofSymbol, 1);
				Update();
			}
		}
	}

	[Description("Gets or sets character that specifies wrapped line.")]
	public char WordWrapSymbol
	{
		get
		{
			return wordWrapSymbol;
		}
		set
		{
			if (wordWrapSymbol != value)
			{
				wordWrapSymbol = value;
				wordWrapString = new string(wordWrapSymbol, 1);
				Update();
			}
		}
	}

	[Description("Gets or sets color used to paint special symbols.")]
	public Color SymbolColor
	{
		get
		{
			return symbolColor;
		}
		set
		{
			if (symbolColor != value)
			{
				symbolColor = value;
				Update();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string SpaceString => spaceString;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string TabString => tabString;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string EolString => eolString;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string EofString => eofString;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string WordWrapString => wordWrapString;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public object XmlInfo
	{
		get
		{
			return new XmlWhiteSpaceInfo(this);
		}
		set
		{
			((XmlWhiteSpaceInfo)value).FixupReferences(this);
		}
	}

	protected void BeginUpdate()
	{
		updateCount++;
	}

	protected void EndUpdate()
	{
		updateCount--;
		if (updateCount == 0)
		{
			Update();
		}
	}

	protected void Update()
	{
		if (updateCount == 0 && owner != null)
		{
			owner.Invalidate();
		}
	}

	public WhiteSpace()
	{
		spaceString = new string(spaceSymbol, 1);
		tabString = new string(tabSymbol, 1);
		eolString = new string(eolSymbol, 1);
		eofString = new string(eofSymbol, 1);
		wordWrapString = new string(wordWrapSymbol, 1);
	}

	public WhiteSpace(ISyntaxEdit owner)
		: this()
	{
		this.owner = owner;
	}

	public bool ShouldSerializeSymbolColor()
	{
		return symbolColor != EditConsts.DefaultWhiteSpaceForeColor;
	}

	public bool ShouldSerializeTabSymbol()
	{
		return tabSymbol != EditConsts.TabSymbol;
	}

	public bool ShouldSerializeSpaceSymbol()
	{
		return spaceSymbol != EditConsts.SpaceSymbol;
	}

	public bool ShouldSerializeEolSymbol()
	{
		return eolSymbol != EditConsts.EolSymbol;
	}

	public bool ShouldSerializeEofSymbol()
	{
		return eofSymbol != EditConsts.EofSymbol;
	}

	public bool ShouldSerializeWordWrapSymbol()
	{
		return wordWrapSymbol != EditConsts.WordWrapSymbol;
	}

	public void Assign(IWhiteSpace source)
	{
		BeginUpdate();
		try
		{
			SpaceSymbol = source.SpaceSymbol;
			TabSymbol = source.TabSymbol;
			EofSymbol = source.EofSymbol;
			EolSymbol = source.EolSymbol;
			WordWrapSymbol = source.WordWrapSymbol;
			Visible = source.Visible;
			SymbolColor = source.SymbolColor;
		}
		finally
		{
			EndUpdate();
		}
	}

	public virtual void ResetTabSymbol()
	{
		TabSymbol = EditConsts.TabSymbol;
	}

	public virtual void ResetSpaceSymbol()
	{
		SpaceSymbol = EditConsts.SpaceSymbol;
	}

	public virtual void ResetEolSymbol()
	{
		EolSymbol = EditConsts.EolSymbol;
	}

	public virtual void ResetEofSymbol()
	{
		EofSymbol = EditConsts.EofSymbol;
	}

	public virtual void ResetWordWrapSymbol()
	{
		WordWrapSymbol = EditConsts.WordWrapSymbol;
	}

	public virtual void ResetSymbolColor()
	{
		SymbolColor = EditConsts.DefaultWhiteSpaceForeColor;
	}

	public virtual void ResetVisible()
	{
		Visible = false;
	}
}
