using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

internal interface IActiveMarkupElement
{
	bool HitTest(int x, int y);

	void MouseEnter(Control parent);

	void MouseLeave(Control parent);

	void MouseDown(Control parent, MouseEventArgs e);

	void MouseUp(Control parent, MouseEventArgs e);

	void Click(Control parent);
}
