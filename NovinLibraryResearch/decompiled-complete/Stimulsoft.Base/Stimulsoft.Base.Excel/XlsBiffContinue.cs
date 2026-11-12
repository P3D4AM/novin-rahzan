namespace Stimulsoft.Base.Excel;

internal class XlsBiffContinue : XlsBiffRecord
{
	internal XlsBiffContinue(byte[] bytes, uint offset)
		: base(bytes, offset)
	{
	}

	internal XlsBiffContinue(byte[] bytes)
		: this(bytes, 0u)
	{
	}
}
