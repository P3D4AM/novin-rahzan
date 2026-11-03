using System;
using System.Collections;
using System.Data;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.CrossTab.Core;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.CrossTab;

public static class StiCrossTabHelper
{
	public static void RenderCells(StiCrossTab masterCrossTab, StiContainer outContainer, int startCol, int startRow, int endCol, int endRow, RectangleM rect)
	{
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		StiCrossTabInfo crossTabInfo = masterCrossTab.CrossTabInfo;
		StiCross cross = crossTabInfo.Cross;
		crossTabInfo.HidedCells.Clear();
		decimal startX = ((RectangleM)(ref rect)).Left - cross.CoordX[startCol];
		decimal startY = ((RectangleM)(ref rect)).Top - cross.CoordY[startRow];
		for (int i = startRow; i < endRow; i++)
		{
			for (int j = startCol; j < endCol; j++)
			{
				StiCell stiCell = cross.Cells[j][i];
				if (crossTabInfo.HidedCells[stiCell] != null)
				{
					continue;
				}
				int num = j + stiCell.Width;
				int num2 = i + stiCell.Height;
				if (num > endCol)
				{
					num = endCol;
				}
				if (num2 > endRow)
				{
					num2 = endRow;
				}
				for (int k = j; k < num; k++)
				{
					for (int l = i; l < num2; l++)
					{
						StiCell stiCell2 = cross.Cells[k][l];
						crossTabInfo.HidedCells[stiCell2] = stiCell2;
					}
				}
				RectangleD cellSize = GetCellSize(j, num, cross, i, num2, startX, startY);
				StiComponent stiComponent = CreateComponentFromCell(stiCell);
				if (stiComponent != null)
				{
					outContainer.Components.Add(stiComponent);
					stiComponent.ClientRectangle = cellSize;
				}
				SetBottomLine(masterCrossTab, stiCell, stiComponent, i, endRow);
			}
		}
	}

	public static RectangleM GetCellRect(StiCrossTab masterCrossTab, int colIndex, int rowIndex)
	{
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		StiCross cross = masterCrossTab.CrossTabInfo.Cross;
		decimal num = default(decimal);
		decimal num2 = default(decimal);
		StiCell stiCell = cross.Cells[colIndex][rowIndex];
		for (int i = colIndex; i < colIndex + stiCell.Width; i++)
		{
			num += cross.Widths[i];
		}
		for (int j = rowIndex; j < rowIndex + stiCell.Height; j++)
		{
			num2 += cross.Heights[j];
		}
		return new RectangleM(cross.CoordX[colIndex], cross.CoordY[rowIndex], num, num2);
	}

	public static SizeM GetCellsRect(StiCrossTab masterCrossTab, int startCol, int startRow, int endCol, int endRow)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		decimal num = default(decimal);
		decimal num2 = default(decimal);
		for (int i = startCol; i < endCol; i++)
		{
			num += masterCrossTab.CrossTabInfo.Cross.Widths[i];
		}
		for (int j = startRow; j < endRow; j++)
		{
			num2 += masterCrossTab.CrossTabInfo.Cross.Heights[j];
		}
		return new SizeM(num, num2);
	}

	private static RectangleD GetCellSize(int colIndex, int cellRight, StiCross cross, int rowIndex, int cellBottom, decimal startX, decimal startY)
	{
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		decimal num = default(decimal);
		decimal num2 = default(decimal);
		for (int i = colIndex; i < cellRight; i++)
		{
			num += cross.Widths[i];
		}
		for (int j = rowIndex; j < cellBottom; j++)
		{
			num2 += cross.Heights[j];
		}
		return new RectangleD((double)(cross.CoordX[colIndex] + startX), (double)(cross.CoordY[rowIndex] + startY), (double)num, (double)num2);
	}

	private static StiComponent CreateComponentFromCell(StiCell cell)
	{
		StiComponent stiComponent = (cell.IsImage ? CreateImageFromCell(cell) : CreateTextFromCell(cell));
		if (stiComponent != null)
		{
			stiComponent.HyperlinkValue = cell.HyperlinkValue;
			stiComponent.ToolTipValue = cell.ToolTipValue;
			stiComponent.TagValue = cell.TagValue;
			stiComponent.DrillDownParameters = cell.DrillDownParameters;
			stiComponent.ComponentPlacement = cell.GetComponentPlacement();
		}
		return stiComponent;
	}

	private static StiComponent CreateTextFromCell(StiCell cell)
	{
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		StiText stiText = null;
		if (cell.Field != null)
		{
			stiText = ((cell.Width <= 1 && cell.Height <= 1 && cell.IsNumeric && !(cell.Field is StiCrossHeader)) ? cell.Field : (((StiService)cell.Field).Clone() as StiCrossField));
			if (stiText is StiCrossHeader stiCrossHeader)
			{
				stiCrossHeader.HeaderLevel = cell.Level;
				stiCrossHeader.HeaderValue = ((cell.Value != null) ? cell.Value.ToString() : null);
			}
			StiText stiText2 = stiText;
			object obj = cell.Field.Border.Clone();
			stiText2.Border = (StiBorder)((obj is StiBorder) ? obj : null);
			stiText.MaxSize = default(SizeD);
			stiText.MinSize = default(SizeD);
			stiText.Guid = StiGuidUtils.NewGuid();
			if (cell.Height > 1)
			{
				stiText.CanBreak = true;
			}
		}
		else if (cell.Width > 1 || cell.Height > 1)
		{
			stiText = new StiText();
			stiText.Border.Side = (StiBorderSides)15;
		}
		if (cell.IsNegativeColor)
		{
			stiText.TextBrush = (StiBrush)new StiSolidBrush(StiOptions.Engine.NegativeColor);
		}
		if (stiText != null)
		{
			stiText.SetTextInternal(cell.Text);
			return stiText;
		}
		return null;
	}

	private static StiComponent CreateImageFromCell(StiCell cell)
	{
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		StiImage stiImage;
		if (cell.Field != null)
		{
			stiImage = new StiImage();
			StiImage stiImage2 = stiImage;
			object obj = cell.Field.Border.Clone();
			stiImage2.Border = (StiBorder)((obj is StiBorder) ? obj : null);
		}
		else
		{
			stiImage = new StiImage();
			stiImage.Border.Side = (StiBorderSides)15;
		}
		if (cell.Value != null)
		{
			stiImage.PutImage(StiImageHelper.GetImageBytesFromObject(cell.Value));
		}
		if (cell.Field is StiCrossSummary stiCrossSummary)
		{
			stiImage.Stretch = stiCrossSummary.Stretch;
			stiImage.AspectRatio = stiCrossSummary.AspectRatio;
			stiImage.HorAlignment = stiCrossSummary.ImageHorAlignment;
			stiImage.VertAlignment = stiCrossSummary.ImageVertAlignment;
		}
		return stiImage;
	}

	private static void SetBottomLine(StiCrossTab crossTab, StiCell cell, StiComponent component, int rowIndex, int endRow)
	{
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		StiCross cross = crossTab.CrossTabInfo.Cross;
		int count = cross.SumFields.Count;
		if (count >= 2 && cell.SummaryIndex >= 0 && cell.SummaryIndex < count - 1 && rowIndex == endRow - 1 && cross.SumFields.ToList().LastOrDefault() is IStiBorder stiBorder && stiBorder.Border.IsBottomBorderSidePresent && component is IStiBorder stiBorder2 && !stiBorder2.Border.IsBottomBorderSidePresent)
		{
			StiBorder border = stiBorder2.Border;
			border.Side = (StiBorderSides)(border.Side + 8);
		}
	}

	public static DataTable CreateCrossForCrossTabDataSource(StiCrossTab masterCrossTab)
	{
		masterCrossTab.CrossTabInfoV1.StartRow = 0;
		masterCrossTab.CrossTabInfoV1.StartCol = 0;
		StiDataHelper.SetData(masterCrossTab, reinit: true, masterCrossTab.Parent as StiDataBand);
		return BuildCrossForCrossTabDataSource(masterCrossTab, designTime: false);
	}

	public static DataTable BuildCrossForCrossTabDataSource(StiCrossTab masterCrossTab, bool designTime)
	{
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection(null);
		StiComponentsCollection stiComponentsCollection2 = new StiComponentsCollection(null);
		StiComponentsCollection stiComponentsCollection3 = new StiComponentsCollection(null);
		StiComponentsCollection stiComponentsCollection4 = new StiComponentsCollection(null);
		StiComponentsCollection stiComponentsCollection5 = new StiComponentsCollection(null);
		StiComponentsCollection stiComponentsCollection6 = new StiComponentsCollection(null);
		DataTable dataTable = new DataTable();
		foreach (StiComponent component in masterCrossTab.Components)
		{
			if (!(component is StiCrossCell) && !(component is StiCrossTitle) && !(component is StiCrossSummaryHeader))
			{
				continue;
			}
			StiCrossRow stiCrossRow = component as StiCrossRow;
			StiCrossColumn stiCrossColumn = component as StiCrossColumn;
			StiCrossSummary stiCrossSummary = component as StiCrossSummary;
			StiCrossSummaryHeader stiCrossSummaryHeader = component as StiCrossSummaryHeader;
			if (component is StiCrossTitle stiCrossTitle)
			{
				if (StringExt.StartsWithInvariant(stiCrossTitle.TypeOfComponent, "Row:"))
				{
					stiComponentsCollection5.Add(stiCrossTitle);
				}
				if (StringExt.StartsWithInvariant(stiCrossTitle.TypeOfComponent, "Col:"))
				{
					stiComponentsCollection6.Add(stiCrossTitle);
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
			if (stiCrossSummary != null && !stiCrossSummary.DisabledByCondition && stiCrossSummary.Enabled)
			{
				stiComponentsCollection3.Add(stiCrossSummary);
			}
			if (stiCrossSummaryHeader != null && !stiCrossSummaryHeader.DisabledByCondition && stiCrossSummaryHeader.Enabled)
			{
				stiComponentsCollection4.Add(stiCrossSummaryHeader);
			}
			DataColumn column = new DataColumn(component.Name, typeof(object));
			dataTable.Columns.Add(column);
			if (component is StiCrossHeader)
			{
				column = new DataColumn("Display__" + component.Name, typeof(object));
				dataTable.Columns.Add(column);
			}
		}
		if (!designTime)
		{
			masterCrossTab.First();
			while (!masterCrossTab.IsEof)
			{
				DataRow dataRow = dataTable.NewRow();
				foreach (StiCrossField component2 in masterCrossTab.Components)
				{
					if (component2 is StiCrossSummaryHeader)
					{
						continue;
					}
					if (component2 is StiCrossCell stiCrossCell)
					{
						StiGetCrossValueEventArgs stiGetCrossValueEventArgs = new StiGetCrossValueEventArgs();
						stiCrossCell.InvokeGetCrossValue(stiGetCrossValueEventArgs);
						dataRow[stiCrossCell.Name] = stiGetCrossValueEventArgs.Value;
						if (stiCrossCell is StiCrossHeader)
						{
							stiGetCrossValueEventArgs = new StiGetCrossValueEventArgs();
							(stiCrossCell as StiCrossHeader).InvokeGetDisplayCrossValue(stiGetCrossValueEventArgs);
							dataRow["Display__" + component2.Name] = stiGetCrossValueEventArgs.Value;
						}
					}
					else if (component2 is StiCrossTitle)
					{
						StiGetValueEventArgs stiGetValueEventArgs = new StiGetValueEventArgs();
						((StiCrossTitle)component2).InvokeGetValue(component2, stiGetValueEventArgs);
						((StiCrossTitle)component2).SetTextInternal(stiGetValueEventArgs.Value);
					}
				}
				dataTable.Rows.Add(dataRow);
				masterCrossTab.Next();
			}
		}
		return dataTable;
	}

	public static void BuildCross(StiCrossTab masterCrossTab, bool designTime)
	{
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection(null);
		StiComponentsCollection stiComponentsCollection2 = new StiComponentsCollection(null);
		StiComponentsCollection stiComponentsCollection3 = new StiComponentsCollection(null);
		StiComponentsCollection stiComponentsCollection4 = new StiComponentsCollection(null);
		StiComponentsCollection stiComponentsCollection5 = new StiComponentsCollection(null);
		StiComponentsCollection stiComponentsCollection6 = new StiComponentsCollection(null);
		StiCrossTitle leftCrossTitle = null;
		StiCrossTitle rightCrossTitle = null;
		StiCrossTitle summaryCrossTitle = null;
		DataTable dataTable = new DataTable();
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
				if (StringExt.StartsWithInvariant(stiCrossTitle.TypeOfComponent, "Row:"))
				{
					stiComponentsCollection5.Add(stiCrossTitle);
				}
				if (StringExt.StartsWithInvariant(stiCrossTitle.TypeOfComponent, "Col:"))
				{
					stiComponentsCollection6.Add(stiCrossTitle);
				}
				if (StringExt.StartsWithInvariant(stiCrossTitle.TypeOfComponent, "LeftTitle"))
				{
					leftCrossTitle = stiCrossTitle;
				}
				if (StringExt.StartsWithInvariant(stiCrossTitle.TypeOfComponent, "RightTitle"))
				{
					rightCrossTitle = stiCrossTitle;
				}
				if (StringExt.StartsWithInvariant(stiCrossTitle.TypeOfComponent, "SummaryTitle"))
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
			if (stiCrossSummary != null && !stiCrossSummary.DisabledByCondition && stiCrossSummary.Enabled)
			{
				stiComponentsCollection3.Add(stiCrossSummary);
			}
			if (stiCrossSummaryHeader != null && !stiCrossSummaryHeader.DisabledByCondition && stiCrossSummaryHeader.Enabled)
			{
				stiComponentsCollection4.Add(stiCrossSummaryHeader);
			}
			DataColumn column = new DataColumn(component.Name, typeof(object));
			dataTable.Columns.Add(column);
			if (component is StiCrossHeader)
			{
				column = new DataColumn("Display__" + component.Name, typeof(object));
				dataTable.Columns.Add(column);
			}
		}
		if (!designTime)
		{
			masterCrossTab.First();
			while (!masterCrossTab.IsEof)
			{
				DataRow dataRow = dataTable.NewRow();
				foreach (StiCrossField component2 in masterCrossTab.Components)
				{
					if (component2 is StiCrossSummaryHeader)
					{
						continue;
					}
					if (component2 is StiCrossCell stiCrossCell)
					{
						StiGetCrossValueEventArgs stiGetCrossValueEventArgs = new StiGetCrossValueEventArgs();
						stiCrossCell.InvokeGetCrossValue(stiGetCrossValueEventArgs);
						object obj = stiGetCrossValueEventArgs.Value;
						if (!(obj is string))
						{
							obj = StiValueHelper.TryToNullableDecimal(stiGetCrossValueEventArgs.Value);
							if (obj == null)
							{
								obj = stiGetCrossValueEventArgs.Value;
							}
						}
						dataRow[stiCrossCell.Name] = obj;
						if (stiCrossCell is StiCrossHeader)
						{
							stiGetCrossValueEventArgs = new StiGetCrossValueEventArgs();
							(stiCrossCell as StiCrossHeader).InvokeGetDisplayCrossValue(stiGetCrossValueEventArgs);
							dataRow["Display__" + component2.Name] = stiGetCrossValueEventArgs.Value;
						}
					}
					else if (component2 is StiCrossTitle)
					{
						StiGetValueEventArgs stiGetValueEventArgs = new StiGetValueEventArgs();
						((StiCrossTitle)component2).InvokeGetValue(component2, stiGetValueEventArgs);
						((StiCrossTitle)component2).TextValue = stiGetValueEventArgs.Value;
					}
				}
				dataTable.Rows.Add(dataRow);
				masterCrossTab.Next();
			}
		}
		StiCross stiCross2 = (masterCrossTab.CrossTabInfo.Cross = new StiCross());
		StiCross stiCross3 = stiCross2;
		stiCross3.DesignTime = designTime;
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
		stiCross3.Create(dataTable, masterCrossTab.Report, masterCrossTab.SummaryDirection, masterCrossTab.EmptyValue);
		stiCross3.CrossTab = null;
	}

	public static int GetEndCol(StiCrossTab masterCrossTab, int startCol, RectangleM rect, bool forceNoBreak = false)
	{
		StiCross cross = masterCrossTab.CrossTabInfo.Cross;
		decimal num = default(decimal);
		int i;
		for (i = startCol; i < cross.ColCount; i++)
		{
			num += cross.Widths[i];
			if (num > ((RectangleM)(ref rect)).Width && masterCrossTab.HorAlignment != StiCrossHorAlignment.Width)
			{
				break;
			}
		}
		if ((i == startCol || forceNoBreak) && i + 1 <= cross.ColCount)
		{
			i++;
		}
		return i;
	}

	public static int GetEndRow(StiCrossTab masterCrossTab, int startRow, RectangleM rect, bool forceNoBreak = false)
	{
		StiCross cross = masterCrossTab.CrossTabInfo.Cross;
		decimal num = default(decimal);
		int i;
		for (i = startRow; i < cross.RowCount; i++)
		{
			num += cross.Heights[i];
			if (num > ((RectangleM)(ref rect)).Height)
			{
				break;
			}
		}
		if ((i == startRow || forceNoBreak) && i + 1 <= cross.RowCount)
		{
			i++;
		}
		return i;
	}

	public static int CheckMergedRowCells(StiCrossTab masterCrossTab, int startRow, int endRow, int startCol, int endCol)
	{
		StiCell[][] cells = masterCrossTab.CrossTabInfo.Cross.Cells;
		int num = 0;
		for (int i = startCol; i < endCol; i++)
		{
			StiCell stiCell = cells[i][endRow - 1];
			StiCell parentCell = stiCell.ParentCell;
			if (stiCell.Field != null && stiCell.Height > 1 && parentCell != null && parentCell.KeepMergedCellsTogether)
			{
				num = Math.Max(num, parentCell.Height - stiCell.Height + 1);
			}
		}
		return Math.Max(endRow - num, startRow + 1);
	}

	public static bool IsColFieldsEmpty(StiCrossTab masterCrossTab)
	{
		if (masterCrossTab.CrossTabInfo.Cross.ColFields.Count == 1)
		{
			return masterCrossTab.CrossTabInfo.Cross.ColFields[0].Name == "EMPTY_FIELD";
		}
		return false;
	}

	public static bool IsRowFieldsEmpty(StiCrossTab masterCrossTab)
	{
		StiCross cross = masterCrossTab.CrossTabInfo.Cross;
		if (cross.IsRowsEmpty && !cross.IsColsEmpty && cross.IsSummariesEmpty)
		{
			return false;
		}
		if (masterCrossTab.CrossTabInfo.Cross.RowFields.Count == 1)
		{
			return masterCrossTab.CrossTabInfo.Cross.RowFields[0].Name == "EMPTY_FIELD";
		}
		return false;
	}

	public static void CreateCross(StiCrossTab masterCrossTab)
	{
		masterCrossTab.CrossTabInfoV1.StartRow = 0;
		masterCrossTab.CrossTabInfoV1.StartCol = 0;
		StiDataHelper.SetData(masterCrossTab, reinit: true, masterCrossTab.Parent as StiDataBand);
		BuildCross(masterCrossTab, designTime: false);
		masterCrossTab.CrossTabInfo.Cross.MaxWidth = (decimal)masterCrossTab.Page.Width;
		masterCrossTab.CrossTabInfo.Cross.MaxHeight = (decimal)masterCrossTab.Page.Height;
		if (masterCrossTab.RightToLeft && (!masterCrossTab.Page.UnlimitedBreakable || StiSubReportsHelper.GetParentBand(masterCrossTab) == null))
		{
			MakeRightToLeft(masterCrossTab);
		}
		masterCrossTab.CrossTabInfo.Cross.DoAutoSize();
	}

	public static void MakeRightToLeft(StiCrossTab masterCrossTab)
	{
		StiCross cross = masterCrossTab.CrossTabInfo.Cross;
		StiCell[][] array = new StiCell[cross.ColCount][];
		for (int i = 0; i < cross.ColCount; i++)
		{
			array[i] = new StiCell[cross.RowCount];
			for (int j = 0; j < cross.RowCount; j++)
			{
				StiCell stiCell = cross.Cells[cross.ColCount - i - 1][j];
				array[i][j] = stiCell;
			}
		}
		Hashtable hashtable = new Hashtable();
		for (int k = 0; k < cross.ColCount; k++)
		{
			for (int l = 0; l < cross.RowCount; l++)
			{
				if (l < 2)
				{
					StiCell stiCell2 = array[cross.ColCount - k - 1][l];
					if (stiCell2 == null || stiCell2.Width <= 1 || stiCell2.IsChangeWidthForRightToLeft)
					{
						continue;
					}
					int num = 0;
					if (hashtable.Contains(stiCell2.Text))
					{
						num = (int)hashtable[stiCell2.Text];
						if (stiCell2.Width == 2)
						{
							hashtable.Remove(stiCell2.Text);
						}
					}
					else
					{
						num = k;
						hashtable[stiCell2.Text] = k;
					}
					StiCell stiCell3 = array[cross.ColCount - num - stiCell2.Width][l];
					stiCell2.IsChangeWidthForRightToLeft = true;
					stiCell3.IsChangeWidthForRightToLeft = true;
					array[cross.ColCount - num - stiCell2.Width][l] = stiCell2;
					array[cross.ColCount - k - 1][l] = stiCell3;
				}
				else
				{
					StiCell stiCell4 = array[k][l];
					if (stiCell4 != null && stiCell4.Width > 1)
					{
						StiCell stiCell5 = array[k - stiCell4.Width + 1][l];
						array[k - stiCell4.Width + 1][l] = stiCell4;
						array[k][l] = stiCell5;
					}
				}
			}
		}
		hashtable.Clear();
		cross.Cells = array;
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
		for (int num2 = cross.ColFields.Count - 1; num2 >= 0; num2--)
		{
			stiComponentsCollection.Add(cross.ColFields[num2]);
		}
	}

	public static void CalculateMaxAndMin(StiContainer outContainer, ref double maxLeft, ref double maxRight, int startIndex)
	{
		maxLeft = 0.0;
		maxRight = 0.0;
		for (int i = startIndex; i < outContainer.Components.Count; i++)
		{
			StiComponent stiComponent = outContainer.Components[i];
			if (i == startIndex)
			{
				maxLeft = stiComponent.Left;
				maxRight = stiComponent.Right;
			}
			else
			{
				maxLeft = Math.Min(maxLeft, stiComponent.Left);
				maxRight = Math.Max(maxRight, stiComponent.Right);
			}
		}
	}

	public static void MakeHorAlignmentByWidth(StiContainer outContainer, int startIndex)
	{
		if (outContainer.Components.Count == 0)
		{
			return;
		}
		double maxLeft = 0.0;
		double maxRight = 0.0;
		CalculateMaxAndMin(outContainer, ref maxLeft, ref maxRight, startIndex);
		Hashtable hashtable = new Hashtable();
		for (int i = startIndex; i < outContainer.Components.Count; i++)
		{
			StiComponent stiComponent = outContainer.Components[i];
			hashtable[stiComponent.Left] = stiComponent.Left;
			hashtable[stiComponent.Right] = stiComponent.Right;
		}
		decimal num = (decimal)outContainer.Width / ((decimal)maxRight - (decimal)maxLeft);
		double[] array = new double[hashtable.Count];
		hashtable.Keys.CopyTo(array, 0);
		double[] array2 = array;
		foreach (double num2 in array2)
		{
			double num3 = num2 - maxLeft;
			num3 = StiAlignValue.AlignToGrid((double)((decimal)num3 * num), 0.01, true);
			hashtable[num2] = num3;
		}
		for (int k = startIndex; k < outContainer.Components.Count; k++)
		{
			StiComponent stiComponent2 = outContainer.Components[k];
			double num4 = 0.0;
			if (hashtable.ContainsKey(stiComponent2.Left))
			{
				num4 = (double)hashtable[stiComponent2.Left];
			}
			double num5 = 0.0;
			if (hashtable.ContainsKey(stiComponent2.Right))
			{
				num5 = (double)hashtable[stiComponent2.Right];
			}
			stiComponent2.Left = num4;
			stiComponent2.Width = num5 - num4;
			if (num < 1m && stiComponent2 is StiText stiText)
			{
				stiText.Font = StiFontUtils.ChangeFontSize(stiText.Font, stiText.Font.Size * (float)num);
			}
		}
	}

	public static void ClearCross(StiCrossTab masterCrossTab)
	{
		if (masterCrossTab?.CrossTabInfo?.Cross != null)
		{
			bool flag = masterCrossTab.CrossTabInfo.Cross.Clear();
			masterCrossTab.CrossTabInfo.Cross = null;
			if (flag && StiOptions.Engine.ReportCache.AllowGCCollect)
			{
				GC.Collect();
				GC.WaitForPendingFinalizers();
				GC.Collect();
			}
		}
	}
}
