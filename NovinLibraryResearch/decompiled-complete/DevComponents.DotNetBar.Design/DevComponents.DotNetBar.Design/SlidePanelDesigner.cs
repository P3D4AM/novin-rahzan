using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.DotNetBar.Design;

[ToolboxItemFilter("System.Windows.Forms.UserControl", ToolboxItemFilterType.Custom)]
[ToolboxItemFilter("System.Windows.Forms.MainMenu", ToolboxItemFilterType.Prevent)]
public class SlidePanelDesigner : ScrollableControlDesigner
{
	protected Pen BorderPen
	{
		get
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Expected O, but got Unknown
			Color color = (((double)((ControlDesigner)this).Control.BackColor.GetBrightness() < 0.5) ? ControlPaint.Light(((ControlDesigner)this).Control.BackColor) : ControlPaint.Dark(((ControlDesigner)this).Control.BackColor));
			Pen val = new Pen(color);
			val.DashStyle = (DashStyle)1;
			return val;
		}
	}

	public SlidePanelDesigner()
	{
		((ControlDesigner)this).AutoResizeHandles = true;
	}

	protected virtual void DrawBorder(Graphics graphics)
	{
		SlidePanel slidePanel = (SlidePanel)((ComponentDesigner)this).Component;
		if (slidePanel != null && ((Control)slidePanel).Visible)
		{
			Pen borderPen = BorderPen;
			Rectangle clientRectangle = ((ControlDesigner)this).Control.ClientRectangle;
			clientRectangle.Width--;
			clientRectangle.Height--;
			graphics.DrawRectangle(borderPen, clientRectangle);
			borderPen.Dispose();
		}
	}

	protected override void OnPaintAdornments(PaintEventArgs pe)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		SlidePanel slidePanel = (SlidePanel)((ComponentDesigner)this).Component;
		if ((int)((UserControl)slidePanel).BorderStyle == 0)
		{
			DrawBorder(pe.Graphics);
		}
		((ParentControlDesigner)this).OnPaintAdornments(pe);
	}
}
