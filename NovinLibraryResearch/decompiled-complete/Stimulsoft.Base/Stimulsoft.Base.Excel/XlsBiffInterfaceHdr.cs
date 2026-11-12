namespace Stimulsoft.Base.Excel;

internal class XlsBiffInterfaceHdr : XlsBiffRecord
{
	public ushort CodePage => ReadUInt16(0);

	internal XlsBiffInterfaceHdr(byte[] bytes, uint offset)
		: base(bytes, offset)
	{
	}

	internal XlsBiffInterfaceHdr(byte[] bytes)
		: this(bytes, 0u)
	{
	}
}
