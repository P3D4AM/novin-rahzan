using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

public class HyperlinkStyle
{
	private Color _TextColor = Color.Empty;

	private Color _BackColor = Color.Empty;

	private eHyperlinkUnderlineStyle _UnderlineStyle;

	public Color TextColor
	{
		get
		{
			return _TextColor;
		}
		set
		{
			if (_TextColor != value)
			{
				_TextColor = value;
			}
		}
	}

	public Color BackColor
	{
		get
		{
			return _BackColor;
		}
		set
		{
			if (_BackColor != value)
			{
				_BackColor = value;
			}
		}
	}

	public eHyperlinkUnderlineStyle UnderlineStyle
	{
		get
		{
			return _UnderlineStyle;
		}
		set
		{
			if (_UnderlineStyle != value)
			{
				_UnderlineStyle = value;
			}
		}
	}

	public bool IsChanged
	{
		get
		{
			if (_TextColor.IsEmpty && _BackColor.IsEmpty)
			{
				return _UnderlineStyle != eHyperlinkUnderlineStyle.None;
			}
			return true;
		}
	}

	public HyperlinkStyle()
	{
	}

	public HyperlinkStyle(Color textColor, eHyperlinkUnderlineStyle underlineStyle)
	{
		_TextColor = textColor;
		_UnderlineStyle = underlineStyle;
	}

	public HyperlinkStyle(Color textColor, Color backColor, eHyperlinkUnderlineStyle underlineStyle)
	{
		_TextColor = textColor;
		_BackColor = backColor;
		_UnderlineStyle = underlineStyle;
	}
}
