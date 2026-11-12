using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Stimulsoft.Base.Excel;

internal class XlsxWorkbook
{
	private const string N_sheet = "sheet";

	private const string N_t = "t";

	private const string N_si = "si";

	private const string N_cellXfs = "cellXfs";

	private const string N_numFmts = "numFmts";

	private const string A_sheetId = "sheetId";

	private const string A_name = "name";

	private const string A_rid = "r:id";

	private const string N_rel = "Relationship";

	private const string A_id = "Id";

	private const string A_target = "Target";

	private List<XlsxWorksheet> sheets;

	private XlsxSST _SST;

	private XlsxStyles _Styles;

	public List<XlsxWorksheet> Sheets
	{
		get
		{
			return sheets;
		}
		set
		{
			sheets = value;
		}
	}

	public XlsxSST SST => _SST;

	public XlsxStyles Styles => _Styles;

	private XlsxWorkbook()
	{
	}

	public XlsxWorkbook(Stream workbookStream, Stream relsStream, Stream sharedStringsStream, Stream stylesStream)
	{
		if (workbookStream == null)
		{
			throw new ArgumentNullException();
		}
		ReadWorkbook(workbookStream);
		ReadWorkbookRels(relsStream);
		ReadSharedStrings(sharedStringsStream);
		ReadStyles(stylesStream);
	}

	private void ReadStyles(Stream xmlFileStream)
	{
		if (xmlFileStream == null)
		{
			return;
		}
		_Styles = new XlsxStyles();
		bool flag = false;
		using XmlReader xmlReader = XmlReader.Create(xmlFileStream);
		while (xmlReader.Read())
		{
			if (!flag && xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "numFmts")
			{
				while (xmlReader.Read() && (xmlReader.NodeType != XmlNodeType.Element || xmlReader.Depth != 1))
				{
					if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "numFmt")
					{
						_Styles.NumFmts.Add(new XlsxNumFmt(int.Parse(xmlReader.GetAttribute("numFmtId")), xmlReader.GetAttribute("formatCode")));
					}
				}
				flag = true;
			}
			if (xmlReader.NodeType != XmlNodeType.Element || !(xmlReader.Name == "cellXfs"))
			{
				continue;
			}
			while (xmlReader.Read() && (xmlReader.NodeType != XmlNodeType.Element || xmlReader.Depth != 1))
			{
				if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "xf")
				{
					_Styles.CellXfs.Add(new XlsxXf(int.Parse(xmlReader.GetAttribute("xfId")), int.Parse(xmlReader.GetAttribute("numFmtId")), xmlReader.GetAttribute("applyNumberFormat")));
				}
			}
			break;
		}
		xmlFileStream.Close();
	}

	private void ReadSharedStrings(Stream xmlFileStream)
	{
		if (xmlFileStream == null)
		{
			return;
		}
		_SST = new XlsxSST();
		using XmlReader xmlReader = XmlReader.Create(xmlFileStream);
		bool flag = false;
		string text = "";
		while (xmlReader.Read())
		{
			if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "si")
			{
				if (flag)
				{
					_SST.Add(text);
				}
				else
				{
					flag = true;
				}
				text = "";
			}
			if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "t")
			{
				text += xmlReader.ReadElementContentAsString();
			}
		}
		if (flag)
		{
			_SST.Add(text);
		}
		xmlFileStream.Close();
	}

	private void ReadWorkbook(Stream xmlFileStream)
	{
		sheets = new List<XlsxWorksheet>();
		using XmlReader xmlReader = XmlReader.Create(xmlFileStream);
		while (xmlReader.Read())
		{
			if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "sheet")
			{
				sheets.Add(new XlsxWorksheet(xmlReader.GetAttribute("name"), int.Parse(xmlReader.GetAttribute("sheetId")), xmlReader.GetAttribute("r:id")));
			}
		}
		xmlFileStream.Close();
	}

	private void ReadWorkbookRels(Stream xmlFileStream)
	{
		using XmlReader xmlReader = XmlReader.Create(xmlFileStream);
		while (xmlReader.Read())
		{
			if (xmlReader.NodeType != XmlNodeType.Element || !(xmlReader.Name == "Relationship"))
			{
				continue;
			}
			string attribute = xmlReader.GetAttribute("Id");
			for (int i = 0; i < sheets.Count; i++)
			{
				XlsxWorksheet xlsxWorksheet = sheets[i];
				if (xlsxWorksheet.RID == attribute)
				{
					xlsxWorksheet.Path = xmlReader.GetAttribute("Target");
					sheets[i] = xlsxWorksheet;
					break;
				}
			}
		}
		xmlFileStream.Close();
	}
}
