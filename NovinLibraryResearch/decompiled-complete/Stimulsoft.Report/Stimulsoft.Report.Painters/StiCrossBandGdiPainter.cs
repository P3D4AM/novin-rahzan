using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Painters;

public class StiCrossBandGdiPainter : StiBandGdiPainter
{
	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Expected O, but got Unknown
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Expected O, but got Unknown
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Expected O, but got Unknown
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Expected O, but got Unknown
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		StiBand stiBand = component as StiBand;
		stiBand.InvokePainting(stiBand, e);
		Graphics graphics = e.Graphics;
		if (!e.Cancel && stiBand.IsDesigning)
		{
			RectangleD paintRectangle = stiBand.GetPaintRectangle();
			Color color = Color.White;
			if (stiBand is StiCrossDataBand)
			{
				color = Color.FromArgb(90, 147, 204);
			}
			else if (stiBand is StiCrossHeaderBand || stiBand is StiCrossFooterBand)
			{
				color = Color.FromArgb(178, 197, 223);
			}
			else if (stiBand is StiCrossGroupHeaderBand || stiBand is StiCrossGroupFooterBand)
			{
				color = Color.FromArgb(239, 155, 52);
			}
			double num = stiBand.Page.Zoom * StiScale.X;
			RectangleD val = default(RectangleD);
			((RectangleD)(ref val))._002Ector(((RectangleD)(ref paintRectangle)).Left, ((RectangleD)(ref paintRectangle)).Bottom, ((RectangleD)(ref paintRectangle)).Width, stiBand.HeaderSize * num);
			RectangleD val2 = paintRectangle;
			((RectangleD)(ref val2)).Height = ((RectangleD)(ref val2)).Height + ((RectangleD)(ref paintRectangle)).Height;
			if (((RectangleD)(ref val2)).Width > 0.0 && ((RectangleD)(ref val2)).Height > 0.0)
			{
				RectangleD clipRectangle = e.ClipRectangle;
				if (((RectangleD)(ref clipRectangle)).IsEmpty || ((RectangleD)(ref val2)).IntersectsWith(e.ClipRectangle))
				{
					if (stiBand.Brush is StiSolidBrush && ((StiSolidBrush)stiBand.Brush).Color == Color.Transparent && stiBand.Report.Info.FillCrossBands && stiBand.IsDesigning)
					{
						Color fillColor = stiBand.Report.Info.GetFillColor(stiBand.HeaderEndColor);
						StiDrawing.FillRectangle(graphics, fillColor, ((RectangleD)(ref paintRectangle)).Left, ((RectangleD)(ref paintRectangle)).Top, ((RectangleD)(ref paintRectangle)).Width, ((RectangleD)(ref paintRectangle)).Height);
					}
					else
					{
						StiDrawing.FillRectangle(graphics, stiBand.Brush, paintRectangle);
					}
					if (stiBand.Report.Info.ShowHeaders && ((RectangleD)(ref val)).Width != 0.0 && ((RectangleD)(ref val)).Height != 0.0)
					{
						SolidBrush val3 = new SolidBrush(color);
						try
						{
							graphics.FillRectangle((Brush)(object)val3, ((RectangleD)(ref val)).ToRectangleF());
						}
						finally
						{
							((IDisposable)val3)?.Dispose();
						}
					}
					if (stiBand.Report.Info.FillCrossBands)
					{
						Color fillColor2 = stiBand.Report.Info.GetFillColor(color);
						StiDrawing.FillRectangle(graphics, fillColor2, ((RectangleD)(ref paintRectangle)).Left, ((RectangleD)(ref paintRectangle)).Top, ((RectangleD)(ref paintRectangle)).Width, ((RectangleD)(ref paintRectangle)).Height);
					}
					if (e.DrawBorderFormatting)
					{
						if (stiBand.IsDesigning)
						{
							Color color2 = Color.FromArgb(220, stiBand.HeaderStartColor);
							Pen val4 = new Pen(color2);
							try
							{
								val4.DashStyle = (DashStyle)1;
								StiDrawing.DrawRectangle(graphics, val4, ((RectangleD)(ref paintRectangle)).Left, ((RectangleD)(ref paintRectangle)).Top, ((RectangleD)(ref paintRectangle)).Width, ((RectangleD)(ref paintRectangle)).Height);
							}
							finally
							{
								((IDisposable)val4)?.Dispose();
							}
						}
						PaintBorder(stiBand, graphics, paintRectangle, num, e.DrawBorderFormatting, drawBorderSides: true);
					}
					StringFormat val5 = new StringFormat();
					try
					{
						Font val6 = new Font("Segoe UI", (float)(10.0 * stiBand.Page.Zoom));
						try
						{
							val5.LineAlignment = (StringAlignment)1;
							val5.Alignment = (StringAlignment)0;
							if (stiBand.Report.Info.ShowHeaders && ((RectangleD)(ref val)).Width != 0.0 && ((RectangleD)(ref val)).Height != 0.0)
							{
								StiTextDrawing.DrawString(graphics, stiBand.Name, val6, Brushes.Black, val, val5);
							}
						}
						finally
						{
							((IDisposable)val6)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)val5)?.Dispose();
					}
					PaintEvents(stiBand, graphics, paintRectangle);
					PaintConditions(stiBand, graphics, paintRectangle);
				}
			}
		}
		e.Cancel = false;
		stiBand.InvokePainted(stiBand, e);
		PaintComponents(stiBand, e);
	}
}
