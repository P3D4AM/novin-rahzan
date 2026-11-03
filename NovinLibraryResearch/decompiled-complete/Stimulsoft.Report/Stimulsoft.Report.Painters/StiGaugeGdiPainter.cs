using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Gauge.Painters;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Gauge.Helpers;

namespace Stimulsoft.Report.Painters;

public class StiGaugeGdiPainter : StiComponentGdiPainter
{
	internal bool IsDashboardMode { get; set; }

	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Expected O, but got Unknown
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		StiGauge stiGauge = component as StiGauge;
		stiGauge.InvokePainting(stiGauge, e);
		if (!e.Cancel && (component.Enabled || component.IsDesigning))
		{
			Graphics graphics = e.Graphics;
			RectangleD paintRectangle = component.GetPaintRectangle();
			if (((RectangleD)(ref paintRectangle)).Width > 0.0 && ((RectangleD)(ref paintRectangle)).Height > 0.0)
			{
				RectangleD clipRectangle = e.ClipRectangle;
				if (((RectangleD)(ref clipRectangle)).IsEmpty || ((RectangleD)(ref paintRectangle)).IntersectsWith(e.ClipRectangle))
				{
					if (e.DrawBorderFormatting)
					{
						DrawGauge(graphics, stiGauge, ((RectangleD)(ref paintRectangle)).ToRectangleF());
					}
					if ((e.DrawTopmostBorderSides && stiGauge.Border.Topmost) || (e.DrawBorderFormatting && !stiGauge.Border.Topmost))
					{
						if ((int)stiGauge.Border.Side == 0 && stiGauge.IsDesigning && !(stiGauge.Page is IStiDashboard))
						{
							Pen val = new Pen(Color.Gray);
							try
							{
								val.DashStyle = (DashStyle)1;
								StiDrawing.DrawRectangle(graphics, val, ((RectangleD)(ref paintRectangle)).Left, ((RectangleD)(ref paintRectangle)).Top, ((RectangleD)(ref paintRectangle)).Width, ((RectangleD)(ref paintRectangle)).Height);
							}
							finally
							{
								((IDisposable)val)?.Dispose();
							}
						}
						if (stiGauge.HighlightState == StiHighlightState.Hide)
						{
							PaintBorder(stiGauge, graphics, paintRectangle, drawBorderFormatting: true, drawBorderSides: true);
						}
					}
				}
			}
		}
		e.Cancel = false;
		stiGauge.InvokePainted(stiGauge, e);
	}

	public void DrawGauge(Graphics g, StiGauge gauge, RectangleF rect)
	{
		DrawGauge(g, gauge, rect, (gauge.Page != null) ? ((float)gauge.Page.Zoom) : 1f);
	}

	public void DrawGauge(Graphics g, StiGauge gauge, RectangleF rect, float zoom)
	{
		DrawGauge(g, gauge, rect, (gauge.Page != null) ? ((float)gauge.Page.Zoom) : 1f, isClip: true);
	}

	public void DrawGauge(Graphics g, StiGauge gauge, RectangleF rect, float zoom, bool isClip)
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Expected O, but got Unknown
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		gauge.ApplyStyle(gauge.GetGaugeStyle());
		if (!IsDashboardMode)
		{
			bool valueOrDefault = (gauge.Report?.Info?.FillComponent).GetValueOrDefault();
			if (gauge.Brush is StiSolidBrush && ((StiSolidBrush)gauge.Brush).Color == Color.Transparent && valueOrDefault && gauge.IsDesigning)
			{
				Color color = Color.FromArgb(150, Color.White);
				StiDrawing.FillRectangle(g, color, (double)rect.Left, (double)rect.Top, (double)rect.Width, (double)rect.Height);
			}
			else
			{
				StiDrawing.FillRectangle(g, gauge.Brush, rect);
			}
		}
		if (isClip)
		{
			g.SetClip(rect);
		}
		if (gauge.IsDesigning)
		{
			StiGaugeV2InitHelper.Prepare(gauge);
		}
		StiGdiGaugeContextPainter stiGdiGaugeContextPainter = new StiGdiGaugeContextPainter(g, gauge, rect, zoom);
		stiGdiGaugeContextPainter.AnimationEngine = base.AnimationEngine;
		if (base.AnimationEngine == null || !base.AnimationEngine.IsRunning(gauge))
		{
			gauge.DrawGauge(stiGdiGaugeContextPainter);
			gauge.PreviousAnimations = stiGdiGaugeContextPainter.Animations;
		}
		if (base.AnimationEngine != null)
		{
			base.AnimationEngine.RegisterContextPainter(gauge, stiGdiGaugeContextPainter);
		}
		stiGdiGaugeContextPainter.Render();
		if (isClip)
		{
			g.ResetClip();
		}
		if ((int)gauge.Border.Side == 0 && gauge.IsDesigning)
		{
			Pen val = new Pen(Color.Gray);
			try
			{
				val.DashStyle = (DashStyle)1;
				StiDrawing.DrawRectangle(g, val, RectangleD.CreateFromRectangle(rect));
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		if (gauge.HighlightState == StiHighlightState.Hide)
		{
			PaintBorder(gauge, g, RectangleD.CreateFromRectangle(rect), drawBorderFormatting: true, drawBorderSides: true);
		}
	}

	public override Image GetImage(StiComponent component, ref float zoom, StiExportFormat format)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		StiGauge stiGauge = component as StiGauge;
		double zoom2 = stiGauge.Report.Info.Zoom;
		if (format != StiExportFormat.HtmlSpan && format != StiExportFormat.HtmlDiv && format != StiExportFormat.HtmlTable)
		{
			zoom *= 2f;
		}
		stiGauge.Report.Info.Zoom = zoom;
		RectangleD paintRectangle = stiGauge.GetPaintRectangle();
		((RectangleD)(ref paintRectangle)).X = 0.0;
		((RectangleD)(ref paintRectangle)).Y = 0.0;
		int num = (int)((RectangleD)(ref paintRectangle)).Width + 2;
		int num2 = (int)((RectangleD)(ref paintRectangle)).Height + 2;
		Bitmap val = new Bitmap(num, num2);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			val2.PageUnit = (GraphicsUnit)2;
			if ((format == StiExportFormat.Pdf || format == StiExportFormat.ImagePng) && stiGauge.Brush != null && ((stiGauge.Brush is StiSolidBrush && ((StiSolidBrush)/*isinst with value type is only supported in some contexts*/).Color.A == 0) || stiGauge.Brush is StiEmptyBrush))
			{
				val2.Clear(Color.FromArgb(1, 255, 255, 255));
			}
			else if ((format == StiExportFormat.Excel || format == StiExportFormat.Excel2007 || format == StiExportFormat.Word2007 || format == StiExportFormat.Rtf || format == StiExportFormat.RtfFrame || format == StiExportFormat.RtfTable || format == StiExportFormat.RtfWinWord) && stiGauge.Brush != null && ((stiGauge.Brush is StiSolidBrush && ((StiSolidBrush)/*isinst with value type is only supported in some contexts*/).Color.A < 16) || stiGauge.Brush is StiEmptyBrush))
			{
				val2.Clear(Color.White);
			}
			else
			{
				val2.Clear(StiBrush.ToColor(stiGauge.Brush));
			}
			((RectangleD)(ref paintRectangle)).X = 0.0;
			((RectangleD)(ref paintRectangle)).Y = 0.0;
			RectangleF rect = ((RectangleD)(ref paintRectangle)).ToRectangleF();
			StiGdiGaugeContextPainter stiGdiGaugeContextPainter = new StiGdiGaugeContextPainter(val2, stiGauge, rect, zoom);
			stiGauge.DrawGauge(stiGdiGaugeContextPainter);
			stiGauge.PreviousAnimations = stiGdiGaugeContextPainter.Animations;
			stiGdiGaugeContextPainter.Render();
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		stiGauge.Report.Info.Zoom = zoom2;
		return (Image)(object)val;
	}
}
