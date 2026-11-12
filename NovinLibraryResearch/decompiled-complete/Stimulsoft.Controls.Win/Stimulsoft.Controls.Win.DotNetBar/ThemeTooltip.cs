using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeTooltip : Themes
{
	protected override string ThemeClass => "TOOLTIP";

	public ThemeTooltip(Control parent)
		: base(parent)
	{
	}

	public void DrawBackground(Graphics g, ThemeTooltipParts part, ThemeTooltipStates state, Rectangle r)
	{
		InternalDrawBackground(g, part, state, r);
	}
}
