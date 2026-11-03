using System.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiTrendLineLinearCoreXF : StiTrendLineCoreXF
{
	public override string LocalizedName => StiLocalization.Get("PropertyMain", "Linear");

	public override void RenderTrendLine(StiAreaGeom geom, PointF?[] points, float posY)
	{
		if (points.Length == 0)
		{
			return;
		}
		float[] array = new float[points.Length];
		float[] array2 = new float[points.Length];
		float num = points[0].Value.X;
		float num2 = points[0].Value.X;
		for (int i = 0; i < points.Length; i++)
		{
			array[i] = points[i].Value.X;
			array2[i] = points[i].Value.Y;
			if (num < array[i])
			{
				num = array[i];
			}
			if (num2 > array[i])
			{
				num2 = array[i];
			}
		}
		int num3 = points.Length;
		float num4 = Sum(array);
		float num5 = Sum(array2);
		float num6 = SumSqr(array);
		float num7 = SumProductions(array, array2);
		float num8 = num4 * num4 - num6 * (float)num3;
		float num9 = (num5 * num4 - num7 * (float)num3) / num8;
		float num10 = (num4 * num7 - num6 * num5) / num8;
		StiTrendLineGeom item = new StiTrendLineGeom(new PointF(num2, num9 * num2 + num10), new PointF(num, num9 * num + num10), base.TrendLine);
		geom.CreateChildGeoms();
		geom.ChildGeoms.Add(item);
	}

	public StiTrendLineLinearCoreXF(IStiTrendLine trendLine)
		: base(trendLine)
	{
	}
}
