using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using ExcelDataReader.Core.NumberFormat;

namespace ExcelDataReader.Core.OpenXmlFormat;

internal class XlsxWorksheet : IWorksheet
{
	private const string NsSpreadsheetMl = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";

	private const string NDimension = "dimension";

	private const string NWorksheet = "worksheet";

	private const string NRow = "row";

	private const string NCol = "col";

	private const string NC = "c";

	private const string NV = "v";

	private const string NIs = "is";

	private const string NT = "t";

	private const string ARef = "ref";

	private const string AR = "r";

	private const string AT = "t";

	private const string AS = "s";

	private const string NSheetData = "sheetData";

	private const string NMergeCells = "mergeCells";

	private const string NMergeCell = "mergeCell";

	private const string NInlineStr = "inlineStr";

	private const string NStr = "str";

	private const string NHeaderFooter = "headerFooter";

	private const string ADifferentFirst = "differentFirst";

	private const string ADifferentOddEven = "differentOddEven";

	private const string NFirstHeader = "firstHeader";

	private const string NFirstFooter = "firstFooter";

	private const string NOddHeader = "oddHeader";

	private const string NOddFooter = "oddFooter";

	private const string NEvenHeader = "evenHeader";

	private const string NEvenFooter = "evenFooter";

	private const string NSheetProperties = "sheetPr";

	private const string NSheetFormatProperties = "sheetFormatPr";

	private const string ADefaultRowHeight = "defaultRowHeight";

	private const string AHidden = "hidden";

	private const string ACustomHeight = "customHeight";

	private const string AHt = "ht";

	private const string NCols = "cols";

	private const string AMin = "min";

	private const string AMax = "max";

	private const string AWidth = "width";

	private const string ACustomWidth = "customWidth";

	public int FieldCount { get; private set; }

	public int RowCount { get; private set; }

	public string Name { get; }

	public string CodeName { get; private set; }

	public string VisibleState { get; }

	public HeaderFooter HeaderFooter { get; private set; }

	public double DefaultRowHeight { get; private set; }

	public int Id { get; }

	public string Rid { get; set; }

	public string Path { get; set; }

	public CellRange[] MergeCells { get; private set; }

	public Col[] ColumnWidths { get; private set; }

	private ZipWorker Document { get; }

	private XlsxWorkbook Workbook { get; }

	public XlsxWorksheet(ZipWorker document, XlsxWorkbook workbook, XlsxBoundSheet refSheet)
	{
		Document = document;
		Workbook = workbook;
		Name = refSheet.Name;
		Id = refSheet.Id;
		Rid = refSheet.Rid;
		VisibleState = refSheet.VisibleState;
		Path = refSheet.Path;
		DefaultRowHeight = 12.75;
		ReadWorksheetGlobals();
	}

	public IEnumerable<Row> ReadRows()
	{
		int rowIndex = 0;
		foreach (XlsxElement item in ReadWorksheetStream(skipSheetData: false))
		{
			if (item.Type == XlsxElementType.Row)
			{
				Row rowBlock = ((XlsxRow)item).Row;
				while (rowIndex < rowBlock.RowIndex)
				{
					yield return new Row
					{
						RowIndex = rowIndex,
						Height = DefaultRowHeight,
						Cells = new List<Cell>()
					};
					int num = rowIndex + 1;
					rowIndex = num;
				}
				rowIndex++;
				yield return rowBlock;
			}
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

	private void ReadWorksheetGlobals()
	{
		if (string.IsNullOrEmpty(Path))
		{
			return;
		}
		int num = int.MinValue;
		int num2 = int.MinValue;
		foreach (XlsxElement item in ReadWorksheetStream(skipSheetData: false))
		{
			if (item.Type == XlsxElementType.Row)
			{
				Row row = ((XlsxRow)item).Row;
				num = Math.Max(num, row.RowIndex);
				num2 = Math.Max(num2, row.GetMaxColumnIndex());
			}
			else if (item.Type == XlsxElementType.HeaderFooter)
			{
				XlsxHeaderFooter xlsxHeaderFooter = (XlsxHeaderFooter)item;
				HeaderFooter = xlsxHeaderFooter.Value;
			}
			else if (item.Type == XlsxElementType.MergeCells)
			{
				XlsxMergeCells xlsxMergeCells = (XlsxMergeCells)item;
				MergeCells = xlsxMergeCells.Value.ToArray();
			}
			else if (item.Type == XlsxElementType.Cols)
			{
				XlsxCols xlsxCols = (XlsxCols)item;
				ColumnWidths = xlsxCols.Value.ToArray();
			}
		}
		if (num != int.MinValue && num2 != int.MinValue)
		{
			FieldCount = num2 + 1;
			RowCount = num + 1;
		}
	}

	private IEnumerable<XlsxElement> ReadWorksheetStream(bool skipSheetData)
	{
		if (string.IsNullOrEmpty(Path))
		{
			yield break;
		}
		using Stream sheetStream = Document.GetWorksheetStream(Path);
		if (sheetStream == null)
		{
			yield break;
		}
		using XmlReader xmlReader = XmlReader.Create(sheetStream);
		foreach (XlsxElement item in ReadWorksheetStream(xmlReader, skipSheetData))
		{
			yield return item;
		}
	}

	private IEnumerable<XlsxElement> ReadWorksheetStream(XmlReader xmlReader, bool skipSheetData)
	{
		if (!xmlReader.IsStartElement("worksheet", "http://schemas.openxmlformats.org/spreadsheetml/2006/main") || !XmlReaderHelper.ReadFirstContent(xmlReader))
		{
			yield break;
		}
		while (!xmlReader.EOF)
		{
			if (xmlReader.IsStartElement("dimension", "http://schemas.openxmlformats.org/spreadsheetml/2006/main"))
			{
				XlsxDimension xlsxDimension = ReadDimension(xmlReader);
				if (xlsxDimension != null)
				{
					yield return xlsxDimension;
				}
			}
			else if (xmlReader.IsStartElement("sheetData", "http://schemas.openxmlformats.org/spreadsheetml/2006/main"))
			{
				if (skipSheetData)
				{
					xmlReader.Skip();
					continue;
				}
				foreach (XlsxRow item in ReadSheetData(xmlReader))
				{
					yield return item;
				}
			}
			else if (xmlReader.IsStartElement("mergeCells", "http://schemas.openxmlformats.org/spreadsheetml/2006/main"))
			{
				XlsxMergeCells xlsxMergeCells = ReadMergeCells(xmlReader);
				if (xlsxMergeCells != null)
				{
					yield return xlsxMergeCells;
				}
			}
			else if (xmlReader.IsStartElement("headerFooter", "http://schemas.openxmlformats.org/spreadsheetml/2006/main"))
			{
				XlsxHeaderFooter xlsxHeaderFooter = ReadHeaderFooter(xmlReader);
				if (xlsxHeaderFooter != null)
				{
					yield return xlsxHeaderFooter;
				}
			}
			else if (xmlReader.IsStartElement("cols", "http://schemas.openxmlformats.org/spreadsheetml/2006/main"))
			{
				XlsxCols xlsxCols = ReadCols(xmlReader);
				if (xlsxCols != null)
				{
					yield return xlsxCols;
				}
			}
			else if (xmlReader.IsStartElement("sheetPr", "http://schemas.openxmlformats.org/spreadsheetml/2006/main"))
			{
				string attribute = xmlReader.GetAttribute("codeName");
				if (!string.IsNullOrEmpty(attribute))
				{
					CodeName = attribute;
				}
				xmlReader.Skip();
			}
			else if (xmlReader.IsStartElement("sheetFormatPr", "http://schemas.openxmlformats.org/spreadsheetml/2006/main"))
			{
				if (double.TryParse(xmlReader.GetAttribute("defaultRowHeight"), NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
				{
					DefaultRowHeight = result;
				}
				xmlReader.Skip();
			}
			else if (!XmlReaderHelper.SkipContent(xmlReader))
			{
				break;
			}
		}
	}

	private XlsxDimension ReadDimension(XmlReader xmlReader)
	{
		string attribute = xmlReader.GetAttribute("ref");
		xmlReader.Skip();
		if (!string.IsNullOrEmpty(attribute))
		{
			XlsxDimension xlsxDimension = new XlsxDimension(attribute);
			if (xlsxDimension.IsRange)
			{
				return xlsxDimension;
			}
		}
		return null;
	}

	private IEnumerable<XlsxRow> ReadSheetData(XmlReader xmlReader)
	{
		if (!XmlReaderHelper.ReadFirstContent(xmlReader))
		{
			yield break;
		}
		Row row = null;
		int nextRowIndex = 0;
		while (!xmlReader.EOF)
		{
			if (xmlReader.IsStartElement("row", "http://schemas.openxmlformats.org/spreadsheetml/2006/main"))
			{
				Row currentRow = ReadRow(xmlReader, nextRowIndex);
				if (row == null)
				{
					row = currentRow;
				}
				else if (currentRow.RowIndex != row.RowIndex)
				{
					yield return new XlsxRow
					{
						Row = row
					};
					row = currentRow;
				}
				else
				{
					row.Cells.AddRange(currentRow.Cells);
				}
				nextRowIndex = currentRow.RowIndex + 1;
			}
			else if (!XmlReaderHelper.SkipContent(xmlReader))
			{
				break;
			}
		}
		if (row != null)
		{
			yield return new XlsxRow
			{
				Row = row
			};
		}
	}

	private XlsxCols ReadCols(XmlReader xmlReader)
	{
		if (!XmlReaderHelper.ReadFirstContent(xmlReader))
		{
			return null;
		}
		List<Col> list = new List<Col>();
		while (!xmlReader.EOF)
		{
			if (xmlReader.IsStartElement("col", "http://schemas.openxmlformats.org/spreadsheetml/2006/main"))
			{
				string? attribute = xmlReader.GetAttribute("min");
				string? attribute2 = xmlReader.GetAttribute("max");
				string attribute3 = xmlReader.GetAttribute("width");
				string attribute4 = xmlReader.GetAttribute("customWidth");
				string attribute5 = xmlReader.GetAttribute("hidden");
				int num = int.Parse(attribute2);
				int num2 = int.Parse(attribute);
				double width = double.Parse(attribute3, CultureInfo.InvariantCulture);
				list.Add(new Col
				{
					CustomWidth = (attribute4 == "1"),
					Hidden = (attribute5 == "1"),
					Max = num - 1,
					Min = num2 - 1,
					Width = width
				});
				xmlReader.Skip();
			}
			else if (!XmlReaderHelper.SkipContent(xmlReader))
			{
				break;
			}
		}
		return new XlsxCols
		{
			Value = list
		};
	}

	private XlsxMergeCells ReadMergeCells(XmlReader xmlReader)
	{
		if (!XmlReaderHelper.ReadFirstContent(xmlReader))
		{
			return null;
		}
		List<CellRange> list = new List<CellRange>();
		while (!xmlReader.EOF)
		{
			if (xmlReader.IsStartElement("mergeCell", "http://schemas.openxmlformats.org/spreadsheetml/2006/main"))
			{
				string? attribute = xmlReader.GetAttribute("ref");
				string from = string.Empty;
				string to = string.Empty;
				string[] array = attribute.Split(new char[1] { ':' });
				if (array.Length == 2)
				{
					from = array[0];
					to = array[1];
				}
				list.Add(new CellRange(from, to));
				xmlReader.Skip();
			}
			else if (!XmlReaderHelper.SkipContent(xmlReader))
			{
				break;
			}
		}
		return new XlsxMergeCells
		{
			Value = list
		};
	}

	private XlsxHeaderFooter ReadHeaderFooter(XmlReader xmlReader)
	{
		bool hasDifferentFirst = xmlReader.GetAttribute("differentFirst") == "1";
		bool hasDifferentOddEven = xmlReader.GetAttribute("differentOddEven") == "1";
		if (!XmlReaderHelper.ReadFirstContent(xmlReader))
		{
			return null;
		}
		HeaderFooter headerFooter = new HeaderFooter(hasDifferentFirst, hasDifferentOddEven);
		while (!xmlReader.EOF)
		{
			if (xmlReader.IsStartElement("oddHeader", "http://schemas.openxmlformats.org/spreadsheetml/2006/main"))
			{
				headerFooter.OddHeader = xmlReader.ReadElementContentAsString();
			}
			else if (xmlReader.IsStartElement("oddFooter", "http://schemas.openxmlformats.org/spreadsheetml/2006/main"))
			{
				headerFooter.OddFooter = xmlReader.ReadElementContentAsString();
			}
			else if (xmlReader.IsStartElement("evenHeader", "http://schemas.openxmlformats.org/spreadsheetml/2006/main"))
			{
				headerFooter.EvenHeader = xmlReader.ReadElementContentAsString();
			}
			else if (xmlReader.IsStartElement("evenFooter", "http://schemas.openxmlformats.org/spreadsheetml/2006/main"))
			{
				headerFooter.EvenFooter = xmlReader.ReadElementContentAsString();
			}
			else if (xmlReader.IsStartElement("firstHeader", "http://schemas.openxmlformats.org/spreadsheetml/2006/main"))
			{
				headerFooter.FirstHeader = xmlReader.ReadElementContentAsString();
			}
			else if (xmlReader.IsStartElement("firstFooter", "http://schemas.openxmlformats.org/spreadsheetml/2006/main"))
			{
				headerFooter.FirstFooter = xmlReader.ReadElementContentAsString();
			}
			else if (!XmlReaderHelper.SkipContent(xmlReader))
			{
				break;
			}
		}
		return new XlsxHeaderFooter(headerFooter);
	}

	private Row ReadRow(XmlReader xmlReader, int nextRowIndex)
	{
		Row row = new Row
		{
			Cells = new List<Cell>()
		};
		if (int.TryParse(xmlReader.GetAttribute("r"), out var result))
		{
			row.RowIndex = result - 1;
		}
		else
		{
			row.RowIndex = nextRowIndex;
		}
		int.TryParse(xmlReader.GetAttribute("hidden"), out var result2);
		int.TryParse(xmlReader.GetAttribute("customHeight"), out var result3);
		double.TryParse(xmlReader.GetAttribute("ht"), NumberStyles.Any, CultureInfo.InvariantCulture, out var result4);
		if (result2 == 0)
		{
			row.Height = ((result3 != 0) ? result4 : DefaultRowHeight);
		}
		if (!XmlReaderHelper.ReadFirstContent(xmlReader))
		{
			return row;
		}
		int nextColumnIndex = 0;
		while (!xmlReader.EOF)
		{
			if (xmlReader.IsStartElement("c", "http://schemas.openxmlformats.org/spreadsheetml/2006/main"))
			{
				Cell cell = ReadCell(xmlReader, nextColumnIndex);
				nextColumnIndex = cell.ColumnIndex + 1;
				row.Cells.Add(cell);
			}
			else if (!XmlReaderHelper.SkipContent(xmlReader))
			{
				break;
			}
		}
		return row;
	}

	private Cell ReadCell(XmlReader xmlReader, int nextColumnIndex)
	{
		Cell cell = new Cell();
		string attribute = xmlReader.GetAttribute("s");
		string attribute2 = xmlReader.GetAttribute("t");
		if (ReferenceHelper.ParseReference(xmlReader.GetAttribute("r"), out var column, out var _))
		{
			cell.ColumnIndex = column - 1;
		}
		else
		{
			cell.ColumnIndex = nextColumnIndex;
		}
		if (attribute != null && int.TryParse(attribute, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
		{
			cell.NumberFormatIndex = Workbook.GetNumberFormatFromXF(result);
		}
		if (!XmlReaderHelper.ReadFirstContent(xmlReader))
		{
			return cell;
		}
		while (!xmlReader.EOF)
		{
			if (xmlReader.IsStartElement("v", "http://schemas.openxmlformats.org/spreadsheetml/2006/main"))
			{
				string text = xmlReader.ReadElementContentAsString();
				if (!string.IsNullOrEmpty(text))
				{
					cell.Value = ConvertCellValue(text, attribute2, cell.NumberFormatIndex);
				}
			}
			else if (xmlReader.IsStartElement("is", "http://schemas.openxmlformats.org/spreadsheetml/2006/main"))
			{
				string text2 = XlsxWorkbook.ReadStringItem(xmlReader);
				if (!string.IsNullOrEmpty(text2))
				{
					cell.Value = ConvertCellValue(text2, attribute2, cell.NumberFormatIndex);
				}
			}
			else if (!XmlReaderHelper.SkipContent(xmlReader))
			{
				break;
			}
		}
		return cell;
	}

	private object ConvertCellValue(string rawValue, string aT, int numberFormatIndex)
	{
		CultureInfo invariantCulture = CultureInfo.InvariantCulture;
		switch (aT)
		{
		case "s":
		{
			if (int.TryParse(rawValue, NumberStyles.Any, invariantCulture, out var result3) && result3 >= 0 && result3 < Workbook.SST.Count)
			{
				return Helpers.ConvertEscapeChars(Workbook.SST[result3]);
			}
			return rawValue;
		}
		case "inlineStr":
		case "str":
			return Helpers.ConvertEscapeChars(rawValue);
		case "b":
			return rawValue == "1";
		case "d":
		{
			if (DateTime.TryParseExact(rawValue, "yyyy-MM-dd", invariantCulture, DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite, out var result2))
			{
				return result2;
			}
			return rawValue;
		}
		case "e":
			return null;
		default:
		{
			if (double.TryParse(rawValue, NumberStyles.Any, invariantCulture, out var result))
			{
				NumberFormatString numberFormatString = GetNumberFormatString(numberFormatIndex);
				if (numberFormatString != null)
				{
					if (numberFormatString.IsDateTimeFormat)
					{
						return Helpers.ConvertFromOATime(result, Workbook.IsDate1904);
					}
					if (numberFormatString.IsTimeSpanFormat)
					{
						return TimeSpan.FromDays(result);
					}
				}
				return result;
			}
			return rawValue;
		}
		}
	}
}
