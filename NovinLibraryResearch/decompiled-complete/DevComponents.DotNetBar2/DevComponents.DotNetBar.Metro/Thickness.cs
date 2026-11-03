using System;
using System.ComponentModel;
using System.Globalization;
using DevComponents.DotNetBar.Metro.Helpers;

namespace DevComponents.DotNetBar.Metro;

[TypeConverter(typeof(ThicknessConverter))]
public struct Thickness : IEquatable<Thickness>
{
	private const string ኬ = ",";

	private double _খ;

	private double _ঘ;

	private double _গ;

	private double _ঙ;

	[Browsable(false)]
	public bool IsZero
	{
		get
		{
			if (_12B7.ኺ(Left) && _12B7.ኺ(Top) && _12B7.ኺ(Right))
			{
				return _12B7.ኺ(Bottom);
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool IsUniform
	{
		get
		{
			if (_12B7.ኴ(Left, Top) && _12B7.ኴ(Left, Right))
			{
				return _12B7.ኴ(Left, Bottom);
			}
			return false;
		}
	}

	public double Left
	{
		get
		{
			return _খ;
		}
		set
		{
			_খ = value;
		}
	}

	public double Top
	{
		get
		{
			return _ঘ;
		}
		set
		{
			_ঘ = value;
		}
	}

	public double Right
	{
		get
		{
			return _গ;
		}
		set
		{
			_গ = value;
		}
	}

	public double Bottom
	{
		get
		{
			return _ঙ;
		}
		set
		{
			_ঙ = value;
		}
	}

	[Browsable(false)]
	public double Horizontal => _খ + _গ;

	[Browsable(false)]
	public double Vertical => _ঘ + _ঙ;

	public Thickness(double uniformThickness)
	{
		_খ = (_ঘ = (_গ = (_ঙ = uniformThickness)));
	}

	public Thickness(double left, double top, double right, double bottom)
	{
		_খ = left;
		_ঘ = top;
		_গ = right;
		_ঙ = bottom;
	}

	public override bool Equals(object obj)
	{
		if (obj is Thickness thickness)
		{
			return this == thickness;
		}
		return false;
	}

	public bool Equals(Thickness thickness)
	{
		return this == thickness;
	}

	public override int GetHashCode()
	{
		return _খ.GetHashCode() ^ _ঘ.GetHashCode() ^ _গ.GetHashCode() ^ _ঙ.GetHashCode();
	}

	public override string ToString()
	{
		return Convert.ToString(_খ) + "," + Convert.ToString(_ঘ) + "," + Convert.ToString(_গ) + "," + Convert.ToString(_ঙ);
	}

	internal string ToString(CultureInfo cultureInfo)
	{
		return Convert.ToString(_খ, cultureInfo) + "," + Convert.ToString(_ঘ, cultureInfo) + "," + Convert.ToString(_গ, cultureInfo) + "," + Convert.ToString(_ঙ, cultureInfo);
	}

	internal bool ክ(bool ኮ, bool ኯ, bool ኰ, bool _12B1)
	{
		if (!ኮ && (Left < 0.0 || Right < 0.0 || Top < 0.0 || Bottom < 0.0))
		{
			return false;
		}
		if (!ኯ && (_12B7.ኻ(Left) || _12B7.ኻ(Right) || _12B7.ኻ(Top) || _12B7.ኻ(Bottom)))
		{
			return false;
		}
		if (!ኰ && (double.IsPositiveInfinity(Left) || double.IsPositiveInfinity(Right) || double.IsPositiveInfinity(Top) || double.IsPositiveInfinity(Bottom)))
		{
			return false;
		}
		if (!_12B1)
		{
			if (!double.IsNegativeInfinity(Left) && !double.IsNegativeInfinity(Right))
			{
				if (!double.IsNegativeInfinity(Top))
				{
					return !double.IsNegativeInfinity(Bottom);
				}
				return false;
			}
			return false;
		}
		return true;
	}

	internal bool ኲ(Thickness ኳ)
	{
		if (_12B7.ኴ(Left, ኳ.Left) && _12B7.ኴ(Top, ኳ.Top) && _12B7.ኴ(Right, ኳ.Right))
		{
			return _12B7.ኴ(Bottom, ኳ.Bottom);
		}
		return false;
	}

	internal static bool ኴ(Thickness ኵ, Thickness _12B6)
	{
		return ኵ.ኲ(_12B6);
	}

	public static bool operator ==(Thickness t1, Thickness t2)
	{
		if ((t1._খ == t2._খ || (_12B7.ኻ(t1._খ) && _12B7.ኻ(t2._খ))) && (t1._ঘ == t2._ঘ || (_12B7.ኻ(t1._ঘ) && _12B7.ኻ(t2._ঘ))) && (t1._গ == t2._গ || (_12B7.ኻ(t1._গ) && _12B7.ኻ(t2._গ))))
		{
			if (t1._ঙ != t2._ঙ)
			{
				if (_12B7.ኻ(t1._ঙ))
				{
					return _12B7.ኻ(t2._ঙ);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	public static bool operator !=(Thickness t1, Thickness t2)
	{
		return !(t1 == t2);
	}
}
