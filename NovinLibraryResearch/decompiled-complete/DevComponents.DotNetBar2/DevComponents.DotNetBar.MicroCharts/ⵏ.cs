using System;
using System.Drawing;

namespace DevComponents.DotNetBar.MicroCharts;

internal class ⵏ : ⳛ
{
	private PlotMicroChartStyle _ᎌ;

	private static int _2CEF => Dpi.Width4;

	public PlotMicroChartStyle ߕ
	{
		get
		{
			return _ᎌ;
		}
		set
		{
			_ᎌ = value;
		}
	}

	public override void CreateChart(ⳝ info)
	{
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Expected O, but got Unknown
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Expected O, but got Unknown
		//IL_0468: Unknown result type (might be due to invalid IL or missing references)
		//IL_046f: Expected O, but got Unknown
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Expected O, but got Unknown
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Expected O, but got Unknown
		//IL_05ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b2: Expected O, but got Unknown
		//IL_05fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0601: Expected O, but got Unknown
		//IL_0647: Unknown result type (might be due to invalid IL or missing references)
		//IL_064e: Expected O, but got Unknown
		//IL_069f: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a6: Expected O, but got Unknown
		Graphics ܓ = info.ܓ;
		int num = Dpi.Width(3);
		int num2 = info.ⳡ - num * 2;
		int num3 = info.Ⳡ - num * 2;
		int num4 = Math.Max(4, num3 / Math.Max(1, info.Ⳟ.Count - 1));
		int num5 = Math.Max(1, info.Ⳟ.Count * num4 / num3);
		int num6 = num;
		double num7 = info.ⳣ;
		double num8 = info.Ⳣ;
		if (_ᎌ.DrawZeroLine && num7 > _ᎌ.ZeroLineValue)
		{
			num7 = _ᎌ.ZeroLineValue;
		}
		if (_ᎌ.DrawControlLine1)
		{
			if (num7 > _ᎌ.ControlLine1StartValue)
			{
				num7 = _ᎌ.ControlLine1StartValue;
			}
			if (num7 > _ᎌ.ControlLine1EndValue)
			{
				num7 = _ᎌ.ControlLine1EndValue;
			}
			if (num8 < _ᎌ.ControlLine1StartValue)
			{
				num8 = _ᎌ.ControlLine1StartValue;
			}
			if (num8 < _ᎌ.ControlLine1EndValue)
			{
				num8 = _ᎌ.ControlLine1EndValue;
			}
		}
		if (_ᎌ.DrawControlLine2)
		{
			if (num7 > _ᎌ.ControlLine2StartValue)
			{
				num7 = _ᎌ.ControlLine2StartValue;
			}
			if (num7 > _ᎌ.ControlLine2EndValue)
			{
				num7 = _ᎌ.ControlLine2EndValue;
			}
			if (num8 < _ᎌ.ControlLine2StartValue)
			{
				num8 = _ᎌ.ControlLine2StartValue;
			}
			if (num8 < _ᎌ.ControlLine2EndValue)
			{
				num8 = _ᎌ.ControlLine2EndValue;
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
		int num11 = 0;
		if (_ᎌ.DrawAverageLine && !_ᎌ.AverageLineColor.IsEmpty)
		{
			Pen val = new Pen(_ᎌ.AverageLineColor, (float)Dpi.Width1);
			try
			{
				ܓ.DrawLine(val, 0, num2 / 2, num3, num2 / 2);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		if (_ᎌ.DrawZeroLine && !_ᎌ.ZeroLineColor.IsEmpty)
		{
			Pen val2 = new Pen(_ᎌ.ZeroLineColor, (float)Dpi.Width1);
			try
			{
				int num12 = Math.Min((int)((double)num2 * (1.0 - (_ᎌ.ZeroLineValue - num7) / num9)) + num, num2 - 1);
				if (num12 < 0)
				{
					num12 = 0;
				}
				ܓ.DrawLine(val2, 0, num12, num3, num12);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		if (_ᎌ.DrawControlLine1 && !_ᎌ.ControlLine1Color.IsEmpty)
		{
			Pen val3 = new Pen(_ᎌ.ControlLine1Color, (float)Dpi.Width1);
			try
			{
				int num13 = Math.Min((int)((double)num2 * (1.0 - (_ᎌ.ControlLine1StartValue - num7) / num9)) + num, num2 + num - 1);
				if (num13 < 0)
				{
					num13 = 0;
				}
				int num14 = Math.Min((int)((double)num2 * (1.0 - (_ᎌ.ControlLine1EndValue - num7) / num9)) + num, num2 + num - 1);
				if (num14 < 0)
				{
					num14 = 0;
				}
				ܓ.DrawLine(val3, 0, num13, num3, num14);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		if (_ᎌ.DrawControlLine2 && !_ᎌ.ControlLine2Color.IsEmpty)
		{
			Pen val4 = new Pen(_ᎌ.ControlLine2Color, (float)Dpi.Width1);
			try
			{
				int num15 = Math.Min((int)((double)num2 * (1.0 - (_ᎌ.ControlLine2StartValue - num7) / num9)) + num, num2 + num - 1);
				if (num15 < 0)
				{
					num15 = 0;
				}
				int num16 = Math.Min((int)((double)num2 * (1.0 - (_ᎌ.ControlLine2EndValue - num7) / num9)) + num, num2 + num - 1);
				if (num16 < 0)
				{
					num16 = 0;
				}
				ܓ.DrawLine(val4, 0, num15, num3, num16);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
		}
		Point p = Point.Empty;
		Point p2 = Point.Empty;
		Brush val5 = (Brush)new SolidBrush(_ᎌ.PlotColor);
		try
		{
			for (int i = 0; i < info.Ⳟ.Count; i += num5)
			{
				double num17 = info.Ⳟ[i];
				Point point = new Point(num6, Math.Min((int)((double)num2 * (1.0 - (num17 - num7) / num9)), num2 - 1) + num);
				if (p.IsEmpty && num17 == info.ⳣ)
				{
					p = point;
				}
				else if (num17 == info.Ⳣ)
				{
					p2 = point;
				}
				ܓ.FillEllipse(val5, new Rectangle(point.X - Dpi.Width1, point.Y - Dpi.Height1, Dpi.Width3, Dpi.Height3));
				array[num11] = point;
				ref _2CE7 reference = ref array2[num11];
				reference = new _2CE7(_2CF1(point, info), new Rectangle(num6, 0, num4, num2), _ᎌ.PlotColor, num17, num11);
				num11++;
				num6 += num4;
			}
		}
		finally
		{
			((IDisposable)val5)?.Dispose();
		}
		if (!p.IsEmpty && !_ᎌ.LowPointColor.IsEmpty)
		{
			SolidBrush val6 = new SolidBrush(_ᎌ.LowPointColor);
			try
			{
				ܓ.FillPolygon((Brush)(object)val6, GetChartPointBounds(p));
			}
			finally
			{
				((IDisposable)val6)?.Dispose();
			}
		}
		if (!p2.IsEmpty && !_ᎌ.HighPointColor.IsEmpty)
		{
			SolidBrush val7 = new SolidBrush(_ᎌ.HighPointColor);
			try
			{
				ܓ.FillPolygon((Brush)(object)val7, GetChartPointBounds(p2));
			}
			finally
			{
				((IDisposable)val7)?.Dispose();
			}
		}
		if (!_ᎌ.FirstPointColor.IsEmpty && array.Length > 0)
		{
			SolidBrush val8 = new SolidBrush(_ᎌ.FirstPointColor);
			try
			{
				ܓ.FillPolygon((Brush)(object)val8, GetChartPointBounds(array[0]));
			}
			finally
			{
				((IDisposable)val8)?.Dispose();
			}
		}
		if (!_ᎌ.LastPointColor.IsEmpty && array.Length > 1)
		{
			SolidBrush val9 = new SolidBrush(_ᎌ.LastPointColor);
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
