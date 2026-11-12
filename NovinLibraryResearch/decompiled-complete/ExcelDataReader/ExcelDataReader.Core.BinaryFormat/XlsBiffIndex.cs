using System.Collections.Generic;

namespace ExcelDataReader.Core.BinaryFormat;

internal class XlsBiffIndex : XlsBiffRecord
{
	public bool IsV8 { get; }

	public uint FirstExistingRow
	{
		get
		{
			if (!IsV8)
			{
				return ReadUInt16(4);
			}
			return ReadUInt32(4);
		}
	}

	public uint LastExistingRow
	{
		get
		{
			if (!IsV8)
			{
				return ReadUInt16(6);
			}
			return ReadUInt32(8);
		}
	}

	public uint[] DbCellAddresses
	{
		get
		{
			int recordSize = base.RecordSize;
			int num = (IsV8 ? 16 : 12);
			if (recordSize <= num)
			{
				return new uint[0];
			}
			List<uint> list = new List<uint>((recordSize - num) / 4);
			for (int i = num; i < recordSize; i += 4)
			{
				list.Add(ReadUInt32(i));
			}
			return list.ToArray();
		}
	}

	internal XlsBiffIndex(byte[] bytes, uint offset, bool isV8)
		: base(bytes, offset)
	{
		IsV8 = isV8;
	}
}
