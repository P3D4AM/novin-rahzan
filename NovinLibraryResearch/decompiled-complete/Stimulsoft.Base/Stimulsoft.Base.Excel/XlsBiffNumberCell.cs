namespace Stimulsoft.Base.Excel;

internal class XlsBiffNumberCell : XlsBiffBlankCell
{
	public double Value => ReadDouble(6);

	internal XlsBiffNumberCell(byte[] bytes)
		: this(bytes, 0u)
	{
	}

	internal XlsBiffNumberCell(byte[] bytes, uint offset)
		: base(bytes, offset)
	{
	}
}
