namespace ExcelDataReader.Core.BinaryFormat;

internal class XlsBiffUncalced : XlsBiffRecord
{
	internal XlsBiffUncalced(byte[] bytes, uint offset)
		: base(bytes, offset)
	{
	}
}
