using System;
using System.ComponentModel;
using System.Globalization;
using Stimulsoft.Controls.Win.DotNetBar.Metro.Helpers;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro;

[TypeConverter(typeof(ThicknessConverter))]
public struct Thickness : IEquatable<Thickness>
{
	private double _Left;

	private double _Top;

	private double _Right;

	private double _Bottom;

	private const string StringValueSeparator = ",";

	[Browsable(false)]
	public bool IsZero
	{
		get
		{
			if (DoubleHelpers.IsZero(Left) && DoubleHelpers.IsZero(Top) && DoubleHelpers.IsZero(Right))
			{
				return DoubleHelpers.IsZero(Bottom);
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool IsUniform
	{
		get
		{
			if (DoubleHelpers.AreClose(Left, Top) && DoubleHelpers.AreClose(Left, Right))
			{
				return DoubleHelpers.AreClose(Left, Bottom);
			}
			return false;
		}
	}

	public double Left
	{
		get
		{
			return _Left;
		}
		set
		{
			_Left = value;
		}
	}

	public double Top
	{
		get
		{
			return _Top;
		}
		set
		{
			_Top = value;
		}
	}

	public double Right
	{
		get
		{
			return _Right;
		}
		set
		{
			_Right = value;
		}
	}

	public double Bottom
	{
		get
		{
			return _Bottom;
		}
		set
		{
			_Bottom = value;
		}
	}

	[Browsable(false)]
	public double Horizontal => _Left + _Right;

	[Browsable(false)]
	public double Vertical => _Top + _Bottom;

	public Thickness(double uniformThickness)
	{
		_Left = (_Top = (_Right = (_Bottom = uniformThickness)));
	}

	public Thickness(double left, double top, double right, double bottom)
	{
		_Left = left;
		_Top = top;
		_Right = right;
		_Bottom = bottom;
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
		return _Left.GetHashCode() ^ _Top.GetHashCode() ^ _Right.GetHashCode() ^ _Bottom.GetHashCode();
	}

	public override string ToString()
	{
		return Convert.ToString(_Left) + "," + Convert.ToString(_Top) + "," + Convert.ToString(_Right) + "," + Convert.ToString(_Bottom);
	}

	internal string ToString(CultureInfo cultureInfo)
	{
		return Convert.ToString(_Left, cultureInfo) + "," + Convert.ToString(_Top, cultureInfo) + "," + Convert.ToString(_Right, cultureInfo) + "," + Convert.ToString(_Bottom, cultureInfo);
	}

	internal bool IsValid(bool allowNegative, bool allowNaN, bool allowPositiveInfinity, bool allowNegativeInfinity)
	{
		if (!allowNegative && (Left < 0.0 || Right < 0.0 || Top < 0.0 || Bottom < 0.0))
		{
			return false;
		}
		if (!allowNaN && (DoubleHelpers.IsNaN(Left) || DoubleHelpers.IsNaN(Right) || DoubleHelpers.IsNaN(Top) || DoubleHelpers.IsNaN(Bottom)))
		{
			return false;
		}
		if (!allowPositiveInfinity && (double.IsPositiveInfinity(Left) || double.IsPositiveInfinity(Right) || double.IsPositiveInfinity(Top) || double.IsPositiveInfinity(Bottom)))
		{
			return false;
		}
		if (!allowNegativeInfinity)
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

	internal bool IsClose(Thickness thickness)
	{
		if (DoubleHelpers.AreClose(Left, thickness.Left) && DoubleHelpers.AreClose(Top, thickness.Top) && DoubleHelpers.AreClose(Right, thickness.Right))
		{
			return DoubleHelpers.AreClose(Bottom, thickness.Bottom);
		}
		return false;
	}

	internal static bool AreClose(Thickness thickness1, Thickness thickness2)
	{
		return thickness1.IsClose(thickness2);
	}

	public static bool operator ==(Thickness t1, Thickness t2)
	{
		if ((t1._Left == t2._Left || (DoubleHelpers.IsNaN(t1._Left) && DoubleHelpers.IsNaN(t2._Left))) && (t1._Top == t2._Top || (DoubleHelpers.IsNaN(t1._Top) && DoubleHelpers.IsNaN(t2._Top))) && (t1._Right == t2._Right || (DoubleHelpers.IsNaN(t1._Right) && DoubleHelpers.IsNaN(t2._Right))))
		{
			if (t1._Bottom != t2._Bottom)
			{
				if (DoubleHelpers.IsNaN(t1._Bottom))
				{
					return DoubleHelpers.IsNaN(t2._Bottom);
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
