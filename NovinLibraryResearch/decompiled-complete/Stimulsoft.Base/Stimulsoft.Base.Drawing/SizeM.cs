using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Base.Drawing;

[TypeConverter(typeof(SizeMConverter))]
public struct SizeM : IStiDefault
{
	public static SizeM Empty = new SizeM(0m, 0m);

	[Browsable(false)]
	public bool IsDefault
	{
		get
		{
			if (Width == 0m)
			{
				return Height == 0m;
			}
			return false;
		}
	}

	[StiOrder(1)]
	[RefreshProperties(RefreshProperties.All)]
	public decimal Width { get; set; }

	[StiOrder(2)]
	[RefreshProperties(RefreshProperties.All)]
	public decimal Height { get; set; }

	[Browsable(false)]
	public bool IsEmpty
	{
		get
		{
			if (Width == 0m)
			{
				return Height == 0m;
			}
			return false;
		}
	}

	public override bool Equals(object obj)
	{
		SizeM sizeM = (SizeM)obj;
		if (sizeM.Width == Width)
		{
			return sizeM.Height == Height;
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

	public SizeD ToSizeD()
	{
		return new SizeD((double)Width, (double)Height);
	}

	public SizeM(decimal width, decimal height)
	{
		this = default(SizeM);
		Width = width;
		Height = height;
	}
}
