namespace Stimulsoft.Base.Excel;

internal class XlsBiffBoolErr : XlsBiffBlankCell
{
	public bool BoolValue => ReadByte(6) == 1;

	internal XlsBiffBoolErr(byte[] bytes)
		: this(bytes, 0u)
	{
	}

	internal XlsBiffBoolErr(byte[] bytes, uint offset)
		: base(bytes, offset)
	{
	}
}
