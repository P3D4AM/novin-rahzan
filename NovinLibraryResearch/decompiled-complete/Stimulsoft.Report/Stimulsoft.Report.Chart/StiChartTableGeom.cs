using System;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiChartTableGeom : StiCellGeom
{
	private string[,] table;

	private float widthCellLegendTableChart;

	private float heightCellHeader;

	private float widthSpace;

	private IStiChartTable chartTable;

	private StiPenGeom pen;

	private StiFontGeom font;

	private StiFontGeom fontHeader;

	private StiBrush labelBrush;

	private StiStringFormatGeom sf;

	private StiStringFormatGeom sfHeader;

	private StiBrush labelHeaderBrush;

	public StiChartTableGeom(RectangleF clientRectangle, string[,] table, float widthCellLegendTableChart, float heightCellHeader, float widthSpace, IStiChartTable chartTable)
		: base(clientRectangle)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		this.widthCellLegendTableChart = widthCellLegendTableChart;
		this.heightCellHeader = heightCellHeader;
		this.widthSpace = widthSpace;
		this.table = table;
		this.chartTable = chartTable;
		pen = new StiPenGeom(chartTable.GridLineColor);
		labelBrush = (StiBrush)new StiSolidBrush(chartTable.TextColor);
		labelHeaderBrush = (StiBrush)new StiSolidBrush(chartTable.Header.TextColor);
	}

	public override void Draw(StiContext context)
	{
		font = StiFontGeom.ChangeFontSize(chartTable.DataCells.Font, chartTable.DataCells.Font.Size * context.Options.Zoom);
		sf = context.GetGenericStringFormat();
		sf.Trimming = (StringTrimming)0;
		sf.FormatFlags = (StringFormatFlags)6144;
		sf.Alignment = (StringAlignment)1;
		sf.LineAlignment = (StringAlignment)1;
		fontHeader = StiFontGeom.ChangeFontSize(chartTable.Header.Font, chartTable.Header.Font.Size * context.Options.Zoom);
		sfHeader = context.GetGenericStringFormat();
		sfHeader.Trimming = (StringTrimming)0;
		if (!chartTable.Header.WordWrap)
		{
			sfHeader.FormatFlags = (StringFormatFlags)6144;
		}
		sfHeader.Alignment = (StringAlignment)1;
		sfHeader.LineAlignment = (StringAlignment)1;
		RectangleF rectangleF = base.ClientRectangle;
		StiAxisArea stiAxisArea = chartTable.Chart.Area as StiAxisArea;
		float x = (stiAxisArea.ReverseHor ? rectangleF.X : (rectangleF.X + widthCellLegendTableChart));
		float y = rectangleF.Y;
		float width = rectangleF.Width - widthCellLegendTableChart;
		float height = heightCellHeader;
		RectangleF rect = new RectangleF(x, y, width, height);
		List<string> list = new List<string>();
		for (int i = 1; i < table.GetLength(1); i++)
		{
			list.Add(table[0, i]);
		}
		DrawHeaderArgument(context, rect, list, stiAxisArea.XAxis.StartFromZero);
		float x2 = (stiAxisArea.ReverseHor ? (rectangleF.Right - widthCellLegendTableChart) : rectangleF.X);
		float y2 = rectangleF.Y + heightCellHeader;
		float width2 = widthCellLegendTableChart;
		float height2 = rectangleF.Height - heightCellHeader;
		RectangleF rect2 = new RectangleF(x2, y2, width2, height2);
		List<string> list2 = new List<string>();
		for (int j = 1; j < table.GetLength(0); j++)
		{
			list2.Add(table[j, 0]);
		}
		DrawTitleLegend(context, rect2, list2);
		float x3 = (stiAxisArea.ReverseHor ? rectangleF.X : (rectangleF.X + widthCellLegendTableChart));
		float y3 = rectangleF.Y + heightCellHeader;
		float width3 = rectangleF.Width - widthCellLegendTableChart;
		float height3 = rectangleF.Height - heightCellHeader;
		RectangleF rect3 = new RectangleF(x3, y3, width3, height3);
		DrawRootTable(context, rect3, stiAxisArea.XAxis.StartFromZero);
	}

	private void DrawHeaderArgument(StiContext context, RectangleF rect, List<string> listArgument, bool startFromZero)
	{
		context.FillRectangle(chartTable.Header.Brush, Rectangle.Ceiling(new RectangleF(rect.X, rect.Y, rect.Width, rect.Height)));
		if (chartTable.GridOutline)
		{
			context.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
		}
		int num = (startFromZero ? (listArgument.Count + 1) : (listArgument.Count - 1));
		float num2 = rect.Width / (float)num;
		for (int i = 0; i < listArgument.Count; i++)
		{
			float num3 = 0f;
			if (i == 0 || i == listArgument.Count - 1)
			{
				num3 = (startFromZero ? (num2 / 2f) : ((0f - num2) / 2f));
			}
			float num4 = (startFromZero ? (num2 / 2f) : ((0f - num2) / 2f));
			if (i == 0)
			{
				num4 = 0f;
			}
			float x = rect.X + (float)i * num2 + num4 + widthSpace;
			float num5 = num2 + num3 - 2f * widthSpace;
			if (num5 < 0f)
			{
				num5 = 0f;
			}
			RectangleF rectangleF = new RectangleF(x, rect.Y, num5, rect.Height);
			if (chartTable.Header.WordWrap)
			{
				context.DrawRotatedString(listArgument[i], fontHeader, labelHeaderBrush, rectangleF, sfHeader, (StiRotationMode)4, 0f, antialiasing: true, (int)Math.Ceiling(num5));
			}
			else
			{
				context.DrawString(listArgument[i], fontHeader, labelHeaderBrush, Rectangle.Ceiling(rectangleF), sfHeader);
			}
			if (chartTable.GridLinesVert && i != listArgument.Count - 1)
			{
				float num6 = (startFromZero ? (rect.X + (float)(i + 1) * num2 + num2 / 2f) : (rect.X + (float)(i + 1) * num2 - num2 / 2f));
				context.DrawLine(pen, num6, rect.Y, num6, rect.Bottom);
			}
		}
	}

	private void DrawTitleLegend(StiContext context, RectangleF rect, List<string> list)
	{
		if (chartTable.GridOutline)
		{
			context.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
		}
		float num = rect.Height / (float)list.Count;
		for (int i = 0; i < list.Count; i++)
		{
			float num2 = rect.X;
			float y = rect.Y;
			float num3 = rect.Width;
			if (chartTable.GridLinesHor && i != 0)
			{
				context.DrawLine(pen, num2, y + num * (float)i, num2 + num3, y + num * (float)i);
			}
			if (chartTable.MarkerVisible)
			{
				RectangleF rect2 = new RectangleF(num2 + 2f, y + 2f + num * (float)i, num - 4f, num - 4f);
				IStiLegendMarker stiLegendMarker = StiMarkerLegendFactory.CreateMarker(chartTable.Chart.Series[i]);
				stiLegendMarker.Draw(context, chartTable.Chart.Series[i], rect2, i, list.Count, -1);
				num2 += num;
				num3 -= num;
			}
			context.DrawString(rect: new RectangleF(num2, y + num * (float)i, num3, num), text: list[i], font: font, brush: labelBrush, sf: sf);
		}
	}

	private void DrawRootTable(StiContext context, RectangleF rect, bool startFromZero)
	{
		if (chartTable.GridOutline)
		{
			context.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
		}
		int length = table.GetLength(0);
		int length2 = table.GetLength(1);
		int num = (startFromZero ? table.GetLength(1) : (table.GetLength(1) - 2));
		float num2 = rect.Width / (float)num;
		float num3 = rect.Height / (float)(length - 1);
		StiFontGeom stiFontGeom = font;
		if (chartTable.DataCells.ShrinkFontToFit)
		{
			for (int i = 1; i < length; i++)
			{
				if (chartTable.GridLinesHor && i != length - 1)
				{
					context.DrawLine(pen, rect.X, rect.Y + (float)i * num3, rect.Right, rect.Y + (float)i * num3);
				}
				for (int j = 1; j < length2; j++)
				{
					float num4 = 0f;
					if (j == 1 || j == length2 - 1)
					{
						num4 = (startFromZero ? (num2 / 2f) : ((0f - num2) / 2f));
					}
					float num5 = (startFromZero ? (num2 / 2f) : ((0f - num2) / 2f));
					if (j == 1)
					{
						num5 = 0f;
					}
					RectangleF rectangleF = new RectangleF(rect.X + (float)(j - 1) * num2 + num5, rect.Y + (float)(i - 1) * num3, num2 + num4, num3);
					bool flag = CheckFontSize(context, table[i, j], stiFontGeom, rectangleF);
					while (!flag)
					{
						stiFontGeom.FontSize -= 0.5f;
						if (stiFontGeom.FontSize <= chartTable.DataCells.ShrinkFontToFitMinimumSize)
						{
							stiFontGeom.FontSize = chartTable.DataCells.ShrinkFontToFitMinimumSize;
							break;
						}
						flag = CheckFontSize(context, table[i, j], stiFontGeom, rectangleF);
					}
				}
			}
		}
		for (int k = 1; k < length; k++)
		{
			if (chartTable.GridLinesHor && k != length - 1)
			{
				context.DrawLine(pen, rect.X, rect.Y + (float)k * num3, rect.Right, rect.Y + (float)k * num3);
			}
			for (int l = 1; l < length2; l++)
			{
				float num6 = 0f;
				if (l == 1 || l == length2 - 1)
				{
					num6 = (startFromZero ? (num2 / 2f) : ((0f - num2) / 2f));
				}
				float num7 = (startFromZero ? (num2 / 2f) : ((0f - num2) / 2f));
				if (l == 1)
				{
					num7 = 0f;
				}
				context.DrawRotatedString(rect: new RectangleF(rect.X + (float)(l - 1) * num2 + num7, rect.Y + (float)(k - 1) * num3, num2 + num6, num3), text: table[k, l], font: stiFontGeom, brush: labelBrush, sf: sf, mode: (StiRotationMode)4, angle: 0f, antialiasing: true);
				if (chartTable.GridLinesVert && l < length2 - 1)
				{
					float num8 = (startFromZero ? (rect.X + (float)l * num2 + num2 / 2f) : (rect.X + (float)l * num2 - num2 / 2f));
					context.DrawLine(pen, num8, rect.Y, num8, rect.Bottom);
				}
			}
		}
	}

	private bool CheckFontSize(StiContext context, string text, StiFontGeom fontGeom, RectangleF rectangleF)
	{
		return context.MeasureRotatedString(text, fontGeom, rectangleF, sf, 0f).Width < rectangleF.Width;
	}
}
