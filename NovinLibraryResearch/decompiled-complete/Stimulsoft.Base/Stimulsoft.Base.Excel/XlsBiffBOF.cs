namespace Stimulsoft.Base.Excel;

internal class XlsBiffBOF : XlsBiffRecord
{
	public ushort Version => ReadUInt16(0);

	public BIFFTYPE Type => (BIFFTYPE)ReadUInt16(2);

	public ushort CreationID
	{
		get
		{
			if (base.RecordSize < 6)
			{
				return 0;
			}
			return ReadUInt16(4);
		}
	}

	public ushort CreationYear
	{
		get
		{
			if (base.RecordSize < 8)
			{
				return 0;
			}
			return ReadUInt16(6);
		}
	}

	public uint HistoryFlag
	{
		get
		{
			if (base.RecordSize < 12)
			{
				return 0u;
			}
			return ReadUInt32(8);
		}
	}

	public uint MinVersionToOpen
	{
		get
		{
			if (base.RecordSize < 16)
			{
				return 0u;
			}
			return ReadUInt32(12);
		}
	}

	internal XlsBiffBOF(byte[] bytes)
		: this(bytes, 0u)
	{
	}

	internal XlsBiffBOF(byte[] bytes, uint offset)
		: base(bytes, offset)
	{
	}
}
