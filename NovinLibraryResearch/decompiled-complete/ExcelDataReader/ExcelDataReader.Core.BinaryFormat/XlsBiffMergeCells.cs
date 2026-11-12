using System.Collections.Generic;

namespace ExcelDataReader.Core.BinaryFormat;

internal class XlsBiffMergeCells : XlsBiffRecord
{
	public List<CellRange> MergeCells { get; }

	public XlsBiffMergeCells(byte[] bytes, uint offset)
		: base(bytes, offset)
	{
		ushort num = ReadUInt16(0);
		MergeCells = new List<CellRange>();
		for (int i = 0; i < num; i++)
		{
			short fromRow = ReadInt16(2 + i * 8);
			short toRow = ReadInt16(2 + i * 8 + 2);
			short fromColumn = ReadInt16(2 + i * 8 + 4);
			short toColumn = ReadInt16(2 + i * 8 + 6);
			CellRange item = new CellRange(fromColumn, fromRow, toColumn, toRow);
			MergeCells.Add(item);
		}
	}
}
