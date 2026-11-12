using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemePage : Themes
{
	protected override string ThemeClass => "PAGE";

	public ThemePage(Control parent)
		: base(parent)
	{
	}

	public void DrawBackground(Graphics g, ThemePageParts part, ThemePageStates state, Rectangle r)
	{
		InternalDrawBackground(g, part, state, r);
	}
}
