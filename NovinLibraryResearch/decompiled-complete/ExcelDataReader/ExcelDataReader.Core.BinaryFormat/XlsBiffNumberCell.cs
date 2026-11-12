namespace ExcelDataReader.Core.BinaryFormat;

internal class XlsBiffNumberCell : XlsBiffBlankCell
{
	public double Value { get; }

	internal XlsBiffNumberCell(byte[] bytes, uint offset, int biffVersion)
		: base(bytes, offset, biffVersion)
	{
		if (base.Id == BIFFRECORDTYPE.NUMBER_OLD)
		{
			Value = ReadDouble(7);
		}
		else
		{
			Value = ReadDouble(6);
		}
	}
}
