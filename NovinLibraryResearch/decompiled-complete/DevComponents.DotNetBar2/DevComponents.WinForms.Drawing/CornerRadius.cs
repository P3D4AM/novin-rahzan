using System;
using System.ComponentModel;
using System.Globalization;

namespace DevComponents.WinForms.Drawing;

[TypeConverter(typeof(CornerRadiusConverter))]
public struct CornerRadius : IEquatable<CornerRadius>
{
	private int __1B44;

	private int __2787;

	private int __2788;

	private int __2789;

	public int TopLeft
	{
		get
		{
			return __1B44;
		}
		set
		{
			__1B44 = value;
		}
	}

	public int TopRight
	{
		get
		{
			return __2787;
		}
		set
		{
			__2787 = value;
		}
	}

	public int BottomRight
	{
		get
		{
			return __2789;
		}
		set
		{
			__2789 = value;
		}
	}

	public int BottomLeft
	{
		get
		{
			return __2788;
		}
		set
		{
			__2788 = value;
		}
	}

	internal bool ኺ
	{
		get
		{
			if (__1B44 == 0 && __2787 == 0 && __2789 == 0)
			{
				return __2788 == 0;
			}
			return false;
		}
	}

	public CornerRadius(int uniformRadius)
	{
		__1B44 = (__2787 = (__2788 = (__2789 = uniformRadius)));
	}

	public CornerRadius(int topLeft, int topRight, int bottomRight, int bottomLeft)
	{
		__1B44 = topLeft;
		__2787 = topRight;
		__2789 = bottomRight;
		__2788 = bottomLeft;
	}

	public override bool Equals(object obj)
	{
		if (obj is CornerRadius cornerRadius)
		{
			return this == cornerRadius;
		}
		return false;
	}

	public bool Equals(CornerRadius cornerRadius)
	{
		return this == cornerRadius;
	}

	public override int GetHashCode()
	{
		return __1B44.GetHashCode() ^ __2787.GetHashCode() ^ __2788.GetHashCode() ^ __2789.GetHashCode();
	}

	public override string ToString()
	{
		return CornerRadiusConverter.ToString(this, CultureInfo.InvariantCulture);
	}

	public static bool operator ==(CornerRadius cr1, CornerRadius cr2)
	{
		if (cr1.__1B44 == cr2.__1B44 && cr1.__2787 == cr2.__2787 && cr1.__2789 == cr2.__2789)
		{
			return cr1.__2788 == cr2.__2788;
		}
		return false;
	}

	public static bool operator !=(CornerRadius cr1, CornerRadius cr2)
	{
		return !(cr1 == cr2);
	}

	internal bool ክ(bool ኮ)
	{
		if (!ኮ && ((double)__1B44 < 0.0 || (double)__2787 < 0.0 || (double)__2788 < 0.0 || (double)__2789 < 0.0))
		{
			return false;
		}
		return true;
	}
}
