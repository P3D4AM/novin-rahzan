using System.Drawing;
using System.Xml.Serialization;

namespace Stimulsoft.Editor;

public class XmlPageHeaderInfo
{
	private PageHeader owner;

	private string leftText;

	private string rightText;

	private Point offset;

	private bool visible = true;

	private bool reverseOnEvenPages;

	private Font font;

	private string fontName;

	private float fontSize;

	private FontStyle fontStyle;

	public string LeftText
	{
		get
		{
			if (owner == null)
			{
				return leftText;
			}
			return owner.LeftText;
		}
		set
		{
			leftText = value;
			if (owner != null)
			{
				owner.LeftText = value;
			}
		}
	}

	public string RightText
	{
		get
		{
			if (owner == null)
			{
				return rightText;
			}
			return owner.RightText;
		}
		set
		{
			rightText = value;
			if (owner != null)
			{
				owner.RightText = value;
			}
		}
	}

	public Point Offset
	{
		get
		{
			if (owner == null)
			{
				return offset;
			}
			return owner.Offset;
		}
		set
		{
			offset = value;
			if (owner != null)
			{
				owner.Offset = value;
			}
		}
	}

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

	public bool ReverseOnEvenPages
	{
		get
		{
			if (owner == null)
			{
				return reverseOnEvenPages;
			}
			return owner.ReverseOnEvenPages;
		}
		set
		{
			reverseOnEvenPages = value;
			if (owner != null)
			{
				owner.ReverseOnEvenPages = value;
			}
		}
	}

	[XmlIgnore]
	public Font Font
	{
		get
		{
			if (owner == null)
			{
				return font;
			}
			return owner.Font;
		}
		set
		{
			font = value;
			if (owner != null)
			{
				owner.Font = value;
			}
		}
	}

	public string FontName
	{
		get
		{
			if (owner == null)
			{
				return fontName;
			}
			return owner.Font.Name;
		}
		set
		{
			fontName = value;
		}
	}

	public float FontSize
	{
		get
		{
			if (owner == null)
			{
				return fontSize;
			}
			return owner.Font.Size;
		}
		set
		{
			fontSize = value;
		}
	}

	public FontStyle FontStyle
	{
		get
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			if (owner == null)
			{
				return fontStyle;
			}
			return owner.Font.Style;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			fontStyle = value;
		}
	}

	public XmlPageHeaderInfo()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		fontName = FontFamily.GenericMonospace.Name;
		fontSize = 10f;
		fontStyle = (FontStyle)0;
	}

	public XmlPageHeaderInfo(PageHeader owner)
		: this()
	{
		this.owner = owner;
	}

	public virtual void Load()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		if (owner != null)
		{
			fontName = Font.Name;
			fontSize = Font.Size;
			fontStyle = Font.Style;
			leftText = LeftText;
			offset = Offset;
			reverseOnEvenPages = ReverseOnEvenPages;
			rightText = RightText;
			visible = Visible;
		}
	}

	public virtual void FixupReferences(PageHeader owner)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		this.owner = owner;
		Font = new Font(fontName, fontSize, fontStyle);
		LeftText = leftText;
		Offset = offset;
		ReverseOnEvenPages = reverseOnEvenPages;
		RightText = rightText;
		Visible = visible;
	}
}
