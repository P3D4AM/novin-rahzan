using System;
using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Painters;

public class StiNumericUpDownControlGdiPainter : StiReportControlGdiPainter
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
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Expected O, but got Unknown
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		StiNumericUpDownControl stiNumericUpDownControl = component as StiNumericUpDownControl;
		stiNumericUpDownControl.InvokePainting(stiNumericUpDownControl, e);
		if (!e.Cancel && (stiNumericUpDownControl.Enabled || stiNumericUpDownControl.IsDesigning))
		{
			Graphics graphics = e.Graphics;
			RectangleD paintRectangle = stiNumericUpDownControl.GetPaintRectangle();
			if (((RectangleD)(ref paintRectangle)).Width > 0.0 && ((RectangleD)(ref paintRectangle)).Height > 0.0)
			{
				RectangleD clipRectangle = e.ClipRectangle;
				if (((RectangleD)(ref clipRectangle)).IsEmpty || ((RectangleD)(ref paintRectangle)).IntersectsWith(e.ClipRectangle))
				{
					SolidBrush val = new SolidBrush(stiNumericUpDownControl.BackColor);
					try
					{
						graphics.FillRectangle((Brush)(object)val, ((RectangleD)(ref paintRectangle)).ToRectangle());
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
					Rectangle buttonRect = StiControlPaint.GetButtonRect(((RectangleD)(ref paintRectangle)).ToRectangle(), false, stiNumericUpDownControl.RightToLeft);
					SolidBrush val2 = new SolidBrush(stiNumericUpDownControl.ForeColor);
					try
					{
						StringFormat val3 = new StringFormat();
						try
						{
							val3.FormatFlags = (StringFormatFlags)4096;
							val3.LineAlignment = (StringAlignment)1;
							graphics.DrawString(stiNumericUpDownControl.Value.ToString(), stiNumericUpDownControl.Font, (Brush)(object)val2, (RectangleF)StiControlPaint.GetContentRect(((RectangleD)(ref paintRectangle)).ToRectangle(), false, stiNumericUpDownControl.RightToLeft), val3);
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
					StiControlPaint.DrawBorder(graphics, ((RectangleD)(ref paintRectangle)).ToRectangle(), false, false, false);
					Rectangle rectangle = new Rectangle(buttonRect.X, buttonRect.Y, buttonRect.Width, buttonRect.Height / 2);
					Rectangle rectangle2 = new Rectangle(buttonRect.X, buttonRect.Y + rectangle.Height, buttonRect.Width, buttonRect.Height - rectangle.Height);
					StiControlPaint.DrawButton(graphics, rectangle, stiNumericUpDownControl.ButtonUpBitmap, false, false, false, stiNumericUpDownControl.Enabled, false);
					StiControlPaint.DrawButton(graphics, rectangle2, stiNumericUpDownControl.ButtonDownBitmap, false, false, false, stiNumericUpDownControl.Enabled, false);
					PaintEvents(stiNumericUpDownControl, e.Graphics, paintRectangle);
					PaintConditions(stiNumericUpDownControl, e.Graphics, paintRectangle);
				}
			}
		}
		e.Cancel = false;
		stiNumericUpDownControl.InvokePainted(stiNumericUpDownControl, e);
	}
}
