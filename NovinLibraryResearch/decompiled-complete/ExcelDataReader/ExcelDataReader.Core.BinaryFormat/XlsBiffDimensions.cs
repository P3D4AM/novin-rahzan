namespace ExcelDataReader.Core.BinaryFormat;

internal class XlsBiffDimensions : XlsBiffRecord
{
	public uint FirstRow { get; }

	public uint LastRow { get; }

	public ushort FirstColumn { get; }

	public ushort LastColumn { get; }

	internal XlsBiffDimensions(byte[] bytes, uint offset, int biffVersion)
		: base(bytes, offset)
	{
		if (biffVersion < 8)
		{
			FirstRow = ReadUInt16(0);
			LastRow = ReadUInt16(2);
			FirstColumn = ReadUInt16(4);
			LastColumn = ReadUInt16(6);
		}
		else
		{
			FirstRow = ReadUInt32(0);
			LastRow = ReadUInt32(4);
			FirstColumn = ReadUInt16(8);
			LastColumn = ReadUInt16(10);
		}
	}
}
