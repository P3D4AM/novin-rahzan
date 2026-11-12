using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro;

[TypeConverter(typeof(BorderColorsConverter))]
public struct BorderColors : IEquatable<BorderColors>
{
	private Color _Left;

	private Color _Top;

	private Color _Right;

	private Color _Bottom;

	private const string StringValueSeparator = ",";

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
			return _Left;
		}
		set
		{
			_Left = value;
		}
	}

	public Color Top
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

	public Color Right
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

	public Color Bottom
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

	public BorderColors(Color uniformBorderColors)
	{
		_Left = (_Top = (_Right = (_Bottom = uniformBorderColors)));
	}

	public BorderColors(Color left, Color top, Color right, Color bottom)
	{
		_Left = left;
		_Top = top;
		_Right = right;
		_Bottom = bottom;
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

	public static bool operator ==(BorderColors t1, BorderColors t2)
	{
		if (t1._Left == t2._Left && t1._Top == t2._Top && t1._Right == t2._Right)
		{
			return t1._Bottom == t2._Bottom;
		}
		return false;
	}

	public static bool operator !=(BorderColors t1, BorderColors t2)
	{
		return !(t1 == t2);
	}
}
