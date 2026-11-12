using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeTaskBand : Themes
{
	protected override string ThemeClass => "TASKBAND";

	public ThemeTaskBand(Control parent)
		: base(parent)
	{
	}

	public void DrawBackground(Graphics g, ThemeTaskBandParts part, ThemeTaskBandStates state, Rectangle r)
	{
		InternalDrawBackground(g, part, state, r);
	}
}
