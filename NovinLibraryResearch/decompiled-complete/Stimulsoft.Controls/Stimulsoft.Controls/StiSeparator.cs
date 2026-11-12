using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
public class StiSeparator : Control
{
	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		((Control)this).OnPaint(e);
		if (((Control)this).Width > 6)
		{
			Graphics graphics = e.Graphics;
			Color color = Color.FromArgb(160, 160, 160);
			graphics.DrawLine(new Pen(color), 3, 2, ((Control)this).Width - 3, 2);
		}
	}

	public StiSeparator()
	{
		((Control)this).MinimumSize = new Size(0, 5);
		((Control)this).MaximumSize = new Size(0, 5);
	}
}
