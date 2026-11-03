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

public class StiPanelControlGdiPainter : StiReportControlGdiPainter
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
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Expected O, but got Unknown
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Expected O, but got Unknown
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Expected O, but got Unknown
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Expected O, but got Unknown
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		StiPanelControl stiPanelControl = component as StiPanelControl;
		stiPanelControl.InvokePainting(stiPanelControl, e);
		if (!e.Cancel && (stiPanelControl.Enabled || stiPanelControl.IsDesigning))
		{
			Graphics graphics = e.Graphics;
			RectangleD paintRectangle = stiPanelControl.GetPaintRectangle();
			if (((RectangleD)(ref paintRectangle)).Width > 0.0 && ((RectangleD)(ref paintRectangle)).Height > 0.0)
			{
				RectangleD clipRectangle = e.ClipRectangle;
				if (((RectangleD)(ref clipRectangle)).IsEmpty || ((RectangleD)(ref paintRectangle)).IntersectsWith(e.ClipRectangle))
				{
					SolidBrush val = new SolidBrush(stiPanelControl.BackColor);
					try
					{
						graphics.FillRectangle((Brush)(object)val, ((RectangleD)(ref paintRectangle)).ToRectangleF());
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
					if (stiPanelControl.BorderStyle != 0)
					{
						Rectangle rectangle = ((RectangleD)(ref paintRectangle)).ToRectangle();
						ControlPaint.DrawBorder3D(graphics, rectangle, (Border3DStyle)stiPanelControl.BorderStyle);
					}
					if (stiPanelControl.IsDesigning)
					{
						if (stiPanelControl.BorderStyle == StiBorderStyle.None)
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
						StringFormat val3 = new StringFormat();
						try
						{
							Font val4 = new Font("Arial", (float)(15.0 * stiPanelControl.Page.Zoom));
							try
							{
								Brush val5 = (Brush)new SolidBrush(Color.Gray);
								try
								{
									val3.LineAlignment = (StringAlignment)1;
									val3.Alignment = (StringAlignment)1;
									StiTextDrawing.DrawString(graphics, stiPanelControl.Name, val4, val5, new RectangleD(((RectangleD)(ref paintRectangle)).Left, ((RectangleD)(ref paintRectangle)).Top, ((RectangleD)(ref paintRectangle)).Width, ((RectangleD)(ref paintRectangle)).Height), val3);
								}
								finally
								{
									((IDisposable)val5)?.Dispose();
								}
							}
							finally
							{
								((IDisposable)val4)?.Dispose();
							}
						}
						finally
						{
							((IDisposable)val3)?.Dispose();
						}
					}
					PaintEvents(stiPanelControl, e.Graphics, paintRectangle);
					PaintConditions(stiPanelControl, e.Graphics, paintRectangle);
				}
			}
		}
		e.Cancel = false;
		stiPanelControl.InvokePainted(stiPanelControl, e);
		PaintComponents(stiPanelControl, e);
	}
}
