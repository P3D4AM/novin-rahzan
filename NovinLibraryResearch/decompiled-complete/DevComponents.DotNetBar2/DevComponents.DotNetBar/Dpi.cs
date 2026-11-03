using System.Drawing;

namespace DevComponents.DotNetBar;

public static class Dpi
{
	private static SizeF __098E;

	private static bool _এ;

	private static bool _ঐ;

	private static bool __0991;

	private static int __0992;

	private static bool _ও;

	public static int Width1;

	public static int Width2;

	public static int Width3;

	public static int Width5;

	public static int Width4;

	public static int Width6;

	public static int Width7;

	public static int Width8;

	public static int Width9;

	public static int Width10;

	public static int Width11;

	public static int Width12;

	public static int Width13;

	public static int Width14;

	public static int Width15;

	public static int Width16;

	public static int Width17;

	public static int Width18;

	public static int Width20;

	public static int Width22;

	public static int Width24;

	public static int Width25;

	public static int Width26;

	public static int Width28;

	public static int Width32;

	public static int Width34;

	public static int Width80;

	public static int Width100;

	public static int Height1;

	public static int Height2;

	public static int Height3;

	public static int Height4;

	public static int Height5;

	public static int Height6;

	public static int Height7;

	public static int Height8;

	public static int Height9;

	public static int Height10;

	public static int Height11;

	public static int Height12;

	public static int Height13;

	public static int Height14;

	public static int Height15;

	public static int Height16;

	public static int Height17;

	public static int Height18;

	public static int Height20;

	public static int Height22;

	public static int Height23;

	public static int Height24;

	public static int Height25;

	public static int Height28;

	public static int Height32;

	public static int Height50;

	public static bool RecordScalePerControl
	{
		get
		{
			return __0991;
		}
		set
		{
			__0991 = value;
		}
	}

	public static bool NormalizeScaleFactor
	{
		get
		{
			return _ঐ;
		}
		set
		{
			_ঐ = value;
		}
	}

	public static SizeF Factor => __098E;

	public static bool UseFactor => _এ;

	public static int CaptionVerticalPadding
	{
		get
		{
			if (!_এ)
			{
				return 0;
			}
			return __0992;
		}
	}

	public static bool AutoScaleImages
	{
		get
		{
			return _ও;
		}
		set
		{
			_ও = value;
		}
	}

	static Dpi()
	{
		__098E = new SizeF(1f, 1f);
		_এ = false;
		_ঐ = false;
		__0991 = false;
		__0992 = 10;
		_ও = true;
		Width1 = 1;
		Width2 = 2;
		Width3 = 3;
		Width5 = 5;
		Width4 = 4;
		Width6 = 6;
		Width7 = 7;
		Width8 = 8;
		Width9 = 9;
		Width10 = 10;
		Width11 = 11;
		Width12 = 12;
		Width13 = 13;
		Width14 = 14;
		Width15 = 15;
		Width16 = 16;
		Width17 = 17;
		Width18 = 18;
		Width20 = 20;
		Width22 = 22;
		Width24 = 24;
		Width25 = 25;
		Width26 = 26;
		Width28 = 28;
		Width32 = 32;
		Width34 = 34;
		Width80 = 80;
		Width100 = 100;
		Height1 = 1;
		Height2 = 2;
		Height3 = 3;
		Height4 = 4;
		Height5 = 5;
		Height6 = 6;
		Height7 = 7;
		Height8 = 8;
		Height9 = 9;
		Height10 = 10;
		Height11 = 11;
		Height12 = 12;
		Height13 = 13;
		Height14 = 14;
		Height15 = 15;
		Height16 = 16;
		Height17 = 17;
		Height18 = 18;
		Height20 = 20;
		Height22 = 22;
		Height23 = 23;
		Height24 = 24;
		Height25 = 25;
		Height28 = 28;
		Height32 = 32;
		Height50 = 50;
	}

	public static Size Size(Size size)
	{
		if (!_এ)
		{
			return size;
		}
		size.Width = (int)((float)size.Width * __098E.Width);
		size.Height = (int)((float)size.Height * __098E.Height);
		return size;
	}

	public static Size Size(Size size, SizeF factor)
	{
		size.Width = (int)((float)size.Width * factor.Width);
		size.Height = (int)((float)size.Height * factor.Height);
		return size;
	}

	public static Size ImageSize(Size size)
	{
		if (!_এ || !_ও)
		{
			return size;
		}
		size.Width = (int)((float)size.Width * __098E.Width);
		size.Height = (int)((float)size.Height * __098E.Height);
		return size;
	}

	public static Size Size(int width, int height)
	{
		if (!_এ)
		{
			return new Size(width, height);
		}
		return new Size((int)((float)width * __098E.Width), (int)((float)height * __098E.Height));
	}

	public static Rectangle Size(Rectangle r)
	{
		if (!_এ)
		{
			return r;
		}
		r.Width = (int)((float)r.Width * __098E.Width);
		r.Height = (int)((float)r.Height * __098E.Height);
		return r;
	}

	public static Point Size(Point p)
	{
		if (!_এ)
		{
			return p;
		}
		p.X = (int)((float)p.X * __098E.Width);
		p.Y = (int)((float)p.Y * __098E.Height);
		return p;
	}

	public static Padding Size(Padding padding)
	{
		if (!_এ)
		{
			return padding;
		}
		return new Padding((int)((float)padding.Left * __098E.Width), (int)((float)padding.Right * __098E.Width), (int)((float)padding.Top * __098E.Height), (int)((float)padding.Bottom * __098E.Height));
	}

	public static int DescaleWidth(int width)
	{
		if (!_এ)
		{
			return width;
		}
		return (int)((float)width / __098E.Width);
	}

	public static int DescaleHeight(int height)
	{
		if (!_এ)
		{
			return height;
		}
		return (int)((float)height / __098E.Height);
	}

	internal static Size ঔ(Size ߒ)
	{
		if (!_এ)
		{
			return ߒ;
		}
		ߒ.Width = (int)((float)ߒ.Width / __098E.Width);
		ߒ.Height = (int)((float)ߒ.Height / __098E.Height);
		return ߒ;
	}

	public static int Width(int width)
	{
		if (!_এ)
		{
			return width;
		}
		return (int)((float)width * __098E.Width);
	}

	public static int ImageWidth(int width)
	{
		if (!_এ || !_ও)
		{
			return width;
		}
		return (int)((float)width * __098E.Width);
	}

	public static int Height(int height)
	{
		if (!_এ)
		{
			return height;
		}
		return (int)((float)height * __098E.Height);
	}

	public static int ImageHeight(int height)
	{
		if (!_এ || !_ও)
		{
			return height;
		}
		return (int)((float)height * __098E.Height);
	}

	public static float Height(float height)
	{
		if (!_এ)
		{
			return height;
		}
		return height * __098E.Height;
	}

	public static void SetScaling(SizeF factor)
	{
		if (factor.Width < 1f)
		{
			factor.Width = 1f;
		}
		if (factor.Height < 1f)
		{
			factor.Height = 1f;
		}
		if (!(factor == __098E))
		{
			if (_ঐ)
			{
				factor.Height = factor.Width;
			}
			if (factor.Width == 1f && factor.Height == 1f)
			{
				_এ = false;
				__098E = factor;
			}
			else
			{
				_এ = true;
				__098E = factor;
			}
			ক();
		}
	}

	private static void ক()
	{
		Width1 = Width(1);
		Width2 = Width(2);
		Width3 = Width(3);
		Width4 = Width(4);
		Width5 = Width(5);
		Width6 = Width(6);
		Width7 = Width(7);
		Width8 = Width(8);
		Width9 = Width(9);
		Width10 = Width(10);
		Width11 = Width(11);
		Width12 = Width(12);
		Width13 = Width(13);
		Width14 = Width(14);
		Width15 = Width(15);
		Width16 = Width(16);
		Width17 = Width(17);
		Width18 = Width(18);
		Width20 = Width(20);
		Width22 = Width(22);
		Width24 = Width(24);
		Width25 = Width(25);
		Width26 = Width(26);
		Width28 = Width(28);
		Width32 = Width(32);
		Width34 = Width(34);
		Width80 = Width(80);
		Width100 = Width(100);
		Height1 = Height(1);
		Height2 = Height(2);
		Height3 = Height(3);
		Height4 = Height(4);
		Height5 = Height(5);
		Height6 = Height(6);
		Height7 = Height(7);
		Height8 = Height(8);
		Height9 = Height(9);
		Height10 = Height(10);
		Height11 = Height(11);
		Height12 = Height(12);
		Height13 = Height(13);
		Height14 = Height(14);
		Height15 = Height(15);
		Height16 = Height(16);
		Height17 = Height(17);
		Height18 = Height(18);
		Height20 = Height(20);
		Height22 = Height(22);
		Height23 = Height(23);
		Height24 = Height(24);
		Height25 = Height(25);
		Height28 = Height(28);
		Height32 = Height(32);
		Height50 = Height(50);
	}
}
