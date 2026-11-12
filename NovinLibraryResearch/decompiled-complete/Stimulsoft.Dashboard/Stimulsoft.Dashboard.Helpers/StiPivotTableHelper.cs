using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Components.PivotTable;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Extensions;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.CrossTab.Core;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Dashboard.Helpers;

public static class StiPivotTableHelper
{
	private static double FieldMargin = 1.5;

	public static Hashtable SummaryTypes { get; set; } = new Hashtable();


	public static Rectangle GetHeadersBounds(StiCell[][] cells)
	{
		int num = -1;
		int num2 = -1;
		int num3 = -1;
		int num4 = -1;
		for (int i = 0; i < cells.Length; i++)
		{
			for (int j = 0; j < cells[i].Length; j++)
			{
				StiCell stiCell = cells[i][j];
				if (stiCell.CellType == StiCellType.HeaderColMain || stiCell.CellType == StiCellType.HeaderColSummaryTotal)
				{
					num = ((num == -1) ? j : num);
					num2 = Math.Max(j, num2);
				}
				if (stiCell.CellType == StiCellType.HeaderRowMain || stiCell.CellType == StiCellType.HeaderRowSummaryTotal)
				{
					num3 = ((num3 == -1) ? i : num3);
					num4 = Math.Max(i, num4);
				}
			}
		}
		return new Rectangle(num3, num, num4 - num3, num2 - num);
	}

	public static Hashtable GetViewerData(IStiPivotTableElement pivotElement)
	{
		Hashtable hashtable = new Hashtable();
		StiPivotTableElement stiPivotTableElement = pivotElement as StiPivotTableElement;
		StiCrossTab stiCrossTab = StiPivotTableToCrossTabCache.Get(stiPivotTableElement);
		if (stiCrossTab == null)
		{
			StiDataTable orCreate = StiElementDataCache.GetOrCreate(stiPivotTableElement);
			stiCrossTab = ConvertToCrossTab(stiPivotTableElement, orCreate);
			StiPivotTableToCrossTabCache.Put(stiPivotTableElement, stiCrossTab);
			StiPivotToConvertedStateCache.PutFalse(stiPivotTableElement);
		}
		bool flag = stiPivotTableElement.Rows.Count == 0 && stiPivotTableElement.Columns.Count == 0 && stiPivotTableElement.Summaries.Count > 0;
		StiCell[][] cells = stiCrossTab.CrossTabInfo.Cross.Cells;
		Rectangle headersBounds = GetHeadersBounds(cells);
		int num = headersBounds.Bottom + 1;
		int num2 = headersBounds.Right + 1;
		int i = 0;
		int j = 0;
		for (; i < stiPivotTableElement.Rows.Count - 1 && !stiPivotTableElement.Rows[i].ShowTotal; i++)
		{
		}
		for (; j < stiPivotTableElement.Columns.Count - 1 && !stiPivotTableElement.Columns[j].ShowTotal; j++)
		{
		}
		hashtable.Add("lastTopNRow", i);
		hashtable.Add("lastTopNCol", j);
		hashtable.Add("headerHeight", num);
		hashtable.Add("headerWidth", num2);
		hashtable.Add("onlySummaries", flag);
		hashtable.Add("pivotColumnsCount", stiPivotTableElement.Columns.Count);
		hashtable.Add("pivotRowsCount", stiPivotTableElement.Rows.Count);
		hashtable.Add("pivotSummariesCount", stiPivotTableElement.Summaries.Count);
		hashtable.Add("pivotSummaryDirection", stiPivotTableElement.SummaryDirection);
		ArrayList arrayList = new ArrayList();
		if (cells.Length != 0)
		{
			Hashtable hashtable2 = new Hashtable();
			for (int k = 0; k < cells.Length; k++)
			{
				for (int l = 0; l < cells[k].Length; l++)
				{
					StiCell stiCell = cells[k][l];
					if (!string.IsNullOrEmpty(stiCell.ParentGuid) && stiCell.ParentCell == stiCell)
					{
						if (hashtable2[stiCell.ParentGuid] == null)
						{
							hashtable2.Add(stiCell.ParentGuid, new List<StiCell>());
						}
						(hashtable2[stiCell.ParentGuid] as List<StiCell>).Add(stiCell);
					}
				}
			}
			ArrayList arrayList2 = new ArrayList();
			for (int m = 0; m < cells.Length; m++)
			{
				ArrayList arrayList3 = new ArrayList();
				for (int n = 0; n < cells[0].Length; n++)
				{
					StiCell stiCell2 = cells[m][n];
					List<string> list = new List<string>();
					string totalCellGuid = null;
					if (stiCell2.ParentCell == stiCell2 && !string.IsNullOrEmpty(stiCell2.Guid) && hashtable2.ContainsKey(stiCell2.Guid))
					{
						foreach (StiCell item in hashtable2[stiCell2.Guid] as List<StiCell>)
						{
							if (item.Field is StiCrossTotal && item.ParentCell == item)
							{
								totalCellGuid = item.Guid;
							}
							else
							{
								list.Add(item.Guid);
							}
						}
					}
					arrayList3.Add(CellItem(cells[m][n], m, n, list, totalCellGuid, m < num2 || n < num, arrayList2, m >= num2));
				}
				arrayList.Add(arrayList3);
			}
			hashtable.Add("cells", arrayList);
			hashtable.Add("sizes", arrayList2);
		}
		return hashtable;
	}

	private static Hashtable CellItem(StiCell cell, int x, int y, List<string> dependedCellGuis, string totalCellGuid, bool isHeader, ArrayList sizes, bool isColumn)
	{
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_055a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0560: Unknown result type (might be due to invalid IL or missing references)
		//IL_0562: Invalid comparison between Unknown and I4
		//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d0: Invalid comparison between Unknown and I4
		//IL_0637: Unknown result type (might be due to invalid IL or missing references)
		//IL_063d: Unknown result type (might be due to invalid IL or missing references)
		//IL_063f: Invalid comparison between Unknown and I4
		//IL_069c: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a4: Invalid comparison between Unknown and I4
		StiUnit stiUnit = cell.Field?.Report?.Unit;
		int num = -1;
		if (stiUnit != null)
		{
			string value = $"{stiUnit.ConvertToHInches(cell.Field.MinSize.Width)};{stiUnit.ConvertToHInches(cell.Field.MaxSize.Width)};{cell.Field.WordWrap}";
			if (sizes.IndexOf(value) == -1)
			{
				sizes.Add(value);
			}
			num = sizes.IndexOf(value);
		}
		Hashtable hashtable = new Hashtable();
		hashtable.Add("rowSpan", cell.Height);
		hashtable.Add("colSpan", cell.Width);
		hashtable.Add("row", y);
		hashtable.Add("column", x);
		hashtable.Add("text", cell.Text);
		hashtable.Add("isColumn", isColumn);
		hashtable.Add("s", num);
		hashtable.Add("st", (cell.Field as StiCrossHeader)?.ShowTotal);
		hashtable.Add("l", cell.Level);
		Hashtable hashtable2 = hashtable;
		if (isHeader)
		{
			if (string.IsNullOrEmpty(cell.Guid))
			{
				cell.Guid = Guid.NewGuid().ToString();
			}
			if (!string.IsNullOrEmpty(cell.ParentGuid))
			{
				hashtable2.Add("parentGuid", cell.ParentGuid);
			}
			if (cell.ParentCell != null && cell.ParentCell == cell)
			{
				hashtable2.Add("parentCellGuid", cell.ParentCell.Guid);
			}
			if (cell.Field != null)
			{
				hashtable2.Add("isTotal", cell.Field is StiCrossTotal && cell.ParentCell == cell);
			}
			hashtable2.Add("guid", cell.Guid);
			if (dependedCellGuis.Count > 0)
			{
				hashtable2.Add("dependedCellGuis", dependedCellGuis);
			}
			if (totalCellGuid != null)
			{
				hashtable2.Add("totalCellGuid", totalCellGuid);
			}
		}
		if (cell.Field?.Indicator is StiIconSetIndicator { Alignment: var alignment } stiIconSetIndicator)
		{
			string text = ((object)(ContentAlignment)(ref alignment)).ToString();
			string text2 = (text.StartsWith("Top") ? "top" : (text.StartsWith("Bottom") ? "bottom" : "middle"));
			string text3 = "<img src='data:image/png;base64," + Convert.ToBase64String(stiIconSetIndicator.CustomIcon) + "' style='vertical-align: " + text2 + ";'>";
			hashtable2.Remove("text");
			if (text.EndsWith("Left"))
			{
				hashtable2.Add("text", text3 + cell.Text);
			}
			else
			{
				hashtable2.Add("text", cell.Text + text3);
			}
		}
		Hashtable hashtable3 = new Hashtable();
		StiConditionPermissions? stiConditionPermissions = cell.Field?.ConditionPermissions;
		if (stiConditionPermissions != StiConditionPermissions.None)
		{
			if ((int?)((uint?)stiConditionPermissions & 0x80u) > (int?)0)
			{
				Color color = (cell.Field.ConditionBrush as StiSolidBrush).Color;
				hashtable3.Add("backgroundColor", $"rgba({color.R}, {color.G}, {color.B}, {color.A})");
			}
			if ((int?)((uint?)stiConditionPermissions & 1u) > (int?)0)
			{
				hashtable3.Add("fontFamily", cell.Field.Font.Name);
			}
			if ((int?)((uint?)stiConditionPermissions & 2u) > (int?)0)
			{
				hashtable3.Add("fontSize", $"{cell.Field.Font.SizeInPoints}pt");
			}
			if ((int?)((uint?)stiConditionPermissions & 4u) > (int?)0)
			{
				hashtable3.Add("fontWeight", ((cell.Field.Font.Style & 1) > 0) ? "bold" : "normal");
			}
			if ((int?)((uint?)stiConditionPermissions & 8u) > (int?)0)
			{
				hashtable3.Add("fontStyle", ((cell.Field.Font.Style & 2) > 0) ? "italic " : "normal");
			}
			string text4 = string.Empty;
			if ((int?)((uint?)stiConditionPermissions & 0x20u) > (int?)0 && (cell.Field.Font.Style & 8) > 0)
			{
				text4 += "line-through ";
			}
			if ((int?)((uint?)stiConditionPermissions & 0x10u) > (int?)0 && (cell.Field.Font.Style & 4) > 0)
			{
				text4 += "underline";
			}
			if (!string.IsNullOrEmpty(text4))
			{
				hashtable3.Add("textDecoration", text4);
			}
			if ((int?)((uint?)stiConditionPermissions & 0x40u) > (int?)0)
			{
				Color color2 = (cell.Field.ConditionTextBrush as StiSolidBrush).Color;
				hashtable3.Add("color", $"rgba({color2.R}, {color2.G}, {color2.B}, {color2.A})");
			}
		}
		switch (cell.Field?.HorAlignment)
		{
		case StiTextHorAlignment.Left:
			hashtable3.Add("text-align", "left");
			break;
		case StiTextHorAlignment.Right:
			hashtable3.Add("text-align", "right");
			break;
		case StiTextHorAlignment.Width:
			hashtable3.Add("text-align", "justify");
			break;
		}
		if (hashtable3.Count > 0)
		{
			hashtable2.Add("style", hashtable3);
		}
		return hashtable2;
	}

	public static void ApplyStyle(StiPivotTableElement pivot, StiCrossTab crossTab, bool exportDataOnly)
	{
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Expected O, but got Unknown
		StiPivotElementStyle pivotTableStyle = StiDashboardStyleHelper.GetPivotTableStyle(pivot);
		StiCell[][] cells = crossTab.CrossTabInfo.Cross.Cells;
		Rectangle headersBounds = GetHeadersBounds(cells);
		int num = headersBounds.Bottom + 1;
		int num2 = headersBounds.Right + 1;
		for (int i = 0; i < cells.Length; i++)
		{
			for (int j = 0; j < cells[i].Length; j++)
			{
				if (cells[i][j].Field == null)
				{
					continue;
				}
				StiCell stiCell = cells[i][j];
				if (StiOptions.Engine.CrossTab.MemoryOptimization && (stiCell.Field.Text == null || stiCell.Field.Text == "") && !stiCell.FieldCloned)
				{
					stiCell.Field = stiCell.Field.Clone() as StiCrossField;
					stiCell.FieldCloned = true;
				}
				bool flag = i < num2 || j < num;
				if (exportDataOnly)
				{
					stiCell.Field.Font = new Font("Arial", 10f, (FontStyle)(flag ? 1 : 0));
					stiCell.Field.Brush = new StiSolidBrush(Color.Transparent);
					stiCell.Field.TextBrush = new StiSolidBrush(Color.Black);
				}
				else if (!flag)
				{
					stiCell.Field.Brush = stiCell.Field.ConditionBrush ?? new StiSolidBrush((j % 2 != 0) ? pivotTableStyle.AlternatingCellBackColor : pivotTableStyle.CellBackColor);
					stiCell.Field.TextBrush = stiCell.Field.ConditionTextBrush ?? new StiSolidBrush((j % 2 != 0) ? pivotTableStyle.AlternatingCellForeColor : pivotTableStyle.CellForeColor);
				}
				else
				{
					if (i < num2 && j >= num)
					{
						stiCell.Field.Brush = stiCell.Field.ConditionBrush ?? new StiSolidBrush(pivotTableStyle.RowHeaderBackColor);
					}
					else
					{
						stiCell.Field.Brush = stiCell.Field.ConditionBrush ?? new StiSolidBrush(pivotTableStyle.ColumnHeaderBackColor);
					}
					stiCell.Field.TextBrush = stiCell.Field.ConditionTextBrush ?? new StiSolidBrush(pivotTableStyle.ColumnHeaderForeColor);
				}
				stiCell.Field.Border.Color = pivotTableStyle.LineColor;
			}
		}
	}

	public static void BuildCross(StiCrossTab masterCrossTab, StiDataTable dataTable, StiPivotTableElement pivot)
	{
		Hashtable hashtable = new Hashtable();
		for (int i = 0; i < dataTable.Meters.Count; i++)
		{
			hashtable[dataTable.Meters[i].Expression] = i;
		}
		masterCrossTab.CrossTabInfoV1.StartRow = 0;
		masterCrossTab.CrossTabInfoV1.StartCol = 0;
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection(null);
		StiComponentsCollection stiComponentsCollection2 = new StiComponentsCollection(null);
		StiComponentsCollection stiComponentsCollection3 = new StiComponentsCollection(null);
		StiComponentsCollection stiComponentsCollection4 = new StiComponentsCollection(null);
		StiComponentsCollection stiComponentsCollection5 = new StiComponentsCollection(null);
		StiComponentsCollection stiComponentsCollection6 = new StiComponentsCollection(null);
		StiCrossTitle leftCrossTitle = null;
		StiCrossTitle rightCrossTitle = null;
		StiCrossTitle summaryCrossTitle = null;
		DataTable dataTable2 = new DataTable();
		foreach (StiComponent component in masterCrossTab.Components)
		{
			if (!(component is StiCrossCell) && !(component is StiCrossTitle) && !(component is StiCrossSummaryHeader))
			{
				continue;
			}
			StiCrossRow stiCrossRow = component as StiCrossRow;
			StiCrossColumn stiCrossColumn = component as StiCrossColumn;
			StiCrossTitle stiCrossTitle = component as StiCrossTitle;
			StiCrossSummary stiCrossSummary = component as StiCrossSummary;
			StiCrossSummaryHeader stiCrossSummaryHeader = component as StiCrossSummaryHeader;
			if (stiCrossTitle != null)
			{
				if (stiCrossTitle.TypeOfComponent.StartsWith("Row:", StringComparison.InvariantCulture))
				{
					stiComponentsCollection5.Add(stiCrossTitle);
				}
				if (stiCrossTitle.TypeOfComponent.StartsWith("Col:", StringComparison.InvariantCulture))
				{
					stiComponentsCollection6.Add(stiCrossTitle);
				}
				if (stiCrossTitle.TypeOfComponent.StartsWith("LeftTitle", StringComparison.InvariantCulture))
				{
					leftCrossTitle = stiCrossTitle;
				}
				if (stiCrossTitle.TypeOfComponent.StartsWith("RightTitle", StringComparison.InvariantCulture))
				{
					rightCrossTitle = stiCrossTitle;
				}
				if (stiCrossTitle.TypeOfComponent.StartsWith("SummaryTitle", StringComparison.InvariantCulture))
				{
					summaryCrossTitle = stiCrossTitle;
				}
				continue;
			}
			if (stiCrossRow != null)
			{
				stiComponentsCollection2.Add(stiCrossRow);
				if (!stiCrossRow.IsTotalVisible)
				{
					stiCrossRow.Total.Height = 0.0;
				}
			}
			if (stiCrossColumn != null)
			{
				stiComponentsCollection.Add(stiCrossColumn);
				if (!stiCrossColumn.IsTotalVisible)
				{
					stiCrossColumn.Total.Width = 0.0;
				}
			}
			if (stiCrossSummary != null)
			{
				stiComponentsCollection3.Add(stiCrossSummary);
			}
			if (stiCrossSummaryHeader != null)
			{
				stiComponentsCollection4.Add(stiCrossSummaryHeader);
			}
			DataColumn column = new DataColumn(component.Name, typeof(object));
			dataTable2.Columns.Add(column);
			if (component is StiCrossHeader)
			{
				column = new DataColumn("Display__" + component.Name, typeof(object));
				dataTable2.Columns.Add(column);
			}
		}
		foreach (object[] row in dataTable.Rows)
		{
			DataRow dataRow = dataTable2.NewRow();
			foreach (StiCrossField component2 in masterCrossTab.Components)
			{
				if (component2 is StiCrossSummaryHeader)
				{
					continue;
				}
				if (component2 is StiCrossCell stiCrossCell)
				{
					object obj = (hashtable.Contains(component2.Alias) ? row[(int)hashtable[component2.Alias]] : null);
					if (ListExt.IsList(obj))
					{
						obj = ListExt.ToList(obj)?.FirstOrDefault();
					}
					if (obj is double || obj is float || obj is int || obj is long || obj is uint || obj is ulong || obj is byte || obj is short || obj is sbyte || obj is ushort)
					{
						obj = Convert.ToDecimal(obj);
					}
					dataRow[stiCrossCell.Name] = obj;
					if (stiCrossCell is StiCrossHeader)
					{
						object obj2 = row[(int)hashtable[component2.Alias]];
						if (ListExt.IsList(obj2))
						{
							obj2 = ListExt.ToList(obj2)?.FirstOrDefault();
						}
						if (obj2 == null)
						{
							obj2 = string.Empty;
						}
						dataRow["Display__" + component2.Name] = obj2;
					}
				}
				else if (component2 is StiCrossTitle && !string.IsNullOrEmpty(component2.Alias))
				{
					object obj3 = row[(int)hashtable[component2.Alias]];
					if (ListExt.IsList(obj3))
					{
						obj3 = ListExt.ToList(obj3)?.FirstOrDefault();
					}
					if (obj3 == null)
					{
						obj3 = string.Empty;
					}
					((StiCrossTitle)component2).SetTextInternal(obj3?.ToString());
				}
			}
			dataTable2.Rows.Add(dataRow);
			masterCrossTab.Next();
		}
		StiCross stiCross2 = (masterCrossTab.CrossTabInfo.Cross = new StiCross());
		StiCross stiCross3 = stiCross2;
		stiCross3.DesignTime = false;
		stiCross3.ColFields = stiComponentsCollection;
		stiCross3.RowFields = stiComponentsCollection2;
		stiCross3.SumFields = stiComponentsCollection3;
		stiCross3.SumHeaderFields = stiComponentsCollection4;
		stiCross3.ColTitleFields = stiComponentsCollection6;
		stiCross3.RowTitleFields = stiComponentsCollection5;
		stiCross3.LeftCrossTitle = leftCrossTitle;
		stiCross3.RightCrossTitle = rightCrossTitle;
		stiCross3.SummaryCrossTitle = summaryCrossTitle;
		stiCross3.CrossTab = masterCrossTab;
		masterCrossTab.DataSourceName = "pivot";
		masterCrossTab.Report.Dictionary.DataSources.Add(new StiUserSource("pivot", "pivot"));
		List<StiPivotSummary> summaries = pivot.Summaries;
		masterCrossTab.SummaryDirection = ((summaries == null || summaries.Count <= 1) ? StiSummaryDirection.UpToDown : pivot.SummaryDirection);
		stiCross3.Create(dataTable2, masterCrossTab.Report, masterCrossTab.SummaryDirection, masterCrossTab.EmptyValue);
		stiCross3.CrossTab = null;
		masterCrossTab.CrossTabInfo.Cross.MaxWidth = (decimal)masterCrossTab.Page.Width;
		masterCrossTab.CrossTabInfo.Cross.MaxHeight = (decimal)masterCrossTab.Page.Height;
		StiPivotElementStyle pivotTableStyle = StiDashboardStyleHelper.GetPivotTableStyle(pivot);
		bool flag = pivot.Rows.Count == 0 && pivot.Columns.Count == 0 && pivot.Summaries.Count > 0;
		if (flag)
		{
			stiCross3.Init(2, pivot.Summaries.Count);
			for (int j = 0; j < pivot.Summaries.Count; j++)
			{
				StiPivotSummary stiPivotSummary = pivot.Summaries[j];
				StiCrossColumn stiCrossColumn2 = new StiCrossColumn();
				stiCrossColumn2.Alias = stiPivotSummary.Expression;
				stiCrossColumn2.Name = "CrossTab_Sum" + j;
				stiCrossColumn2.Page = masterCrossTab.Page;
				stiCrossColumn2.HorAlignment = StiTextHorAlignment.Center;
				stiCrossColumn2.TextFormat = (StiFormatService)stiPivotSummary.TextFormat.Clone();
				stiCrossColumn2.Guid = Guid.NewGuid().ToString().Replace("-", "");
				stiCrossColumn2.Font = StiElementConsts.Table.Font.GetCachedGdiFont();
				stiCrossColumn2.Brush = new StiSolidBrush(pivotTableStyle.CellBackColor);
				stiCrossColumn2.Border = new StiBorder((j == pivot.Summaries.Count - 1) ? (StiBorderSides.Top | StiBorderSides.Right) : ((j == 0) ? (StiBorderSides.Right | StiBorderSides.Bottom) : (StiBorderSides.Top | StiBorderSides.Right | StiBorderSides.Bottom)), pivotTableStyle.LineColor, 1.0, StiPenStyle.Solid);
				stiCrossColumn2.Height = StiElementConsts.Table.GetHeight(stiCrossColumn2.Font);
				stiCrossColumn2.Margins = new StiMargins(FieldMargin);
				stiCross3.Cells[0][j] = new StiCell();
				stiCross3.Cells[0][j].Text = GetLabel(stiPivotSummary);
				stiCross3.Cells[0][j].Field = stiCrossColumn2;
				stiCross3.Cells[0][j].ParentCell = stiCross3.Cells[0][j];
				string text = ((stiPivotSummary.TextFormat != null && !(stiPivotSummary.TextFormat is StiGeneralFormatService)) ? ((dataTable.Rows.Count > 0) ? stiPivotSummary.TextFormat.Format(dataTable.Rows[0][j]) : string.Empty) : ((dataTable.Rows.Count > 0) ? dataTable.Rows[0][j]?.ToString() : string.Empty));
				StiCrossSummary stiCrossSummary2 = new StiCrossSummary();
				stiCrossSummary2.Name = "CrossTab_Sum" + j;
				stiCrossSummary2.Value.Value = text;
				stiCrossSummary2.Page = masterCrossTab.Page;
				stiCrossSummary2.HorAlignment = StiTextHorAlignment.Right;
				stiCrossSummary2.TextFormat = (StiFormatService)stiPivotSummary.TextFormat.Clone();
				stiCrossSummary2.Guid = Guid.NewGuid().ToString().Replace("-", "");
				stiCrossSummary2.Font = StiElementConsts.Table.Font.GetCachedGdiFont();
				stiCrossSummary2.Border = new StiBorder(StiBorderSides.None, pivotTableStyle.LineColor, 1.0, StiPenStyle.Solid);
				stiCrossSummary2.Height = StiElementConsts.Table.GetHeight(stiCrossSummary2.Font);
				stiCrossSummary2.Margins = new StiMargins(FieldMargin);
				stiCross3.Cells[1][j] = new StiCell();
				stiCross3.Cells[1][j].Field = stiCrossSummary2;
				stiCross3.Cells[1][j].Text = text;
				stiCross3.Cells[1][j].ParentCell = stiCross3.Cells[1][j];
			}
		}
		if (masterCrossTab.RightToLeft)
		{
			StiCrossTabHelper.MakeRightToLeft(masterCrossTab);
		}
		masterCrossTab.CrossTabInfo.Cross.AlignToGrid = false;
		masterCrossTab.CrossTabInfo.Cross.DoAutoSize();
		if (flag)
		{
			return;
		}
		for (int k = 0; k < stiCross3.Cells.Length; k++)
		{
			for (int l = 0; l < stiCross3.Cells[k].Length; l++)
			{
				if (stiCross3.Cells[k][l].Field != null)
				{
					stiCross3.Cells[k][l].Field.Margins = (StiMargins)stiCross3.Cells[k][l].Field.Margins.Clone();
					if (k == 0)
					{
						stiCross3.Cells[k][l].Field.Border.Side = stiCross3.Cells[k][l].Field.Border.Side & ~StiBorderSides.Left;
					}
					if (l == 0)
					{
						stiCross3.Cells[k][l].Field.Border.Side = stiCross3.Cells[k][l].Field.Border.Side & ~StiBorderSides.Top;
					}
					if (k == stiCross3.Cells.Length - 1)
					{
						stiCross3.Cells[k][l].Field.Border.Side = stiCross3.Cells[k][l].Field.Border.Side & ~StiBorderSides.Right;
					}
					if (l == stiCross3.Cells[k].Length - 1)
					{
						stiCross3.Cells[k][l].Field.Border.Side = stiCross3.Cells[k][l].Field.Border.Side & ~StiBorderSides.Bottom;
					}
				}
			}
		}
		int num = pivot.Columns.Count + ((pivot.Rows.Count > 0) ? 1 : 0);
		int num2 = ((pivot.Rows.Count == 0) ? 1 : pivot.Rows.Count) + ((pivot.Summaries.Count > 1) ? 1 : 0);
		for (int m = num; m < stiCross3.Cells[0].Length; m++)
		{
			for (int n = num2; n < stiCross3.Cells.Length; n++)
			{
				if (stiCross3.Cells[n][m].Field != null)
				{
					stiCross3.Cells[n][m].Field.Border.Side = StiBorderSides.Left | StiBorderSides.Right;
				}
			}
		}
		if (stiCross3.SumFields.Count > 1)
		{
			for (int num3 = 0; num3 < stiCross3.Cells[0].Length; num3++)
			{
				if (stiCross3.Cells[num2 - 1][num3].Field != null)
				{
					stiCross3.Cells[num2 - 1][num3].Field.Border = new StiBorder(StiBorderSides.All, pivotTableStyle.LineColor, 1.0, StiPenStyle.Solid);
				}
			}
		}
		if (stiCross3.Cells[0][0].Field != null)
		{
			stiCross3.Cells[0][0].Field.Border.Side = StiBorderSides.Right | StiBorderSides.Bottom;
		}
	}

	public static StiCrossTab ConvertToCrossTab(StiPivotTableElement pivot, StiDataTable dataTable)
	{
		StiReport stiReport = new StiReport
		{
			CalculationMode = StiCalculationMode.Interpretation,
			Info = 
			{
				Zoom = 1.0
			},
			ReportUnit = ((pivot?.Report == null) ? StiReportUnitType.HundredthsOfInch : pivot.Report.ReportUnit)
		};
		StiEngine engine = new StiEngine(stiReport);
		stiReport.Engine = engine;
		StiPage stiPage = new StiPage();
		stiReport.Pages.Add(stiPage);
		stiReport.Dictionary.Variables = pivot.Report.Dictionary.Variables;
		StiCrossTab stiCrossTab = new StiCrossTab
		{
			Name = "CrossTab",
			CrossTabStyle = null,
			CrossTabStyleIndex = 9
		};
		stiPage.Components.Add(stiCrossTab);
		UpdateCrossTab(stiCrossTab, pivot, dataTable);
		stiPage.Components.Remove(stiCrossTab);
		return stiCrossTab;
	}

	internal static void UpdateCrossTab(StiCrossTab crossTab, StiPivotTableElement pivot, StiDataTable dataTable, bool build = true)
	{
		Hashtable createdTotals = new Hashtable();
		List<StiCrossTotal> list = new List<StiCrossTotal>();
		List<StiCrossTotal> list2 = new List<StiCrossTotal>();
		List<StiCrossSummaryHeader> list3 = new List<StiCrossSummaryHeader>();
		string text = "";
		Hashtable hashtable = new Hashtable();
		Hashtable hashtable2 = new Hashtable();
		if (crossTab.Report.Designer != null)
		{
			crossTab.Report.Designer.TextAliasesHash.Clear();
		}
		list.Clear();
		list2.Clear();
		List<StiCrossRow> list4 = new List<StiCrossRow>();
		List<StiCrossColumn> list5 = new List<StiCrossColumn>();
		Hashtable hashtable3 = new Hashtable();
		StiPivotElementStyle pivotTableStyle = StiDashboardStyleHelper.GetPivotTableStyle(pivot);
		int num = 0;
		foreach (StiPivotRow row in pivot.Rows)
		{
			StiCrossRow stiCrossRow = new StiCrossRow();
			stiCrossRow.ShowTotal = row.ShowTotal;
			stiCrossRow.Alias = row.Expression;
			StiDisplayCrossValueExpression displayValue = stiCrossRow.DisplayValue;
			string value = (stiCrossRow.Value.Value = "{" + row.Expression + "}");
			displayValue.Value = value;
			if (string.IsNullOrEmpty(text) && row.Expression.Length > 0 && row.Expression.IndexOf(".", StringComparison.Ordinal) >= 0)
			{
				text = row.Expression.Substring(0, row.Expression.IndexOf(".", StringComparison.Ordinal));
			}
			stiCrossRow.Page = null;
			stiCrossRow.Name = $"{crossTab.Name}_Row{num + 1}";
			stiCrossRow.Page = crossTab.Page;
			stiCrossRow.HorAlignment = (StiTextHorAlignment)Enum.Parse(typeof(StiTextHorAlignment), row.HorAlignment.ToString());
			stiCrossRow.TextFormat = (StiFormatService)row.TextFormat.Clone();
			stiCrossRow.Guid = Guid.NewGuid().ToString().Replace("-", "");
			stiCrossRow.MeterKey = row.Key;
			stiCrossRow.WordWrap = row.Size.WordWrap;
			stiCrossRow.Margins = new StiMargins(FieldMargin);
			if (row.Size.MaxWidth > 0)
			{
				stiCrossRow.MaxSize = new SizeD(crossTab.Report.Unit.ConvertFromHInches((double)row.Size.MaxWidth), 0.0);
			}
			if (row.Size.MinWidth > 0)
			{
				stiCrossRow.MinSize = new SizeD(crossTab.Report.Unit.ConvertFromHInches((double)row.Size.MinWidth), 0.0);
			}
			if (row.Size.Width > 0)
			{
				stiCrossRow.MaxSize = new SizeD(crossTab.Report.Unit.ConvertFromHInches((double)row.Size.Width), 0.0);
				stiCrossRow.MinSize = new SizeD(crossTab.Report.Unit.ConvertFromHInches((double)row.Size.Width), 0.0);
			}
			GetCondition(pivot, row, stiCrossRow);
			SetTopN(stiCrossRow, row.TopN, pivot);
			SetupTitle(stiCrossRow, pivotTableStyle);
			hashtable[row] = stiCrossRow;
			if (crossTab.Components.Contains(stiCrossRow))
			{
				crossTab.Components.Remove(stiCrossRow);
			}
			crossTab.Components.Add(stiCrossRow);
			hashtable3[stiCrossRow] = stiCrossRow;
			list4.Add(stiCrossRow);
			StiCrossTotal stiCrossTotal = stiCrossRow.Total;
			if (stiCrossTotal == null)
			{
				stiCrossTotal = CreateRowTotal(pivot, stiCrossRow, createdTotals, pivotTableStyle);
				if (!string.IsNullOrWhiteSpace(row.TotalLabel))
				{
					stiCrossTotal.Text.Value = row.TotalLabel;
				}
				stiCrossRow.TotalGuid = stiCrossTotal.Guid;
				crossTab.Components.Add(stiCrossTotal);
			}
			hashtable3[stiCrossTotal] = stiCrossTotal;
			list.Add(stiCrossTotal);
			num++;
		}
		int num2 = 0;
		foreach (StiPivotColumn column in pivot.Columns)
		{
			StiCrossColumn stiCrossColumn = new StiCrossColumn();
			stiCrossColumn.ShowTotal = column.ShowTotal;
			stiCrossColumn.Alias = column.Expression;
			StiDisplayCrossValueExpression displayValue2 = stiCrossColumn.DisplayValue;
			string value = (stiCrossColumn.Value.Value = "{" + column.Expression + "}");
			displayValue2.Value = value;
			stiCrossColumn.Page = null;
			stiCrossColumn.Name = $"{crossTab}_Column{num2 + 1}";
			stiCrossColumn.Page = crossTab.Page;
			stiCrossColumn.HorAlignment = (StiTextHorAlignment)Enum.Parse(typeof(StiTextHorAlignment), column.HorAlignment.ToString());
			stiCrossColumn.Guid = Guid.NewGuid().ToString().Replace("-", "");
			stiCrossColumn.MeterKey = column.Key;
			stiCrossColumn.WordWrap = column.Size.WordWrap;
			stiCrossColumn.Margins = new StiMargins(FieldMargin);
			if (column.Size.MinWidth > 0)
			{
				stiCrossColumn.MinSize = new SizeD(crossTab.Report.Unit.ConvertFromHInches((double)column.Size.MinWidth), 0.0);
			}
			if (column.Size.MaxWidth > 0)
			{
				stiCrossColumn.MaxSize = new SizeD(crossTab.Report.Unit.ConvertFromHInches((double)column.Size.MaxWidth), 0.0);
			}
			if (column.Size.Width > 0)
			{
				stiCrossColumn.MaxSize = new SizeD(crossTab.Report.Unit.ConvertFromHInches((double)column.Size.Width), 0.0);
				stiCrossColumn.MinSize = new SizeD(crossTab.Report.Unit.ConvertFromHInches((double)column.Size.Width), 0.0);
			}
			GetCondition(pivot, column, stiCrossColumn);
			SetTopN(stiCrossColumn, column.TopN, pivot);
			if (string.IsNullOrEmpty(text) && column.Expression.Length > 0 && column.Expression.IndexOf(".", StringComparison.Ordinal) >= 0)
			{
				text = column.Expression.Substring(0, column.Expression.IndexOf(".", StringComparison.Ordinal));
			}
			stiCrossColumn.TextFormat = (StiFormatService)column.TextFormat.Clone();
			SetupTitle(stiCrossColumn, pivotTableStyle);
			hashtable2[column] = stiCrossColumn;
			if (crossTab.Components.Contains(stiCrossColumn))
			{
				crossTab.Components.Remove(stiCrossColumn);
			}
			crossTab.Components.Add(stiCrossColumn);
			hashtable3[stiCrossColumn] = stiCrossColumn;
			list5.Add(stiCrossColumn);
			StiCrossTotal stiCrossTotal2 = stiCrossColumn.Total;
			if (stiCrossTotal2 == null)
			{
				stiCrossTotal2 = CreateColTotal(pivot, stiCrossColumn, createdTotals, list2, pivotTableStyle);
				if (!string.IsNullOrWhiteSpace(column.TotalLabel))
				{
					stiCrossTotal2.Text.Value = column.TotalLabel;
				}
				stiCrossColumn.TotalGuid = stiCrossTotal2.Guid;
				crossTab.Components.Add(stiCrossTotal2);
			}
			hashtable3[stiCrossTotal2] = stiCrossTotal2;
			list2.Add(stiCrossTotal2);
			num2++;
		}
		int num3 = 1;
		foreach (StiPivotSummary summary in pivot.Summaries)
		{
			StiCrossSummary stiCrossSummary = new StiCrossSummary();
			stiCrossSummary.Alias = summary.Expression;
			SetSummaryType(stiCrossSummary, summary.Expression);
			stiCrossSummary.Page = null;
			stiCrossSummary.Name = crossTab.Name + "_Sum" + num3;
			stiCrossSummary.Page = crossTab.Page;
			stiCrossSummary.HorAlignment = (StiTextHorAlignment)Enum.Parse(typeof(StiTextHorAlignment), summary.HorAlignment.ToString());
			stiCrossSummary.TextFormat = (StiFormatService)summary.TextFormat.Clone();
			stiCrossSummary.Guid = Guid.NewGuid().ToString().Replace("-", "");
			GetCondition(pivot, summary, stiCrossSummary);
			stiCrossSummary.Font = StiElementConsts.Table.Font.GetCachedGdiFont();
			stiCrossSummary.Border = new StiBorder(StiBorderSides.All, pivotTableStyle.LineColor, 1.0, StiPenStyle.Solid);
			stiCrossSummary.Height = StiElementConsts.Table.GetHeight(stiCrossSummary.Font);
			stiCrossSummary.Margins = new StiMargins(FieldMargin);
			if (summary.Size.MinWidth > 0)
			{
				stiCrossSummary.MinSize = new SizeD(crossTab.Report.Unit.ConvertFromHInches((double)summary.Size.MinWidth), 0.0);
			}
			if (summary.Size.MaxWidth > 0)
			{
				stiCrossSummary.MaxSize = new SizeD(crossTab.Report.Unit.ConvertFromHInches((double)summary.Size.MaxWidth), 0.0);
			}
			if (summary.Size.Width > 0)
			{
				stiCrossSummary.MaxSize = new SizeD(crossTab.Report.Unit.ConvertFromHInches((double)summary.Size.Width), 0.0);
				stiCrossSummary.MinSize = new SizeD(crossTab.Report.Unit.ConvertFromHInches((double)summary.Size.Width), 0.0);
			}
			hashtable3[stiCrossSummary] = stiCrossSummary;
			if (crossTab.Components.Contains(stiCrossSummary))
			{
				crossTab.Components.Remove(stiCrossSummary);
			}
			crossTab.Components.Add(stiCrossSummary);
			num3++;
		}
		if (pivot.Summaries.Count < 2)
		{
			foreach (StiCrossSummaryHeader item in list3)
			{
				if (crossTab.Components.Contains(item))
				{
					crossTab.Components.Remove(item);
				}
			}
			list3.Clear();
		}
		else if (list3.Count > pivot.Summaries.Count)
		{
			while (list3.Count > pivot.Summaries.Count)
			{
				StiCrossSummaryHeader component = list3[list3.Count - 1];
				if (crossTab.Components.Contains(component))
				{
					crossTab.Components.Remove(component);
				}
				list3.RemoveAt(list3.Count - 1);
			}
		}
		else if (list3.Count < pivot.Summaries.Count)
		{
			while (list3.Count < pivot.Summaries.Count)
			{
				StiCrossSummaryHeader stiCrossSummaryHeader = new StiCrossSummaryHeader
				{
					Text = 
					{
						Value = ((!string.IsNullOrEmpty(pivot.Summaries[list3.Count].Label)) ? pivot.Summaries[list3.Count].Label : (list3.Count + 1).ToString())
					},
					Page = null,
					Name = crossTab.Name + "_SumHeader" + (list3.Count + 1),
					Margins = new StiMargins(FieldMargin)
				};
				stiCrossSummaryHeader.Page = crossTab.Page;
				crossTab.ApplyFieldStyle(stiCrossSummaryHeader);
				crossTab.Components.Add(stiCrossSummaryHeader);
				list3.Add(stiCrossSummaryHeader);
			}
		}
		foreach (StiCrossSummaryHeader item2 in list3)
		{
			hashtable3[item2] = item2;
		}
		int num4 = 1;
		foreach (StiCrossTotal item3 in list)
		{
			item3.Page = null;
			item3.Name = crossTab.Name + "_RowTotal" + num4++;
			item3.Page = crossTab.Page;
		}
		int num5 = 1;
		foreach (StiCrossColumnTotal item4 in list2)
		{
			item4.Page = null;
			item4.Name = crossTab.Name + "_ColTotal" + num5++;
			item4.Page = crossTab.Page;
		}
		bool flag = pivot.Columns.Count > 0 && pivot.Rows.Count > 0;
		List<StiCrossTitle> list6 = new List<StiCrossTitle>();
		List<StiCrossTitle> list7 = new List<StiCrossTitle>();
		StiCrossTitle stiCrossTitle = null;
		StiCrossTitle stiCrossTitle2 = null;
		StiCrossTitle stiCrossTitle3 = null;
		foreach (StiCrossField component2 in crossTab.Components)
		{
			if (component2 is StiCrossTitle)
			{
				StiCrossTitle stiCrossTitle4 = component2 as StiCrossTitle;
				if (stiCrossTitle4.TypeOfComponent.StartsWith("Col:", StringComparison.InvariantCulture))
				{
					list6.Add(stiCrossTitle4);
				}
				if (stiCrossTitle4.TypeOfComponent.StartsWith("Row:", StringComparison.InvariantCulture))
				{
					list7.Add(stiCrossTitle4);
				}
				if (stiCrossTitle4.TypeOfComponent.StartsWith("LeftTitle", StringComparison.InvariantCulture))
				{
					stiCrossTitle = stiCrossTitle4;
				}
				if (stiCrossTitle4.TypeOfComponent.StartsWith("RightTitle", StringComparison.InvariantCulture))
				{
					stiCrossTitle2 = stiCrossTitle4;
				}
				if (stiCrossTitle4.TypeOfComponent.StartsWith("SummaryTitle", StringComparison.InvariantCulture))
				{
					stiCrossTitle3 = stiCrossTitle4;
				}
			}
		}
		if (pivot.Rows.Count == 0)
		{
			foreach (StiCrossColumn item5 in list5)
			{
				string text4 = "Col:" + item5.Name;
				bool flag2 = false;
				foreach (StiCrossTitle item6 in list6)
				{
					if (item6.TypeOfComponent == text4)
					{
						list6.Remove(item6);
						flag2 = true;
						hashtable3[item6] = item6;
						break;
					}
				}
				if (flag2)
				{
					continue;
				}
				string text5 = string.Empty;
				StiCrossTitle stiCrossTitle5 = new StiCrossTitle
				{
					Name = item5.Name + "_Title",
					TypeOfComponent = "Col:" + item5.Name
				};
				foreach (StiPivotColumn column2 in pivot.Columns)
				{
					if (hashtable2[column2] == item5)
					{
						text5 = GetLabel(column2);
						stiCrossTitle5.HorAlignment = StiTextHorAlignment.Center;
						stiCrossTitle5.TextFormat = (StiFormatService)column2.TextFormat.Clone();
					}
				}
				stiCrossTitle5.Text = text5;
				SetupField(stiCrossTitle5);
				SetupTitle(stiCrossTitle5, pivotTableStyle);
				crossTab.Components.Add(stiCrossTitle5);
				hashtable3[stiCrossTitle5] = stiCrossTitle5;
			}
		}
		foreach (StiCrossRow item7 in list4)
		{
			string text6 = "Row:" + item7.Name;
			bool flag3 = false;
			foreach (StiCrossTitle item8 in list7)
			{
				if (item8.TypeOfComponent == text6)
				{
					list7.Remove(item8);
					flag3 = true;
					hashtable3[item8] = item8;
					break;
				}
			}
			if (flag3)
			{
				continue;
			}
			string value2 = string.Empty;
			StiCrossTitle stiCrossTitle6 = new StiCrossTitle();
			foreach (StiPivotRow row2 in pivot.Rows)
			{
				if (hashtable[row2] == item7)
				{
					value2 = GetLabel(row2);
					stiCrossTitle6.HorAlignment = StiTextHorAlignment.Center;
					stiCrossTitle6.TextFormat = (StiFormatService)row2.TextFormat.Clone();
				}
			}
			SetupField(stiCrossTitle6);
			stiCrossTitle6.Name = item7.Name + "_Title";
			stiCrossTitle6.Text.Value = value2;
			stiCrossTitle6.TypeOfComponent = "Row:" + item7.Name;
			SetupTitle(stiCrossTitle6, pivotTableStyle);
			crossTab.Components.Add(stiCrossTitle6);
			hashtable3[stiCrossTitle6] = stiCrossTitle6;
		}
		if (stiCrossTitle == null)
		{
			if (flag)
			{
				stiCrossTitle = new StiCrossTitle
				{
					Name = crossTab.Name + "_LeftTitle"
				};
				SetupField(stiCrossTitle);
				if (pivot.Title != null)
				{
					stiCrossTitle.Text.Value = pivot.Title.Text;
				}
				stiCrossTitle.TypeOfComponent = "LeftTitle";
				SetupTitle(stiCrossTitle, pivotTableStyle);
				crossTab.Components.Add(stiCrossTitle);
				hashtable3[stiCrossTitle] = stiCrossTitle;
			}
		}
		else if (flag)
		{
			hashtable3[stiCrossTitle] = stiCrossTitle;
		}
		if (stiCrossTitle2 == null)
		{
			if (flag)
			{
				string text7 = string.Empty;
				bool flag4 = true;
				foreach (StiPivotColumn column3 in pivot.Columns)
				{
					if (!flag4)
					{
						text7 += ", ";
					}
					text7 += GetLabel(column3);
					flag4 = false;
				}
				stiCrossTitle2 = new StiCrossTitle();
				stiCrossTitle2.Name = crossTab.Name + "_RightTitle";
				stiCrossTitle2.Text.Value = text7;
				stiCrossTitle2.TypeOfComponent = "RightTitle";
				SetupField(stiCrossTitle2);
				SetupTitle(stiCrossTitle2, pivotTableStyle);
				crossTab.Components.Add(stiCrossTitle2);
				hashtable3[stiCrossTitle2] = stiCrossTitle2;
			}
		}
		else if (flag)
		{
			hashtable3[stiCrossTitle2] = stiCrossTitle2;
		}
		bool flag5 = (pivot.Columns.Count == 0 || pivot.Rows.Count == 0) && pivot.Summaries.Count > 0;
		string text8 = string.Join(", ", pivot.Summaries.Select((StiPivotSummary s) => s.Label).ToArray());
		if (stiCrossTitle3 == null)
		{
			if (flag5)
			{
				stiCrossTitle3 = new StiCrossTitle();
				stiCrossTitle3.Name = crossTab.Name + "_SummaryTitle";
				stiCrossTitle3.Text.Value = text8;
				stiCrossTitle3.TypeOfComponent = "SummaryTitle";
				SetupField(stiCrossTitle3);
				SetupTitle(stiCrossTitle3, pivotTableStyle);
				crossTab.Components.Add(stiCrossTitle3);
				hashtable3[stiCrossTitle3] = stiCrossTitle3;
			}
		}
		else if (flag5)
		{
			if (string.IsNullOrEmpty(stiCrossTitle3.Text.Value) || stiCrossTitle3.Text.Value.StartsWith(text8) || text8.StartsWith(stiCrossTitle3.Text.Value))
			{
				stiCrossTitle3.Text.Value = text8;
			}
			hashtable3[stiCrossTitle3] = stiCrossTitle3;
		}
		int num6 = 0;
		while (num6 < crossTab.Components.Count)
		{
			StiComponent key = crossTab.Components[num6];
			if (hashtable3[key] == null)
			{
				crossTab.Components.RemoveAt(num6);
			}
			else
			{
				num6++;
			}
		}
		RectangleM empty = RectangleM.Empty;
		crossTab.DataSourceName = text;
		if (!build)
		{
			return;
		}
		BuildCross(crossTab, dataTable, pivot);
		foreach (StiCrossField component3 in crossTab.Components)
		{
			if (crossTab.CrossTabInfo.Cross.Fields[component3] is Point)
			{
				Point point = (Point)crossTab.CrossTabInfo.Cross.Fields[component3];
				crossTab.CrossTabInfo.Cross.SetTextOfCell(point.X, point.Y, component3.CellText);
			}
		}
		crossTab.CrossTabInfo.Cross.MaxWidth = (decimal)crossTab.Page.Width;
		crossTab.CrossTabInfo.Cross.MaxHeight = (decimal)crossTab.Page.Height;
		crossTab.CrossTabInfo.Cross.DoAutoSize();
		Hashtable hashtable4 = new Hashtable();
		foreach (StiCrossField component4 in crossTab.Components)
		{
			if (component4 is StiCrossHeader { IsTotalVisible: false } stiCrossHeader)
			{
				hashtable4[stiCrossHeader.Total] = stiCrossHeader.Total;
			}
		}
		decimal num7 = crossTab.Report.Unit.ConvertFromHInches(2m);
		decimal num8 = crossTab.Report.Unit.ConvertFromHInches(2m);
		foreach (StiCrossField component5 in crossTab.Components)
		{
			if (!(crossTab.CrossTabInfo.Cross.Fields[component5] is Point))
			{
				continue;
			}
			Point point2 = (Point)crossTab.CrossTabInfo.Cross.Fields[component5];
			RectangleM rect = crossTab.Report.Unit.ConvertToHInches(StiCrossTabHelper.GetCellRect(crossTab, point2.X, point2.Y));
			empty.Width = Math.Max(empty.Width, rect.Right);
			empty.Height = Math.Max(empty.Height, rect.Bottom);
			rect = crossTab.Report.Unit.ConvertFromHInches(rect);
			if (hashtable4[component5] != null)
			{
				rect.Width = 0m;
				rect.Height = 0m;
			}
			decimal num9 = default(decimal);
			decimal num10 = default(decimal);
			if (component5 is StiCrossTitle stiCrossTitle7)
			{
				if (stiCrossTitle7.TypeOfComponent.StartsWith("LeftTitle", StringComparison.InvariantCulture))
				{
					num9 = default(decimal);
					num10 = default(decimal);
				}
				if (stiCrossTitle7.TypeOfComponent.StartsWith("RightTitle", StringComparison.InvariantCulture))
				{
					num9 = num7;
					num10 = default(decimal);
				}
				if (stiCrossTitle7.TypeOfComponent.StartsWith("SummaryTitle", StringComparison.InvariantCulture))
				{
					if (pivot.Rows.Count == 0)
					{
						num9 = default(decimal);
						num10 = num8 * 2m;
					}
					if (pivot.Columns.Count == 0)
					{
						num9 = num7;
						num10 = num8;
					}
				}
				if (stiCrossTitle7.TypeOfComponent.StartsWith("Row:", StringComparison.InvariantCulture))
				{
					num9 = default(decimal);
					num10 = num8;
				}
				if (stiCrossTitle7.TypeOfComponent.StartsWith("Col:", StringComparison.InvariantCulture))
				{
					num9 = default(decimal);
					num10 = num8;
				}
			}
			if (component5 is StiCrossRow)
			{
				num9 = default(decimal);
				num10 = num8 * 2m;
			}
			if (component5 is StiCrossRowTotal)
			{
				num9 = default(decimal);
				num10 = num8 * 2m;
			}
			if (component5 is StiCrossColumn)
			{
				num9 = num7;
				num10 = num8;
			}
			if (component5 is StiCrossColumnTotal)
			{
				num9 = num7;
				num10 = num8;
			}
			if (component5 is StiCrossSummary)
			{
				num9 = num7;
				num10 = num8 * 2m;
			}
			if (component5 is StiCrossSummaryHeader)
			{
				if (crossTab.SummaryDirection == StiSummaryDirection.LeftToRight)
				{
					num9 = num7;
					num10 = num8;
				}
				else
				{
					num9 = default(decimal);
					num10 = num8 * 2m;
				}
			}
			rect.X += num9;
			rect.Y += num10;
			component5.ClientRectangle = rect.ToRectangleD();
		}
	}

	private static void GetCondition(StiPivotTableElement pivot, StiMeter meter, StiCrossField comp)
	{
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		List<IStiPivotTableElementCondition> list = pivot.PivotTableConditions?.FindAll((IStiPivotTableElementCondition p) => p.KeyValueMeter == meter.Key || p.KeyValueMeter == StiLabelHelper.GetLabel(meter));
		if (list == null || list.Count <= 0)
		{
			return;
		}
		foreach (IStiPivotTableElementCondition item in list)
		{
			StiCondition stiCondition = new StiCondition();
			StiFilter filter = new StiFilter
			{
				Condition = item.Condition,
				DataType = item.DataType,
				Value1 = item.Value
			};
			string text = StiDataHelper.GetFilterExpression(filter, "value2", comp.Report);
			if (item.DataType != 0)
			{
				text = "value2 == null ? False : (value2.ToString().Length > 0 ? " + text + " : False)";
			}
			stiCondition.Expression = text;
			stiCondition.Permissions = item.Permissions;
			stiCondition.Permissions &= ~StiConditionPermissions.Borders;
			stiCondition.BackColor = item.BackColor;
			stiCondition.Font = item.Font;
			stiCondition.TextColor = item.TextColor;
			if (item.IconAlignment != 0)
			{
				stiCondition.Icon = item.GetIcon(isExporting: true);
				stiCondition.IconSize = item.IconSize;
				stiCondition.IconAlignment = (ContentAlignment)((item.IconAlignment == StiIconAlignment.Right) ? 64 : 16);
			}
			DictionaryEntry dictionaryEntry = new DictionaryEntry(stiCondition, "{" + text + "}");
			if (comp.Report.Engine.ParserConversionStore["*StiConditionExpression*" + comp.Name] == null)
			{
				ArrayList value = new ArrayList();
				comp.Report.Engine.ParserConversionStore["*StiConditionExpression*" + comp.Name] = value;
			}
			(comp.Report.Engine.ParserConversionStore["*StiConditionExpression*" + comp.Name] as ArrayList).Add(dictionaryEntry);
			comp.Conditions.Add(stiCondition);
		}
	}

	private static void SetupTitle(StiCrossField field, StiPivotElementStyle style)
	{
		field.Border = new StiBorder(StiBorderSides.All, style.LineColor, 1.0, StiPenStyle.Solid);
		field.Font = StiElementConsts.Table.Font.GetCachedGdiFont();
		field.Height = StiElementConsts.Table.GetHeight(field.Font);
	}

	private static void SetupField(StiCrossField field)
	{
		field.Margins = new StiMargins(FieldMargin);
		field.Guid = Guid.NewGuid().ToString().Replace("-", "");
	}

	private static StiCrossRowTotal CreateRowTotal(StiPivotTableElement element, StiCrossRow row, Hashtable CreatedTotals, StiPivotElementStyle style)
	{
		string guid = row.Guid;
		StiCrossRowTotal stiCrossRowTotal = CreatedTotals[guid] as StiCrossRowTotal;
		if (stiCrossRowTotal == null)
		{
			stiCrossRowTotal = new StiCrossRowTotal
			{
				Text = (string.IsNullOrEmpty(element.TotalLabel) ? Loc.GetMain("Total") : element.TotalLabel),
				WordWrap = row.WordWrap,
				MinSize = row.MinSize,
				MaxSize = row.MaxSize
			};
			SetupField(stiCrossRowTotal);
			SetupTitle(stiCrossRowTotal, style);
		}
		CreatedTotals[guid] = stiCrossRowTotal;
		stiCrossRowTotal.TagValue = row;
		return stiCrossRowTotal;
	}

	private static StiCrossColumnTotal CreateColTotal(StiPivotTableElement element, StiCrossColumn column, Hashtable CreatedTotals, List<StiCrossTotal> colTotals, StiPivotElementStyle style)
	{
		string guid = column.Guid;
		StiCrossColumnTotal stiCrossColumnTotal = CreatedTotals[guid] as StiCrossColumnTotal;
		if (stiCrossColumnTotal == null)
		{
			stiCrossColumnTotal = new StiCrossColumnTotal
			{
				Text = (string.IsNullOrEmpty(element.TotalLabel) ? Loc.GetMain("Total") : element.TotalLabel),
				WordWrap = column.WordWrap,
				MinSize = column.MinSize,
				MaxSize = column.MaxSize
			};
			SetupField(stiCrossColumnTotal);
			SetupTitle(stiCrossColumnTotal, style);
		}
		CreatedTotals[guid] = stiCrossColumnTotal;
		stiCrossColumnTotal.TagValue = column;
		colTotals.Add(stiCrossColumnTotal);
		return stiCrossColumnTotal;
	}

	private static string GetLabel(StiMeter meter)
	{
		if (!string.IsNullOrEmpty(meter.Label))
		{
			return meter.Label;
		}
		return meter.Expression.Substring(meter.Expression.LastIndexOf(".", StringComparison.Ordinal) + 1).Replace("]", "").Replace(")", "");
	}

	private static Hashtable GetSummaryTypes()
	{
		if (SummaryTypes.Count == 0)
		{
			SummaryTypes.Add("None", "None");
			SummaryTypes.Add("Sum", "Sum");
			SummaryTypes.Add("Avg", "Average");
			SummaryTypes.Add("Min", "Min");
			SummaryTypes.Add("Max", "Max");
			SummaryTypes.Add("DistinctCount", "CountDistinct");
			SummaryTypes.Add("Count", "Count");
			SummaryTypes.Add("Median", "Median");
		}
		return SummaryTypes;
	}

	private static void SetTopN(StiCrossHeader topNObj, StiDataTopN topN, StiPivotTableElement pivot)
	{
		topNObj.TopN = topN.Clone() as StiDataTopN;
		if (topNObj.TopN.Mode == StiDataTopNMode.None)
		{
			return;
		}
		if (topNObj.TopN.ShowOthers)
		{
			topNObj.TopN.OthersText = (string.IsNullOrEmpty(topNObj.TopN.OthersText) ? Loc.Get("FormDesigner", "Others") : StiReportParser.Parse(topNObj.TopN.OthersText, pivot));
		}
		foreach (StiPivotSummary summary in pivot.Summaries)
		{
			if ((string.IsNullOrEmpty(summary.Label) || !(summary.Label != topNObj.TopN.MeasureField)) && !(StiLabelHelper.GetLabel(summary) != topNObj.TopN.MeasureField) && !string.IsNullOrEmpty(summary.Expression) && summary.Expression.IndexOf("(", StringComparison.InvariantCulture) != -1 && summary.Expression.IndexOf(")", StringComparison.InvariantCulture) != -1)
			{
				topNObj.TopN.MeasureField = summary.Expression.Substring(summary.Expression.IndexOf("(", StringComparison.InvariantCulture) + 1).Replace(")", "");
			}
		}
	}

	private static void SetSummaryType(StiCrossSummary summary, string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return;
		}
		foreach (string key in GetSummaryTypes().Keys)
		{
			if (value.StartsWith(key))
			{
				summary.Summary = ((!(key != "Count")) ? StiSummaryType.Sum : ((StiSummaryType)Enum.Parse(typeof(StiSummaryType), (string)GetSummaryTypes()[key])));
				value = value.Replace(key + "(", "");
			}
		}
		summary.Value.Value = "{" + value.Replace(")", "") + "}";
	}
}
