using System.IO;

namespace Stimulsoft.Base.Excel;

public static class ExcelReaderFactory
{
	public static IExcelDataReader CreateBinaryReader(Stream fileStream)
	{
		IExcelDataReader excelDataReader = new ExcelBinaryReader();
		excelDataReader.Initialize(fileStream);
		return excelDataReader;
	}

	public static IExcelDataReader CreateOpenXmlReader(Stream fileStream)
	{
		IExcelDataReader excelDataReader = new ExcelOpenXmlReader();
		excelDataReader.Initialize(fileStream);
		return excelDataReader;
	}
}
