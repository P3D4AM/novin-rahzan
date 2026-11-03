using System.Drawing;

namespace DevComponents.DotNetBar.Rendering;

public class TabCloseButtonColorTable
{
	public TabCloseButtonStateColorTable Normal = new TabCloseButtonStateColorTable();

	public TabCloseButtonStateColorTable MouseOver = new TabCloseButtonStateColorTable(new Color[1] { ColorScheme.GetColor(14369590) }, ColorScheme.GetColor(16645114), ColorScheme.GetColor(14369590));

	public TabCloseButtonStateColorTable Pressed = new TabCloseButtonStateColorTable(new Color[1] { ColorScheme.GetColor(11023658) }, ColorScheme.GetColor(16645114), ColorScheme.GetColor(11023658));
}
