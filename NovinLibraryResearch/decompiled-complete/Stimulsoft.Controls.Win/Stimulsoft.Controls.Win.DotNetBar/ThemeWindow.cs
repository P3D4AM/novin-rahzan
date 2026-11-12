using System;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeWindow : Themes
{
	protected override string ThemeClass => "WINDOW";

	public ThemeWindow(Control parent)
		: base(parent)
	{
	}

	public void DrawBackground(Graphics g, ThemeWindowParts part, ThemeWindowStates state, Rectangle r)
	{
		InternalDrawBackground(g, part, state, r);
	}

	public void DrawText(Graphics g, string text, Font font, Rectangle layoutRect, ThemeWindowParts part, ThemeWindowStates state, ThemeTextFormat format, bool drawdisabled)
	{
		InternalDrawText(g, text, font, layoutRect, part, state, format, drawdisabled);
	}

	public void DrawText(Graphics g, string text, Font font, Rectangle layoutRect, ThemeWindowParts part, ThemeWindowStates state, ThemeTextFormat format)
	{
		InternalDrawText(g, text, font, layoutRect, part, state, format, drawdisabled: false);
	}

	public void DrawText(Graphics g, string text, Font font, Rectangle layoutRect, ThemeWindowParts part, ThemeWindowStates state)
	{
		InternalDrawText(g, text, font, layoutRect, part, state, ThemeTextFormat.Top, drawdisabled: false);
	}

	public void DrawTextEx(Graphics g, string text, Font font, Rectangle layoutRect, ThemePart part, ThemeState state, ThemeTextFormat format, DTTOPTS options)
	{
		InternalDrawTextEx(g, text, font, layoutRect, part, state, format, options);
	}

	public IntPtr GetThemeBackgroundRegion(Graphics g, ThemeWindowParts part, ThemeWindowStates state, Rectangle r)
	{
		return InternalGetThemeBackgroundRegion(g, part, state, r);
	}
}
