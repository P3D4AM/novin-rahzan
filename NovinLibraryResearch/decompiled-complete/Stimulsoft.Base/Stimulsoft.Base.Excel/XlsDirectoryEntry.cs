using System;
using System.Text;

namespace Stimulsoft.Base.Excel;

internal class XlsDirectoryEntry
{
	public const int Length = 128;

	private readonly byte[] m_bytes;

	private XlsDirectoryEntry m_child;

	private XlsDirectoryEntry m_leftSibling;

	private XlsDirectoryEntry m_rightSibling;

	private XlsHeader m_header;

	public string EntryName => Encoding.Unicode.GetString(m_bytes, 0, EntryLength).TrimEnd(new char[1]);

	public ushort EntryLength => BitConverter.ToUInt16(m_bytes, 64);

	public STGTY EntryType => (STGTY)Buffer.GetByte(m_bytes, 66);

	public DECOLOR EntryColor => (DECOLOR)Buffer.GetByte(m_bytes, 67);

	public uint LeftSiblingSid => BitConverter.ToUInt32(m_bytes, 68);

	public XlsDirectoryEntry LeftSibling
	{
		get
		{
			return m_leftSibling;
		}
		set
		{
			if (m_leftSibling == null)
			{
				m_leftSibling = value;
			}
		}
	}

	public uint RightSiblingSid => BitConverter.ToUInt32(m_bytes, 72);

	public XlsDirectoryEntry RightSibling
	{
		get
		{
			return m_rightSibling;
		}
		set
		{
			if (m_rightSibling == null)
			{
				m_rightSibling = value;
			}
		}
	}

	public uint ChildSid => BitConverter.ToUInt32(m_bytes, 76);

	public XlsDirectoryEntry Child
	{
		get
		{
			return m_child;
		}
		set
		{
			if (m_child == null)
			{
				m_child = value;
			}
		}
	}

	public Guid ClassId
	{
		get
		{
			byte[] array = new byte[16];
			Buffer.BlockCopy(m_bytes, 80, array, 0, 16);
			return new Guid(array);
		}
	}

	public uint UserFlags => BitConverter.ToUInt32(m_bytes, 96);

	public DateTime CreationTime => DateTime.FromFileTime(BitConverter.ToInt64(m_bytes, 100));

	public DateTime LastWriteTime => DateTime.FromFileTime(BitConverter.ToInt64(m_bytes, 108));

	public uint StreamFirstSector => BitConverter.ToUInt32(m_bytes, 116);

	public uint StreamSize => BitConverter.ToUInt32(m_bytes, 120);

	public bool IsEntryMiniStream => StreamSize < m_header.MiniStreamCutoff;

	public uint PropType => BitConverter.ToUInt32(m_bytes, 124);

	public XlsDirectoryEntry(byte[] bytes, XlsHeader header)
	{
		if (bytes.Length < 128)
		{
			throw new BiffRecordException("Directory Entry error: Array is too small.");
		}
		m_bytes = bytes;
		m_header = header;
	}
}
