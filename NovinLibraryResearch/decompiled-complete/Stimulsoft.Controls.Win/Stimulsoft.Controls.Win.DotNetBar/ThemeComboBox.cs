using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeComboBox : Themes
{
	protected override string ThemeClass => "COMBOBOX";

	public ThemeComboBox(Control parent)
		: base(parent)
	{
	}

	public void DrawBackground(Graphics g, ThemeComboBoxParts part, ThemeComboBoxStates state, Rectangle r)
	{
		InternalDrawBackground(g, part, state, r);
	}
}
