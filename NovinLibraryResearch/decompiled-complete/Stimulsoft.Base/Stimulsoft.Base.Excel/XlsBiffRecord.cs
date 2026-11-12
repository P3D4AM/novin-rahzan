using System;

namespace Stimulsoft.Base.Excel;

internal class XlsBiffRecord
{
	protected byte[] m_bytes;

	protected int m_readoffset;

	internal byte[] Bytes => m_bytes;

	internal int Offset => m_readoffset - 4;

	public BIFFRECORDTYPE ID => (BIFFRECORDTYPE)BitConverter.ToUInt16(m_bytes, m_readoffset - 4);

	public ushort RecordSize => BitConverter.ToUInt16(m_bytes, m_readoffset - 2);

	public int Size => 4 + RecordSize;

	public bool IsCell
	{
		get
		{
			bool result = false;
			switch (ID)
			{
			case BIFFRECORDTYPE.MULRK:
			case BIFFRECORDTYPE.MULBLANK:
			case BIFFRECORDTYPE.LABELSST:
			case BIFFRECORDTYPE.BLANK:
			case BIFFRECORDTYPE.NUMBER:
			case BIFFRECORDTYPE.BOOLERR:
			case BIFFRECORDTYPE.RK:
			case BIFFRECORDTYPE.FORMULA:
				result = true;
				break;
			}
			return result;
		}
	}

	protected XlsBiffRecord(byte[] bytes)
		: this(bytes, 0u)
	{
	}

	protected XlsBiffRecord(byte[] bytes, uint offset)
	{
		if (bytes.Length - offset < 4)
		{
			throw new ArgumentException("Error: Buffer size is less than minimum BIFF record size.");
		}
		m_bytes = bytes;
		m_readoffset = (int)(4 + offset);
		if (bytes.Length < offset + Size)
		{
			throw new ArgumentException("BIFF Stream error: Buffer size is less than entry length.");
		}
	}

	public static XlsBiffRecord GetRecord(byte[] bytes, uint offset)
	{
		uint num = BitConverter.ToUInt16(bytes, (int)offset);
		switch ((BIFFRECORDTYPE)(ushort)num)
		{
		case BIFFRECORDTYPE.BOF_V2:
		case BIFFRECORDTYPE.BOF_V3:
		case BIFFRECORDTYPE.BOF_V4:
		case BIFFRECORDTYPE.BOF:
			return new XlsBiffBOF(bytes, offset);
		case BIFFRECORDTYPE.EOF:
			return new XlsBiffEOF(bytes, offset);
		case BIFFRECORDTYPE.INTERFACEHDR:
			return new XlsBiffInterfaceHdr(bytes, offset);
		case BIFFRECORDTYPE.SST:
			return new XlsBiffSST(bytes, offset);
		case BIFFRECORDTYPE.INDEX:
			return new XlsBiffIndex(bytes, offset);
		case BIFFRECORDTYPE.ROW:
			return new XlsBiffRow(bytes, offset);
		case BIFFRECORDTYPE.DBCELL:
			return new XlsBiffDbCell(bytes, offset);
		case BIFFRECORDTYPE.BLANK_OLD:
		case BIFFRECORDTYPE.BOOLERR_OLD:
		case BIFFRECORDTYPE.BLANK:
		case BIFFRECORDTYPE.BOOLERR:
			return new XlsBiffBlankCell(bytes, offset);
		case BIFFRECORDTYPE.MULBLANK:
			return new XlsBiffMulBlankCell(bytes, offset);
		case BIFFRECORDTYPE.LABEL_OLD:
		case BIFFRECORDTYPE.RSTRING:
		case BIFFRECORDTYPE.LABEL:
			return new XlsBiffLabelCell(bytes, offset);
		case BIFFRECORDTYPE.LABELSST:
			return new XlsBiffLabelSSTCell(bytes, offset);
		case BIFFRECORDTYPE.INTEGER_OLD:
		case BIFFRECORDTYPE.INTEGER:
			return new XlsBiffIntegerCell(bytes, offset);
		case BIFFRECORDTYPE.NUMBER_OLD:
		case BIFFRECORDTYPE.NUMBER:
			return new XlsBiffNumberCell(bytes, offset);
		case BIFFRECORDTYPE.RK:
			return new XlsBiffRKCell(bytes, offset);
		case BIFFRECORDTYPE.MULRK:
			return new XlsBiffMulRKCell(bytes, offset);
		case BIFFRECORDTYPE.FORMULA_OLD:
		case BIFFRECORDTYPE.FORMULA:
			return new XlsBiffFormulaCell(bytes, offset);
		case BIFFRECORDTYPE.FORMAT_V23:
		case BIFFRECORDTYPE.FORMAT:
			return new XlsBiffFormatString(bytes, offset);
		case BIFFRECORDTYPE.STRING:
			return new XlsBiffFormulaString(bytes, offset);
		case BIFFRECORDTYPE.CONTINUE:
			return new XlsBiffContinue(bytes, offset);
		case BIFFRECORDTYPE.DIMENSIONS:
			return new XlsBiffDimensions(bytes, offset);
		case BIFFRECORDTYPE.BOUNDSHEET:
			return new XlsBiffBoundSheet(bytes, offset);
		case BIFFRECORDTYPE.WINDOW1:
			return new XlsBiffWindow1(bytes, offset);
		case BIFFRECORDTYPE.CODEPAGE:
			return new XlsBiffSimpleValueRecord(bytes, offset);
		case BIFFRECORDTYPE.FNGROUPCOUNT:
			return new XlsBiffSimpleValueRecord(bytes, offset);
		case BIFFRECORDTYPE.RECORD1904:
			return new XlsBiffSimpleValueRecord(bytes, offset);
		case BIFFRECORDTYPE.BOOKBOOL:
			return new XlsBiffSimpleValueRecord(bytes, offset);
		case BIFFRECORDTYPE.BACKUP:
			return new XlsBiffSimpleValueRecord(bytes, offset);
		case BIFFRECORDTYPE.HIDEOBJ:
			return new XlsBiffSimpleValueRecord(bytes, offset);
		case BIFFRECORDTYPE.USESELFS:
			return new XlsBiffSimpleValueRecord(bytes, offset);
		case BIFFRECORDTYPE.UNCALCED:
			return new XlsBiffUncalced(bytes, offset);
		default:
			return new XlsBiffRecord(bytes, offset);
		}
	}

	public byte ReadByte(int offset)
	{
		return Buffer.GetByte(m_bytes, m_readoffset + offset);
	}

	public ushort ReadUInt16(int offset)
	{
		return BitConverter.ToUInt16(m_bytes, m_readoffset + offset);
	}

	public uint ReadUInt32(int offset)
	{
		return BitConverter.ToUInt32(m_bytes, m_readoffset + offset);
	}

	public ulong ReadUInt64(int offset)
	{
		return BitConverter.ToUInt64(m_bytes, m_readoffset + offset);
	}

	public short ReadInt16(int offset)
	{
		return BitConverter.ToInt16(m_bytes, m_readoffset + offset);
	}

	public int ReadInt32(int offset)
	{
		return BitConverter.ToInt32(m_bytes, m_readoffset + offset);
	}

	public long ReadInt64(int offset)
	{
		return BitConverter.ToInt64(m_bytes, m_readoffset + offset);
	}

	public byte[] ReadArray(int offset, int size)
	{
		byte[] array = new byte[size];
		Buffer.BlockCopy(m_bytes, m_readoffset + offset, array, 0, size);
		return array;
	}

	public float ReadFloat(int offset)
	{
		return BitConverter.ToSingle(m_bytes, m_readoffset + offset);
	}

	public double ReadDouble(int offset)
	{
		return BitConverter.ToDouble(m_bytes, m_readoffset + offset);
	}
}
