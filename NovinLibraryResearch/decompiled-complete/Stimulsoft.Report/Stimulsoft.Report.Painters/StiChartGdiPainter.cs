using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Painters;

public class StiChartGdiPainter : StiComponentGdiPainter
{
	public void DrawChart(StiChart chart, Graphics g, RectangleF rect, bool useMargins, bool useBackground, double? zoom = null)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		if (useBackground)
		{
			if (chart.Brush is StiSolidBrush && ((StiSolidBrush)chart.Brush).Color == Color.Transparent && chart.Report != null && chart.Report.Info.FillComponent && chart.IsDesigning)
			{
				Color color = Color.FromArgb(150, Color.White);
				StiDrawing.FillRectangle(g, color, (double)rect.Left, (double)rect.Top, (double)rect.Width, (double)rect.Height);
			}
			else
			{
				StiDrawing.FillRectangle(g, chart.Brush, rect);
			}
		}
		if (!zoom.HasValue && chart?.Report?.Info != null)
		{
			zoom = chart.Report.Info.Zoom * StiScale.Factor;
		}
		GraphicsState val = g.Save();
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 1f;
		float num5 = 1f;
		switch (chart.Rotation)
		{
		case StiImageRotation.Rotate90CCW:
			num = -90f;
			num3 = rect.Height;
			break;
		case StiImageRotation.Rotate90CW:
			num = 90f;
			num2 = rect.Width;
			break;
		case StiImageRotation.Rotate180:
			num = -180f;
			num2 = rect.Width;
			num3 = rect.Height;
			break;
		case StiImageRotation.FlipVertical:
			num5 = -1f;
			num3 = rect.Height;
			break;
		case StiImageRotation.FlipHorizontal:
			num4 = -1f;
			num2 = rect.Width;
			break;
		}
		g.SetClip(rect, (CombineMode)1);
		g.TranslateTransform((float)(int)rect.X, (float)(int)rect.Y);
		StiImageRotation rotation = chart.Rotation;
		if ((uint)(rotation - 1) <= 1u)
		{
			RectangleF rectangleF = rect;
			rect.Width = rectangleF.Height;
			rect.Height = rectangleF.Width;
		}
		rect.X = 0f;
		rect.Y = 0f;
		StiGdiContextPainter stiGdiContextPainter = new StiGdiContextPainter(g);
		stiGdiContextPainter.AnimationEngine = base.AnimationEngine;
		StiContext stiContext2 = (stiGdiContextPainter.Context = new StiContext(stiGdiContextPainter, isGdi: true, isWpf: false, chart.IsPrinting, (float)zoom.GetValueOrDefault(1.0)));
		if (!useMargins)
		{
			rect.Width--;
			rect.Height--;
		}
		chart.IsAnimation = base.AnimationEngine != null;
		if (base.AnimationEngine == null || !base.AnimationEngine.IsRunning(chart))
		{
			StiCellGeom stiCellGeom = chart.Core.Render(stiContext2, new RectangleF(0f, 0f, rect.Width, rect.Height), useMargins);
			stiCellGeom.DrawGeom(stiContext2);
			chart.PreviousAnimations = stiContext2.Animations;
		}
		chart.IsAnimation = false;
		if (base.AnimationEngine != null)
		{
			base.AnimationEngine.RegisterContextPainter(chart, stiGdiContextPainter);
		}
		g.TranslateTransform(num2, num3);
		g.ScaleTransform(num4, num5);
		g.RotateTransform(num);
		stiContext2.Render(rect);
		g.Restore(val);
	}

	public void PaintChart(StiChart chart, Graphics g, RectangleF rect, bool drawBorder, bool drawFormatting)
	{
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Expected O, but got Unknown
		if (drawFormatting)
		{
			if (StiOptions.Print.ChartAsBitmap && chart.IsPrinting)
			{
				float zoom = 1f;
				Image image = GetImage(chart, ref zoom, StiExportFormat.None);
				try
				{
					g.DrawImage(image, rect);
				}
				finally
				{
					((IDisposable)image)?.Dispose();
				}
			}
			else
			{
				DrawChart(chart, g, rect, useMargins: true, useBackground: true);
			}
		}
		if ((!drawBorder || !chart.Border.Topmost) && (!drawFormatting || chart.Border.Topmost))
		{
			return;
		}
		if ((int)chart.Border.Side == 0 && chart.IsDesigning && !(chart.Page is IStiDashboard))
		{
			Pen val = new Pen(Color.Gray);
			try
			{
				val.DashStyle = (DashStyle)1;
				StiDrawing.DrawRectangle(g, val, (double)rect.Left, (double)rect.Top, (double)rect.Width, (double)rect.Height);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		if (chart.HighlightState == StiHighlightState.Hide)
		{
			PaintBorder(chart, g, RectangleD.CreateFromRectangle(rect), drawBorderFormatting: true, drawBorderSides: true);
		}
	}

	public override Image GetImage(StiComponent component, ref float zoom, StiExportFormat format)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		StiChart stiChart = component as StiChart;
		double zoom2 = stiChart.Report.Info.Zoom;
		if (format != StiExportFormat.HtmlSpan && format != StiExportFormat.HtmlDiv && format != StiExportFormat.HtmlTable)
		{
			zoom *= 2f;
		}
		stiChart.Report.Info.Zoom = zoom;
		RectangleD paintRectangle = stiChart.GetPaintRectangle();
		((RectangleD)(ref paintRectangle)).X = 0.0;
		((RectangleD)(ref paintRectangle)).Y = 0.0;
		int num = (int)((RectangleD)(ref paintRectangle)).Width + 2;
		int num2 = (int)((RectangleD)(ref paintRectangle)).Height + 2;
		Bitmap val = new Bitmap(num, num2);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			val2.PageUnit = (GraphicsUnit)2;
			if ((format == StiExportFormat.Pdf || format == StiExportFormat.ImagePng) && stiChart.Brush != null && ((stiChart.Brush is StiSolidBrush && ((StiSolidBrush)/*isinst with value type is only supported in some contexts*/).Color.A == 0) || stiChart.Brush is StiEmptyBrush))
			{
				val2.Clear(Color.FromArgb(1, 255, 255, 255));
			}
			else if ((format == StiExportFormat.Excel || format == StiExportFormat.Excel2007 || format == StiExportFormat.Word2007 || format == StiExportFormat.Rtf || format == StiExportFormat.RtfFrame || format == StiExportFormat.RtfTable || format == StiExportFormat.RtfWinWord) && stiChart.Brush != null && ((stiChart.Brush is StiSolidBrush && ((StiSolidBrush)/*isinst with value type is only supported in some contexts*/).Color.A < 16) || stiChart.Brush is StiEmptyBrush))
			{
				val2.Clear(Color.White);
			}
			else
			{
				val2.Clear(StiBrush.ToColor(stiChart.Brush));
			}
			((RectangleD)(ref paintRectangle)).X = 0.0;
			((RectangleD)(ref paintRectangle)).Y = 0.0;
			DrawChart(stiChart, val2, ((RectangleD)(ref paintRectangle)).ToRectangleF(), useMargins: true, useBackground: true);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		stiChart.Report.Info.Zoom = zoom2;
		return (Image)(object)val;
	}

	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Invalid comparison between Unknown and I4
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		StiChart stiChart = component as StiChart;
		stiChart.InvokePainting(stiChart, e);
		if (!e.Cancel && (stiChart.Enabled || stiChart.IsDesigning))
		{
			Graphics graphics = e.Graphics;
			RectangleD paintRectangle = stiChart.GetPaintRectangle();
			if (((RectangleD)(ref paintRectangle)).Width > 0.0 && ((RectangleD)(ref paintRectangle)).Height > 0.0)
			{
				RectangleD clipRectangle = e.ClipRectangle;
				if (((RectangleD)(ref clipRectangle)).IsEmpty || ((RectangleD)(ref paintRectangle)).IntersectsWith(e.ClipRectangle))
				{
					PaintChart(stiChart, graphics, ((RectangleD)(ref paintRectangle)).ToRectangleF(), e.DrawTopmostBorderSides, e.DrawBorderFormatting);
					if (stiChart.Series.Count == 0 && stiChart.IsDesigning && e.DrawBorderFormatting)
					{
						PaintNoDefinedStatus(graphics, ((RectangleD)(ref paintRectangle)).ToRectangleF(), component, "StiChart", Loc.GetMain("NoElements"));
					}
					if (stiChart.HighlightState == StiHighlightState.Hide && (int)stiChart.Border.Side != 15)
					{
						PaintMarkers(stiChart, graphics, paintRectangle);
					}
				}
			}
			PaintQuickButtons(stiChart, e.Graphics);
			PaintEvents(stiChart, e.Graphics, paintRectangle);
		}
		e.Cancel = false;
		stiChart.InvokePainted(stiChart, e);
	}
}
