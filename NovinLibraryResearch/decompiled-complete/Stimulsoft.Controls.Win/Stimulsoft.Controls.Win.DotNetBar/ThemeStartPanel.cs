using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeStartPanel : Themes
{
	protected override string ThemeClass => "STARTPANEL";

	public ThemeStartPanel(Control parent)
		: base(parent)
	{
	}

	public void DrawBackground(Graphics g, ThemeStartPanelParts part, ThemeStartPanelStates state, Rectangle r)
	{
		InternalDrawBackground(g, part, state, r);
	}
}
