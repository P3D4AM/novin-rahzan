using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeTrackbar : Themes
{
	protected override string ThemeClass => "TRACKBAR";

	public ThemeTrackbar(Control parent)
		: base(parent)
	{
	}

	public void DrawBackground(Graphics g, ThemeTrackbarParts part, ThemeTrackbarStates state, Rectangle r)
	{
		InternalDrawBackground(g, part, state, r);
	}
}
