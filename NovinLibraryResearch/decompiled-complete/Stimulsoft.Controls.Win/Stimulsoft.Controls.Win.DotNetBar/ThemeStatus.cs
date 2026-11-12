using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeStatus : Themes
{
	protected override string ThemeClass => "STATUS";

	public ThemeStatus(Control parent)
		: base(parent)
	{
	}

	public void DrawBackground(Graphics g, ThemeStatusParts part, ThemeStatusStates state, Rectangle r)
	{
		InternalDrawBackground(g, part, state, r);
	}
}
