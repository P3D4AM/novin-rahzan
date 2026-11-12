using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win;

[ToolboxItem(false)]
public class StiHitTestVisibleLabel : Label
{
	private const int WM_NCHITTEST = 132;

	private const int HTTRANSPARENT = -1;

	protected override void WndProc(ref Message message)
	{
		if (((Message)(ref message)).Msg == 132 && !((Component)this).DesignMode)
		{
			((Message)(ref message)).Result = (IntPtr)(-1);
		}
		else
		{
			((Label)this).WndProc(ref message);
		}
	}
}
