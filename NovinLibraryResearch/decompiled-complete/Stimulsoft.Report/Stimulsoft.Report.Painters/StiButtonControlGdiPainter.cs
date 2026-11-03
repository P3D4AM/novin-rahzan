using System;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Painters;

public class StiButtonControlGdiPainter : StiReportControlGdiPainter
{
	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		StiButtonControl stiButtonControl = component as StiButtonControl;
		stiButtonControl.InvokePainting(stiButtonControl, e);
		if (!e.Cancel && (stiButtonControl.Enabled || stiButtonControl.IsDesigning))
		{
			RectangleD paintRectangle = stiButtonControl.GetPaintRectangle();
			if (((RectangleD)(ref paintRectangle)).Width > 0.0 && ((RectangleD)(ref paintRectangle)).Height > 0.0)
			{
				RectangleD clipRectangle = e.ClipRectangle;
				if (((RectangleD)(ref clipRectangle)).IsEmpty || ((RectangleD)(ref paintRectangle)).IntersectsWith(e.ClipRectangle))
				{
					Button val = new Button();
					try
					{
						((Control)val).Text = stiButtonControl.Text;
						((ButtonBase)val).Image = stiButtonControl.Image;
						((ButtonBase)val).TextAlign = stiButtonControl.TextAlign;
						((ButtonBase)val).ImageAlign = stiButtonControl.ImageAlign;
						DrawControl(component as StiReportControl, e.Graphics, (Control)(object)val, paintRectangle);
						((ButtonBase)val).Image = null;
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
					PaintEvents(stiButtonControl, e.Graphics, paintRectangle);
					PaintConditions(stiButtonControl, e.Graphics, paintRectangle);
				}
			}
		}
		e.Cancel = false;
		stiButtonControl.InvokePainted(stiButtonControl, e);
	}
}
