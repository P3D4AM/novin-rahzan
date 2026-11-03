using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiLegendCoreXF : ICloneable, IStiApplyStyle
{
	private IStiLegend legend;

	public IStiLegend Legend
	{
		get
		{
			return legend;
		}
		set
		{
			legend = value;
		}
	}

	public virtual void ApplyStyle(IStiChartStyle style)
	{
		if (Legend.AllowApplyStyle)
		{
			IStiLegend stiLegend = Legend;
			object obj = style.Core.LegendBrush.Clone();
			stiLegend.Brush = (StiBrush)((obj is StiBrush) ? obj : null);
			Legend.LabelsColor = style.Core.LegendLabelsColor;
			Legend.TitleColor = style.Core.LegendTitleColor;
			Legend.BorderColor = style.Core.LegendBorderColor;
			Legend.ShowShadow = style.Core.LegendShowShadow;
			Legend.Font = style.Core.LegendFont;
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public StiCellGeom Render(StiContext context, RectangleF rect)
	{
		int count;
		List<StiLegendItemCoreXF> legendItems = GetLegendItems(out count);
		if (!legend.Visible || legendItems.Count == 0)
		{
			return null;
		}
		int countColumns = 0;
		int countRows = 0;
		SizeF legendSize = GetLegendSize(context, rect, out countColumns, out countRows);
		RectangleF rectangleF = new RectangleF(rect.X, rect.Y, legendSize.Width, legendSize.Height);
		RectangleF rectangleF2 = rectangleF;
		rectangleF2.Location = new PointF(0f, 0f);
		StiLegendTitleGeom stiLegendTitleGeom = null;
		if (!string.IsNullOrEmpty(legend.Title))
		{
			SizeF titleSize = GetTitleSize(context);
			RectangleF clientRectangle = rectangleF;
			clientRectangle.Location = new PointF(0f, 0f);
			clientRectangle.Height = titleSize.Height;
			stiLegendTitleGeom = new StiLegendTitleGeom(legend, clientRectangle);
			rectangleF2.Y += clientRectangle.Height;
			rectangleF2.Height -= clientRectangle.Height;
		}
		StiLegendGeom stiLegendGeom = new StiLegendGeom(legend, rectangleF, legendItems);
		if (stiLegendTitleGeom != null)
		{
			stiLegendGeom.CreateChildGeoms();
			stiLegendGeom.ChildGeoms.Add(stiLegendTitleGeom);
			stiLegendGeom.LegendTitleGeom = stiLegendTitleGeom;
		}
		float num = (float)legend.HorSpacing * context.Options.Zoom;
		float num2 = (float)legend.VertSpacing * context.Options.Zoom;
		if (rectangleF.Width > 0f && rectangleF.Height > 0f)
		{
			if (legend.Columns == 0)
			{
				float num3 = num + rectangleF2.X;
				float num4 = num2 + rectangleF2.Y;
				float num5 = num3;
				float num6 = num4;
				float num7 = 0f;
				float num8 = 0f;
				int[,] matrixIndexItem = GetMatrixIndexItem(countColumns, countRows, legendItems.Count);
				for (int i = 0; i < matrixIndexItem.GetLength(0); i++)
				{
					for (int j = 0; j < matrixIndexItem.GetLength(1); j++)
					{
						int num9 = matrixIndexItem[i, j];
						if (num9 >= 0)
						{
							StiLegendItemCoreXF stiLegendItemCoreXF = legendItems[num9];
							SizeF itemRealSize = GetItemRealSize(context, stiLegendItemCoreXF);
							num7 = Math.Max(num7, itemRealSize.Width);
							num8 = Math.Max(num8, itemRealSize.Height);
							StiLegendItemGeom item = new StiLegendItemGeom(clientRectangle: new RectangleF(num5, num6, itemRealSize.Width, itemRealSize.Height), legend: legend, item: stiLegendItemCoreXF, colorIndex: stiLegendItemCoreXF.ColorIndex, legendItemsCount: count, legendItemIndex: stiLegendItemCoreXF.Index);
							stiLegendGeom.CreateChildGeoms();
							stiLegendGeom.ChildGeoms.Add(item);
						}
						num6 += num8 + num2;
					}
					num6 = num4;
					num5 += num7 + num;
					num7 = 0f;
				}
			}
			else
			{
				float num10 = 0f;
				float num11 = 0f;
				SizeF itemSize = GetItemSize(context, legendItems, 0);
				if (legend.Direction == StiLegendDirection.LeftToRight || legend.Direction == StiLegendDirection.TopToBottom)
				{
					num10 = num + rectangleF2.X;
					num11 = num2 + rectangleF2.Y;
				}
				else if (legend.Direction == StiLegendDirection.RightToLeft)
				{
					num10 = rectangleF2.Right - num - itemSize.Width;
					num11 = num2 + rectangleF2.Y;
				}
				else if (legend.Direction == StiLegendDirection.BottomToTop)
				{
					num10 = num + rectangleF2.X;
					num11 = rectangleF2.Bottom - num2 - itemSize.Height;
				}
				float num12 = num10;
				float num13 = num11;
				int num14 = 0;
				int num15 = 0;
				foreach (StiLegendItemCoreXF item3 in legendItems)
				{
					itemSize = GetItemSize(context, legendItems, item3);
					RectangleF clientRectangle3 = new RectangleF(num12, num13, itemSize.Width, itemSize.Height);
					StiLegendItemGeom item2 = new StiLegendItemGeom(legend, item3, clientRectangle3, item3.ColorIndex, count, item3.Index);
					stiLegendGeom.CreateChildGeoms();
					stiLegendGeom.ChildGeoms.Add(item2);
					num14++;
					if (legend.Direction == StiLegendDirection.TopToBottom)
					{
						num13 += num2 + clientRectangle3.Height;
						if (num14 == countColumns)
						{
							num13 = num11;
							num12 += itemSize.Width + num;
							num14 = 0;
						}
					}
					else if (legend.Direction == StiLegendDirection.LeftToRight)
					{
						num12 += num + clientRectangle3.Width;
						if (num14 == countColumns)
						{
							num12 = num10;
							num13 += itemSize.Height + num2;
							num14 = 0;
						}
					}
					else if (legend.Direction == StiLegendDirection.RightToLeft)
					{
						itemSize = GetItemSize(context, legendItems, num15 + 1);
						num12 -= num + itemSize.Width;
						if (num14 == countColumns)
						{
							num12 = num10;
							num13 += itemSize.Height + num2;
							num14 = 0;
						}
					}
					else if (legend.Direction == StiLegendDirection.BottomToTop)
					{
						num13 -= num2 + clientRectangle3.Height;
						if (num14 == countColumns)
						{
							num13 = num11;
							num12 += itemSize.Width + num;
							num14 = 0;
						}
					}
					num15++;
				}
			}
		}
		return stiLegendGeom;
	}

	private int[,] GetMatrixIndexItem(int countColumns, int countRows, int countItems)
	{
		int[,] array = new int[countColumns, countRows];
		if (legend.Direction == StiLegendDirection.TopToBottom)
		{
			int num = 0;
			for (int i = 0; i < countRows; i++)
			{
				for (int j = 0; j < countColumns; j++)
				{
					array[j, i] = ((num < countItems) ? num : (-1));
					num++;
				}
			}
		}
		else if (legend.Direction == StiLegendDirection.BottomToTop)
		{
			int num2 = 0;
			for (int num3 = countRows - 1; num3 >= 0; num3--)
			{
				for (int k = 0; k < countColumns; k++)
				{
					array[k, num3] = ((num2 < countItems) ? num2 : (-1));
					num2++;
				}
			}
		}
		else if (legend.Direction == StiLegendDirection.LeftToRight)
		{
			int num4 = 0;
			for (int l = 0; l < countColumns; l++)
			{
				for (int m = 0; m < countRows; m++)
				{
					array[l, m] = ((num4 < countItems) ? num4 : (-1));
					num4++;
				}
			}
		}
		else if (legend.Direction == StiLegendDirection.RightToLeft)
		{
			int num5 = 0;
			for (int num6 = countColumns - 1; num6 >= 0; num6--)
			{
				for (int n = 0; n < countRows; n++)
				{
					array[num6, n] = ((num5 < countItems) ? num5 : (-1));
					num5++;
				}
			}
		}
		return array;
	}

	internal string GetArgumentText(IStiSeries series, int index)
	{
		if (series.Arguments.Length > index && series.Arguments[index] != null)
		{
			return series.Arguments[index].ToString();
		}
		return string.Empty;
	}

	internal int GetLegendItemColumn(List<StiLegendItemCoreXF> seriesItems, StiLegendItemCoreXF seriesItem)
	{
		int num = 0;
		int num2 = 0;
		foreach (StiLegendItemCoreXF seriesItem2 in seriesItems)
		{
			if (seriesItem2 == seriesItem)
			{
				break;
			}
			num++;
			if (num >= legend.Columns)
			{
				num = 0;
				num2++;
			}
		}
		if (legend.Direction == StiLegendDirection.TopToBottom || legend.Direction == StiLegendDirection.BottomToTop)
		{
			return num2;
		}
		return num;
	}

	internal SizeF GetTitleSize(StiContext context)
	{
		if (string.IsNullOrEmpty(legend.Title))
		{
			return new SizeF(0f, 0f);
		}
		StiFontGeom font = StiFontGeom.ChangeFontSize(legend.TitleFont, legend.TitleFont.Size * context.Options.Zoom);
		StiStringFormatGeom defaultStringFormat = context.GetDefaultStringFormat();
		defaultStringFormat.FormatFlags = (StringFormatFlags)0;
		SizeF result = context.MeasureString(legend.Title, font, 1000000000, defaultStringFormat);
		result.Width++;
		return result;
	}

	internal SizeF GetItemSize(StiContext context, List<StiLegendItemCoreXF> seriesItems, int seriesIndex)
	{
		if (seriesIndex >= 0 && seriesIndex < seriesItems.Count)
		{
			StiLegendItemCoreXF seriesItem = seriesItems[seriesIndex];
			return GetItemSize(context, seriesItems, seriesItem);
		}
		return default(SizeF);
	}

	internal SizeF GetItemRealSize(StiContext context, StiLegendItemCoreXF seriesItem)
	{
		StiFontGeom font = StiFontGeom.ChangeFontSize(legend.Font, legend.Font.Size * context.Options.Zoom);
		SizeF sizeF = context.MeasureString(seriesItem.Text, font);
		float num = sizeF.Width;
		float num2 = sizeF.Height;
		if (legend.MarkerVisible)
		{
			num += (float)legend.MarkerSize.Width * context.Options.Zoom;
			num2 = Math.Max((float)legend.MarkerSize.Height * context.Options.Zoom, num2);
		}
		return new SizeF(num, num2);
	}

	internal SizeF GetItemSize(StiContext context, List<StiLegendItemCoreXF> seriesItems, StiLegendItemCoreXF seriesItem)
	{
		float num = 0f;
		float num2 = 0f;
		int legendItemColumn = GetLegendItemColumn(seriesItems, seriesItem);
		StiFontGeom font = StiFontGeom.ChangeFontSize(legend.Font, legend.Font.Size * context.Options.Zoom);
		foreach (StiLegendItemCoreXF seriesItem2 in seriesItems)
		{
			if (GetLegendItemColumn(seriesItems, seriesItem2) == legendItemColumn)
			{
				SizeF sizeF = context.MeasureString(seriesItem2.Text, font);
				num = Math.Max(sizeF.Width, num);
				num2 = Math.Max(sizeF.Height, num2);
			}
		}
		if (legend.MarkerVisible)
		{
			num = Math.Max((float)legend.MarkerSize.Width * context.Options.Zoom, num) + (float)legend.MarkerSize.Width * context.Options.Zoom;
			num2 = Math.Max((float)legend.MarkerSize.Height * context.Options.Zoom, num2);
		}
		return new SizeF(num, num2);
	}

	private SizeF GetItemsAutoSize(StiContext context, List<StiLegendItemCoreXF> seriesItems, RectangleF rect, out int countColumns, out int countRows)
	{
		bool flag = legend.Direction == StiLegendDirection.TopToBottom || legend.Direction == StiLegendDirection.BottomToTop;
		float num = (float)legend.HorSpacing * context.Options.Zoom;
		float num2 = (float)legend.VertSpacing * context.Options.Zoom;
		StiFontGeom font = StiFontGeom.ChangeFontSize(legend.Font, legend.Font.Size * context.Options.Zoom);
		int num3 = 0;
		int num4 = 0;
		List<float> list = new List<float>();
		List<float> list2 = new List<float>();
		float num5 = 0f;
		int? num6 = null;
		for (int i = 0; i < seriesItems.Count; i++)
		{
			StiLegendItemCoreXF stiLegendItemCoreXF = seriesItems[i];
			SizeF sizeF = context.MeasureString(stiLegendItemCoreXF.Text, font);
			float num7 = sizeF.Width + num;
			float num8 = sizeF.Height + num2;
			if (legend.MarkerVisible)
			{
				num7 += (float)legend.MarkerSize.Width * context.Options.Zoom;
				num8 = Math.Max((float)legend.MarkerSize.Height * context.Options.Zoom + num2, num8);
			}
			num5 = Math.Max(num8, num5);
			if (flag)
			{
				if ((list.Count - 1 < num3 && !num6.HasValue) || (list.Count - 1 < num3 && num6.HasValue && num3 <= num6))
				{
					list.Add(num7);
				}
				else if (list[num3] < num7)
				{
					list[num3] = num7;
					list.RemoveRange(num3 + 1, list.Count() - (num3 + 1));
					i = -1;
					num3 = 0;
					num4 = 0;
					continue;
				}
				num3++;
				if (num6.HasValue && num6 <= num3)
				{
					num3 = 0;
					num4++;
				}
				if (list.Sum() + num > rect.Width && list.Count > 1)
				{
					num3 = 0;
					num4++;
					if (i > 0)
					{
						i--;
					}
					if (list.Count > 0)
					{
						list.RemoveAt(list.Count - 1);
						num6 = list.Count();
					}
				}
				continue;
			}
			if (list2.Count - 1 < num4)
			{
				list2.Add(num8);
			}
			num4++;
			if (list.Count - 1 < num3)
			{
				list.Add(num7);
			}
			else
			{
				list[num3] = Math.Max(list[num3], num7);
			}
			if (list2.Sum() + num2 > rect.Height && list2.Count() > 1)
			{
				num3++;
				num4 = 0;
				if (i > 0)
				{
					i--;
				}
				if (list2.Count > 0)
				{
					list2.RemoveAt(list2.Count - 1);
				}
			}
		}
		countColumns = list.Count;
		if (flag)
		{
			countRows = (int)Math.Ceiling((float)seriesItems.Count / (float)countColumns);
		}
		else
		{
			countRows = list2.Count();
		}
		return new SizeF(list.Sum() + num, (float)countRows * num5 + num2);
	}

	internal SizeF GetItemsSize(StiContext context, List<StiLegendItemCoreXF> seriesItems)
	{
		float num = (float)legend.HorSpacing * context.Options.Zoom;
		float num2 = 0f;
		float num3 = 0f;
		int num4 = legend.Columns;
		if (legend.Direction == StiLegendDirection.TopToBottom || legend.Direction == StiLegendDirection.BottomToTop)
		{
			num4 = (seriesItems.Count - 1) / legend.Columns + 1;
		}
		for (int i = 0; i < num4; i++)
		{
			foreach (StiLegendItemCoreXF seriesItem in seriesItems)
			{
				if (GetLegendItemColumn(seriesItems, seriesItem) == i)
				{
					SizeF itemSize = GetItemSize(context, seriesItems, seriesItem);
					num2 += itemSize.Width + num;
					num3 = Math.Max(itemSize.Height, num3);
					break;
				}
			}
		}
		return new SizeF(num2, num3);
	}

	internal SizeF GetSeriesSize(StiContext context, RectangleF rect, out int countColumns, out int countRows)
	{
		float num = (float)legend.HorSpacing * context.Options.Zoom;
		float num2 = (float)legend.VertSpacing * context.Options.Zoom;
		List<IStiSeries> series = legend.Chart.Area.Core.GetSeries();
		int count;
		List<StiLegendItemCoreXF> legendItems = GetLegendItems(out count);
		count = legendItems.Count;
		float width = 0f;
		float height = 0f;
		if (legend.Columns == 0)
		{
			SizeF itemsAutoSize = GetItemsAutoSize(context, legendItems, rect, out countColumns, out countRows);
			width = itemsAutoSize.Width;
			height = itemsAutoSize.Height;
		}
		else
		{
			countRows = 0;
			countColumns = legend.Columns;
			SizeF itemsSize = GetItemsSize(context, legendItems);
			switch (legend.Direction)
			{
			case StiLegendDirection.TopToBottom:
			case StiLegendDirection.BottomToTop:
				width = itemsSize.Width + num;
				if (countColumns > count)
				{
					countColumns = count;
				}
				height = (itemsSize.Height + num2) * (float)countColumns + num2;
				break;
			case StiLegendDirection.LeftToRight:
			case StiLegendDirection.RightToLeft:
			{
				int num3 = count / countColumns;
				if (num3 * countColumns < count)
				{
					num3++;
				}
				width = itemsSize.Width + num;
				height = (itemsSize.Height + num2) * (float)num3 + num2;
				break;
			}
			}
		}
		return new SizeF(width, height);
	}

	internal SizeF GetLegendSize(StiContext context, RectangleF rect, out int countColumns, out int countRows)
	{
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		SizeF titleSize = GetTitleSize(context);
		SizeF seriesSize = GetSeriesSize(context, rect, out countColumns, out countRows);
		SizeF result = new SizeF(Math.Max(titleSize.Width, seriesSize.Width), titleSize.Height + seriesSize.Height);
		float num = 0f;
		float num2 = 0f;
		SizeD size;
		if (legend.Chart != null)
		{
			IStiChart chart = legend.Chart;
			size = legend.Size;
			num = (float)chart.ConvertToHInches(((SizeD)(ref size)).Width * (double)context.Options.Zoom);
			IStiChart chart2 = legend.Chart;
			size = legend.Size;
			num2 = (float)chart2.ConvertToHInches(((SizeD)(ref size)).Height * (double)context.Options.Zoom);
		}
		else
		{
			size = legend.Size;
			num = (float)(((SizeD)(ref size)).Width * (double)context.Options.Zoom);
			size = legend.Size;
			num2 = (float)(((SizeD)(ref size)).Height * (double)context.Options.Zoom);
		}
		if (num != 0f)
		{
			result.Width = num;
		}
		if (num2 != 0f)
		{
			result.Height = num2;
		}
		result.Width = (int)Math.Round(result.Width);
		result.Height = (int)Math.Round(result.Height);
		return result;
	}

	internal List<StiLegendItemCoreXF> GetLegendItems(out int count)
	{
		count = 0;
		List<IStiSeries> series = legend.Chart.Area.Core.GetSeries();
		IStiAxisArea stiAxisArea = legend.Chart.Area as IStiAxisArea;
		List<StiLegendItemCoreXF> list = new List<StiLegendItemCoreXF>();
		if (legend.Chart.Area.ColorEach || series.Any((IStiSeries s) => s is IStiFunnelSeries) || series.Any((IStiSeries s) => s is IStiPictorialSeries) || series.Any((IStiSeries s) => s is IStiDoughnutSeries))
		{
			int num = 0;
			foreach (IStiSeries item in series)
			{
				num = Math.Max(item.Values.Length, num);
			}
			int num2 = 0;
			bool flag = stiAxisArea != null && (stiAxisArea.ReverseHor || stiAxisArea.AxisCore.SeriesOrientation == StiChartSeriesOrientation.Horizontal);
			foreach (IStiSeries item2 in series)
			{
				if (!legend.Chart.Area.Core.IsAcceptableSeries(item2.GetType()))
				{
					continue;
				}
				for (int i = 0; i < num; i++)
				{
					if (item2.ShowInLegend && item2.Values != null)
					{
						if (i >= item2.Values.Length)
						{
							continue;
						}
						int num3 = (flag ? (item2.Values.Length - i - 1) : i);
						double? num4 = item2.Values[num3];
						if (num4.HasValue)
						{
							IStiPieSeries stiPieSeries = item2 as IStiPieSeries;
							IStiFunnelSeries stiFunnelSeries = item2 as IStiFunnelSeries;
							IStiDoughnutSeries stiDoughnutSeries = item2 as IStiDoughnutSeries;
							if (((stiPieSeries == null || stiPieSeries.ShowZeros) && (stiDoughnutSeries == null || stiDoughnutSeries.ShowZeros) && (stiFunnelSeries == null || stiFunnelSeries.ShowZeros)) || num4 != 0.0)
							{
								list.Add(new StiLegendItemCoreXF(legend.Chart.SeriesLabels.Core.GetLabelText(item2, num4, GetArgumentText(item2, num3), item2.Core.GetTag(num3), item2.CoreTitle, useLegendValueType: true), item2, num3, num2));
							}
						}
					}
					count++;
					num2++;
				}
			}
		}
		else if (legend.Chart.Area is IStiSunburstArea)
		{
			if (series.Count >= 1)
			{
				IStiSeries stiSeries = series[0];
				List<object> list2 = (from v in stiSeries.Arguments
					group v by v into x
					select x.Key).ToList();
				int num5 = 0;
				foreach (object item3 in list2)
				{
					string text = item3?.ToString();
					if (!string.IsNullOrEmpty(text))
					{
						list.Add(new StiLegendItemCoreXF(text, stiSeries, num5, num5));
					}
					num5++;
					count++;
				}
			}
		}
		else if (legend.Chart.Area is IStiBoxAndWhiskerArea)
		{
			if (series.Count >= 1)
			{
				IStiSeries series2 = series[0];
				int num6 = 0;
				foreach (IStiSeries item4 in series)
				{
					list.Add(new StiLegendItemCoreXF(item4.CoreTitle, series2, num6, num6));
					num6++;
					count++;
				}
			}
		}
		else if (legend.Chart.Area is IStiWaterfallArea)
		{
			if (series.Count >= 1)
			{
				IStiWaterfallSeries stiWaterfallSeries = series[0] as IStiWaterfallSeries;
				list.Add(new StiLegendItemCoreXF(StiLocalization.Get("PropertyMain", "Positive"), stiWaterfallSeries, 0, 0));
				list.Add(new StiLegendItemCoreXF(StiLocalization.Get("PropertyMain", "Negative"), stiWaterfallSeries, 0, 1));
				list.Add(new StiLegendItemCoreXF(stiWaterfallSeries.Total.Text, stiWaterfallSeries, 0, 2));
				count = 3;
			}
		}
		else
		{
			foreach (IStiSeries item5 in series)
			{
				if (legend.Chart.Area.Core.IsAcceptableSeries(item5.GetType()) && item5.ShowInLegend && (!legend.HideSeriesWithEmptyTitle || !string.IsNullOrEmpty(item5.CoreTitle) || legend.Chart.IsDesigning))
				{
					list.Add(new StiLegendItemCoreXF(item5.CoreTitle, item5, -1, 0));
					count++;
				}
			}
		}
		return list;
	}

	public StiLegendCoreXF(IStiLegend legend)
	{
		this.legend = legend;
	}
}
