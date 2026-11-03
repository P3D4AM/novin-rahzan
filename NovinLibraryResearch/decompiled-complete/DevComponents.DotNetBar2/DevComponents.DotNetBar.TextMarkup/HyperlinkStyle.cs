using System.Drawing;

namespace DevComponents.DotNetBar.TextMarkup;

public class HyperlinkStyle
{
	private Color __0CD1 = Color.Empty;

	private Color __0CD2 = Color.Empty;

	private eHyperlinkUnderlineStyle __0CD3;

	public Color TextColor
	{
		get
		{
			return __0CD1;
		}
		set
		{
			if (__0CD1 != value)
			{
				__0CD1 = value;
			}
		}
	}

	public Color BackColor
	{
		get
		{
			return __0CD2;
		}
		set
		{
			if (__0CD2 != value)
			{
				__0CD2 = value;
			}
		}
	}

	public eHyperlinkUnderlineStyle UnderlineStyle
	{
		get
		{
			return __0CD3;
		}
		set
		{
			if (__0CD3 != value)
			{
				__0CD3 = value;
			}
		}
	}

	public bool IsChanged
	{
		get
		{
			if (__0CD1.IsEmpty && __0CD2.IsEmpty)
			{
				return __0CD3 != eHyperlinkUnderlineStyle.None;
			}
			return true;
		}
	}

	public HyperlinkStyle()
	{
	}

	public HyperlinkStyle(Color textColor, eHyperlinkUnderlineStyle underlineStyle)
	{
		__0CD1 = textColor;
		__0CD3 = underlineStyle;
	}

	public HyperlinkStyle(Color textColor, Color backColor, eHyperlinkUnderlineStyle underlineStyle)
	{
		__0CD1 = textColor;
		__0CD2 = backColor;
		__0CD3 = underlineStyle;
	}
}
