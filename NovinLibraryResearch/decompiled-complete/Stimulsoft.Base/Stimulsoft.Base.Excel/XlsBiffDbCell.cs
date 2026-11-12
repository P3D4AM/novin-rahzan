using System.Collections.Generic;

namespace Stimulsoft.Base.Excel;

internal class XlsBiffDbCell : XlsBiffRecord
{
	public int RowAddress => base.Offset - ReadInt32(0);

	public uint[] CellAddresses
	{
		get
		{
			int num = RowAddress - 20;
			List<uint> list = new List<uint>();
			for (int i = 4; i < base.RecordSize; i += 4)
			{
				list.Add((uint)(num + ReadUInt16(i)));
			}
			return list.ToArray();
		}
	}

	internal XlsBiffDbCell(byte[] bytes, uint offset)
		: base(bytes, offset)
	{
	}

	internal XlsBiffDbCell(byte[] bytes)
		: this(bytes, 0u)
	{
	}
}
