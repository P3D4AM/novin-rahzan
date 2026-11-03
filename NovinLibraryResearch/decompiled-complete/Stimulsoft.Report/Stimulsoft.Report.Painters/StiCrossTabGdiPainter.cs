using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Painters;

public class StiCrossTabGdiPainter : StiContainerGdiPainter
{
	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Expected O, but got Unknown
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Expected O, but got Unknown
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Expected O, but got Unknown
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		StiCrossTab stiCrossTab = (StiCrossTab)component;
		stiCrossTab.InvokePainting(stiCrossTab, e);
		Graphics graphics = e.Graphics;
		if (!e.Cancel)
		{
			RectangleD paintRectangle = stiCrossTab.GetPaintRectangle();
			if (((RectangleD)(ref paintRectangle)).Width > 0.0 && ((RectangleD)(ref paintRectangle)).Height > 0.0)
			{
				RectangleD clipRectangle = e.ClipRectangle;
				if (((RectangleD)(ref clipRectangle)).IsEmpty || ((RectangleD)(ref paintRectangle)).IntersectsWith(e.ClipRectangle))
				{
					if (stiCrossTab.IsDesigning)
					{
						if (stiCrossTab.Report.Info.FillContainer)
						{
							Color color = Color.FromArgb(100, 167, 215, 115);
							StiDrawing.FillRectangle(graphics, color, ((RectangleD)(ref paintRectangle)).Left, ((RectangleD)(ref paintRectangle)).Top, ((RectangleD)(ref paintRectangle)).Width, ((RectangleD)(ref paintRectangle)).Height);
						}
						StiDrawing.DrawRectangle(graphics, Pens.Black, ((RectangleD)(ref paintRectangle)).Left, ((RectangleD)(ref paintRectangle)).Top, ((RectangleD)(ref paintRectangle)).Width, ((RectangleD)(ref paintRectangle)).Height);
						StringFormat val = new StringFormat();
						try
						{
							Font val2 = new Font("Segoe UI", (float)(15.0 * stiCrossTab.Page.Zoom));
							try
							{
								SolidBrush val3 = new SolidBrush(Color.FromArgb(102, 99, 99));
								try
								{
									val.LineAlignment = (StringAlignment)1;
									val.Alignment = (StringAlignment)1;
									StiTextDrawing.DrawString(graphics, stiCrossTab.Name, val2, (Brush)(object)val3, new RectangleD(((RectangleD)(ref paintRectangle)).Left, ((RectangleD)(ref paintRectangle)).Top, ((RectangleD)(ref paintRectangle)).Width, ((RectangleD)(ref paintRectangle)).Height), val);
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
						}
						finally
						{
							((IDisposable)val)?.Dispose();
						}
						PaintMarkers(stiCrossTab, graphics, paintRectangle);
					}
					PaintEvents(stiCrossTab, e.Graphics, paintRectangle);
					PaintConditions(stiCrossTab, e.Graphics, paintRectangle);
				}
			}
			GraphicsState val4 = graphics.Save();
			((RectangleD)(ref paintRectangle)).X = ((RectangleD)(ref paintRectangle)).X - 2.0;
			((RectangleD)(ref paintRectangle)).Y = ((RectangleD)(ref paintRectangle)).Y - 2.0;
			((RectangleD)(ref paintRectangle)).Width = ((RectangleD)(ref paintRectangle)).Width + 4.0;
			((RectangleD)(ref paintRectangle)).Height = ((RectangleD)(ref paintRectangle)).Height + 4.0;
			graphics.SetClip(((RectangleD)(ref paintRectangle)).ToRectangleF(), (CombineMode)1);
			PaintComponents(stiCrossTab, e);
			graphics.Restore(val4);
		}
		e.Cancel = false;
		stiCrossTab.InvokePainted(stiCrossTab, e);
		PaintQuickButtons(stiCrossTab, e.Graphics);
	}
}
