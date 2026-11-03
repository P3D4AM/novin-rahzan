using System;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Painters;

public class StiRichTextBoxControlGdiPainter : StiReportControlGdiPainter
{
	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		StiRichTextBoxControl stiRichTextBoxControl = component as StiRichTextBoxControl;
		stiRichTextBoxControl.InvokePainting(stiRichTextBoxControl, e);
		if (!e.Cancel && (stiRichTextBoxControl.Enabled || stiRichTextBoxControl.IsDesigning))
		{
			Graphics graphics = e.Graphics;
			RectangleD paintRectangle = stiRichTextBoxControl.GetPaintRectangle();
			if (((RectangleD)(ref paintRectangle)).Width > 0.0 && ((RectangleD)(ref paintRectangle)).Height > 0.0)
			{
				RectangleD clipRectangle = e.ClipRectangle;
				if (((RectangleD)(ref clipRectangle)).IsEmpty || ((RectangleD)(ref paintRectangle)).IntersectsWith(e.ClipRectangle))
				{
					SolidBrush val = new SolidBrush(stiRichTextBoxControl.BackColor);
					try
					{
						graphics.FillRectangle((Brush)(object)val, ((RectangleD)(ref paintRectangle)).ToRectangle());
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
					Rectangle rectangle = ((RectangleD)(ref paintRectangle)).ToRectangle();
					ControlPaint.DrawBorder3D(graphics, rectangle, (Border3DStyle)10);
					((RectangleD)(ref paintRectangle)).X = ((RectangleD)(ref paintRectangle)).X + 4.0;
					((RectangleD)(ref paintRectangle)).Y = ((RectangleD)(ref paintRectangle)).Y + 4.0;
					((RectangleD)(ref paintRectangle)).Width = ((RectangleD)(ref paintRectangle)).Width - 8.0;
					((RectangleD)(ref paintRectangle)).Height = ((RectangleD)(ref paintRectangle)).Height - 8.0;
					stiRichTextBoxControl.RenderMetafile();
					if (stiRichTextBoxControl.Image != null)
					{
						float width = (float)(((RectangleD)(ref paintRectangle)).Width / stiRichTextBoxControl.Page.Zoom);
						float height = (float)(((RectangleD)(ref paintRectangle)).Height / stiRichTextBoxControl.Page.Zoom);
						try
						{
							graphics.DrawImage(stiRichTextBoxControl.Image, ((RectangleD)(ref paintRectangle)).ToRectangleF(), new RectangleF(0f, 0f, width, height), (GraphicsUnit)2);
						}
						catch
						{
						}
					}
					PaintEvents(stiRichTextBoxControl, graphics, paintRectangle);
					PaintConditions(stiRichTextBoxControl, graphics, paintRectangle);
				}
			}
		}
		e.Cancel = false;
		stiRichTextBoxControl.InvokePainted(stiRichTextBoxControl, e);
	}
}
