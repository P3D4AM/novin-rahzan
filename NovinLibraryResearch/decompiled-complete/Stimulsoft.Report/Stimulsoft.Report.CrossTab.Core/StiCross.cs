using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Services;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Functions;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.CrossTab.Core;

public class StiCross : StiGrid
{
	private const string StrNull = "";

	public const string EmptyField = "EMPTY_FIELD";

	private int oneCellSize;

	private int oneCellWidth;

	private int oneCellHeight;

	private StiSummaryDirection summaryDirection;

	private int colsHeaderHeight;

	private int rowsHeaderWidth;

	private int widthCorrection;

	private int heightCorrection;

	private int colsWidth;

	private int rowsHeight;

	private Hashtable columnsCell = new Hashtable();

	private Hashtable rowsCell = new Hashtable();

	private StiComponentsCollection rowFields;

	private StiComponentsCollection colFields;

	private StiComponentsCollection sumFields;

	private StiComponentsCollection sumHeaderFields;

	internal StiCrossTab CrossTab { get; set; }

	private bool IsSummaryPresent
	{
		get
		{
			if (base.Report.IsDesigning)
			{
				return true;
			}
			if (SummaryCrossTitle != null)
			{
				return SummaryCrossTitle.IsEnabled;
			}
			return false;
		}
	}

	private bool IsRowTitlePresent
	{
		get
		{
			if (base.Report.IsDesigning)
			{
				return true;
			}
			foreach (StiCrossRow rowField in RowFields)
			{
				if (!rowField.IsEnabled)
				{
					return false;
				}
			}
			return true;
		}
	}

	private bool IsTopLinePresent
	{
		get
		{
			if (IsColsEmpty)
			{
				return false;
			}
			if (LeftCrossTitle == null && RightCrossTitle == null)
			{
				return false;
			}
			if (base.Report.IsDesigning)
			{
				return true;
			}
			if (LeftCrossTitle.IsEnabled && RightCrossTitle.IsEnabled)
			{
				return true;
			}
			return false;
		}
	}

	internal bool IsTopCrossTitleVisible
	{
		get
		{
			if (!IsRowsEmpty)
			{
				return IsCrossTitleEnabled;
			}
			return false;
		}
	}

	internal bool IsLeftCrossTitleVisible
	{
		get
		{
			if (!IsTopCrossTitleVisible)
			{
				return IsCrossTitleEnabled;
			}
			return false;
		}
	}

	internal bool IsCrossTitleEnabled
	{
		get
		{
			if (LeftCrossTitle == null || !LeftCrossTitle.Enabled || RightCrossTitle == null || !RightCrossTitle.Enabled)
			{
				if (SummaryCrossTitle != null)
				{
					return SummaryCrossTitle.Enabled;
				}
				return false;
			}
			return true;
		}
	}

	internal bool IsCrossTitlePrintOnAllPages
	{
		get
		{
			if (LeftCrossTitle == null || !LeftCrossTitle.PrintOnAllPages || RightCrossTitle == null || !RightCrossTitle.PrintOnAllPages)
			{
				if (SummaryCrossTitle != null)
				{
					return SummaryCrossTitle.PrintOnAllPages;
				}
				return false;
			}
			return true;
		}
	}

	private bool IsShowSummarySubHeaders
	{
		get
		{
			if (base.DesignTime)
			{
				return true;
			}
			foreach (StiCrossSummaryHeader sumHeaderField in SumHeaderFields)
			{
				if (sumHeaderField.Enabled)
				{
					return true;
				}
			}
			return false;
		}
	}

	private bool IsSummarySubHeadersPresent
	{
		get
		{
			if (SumFields.Count > 1 && IsShowSummarySubHeaders)
			{
				return SumHeaderFields.Count > 1;
			}
			return false;
		}
	}

	private bool IsLeftTopLinePresent
	{
		get
		{
			if (!IsTopLinePresent)
			{
				return false;
			}
			if (LeftCrossTitle == null)
			{
				return false;
			}
			if (base.Report.IsDesigning)
			{
				return true;
			}
			return LeftCrossTitle.IsEnabled;
		}
	}

	private bool IsRightTopLinePresent
	{
		get
		{
			if (!IsTopLinePresent)
			{
				return false;
			}
			if (RightCrossTitle == null)
			{
				return false;
			}
			if (base.Report.IsDesigning)
			{
				return true;
			}
			return RightCrossTitle.IsEnabled;
		}
	}

	public bool IsRowsEmpty
	{
		get
		{
			if (RowFields.Count == 1)
			{
				return RowFields[0].Name == "EMPTY_FIELD";
			}
			return false;
		}
	}

	public bool IsColsEmpty
	{
		get
		{
			if (ColFields.Count == 1)
			{
				return ColFields[0].Name == "EMPTY_FIELD";
			}
			return false;
		}
	}

	public bool IsSummariesEmpty
	{
		get
		{
			if (SumFields.Count == 1)
			{
				return SumFields[0].Name == "EMPTY_FIELD";
			}
			return false;
		}
	}

	public StiRowCollection Rows { get; } = new StiRowCollection();


	public StiColumnCollection Cols { get; } = new StiColumnCollection();


	public StiComponentsCollection ColTitleFields { get; set; }

	public StiComponentsCollection RowTitleFields { get; set; }

	public StiComponentsCollection RowFields
	{
		get
		{
			return rowFields;
		}
		set
		{
			if (value == null || value.Count == 0)
			{
				if (value == null)
				{
					value = new StiComponentsCollection();
				}
				else
				{
					value.Clear();
				}
				value.Add(new StiCrossRow
				{
					Name = "EMPTY_FIELD"
				});
			}
			rowFields = value;
		}
	}

	public StiComponentsCollection ColFields
	{
		get
		{
			return colFields;
		}
		set
		{
			if (value == null || value.Count == 0)
			{
				if (value == null)
				{
					value = new StiComponentsCollection();
				}
				else
				{
					value.Clear();
				}
				value.Add(new StiCrossColumn
				{
					Name = "EMPTY_FIELD"
				});
			}
			colFields = value;
		}
	}

	public StiComponentsCollection SumFields
	{
		get
		{
			return sumFields;
		}
		set
		{
			if (value == null || value.Count == 0)
			{
				if (value == null)
				{
					value = new StiComponentsCollection();
				}
				else
				{
					value.Clear();
				}
				value.Add(new StiCrossSummary
				{
					Name = "EMPTY_FIELD"
				});
			}
			sumFields = value;
		}
	}

	public StiComponentsCollection SumHeaderFields
	{
		get
		{
			return sumHeaderFields;
		}
		set
		{
			if (value == null || value.Count == 0)
			{
				if (value == null)
				{
					value = new StiComponentsCollection();
				}
				else
				{
					value.Clear();
				}
				value.Add(new StiCrossSummaryHeader
				{
					Name = "EMPTY_FIELD"
				});
			}
			sumHeaderFields = value;
		}
	}

	public StiSummaryContainer SummaryContainer { get; private set; }

	public StiCrossTitle LeftCrossTitle { get; set; }

	public StiCrossTitle RightCrossTitle { get; set; }

	public StiCrossTitle SummaryCrossTitle { get; set; }

	private void InvokeEvents(StiComponent component, string displayValue)
	{
		StiReport report = base.Report;
		if (report != null && report.CalculationMode == StiCalculationMode.Compilation)
		{
			StiValueEventArgs stiValueEventArgs = new StiValueEventArgs
			{
				DisplayValue = displayValue
			};
			component.InvokeGetHyperlink(component, stiValueEventArgs);
			component.HyperlinkValue = stiValueEventArgs.Value;
			StiValueEventArgs stiValueEventArgs2 = new StiValueEventArgs
			{
				DisplayValue = displayValue
			};
			component.InvokeGetToolTip(component, stiValueEventArgs2);
			component.ToolTipValue = stiValueEventArgs2.Value;
			StiValueEventArgs stiValueEventArgs3 = new StiValueEventArgs
			{
				DisplayValue = displayValue
			};
			component.InvokeGetTag(component, stiValueEventArgs3);
			component.TagValue = stiValueEventArgs3.Value;
		}
		else
		{
			if (component.Hyperlink.Value.Length > 0)
			{
				object obj = StiParser.ParseTextValue(component.Hyperlink.Value, component);
				component.HyperlinkValue = base.Report.ToString(obj);
			}
			if (component.ToolTip.Value.Length > 0)
			{
				object obj2 = StiParser.ParseTextValue(component.ToolTip.Value, component);
				component.ToolTipValue = base.Report.ToString(obj2);
			}
			if (component.Tag.Value.Length > 0)
			{
				object obj3 = StiParser.ParseTextValue(component.Tag.Value, component);
				component.TagValue = base.Report.ToString(obj3);
			}
		}
		component.InvokeBeforePrint(component, EventArgs.Empty);
		component.InvokeAfterPrint(component, EventArgs.Empty);
	}

	private void InvokeEvents(StiComponent component)
	{
		StiReport report = base.Report;
		if (report != null && report.CalculationMode == StiCalculationMode.Compilation)
		{
			StiValueEventArgs stiValueEventArgs = new StiValueEventArgs();
			component.InvokeGetHyperlink(component, stiValueEventArgs);
			component.HyperlinkValue = stiValueEventArgs.Value;
			StiValueEventArgs stiValueEventArgs2 = new StiValueEventArgs();
			component.InvokeGetToolTip(component, stiValueEventArgs2);
			component.ToolTipValue = stiValueEventArgs2.Value;
			StiValueEventArgs stiValueEventArgs3 = new StiValueEventArgs();
			component.InvokeGetTag(component, stiValueEventArgs3);
			component.TagValue = stiValueEventArgs3.Value;
		}
		else
		{
			if (component.Hyperlink.Value.Length > 0)
			{
				object obj = StiParser.ParseTextValue(component.Hyperlink.Value, component);
				component.HyperlinkValue = base.Report.ToString(obj);
			}
			if (component.ToolTip.Value.Length > 0)
			{
				object obj2 = StiParser.ParseTextValue(component.ToolTip.Value, component);
				component.ToolTipValue = base.Report.ToString(obj2);
			}
			if (component.Tag.Value.Length > 0)
			{
				object obj3 = StiParser.ParseTextValue(component.Tag.Value, component);
				component.TagValue = base.Report.ToString(obj3);
			}
		}
		component.InvokeBeforePrint(component, EventArgs.Empty);
		component.InvokeAfterPrint(component, EventArgs.Empty);
	}

	private void AddRowTotal(StiRowCollection rows, int level, int maxLevel, int setLevel)
	{
		while (true)
		{
			StiRow stiRow = new StiRow("", "")
			{
				IsTotal = true,
				Level = setLevel
			};
			StiGetValueEventArgs stiGetValueEventArgs = new StiGetValueEventArgs();
			StiCrossTotal total = ((StiCrossRow)RowFields[setLevel]).Total;
			total?.InvokeGetValue(total, stiGetValueEventArgs);
			stiRow.DisplayValue = stiGetValueEventArgs.Value;
			rows.Add(stiRow);
			level++;
			if (level >= maxLevel)
			{
				break;
			}
			rows = rows[rows.Count - 1].Rows;
		}
	}

	private void AddColTotal(StiColumnCollection cols, int level, int maxLevel, int setLevel)
	{
		while (true)
		{
			StiColumn stiColumn = new StiColumn("", "")
			{
				IsTotal = true,
				Level = setLevel
			};
			StiGetValueEventArgs stiGetValueEventArgs = new StiGetValueEventArgs();
			StiCrossTotal total = ((StiCrossColumn)ColFields[setLevel]).Total;
			total?.InvokeGetValue(total, stiGetValueEventArgs);
			stiColumn.DisplayValue = stiGetValueEventArgs.Value;
			cols.Add(stiColumn);
			level++;
			if (level >= maxLevel)
			{
				break;
			}
			cols = cols[cols.Count - 1].Cols;
		}
	}

	private void SortRows()
	{
		SortRows(Rows, 0, GetRowsHeaderWidth());
	}

	private void SortRows(StiRowCollection rows, int level, int width)
	{
		foreach (StiRow row in rows)
		{
			SortRows(row.Rows, level + 1, width);
		}
		if (level < width)
		{
			StiSortDirection sortDirection = (RowFields[level] as StiCrossRow).SortDirection;
			if (sortDirection != StiSortDirection.None)
			{
				rows.Sort(sortDirection, (RowFields[level] as StiCrossRow).SortType);
			}
		}
	}

	private void SortCols()
	{
		SortCols(Cols, 0, GetColsHeaderHeight());
	}

	private void SortCols(StiColumnCollection cols, int level, int height)
	{
		foreach (StiColumn col in cols)
		{
			SortCols(col.Cols, level + 1, height);
		}
		if (level < height)
		{
			StiSortDirection sortDirection = (ColFields[level] as StiCrossColumn).SortDirection;
			if (sortDirection != StiSortDirection.None)
			{
				cols.Sort(sortDirection, (ColFields[level] as StiCrossColumn).SortType);
			}
		}
	}

	private void CreateRowTotals()
	{
		int num = GetRowsHeaderWidth();
		if (num > 0)
		{
			CreateRowTotals(Rows, 0, num);
		}
	}

	private void CreateRowTotals(StiRowCollection rows, int level, int maxLevel)
	{
		if (rows.Count <= 0)
		{
			return;
		}
		foreach (StiRow row2 in rows)
		{
			CreateRowTotals(row2.Rows, level + 1, maxLevel);
		}
		StiCrossRow row = RowFields[level] as StiCrossRow;
		StiCrossRow prevRow = ((level == 0) ? null : (RowFields[level - 1] as StiCrossRow));
		if (AllowTotal(row, prevRow))
		{
			AddRowTotal(rows, level, maxLevel, level);
		}
	}

	private void CreateColTotals()
	{
		int num = GetColsHeaderHeight();
		if (num > 0)
		{
			CreateColTotals(Cols, 0, num);
		}
	}

	private void CreateColTotals(StiColumnCollection cols, int level, int maxLevel)
	{
		if (cols.Count <= 0)
		{
			return;
		}
		foreach (StiColumn col in cols)
		{
			CreateColTotals(col.Cols, level + 1, maxLevel);
		}
		StiCrossColumn row = ColFields[level] as StiCrossColumn;
		StiCrossColumn prevRow = ((level == 0) ? null : (ColFields[level - 1] as StiCrossColumn));
		if (AllowTotal(row, prevRow))
		{
			AddColTotal(cols, level, maxLevel, level);
		}
	}

	private object GetDataFromDataRow(DataRow dataRow, string field)
	{
		if (dataRow == null)
		{
			return "";
		}
		return dataRow[field] ?? "";
	}

	private object GetValueFromDataRow(DataRow dataRow, StiCrossSummary field)
	{
		if (dataRow == null)
		{
			return null;
		}
		try
		{
			object obj = dataRow[field.Name];
			switch (field.Summary)
			{
			case StiSummaryType.Count:
			case StiSummaryType.CountDistinct:
			case StiSummaryType.Image:
				return obj;
			case StiSummaryType.None:
			{
				if (obj is DateTime)
				{
					return obj;
				}
				if (!field.HideZeros)
				{
					return obj;
				}
				decimal? num = null;
				try
				{
					if (obj is string text2)
					{
						if (!text2.Any(char.IsDigit))
						{
							return obj;
						}
						string s = text2.Replace(".", ",").Replace(",", Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator).Replace(" ", "");
						if (decimal.TryParse(s, out var result2))
						{
							num = result2;
						}
					}
					else
					{
						num = (decimal)Convert.ChangeType(obj, typeof(decimal));
					}
					decimal? num2 = num;
					if ((num2.GetValueOrDefault() == default(decimal)) & num2.HasValue)
					{
						return string.Empty;
					}
				}
				catch
				{
				}
				return obj;
			}
			default:
				if (obj is string)
				{
					string text = obj as string;
					if (text.Length == 0)
					{
						return null;
					}
					text = text.Replace(".", ",").Replace(",", Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
					decimal result;
					return decimal.TryParse(text, out result) ? result : 0m;
				}
				if (obj is DateTime)
				{
					return obj;
				}
				if (obj == null)
				{
					return 0m;
				}
				if (obj == DBNull.Value)
				{
					return 0m;
				}
				return (decimal)Convert.ChangeType(obj, typeof(decimal));
			}
		}
		catch
		{
			return 0;
		}
	}

	private bool AllowTotal(StiCrossHeader row, StiCrossHeader prevRow)
	{
		if (row == null)
		{
			return false;
		}
		if (prevRow != null && prevRow.Interaction is StiCrossHeaderInteraction { CollapsingEnabled: not false })
		{
			return false;
		}
		return row.IsTotalVisible;
	}

	private StiRow GetRow(DataRow dataRow)
	{
		bool flag = false;
		StiRowCollection rows = Rows;
		StiRow stiRow = null;
		int num = 0;
		foreach (StiCrossRow rowField in rowFields)
		{
			object obj = "";
			object displayValue = "";
			if (rowField.Name != "EMPTY_FIELD")
			{
				obj = GetDataFromDataRow(dataRow, rowField.Name);
				displayValue = GetDataFromDataRow(dataRow, "Display__" + rowField.Name);
			}
			if (flag)
			{
				obj = "all";
				displayValue = "all";
			}
			stiRow = rows[obj];
			if (stiRow == null)
			{
				stiRow = new StiRow(obj, displayValue);
				StiValueEventArgs stiValueEventArgs = new StiValueEventArgs();
				rowField.InvokeGetHyperlink(rowField, stiValueEventArgs);
				stiRow.HyperlinkValue = stiValueEventArgs.Value;
				StiValueEventArgs stiValueEventArgs2 = new StiValueEventArgs();
				rowField.InvokeGetToolTip(rowField, stiValueEventArgs2);
				stiRow.ToolTipValue = stiValueEventArgs2.Value;
				StiValueEventArgs stiValueEventArgs3 = new StiValueEventArgs();
				rowField.InvokeGetTag(rowField, stiValueEventArgs3);
				stiRow.TagValue = stiValueEventArgs3.Value;
				rows.Add(stiRow);
			}
			flag = StiCrossTabV2Builder.IsCollapsed(rowField, num, obj as string);
			rows = stiRow.Rows;
			num++;
		}
		return stiRow;
	}

	private StiColumn GetColumn(DataRow dataRow)
	{
		bool flag = false;
		StiColumnCollection cols = Cols;
		StiColumn stiColumn = null;
		int num = 0;
		foreach (StiCrossColumn colField in colFields)
		{
			object obj = string.Empty;
			object displayValue = string.Empty;
			if (colField.Name != "EMPTY_FIELD")
			{
				obj = GetDataFromDataRow(dataRow, colField.Name);
				displayValue = GetDataFromDataRow(dataRow, "Display__" + colField.Name);
			}
			if (flag)
			{
				obj = "all";
				displayValue = "all";
			}
			stiColumn = cols[obj];
			if (stiColumn == null)
			{
				stiColumn = new StiColumn(obj, displayValue);
				StiValueEventArgs stiValueEventArgs = new StiValueEventArgs();
				colField.InvokeGetHyperlink(colField, stiValueEventArgs);
				stiColumn.HyperlinkValue = stiValueEventArgs.Value;
				StiValueEventArgs stiValueEventArgs2 = new StiValueEventArgs();
				colField.InvokeGetToolTip(colField, stiValueEventArgs2);
				stiColumn.ToolTipValue = stiValueEventArgs2.Value;
				StiValueEventArgs stiValueEventArgs3 = new StiValueEventArgs();
				colField.InvokeGetTag(colField, stiValueEventArgs3);
				stiColumn.TagValue = stiValueEventArgs3.Value;
				cols.Add(stiColumn);
			}
			flag = StiCrossTabV2Builder.IsCollapsed(colField, num, obj as string);
			cols = stiColumn.Cols;
			num++;
		}
		return stiColumn;
	}

	private void CalculateTopN()
	{
		for (int num = colFields.Count - 1; num >= 0; num--)
		{
			StiCrossColumn stiCrossColumn = colFields[num] as StiCrossColumn;
			int sumFiledIndex = GetSumFiledIndex(stiCrossColumn.TopN);
			if (sumFiledIndex >= 0)
			{
				ProcessTopNColumns(num, 0, sumFiledIndex, Cols, stiCrossColumn.TopN);
			}
		}
		for (int num2 = rowFields.Count - 1; num2 >= 0; num2--)
		{
			StiCrossRow stiCrossRow = rowFields[num2] as StiCrossRow;
			int sumFiledIndex2 = GetSumFiledIndex(stiCrossRow.TopN);
			if (sumFiledIndex2 >= 0)
			{
				ProcessTopNRows(num2, 0, sumFiledIndex2, Rows, stiCrossRow.TopN);
			}
		}
	}

	private void ProcessTopNRows(int level, int curLevel, int sumIndex, StiRowCollection rows, StiDataTopN topN)
	{
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Invalid comparison between Unknown and I4
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Invalid comparison between Unknown and I4
		if (curLevel < level)
		{
			foreach (StiRow row in rows)
			{
				ProcessTopNRows(level, curLevel + 1, sumIndex, row.Rows, topN);
			}
			return;
		}
		SortedList sortedList = new SortedList();
		foreach (StiRow row2 in rows)
		{
			List<StiRow> list = new List<StiRow>();
			GetRowsArray(row2.Rows, list);
			if (list.Count == 0)
			{
				list.Add(row2);
			}
			ArrayList arrayList = new ArrayList();
			foreach (StiRow item in list)
			{
				foreach (StiColumn key in SummaryContainer.GetDataCol().Keys)
				{
					if (!((SummaryContainer.GetDataCol()[key] as Hashtable)[item] is StiSummary stiSummary))
					{
						continue;
					}
					foreach (object item2 in stiSummary.Sums[sumIndex])
					{
						arrayList.Add(item2);
					}
				}
			}
			object summaryResult = GetSummaryResult(arrayList, sumIndex, grandTotal: false);
			if (sortedList[summaryResult] == null)
			{
				sortedList[summaryResult] = new List<StiRow>();
			}
			(sortedList[summaryResult] as List<StiRow>).Add(row2);
		}
		List<StiRow> list2 = new List<StiRow>();
		foreach (object key2 in sortedList.Keys)
		{
			list2.AddRange(sortedList[key2] as List<StiRow>);
		}
		List<StiRow> list3 = new List<StiRow>();
		for (int i = (((int)topN.Mode == 1) ? Math.Max(0, list2.Count - topN.Count) : 0); i < (((int)topN.Mode == 1) ? list2.Count : Math.Min(list2.Count, topN.Count)); i++)
		{
			list3.Add(list2[i]);
		}
		int num = 0;
		List<StiRow> list4 = new List<StiRow>();
		while (num < rows.Count)
		{
			if (!list3.Contains(rows[num]))
			{
				list4.Add(rows[num]);
				rows.RemoveAt(num);
			}
			else
			{
				num++;
			}
		}
		if (topN.ShowOthers && list4.Count > 0)
		{
			StiRow stiRow3 = new StiRow(topN.OthersText, topN.OthersText)
			{
				OthersText = topN.OthersText
			};
			rows.Add(stiRow3);
			FillOtherRows(stiRow3, list4);
		}
	}

	private void FillOtherRows(StiRow otherRow, IList rows)
	{
		foreach (StiRow row in rows)
		{
			if (row.Rows.Count == 0)
			{
				foreach (StiColumn key in SummaryContainer.GetDataCol().Keys)
				{
					if (!((SummaryContainer.GetDataCol()[key] as Hashtable)[row] is StiSummary stiSummary))
					{
						continue;
					}
					StiSummary summary = SummaryContainer.GetSummary(key, otherRow);
					int num = 0;
					foreach (StiCrossSummary sumField in sumFields)
					{
						if (sumField.Name != "EMPTY_FIELD")
						{
							if (summary.Sums[num].Count == 0)
							{
								summary.HyperlinkValues = stiSummary.HyperlinkValues;
								summary.ToolTipValues = stiSummary.ToolTipValues;
								summary.TagValues = stiSummary.TagValues;
								summary.DrillDownParameters = stiSummary.DrillDownParameters;
							}
							summary.Sums[num].AddRange(stiSummary.Sums[num]);
							num++;
						}
					}
				}
				continue;
			}
			foreach (StiRow row2 in row.Rows)
			{
				if (otherRow.Rows[row2.Value] == null)
				{
					otherRow.Rows.Add(row2);
					continue;
				}
				List<StiRow> list = new List<StiRow>();
				list.Add(row2);
				FillOtherRows(otherRow.Rows[row2.Value], list);
			}
		}
	}

	private void ProcessTopNColumns(int level, int curLevel, int sumIndex, StiColumnCollection cols, StiDataTopN topN)
	{
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Invalid comparison between Unknown and I4
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Invalid comparison between Unknown and I4
		if (curLevel < level)
		{
			foreach (StiColumn col in cols)
			{
				ProcessTopNColumns(level, curLevel + 1, sumIndex, col.Cols, topN);
			}
			return;
		}
		SortedList sortedList = new SortedList();
		foreach (StiColumn col2 in cols)
		{
			List<StiColumn> list = new List<StiColumn>();
			GetColsArray(col2.Cols, list);
			if (list.Count == 0)
			{
				list.Add(col2);
			}
			ArrayList arrayList = new ArrayList();
			foreach (StiColumn item in list)
			{
				if (!(SummaryContainer.GetDataCol()[item] is Hashtable hashtable))
				{
					continue;
				}
				foreach (StiSummary value in hashtable.Values)
				{
					foreach (object item2 in value.Sums[sumIndex])
					{
						arrayList.Add(item2);
					}
				}
			}
			object summaryResult = GetSummaryResult(arrayList, sumIndex, grandTotal: false);
			if (sortedList[summaryResult] == null)
			{
				sortedList[summaryResult] = new List<StiColumn>();
			}
			(sortedList[summaryResult] as List<StiColumn>).Add(col2);
		}
		List<StiColumn> list2 = new List<StiColumn>();
		foreach (object key in sortedList.Keys)
		{
			list2.AddRange(sortedList[key] as List<StiColumn>);
		}
		List<StiColumn> list3 = new List<StiColumn>();
		for (int i = (((int)topN.Mode == 1) ? Math.Max(0, list2.Count - topN.Count) : 0); i < (((int)topN.Mode == 1) ? list2.Count : Math.Min(list2.Count, topN.Count)); i++)
		{
			list3.Add(list2[i]);
		}
		int num = 0;
		List<StiColumn> list4 = new List<StiColumn>();
		while (num < cols.Count)
		{
			if (!list3.Contains(cols[num]))
			{
				list4.Add(cols[num]);
				cols.RemoveAt(num);
			}
			else
			{
				num++;
			}
		}
		if (topN.ShowOthers && list4.Count > 0)
		{
			StiColumn stiColumn3 = new StiColumn(topN.OthersText, topN.OthersText)
			{
				OthersText = topN.OthersText
			};
			cols.Add(stiColumn3);
			FillOtherColumns(stiColumn3, list4);
		}
	}

	private void FillOtherColumns(StiColumn otherCol, IList cols)
	{
		foreach (StiColumn col in cols)
		{
			if (col.Cols.Count == 0)
			{
				if (!(SummaryContainer.GetDataCol()[col] is Hashtable hashtable))
				{
					continue;
				}
				foreach (StiRow key in hashtable.Keys)
				{
					StiSummary summary = SummaryContainer.GetSummary(otherCol, key);
					StiSummary stiSummary = hashtable[key] as StiSummary;
					int num = 0;
					foreach (StiCrossSummary sumField in sumFields)
					{
						if (sumField.Name != "EMPTY_FIELD")
						{
							if (summary.Sums[num].Count == 0)
							{
								summary.HyperlinkValues = stiSummary.HyperlinkValues;
								summary.ToolTipValues = stiSummary.ToolTipValues;
								summary.TagValues = stiSummary.TagValues;
								summary.DrillDownParameters = stiSummary.DrillDownParameters;
							}
							summary.Sums[num].AddRange(stiSummary.Sums[num]);
							num++;
						}
					}
				}
				continue;
			}
			foreach (StiColumn col2 in col.Cols)
			{
				if (otherCol.Cols[col2.Value] == null)
				{
					otherCol.Cols.Add(col2);
					continue;
				}
				List<StiColumn> list = new List<StiColumn>();
				list.Add(col2);
				FillOtherColumns(otherCol.Cols[col2.Value], list);
			}
		}
	}

	private int GetSumFiledIndex(StiDataTopN topN)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		if (topN != null && (int)topN.Mode != 0 && !string.IsNullOrEmpty(topN.MeasureField))
		{
			for (int i = 0; i < sumFields.Count; i++)
			{
				if (sumFields[i].Alias.EndsWith(topN.MeasureField + ")"))
				{
					return i;
				}
			}
		}
		return -1;
	}

	private void CalculateDataTable(DataTable table)
	{
		if (table == null || table.Rows.Count == 0)
		{
			CalculateDataRow(null);
		}
		else if (CrossTab.DataSource != null)
		{
			int position = CrossTab.DataSource.Position;
			int num = 0;
			foreach (DataRow row in table.Rows)
			{
				CrossTab.DataSource.Position = num++;
				CalculateDataRow(row);
			}
			CrossTab.DataSource.Position = position;
		}
		else
		{
			if (CrossTab.BusinessObject == null)
			{
				return;
			}
			int position2 = CrossTab.BusinessObject.Position;
			int num2 = 0;
			foreach (DataRow row2 in table.Rows)
			{
				CrossTab.BusinessObject.Position = num2++;
				CalculateDataRow(row2);
			}
			CrossTab.BusinessObject.Position = position2;
		}
	}

	private void CalculateDataRow(DataRow dataRow)
	{
		StiRow row = GetRow(dataRow);
		StiColumn column = GetColumn(dataRow);
		StiSummary summary = SummaryContainer.GetSummary(column, row);
		int num = 0;
		foreach (StiCrossSummary sumField in sumFields)
		{
			if (!(sumField.Name != "EMPTY_FIELD"))
			{
				continue;
			}
			object valueFromDataRow = GetValueFromDataRow(dataRow, sumField);
			summary.Sums[num].Add(valueFromDataRow);
			InvokeEvents(sumField);
			summary.HyperlinkValues[num] = sumField.HyperlinkValue;
			summary.ToolTipValues[num] = sumField.ToolTipValue;
			summary.TagValues[num] = sumField.TagValue;
			if (sumField.DrillDownParameters != null)
			{
				if (summary.DrillDownParameters == null)
				{
					summary.DrillDownParameters = new Dictionary<string, object>[summary.TagValues.Length];
				}
				summary.DrillDownParameters[num] = sumField.DrillDownParameters;
			}
			num++;
		}
		foreach (StiCrossRow rowField in rowFields)
		{
			if (!(rowField.Name != "EMPTY_FIELD"))
			{
				continue;
			}
			InvokeEvents(rowField);
			row.HyperlinkValue = rowField.HyperlinkValue;
			row.ToolTipValue = rowField.ToolTipValue;
			row.TagValue = rowField.TagValue;
			if (rowField.DrillDownParameters != null)
			{
				if (row.DrillDownParameters == null)
				{
					row.DrillDownParameters = new Dictionary<string, object>();
				}
				row.DrillDownParameters = rowField.DrillDownParameters;
			}
		}
		foreach (StiCrossColumn colField in colFields)
		{
			if (!(colField.Name != "EMPTY_FIELD"))
			{
				continue;
			}
			InvokeEvents(colField);
			column.HyperlinkValue = colField.HyperlinkValue;
			column.ToolTipValue = colField.ToolTipValue;
			column.TagValue = colField.TagValue;
			if (colField.DrillDownParameters != null)
			{
				if (column.DrillDownParameters == null)
				{
					column.DrillDownParameters = new Dictionary<string, object>();
				}
				column.DrillDownParameters = colField.DrillDownParameters;
			}
		}
	}

	private int CopyRows(StiRowCollection rows, int left, int top, int level, out string parentGuid)
	{
		int num = 0;
		parentGuid = Guid.NewGuid().ToString();
		foreach (StiRow row in rows)
		{
			string parentGuid2 = "";
			int num2 = CopyRows(row.Rows, left + 1, top, level + 1, out parentGuid2);
			StiCrossField stiCrossField = RowFields[level] as StiCrossField;
			num += num2;
			int num3 = 1;
			if (StiCrossTabV2Builder.IsCollapsed(stiCrossField as StiCrossHeader, level, row.Value as string))
			{
				StiRow stiRow2 = row;
				while (stiRow2.Rows.Count != 0)
				{
					num3++;
					stiRow2 = stiRow2.Rows[0];
				}
			}
			Rectangle rectangle = new Rectangle(left, top, num3, num2);
			if (row.IsTotal)
			{
				rectangle.Width = rowsHeaderWidth + widthCorrection - rectangle.X - ((IsSummarySubHeadersPresent && summaryDirection == StiSummaryDirection.UpToDown) ? 1 : 0);
				stiCrossField = ((StiCrossHeader)stiCrossField).Total;
			}
			string text = stiCrossField.TextFormat.Format(row.DisplayValue);
			InvokeEvents(stiCrossField, text);
			bool isNumeric = stiCrossField is StiCrossRow && !(stiCrossField.TextFormat is StiGeneralFormatService);
			if (stiCrossField.MergeHeaders || base.Report.IsDesigning)
			{
				SetCell(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, text, text, stiCrossField, isNumeric, row.HyperlinkValue, row.ToolTipValue, row.TagValue, row.DrillDownParameters, level, parentGuid, parentGuid2, stiCrossField is StiCrossRow && (stiCrossField as StiCrossRow).KeepMergedCellsTogether, row.IsTotal ? StiCellType.HeaderRowTotal : StiCellType.HeaderRow);
			}
			else
			{
				for (int i = 0; i < rectangle.Height; i++)
				{
					SetCell(rectangle.X, rectangle.Y + i, rectangle.Width, 1, text, text, stiCrossField, isNumeric, row.HyperlinkValue, row.ToolTipValue, row.TagValue, row.DrillDownParameters, level, parentGuid, parentGuid2, stiCrossField is StiCrossRow && (stiCrossField as StiCrossRow).KeepMergedCellsTogether, row.IsTotal ? StiCellType.HeaderRowTotal : StiCellType.HeaderRow);
				}
			}
			top += rectangle.Height;
		}
		return Math.Max(num, oneCellHeight);
	}

	private int CopyCols(StiColumnCollection cols, int left, int top, int level, out string parentGuid)
	{
		int num = 0;
		parentGuid = Guid.NewGuid().ToString();
		foreach (StiColumn col in cols)
		{
			string parentGuid2 = "";
			int num2 = CopyCols(col.Cols, left, top + 1, level + 1, out parentGuid2);
			StiCrossField stiCrossField = ColFields[level] as StiCrossField;
			num += num2;
			int num3 = 1;
			if (StiCrossTabV2Builder.IsCollapsed(stiCrossField as StiCrossHeader, level, col.Value as string))
			{
				StiColumn stiColumn2 = col;
				while (stiColumn2.Cols.Count != 0)
				{
					num3++;
					stiColumn2 = stiColumn2.Cols[0];
				}
			}
			Rectangle rectangle = new Rectangle(left, top, num2, num3);
			if (col.IsTotal)
			{
				rectangle.Height = colsHeaderHeight + heightCorrection - rectangle.Y - ((IsSummarySubHeadersPresent && summaryDirection == StiSummaryDirection.LeftToRight) ? 1 : 0);
				stiCrossField = ((StiCrossHeader)stiCrossField).Total;
			}
			string text = stiCrossField.TextFormat.Format(col.DisplayValue);
			InvokeEvents(stiCrossField, text);
			bool isNumeric = stiCrossField is StiCrossColumn && !(stiCrossField.TextFormat is StiGeneralFormatService);
			if (stiCrossField.MergeHeaders || base.Report.IsDesigning)
			{
				SetCell(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, text, text, stiCrossField, isNumeric, col.HyperlinkValue, col.ToolTipValue, col.TagValue, col.DrillDownParameters, level, parentGuid, parentGuid2, keepMergedCellsTogether: false, col.IsTotal ? StiCellType.HeaderColTotal : StiCellType.HeaderCol);
			}
			else
			{
				for (int i = 0; i < rectangle.Width; i++)
				{
					SetCell(rectangle.X + i, rectangle.Y, 1, rectangle.Height, text, text, stiCrossField, isNumeric, col.HyperlinkValue, col.ToolTipValue, col.TagValue, col.DrillDownParameters, level, parentGuid, parentGuid2, keepMergedCellsTogether: false, col.IsTotal ? StiCellType.HeaderColTotal : StiCellType.HeaderCol);
				}
			}
			left += rectangle.Width;
		}
		return Math.Max(num, oneCellWidth);
	}

	private void CopySummaries(int left, int top, object emptyValue)
	{
		List<StiRow> rowsArray = GetRowsArray();
		List<StiColumn> colsArray = GetColsArray();
		ArrayList[] array = new ArrayList[colsHeaderHeight * oneCellSize];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = new ArrayList();
		}
		int num = 0;
		int num2 = 0;
		foreach (StiRow item in rowsArray)
		{
			ArrayList[] array2 = array;
			foreach (ArrayList arrayList in array2)
			{
				arrayList.Clear();
			}
			num2 = 0;
			foreach (StiColumn item2 in colsArray)
			{
				if (item2.IsTotal)
				{
					for (int k = 0; k < oneCellSize; k++)
					{
						StiCrossSummary stiCrossSummary = SumFields[k] as StiCrossSummary;
						ArrayList arrayList2 = array[item2.Level * oneCellSize + k];
						object summary = GetSummary(arrayList2, k, grandTotal: true);
						arrayList2.Clear();
						int num3 = ((summaryDirection == StiSummaryDirection.UpToDown) ? (left + num2) : (left + num2 * oneCellSize + k));
						int num4 = ((summaryDirection == StiSummaryDirection.UpToDown) ? (top + num * oneCellSize + k) : (top + num));
						StiSummary summary2 = SummaryContainer.GetSummary(item2, item, create: true);
						summary2.Sums[k].Add(summary);
						InvokeEvents(stiCrossSummary);
						StiCell stiCell = SetCellValue(num3, num4, summary, k, item2.Level, StiFieldType.Column, stiCrossSummary.HyperlinkValue, stiCrossSummary.ToolTipValue, stiCrossSummary.TagValue, stiCrossSummary.DrillDownParameters);
						stiCell.SummaryIndex = k;
						stiCell.IsCrossSummary = true;
						if (stiCrossSummary.Summary == StiSummaryType.Image || stiCrossSummary.Summary == StiSummaryType.None)
						{
							stiCell.Value = null;
							stiCell.Text = string.Empty;
						}
						CalculateColumnPercentages(stiCell, num3, num4);
						if (stiCrossSummary.UseStyleOfSummaryInColumnTotal)
						{
							SetCellField(num3, num4, ((StiService)stiCrossSummary).Clone() as StiCrossField);
						}
					}
				}
				else
				{
					StiSummary summary3 = SummaryContainer.GetSummary(item2, item, create: false);
					object[] array3 = CopySummary(summary3, left, top, num2, num, setValue: true, grandTotal: false, emptyValue);
					for (int l = 0; l < colsHeaderHeight; l++)
					{
						for (int m = 0; m < oneCellSize; m++)
						{
							ArrayList arrayList3 = array[l * oneCellSize + m];
							arrayList3.Add(array3[m]);
						}
					}
				}
				num2++;
			}
			num++;
		}
		array = new ArrayList[rowsHeaderWidth * oneCellSize];
		for (int n = 0; n < array.Length; n++)
		{
			array[n] = new ArrayList();
		}
		num2 = 0;
		foreach (StiColumn item3 in colsArray)
		{
			for (int num5 = 0; num5 < rowsHeaderWidth * oneCellSize; num5++)
			{
				array[num5].Clear();
			}
			num = 0;
			foreach (StiRow item4 in rowsArray)
			{
				if (item4.IsTotal)
				{
					for (int num6 = 0; num6 < oneCellSize; num6++)
					{
						StiCrossSummary stiCrossSummary2 = SumFields[num6] as StiCrossSummary;
						ArrayList arrayList4 = array[item4.Level * oneCellSize + num6];
						object summary4 = GetSummary(arrayList4, num6, grandTotal: true);
						arrayList4.Clear();
						int num7 = ((summaryDirection == StiSummaryDirection.UpToDown) ? (left + num2) : (left + num2 * oneCellSize + num6));
						int num8 = ((summaryDirection == StiSummaryDirection.UpToDown) ? (top + num * oneCellSize + num6) : (top + num));
						InvokeEvents(stiCrossSummary2);
						StiCell stiCell2 = SetCellValue(num7, num8, summary4, num6, item4.Level, StiFieldType.Row, stiCrossSummary2.HyperlinkValue, stiCrossSummary2.ToolTipValue, stiCrossSummary2.TagValue, stiCrossSummary2.DrillDownParameters);
						stiCell2.SummaryIndex = num6;
						if (stiCrossSummary2.Summary == StiSummaryType.Image || stiCrossSummary2.Summary == StiSummaryType.None)
						{
							stiCell2.Value = null;
							stiCell2.Text = string.Empty;
						}
						CalculateRowPercentages(stiCell2, num7, num8);
						if (stiCrossSummary2.UseStyleOfSummaryInRowTotal)
						{
							SetCellField(num7, num8, ((StiService)stiCrossSummary2).Clone() as StiCrossField);
						}
					}
				}
				else
				{
					StiSummary summary5 = SummaryContainer.GetSummary(item3, item4, create: false);
					object[] array4 = CopySummary(summary5, left, top, num2, num, setValue: false, item3.IsTotal, emptyValue);
					for (int num9 = 0; num9 < rowsHeaderWidth; num9++)
					{
						for (int num10 = 0; num10 < oneCellSize; num10++)
						{
							ArrayList arrayList5 = array[num9 * oneCellSize + num10];
							arrayList5.Add(array4[num10]);
						}
					}
				}
				num++;
			}
			num2++;
		}
	}

	private void CalculateColumnPercentages(StiCell cell, int x, int y)
	{
		bool flag = false;
		decimal num = ((cell.IsNumeric && cell.Value != null) ? ((decimal)cell.Value) : 0m);
		for (int num2 = x - 1; num2 >= 0; num2--)
		{
			StiCell stiCell = base.Cells[num2][y];
			if (!(stiCell.Field is StiCrossSummary stiCrossSummary))
			{
				break;
			}
			decimal num3 = (stiCell.IsNumeric ? ((decimal)stiCell.Value) : 0m);
			num3 = ((num != 0m) ? Math.Round(num3 / num * 100m, 2) : 0m);
			if (stiCell.IsNumeric && stiCrossSummary.ShowPercents)
			{
				stiCell.Value = num3;
				stiCell.Text = $"{num3}%";
				flag = true;
			}
		}
		if (flag && cell.Field is StiCrossTotal)
		{
			cell.Text = "100%";
		}
	}

	private void CalculateRowPercentages(StiCell cell, int x, int y)
	{
		bool flag = false;
		decimal num = ((cell.IsNumeric && cell.Value != null) ? ((decimal)cell.Value) : 0m);
		for (int num2 = y - 1; num2 >= 0; num2--)
		{
			StiCell stiCell = base.Cells[x][num2];
			if (!(stiCell.Field is StiCrossSummary stiCrossSummary))
			{
				break;
			}
			decimal num3 = (stiCell.IsNumeric ? ((decimal)stiCell.Value) : 0m);
			num3 = ((num != 0m) ? Math.Round(num3 / num * 100m, 2) : 0m);
			if (stiCell.IsNumeric && stiCrossSummary.ShowPercents)
			{
				stiCell.Value = num3;
				stiCell.Text = $"{num3}%";
				flag = true;
			}
		}
		if (flag && cell.Field is StiCrossTotal)
		{
			cell.Text = "100%";
		}
	}

	private object[] CopySummary(StiSummary summary, int left, int top, int colIndex, int rowIndex, bool setValue, bool grandTotal, object emptyValue)
	{
		object[] array = new object[oneCellSize];
		for (int i = 0; i < oneCellSize; i++)
		{
			object value = emptyValue;
			if (summary != null)
			{
				value = (array[i] = GetSummary(summary, i, grandTotal));
			}
			int num = 0;
			int num2 = 0;
			if (summaryDirection == StiSummaryDirection.UpToDown)
			{
				num = left + colIndex;
				num2 = top + rowIndex * oneCellSize + i;
			}
			else
			{
				num = left + colIndex * oneCellSize + i;
				num2 = top + rowIndex;
			}
			object hyperlinkValue = ((summary != null) ? summary.HyperlinkValues[i] : null);
			object toolTipValue = ((summary != null) ? summary.ToolTipValues[i] : null);
			object tagValue = ((summary != null) ? summary.TagValues[i] : null);
			Dictionary<string, object> drillDownParameters = ((summary != null && summary.DrillDownParameters != null) ? summary.DrillDownParameters[i] : null);
			if (setValue && num < base.Widths.Length && num2 < base.Heights.Length)
			{
				StiCell stiCell = SetCellValue(num, num2, value, i, 0, StiFieldType.Cell, hyperlinkValue, toolTipValue, tagValue, drillDownParameters);
				stiCell.SummaryIndex = i;
			}
		}
		return array;
	}

	private StiCell SetCellValue(int x, int y, object value, int calcIndex, int level, StiFieldType fieldType, object hyperlinkValue, object toolTipValue, object tagValue, Dictionary<string, object> drillDownParameters)
	{
		switch (fieldType)
		{
		case StiFieldType.Column:
		{
			StiCrossField columnTotalCell = GetColumnTotalCell(level, calcIndex);
			string text3 = ConvertValueToString(value, columnTotalCell);
			StiCell stiCell3 = SetCell(x, y, 1, 1, text3, value, columnTotalCell, value is decimal, hyperlinkValue, toolTipValue, tagValue, drillDownParameters);
			CheckNegativeColor(value, columnTotalCell, stiCell3);
			return stiCell3;
		}
		case StiFieldType.Row:
		{
			StiCrossField rowTotalCell = GetRowTotalCell(level, calcIndex);
			string text2 = ConvertValueToString(value, rowTotalCell);
			StiCell stiCell2 = SetCell(x, y, 1, 1, text2, value, rowTotalCell, value is decimal, hyperlinkValue, toolTipValue, tagValue, drillDownParameters);
			CheckNegativeColor(value, rowTotalCell, stiCell2);
			return stiCell2;
		}
		case StiFieldType.Cell:
		{
			StiCrossField stiCrossField = SumFields[calcIndex] as StiCrossField;
			string text = ConvertValueToString(value, stiCrossField);
			StiCell stiCell = SetCell(x, y, 1, 1, text, value, stiCrossField, value is decimal, hyperlinkValue, toolTipValue, tagValue, drillDownParameters);
			CheckNegativeColor(value, stiCrossField, stiCell);
			return stiCell;
		}
		default:
			return null;
		}
	}

	private static void CheckNegativeColor(object value, StiCrossField field3, StiCell cell)
	{
		if (StiNegativeColorChecker.IsNegativeInRed(field3.TextFormat))
		{
			decimal? num = StiValueHelper.TryToNullableDecimal(value);
			cell.IsNegativeColor = num.HasValue && num.Value < 0m;
		}
	}

	private object GetSummary(StiSummary summary, int sumIndex, bool grandTotal)
	{
		StiCrossSummary stiCrossSummary = SumFields[sumIndex] as StiCrossSummary;
		ArrayList arrayList = new ArrayList();
		foreach (object item in summary.Sums[sumIndex])
		{
			arrayList.Add(item);
		}
		object summaryResult = GetSummaryResult(arrayList, sumIndex, grandTotal);
		if (!IsHideZeros(summaryResult) || !stiCrossSummary.HideZeros)
		{
			return summaryResult;
		}
		return CrossTab.EmptyValue;
	}

	private object GetSummary(ArrayList sums, int sumIndex, bool grandTotal)
	{
		StiCrossSummary stiCrossSummary = SumFields[sumIndex] as StiCrossSummary;
		object summaryResult = GetSummaryResult(sums, sumIndex, grandTotal);
		if (!IsHideZeros(summaryResult) || !stiCrossSummary.HideZeros)
		{
			return summaryResult;
		}
		return CrossTab.EmptyValue;
	}

	private object GetSummaryResult(ArrayList sums, int totalIndex, bool grandTotal)
	{
		StiCrossSummary stiCrossSummary = SumFields[totalIndex] as StiCrossSummary;
		StiSummaryType stiSummaryType = stiCrossSummary.Summary;
		StiSummaryValues summaryValues = stiCrossSummary.SummaryValues;
		if (grandTotal && (stiSummaryType == StiSummaryType.Count || stiSummaryType == StiSummaryType.CountDistinct))
		{
			stiSummaryType = StiSummaryType.Sum;
		}
		switch (stiSummaryType)
		{
		case StiSummaryType.None:
		case StiSummaryType.Image:
			return GetsSummaryResultImageAndNone(sums, totalIndex);
		case StiSummaryType.Sum:
			return GetSummaryResultSum(sums);
		case StiSummaryType.Max:
			return GetsSummaryResultMax(sums, stiCrossSummary);
		case StiSummaryType.Min:
			return GetsSummaryResultMin(sums, stiCrossSummary);
		case StiSummaryType.Count:
			return GetsSummaryResultCount(sums, summaryValues);
		case StiSummaryType.Median:
			return Funcs.Median((object)sums);
		case StiSummaryType.Average:
			return GetSummaryResultAverage(sums, summaryValues);
		case StiSummaryType.CountDistinct:
			return GetSummaryResultCountDistinct(sums, summaryValues);
		default:
			return 0m;
		}
	}

	private object GetsSummaryResultImageAndNone(ArrayList sums, int totalIndex)
	{
		int num = 0;
		if (totalIndex != 0)
		{
			for (int num2 = totalIndex - 1; num2 >= 0; num2--)
			{
				StiCrossSummary stiCrossSummary = SumFields[num2] as StiCrossSummary;
				if (stiCrossSummary.IndexOfSelectValue != -1)
				{
					num = stiCrossSummary.IndexOfSelectValue;
					if (num < sums.Count)
					{
						break;
					}
					num = 0;
				}
			}
		}
		if (sums == null || sums.Count == 0)
		{
			return string.Empty;
		}
		return sums[num];
	}

	private object GetSummaryResultSum(ArrayList sums)
	{
		decimal num = default(decimal);
		bool flag = true;
		foreach (object sum in sums)
		{
			if (sum != null && !DBNull.Value.Equals(sum))
			{
				flag = false;
				if (DecimalHelper.CanConvertToDecimal(num))
				{
					num += DecimalHelper.ConvertToDecimal(sum);
				}
			}
		}
		if (flag)
		{
			return CrossTab.EmptyValue;
		}
		return num;
	}

	private object GetsSummaryResultMax(ArrayList sums, StiCrossSummary crossSummary)
	{
		decimal num = default(decimal);
		bool flag = true;
		if (sums != null && sums.Count > 0 && IsDateTime(sums))
		{
			bool flag2 = false;
			DateTime? dateTime = null;
			int indexOfSelectValue = -1;
			int num2 = 0;
			foreach (object sum in sums)
			{
				if (sum == null || DBNull.Value.Equals(sum))
				{
					continue;
				}
				flag = false;
				if (sum is DateTime value)
				{
					if (flag2)
					{
						if (dateTime < value)
						{
							dateTime = value;
							indexOfSelectValue = num2;
						}
					}
					else
					{
						dateTime = value;
						flag2 = true;
						indexOfSelectValue = 0;
					}
				}
				num2++;
			}
			crossSummary.IndexOfSelectValue = indexOfSelectValue;
			if (flag)
			{
				return CrossTab.EmptyValue;
			}
			if (flag2)
			{
				return dateTime;
			}
			return null;
		}
		int indexOfSelectValue2 = -1;
		int num3 = 0;
		foreach (object sum2 in sums)
		{
			if (sum2 == null || DBNull.Value.Equals(sum2))
			{
				continue;
			}
			flag = false;
			if (num3 == 0)
			{
				if (DecimalHelper.CanConvertToDecimal(sum2))
				{
					num = DecimalHelper.ConvertToDecimal(sum2);
					indexOfSelectValue2 = num3;
				}
			}
			else if (DecimalHelper.CanConvertToDecimal(sum2))
			{
				decimal num4 = DecimalHelper.ConvertToDecimal(sum2);
				if (num < num4)
				{
					num = num4;
					indexOfSelectValue2 = num3;
				}
			}
			num3++;
		}
		crossSummary.IndexOfSelectValue = indexOfSelectValue2;
		if (flag)
		{
			return CrossTab.EmptyValue;
		}
		return num;
	}

	private object GetsSummaryResultMin(ArrayList sums, StiCrossSummary crossSummary)
	{
		decimal num = default(decimal);
		bool flag = true;
		if (sums != null && sums.Count > 0 && IsDateTime(sums))
		{
			bool flag2 = false;
			DateTime? dateTime = null;
			int indexOfSelectValue = -1;
			int num2 = 0;
			foreach (object sum in sums)
			{
				if (sum == null || DBNull.Value.Equals(sum))
				{
					continue;
				}
				flag = false;
				if (sum is DateTime value)
				{
					if (flag2)
					{
						if (dateTime > value)
						{
							dateTime = value;
							indexOfSelectValue = num2;
						}
					}
					else
					{
						dateTime = value;
						flag2 = true;
						indexOfSelectValue = 0;
					}
				}
				num2++;
			}
			crossSummary.IndexOfSelectValue = indexOfSelectValue;
			if (flag)
			{
				return CrossTab.EmptyValue;
			}
			if (flag2)
			{
				return dateTime;
			}
			return null;
		}
		int num3 = 0;
		int indexOfSelectValue2 = -1;
		foreach (object sum2 in sums)
		{
			if (sum2 == null || DBNull.Value.Equals(sum2))
			{
				continue;
			}
			flag = false;
			if (num3 == 0)
			{
				if (DecimalHelper.CanConvertToDecimal(sum2))
				{
					num = DecimalHelper.ConvertToDecimal(sum2);
					indexOfSelectValue2 = 0;
				}
			}
			else if (DecimalHelper.CanConvertToDecimal(sum2))
			{
				decimal num4 = DecimalHelper.ConvertToDecimal(sum2);
				if (num > num4)
				{
					num = num4;
					indexOfSelectValue2 = num3;
				}
			}
			num3++;
		}
		crossSummary.IndexOfSelectValue = indexOfSelectValue2;
		if (flag)
		{
			return CrossTab.EmptyValue;
		}
		return num;
	}

	private object GetsSummaryResultCount(ArrayList sums, StiSummaryValues summaryValues)
	{
		bool flag = true;
		if (sums == null || sums.Count == 0)
		{
			return 0;
		}
		decimal num = default(decimal);
		foreach (object sum in sums)
		{
			flag = false;
			decimal num2 = default(decimal);
			if (DecimalHelper.CanConvertToDecimal(sum))
			{
				num2 = DecimalHelper.ConvertToDecimal(sum);
			}
			if (summaryValues == StiSummaryValues.AllValues)
			{
				++num;
			}
			if (summaryValues == StiSummaryValues.SkipNulls && sum != null)
			{
				++num;
			}
			if (summaryValues == StiSummaryValues.SkipZerosAndNulls && sum != null && num2 != 0m)
			{
				++num;
			}
		}
		if (flag)
		{
			return CrossTab.EmptyValue;
		}
		return num;
	}

	private object GetSummaryResultAverage(ArrayList sums, StiSummaryValues summaryValues)
	{
		decimal num = default(decimal);
		bool flag = true;
		if (sums != null && sums.Count > 0 && IsDateTime(sums))
		{
			decimal num2 = default(decimal);
			long num3 = 0L;
			foreach (object sum in sums)
			{
				if (sum != null && !DBNull.Value.Equals(sum))
				{
					flag = false;
					if (sum is DateTime dateTime)
					{
						num2 += (decimal)dateTime.Ticks;
						num3++;
					}
				}
			}
			if (num3 == 0L)
			{
				return null;
			}
			if (flag)
			{
				return CrossTab.EmptyValue;
			}
			return new DateTime((long)(num2 / (decimal)num3));
		}
		if (sums == null || sums.Count == 0)
		{
			return 0;
		}
		int num4 = 0;
		foreach (object sum2 in sums)
		{
			if (sum2 != null && !DBNull.Value.Equals(sum2))
			{
				flag = false;
				decimal num5 = default(decimal);
				if (DecimalHelper.CanConvertToDecimal(sum2))
				{
					num5 = DecimalHelper.ConvertToDecimal(sum2);
					num += num5;
				}
				if (summaryValues == StiSummaryValues.AllValues)
				{
					num4++;
				}
				if (summaryValues == StiSummaryValues.SkipNulls)
				{
					num4++;
				}
				if (summaryValues == StiSummaryValues.SkipZerosAndNulls && num5 != 0m)
				{
					num4++;
				}
			}
		}
		if (num4 == 0)
		{
			return 0;
		}
		if (flag)
		{
			return CrossTab.EmptyValue;
		}
		return Math.Round(num / (decimal)num4, 4);
	}

	private object GetSummaryResultCountDistinct(ArrayList sums, StiSummaryValues summaryValues)
	{
		bool flag = true;
		if (sums == null || sums.Count == 0)
		{
			return 0;
		}
		Hashtable hashtable = new Hashtable();
		decimal num = default(decimal);
		bool flag2 = false;
		foreach (object sum in sums)
		{
			flag = false;
			decimal num2 = default(decimal);
			bool flag3 = false;
			if (DecimalHelper.CanConvertToDecimal(sum))
			{
				num2 = DecimalHelper.ConvertToDecimal(sum);
				flag3 = true;
			}
			if (sum == null)
			{
				if (summaryValues == StiSummaryValues.AllValues && !flag2)
				{
					flag2 = true;
					++num;
				}
			}
			else if (hashtable[sum] == null && (!(num2 == 0m && flag3) || summaryValues != StiSummaryValues.SkipZerosAndNulls))
			{
				hashtable[sum] = sum;
				++num;
			}
		}
		if (flag)
		{
			return CrossTab.EmptyValue;
		}
		return num;
	}

	private StiCrossField GetColumnTotalCell(int level, int calcIndex)
	{
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		StiCrossField stiCrossField = columnsCell[level * oneCellSize + calcIndex] as StiCrossField;
		if (stiCrossField == null)
		{
			StiCrossHeader stiCrossHeader = ColFields[level] as StiCrossHeader;
			stiCrossField = stiCrossHeader.Total.Clone(cloneProperties: true) as StiCrossField;
			columnsCell[level * oneCellSize + calcIndex] = stiCrossField;
			if (!string.IsNullOrEmpty(CrossTab?.CrossTabStyle) && base.Report?.Styles[CrossTab.CrossTabStyle] is StiCrossTabStyle && stiCrossField.ComponentStyle != null)
			{
				StiCrossTabStyle stiCrossTabStyle = base.Report.Styles[CrossTab.CrossTabStyle] as StiCrossTabStyle;
				stiCrossField.TextBrush = (StiBrush)new StiSolidBrush(stiCrossTabStyle.TotalCellColumnForeColor);
				stiCrossField.Brush = (StiBrush)new StiSolidBrush(stiCrossTabStyle.TotalCellColumnBackColor);
			}
			stiCrossField.HorAlignment = ((StiCrossField)SumFields[calcIndex]).HorAlignment;
			stiCrossField.VertAlignment = ((StiCrossField)SumFields[calcIndex]).VertAlignment;
			stiCrossField.TextFormat = ((StiService)((StiCrossField)SumFields[calcIndex]).TextFormat).Clone() as StiFormatService;
		}
		return stiCrossField;
	}

	private StiCrossField GetRowTotalCell(int level, int calcIndex)
	{
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected O, but got Unknown
		StiCrossField stiCrossField = rowsCell[level * oneCellSize + calcIndex] as StiCrossField;
		if (stiCrossField == null)
		{
			stiCrossField = ((StiCrossHeader)RowFields[level]).Total.Clone(cloneProperties: true) as StiCrossField;
			rowsCell[level * oneCellSize + calcIndex] = stiCrossField;
			if (!string.IsNullOrEmpty(CrossTab?.CrossTabStyle) && base.Report?.Styles[CrossTab.CrossTabStyle] is StiCrossTabStyle && stiCrossField.ComponentStyle != null)
			{
				StiCrossTabStyle stiCrossTabStyle = base.Report.Styles[CrossTab.CrossTabStyle] as StiCrossTabStyle;
				stiCrossField.TextBrush = (StiBrush)new StiSolidBrush(stiCrossTabStyle.TotalCellRowForeColor);
				stiCrossField.Brush = (StiBrush)new StiSolidBrush(stiCrossTabStyle.TotalCellRowBackColor);
			}
			stiCrossField.HorAlignment = ((StiCrossField)SumFields[calcIndex]).HorAlignment;
			stiCrossField.VertAlignment = ((StiCrossField)SumFields[calcIndex]).VertAlignment;
			stiCrossField.TextFormat = ((StiService)((StiCrossField)SumFields[calcIndex]).TextFormat).Clone() as StiFormatService;
		}
		return stiCrossField;
	}

	private List<StiRow> GetRowsArray()
	{
		List<StiRow> list = new List<StiRow>();
		GetRowsArray(Rows, list);
		return list;
	}

	private void GetRowsArray(StiRowCollection rows, List<StiRow> array)
	{
		foreach (StiRow row in rows)
		{
			if (row.Rows.Count == 0)
			{
				array.Add(row);
			}
			GetRowsArray(row.Rows, array);
		}
	}

	private List<StiColumn> GetColsArray()
	{
		List<StiColumn> list = new List<StiColumn>();
		GetColsArray(Cols, list);
		return list;
	}

	private void GetColsArray(StiColumnCollection cols, List<StiColumn> array)
	{
		foreach (StiColumn col in cols)
		{
			if (col.Cols.Count == 0)
			{
				array.Add(col);
			}
			GetColsArray(col.Cols, array);
		}
	}

	private int GetRowsHeaderWidth()
	{
		if (IsRowsEmpty)
		{
			return 0;
		}
		return GetRowsHeaderWidth(Rows);
	}

	private int GetRowsHeaderWidth(StiRowCollection rows)
	{
		int num = 0;
		while (rows.Count > 0)
		{
			num++;
			rows = rows[0].Rows;
		}
		return num;
	}

	private int GetColsHeaderHeight()
	{
		if (IsColsEmpty)
		{
			return 0;
		}
		return GetColsHeaderHeight(Cols);
	}

	private int GetColsHeaderHeight(StiColumnCollection cols)
	{
		int num = 0;
		while (cols.Count > 0)
		{
			num++;
			cols = cols[0].Cols;
		}
		return num;
	}

	private int GetRowsHeight()
	{
		return GetRowsHeight(Rows, GetRowsHeaderWidth(), 0);
	}

	private int GetRowsHeight(StiRowCollection rows, int maxLevel, int curLevel)
	{
		int num = 0;
		foreach (StiRow row in rows)
		{
			num += GetRowsHeight(row.Rows, maxLevel, curLevel + 1);
			if (curLevel == maxLevel - 1)
			{
				num += oneCellHeight;
			}
		}
		return num;
	}

	private int GetColsWidth()
	{
		return GetColsWidth(Cols, GetColsHeaderHeight(), 0);
	}

	private int GetColsWidth(StiColumnCollection cols, int maxLevel, int curLevel)
	{
		int num = 0;
		foreach (StiColumn col in cols)
		{
			num += GetColsWidth(col.Cols, maxLevel, curLevel + 1);
			if (curLevel == maxLevel - 1)
			{
				num += oneCellWidth;
			}
		}
		return num;
	}

	private void EnumerateRows(StiRowCollection rows, int level)
	{
		StiCrossRow stiCrossRow = RowFields[level] as StiCrossRow;
		string enumeratorSeparator = stiCrossRow.EnumeratorSeparator;
		int num = 0;
		foreach (StiRow row in rows)
		{
			if (row.Rows != null && row.Rows.Count > 0)
			{
				EnumerateRows(row.Rows, level + 1);
			}
			if (stiCrossRow.EnumeratorType != 0)
			{
				num++;
				string text = null;
				text = ((stiCrossRow.EnumeratorType == StiEnumeratorType.Arabic) ? num.ToString() : ((stiCrossRow.EnumeratorType != StiEnumeratorType.ABC) ? Func.Convert.ToRoman(num) : Func.Convert.ToABC(num)));
				row.DisplayValue = $"{text}{enumeratorSeparator}{row.DisplayValue}";
			}
		}
	}

	private void EnumerateColumns(StiColumnCollection columns, int level)
	{
		StiCrossColumn stiCrossColumn = ColFields[level] as StiCrossColumn;
		string enumeratorSeparator = stiCrossColumn.EnumeratorSeparator;
		int num = 0;
		foreach (StiColumn column in columns)
		{
			if (column.Cols != null && column.Cols.Count > 0)
			{
				EnumerateColumns(column.Cols, level + 1);
			}
			if (stiCrossColumn.EnumeratorType != 0)
			{
				num++;
				string text = null;
				text = ((stiCrossColumn.EnumeratorType == StiEnumeratorType.Arabic) ? num.ToString() : ((stiCrossColumn.EnumeratorType != StiEnumeratorType.ABC) ? Func.Convert.ToRoman(num) : Func.Convert.ToABC(num)));
				column.DisplayValue = $"{text}{enumeratorSeparator}{column.DisplayValue}";
			}
		}
	}

	private void CheckSeparators()
	{
		int num = 0;
		Hashtable hashtable = new Hashtable();
		Hashtable hashtable2 = new Hashtable();
		bool flag = false;
		for (int i = 0; i < base.ColCount; i++)
		{
			int num2 = 1;
			for (int j = 0; j < base.RowCount; j++)
			{
				StiCell stiCell = base.Cells[i][j];
				if (stiCell.ParentCell != stiCell || !stiCell.Text.Contains("#|#") || stiCell.Width != 1)
				{
					continue;
				}
				int num3 = 1;
				for (int k = 0; k < stiCell.Text.Length; k++)
				{
					if (k < stiCell.Text.Length - 3 && stiCell.Text[k] == '#' && stiCell.Text[k + 1] == '|' && stiCell.Text[k + 2] == '#')
					{
						hashtable[stiCell] = stiCell;
						flag = true;
						num3++;
					}
				}
				num2 = Math.Max(num2, num3);
			}
			hashtable2[i] = num2;
			num += num2;
		}
		if (!(num > base.ColCount || flag))
		{
			return;
		}
		int colCount = base.ColCount;
		StiCell[][] cells = base.Cells;
		Init(num, base.RowCount);
		int num4 = 0;
		for (int l = 0; l < colCount; l++)
		{
			int num5 = (int)hashtable2[l];
			int val = 1;
			for (int m = 0; m < base.RowCount; m++)
			{
				StiCell stiCell2 = cells[l][m];
				if (hashtable[stiCell2] != null && stiCell2.ParentCell == stiCell2)
				{
					string[] array = stiCell2.Text.Split(new string[1] { "#|#" }, StringSplitOptions.None);
					val = Math.Max(val, array.Length + 1);
					stiCell2.Text = array[0];
					base.Cells[num4][m] = stiCell2;
					for (int n = 1; n < array.Length; n++)
					{
						StiCell stiCell3 = stiCell2.Clone() as StiCell;
						base.Cells[num4 + n][m] = stiCell3;
						stiCell3.Text = array[n];
					}
				}
				else
				{
					stiCell2.ParentCell.Width += num5 - 1;
					base.Cells[num4][m] = stiCell2;
				}
			}
			num4 += num5;
		}
	}

	public void Create(DataTable table, StiReport report, StiSummaryDirection direction, string emptyValue)
	{
		summaryDirection = direction;
		base.Report = report;
		oneCellSize = SumFields.Count;
		oneCellWidth = ((summaryDirection != 0) ? 1 : oneCellSize);
		oneCellHeight = ((summaryDirection != StiSummaryDirection.UpToDown) ? 1 : oneCellSize);
		SummaryContainer = new StiSummaryContainer(oneCellSize);
		Rows.Clear();
		Cols.Clear();
		CalculateDataTable(table);
		CalculateTopN();
		SortRows();
		SortCols();
		if (Rows.Count > 0)
		{
			EnumerateRows(Rows, 0);
		}
		if (Cols.Count > 0)
		{
			EnumerateColumns(Cols, 0);
		}
		CreateRowTotals();
		CreateColTotals();
		colsHeaderHeight = GetColsHeaderHeight();
		rowsHeaderWidth = GetRowsHeaderWidth();
		if (IsSummarySubHeadersPresent)
		{
			if (direction == StiSummaryDirection.LeftToRight)
			{
				colsHeaderHeight++;
			}
			else
			{
				rowsHeaderWidth++;
			}
		}
		colsWidth = GetColsWidth();
		rowsHeight = GetRowsHeight();
		int num = colsWidth + rowsHeaderWidth;
		int num2 = rowsHeight + colsHeaderHeight;
		if (IsRowsEmpty)
		{
			num2 += oneCellHeight;
		}
		if (IsColsEmpty)
		{
			num += oneCellWidth;
		}
		widthCorrection = 0;
		heightCorrection = 0;
		if (IsColsEmpty && IsSummaryPresent)
		{
			heightCorrection = 1;
		}
		if (IsRowsEmpty && IsSummaryPresent)
		{
			widthCorrection = 1;
		}
		if (IsRowsEmpty && !IsColsEmpty && IsSummariesEmpty)
		{
			widthCorrection = 1;
		}
		if (IsTopLinePresent)
		{
			heightCorrection++;
		}
		Init(num + widthCorrection, num2 + heightCorrection);
		if (IsRowsEmpty && IsColsEmpty)
		{
			return;
		}
		int num3 = 0;
		foreach (StiCrossColumn colField in ColFields)
		{
			StiCrossTitle stiCrossTitle = null;
			foreach (StiCrossTitle colTitleField in ColTitleFields)
			{
				if (colTitleField.TypeOfComponent == "Col:" + colField.Name)
				{
					stiCrossTitle = colTitleField;
					break;
				}
			}
			if (stiCrossTitle != null && stiCrossTitle.IsEnabled)
			{
				int cellHeight = 1;
				int cellWidth = Math.Max(rowsHeaderWidth, 1);
				InvokeEvents(stiCrossTitle);
				SetCell(0, num3++, cellWidth, cellHeight, stiCrossTitle.TextValue ?? stiCrossTitle.GetTextInternal(), emptyValue, stiCrossTitle, isNumeric: false, stiCrossTitle.HyperlinkValue, stiCrossTitle.ToolTipValue, stiCrossTitle.TagValue, stiCrossTitle.DrillDownParameters, StiCellType.CornerCol);
			}
		}
		bool isRowTitlePresent = IsRowTitlePresent;
		num3 = 0;
		foreach (StiCrossRow rowField in RowFields)
		{
			StiCrossTitle stiCrossTitle3 = null;
			foreach (StiCrossTitle rowTitleField in RowTitleFields)
			{
				if (rowTitleField.TypeOfComponent == "Row:" + rowField.Name)
				{
					stiCrossTitle3 = rowTitleField;
					break;
				}
			}
			if (stiCrossTitle3 != null && isRowTitlePresent)
			{
				int cellWidth2 = 1;
				int num4 = Math.Max(colsHeaderHeight, 1);
				int cellY = (IsTopLinePresent ? 1 : 0);
				InvokeEvents(stiCrossTitle3);
				if (direction == StiSummaryDirection.LeftToRight && IsColsEmpty && SumFields.Count > 1)
				{
					num4++;
				}
				SetCell(num3++, cellY, cellWidth2, num4, stiCrossTitle3.TextValue ?? stiCrossTitle3.GetTextInternal(), emptyValue, stiCrossTitle3, isNumeric: false, stiCrossTitle3.HyperlinkValue, stiCrossTitle3.ToolTipValue, stiCrossTitle3.TagValue, stiCrossTitle3.DrillDownParameters, StiCellType.CornerRow);
			}
		}
		if (IsSummarySubHeadersPresent)
		{
			if (direction == StiSummaryDirection.LeftToRight)
			{
				int cellY2 = colsHeaderHeight + heightCorrection - 1;
				num3 = rowsHeaderWidth + widthCorrection;
				int num5 = 0;
				int num6 = ((colsWidth == 0) ? SumHeaderFields.Count : colsWidth);
				for (int i = 0; i < num6; i++)
				{
					StiCrossSummaryHeader stiCrossSummaryHeader = SumHeaderFields[num5] as StiCrossSummaryHeader;
					if (!base.DesignTime)
					{
						if (stiCrossSummaryHeader.Text != null && stiCrossSummaryHeader.Text.Value != null)
						{
							if (stiCrossSummaryHeader.OriginalValue == null)
							{
								stiCrossSummaryHeader.OriginalValue = stiCrossSummaryHeader.Text.Value;
							}
							else
							{
								stiCrossSummaryHeader.Text.Value = (string)stiCrossSummaryHeader.OriginalValue;
							}
						}
						StiGetValueEventArgs stiGetValueEventArgs = new StiGetValueEventArgs();
						stiCrossSummaryHeader.InvokeGetValue(stiCrossSummaryHeader, stiGetValueEventArgs);
						stiCrossSummaryHeader.SetTextInternal(stiGetValueEventArgs.Value);
					}
					InvokeEvents(stiCrossSummaryHeader);
					SetCell(num3, cellY2, 1, 1, stiCrossSummaryHeader.TextValue ?? stiCrossSummaryHeader.GetTextInternal(), emptyValue, stiCrossSummaryHeader, isNumeric: false, stiCrossSummaryHeader.HyperlinkValue, stiCrossSummaryHeader.ToolTipValue, stiCrossSummaryHeader.TagValue, stiCrossSummaryHeader.DrillDownParameters, StiCellType.HeaderColSummaryTotal);
					num3++;
					num5++;
					if (num5 == SumHeaderFields.Count)
					{
						num5 = 0;
					}
				}
			}
			else
			{
				int cellX = rowsHeaderWidth + widthCorrection - 1;
				num3 = colsHeaderHeight + heightCorrection;
				int num7 = 0;
				int num8 = ((rowsHeight == 0) ? SumHeaderFields.Count : rowsHeight);
				for (int j = 0; j < num8; j++)
				{
					StiCrossSummaryHeader stiCrossSummaryHeader2 = SumHeaderFields[num7] as StiCrossSummaryHeader;
					if (!base.DesignTime)
					{
						if (stiCrossSummaryHeader2.Text != null && stiCrossSummaryHeader2.Text.Value != null)
						{
							if (stiCrossSummaryHeader2.OriginalValue == null)
							{
								stiCrossSummaryHeader2.OriginalValue = stiCrossSummaryHeader2.Text.Value;
							}
							else
							{
								stiCrossSummaryHeader2.Text.Value = (string)stiCrossSummaryHeader2.OriginalValue;
							}
						}
						StiGetValueEventArgs stiGetValueEventArgs2 = new StiGetValueEventArgs();
						stiCrossSummaryHeader2.InvokeGetValue(stiCrossSummaryHeader2, stiGetValueEventArgs2);
						stiCrossSummaryHeader2.SetTextInternal(stiGetValueEventArgs2.Value);
					}
					InvokeEvents(stiCrossSummaryHeader2);
					SetCell(cellX, num3, 1, 1, stiCrossSummaryHeader2.TextValue ?? stiCrossSummaryHeader2.GetTextInternal(), emptyValue, stiCrossSummaryHeader2, isNumeric: false, stiCrossSummaryHeader2.HyperlinkValue, stiCrossSummaryHeader2.ToolTipValue, stiCrossSummaryHeader2.TagValue, stiCrossSummaryHeader2.DrillDownParameters, StiCellType.HeaderRowSummaryTotal);
					num3++;
					num7++;
					if (num7 == SumHeaderFields.Count)
					{
						num7 = 0;
					}
				}
			}
		}
		if (IsTopLinePresent)
		{
			if (IsLeftTopLinePresent)
			{
				InvokeEvents(LeftCrossTitle);
				SetCell(0, 0, rowsHeaderWidth, 1, LeftCrossTitle.TextValue ?? LeftCrossTitle.GetTextInternal(), emptyValue, LeftCrossTitle, isNumeric: false, LeftCrossTitle.HyperlinkValue, LeftCrossTitle.ToolTipValue, LeftCrossTitle.TagValue, LeftCrossTitle.DrillDownParameters, StiCellType.LeftTopLine);
			}
			if (IsRightTopLinePresent && num - rowsHeaderWidth > 0)
			{
				InvokeEvents(RightCrossTitle);
				SetCell(rowsHeaderWidth, 0, num - rowsHeaderWidth, 1, RightCrossTitle.TextValue ?? RightCrossTitle.GetTextInternal(), emptyValue, RightCrossTitle, isNumeric: false, RightCrossTitle.HyperlinkValue, RightCrossTitle.ToolTipValue, RightCrossTitle.TagValue, RightCrossTitle.DrillDownParameters, StiCellType.RightTopLine);
			}
		}
		if (SummaryCrossTitle != null)
		{
			if (IsColsEmpty)
			{
				InvokeEvents(SummaryCrossTitle);
				SetCell(rowsHeaderWidth, 0, oneCellWidth, 1, SummaryCrossTitle.TextValue ?? SummaryCrossTitle.GetTextInternal(), emptyValue, SummaryCrossTitle, isNumeric: false, SummaryCrossTitle.HyperlinkValue, SummaryCrossTitle.ToolTipValue, SummaryCrossTitle.TagValue, SummaryCrossTitle.DrillDownParameters, StiCellType.HeaderCol);
			}
			if (IsRowsEmpty)
			{
				InvokeEvents(SummaryCrossTitle);
				int cellY3 = ((direction == StiSummaryDirection.LeftToRight) ? (colsHeaderHeight - 1) : colsHeaderHeight);
				int cellHeight2 = ((direction == StiSummaryDirection.LeftToRight) ? (oneCellHeight + 1) : oneCellHeight);
				if (SummaryCrossTitle.Enabled)
				{
					SetCell(0, cellY3, 1, cellHeight2, SummaryCrossTitle.TextValue ?? SummaryCrossTitle.GetTextInternal(), emptyValue, SummaryCrossTitle, isNumeric: false, SummaryCrossTitle.HyperlinkValue, SummaryCrossTitle.ToolTipValue, SummaryCrossTitle.TagValue, SummaryCrossTitle.DrillDownParameters, StiCellType.HeaderRow);
				}
			}
		}
		string parentGuid = "";
		if (!IsRowsEmpty)
		{
			CopyRows(Rows, 0, colsHeaderHeight + heightCorrection, 0, out parentGuid);
		}
		if (!IsColsEmpty)
		{
			CopyCols(Cols, rowsHeaderWidth + widthCorrection, heightCorrection, 0, out parentGuid);
		}
		CopySummaries(rowsHeaderWidth + widthCorrection, colsHeaderHeight + heightCorrection, emptyValue);
		if (!base.DesignTime)
		{
			CheckSeparators();
		}
		base.Report.Cells.Clear();
		for (int k = 0; k < base.ColCount; k++)
		{
			for (int l = 0; l < base.RowCount; l++)
			{
				StiCell stiCell = base.Cells[k][l];
				if (stiCell.IsNumeric && stiCell.Value is decimal)
				{
					base.Report.Cells.SetCell(k, l, (decimal)stiCell.Value);
				}
				else
				{
					base.Report.Cells.SetCell(k, l, 0m);
				}
			}
		}
		for (int m = 0; m < base.ColCount; m++)
		{
			for (int n = 0; n < base.RowCount; n++)
			{
				StiCell stiCell2 = base.Cells[m][n];
				if (stiCell2.ParentCell != stiCell2)
				{
					continue;
				}
				if (stiCell2.Field is StiCrossSummary && ((StiCrossSummary)stiCell2.Field).Summary == StiSummaryType.Image)
				{
					stiCell2.IsImage = true;
				}
				StiProcessCellEventArgs stiProcessCellEventArgs = new StiProcessCellEventArgs
				{
					Row = n,
					Column = m,
					Cell = stiCell2
				};
				decimal num9 = default(decimal);
				if (stiCell2.Value is decimal)
				{
					num9 = (decimal)stiCell2.Value;
				}
				stiProcessCellEventArgs.Text = stiCell2.Text;
				stiProcessCellEventArgs.Value = num9;
				report.Cells.DistX = m;
				report.Cells.DistY = n;
				if (stiCell2.Field is StiCrossTotal)
				{
					stiCell2.Field = stiCell2.Field;
				}
				if (stiCell2.Field != null)
				{
					stiCell2.Field.InvokeProcessCell(stiProcessCellEventArgs);
				}
				if (stiProcessCellEventArgs.Value != num9)
				{
					base.Report.Cells.SetCell(m, n, stiProcessCellEventArgs.Value);
					stiCell2.Value = stiProcessCellEventArgs.Value;
					stiCell2.Text = stiCell2.Field.TextFormat.Format(stiProcessCellEventArgs.Value);
				}
				else
				{
					stiCell2.Text = stiProcessCellEventArgs.Text;
				}
				if (stiCell2.IsNumeric && stiCell2.Field != null && (!stiCell2.Field.HideZeros || !IsHideZeros(stiCell2.Value)))
				{
					if (stiCell2.Value != null)
					{
						stiCell2.Field.ExcelDataValue = stiCell2.Value.ToString();
					}
					StiNumberFormatService obj = stiCell2.Field.TextFormat as StiNumberFormatService;
					if (obj != null && (obj.State & StiTextFormatState.Abbreviation) > StiTextFormatState.None)
					{
						stiCell2.Field.ExcelDataValue = null;
					}
				}
			}
		}
		if (IsRowsEmpty && !IsColsEmpty && IsSummariesEmpty)
		{
			decimal[] array = base.Heights;
			Array.Resize(ref array, base.Heights.Length - 1);
			base.Heights = array;
			for (int num10 = 0; num10 < base.Cells.Length; num10++)
			{
				StiCell[] array2 = base.Cells[num10];
				Array.Resize(ref array2, array2.Length - 1);
				base.Cells[num10] = array2;
			}
		}
	}

	public bool Clear()
	{
		bool result = false;
		if (base.Cells != null && base.Cells.Length > 1 && base.Cells[0] != null)
		{
			int num = base.Cells.Length * base.Cells[0].Length;
			if (num > 50000)
			{
				result = true;
			}
		}
		base.Cells = null;
		return result;
	}

	internal int GetCorrectedColumnsHeaderHeight()
	{
		return colsHeaderHeight + heightCorrection;
	}

	private bool IsHideZeros(object value)
	{
		if (value != null && TypeExt.IsNumericType(value.GetType()))
		{
			return StiValueHelper.TryToDecimal(value) == 0m;
		}
		return false;
	}

	private bool IsDateTime(ArrayList list)
	{
		return list.Cast<object>().Any((object s) => s is DateTime);
	}

	private static string ConvertValueToString(object value, StiCrossField field)
	{
		if (value == null)
		{
			return string.Empty;
		}
		if (value is decimal && (decimal)value == 0m && field.HideZeros)
		{
			return string.Empty;
		}
		if (value is decimal)
		{
			if (field.TextFormat is StiCustomFormatService && !string.IsNullOrWhiteSpace(field.TextFormat.StringFormat) && field.TextFormat.StringFormat.Contains("{"))
			{
				string stringFormat = field.TextFormat.StringFormat;
				try
				{
					field.TextFormat.StringFormat = Convert.ToString(StiParser.ParseTextValue(stringFormat, field));
					return field.TextFormat.Format((decimal)value);
				}
				finally
				{
					field.TextFormat.StringFormat = stringFormat;
				}
			}
			return field.TextFormat.Format((decimal)value);
		}
		if (value is DateTime)
		{
			if (field.TextFormat is StiCustomFormatService && !string.IsNullOrWhiteSpace(field.TextFormat.StringFormat) && field.TextFormat.StringFormat.Contains("{"))
			{
				string stringFormat2 = field.TextFormat.StringFormat;
				try
				{
					field.TextFormat.StringFormat = Convert.ToString(StiParser.ParseTextValue(stringFormat2, field));
					return field.TextFormat.Format((DateTime)value);
				}
				finally
				{
					field.TextFormat.StringFormat = stringFormat2;
				}
			}
			return field.TextFormat.Format((DateTime)value);
		}
		return value.ToString();
	}
}
