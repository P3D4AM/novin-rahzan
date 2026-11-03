using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Threading;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiChartTableCore : ICloneable, IStiApplyStyle
{
	internal const float WidthSpace = 2f;

	public IStiChartTable ChartTable { get; set; }

	public StiChartTableCore(IStiChartTable table)
	{
		ChartTable = table;
	}

	public virtual void ApplyStyle(IStiChartStyle style)
	{
		if (ChartTable.AllowApplyStyle)
		{
			ChartTable.GridLineColor = style.Core.LegendBorderColor;
			ChartTable.TextColor = style.Core.LegendLabelsColor;
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public bool ShowTable()
	{
		if ((ChartTable.Chart.Area is IStiClusteredColumnArea || ChartTable.Chart.Area is IStiStackedColumnArea) && !(ChartTable.Chart.Area is IStiCandlestickArea) && !(ChartTable.Chart.Area is IStiRangeArea) && !(ChartTable.Chart.Area is IStiScatterArea) && !(ChartTable.Chart.Area is IStiStockArea) && ChartTable.Visible)
		{
			return true;
		}
		return false;
	}

	public float GetHeightTable(StiContext context, float widthTable)
	{
		StiFontGeom font = StiFontGeom.ChangeFontSize(ChartTable.DataCells.Font, ChartTable.DataCells.Font.Size * context.Options.Zoom);
		return context.MeasureString("HeightText", font).Height * (float)ChartTable.Chart.Series.Count + GetHeightHeaderTable(context, widthTable);
	}

	public float GetHeightHeaderTable(StiContext context, float widthTable)
	{
		float num = 0f;
		StiAxisArea stiAxisArea = ChartTable.Chart.Area as StiAxisArea;
		bool startFromZero = stiAxisArea.XAxis.StartFromZero;
		List<string> arguments = GetArguments();
		StiFontGeom font = StiFontGeom.ChangeFontSize(ChartTable.Header.Font, ChartTable.Header.Font.Size * context.Options.Zoom);
		StiStringFormatGeom genericStringFormat = context.GetGenericStringFormat();
		genericStringFormat.Trimming = (StringTrimming)0;
		if (!ChartTable.Header.WordWrap)
		{
			genericStringFormat.FormatFlags = (StringFormatFlags)6144;
		}
		genericStringFormat.Alignment = (StringAlignment)1;
		genericStringFormat.LineAlignment = (StringAlignment)1;
		int num2 = (stiAxisArea.XAxis.StartFromZero ? (arguments.Count + 1) : (arguments.Count - 1));
		float num3 = (widthTable - GetWidthCellLegend(context)) / (float)num2;
		for (int i = 0; i < arguments.Count; i++)
		{
			float num4 = 0f;
			if (i == 0 || i == arguments.Count - 1)
			{
				num4 = (startFromZero ? (num3 / 2f) : ((0f - num3) / 2f));
			}
			float num5 = num3 + num4 - 4f * context.Options.Zoom;
			if (num5 < 0f)
			{
				num5 = 0f;
			}
			float height = context.MeasureRotatedString(arguments[i], font, new PointF(0f, 0f), genericStringFormat, (StiRotationMode)4, 0f, (int)num5).Height;
			num = Math.Max(num, height);
		}
		return num;
	}

	public float GetWidthCellLegend(StiContext context)
	{
		StiFontGeom font = StiFontGeom.ChangeFontSize(ChartTable.DataCells.Font, ChartTable.DataCells.Font.Size * context.Options.Zoom);
		StiStringFormatGeom genericStringFormat = context.GetGenericStringFormat();
		genericStringFormat.FormatFlags = (StringFormatFlags)6144;
		genericStringFormat.Alignment = (StringAlignment)1;
		genericStringFormat.LineAlignment = (StringAlignment)1;
		float num = 0f;
		RectangleF rectangleF = default(RectangleF);
		foreach (StiSeries item in ChartTable.Chart.Series)
		{
			rectangleF = context.MeasureRotatedString(item.CoreTitle, font, new PointF(0f, 0f), genericStringFormat, (StiRotationMode)4, 0f);
			num = Math.Max(rectangleF.Width, num);
		}
		if (ChartTable.MarkerVisible)
		{
			num += rectangleF.Height;
		}
		return (float)Math.Ceiling(num);
	}

	public StiCellGeom Render(StiContext context, RectangleF rect)
	{
		return new StiChartTableGeom(rect, GetTableValues(), GetWidthCellLegend(context), GetHeightHeaderTable(context, rect.Width), 2f * context.Options.Zoom, ChartTable);
	}

	private int GetMaxCountValues(StiSeriesCollection series)
	{
		int num = 0;
		foreach (StiSeries item in series)
		{
			num = Math.Max(num, item.Values.Length);
		}
		return num;
	}

	private List<string> GetArguments()
	{
		StiSeriesCollection series = ChartTable.Chart.Series;
		List<string> list = new List<string>();
		int maxCountValues = GetMaxCountValues(series);
		for (int i = 0; i < maxCountValues; i++)
		{
			string objectValue = ((series[0].Arguments.Length > i) ? series[0].Arguments[i].ToString() : (i + 1).ToString());
			string labelText = GetLabelText(objectValue, series[0]);
			list.Add(labelText);
		}
		return list;
	}

	internal string GetLabelText(object objectValue, IStiSeries series)
	{
		CultureInfo cultureInfo = null;
		try
		{
			string text = ((StiChart)ChartTable.Chart)?.Report?.Culture;
			if (!string.IsNullOrEmpty(((StiChart)ChartTable.Chart)?.Report?.Culture))
			{
				cultureInfo = Thread.CurrentThread.CurrentCulture;
				Thread.CurrentThread.CurrentCulture = new CultureInfo(text);
			}
			string text2 = null;
			if (series != null)
			{
				text2 = series.Format;
			}
			if (text2 == null || text2.Trim().Length == 0)
			{
				text2 = ChartTable.Header.Format;
			}
			if (text2 != null && text2.Trim().Length != 0)
			{
				if (objectValue is string)
				{
					string s = objectValue.ToString().Replace(".", ",").Replace(",", Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
					if (decimal.TryParse(s, out var result))
					{
						objectValue = result;
					}
					else
					{
						string name = (string.IsNullOrEmpty(text) ? "en-US" : text);
						if (DateTime.TryParse(objectValue.ToString(), new CultureInfo(name, useUserOverride: true), DateTimeStyles.None, out var result2))
						{
							objectValue = result2;
						}
						else if (DateTime.TryParse(objectValue.ToString(), out result2))
						{
							objectValue = result2;
						}
					}
				}
				else if (objectValue == null)
				{
					return string.Empty;
				}
				if (!StiChartOptions.OldChartPercentMode && text2.StartsWith("P", StringComparison.InvariantCulture))
				{
					int result3 = 0;
					if (text2.Length > 1)
					{
						int.TryParse(text2.Remove(0, 1), out result3);
					}
					return string.Format("{0:N" + result3 + "}{1}{2}", objectValue, "%", ChartTable.Header.TextAfter);
				}
				StiErrorFormatEntity stiErrorFormatEntity = default(StiErrorFormatEntity);
				stiErrorFormatEntity.Format = text2;
				stiErrorFormatEntity.Value = objectValue;
				StiErrorFormatEntity item = stiErrorFormatEntity;
				try
				{
					if (!StiErrorFormatEntity.Hash.Contains(item))
					{
						return string.Format("{0:" + text2 + "}{1}", objectValue, ChartTable.Header.TextAfter);
					}
				}
				catch
				{
					StiErrorFormatEntity.Hash.Add(item);
				}
			}
			return $"{objectValue}{ChartTable.Header.TextAfter}";
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
		return objectValue.ToString();
	}

	private string[,] GetTableValues()
	{
		StiSeriesCollection series = ChartTable.Chart.Series;
		List<string> arguments = GetArguments();
		string[,] array = new string[series.Count + 1, arguments.Count + 1];
		for (int i = 0; i < arguments.Count; i++)
		{
			array[0, i + 1] = arguments[i];
		}
		StiAxisArea stiAxisArea = ChartTable.Chart.Area as StiAxisArea;
		int num = 1;
		foreach (StiSeries item in series)
		{
			array[num, 0] = item.CoreTitle;
			int num2 = 1;
			for (int j = 0; j < item.Values.Length; j++)
			{
				double? num3 = (stiAxisArea.ReverseHor ? item.Values[item.Values.Length - j - 1] : item.Values[j]);
				string format = ChartTable.Chart.Table.Format;
				if (format != null && format.Trim().Length != 0 && num3.HasValue)
				{
					if (format.StartsWith("P", StringComparison.InvariantCulture) && !StiChartOptions.OldChartPercentMode)
					{
						int signs = 0;
						double num4;
						try
						{
							if (format.Length > 1)
							{
								signs = Convert.ToInt32(format.Remove(0, 1));
							}
							num4 = item.SeriesLabels.Core.RecalcValue(num3.Value, signs);
						}
						catch
						{
							num4 = item.SeriesLabels.Core.RecalcValue(num3.Value, 2);
						}
						array[num, num2] = string.Format("{0:N" + signs + "}{1}", num4, "%");
					}
					else
					{
						object arg = num3;
						if (item.Core.IsDateTimeValues)
						{
							arg = DateTime.FromOADate(num3.Value);
						}
						array[num, num2] = string.Format("{0:" + format + "}", arg);
					}
				}
				else
				{
					array[num, num2] = num3.ToString();
				}
				num2++;
			}
			num++;
		}
		return array;
	}
}
