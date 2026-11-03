using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Images;

namespace Stimulsoft.Report.Painters;

public class StiTableCellCheckBoxGdiPainter : StiComponentGdiPainter
{
	public static void PaintCheck(StiTableCellCheckBox checkBox, Graphics g, RectangleD rect, float zoom)
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
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		StiTableCellCheckBox stiTableCellCheckBox = (StiTableCellCheckBox)component;
		double zoom2 = stiTableCellCheckBox.Report.Info.Zoom;
		stiTableCellCheckBox.Report.Info.Zoom = zoom;
		RectangleD paintRectangle = stiTableCellCheckBox.GetPaintRectangle();
		if (((RectangleD)(ref paintRectangle)).Width < 40.0)
		{
			zoom *= 2f;
			stiTableCellCheckBox.Report.Info.Zoom = zoom;
			paintRectangle = stiTableCellCheckBox.GetPaintRectangle();
		}
		((RectangleD)(ref paintRectangle)).X = 0.0;
		((RectangleD)(ref paintRectangle)).Y = 0.0;
		stiTableCellCheckBox.Report.Info.Zoom = zoom2;
		int num = (int)((RectangleD)(ref paintRectangle)).Width + 2;
		int num2 = (int)((RectangleD)(ref paintRectangle)).Height + 2;
		Bitmap val = new Bitmap(num, num2);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			val2.PageUnit = (GraphicsUnit)2;
			if (format != StiExportFormat.ImagePng)
			{
				val2.FillRectangle(Brushes.White, new Rectangle(0, 0, num, num2));
			}
			StiDrawing.FillRectangle(val2, stiTableCellCheckBox.Brush, paintRectangle);
			PaintCheck(stiTableCellCheckBox, val2, paintRectangle, zoom);
			return (Image)(object)val;
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	private void DrawCellImage(Graphics g, RectangleD rect)
	{
		if (!(((RectangleD)(ref rect)).Width <= (double)StiScale.I9) && !(((RectangleD)(ref rect)).Height <= (double)StiScale.I9))
		{
			Image val = StiReportImages.Styles.CheckBox((StiImageSize)0);
			g.DrawImageUnscaled(val, new Rectangle((int)((RectangleD)(ref rect)).X, (int)((RectangleD)(ref rect)).Y, val.Width, val.Height));
		}
	}

	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		StiTableCellCheckBox stiTableCellCheckBox = (StiTableCellCheckBox)component;
		if (!e.DrawBorderFormatting && e.DrawTopmostBorderSides && !stiTableCellCheckBox.Border.Topmost)
		{
			return;
		}
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		if (e.DrawBorderFormatting)
		{
			stiTableCellCheckBox.InvokePainting(stiTableCellCheckBox, e);
			if (!stiTableCellCheckBox.Enabled)
			{
				e.Cancel = false;
				stiTableCellCheckBox.InvokePainted(stiTableCellCheckBox, e);
				return;
			}
		}
		if (!e.Cancel && (stiTableCellCheckBox.Enabled || stiTableCellCheckBox.IsDesigning))
		{
			Graphics graphics = e.Graphics;
			RectangleD paintRectangle = stiTableCellCheckBox.GetPaintRectangle();
			double zoom = stiTableCellCheckBox.Page.Zoom;
			if (((RectangleD)(ref paintRectangle)).Width > 0.0 && ((RectangleD)(ref paintRectangle)).Height > 0.0)
			{
				RectangleD clipRectangle = e.ClipRectangle;
				if (((RectangleD)(ref clipRectangle)).IsEmpty || ((RectangleD)(ref paintRectangle)).IntersectsWith(e.ClipRectangle))
				{
					if (e.DrawBorderFormatting)
					{
						if (stiTableCellCheckBox.Brush is StiSolidBrush && ((StiSolidBrush)stiTableCellCheckBox.Brush).Color == Color.Transparent && stiTableCellCheckBox.Report.Info.FillComponent && stiTableCellCheckBox.IsDesigning)
						{
							Color color = Color.FromArgb(150, Color.White);
							StiDrawing.FillRectangle(graphics, color, ((RectangleD)(ref paintRectangle)).Left, ((RectangleD)(ref paintRectangle)).Top, ((RectangleD)(ref paintRectangle)).Width, ((RectangleD)(ref paintRectangle)).Height);
						}
						else
						{
							StiDrawing.FillRectangle(graphics, stiTableCellCheckBox.Brush, paintRectangle);
						}
						if (component.IsSelected)
						{
							Color color2 = Color.FromArgb(80, Color.FromArgb(168, 205, 241));
							StiDrawing.FillRectangle(graphics, color2, paintRectangle);
						}
					}
					if (e.DrawBorderFormatting)
					{
						if (stiTableCellCheckBox.IsPrinting)
						{
							float zoom2 = (float)zoom;
							Image image = stiTableCellCheckBox.GetImage(ref zoom2, StiExportFormat.ImagePng);
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
							PaintCheck(stiTableCellCheckBox, graphics, paintRectangle, (float)zoom);
						}
					}
					if (e.DrawBorderFormatting)
					{
						PaintMarkers(stiTableCellCheckBox, graphics, paintRectangle);
					}
					if (e.DrawBorderFormatting && stiTableCellCheckBox.IsDesigning)
					{
						DrawCellImage(graphics, paintRectangle);
					}
					if (stiTableCellCheckBox.HighlightState == StiHighlightState.Hide)
					{
						PaintBorder(stiTableCellCheckBox, graphics, paintRectangle, zoom, e.DrawBorderFormatting, e.DrawTopmostBorderSides || !stiTableCellCheckBox.Border.Topmost);
					}
					if (e.DrawBorderFormatting)
					{
						PaintEvents(stiTableCellCheckBox, e.Graphics, paintRectangle);
						PaintQuickButtons(stiTableCellCheckBox, e.Graphics);
						PaintConditions(stiTableCellCheckBox, e.Graphics, paintRectangle);
					}
				}
			}
		}
		e.Cancel = false;
		if (e.DrawBorderFormatting)
		{
			stiTableCellCheckBox.InvokePainted(stiTableCellCheckBox, e);
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
