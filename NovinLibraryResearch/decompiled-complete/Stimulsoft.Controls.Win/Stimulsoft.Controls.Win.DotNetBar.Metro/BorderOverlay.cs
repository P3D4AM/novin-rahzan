using System;
using System.ComponentModel;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Metro.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro;

[ToolboxItem(false)]
internal class BorderOverlay : Control
{
	public BorderOverlay()
	{
		((Control)this).SetStyle((ControlStyles)74262, true);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		MetroRender.Paint(this, e);
		((Control)this).OnPaint(e);
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 132)
		{
			((Message)(ref m)).Result = new IntPtr(-1);
		}
		else
		{
			((Control)this).WndProc(ref m);
		}
	}
}
