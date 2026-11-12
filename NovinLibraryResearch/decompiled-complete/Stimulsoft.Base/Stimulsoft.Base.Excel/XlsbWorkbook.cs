using System;
using System.Collections.Generic;
using System.IO;

namespace Stimulsoft.Base.Excel;

internal class XlsbWorkbook
{
	private List<XlsbWorksheet> sheets;

	public List<XlsbWorksheet> Sheets
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

	private XlsbWorkbook()
	{
	}

	public XlsbWorkbook(Stream workbookStream, Stream sharedStringsStream, Stream stylesStream)
	{
		if (workbookStream == null)
		{
			throw new ArgumentNullException();
		}
		ReadWorkbook(workbookStream);
		ReadSharedStrings(sharedStringsStream);
		ReadStyles(stylesStream);
	}

	private void ReadStyles(Stream fileStream)
	{
		throw new NotImplementedException();
	}

	private void ReadSharedStrings(Stream fileStream)
	{
		if (fileStream == null)
		{
			return;
		}
		throw new NotImplementedException();
	}

	private void ReadWorkbook(Stream fileStream)
	{
		try
		{
			byte[] array = new BinaryReader(fileStream).ReadBytes((int)fileStream.Length);
			uint num = 0u;
			while (num < array.Length)
			{
				XlsbRecord record = XlsbRecord.GetRecord(array, num);
				short num2 = 0;
				uint num3 = 0u;
				num2 = (short)record.ID;
				num3 = record.GetLength();
				if (num2 != 0 || num3 != 0)
				{
					num += num3;
				}
			}
		}
		finally
		{
			byte[] array = null;
		}
	}
}
