using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
public class StiLabelUX : Label
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
			return StiUX.Foreground;
		}
		set
		{
			if (!StiUX.IsDark)
			{
				((Control)this).ForeColor = value;
			}
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		if (((Control)this).Enabled || !StiUX.IsDark)
		{
			((Label)this).OnPaint(e);
			return;
		}
		Graphics graphics = e.Graphics;
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		StiDrawing.FillRectangle(graphics, ((Control)this).BackColor, clientRectangle);
		TextFormatFlags val = (TextFormatFlags)20;
		TextRenderer.DrawText((IDeviceContext)(object)graphics, ((Control)this).Text, ((Control)this).Font, clientRectangle, StiUX.InputForegroundDisabled, val);
	}
}
