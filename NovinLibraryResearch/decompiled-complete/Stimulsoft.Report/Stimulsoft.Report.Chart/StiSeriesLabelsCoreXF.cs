using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components.TextFormats;

namespace Stimulsoft.Report.Chart;

public abstract class StiSeriesLabelsCoreXF : ICloneable, IStiApplyStyle
{
	private IStiSeriesLabels seriesLabels;

	[Browsable(false)]
	public abstract int Position { get; }

	[Browsable(false)]
	public abstract StiSeriesLabelsType SeriesLabelsType { get; }

	public IStiSeriesLabels SeriesLabels
	{
		get
		{
			return seriesLabels;
		}
		set
		{
			seriesLabels = value;
		}
	}

	public abstract string LocalizedName { get; }

	public object Clone()
	{
		return MemberwiseClone();
	}

	public virtual void ApplyStyle(IStiChartStyle style)
	{
		if (SeriesLabels.AllowApplyStyle)
		{
			SeriesLabels.LabelColor = style.Core.SeriesLabelsColor;
			SeriesLabels.BorderColor = style.Core.SeriesLabelsBorderColor;
			SeriesLabels.Brush = style.Core.SeriesLabelsBrush;
			SeriesLabels.Font = style.Core.SeriesLabelsFont;
		}
	}

	public StiBrush ProcessSeriesColors(int pointIndex, StiBrush brush, IStiSeries series)
	{
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		StiBrush result = brush;
		if (SeriesLabels == null)
		{
			return result;
		}
		foreach (StiChartCondition seriesLabelsCondition in SeriesLabels.Chart.SeriesLabelsConditions)
		{
			if (series.Values == null || series.Values.Length <= pointIndex)
			{
				continue;
			}
			object itemValue = series.Values[pointIndex];
			object itemValueEnd = null;
			if (series is IStiRangeSeries && pointIndex < ((IStiRangeSeries)series).ValuesEnd.Length)
			{
				itemValueEnd = ((IStiRangeSeries)series).ValuesEnd[pointIndex];
			}
			object itemArgument = null;
			if (series.Arguments != null && pointIndex < series.Arguments.Length)
			{
				itemArgument = series.Arguments[pointIndex];
			}
			object itemTooltip = null;
			if (series.ToolTips != null && pointIndex < series.ToolTips.Length)
			{
				itemTooltip = series.ToolTips[pointIndex];
			}
			object itemValueOpen = null;
			object itemValueClose = null;
			object itemValueLow = null;
			object itemValueHigh = null;
			if (series is IStiFinancialSeries)
			{
				if (pointIndex < ((IStiFinancialSeries)series).ValuesOpen.Length)
				{
					itemValueOpen = ((IStiFinancialSeries)series).ValuesOpen[pointIndex];
				}
				if (pointIndex < ((IStiFinancialSeries)series).ValuesClose.Length)
				{
					itemValueClose = ((IStiFinancialSeries)series).ValuesClose[pointIndex];
				}
				if (pointIndex < ((IStiFinancialSeries)series).ValuesLow.Length)
				{
					itemValueLow = ((IStiFinancialSeries)series).ValuesLow[pointIndex];
				}
				if (pointIndex < ((IStiFinancialSeries)series).ValuesHigh.Length)
				{
					itemValueHigh = ((IStiFinancialSeries)series).ValuesHigh[pointIndex];
				}
			}
			object filterData = StiChartHelper.GetFilterData(null, seriesLabelsCondition, null);
			if (!StiChartHelper.GetFilterResult(seriesLabelsCondition, itemArgument, itemValue, itemValueEnd, itemValueOpen, itemValueClose, itemValueLow, itemValueHigh, itemTooltip, filterData))
			{
				continue;
			}
			result = SeriesLabels.Chart.Style.Core.GetColumnBrush(seriesLabelsCondition.Color);
			if (SeriesLabels.Chart.Area is IStiClusteredBarArea)
			{
				if (result is StiGradientBrush)
				{
					StiGradientBrush val = (StiGradientBrush)result;
					val.Angle += 90.0;
				}
				if (result is StiGlareBrush)
				{
					StiGlareBrush val2 = (StiGlareBrush)result;
					val2.Angle += 90.0;
				}
			}
			return result;
		}
		return result;
	}

	internal Color GetSeriesLabelColor(IStiSeries series, int colorIndex, int colorCount)
	{
		Color color = (Color)series.Core.GetSeriesBorderColor(colorIndex, colorCount);
		if (color == Color.Transparent)
		{
			color = StiBrush.ToColor(series.Core.GetSeriesBrush(colorIndex, colorCount));
		}
		return color;
	}

	protected Color GetBorderColor(IStiSeries series, int colorIndex, int colorCount)
	{
		if (SeriesLabels.UseSeriesColor)
		{
			return GetSeriesLabelColor(series, colorIndex, colorCount);
		}
		return SeriesLabels.BorderColor;
	}

	protected Color GetLabelColor(IStiSeries series, int colorIndex, int colorCount)
	{
		if (SeriesLabels.UseSeriesColor)
		{
			return GetSeriesLabelColor(series, colorIndex, colorCount);
		}
		return SeriesLabels.LabelColor;
	}

	internal virtual double RecalcValue(double value, int signs)
	{
		return value;
	}

	internal string GetLabelText(IStiSeries series, double? value, string argument, string tag, string seriesName)
	{
		return GetLabelText(series, value, argument, tag, seriesName, useLegendValueType: false);
	}

	internal string GetLabelText(IStiSeries series, double? value, string argument, string tag, string seriesName, bool useLegendValueType)
	{
		return GetLabelText(series, value, argument, tag, seriesName, 0.0, useLegendValueType);
	}

	internal string GetLabelText(IStiSeries series, double? value, string argument, string tag, string seriesName, double weight, bool useLegendValueType)
	{
		string text = null;
		StiSeriesLabelsValueType stiSeriesLabelsValueType = SeriesLabels.ValueType;
		if (useLegendValueType)
		{
			stiSeriesLabelsValueType = SeriesLabels.LegendValueType;
		}
		switch (stiSeriesLabelsValueType)
		{
		case StiSeriesLabelsValueType.Argument:
			text = GetArgument(series, argument);
			break;
		case StiSeriesLabelsValueType.Value:
			text = GetFormattedValue(series, value);
			break;
		case StiSeriesLabelsValueType.SeriesTitle:
			text = seriesName;
			break;
		case StiSeriesLabelsValueType.Tag:
			text = tag;
			break;
		case StiSeriesLabelsValueType.Weight:
			text = weight.ToString();
			break;
		case StiSeriesLabelsValueType.ValueArgument:
		{
			string argument3 = GetArgument(series, argument);
			text = (string.IsNullOrEmpty(argument3) ? GetFormattedValue(series, value) : $"{GetFormattedValue(series, value)} {SeriesLabels.ValueTypeSeparator} {argument3}");
			break;
		}
		case StiSeriesLabelsValueType.ArgumentValue:
		{
			string formattedValue2 = GetFormattedValue(series, value);
			text = (string.IsNullOrEmpty(formattedValue2) ? GetArgument(series, argument) : $"{GetArgument(series, argument)} {SeriesLabels.ValueTypeSeparator} {formattedValue2}");
			break;
		}
		case StiSeriesLabelsValueType.SeriesTitleArgument:
		{
			string argument2 = GetArgument(series, argument);
			text = (string.IsNullOrEmpty(argument2) ? seriesName : $"{seriesName} {SeriesLabels.ValueTypeSeparator} {argument2}");
			break;
		}
		case StiSeriesLabelsValueType.SeriesTitleValue:
		{
			string formattedValue = GetFormattedValue(series, value);
			text = (string.IsNullOrEmpty(formattedValue) ? seriesName : $"{seriesName} {SeriesLabels.ValueTypeSeparator} {formattedValue}");
			break;
		}
		}
		if (useLegendValueType)
		{
			return text;
		}
		return $"{SeriesLabels.TextBefore}{text}{SeriesLabels.TextAfter}";
	}

	private string GetArgument(IStiSeries series, string argument)
	{
		string result = argument;
		if (series.Core.IsDateTimeArguments && DateTime.TryParse(argument, out var result2))
		{
			result = GetFormatted(series, result2.ToOADate(), isDateTime: true);
		}
		return result;
	}

	private string GetFormatted(IStiSeries series, double? value, bool isDateTime)
	{
		CultureInfo cultureInfo = null;
		try
		{
			object obj = value;
			if (isDateTime)
			{
				obj = DateTime.FromOADate(value.Value);
			}
			StiPieSeriesLabels stiPieSeriesLabels = SeriesLabels as StiPieSeriesLabels;
			StiAxisSeriesLabels stiAxisSeriesLabels = SeriesLabels as StiAxisSeriesLabels;
			if (SeriesLabels.FormatService != null && !(SeriesLabels.FormatService is StiGeneralFormatService) && (stiPieSeriesLabels == null || !stiPieSeriesLabels.ShowInPercent) && (stiAxisSeriesLabels == null || !stiAxisSeriesLabels.ShowInPercent))
			{
				return SeriesLabels.FormatService.Format(obj);
			}
			string name = ((StiChart)SeriesLabels.Chart)?.Report?.Culture;
			if (!string.IsNullOrEmpty(((StiChart)SeriesLabels.Chart)?.Report?.Culture))
			{
				cultureInfo = Thread.CurrentThread.CurrentCulture;
				Thread.CurrentThread.CurrentCulture = new CultureInfo(name);
			}
			string text = null;
			if (series != null)
			{
				text = series.Format;
			}
			if (text == null || text.Trim().Length == 0)
			{
				text = SeriesLabels.Format;
			}
			if (text != null && text.Trim().Length != 0)
			{
				if (text.StartsWith("P", StringComparison.InvariantCulture) && SeriesLabels.Chart != null && !StiChartOptions.OldChartPercentMode)
				{
					int signs = 0;
					double num;
					try
					{
						if (text.Length > 1)
						{
							signs = Convert.ToInt32(text.Remove(0, 1));
						}
						num = RecalcValue(value.Value, signs);
					}
					catch
					{
						num = RecalcValue(value.Value, 2);
					}
					return string.Format("{0:N" + signs + "}{1}", num, "%");
				}
				string text2 = string.Format("{0:" + text + "}", obj);
				if (!isDateTime && text2 == text)
				{
					return obj.ToString();
				}
				return text2;
			}
		}
		catch
		{
		}
		finally
		{
			if (cultureInfo != null)
			{
				Thread.CurrentThread.CurrentCulture = cultureInfo;
			}
		}
		return RecalcValue(value.Value, 2).ToString();
	}

	protected virtual string GetFormattedValue(IStiSeries series, double? value)
	{
		if (!value.HasValue)
		{
			return string.Empty;
		}
		return GetFormatted(series, value, series.Core.IsDateTimeValues);
	}

	protected internal StiStringFormatGeom GetStringFormatGeom(StiContext context)
	{
		StiStringFormatGeom genericStringFormat = context.GetGenericStringFormat();
		genericStringFormat.Trimming = (StringTrimming)0;
		if (!SeriesLabels.WordWrap)
		{
			genericStringFormat.FormatFlags = (StringFormatFlags)6144;
		}
		return genericStringFormat;
	}

	public StiSeriesLabelsCoreXF(IStiSeriesLabels seriesLabels)
	{
		this.seriesLabels = seriesLabels;
	}
}
