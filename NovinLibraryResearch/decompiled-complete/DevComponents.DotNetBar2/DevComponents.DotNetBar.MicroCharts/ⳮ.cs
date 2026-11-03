using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar.MicroCharts;

internal class ⳮ : ⳛ
{
	private AreaMicroChartStyle __2CF0;

	private static int _2CEF => Dpi.Width4;

	public AreaMicroChartStyle ߕ
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
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Expected O, but got Unknown
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Expected O, but got Unknown
		//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d8: Expected O, but got Unknown
		//IL_041e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0425: Expected O, but got Unknown
		//IL_0476: Unknown result type (might be due to invalid IL or missing references)
		//IL_047d: Expected O, but got Unknown
		Graphics ܓ = info.ܓ;
		int num = Dpi.Width(3);
		int num2 = info.ⳡ - num * 2;
		int num3 = info.Ⳡ - num * 2;
		int num4 = Math.Max(3, num3 / Math.Max(1, info.Ⳟ.Count - 1));
		int num5 = Math.Max(1, info.Ⳟ.Count * num4 / num3);
		int num6 = num;
		double num7 = info.ⳣ;
		double zeroLineValue = __2CF0.ZeroLineValue;
		if (num7 > zeroLineValue)
		{
			num7 = zeroLineValue;
		}
		double num8 = info.Ⳣ - num7;
		if (num8 == 0.0)
		{
			num8 = 1.0;
		}
		int y = Math.Min((int)((double)num2 * (1.0 - (zeroLineValue - num7) / num8)), num2 - 1) + num;
		int num9 = (int)Math.Ceiling((double)info.Ⳟ.Count / (double)num5);
		Point[] array = new Point[num9];
		_2CE7[] array2 = new _2CE7[num9];
		int num10 = 0;
		GraphicsPath val = new GraphicsPath();
		new Point(0, y);
		Point p = Point.Empty;
		Point p2 = Point.Empty;
		for (int i = 0; i < info.Ⳟ.Count; i += num5)
		{
			double num11 = info.Ⳟ[i];
			double num12 = ((i < info.Ⳟ.Count - 1) ? info.Ⳟ[i + 1] : zeroLineValue);
			Point point = new Point(num6, Math.Min((int)((double)num2 * (1.0 - (num11 - num7) / num8)), num2 - 1) + num);
			Point point2 = new Point(num6 + num4, Math.Min((int)((double)num2 * (1.0 - (num12 - num7) / num8)), num2 - 1));
			if (p.IsEmpty && num11 == info.ⳣ)
			{
				p = point;
			}
			else if (num11 == info.Ⳣ)
			{
				p2 = point;
			}
			array[num10] = point;
			if (i + num5 >= info.Ⳟ.Count)
			{
				val.AddPolygon(new Point[5]
				{
					new Point(num6, y),
					point,
					point2,
					new Point(num3, point2.Y),
					new Point(num3, y)
				});
			}
			else
			{
				val.AddPolygon(new Point[4]
				{
					new Point(num6, y),
					point,
					point2,
					new Point(point2.X, y)
				});
			}
			ref _2CE7 reference = ref array2[num10];
			reference = new _2CE7(_2CF1(point, info), new Rectangle(point.X - num4 / 2, 0, point2.X - point.X, num2), Color.Gray, num11, num10);
			num10++;
			num6 += num4;
		}
		SolidBrush val2 = new SolidBrush(__2CF0.AreaColor);
		try
		{
			ܓ.FillPath((Brush)(object)val2, val);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		val.Dispose();
		if (!p.IsEmpty && !__2CF0.LowPointColor.IsEmpty)
		{
			SolidBrush val3 = new SolidBrush(__2CF0.LowPointColor);
			try
			{
				ܓ.FillPolygon((Brush)(object)val3, GetChartPointBounds(p));
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		if (!p2.IsEmpty && !__2CF0.HighPointColor.IsEmpty)
		{
			SolidBrush val4 = new SolidBrush(__2CF0.HighPointColor);
			try
			{
				ܓ.FillPolygon((Brush)(object)val4, GetChartPointBounds(p2));
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
		}
		if (!__2CF0.FirstPointColor.IsEmpty && array.Length > 0)
		{
			SolidBrush val5 = new SolidBrush(__2CF0.FirstPointColor);
			try
			{
				ܓ.FillPolygon((Brush)(object)val5, GetChartPointBounds(array[0]));
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
		}
		if (!__2CF0.LastPointColor.IsEmpty && array.Length > 1)
		{
			SolidBrush val6 = new SolidBrush(__2CF0.LastPointColor);
			try
			{
				ܓ.FillPolygon((Brush)(object)val6, GetChartPointBounds(array[^1]));
			}
			finally
			{
				((IDisposable)val6)?.Dispose();
			}
		}
		info.ⳟ = array2;
	}

	private Rectangle _2CF1(Point Ⳳ, ⳝ _108F)
	{
		return new Rectangle(Ⳳ.X - _2CEF, Ⳳ.Y - _2CEF, _2CEF * 2, _2CEF * 2);
	}
}
