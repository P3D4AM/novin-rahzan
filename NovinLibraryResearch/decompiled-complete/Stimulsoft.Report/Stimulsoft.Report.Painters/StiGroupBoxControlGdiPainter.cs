using System;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Painters;

public class StiGroupBoxControlGdiPainter : StiReportControlGdiPainter
{
	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		StiGroupBoxControl stiGroupBoxControl = component as StiGroupBoxControl;
		stiGroupBoxControl.InvokePainting(stiGroupBoxControl, e);
		if (!e.Cancel && (stiGroupBoxControl.Enabled || stiGroupBoxControl.IsDesigning))
		{
			Graphics graphics = e.Graphics;
			RectangleD paintRectangle = stiGroupBoxControl.GetPaintRectangle();
			if (((RectangleD)(ref paintRectangle)).Width > 0.0 && ((RectangleD)(ref paintRectangle)).Height > 0.0)
			{
				RectangleD clipRectangle = e.ClipRectangle;
				if (((RectangleD)(ref clipRectangle)).IsEmpty || ((RectangleD)(ref paintRectangle)).IntersectsWith(e.ClipRectangle))
				{
					GroupBox val = new GroupBox();
					try
					{
						((Control)val).Text = stiGroupBoxControl.Text;
						DrawControl(stiGroupBoxControl, e.Graphics, (Control)(object)val, paintRectangle);
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
					PaintEvents(stiGroupBoxControl, e.Graphics, paintRectangle);
					PaintConditions(stiGroupBoxControl, e.Graphics, paintRectangle);
				}
			}
		}
		e.Cancel = false;
		stiGroupBoxControl.InvokePainted(stiGroupBoxControl, e);
		PaintComponents(stiGroupBoxControl, e);
	}
}
