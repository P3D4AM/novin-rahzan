using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ExcelDataReader.Core.NumberFormat;
using ExcelDataReader.Log;

namespace ExcelDataReader.Core.BinaryFormat;

internal class XlsWorksheet : IWorksheet
{
	internal class XlsRowBlock
	{
		public Dictionary<int, Row> Rows { get; set; }
	}

	internal class XlsRowOffset
	{
		public XlsBiffRow Record { get; set; }

		public int MinCellOffset { get; set; }

		public int MaxCellOffset { get; set; }

		public int MaxOverlapRowIndex { get; set; }
	}

	public string Name { get; }

	public string CodeName { get; private set; }

	public string VisibleState { get; }

	public HeaderFooter HeaderFooter { get; private set; }

	public CellRange[] MergeCells { get; private set; }

	public Col[] ColumnWidths { get; private set; }

	public uint DataOffset { get; }

	public Stream Stream { get; }

	public Encoding Encoding { get; private set; }

	public double DefaultRowHeight { get; set; }

	public Dictionary<int, XlsRowOffset> RowOffsetMap { get; }

	public int FieldCount { get; private set; }

	public int RowCount { get; private set; }

	public bool IsDate1904 { get; private set; }

	public XlsWorkbook Workbook { get; }

	public XlsWorksheet(XlsWorkbook workbook, XlsBiffBoundSheet refSheet, Stream stream)
	{
		Workbook = workbook;
		Stream = stream;
		IsDate1904 = workbook.IsDate1904;
		Encoding = workbook.Encoding;
		RowOffsetMap = new Dictionary<int, XlsRowOffset>();
		DefaultRowHeight = 255.0;
		Name = refSheet.GetSheetName(workbook.Encoding);
		DataOffset = refSheet.StartOffset;
		switch (refSheet.VisibleState)
		{
		case XlsBiffBoundSheet.SheetVisibility.Hidden:
			VisibleState = "hidden";
			break;
		case XlsBiffBoundSheet.SheetVisibility.VeryHidden:
			VisibleState = "veryhidden";
			break;
		default:
			VisibleState = "visible";
			break;
		}
		ReadWorksheetGlobals();
	}

	public IEnumerable<Row> ReadRows()
	{
		int rowIndex = 0;
		using XlsBiffStream biffStream = new XlsBiffStream(Stream, (int)DataOffset, Workbook.BiffVersion, null, Workbook.SecretKey, Workbook.Encryption);
		foreach (Row rowBlock in ReadWorksheetRows(biffStream))
		{
			while (rowIndex < rowBlock.RowIndex)
			{
				yield return new Row
				{
					RowIndex = rowIndex,
					Height = DefaultRowHeight / 20.0,
					Cells = new List<Cell>()
				};
				int num = rowIndex + 1;
				rowIndex = num;
			}
			rowIndex++;
			yield return rowBlock;
		}
	}

	public NumberFormatString GetNumberFormatString(int numberFormatIndex)
	{
		if (Workbook.Formats.TryGetValue(numberFormatIndex, out var value))
		{
			return value;
		}
		return null;
	}

	private void GetBlockSize(int startRow, out int blockRowCount, out int minOffset, out int maxOffset)
	{
		minOffset = int.MaxValue;
		maxOffset = int.MinValue;
		int i = 0;
		for (blockRowCount = Math.Min(32, RowCount - startRow); i < blockRowCount; i++)
		{
			if (RowOffsetMap.TryGetValue(startRow + i, out var value))
			{
				minOffset = Math.Min(value.MinCellOffset, minOffset);
				maxOffset = Math.Max(value.MaxCellOffset, maxOffset);
				if (value.MaxOverlapRowIndex != int.MinValue)
				{
					int num = value.MaxOverlapRowIndex + 1;
					blockRowCount = Math.Max(blockRowCount, num - startRow);
				}
			}
		}
	}

	private IEnumerable<Row> ReadWorksheetRows(XlsBiffStream biffStream)
	{
		int blockRowCount;
		for (int rowIndex = 0; rowIndex < RowCount; rowIndex += blockRowCount)
		{
			GetBlockSize(rowIndex, out blockRowCount, out var minOffset, out var maxOffset);
			XlsRowBlock block = ReadNextBlock(biffStream, rowIndex, blockRowCount, minOffset, maxOffset);
			int i = 0;
			while (i < blockRowCount)
			{
				if (block.Rows.TryGetValue(rowIndex + i, out var value))
				{
					yield return value;
				}
				int num = i + 1;
				i = num;
			}
		}
	}

	private XlsRowBlock ReadNextBlock(XlsBiffStream biffStream, int startRow, int rows, int minOffset, int maxOffset)
	{
		XlsRowBlock result = new XlsRowBlock
		{
			Rows = new Dictionary<int, Row>()
		};
		for (int i = 0; i < rows; i++)
		{
			if (RowOffsetMap.TryGetValue(startRow + i, out var _))
			{
				EnsureRow(result, startRow + i);
			}
		}
		if (minOffset == int.MaxValue)
		{
			return result;
		}
		biffStream.Position = minOffset;
		XlsBiffRecord ixfe = null;
		XlsBiffRecord xlsBiffRecord;
		while (biffStream.Position <= maxOffset && (xlsBiffRecord = biffStream.Read()) != null)
		{
			if (xlsBiffRecord.Id == BIFFRECORDTYPE.IXFE)
			{
				ixfe = xlsBiffRecord;
			}
			if (xlsBiffRecord.IsCell)
			{
				XlsBiffBlankCell xlsBiffBlankCell = (XlsBiffBlankCell)xlsBiffRecord;
				Row row = EnsureRow(result, xlsBiffBlankCell.RowIndex);
				if (xlsBiffBlankCell.Id == BIFFRECORDTYPE.MULRK)
				{
					List<Cell> collection = ReadMultiCell(xlsBiffBlankCell);
					row.Cells.AddRange(collection);
				}
				else
				{
					int formatIndexForCell = GetFormatIndexForCell(xlsBiffBlankCell, ixfe);
					Cell item = ReadSingleCell(biffStream, xlsBiffBlankCell, formatIndexForCell);
					row.Cells.Add(item);
				}
				ixfe = null;
			}
		}
		return result;
	}

	private Row EnsureRow(XlsRowBlock result, int rowIndex)
	{
		if (!result.Rows.TryGetValue(rowIndex, out var value))
		{
			double height = DefaultRowHeight / 20.0;
			if (RowOffsetMap.TryGetValue(rowIndex, out var value2) && value2.Record != null)
			{
				height = (value2.Record.UseDefaultRowHeight ? DefaultRowHeight : ((double)value2.Record.RowHeight)) / 20.0;
			}
			value = new Row
			{
				RowIndex = rowIndex,
				Height = height,
				Cells = new List<Cell>()
			};
			result.Rows.Add(rowIndex, value);
		}
		return value;
	}

	private List<Cell> ReadMultiCell(XlsBiffBlankCell cell)
	{
		LogManager.Log(this).Debug("ReadMultiCell {0}", cell.Id);
		List<Cell> list = new List<Cell>();
		BIFFRECORDTYPE id = cell.Id;
		if (id == BIFFRECORDTYPE.MULRK)
		{
			XlsBiffMulRKCell xlsBiffMulRKCell = (XlsBiffMulRKCell)cell;
			ushort lastColumnIndex = xlsBiffMulRKCell.LastColumnIndex;
			for (ushort num = cell.ColumnIndex; num <= lastColumnIndex; num++)
			{
				int numberFormatFromXF = Workbook.GetNumberFormatFromXF(xlsBiffMulRKCell.GetXF(num));
				Cell cell2 = new Cell
				{
					ColumnIndex = num,
					Value = TryConvertOADateTime(xlsBiffMulRKCell.GetValue(num), numberFormatFromXF),
					NumberFormatIndex = numberFormatFromXF
				};
				list.Add(cell2);
				LogManager.Log(this).Debug("VALUE[{1}]: {0}", cell2.Value, num);
			}
		}
		return list;
	}

	private Cell ReadSingleCell(XlsBiffStream biffStream, XlsBiffBlankCell cell, int numberFormatIndex)
	{
		LogManager.Log(this).Debug("ReadSingleCell {0}", cell.Id);
		Cell cell2 = new Cell
		{
			ColumnIndex = cell.ColumnIndex,
			NumberFormatIndex = numberFormatIndex
		};
		switch (cell.Id)
		{
		case BIFFRECORDTYPE.BOOLERR:
			if (cell.ReadByte(7) == 0)
			{
				cell2.Value = cell.ReadByte(6) != 0;
			}
			break;
		case BIFFRECORDTYPE.BOOLERR_OLD:
			if (cell.ReadByte(8) == 0)
			{
				cell2.Value = cell.ReadByte(7) != 0;
			}
			break;
		case BIFFRECORDTYPE.INTEGER_OLD:
		case BIFFRECORDTYPE.INTEGER:
		{
			int value3 = ((XlsBiffIntegerCell)cell).Value;
			cell2.Value = TryConvertOADateTime(value3, numberFormatIndex);
			break;
		}
		case BIFFRECORDTYPE.NUMBER_OLD:
		case BIFFRECORDTYPE.NUMBER:
		{
			double value2 = ((XlsBiffNumberCell)cell).Value;
			cell2.Value = TryConvertOADateTime(value2, numberFormatIndex);
			break;
		}
		case BIFFRECORDTYPE.LABEL_OLD:
		case BIFFRECORDTYPE.RSTRING:
		case BIFFRECORDTYPE.LABEL:
			cell2.Value = ((XlsBiffLabelCell)cell).GetValue(Encoding);
			break;
		case BIFFRECORDTYPE.LABELSST:
			cell2.Value = Workbook.SST.GetString(((XlsBiffLabelSSTCell)cell).SSTIndex, Encoding);
			break;
		case BIFFRECORDTYPE.RK:
		{
			double value2 = ((XlsBiffRKCell)cell).Value;
			cell2.Value = TryConvertOADateTime(value2, numberFormatIndex);
			break;
		}
		case BIFFRECORDTYPE.FORMULA:
		case BIFFRECORDTYPE.FORMULA_V3:
		case BIFFRECORDTYPE.FORMULA_V4:
		{
			object value = TryGetFormulaValue(biffStream, (XlsBiffFormulaCell)cell, numberFormatIndex);
			cell2.Value = value;
			break;
		}
		}
		LogManager.Log(this).Debug("VALUE: {0}", cell2.Value);
		return cell2;
	}

	private object TryGetFormulaValue(XlsBiffStream biffStream, XlsBiffFormulaCell formulaCell, int numberFormatIndex)
	{
		return formulaCell.FormulaType switch
		{
			XlsBiffFormulaCell.FormulaValueType.Boolean => formulaCell.BooleanValue, 
			XlsBiffFormulaCell.FormulaValueType.Error => null, 
			XlsBiffFormulaCell.FormulaValueType.EmptyString => string.Empty, 
			XlsBiffFormulaCell.FormulaValueType.Number => TryConvertOADateTime(formulaCell.XNumValue, numberFormatIndex), 
			XlsBiffFormulaCell.FormulaValueType.String => TryGetFormulaString(biffStream), 
			_ => null, 
		};
	}

	private string TryGetFormulaString(XlsBiffStream biffStream)
	{
		XlsBiffRecord xlsBiffRecord = biffStream.Read();
		if (xlsBiffRecord != null && xlsBiffRecord.Id == BIFFRECORDTYPE.SHAREDFMLA)
		{
			xlsBiffRecord = biffStream.Read();
		}
		if (xlsBiffRecord != null && xlsBiffRecord.Id == BIFFRECORDTYPE.STRING)
		{
			return ((XlsBiffFormulaString)xlsBiffRecord).GetValue(Encoding);
		}
		return null;
	}

	private object TryConvertOADateTime(double value, int numberFormatIndex)
	{
		NumberFormatString numberFormatString = GetNumberFormatString(numberFormatIndex);
		if (numberFormatString != null)
		{
			if (numberFormatString.IsDateTimeFormat)
			{
				return Helpers.ConvertFromOATime(value, IsDate1904);
			}
			if (numberFormatString.IsTimeSpanFormat)
			{
				return TimeSpan.FromDays(value);
			}
		}
		return value;
	}

	private object TryConvertOADateTime(int value, int numberFormatIndex)
	{
		NumberFormatString numberFormatString = GetNumberFormatString(numberFormatIndex);
		if (numberFormatString != null)
		{
			if (numberFormatString.IsDateTimeFormat)
			{
				return Helpers.ConvertFromOATime(value, IsDate1904);
			}
			if (numberFormatString.IsTimeSpanFormat)
			{
				return TimeSpan.FromDays(value);
			}
		}
		return value;
	}

	private int GetFormatIndexForCell(XlsBiffBlankCell cell, XlsBiffRecord ixfe)
	{
		if (Workbook.BiffVersion == 2)
		{
			if (cell.XFormat == 63 && ixfe != null)
			{
				ushort xfIndex = ixfe.ReadUInt16(0);
				return Workbook.GetNumberFormatFromXF(xfIndex);
			}
			if (cell.XFormat > 63)
			{
				return 0;
			}
			if (cell.XFormat < Workbook.GetExtendedFormatCount())
			{
				return Workbook.GetNumberFormatFromXF(cell.XFormat);
			}
			return Workbook.GetNumberFormatFromFileIndex(cell.Format);
		}
		return Workbook.GetNumberFormatFromXF(cell.XFormat);
	}

	private void ReadWorksheetGlobals()
	{
		using XlsBiffStream xlsBiffStream = new XlsBiffStream(Stream, (int)DataOffset, Workbook.BiffVersion, null, Workbook.SecretKey, Workbook.Encryption);
		if (xlsBiffStream.BiffVersion == 0 || xlsBiffStream.BiffType != BIFFTYPE.Worksheet)
		{
			return;
		}
		XlsBiffHeaderFooterString xlsBiffHeaderFooterString = null;
		XlsBiffHeaderFooterString xlsBiffHeaderFooterString2 = null;
		int num = -1;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		List<CellRange> list = new List<CellRange>();
		Dictionary<ushort, XlsBiffFormatString> dictionary = new Dictionary<ushort, XlsBiffFormatString>();
		int position = xlsBiffStream.Position;
		XlsBiffRecord xlsBiffRecord = xlsBiffStream.Read();
		List<Col> list2 = new List<Col>();
		while (xlsBiffRecord != null && !(xlsBiffRecord is XlsBiffEof))
		{
			if (xlsBiffRecord is XlsBiffDimensions xlsBiffDimensions)
			{
				FieldCount = xlsBiffDimensions.LastColumn;
				RowCount = (int)xlsBiffDimensions.LastRow;
			}
			if (xlsBiffRecord.Id == BIFFRECORDTYPE.DEFAULTROWHEIGHT || xlsBiffRecord.Id == BIFFRECORDTYPE.DEFAULTROWHEIGHT_V2)
			{
				XlsBiffDefaultRowHeight xlsBiffDefaultRowHeight = (XlsBiffDefaultRowHeight)xlsBiffRecord;
				DefaultRowHeight = xlsBiffDefaultRowHeight.RowHeight;
			}
			if (xlsBiffRecord.Id == BIFFRECORDTYPE.RECORD1904)
			{
				IsDate1904 = ((XlsBiffSimpleValueRecord)xlsBiffRecord).Value == 1;
			}
			if (xlsBiffRecord.Id == BIFFRECORDTYPE.XF_V2 || xlsBiffRecord.Id == BIFFRECORDTYPE.XF_V3 || xlsBiffRecord.Id == BIFFRECORDTYPE.XF_V4)
			{
				Workbook.AddExtendedFormat(-1, ((XlsBiffXF)xlsBiffRecord).Format, applyNumberFormat: true);
			}
			if (xlsBiffRecord.Id == BIFFRECORDTYPE.MERGECELLS)
			{
				list.AddRange(((XlsBiffMergeCells)xlsBiffRecord).MergeCells);
			}
			if (xlsBiffRecord.Id == BIFFRECORDTYPE.COLINFO)
			{
				list2.Add(((XlsBiffColInfo)xlsBiffRecord).Value);
			}
			if (xlsBiffRecord.Id == BIFFRECORDTYPE.FORMAT)
			{
				XlsBiffFormatString xlsBiffFormatString = (XlsBiffFormatString)xlsBiffRecord;
				if (Workbook.BiffVersion >= 5)
				{
					dictionary.Add(xlsBiffFormatString.Index, xlsBiffFormatString);
				}
				else
				{
					dictionary.Add((ushort)dictionary.Count, xlsBiffFormatString);
				}
			}
			if (xlsBiffRecord.Id == BIFFRECORDTYPE.FORMAT_V23)
			{
				XlsBiffFormatString value = (XlsBiffFormatString)xlsBiffRecord;
				dictionary.Add((ushort)dictionary.Count, value);
			}
			if (xlsBiffRecord.Id == BIFFRECORDTYPE.CODEPAGE)
			{
				XlsBiffSimpleValueRecord xlsBiffSimpleValueRecord = (XlsBiffSimpleValueRecord)xlsBiffRecord;
				Encoding = EncodingHelper.GetEncoding(xlsBiffSimpleValueRecord.Value);
			}
			if (xlsBiffRecord.Id == BIFFRECORDTYPE.HEADER && xlsBiffRecord.RecordSize > 0)
			{
				xlsBiffHeaderFooterString = (XlsBiffHeaderFooterString)xlsBiffRecord;
			}
			if (xlsBiffRecord.Id == BIFFRECORDTYPE.FOOTER && xlsBiffRecord.RecordSize > 0)
			{
				xlsBiffHeaderFooterString2 = (XlsBiffHeaderFooterString)xlsBiffRecord;
			}
			if (xlsBiffRecord.Id == BIFFRECORDTYPE.CODENAME)
			{
				XlsBiffCodeName xlsBiffCodeName = (XlsBiffCodeName)xlsBiffRecord;
				CodeName = xlsBiffCodeName.GetValue(Encoding);
			}
			if (xlsBiffRecord.Id == BIFFRECORDTYPE.ROW || xlsBiffRecord.Id == BIFFRECORDTYPE.ROW_V2)
			{
				XlsBiffRow xlsBiffRow = (XlsBiffRow)xlsBiffRecord;
				SetMinMaxRow(xlsBiffRow.RowIndex, xlsBiffRow);
				num4 = Math.Max(num4, xlsBiffRow.RowIndex + 1);
			}
			if (xlsBiffRecord.Id == BIFFRECORDTYPE.IXFE)
			{
				num = position;
			}
			if (xlsBiffRecord.IsCell)
			{
				XlsBiffBlankCell xlsBiffBlankCell = (XlsBiffBlankCell)xlsBiffRecord;
				num2 = Math.Max(num2, xlsBiffBlankCell.ColumnIndex + 1);
				num3 = Math.Max(num3, xlsBiffBlankCell.RowIndex + 1);
				if (num != -1)
				{
					SetMinMaxRowOffset(xlsBiffBlankCell.RowIndex, num, num3 - 1);
					num = -1;
				}
				SetMinMaxRowOffset(xlsBiffBlankCell.RowIndex, position, num3 - 1);
			}
			position = xlsBiffStream.Position;
			xlsBiffRecord = xlsBiffStream.Read();
			if (xlsBiffRecord is XlsBiffBOF)
			{
				break;
			}
		}
		if (xlsBiffHeaderFooterString != null || xlsBiffHeaderFooterString2 != null)
		{
			HeaderFooter = new HeaderFooter(hasDifferentFirst: false, hasDifferentOddEven: false)
			{
				OddHeader = xlsBiffHeaderFooterString?.GetValue(Encoding),
				OddFooter = xlsBiffHeaderFooterString2?.GetValue(Encoding)
			};
		}
		foreach (KeyValuePair<ushort, XlsBiffFormatString> item in dictionary)
		{
			Workbook.AddNumberFormat(item.Key, item.Value.GetValue(Encoding));
		}
		if (list.Count > 0)
		{
			MergeCells = list.ToArray();
		}
		if (FieldCount < num2)
		{
			FieldCount = num2;
		}
		num3 = Math.Max(num3, num4);
		if (RowCount < num3)
		{
			RowCount = num3;
		}
		if (list2.Count > 0)
		{
			ColumnWidths = list2.ToArray();
		}
	}

	private void SetMinMaxRow(int rowIndex, XlsBiffRow row)
	{
		if (!RowOffsetMap.TryGetValue(rowIndex, out var value))
		{
			value = new XlsRowOffset();
			value.MinCellOffset = int.MaxValue;
			value.MaxCellOffset = int.MinValue;
			value.MaxOverlapRowIndex = int.MinValue;
			RowOffsetMap.Add(rowIndex, value);
		}
		value.Record = row;
	}

	private void SetMinMaxRowOffset(int rowIndex, int recordOffset, int maxOverlapRow)
	{
		if (!RowOffsetMap.TryGetValue(rowIndex, out var value))
		{
			value = new XlsRowOffset();
			value.MinCellOffset = int.MaxValue;
			value.MaxCellOffset = int.MinValue;
			value.MaxOverlapRowIndex = int.MinValue;
			RowOffsetMap.Add(rowIndex, value);
		}
		value.MinCellOffset = Math.Min(recordOffset, value.MinCellOffset);
		value.MaxCellOffset = Math.Max(recordOffset, value.MaxCellOffset);
		value.MaxOverlapRowIndex = Math.Max(maxOverlapRow, value.MaxOverlapRowIndex);
	}
}
