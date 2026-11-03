using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace DevComponents.DotNetBar.Metro.Helpers;

internal static class _12B7
{
	[StructLayout(LayoutKind.Explicit)]
	private struct ኼ
	{
		[FieldOffset(0)]
		internal double ኽ;

		[FieldOffset(0)]
		internal ulong ኾ;
	}

	public static bool ኴ(double ኸ, double ኹ)
	{
		if (ኸ == ኹ)
		{
			return true;
		}
		double num = (Math.Abs(ኸ) + Math.Abs(ኹ) + 10.0) * 2.220446049250313E-16;
		double num2 = ኸ - ኹ;
		if (0.0 - num < num2)
		{
			return num > num2;
		}
		return false;
	}

	public static bool ኺ(double ԝ)
	{
		return Math.Abs(ԝ) < 2.220446049250313E-15;
	}

	public static bool ኻ(double ԝ)
	{
		ኼ ኼ = default(ኼ);
		ኼ.ኽ = ԝ;
		ulong num = ኼ.ኾ & 0xFFF0000000000000uL;
		ulong num2 = ኼ.ኾ & 0xFFFFFFFFFFFFFuL;
		if (num != 9218868437227405312L && num != 18442240474082181120uL)
		{
			return false;
		}
		return num2 != 0;
	}
}
internal struct _1A78
{
	public double _1A79;

	public double _1A7A;

	public double _༡;

	public _1A78(double hue, double saturation, double value)
	{
		_1A79 = hue;
		_1A7A = saturation;
		_༡ = value;
	}

	public override string ToString()
	{
		return $"Hue={_1A79}, Saturation={_1A7A}, Value={_༡}";
	}
}
internal static class _1A7B
{
	public static _1A78 _1A7C(Color _065B)
	{
		double num = (double)(int)_065B.R / 255.0;
		double num2 = (double)(int)_065B.G / 255.0;
		double num3 = (double)(int)_065B.B / 255.0;
		double num4 = _᪂(num, num2, num3);
		double num5 = _᪁(num, num2, num3);
		double num6 = num5 - num4;
		_1A78 result = default(_1A78);
		result._༡ = num5;
		if (num6 == 0.0)
		{
			result._1A79 = 0.0;
			result._1A7A = 0.0;
		}
		else
		{
			result._1A7A = num6 / num5;
			double num7 = ((num5 - num) / 6.0 + num6 / 2.0) / num6;
			double num8 = ((num5 - num2) / 6.0 + num6 / 2.0) / num6;
			double num9 = ((num5 - num3) / 6.0 + num6 / 2.0) / num6;
			if (num == num5)
			{
				result._1A79 = num9 - num8;
			}
			else if (num2 == num5)
			{
				result._1A79 = 1.0 / 3.0 + num7 - num9;
			}
			else if (num3 == num5)
			{
				result._1A79 = 2.0 / 3.0 + num8 - num7;
			}
			if (result._1A79 < 0.0)
			{
				result._1A79 += 1.0;
			}
			if (result._1A79 > 1.0)
			{
				result._1A79 -= 1.0;
			}
		}
		return result;
	}

	public static Color _1A7D(double _1A7E, double ٮ, double _1A7F)
	{
		return _1A7D(new _1A78(_1A7E, ٮ, _1A7F));
	}

	public static Color _1A7D(_1A78 _᪀)
	{
		if (_᪀._༡ < 0.0)
		{
			_᪀._༡ = 0.0;
		}
		else if (_᪀._༡ > 1.0)
		{
			_᪀._༡ = 1.0;
		}
		if (_᪀._1A7A > 1.0)
		{
			_᪀._1A7A = 1.0;
		}
		else if (_᪀._1A7A < 0.0)
		{
			_᪀._1A7A = 0.0;
		}
		if (_᪀._1A7A == 0.0)
		{
			return Color.FromArgb((int)(_᪀._༡ * 255.0), (int)(_᪀._༡ * 255.0), (int)(_᪀._༡ * 255.0));
		}
		double num = _᪀._1A79 * 6.0;
		if (num == 6.0)
		{
			num = 0.0;
		}
		int num2 = (int)num;
		double num3 = _᪀._༡ * (1.0 - _᪀._1A7A);
		double num4 = _᪀._༡ * (1.0 - _᪀._1A7A * (num - (double)num2));
		double num5 = _᪀._༡ * (1.0 - _᪀._1A7A * (1.0 - (num - (double)num2)));
		double num6;
		double num7;
		double num8;
		switch (num2)
		{
		case 0:
			num6 = _᪀._༡;
			num7 = num5;
			num8 = num3;
			break;
		case 1:
			num6 = num4;
			num7 = _᪀._༡;
			num8 = num3;
			break;
		case 2:
			num6 = num3;
			num7 = _᪀._༡;
			num8 = num5;
			break;
		case 3:
			num6 = num3;
			num7 = num4;
			num8 = _᪀._༡;
			break;
		case 4:
			num6 = num5;
			num7 = num3;
			num8 = _᪀._༡;
			break;
		default:
			num6 = _᪀._༡;
			num7 = num3;
			num8 = num4;
			break;
		}
		return Color.FromArgb((int)(num6 * 255.0), (int)(num7 * 255.0), (int)(num8 * 255.0));
	}

	private static double _᪁(double _0897, double _0898, double _0899)
	{
		double num = 0.0;
		if (_0897 > _0898)
		{
			if (_0897 > _0899)
			{
				return _0897;
			}
			return _0899;
		}
		if (_0899 > _0898)
		{
			return _0899;
		}
		return _0898;
	}

	private static double _᪂(double _0897, double _0898, double _0899)
	{
		double num = 0.0;
		if (_0897 < _0898)
		{
			if (_0897 < _0899)
			{
				return _0897;
			}
			return _0899;
		}
		if (_0899 < _0898)
		{
			return _0899;
		}
		return _0898;
	}

	public static Color _᪃(Color _0BDE)
	{
		if (_0BDE.IsEmpty)
		{
			return Color.Black;
		}
		_1A78 _᪀ = _1A7C(_0BDE);
		_᪀._༡ += ((_᪀._༡ > 0.5) ? (-0.2) : 0.2);
		return _1A7D(_᪀);
	}
}
