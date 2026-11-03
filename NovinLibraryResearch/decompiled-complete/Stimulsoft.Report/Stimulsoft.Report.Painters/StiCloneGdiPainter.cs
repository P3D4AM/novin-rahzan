using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Painters;

public class StiCloneGdiPainter : StiContainerGdiPainter
{
	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Invalid comparison between Unknown and I4
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Expected O, but got Unknown
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Expected O, but got Unknown
		StiClone stiClone = component as StiClone;
		if (!e.DrawBorderFormatting && e.DrawTopmostBorderSides && !stiClone.Border.Topmost)
		{
			return;
		}
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		if (e.DrawBorderFormatting)
		{
			stiClone.InvokePainting(stiClone, e);
		}
		if (!e.Cancel)
		{
			Graphics graphics = e.Graphics;
			if (stiClone.IsDesigning)
			{
				RectangleD paintRectangle = stiClone.GetPaintRectangle();
				if (e.DrawBorderFormatting)
				{
					if (stiClone.Brush is StiSolidBrush && ((StiSolidBrush)stiClone.Brush).Color == Color.Transparent && stiClone.Report.Info.FillComponent && stiClone.IsDesigning)
					{
						Color color = Color.FromArgb(245, 245, 245);
						StiDrawing.FillRectangle(graphics, color, ((RectangleD)(ref paintRectangle)).Left, ((RectangleD)(ref paintRectangle)).Top, ((RectangleD)(ref paintRectangle)).Width, ((RectangleD)(ref paintRectangle)).Height);
					}
					else
					{
						StiDrawing.FillRectangle(graphics, stiClone.Brush, paintRectangle);
					}
				}
				if (e.DrawBorderFormatting)
				{
					StringFormat val = new StringFormat();
					try
					{
						Font val2 = new Font("Segoe UI", (float)(15.0 * stiClone.Page.Zoom));
						try
						{
							SolidBrush val3 = new SolidBrush(Color.Gray);
							try
							{
								val.LineAlignment = (StringAlignment)1;
								val.Alignment = (StringAlignment)1;
								StiTextDrawing.DrawString(graphics, stiClone.Name, val2, (Brush)(object)val3, new RectangleD(((RectangleD)(ref paintRectangle)).Left, ((RectangleD)(ref paintRectangle)).Top, ((RectangleD)(ref paintRectangle)).Width, ((RectangleD)(ref paintRectangle)).Height), val);
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
				}
				if (e.DrawBorderFormatting && stiClone.HighlightState == StiHighlightState.Hide && (int)stiClone.Border.Side != 15)
				{
					PaintMarkers(stiClone, graphics, paintRectangle);
				}
				if ((int)stiClone.Border.Side == 0 && stiClone.IsDesigning && e.DrawBorderFormatting)
				{
					Pen val4 = new Pen(Color.FromArgb(128, 128, 128));
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
				if (stiClone.HighlightState == StiHighlightState.Hide)
				{
					PaintBorder(stiClone, graphics, paintRectangle, stiClone.Page.Zoom, e.DrawBorderFormatting, e.DrawTopmostBorderSides || !stiClone.Border.Topmost);
				}
				if (e.DrawBorderFormatting)
				{
					PaintEvents(stiClone, graphics, paintRectangle);
					PaintConditions(stiClone, graphics, paintRectangle);
				}
			}
		}
		e.Cancel = false;
		if (e.DrawBorderFormatting)
		{
			stiClone.InvokePainted(stiClone, e);
		}
	}
}
