using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Base.Drawing;

public sealed class StiCapStyleHelper
{
	public static int GetMaxWidthOfCapStyles(Graphics g, Font font)
	{
		Array values = Enum.GetValues(typeof(StiCapStyle));
		int num = 0;
		for (int i = 0; i < values.Length; i++)
		{
			object value = values.GetValue(i);
			string text = StiLocalization.Get("PropertyEnum", typeof(StiCapStyle).Name + Enum.GetName(typeof(StiCapStyle), value), throwError: false);
			int val = (int)g.MeasureString(text, font).Width;
			num = Math.Max(num, val);
		}
		return num + 60;
	}

	public static void DrawCapStyle(DrawItemEventArgs e)
	{
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Expected O, but got Unknown
		//IL_0501: Unknown result type (might be due to invalid IL or missing references)
		//IL_0508: Expected O, but got Unknown
		//IL_0672: Unknown result type (might be due to invalid IL or missing references)
		//IL_0679: Expected O, but got Unknown
		//IL_0810: Unknown result type (might be due to invalid IL or missing references)
		//IL_081c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0823: Expected O, but got Unknown
		//IL_075f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0766: Expected O, but got Unknown
		//IL_083c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0867: Unknown result type (might be due to invalid IL or missing references)
		//IL_086e: Expected O, but got Unknown
		//IL_086e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0875: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		Rectangle bounds = e.Bounds;
		Rectangle rectangle = new Rectangle(bounds.X + (int)Math.Ceiling(2.0 * StiScale.X), bounds.Y + (int)Math.Ceiling(2.0 * StiScale.Y), (int)Math.Ceiling(52.0 * StiScale.X), (int)Math.Ceiling(14.0 * StiScale.Y));
		bounds.Width--;
		StiControlPaint.DrawItemBackground(graphics, bounds, e.State);
		Array values = Enum.GetValues(typeof(StiCapStyle));
		Pen val = new Pen(Color.DimGray, 1f);
		try
		{
			StiCapStyle stiCapStyle = StiCapStyle.None;
			if (e.Index != -1)
			{
				stiCapStyle = (StiCapStyle)values.GetValue(e.Index);
			}
			graphics.FillRectangle(Brushes.White, rectangle);
			int num = rectangle.Top + rectangle.Height / 2;
			int num2 = rectangle.Width / 4;
			SmoothingMode smoothingMode = graphics.SmoothingMode;
			graphics.SmoothingMode = (SmoothingMode)4;
			switch (stiCapStyle)
			{
			case StiCapStyle.None:
				graphics.DrawLine(val, rectangle.X + num2, num, rectangle.Right - num2, num);
				break;
			case StiCapStyle.Arrow:
			{
				Rectangle rectangle7 = new Rectangle(rectangle.Right - num2 - (int)Math.Ceiling(4.0 * StiScale.X), rectangle.Y + (int)Math.Ceiling(2.0 * StiScale.Y), (int)Math.Ceiling(5.0 * StiScale.X), (int)Math.Ceiling(11.0 * StiScale.Y));
				graphics.SmoothingMode = smoothingMode;
				PointF[] array = new PointF[3]
				{
					new PointF(rectangle7.Right, (float)rectangle7.Y + (float)(rectangle7.Height / 2)),
					new PointF(rectangle7.Left, rectangle7.Y),
					new PointF(rectangle7.Left, rectangle7.Bottom)
				};
				SolidBrush val7 = new SolidBrush(Color.DimGray);
				try
				{
					graphics.DrawLine(val, (float)(rectangle.X + num2), array[0].Y, (float)(rectangle.Right - num2), array[0].Y);
					graphics.FillPolygon((Brush)(object)val7, array);
				}
				finally
				{
					((IDisposable)val7)?.Dispose();
				}
				break;
			}
			case StiCapStyle.Open:
			{
				Rectangle rectangle6 = new Rectangle(rectangle.Right - num2 - (int)Math.Ceiling(4.0 * StiScale.X), rectangle.Y + (int)Math.Ceiling(4.0 * StiScale.Y), (int)Math.Ceiling(5.0 * StiScale.X), (int)Math.Ceiling(7.0 * StiScale.Y));
				PointF[] array = new PointF[3]
				{
					new PointF(rectangle6.X, rectangle6.Y),
					new PointF(rectangle6.Right, (float)rectangle6.Y + (float)(rectangle6.Height / 2)),
					new PointF(rectangle6.X, rectangle6.Bottom)
				};
				Pen val6 = new Pen(Color.DimGray);
				try
				{
					graphics.DrawLine(val, (float)(rectangle.X + num2), array[1].Y, (float)(rectangle.Right - num2), array[1].Y);
					graphics.DrawLines(val6, array);
				}
				finally
				{
					((IDisposable)val6)?.Dispose();
				}
				break;
			}
			case StiCapStyle.Stealth:
			{
				Rectangle rectangle5 = new Rectangle(rectangle.Right - num2 - (int)Math.Ceiling(9.0 * StiScale.X), rectangle.Y + (int)Math.Ceiling(2.0 * StiScale.Y), (int)Math.Ceiling(10.0 * StiScale.X), (int)Math.Ceiling(11.0 * StiScale.Y));
				PointF[] array = new PointF[5]
				{
					new PointF(rectangle5.X, rectangle5.Y),
					new PointF(rectangle5.Right, (float)rectangle5.Y + (float)(rectangle5.Height / 2)),
					new PointF(rectangle5.X, rectangle5.Bottom),
					new PointF((float)rectangle5.X + (float)(rectangle5.Width / 3), (float)rectangle5.Y + (float)(rectangle5.Height / 2)),
					new PointF(rectangle5.X, rectangle5.Y)
				};
				SolidBrush val5 = new SolidBrush(Color.DimGray);
				try
				{
					graphics.DrawLine(val, (float)(rectangle.X + num2), array[1].Y, (float)(rectangle.Right - num2), array[1].Y);
					graphics.FillPolygon((Brush)(object)val5, array);
				}
				finally
				{
					((IDisposable)val5)?.Dispose();
				}
				break;
			}
			case StiCapStyle.Diamond:
			{
				Rectangle rectangle4 = new Rectangle(rectangle.Right - num2 - (int)Math.Ceiling(6.0 * StiScale.X), rectangle.Y + (int)Math.Ceiling(3.0 * StiScale.Y), (int)Math.Ceiling(8.0 * StiScale.X), (int)Math.Ceiling(9.0 * StiScale.Y));
				PointF[] array = new PointF[4]
				{
					new PointF(rectangle4.X, (float)rectangle4.Y + (float)(rectangle4.Height / 2)),
					new PointF((float)rectangle4.X + (float)(rectangle4.Width / 2), rectangle4.Y),
					new PointF(rectangle4.Right, (float)rectangle4.Y + (float)(rectangle4.Height / 2)),
					new PointF((float)rectangle4.X + (float)(rectangle4.Width / 2), rectangle4.Bottom)
				};
				Brush val4 = (Brush)new SolidBrush(Color.DimGray);
				try
				{
					graphics.DrawLine(val, (float)(rectangle.X + num2), array[0].Y, (float)(rectangle.Right - num2), array[0].Y);
					graphics.FillPolygon(val4, array);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
				break;
			}
			case StiCapStyle.Oval:
			{
				graphics.DrawLine(val, rectangle.X + num2, num, rectangle.Right - num2, num);
				Rectangle rectangle3 = new Rectangle(rectangle.Right - num2 - (int)Math.Ceiling(4.0 * StiScale.X), rectangle.Y + (int)Math.Ceiling(4.0 * StiScale.Y), (int)Math.Ceiling(6.0 * StiScale.X), (int)Math.Ceiling(6.0 * StiScale.Y));
				SolidBrush val3 = new SolidBrush(Color.DimGray);
				try
				{
					graphics.FillEllipse((Brush)(object)val3, rectangle3);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
				break;
			}
			case StiCapStyle.Square:
			{
				graphics.DrawLine(val, rectangle.X + num2, num, rectangle.Right - num2, num);
				Rectangle rectangle2 = new Rectangle(rectangle.Right - num2 - (int)Math.Ceiling(4.0 * StiScale.X), rectangle.Y + (int)Math.Ceiling(4.0 * StiScale.Y), (int)Math.Ceiling(6.0 * StiScale.X), (int)Math.Ceiling(6.0 * StiScale.Y));
				graphics.SmoothingMode = smoothingMode;
				Brush val2 = (Brush)new SolidBrush(Color.DimGray);
				try
				{
					graphics.FillRectangle(val2, rectangle2);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
				break;
			}
			}
			graphics.SmoothingMode = smoothingMode;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		graphics.DrawRectangle(Pens.Black, rectangle);
		Font val8 = new Font("Arial", 8f);
		try
		{
			StringFormat val9 = new StringFormat();
			try
			{
				val9.LineAlignment = (StringAlignment)1;
				object value = values.GetValue(e.Index);
				string text = StiLocalization.Get("PropertyEnum", typeof(StiCapStyle).Name + Enum.GetName(typeof(StiCapStyle), value), throwError: false);
				e.Graphics.DrawString(text, val8, Brushes.Black, (RectangleF)new Rectangle((int)Math.Ceiling(55.0 * StiScale.X), bounds.Top + (int)Math.Ceiling(4.0 * StiScale.Y), bounds.Width - (int)Math.Ceiling(18.0 * StiScale.X), (int)Math.Ceiling(10.0 * StiScale.Y)), val9);
			}
			finally
			{
				((IDisposable)val9)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val8)?.Dispose();
		}
	}
}
