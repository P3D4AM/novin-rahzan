namespace ExcelDataReader.Core.BinaryFormat;

internal class XlsBiffInterfaceHdr : XlsBiffRecord
{
	public ushort CodePage => ReadUInt16(0);

	internal XlsBiffInterfaceHdr(byte[] bytes, uint offset)
		: base(bytes, offset)
	{
	}
}
