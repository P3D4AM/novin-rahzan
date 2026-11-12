using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Stimulsoft.Base.Excel;

internal class XlsRootDirectory
{
	private readonly List<XlsDirectoryEntry> m_entries;

	private readonly XlsDirectoryEntry m_root;

	public ReadOnlyCollection<XlsDirectoryEntry> Entries => m_entries.AsReadOnly();

	public XlsDirectoryEntry RootEntry => m_root;

	public XlsRootDirectory(XlsHeader hdr)
	{
		XlsStream xlsStream = new XlsStream(hdr, hdr.RootDirectoryEntryStart, isMini: false, null);
		byte[] array = xlsStream.ReadStream();
		List<XlsDirectoryEntry> list = new List<XlsDirectoryEntry>();
		for (int i = 0; i < array.Length; i += 128)
		{
			byte[] array2 = new byte[128];
			Buffer.BlockCopy(array, i, array2, 0, array2.Length);
			list.Add(new XlsDirectoryEntry(array2, hdr));
		}
		m_entries = list;
		for (int j = 0; j < list.Count; j++)
		{
			XlsDirectoryEntry xlsDirectoryEntry = list[j];
			if (m_root == null && xlsDirectoryEntry.EntryType == STGTY.STGTY_ROOT)
			{
				m_root = xlsDirectoryEntry;
			}
			if (xlsDirectoryEntry.ChildSid != uint.MaxValue)
			{
				xlsDirectoryEntry.Child = list[(int)xlsDirectoryEntry.ChildSid];
			}
			if (xlsDirectoryEntry.LeftSiblingSid != uint.MaxValue)
			{
				xlsDirectoryEntry.LeftSibling = list[(int)xlsDirectoryEntry.LeftSiblingSid];
			}
			if (xlsDirectoryEntry.RightSiblingSid != uint.MaxValue)
			{
				xlsDirectoryEntry.RightSibling = list[(int)xlsDirectoryEntry.RightSiblingSid];
			}
		}
		xlsStream.CalculateMiniFat(this);
	}

	public XlsDirectoryEntry FindEntry(string EntryName)
	{
		foreach (XlsDirectoryEntry entry in m_entries)
		{
			if (entry.EntryName == EntryName)
			{
				return entry;
			}
		}
		return null;
	}
}
