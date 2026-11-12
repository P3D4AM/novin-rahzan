using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeScrollBar : Themes
{
	protected override string ThemeClass => "SCROLLBAR";

	public ThemeScrollBar(Control parent)
		: base(parent)
	{
	}

	public void DrawBackground(Graphics g, ThemeScrollBarParts part, ThemeScrollBarStates state, Rectangle r)
	{
		InternalDrawBackground(g, part, state, r);
	}
}
