using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.PivotTable;
using Stimulsoft.Dashboard.Export.Helpers;
using Stimulsoft.Dashboard.Export.Settings;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.CrossTab.Core;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Dashboard.Export.Tools;

public class StiPivotTableElementExportTool : StiElementExportTool
{
	private class ExportCell
	{
		public int X;

		public int Y;

		public int Width = 1;

		public int Height = 1;

		public StiCell Cell;

		public int CellRow = -1;
	}

	public override void Render(IStiElement element, StiPanel destination, RectangleD rect, StiDashboardExportSettings settings)
	{
		if (RenderEmptyDataMessage(element, destination, rect, settings))
		{
			return;
		}
		StiReport report = destination.Report;
		StiPivotTableElement stiPivotTableElement = element as StiPivotTableElement;
		StiPage page = destination.Page;
		bool flag = settings is StiSvgDashboardExportSettings && ((StiSvgDashboardExportSettings)settings).DesignMode;
		bool exportDataOnly = StiExportSettingsHelper.IsDataExport(settings);
		StiCrossTab stiCrossTab = StiPivotTableToCrossTabCache.Get(stiPivotTableElement);
		if (stiCrossTab == null || StiScale.Factor != 1.0)
		{
			StiDataTable dataTable = StiElementDataCache.TryToGetOrCreate(stiPivotTableElement);
			stiPivotTableElement.IsPivotTableExporting = true;
			stiCrossTab = StiPivotTableHelper.ConvertToCrossTab(stiPivotTableElement, dataTable);
			stiPivotTableElement.IsPivotTableExporting = false;
			if (StiScale.Factor == 1.0)
			{
				StiPivotTableToCrossTabCache.Put(stiPivotTableElement, stiCrossTab);
			}
			StiPivotToConvertedStateCache.PutFalse(stiPivotTableElement);
		}
		StiPivotTableHelper.ApplyStyle(stiPivotTableElement, stiCrossTab, exportDataOnly);
		if (flag)
		{
			RenderForWebDesigner(stiCrossTab, stiPivotTableElement);
			StiPivotTableToCrossTabCache.Remove(stiPivotTableElement);
		}
		ConvertSizes(stiPivotTableElement, stiCrossTab);
		StiCross cross = stiCrossTab.CrossTabInfo.Cross;
		double num = (double)cross.Widths.Sum();
		double num2 = (double)cross.Heights.Sum();
		if ((num <= rect.Width && num2 <= rect.Height) || flag || settings.RenderSinglePage)
		{
			if (settings is StiImageDashboardExportSettings { RenderSingleElement: not false } stiImageDashboardExportSettings)
			{
				SizeD sizeD = CalculateSize(element, destination, settings, num, num2);
				StiPanel stiPanel = destination;
				double width = (destination.Page.Width = Math.Min(sizeD.Width, stiImageDashboardExportSettings.MaxWidth));
				stiPanel.Width = width;
				StiPanel stiPanel2 = destination;
				width = (destination.Page.Height = Math.Min(sizeD.Height, stiImageDashboardExportSettings.MaxHeight));
				stiPanel2.Height = width;
			}
			StiBorderExportHelper.Render(element, destination, settings);
			StiBackColorExportHelper.Render(element, destination);
			RectangleD rect2 = StiTitleExportHelper.Render(element, destination, settings);
			rect2 = StiLayoutExportHelper.ExcludePadding(element, rect2);
			rect2 = StiLayoutExportHelper.ExcludeCornerRadius(element, rect2);
			RenderCells(destination, rect2, rect2.ToRectangleM(), stiCrossTab, flag);
		}
		else if (settings is StiExcelDashboardExportSettings || settings is StiHtmlDashboardExportSettings)
		{
			if (settings.RenderSingleElement || report.RenderedPages[0].Components.Count == 0)
			{
				report.RenderedPages.RemoveAt(0);
			}
			SizeD sizeD2 = CalculateSize(element, destination, settings, num, num2);
			page = new StiPage
			{
				Width = sizeD2.Width,
				Height = sizeD2.Height,
				Brush = new StiSolidBrush(StiDashboardStyleHelper.GetBackColor(element))
			};
			StiPanel stiPanel3 = new StiPanel
			{
				Name = element.Name,
				ClientRectangle = new RectangleD(0.0, 0.0, sizeD2.Width, sizeD2.Height)
			};
			page.Components.Add(stiPanel3);
			report.RenderedPages.Add(page);
			StiBorderExportHelper.Render(element, stiPanel3, settings);
			StiBackColorExportHelper.Render(element, stiPanel3);
			RectangleD rect3 = StiTitleExportHelper.Render(element, stiPanel3, settings);
			page.UnlimitedHeight = true;
			RenderCells(stiPanel3, rect3, rect3.ToRectangleM(), stiCrossTab);
		}
		else
		{
			StiPage stiPage = page.Clone(cloneProperties: true, cloneComponents: false) as StiPage;
			report.RenderedPages.Add(stiPage);
			StiPanel stiPanel4 = destination.Clone(cloneProperties: true, cloneComponents: false) as StiPanel;
			stiPanel4.Left = 0.0;
			stiPanel4.Top = 0.0;
			stiPanel4.Width = stiPage.Width;
			stiPanel4.Height = stiPage.Height;
			rect = new RectangleD(0.0, 0.0, stiPage.Width, stiPage.Height);
			stiPage.Components.Add(stiPanel4);
			destination = stiPanel4;
			if ((settings != null && settings.RenderSingleElement) || report.RenderedPages[0].Components.Count == 0)
			{
				report.RenderedPages.RemoveAt(0);
			}
			StiBorderExportHelper.Render(element, destination, settings);
			StiBackColorExportHelper.Render(element, destination);
			RectangleD rect4 = StiTitleExportHelper.Render(element, destination, settings);
			rect4 = StiLayoutExportHelper.ExcludePadding(element, rect4);
			rect4 = StiLayoutExportHelper.ExcludeCornerRadius(element, rect4);
			RectangleD rect5 = new RectangleD(rect4.Left, rect4.Top, rect4.Width + num, rect4.Height + num2);
			page.UnlimitedHeight = true;
			RenderCells(destination, rect5, rect4.ToRectangleM(), stiCrossTab);
			for (int i = 0; i < stiPage.SegmentPerWidth; i++)
			{
				for (int j = 0; j < stiPage.SegmentPerHeight; j++)
				{
					if (i != 0 || j != 0)
					{
						StiPanel stiPanel5 = destination.Clone(cloneProperties: true, cloneComponents: false) as StiPanel;
						stiPanel5.Left = (double)i * page.Width;
						stiPanel5.Top = (double)j * page.Height;
						stiPanel5.Width = page.Width;
						stiPanel5.Height = page.Height;
						stiPage.Components.Insert(0, stiPanel5);
						StiBorderExportHelper.Render(element, stiPanel5, settings);
						StiBackColorExportHelper.Render(element, stiPanel5);
						StiTitleExportHelper.Render(element, stiPanel5, settings);
					}
				}
			}
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

	private static SizeD CalculateSize(IStiElement element, StiPanel destination, StiDashboardExportSettings settings, double crossWidth, double crossHeight)
	{
		StiPage stiPage = new StiPage();
		StiPanel component = new StiPanel
		{
			Name = element.Name,
			ClientRectangle = new RectangleD(0.0, 0.0, crossWidth, crossHeight)
		};
		stiPage.Components.Add(component);
		destination.Report.RenderedPages.Add(stiPage);
		StiBorderExportHelper.Render(element, component, settings);
		StiBackColorExportHelper.Render(element, component);
		RectangleD rectangleD = StiTitleExportHelper.Render(element, component, settings);
		destination.Report.RenderedPages.Remove(stiPage);
		double width = crossWidth * 2.0 - rectangleD.Width + rectangleD.Left + 1.0;
		double height = crossHeight * 2.0 - rectangleD.Height + rectangleD.Top + 1.0;
		return new SizeD(width, height);
	}

	private static void RenderCells(StiPanel destination, RectangleD rect, RectangleM destinationRectangle, StiCrossTab renderedCrossTab, bool designMode = false)
	{
		renderedCrossTab.Page.UnlimitedHeight = true;
		destination.CanBreak = true;
		destination.DockStyle = StiDockStyle.None;
		StiCrossTabParams pars = new StiCrossTabParams
		{
			StartColumn = 0,
			StartRow = 0,
			DestinationContainer = destination,
			DestinationRectangle = destinationRectangle,
			ForceNoBreak = designMode
		};
		StiCrossTabV2Builder stiCrossTabV2Builder = new StiCrossTabV2Builder();
		stiCrossTabV2Builder.RenderCrossTabUnlimitedBreakable(pars, renderedCrossTab, destination, rect.ToRectangleM());
	}

	private static void ConvertSizes(StiPivotTableElement pivot, StiCrossTab renderedCrossTab)
	{
		StiCross cross = renderedCrossTab.CrossTabInfo.Cross;
		StiUnit unit = pivot.Report.Unit;
		if (!StiPivotToConvertedStateCache.IsConverted(pivot))
		{
			Convert(cross.Heights, unit);
			Convert(cross.Widths, unit);
			Convert(cross.CoordX, unit);
			Convert(cross.CoordY, unit);
			StiPivotToConvertedStateCache.PutTrue(pivot);
		}
	}

	private static void Convert(decimal[] array, StiUnit unit)
	{
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = unit.ConvertToHInches(array[i]);
		}
	}

	private static void addCell(List<ExportCell> eCells, int x, int y, StiCell cell)
	{
		eCells.Add(new ExportCell
		{
			X = x,
			Y = y,
			Width = cell.Width,
			Height = cell.Height,
			Cell = cell
		});
	}

	private static bool checkType(StiCell[][] cells, int x, int y, StiCellType[] types)
	{
		foreach (StiCellType stiCellType in types)
		{
			if (x < cells.Length - 1 && y < cells[x].Length - 1 && cells[x][y].CellType == stiCellType)
			{
				return true;
			}
		}
		return false;
	}

	private static void RenderForWebDesigner(StiCrossTab crossTab, StiPivotTableElement pivot)
	{
		if (pivot.Columns.Count <= 1 && pivot.Rows.Count <= 1)
		{
			return;
		}
		StiCell[][] cells = crossTab.CrossTabInfo.Cross.Cells;
		int num = 0;
		List<ExportCell> list = new List<ExportCell>();
		List<int> list2 = new List<int>();
		ExportCell exportCell = null;
		ExportCell exportCell2 = null;
		int num2 = ((pivot.SummaryDirection != 0) ? 1 : pivot.Summaries.Count);
		int num3 = ((pivot.SummaryDirection != StiSummaryDirection.UpToDown) ? 1 : pivot.Summaries.Count);
		Rectangle headersBounds = StiPivotTableHelper.GetHeadersBounds(cells);
		List<ExportCell> list3 = new List<ExportCell>();
		for (int i = 0; i < cells.Length; i++)
		{
			StiCell stiCell = cells[i][0];
			if (stiCell.CellType == StiCellType.LeftTopLineMain)
			{
				exportCell = new ExportCell
				{
					Cell = stiCell,
					X = i,
					Width = stiCell.Width
				};
			}
			if (stiCell.CellType == StiCellType.RightTopLineMain)
			{
				exportCell2 = new ExportCell
				{
					Cell = stiCell,
					X = i
				};
			}
			for (num = 0; num < cells[i].Length; num++)
			{
				stiCell = cells[i][num];
				if (stiCell.CellType == StiCellType.CornerColMain || stiCell.CellType == StiCellType.CornerRowMain)
				{
					list.Add(new ExportCell
					{
						Cell = stiCell,
						X = i,
						Y = num,
						Width = stiCell.Width,
						Height = stiCell.Height
					});
				}
			}
		}
		if (headersBounds.Top > -1)
		{
			int num4 = -1;
			for (int j = 0; j < cells.Length; j++)
			{
				StiCell stiCell2 = cells[j][headersBounds.Top];
				if (stiCell2.CellType != StiCellType.HeaderColMain && stiCell2.CellType != StiCellType.HeaderColTotalMain)
				{
					continue;
				}
				num4 = ((num4 == -1) ? j : num4);
				int num5 = headersBounds.Bottom - headersBounds.Top + ((num2 <= 1) ? 1 : 0);
				list.Add(new ExportCell
				{
					Cell = stiCell2,
					X = num4,
					Y = headersBounds.Top,
					Height = num5,
					Width = num2
				});
				stiCell2.Text = ((num5 > 1 && stiCell2.CellType != StiCellType.HeaderColTotalMain) ? "> " : "") + stiCell2.Text;
				list2.Add(j + stiCell2.Width - 1);
				if (num2 > 1)
				{
					for (int k = j + stiCell2.Width - num2; k < j + stiCell2.Width; k++)
					{
						list.Add(new ExportCell
						{
							Cell = cells[k][headersBounds.Bottom],
							X = num4,
							Y = headersBounds.Bottom
						});
						list2.Add(k);
						num4++;
					}
				}
				else
				{
					num4++;
				}
			}
		}
		if (headersBounds.Left > -1)
		{
			int num6 = -1;
			for (num = 0; num < cells[0].Length; num++)
			{
				StiCell stiCell3 = cells[0][num];
				if (stiCell3.CellType != StiCellType.HeaderRowMain && stiCell3.CellType != StiCellType.HeaderRowTotalMain)
				{
					continue;
				}
				num6 = ((num6 == -1) ? num : num6);
				int num7 = headersBounds.Right + ((num3 <= 1) ? 1 : 0);
				list.Add(new ExportCell
				{
					Cell = stiCell3,
					X = 0,
					Y = num6,
					Height = num3,
					Width = num7,
					CellRow = ((num3 == 1) ? (num + stiCell3.Height - 1) : (-1))
				});
				stiCell3.Text = ((num7 > 1 && stiCell3.CellType != StiCellType.HeaderRowTotalMain) ? "> " : "") + stiCell3.Text;
				if (num3 > 1)
				{
					for (int l = num + stiCell3.Height - num3; l < num + stiCell3.Height; l++)
					{
						list.Add(new ExportCell
						{
							Cell = cells[headersBounds.Right][l],
							X = headersBounds.Right,
							Y = num6,
							CellRow = l
						});
						num6++;
					}
				}
				else
				{
					num6++;
				}
			}
		}
		if (exportCell != null)
		{
			exportCell.Width = headersBounds.Right - headersBounds.Left + 1;
			list.Add(exportCell);
		}
		if (exportCell2 != null)
		{
			exportCell2.Width = list.Max((ExportCell c) => c.X) - headersBounds.Right;
			list.Add(exportCell2);
		}
		StiPivotElementStyle pivotTableStyle = StiDashboardStyleHelper.GetPivotTableStyle(pivot);
		int num8 = list.Max((ExportCell c) => c.Y) + 1;
		int num9 = list.Max((ExportCell c) => c.X) + 1;
		StiCell[,] array = new StiCell[num9, num8];
		foreach (ExportCell ec in list)
		{
			array[ec.X, ec.Y] = ec.Cell;
			ec.Cell.Width = ec.Width;
			ec.Cell.Height = ec.Height;
			if (ec.CellRow < 0)
			{
				continue;
			}
			int num10 = ec.Y;
			for (num = ec.Y; num < ec.Y + ec.Height; num++)
			{
				int num11 = headersBounds.Right + 1;
				num10 += (list2.Any((int c) => c >= ec.Width && c < cells.Length) ? 1 : 0);
				for (int m = ec.Width; m < cells.Length; m++)
				{
					if (list2.IndexOf(m) < 0)
					{
						continue;
					}
					array[num11++, num] = cells[m][ec.CellRow + num - ec.Y];
					array[num11 - 1, num].Field.Border.Side = ((m != cells.Length - 1) ? StiBorderSides.Right : StiBorderSides.None);
					StiCell stiCell4 = array[num11 - 1, num];
					if (stiCell4?.Field?.Brush is StiSolidBrush)
					{
						Color color = (stiCell4.Field.Brush as StiSolidBrush).Color;
						if (color == pivotTableStyle.AlternatingCellBackColor || color == pivotTableStyle.CellBackColor)
						{
							stiCell4.Field.Brush = new StiSolidBrush((num10 % 2 != 0) ? pivotTableStyle.AlternatingCellBackColor : pivotTableStyle.CellBackColor);
							stiCell4.Field.TextBrush = new StiSolidBrush((num10 % 2 != 0) ? pivotTableStyle.AlternatingCellForeColor : pivotTableStyle.CellForeColor);
						}
					}
					if (stiCell4?.Field != null)
					{
						stiCell4.Field.Brush = stiCell4.Field.ConditionBrush ?? stiCell4.Field.Brush;
						stiCell4.Field.TextBrush = stiCell4.Field.ConditionTextBrush ?? stiCell4.Field.TextBrush;
					}
				}
			}
		}
		cells = new StiCell[num9][];
		for (int n = 0; n < num9; n++)
		{
			cells[n] = new StiCell[num8];
			for (int num12 = 0; num12 < num8; num12++)
			{
				cells[n][num12] = ((array[n, num12] != null) ? array[n, num12] : new StiCell());
				if (cells[n][num12].Field != null && n == num9 - 1)
				{
					cells[n][num12].Field.Border.Side = cells[n][num12].Field.Border.Side & ~StiBorderSides.Right;
				}
				if (num12 > 0)
				{
					cells[n][num12].ParentCell = cells[n][num12];
				}
			}
		}
		decimal[] array2 = new decimal[num8];
		decimal[] array3 = new decimal[num9];
		Array.Copy(crossTab.CrossTabInfo.Cross.Heights, array2, num8);
		Array.Copy(crossTab.CrossTabInfo.Cross.Widths, array3, num9);
		crossTab.CrossTabInfo.Cross.Cells = cells;
		crossTab.CrossTabInfo.Cross.Widths = array3;
		crossTab.CrossTabInfo.Cross.Heights = array2;
		crossTab.CrossTabInfo.Cross.DoAutoSize();
	}
}
