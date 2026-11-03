namespace Stimulsoft.Report.Chart;

public class StiMarkerLegendFactory
{
	public static IStiLegendMarker CreateMarker(IStiSeries series)
	{
		if (series is IStiFontIconsSeries && ((IStiFontIconsSeries)series).Icon.HasValue)
		{
			return new StiLegendFontIconMarker();
		}
		if (series is IStiDoughnutSeries)
		{
			return new StiLegendDoughnutMarker();
		}
		if (series is IStiPieSeries)
		{
			return new StiLegendPieMarker();
		}
		if (series is StiAreaSeries)
		{
			return new StiLegendAreaMarker();
		}
		if (series is StiStackedAreaSeries)
		{
			return new StiLegendStackedAreaMarker();
		}
		if (series is StiSplineAreaSeries)
		{
			return new StiLegendSplineAreaMarker();
		}
		if (series is StiStackedSplineAreaSeries)
		{
			return new StiLegendStackedSplineAreaMarker();
		}
		if (series is StiSteppedAreaSeries)
		{
			return new StiLegendSteppedAreaMarker();
		}
		if (series is StiRangeSeries)
		{
			return new StiLegendRangeMarker();
		}
		if (series is StiSplineRangeSeries)
		{
			return new StiLegendSplineRangeMarker();
		}
		if (series is StiSteppedRangeSeries)
		{
			return new StiLegendSteppedRangeMarker();
		}
		if (series is IStiBaseLineSeries || series is IStiStackedBaseLineSeries || series is IStiRadarSeries)
		{
			return new StiLegendLineMarker();
		}
		if (series is IStiFunnelSeries)
		{
			return new StiLegendFunnelMarker();
		}
		if (series is IStiStockSeries)
		{
			return new StiLegendStockMarker();
		}
		if (series is IStiCandlestickSeries)
		{
			return new StiLegendCandelstickMarker();
		}
		if (series is IStiPictorialSeries)
		{
			return new StiLegendPictorialMarker();
		}
		return new StiLegendColumnMarker();
	}
}
