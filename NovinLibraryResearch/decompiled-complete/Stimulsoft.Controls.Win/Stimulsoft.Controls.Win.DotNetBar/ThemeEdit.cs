using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeEdit : Themes
{
	protected override string ThemeClass => "EDIT";

	public ThemeEdit(Control parent)
		: base(parent)
	{
	}

	public void DrawBackground(Graphics g, ThemeEditParts part, ThemeEditStates state, Rectangle r)
	{
		InternalDrawBackground(g, part, state, r);
	}
}
