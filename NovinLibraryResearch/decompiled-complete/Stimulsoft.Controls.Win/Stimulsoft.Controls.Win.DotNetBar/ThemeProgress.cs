using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeProgress : Themes
{
	protected override string ThemeClass => "PROGRESS";

	public ThemeProgress(Control parent)
		: base(parent)
	{
	}

	public void DrawBackground(Graphics g, ThemeProgressParts part, ThemeProgressStates state, Rectangle r)
	{
		InternalDrawBackground(g, part, state, r);
	}
}
