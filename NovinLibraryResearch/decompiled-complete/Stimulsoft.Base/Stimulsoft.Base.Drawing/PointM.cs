using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base.Drawing.Design;

namespace Stimulsoft.Base.Drawing;

[TypeConverter(typeof(PointMConverter))]
public struct PointM
{
	public static PointM Empty;

	public decimal X { get; set; }

	public decimal Y { get; set; }

	[Browsable(false)]
	public bool IsEmpty
	{
		get
		{
			if (X == 0m)
			{
				return Y == 0m;
			}
			return false;
		}
	}

	public override string ToString()
	{
		return $"{{X={X}, Y={Y}}}";
	}

	public override bool Equals(object obj)
	{
		PointM pointM = (PointM)obj;
		if (pointM.X == X)
		{
			return pointM.Y == Y;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public PointF ToPointF()
	{
		return new PointF((float)X, (float)Y);
	}

	public PointD ToPointD()
	{
		return new PointD((double)X, (double)Y);
	}

	public Point ToPoint()
	{
		return new Point((int)X, (int)Y);
	}

	public PointM(decimal x, decimal y)
	{
		this = default(PointM);
		X = x;
		Y = y;
	}
}
