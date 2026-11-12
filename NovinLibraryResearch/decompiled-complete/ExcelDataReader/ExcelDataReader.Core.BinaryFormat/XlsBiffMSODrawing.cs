namespace ExcelDataReader.Core.BinaryFormat;

internal class XlsBiffMSODrawing : XlsBiffRecord
{
	internal XlsBiffMSODrawing(byte[] bytes, uint offset)
		: base(bytes, offset)
	{
	}
}
