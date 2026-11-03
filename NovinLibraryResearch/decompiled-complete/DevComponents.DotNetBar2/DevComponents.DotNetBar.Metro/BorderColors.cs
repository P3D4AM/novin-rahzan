using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace DevComponents.DotNetBar.Metro;

[TypeConverter(typeof(BorderColorsConverter))]
public struct BorderColors : IEquatable<BorderColors>
{
	private const string ኬ = ",";

	private Color _খ;

	private Color _ঘ;

	private Color _গ;

	private Color _ঙ;

	[Browsable(false)]
	public bool IsEmpty
	{
		get
		{
			if (Left.IsEmpty && Top.IsEmpty && Right.IsEmpty)
			{
				return Bottom.IsEmpty;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool IsUniform
	{
		get
		{
			if (Left == Top && Top == Right)
			{
				return Right == Bottom;
			}
			return false;
		}
	}

	public Color Left
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

	public Color Top
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

	public Color Right
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

	public Color Bottom
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

	public BorderColors(Color uniformBorderColors)
	{
		_খ = (_ঘ = (_গ = (_ঙ = uniformBorderColors)));
	}

	public BorderColors(Color left, Color top, Color right, Color bottom)
	{
		_খ = left;
		_ঘ = top;
		_গ = right;
		_ঙ = bottom;
	}

	public override bool Equals(object obj)
	{
		if (obj is BorderColors borderColors)
		{
			return this == borderColors;
		}
		return false;
	}

	public bool Equals(BorderColors BorderColors)
	{
		return this == BorderColors;
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

	public static bool operator ==(BorderColors t1, BorderColors t2)
	{
		if (t1._খ == t2._খ && t1._ঘ == t2._ঘ && t1._গ == t2._গ)
		{
			return t1._ঙ == t2._ঙ;
		}
		return false;
	}

	public static bool operator !=(BorderColors t1, BorderColors t2)
	{
		return !(t1 == t2);
	}
}
