using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeTab : Themes
{
	protected override string ThemeClass => "TAB";

	public ThemeTab(Control parent)
		: base(parent)
	{
	}

	public void DrawBackground(Graphics g, ThemeTabParts part, ThemeTabStates state, Rectangle r)
	{
		InternalDrawBackground(g, part, state, r);
	}
}
