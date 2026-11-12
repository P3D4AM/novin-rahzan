using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base.Drawing.Design;

namespace Stimulsoft.Base.Drawing;

[TypeConverter(typeof(PointDConverter))]
public struct PointD
{
	public static PointD Empty;

	public double X { get; set; }

	public double Y { get; set; }

	[Browsable(false)]
	public bool IsEmpty
	{
		get
		{
			if (X == 0.0)
			{
				return Y == 0.0;
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
		PointD pointD = (PointD)obj;
		if (pointD.X == X)
		{
			return pointD.Y == Y;
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

	public Point ToPoint()
	{
		return new Point((int)X, (int)Y);
	}

	public PointD(double x, double y)
	{
		X = x;
		Y = y;
	}
}
