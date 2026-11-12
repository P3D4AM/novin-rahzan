using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeMenuBand : Themes
{
	protected override string ThemeClass => "MENUBAND";

	public ThemeMenuBand(Control parent)
		: base(parent)
	{
	}

	public void DrawBackground(Graphics g, ThemeMenuBandParts part, ThemeMenuBandStates state, Rectangle r)
	{
		InternalDrawBackground(g, part, state, r);
	}
}
