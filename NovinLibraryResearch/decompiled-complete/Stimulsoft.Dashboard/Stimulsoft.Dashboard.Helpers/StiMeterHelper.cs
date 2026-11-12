using System;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Meters;
using Stimulsoft.Dashboard.Components;
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
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Functions;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Dashboard.Helpers;

internal static class StiMeterHelper
{
	internal static class Chart
	{
		internal static StiValueChartMeter GetValue(StiMeter meter, StiChartSeriesType? seriesType = null)
		{
			if (meter is StiValueChartMeter)
			{
				return meter as StiValueChartMeter;
			}
			return new StiValueChartMeter
			{
				Key = meter.Key,
				Expression = ToTotalExpression(meter),
				Label = meter.Label,
				SeriesType = (seriesType ?? StiChartSeriesType.ClusteredColumn)
			};
		}

		internal static StiEndValueChartMeter GetEndValue(StiMeter meter)
		{
			if (meter is StiEndValueChartMeter)
			{
				return meter as StiEndValueChartMeter;
			}
			return new StiEndValueChartMeter
			{
				Key = meter.Key,
				Expression = ToTotalExpression(meter),
				Label = meter.Label
			};
		}

		internal static StiOpenValueChartMeter GetOpenValue(StiMeter meter)
		{
			if (meter is StiOpenValueChartMeter)
			{
				return meter as StiOpenValueChartMeter;
			}
			return new StiOpenValueChartMeter
			{
				Key = meter.Key,
				Expression = ToTotalExpression(meter),
				Label = meter.Label
			};
		}

		internal static StiCloseValueChartMeter GetCloseValue(StiMeter meter)
		{
			if (meter is StiCloseValueChartMeter)
			{
				return meter as StiCloseValueChartMeter;
			}
			return new StiCloseValueChartMeter
			{
				Key = meter.Key,
				Expression = ToTotalExpression(meter),
				Label = meter.Label
			};
		}

		internal static StiLowValueChartMeter GetLowValue(StiMeter meter)
		{
			if (meter is StiLowValueChartMeter)
			{
				return meter as StiLowValueChartMeter;
			}
			return new StiLowValueChartMeter
			{
				Key = meter.Key,
				Expression = ToTotalExpression(meter),
				Label = meter.Label
			};
		}

		internal static StiHighValueChartMeter GetHighValue(StiMeter meter)
		{
			if (meter is StiHighValueChartMeter)
			{
				return meter as StiHighValueChartMeter;
			}
			return new StiHighValueChartMeter
			{
				Key = meter.Key,
				Expression = ToTotalExpression(meter),
				Label = meter.Label
			};
		}

		internal static StiValueChartMeter GetValue(IStiAppDataCell cell, StiChartElement element)
		{
			return new StiValueChartMeter
			{
				Expression = ToTotalExpression(cell),
				SeriesType = ((element.Values == null || element.Values.Count <= 0) ? StiChartSeriesType.ClusteredColumn : element.Values.FirstOrDefault().SeriesType),
				Label = ToAlias(cell)
			};
		}

		internal static StiValueChartMeter GetGanttStartValue(IStiAppDataCell cell)
		{
			return new StiValueChartMeter
			{
				Expression = ToExpression(cell),
				Label = ToAlias(cell),
				SeriesType = StiChartSeriesType.Gantt
			};
		}

		internal static StiEndValueChartMeter GetGanttEndValue(IStiAppDataCell cell)
		{
			return new StiEndValueChartMeter
			{
				Expression = ToExpression(cell),
				Label = ToAlias(cell)
			};
		}

		internal static StiEndValueChartMeter GetEndValue(IStiAppDataCell cell)
		{
			return new StiEndValueChartMeter
			{
				Expression = ToTotalExpression(cell),
				Label = ToAlias(cell)
			};
		}

		internal static StiCloseValueChartMeter GetCloseValue(IStiAppDataCell cell)
		{
			return new StiCloseValueChartMeter
			{
				Expression = ToTotalExpression(cell),
				Label = ToAlias(cell)
			};
		}

		internal static StiLowValueChartMeter GetLowValue(IStiAppDataCell cell)
		{
			return new StiLowValueChartMeter
			{
				Expression = ToTotalExpression(cell),
				Label = ToAlias(cell)
			};
		}

		internal static StiHighValueChartMeter GetHighValue(IStiAppDataCell cell)
		{
			return new StiHighValueChartMeter
			{
				Expression = ToTotalExpression(cell),
				Label = ToAlias(cell)
			};
		}

		internal static StiWeightChartMeter GetWeight(StiMeter meter)
		{
			if (meter is StiWeightChartMeter)
			{
				return meter as StiWeightChartMeter;
			}
			return new StiWeightChartMeter
			{
				Key = meter.Key,
				Expression = ToTotalExpression(meter),
				Label = meter.Label
			};
		}

		internal static StiWeightChartMeter GetWeight(IStiAppDataCell cell)
		{
			return new StiWeightChartMeter
			{
				Expression = ToTotalExpression(cell),
				Label = ToAlias(cell)
			};
		}

		internal static StiArgumentChartMeter GetArgument(StiMeter meter)
		{
			if (meter is StiArgumentChartMeter)
			{
				return meter as StiArgumentChartMeter;
			}
			return new StiArgumentChartMeter
			{
				Key = meter.Key,
				Expression = StiExpressionHelper.RemoveFunction(meter.Expression),
				Label = meter.Label
			};
		}

		internal static StiArgumentChartMeter GetArgument(IStiAppDataCell cell)
		{
			return new StiArgumentChartMeter
			{
				Expression = ToExpression(cell),
				Label = ToAlias(cell)
			};
		}

		internal static StiArgumentChartMeter GetX(IStiAppDataCell cell)
		{
			return new StiArgumentChartMeter
			{
				Expression = ToExpression(cell),
				Label = ToAlias(cell)
			};
		}

		internal static StiArgumentChartMeter GetX(StiMeter meter)
		{
			if (meter is StiArgumentChartMeter)
			{
				return meter as StiArgumentChartMeter;
			}
			return new StiArgumentChartMeter
			{
				Key = meter.Key,
				Expression = StiExpressionHelper.RemoveFunction(meter.Expression),
				Label = meter.Label
			};
		}

		internal static StiValueChartMeter GetY(IStiAppDataCell cell)
		{
			return new StiValueChartMeter
			{
				Expression = ToExpression(cell),
				Label = ToAlias(cell)
			};
		}

		internal static StiValueChartMeter GetY(StiMeter meter)
		{
			if (meter is StiValueChartMeter)
			{
				return meter as StiValueChartMeter;
			}
			return new StiValueChartMeter
			{
				Key = meter.Key,
				Expression = StiExpressionHelper.RemoveFunction(meter.Expression),
				Label = meter.Label
			};
		}

		internal static StiSeriesChartMeter GetSeries(StiMeter meter)
		{
			if (meter is StiSeriesChartMeter)
			{
				return meter as StiSeriesChartMeter;
			}
			return new StiSeriesChartMeter
			{
				Key = meter.Key,
				Expression = StiExpressionHelper.RemoveFunction(meter.Expression),
				Label = meter.Label
			};
		}

		internal static StiSeriesChartMeter GetSeries(IStiAppDataCell cell)
		{
			return new StiSeriesChartMeter
			{
				Expression = ToExpression(cell),
				Label = ToAlias(cell)
			};
		}
	}

	internal static class ComboBox
	{
		internal static StiNameComboBoxMeter GetName(StiMeter meter)
		{
			if (meter is StiNameComboBoxMeter)
			{
				return meter as StiNameComboBoxMeter;
			}
			return new StiNameComboBoxMeter
			{
				Key = meter.Key,
				Expression = meter.Expression,
				Label = meter.Label
			};
		}

		internal static StiNameComboBoxMeter GetName(IStiAppDataCell cell)
		{
			return new StiNameComboBoxMeter
			{
				Expression = ToExpression(cell),
				Label = ToAlias(cell)
			};
		}

		internal static StiKeyComboBoxMeter GetKey(StiMeter meter)
		{
			if (meter is StiKeyComboBoxMeter)
			{
				return meter as StiKeyComboBoxMeter;
			}
			return new StiKeyComboBoxMeter
			{
				Key = meter.Key,
				Expression = meter.Expression,
				Label = meter.Label
			};
		}

		internal static StiKeyComboBoxMeter GetKey(IStiAppDataCell cell)
		{
			return new StiKeyComboBoxMeter
			{
				Expression = ToExpression(cell),
				Label = ToAlias(cell)
			};
		}
	}

	internal static class DataFilter
	{
		internal static StiDataFilterRule GetFilter(StiDataColumn dataColumn)
		{
			string text = "";
			if (dataColumn?.Type != null)
			{
				if (dataColumn.Type.IsDateType())
				{
					text = DateTime.Now.ToString("MM'/'dd'/'yyyy");
				}
				else if (dataColumn.Type.IsNumericType())
				{
					text = "0";
				}
			}
			return new StiDataFilterRule
			{
				Key = StiKeyHelper.GenerateKey(),
				Condition = StiDataFilterCondition.EqualTo,
				Path = ToExpression(dataColumn),
				Value = text,
				Value2 = text
			};
		}

		internal static StiDataFilterRule GetFilter(StiTableColumn column, StiElement element)
		{
			IStiAppDataColumn dataColumnFromExpression = StiDataExpressionHelper.GetDataColumnFromExpression(element.Page as StiDashboard, column.Expression);
			string text = "";
			if (dataColumnFromExpression?.GetDataType() != null)
			{
				if (dataColumnFromExpression.GetDataType().IsDateType())
				{
					text = DateTime.Now.ToString("MM'/'dd'/'yyyy");
				}
				else if (dataColumnFromExpression.GetDataType().IsNumericType())
				{
					text = "0";
				}
			}
			return new StiDataFilterRule
			{
				Key = StiKeyHelper.GenerateKey(),
				Condition = StiDataFilterCondition.EqualTo,
				Path = StiExpressionHelper.RemoveFunction(column.Expression),
				Value = text,
				Value2 = text
			};
		}
	}

	internal static class DatePicker
	{
		internal static StiValueDatePickerMeter GetValue(StiMeter meter)
		{
			if (meter is StiValueDatePickerMeter)
			{
				return meter as StiValueDatePickerMeter;
			}
			return new StiValueDatePickerMeter
			{
				Key = meter.Key,
				Expression = meter.Expression,
				Label = meter.Label
			};
		}

		internal static StiValueDatePickerMeter GetValue(IStiAppDataCell cell)
		{
			return new StiValueDatePickerMeter
			{
				Expression = ToExpression(cell),
				Label = ToAlias(cell)
			};
		}
	}

	internal static class Gauge
	{
		internal static StiSeriesGaugeMeter GetSeries(StiMeter meter)
		{
			if (meter is StiSeriesGaugeMeter)
			{
				return meter as StiSeriesGaugeMeter;
			}
			return new StiSeriesGaugeMeter
			{
				Key = meter.Key,
				Expression = StiExpressionHelper.RemoveFunction(meter.Expression),
				Label = meter.Label
			};
		}

		internal static StiSeriesGaugeMeter GetSeries(IStiAppDataCell cell)
		{
			return new StiSeriesGaugeMeter
			{
				Expression = ToExpression(cell),
				Label = ToAlias(cell)
			};
		}

		internal static StiValueGaugeMeter GetValue(StiMeter meter)
		{
			if (meter is StiValueGaugeMeter)
			{
				return meter as StiValueGaugeMeter;
			}
			return new StiValueGaugeMeter
			{
				Key = meter.Key,
				Expression = ToTotalExpression(meter),
				Label = meter.Label
			};
		}

		internal static StiTargetGaugeMeter GetTarget(StiMeter meter)
		{
			if (meter is StiTargetGaugeMeter)
			{
				return meter as StiTargetGaugeMeter;
			}
			return new StiTargetGaugeMeter
			{
				Key = meter.Key,
				Expression = ToTotalExpression(meter),
				Label = meter.Label
			};
		}

		internal static StiValueGaugeMeter GetValue(IStiAppDataCell cell)
		{
			return new StiValueGaugeMeter
			{
				Expression = ToTotalExpression(cell),
				Label = ToAlias(cell)
			};
		}

		internal static StiTargetGaugeMeter GetTarget(IStiAppDataCell cell)
		{
			return new StiTargetGaugeMeter
			{
				Expression = ToTotalExpression(cell),
				Label = ToAlias(cell)
			};
		}
	}

	internal static class Indicator
	{
		internal static StiSeriesIndicatorMeter GetSeries(StiMeter meter)
		{
			if (meter is StiSeriesIndicatorMeter)
			{
				return meter as StiSeriesIndicatorMeter;
			}
			return new StiSeriesIndicatorMeter
			{
				Key = meter.Key,
				Expression = StiExpressionHelper.RemoveFunction(meter.Expression),
				Label = meter.Label
			};
		}

		internal static StiSeriesIndicatorMeter GetSeries(IStiAppDataCell cell)
		{
			return new StiSeriesIndicatorMeter
			{
				Expression = ToExpression(cell),
				Label = ToAlias(cell)
			};
		}

		internal static StiValueIndicatorMeter GetValue(StiMeter meter)
		{
			if (meter is StiValueIndicatorMeter)
			{
				return meter as StiValueIndicatorMeter;
			}
			return new StiValueIndicatorMeter
			{
				Key = meter.Key,
				Expression = ToTotalExpression(meter),
				Label = meter.Label
			};
		}

		internal static StiValueIndicatorMeter GetValue(IStiAppDataCell cell)
		{
			return new StiValueIndicatorMeter
			{
				Expression = ToTotalExpression(cell),
				Label = ToAlias(cell)
			};
		}

		internal static StiTargetIndicatorMeter GetTarget(StiMeter meter)
		{
			if (meter is StiTargetIndicatorMeter)
			{
				return meter as StiTargetIndicatorMeter;
			}
			return new StiTargetIndicatorMeter
			{
				Key = meter.Key,
				Expression = ToTotalExpression(meter),
				Label = meter.Label
			};
		}

		internal static StiTargetIndicatorMeter GetTarget(IStiAppDataCell cell)
		{
			return new StiTargetIndicatorMeter
			{
				Expression = ToTotalExpression(cell),
				Label = ToAlias(cell)
			};
		}
	}

	internal static class ListBox
	{
		internal static StiNameListBoxMeter GetName(StiMeter meter)
		{
			if (meter is StiNameListBoxMeter)
			{
				return meter as StiNameListBoxMeter;
			}
			return new StiNameListBoxMeter
			{
				Key = meter.Key,
				Expression = meter.Expression,
				Label = meter.Label
			};
		}

		internal static StiNameListBoxMeter GetName(IStiAppDataCell cell)
		{
			return new StiNameListBoxMeter
			{
				Expression = ToExpression(cell),
				Label = ToAlias(cell)
			};
		}

		internal static StiKeyListBoxMeter GetKey(StiMeter meter)
		{
			if (meter is StiKeyListBoxMeter)
			{
				return meter as StiKeyListBoxMeter;
			}
			return new StiKeyListBoxMeter
			{
				Key = meter.Key,
				Expression = meter.Expression,
				Label = meter.Label
			};
		}

		internal static StiKeyListBoxMeter GetKey(IStiAppDataCell cell)
		{
			return new StiKeyListBoxMeter
			{
				Expression = ToExpression(cell),
				Label = ToAlias(cell)
			};
		}
	}

	internal static class OnlineMap
	{
		internal static StiLatitudeMapMeter GetLatitude(StiMeter meter)
		{
			if (meter is StiLatitudeMapMeter)
			{
				return meter as StiLatitudeMapMeter;
			}
			return new StiLatitudeMapMeter
			{
				Key = meter.Key,
				Expression = meter.Expression,
				Label = meter.Label
			};
		}

		internal static StiLongitudeMapMeter GetLongitude(StiMeter meter)
		{
			if (meter is StiLongitudeMapMeter)
			{
				return meter as StiLongitudeMapMeter;
			}
			return new StiLongitudeMapMeter
			{
				Key = meter.Key,
				Expression = meter.Expression,
				Label = meter.Label
			};
		}

		internal static StiLocationMapMeter GetLocation(StiMeter meter)
		{
			if (meter is StiLocationMapMeter)
			{
				return meter as StiLocationMapMeter;
			}
			return new StiLocationMapMeter
			{
				Key = meter.Key,
				Expression = meter.Expression,
				Label = meter.Label
			};
		}

		internal static StiGisMapMeter GetGis(StiMeter meter)
		{
			if (meter is StiGisMapMeter)
			{
				return meter as StiGisMapMeter;
			}
			return new StiGisMapMeter
			{
				Key = meter.Key,
				Expression = meter.Expression,
				Label = meter.Label
			};
		}

		internal static StiGisMapColorMeter GetGisColor(StiMeter meter)
		{
			if (meter is StiGisMapColorMeter)
			{
				return meter as StiGisMapColorMeter;
			}
			return new StiGisMapColorMeter
			{
				Key = meter.Key,
				Expression = meter.Expression,
				Label = meter.Label
			};
		}

		internal static StiGisMapLineSizeMeter GetGisLineSize(StiMeter meter)
		{
			if (meter is StiGisMapLineSizeMeter)
			{
				return meter as StiGisMapLineSizeMeter;
			}
			return new StiGisMapLineSizeMeter
			{
				Key = meter.Key,
				Expression = meter.Expression,
				Label = meter.Label
			};
		}

		internal static StiGisMapDescriptionMeter GetGisDescription(StiMeter meter)
		{
			if (meter is StiGisMapDescriptionMeter)
			{
				return meter as StiGisMapDescriptionMeter;
			}
			return new StiGisMapDescriptionMeter
			{
				Key = meter.Key,
				Expression = meter.Expression,
				Label = meter.Label
			};
		}

		internal static StiLocationValueMapMeter GetLocationValue(StiMeter meter)
		{
			if (meter is StiLocationValueMapMeter)
			{
				return meter as StiLocationValueMapMeter;
			}
			return new StiLocationValueMapMeter
			{
				Key = meter.Key,
				Expression = meter.Expression,
				Label = meter.Label
			};
		}

		internal static StiLocationColorMapMeter GetLocationColorMeter(StiMeter meter)
		{
			if (meter is StiLocationColorMapMeter)
			{
				return meter as StiLocationColorMapMeter;
			}
			return new StiLocationColorMapMeter
			{
				Key = meter.Key,
				Expression = meter.Expression,
				Label = meter.Label
			};
		}

		internal static StiLocationArgumentMapMeter GetLocationArgument(StiMeter meter)
		{
			if (meter is StiLocationArgumentMapMeter)
			{
				return meter as StiLocationArgumentMapMeter;
			}
			return new StiLocationArgumentMapMeter
			{
				Key = meter.Key,
				Expression = meter.Expression,
				Label = meter.Label
			};
		}

		internal static StiLatitudeMapMeter GetLatitude(IStiAppDataCell cell)
		{
			return new StiLatitudeMapMeter
			{
				Expression = ToExpression(cell),
				Label = ToAlias(cell)
			};
		}

		internal static StiLongitudeMapMeter GetLongitude(IStiAppDataCell cell)
		{
			return new StiLongitudeMapMeter
			{
				Expression = ToExpression(cell),
				Label = ToAlias(cell)
			};
		}

		internal static StiLocationMapMeter GetLocation(IStiAppDataCell cell)
		{
			return new StiLocationMapMeter
			{
				Expression = ToExpression(cell),
				Label = ToAlias(cell)
			};
		}

		internal static StiGisMapMeter GetGis(IStiAppDataCell cell)
		{
			return new StiGisMapMeter
			{
				Expression = ToExpression(cell),
				Label = ToAlias(cell)
			};
		}

		internal static StiGisMapColorMeter GetGisColor(IStiAppDataCell cell)
		{
			return new StiGisMapColorMeter
			{
				Expression = ToExpression(cell),
				Label = ToAlias(cell)
			};
		}

		internal static StiGisMapLineSizeMeter GetGisLineSize(IStiAppDataCell cell)
		{
			return new StiGisMapLineSizeMeter
			{
				Expression = ToExpression(cell),
				Label = ToAlias(cell)
			};
		}

		internal static StiGisMapDescriptionMeter GetGisDescription(IStiAppDataCell cell)
		{
			return new StiGisMapDescriptionMeter
			{
				Expression = ToExpression(cell),
				Label = ToAlias(cell)
			};
		}

		internal static StiLocationValueMapMeter GetLocationValue(IStiAppDataCell cell)
		{
			return new StiLocationValueMapMeter
			{
				Expression = ToExpression(cell),
				Label = ToAlias(cell)
			};
		}

		internal static StiLocationColorMapMeter GetLocationColorMeter(IStiAppDataCell cell)
		{
			return new StiLocationColorMapMeter
			{
				Expression = ToExpression(cell),
				Label = ToAlias(cell)
			};
		}

		internal static StiLocationArgumentMapMeter GetLocationArgument(IStiAppDataCell cell)
		{
			return new StiLocationArgumentMapMeter
			{
				Expression = ToExpression(cell),
				Label = ToAlias(cell)
			};
		}
	}

	internal static class Pivot
	{
		internal static StiPivotColumn GetColumn(StiMeter meter)
		{
			if (meter is StiPivotColumn)
			{
				return meter as StiPivotColumn;
			}
			StiPivotColumn stiPivotColumn = new StiPivotColumn();
			stiPivotColumn.Key = meter.Key;
			stiPivotColumn.Expression = meter.Expression;
			stiPivotColumn.Label = meter.Label;
			stiPivotColumn.TextFormat = GetFormat(meter);
			stiPivotColumn.TopN = GetTopN(meter);
			stiPivotColumn.HorAlignment = (meter as IStiHorAlignment)?.HorAlignment ?? StiHorAlignment.Center;
			stiPivotColumn.Size = (meter as IStiTableColumnSize)?.Size ?? new StiTableColumnSize();
			stiPivotColumn.TotalLabel = GetTotalLabel(meter);
			stiPivotColumn.ShowTotal = GetShowTotal(meter);
			return stiPivotColumn;
		}

		internal static StiPivotRow GetRow(StiMeter meter)
		{
			if (meter is StiPivotRow)
			{
				return meter as StiPivotRow;
			}
			StiPivotRow stiPivotRow = new StiPivotRow();
			stiPivotRow.Key = meter.Key;
			stiPivotRow.Expression = meter.Expression;
			stiPivotRow.Label = meter.Label;
			stiPivotRow.TextFormat = GetFormat(meter);
			stiPivotRow.TopN = GetTopN(meter);
			stiPivotRow.HorAlignment = (meter as IStiHorAlignment)?.HorAlignment ?? StiHorAlignment.Center;
			stiPivotRow.Size = (meter as IStiTableColumnSize)?.Size ?? new StiTableColumnSize();
			stiPivotRow.TotalLabel = GetTotalLabel(meter);
			stiPivotRow.ShowTotal = GetShowTotal(meter);
			return stiPivotRow;
		}

		internal static bool GetShowTotal(StiMeter meter)
		{
			if (meter is IStiPivotRow)
			{
				return (meter as IStiPivotRow).ShowTotal;
			}
			if (meter is IStiPivotColumn)
			{
				return (meter as IStiPivotColumn).ShowTotal;
			}
			return true;
		}

		internal static string GetTotalLabel(StiMeter meter)
		{
			if (meter is IStiPivotRow)
			{
				return (meter as IStiPivotRow).TotalLabel;
			}
			if (meter is IStiPivotColumn)
			{
				return (meter as IStiPivotColumn).TotalLabel;
			}
			return "";
		}

		internal static StiPivotSummary GetSummary(StiMeter meter)
		{
			if (meter is StiPivotSummary)
			{
				return meter as StiPivotSummary;
			}
			return new StiPivotSummary
			{
				Key = meter.Key,
				Expression = ToTotalExpression(meter),
				Label = meter.Label,
				TextFormat = GetFormat(meter)
			};
		}

		internal static StiPivotSummary GetSummary(StiMeter meter, IStiDashboard dashboard)
		{
			if (meter is StiPivotSummary)
			{
				return meter as StiPivotSummary;
			}
			return new StiPivotSummary
			{
				Key = meter.Key,
				Expression = ToTotalExpression(meter, dashboard),
				Label = meter.Label,
				TextFormat = GetFormat(meter)
			};
		}

		internal static StiPivotColumn GetColumn(IStiAppDataCell cell)
		{
			return new StiPivotColumn
			{
				Expression = ToExpression(cell),
				Label = ToAlias(cell)
			};
		}

		internal static StiPivotRow GetRow(IStiAppDataCell cell)
		{
			return new StiPivotRow
			{
				Expression = ToExpression(cell),
				Label = ToAlias(cell)
			};
		}

		internal static StiPivotSummary GetSummary(IStiAppDataCell cell)
		{
			return new StiPivotSummary
			{
				Expression = ToTotalExpression(cell),
				Label = ToAlias(cell)
			};
		}

		internal static StiFormatService GetFormat(StiMeter meter)
		{
			if (meter is StiPivotColumn)
			{
				return (meter as StiPivotColumn).TextFormat;
			}
			if (meter is StiPivotRow)
			{
				return (meter as StiPivotRow).TextFormat;
			}
			if (meter is StiPivotSummary)
			{
				return (meter as StiPivotSummary).TextFormat;
			}
			return new StiGeneralFormatService();
		}

		internal static StiDataTopN GetTopN(StiMeter meter)
		{
			if (meter is StiPivotColumn)
			{
				return (meter as StiPivotColumn).TopN;
			}
			if (meter is StiPivotRow)
			{
				return (meter as StiPivotRow).TopN;
			}
			return new StiDataTopN();
		}
	}

	internal static class Progress
	{
		internal static StiSeriesProgressMeter GetSeries(StiMeter meter)
		{
			if (meter is StiSeriesProgressMeter)
			{
				return meter as StiSeriesProgressMeter;
			}
			return new StiSeriesProgressMeter
			{
				Key = meter.Key,
				Expression = StiExpressionHelper.RemoveFunction(meter.Expression),
				Label = meter.Label
			};
		}

		internal static StiSeriesProgressMeter GetSeries(IStiAppDataCell cell)
		{
			return new StiSeriesProgressMeter
			{
				Expression = ToExpression(cell),
				Label = ToAlias(cell)
			};
		}

		internal static StiValueProgressMeter GetValue(StiMeter meter)
		{
			if (meter is StiValueProgressMeter)
			{
				return meter as StiValueProgressMeter;
			}
			return new StiValueProgressMeter
			{
				Key = meter.Key,
				Expression = ToTotalExpression(meter),
				Label = meter.Label
			};
		}

		internal static StiValueProgressMeter GetValue(IStiAppDataCell cell)
		{
			return new StiValueProgressMeter
			{
				Expression = ToTotalExpression(cell),
				Label = ToAlias(cell)
			};
		}

		internal static StiTargetProgressMeter GetTarget(StiMeter meter)
		{
			if (meter is StiTargetProgressMeter)
			{
				return meter as StiTargetProgressMeter;
			}
			return new StiTargetProgressMeter
			{
				Key = meter.Key,
				Expression = ToTotalExpression(meter),
				Label = meter.Label
			};
		}

		internal static StiTargetProgressMeter GetTarget(IStiAppDataCell cell)
		{
			return new StiTargetProgressMeter
			{
				Expression = ToTotalExpression(cell),
				Label = ToAlias(cell)
			};
		}
	}

	internal static class RegionMap
	{
		internal static StiKeyMapMeter GetKey(StiMeter meter)
		{
			if (meter is StiKeyMapMeter)
			{
				return meter as StiKeyMapMeter;
			}
			return new StiKeyMapMeter
			{
				Key = meter.Key,
				Expression = meter.Expression,
				Label = meter.Label
			};
		}

		internal static StiNameMapMeter GetName(StiMeter meter)
		{
			if (meter is StiNameMapMeter)
			{
				return meter as StiNameMapMeter;
			}
			return new StiNameMapMeter
			{
				Key = meter.Key,
				Expression = meter.Expression,
				Label = meter.Label
			};
		}

		internal static StiGroupMapMeter GetGroup(StiMeter meter)
		{
			if (meter is StiGroupMapMeter)
			{
				return meter as StiGroupMapMeter;
			}
			return new StiGroupMapMeter
			{
				Key = meter.Key,
				Expression = meter.Expression,
				Label = meter.Label
			};
		}

		internal static StiValueMapMeter GetValue(StiMeter meter, IStiDashboard dashboard)
		{
			StiValueMapMeter stiValueMapMeter = new StiValueMapMeter();
			stiValueMapMeter.LoadFromJsonObject(meter.SaveToJsonObject(StiJsonSaveMode.Report));
			string newFunction = (StiDataExpressionHelper.IsNumericDataColumnInExpression(dashboard, stiValueMapMeter.Expression) ? "Sum" : "Count");
			stiValueMapMeter.Expression = StiExpressionHelper.ReplaceFunction(stiValueMapMeter.Expression, newFunction);
			return stiValueMapMeter;
		}

		internal static StiColorMapMeter GetColor(StiMeter meter)
		{
			if (meter is StiColorMapMeter)
			{
				return meter as StiColorMapMeter;
			}
			return new StiColorMapMeter
			{
				Key = meter.Key,
				Expression = meter.Expression,
				Label = meter.Label
			};
		}

		internal static StiKeyMapMeter GetKey(IStiAppDataCell cell)
		{
			return new StiKeyMapMeter
			{
				Expression = ToExpression(cell),
				Label = ToAlias(cell)
			};
		}

		internal static StiNameMapMeter GetName(IStiAppDataCell cell)
		{
			return new StiNameMapMeter
			{
				Expression = ToExpression(cell),
				Label = ToAlias(cell)
			};
		}

		internal static StiValueMapMeter GetValue(IStiAppDataCell cell)
		{
			return new StiValueMapMeter
			{
				Expression = ToTotalExpression(cell),
				Label = ToAlias(cell)
			};
		}

		internal static StiGroupMapMeter GetGroup(IStiAppDataCell cell)
		{
			return new StiGroupMapMeter
			{
				Expression = ToExpression(cell),
				Label = ToAlias(cell)
			};
		}

		internal static StiColorMapMeter GetColor(IStiAppDataCell cell)
		{
			return new StiColorMapMeter
			{
				Expression = ToExpression(cell),
				Label = ToAlias(cell)
			};
		}
	}

	internal static class Table
	{
		internal static StiTableColumn GetColumn(StiMeter meter)
		{
			StiTableColumn stiTableColumn = meter as StiTableColumn;
			if (stiTableColumn == null)
			{
				stiTableColumn = ((meter is IStiMeasureMeter) ? ((StiTableColumn)new StiMeasureColumn()) : ((StiTableColumn)new StiDimensionColumn()));
			}
			stiTableColumn.LoadFromJsonObject(meter.SaveToJsonObject(StiJsonSaveMode.Report));
			return stiTableColumn;
		}

		internal static StiDimensionColumn GetDimension(StiTableColumn tableColumn)
		{
			StiDimensionColumn stiDimensionColumn = new StiDimensionColumn();
			stiDimensionColumn.LoadFromJsonObject(tableColumn.SaveToJsonObject(StiJsonSaveMode.Report));
			if (Funcs.IsMeasureFunction(stiDimensionColumn.Expression))
			{
				stiDimensionColumn.Expression = StiExpressionHelper.RemoveFunction(stiDimensionColumn.Expression);
			}
			return stiDimensionColumn;
		}

		internal static StiDimensionColumn GetDimension(IStiAppDataCell cell)
		{
			return new StiDimensionColumn
			{
				TextFormat = StiTextFormatHelper.GetDefaultFormatForColumn(cell),
				Expression = ToExpression(cell),
				HorAlignment = StiTableAlignmentHelper.GetHorAlign(cell),
				Label = ToAlias(cell)
			};
		}

		internal static StiMeasureColumn GetMeasure(StiTableColumn tableColumn, IStiDashboard dashboard)
		{
			StiMeasureColumn stiMeasureColumn = new StiMeasureColumn();
			stiMeasureColumn.LoadFromJsonObject(tableColumn.SaveToJsonObject(StiJsonSaveMode.Report));
			if (tableColumn is StiDimensionColumn || tableColumn is StiSparklinesColumn)
			{
				string newFunction = (StiDataExpressionHelper.IsNumericDataColumnInExpression(dashboard, stiMeasureColumn.Expression) ? "Sum" : "Count");
				stiMeasureColumn.Expression = StiExpressionHelper.ReplaceFunction(stiMeasureColumn.Expression, newFunction);
			}
			return stiMeasureColumn;
		}

		internal static StiMeasureColumn GetMeasure(IStiAppDataCell cell)
		{
			return new StiMeasureColumn
			{
				TextFormat = StiTextFormatHelper.GetDefaultFormatForColumn(cell),
				Expression = ToTotalExpression(cell),
				HorAlignment = StiTableAlignmentHelper.GetHorAlign(cell),
				Label = ToAlias(cell)
			};
		}

		internal static StiDataBarsColumn GetDataBars(StiTableColumn tableColumn, IStiDashboard dashboard)
		{
			StiDataBarsColumn stiDataBarsColumn = new StiDataBarsColumn();
			stiDataBarsColumn.LoadFromJsonObject(tableColumn.SaveToJsonObject(StiJsonSaveMode.Report));
			if (tableColumn is StiDimensionColumn || tableColumn is StiSparklinesColumn)
			{
				string newFunction = (StiDataExpressionHelper.IsNumericDataColumnInExpression(dashboard, stiDataBarsColumn.Expression) ? "Sum" : "First");
				stiDataBarsColumn.Expression = StiExpressionHelper.ReplaceFunction(stiDataBarsColumn.Expression, newFunction);
			}
			return stiDataBarsColumn;
		}

		internal static StiColorScaleColumn GetColorScale(StiTableColumn tableColumn, IStiDashboard dashboard)
		{
			StiColorScaleColumn stiColorScaleColumn = new StiColorScaleColumn();
			stiColorScaleColumn.LoadFromJsonObject(tableColumn.SaveToJsonObject(StiJsonSaveMode.Report));
			if (tableColumn is StiDimensionColumn || tableColumn is StiSparklinesColumn)
			{
				string newFunction = (StiDataExpressionHelper.IsNumericDataColumnInExpression(dashboard, stiColorScaleColumn.Expression) ? "Sum" : "Count");
				stiColorScaleColumn.Expression = StiExpressionHelper.ReplaceFunction(stiColorScaleColumn.Expression, newFunction);
			}
			return stiColorScaleColumn;
		}

		internal static StiSparklinesColumn GetSparklines(StiTableColumn tableColumn)
		{
			StiSparklinesColumn stiSparklinesColumn = new StiSparklinesColumn();
			stiSparklinesColumn.LoadFromJsonObject(tableColumn.SaveToJsonObject(StiJsonSaveMode.Report));
			if (Funcs.IsMeasureFunction(stiSparklinesColumn.Expression))
			{
				stiSparklinesColumn.Expression = StiExpressionHelper.RemoveFunction(stiSparklinesColumn.Expression);
			}
			return stiSparklinesColumn;
		}

		internal static StiBubbleColumn GetBubble(StiTableColumn tableColumn, IStiDashboard dashboard)
		{
			StiBubbleColumn stiBubbleColumn = new StiBubbleColumn();
			stiBubbleColumn.LoadFromJsonObject(tableColumn.SaveToJsonObject(StiJsonSaveMode.Report));
			if (tableColumn is StiDimensionColumn || tableColumn is StiSparklinesColumn)
			{
				string newFunction = (StiDataExpressionHelper.IsNumericDataColumnInExpression(dashboard, stiBubbleColumn.Expression) ? "Sum" : "Count");
				stiBubbleColumn.Expression = StiExpressionHelper.ReplaceFunction(stiBubbleColumn.Expression, newFunction);
			}
			return stiBubbleColumn;
		}

		internal static StiIndicatorColumn GetIndicator(StiTableColumn tableColumn, IStiDashboard dashboard)
		{
			StiIndicatorColumn stiIndicatorColumn = new StiIndicatorColumn();
			stiIndicatorColumn.LoadFromJsonObject(tableColumn.SaveToJsonObject(StiJsonSaveMode.Report));
			if (tableColumn is StiDimensionColumn || tableColumn is StiSparklinesColumn)
			{
				string newFunction = (StiDataExpressionHelper.IsNumericDataColumnInExpression(dashboard, stiIndicatorColumn.Expression) ? "Sum" : "Count");
				stiIndicatorColumn.Expression = StiExpressionHelper.ReplaceFunction(stiIndicatorColumn.Expression, newFunction);
			}
			stiIndicatorColumn.HorAlignment = StiHorAlignment.Right;
			return stiIndicatorColumn;
		}
	}

	internal static class Cards
	{
		internal static StiCardsColumn GetColumn(StiMeter meter)
		{
			StiCardsColumn stiCardsColumn = meter as StiCardsColumn;
			if (stiCardsColumn == null)
			{
				stiCardsColumn = ((meter is IStiMeasureMeter) ? ((StiCardsColumn)new StiMeasureCardsColumn()) : ((StiCardsColumn)new StiDimensionCardsColumn()));
			}
			stiCardsColumn.LoadFromJsonObject(meter.SaveToJsonObject(StiJsonSaveMode.Report));
			return stiCardsColumn;
		}

		internal static StiDimensionCardsColumn GetDimension(IStiAppDataCell cell)
		{
			return new StiDimensionCardsColumn
			{
				TextFormat = StiTextFormatHelper.GetDefaultFormatForColumn(cell),
				Expression = ToExpression(cell),
				HorAlignment = StiTableAlignmentHelper.GetHorAlign(cell),
				Label = ToAlias(cell)
			};
		}

		internal static StiDimensionCardsColumn GetDimension(StiCardsColumn cardsColumn)
		{
			StiDimensionCardsColumn stiDimensionCardsColumn = new StiDimensionCardsColumn();
			stiDimensionCardsColumn.LoadFromJsonObject(cardsColumn.SaveToJsonObject(StiJsonSaveMode.Report));
			if (Funcs.IsMeasureFunction(stiDimensionCardsColumn.Expression))
			{
				stiDimensionCardsColumn.Expression = StiExpressionHelper.RemoveFunction(stiDimensionCardsColumn.Expression);
			}
			return stiDimensionCardsColumn;
		}

		internal static StiMeasureCardsColumn GetMeasure(StiCardsColumn cardsColumn, IStiDashboard dashboard)
		{
			StiMeasureCardsColumn stiMeasureCardsColumn = new StiMeasureCardsColumn();
			stiMeasureCardsColumn.LoadFromJsonObject(cardsColumn.SaveToJsonObject(StiJsonSaveMode.Report));
			if (cardsColumn is StiDimensionCardsColumn || cardsColumn is StiSparklinesCardsColumn)
			{
				string newFunction = (StiDataExpressionHelper.IsNumericDataColumnInExpression(dashboard, stiMeasureCardsColumn.Expression) ? "Sum" : "Count");
				stiMeasureCardsColumn.Expression = StiExpressionHelper.ReplaceFunction(stiMeasureCardsColumn.Expression, newFunction);
			}
			return stiMeasureCardsColumn;
		}

		internal static StiMeasureCardsColumn GetMeasure(IStiAppDataCell cell)
		{
			return new StiMeasureCardsColumn
			{
				TextFormat = StiTextFormatHelper.GetDefaultFormatForColumn(cell),
				Expression = ToTotalExpression(cell),
				HorAlignment = StiTableAlignmentHelper.GetHorAlign(cell),
				Label = ToAlias(cell)
			};
		}

		internal static StiDataBarsCardsColumn GetDataBars(StiCardsColumn cardsColumn, IStiDashboard dashboard)
		{
			StiDataBarsCardsColumn stiDataBarsCardsColumn = new StiDataBarsCardsColumn();
			stiDataBarsCardsColumn.LoadFromJsonObject(cardsColumn.SaveToJsonObject(StiJsonSaveMode.Report));
			if (cardsColumn is StiDimensionCardsColumn || cardsColumn is StiSparklinesCardsColumn)
			{
				string newFunction = (StiDataExpressionHelper.IsNumericDataColumnInExpression(dashboard, stiDataBarsCardsColumn.Expression) ? "Sum" : "First");
				stiDataBarsCardsColumn.Expression = StiExpressionHelper.ReplaceFunction(stiDataBarsCardsColumn.Expression, newFunction);
			}
			return stiDataBarsCardsColumn;
		}

		internal static StiColorScaleCardsColumn GetColorScale(StiCardsColumn cardsColumn, IStiDashboard dashboard)
		{
			StiColorScaleCardsColumn stiColorScaleCardsColumn = new StiColorScaleCardsColumn();
			stiColorScaleCardsColumn.LoadFromJsonObject(cardsColumn.SaveToJsonObject(StiJsonSaveMode.Report));
			if (cardsColumn is StiDimensionCardsColumn || cardsColumn is StiSparklinesCardsColumn)
			{
				string newFunction = (StiDataExpressionHelper.IsNumericDataColumnInExpression(dashboard, stiColorScaleCardsColumn.Expression) ? "Sum" : "Count");
				stiColorScaleCardsColumn.Expression = StiExpressionHelper.ReplaceFunction(stiColorScaleCardsColumn.Expression, newFunction);
			}
			return stiColorScaleCardsColumn;
		}

		internal static StiSparklinesCardsColumn GetSparklines(StiCardsColumn cardsColumn)
		{
			StiSparklinesCardsColumn stiSparklinesCardsColumn = new StiSparklinesCardsColumn();
			stiSparklinesCardsColumn.LoadFromJsonObject(cardsColumn.SaveToJsonObject(StiJsonSaveMode.Report));
			if (Funcs.IsMeasureFunction(stiSparklinesCardsColumn.Expression))
			{
				stiSparklinesCardsColumn.Expression = StiExpressionHelper.RemoveFunction(stiSparklinesCardsColumn.Expression);
			}
			return stiSparklinesCardsColumn;
		}

		internal static StiBubbleCardsColumn GetBubble(StiCardsColumn cardsColumn, IStiDashboard dashboard)
		{
			StiBubbleCardsColumn stiBubbleCardsColumn = new StiBubbleCardsColumn();
			stiBubbleCardsColumn.LoadFromJsonObject(cardsColumn.SaveToJsonObject(StiJsonSaveMode.Report));
			if (cardsColumn is StiDimensionCardsColumn || cardsColumn is StiSparklinesCardsColumn)
			{
				string newFunction = (StiDataExpressionHelper.IsNumericDataColumnInExpression(dashboard, stiBubbleCardsColumn.Expression) ? "Sum" : "Count");
				stiBubbleCardsColumn.Expression = StiExpressionHelper.ReplaceFunction(stiBubbleCardsColumn.Expression, newFunction);
			}
			return stiBubbleCardsColumn;
		}

		internal static StiIndicatorCardsColumn GetIndicator(StiCardsColumn cardsColumn, IStiDashboard dashboard)
		{
			StiIndicatorCardsColumn stiIndicatorCardsColumn = new StiIndicatorCardsColumn();
			stiIndicatorCardsColumn.LoadFromJsonObject(cardsColumn.SaveToJsonObject(StiJsonSaveMode.Report));
			if (cardsColumn is StiDimensionCardsColumn || cardsColumn is StiSparklinesCardsColumn)
			{
				string newFunction = (StiDataExpressionHelper.IsNumericDataColumnInExpression(dashboard, stiIndicatorCardsColumn.Expression) ? "Sum" : "Count");
				stiIndicatorCardsColumn.Expression = StiExpressionHelper.ReplaceFunction(stiIndicatorCardsColumn.Expression, newFunction);
			}
			stiIndicatorCardsColumn.HorAlignment = StiHorAlignment.Right;
			return stiIndicatorCardsColumn;
		}
	}

	internal static class TreeView
	{
		internal static StiKeyTreeViewMeter GetKey(StiMeter meter)
		{
			if (meter is StiKeyTreeViewMeter)
			{
				return meter as StiKeyTreeViewMeter;
			}
			return new StiKeyTreeViewMeter
			{
				Key = meter.Key,
				Expression = meter.Expression,
				Label = meter.Label
			};
		}

		internal static StiKeyTreeViewMeter GetKey(IStiAppDataCell cell)
		{
			return new StiKeyTreeViewMeter
			{
				Expression = ToExpression(cell),
				Label = ToAlias(cell)
			};
		}
	}

	internal static class TreeViewBox
	{
		internal static StiKeyTreeViewBoxMeter GetKey(StiMeter meter)
		{
			if (meter is StiKeyTreeViewBoxMeter)
			{
				return meter as StiKeyTreeViewBoxMeter;
			}
			return new StiKeyTreeViewBoxMeter
			{
				Key = meter.Key,
				Expression = meter.Expression,
				Label = meter.Label
			};
		}

		internal static StiKeyTreeViewBoxMeter GetKey(IStiAppDataCell cell)
		{
			return new StiKeyTreeViewBoxMeter
			{
				Expression = ToExpression(cell),
				Label = ToAlias(cell)
			};
		}
	}

	private static string ToTotalExpression(IStiAppDataCell cell)
	{
		if (cell is StiDataColumn stiDataColumn)
		{
			if (!stiDataColumn.IsNumericType())
			{
				return ToCountExpression(stiDataColumn);
			}
			return ToSumExpression(stiDataColumn);
		}
		return cell?.GetName();
	}

	private static string ToTotalExpression(StiMeter meter)
	{
		if (StiExpressionHelper.IsFunctionPresent(meter.Expression))
		{
			return meter.Expression;
		}
		return ToCountExpression(meter.Expression);
	}

	private static string ToTotalExpression(StiMeter meter, IStiDashboard dashboard)
	{
		if (StiExpressionHelper.IsFunctionPresent(meter.Expression))
		{
			return meter.Expression;
		}
		if (!StiDataExpressionHelper.IsNumericDataColumnInExpression(dashboard, meter.Expression))
		{
			return ToCountExpression(meter.Expression);
		}
		return ToSumExpression(meter.Expression);
	}

	private static string ToSumExpression(StiDataColumn dataColumn)
	{
		return "Sum(" + ToExpression(dataColumn) + ")";
	}

	private static string ToSumExpression(string expression)
	{
		return "Sum(" + expression + ")";
	}

	private static string ToCountExpression(StiDataColumn dataColumn)
	{
		return "Count(" + ToExpression(dataColumn) + ")";
	}

	private static string ToCountExpression(string expression)
	{
		return "Count(" + expression + ")";
	}

	private static string ToExpression(IStiAppDataCell cell)
	{
		if (cell is StiDataColumn stiDataColumn)
		{
			return Funcs.ToExpression(stiDataColumn.DataSource.Name, stiDataColumn.Name);
		}
		return Funcs.ToExpression(cell.GetName());
	}

	public static string ToAlias(IStiAppDataCell cell)
	{
		if (cell is IStiAppAlias stiAppAlias)
		{
			return stiAppAlias.GetAlias();
		}
		return cell?.GetName();
	}

	public static Type ToDataType(IStiAppDataCell cell)
	{
		Type type = (cell as StiDataColumn)?.Type;
		if (type != null)
		{
			return type;
		}
		return (cell as StiVariable)?.Type;
	}
}
