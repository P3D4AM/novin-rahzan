using System.Collections.Generic;
using System.IO;
using System.Xml;
using ExcelDataReader.Exceptions;

namespace ExcelDataReader.Core.OpenXmlFormat;

internal class XlsxWorkbook : CommonWorkbook, IWorkbook<XlsxWorksheet>
{
	private const string NsSpreadsheetMl = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";

	private const string NsRelationship = "http://schemas.openxmlformats.org/package/2006/relationships";

	private const string NsDocumentRelationship = "http://schemas.openxmlformats.org/officeDocument/2006/relationships";

	private const string ElementSst = "sst";

	private const string ElementSheets = "sheets";

	private const string ElementSheet = "sheet";

	private const string ElementT = "t";

	private const string ElementR = "r";

	private const string ElementStringItem = "si";

	private const string ElementStyleSheet = "styleSheet";

	private const string ElementCellCrossReference = "cellXfs";

	private const string ElementNumberFormats = "numFmts";

	private const string ElementWorkbook = "workbook";

	private const string ElementWorkbookProperties = "workbookPr";

	private const string AttributeSheetId = "sheetId";

	private const string AttributeVisibleState = "state";

	private const string AttributeName = "name";

	private const string AttributeRelationshipId = "id";

	private const string ElementRelationship = "Relationship";

	private const string ElementRelationships = "Relationships";

	private const string AttributeId = "Id";

	private const string AttributeTarget = "Target";

	private const string NXF = "xf";

	private const string ANumFmtId = "numFmtId";

	private const string AXFId = "xfId";

	private const string AApplyNumberFormat = "applyNumberFormat";

	private const string NNumFmt = "numFmt";

	private const string AFormatCode = "formatCode";

	private ZipWorker _zipWorker;

	public List<XlsxBoundSheet> Sheets { get; } = new List<XlsxBoundSheet>();


	public XlsxSST SST { get; } = new XlsxSST();


	public bool IsDate1904 { get; private set; }

	public int ResultsCount => Sheets?.Count ?? (-1);

	public XlsxWorkbook(ZipWorker zipWorker)
	{
		_zipWorker = zipWorker;
		ReadWorkbook();
		ReadWorkbookRels();
		ReadSharedStrings();
		ReadStyles();
	}

	public static string ReadStringItem(XmlReader reader)
	{
		string text = string.Empty;
		if (!XmlReaderHelper.ReadFirstContent(reader))
		{
			return text;
		}
		while (!reader.EOF)
		{
			if (reader.IsStartElement("t", "http://schemas.openxmlformats.org/spreadsheetml/2006/main"))
			{
				text += reader.ReadElementContentAsString();
			}
			else if (reader.IsStartElement("r", "http://schemas.openxmlformats.org/spreadsheetml/2006/main"))
			{
				text += ReadRichTextRun(reader);
			}
			else if (!XmlReaderHelper.SkipContent(reader))
			{
				break;
			}
		}
		return text;
	}

	public IEnumerable<XlsxWorksheet> ReadWorksheets()
	{
		foreach (XlsxBoundSheet sheet in Sheets)
		{
			yield return new XlsxWorksheet(_zipWorker, this, sheet);
		}
	}

	private static string ReadRichTextRun(XmlReader reader)
	{
		string text = string.Empty;
		if (!XmlReaderHelper.ReadFirstContent(reader))
		{
			return text;
		}
		while (!reader.EOF)
		{
			if (reader.IsStartElement("t", "http://schemas.openxmlformats.org/spreadsheetml/2006/main"))
			{
				text += reader.ReadElementContentAsString();
			}
			else if (!XmlReaderHelper.SkipContent(reader))
			{
				break;
			}
		}
		return text;
	}

	private void ReadWorkbook()
	{
		using Stream stream = _zipWorker.GetWorkbookStream();
		if (stream == null)
		{
			throw new HeaderException("Detected ZIP file, but not a valid OpenXml file.");
		}
		using XmlReader reader = XmlReader.Create(stream);
		ReadWorkbook(reader);
	}

	private void ReadWorkbook(XmlReader reader)
	{
		if (!reader.IsStartElement("workbook", "http://schemas.openxmlformats.org/spreadsheetml/2006/main") || !XmlReaderHelper.ReadFirstContent(reader))
		{
			return;
		}
		while (!reader.EOF)
		{
			if (reader.IsStartElement("workbookPr", "http://schemas.openxmlformats.org/spreadsheetml/2006/main"))
			{
				IsDate1904 = reader.GetAttribute("date1904") == "1";
				reader.Skip();
			}
			else if (reader.IsStartElement("sheets", "http://schemas.openxmlformats.org/spreadsheetml/2006/main"))
			{
				ReadSheets(reader);
			}
			else if (!XmlReaderHelper.SkipContent(reader))
			{
				break;
			}
		}
	}

	private void ReadSheets(XmlReader reader)
	{
		if (!XmlReaderHelper.ReadFirstContent(reader))
		{
			return;
		}
		while (!reader.EOF)
		{
			if (reader.IsStartElement("sheet", "http://schemas.openxmlformats.org/spreadsheetml/2006/main"))
			{
				Sheets.Add(new XlsxBoundSheet(reader.GetAttribute("name"), int.Parse(reader.GetAttribute("sheetId")), reader.GetAttribute("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships"), reader.GetAttribute("state")));
				reader.Skip();
			}
			else if (!XmlReaderHelper.SkipContent(reader))
			{
				break;
			}
		}
	}

	private void ReadWorkbookRels()
	{
		using Stream stream = _zipWorker.GetWorkbookRelsStream();
		if (stream == null)
		{
			return;
		}
		using XmlReader reader = XmlReader.Create(stream);
		ReadWorkbookRels(reader);
	}

	private void ReadWorkbookRels(XmlReader reader)
	{
		if (!reader.IsStartElement("Relationships", "http://schemas.openxmlformats.org/package/2006/relationships") || !XmlReaderHelper.ReadFirstContent(reader))
		{
			return;
		}
		while (!reader.EOF)
		{
			if (reader.IsStartElement("Relationship", "http://schemas.openxmlformats.org/package/2006/relationships"))
			{
				string attribute = reader.GetAttribute("Id");
				foreach (XlsxBoundSheet sheet in Sheets)
				{
					if (sheet.Rid == attribute)
					{
						sheet.Path = reader.GetAttribute("Target");
						break;
					}
				}
				reader.Skip();
			}
			else if (!XmlReaderHelper.SkipContent(reader))
			{
				break;
			}
		}
	}

	private void ReadSharedStrings()
	{
		using Stream stream = _zipWorker.GetSharedStringsStream();
		if (stream == null)
		{
			return;
		}
		using XmlReader reader = XmlReader.Create(stream);
		ReadSharedStrings(reader);
	}

	private void ReadSharedStrings(XmlReader reader)
	{
		if (!reader.IsStartElement("sst", "http://schemas.openxmlformats.org/spreadsheetml/2006/main") || !XmlReaderHelper.ReadFirstContent(reader))
		{
			return;
		}
		while (!reader.EOF)
		{
			if (reader.IsStartElement("si", "http://schemas.openxmlformats.org/spreadsheetml/2006/main"))
			{
				string item = ReadStringItem(reader);
				SST.Add(item);
			}
			else if (!XmlReaderHelper.SkipContent(reader))
			{
				break;
			}
		}
	}

	private void ReadStyles()
	{
		using Stream stream = _zipWorker.GetStylesStream();
		if (stream == null)
		{
			return;
		}
		using XmlReader reader = XmlReader.Create(stream);
		ReadStyles(reader);
	}

	private void ReadStyles(XmlReader reader)
	{
		if (!reader.IsStartElement("styleSheet", "http://schemas.openxmlformats.org/spreadsheetml/2006/main") || !XmlReaderHelper.ReadFirstContent(reader))
		{
			return;
		}
		while (!reader.EOF)
		{
			if (reader.IsStartElement("cellXfs", "http://schemas.openxmlformats.org/spreadsheetml/2006/main"))
			{
				ReadCellXfs(reader);
			}
			else if (reader.IsStartElement("numFmts", "http://schemas.openxmlformats.org/spreadsheetml/2006/main"))
			{
				ReadNumberFormats(reader);
			}
			else if (!XmlReaderHelper.SkipContent(reader))
			{
				break;
			}
		}
	}

	private void ReadCellXfs(XmlReader reader)
	{
		if (!XmlReaderHelper.ReadFirstContent(reader))
		{
			return;
		}
		while (!reader.EOF)
		{
			if (reader.IsStartElement("xf", "http://schemas.openxmlformats.org/spreadsheetml/2006/main"))
			{
				int.TryParse(reader.GetAttribute("xfId"), out var result);
				int.TryParse(reader.GetAttribute("numFmtId"), out var result2);
				bool applyNumberFormat = reader.GetAttribute("applyNumberFormat") != "0";
				AddExtendedFormat(result, result2, applyNumberFormat);
				reader.Skip();
			}
			else if (!XmlReaderHelper.SkipContent(reader))
			{
				break;
			}
		}
	}

	private void ReadNumberFormats(XmlReader reader)
	{
		if (!XmlReaderHelper.ReadFirstContent(reader))
		{
			return;
		}
		while (!reader.EOF)
		{
			if (reader.IsStartElement("numFmt", "http://schemas.openxmlformats.org/spreadsheetml/2006/main"))
			{
				int.TryParse(reader.GetAttribute("numFmtId"), out var result);
				string attribute = reader.GetAttribute("formatCode");
				AddNumberFormat(result, attribute);
				reader.Skip();
			}
			else if (!XmlReaderHelper.SkipContent(reader))
			{
				break;
			}
		}
	}
}
