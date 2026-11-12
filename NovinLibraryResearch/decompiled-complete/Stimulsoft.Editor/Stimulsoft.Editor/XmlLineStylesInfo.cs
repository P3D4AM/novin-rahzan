namespace Stimulsoft.Editor;

public class XmlLineStylesInfo
{
	private LineStylesEx owner;

	private LineStyle[] styles;

	public LineStyle[] Styles
	{
		get
		{
			if (owner != null)
			{
				LineStyle[] array = new LineStyle[owner.Count];
				for (int i = 0; i < owner.Count; i++)
				{
					array[i] = owner[i];
				}
				return array;
			}
			return styles;
		}
		set
		{
			styles = value;
			if (owner != null)
			{
				owner.Clear();
				foreach (LineStyle value2 in value)
				{
					owner.Add(value2);
				}
			}
		}
	}

	public XmlLineStylesInfo()
	{
	}

	public XmlLineStylesInfo(LineStylesEx owner)
		: this()
	{
		this.owner = owner;
	}

	public virtual void Load()
	{
		if (owner != null)
		{
			styles = Styles;
		}
	}

	public virtual void FixupReferences(LineStylesEx owner)
	{
		this.owner = owner;
		Styles = styles;
	}
}
