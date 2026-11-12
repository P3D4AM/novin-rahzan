using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Components.Design;

[ToolboxItem(false)]
public class StiPanelBox : Panel
{
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Color ForeColor
	{
		get
		{
			if (!StiUX.IsDark)
			{
				return ((Control)this).ForeColor;
			}
			return StiUX.InputForeground;
		}
		set
		{
			if (!StiUX.IsDark)
			{
				((Control)this).ForeColor = value;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Color BackColor
	{
		get
		{
			if (!StiUX.IsDark)
			{
				return ((Control)this).BackColor;
			}
			return StiUX.InputBackground;
		}
		set
		{
			if (!StiUX.IsDark)
			{
				((Control)this).BackColor = value;
			}
		}
	}

	protected override void OnPaintBackground(PaintEventArgs e)
	{
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		((Control)this).OnPaint(e);
		e.Graphics.Clear(((Control)this).BackColor);
		e.Graphics.DrawRectangle(SystemPens.ControlDark, 0, 0, ((Control)this).Width - 1, ((Control)this).Height - 1);
	}

	public StiPanelBox()
	{
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)65536, true);
		((Control)this).BackColor = StiUX.InputBackground;
	}
}
