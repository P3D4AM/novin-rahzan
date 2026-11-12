using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeRebar : Themes
{
	protected override string ThemeClass => "REBAR";

	public ThemeRebar(Control parent)
		: base(parent)
	{
	}

	public void DrawBackground(Graphics g, ThemeRebarParts part, ThemeRebarStates state, Rectangle r)
	{
		InternalDrawBackground(g, part, state, r);
	}
}
