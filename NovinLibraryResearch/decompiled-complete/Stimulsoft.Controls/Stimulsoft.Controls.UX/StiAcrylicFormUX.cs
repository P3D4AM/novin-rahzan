using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.UX;

[ToolboxItem(false)]
public class StiAcrylicFormUX : Form
{
	protected override void OnHandleCreated(EventArgs e)
	{
		StiAcrylicWindowsHelper.EnableAcrylic(((Control)this).Handle, Color.Transparent);
		((Form)this).OnHandleCreated(e);
	}

	protected override void OnPaintBackground(PaintEventArgs e)
	{
		e.Graphics.Clear(Color.Transparent);
	}
}
