using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeTreeView : Themes
{
	protected override string ThemeClass => "TREEVIEW";

	public ThemeTreeView(Control parent)
		: base(parent)
	{
	}

	public void DrawBackground(Graphics g, ThemeTreeViewParts part, ThemeTreeViewStates state, Rectangle r)
	{
		InternalDrawBackground(g, part, state, r);
	}
}
