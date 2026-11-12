using System.Drawing;
using System.Windows.Forms.Design;

namespace DevComponents.DotNetBar.Design;

public class DateNavigatorDesigner : PanelExDesigner
{
	protected override void SetDesignTimeDefaults()
	{
		if (((ControlDesigner)this).Control is PanelEx panelEx)
		{
			panelEx.CanvasColor = SystemColors.Control;
		}
	}
}
