using System.IO;

namespace Stimulsoft.Base.Excel;

public static class Factory
{
	public static IExcelDataReader CreateReader(Stream fileStream, ExcelFileType excelFileType)
	{
		IExcelDataReader excelDataReader = null;
		switch (excelFileType)
		{
		case ExcelFileType.Binary:
			excelDataReader = new ExcelBinaryReader();
			excelDataReader.Initialize(fileStream);
			break;
		case ExcelFileType.OpenXml:
			excelDataReader = new ExcelOpenXmlReader();
			excelDataReader.Initialize(fileStream);
			break;
		}
		return excelDataReader;
	}
}
