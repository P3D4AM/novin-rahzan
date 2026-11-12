using System.IO;
using ExcelDataReader.Core;
using ExcelDataReader.Core.OpenXmlFormat;

namespace ExcelDataReader;

internal class ExcelOpenXmlReader : ExcelDataReader<XlsxWorkbook, XlsxWorksheet>
{
	private ZipWorker Document { get; set; }

	public ExcelOpenXmlReader(Stream stream)
	{
		Document = new ZipWorker(stream);
		base.Workbook = new XlsxWorkbook(Document);
		Reset();
	}

	public override void Close()
	{
		base.Close();
		Document?.Dispose();
		base.Workbook = null;
		Document = null;
	}
}
