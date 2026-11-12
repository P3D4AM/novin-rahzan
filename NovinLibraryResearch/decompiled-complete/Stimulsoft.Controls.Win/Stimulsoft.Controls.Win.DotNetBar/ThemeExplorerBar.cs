using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeExplorerBar : Themes
{
	protected override string ThemeClass => "EXPLORERBAR";

	public ThemeExplorerBar(Control parent)
		: base(parent)
	{
	}

	public void DrawBackground(Graphics g, ThemeExplorerBarParts part, ThemeExplorerBarStates state, Rectangle r)
	{
		InternalDrawBackground(g, part, state, r);
	}

	public void DrawText(Graphics g, string text, Font font, Rectangle layoutRect, ThemeExplorerBarParts part, ThemeExplorerBarStates state, ThemeTextFormat format, bool drawdisabled)
	{
		InternalDrawText(g, text, font, layoutRect, part, state, format, drawdisabled);
	}

	public void DrawText(Graphics g, string text, Font font, Rectangle layoutRect, ThemeExplorerBarParts part, ThemeExplorerBarStates state, ThemeTextFormat format)
	{
		InternalDrawText(g, text, font, layoutRect, part, state, format, drawdisabled: false);
	}

	public void DrawText(Graphics g, string text, Font font, Rectangle layoutRect, ThemeExplorerBarParts part, ThemeExplorerBarStates state)
	{
		InternalDrawText(g, text, font, layoutRect, part, state, ThemeTextFormat.Top, drawdisabled: false);
	}
}
