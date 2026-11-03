using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.CrossTab.Core;

public class StiGrid
{
	public bool AlignToGrid = true;

	public decimal MaxWidth { get; set; } = 10m;


	public decimal MaxHeight { get; set; } = 10m;


	private double GridSize => Report.Info.GridSize;

	public StiReport Report { get; set; }

	public Hashtable Fields { get; } = new Hashtable();


	public bool DesignTime { get; set; }

	public decimal[] Widths { get; set; }

	public decimal[] Heights { get; set; }

	public decimal[] CoordX { get; private set; }

	public decimal[] CoordY { get; private set; }

	public StiCell[][] Cells { get; set; }

	public int RowCount
	{
		get
		{
			return Heights.Length;
		}
		set
		{
			Init(Widths.Length, value);
		}
	}

	public int ColCount
	{
		get
		{
			return Widths.Length;
		}
		set
		{
			Init(value, Heights.Length);
		}
	}

	public void SetTextOfCell(int x, int y, string value)
	{
		Cells[x][y].Text = value;
	}

	private decimal Align(decimal value)
	{
		return StiAlignValue.AlignToMaxGrid(value, (decimal)GridSize, AlignToGrid);
	}

	private decimal GetCellTotalWidth(int cellX, int width)
	{
		decimal result = default(decimal);
		for (int i = cellX; i < cellX + width; i++)
		{
			if (i < Widths.Length)
			{
				result += Widths[i];
			}
		}
		return result;
	}

	private decimal GetCellTotalHeight(int cellY, int height)
	{
		decimal result = default(decimal);
		for (int i = cellY; i < cellY + height; i++)
		{
			result += Heights[i];
		}
		return result;
	}

	public void DoAutoSize()
	{
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0521: Unknown result type (might be due to invalid IL or missing references)
		//IL_0526: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0564: Unknown result type (might be due to invalid IL or missing references)
		//IL_0569: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_0548: Unknown result type (might be due to invalid IL or missing references)
		//IL_0642: Unknown result type (might be due to invalid IL or missing references)
		//IL_0647: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0586: Unknown result type (might be due to invalid IL or missing references)
		//IL_058b: Unknown result type (might be due to invalid IL or missing references)
		//IL_066a: Unknown result type (might be due to invalid IL or missing references)
		//IL_066f: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_068c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0691: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_0844: Unknown result type (might be due to invalid IL or missing references)
		//IL_0849: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_043d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_040d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Invalid comparison between Unknown and I4
		//IL_0870: Unknown result type (might be due to invalid IL or missing references)
		//IL_0875: Unknown result type (might be due to invalid IL or missing references)
		//IL_0428: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Invalid comparison between Unknown and I4
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Invalid comparison between Unknown and I4
		SizeD val;
		for (int i = 0; i < ColCount; i++)
		{
			for (int j = 0; j < RowCount; j++)
			{
				StiCell stiCell = Cells[i][j];
				if (stiCell.Field != null && stiCell.ParentCell == stiCell)
				{
					double num = 10000.0;
					val = stiCell.Field.MaxSize;
					if (((SizeD)(ref val)).Width != 0.0)
					{
						StiCrossField field = stiCell.Field;
						StiUnit unit = stiCell.Field.Report.Unit;
						val = stiCell.Field.MaxSize;
						num = GetFieldWidth(field, unit.ConvertToHInches(((SizeD)(ref val)).Width));
					}
					val = stiCell.Field.MaxSize;
					if (((SizeD)(ref val)).Height != 0.0 && (stiCell.Field.Angle == 90f || stiCell.Field.Angle == 270f))
					{
						StiCrossField field2 = stiCell.Field;
						StiUnit unit2 = stiCell.Field.Report.Unit;
						val = stiCell.Field.MaxSize;
						num = GetFieldHeight(field2, unit2.ConvertToHInches(((SizeD)(ref val)).Height));
					}
					StiExpression text = stiCell.Field.Text;
					stiCell.Field.Text = stiCell.Text;
					SizeD size = SizeD.Empty;
					if (stiCell.Field.Report == null || !stiCell.Field.Report.IsWpf)
					{
						size = ((stiCell.Field.TextQuality == StiTextQuality.Wysiwyg || stiCell.Field.CheckAllowHtmlTags()) ? StiWysiwygTextRender.MeasureString(num, stiCell.Field.Font, stiCell.Field) : ((stiCell.Field.TextQuality != StiTextQuality.Typographic) ? StiStandardTextRenderer.MeasureString(num, stiCell.Field.Font, stiCell.Field) : StiTypographicTextRender.MeasureString(num, stiCell.Field.Font, stiCell.Field)));
					}
					else
					{
						size = StiWpfTextRender.MeasureString(num, stiCell.Field);
						if (stiCell.Field.CheckAllowHtmlTags())
						{
							((SizeD)(ref size)).Height = ((SizeD)(ref size)).Height * StiOptions.Engine.TextDrawingMeasurement.MeasurementFactorWpfWithHtmlInCrossTab;
						}
					}
					stiCell.Field.Text = text;
					((SizeD)(ref size)).Width = ((SizeD)(ref size)).Width + (stiCell.Field.Margins.Left + stiCell.Field.Margins.Right);
					((SizeD)(ref size)).Height = ((SizeD)(ref size)).Height + (stiCell.Field.Margins.Top + stiCell.Field.Margins.Bottom);
					StiIconSetIndicator stiIconSetIndicator = stiCell.Field.Indicator as StiIconSetIndicator;
					if (stiIconSetIndicator?.CustomIcon != null && stiIconSetIndicator != null && stiIconSetIndicator.CustomIconSize.HasValue)
					{
						((SizeD)(ref size)).Height = Math.Max(((SizeD)(ref size)).Height, stiIconSetIndicator.CustomIconSize.Value.Height);
						if ((int)stiIconSetIndicator.Alignment != 512 && (int)stiIconSetIndicator.Alignment != 32 && (int)stiIconSetIndicator.Alignment != 2)
						{
							((SizeD)(ref size)).Width = ((SizeD)(ref size)).Width + (double)stiIconSetIndicator.CustomIconSize.Value.Width;
						}
					}
					((SizeD)(ref size)).Width = Report.Unit.ConvertFromHInches(((SizeD)(ref size)).Width);
					((SizeD)(ref size)).Height = Report.Unit.ConvertFromHInches(((SizeD)(ref size)).Height);
					val = stiCell.Field.MaxSize;
					if (((SizeD)(ref val)).Width != 0.0)
					{
						val = stiCell.Field.MaxSize;
						if (((SizeD)(ref val)).Width < ((SizeD)(ref size)).Width)
						{
							val = stiCell.Field.MaxSize;
							((SizeD)(ref size)).Width = ((SizeD)(ref val)).Width;
						}
					}
					val = stiCell.Field.MinSize;
					if (((SizeD)(ref val)).Width != 0.0)
					{
						val = stiCell.Field.MinSize;
						if (((SizeD)(ref val)).Width > ((SizeD)(ref size)).Width)
						{
							val = stiCell.Field.MinSize;
							((SizeD)(ref size)).Width = ((SizeD)(ref val)).Width;
						}
					}
					stiCell.Size = size;
				}
				else
				{
					stiCell.Size = SizeD.Empty;
					if (i > 0 && stiCell.ParentCell == null && Cells[i - 1][j].Width == 1)
					{
						Cells[i - 1][j].Width++;
					}
				}
			}
		}
		for (int k = 0; k < ColCount; k++)
		{
			decimal val2 = (decimal)Report.Unit.ConvertFromHInches(StiOptions.Engine.CrossTab.DefaultWidth);
			for (int l = 0; l < RowCount; l++)
			{
				StiCell stiCell2 = Cells[k][l];
				if (stiCell2.Width > 1)
				{
					continue;
				}
				val = stiCell2.Size;
				decimal num2 = (decimal)((SizeD)(ref val)).Width;
				if (stiCell2.Field != null)
				{
					val = stiCell2.Field.MinSize;
					if (((SizeD)(ref val)).Width != 0.0)
					{
						decimal val3 = num2;
						val = stiCell2.Field.MinSize;
						num2 = Math.Max(val3, (decimal)((SizeD)(ref val)).Width);
					}
					val = stiCell2.Field.MaxSize;
					if (((SizeD)(ref val)).Width != 0.0)
					{
						decimal val4 = num2;
						val = stiCell2.Field.MaxSize;
						num2 = Math.Min(val4, (decimal)((SizeD)(ref val)).Width);
					}
				}
				val2 = Math.Max(val2, num2);
			}
			val2 = Math.Min(val2, MaxWidth);
			val2 = Align(val2);
			Widths[k] = val2;
		}
		for (int m = 0; m < RowCount; m++)
		{
			decimal val5 = (decimal)Report.Unit.ConvertFromHInches(StiOptions.Engine.CrossTab.DefaultHeight);
			for (int n = 0; n < ColCount; n++)
			{
				StiCell stiCell3 = Cells[n][m];
				if (stiCell3.Height > 1)
				{
					continue;
				}
				val = stiCell3.Size;
				decimal num3 = (decimal)((SizeD)(ref val)).Height;
				if (stiCell3.Field != null)
				{
					val = stiCell3.Field.MinSize;
					if (((SizeD)(ref val)).Height != 0.0)
					{
						decimal val6 = num3;
						val = stiCell3.Field.MinSize;
						num3 = Math.Max(val6, (decimal)((SizeD)(ref val)).Height);
					}
					val = stiCell3.Field.MaxSize;
					if (((SizeD)(ref val)).Height != 0.0)
					{
						decimal val7 = num3;
						val = stiCell3.Field.MaxSize;
						num3 = Math.Min(val7, (decimal)((SizeD)(ref val)).Height);
					}
				}
				val5 = Math.Max(val5, num3);
			}
			val5 = Math.Min(val5, MaxHeight);
			val5 = Align(val5);
			Heights[m] = val5;
		}
		for (int num4 = 0; num4 < ColCount; num4++)
		{
			for (int num5 = 0; num5 < RowCount; num5++)
			{
				StiCell stiCell4 = Cells[num4][num5];
				if (stiCell4.Field == null || stiCell4.ParentCell != stiCell4 || (stiCell4.Width <= 1 && stiCell4.Height <= 1))
				{
					continue;
				}
				decimal cellTotalWidth = GetCellTotalWidth(num4, stiCell4.Width);
				decimal cellTotalHeight = GetCellTotalHeight(num5, stiCell4.Height);
				val = stiCell4.Size;
				if ((decimal)((SizeD)(ref val)).Width > cellTotalWidth && cellTotalWidth > 0m)
				{
					val = stiCell4.Size;
					decimal num6 = (decimal)((SizeD)(ref val)).Width / cellTotalWidth;
					for (int num7 = num4; num7 < num4 + stiCell4.Width; num7++)
					{
						if (num7 < Widths.Length)
						{
							Widths[num7] *= num6;
						}
					}
				}
				val = stiCell4.Size;
				if (!((decimal)((SizeD)(ref val)).Height > cellTotalHeight) || !(cellTotalHeight > 0m))
				{
					continue;
				}
				val = stiCell4.Size;
				decimal num8 = (decimal)((SizeD)(ref val)).Height / cellTotalHeight;
				for (int num9 = num5; num9 < num5 + stiCell4.Height; num9++)
				{
					if (num9 < Heights.Length)
					{
						Heights[num9] *= num8;
					}
				}
			}
		}
		for (int num10 = 0; num10 < ColCount; num10++)
		{
			Widths[num10] = StiAlignValue.AlignToMaxGrid(Widths[num10], (decimal)Report.Info.GridSize, AlignToGrid);
		}
		for (int num11 = 0; num11 < RowCount; num11++)
		{
			Heights[num11] = StiAlignValue.AlignToMaxGrid(Heights[num11], (decimal)Report.Info.GridSize, AlignToGrid);
		}
		decimal num12 = default(decimal);
		for (int num13 = 0; num13 < ColCount; num13++)
		{
			CoordX[num13] = num12;
			num12 += Widths[num13];
		}
		decimal num14 = default(decimal);
		for (int num15 = 0; num15 < RowCount; num15++)
		{
			CoordY[num15] = num14;
			num14 += Heights[num15];
		}
	}

	private double GetFieldWidth(StiCrossField field, double value)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		RectangleD rect = default(RectangleD);
		((RectangleD)(ref rect))._002Ector(0.0, 0.0, value, field.Height);
		rect = field.ConvertTextMargins(rect, convert: false);
		rect = field.ConvertTextBorders(rect, convert: false);
		return ((RectangleD)(ref rect)).Width;
	}

	private double GetFieldHeight(StiCrossField field, double value)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		RectangleD rect = default(RectangleD);
		((RectangleD)(ref rect))._002Ector(0.0, 0.0, field.Height, value);
		rect = field.ConvertTextMargins(rect, convert: false);
		rect = field.ConvertTextBorders(rect, convert: false);
		return ((RectangleD)(ref rect)).Height;
	}

	public StiCell SetCell(int cellX, int cellY, int cellWidth, int cellHeight, object text, object value, StiCrossField field, bool isNumeric, object hyperlink, object toolTip, object tag, Dictionary<string, object> drillDownParameters, StiCellType type = StiCellType.Cell)
	{
		return SetCell(cellX, cellY, cellWidth, cellHeight, text, value, field, isNumeric, hyperlink, toolTip, tag, drillDownParameters, -1, null, null, keepMergedCellsTogether: false, type);
	}

	public StiCell SetCell(int cellX, int cellY, int cellWidth, int cellHeight, object text, object value, StiCrossField field, bool isNumeric, object hyperlink, object toolTip, object tag, Dictionary<string, object> drillDownParameters, int level, string parentGuid, string guid, bool keepMergedCellsTogether, StiCellType type = StiCellType.Cell)
	{
		StiCell parentCell = Cells[cellX][cellY];
		StiCrossField field2 = field;
		if (!DesignTime)
		{
			field = ((StiService)field).Clone() as StiCrossField;
		}
		object tagValue = field.TagValue;
		object toolTipValue = field.ToolTipValue;
		object hyperlinkValue = field.HyperlinkValue;
		string textValue = field.TextValue;
		StiValueEventArgs e = new StiValueEventArgs(value);
		field.TagValue = tag;
		field.ToolTipValue = toolTip;
		field.HyperlinkValue = hyperlink;
		StiCrossSummary stiCrossSummary = field as StiCrossSummary;
		if (stiCrossSummary != null)
		{
			if (CellExists(0, cellY))
			{
				stiCrossSummary.CrossRowValue = Cells[0][cellY].Value as string;
			}
			if (CellExists(cellX, 1))
			{
				stiCrossSummary.CrossColumnValue = Cells[cellX][1].Value as string;
			}
		}
		field.InvokeTextProcess(field, e);
		field.TagValue = tagValue;
		field.ToolTipValue = toolTipValue;
		field.HyperlinkValue = hyperlinkValue;
		field.OriginalValue = value;
		if (stiCrossSummary != null)
		{
			stiCrossSummary.CrossRowValue = null;
			stiCrossSummary.CrossColumnValue = null;
		}
		if (field.TextValue != textValue)
		{
			text = field.TextValue;
		}
		if (!StiOptions.Engine.CrossTab.MemoryOptimization || (text != null && text.ToString() != ""))
		{
			field2 = field;
		}
		StiCellType cellType = type;
		switch (type)
		{
		case StiCellType.HeaderCol:
			cellType = StiCellType.HeaderColMain;
			break;
		case StiCellType.HeaderColTotal:
			cellType = StiCellType.HeaderColTotalMain;
			break;
		case StiCellType.HeaderRow:
			cellType = StiCellType.HeaderRowMain;
			break;
		case StiCellType.HeaderRowTotal:
			cellType = StiCellType.HeaderRowTotalMain;
			break;
		case StiCellType.CornerCol:
			cellType = StiCellType.CornerColMain;
			break;
		case StiCellType.CornerRow:
			cellType = StiCellType.CornerRowMain;
			break;
		case StiCellType.LeftTopLine:
			cellType = StiCellType.LeftTopLineMain;
			break;
		case StiCellType.RightTopLine:
			cellType = StiCellType.RightTopLineMain;
			break;
		}
		for (int i = cellX; i < cellX + cellWidth; i++)
		{
			for (int j = cellY; j < cellY + cellHeight; j++)
			{
				if (text != null)
				{
					Cells[i][j].Text = text.ToString();
				}
				Cells[i][j].ParentCell = parentCell;
				Cells[i][j].Value = value;
				Cells[i][j].Width = cellWidth - (i - cellX);
				Cells[i][j].Height = cellHeight - (j - cellY);
				Cells[i][j].Field = field2;
				Cells[i][j].IsNumeric = isNumeric;
				Cells[i][j].HyperlinkValue = hyperlink;
				Cells[i][j].ToolTipValue = toolTip;
				Cells[i][j].TagValue = tag;
				Cells[i][j].DrillDownParameters = drillDownParameters;
				Cells[i][j].Level = level;
				Cells[i][j].ParentGuid = parentGuid;
				Cells[i][j].Guid = guid;
				Cells[i][j].KeepMergedCellsTogether = keepMergedCellsTogether;
				Cells[i][j].CellType = type;
			}
		}
		Cells[cellX][cellY].CellType = cellType;
		if (DesignTime && Fields[field] == null)
		{
			Fields[field] = new Point(cellX, cellY);
		}
		return Cells[cellX][cellY];
	}

	private bool CellExists(int cellX, int cellY)
	{
		if (Cells.GetLength(0) > cellX)
		{
			return Cells[cellX].GetLength(0) > cellY;
		}
		return false;
	}

	public void SetCellField(int cellX, int cellY, StiCrossField field)
	{
		if (Cells[cellX][cellY] != null)
		{
			Cells[cellX][cellY].Field = field;
		}
	}

	public void Init(int colCount, int rowCount)
	{
		if (StiOptions.Engine.CrossTab.AllowGCCollect && (colCount > 1000 || rowCount > 1000))
		{
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();
		}
		Widths = new decimal[colCount];
		Heights = new decimal[rowCount];
		CoordX = new decimal[colCount];
		CoordY = new decimal[rowCount];
		for (int i = 0; i < colCount; i++)
		{
			Widths[i] = 10m;
		}
		for (int j = 0; j < rowCount; j++)
		{
			Heights[j] = 10m;
		}
		Cells = new StiCell[colCount][];
		for (int k = 0; k < colCount; k++)
		{
			Cells[k] = new StiCell[rowCount];
			for (int l = 0; l < rowCount; l++)
			{
				Cells[k][l] = new StiCell();
			}
		}
	}
}
