using System;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Painters;

public class StiCheckedListBoxControlGdiPainter : StiReportControlGdiPainter
{
	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		StiCheckedListBoxControl stiCheckedListBoxControl = component as StiCheckedListBoxControl;
		stiCheckedListBoxControl.InvokePainting(stiCheckedListBoxControl, e);
		if (!e.Cancel && (stiCheckedListBoxControl.Enabled || stiCheckedListBoxControl.IsDesigning))
		{
			Graphics graphics = e.Graphics;
			RectangleD paintRectangle = stiCheckedListBoxControl.GetPaintRectangle();
			if (((RectangleD)(ref paintRectangle)).Width > 0.0 && ((RectangleD)(ref paintRectangle)).Height > 0.0)
			{
				RectangleD clipRectangle = e.ClipRectangle;
				if (((RectangleD)(ref clipRectangle)).IsEmpty || ((RectangleD)(ref paintRectangle)).IntersectsWith(e.ClipRectangle))
				{
					RectangleD val = paintRectangle;
					SolidBrush val2 = new SolidBrush(stiCheckedListBoxControl.BackColor);
					try
					{
						graphics.FillRectangle((Brush)(object)val2, ((RectangleD)(ref paintRectangle)).ToRectangle());
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
					Rectangle rectangle = ((RectangleD)(ref paintRectangle)).ToRectangle();
					ControlPaint.DrawBorder3D(graphics, rectangle, (Border3DStyle)10);
					((RectangleD)(ref paintRectangle)).X = ((RectangleD)(ref paintRectangle)).X + 4.0;
					((RectangleD)(ref paintRectangle)).Y = ((RectangleD)(ref paintRectangle)).Y + 4.0;
					((RectangleD)(ref paintRectangle)).Width = ((RectangleD)(ref paintRectangle)).Width - 8.0;
					((RectangleD)(ref paintRectangle)).Height = ((RectangleD)(ref paintRectangle)).Height - 8.0;
					SolidBrush val3 = new SolidBrush(stiCheckedListBoxControl.ForeColor);
					try
					{
						StringFormat val4 = new StringFormat();
						try
						{
							val4.FormatFlags = (StringFormatFlags)4096;
							foreach (object item in stiCheckedListBoxControl.Items)
							{
								string text = item.ToString();
								graphics.DrawString(text, stiCheckedListBoxControl.Font, (Brush)(object)val3, ((RectangleD)(ref paintRectangle)).ToRectangleF(), val4);
								((RectangleD)(ref paintRectangle)).Y = ((RectangleD)(ref paintRectangle)).Y + (double)stiCheckedListBoxControl.ItemHeight;
								((RectangleD)(ref paintRectangle)).Height = ((RectangleD)(ref paintRectangle)).Height - (double)stiCheckedListBoxControl.ItemHeight;
								if (((RectangleD)(ref paintRectangle)).Height < 0.0)
								{
									break;
								}
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
					PaintEvents(stiCheckedListBoxControl, e.Graphics, paintRectangle);
					PaintConditions(stiCheckedListBoxControl, e.Graphics, paintRectangle);
				}
			}
		}
		e.Cancel = false;
		stiCheckedListBoxControl.InvokePainted(stiCheckedListBoxControl, e);
	}
}
