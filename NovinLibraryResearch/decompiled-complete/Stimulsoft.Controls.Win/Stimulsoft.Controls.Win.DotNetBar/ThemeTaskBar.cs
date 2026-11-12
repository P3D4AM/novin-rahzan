using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeTaskBar : Themes
{
	protected override string ThemeClass => "TASKBAR";

	public ThemeTaskBar(Control parent)
		: base(parent)
	{
	}

	public void DrawBackground(Graphics g, ThemeTaskBarParts part, ThemeTaskBarStates state, Rectangle r)
	{
		InternalDrawBackground(g, part, state, r);
	}
}
