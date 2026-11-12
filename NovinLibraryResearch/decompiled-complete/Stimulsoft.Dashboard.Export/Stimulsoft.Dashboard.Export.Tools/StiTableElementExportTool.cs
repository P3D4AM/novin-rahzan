using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Meters;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Dashboard.Drawing.Painters;
using Stimulsoft.Dashboard.Drawing.Painters.Table;
using Stimulsoft.Dashboard.Export.Helpers;
using Stimulsoft.Dashboard.Export.Settings;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Functions;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Export;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Import;

namespace Stimulsoft.Dashboard.Export.Tools;

public class StiTableElementExportTool : StiElementExportTool
{
	public override void Render(IStiElement element, StiPanel destination, RectangleD rect, StiDashboardExportSettings settings)
	{
		if (settings == null || RenderEmptyDataMessage(element, destination, rect, settings))
		{
			return;
		}
		StiReport report = destination.Report;
		StiPage page = destination.Page;
		StiTableElement table = element as StiTableElement;
		bool flag = StiExportSettingsHelper.IsDataExport(settings);
		bool flag2 = settings.RenderSingleElement && (settings.Format == StiDashboardExportFormat.Excel || settings.Format == StiDashboardExportFormat.Html || settings.Format == StiDashboardExportFormat.Image);
		if (!settings.RenderSinglePage || flag2)
		{
			rect.Height = 2147483647.0;
		}
		StiPanel tempDestination = destination.Clone(cloneProperties: true, cloneComponents: false) as StiPanel;
		RenderCells(tempDestination, rect, table, 1.0, settings.Format, flag, settings.IsDesignMode);
		if (flag2)
		{
			FitToTable(element, destination, rect, settings, tempDestination.Components.ToList(), flag);
		}
		int num = 0;
		double num2 = destination.Top;
		if (tempDestination.Components.Count > 0)
		{
			num2 += tempDestination.GetComponentsList().Min((StiComponent c) => c.Top);
		}
		while (true)
		{
			(from c in tempDestination.GetComponentsList()
				where c.Bottom <= destination.Height
				select c).ToList().ForEach(delegate(StiComponent c)
			{
				tempDestination.Components.Remove(c);
				if (c.Left < destination.Width || settings.Format == StiDashboardExportFormat.Data)
				{
					destination.Components.Add(c);
					if (c.Right > destination.Width && settings.Format != StiDashboardExportFormat.Data)
					{
						c.Width = Math.Max(0.0, destination.Width - c.Left);
					}
				}
			});
			if ((settings != null && settings.RenderSinglePage) || tempDestination.Components.Count == 0)
			{
				break;
			}
			double minTop = tempDestination.GetComponentsList().Min((StiComponent c) => c.Top);
			tempDestination.GetComponentsList().ForEach(delegate(StiComponent c)
			{
				c.Top = c.Top - minTop + 1.0;
			});
			if (report.RenderedPages.Count <= ++num)
			{
				report.RenderedPages.Add((StiPage)page.Clone(cloneProperties: true, cloneComponents: false));
			}
			page = report.RenderedPages[num];
			destination = (StiPanel)destination.Clone(cloneProperties: true, cloneComponents: false);
			if (settings == null || !settings.RenderSingleElement)
			{
				destination.Top = num2;
			}
			else if (!flag)
			{
				destination.Top = ((IStiMargin)element).Margin.Top;
				destination.Left = ((IStiMargin)element).Margin.Left;
			}
			page.Components.Add(destination);
		}
		foreach (StiPage renderedPage in destination.Report.RenderedPages)
		{
			renderedPage.GetComponentsList().ForEach(delegate(StiComponent c)
			{
				c.Report = report;
				c.Page = page;
			});
		}
	}

	public static List<object> RenderCellsForViewer(IStiTableElement element, double tableWidth)
	{
		//IL_047d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_0488: Unknown result type (might be due to invalid IL or missing references)
		//IL_048f: Expected O, but got Unknown
		StiTableElement stiTableElement = element as StiTableElement;
		StiDataTable stiDataTable = StiElementDataCache.TryToGetOrCreate(stiTableElement);
		if (stiDataTable == null)
		{
			return null;
		}
		int num = 1;
		List<object> list = new List<object>();
		int height = StiElementConsts.Table.GetHeight(stiTableElement.Font, num);
		IEnumerable<StiTableColumn> enumerable = stiDataTable.Meters.Where((IStiMeter c) => c is StiTableColumn).Cast<StiTableColumn>();
		bool flag = enumerable.FirstOrDefault((StiTableColumn xx) => xx.ShowTotalSummary) != null;
		int num2 = 0;
		double num3 = 0.0;
		foreach (StiTableColumn item in enumerable)
		{
			if (!StiTableColumnVisibilityHelper.GetVisible(item, stiTableElement.Report))
			{
				num2++;
				continue;
			}
			double num4 = MeasureHeader(stiTableElement, item, num);
			if (item is StiSparklinesColumn)
			{
				num4 = MeasureSparklinesCell(num, num4);
			}
			else
			{
				foreach (object[] row in stiDataTable.Rows)
				{
					object rowValue = row[num2];
					double val = ((!(item is StiIndicatorColumn)) ? ((!(item is StiDataBarsColumn)) ? MeasureCommonCell(stiTableElement, item, num, rowValue, num4).Width : MeasureDataBarsCell(stiTableElement, item, num, rowValue)) : MeasureIndicatorCell(stiTableElement, item, num, rowValue));
					num4 = Math.Max(num4, val);
				}
				if (item.ShowTotalSummary)
				{
					decimal num5 = StiTableElementHelper.CalculateTotal(item, num2, stiDataTable);
					double val2 = MeasureFooter(stiTableElement, item, num, num5);
					num4 = Math.Max(num4, val2);
				}
			}
			decimal min = StiTableElementHelper.MinDataColumnValue(item, stiDataTable, num2);
			decimal max = StiTableElementHelper.MaxDataColumnValue(item, stiDataTable, num2);
			int num6 = 0;
			num3 += num4;
			foreach (object[] row2 in stiDataTable.Rows)
			{
				if (list.Count < stiDataTable.Rows.Count)
				{
					list.Add(new List<object>());
				}
				object obj = row2[num2];
				if (obj is StiSystemVariableObject)
				{
					obj = Funcs.GetSystemVariable((StiSystemVariableObject)obj, num6 + 1);
				}
				RectangleD rect = new RectangleD(0.0, 0.0, num4, height);
				StiPanel stiPanel = new StiPanel();
				RenderCell(stiPanel, rect, stiTableElement, row2, enumerable.Select((StiTableColumn c) => c.Key).ToArray(), item, num, obj, min, max, (num6 & 1) == 1, StiDashboardExportFormat.Html5, exportDataOnly: false);
				Hashtable hashtable = new Hashtable();
				if (stiTableElement.TableConditions.Count > 0 && stiPanel.Components.Count > 0)
				{
					StiComponent stiComponent = stiPanel.Components[0];
					if (!(stiComponent is StiText))
					{
						if (stiComponent is IStiBrush)
						{
							hashtable["backColor"] = ColorTranslator.ToHtml(StiBrush.ToColor(((IStiBrush)stiComponent).Brush));
						}
						if (stiComponent is IStiTextBrush)
						{
							hashtable["textColor"] = ColorTranslator.ToHtml(StiBrush.ToColor(((IStiTextBrush)stiComponent).TextBrush));
						}
					}
				}
				if (obj != null && obj is bool)
				{
					hashtable["boolValue"] = obj;
				}
				else
				{
					foreach (object component in stiPanel.Components)
					{
						if (component is StiText)
						{
							hashtable["text"] = ((StiText)component).Text?.Value;
							hashtable["textColor"] = ColorTranslator.ToHtml(StiBrush.ToColor(((StiText)component).TextBrush));
							hashtable["backColor"] = ColorTranslator.ToHtml(StiBrush.ToColor(((StiText)component).Brush));
							hashtable["textWidth"] = ((StiText)component).Width;
							hashtable["textHeight"] = ((StiText)component).Height;
							if (stiTableElement.TableConditions.Count > 0)
							{
								Font val3 = ((IStiFont)stiTableElement)?.Font;
								FontStyle val4 = StiTableElementConditionHelper.ProcessFontStyle(val3, item.Key, stiTableElement, row2, enumerable.Select((StiTableColumn c) => c.Key).ToArray());
								val3 = new Font(val3, val4);
								hashtable["textFontStyle"] = new Hashtable
								{
									["bold"] = val3.Bold,
									["italic"] = val3.Italic,
									["underline"] = val3.Underline,
									["strikeout"] = val3.Strikeout
								};
							}
							if (obj != null && obj.ToString() != hashtable["text"] as string)
							{
								hashtable["value"] = obj.ToString();
							}
						}
						else if (component is StiImage)
						{
							hashtable["image"] = ImageToBase64(((StiImage)component).ImageBytesToDraw);
							hashtable["imageWidth"] = ((StiImage)component).Width;
							hashtable["imageHeight"] = ((StiImage)component).Height;
						}
						else if (component is StiSparkline)
						{
							hashtable["sparkline"] = component;
						}
					}
				}
				if (hashtable["boolValue"] == null && hashtable["text"] == null && hashtable["image"] == null && hashtable["sparkline"] == null)
				{
					hashtable["text"] = obj;
				}
				((List<object>)list[num6]).Add(hashtable);
				num6++;
			}
			if (flag)
			{
				if (list.Count < stiDataTable.Rows.Count + 1)
				{
					list.Add(new List<object>());
				}
				Hashtable hashtable2 = new Hashtable();
				hashtable2["text"] = string.Empty;
				if (item.ShowTotalSummary)
				{
					decimal num7 = StiTableElementHelper.CalculateTotal(item, num2, stiDataTable);
					hashtable2["text"] = ((item.SummaryType == StiSummaryColumnType.Count) ? ((int)num7).ToString() : StiTextFormatHelper.FormatBasedOnColumnType(stiTableElement, item, num7));
				}
				((List<object>)list[list.Count - 1]).Add(hashtable2);
			}
			num2++;
		}
		double factorX = ((stiTableElement.SizeMode == StiTableSizeMode.Fit) ? (tableWidth / num3) : 1.0);
		int cellPadding = ((stiTableElement.SizeMode == StiTableSizeMode.Fit) ? 8 : 0);
		list.ForEach(delegate(object cellsRow)
		{
			((List<object>)cellsRow).ForEach(delegate(object cell)
			{
				Hashtable hashtable3 = cell as Hashtable;
				if (hashtable3.Contains("sparkline"))
				{
					StiSparkline stiSparkline = hashtable3["sparkline"] as StiSparkline;
					hashtable3["sparkline"] = StiHtmlExportService.GetSparklineData(null, stiSparkline, stiSparkline.Width * factorX - (double)cellPadding, stiSparkline.Height);
					hashtable3["sparklineWidth"] = stiSparkline.Width - (double)cellPadding;
					hashtable3["sparklineHeight"] = stiSparkline.Height;
				}
			});
		});
		return list;
	}

	private static void FitToTable(IStiElement element, StiPanel destination, RectangleD rect, StiDashboardExportSettings settings, List<StiComponent> cells, bool exportDataOnly)
	{
		rect.Width = cells.Max((StiComponent c) => c.Right);
		rect.Height = cells.Max((StiComponent c) => c.Bottom);
		if (!exportDataOnly)
		{
			StiPadding padding = ((IStiPadding)element).Padding;
			rect.Width = cells.Max((StiComponent c) => c.Right) + padding.Right;
			rect.Height = cells.Max((StiComponent c) => c.Bottom) + padding.Bottom;
			StiText stiText = ((destination.Components.Count > 0) ? (destination.Components[destination.Components.Count - 1] as StiText) : null);
			if (stiText != null)
			{
				stiText.Width = rect.Width;
			}
			StiMargin margin = ((IStiMargin)element).Margin;
			rect.Width += margin.Left + margin.Right;
			rect.Height += margin.Top + margin.Bottom;
		}
		if (settings.Format == StiDashboardExportFormat.Image)
		{
			rect.Width = Math.Min(rect.Width, ((StiImageDashboardExportSettings)settings).MaxWidth);
			rect.Height = Math.Min(rect.Height, ((StiImageDashboardExportSettings)settings).MaxHeight);
		}
		double width2 = (destination.Page.Width = rect.Width);
		destination.Width = width2;
		width2 = (destination.Page.Height = rect.Height);
		destination.Height = width2;
	}

	private static void RenderCells(StiPanel destination, RectangleD rect, StiTableElement table, double zoom, StiDashboardExportFormat format, bool exportDataOnly, bool designMode)
	{
		StiDataTable stiDataTable = StiElementDataCache.TryToGetOrCreate(table);
		if (stiDataTable == null)
		{
			return;
		}
		StiScale.Lock();
		Font headerFont = table.HeaderFont;
		Font font = table.Font;
		Font footerFont = table.FooterFont;
		table.HeaderFont = StiElementConsts.Table.Font.GetGdiFont(StiDpiHelper.DeviceCapsScale, null, table.HeaderFont);
		table.Font = StiElementConsts.Table.Font.GetGdiFont(StiDpiHelper.DeviceCapsScale, null, table.Font);
		table.FooterFont = StiElementConsts.Table.Font.GetGdiFont(StiDpiHelper.DeviceCapsScale, null, table.FooterFont);
		int height = StiElementConsts.Table.GetHeight(table.HeaderFont, zoom);
		int height2 = StiElementConsts.Table.GetHeight(table.Font, zoom);
		IEnumerable<StiTableColumn> enumerable = stiDataTable.Meters.Where((IStiMeter c) => c is StiTableColumn).Cast<StiTableColumn>();
		bool flag = enumerable.FirstOrDefault((StiTableColumn xx) => xx.ShowTotalSummary) != null;
		int num = (flag ? StiElementConsts.Table.GetHeight(table.FooterFont, zoom) : 0);
		List<double> list = new List<double>();
		List<double> list2 = new List<double>();
		int num2 = 0;
		int num3 = 0;
		foreach (StiTableColumn item in enumerable)
		{
			double num4 = 0.0;
			if (StiTableColumnVisibilityHelper.GetVisible(item, table.Report))
			{
				num4 = MeasureHeader(table, item, zoom);
				if (item.ShowTotalSummary)
				{
					decimal num5 = StiTableElementHelper.CalculateTotal(item, num2, stiDataTable);
					double val = MeasureFooter(table, item, zoom, num5);
					num4 = Math.Max(num4, val);
				}
			}
			list.Add(num4);
			num2++;
		}
		double num6 = rect.Y + (double)height;
		if (num6 < rect.Bottom - (double)num)
		{
			foreach (object[] row in stiDataTable.Rows)
			{
				double num7 = height2;
				num2 = -1;
				foreach (StiTableColumn item2 in enumerable)
				{
					num2++;
					if (!StiTableColumnVisibilityHelper.GetVisible(item2, table.Report))
					{
						continue;
					}
					double num8 = list[num2];
					if (item2 is StiSparklinesColumn)
					{
						num8 = MeasureSparklinesCell(zoom, num8);
					}
					else
					{
						object rowValue = row[num2];
						double val2;
						if (item2 is StiIndicatorColumn)
						{
							val2 = MeasureIndicatorCell(table, item2, zoom, rowValue);
						}
						else if (item2 is StiDataBarsColumn)
						{
							val2 = MeasureDataBarsCell(table, item2, zoom, rowValue);
						}
						else
						{
							SizeD sizeD = MeasureCommonCell(table, item2, zoom, rowValue, num8);
							val2 = sizeD.Width;
							num7 = Math.Max(sizeD.Height, num7);
						}
						num8 = Math.Max(num8, val2);
					}
					list[num2] = num8;
				}
				if (num6 + num7 > rect.Bottom - (double)num)
				{
					num7 = rect.Bottom - (double)num - num6;
					if (num7 < 1.2)
					{
						num7 = 1.2;
					}
				}
				list2.Add(num7);
				num6 += num7;
				num3++;
				if (num6 >= rect.Bottom - (double)num)
				{
					break;
				}
			}
			list2.Add(height2);
		}
		if (table.SizeMode == StiTableSizeMode.Fit)
		{
			double factorX = rect.Width / list.Sum();
			if (factorX > 1.0 || !designMode)
			{
				list = list.Select((double w) => w * factorX).ToList();
			}
			num3 = -1;
			foreach (object[] row2 in stiDataTable.Rows)
			{
				num3++;
				if (num3 >= list2.Count)
				{
					break;
				}
				num2 = -1;
				double num9 = list2[num3];
				foreach (StiTableColumn item3 in enumerable)
				{
					num2++;
					if (!(item3 is StiIndicatorColumn) && !(item3 is StiIndicatorColumn) && !(item3 is StiDataBarsColumn) && item3 is IStiTableColumnSize)
					{
						IStiTableColumnSize stiTableColumnSize = item3 as IStiTableColumnSize;
						int width = stiTableColumnSize.Size.Width;
						stiTableColumnSize.Size.Width = (int)list[num2] - StiScale.I4;
						num9 = Math.Max(MeasureCommonCell(table, item3, zoom, row2[num2], stiTableColumnSize.Size.Width).Height, num9);
						stiTableColumnSize.Size.Width = width;
					}
				}
				list2[num3] = num9;
			}
		}
		table.HeaderFont = headerFont;
		table.Font = font;
		table.FooterFont = footerFont;
		StiScale.Unlock();
		double num10 = rect.X;
		num2 = 0;
		foreach (StiTableColumn item4 in enumerable)
		{
			if (!StiTableColumnVisibilityHelper.GetVisible(item4, table.Report))
			{
				num2++;
				continue;
			}
			double num11 = list[num2];
			if (format == StiDashboardExportFormat.Excel)
			{
				num11 *= 1.1;
			}
			num6 = rect.Y;
			if (format != StiDashboardExportFormat.Data)
			{
				RectangleD rect2 = new RectangleD(num10, num6, num11, height);
				RenderHeader(destination, rect2, table, item4, zoom, format, exportDataOnly);
				num6 += rect2.Height;
			}
			if (num6 < rect.Bottom - (double)num)
			{
				decimal min = StiTableElementHelper.MinDataColumnValue(item4, stiDataTable, num2);
				decimal max = StiTableElementHelper.MaxDataColumnValue(item4, stiDataTable, num2);
				num3 = 0;
				foreach (object[] row3 in stiDataTable.Rows)
				{
					double num12 = Math.Ceiling(list2[num3]);
					object obj = row3[num2];
					if (obj is StiSystemVariableObject)
					{
						obj = Funcs.GetSystemVariable((StiSystemVariableObject)obj, num3 + 1);
					}
					RectangleD rect3 = new RectangleD(num10, num6, num11, num12);
					RenderCell(destination, rect3, table, row3, enumerable.Select((StiTableColumn c) => c.Key).ToArray(), item4, zoom, obj, min, max, (num3 & 1) == 1, format, exportDataOnly);
					num6 += num12;
					num3++;
					if (num6 >= rect.Bottom - (double)num)
					{
						break;
					}
				}
			}
			if (format != StiDashboardExportFormat.Data && !exportDataOnly && flag)
			{
				RectangleD rect4 = new RectangleD(num10, designMode ? (rect.Bottom - (double)num) : num6, num11, num);
				RenderFooter(destination, rect4, table, item4, num2, zoom, stiDataTable);
			}
			num10 += num11;
			num2++;
		}
	}

	private static double MeasureSparklinesCell(double zoom, double columnWidth)
	{
		return Math.Max(columnWidth, 80.0 * zoom);
	}

	private static double MeasureHeader(StiTableElement table, StiTableColumn column, double zoom)
	{
		return StiTableElementGdiPainter.MeasureHeader(table, column).Width * zoom;
	}

	private static double MeasureFooter(StiTableElement table, StiTableColumn column, double zoom, object value)
	{
		string caption = StiTextFormatHelper.FormatBasedOnColumnType(table, column, value);
		return StiTableElementGdiPainter.MeasureCell(caption, table.FooterFont, table, column).Width * zoom;
	}

	private static double MeasureIndicatorCell(StiTableElement table, StiTableColumn column, double zoom, object rowValue)
	{
		StiIndicatorColumn column2 = column as StiIndicatorColumn;
		string cellText = StiIndicatorCellPainter.GetCellText(table, column2, StiValueHelper.TryToNullableDecimal(rowValue));
		return StiTableElementGdiPainter.MeasureCell(cellText, table.Font, table, column).Width * zoom * 1.25;
	}

	private static double MeasureDataBarsCell(StiTableElement table, StiTableColumn column, double zoom, object rowValue)
	{
		string caption = StiTextFormatHelper.FormatBasedOnColumnType(table, column, rowValue);
		double num = StiTableElementGdiPainter.MeasureCell(caption, table.Font, table, column).Width * zoom;
		if (column.HorAlignment != StiHorAlignment.Center)
		{
			num *= 2.0;
		}
		return num;
	}

	private static SizeD MeasureCommonCell(StiTableElement table, StiTableColumn column, double zoom, object rowValue, double columnWidth)
	{
		if (Stimulsoft.Report.Helpers.StiImageHelper.IsImage(rowValue))
		{
			return new SizeD(columnWidth, 0.0);
		}
		string caption = StiTextFormatHelper.FormatBasedOnColumnType(table, column, rowValue);
		return StiTableElementGdiPainter.MeasureCell(caption, table.Font, table, column).Multiply(zoom);
	}

	private static void RenderHeader(StiPanel destination, RectangleD rect, StiTableElement table, StiTableColumn column, double zoom, StiDashboardExportFormat format, bool exportDataOnly)
	{
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		string text = ((column != null) ? StiLabelHelper.GetLabel(column) : string.Empty);
		string name = (string.IsNullOrWhiteSpace(text) ? (table.Name + "_Column") : StiNameValidator.CorrectName(text));
		StiTableElementStyle tableStyle = StiDashboardStyleHelper.GetTableStyle(table);
		bool flag = table.Columns.IndexOf(column) != 0 && rect.Left < destination.Width && !exportDataOnly;
		bool flag2 = table.Columns.IndexOf(column) != table.Columns.Count - 1 && rect.Right <= destination.Width && !exportDataOnly;
		StiBorderSides side = (flag ? StiBorderSides.Left : StiBorderSides.None) | (flag2 ? StiBorderSides.Right : StiBorderSides.None);
		Color color = (exportDataOnly ? Color.Black : GetHeaderForeColor(table));
		Color color2 = (exportDataOnly ? Color.Transparent : tableStyle.HeaderBackColor);
		Font baseFont = (Font)((!exportDataOnly) ? ((object)table.HeaderFont) : ((object)new Font("Arial", 10f, (FontStyle)1)));
		StiText stiText = new StiText(rect);
		stiText.Name = name;
		stiText.AllowHtmlTags = true;
		stiText.AutoWidth = false;
		stiText.HorAlignment = StiTextHorAlignment.Center;
		stiText.VertAlignment = StiVertAlignment.Center;
		stiText.WordWrap = false;
		stiText.Border = new StiBorder(side, tableStyle.LineColor, 1.0, StiPenStyle.Solid);
		stiText.Brush = new StiSolidBrush(color2);
		stiText.Font = StiElementConsts.Table.Font.GetGdiFont(zoom, null, baseFont);
		stiText.Text = text;
		stiText.TextFormat = column.TextFormat?.Clone() as StiFormatService;
		stiText.TextBrush = new StiSolidBrush(color);
		stiText.ComponentPlacement = "h." + table.Name;
		stiText.Trimming = (StringTrimming)3;
		StiText component = stiText;
		destination.Components.Add(component);
	}

	private static void RenderFooter(StiPanel destination, RectangleD rect, StiTableElement table, StiTableColumn column, int columnIndex, double zoom, StiDataTable dataTable)
	{
		StiTableElementStyle tableStyle = StiDashboardStyleHelper.GetTableStyle(table);
		Color footerForeColor = GetFooterForeColor(table);
		Color footerBackColor = tableStyle.FooterBackColor;
		Font footerFont = table.FooterFont;
		string text = string.Empty;
		if (column.ShowTotalSummary)
		{
			decimal num = StiTableElementHelper.CalculateTotal(column, columnIndex, dataTable);
			text = ((column.SummaryType == StiSummaryColumnType.Count) ? ((int)num).ToString() : StiTextFormatHelper.FormatBasedOnColumnType(table, column, num));
		}
		StiTextHorAlignment horAlignment = StiTextHorAlignment.Right;
		if (column.SummaryAlignment == StiHorAlignment.Left)
		{
			horAlignment = StiTextHorAlignment.Left;
		}
		else if (column.SummaryAlignment == StiHorAlignment.Center)
		{
			horAlignment = StiTextHorAlignment.Center;
		}
		destination.Components.Add(new StiText(rect)
		{
			Name = table.Name + "_FooterCell",
			Border = GetBorderSides(destination, rect, table, column, StiBorderSides.All),
			Brush = new StiSolidBrush(footerBackColor),
			ExcelDataValue = text,
			Font = StiElementConsts.Table.Font.GetGdiFont(zoom, null, footerFont),
			HorAlignment = horAlignment,
			VertAlignment = StiVertAlignment.Center,
			Margins = new StiMargins(0.0, 0.0, 1.0, 0.0),
			TextQuality = StiTextQuality.Typographic,
			Text = text,
			TextBrush = new StiSolidBrush(footerForeColor),
			TextFormat = (column.TextFormat?.Clone() as StiFormatService),
			WordWrap = false,
			ComponentPlacement = "f." + table.Name
		});
	}

	internal static void RenderCell(StiPanel destination, RectangleD rect, StiTableElement table, object[] rowValues, string[] columnKeys, StiTableColumn column, double zoom, object value, decimal min, decimal max, bool? isInterlaced, StiDashboardExportFormat format, bool exportDataOnly)
	{
		StiTableElementStyle tableStyle = StiDashboardStyleHelper.GetTableStyle(table);
		Color foreColor = GetForeColor(table, column, tableStyle, isInterlaced.GetValueOrDefault(false));
		Color backgroundColor = StiTableElementGdiPainter.GetBackgroundColor(tableStyle, isInterlaced.GetValueOrDefault(false));
		backgroundColor = StiTableElementConditionHelper.ProcessBackColor(backgroundColor, column.Key, table, rowValues, columnKeys).GetValueOrDefault();
		if (exportDataOnly)
		{
			RenderTextCell(destination, rect, table, rowValues, columnKeys, column, zoom, value, StiBorderSides.None, Color.Black, Color.Transparent, exportDataOnly: true);
		}
		else if (column is StiDataBarsColumn || column is StiSparklinesColumn || column is StiIndicatorColumn || column is StiBubbleColumn)
		{
			RenderGraphicCell(destination, rect, table, rowValues, columnKeys, column, zoom, value, min, max, backgroundColor, tableStyle, isInterlaced.GetValueOrDefault(false), format);
		}
		else if (column is StiColorScaleColumn)
		{
			DrawColorScaleColumn(destination, rect, table, rowValues, columnKeys, column, zoom, value, min, max, tableStyle, format);
		}
		else if (Stimulsoft.Report.Helpers.StiImageHelper.IsImage(value))
		{
			RenderImageCell(destination, rect, table, column, value, backgroundColor, format);
		}
		else if (value is bool)
		{
			RenderBoolCell(destination, rect, table, rowValues, columnKeys, column, zoom, (bool)value, foreColor, backgroundColor, format);
		}
		else
		{
			RenderTextCell(destination, rect, table, rowValues, columnKeys, column, zoom, value, StiBorderSides.All, foreColor, backgroundColor);
		}
	}

	private static void RenderGraphicCell(StiPanel destination, RectangleD rect, StiTableElement table, object[] rowValues, string[] columnKeys, StiTableColumn column, double zoom, object value, decimal min, decimal max, Color backColor, StiTableElementStyle style, bool isInterlaced, StiDashboardExportFormat format)
	{
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Expected O, but got Unknown
		string value2 = string.Empty;
		RectangleD rect2 = rect;
		Color color = GetForeColor(table, column, style, isInterlaced);
		int num = (int)rect.Width;
		int num2 = (int)rect.Height;
		if (column is StiIndicatorColumn)
		{
			num *= 3;
			num2 *= 3;
		}
		if (column is StiSparklinesColumn)
		{
			decimal[] values = StiSparklinesCellPainter.CastToArray(value)?.Select(StiValueHelper.TryToDecimal)?.ToArray();
			StiSparklinesColumn stiSparklinesColumn = column as StiSparklinesColumn;
			StiSparkline component = new StiSparkline(rect)
			{
				Values = values,
				PositiveColor = (stiSparklinesColumn.AllowCustomColors ? stiSparklinesColumn.PositiveColor : style.CellSparkline),
				NegativeColor = (stiSparklinesColumn.AllowCustomColors ? stiSparklinesColumn.NegativeColor : style.CellWinLossNegative),
				ShowFirstLastPoints = stiSparklinesColumn.ShowFirstLastPoints,
				ShowHighLowPoints = stiSparklinesColumn.ShowHighLowPoints,
				Type = (StiSparklineType)stiSparklinesColumn.Type,
				ComponentPlacement = "d." + table.Name,
				Border = GetBorderSides(destination, rect, table, column, StiBorderSides.All),
				Brush = new StiSolidBrush(backColor)
			};
			destination.Components.Add(component);
			return;
		}
		Bitmap val = new Bitmap(num, num2);
		StiBorderSides borderSides = StiBorderSides.None;
		string text = "GraphicCell";
		StiHorAlignment horAlignment = column.HorAlignment;
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			RectangleD rect3 = new RectangleD(0.0, 0.0, rect.Width, rect.Height);
			StiDrawing.FillRectangle(val2, backColor, new RectangleD(0.0, 0.0, num, num2));
			if (column is StiIndicatorColumn)
			{
				val2.ScaleTransform(3f, 3f);
			}
			if (column is StiDataBarsColumn)
			{
				StiDataBarsCellPainter.Draw(val2, rect3, table, rowValues, columnKeys, column, zoom, StiValueHelper.TryToDecimal(value), min, max, isInterlaced, isSelected: false, drawText: false);
				if (format != StiDashboardExportFormat.Data)
				{
					rect2 = StiDataBarsCellPainter.CalculateTextRect(rect, column);
					rect2.Inflate(2.0, 2.0);
				}
				value2 = StiTextFormatHelper.FormatBasedOnColumnType(table, column, value);
				color = StiDataBarsCellPainter.GetForeColor(table, column, style, isInterlaced, isSelected: false);
				decimal num3 = StiValueHelper.TryToDecimal(value);
				if (num3 < 0m && StiNegativeColorChecker.IsNegativeInRed(column.TextFormat))
				{
					color = StiOptions.Engine.NegativeColor;
				}
				color = StiTableElementConditionHelper.ProcessForeColor(color, column.Key, table, rowValues, columnKeys);
				switch (column.HorAlignment)
				{
				case StiHorAlignment.Left:
					borderSides = StiBorderSides.Left;
					break;
				case StiHorAlignment.Right:
					borderSides = StiBorderSides.Right;
					break;
				case StiHorAlignment.Center:
					borderSides = StiBorderSides.Left | StiBorderSides.Right;
					break;
				}
				text = "DataBarsCell";
			}
			else if (column is StiIndicatorColumn)
			{
				StiIndicatorCellPainter.Draw(val2, rect3, table, rowValues, columnKeys, column as StiIndicatorColumn, zoom, StiValueHelper.TryToNullableDecimal(value), style, drawText: false);
				rect2 = StiIndicatorCellPainter.CalculateTextRect(rect, column as StiIndicatorColumn, zoom, rect2.Width, table.IsExporting);
				value2 = StiIndicatorCellPainter.GetCellText(table, column as StiIndicatorColumn, StiValueHelper.TryToNullableDecimal(value));
				color = StiIndicatorCellPainter.GetColor(column.ForeColor, StiValueHelper.TryToNullableDecimal(value), style);
				color = StiTableElementConditionHelper.ProcessForeColor(color, column.Key, table, rowValues, columnKeys);
				switch (column.HorAlignment)
				{
				case StiHorAlignment.Left:
					borderSides = StiBorderSides.Right;
					break;
				case StiHorAlignment.Center:
				case StiHorAlignment.Right:
					borderSides = StiBorderSides.Left;
					break;
				}
				text = "IndicatorCell";
			}
			else if (column is StiBubbleColumn)
			{
				StiBubbleCellPainter.Draw(val2, rect3, table, column as StiBubbleColumn, zoom, StiValueHelper.TryToDecimal(value), min, max, isInterlaced, isSelected: false, drawText: false);
				if (format != StiDashboardExportFormat.Data)
				{
					rect2 = StiBubbleCellPainter.CalculateTextRect(table.Font, rect, column as StiBubbleColumn, table.Zoom);
					rect2.Inflate(2.0, 2.0);
				}
				value2 = StiBubbleCellPainter.GetCellText(table, column as StiBubbleColumn, StiValueHelper.TryToDecimal(value));
				color = StiBubbleCellPainter.GetForeColor(table, column, style, isInterlaced, isSelected: false);
				color = StiTableElementConditionHelper.ProcessForeColor(color, column.Key, table, rowValues, columnKeys);
				switch (column.HorAlignment)
				{
				case StiHorAlignment.Left:
					borderSides = StiBorderSides.Left;
					break;
				case StiHorAlignment.Right:
					borderSides = StiBorderSides.Right;
					break;
				}
				text = "BubbleCell";
				if (column.HorAlignment == StiHorAlignment.Center && format != StiDashboardExportFormat.Data)
				{
					value2 = null;
				}
				if (column.HorAlignment == StiHorAlignment.Left)
				{
					horAlignment = StiHorAlignment.Right;
				}
				if (column.HorAlignment == StiHorAlignment.Right)
				{
					horAlignment = StiHorAlignment.Left;
				}
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		if (format != StiDashboardExportFormat.Data)
		{
			destination.Components.Add(new StiImage(rect)
			{
				Name = table.Name + "_" + text,
				Border = GetBorderSides(destination, rect, table, column, StiBorderSides.All),
				Stretch = true,
				AspectRatio = (column is StiIndicatorColumn || column is StiBubbleColumn),
				ImageToDraw = (Image)(object)val,
				HorAlignment = horAlignment,
				Brush = new StiSolidBrush(backColor)
			});
		}
		if (format == StiDashboardExportFormat.Data || !string.IsNullOrEmpty(value2))
		{
			RenderTextCell(destination, rect2, table, rowValues, columnKeys, column, zoom, value, borderSides, color, backColor, exportDataOnly: false, false);
		}
	}

	private static void DrawColorScaleColumn(StiPanel destination, RectangleD rect, StiTableElement table, object[] rowValues, string[] columnKeys, StiTableColumn column, double zoom, object value, decimal min, decimal max, StiTableElementStyle style, StiDashboardExportFormat format)
	{
		StiColorScaleColumn stiColorScaleColumn = column as StiColorScaleColumn;
		Color foreColor = GetForeColor(table, column, style, isInterlaced: false);
		foreColor = StiTableElementConditionHelper.ProcessForeColor(foreColor, column.Key, table, rowValues, columnKeys);
		Color scaleColor = StiColorScaleCellPainter.GetScaleColor(StiValueHelper.TryToDecimal(value), min, max, style.CellBackColor, stiColorScaleColumn.MinimumColor, stiColorScaleColumn.MaximumColor);
		scaleColor = StiTableElementConditionHelper.ProcessBackColor(scaleColor, column.Key, table, rowValues, columnKeys).GetValueOrDefault();
		RenderTextCell(destination, rect, table, rowValues, columnKeys, column, zoom, value, StiBorderSides.All, foreColor, scaleColor);
	}

	private static void RenderBoolCell(StiPanel destination, RectangleD rect, StiTableElement table, object[] rowValues, string[] columnKeys, StiTableColumn column, double zoom, bool value, Color foreColor, Color backColor, StiDashboardExportFormat format)
	{
		if (format == StiDashboardExportFormat.Data)
		{
			RenderTextCell(destination, rect, table, rowValues, columnKeys, column, zoom, value, StiBorderSides.None, foreColor, backColor);
			return;
		}
		foreColor = StiTableElementConditionHelper.ProcessForeColor(foreColor, column.Key, table, rowValues, columnKeys);
		backColor = StiTableElementConditionHelper.ProcessBackColor(backColor, column.Key, table, rowValues, columnKeys).GetValueOrDefault();
		StiCheckStyle stiCheckStyle = (value ? StiCheckStyle.CheckRectangle : StiCheckStyle.NoneRectangle);
		destination.Components.Add(new StiCheckBox(rect)
		{
			Name = table.Name + "_BoolCell",
			Border = GetBorderSides(destination, rect, table, column, StiBorderSides.All),
			ContourColor = Color.Transparent,
			TextBrush = new StiSolidBrush(foreColor),
			Brush = new StiSolidBrush(backColor),
			CheckStyleForTrue = stiCheckStyle,
			CheckStyleForFalse = StiCheckStyle.NoneRectangle,
			CheckedValue = (stiCheckStyle != StiCheckStyle.NoneRectangle),
			ExcelDataValue = value.ToString(),
			ComponentPlacement = "d." + table.Name
		});
	}

	private static void RenderTextCell(StiPanel destination, RectangleD rect, StiTableElement table, object[] rowValues, string[] columnKeys, StiTableColumn column, double zoom, object value, StiBorderSides borderSides, Color foreColor, Color backColor, bool exportDataOnly = false, bool? wordWrap = null)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		string text = StiTextFormatHelper.FormatBasedOnColumnType(table, column, value);
		string text2 = ((column != null) ? StiLabelHelper.GetLabel(column) : string.Empty);
		StiDimensionColumn stiDimensionColumn = column as StiDimensionColumn;
		Font val = (Font)((!exportDataOnly) ? ((object)table.Font) : ((object)new Font("Arial", 10f)));
		if (stiDimensionColumn != null && stiDimensionColumn.ShowHyperlink)
		{
			val = new Font(val, (FontStyle)(val.Style | 4));
		}
		FontStyle val2 = StiTableElementConditionHelper.ProcessFontStyle(val, column.Key, table, rowValues, columnKeys);
		val = new Font(val, val2);
		decimal num = StiValueHelper.TryToDecimal(value);
		if (num < 0m && StiNegativeColorChecker.IsNegativeInRed(column.TextFormat))
		{
			foreColor = StiOptions.Engine.NegativeColor;
		}
		if (column is StiSparklinesColumn)
		{
			object[] source = StiSparklinesCellPainter.CastToArray(value);
			decimal[] values = source.Select(StiValueHelper.TryToDecimal).ToArray();
			text = string.Join(CultureInfo.CurrentCulture.TextInfo.ListSeparator, values);
		}
		foreColor = StiTableElementConditionHelper.ProcessForeColor(foreColor, column.Key, table, rowValues, columnKeys);
		backColor = StiTableElementConditionHelper.ProcessBackColor(backColor, column.Key, table, rowValues, columnKeys).GetValueOrDefault();
		StiTextHorAlignment horAlignment = StiTitleExportHelper.Convert(column.HorAlignment);
		if (column is StiBubbleColumn || column is StiDataBarsColumn)
		{
			horAlignment = StiTextHorAlignment.Right;
		}
		if (column is IStiTableColumnSize stiTableColumnSize && !wordWrap.HasValue)
		{
			wordWrap = stiTableColumnSize.Size.WordWrap;
		}
		string hyperlinkValue = null;
		if (stiDimensionColumn != null && stiDimensionColumn.ShowHyperlink && !string.IsNullOrWhiteSpace(stiDimensionColumn.HyperlinkPattern))
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("Value", value);
			int columnIndex;
			for (columnIndex = 0; columnIndex < columnKeys.Length; columnIndex++)
			{
				StiTableColumn stiTableColumn = table.Columns.FirstOrDefault((StiTableColumn tc) => tc.Key == columnKeys[columnIndex]);
				string key = "Row." + StiCodeDomSerializator.ReplaceSymbols(stiTableColumn.Label);
				dictionary.Add(key, rowValues[columnIndex]?.ToString());
			}
			hyperlinkValue = StiReportParser.Parse(stiDimensionColumn.HyperlinkPattern, table, allowCache: false, dictionary);
		}
		destination.Components.Add(new StiText(rect)
		{
			Name = ((exportDataOnly && !string.IsNullOrEmpty(text2)) ? text2 : (table.Name + "_TextCell")),
			Border = GetBorderSides(destination, rect, table, column, borderSides),
			Brush = new StiSolidBrush(backColor),
			ExcelDataValue = text,
			Font = StiElementConsts.Table.Font.GetGdiFont(zoom, null, val),
			HorAlignment = horAlignment,
			VertAlignment = StiVertAlignment.Center,
			Margins = new StiMargins(2.0, 2.0, 1.0, 0.0),
			TextQuality = StiTextQuality.Typographic,
			Text = text,
			TextBrush = new StiSolidBrush(foreColor),
			TextFormat = (column.TextFormat?.Clone() as StiFormatService),
			WordWrap = (wordWrap != false),
			ComponentPlacement = "d." + table.Name,
			Trimming = (StringTrimming)3,
			HyperlinkValue = hyperlinkValue
		});
	}

	private static void RenderImageCell(StiPanel destination, RectangleD rect, StiTableElement table, StiTableColumn column, object value, Color backColor, StiDashboardExportFormat format)
	{
		if (format != StiDashboardExportFormat.Data)
		{
			Image imageFromObject = Stimulsoft.Report.Helpers.StiImageHelper.GetImageFromObject(value);
			if (imageFromObject != null)
			{
				destination.Components.Add(new StiImage(rect)
				{
					Name = table.Name + "_ImageCell",
					Border = GetBorderSides(destination, rect, table, column, StiBorderSides.All),
					Brush = new StiSolidBrush(backColor),
					Stretch = true,
					AspectRatio = true,
					ImageToDraw = imageFromObject,
					HorAlignment = column.HorAlignment
				});
			}
		}
	}

	private static Color GetForeColor(StiTableElement table, StiTableColumn column, StiTableElementStyle style, bool isInterlaced)
	{
		if (column.ForeColor != Color.Transparent)
		{
			return column.ForeColor;
		}
		if (column is StiDimensionColumn && ((StiDimensionColumn)column).ShowHyperlink)
		{
			return StiColor.Get("165dc8");
		}
		Color foreColor = StiDashboardExpressionHelper.GetForeColor(table, table.ForeColor);
		if (foreColor != Color.Transparent)
		{
			return foreColor;
		}
		if (style == null)
		{
			return StiElementConsts.Table.Font.Color;
		}
		if (!isInterlaced)
		{
			return style.CellForeColor;
		}
		return style.AlternatingCellForeColor;
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

	private static StiBorder GetBorderSides(StiPanel destination, RectangleD rect, StiTableElement table, StiTableColumn column, StiBorderSides borderSides)
	{
		StiTableElementStyle tableStyle = StiDashboardStyleHelper.GetTableStyle(table);
		bool flag = (borderSides & StiBorderSides.Left) != 0 && table.Columns.IndexOf(column) != 0 && rect.Left < destination.Width;
		bool flag2 = (borderSides & StiBorderSides.Right) != 0 && table.Columns.IndexOf(column) != table.Columns.Count - 1 && rect.Right <= destination.Width;
		StiBorderSides stiBorderSides = (flag ? StiBorderSides.Left : StiBorderSides.None) | (flag2 ? StiBorderSides.Right : StiBorderSides.None);
		int num = ((stiBorderSides != 0) ? 1 : 0);
		Color color = ((stiBorderSides != 0) ? tableStyle.LineColor : Color.Transparent);
		return new StiBorder(stiBorderSides, color, num, StiPenStyle.Solid);
	}

	private static string ImageToBase64(byte[] bytes)
	{
		if (OleUnit.IsOleHeader(bytes))
		{
			OleUnit.ObjectHeader objectHeader = new OleUnit.ObjectHeader(bytes);
			byte[] array = new byte[bytes.Length - objectHeader.HeaderLen];
			Array.Copy(bytes, objectHeader.HeaderLen, array, 0, bytes.Length - objectHeader.HeaderLen);
			bytes = array;
		}
		string text = "data:image;base64,";
		if (Stimulsoft.Report.Helpers.StiImageHelper.IsPng(bytes))
		{
			text = "data:image/png;base64,";
		}
		else if (Stimulsoft.Report.Helpers.StiImageHelper.IsMetafile(bytes))
		{
			text = "data:image/x-wmf;base64,";
		}
		else if (Stimulsoft.Report.Helpers.StiImageHelper.IsBmp(bytes))
		{
			text = "data:image/bmp;base64,";
		}
		else if (Stimulsoft.Report.Helpers.StiImageHelper.IsJpeg(bytes))
		{
			text = "data:image/jpeg;base64,";
		}
		else if (Stimulsoft.Report.Helpers.StiImageHelper.IsGif(bytes))
		{
			text = "data:image/gif;base64,";
		}
		else if (Stimulsoft.Base.Helpers.StiSvgHelper.IsSvg(bytes))
		{
			text = "data:image/svg+xml;base64,";
		}
		return text + Convert.ToBase64String(bytes);
	}
}
