using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Painters;

public class StiViewGdiPainter : StiComponentGdiPainter
{
	private const int maxImageMemorySize = 45000000;

	internal static readonly string StiGetRawFormatTag = "GetRawFormat";

	public virtual void PaintImage(StiView view, Graphics g, RectangleD rect, float svgScale = 1f)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected I4, but got Unknown
		//IL_040c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0411: Unknown result type (might be due to invalid IL or missing references)
		//IL_0413: Unknown result type (might be due to invalid IL or missing references)
		//IL_0426: Expected I4, but got Unknown
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Expected I4, but got Unknown
		//IL_046e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0473: Unknown result type (might be due to invalid IL or missing references)
		//IL_0475: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_048a: Expected I4, but got Unknown
		//IL_05ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b2: Expected O, but got Unknown
		if (!view.ExistImageToDraw())
		{
			return;
		}
		Image val = view.TakeGdiImageToDraw(svgScale);
		if (val == null)
		{
			return;
		}
		if (view.TagValue as string == StiGetRawFormatTag)
		{
			view.TagValue = val.RawFormat;
		}
		val = RotateImage(val, view);
		bool flag = val is Metafile;
		if (view is StiImage)
		{
			rect = (view as StiImage).ConvertImageMargins(rect, convert: true);
		}
		RectangleF rectangleF = new RectangleF((float)((RectangleD)(ref rect)).X, (float)((RectangleD)(ref rect)).Y, (float)((RectangleD)(ref rect)).Width, (float)((RectangleD)(ref rect)).Height);
		RectangleF empty = RectangleF.Empty;
		RectangleF rectangleF2 = ((RectangleD)(ref rect)).ToRectangleF();
		if (!view.Stretch && (!(view is StiImage) || !(view as StiImage).Icon.HasValue))
		{
			double num = (view.IsExporting ? 1.0 : StiScale.Factor);
			empty = new RectangleF(0f, 0f, (float)(((RectangleD)(ref rect)).Width / (view.Report.Info.Zoom * view.MultipleFactor * num)), (float)(((RectangleD)(ref rect)).Height / (view.Report.Info.Zoom * view.MultipleFactor * num)));
			float num2 = (float)(view.Page.Zoom * view.MultipleFactor * num);
			StiHorAlignment horAlignment = view.HorAlignment;
			switch (horAlignment - 1)
			{
			case 1:
				if (rectangleF.Width > (float)val.Width * num2)
				{
					rectangleF.X += (rectangleF.Width - (float)val.Width * num2) / 2f;
					break;
				}
				empty.X = ((float)val.Width - rectangleF.Width / num2) / 2f;
				empty.Width = rectangleF.Width / num2;
				break;
			case 2:
				if (rectangleF.Width > (float)val.Width * num2)
				{
					rectangleF.X = rectangleF.Right - (float)val.Width * num2;
					break;
				}
				empty.X = (float)val.Width - rectangleF.Width / num2;
				empty.Width = rectangleF.Width / num2;
				break;
			}
			StiVertAlignment vertAlignment = view.VertAlignment;
			switch ((int)vertAlignment)
			{
			case 1:
				if (rectangleF.Height > (float)val.Height * num2)
				{
					rectangleF.Y += (rectangleF.Height - (float)val.Height * num2) / 2f;
					break;
				}
				empty.Y = ((float)val.Height - rectangleF.Height / num2) / 2f;
				empty.Height = rectangleF.Height / num2;
				break;
			case 2:
				if (rectangleF.Height > (float)val.Height * num2)
				{
					rectangleF.Y = rectangleF.Bottom - (float)val.Height * num2;
					break;
				}
				empty.Y = (float)val.Height - rectangleF.Height / num2;
				empty.Height = rectangleF.Height / num2;
				break;
			}
			if (rectangleF.Width > (float)val.Width * num2)
			{
				rectangleF.Width = (float)val.Width * num2;
				empty.Width = val.Width;
			}
			if (rectangleF.Height > (float)val.Height * num2)
			{
				rectangleF.Height = (float)val.Height * num2;
				empty.Height = val.Height;
			}
		}
		else
		{
			empty = new RectangleF(0f, 0f, val.Width, val.Height);
			if (view.AspectRatio)
			{
				float num3 = rectangleF.Width / empty.Width;
				float num4 = rectangleF.Height / empty.Height;
				if (num3 > num4)
				{
					rectangleF.Width = empty.Width * num4;
				}
				else
				{
					rectangleF.Height = empty.Height * num3;
				}
				StiVertAlignment vertAlignment2 = view.VertAlignment;
				switch ((int)vertAlignment2)
				{
				case 1:
					rectangleF.Y = (float)((double)rectangleF.Y + (((RectangleD)(ref rect)).Height - (double)rectangleF.Height) / 2.0);
					break;
				case 2:
					rectangleF.Y = (float)(((RectangleD)(ref rect)).Bottom - (double)rectangleF.Height);
					break;
				}
				StiHorAlignment horAlignment2 = view.HorAlignment;
				switch (horAlignment2 - 1)
				{
				case 1:
					rectangleF.X = (float)((double)rectangleF.X + (((RectangleD)(ref rect)).Width - (double)rectangleF.Width) / 2.0);
					break;
				case 2:
					rectangleF.X = (float)(((RectangleD)(ref rect)).Right - (double)rectangleF.Width);
					break;
				}
			}
		}
		GraphicsState val2 = g.Save();
		g.SetClip(rectangleF2, (CombineMode)1);
		if (!StiOptions.Engine.DisableAntialiasingInPainters)
		{
			if (!view.Smoothing && view.IsPrinting)
			{
				g.InterpolationMode = (InterpolationMode)5;
				g.SmoothingMode = (SmoothingMode)3;
			}
			else
			{
				g.InterpolationMode = (InterpolationMode)6;
			}
		}
		if (view.IsPrinting && val is Metafile && StiOptions.Print.MetafileAsBitmap)
		{
			float num5 = g.DpiX / 100f;
			float num6 = rectangleF.Width * num5 * (rectangleF.Height * num5) * 4f;
			if (num6 > 45000000f)
			{
				num5 /= (float)Math.Sqrt(num6 / 45000000f);
			}
			int num7 = (int)(rectangleF.Width * num5);
			int num8 = (int)(rectangleF.Height * num5);
			if (num7 > 1 && num8 > 1)
			{
				Bitmap val3 = new Bitmap(num7, num8, (PixelFormat)2498570);
				try
				{
					Graphics val4 = Graphics.FromImage((Image)(object)val3);
					try
					{
						val4.DrawImage(val, new RectangleF(0f, 0f, num7, num8));
						g.DrawImage((Image)(object)val3, rectangleF);
					}
					finally
					{
						((IDisposable)val4)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
		}
		else if (flag)
		{
			if (rectangleF.Width > 0f && rectangleF.Height > 0f)
			{
				g.DrawImage(val, rectangleF);
			}
		}
		else if (!StiOptions.Engine.Image.ConvertSvgToBitmap && StiSvgHelper.IsSvg(view.ImageBytesToDraw))
		{
			StiSvgHelper.DrawSvg(view.ImageBytesToDraw, ((RectangleD)(ref rect)).ToRectangleF(), view.Stretch, view.AspectRatio, view.Page.Zoom, g);
		}
		else
		{
			g.DrawImage(val, Rectangle.Round(rectangleF), Rectangle.Round(empty), (GraphicsUnit)2);
		}
		g.Restore(val2);
		val.Dispose();
	}

	protected Image RotateImage(Image gdiImage, StiView viewComp)
	{
		if (gdiImage == null)
		{
			return null;
		}
		if (viewComp is StiImage { ImageRotation: not StiImageRotation.None } stiImage)
		{
			return StiImageHelper.RotateImage(gdiImage, stiImage.ImageRotation, disposeOriginal: true);
		}
		return gdiImage;
	}

	public override Image GetImage(StiComponent component, ref float zoom, StiExportFormat format)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		StiView stiView = (StiView)component;
		if (!stiView.ExistImageToDraw() || stiView.Report == null)
		{
			return null;
		}
		double zoom2 = stiView.Page.Zoom;
		stiView.Report.Info.Zoom = zoom;
		try
		{
			RectangleD val = component.ComponentToPage(component.ClientRectangle);
			RectangleD rect = ((RectangleD)(ref val)).Normalize();
			val = stiView.Report.Unit.ConvertToHInches(rect);
			rect = ((RectangleD)(ref val)).Multiply((double)zoom);
			((RectangleD)(ref rect)).X = 0.0;
			((RectangleD)(ref rect)).Y = 0.0;
			stiView.Report.Info.Zoom = zoom;
			int num = (int)((RectangleD)(ref rect)).Width + 1;
			int num2 = (int)((RectangleD)(ref rect)).Height + 1;
			Bitmap val2 = new Bitmap(num, num2);
			Graphics val3 = Graphics.FromImage((Image)(object)val2);
			try
			{
				val3.PageUnit = (GraphicsUnit)2;
				if (stiView.Brush is StiSolidBrush)
				{
					Color color = StiBrush.ToColor(stiView.Brush);
					if (color.A == 0)
					{
						if (format == StiExportFormat.Pdf || format == StiExportFormat.Xps || format == StiExportFormat.ImagePng)
						{
							color = Color.FromArgb(1, 255, 255, 255);
						}
						else
						{
							color = Color.White;
							if (stiView.Page != null)
							{
								color = StiBrush.ToColor(stiView.Page.Brush);
							}
							if (color.Equals((object?)Color.Transparent) || color.Equals((object?)Color.Empty))
							{
								color = Color.White;
							}
						}
					}
					val3.Clear(color);
				}
				else if (stiView.Brush is StiEmptyBrush)
				{
					if (format == StiExportFormat.Pdf || format == StiExportFormat.Xps || format == StiExportFormat.ImagePng)
					{
						val3.Clear(Color.FromArgb(1, 255, 255, 255));
					}
					else
					{
						Color color2 = StiBrush.ToColor(stiView.Page?.Brush);
						if (color2.Equals((object?)Color.Transparent) || color2.Equals((object?)Color.Empty))
						{
							color2 = Color.White;
						}
						val3.Clear(Color.White);
					}
				}
				else
				{
					StiDrawing.FillRectangle(val3, stiView.Brush, new RectangleD(0.0, 0.0, (double)num, (double)num2));
				}
				PaintImage(stiView, val3, new RectangleD(0.0, 0.0, (double)num, (double)num2), zoom);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			return (Image)(object)val2;
		}
		catch
		{
			return null;
		}
		finally
		{
			if (stiView.Report != null)
			{
				stiView.Report.Info.Zoom = zoom2;
			}
		}
	}

	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		StiView stiView = (StiView)component;
		if (!e.DrawBorderFormatting && e.DrawTopmostBorderSides && !stiView.Border.Topmost)
		{
			return;
		}
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		if (e.DrawBorderFormatting)
		{
			stiView.InvokePainting(stiView, e);
		}
		if (!e.Cancel && (stiView.Enabled || stiView.IsDesigning))
		{
			Graphics graphics = e.Graphics;
			RectangleD paintRectangle = stiView.GetPaintRectangle();
			if (((RectangleD)(ref paintRectangle)).Width > 0.0 && ((RectangleD)(ref paintRectangle)).Height > 0.0)
			{
				RectangleD clipRectangle = e.ClipRectangle;
				if (((RectangleD)(ref clipRectangle)).IsEmpty || ((RectangleD)(ref paintRectangle)).IntersectsWith(e.ClipRectangle))
				{
					if (e.DrawBorderFormatting)
					{
						if (stiView.Brush is StiSolidBrush && ((StiSolidBrush)stiView.Brush).Color == Color.Transparent && stiView.Report.Info.FillComponent && stiView.IsDesigning && !stiView.ExistImageToDraw())
						{
							Color color = Color.FromArgb(50, Color.Green);
							StiDrawing.FillRectangle(graphics, color, ((RectangleD)(ref paintRectangle)).Left, ((RectangleD)(ref paintRectangle)).Top, ((RectangleD)(ref paintRectangle)).Width, ((RectangleD)(ref paintRectangle)).Height);
						}
						else
						{
							StiDrawing.FillRectangle(graphics, stiView.Brush, paintRectangle);
						}
					}
					if (e.DrawBorderFormatting)
					{
						if (stiView.ObjectToDraw != null && StiOptions.Configuration.IsWPF)
						{
							float zoom = graphics.DpiX / 96f;
							Image image = stiView.GetImage(ref zoom, StiExportFormat.ImagePng);
							stiView.PutImageToDraw(image);
							image.Dispose();
						}
						if (stiView.ExistImageToDraw())
						{
							float svgScale = (float)Math.Min(3.0, Math.Max(1.0, stiView.Page.Zoom * StiScale.Factor));
							PaintImage(stiView, graphics, paintRectangle, svgScale);
						}
					}
					if (e.DrawBorderFormatting)
					{
						PaintMarkers(stiView, graphics, paintRectangle);
					}
					if (stiView.HighlightState == StiHighlightState.Hide)
					{
						PaintBorder(stiView, graphics, paintRectangle, stiView.Page.Zoom, e.DrawBorderFormatting, e.DrawTopmostBorderSides || !stiView.Border.Topmost);
					}
					if (e.DrawBorderFormatting)
					{
						if (stiView.IsDesigning)
						{
							PaintQuickButtons(stiView, e.Graphics);
						}
						PaintEvents(stiView, e.Graphics, paintRectangle);
						PaintConditions(stiView, e.Graphics, paintRectangle);
					}
				}
			}
		}
		e.Cancel = false;
		if (e.DrawBorderFormatting)
		{
			stiView.InvokePainted(stiView, e);
		}
	}
}
