namespace ExcelDataReader.Core.BinaryFormat;

internal class XlsBiffSimpleValueRecord : XlsBiffRecord
{
	public ushort Value => ReadUInt16(0);

	internal XlsBiffSimpleValueRecord(byte[] bytes, uint offset)
		: base(bytes, offset)
	{
	}
}
