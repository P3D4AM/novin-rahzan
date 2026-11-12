using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
public class StiFormUX : Form
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
			return StiUX.ButtonForeground;
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
			return StiUX.Background;
		}
		set
		{
			if (!StiUX.IsDark)
			{
				((Form)this).BackColor = value;
			}
		}
	}

	protected override void OnPaintBackground(PaintEventArgs e)
	{
		if (StiUX.IsDark)
		{
			e.Graphics.Clear(StiUX.Background);
		}
		else
		{
			((ScrollableControl)this).OnPaintBackground(e);
		}
	}
}
