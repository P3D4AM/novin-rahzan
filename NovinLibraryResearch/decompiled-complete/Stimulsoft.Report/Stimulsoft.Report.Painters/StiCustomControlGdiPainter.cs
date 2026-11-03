using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Controls;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Painters;

public class StiCustomControlGdiPainter : StiReportControlGdiPainter
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
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Expected O, but got Unknown
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Expected O, but got Unknown
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		StiCustomControl stiCustomControl = component as StiCustomControl;
		stiCustomControl.InvokePainting(stiCustomControl, e);
		if (!e.Cancel && (stiCustomControl.Enabled || stiCustomControl.IsDesigning))
		{
			Graphics graphics = e.Graphics;
			RectangleD paintRectangle = stiCustomControl.GetPaintRectangle();
			if (((RectangleD)(ref paintRectangle)).Width > 0.0 && ((RectangleD)(ref paintRectangle)).Height > 0.0)
			{
				RectangleD clipRectangle = e.ClipRectangle;
				if (((RectangleD)(ref clipRectangle)).IsEmpty || ((RectangleD)(ref paintRectangle)).IntersectsWith(e.ClipRectangle))
				{
					SolidBrush val = new SolidBrush(stiCustomControl.BackColor);
					try
					{
						graphics.FillRectangle((Brush)(object)val, ((RectangleD)(ref paintRectangle)).ToRectangleF());
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
					if (stiCustomControl.BorderStyle != 0)
					{
						Rectangle rectangle = ((RectangleD)(ref paintRectangle)).ToRectangle();
						ControlPaint.DrawBorder3D(graphics, rectangle, (Border3DStyle)stiCustomControl.BorderStyle);
					}
					if (stiCustomControl.IsDesigning)
					{
						if (stiCustomControl.BorderStyle == StiBorderStyle.None)
						{
							Pen val2 = new Pen(SystemColors.ControlDarkDark, 1f);
							try
							{
								val2.DashStyle = (DashStyle)1;
								StiDrawing.DrawRectangle(graphics, val2, ((RectangleD)(ref paintRectangle)).Left, ((RectangleD)(ref paintRectangle)).Top, ((RectangleD)(ref paintRectangle)).Width, ((RectangleD)(ref paintRectangle)).Height);
							}
							finally
							{
								((IDisposable)val2)?.Dispose();
							}
						}
						if (stiCustomControl.Control != null)
						{
							Bitmap val3 = new Bitmap((int)stiCustomControl.Width, (int)stiCustomControl.Height);
							((Control)stiCustomControl.Control).DrawToBitmap(val3, ((Control)stiCustomControl.Control).ClientRectangle);
							graphics.DrawImage((Image)(object)val3, new Rectangle((int)stiCustomControl.Left, (int)stiCustomControl.Top, (int)stiCustomControl.Width, (int)stiCustomControl.Height));
						}
					}
					PaintEvents(stiCustomControl, e.Graphics, paintRectangle);
					PaintConditions(stiCustomControl, e.Graphics, paintRectangle);
				}
			}
		}
		e.Cancel = false;
		stiCustomControl.InvokePainted(stiCustomControl, e);
		PaintComponents(stiCustomControl, e);
	}
}
