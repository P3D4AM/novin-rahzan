namespace Stimulsoft.Base.Excel;

internal class XlsBiffSimpleValueRecord : XlsBiffRecord
{
	public ushort Value => ReadUInt16(0);

	internal XlsBiffSimpleValueRecord(byte[] bytes, uint offset)
		: base(bytes, offset)
	{
	}

	internal XlsBiffSimpleValueRecord(byte[] bytes)
		: this(bytes, 0u)
	{
	}
}
