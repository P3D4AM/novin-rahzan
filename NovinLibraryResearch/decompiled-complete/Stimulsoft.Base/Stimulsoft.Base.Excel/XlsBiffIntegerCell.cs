namespace Stimulsoft.Base.Excel;

internal class XlsBiffIntegerCell : XlsBiffBlankCell
{
	public uint Value => ReadUInt16(6);

	internal XlsBiffIntegerCell(byte[] bytes)
		: this(bytes, 0u)
	{
	}

	internal XlsBiffIntegerCell(byte[] bytes, uint offset)
		: base(bytes, offset)
	{
	}
}
