using System;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class MDIClientMsgHandler : NativeWindow
{
	private const int WM_MDISETMENU = 560;

	private const int WM_MDIREFRESHMENU = 564;

	public bool EatMessage;

	public event EventHandler MdiSetMenu;

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 560 || ((Message)(ref m)).Msg == 564)
		{
			if (this.MdiSetMenu != null)
			{
				this.MdiSetMenu(this, new EventArgs());
			}
			if (EatMessage)
			{
				EatMessage = false;
				return;
			}
		}
		((NativeWindow)this).WndProc(ref m);
	}
}
