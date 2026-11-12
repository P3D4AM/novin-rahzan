using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeButton : Themes
{
	protected override string ThemeClass => "Button";

	public ThemeButton(Control parent)
		: base(parent)
	{
	}

	public void DrawBackground(Graphics g, ThemeButtonParts part, ThemeButtonStates state, Rectangle r)
	{
		InternalDrawBackground(g, part, state, r);
	}

	public void DrawText(Graphics g, string text, Font font, Rectangle layoutRect, ThemeButtonParts part, ThemeButtonStates state, ThemeTextFormat format, bool drawdisabled)
	{
		InternalDrawText(g, text, font, layoutRect, part, state, format, drawdisabled);
	}

	public void DrawText(Graphics g, string text, Font font, Rectangle layoutRect, ThemeButtonParts part, ThemeButtonStates state, ThemeTextFormat format)
	{
		InternalDrawText(g, text, font, layoutRect, part, state, format, drawdisabled: false);
	}

	public void DrawText(Graphics g, string text, Font font, Rectangle layoutRect, ThemeButtonParts part, ThemeButtonStates state)
	{
		InternalDrawText(g, text, font, layoutRect, part, state, ThemeTextFormat.Top, drawdisabled: false);
	}
}
