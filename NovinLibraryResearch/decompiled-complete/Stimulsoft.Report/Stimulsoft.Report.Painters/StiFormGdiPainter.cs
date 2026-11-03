using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Painters;

public class StiFormGdiPainter : StiPageGdiPainter
{
	private Rectangle GetOffsetRect(StiForm form, Rectangle rect, int minWidth, int minHeight)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		if (form.IsSelected)
		{
			int width = rect.Width;
			RectangleD offsetRectangle = form.OffsetRectangle;
			rect.Width = Math.Max(width + (int)(((RectangleD)(ref offsetRectangle)).Width * StiScale.Factor), minWidth);
			int height = rect.Height;
			offsetRectangle = form.OffsetRectangle;
			rect.Height = Math.Max(height + (int)(((RectangleD)(ref offsetRectangle)).Height * StiScale.Factor), minHeight);
		}
		else
		{
			rect.Width = Math.Max(rect.Width, minWidth);
			rect.Height = Math.Max(rect.Height, minHeight);
		}
		return rect;
	}

	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Expected O, but got Unknown
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Expected O, but got Unknown
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Expected O, but got Unknown
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Invalid comparison between Unknown and I4
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Expected O, but got Unknown
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Expected O, but got Unknown
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Expected O, but got Unknown
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Expected O, but got Unknown
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_0437: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Expected O, but got Unknown
		//IL_0423: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Expected O, but got Unknown
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0472: Unknown result type (might be due to invalid IL or missing references)
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		StiForm stiForm = component as StiForm;
		Graphics graphics = e.Graphics;
		int num = (int)(stiForm.Margins.Left * StiScale.Factor);
		int num2 = (int)(stiForm.Margins.Top * StiScale.Factor);
		RectangleD val = stiForm.DisplayRectangle;
		int num3 = StiScale.XXI(((RectangleD)(ref val)).Width);
		val = stiForm.DisplayRectangle;
		int height = StiScale.YYI(((RectangleD)(ref val)).Height);
		val = e.ClipRectangle;
		if (!((RectangleD)(ref val)).IsEmpty)
		{
			graphics.ResetClip();
			val = e.ClipRectangle;
			graphics.SetClip(((RectangleD)(ref val)).ToRectangleF());
		}
		Rectangle offsetRect = GetOffsetRect(stiForm, new Rectangle(0, 0, num3, height), StiScale.XXI(112.0), StiScale.YYI(32.0));
		ControlPaint.DrawBorder3D(graphics, offsetRect, (Border3DStyle)5, (Border3DSide)2063);
		int num4 = StiScale.YYI(20.0);
		Rectangle rectangle = new Rectangle(StiScale.I2, StiScale.I2, num3 - StiScale.I4, num4 - StiScale.I2);
		if (stiForm.IsSelected)
		{
			int width = rectangle.Width;
			val = stiForm.OffsetRectangle;
			rectangle.Width = Math.Max(width + (int)(((RectangleD)(ref val)).Width * StiScale.Factor), StiScale.YYI(108.0));
		}
		graphics.FillRectangle((Brush)new LinearGradientBrush(rectangle, StiColors.ActiveCaptionEnd, StiColors.ActiveCaptionStart, 0f), rectangle);
		rectangle.X += StiScale.I2;
		Font val2 = new Font("Arial", 8f, (FontStyle)1);
		try
		{
			SolidBrush val3 = new SolidBrush(SystemColors.ActiveCaptionText);
			try
			{
				StringFormat val4 = new StringFormat();
				try
				{
					val4.LineAlignment = (StringAlignment)1;
					val4.FormatFlags = (StringFormatFlags)4096;
					val4.Trimming = (StringTrimming)3;
					if ((int)stiForm.RightToLeft == 1)
					{
						val4.FormatFlags = (StringFormatFlags)(val4.FormatFlags | 1);
					}
					graphics.DrawString(stiForm.Text, val2, (Brush)(object)val3, (RectangleF)rectangle, val4);
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
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		val = stiForm.ClientRectangle;
		Rectangle offsetRect2 = GetOffsetRect(stiForm, StiScale.I(((RectangleD)(ref val)).ToRectangle()), StiScale.I(104), StiScale.I8);
		offsetRect2.Width++;
		offsetRect2.Height++;
		offsetRect2.Y += StiScale.YYI(20.0);
		offsetRect2.X += StiScale.I4;
		graphics.SetClip(offsetRect2);
		if (stiForm.Report.Info.ShowGrid)
		{
			int num5 = StiScale.I(stiForm.GridSize);
			Bitmap val5 = new Bitmap(num5, num5);
			Graphics val6 = Graphics.FromImage((Image)(object)val5);
			try
			{
				Brush val7 = (Brush)new SolidBrush(stiForm.BackColor);
				try
				{
					val6.FillRectangle(val7, 0, 0, num5, num5);
				}
				finally
				{
					((IDisposable)val7)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val6)?.Dispose();
			}
			val5.SetPixel(0, 0, SystemColors.ControlDarkDark);
			Rectangle rectangle2 = offsetRect2;
			graphics.TranslateTransform((float)rectangle2.X, (float)rectangle2.Y);
			rectangle2.X = 0;
			rectangle2.Y = 0;
			TextureBrush val8 = new TextureBrush((Image)(object)val5);
			graphics.FillRectangle((Brush)(object)val8, rectangle2);
			graphics.ResetTransform();
		}
		graphics.TranslateTransform((float)num, (float)num2);
		PaintTableLines(stiForm, graphics);
		Bitmap val9 = new Bitmap(offsetRect2.Width, offsetRect2.Height);
		Graphics val10 = Graphics.FromImage((Image)(object)val9);
		try
		{
			PaintComponents(stiForm, new StiPaintEventArgs(val10, e.ClipRectangle));
			val10.ResetClip();
			if (stiForm.Report.Info.ShowOrder && stiForm.IsDesigning)
			{
				PaintOrderAndQuickInfo(stiForm, val10, "");
			}
		}
		finally
		{
			((IDisposable)val10)?.Dispose();
		}
		graphics.DrawImage((Image)(object)val9, 0, 0, offsetRect2.Width, offsetRect2.Height);
		graphics.ResetClip();
		if (stiForm.Report.Info.ShowDimensionLines && stiForm.IsDesigning && (int)stiForm.Report.Info.CurrentAction != 0)
		{
			StiDimensionLinesHelper.DrawDimensionLines(graphics, stiForm);
		}
		val = stiForm.SelectedRectangle;
		if (!((RectangleD)(ref val)).IsEmpty)
		{
			SolidBrush val11 = new SolidBrush(Color.FromArgb(75, Color.Blue));
			try
			{
				Pen val12 = new Pen(Color.DimGray);
				try
				{
					val12.DashStyle = (DashStyle)1;
					val = stiForm.SelectedRectangle;
					RectangleF rectangleF = ((RectangleD)(ref val)).ToRectangleF();
					graphics.FillRectangle((Brush)(object)val11, rectangleF);
					graphics.DrawRectangle(val12, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
				}
				finally
				{
					((IDisposable)val12)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val11)?.Dispose();
			}
		}
		PaintSelection((StiComponent)stiForm, e);
	}
}
