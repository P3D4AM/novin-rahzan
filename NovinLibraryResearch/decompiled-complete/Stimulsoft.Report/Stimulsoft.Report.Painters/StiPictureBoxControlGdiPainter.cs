using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Painters;

public class StiPictureBoxControlGdiPainter : StiReportControlGdiPainter
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
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Invalid comparison between Unknown and I4
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Invalid comparison between Unknown and I4
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Expected I4, but got Unknown
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		StiPictureBoxControl stiPictureBoxControl = component as StiPictureBoxControl;
		stiPictureBoxControl.InvokePainting(stiPictureBoxControl, e);
		if (!e.Cancel && (stiPictureBoxControl.Enabled || stiPictureBoxControl.IsDesigning))
		{
			Graphics graphics = e.Graphics;
			RectangleD paintRectangle = stiPictureBoxControl.GetPaintRectangle();
			if (((RectangleD)(ref paintRectangle)).Width > 0.0 && ((RectangleD)(ref paintRectangle)).Height > 0.0)
			{
				RectangleD clipRectangle = e.ClipRectangle;
				if (((RectangleD)(ref clipRectangle)).IsEmpty || ((RectangleD)(ref paintRectangle)).IntersectsWith(e.ClipRectangle))
				{
					SolidBrush val = new SolidBrush(stiPictureBoxControl.BackColor);
					try
					{
						graphics.FillRectangle((Brush)(object)val, ((RectangleD)(ref paintRectangle)).ToRectangleF());
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
					Rectangle rectangle = ((RectangleD)(ref paintRectangle)).ToRectangle();
					if ((int)stiPictureBoxControl.BorderStyle == 2)
					{
						ControlPaint.DrawBorder3D(graphics, rectangle, (Border3DStyle)10);
					}
					else if ((int)stiPictureBoxControl.BorderStyle == 1)
					{
						graphics.DrawRectangle(Pens.Black, rectangle);
					}
					rectangle.X += stiPictureBoxControl.GetBorderWidth();
					rectangle.Y += stiPictureBoxControl.GetBorderHeight();
					rectangle.Width -= stiPictureBoxControl.GetBorderWidth() * 2;
					rectangle.Height -= stiPictureBoxControl.GetBorderHeight() * 2;
					if (stiPictureBoxControl.IsDesigning && (int)stiPictureBoxControl.BorderStyle == 0)
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
					if (stiPictureBoxControl.Image != null)
					{
						GraphicsState val3 = graphics.Save();
						graphics.SetClip(rectangle);
						int width = stiPictureBoxControl.Image.Width;
						int height = stiPictureBoxControl.Image.Height;
						PictureBoxSizeMode sizeMode = stiPictureBoxControl.SizeMode;
						switch ((int)sizeMode)
						{
						case 0:
						case 2:
							graphics.DrawImage(stiPictureBoxControl.Image, rectangle.X, rectangle.Y, width, height);
							break;
						case 3:
							graphics.DrawImage(stiPictureBoxControl.Image, rectangle.X + (rectangle.Width - width) / 2, rectangle.Y + (rectangle.Height - height) / 2, width, height);
							break;
						case 1:
							graphics.DrawImage(stiPictureBoxControl.Image, rectangle);
							break;
						}
						graphics.Restore(val3);
					}
					PaintEvents(stiPictureBoxControl, e.Graphics, paintRectangle);
					PaintConditions(stiPictureBoxControl, e.Graphics, paintRectangle);
				}
			}
		}
		e.Cancel = false;
		stiPictureBoxControl.InvokePainted(stiPictureBoxControl, e);
	}
}
