namespace Stimulsoft.Base.Excel;

internal class XlsBiffLabelSSTCell : XlsBiffBlankCell
{
	public uint SSTIndex => ReadUInt32(6);

	internal XlsBiffLabelSSTCell(byte[] bytes)
		: this(bytes, 0u)
	{
	}

	internal XlsBiffLabelSSTCell(byte[] bytes, uint offset)
		: base(bytes, offset)
	{
	}

	public string Text(XlsBiffSST sst)
	{
		return sst.GetString(SSTIndex);
	}
}
