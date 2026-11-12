namespace Stimulsoft.Editor;

public class XmlSpellingInfo
{
	private Spelling owner;

	private string spellColor = EditConsts.DefaultSpellForeColor.Name;

	public string SpellColor
	{
		get
		{
			if (owner == null)
			{
				return spellColor;
			}
			return XmlHelper.SerializeColor(owner.SpellColor);
		}
		set
		{
			spellColor = value;
			if (owner != null)
			{
				owner.SpellColor = XmlHelper.DeserializeColor(value);
			}
		}
	}

	public XmlSpellingInfo()
	{
	}

	public XmlSpellingInfo(Spelling owner)
		: this()
	{
		this.owner = owner;
	}

	public virtual void Load()
	{
		if (owner != null)
		{
			spellColor = SpellColor;
		}
	}

	public virtual void FixupReferences(Spelling owner)
	{
		this.owner = owner;
		SpellColor = spellColor;
	}
}
