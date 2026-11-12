using System.ComponentModel;
using System.Drawing;

namespace Stimulsoft.Editor;

public class Spelling : ISpellingEx, ISpelling
{
	private Color spellColor = EditConsts.DefaultSpellForeColor;

	private ISyntaxEdit owner;

	[DefaultValue(false)]
	[Description("Gets or sets a value indicating whether the document can check spelling for its content.")]
	public bool CheckSpelling
	{
		get
		{
			if (owner == null)
			{
				return false;
			}
			return owner.Source.CheckSpelling;
		}
		set
		{
			if (owner != null)
			{
				owner.Source.CheckSpelling = value;
			}
		}
	}

	[Description("Gets or sets a value representing color to draw wavy underlines under mispelled words.")]
	public Color SpellColor
	{
		get
		{
			return spellColor;
		}
		set
		{
			if (spellColor != value)
			{
				spellColor = value;
				if (CheckSpelling || owner != null)
				{
					owner.Invalidate();
				}
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public object XmlInfo
	{
		get
		{
			return new XmlSpellingInfo(this);
		}
		set
		{
			((XmlSpellingInfo)value).FixupReferences(this);
		}
	}

	[Browsable(false)]
	public event WordSpellEvent WordSpell
	{
		add
		{
			if (owner != null)
			{
				owner.Source.WordSpell += value;
			}
		}
		remove
		{
			if (owner != null)
			{
				owner.Source.WordSpell -= value;
			}
		}
	}

	public Spelling(ISyntaxEdit owner)
	{
		this.owner = owner;
	}

	public bool IsWordCorrect(string text)
	{
		if (owner == null)
		{
			return true;
		}
		return owner.Source.IsWordCorrect(text);
	}

	public void Assign(ISpellingEx source)
	{
		CheckSpelling = source.CheckSpelling;
		SpellColor = source.SpellColor;
	}

	public virtual void ResetSpellColor()
	{
		SpellColor = EditConsts.DefaultSpellForeColor;
	}

	public virtual void ResetCheckSpelling()
	{
		CheckSpelling = false;
	}

	public bool ShouldSerializeSpellColor()
	{
		return spellColor != EditConsts.DefaultSpellForeColor;
	}
}
