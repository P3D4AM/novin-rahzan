namespace Stimulsoft.Editor;

public class XmlLineSeparatorInfo
{
	private LineSeparator owner;

	private SeparatorOptions options;

	private string highlightBackColor = EditConsts.DefaultLineSeparatorColor.Name;

	private string highlightForeColor = string.Empty;

	private string lineColor = EditConsts.DefaultLineSeparatorLineColor.Name;

	public SeparatorOptions Options
	{
		get
		{
			if (owner == null)
			{
				return options;
			}
			return owner.Options;
		}
		set
		{
			options = value;
			if (owner != null)
			{
				owner.Options = value;
			}
		}
	}

	public string HighlightBackColor
	{
		get
		{
			if (owner == null)
			{
				return highlightBackColor;
			}
			return XmlHelper.SerializeColor(owner.HighlightBackColor);
		}
		set
		{
			highlightBackColor = value;
			if (owner != null)
			{
				owner.HighlightBackColor = XmlHelper.DeserializeColor(value);
			}
		}
	}

	public string HighlightForeColor
	{
		get
		{
			if (owner == null)
			{
				return highlightForeColor;
			}
			return XmlHelper.SerializeColor(owner.HighlightForeColor);
		}
		set
		{
			highlightForeColor = value;
			if (owner != null)
			{
				owner.HighlightForeColor = XmlHelper.DeserializeColor(value);
			}
		}
	}

	public string LineColor
	{
		get
		{
			if (owner == null)
			{
				return lineColor;
			}
			return XmlHelper.SerializeColor(owner.LineColor);
		}
		set
		{
			lineColor = value;
			if (owner != null)
			{
				owner.LineColor = XmlHelper.DeserializeColor(value);
			}
		}
	}

	public XmlLineSeparatorInfo()
	{
	}

	public XmlLineSeparatorInfo(LineSeparator owner)
		: this()
	{
		this.owner = owner;
	}

	public virtual void Load()
	{
		if (owner != null)
		{
			options = Options;
			highlightBackColor = HighlightBackColor;
			highlightForeColor = HighlightForeColor;
			lineColor = LineColor;
		}
	}

	public virtual void FixupReferences(LineSeparator owner)
	{
		this.owner = owner;
		Options = options;
		HighlightBackColor = highlightBackColor;
		HighlightForeColor = highlightForeColor;
		LineColor = lineColor;
	}
}
