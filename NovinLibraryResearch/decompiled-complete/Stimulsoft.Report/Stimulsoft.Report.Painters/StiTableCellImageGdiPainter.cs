using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Images;

namespace Stimulsoft.Report.Painters;

public class StiTableCellImageGdiPainter : StiViewGdiPainter
{
	public override Image GetImage(StiComponent component, ref float zoom, StiExportFormat format)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		StiView stiView = (StiView)component;
		if (!stiView.ExistImageToDraw())
		{
			return null;
		}
		double zoom2 = stiView.Page.Zoom;
		stiView.Report.Info.Zoom = zoom;
		try
		{
			RectangleD paintRectangle = stiView.GetPaintRectangle();
			((RectangleD)(ref paintRectangle)).X = 0.0;
			((RectangleD)(ref paintRectangle)).Y = 0.0;
			stiView.Report.Info.Zoom = zoom;
			int num = (int)((RectangleD)(ref paintRectangle)).Width + 1;
			int num2 = (int)((RectangleD)(ref paintRectangle)).Height + 1;
			Bitmap val = new Bitmap(num, num2);
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				val2.PageUnit = (GraphicsUnit)2;
				if (stiView.Brush is StiSolidBrush)
				{
					Color color = StiBrush.ToColor(stiView.Brush);
					if (color == Color.Transparent)
					{
						color = ((format != StiExportFormat.Pdf && format != StiExportFormat.Xps && format != StiExportFormat.ImagePng) ? Color.White : Color.FromArgb(1, 255, 255, 255));
					}
					val2.Clear(color);
				}
				else if (stiView.Brush is StiEmptyBrush)
				{
					val2.Clear(Color.White);
				}
				else
				{
					StiDrawing.FillRectangle(val2, stiView.Brush, new RectangleD(0.0, 0.0, (double)num, (double)num2));
				}
				PaintImage(stiView, val2, new RectangleD(0.0, 0.0, (double)num, (double)num2));
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			return (Image)(object)val;
		}
		catch
		{
			return null;
		}
		finally
		{
			stiView.Report.Info.Zoom = zoom2;
		}
	}

	private void DrawText(Graphics g, StiTableCellImage image, RectangleD rect)
	{
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Expected O, but got Unknown
		string text = string.Empty;
		if (image.ExistImageToDraw() || !image.IsDesigning)
		{
			return;
		}
		if (!string.IsNullOrEmpty(image.DataColumn))
		{
			text = string.Format("{0}: {1}", StiLocalization.Get("PropertyMain", "DataColumn"), image.DataColumn);
		}
		else if (image.ImageURL != null && image.ImageURL.Value != null && image.ImageURL.Value.Length > 0)
		{
			text = string.Format("{0}: {1}", StiLocalization.Get("PropertyMain", "ImageURL"), StiExpressionPacker.PackExpression(image.ImageURL.Value, image.Report.Designer, useBraces: true));
		}
		else if (!string.IsNullOrEmpty(image.File))
		{
			text = string.Format("{0}: {1}", StiLocalization.Get("PropertyMain", "File"), image.File);
		}
		else if (image.ImageData != null && image.ImageData.Value != null && image.ImageData.Value.Length > 0)
		{
			text = string.Format("{0}: {1}", StiLocalization.Get("PropertyMain", "ImageData"), StiExpressionPacker.PackExpression(image.ImageData.Value, image.Report.Designer, useBraces: true));
		}
		if (text.Length > 0)
		{
			Font val = new Font("Arial", 9f * (float)image.Report.Info.Zoom);
			try
			{
				g.DrawString(text, val, Brushes.Black, ((RectangleD)(ref rect)).ToRectangleF());
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}

	private void DrawCellImage(Graphics g, RectangleD rect)
	{
		if (!(((RectangleD)(ref rect)).Width <= (double)StiScale.I9) && !(((RectangleD)(ref rect)).Height <= (double)StiScale.I9))
		{
			Image val = StiReportImages.Styles.Image((StiImageSize)0);
			g.DrawImageUnscaled(val, new Rectangle((int)((RectangleD)(ref rect)).X, (int)((RectangleD)(ref rect)).Y, val.Width, val.Height));
		}
	}

	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		StiTableCellImage stiTableCellImage = (StiTableCellImage)component;
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
			if (!stiTableCellImage.Enabled)
			{
				e.Cancel = false;
				stiView.InvokePainted(stiView, e);
				return;
			}
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
						if (stiTableCellImage.Brush is StiSolidBrush && ((StiSolidBrush)stiTableCellImage.Brush).Color == Color.Transparent && stiTableCellImage.Report.Info.FillComponent && stiTableCellImage.IsDesigning)
						{
							Color color = Color.FromArgb(150, Color.White);
							StiDrawing.FillRectangle(graphics, color, ((RectangleD)(ref paintRectangle)).Left, ((RectangleD)(ref paintRectangle)).Top, ((RectangleD)(ref paintRectangle)).Width, ((RectangleD)(ref paintRectangle)).Height);
						}
						else
						{
							StiDrawing.FillRectangle(graphics, stiTableCellImage.Brush, paintRectangle);
						}
						if (stiTableCellImage.IsSelected)
						{
							Color color2 = Color.FromArgb(80, Color.FromArgb(168, 205, 241));
							StiDrawing.FillRectangle(graphics, color2, paintRectangle);
						}
					}
					if (e.DrawBorderFormatting && stiView.ExistImageToDraw())
					{
						PaintImage(stiView, graphics, paintRectangle);
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
						if (stiTableCellImage.IsDesigning)
						{
							DrawText(graphics, stiTableCellImage, paintRectangle);
							DrawCellImage(graphics, paintRectangle);
						}
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

	public override void PaintSelection(StiComponent component, StiPaintEventArgs e)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = e.Graphics;
		if (component.IsDesigning && component.IsSelected && !component.Report.Info.IsComponentsMoving)
		{
			RectangleD paintRectangle = component.GetPaintRectangle();
			int i = StiScale.I2;
			SolidBrush val = new SolidBrush(Color.DimGray);
			try
			{
				StiDrawing.DrawSelectedRectangle(graphics, i, (Brush)(object)val, paintRectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}
}
