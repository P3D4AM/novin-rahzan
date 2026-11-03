using System;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Painters;

public class StiTextBoxControlGdiPainter : StiReportControlGdiPainter
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
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Expected O, but got Unknown
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Expected O, but got Unknown
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Invalid comparison between Unknown and I4
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		StiTextBoxControl stiTextBoxControl = component as StiTextBoxControl;
		stiTextBoxControl.InvokePainting(stiTextBoxControl, e);
		if (!e.Cancel && (stiTextBoxControl.Enabled || stiTextBoxControl.IsDesigning))
		{
			Graphics graphics = e.Graphics;
			RectangleD paintRectangle = stiTextBoxControl.GetPaintRectangle();
			if (((RectangleD)(ref paintRectangle)).Width > 0.0 && ((RectangleD)(ref paintRectangle)).Height > 0.0)
			{
				RectangleD clipRectangle = e.ClipRectangle;
				if (((RectangleD)(ref clipRectangle)).IsEmpty || ((RectangleD)(ref paintRectangle)).IntersectsWith(e.ClipRectangle))
				{
					SolidBrush val = new SolidBrush(stiTextBoxControl.BackColor);
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
					SolidBrush val2 = new SolidBrush(stiTextBoxControl.ForeColor);
					try
					{
						StringFormat val3 = new StringFormat();
						try
						{
							val3.FormatFlags = (StringFormatFlags)4096;
							val3.Trimming = (StringTrimming)3;
							val3.FormatFlags = (StringFormatFlags)4096;
							val3.Trimming = (StringTrimming)3;
							if ((int)stiTextBoxControl.RightToLeft == 1)
							{
								val3.FormatFlags = (StringFormatFlags)(val3.FormatFlags | 1);
							}
							graphics.DrawString(stiTextBoxControl.Text, stiTextBoxControl.Font, (Brush)(object)val2, ((RectangleD)(ref paintRectangle)).ToRectangleF(), val3);
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
					PaintEvents(stiTextBoxControl, graphics, paintRectangle);
					PaintConditions(stiTextBoxControl, graphics, paintRectangle);
				}
			}
		}
		e.Cancel = false;
		stiTextBoxControl.InvokePainted(stiTextBoxControl, e);
	}
}
