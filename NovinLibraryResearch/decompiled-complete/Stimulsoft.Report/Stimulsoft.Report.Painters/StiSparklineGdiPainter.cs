using System;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Data.Exceptions;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Painters;

public class StiSparklineGdiPainter : StiComponentGdiPainter
{
	public override Image GetImage(StiComponent component, ref float zoom, StiExportFormat format)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		StiSparkline stiSparkline = (StiSparkline)component;
		if (format != StiExportFormat.HtmlSpan && format != StiExportFormat.HtmlDiv && format != StiExportFormat.HtmlTable)
		{
			zoom *= 2f;
		}
		double zoom2 = stiSparkline.Report.Info.Zoom;
		stiSparkline.Report.Info.Zoom = zoom;
		RectangleD paintRectangle = stiSparkline.GetPaintRectangle();
		((RectangleD)(ref paintRectangle)).X = 0.0;
		((RectangleD)(ref paintRectangle)).Y = 0.0;
		((RectangleD)(ref paintRectangle)).Width = ((RectangleD)(ref paintRectangle)).Width / StiScale.Factor;
		((RectangleD)(ref paintRectangle)).Height = ((RectangleD)(ref paintRectangle)).Height / StiScale.Factor;
		stiSparkline.Report.Info.Zoom = zoom2;
		int num = (int)((RectangleD)(ref paintRectangle)).Width + 2;
		int num2 = (int)((RectangleD)(ref paintRectangle)).Height + 2;
		Bitmap val = new Bitmap(num, num2);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			val2.PageUnit = (GraphicsUnit)2;
			bool flag = stiSparkline.Brush != null && ((stiSparkline.Brush is StiSolidBrush && ((StiSolidBrush)/*isinst with value type is only supported in some contexts*/).Color.A == 0) || stiSparkline.Brush is StiEmptyBrush);
			if (format == StiExportFormat.Pdf || format == StiExportFormat.ImagePng)
			{
				if (flag)
				{
					val2.Clear(Color.FromArgb(1, 255, 255, 255));
				}
			}
			else
			{
				val2.Clear(Color.White);
			}
			StiDrawing.FillRectangle(val2, stiSparkline.Brush, paintRectangle);
			DrawSparkline(val2, paintRectangle, stiSparkline, zoom);
			return (Image)(object)val;
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		StiSparkline stiSparkline = (StiSparkline)component;
		if (!e.DrawBorderFormatting && e.DrawTopmostBorderSides && !stiSparkline.Border.Topmost)
		{
			return;
		}
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		if (e.DrawBorderFormatting)
		{
			stiSparkline.InvokePainting(stiSparkline, e);
		}
		if (!e.Cancel)
		{
			Graphics graphics = e.Graphics;
			RectangleD paintRectangle = stiSparkline.GetPaintRectangle();
			float num = (float)(stiSparkline.Page.Zoom * StiScale.Factor);
			if (((RectangleD)(ref paintRectangle)).Width > 0.0 && ((RectangleD)(ref paintRectangle)).Height > 0.0)
			{
				RectangleD clipRectangle = e.ClipRectangle;
				if (((RectangleD)(ref clipRectangle)).IsEmpty || ((RectangleD)(ref paintRectangle)).IntersectsWith(e.ClipRectangle))
				{
					if (e.DrawBorderFormatting)
					{
						PaintBackground(stiSparkline, graphics, paintRectangle);
					}
					DrawSparkline(graphics, paintRectangle, stiSparkline, num);
					if (e.DrawBorderFormatting)
					{
						PaintMarkers(stiSparkline, graphics, paintRectangle);
					}
					if (stiSparkline.HighlightState == StiHighlightState.Hide)
					{
						PaintBorder(stiSparkline, graphics, paintRectangle, num, e.DrawBorderFormatting, e.DrawTopmostBorderSides || !stiSparkline.Border.Topmost);
					}
					if (e.DrawBorderFormatting)
					{
						PaintEvents(stiSparkline, e.Graphics, paintRectangle);
						PaintConditions(stiSparkline, e.Graphics, paintRectangle);
					}
				}
			}
		}
		e.Cancel = false;
		if (e.DrawBorderFormatting)
		{
			stiSparkline.InvokePainted(stiSparkline, e);
		}
	}

	public virtual void PaintBackground(StiSparkline sparkline, Graphics g, RectangleD rect)
	{
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (sparkline.Brush is StiSolidBrush && ((StiSolidBrush)sparkline.Brush).Color.A == 0 && sparkline.Report.Info.FillComponent && sparkline.IsDesigning)
		{
			Color color = Color.FromArgb(150, Color.GhostWhite);
			StiDrawing.FillRectangle(g, color, ((RectangleD)(ref rect)).Left, ((RectangleD)(ref rect)).Top, ((RectangleD)(ref rect)).Width, ((RectangleD)(ref rect)).Height);
		}
		else
		{
			StiDrawing.FillRectangle(g, sparkline.Brush, rect);
		}
	}

	internal static void DrawSparkline(Graphics g, RectangleD rect, StiSparkline sparkline, float zoom)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		StiGdiContextPainter contextPainter = new StiGdiContextPainter(g);
		StiContext context = new StiContext(contextPainter, isGdi: true, isWpf: false, sparkline.IsPrinting, zoom);
		RenderSparkline(context, rect, sparkline, zoom);
	}

	internal static void RenderSparkline(StiContext context, RectangleD rect, StiSparkline sparkline, float zoom)
	{
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		object[] array = sparkline.FetchValues();
		Color positiveColor = sparkline.PositiveColor;
		Color negativeColor = sparkline.NegativeColor;
		if (!string.IsNullOrEmpty(sparkline.ComponentStyle) && sparkline.Report.Styles.ToList().FirstOrDefault((StiBaseStyle x) => x.Name == sparkline.ComponentStyle) is StiIndicatorStyle stiIndicatorStyle)
		{
			positiveColor = stiIndicatorStyle.PositiveColor;
			negativeColor = stiIndicatorStyle.NegativeColor;
		}
		switch (sparkline.Type)
		{
		case StiSparklineType.Column:
			StiColumnSparklinesCellPainter.Draw(context, rect, array, positiveColor, negativeColor);
			break;
		case StiSparklineType.Line:
			StiLineSparklinesCellPainter.Draw(context, rect, array, zoom, positiveColor, showArea: false, sparkline.ShowFirstLastPoints, sparkline.ShowHighLowPoints);
			break;
		case StiSparklineType.Area:
			StiLineSparklinesCellPainter.Draw(context, rect, array, zoom, positiveColor, showArea: true, sparkline.ShowFirstLastPoints, sparkline.ShowHighLowPoints);
			break;
		case StiSparklineType.WinLoss:
			StiWinLossSparklinesCellPainter.Draw(context, rect, array, positiveColor, negativeColor);
			break;
		default:
			throw new StiTypeNotRecognizedException((object)sparkline.Type);
		}
		context.Render(((RectangleD)(ref rect)).ToRectangleF());
	}
}
