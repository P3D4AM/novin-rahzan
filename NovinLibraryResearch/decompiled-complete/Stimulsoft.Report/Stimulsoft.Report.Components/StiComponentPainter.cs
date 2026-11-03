using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Components;

public class StiComponentPainter
{
	public static void DrawCheckStyle(Graphics g, RectangleD rect, StiCheckStyle checkStyle, StiCheckBox checkBox, float zoom)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Expected O, but got Unknown
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Expected O, but got Unknown
		if (checkStyle == StiCheckStyle.None)
		{
			return;
		}
		SmoothingMode smoothingMode = g.SmoothingMode;
		g.SmoothingMode = (SmoothingMode)4;
		GraphicsState val = g.Save();
		g.SetClip(((RectangleD)(ref rect)).ToRectangleF(), (CombineMode)1);
		StringFormat val2 = new StringFormat();
		try
		{
			char c = ' ';
			string text = null;
			int num = 0;
			switch (checkStyle)
			{
			case StiCheckStyle.Cross:
				c = 'û';
				text = "Wingdings";
				break;
			case StiCheckStyle.Check:
				c = 'ü';
				text = "Wingdings";
				num = 1;
				break;
			case StiCheckStyle.CrossRectangle:
				c = 'ý';
				text = "Wingdings";
				num = 2;
				break;
			case StiCheckStyle.CheckRectangle:
				c = 'þ';
				text = "Wingdings";
				num = 3;
				break;
			case StiCheckStyle.CrossCircle:
				c = 'V';
				text = "Wingdings 2";
				num = 4;
				break;
			case StiCheckStyle.DotCircle:
				c = '¤';
				text = "Wingdings";
				num = 5;
				break;
			case StiCheckStyle.DotRectangle:
				c = '©';
				text = "Wingdings 2";
				num = 6;
				break;
			case StiCheckStyle.NoneCircle:
				c = '¡';
				text = "Wingdings";
				num = 7;
				break;
			case StiCheckStyle.NoneRectangle:
				c = '\u00a8';
				text = "Wingdings";
				num = 8;
				break;
			}
			bool flag = false;
			try
			{
				FontFamily val3 = new FontFamily(text);
				try
				{
					if (val3.GetName(0) != text)
					{
						flag = true;
					}
					else
					{
						GraphicsPath val4 = new GraphicsPath();
						try
						{
							val4.AddString(c.ToString(), val3, 0, Math.Min((float)((RectangleD)(ref rect)).Height, (float)((RectangleD)(ref rect)).Width), new RectangleF(0f, 0f, (float)((RectangleD)(ref rect)).Width, (float)((RectangleD)(ref rect)).Height), val2);
							RectangleF bounds = val4.GetBounds();
							float num2 = ((float)((RectangleD)(ref rect)).Width - bounds.Width) / 2f - bounds.Left;
							float num3 = ((float)((RectangleD)(ref rect)).Height - bounds.Height) / 2f - bounds.Top;
							g.TranslateTransform((float)((RectangleD)(ref rect)).X + num2, (float)((RectangleD)(ref rect)).Y + num3);
							Brush brush = StiBrush.GetBrush(checkBox.TextBrush, bounds);
							try
							{
								g.FillPath(brush, val4);
							}
							finally
							{
								((IDisposable)brush)?.Dispose();
							}
							Pen val5 = new Pen(checkBox.ContourColor);
							try
							{
								val5.Width = (float)(checkBox.Size * (double)zoom);
								g.DrawPath(val5, val4);
							}
							finally
							{
								((IDisposable)val5)?.Dispose();
							}
						}
						finally
						{
							((IDisposable)val4)?.Dispose();
						}
					}
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			catch
			{
				flag = true;
			}
			if (flag)
			{
				Bitmap image = StiImageUtils.GetImage(typeof(StiReport), "Stimulsoft.Report.Images.CheckStyles.png", false);
				try
				{
					RectangleF rectangleF = new RectangleF(num * 200, 0f, 200f, 200f);
					double num4 = Math.Min(((RectangleD)(ref rect)).Width, ((RectangleD)(ref rect)).Height);
					RectangleF rectangleF2 = new RectangleF((float)(((RectangleD)(ref rect)).X + (((RectangleD)(ref rect)).Width - num4) / 2.0), (float)(((RectangleD)(ref rect)).Y + (((RectangleD)(ref rect)).Height - num4) / 2.0), (float)num4, (float)num4);
					g.DrawImage((Image)(object)image, rectangleF2, rectangleF, (GraphicsUnit)2);
				}
				finally
				{
					((IDisposable)image)?.Dispose();
				}
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		g.Restore(val);
		g.SmoothingMode = smoothingMode;
	}
}
