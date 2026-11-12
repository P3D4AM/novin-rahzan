using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Meters;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Dashboard.Drawing.Helpers;
using Stimulsoft.Dashboard.Drawing.Painters.Table;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Extensions;
using Stimulsoft.Data.Functions;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Images;

namespace Stimulsoft.Dashboard.Drawing.Painters;

public class StiTableElementGdiPainter : StiElementGdiPainter
{
	private static Hashtable captionSizeCache = new Hashtable();

	public override void PaintContent(Graphics g, RectangleD rect, IStiElement element)
	{
		StiErrorStack.SetOk(element.GetKey());
		GraphicsState val = g.Save();
		StiTableElement stiTableElement = element as StiTableElement;
		try
		{
			g.SetClip(rect.ToRectangleF());
			rect = StiElementGdiPainter.ApplyCornerRadiusAndSetClip(g, rect, stiTableElement);
			StiDataTable dataTable = (StiDbsThumbnaiModelHelper.AllowPage(element.Page) ? StiThumbnailDataHelper.Get(element) : StiElementDataCache.GetOrCreateWithProgress(stiTableElement));
			if (IsNoResult(element, dataTable))
			{
				PaintNoResult(g, rect, stiTableElement);
			}
			else
			{
				PaintTable(g, rect, stiTableElement, dataTable);
			}
		}
		catch (Exception ex)
		{
			StiErrorStack.SetError(element.GetKey(), ex.Message);
			PaintError(g, rect.ToRectangleF(), element);
		}
		finally
		{
			StiTableElementClickHelper.ResetClick(stiTableElement);
			StiTableElementMouseOverHelper.ResetMouseOverPoint(stiTableElement);
			g.Restore(val);
		}
	}

	private static void PaintTable(Graphics g, RectangleD rect, StiTableElement table, StiDataTable dataTable)
	{
		List<StiTableColumn> list = dataTable.Meters.Where((IStiMeter c) => c is StiTableColumn).Cast<StiTableColumn>().ToList();
		double[] widths = new double[list.Count];
		List<double> heights = new List<double>();
		MeasureCells(g, rect, table, dataTable, list, widths, heights);
		PaintCells(g, rect, table, dataTable, list, widths, heights);
	}

	private static void MeasureCells(Graphics g, RectangleD rect, StiTableElement table, StiDataTable dataTable, List<StiTableColumn> columns, double[] widths, List<double> heights)
	{
		double num = StiElementConsts.Table.GetHeight(table.HeaderFont, table.Zoom * StiScale.Factor);
		for (int i = 0; i < columns.Count; i++)
		{
			StiTableColumn stiTableColumn = columns[i];
			if (!StiTableColumnVisibilityHelper.GetVisible(stiTableColumn, table.Report))
			{
				continue;
			}
			SizeD sizeD = MeasureHeader(g, table, stiTableColumn);
			double width = sizeD.Width;
			num = Math.Max(num, sizeD.Height);
			StiTableColumnSizeHelper stiTableColumnSizeHelper = new StiTableColumnSizeHelper(table.HeaderFont, stiTableColumn);
			width = stiTableColumnSizeHelper.GetColumnWidth(width);
			widths[i] = width * table.Zoom;
			if (stiTableColumn.ShowTotalSummary)
			{
				decimal num2 = StiTableElementHelper.CalculateTotal(stiTableColumn, i, dataTable);
				string text = StiTextFormatHelper.FormatBasedOnColumnType(table, stiTableColumn, num2);
				if (!string.IsNullOrEmpty(text))
				{
					double width2 = g.MeasureString(text, table.HeaderFont).Width;
					width2 = stiTableColumnSizeHelper.GetColumnWidth(width2);
					widths[i] = Math.Max(widths[i], width2 * table.Zoom);
				}
			}
		}
		heights.Add(num);
		double num3 = num;
		if (num3 > rect.Height)
		{
			return;
		}
		foreach (object[] row in dataTable.Rows)
		{
			double num4 = 0.0;
			double num5 = 0.0;
			for (int j = 0; j < columns.Count; j++)
			{
				StiTableColumn column = columns[j];
				if (StiTableColumnVisibilityHelper.GetVisible(column, table.Report))
				{
					SizeD sizeD2 = MeasureColumn(g, table, column, row[j]);
					widths[j] = Math.Max(widths[j], sizeD2.Width);
					num4 = Math.Max(num4, sizeD2.Height);
					num5 += widths[j];
					if (num5 > rect.Width && table.SizeMode != StiTableSizeMode.Fit)
					{
						break;
					}
				}
			}
			heights.Add(num4);
			num3 += num4;
			if (num3 > rect.Height)
			{
				break;
			}
		}
	}

	private static void PaintCells(Graphics g, RectangleD rect, StiTableElement table, StiDataTable dataTable, List<StiTableColumn> columns, double[] widths, List<double> heights)
	{
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Expected O, but got Unknown
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Expected O, but got Unknown
		StiTableElementStyle tableStyle = StiDashboardStyleHelper.GetTableStyle(table);
		double num = widths.Sum();
		double num2 = ((table.SizeMode == StiTableSizeMode.Fit && rect.Width > num) ? (rect.Width / num) : 1.0);
		bool flag = columns.FirstOrDefault((StiTableColumn xx) => xx.ShowTotalSummary) != null;
		if (dataTable == null || dataTable.IsEmpty)
		{
			flag = false;
		}
		double num3 = rect.X;
		int num4 = 0;
		foreach (StiTableColumn column in columns)
		{
			int num5 = 0;
			double num6 = widths[num4] * num2;
			double y = rect.Y;
			RectangleD rect2 = new RectangleD(num3, y, num6, heights[num5]);
			bool visible = StiTableColumnVisibilityHelper.GetVisible(column, table.Report);
			if (visible)
			{
				PaintHeader(g, rect2, table, dataTable, column, tableStyle);
			}
			y += rect2.Height;
			num5++;
			if (visible && y <= rect.Bottom)
			{
				decimal min = StiTableElementHelper.MinDataColumnValue(column, dataTable, num4);
				decimal max = StiTableElementHelper.MaxDataColumnValue(column, dataTable, num4);
				foreach (object[] row in dataTable.Rows)
				{
					object obj = row[num4];
					if (obj is StiSystemVariableObject)
					{
						obj = Funcs.GetSystemVariable((StiSystemVariableObject)obj, num5);
					}
					RectangleD rect3 = new RectangleD(num3, y, num6, heights[num5]);
					string[] columnKeys = columns.Select((StiTableColumn c) => c.Key).ToArray();
					PaintCell(g, rect3, table, columnKeys, row, column, obj, table.Zoom, tableStyle, min, max, num5, isSelected: false, drawBorderSides: true);
					y += heights[num5];
					if (y >= rect.Bottom)
					{
						break;
					}
					num5++;
				}
				if (flag)
				{
					double num7 = 28.0 * StiScale.Factor;
					SolidBrush val = new SolidBrush(tableStyle.SelectedCellForeColor);
					try
					{
						RectangleD rect4 = new RectangleD(num3, rect.Bottom - num7, num6, num7);
						StiDrawing.FillRectangle(g, tableStyle.FooterBackColor, rect4);
						if (num4 != 0)
						{
							StiDrawing.DrawLine(g, tableStyle.LineColor, rect4.Left, rect4.Top, rect4.Left, rect4.Bottom);
						}
						if (column.ShowTotalSummary)
						{
							decimal num8 = StiTableElementHelper.CalculateTotal(column, num4, dataTable);
							string text = ((column.SummaryType == StiSummaryColumnType.Count) ? ((int)num8).ToString() : StiTextFormatHelper.FormatBasedOnColumnType(table, column, num8));
							if (!string.IsNullOrEmpty(text))
							{
								StringFormat val2 = new StringFormat();
								try
								{
									if (column.SummaryAlignment == StiHorAlignment.Left)
									{
										val2.Alignment = (StringAlignment)0;
									}
									else if (column.SummaryAlignment == StiHorAlignment.Center)
									{
										val2.Alignment = (StringAlignment)1;
									}
									else if (column.SummaryAlignment == StiHorAlignment.Right)
									{
										val2.Alignment = (StringAlignment)2;
									}
									val2.LineAlignment = (StringAlignment)1;
									Font gdiFont = StiElementConsts.Table.Font.GetGdiFont(table.Zoom, null, table.FooterFont);
									try
									{
										StiDrawing.DrawString(g, text, gdiFont, GetFooterForeColor(table), rect4, val2);
									}
									finally
									{
										((IDisposable)gdiFont)?.Dispose();
									}
								}
								finally
								{
									((IDisposable)val2)?.Dispose();
								}
							}
						}
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
			}
			num3 += num6;
			if (num3 > rect.Right)
			{
				if (num4 + 1 <= columns.Count - 1)
				{
					if (rect2.Right > rect.Right)
					{
						rect2.Width = rect.Right - rect2.X;
					}
					PaintArrowMoreColumns(g, rect2);
				}
				break;
			}
			num4++;
		}
	}

	private static void PaintHeader(Graphics g, RectangleD rect, StiTableElement table, StiDataTable dataTable, StiTableColumn column, StiTableElementStyle style)
	{
		if (rect.Width > 0.0 && rect.Height > 0.0)
		{
			PointD? pointClick = StiTableElementClickHelper.GetPointClick(table);
			if (pointClick.HasValue && rect.Contains(pointClick.Value))
			{
				StiTableElementSelection.ResetSelection(table);
				StiTableElementSelection.Select(table, column);
				column.Key = StiKeyHelper.GetOrGeneratedKey(column.Key);
				StiTableElementClickHelper.InvokeTableHeaderClick(table, dataTable, column.Key, rect);
			}
			PointD? mouseOverPoint = StiTableElementMouseOverHelper.GetMouseOverPoint(table);
			bool flag = mouseOverPoint.HasValue && rect.Contains(mouseOverPoint.Value);
			Color color = (StiTableElementSelection.IsSelected(table, column) ? style.HotHeaderBackColor : style.HeaderBackColor);
			if (flag)
			{
				color = style.HotHeaderBackColor;
			}
			StiDrawing.FillRectangle(g, color, rect);
			PaintFrozenArea(g, rect, table, column);
			IStiTableDashboardInteraction stiTableDashboardInteraction = table.DashboardInteraction as IStiTableDashboardInteraction;
			bool flag2 = stiTableDashboardInteraction.AllowUserFiltering || stiTableDashboardInteraction.AllowUserSorting;
			if (table.Columns.Where((StiTableColumn c) => StiTableColumnVisibilityHelper.GetVisible(c, table.Report)).IndexOf(column) != 0)
			{
				StiDrawing.DrawLine(g, style.LineColor, rect.Left, rect.Top, rect.Left, rect.Bottom);
			}
			PaintHeaderText(g, rect, table, column);
			if (flag2)
			{
				PaintSortArrow(g, rect, table, column, style);
				PaintFilterArrow(g, rect, table, column, style, flag);
			}
		}
	}

	public static void PaintFrozenArea(Graphics g, RectangleD rect, StiTableElement table, StiTableColumn column)
	{
		if (table.FrozenColumns != 0)
		{
			int num = table.Columns.IndexOf(column);
			if (num == table.FrozenColumns - 1)
			{
				StiDrawing.FillRectangle(g, StiColor.Get("10FFFFFF"), new RectangleD(rect.Right - (double)StiScale.I6, rect.Top, StiScale.I6, rect.Height));
			}
			else if (num == table.FrozenColumns)
			{
				StiDrawing.FillRectangle(g, StiColor.Get("10000000"), new RectangleD(rect.Left, rect.Top, StiScale.I6, rect.Height));
			}
		}
	}

	private static void PaintHeaderText(Graphics g, RectangleD rect, StiTableElement table, StiTableColumn column)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		if (column == null)
		{
			return;
		}
		string label = StiLabelHelper.GetLabel(column);
		if (string.IsNullOrEmpty(label))
		{
			return;
		}
		StringFormat val = new StringFormat(StringFormat.GenericTypographic);
		try
		{
			Font gdiFont = StiElementConsts.Table.Font.GetGdiFont(table.Zoom, null, table.HeaderFont);
			try
			{
				val.LineAlignment = (StringAlignment)1;
				val.Alignment = (StringAlignment)1;
				val.Trimming = (StringTrimming)3;
				val.FormatFlags = (StringFormatFlags)((!StiTableColumnSizeHelper.IsWordWrap(column)) ? 4096 : 0);
				StiTextDrawing.DrawString(g, label, gdiFont, GetHeaderForeColor(table), rect, val);
			}
			finally
			{
				((IDisposable)gdiFont)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private static Color GetHeaderForeColor(StiTableElement table)
	{
		if (table.HeaderForeColor != Color.Transparent)
		{
			return table.HeaderForeColor;
		}
		StiTableElementStyle tableStyle = StiDashboardStyleHelper.GetTableStyle(table);
		return tableStyle.HeaderForeColor;
	}

	private static Color GetFooterForeColor(StiTableElement table)
	{
		if (table.FooterForeColor != Color.Transparent)
		{
			return table.FooterForeColor;
		}
		StiTableElementStyle tableStyle = StiDashboardStyleHelper.GetTableStyle(table);
		return tableStyle.FooterForeColor;
	}

	internal static void PaintCell(Graphics g, RectangleD rect, StiTableElement table, string[] columnKeys, object[] rowValues, StiTableColumn column, object value, double zoom, StiTableElementStyle style, decimal min, decimal max, int rowIndex, bool isSelected, bool drawBorderSides)
	{
		bool? isInterlaced = (rowIndex & 1) == 0;
		Color? defaultColor = (isInterlaced.HasValue ? new Color?(GetBackgroundColor(style, isInterlaced.Value)) : null);
		if (!isSelected)
		{
			defaultColor = StiTableElementConditionHelper.ProcessBackColor(defaultColor, column.Key, table, rowValues, columnKeys);
			if (defaultColor.HasValue)
			{
				StiDrawing.FillRectangle(g, defaultColor.GetValueOrDefault(), rect);
			}
		}
		PaintFrozenArea(g, rect, table, column);
		if (drawBorderSides && table.Columns.Where((StiTableColumn c) => StiTableColumnVisibilityHelper.GetVisible(c, table.Report)).IndexOf(column) != 0)
		{
			StiDrawing.DrawLine(g, style.LineColor, rect.Left, rect.Top, rect.Left, rect.Bottom);
		}
		GraphicsState val = g.Save();
		try
		{
			g.SetClip(rect.ToRectangleF(), (CombineMode)1);
			if (column is StiDataBarsColumn)
			{
				StiDataBarsCellPainter.Draw(g, rect, table, rowValues, columnKeys, column, zoom, StiValueHelper.TryToDecimal(value), min, max, isInterlaced.GetValueOrDefault(false), isSelected);
			}
			else if (column is StiColorScaleColumn)
			{
				StiColorScaleColumn stiColorScaleColumn = column as StiColorScaleColumn;
				StiColorScaleCellPainter.Draw(g, rect, table, columnKeys, rowValues, column, StiValueHelper.TryToDecimal(value), zoom, min, max, style, stiColorScaleColumn.MinimumColor, stiColorScaleColumn.MaximumColor);
			}
			else if (column is StiSparklinesColumn)
			{
				StiSparklinesCellPainter.Draw(g, rect, column as StiSparklinesColumn, value, (float)(zoom * StiScale.Factor), style);
			}
			else if (column is StiIndicatorColumn)
			{
				StiIndicatorCellPainter.Draw(g, rect, table, rowValues, columnKeys, column as StiIndicatorColumn, zoom, StiValueHelper.TryToNullableDecimal(value), style);
			}
			else if (column is StiBubbleColumn)
			{
				StiBubbleCellPainter.Draw(g, rect, table, column, zoom, StiValueHelper.TryToDecimal(value), min, max, isInterlaced.GetValueOrDefault(false), isSelected);
			}
			else if (StiImageHelper.IsImage(value))
			{
				StiImageCellPainter.Draw(g, rect, value, column);
			}
			else if (value is bool)
			{
				StiBoolCellPainter.Draw(g, rect, table, rowValues, columnKeys, column, value, 1f, isSelected);
			}
			else
			{
				string text = StiTextFormatHelper.FormatBasedOnColumnType(table, column, value);
				StiTextCellPainter.Draw(g, rect, table, rowValues, columnKeys, column, value, zoom, text, style, isSelected, isInterlaced);
			}
		}
		finally
		{
			g.Restore(val);
		}
	}

	private static void PaintSortIndex(Graphics g, RectangleF textRect, StiTableElement table, StiTableColumn column, StiTableElementStyle style)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		List<StiDataSortRule> userSorts = table.UserSorts;
		if (userSorts.Count <= 1)
		{
			return;
		}
		StiDataSortRule stiDataSortRule = userSorts.FirstOrDefault((StiDataSortRule r) => string.Equals(r.Key, column.Key, StringComparison.InvariantCultureIgnoreCase));
		if (stiDataSortRule == null)
		{
			return;
		}
		int num = userSorts.IndexOf(stiDataSortRule) + 1;
		if (num == -1)
		{
			return;
		}
		Font val = new Font("Arial", (float)(7.0 * table.Zoom));
		try
		{
			SolidBrush val2 = new SolidBrush(style.HeaderForeColor);
			try
			{
				StringFormat val3 = new StringFormat();
				try
				{
					val3.LineAlignment = (StringAlignment)1;
					val3.Alignment = (StringAlignment)0;
					val3.Trimming = (StringTrimming)3;
					val3.FormatFlags = (StringFormatFlags)(val3.FormatFlags | 0x1000);
					g.DrawString(num.ToString(), val, (Brush)(object)val2, textRect, val3);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
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

	private static void PaintFilterArrow(Graphics g, RectangleD rectHeader, StiTableElement table, StiTableColumn column, StiTableElementStyle style, bool isMouseOver)
	{
		if (table.UserFilters != null && table.UserFilters.Any((StiDataFilterRule r) => r.Key == column.Key))
		{
			Image val = ((style != null) ? (isMouseOver ? StiReportImages.Actions.DropDownFilterMouseOverWhite() : StiReportImages.Actions.DropDownFilterWhite()) : (isMouseOver ? StiReportImages.Actions.DropDownFilterMouseOver() : StiReportImages.Actions.DropDownFilter()));
			g.DrawImage(val, GetRightImageRect(table, rectHeader, val).ToRectangle());
		}
		else if (isMouseOver)
		{
			Image val2 = ((style == null) ? StiReportImages.Actions.DropDownArrowMouseOver() : StiReportImages.Actions.DropDownArrowMouseOverWhite());
			g.DrawImage(val2, GetRightImageRect(table, rectHeader, val2).ToRectangle());
		}
	}

	private static void PaintSortArrow(Graphics g, RectangleD rect, StiTableElement table, StiTableColumn column, StiTableElementStyle style)
	{
		Image obj;
		switch (StiDataSortRuleHelper.GetSortDirection(table.UserSorts, column.Key))
		{
		case StiDataSortDirection.None:
			return;
		default:
			obj = StiReportImages.Actions.SortArrowDescWhite();
			break;
		case StiDataSortDirection.Ascending:
			obj = StiReportImages.Actions.SortArrowAscWhite();
			break;
		}
		Image val = obj;
		float width = (float)((double)val.Width * table.Zoom);
		float num = (float)((double)val.Height * table.Zoom);
		RectangleF rectangleF = new RectangleF((float)rect.X, (float)(rect.Y + (rect.Height - (double)num) / 2.0), width, num);
		g.DrawImage(val, rectangleF);
		rectangleF.X += rectangleF.Width - (float)StiScale.I4;
		rectangleF.Width = 100000f;
		PaintSortIndex(g, rectangleF, table, column, style);
	}

	private static void PaintArrowMoreColumns(Graphics g, RectangleD rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		SmoothingMode smoothingMode = g.SmoothingMode;
		try
		{
			g.SmoothingMode = (SmoothingMode)4;
			float num = (float)(rect.Height / 10.0);
			RectangleF rectangleF = rect.ToRectangleF();
			PointF[] array = new PointF[3]
			{
				new PointF(rectangleF.Right - num * 5f, rectangleF.Y + num * 3f),
				new PointF(rectangleF.Right - num * 2f, rectangleF.Y + num * 5f),
				new PointF(rectangleF.Right - num * 5f, rectangleF.Y + num * 7f)
			};
			g.FillPolygon(Brushes.White, array);
		}
		finally
		{
			g.SmoothingMode = smoothingMode;
		}
	}

	internal static Color GetBackgroundColor(StiTableElementStyle style, bool isInterlaced)
	{
		if (!isInterlaced)
		{
			return style.CellBackColor;
		}
		return style.AlternatingCellBackColor;
	}

	private static SizeD MeasureColumn(Graphics g, StiTableElement table, StiTableColumn column, object rowValue)
	{
		int height = StiElementConsts.Table.GetHeight(table.Font, table.Zoom * StiScale.Factor);
		SizeD sizeD;
		if (column is StiSparklinesColumn)
		{
			sizeD = MeasureSparklinesCell(table, 0.0, (float)table.Zoom);
		}
		else if (column is StiIndicatorColumn)
		{
			sizeD = MeasureIndicatorCell(g, table, column, rowValue, (float)table.Zoom);
		}
		else if (column is StiDataBarsColumn)
		{
			sizeD = MeasureDataBarsCell(g, table, column, rowValue, (float)table.Zoom);
		}
		else if (column is StiBubbleColumn)
		{
			sizeD = MeasureBubbleCell(g, table, column, rowValue, (float)table.Zoom);
		}
		else
		{
			if (StiImageHelper.IsImage(rowValue))
			{
				return new SizeD(0.0, StiElementConsts.Table.GetHeight(table.Font, table.Zoom));
			}
			sizeD = MeasureCommonCell(g, table, column, rowValue, (float)table.Zoom);
		}
		return new SizeD(sizeD.Width, Math.Max(sizeD.Height, height));
	}

	private static RectangleD GetRightImageRect(StiTableElement table, RectangleD rect, Image image)
	{
		double num = (double)image.Width * table.Zoom;
		double num2 = (double)image.Height * table.Zoom;
		return new RectangleD(rect.Right - (double)StiScale.I3 - num, rect.Y + (rect.Height - num2) / 2.0, num, num2);
	}

	public static SizeD MeasureSparklinesCell(StiTableElement table, double columnWidth, float zoom)
	{
		return new SizeD(Math.Max(columnWidth, 80f * zoom), 0.0);
	}

	public static SizeD MeasureIndicatorCell(Graphics g, StiTableElement table, StiTableColumn column, object rowValue, float zoom)
	{
		StiIndicatorColumn column2 = column as StiIndicatorColumn;
		string cellText = StiIndicatorCellPainter.GetCellText(table, column2, StiValueHelper.TryToNullableDecimal(rowValue));
		SizeD sizeD = MeasureCell(g, cellText, table.Font, table, column);
		sizeD.Width += StiScale.I4;
		sizeD = sizeD.Multiply(zoom);
		sizeD.Width *= 1.25;
		return sizeD;
	}

	public static SizeD MeasureDataBarsCell(Graphics g, StiTableElement table, StiTableColumn column, object rowValue, float zoom)
	{
		string caption = StiTextFormatHelper.FormatBasedOnColumnType(table, column, rowValue);
		SizeD result = MeasureCell(g, caption, table.Font, table, column).Multiply(zoom);
		if (column.HorAlignment != StiHorAlignment.Center)
		{
			result.Width *= 2.0;
		}
		result.Width += StiScale.I2;
		result.Height += StiScale.I2;
		return result;
	}

	public static SizeD MeasureBubbleCell(Graphics g, StiTableElement table, StiTableColumn column, object rowValue, float zoom)
	{
		int height = StiElementConsts.Table.GetHeight(table.Font, table.Zoom * StiScale.Factor);
		if (column.HorAlignment == StiHorAlignment.Center)
		{
			height -= 2;
			return new SizeD(height, StiElementConsts.Table.GetHeight(table.Font));
		}
		string caption = StiTextFormatHelper.FormatBasedOnColumnType(table, column, rowValue);
		SizeD sizeD = MeasureCell(g, caption, table.Font, table, column);
		sizeD.Width += StiScale.I4;
		sizeD = sizeD.Multiply(zoom);
		sizeD.Width += height;
		return sizeD;
	}

	public static SizeD MeasureCommonCell(Graphics g, StiTableElement table, StiTableColumn column, object rowValue, float zoom)
	{
		string caption = StiTextFormatHelper.FormatBasedOnColumnType(table, column, rowValue);
		return MeasureCell(g, caption, table.Font, table, column).Multiply(zoom);
	}

	internal static SizeD MeasureHeader(StiTableElement table, StiTableColumn column)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		Bitmap val = new Bitmap(1, 1);
		try
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				return MeasureHeader(val2, table, column);
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

	private static SizeD MeasureHeader(Graphics g, StiTableElement table, StiTableColumn column)
	{
		string label = StiLabelHelper.GetLabel(column);
		SizeD result = MeasureCell(g, label, table.HeaderFont, table, column, addMargins: false);
		result.Width += StiScale.I(36);
		return result;
	}

	internal static SizeD MeasureCell(string caption, Font baseFont, StiTableElement table, StiTableColumn column)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		Bitmap val = new Bitmap(1, 1);
		try
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				return MeasureCell(val2, caption, baseFont, table, column);
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

	private static SizeD MeasureCell(Graphics g, string caption, Font baseFont, StiTableElement table, StiTableColumn column, bool addMargins = true)
	{
		SizeD sizeD = new SizeD(8.0, StiElementConsts.Table.GetHeight(baseFont));
		if (string.IsNullOrEmpty(caption))
		{
			return sizeD;
		}
		StiTableColumnSizeHelper stiTableColumnSizeHelper = new StiTableColumnSizeHelper(baseFont, column);
		string key = caption + stiTableColumnSizeHelper.GetUniqueCode() + StiScale.Factor;
		if (captionSizeCache[key] is SizeD)
		{
			return (SizeD)captionSizeCache[key];
		}
		SizeF cellSize = GetCellSize(g, caption, stiTableColumnSizeHelper, baseFont);
		sizeD.Width = cellSize.Width;
		sizeD.Height = cellSize.Height;
		if (addMargins)
		{
			sizeD.Width += StiScale.I8;
			sizeD.Height += StiScale.I8;
		}
		captionSizeCache[key] = sizeD;
		return sizeD;
	}

	private static SizeF GetCellSize(Graphics g, string caption, StiTableColumnSizeHelper columnSize, Font baseFont)
	{
		object obj = StringFormat.GenericTypographic.Clone();
		StringFormat val = (StringFormat)((obj is StringFormat) ? obj : null);
		try
		{
			Font gdiFont = StiElementConsts.Table.Font.GetGdiFont(1.0, null, baseFont);
			try
			{
				val.FormatFlags = (StringFormatFlags)0;
				int num = StiScale.I((columnSize.Width != 0) ? columnSize.Width : columnSize.MaxWidth);
				SizeF result = g.MeasureString(caption, gdiFont, num, val);
				result.Width = (float)columnSize.GetColumnWidth(result.Width);
				result.Height = (float)columnSize.GetColumnHeight(result.Height);
				return result;
			}
			finally
			{
				((IDisposable)gdiFont)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	internal static SizeD MeasureHeaders(StiTableElement table, Font baseFont)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		Bitmap val = new Bitmap(1, 1);
		try
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				object obj = StringFormat.GenericTypographic.Clone();
				StringFormat val3 = (StringFormat)((obj is StringFormat) ? obj : null);
				try
				{
					Font gdiFont = StiElementConsts.Table.Font.GetGdiFont(1.0, null, baseFont);
					try
					{
						SizeD result = new SizeD(8.0, StiElementConsts.Table.GetHeight(gdiFont));
						foreach (StiTableColumn column in table.Columns)
						{
							SizeD sizeD = new SizeD(StiScale.I8, 0.0);
							string label = StiLabelHelper.GetLabel(column);
							if (!string.IsNullOrEmpty(label))
							{
								SizeD sizeD2 = MeasureHeader(val2, table, column);
								sizeD.Width += sizeD2.Width;
								result.Height = Math.Max(result.Height, sizeD2.Height);
							}
							result.Width += sizeD.Width;
						}
						if (result.Height > (double)StiElementConsts.Table.GetHeight(gdiFont))
						{
							result.Height += StiScale.I16;
						}
						return result;
					}
					finally
					{
						((IDisposable)gdiFont)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
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
}
