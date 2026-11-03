using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Report.Chart;

public abstract class StiStyleCoreXF
{
	public abstract string LocalizedName { get; }

	[StiNonSerialized]
	public virtual StiChartStyleId StyleId => StiChartStyleId.StiStyle01;

	public virtual StiBrush ChartBrush => (StiBrush)new StiSolidBrush(StiColorUtils.Light(BasicStyleColor, (byte)100));

	public virtual StiBrush ChartAreaBrush => (StiBrush)new StiGradientBrush(StiColorUtils.Light(BasicStyleColor, (byte)80), StiColorUtils.Light(BasicStyleColor, (byte)40), 90.0);

	public virtual Color ChartAreaBorderColor => StiColorUtils.Dark(BasicStyleColor, (byte)150);

	[Browsable(false)]
	public IStiChart Chart { get; set; }

	public virtual bool ChartAreaShowShadow => false;

	public virtual StiBrush SeriesLabelsBrush => (StiBrush)new StiSolidBrush(BasicStyleColor);

	public virtual Color SeriesLabelsColor => StiColorUtils.Dark(BasicStyleColor, (byte)150);

	public virtual Color SeriesLabelsBorderColor => StiColorUtils.Dark(BasicStyleColor, (byte)150);

	public virtual Color SeriesLabelsLineColor => StiColorUtils.Dark(BasicStyleColor, (byte)150);

	public virtual Font SeriesLabelsFont => new Font("Arial", 7f);

	public virtual Color TrendLineColor => StiColorUtils.Dark(BasicStyleColor, (byte)150);

	public virtual bool TrendLineShowShadow => false;

	public virtual StiBrush LegendBrush => (StiBrush)new StiGradientBrush(StiColorUtils.Light(BasicStyleColor, (byte)80), StiColorUtils.Light(BasicStyleColor, (byte)20), 90.0);

	public virtual Color LegendLabelsColor => StiColorUtils.Dark(BasicStyleColor, (byte)150);

	public virtual Color LegendBorderColor => StiColorUtils.Dark(BasicStyleColor, (byte)100);

	public virtual Color LegendTitleColor => StiColorUtils.Dark(BasicStyleColor, (byte)150);

	public virtual bool LegendShowShadow { get; }

	public virtual Font LegendFont => new Font("Arial", 8f);

	public virtual Color AxisTitleColor => StiColorUtils.Dark(BasicStyleColor, (byte)150);

	public virtual Color AxisLineColor => StiColorUtils.Dark(BasicStyleColor, (byte)150);

	public virtual Color AxisLabelsColor => StiColorUtils.Dark(BasicStyleColor, (byte)150);

	public virtual StiBrush InterlacingHorBrush => (StiBrush)new StiSolidBrush(Color.FromArgb(10, StiColorUtils.Dark(BasicStyleColor, (byte)100)));

	public virtual StiBrush InterlacingVertBrush => (StiBrush)new StiSolidBrush(Color.FromArgb(10, StiColorUtils.Dark(BasicStyleColor, (byte)100)));

	public virtual Color GridLinesHorColor => Color.FromArgb(100, StiColorUtils.Dark(BasicStyleColor, (byte)150));

	public virtual Color GridLinesVertColor => Color.FromArgb(100, StiColorUtils.Dark(BasicStyleColor, (byte)150));

	public virtual bool SeriesLighting => true;

	public virtual bool SeriesShowShadow { get; }

	public virtual bool SeriesShowBorder { get; } = true;


	public virtual int SeriesBorderThickness { get; } = 1;


	public virtual bool MarkerVisible { get; set; } = true;


	public virtual Color FirstStyleColor => StyleColors[0];

	public virtual Color LastStyleColor => StyleColors[StyleColors.Length - 1];

	public abstract Color[] StyleColors { get; }

	public abstract Color BasicStyleColor { get; }

	public virtual void FillColumn(StiContext context, RectangleF rect, StiBrush brush, StiInteractionDataGeom interaction, int elementIndex = -1)
	{
		context.FillRectangle(brush, rect.X, rect.Y, rect.Width, rect.Height, interaction, elementIndex);
	}

	public virtual StiBrush GetAreaBrush(Color color)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		return (StiBrush)new StiSolidBrush(Color.FromArgb(200, color));
	}

	public virtual StiBrush GetColumnBrush(Color color)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		return (StiBrush)new StiGradientBrush(color, StiColorUtils.Dark(color, (byte)50), 0.0);
	}

	public virtual Color GetColumnBorder(Color color)
	{
		if (!SeriesShowBorder)
		{
			return Color.Transparent;
		}
		return StiColorUtils.Dark(color, (byte)100);
	}

	public virtual Color[] GetColors(int seriesCount, Color[] seriesColors = null)
	{
		Color[] array = new Color[seriesCount];
		Color[] array2 = ((seriesColors != null) ? seriesColors : StyleColors);
		int num = 0;
		int num2 = 0;
		Color[] array3 = new Color[array2.Length];
		int num3 = 0;
		for (int i = 0; i < seriesCount; i++)
		{
			if (array2.Length == 0 || array2.Length < num)
			{
				array[i] = Color.FromArgb(255, 112, 173, 71);
			}
			else if (num3 > 1)
			{
				array[i] = array3[num];
			}
			else if (num2 != 0)
			{
				Color color = array2[num];
				int alpha = Math.Min(color.A + num2, 255);
				int red = Math.Min(color.R + num2, 255);
				int green = Math.Min(color.G + num2, 255);
				int blue = Math.Min(color.B + num2, 255);
				array3[num] = (array[i] = Color.FromArgb(alpha, red, green, blue));
			}
			else
			{
				array[i] = array2[num];
			}
			num++;
			if (num == array2.Length)
			{
				num3++;
				num = 0;
				num2 = 50;
			}
		}
		return array;
	}

	public Color GetColorByIndex(int index, int count, Color[] seriesColors)
	{
		return GetColors(count, seriesColors)[index];
	}

	public Color GetColorBySeries(IStiSeries series, Color[] seriesColors = null)
	{
		return GetColors(series.Chart.Series.Count, seriesColors)[series.Chart.Series.IndexOf(series)];
	}
}
