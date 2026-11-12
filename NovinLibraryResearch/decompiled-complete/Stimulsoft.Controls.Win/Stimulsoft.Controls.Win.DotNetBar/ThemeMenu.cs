using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeMenu : Themes
{
	protected override string ThemeClass => "MENU";

	public ThemeMenu(Control parent)
		: base(parent)
	{
	}

	public void DrawBackground(Graphics g, ThemeMenuParts part, ThemeMenuStates state, Rectangle r)
	{
		InternalDrawBackground(g, part, state, r);
	}
}
