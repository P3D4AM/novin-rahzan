using System.ComponentModel;
using System.Windows.Forms;
using Stimulsoft.Controls;

namespace Stimulsoft.Report.Design.Controls;

[ToolboxItem(false)]
public class StiMouseScrollableControl : Stimulsoft.Controls.StiScrollableControl
{
	protected override void OnMouseWheel(MouseEventArgs e)
	{
		if (e.Delta > 0)
		{
			base.VScrollBar.DoUp();
		}
		else
		{
			base.VScrollBar.DoDown();
		}
	}
}
