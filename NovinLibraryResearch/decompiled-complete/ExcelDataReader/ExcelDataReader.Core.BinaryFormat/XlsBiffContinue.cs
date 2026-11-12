namespace ExcelDataReader.Core.BinaryFormat;

internal class XlsBiffContinue : XlsBiffRecord
{
	internal XlsBiffContinue(byte[] bytes, uint offset)
		: base(bytes, offset)
	{
	}
}
