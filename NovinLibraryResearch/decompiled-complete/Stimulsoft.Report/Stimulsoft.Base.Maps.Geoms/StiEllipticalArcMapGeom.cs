using System;
using System.Drawing.Drawing2D;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Base.Maps.Geoms;

public class StiEllipticalArcMapGeom : StiMapGeom
{
	private const double RadiansPerDegree = Math.PI / 180.0;

	private const double DoublePI = Math.PI * 2.0;

	public double StartX { get; set; }

	public double StartY { get; set; }

	public double RadiusX { get; set; }

	public double RadiusY { get; set; }

	public double Angle { get; set; }

	public StiSvgArcSize Size { get; set; }

	public StiSvgArcSweep Sweep { get; set; }

	public double EndX { get; set; }

	public double EndY { get; set; }

	public override StiMapGeomType GeomType => StiMapGeomType.EllipticalArc;

	private static double CalculateVectorAngle(double ux, double uy, double vx, double vy)
	{
		double num = Math.Atan2(uy, ux);
		double num2 = Math.Atan2(vy, vx);
		if (num2 >= num)
		{
			return num2 - num;
		}
		return Math.PI * 2.0 - (num - num2);
	}

	public void AddToPath(GraphicsPath graphicsPath)
	{
		if (StartX == EndX && StartY == EndY)
		{
			return;
		}
		if (RadiusX == 0.0 && RadiusY == 0.0)
		{
			graphicsPath.AddLine((float)StartX, (float)StartY, (float)EndX, (float)EndY);
			return;
		}
		double num = Math.Sin(Angle * (Math.PI / 180.0));
		double num2 = Math.Cos(Angle * (Math.PI / 180.0));
		double num3 = num2 * (StartX - EndX) / 2.0 + num * (StartY - EndY) / 2.0;
		double num4 = (0.0 - num) * (StartX - EndX) / 2.0 + num2 * (StartY - EndY) / 2.0;
		double num5 = RadiusX * RadiusX * RadiusY * RadiusY - RadiusX * RadiusX * num4 * num4 - RadiusY * RadiusY * num3 * num3;
		double num6 = RadiusX;
		double num7 = RadiusY;
		double num9;
		if (num5 < 0.0)
		{
			float num8 = (float)Math.Sqrt(1.0 - num5 / (RadiusX * RadiusX * RadiusY * RadiusY));
			num6 *= (double)num8;
			num7 *= (double)num8;
			num9 = 0.0;
		}
		else
		{
			num9 = (((Size == StiSvgArcSize.Large && Sweep == StiSvgArcSweep.Positive) || (Size == StiSvgArcSize.Small && Sweep == StiSvgArcSweep.Negative)) ? (-1.0) : 1.0) * Math.Sqrt(num5 / (RadiusX * RadiusX * num4 * num4 + RadiusY * RadiusY * num3 * num3));
		}
		double num10 = num9 * num6 * num4 / num7;
		double num11 = (0.0 - num9) * num7 * num3 / num6;
		double num12 = num2 * num10 - num * num11 + (StartX + EndX) / 2.0;
		double num13 = num * num10 + num2 * num11 + (StartY + EndY) / 2.0;
		double num14 = CalculateVectorAngle(1.0, 0.0, (num3 - num10) / num6, (num4 - num11) / num7);
		double num15 = CalculateVectorAngle((num3 - num10) / num6, (num4 - num11) / num7, (0.0 - num3 - num10) / num6, (0.0 - num4 - num11) / num7);
		if (Sweep == StiSvgArcSweep.Negative && num15 > 0.0)
		{
			num15 -= Math.PI * 2.0;
		}
		else if (Sweep == StiSvgArcSweep.Positive && num15 < 0.0)
		{
			num15 += Math.PI * 2.0;
		}
		int num16 = (int)Math.Ceiling(Math.Abs(num15 / (Math.PI / 2.0)));
		double num17 = num15 / (double)num16;
		double num18 = 2.6666666666666665 * Math.Sin(num17 / 4.0) * Math.Sin(num17 / 4.0) / Math.Sin(num17 / 2.0);
		double num19 = StartX;
		double num20 = StartY;
		for (int i = 0; i < num16; i++)
		{
			double num21 = Math.Cos(num14);
			double num22 = Math.Sin(num14);
			double num23 = num14 + num17;
			double num24 = Math.Cos(num23);
			double num25 = Math.Sin(num23);
			double num26 = num2 * num6 * num24 - num * num7 * num25 + num12;
			double num27 = num * num6 * num24 + num2 * num7 * num25 + num13;
			double num28 = num18 * ((0.0 - num2) * num6 * num22 - num * num7 * num21);
			double num29 = num18 * ((0.0 - num) * num6 * num22 + num2 * num7 * num21);
			double num30 = num18 * (num2 * num6 * num25 + num * num7 * num24);
			double num31 = num18 * (num * num6 * num25 - num2 * num7 * num24);
			graphicsPath.AddBezier((float)num19, (float)num20, (float)(num19 + num28), (float)(num20 + num29), (float)(num26 + num30), (float)(num27 + num31), (float)num26, (float)num27);
			num14 = num23;
			num19 = (float)num26;
			num20 = (float)num27;
		}
	}

	public override PointD GetLastPoint()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return new PointD(EndX, EndY);
	}

	public override string ToString()
	{
		return $"EllipticalArc={StartX},{StartY}, {RadiusX},{RadiusY}, {Angle},{Size},{Sweep},{EndX},{EndY}";
	}
}
