using System;
using System.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiTrendLineLogarithmicCoreXF : StiTrendLineCoreXF
{
	public override string LocalizedName => StiLocalization.Get("PropertyMain", "Logarithmic");

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
			array[i] = (float)Math.Log(points[i].Value.X);
			array2[i] = points[i].Value.Y;
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
		float num3 = Sum(array2);
		float num4 = SumSqr(array);
		float num5 = SumProductions(array, array2);
		float num6 = num2 * num2 - num4 * (float)num;
		float num7 = (num3 * num2 - num5 * (float)num) / num6;
		float num8 = (num2 * num5 - num4 * num3) / num6;
		PointF?[] array3 = new PointF?[31];
		for (int j = 0; j < 31; j++)
		{
			float num9 = x2 + (x - x2) / 30f * (float)j;
			float y = num7 * (float)Math.Log(num9) + num8;
			array3[j] = new PointF(num9, y);
		}
		StiTrendCurveGeom item = new StiTrendCurveGeom(array3, base.TrendLine);
		geom.CreateChildGeoms();
		geom.ChildGeoms.Add(item);
	}

	public StiTrendLineLogarithmicCoreXF(IStiTrendLine trendLine)
		: base(trendLine)
	{
	}
}
