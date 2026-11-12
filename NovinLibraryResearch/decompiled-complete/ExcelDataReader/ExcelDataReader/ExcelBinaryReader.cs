using System.IO;
using System.Text;
using ExcelDataReader.Core.BinaryFormat;

namespace ExcelDataReader;

internal class ExcelBinaryReader : ExcelDataReader<XlsWorkbook, XlsWorksheet>
{
	public ExcelBinaryReader(Stream stream, string password, Encoding fallbackEncoding)
	{
		base.Workbook = new XlsWorkbook(stream, password, fallbackEncoding);
		Reset();
	}

	public override void Close()
	{
		base.Close();
		base.Workbook?.Stream?.Dispose();
		base.Workbook = null;
	}
}
