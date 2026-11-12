namespace ExcelDataReader.Core.BinaryFormat;

internal class XlsBiffIntegerCell : XlsBiffBlankCell
{
	public int Value { get; }

	internal XlsBiffIntegerCell(byte[] bytes, uint offset, int biffVersion)
		: base(bytes, offset, biffVersion)
	{
		if (base.Id == BIFFRECORDTYPE.INTEGER_OLD)
		{
			Value = ReadUInt16(7);
		}
		else
		{
			Value = ReadUInt16(6);
		}
	}
}
