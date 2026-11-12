namespace ExcelDataReader.Core.BinaryFormat;

internal class XlsBiffQuickTip : XlsBiffRecord
{
	internal XlsBiffQuickTip(byte[] bytes, uint offset)
		: base(bytes, offset)
	{
	}
}
