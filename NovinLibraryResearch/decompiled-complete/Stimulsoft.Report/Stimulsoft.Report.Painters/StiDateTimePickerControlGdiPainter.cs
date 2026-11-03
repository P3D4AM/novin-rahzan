using System;
using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Painters;

public class StiDateTimePickerControlGdiPainter : StiReportControlGdiPainter
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
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		StiDateTimePickerControl stiDateTimePickerControl = component as StiDateTimePickerControl;
		stiDateTimePickerControl.InvokePainting(stiDateTimePickerControl, e);
		if (!e.Cancel && (stiDateTimePickerControl.Enabled || stiDateTimePickerControl.IsDesigning))
		{
			Graphics graphics = e.Graphics;
			RectangleD paintRectangle = stiDateTimePickerControl.GetPaintRectangle();
			if (((RectangleD)(ref paintRectangle)).Width > 0.0 && ((RectangleD)(ref paintRectangle)).Height > 0.0)
			{
				RectangleD clipRectangle = e.ClipRectangle;
				if (((RectangleD)(ref clipRectangle)).IsEmpty || ((RectangleD)(ref paintRectangle)).IntersectsWith(e.ClipRectangle))
				{
					SolidBrush val = new SolidBrush(stiDateTimePickerControl.BackColor);
					try
					{
						graphics.FillRectangle((Brush)(object)val, ((RectangleD)(ref paintRectangle)).ToRectangle());
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
					Rectangle buttonRect = StiControlPaint.GetButtonRect(((RectangleD)(ref paintRectangle)).ToRectangle(), false, stiDateTimePickerControl.RightToLeft);
					SolidBrush val2 = new SolidBrush(stiDateTimePickerControl.ForeColor);
					try
					{
						StringFormat val3 = new StringFormat();
						try
						{
							val3.FormatFlags = (StringFormatFlags)4096;
							val3.LineAlignment = (StringAlignment)1;
							graphics.DrawString(stiDateTimePickerControl.Value.ToString("G"), stiDateTimePickerControl.Font, (Brush)(object)val2, (RectangleF)StiControlPaint.GetContentRect(((RectangleD)(ref paintRectangle)).ToRectangle(), false, stiDateTimePickerControl.RightToLeft), val3);
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
					StiControlPaint.DrawButton(graphics, buttonRect, stiDateTimePickerControl.ButtonBitmap, false, false, false, stiDateTimePickerControl.Enabled, false);
					PaintEvents(stiDateTimePickerControl, e.Graphics, paintRectangle);
					PaintConditions(stiDateTimePickerControl, e.Graphics, paintRectangle);
				}
			}
		}
		e.Cancel = false;
		stiDateTimePickerControl.InvokePainted(stiDateTimePickerControl, e);
	}
}
