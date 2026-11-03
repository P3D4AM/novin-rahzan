using System;
using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Painters;

public class StiUndefinedComponentGdiPainter : StiComponentGdiPainter
{
	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Expected O, but got Unknown
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		component.InvokePainting(component, e);
		if (!e.Cancel && (component.Enabled || component.IsDesigning))
		{
			Graphics graphics = e.Graphics;
			RectangleD paintRectangle = component.GetPaintRectangle();
			if (((RectangleD)(ref paintRectangle)).Width > 0.0 && ((RectangleD)(ref paintRectangle)).Height > 0.0)
			{
				RectangleD clipRectangle = e.ClipRectangle;
				if (((RectangleD)(ref clipRectangle)).IsEmpty || ((RectangleD)(ref paintRectangle)).IntersectsWith(e.ClipRectangle))
				{
					graphics.FillRectangle(Brushes.Red, ((RectangleD)(ref paintRectangle)).ToRectangleF());
					Font val = new Font("Arial", (float)(10.0 * component.Report.Info.Zoom));
					try
					{
						StringFormat val2 = new StringFormat();
						try
						{
							val2.LineAlignment = (StringAlignment)1;
							val2.Alignment = (StringAlignment)1;
							graphics.DrawString("Undefined Component", val, Brushes.White, ((RectangleD)(ref paintRectangle)).ToRectangleF(), val2);
						}
						finally
						{
							((IDisposable)val2)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
			}
			PaintQuickButtons(component, e.Graphics);
			PaintEvents(component, e.Graphics, paintRectangle);
		}
		e.Cancel = false;
		component.InvokePainted(component, e);
	}
}
