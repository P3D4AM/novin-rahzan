namespace ExcelDataReader.Core.BinaryFormat;

internal class XlsBiffLabelSSTCell : XlsBiffBlankCell
{
	public uint SSTIndex => ReadUInt32(6);

	internal XlsBiffLabelSSTCell(byte[] bytes, uint offset, int biffVersion)
		: base(bytes, offset, biffVersion)
	{
	}
}
