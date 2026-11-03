using System;
using System.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiTrendLineExponentialCoreXF : StiTrendLineCoreXF
{
	public override string LocalizedName => StiLocalization.Get("PropertyMain", "Exponential");

	public override void RenderTrendLine(StiAreaGeom geom, PointF?[] points, float posY)
	{
		if (points.Length == 0)
		{
			return;
		}
		float[] array = new float[points.Length];
		float[] array2 = new float[points.Length];
		float x = points[0].Value.X;
		float x2 = points[0].Value.X;
		for (int i = 0; i < points.Length; i++)
		{
			array[i] = points[i].Value.X;
			array2[i] = posY - points[i].Value.Y;
			if (x < points[i].Value.X)
			{
				x = points[i].Value.X;
			}
			if (x2 > points[i].Value.X)
			{
				x2 = points[i].Value.X;
			}
		}
		int num = points.Length;
		float num2 = Sum(array);
		float num3 = SumSqr(array);
		float num4 = SumProductionsXLogY(array, array2);
		float num5 = SumLn(array2);
		float num6 = ((float)num * num4 - num2 * num5) / ((float)num * num3 - num2 * num2);
		float num7 = (num3 * num5 - num2 * num4) / ((float)num * num3 - num2 * num2);
		float num8 = (float)Math.Exp(num7);
		PointF?[] array3 = new PointF?[31];
		for (int j = 0; j < 31; j++)
		{
			float num9 = x2 + (x - x2) / 30f * (float)j;
			float num10 = num8 * (float)Math.Exp(num6 * num9);
			array3[j] = new PointF(num9, posY - num10);
		}
		StiTrendCurveGeom item = new StiTrendCurveGeom(array3, base.TrendLine);
		geom.CreateChildGeoms();
		geom.ChildGeoms.Add(item);
	}

	public StiTrendLineExponentialCoreXF(IStiTrendLine trendLine)
		: base(trendLine)
	{
	}
}
