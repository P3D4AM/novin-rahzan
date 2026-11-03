using System.Drawing;

namespace DevComponents.DotNetBar.TextMarkup;

public class MarkupDrawContext
{
	public Graphics Graphics;

	public Font CurrentFont;

	public Color CurrentForeColor = SystemColors.ControlText;

	public bool RightToLeft;

	public Point Offset = Point.Empty;

	public bool HyperLink;

	public HyperlinkStyle HyperlinkStyle;

	public bool Underline;

	public Rectangle ClipRectangle = Rectangle.Empty;

	public bool HotKeyPrefixVisible;

	public object ContextObject;

	public bool AllowMultiLine = true;

	public bool IgnoreFormattingColors;

	public bool StrikeOut;

	public MarkupDrawContext(Graphics g, Font currentFont, Color currentForeColor, bool rightToLeft)
		: this(g, currentFont, currentForeColor, rightToLeft, Rectangle.Empty, hotKeyPrefixVisible: false)
	{
	}

	public MarkupDrawContext(Graphics g, Font currentFont, Color currentForeColor, bool rightToLeft, Rectangle clipRectangle, bool hotKeyPrefixVisible)
	{
		Graphics = g;
		CurrentFont = currentFont;
		CurrentForeColor = currentForeColor;
		RightToLeft = rightToLeft;
		ClipRectangle = clipRectangle;
		HotKeyPrefixVisible = hotKeyPrefixVisible;
	}

	public MarkupDrawContext(Graphics g, Font currentFont, Color currentForeColor, bool rightToLeft, Rectangle clipRectangle, bool hotKeyPrefixVisible, object contextObject)
	{
		Graphics = g;
		CurrentFont = currentFont;
		CurrentForeColor = currentForeColor;
		RightToLeft = rightToLeft;
		ClipRectangle = clipRectangle;
		HotKeyPrefixVisible = hotKeyPrefixVisible;
		ContextObject = contextObject;
	}
}
