namespace ExcelDataReader.Core.BinaryFormat;

internal class XlsBiffEof : XlsBiffRecord
{
	internal XlsBiffEof(byte[] bytes, uint offset)
		: base(bytes, offset)
	{
	}
}
