using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls.Menu;

[ToolboxItem(false)]
public class StiContextMenu : ContextMenuStrip
{
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsDarkMode { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool CloseOnClick { get; set; }

	protected override void OnPaint(PaintEventArgs e)
	{
		Graphics graphics = e.Graphics;
		Rectangle rectangle = new Rectangle(0, 0, ((Control)this).Width - 1, ((Control)this).Height - 1);
		Color color = (IsDarkMode ? StiColor.Get("2b2b2b") : StiColor.Get("ffffff"));
		StiDrawing.FillRectangle(graphics, color, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
		((ToolStrip)this).OnPaint(e);
		Color color2 = (IsDarkMode ? StiColor.Get("a0a0a0") : StiColor.Get("aaaaaa"));
		StiDrawing.DrawRectangle(graphics, color2, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
	}

	protected override void OnClosing(ToolStripDropDownClosingEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		((ToolStripDropDown)this).OnClosing(e);
		if ((int)e.CloseReason == 2 && !CloseOnClick)
		{
			((CancelEventArgs)(object)e).Cancel = true;
		}
	}

	public StiContextMenu()
	{
		((ToolStripDropDownMenu)this).ShowImageMargin = false;
		((ToolStripDropDownMenu)this).ShowCheckMargin = false;
	}
}
