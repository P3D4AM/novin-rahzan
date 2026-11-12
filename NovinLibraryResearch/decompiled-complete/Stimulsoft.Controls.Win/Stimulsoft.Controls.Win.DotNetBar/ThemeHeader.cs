using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeHeader : Themes
{
	protected override string ThemeClass => "HEADER";

	public ThemeHeader(Control parent)
		: base(parent)
	{
	}

	public void DrawBackground(Graphics g, ThemeHeaderParts part, ThemeHeaderStates state, Rectangle r)
	{
		InternalDrawBackground(g, part, state, r);
	}

	public void DrawText(Graphics g, string text, Font font, Rectangle layoutRect, ThemeHeaderParts part, ThemeHeaderStates state, ThemeTextFormat format, bool drawdisabled)
	{
		InternalDrawText(g, text, font, layoutRect, part, state, format, drawdisabled);
	}

	public void DrawText(Graphics g, string text, Font font, Rectangle layoutRect, ThemeHeaderParts part, ThemeHeaderStates state, ThemeTextFormat format)
	{
		InternalDrawText(g, text, font, layoutRect, part, state, format, drawdisabled: false);
	}

	public void DrawText(Graphics g, string text, Font font, Rectangle layoutRect, ThemeHeaderParts part, ThemeHeaderStates state)
	{
		InternalDrawText(g, text, font, layoutRect, part, state, ThemeTextFormat.Top, drawdisabled: false);
	}
}
