namespace ExcelDataReader.Core.BinaryFormat;

internal class XlsBiffXF : XlsBiffRecord
{
	public int Format { get; }

	internal XlsBiffXF(byte[] bytes, uint offset)
		: base(bytes, offset)
	{
		switch (base.Id)
		{
		case BIFFRECORDTYPE.XF_V2:
			Format = ReadByte(2) & 0x3F;
			break;
		case BIFFRECORDTYPE.XF_V3:
			Format = ReadByte(1);
			break;
		case BIFFRECORDTYPE.XF_V4:
			Format = ReadByte(1);
			break;
		default:
			Format = ReadUInt16(2);
			break;
		}
	}
}
