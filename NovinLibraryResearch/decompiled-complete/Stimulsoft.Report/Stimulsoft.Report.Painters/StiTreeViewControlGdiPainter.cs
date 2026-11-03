using System;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Painters;

public class StiTreeViewControlGdiPainter : StiReportControlGdiPainter
{
	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Expected O, but got Unknown
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Expected O, but got Unknown
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		StiTreeViewControl stiTreeViewControl = component as StiTreeViewControl;
		stiTreeViewControl.InvokePainting(stiTreeViewControl, e);
		if (!e.Cancel && (stiTreeViewControl.Enabled || stiTreeViewControl.IsDesigning))
		{
			Graphics graphics = e.Graphics;
			RectangleD paintRectangle = stiTreeViewControl.GetPaintRectangle();
			if (((RectangleD)(ref paintRectangle)).Width > 0.0 && ((RectangleD)(ref paintRectangle)).Height > 0.0)
			{
				RectangleD clipRectangle = e.ClipRectangle;
				if (((RectangleD)(ref clipRectangle)).IsEmpty || ((RectangleD)(ref paintRectangle)).IntersectsWith(e.ClipRectangle))
				{
					SolidBrush val = new SolidBrush(stiTreeViewControl.BackColor);
					try
					{
						graphics.FillRectangle((Brush)(object)val, ((RectangleD)(ref paintRectangle)).ToRectangleF());
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
					Rectangle rectangle = ((RectangleD)(ref paintRectangle)).ToRectangle();
					ControlPaint.DrawBorder3D(graphics, rectangle, (Border3DStyle)10);
					rectangle.X += SystemInformation.Border3DSize.Width;
					rectangle.Y += SystemInformation.Border3DSize.Height;
					rectangle.Width -= SystemInformation.Border3DSize.Width * 2;
					rectangle.Height -= SystemInformation.Border3DSize.Height * 2;
					SolidBrush val2 = new SolidBrush(stiTreeViewControl.ForeColor);
					try
					{
						StringFormat val3 = new StringFormat();
						try
						{
							graphics.DrawString(stiTreeViewControl.Name, stiTreeViewControl.Font, (Brush)(object)val2, (RectangleF)rectangle, val3);
						}
						finally
						{
							((IDisposable)val3)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
					PaintEvents(stiTreeViewControl, graphics, paintRectangle);
					PaintConditions(stiTreeViewControl, graphics, paintRectangle);
				}
			}
		}
		e.Cancel = false;
		stiTreeViewControl.InvokePainted(stiTreeViewControl, e);
	}
}
