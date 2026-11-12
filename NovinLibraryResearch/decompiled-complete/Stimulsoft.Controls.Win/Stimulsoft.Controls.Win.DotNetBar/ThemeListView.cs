using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeListView : Themes
{
	protected override string ThemeClass => "LISTVIEW";

	public ThemeListView(Control parent)
		: base(parent)
	{
	}

	public void DrawBackground(Graphics g, ThemeListViewParts part, ThemeListViewStates state, Rectangle r)
	{
		InternalDrawBackground(g, part, state, r);
	}
}
