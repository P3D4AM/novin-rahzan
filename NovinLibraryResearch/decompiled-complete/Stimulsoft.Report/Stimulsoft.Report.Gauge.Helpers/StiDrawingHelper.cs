using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Report.Gauge.Helpers;

public static class StiDrawingHelper
{
	internal static readonly float PiDiv180 = (float)Math.PI / 180f;

	private static readonly float FourDivThree = 1.3333334f;

	public static GraphicsPath GetRoundedPath(RectangleF rect, int offset, int leftTop, int rightTop, int rightBottom, int leftBottom)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		float num = rect.X + rect.Width - (float)offset;
		float num2 = rect.Y + rect.Height - (float)offset;
		if (rightTop != 0)
		{
			val.AddLine(rect.X + (float)leftTop, rect.Y, num - (float)(leftTop * 2), rect.Y);
			val.AddArc(num - (float)(leftTop * 2), rect.Y, (float)(leftTop * 2), (float)(leftTop * 2), 270f, 90f);
		}
		else
		{
			val.AddLine(rect.X + (float)leftTop, rect.Y, num, rect.Y);
		}
		if (rightBottom != 0)
		{
			val.AddLine(num, rect.Y + (float)rightTop, num, num2 - (float)(rightTop * 2));
			val.AddArc(num - (float)(rightTop * 2), num2 - (float)(rightTop * 2), (float)(rightTop * 2), (float)(rightTop * 2), 0f, 90f);
		}
		else
		{
			val.AddLine(num, rect.Y + (float)rightTop, num, num2);
		}
		if (leftBottom != 0)
		{
			val.AddLine(num - (float)(rightBottom * 2), num2, rect.X + (float)rightBottom, num2);
			val.AddArc(rect.X, num2 - (float)(rightBottom * 2), (float)(rightBottom * 2), (float)(rightBottom * 2), 90f, 90f);
		}
		else
		{
			val.AddLine(num - (float)rightBottom, num2, rect.X, num2);
		}
		if (leftTop != 0)
		{
			val.AddLine(rect.X, num2 - (float)(leftTop * 2), rect.X, rect.Y + (float)leftTop);
			val.AddArc(rect.X, rect.Y, (float)(leftTop * 2), (float)(leftTop * 2), 180f, 90f);
		}
		else
		{
			val.AddLine(rect.X, num2 - (float)leftTop, rect.X, rect.Y);
		}
		val.CloseFigure();
		return val;
	}

	public static GraphicsPath GetArcGeometry(RectangleF rect, float startAngle, float sweepAngle, float startWidth, float endWidth)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		PointF centerPoint = new PointF(StiRectangleHelper.CenterX(rect), StiRectangleHelper.CenterY(rect));
		float num = Math.Min(rect.Width / 2f, rect.Height / 2f);
		float num2 = startAngle * PiDiv180;
		float x = centerPoint.X + num * (float)Math.Cos(num2);
		float y = centerPoint.Y + num * (float)Math.Sin(num2);
		PointF pointF = new PointF(x, y);
		PointF pointF2 = pointF;
		float num3 = Round(Math.Abs(sweepAngle / 90f));
		float num4 = sweepAngle / num3;
		num2 = startAngle;
		for (int i = 0; (float)i < num3; i++)
		{
			List<PointF> list = ConvertArcToCubicBezier(centerPoint, num, num2, num4);
			val.AddBezier(pointF, list[1], list[2], list[3]);
			pointF = list[3];
			num2 += num4;
		}
		PointF pointF3 = pointF;
		num3 = Round(Math.Abs(sweepAngle / 90f));
		num4 = sweepAngle / num3;
		float num5 = num - rect.Width * startWidth;
		float num6 = num - rect.Width * endWidth;
		if (num5 <= 0f || num6 <= 0f)
		{
			return null;
		}
		float num7 = num5 - num6;
		float num8 = 1f / num3;
		float num9 = 0f;
		bool flag = false;
		num2 = startAngle;
		for (int j = 0; (float)j < num3; j++)
		{
			float radius = num5 - num7 * num9;
			float radius2 = num5 - num7 * (num9 + num8);
			List<PointF> list2 = ConvertArcToCubicBezier(centerPoint, radius, radius2, num2, num4);
			if (!flag)
			{
				val.StartFigure();
				val.AddLine(pointF2, list2[0]);
				pointF = list2[0];
				flag = true;
			}
			val.AddBezier(pointF, list2[1], list2[2], list2[3]);
			pointF = list2[3];
			num2 += num4;
			num9 += num8;
		}
		val.AddLine(pointF, pointF3);
		return val;
	}

	public static GraphicsPath GetRadialRangeGeometry(PointF centerPoint, float startAngle, float sweepAngle, float radius1, float radius2, float radius3, float radius4)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		float num = startAngle * PiDiv180;
		float x = centerPoint.X + radius1 * (float)Math.Cos(num);
		float y = centerPoint.Y + radius1 * (float)Math.Sin(num);
		PointF pointF = new PointF(x, y);
		PointF pointF2 = pointF;
		float num2 = Round(Math.Abs(sweepAngle / 90f));
		float num3 = sweepAngle / num2;
		float num4 = radius1 - radius2;
		float num5 = 1f / num2;
		float num6 = 0f;
		num = startAngle;
		for (int i = 0; (float)i < num2; i++)
		{
			float radius5 = radius1 - num4 * num6;
			float radius6 = radius1 - num4 * (num6 + num5);
			List<PointF> list = ConvertArcToCubicBezier(centerPoint, radius5, radius6, num, num3);
			val.AddBezier(pointF, list[1], list[2], list[3]);
			pointF = list[3];
			num += num3;
			num6 += num5;
		}
		PointF pointF3 = pointF;
		num3 = sweepAngle / num2;
		num4 = radius3 - radius4;
		num5 = 1f / num2;
		num6 = 0f;
		bool flag = false;
		num = startAngle;
		for (int j = 0; (float)j < num2; j++)
		{
			float radius7 = radius3 - num4 * num6;
			float radius8 = radius3 - num4 * (num6 + num5);
			List<PointF> list2 = ConvertArcToCubicBezier(centerPoint, radius7, radius8, num, num3);
			if (!flag)
			{
				val.StartFigure();
				val.AddLine(pointF2, list2[0]);
				pointF = list2[0];
				flag = true;
			}
			val.AddBezier(pointF, list2[1], list2[2], list2[3]);
			pointF = list2[3];
			num += num3;
			num6 += num5;
		}
		val.AddLine(pointF, pointF3);
		return val;
	}

	private static float Round(float value)
	{
		int num = (int)value;
		float num2 = value - (float)num;
		if (!(num2 > 0f))
		{
			return num;
		}
		return num + 1;
	}

	internal static List<PointF> ConvertArcToCubicBezier(PointF centerPoint, float radius, float startAngle, float sweepAngle)
	{
		float num = startAngle * PiDiv180;
		float num2 = sweepAngle * PiDiv180;
		float num3 = num + num2;
		float x = centerPoint.X + radius * (float)Math.Cos(num);
		float y = centerPoint.Y + radius * (float)Math.Sin(num);
		float x2 = centerPoint.X + radius * (float)Math.Cos(num3);
		float y2 = centerPoint.Y + radius * (float)Math.Sin(num3);
		float num4 = radius * FourDivThree * (float)Math.Tan(0.25 * (double)num2);
		float num5 = (float)Math.Atan(num4 / radius);
		float num6 = radius / (float)Math.Cos(num5);
		num5 += num;
		float x3 = centerPoint.X + num6 * (float)Math.Cos(num5);
		float y3 = centerPoint.Y + num6 * (float)Math.Sin(num5);
		num5 = (float)Math.Atan((0f - num4) / radius);
		num5 += num3;
		float x4 = centerPoint.X + num6 * (float)Math.Cos(num5);
		float y4 = centerPoint.Y + num6 * (float)Math.Sin(num5);
		List<PointF> list = new List<PointF>(4);
		list.Add(new PointF(x, y));
		list.Add(new PointF(x3, y3));
		list.Add(new PointF(x4, y4));
		list.Add(new PointF(x2, y2));
		return list;
	}

	internal static List<PointF> ConvertArcToCubicBezier(PointF centerPoint, float radius1, float radius2, float startAngle, float sweepAngle)
	{
		float num = startAngle * PiDiv180;
		float num2 = sweepAngle * PiDiv180;
		float num3 = num + num2;
		float x = centerPoint.X + radius1 * (float)Math.Cos(num);
		float y = centerPoint.Y + radius1 * (float)Math.Sin(num);
		float x2 = centerPoint.X + radius2 * (float)Math.Cos(num3);
		float y2 = centerPoint.Y + radius2 * (float)Math.Sin(num3);
		float num4 = (radius1 - radius2) / 3f;
		radius1 -= num4;
		radius2 += num4;
		float num5 = radius1 * FourDivThree * (float)Math.Tan(0.25 * (double)num2);
		float num6 = (float)Math.Atan(num5 / radius1);
		float num7 = radius1 / (float)Math.Cos(num6);
		num6 += num;
		float x3 = centerPoint.X + num7 * (float)Math.Cos(num6);
		float y3 = centerPoint.Y + num7 * (float)Math.Sin(num6);
		num6 = (float)Math.Atan((0f - num5) / radius1);
		num6 += num3;
		float x4 = centerPoint.X + num7 * (float)Math.Cos(num6);
		float y4 = centerPoint.Y + num7 * (float)Math.Sin(num6);
		return new List<PointF>(4)
		{
			new PointF(x, y),
			new PointF(x3, y3),
			new PointF(x4, y4),
			new PointF(x2, y2)
		};
	}
}
