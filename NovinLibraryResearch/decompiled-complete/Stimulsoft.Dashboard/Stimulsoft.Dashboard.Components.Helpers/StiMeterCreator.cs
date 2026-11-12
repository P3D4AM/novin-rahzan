using System;
using Stimulsoft.Dashboard.Components.Cards;
using Stimulsoft.Dashboard.Components.Chart;
using Stimulsoft.Dashboard.Components.ComboBox;
using Stimulsoft.Dashboard.Components.DatePicker;
using Stimulsoft.Dashboard.Components.Gauge;
using Stimulsoft.Dashboard.Components.Indicator;
using Stimulsoft.Dashboard.Components.ListBox;
using Stimulsoft.Dashboard.Components.OnlineMap;
using Stimulsoft.Dashboard.Components.PivotTable;
using Stimulsoft.Dashboard.Components.Progress;
using Stimulsoft.Dashboard.Components.RegionMap;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Dashboard.Components.TreeView;
using Stimulsoft.Dashboard.Components.TreeViewBox;
using Stimulsoft.Data.Exceptions;

namespace Stimulsoft.Dashboard.Components.Helpers;

internal static class StiMeterCreator
{
	public static StiMeter New(string identName)
	{
		if (string.IsNullOrEmpty(identName))
		{
			return null;
		}
		if (!Enum.TryParse<StiMeterIdent>(identName, ignoreCase: true, out var result))
		{
			return null;
		}
		return New(result);
	}

	public static StiMeter New(StiMeterIdent ident)
	{
		return ident switch
		{
			StiMeterIdent.ArgumentChartMeter => new StiArgumentChartMeter(), 
			StiMeterIdent.SeriesChartMeter => new StiSeriesChartMeter(), 
			StiMeterIdent.ValueChartMeter => new StiValueChartMeter(), 
			StiMeterIdent.StartValueChartMeter => new StiStartValueChartMeter(), 
			StiMeterIdent.EndValueChartMeter => new StiEndValueChartMeter(), 
			StiMeterIdent.OpenValueChartMeter => new StiOpenValueChartMeter(), 
			StiMeterIdent.CloseValueChartMeter => new StiCloseValueChartMeter(), 
			StiMeterIdent.LowValueChartMeter => new StiLowValueChartMeter(), 
			StiMeterIdent.HighValueChartMeter => new StiHighValueChartMeter(), 
			StiMeterIdent.WeightChartMeter => new StiWeightChartMeter(), 
			StiMeterIdent.XChartMeter => new StiXChartMeter(), 
			StiMeterIdent.YChartMeter => new StiYChartMeter(), 
			StiMeterIdent.KeyComboBoxMeter => new StiKeyComboBoxMeter(), 
			StiMeterIdent.NameComboBoxMeter => new StiNameComboBoxMeter(), 
			StiMeterIdent.MaxGaugeMeter => new StiMaxGaugeMeter(), 
			StiMeterIdent.MinGaugeMeter => new StiMinGaugeMeter(), 
			StiMeterIdent.SeriesGaugeMeter => new StiSeriesGaugeMeter(), 
			StiMeterIdent.ValueGaugeMeter => new StiValueGaugeMeter(), 
			StiMeterIdent.TargetGaugeMeter => new StiTargetGaugeMeter(), 
			StiMeterIdent.SeriesIndicatorMeter => new StiSeriesIndicatorMeter(), 
			StiMeterIdent.TargetIndicatorMeter => new StiTargetIndicatorMeter(), 
			StiMeterIdent.ValueIndicatorMeter => new StiValueIndicatorMeter(), 
			StiMeterIdent.KeyListBoxMeter => new StiKeyListBoxMeter(), 
			StiMeterIdent.NameListBoxMeter => new StiNameListBoxMeter(), 
			StiMeterIdent.LatitudeMapMeter => new StiLatitudeMapMeter(), 
			StiMeterIdent.LongitudeMapMeter => new StiLongitudeMapMeter(), 
			StiMeterIdent.LocationMapMeter => new StiLocationMapMeter(), 
			StiMeterIdent.LocationValueMapMeter => new StiLocationValueMapMeter(), 
			StiMeterIdent.GisMapMeter => new StiGisMapMeter(), 
			StiMeterIdent.LocationColorMapMeter => new StiLocationColorMapMeter(), 
			StiMeterIdent.LocationArgumentMapMeter => new StiLocationArgumentMapMeter(), 
			StiMeterIdent.GisMapColorMeter => new StiGisMapColorMeter(), 
			StiMeterIdent.GisMapLineSizeMeter => new StiGisMapLineSizeMeter(), 
			StiMeterIdent.GisMapDescriptionMeter => new StiGisMapDescriptionMeter(), 
			StiMeterIdent.KeyMapMeter => new StiKeyMapMeter(), 
			StiMeterIdent.NameMapMeter => new StiNameMapMeter(), 
			StiMeterIdent.GroupMapMeter => new StiNameMapMeter(), 
			StiMeterIdent.ColorMapMeter => new StiColorMapMeter(), 
			StiMeterIdent.ValueMapMeter => new StiValueMapMeter(), 
			StiMeterIdent.PivotColumn => new StiPivotColumn(), 
			StiMeterIdent.PivotRow => new StiPivotRow(), 
			StiMeterIdent.PivotSummary => new StiPivotSummary(), 
			StiMeterIdent.SeriesProgressMeter => new StiSeriesProgressMeter(), 
			StiMeterIdent.TargetProgressMeter => new StiTargetProgressMeter(), 
			StiMeterIdent.ValueProgressMeter => new StiValueProgressMeter(), 
			StiMeterIdent.ColorScaleColumn => new StiColorScaleColumn(), 
			StiMeterIdent.DataBarsColumn => new StiDataBarsColumn(), 
			StiMeterIdent.DimensionColumn => new StiDimensionColumn(), 
			StiMeterIdent.IndicatorColumn => new StiIndicatorColumn(), 
			StiMeterIdent.MeasureColumn => new StiMeasureColumn(), 
			StiMeterIdent.SparklinesColumn => new StiSparklinesColumn(), 
			StiMeterIdent.BubbleColumn => new StiBubbleColumn(), 
			StiMeterIdent.ColorScaleCardsColumn => new StiColorScaleCardsColumn(), 
			StiMeterIdent.DataBarsCardsColumn => new StiDataBarsCardsColumn(), 
			StiMeterIdent.DimensionCardsColumn => new StiDimensionCardsColumn(), 
			StiMeterIdent.IndicatorCardsColumn => new StiIndicatorCardsColumn(), 
			StiMeterIdent.MeasureCardsColumn => new StiMeasureCardsColumn(), 
			StiMeterIdent.SparklinesCardsColumn => new StiSparklinesCardsColumn(), 
			StiMeterIdent.BubbleCardsColumn => new StiBubbleCardsColumn(), 
			StiMeterIdent.KeyTreeViewMeter => new StiKeyTreeViewMeter(), 
			StiMeterIdent.KeyTreeViewBoxMeter => new StiKeyTreeViewBoxMeter(), 
			StiMeterIdent.ValueDatePickerMeter => new StiValueDatePickerMeter(), 
			_ => throw new StiTypeNotRecognizedException(ident), 
		};
	}
}
