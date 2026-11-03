using System.Drawing;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiCustomStyleCoreXF : StiStyleCoreXF01
{
	public Stimulsoft.Report.StiChartStyle ReportChartStyle;

	public override string LocalizedName => "CustomStyle";

	public override StiBrush ChartBrush
	{
		get
		{
			if (ReportStyle == null)
			{
				return base.ChartBrush;
			}
			return ReportStyle.Brush;
		}
	}

	public override StiBrush ChartAreaBrush
	{
		get
		{
			if (ReportStyle == null)
			{
				return base.ChartAreaBrush;
			}
			return ReportStyle.ChartAreaBrush;
		}
	}

	public override Color ChartAreaBorderColor
	{
		get
		{
			if (ReportStyle == null)
			{
				return base.ChartAreaBorderColor;
			}
			return ReportStyle.ChartAreaBorderColor;
		}
	}

	public override bool ChartAreaShowShadow
	{
		get
		{
			if (ReportStyle == null)
			{
				return base.ChartAreaShowShadow;
			}
			return ReportStyle.ChartAreaShowShadow;
		}
	}

	public override bool SeriesLighting
	{
		get
		{
			if (ReportStyle == null)
			{
				return base.SeriesLighting;
			}
			return ReportStyle.SeriesLighting;
		}
	}

	public override bool SeriesShowShadow
	{
		get
		{
			if (ReportStyle == null)
			{
				return base.SeriesShowShadow;
			}
			return ReportStyle.SeriesShowShadow;
		}
	}

	public override bool SeriesShowBorder
	{
		get
		{
			if (ReportStyle == null)
			{
				return base.SeriesShowBorder;
			}
			return ReportStyle.SeriesShowBorder;
		}
	}

	public override StiBrush SeriesLabelsBrush
	{
		get
		{
			if (ReportStyle == null)
			{
				return base.SeriesLabelsBrush;
			}
			return ReportStyle.SeriesLabelsBrush;
		}
	}

	public override Color SeriesLabelsColor
	{
		get
		{
			if (ReportStyle == null)
			{
				return base.SeriesLabelsColor;
			}
			return ReportStyle.SeriesLabelsColor;
		}
	}

	public override Color SeriesLabelsBorderColor
	{
		get
		{
			if (ReportStyle == null)
			{
				return base.SeriesLabelsBorderColor;
			}
			return ReportStyle.SeriesLabelsBorderColor;
		}
	}

	public override Color SeriesLabelsLineColor
	{
		get
		{
			if (ReportStyle == null)
			{
				return base.SeriesLabelsLineColor;
			}
			return ReportStyle.SeriesLabelsLineColor;
		}
	}

	public override int SeriesBorderThickness
	{
		get
		{
			if (ReportStyle == null)
			{
				return base.SeriesBorderThickness;
			}
			return ReportStyle.SeriesBorderThickness;
		}
	}

	public override Color TrendLineColor
	{
		get
		{
			if (ReportStyle == null)
			{
				return base.TrendLineColor;
			}
			return ReportStyle.TrendLineColor;
		}
	}

	public override bool TrendLineShowShadow
	{
		get
		{
			if (ReportStyle == null)
			{
				return base.TrendLineShowShadow;
			}
			return ReportStyle.TrendLineShowShadow;
		}
	}

	public override StiBrush LegendBrush
	{
		get
		{
			if (ReportStyle == null)
			{
				return base.LegendBrush;
			}
			return ReportStyle.LegendBrush;
		}
	}

	public override Color LegendLabelsColor
	{
		get
		{
			if (ReportStyle == null)
			{
				return base.LegendLabelsColor;
			}
			return ReportStyle.LegendLabelsColor;
		}
	}

	public override Color LegendBorderColor
	{
		get
		{
			if (ReportStyle == null)
			{
				return base.LegendBorderColor;
			}
			return ReportStyle.LegendBorderColor;
		}
	}

	public override Color LegendTitleColor
	{
		get
		{
			if (ReportStyle == null)
			{
				return base.LegendTitleColor;
			}
			return ReportStyle.LegendTitleColor;
		}
	}

	public override bool MarkerVisible
	{
		get
		{
			if (ReportStyle == null)
			{
				return base.MarkerVisible;
			}
			return ReportStyle.MarkerVisible;
		}
	}

	public override Color AxisTitleColor
	{
		get
		{
			if (ReportStyle == null)
			{
				return base.AxisTitleColor;
			}
			return ReportStyle.AxisTitleColor;
		}
	}

	public override Color AxisLineColor
	{
		get
		{
			if (ReportStyle == null)
			{
				return base.AxisLineColor;
			}
			return ReportStyle.AxisLineColor;
		}
	}

	public override Color AxisLabelsColor
	{
		get
		{
			if (ReportStyle == null)
			{
				return base.AxisLabelsColor;
			}
			return ReportStyle.AxisLabelsColor;
		}
	}

	public override StiBrush InterlacingHorBrush
	{
		get
		{
			if (ReportStyle == null)
			{
				return base.InterlacingHorBrush;
			}
			return ReportStyle.InterlacingHorBrush;
		}
	}

	public override StiBrush InterlacingVertBrush
	{
		get
		{
			if (ReportStyle == null)
			{
				return base.InterlacingVertBrush;
			}
			return ReportStyle.InterlacingVertBrush;
		}
	}

	public override Color GridLinesHorColor
	{
		get
		{
			if (ReportStyle == null)
			{
				return base.GridLinesHorColor;
			}
			return ReportStyle.GridLinesHorColor;
		}
	}

	public override Color GridLinesVertColor
	{
		get
		{
			if (ReportStyle == null)
			{
				return base.GridLinesVertColor;
			}
			return ReportStyle.GridLinesVertColor;
		}
	}

	public override Color[] StyleColors
	{
		get
		{
			if (ReportStyle == null)
			{
				return base.StyleColors;
			}
			return ReportStyle.StyleColors;
		}
	}

	public override Color BasicStyleColor
	{
		get
		{
			if (ReportStyle == null)
			{
				return base.BasicStyleColor;
			}
			return ReportStyle.BasicStyleColor;
		}
	}

	public string ReportStyleName { get; set; }

	public Stimulsoft.Report.StiChartStyle ReportStyle
	{
		get
		{
			string text = ((ReportStyleName == null) ? ((StiChart)base.Chart).CustomStyleName : ReportStyleName);
			if (base.Chart == null || ((StiChart)base.Chart).Report == null || text == null || text.Length == 0)
			{
				return null;
			}
			return ((StiChart)base.Chart).Report.Styles[text] as Stimulsoft.Report.StiChartStyle;
		}
	}

	public StiCustomStyle CustomStyle { get; }

	public override StiBrush GetColumnBrush(Color color)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Expected O, but got Unknown
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Expected O, but got Unknown
		if (ReportStyle == null)
		{
			return (StiBrush)new StiSolidBrush(color);
		}
		return (StiBrush)(ReportStyle.BrushType switch
		{
			StiBrushType.Glare => (object)new StiGlareBrush(StiColorUtils.Dark(color, (byte)50), color, 0.0), 
			StiBrushType.Gradient0 => (object)new StiGradientBrush(StiColorUtils.Dark(color, (byte)50), color, 0.0), 
			StiBrushType.Gradient90 => (object)new StiGradientBrush(StiColorUtils.Dark(color, (byte)50), color, 90.0), 
			StiBrushType.Gradient180 => (object)new StiGradientBrush(StiColorUtils.Dark(color, (byte)50), color, 180.0), 
			StiBrushType.Gradient270 => (object)new StiGradientBrush(StiColorUtils.Dark(color, (byte)50), color, 270.0), 
			StiBrushType.Gradient45 => (object)new StiGradientBrush(StiColorUtils.Dark(color, (byte)50), color, 45.0), 
			StiBrushType.Solid => (object)new StiSolidBrush(color), 
			_ => (object)new StiSolidBrush(color), 
		});
	}

	public StiCustomStyleCoreXF(StiCustomStyle customStyle)
	{
		CustomStyle = customStyle;
	}
}
