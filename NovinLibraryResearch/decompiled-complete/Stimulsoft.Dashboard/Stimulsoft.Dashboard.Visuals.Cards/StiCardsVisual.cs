using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;
using Stimulsoft.Dashboard.Components.Cards;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Dashboard.Visuals.Cards;

public class StiCardsVisual : StiVisual
{
	public StiCardsElement Element { get; }

	public StiDataTable DataTable { get; }

	public override void Draw(StiContext context, RectangleF rect)
	{
		PaintCards(context, rect);
	}

	public void DrawEmptyData(Graphics g, RectangleF rect)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		Color dataEmptyForeColor = StiDashboardStyleHelper.GetDataEmptyForeColor(Element);
		string text = StiLocalization.Get("Dashboard", "Blank");
		Font val = new Font("Arial", 14f);
		SolidBrush val2 = new SolidBrush(dataEmptyForeColor);
		try
		{
			g.DrawString(text, val, (Brush)(object)val2, rect);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	private void PaintCards(StiContext context, RectangleF rect)
	{
		float num = context.Options.Zoom;
		if (Element.IsDesigning)
		{
			num *= (float)Element.Zoom;
		}
		List<StiCardsColumn> list = DataTable.Meters.Where((IStiMeter c) => c is StiCardsColumn).Cast<StiCardsColumn>().ToList();
		StiCardsElementStyle cardsStyle = StiDashboardStyleHelper.GetCardsStyle(Element);
		GetCardRectanglesList(list, rect, num, out var cardRectangleList, out var itemRowRectList);
		List<decimal> list2 = new List<decimal>();
		List<decimal> list3 = new List<decimal>();
		for (int i = 0; i < list.Count; i++)
		{
			StiCardsColumn column = list[i];
			decimal item = StiCardsElementHelper.MinDataColumnValue(column, DataTable, i);
			decimal item2 = StiCardsElementHelper.MaxDataColumnValue(column, DataTable, i);
			list2.Add(item);
			list3.Add(item2);
		}
		bool flag = false;
		for (int j = 0; j < DataTable.Rows.Count; j++)
		{
			object[] values = DataTable.Rows[j];
			RectangleD rect2 = cardRectangleList[j];
			List<RectangleD> rectangles = itemRowRectList[j];
			if (!flag && (rect2.Right - (double)rect.Right > 1.0 || rect2.Bottom - (double)rect.Bottom > 1.0))
			{
				flag = true;
			}
			PaintCardItem(context, rect2, Element, rectangles, list, values, list2, list3, cardsStyle, num);
		}
		if (flag)
		{
			float factor = (float)(StiElementScale.Factor((IStiElement)Element) * StiScale.Factor);
			DrawArrowMore(context, Element, rect, factor);
		}
	}

	private void GetCardRectanglesList(List<StiCardsColumn> columns, RectangleF rect, float zoom, out List<RectangleD> cardRectangleList, out List<List<RectangleD>> itemRowRectList)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		cardRectangleList = new List<RectangleD>();
		itemRowRectList = new List<List<RectangleD>>();
		Bitmap val = new Bitmap(1, 1);
		try
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				StiGdiContextPainter contextPainter = new StiGdiContextPainter(val2);
				StiContext context = new StiContext(contextPainter, isGdi: true, isWpf: false, isPrinting: false, zoom);
				double num = 0.0;
				double num2 = 0.0;
				foreach (object[] row in DataTable.Rows)
				{
					List<RectangleD> rectangles = new List<RectangleD>();
					SizeF sizeF = MeasureCards(context, Element, columns, row, ref rectangles, zoom);
					num = Math.Max(num, sizeF.Width);
					num2 = Math.Max(num2, sizeF.Height);
					itemRowRectList.Add(rectangles);
				}
				if (Element.ColumnCount > 0 && Element.Orientation == StiItemOrientation.Horizontal)
				{
					num = rect.Width / (float)Element.ColumnCount;
				}
				else if (Element.ColumnCount > 0 && Element.Orientation == StiItemOrientation.Vertical)
				{
					num2 = rect.Height / (float)Element.ColumnCount;
				}
				cardRectangleList = ((Element.Orientation == StiItemOrientation.Horizontal) ? GetRectanglesHorizontalOrientation(rect, num, num2) : GetRectanglesVerticalOrientation(rect, num, num2));
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private List<RectangleD> GetRectanglesHorizontalOrientation(RectangleF rect, double itemMaxWidth, double itemMaxHeight)
	{
		List<RectangleD> list = new List<RectangleD>();
		double num = Math.Floor((double)rect.Width / itemMaxWidth);
		if (num == 0.0)
		{
			num = 1.0;
		}
		itemMaxWidth = (double)rect.Width / num;
		float num2 = rect.X;
		float num3 = rect.Y;
		int num4 = 0;
		for (int i = 0; i < DataTable.Rows.Count; i++)
		{
			RectangleD item = new RectangleD(num2, num3, itemMaxWidth, itemMaxHeight);
			list.Add(item);
			num2 += (float)item.Width;
			num4++;
			if ((double)num4 >= num)
			{
				num2 = rect.X;
				num3 += (float)item.Height;
				num4 = 0;
			}
		}
		return list;
	}

	private List<RectangleD> GetRectanglesVerticalOrientation(RectangleF rect, double itemMaxWidth, double itemMaxHeight)
	{
		List<RectangleD> list = new List<RectangleD>();
		double num = Math.Floor((double)rect.Height / itemMaxHeight);
		if (num > 1.0)
		{
			itemMaxHeight = (double)rect.Height / num;
		}
		float num2 = rect.X;
		float num3 = rect.Y;
		int num4 = 0;
		for (int i = 0; i < DataTable.Rows.Count; i++)
		{
			RectangleD item = new RectangleD(num2, num3, itemMaxWidth, itemMaxHeight);
			list.Add(item);
			num3 += (float)item.Height;
			num4++;
			if ((double)num4 >= num)
			{
				num3 = rect.Y;
				num2 += (float)item.Width;
				num4 = 0;
			}
		}
		return list;
	}

	internal RectangleF GetContentRectangle(RectangleF rect)
	{
		if (DataTable == null || DataTable.Rows.Count <= 1)
		{
			return rect;
		}
		float zoom = (float)StiScale.Factor;
		List<StiCardsColumn> columns = DataTable.Meters.Where((IStiMeter c) => c is StiCardsColumn).Cast<StiCardsColumn>().ToList();
		GetCardRectanglesList(columns, rect, zoom, out var cardRectangleList, out var _);
		RectangleD b = RectangleD.Empty;
		foreach (RectangleD item in cardRectangleList)
		{
			b = RectangleD.Union(item, b);
		}
		return b.ToRectangleF();
	}

	private void PaintCardItem(StiContext context, RectangleD rect, StiCardsElement cards, List<RectangleD> rectangles, List<StiCardsColumn> columns, object[] values, List<decimal> minList, List<decimal> maxList, StiCardsElementStyle style, double zoom)
	{
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Expected O, but got Unknown
		RectangleD rectangleD = new RectangleD(rect.X + cards.CardMargin.Left * zoom, rect.Y + cards.CardMargin.Top * zoom, rect.Width - cards.CardMargin.Left * zoom - cards.CardMargin.Right * zoom, rect.Height - cards.CardMargin.Top * zoom - cards.CardMargin.Bottom * zoom);
		context.PushClip(rectangleD.ToRectangleF());
		context.FillRectangle(style.CellBackColor, rectangleD.ToRectangle());
		for (int i = 0; i < values.Length; i++)
		{
			StiCardsColumn stiCardsColumn = columns[i];
			if (StiCardsColumnVisibilityHelper.GetVisible(stiCardsColumn, cards.Report))
			{
				object obj = values[i];
				decimal min = minList[i];
				decimal max = maxList[i];
				Color foreColor = GetForeColor(columns[i], style);
				Font font = stiCardsColumn.Font;
				RectangleD rectangleD2 = rectangles[i];
				RectangleD rect2 = new RectangleD(rect.X + cards.CardMargin.Left * zoom + cards.CardPadding.Left * zoom + rectangleD2.X, rect.Y + cards.CardMargin.Top * zoom + cards.CardPadding.Top * zoom + rectangleD2.Y, rect.Width - cards.CardMargin.Left * zoom - cards.CardMargin.Right * zoom - cards.CardPadding.Left * zoom - cards.CardPadding.Right * zoom, rectangles[i].Height - cards.CardPadding.Top * zoom - cards.CardPadding.Bottom * zoom);
				if (stiCardsColumn is StiDataBarsCardsColumn column)
				{
					StiCardsCellDrawer.DataBarsDraw(context, rect2, cards, style, foreColor, column, zoom, StiValueHelper.TryToDecimal(obj), min, max);
					continue;
				}
				if (stiCardsColumn is StiColorScaleCardsColumn column2)
				{
					StiCardsCellDrawer.ColorScaleDraw(context, rect2, cards, style, foreColor, column2, zoom, StiValueHelper.TryToDecimal(obj), min, max);
					continue;
				}
				if (stiCardsColumn is StiSparklinesCardsColumn column3)
				{
					StiCardsCellDrawer.SparklinesDraw(context, rect2, column3, obj, zoom, style);
					continue;
				}
				if (stiCardsColumn is StiBubbleCardsColumn bubbleColumn)
				{
					StiCardsCellDrawer.BubbleDraw(context, rect2, cards, style, foreColor, bubbleColumn, zoom, StiValueHelper.TryToDecimal(obj), min, max);
					continue;
				}
				if (stiCardsColumn is StiIndicatorCardsColumn column4)
				{
					StiCardsCellDrawer.IndicatorDraw(context, rect2, cards, style, column4, zoom, StiValueHelper.TryToNullableDecimal(obj));
					continue;
				}
				Font font2 = new Font(font.FontFamily, font.Size * (float)zoom);
				SizeF rectSize = context.MeasureString(obj?.ToString(), new StiFontGeom(font2));
				StiCardsCellDrawer.DrawText(context, obj?.ToString(), font, foreColor, stiCardsColumn.HorAlignment, rect2.ToRectangle(), rectSize, zoom);
			}
		}
		context.PopClip();
		StiPenGeom pen = new StiPenGeom(style.LineColor, 1f);
		context.DrawRectangle(pen, rectangleD.ToRectangle());
	}

	private static SizeF MeasureCards(StiContext context, StiCardsElement cards, List<StiCardsColumn> columns, object[] objects, ref List<RectangleD> rectangles, float zoom)
	{
		float num = 0f;
		float num2 = 0f;
		for (int i = 0; i < objects.Length; i++)
		{
			StiCardsColumn stiCardsColumn = columns[i];
			if (!StiCardsColumnVisibilityHelper.GetVisible(stiCardsColumn, cards.Report))
			{
				rectangles.Add(RectangleD.Empty);
				continue;
			}
			string text = objects[i]?.ToString();
			SizeF sizeF;
			if (stiCardsColumn is StiSparklinesCardsColumn sparklinesCardsColumn)
			{
				sizeF = MeasureSparklinesCell(sparklinesCardsColumn, zoom);
			}
			else if (stiCardsColumn is StiIndicatorCardsColumn column)
			{
				sizeF = MeasureIndicatorCell(context, cards, column, objects[i], zoom);
			}
			else if (stiCardsColumn is StiBubbleCardsColumn column2)
			{
				sizeF = MeasureBubbleCell(context, cards, column2, objects[i], zoom);
			}
			else
			{
				StiFontGeom fontGeom = GetFontGeom(stiCardsColumn.Font, zoom);
				sizeF = context.MeasureString(text, fontGeom);
			}
			RectangleD item = new RectangleD(0.0, num2, (double)sizeF.Width + cards.CardPadding.Left * (double)zoom + cards.CardPadding.Right * (double)zoom, (double)sizeF.Height + cards.CardPadding.Top * (double)zoom + cards.CardPadding.Bottom * (double)zoom);
			rectangles.Add(item);
			num = Math.Max(num, (float)item.Width + (float)cards.CardPadding.Left * zoom + (float)cards.CardPadding.Right * zoom);
			num2 += sizeF.Height;
		}
		return new SizeF(num, num2 + (float)cards.CardPadding.Top * zoom + (float)cards.CardPadding.Bottom * zoom + (float)cards.CardMargin.Top * zoom + (float)cards.CardMargin.Bottom * zoom);
	}

	public static SizeF MeasureSparklinesCell(StiSparklinesCardsColumn sparklinesCardsColumn, float zoom)
	{
		int num = ((sparklinesCardsColumn.Height == 0) ? 20 : sparklinesCardsColumn.Height);
		return new SizeF(0f, (float)num * zoom);
	}

	public static SizeF MeasureIndicatorCell(StiContext context, StiCardsElement cards, StiCardsColumn column, object rowValue, float zoom)
	{
		StiIndicatorCardsColumn column2 = column as StiIndicatorCardsColumn;
		string cellText = StiCardsCellDrawer.GetCellText(cards, column2, StiValueHelper.TryToNullableDecimal(rowValue));
		StiFontGeom fontGeom = GetFontGeom(column.Font, zoom);
		return context.MeasureString(cellText, fontGeom);
	}

	public static SizeF MeasureBubbleCell(StiContext context, StiCardsElement cards, StiCardsColumn column, object rowValue, float zoom)
	{
		string text = StiTextFormatHelper.FormatBasedOnColumnType(cards, column.TextFormat, column, rowValue);
		StiFontGeom fontGeom = GetFontGeom(column.Font, zoom);
		SizeF sizeF = context.MeasureString(text, fontGeom);
		sizeF.Width += StiScale.I4;
		return new SizeF(sizeF.Width * zoom + sizeF.Height, sizeF.Height * zoom);
	}

	private static Color GetForeColor(StiCardsColumn column, StiCardsElementStyle style)
	{
		if (column.ForeColor != Color.Transparent)
		{
			return column.ForeColor;
		}
		return style?.CellForeColor ?? StiElementConsts.Table.Font.Color;
	}

	internal static StiFontGeom GetFontGeom(Font font, float zoom)
	{
		return StiFontGeom.ChangeFontSize(font, font.Size * zoom);
	}

	public StiCardsVisual(StiCardsElement element, StiDataTable dataTable)
	{
		Element = element;
		DataTable = dataTable;
	}
}
