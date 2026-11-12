using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Base.Drawing;

[TypeConverter(typeof(SizeDConverter))]
public struct SizeD : IStiDefault
{
	public static SizeD Empty = new SizeD(0.0, 0.0);

	[Browsable(false)]
	public bool IsDefault
	{
		get
		{
			if (Width == 0.0)
			{
				return Height == 0.0;
			}
			return false;
		}
	}

	[StiOrder(1)]
	[RefreshProperties(RefreshProperties.All)]
	public double Width { get; set; }

	[StiOrder(2)]
	[RefreshProperties(RefreshProperties.All)]
	public double Height { get; set; }

	[Browsable(false)]
	public bool IsEmpty
	{
		get
		{
			if (Width == 0.0)
			{
				return Height == 0.0;
			}
			return false;
		}
	}

	public override bool Equals(object obj)
	{
		SizeD sizeD = (SizeD)obj;
		if (sizeD.Width == Width)
		{
			return sizeD.Height == Height;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override string ToString()
	{
		return $"{{Width={Width}, Height={Height}}}";
	}

	public SizeF ToSizeF()
	{
		return new SizeF((float)Width, (float)Height);
	}

	public Size ToSize()
	{
		return new Size((int)Width, (int)Height);
	}

	public SizeD Multiply(double multipleFactor)
	{
		return new SizeD(Width * multipleFactor, Height * multipleFactor);
	}

	public static SizeD CreateFromSize(SizeF size)
	{
		return new SizeD(size.Width, size.Height);
	}

	public SizeD(double width, double height)
	{
		Width = width;
		Height = height;
	}
}
