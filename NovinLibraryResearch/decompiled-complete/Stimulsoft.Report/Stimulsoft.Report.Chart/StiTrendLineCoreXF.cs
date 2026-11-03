using System;
using System.Drawing;

namespace Stimulsoft.Report.Chart;

public abstract class StiTrendLineCoreXF : ICloneable, IStiApplyStyle
{
	private IStiTrendLine trendLine;

	public abstract string LocalizedName { get; }

	public IStiTrendLine TrendLine
	{
		get
		{
			return trendLine;
		}
		set
		{
			trendLine = value;
		}
	}

	public virtual void ApplyStyle(IStiChartStyle style)
	{
		if (TrendLine.AllowApplyStyle)
		{
			TrendLine.LineColor = style.Core.TrendLineColor;
			TrendLine.ShowShadow = style.Core.TrendLineShowShadow;
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public virtual void RenderTrendLine(StiAreaGeom geom, PointF?[] points, float posY)
	{
	}

	protected float Sum(float[] values)
	{
		float num = 0f;
		foreach (float num2 in values)
		{
			num += num2;
		}
		return num;
	}

	protected float SumSqr(float[] values)
	{
		float num = 0f;
		foreach (float num2 in values)
		{
			num += num2 * num2;
		}
		return num;
	}

	protected float SumProductions(float[] valuesX, float[] valuesY)
	{
		float num = 0f;
		for (int i = 0; i < valuesX.Length; i++)
		{
			num += valuesX[i] * valuesY[i];
		}
		return num;
	}

	protected float SumProductionsXLogY(float[] valuesX, float[] valuesY)
	{
		float num = 0f;
		for (int i = 0; i < valuesX.Length; i++)
		{
			num += valuesX[i] * (float)Math.Log(valuesY[i]);
		}
		return num;
	}

	protected float SumLn(float[] values)
	{
		float num = 0f;
		foreach (float num2 in values)
		{
			num += (float)Math.Log(num2);
		}
		return num;
	}

	public StiTrendLineCoreXF(IStiTrendLine trendLine)
	{
		this.trendLine = trendLine;
	}
}
