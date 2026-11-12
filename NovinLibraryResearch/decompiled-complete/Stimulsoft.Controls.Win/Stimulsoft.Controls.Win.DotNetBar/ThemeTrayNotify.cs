using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeTrayNotify : Themes
{
	protected override string ThemeClass => "TRAYNOTIFY";

	public ThemeTrayNotify(Control parent)
		: base(parent)
	{
	}

	public void DrawBackground(Graphics g, ThemeTrayNotifyParts part, ThemeTrayNotifyStates state, Rectangle r)
	{
		InternalDrawBackground(g, part, state, r);
	}
}
