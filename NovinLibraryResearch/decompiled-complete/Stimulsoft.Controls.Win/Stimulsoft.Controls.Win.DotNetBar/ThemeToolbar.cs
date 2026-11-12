using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeToolbar : Themes
{
	protected override string ThemeClass => "Toolbar";

	public ThemeToolbar(Control parent)
		: base(parent)
	{
	}

	public void DrawBackground(Graphics g, ThemeToolbarParts part, ThemeToolbarStates state, Rectangle r)
	{
		InternalDrawBackground(g, part, state, r);
	}

	public void DrawText(Graphics g, string text, Font font, Rectangle layoutRect, ThemeToolbarParts part, ThemeToolbarStates state, ThemeTextFormat format, bool drawdisabled)
	{
		InternalDrawText(g, text, font, layoutRect, part, state, format, drawdisabled);
	}

	public void DrawText(Graphics g, string text, Font font, Rectangle layoutRect, ThemeToolbarParts part, ThemeToolbarStates state, ThemeTextFormat format)
	{
		InternalDrawText(g, text, font, layoutRect, part, state, format, drawdisabled: false);
	}

	public void DrawText(Graphics g, string text, Font font, Rectangle layoutRect, ThemeToolbarParts part, ThemeToolbarStates state)
	{
		InternalDrawText(g, text, font, layoutRect, part, state, ThemeTextFormat.Top, drawdisabled: false);
	}
}
