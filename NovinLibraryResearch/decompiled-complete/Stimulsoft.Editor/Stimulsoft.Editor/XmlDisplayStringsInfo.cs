namespace Stimulsoft.Editor;

public class XmlDisplayStringsInfo
{
	private DisplayStrings owner;

	private bool wordWrap;

	private bool wrapAtMargin;

	public bool WordWrap
	{
		get
		{
			if (owner == null)
			{
				return wordWrap;
			}
			return owner.WordWrap;
		}
		set
		{
			wordWrap = value;
			if (owner != null)
			{
				owner.WordWrap = value;
			}
		}
	}

	public bool WrapAtMargin
	{
		get
		{
			if (owner == null)
			{
				return wrapAtMargin;
			}
			return owner.WrapAtMargin;
		}
		set
		{
			wrapAtMargin = value;
			if (owner != null)
			{
				owner.WrapAtMargin = value;
			}
		}
	}

	public XmlDisplayStringsInfo()
	{
	}

	public XmlDisplayStringsInfo(DisplayStrings owner)
		: this()
	{
		this.owner = owner;
	}

	public virtual void Load()
	{
		if (owner != null)
		{
			wordWrap = WordWrap;
			wrapAtMargin = WrapAtMargin;
		}
	}

	public virtual void FixupReferences(DisplayStrings owner)
	{
		this.owner = owner;
		WordWrap = wordWrap;
		WrapAtMargin = wrapAtMargin;
	}
}
