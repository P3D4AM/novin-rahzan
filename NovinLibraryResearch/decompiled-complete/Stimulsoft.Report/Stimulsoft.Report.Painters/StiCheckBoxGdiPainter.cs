using System;
using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Painters;

public class StiCheckBoxGdiPainter : StiComponentGdiPainter
{
	public static void PaintCheck(StiCheckBox checkBox, Graphics g, RectangleD rect, float zoom)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		bool flag = true;
		bool flag2 = false;
		if (!checkBox.IsDesigning || checkBox.Report.IsPageDesigner)
		{
			flag = checkBox.IsChecked();
			flag2 = checkBox.IsUnchecked();
		}
		if (flag || flag2)
		{
			StiCheckStyle checkStyle = (flag ? checkBox.CheckStyleForTrue : checkBox.CheckStyleForFalse);
			StiComponentPainter.DrawCheckStyle(g, rect, checkStyle, checkBox, zoom);
		}
	}

	public override Image GetImage(StiComponent component, ref float zoom, StiExportFormat format)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		if (component.Report == null)
		{
			return null;
		}
		StiCheckBox stiCheckBox = component as StiCheckBox;
		double zoom2 = stiCheckBox.Report.Info.Zoom;
		stiCheckBox.Report.Info.Zoom = zoom;
		RectangleD paintRectangle = stiCheckBox.GetPaintRectangle();
		if (((RectangleD)(ref paintRectangle)).Width < 40.0)
		{
			zoom *= 2f;
			stiCheckBox.Report.Info.Zoom = zoom;
			paintRectangle = stiCheckBox.GetPaintRectangle();
		}
		((RectangleD)(ref paintRectangle)).X = 0.0;
		((RectangleD)(ref paintRectangle)).Y = 0.0;
		stiCheckBox.Report.Info.Zoom = zoom2;
		int num = (int)((RectangleD)(ref paintRectangle)).Width + 1;
		int num2 = (int)((RectangleD)(ref paintRectangle)).Height + 1;
		Bitmap val = new Bitmap(num, num2);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			val2.PageUnit = (GraphicsUnit)2;
			if (format != StiExportFormat.ImagePng)
			{
				val2.FillRectangle(Brushes.White, new Rectangle(0, 0, num, num2));
			}
			StiDrawing.FillRectangle(val2, stiCheckBox.Brush, paintRectangle);
			PaintCheck(stiCheckBox, val2, paintRectangle, zoom);
			return (Image)(object)val;
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		StiCheckBox stiCheckBox = component as StiCheckBox;
		if (!e.DrawBorderFormatting && e.DrawTopmostBorderSides && !stiCheckBox.Border.Topmost)
		{
			return;
		}
		stiCheckBox.InvokePainting(stiCheckBox, e);
		if (!e.Cancel && (stiCheckBox.Enabled || stiCheckBox.IsDesigning))
		{
			Graphics graphics = e.Graphics;
			RectangleD paintRectangle = stiCheckBox.GetPaintRectangle();
			double zoom = stiCheckBox.Page.Zoom;
			if (((RectangleD)(ref paintRectangle)).Width > 0.0 && ((RectangleD)(ref paintRectangle)).Height > 0.0)
			{
				RectangleD clipRectangle = e.ClipRectangle;
				if (((RectangleD)(ref clipRectangle)).IsEmpty || ((RectangleD)(ref paintRectangle)).IntersectsWith(e.ClipRectangle))
				{
					if (e.DrawBorderFormatting)
					{
						if (stiCheckBox.Brush is StiSolidBrush && ((StiSolidBrush)stiCheckBox.Brush).Color == Color.Transparent && stiCheckBox.Report.Info.FillComponent && stiCheckBox.IsDesigning)
						{
							Color color = Color.FromArgb(150, Color.White);
							StiDrawing.FillRectangle(graphics, color, ((RectangleD)(ref paintRectangle)).Left, ((RectangleD)(ref paintRectangle)).Top, ((RectangleD)(ref paintRectangle)).Width, ((RectangleD)(ref paintRectangle)).Height);
						}
						else
						{
							StiDrawing.FillRectangle(graphics, stiCheckBox.Brush, paintRectangle);
						}
					}
					if (e.DrawBorderFormatting)
					{
						if (stiCheckBox.IsPrinting)
						{
							float zoom2 = (float)zoom;
							Image image = stiCheckBox.GetImage(ref zoom2, StiExportFormat.ImagePng);
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
							PaintCheck(stiCheckBox, graphics, paintRectangle, (float)zoom);
						}
					}
					if (e.DrawBorderFormatting)
					{
						PaintMarkers(stiCheckBox, graphics, paintRectangle);
					}
					if (stiCheckBox.HighlightState == StiHighlightState.Hide)
					{
						PaintBorder(stiCheckBox, graphics, paintRectangle, zoom, e.DrawBorderFormatting, e.DrawTopmostBorderSides || !stiCheckBox.Border.Topmost);
					}
					if (e.DrawBorderFormatting)
					{
						PaintEvents(stiCheckBox, e.Graphics, paintRectangle);
						PaintConditions(stiCheckBox, e.Graphics, paintRectangle);
					}
				}
			}
		}
		e.Cancel = false;
		stiCheckBox.InvokePainted(stiCheckBox, e);
	}
}
