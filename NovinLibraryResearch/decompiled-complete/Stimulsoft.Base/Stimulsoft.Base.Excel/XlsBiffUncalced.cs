namespace Stimulsoft.Base.Excel;

internal class XlsBiffUncalced : XlsBiffRecord
{
	internal XlsBiffUncalced(byte[] bytes, uint offset)
		: base(bytes, offset)
	{
	}

	internal XlsBiffUncalced(byte[] bytes)
		: this(bytes, 0u)
	{
	}
}
