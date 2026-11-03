using System;
using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Painters;

public class StiWinControlGdiPainter : StiComponentGdiPainter
{
	public override Image GetImage(StiComponent component, ref float zoom, StiExportFormat format)
	{
		StiWinControl stiWinControl = (StiWinControl)component;
		return stiWinControl.GetControlImage();
	}

	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		StiWinControl stiWinControl = (StiWinControl)component;
		if (!e.DrawBorderFormatting && e.DrawTopmostBorderSides && !stiWinControl.Border.Topmost)
		{
			return;
		}
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		if (e.DrawBorderFormatting)
		{
			stiWinControl.InvokePainting(stiWinControl, e);
		}
		if (!e.Cancel && (stiWinControl.Enabled || stiWinControl.IsDesigning))
		{
			Graphics graphics = e.Graphics;
			RectangleD paintRectangle = stiWinControl.GetPaintRectangle();
			if (((RectangleD)(ref paintRectangle)).Width > 0.0 && ((RectangleD)(ref paintRectangle)).Height > 0.0)
			{
				RectangleD clipRectangle = e.ClipRectangle;
				if (((RectangleD)(ref clipRectangle)).IsEmpty || ((RectangleD)(ref paintRectangle)).IntersectsWith(e.ClipRectangle))
				{
					if (e.DrawBorderFormatting && stiWinControl.Control == null && stiWinControl.Report.Info.FillComponent && stiWinControl.IsDesigning)
					{
						Color color = Color.FromArgb(150, Color.White);
						StiDrawing.FillRectangle(graphics, color, ((RectangleD)(ref paintRectangle)).Left, ((RectangleD)(ref paintRectangle)).Top, ((RectangleD)(ref paintRectangle)).Width, ((RectangleD)(ref paintRectangle)).Height);
					}
					if (e.DrawBorderFormatting && stiWinControl.Control != null)
					{
						Image controlImage = stiWinControl.GetControlImage();
						try
						{
							if (controlImage != null)
							{
								graphics.DrawImage(controlImage, ((RectangleD)(ref paintRectangle)).ToRectangle());
							}
						}
						finally
						{
							((IDisposable)controlImage)?.Dispose();
						}
					}
					if (e.DrawBorderFormatting)
					{
						PaintMarkers(stiWinControl, graphics, paintRectangle);
					}
					Rectangle rectangle = ((RectangleD)(ref paintRectangle)).ToRectangle();
					if (stiWinControl.HighlightState == StiHighlightState.Hide)
					{
						PaintBorder(stiWinControl, graphics, RectangleD.CreateFromRectangle(rectangle), stiWinControl.Page.Zoom, e.DrawBorderFormatting, e.DrawTopmostBorderSides);
					}
					if (e.DrawBorderFormatting)
					{
						PaintEvents(stiWinControl, e.Graphics, paintRectangle);
						PaintConditions(stiWinControl, e.Graphics, paintRectangle);
					}
				}
			}
		}
		e.Cancel = false;
		if (e.DrawBorderFormatting)
		{
			stiWinControl.InvokePainted(stiWinControl, e);
		}
	}
}
