using System;
using System.Drawing;

namespace DevComponents.DotNetBar.MicroCharts;

internal class ⴆ : ⳛ
{
	private LineMicroChartStyle __2CF0;

	private static int _2CEF => Dpi.Width4;

	public LineMicroChartStyle ߕ
	{
		get
		{
			return __2CF0;
		}
		set
		{
			__2CF0 = value;
		}
	}

	public override void CreateChart(ⳝ info)
	{
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Expected O, but got Unknown
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Expected O, but got Unknown
		//IL_0411: Unknown result type (might be due to invalid IL or missing references)
		//IL_0418: Expected O, but got Unknown
		//IL_057d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0584: Expected O, but got Unknown
		//IL_04d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04df: Expected O, but got Unknown
		//IL_05cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d4: Expected O, but got Unknown
		//IL_0670: Unknown result type (might be due to invalid IL or missing references)
		//IL_0677: Expected O, but got Unknown
		//IL_0623: Unknown result type (might be due to invalid IL or missing references)
		//IL_062a: Expected O, but got Unknown
		//IL_06c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cf: Expected O, but got Unknown
		Graphics ܓ = info.ܓ;
		int num = Dpi.Width(3);
		int num2 = info.ⳡ - num * 2;
		int num3 = info.Ⳡ - num * 2;
		int num4 = Math.Max(1, num3 / Math.Max(1, info.Ⳟ.Count - 1));
		int num5 = Math.Max(1, info.Ⳟ.Count * num4 / num3);
		if (info.Ⳟ.Count <= 2)
		{
			num5 = 1;
		}
		int num6 = num;
		double num7 = info.ⳣ;
		double num8 = info.Ⳣ;
		if (__2CF0.DrawZeroLine && num7 > __2CF0.ZeroLineValue)
		{
			num7 = __2CF0.ZeroLineValue;
		}
		if (__2CF0.DrawControlLine1)
		{
			if (num7 > __2CF0.ControlLine1StartValue)
			{
				num7 = __2CF0.ControlLine1StartValue;
			}
			if (num7 > __2CF0.ControlLine1EndValue)
			{
				num7 = __2CF0.ControlLine1EndValue;
			}
			if (num8 < __2CF0.ControlLine1StartValue)
			{
				num8 = __2CF0.ControlLine1StartValue;
			}
			if (num8 < __2CF0.ControlLine1EndValue)
			{
				num8 = __2CF0.ControlLine1EndValue;
			}
		}
		if (__2CF0.DrawControlLine2)
		{
			if (num7 > __2CF0.ControlLine2StartValue)
			{
				num7 = __2CF0.ControlLine2StartValue;
			}
			if (num7 > __2CF0.ControlLine2EndValue)
			{
				num7 = __2CF0.ControlLine2EndValue;
			}
			if (num8 < __2CF0.ControlLine2StartValue)
			{
				num8 = __2CF0.ControlLine2StartValue;
			}
			if (num8 < __2CF0.ControlLine2EndValue)
			{
				num8 = __2CF0.ControlLine2EndValue;
			}
		}
		double num9 = num8 - num7;
		if (num9 == 0.0)
		{
			num9 = 1.0;
		}
		int num10 = (int)Math.Ceiling((double)info.Ⳟ.Count / (double)num5);
		Point[] array = new Point[num10];
		_2CE7[] array2 = new _2CE7[num10];
		Point p = Point.Empty;
		Point p2 = Point.Empty;
		int num11 = 0;
		for (int i = 0; i < info.Ⳟ.Count; i += num5)
		{
			double num12 = info.Ⳟ[i];
			Point point = new Point(num6, Math.Min((int)((double)num2 * (1.0 - (num12 - num7) / num9)), num2 - 1) + num);
			if (p.IsEmpty && num12 == info.ⳣ)
			{
				p = point;
			}
			else if (num12 == num8)
			{
				p2 = point;
			}
			array[num11] = point;
			ref _2CE7 reference = ref array2[num11];
			reference = new _2CE7(_2CF1(point, info), new Rectangle(point.X - num4 / 2, 0, num4, num2), __2CF0.LineColor, num12, num11);
			num11++;
			num6 += num4;
			if (num6 > num3 || i >= info.Ⳟ.Count - num5 * 2)
			{
				num6 = num3;
			}
		}
		if (__2CF0.DrawAverageLine && !__2CF0.AverageLineColor.IsEmpty)
		{
			Pen val = new Pen(__2CF0.AverageLineColor, (float)Dpi.Width1);
			try
			{
				ܓ.DrawLine(val, 0, num2 / 2, num3, num2 / 2);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		if (__2CF0.DrawZeroLine && !__2CF0.ZeroLineColor.IsEmpty)
		{
			Pen val2 = new Pen(__2CF0.ZeroLineColor, (float)Dpi.Width1);
			try
			{
				int num13 = Math.Min((int)((double)num2 * (1.0 - (__2CF0.ZeroLineValue - num7) / num9)) + num, num2 + num - 1);
				if (num13 < 0)
				{
					num13 = 0;
				}
				ܓ.DrawLine(val2, 0, num13, num3, num13);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		if (__2CF0.DrawControlLine1 && !__2CF0.ControlLine1Color.IsEmpty)
		{
			Pen val3 = new Pen(__2CF0.ControlLine1Color, (float)Dpi.Width1);
			try
			{
				int num14 = Math.Min((int)((double)num2 * (1.0 - (__2CF0.ControlLine1StartValue - num7) / num9)) + num, num2 + num - 1);
				if (num14 < 0)
				{
					num14 = 0;
				}
				int num15 = Math.Min((int)((double)num2 * (1.0 - (__2CF0.ControlLine1EndValue - num7) / num9)) + num, num2 + num - 1);
				if (num15 < 0)
				{
					num15 = 0;
				}
				ܓ.DrawLine(val3, 0, num14, num3, num15);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		if (__2CF0.DrawControlLine2 && !__2CF0.ControlLine2Color.IsEmpty)
		{
			Pen val4 = new Pen(__2CF0.ControlLine2Color, (float)Dpi.Width1);
			try
			{
				int num16 = Math.Min((int)((double)num2 * (1.0 - (__2CF0.ControlLine2StartValue - num7) / num9)) + num, num2 + num - 1);
				if (num16 < 0)
				{
					num16 = 0;
				}
				int num17 = Math.Min((int)((double)num2 * (1.0 - (__2CF0.ControlLine2EndValue - num7) / num9)) + num, num2 + num - 1);
				if (num17 < 0)
				{
					num17 = 0;
				}
				ܓ.DrawLine(val4, 0, num16, num3, num17);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
		}
		if (array.Length > 1)
		{
			Pen val5 = new Pen(__2CF0.LineColor, (float)Dpi.Width1);
			try
			{
				ܓ.DrawLines(val5, array);
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
		}
		if (!p.IsEmpty && !__2CF0.LowPointColor.IsEmpty && array.Length > 0)
		{
			SolidBrush val6 = new SolidBrush(__2CF0.LowPointColor);
			try
			{
				ܓ.FillPolygon((Brush)(object)val6, GetChartPointBounds(p));
			}
			finally
			{
				((IDisposable)val6)?.Dispose();
			}
		}
		if (!p2.IsEmpty && !__2CF0.HighPointColor.IsEmpty && array.Length > 0)
		{
			SolidBrush val7 = new SolidBrush(__2CF0.HighPointColor);
			try
			{
				ܓ.FillPolygon((Brush)(object)val7, GetChartPointBounds(p2));
			}
			finally
			{
				((IDisposable)val7)?.Dispose();
			}
		}
		if (!__2CF0.FirstPointColor.IsEmpty && array.Length > 0)
		{
			SolidBrush val8 = new SolidBrush(__2CF0.FirstPointColor);
			try
			{
				ܓ.FillPolygon((Brush)(object)val8, GetChartPointBounds(array[0]));
			}
			finally
			{
				((IDisposable)val8)?.Dispose();
			}
		}
		if (!__2CF0.LastPointColor.IsEmpty && array.Length > 1)
		{
			SolidBrush val9 = new SolidBrush(__2CF0.LastPointColor);
			try
			{
				ܓ.FillPolygon((Brush)(object)val9, GetChartPointBounds(array[^1]));
			}
			finally
			{
				((IDisposable)val9)?.Dispose();
			}
		}
		info.ⳟ = array2;
	}

	private Rectangle _2CF1(Point Ⳳ, ⳝ _108F)
	{
		return new Rectangle(Ⳳ.X - _2CEF, Ⳳ.Y - _2CEF, _2CEF * 2, _2CEF * 2);
	}
}
