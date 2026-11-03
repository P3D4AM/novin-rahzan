using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Maps;
using Stimulsoft.Report.Maps.Helpers;

namespace Stimulsoft.Report.Painters;

public class StiMapGdiPainter : StiComponentGdiPainter
{
	public bool UseBackground { get; set; } = true;


	public void DrawMap(StiMap map, Graphics g, RectangleF rect, bool drawBorder, bool drawFormatting, bool useZoom)
	{
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Expected O, but got Unknown
		if (drawFormatting)
		{
			RectangleF rectangleF = new RectangleF(rect.Location, rect.Size);
			StiMapStyle mapStyle = StiMap.GetMapStyle(map);
			if (mapStyle != null)
			{
				StiDrawing.FillRectangle(g, mapStyle.BackColor, (double)rectangleF.Left, (double)rectangleF.Top, (double)rectangleF.Width, (double)rectangleF.Height);
			}
			else if (map.Brush is StiSolidBrush && ((StiSolidBrush)map.Brush).Color == Color.Transparent && map.Report.Info.FillComponent && map.IsDesigning)
			{
				Color color = Color.FromArgb(150, Color.White);
				StiDrawing.FillRectangle(g, color, (double)rectangleF.Left, (double)rectangleF.Top, (double)rectangleF.Width, (double)rectangleF.Height);
			}
			else
			{
				StiDrawing.FillRectangle(g, map.GetStyleBackground(), rectangleF);
			}
			GraphicsState val = g.Save();
			g.SetClip(rectangleF, (CombineMode)1);
			g.TranslateTransform((float)(int)rectangleF.X, (float)(int)rectangleF.Y);
			rectangleF.X = 0f;
			rectangleF.Y = 0f;
			if (!drawBorder)
			{
				rectangleF.Width--;
				rectangleF.Height--;
			}
			if (map.MapMode == StiMapMode.Online)
			{
				PaintOnlineMap(g, rectangleF, map);
			}
			else
			{
				map.GetMapData();
				StiGdiMapContextPainter stiGdiMapContextPainter = new StiGdiMapContextPainter(map);
				stiGdiMapContextPainter.AnimationEngine = base.AnimationEngine;
				if (base.AnimationEngine != null)
				{
					base.AnimationEngine.RegisterContextPainter(map, stiGdiMapContextPainter);
				}
				stiGdiMapContextPainter.Render(g, useZoom);
			}
			g.Restore(val);
		}
		if ((!drawBorder || !map.Border.Topmost) && (!drawFormatting || map.Border.Topmost))
		{
			return;
		}
		if ((int)map.Border.Side == 0 && map.IsDesigning)
		{
			Pen val2 = new Pen(Color.Gray);
			try
			{
				val2.DashStyle = (DashStyle)1;
				StiDrawing.DrawRectangle(g, val2, (double)rect.Left, (double)rect.Top, (double)rect.Width, (double)rect.Height);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		if (map.HighlightState == StiHighlightState.Hide)
		{
			PaintBorder(map, g, RectangleD.CreateFromRectangle(rect), drawBorderFormatting: true, drawBorderSides: true);
		}
	}

	public void PaintOnlineMap(Graphics g, RectangleF rect, StiMap map)
	{
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		object lastImage = StiMapDrawingCache.GetLastImage(map);
		Bitmap val = (Bitmap)((lastImage is Bitmap) ? lastImage : null);
		Size size = rect.Size.ToSize();
		try
		{
			if (val == null)
			{
				List<string> list = new List<string>();
				if (!string.IsNullOrEmpty(map.PushPins))
				{
					object obj = JsonConvert.DeserializeObject(map.PushPins);
					list = (List<string>)((JToken)((obj is JArray) ? obj : null)).ToObject(typeof(List<string>));
				}
				else
				{
					list.Add("pp=0,0;60");
				}
				if (map.IsDesigning)
				{
					if (UseBackground)
					{
						StiBingMapDataHelper.CreateImageBackground(map, list, size);
						return;
					}
					try
					{
						val = StiBingMapHelper.GetImage(size, list);
					}
					catch
					{
					}
					if (val != null)
					{
						g.DrawImage((Image)(object)val, new Rectangle(0, 0, (int)rect.Width, (int)rect.Height));
					}
				}
				else
				{
					val = StiBingMapHelper.GetImage(size, list);
					if (val != null)
					{
						StiMapDrawingCache.StoreLastImage(map, val);
						g.DrawImage((Image)(object)val, new Rectangle(0, 0, (int)rect.Width, (int)rect.Height));
					}
				}
			}
			else
			{
				g.DrawImage((Image)(object)val, new Rectangle(0, 0, (int)rect.Width, (int)rect.Height));
			}
		}
		catch (Exception ex)
		{
			g.DrawRectangle(Pens.White, new Rectangle(0, 0, (int)rect.Width, (int)rect.Height));
			Font val2 = new Font("Arial", 10f);
			try
			{
				string text = "BING: " + ex.Message;
				SizeF sizeF = g.MeasureString(text, val2);
				g.DrawString(text, val2, Brushes.Gray, (rect.Width - sizeF.Width) / 2f, (rect.Height - sizeF.Height) / 2f);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
	}

	public Bitmap GetScaleImage(StiComponent component)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		StiMap stiMap = component as StiMap;
		double zoom = stiMap.Report.Info.Zoom;
		stiMap.Report.Info.Zoom = 2.0;
		RectangleD paintRectangle = stiMap.GetPaintRectangle();
		((RectangleD)(ref paintRectangle)).X = 0.0;
		((RectangleD)(ref paintRectangle)).Y = 0.0;
		int num = (int)((RectangleD)(ref paintRectangle)).Width + 2;
		int num2 = (int)((RectangleD)(ref paintRectangle)).Height + 2;
		Bitmap val = new Bitmap(num, num2);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			val2.PageUnit = (GraphicsUnit)2;
			val2.Clear(StiBrush.ToColor(stiMap.GetStyleBackground()));
			((RectangleD)(ref paintRectangle)).X = 0.0;
			((RectangleD)(ref paintRectangle)).Y = 0.0;
			DrawMap(stiMap, val2, ((RectangleD)(ref paintRectangle)).ToRectangleF(), drawBorder: false, drawFormatting: true, useZoom: true);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		stiMap.Report.Info.Zoom = zoom;
		return val;
	}

	public override Image GetImage(StiComponent component, ref float zoom, StiExportFormat format)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Expected O, but got Unknown
		StiMap stiMap = component as StiMap;
		double zoom2 = stiMap.Report.Info.Zoom;
		if (format != StiExportFormat.HtmlSpan && format != StiExportFormat.HtmlDiv && format != StiExportFormat.HtmlTable && UseBackground)
		{
			zoom *= 2f;
		}
		stiMap.Report.Info.Zoom = zoom;
		RectangleD paintRectangle = stiMap.GetPaintRectangle();
		((RectangleD)(ref paintRectangle)).X = 0.0;
		((RectangleD)(ref paintRectangle)).Y = 0.0;
		int num = (int)((RectangleD)(ref paintRectangle)).Width + 2;
		int num2 = (int)((RectangleD)(ref paintRectangle)).Height + 2;
		Bitmap val = new Bitmap(num, num2);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			val2.PageUnit = (GraphicsUnit)2;
			if ((format == StiExportFormat.Pdf || format == StiExportFormat.ImagePng) && stiMap.Brush != null && ((stiMap.Brush is StiSolidBrush && ((StiSolidBrush)/*isinst with value type is only supported in some contexts*/).Color.A == 0) || stiMap.Brush is StiEmptyBrush))
			{
				val2.Clear(Color.FromArgb(1, 255, 255, 255));
			}
			else
			{
				val2.Clear(StiBrush.ToColor(stiMap.GetStyleBackground()));
			}
			((RectangleD)(ref paintRectangle)).X = 0.0;
			((RectangleD)(ref paintRectangle)).Y = 0.0;
			DrawMap(stiMap, val2, ((RectangleD)(ref paintRectangle)).ToRectangleF(), drawBorder: true, drawFormatting: true, useZoom: true);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		stiMap.Report.Info.Zoom = zoom2;
		return (Image)(object)val;
	}

	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Invalid comparison between Unknown and I4
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		StiMap stiMap = component as StiMap;
		stiMap.InvokePainting(stiMap, e);
		if (!e.Cancel && (stiMap.Enabled || stiMap.IsDesigning))
		{
			Graphics graphics = e.Graphics;
			RectangleD paintRectangle = stiMap.GetPaintRectangle();
			if (((RectangleD)(ref paintRectangle)).Width > 0.0 && ((RectangleD)(ref paintRectangle)).Height > 0.0)
			{
				RectangleD clipRectangle = e.ClipRectangle;
				if (((RectangleD)(ref clipRectangle)).IsEmpty || ((RectangleD)(ref paintRectangle)).IntersectsWith(e.ClipRectangle))
				{
					if (StiOptions.Print.ChartAsBitmap && stiMap.IsPrinting)
					{
						float zoom = 1f;
						Image image = GetImage(stiMap, ref zoom, StiExportFormat.None);
						try
						{
							graphics.DrawImage(image, ((RectangleD)(ref paintRectangle)).ToRectangleF());
						}
						finally
						{
							((IDisposable)image)?.Dispose();
						}
					}
					else
					{
						StiProgressStatus val = StiComponentProgressHelper.Contains((IStiAppComponent)(object)component);
						if ((int)val != 0 && stiMap.IsDesigning)
						{
							StiDrawing.FillRectangle(graphics, Color.FromArgb(153, Color.White), paintRectangle);
							StiComponentGdiPainter.PaintProgress(graphics, paintRectangle, val);
							return;
						}
						DrawMap(stiMap, graphics, ((RectangleD)(ref paintRectangle)).ToRectangleF(), e.DrawTopmostBorderSides, e.DrawBorderFormatting, useZoom: true);
					}
					if (e.DrawBorderFormatting)
					{
						if (stiMap.IsDesigning)
						{
							PaintQuickButtons(stiMap, e.Graphics);
						}
						PaintEvents(stiMap, e.Graphics, paintRectangle);
						PaintConditions(stiMap, e.Graphics, paintRectangle);
					}
					if (stiMap.HighlightState == StiHighlightState.Hide && (int)stiMap.Border.Side != 15)
					{
						PaintMarkers(stiMap, graphics, paintRectangle);
					}
				}
			}
		}
		e.Cancel = false;
		stiMap.InvokePainted(stiMap, e);
	}
}
