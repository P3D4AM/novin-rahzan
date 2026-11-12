using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeSpin : Themes
{
	protected override string ThemeClass => "SPIN";

	public ThemeSpin(Control parent)
		: base(parent)
	{
	}

	public void DrawBackground(Graphics g, ThemeSpinParts part, ThemeSpinStates state, Rectangle r)
	{
		InternalDrawBackground(g, part, state, r);
	}
}
