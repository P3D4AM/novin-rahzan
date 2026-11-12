using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ExcelDataReader.Exceptions;

namespace ExcelDataReader.Core.CompoundFormat;

internal class CompoundDocument
{
	internal CompoundHeader Header { get; }

	internal List<uint> SectorTable { get; }

	internal List<uint> MiniSectorTable { get; }

	internal CompoundDirectoryEntry RootEntry { get; set; }

	internal List<CompoundDirectoryEntry> Entries { get; set; }

	public CompoundDocument(Stream stream)
	{
		BinaryReader reader = new BinaryReader(stream);
		Header = ReadHeader(reader);
		if (!Header.IsSignatureValid)
		{
			throw new HeaderException("Invalid file signature.");
		}
		if (Header.ByteOrder != 65534 && Header.ByteOrder != ushort.MaxValue)
		{
			throw new HeaderException("Invalid byte order specified in header.");
		}
		List<uint> chain = ReadDifSectorChain(reader);
		SectorTable = ReadSectorTable(reader, chain);
		List<uint> sectorChain = GetSectorChain(Header.MiniFatFirstSector, SectorTable);
		MiniSectorTable = ReadSectorTable(reader, sectorChain);
		List<uint> sectorChain2 = GetSectorChain(Header.RootDirectoryEntryStart, SectorTable);
		byte[] bytes = ReadStream(stream, sectorChain2, sectorChain2.Count * Header.SectorSize);
		ReadDirectoryEntries(bytes);
	}

	internal static bool IsCompoundDocument(byte[] probe)
	{
		return BitConverter.ToUInt64(probe, 0) == 16220472316735377360uL;
	}

	internal CompoundDirectoryEntry FindEntry(string entryName)
	{
		foreach (CompoundDirectoryEntry entry in Entries)
		{
			if (string.Equals(entry.EntryName, entryName, StringComparison.CurrentCultureIgnoreCase))
			{
				return entry;
			}
		}
		return null;
	}

	internal long GetMiniSectorOffset(uint sector)
	{
		return Header.MiniSectorSize * sector;
	}

	internal long GetSectorOffset(uint sector)
	{
		return 512 + Header.SectorSize * sector;
	}

	internal List<uint> GetSectorChain(uint sector, List<uint> sectorTable)
	{
		List<uint> list = new List<uint>();
		while (sector != 4294967294u)
		{
			list.Add(sector);
			sector = GetNextSector(sector, sectorTable);
			if (list.Contains(sector))
			{
				throw new CompoundDocumentException("Cyclic sector chain in compound document.");
			}
		}
		TrimSectorChain(list, FATMARKERS.FAT_FreeSpace);
		return list;
	}

	internal byte[] ReadStream(Stream stream, uint baseSector, int length, bool isMini)
	{
		using CompoundStream compoundStream = new CompoundStream(this, stream, baseSector, length, isMini, leaveOpen: true);
		byte[] array = new byte[length];
		compoundStream.Read(array, 0, length);
		return array;
	}

	internal byte[] ReadStream(Stream stream, List<uint> sectors, int length)
	{
		using CompoundStream compoundStream = new CompoundStream(this, stream, sectors, length, leaveOpen: true);
		byte[] array = new byte[length];
		compoundStream.Read(array, 0, length);
		return array;
	}

	private void ReadDirectoryEntries(byte[] bytes)
	{
		try
		{
			Entries = new List<CompoundDirectoryEntry>();
			using MemoryStream memoryStream = new MemoryStream(bytes);
			using BinaryReader reader = new BinaryReader(memoryStream);
			RootEntry = ReadDirectoryEntry(reader);
			Entries.Add(RootEntry);
			while (memoryStream.Position < memoryStream.Length)
			{
				CompoundDirectoryEntry item = ReadDirectoryEntry(reader);
				Entries.Add(item);
			}
		}
		catch (EndOfStreamException inner)
		{
			throw new CompoundDocumentException("The excel file may be corrupt or truncated. We've read past the end of the file.", inner);
		}
	}

	private CompoundDirectoryEntry ReadDirectoryEntry(BinaryReader reader)
	{
		CompoundDirectoryEntry compoundDirectoryEntry = new CompoundDirectoryEntry();
		byte[] bytes = reader.ReadBytes(64);
		ushort num = reader.ReadUInt16();
		if (num > 0)
		{
			num = Math.Min((ushort)64, num);
			compoundDirectoryEntry.EntryName = Encoding.Unicode.GetString(bytes, 0, num).TrimEnd(new char[1]);
		}
		compoundDirectoryEntry.EntryType = (STGTY)reader.ReadByte();
		compoundDirectoryEntry.EntryColor = (DECOLOR)reader.ReadByte();
		compoundDirectoryEntry.LeftSiblingSid = reader.ReadUInt32();
		compoundDirectoryEntry.RightSiblingSid = reader.ReadUInt32();
		compoundDirectoryEntry.ChildSid = reader.ReadUInt32();
		compoundDirectoryEntry.ClassId = new Guid(reader.ReadBytes(16));
		compoundDirectoryEntry.UserFlags = reader.ReadUInt32();
		compoundDirectoryEntry.CreationTime = ReadFileTime(reader);
		compoundDirectoryEntry.LastWriteTime = ReadFileTime(reader);
		compoundDirectoryEntry.StreamFirstSector = reader.ReadUInt32();
		compoundDirectoryEntry.StreamSize = reader.ReadUInt32();
		compoundDirectoryEntry.PropType = reader.ReadUInt32();
		compoundDirectoryEntry.IsEntryMiniStream = compoundDirectoryEntry.StreamSize < Header.MiniStreamCutoff;
		return compoundDirectoryEntry;
	}

	private DateTime ReadFileTime(BinaryReader reader)
	{
		long num = reader.ReadInt64();
		if (num >= 0)
		{
			long num2 = num;
			DateTime maxValue = DateTime.MaxValue;
			if (num2 <= maxValue.ToFileTime())
			{
				goto IL_001f;
			}
		}
		num = 0L;
		goto IL_001f;
		IL_001f:
		return DateTime.FromFileTime(num);
	}

	private CompoundHeader ReadHeader(BinaryReader reader)
	{
		CompoundHeader compoundHeader = new CompoundHeader();
		compoundHeader.Signature = reader.ReadUInt64();
		compoundHeader.ClassId = new Guid(reader.ReadBytes(16));
		compoundHeader.Version = reader.ReadUInt16();
		compoundHeader.DllVersion = reader.ReadUInt16();
		compoundHeader.ByteOrder = reader.ReadUInt16();
		compoundHeader.SectorSizeInPot = reader.ReadUInt16();
		compoundHeader.MiniSectorSizeInPot = reader.ReadUInt16();
		reader.ReadBytes(6);
		compoundHeader.DirectorySectorCount = reader.ReadInt32();
		compoundHeader.FatSectorCount = reader.ReadInt32();
		compoundHeader.RootDirectoryEntryStart = reader.ReadUInt32();
		compoundHeader.TransactionSignature = reader.ReadUInt32();
		compoundHeader.MiniStreamCutoff = reader.ReadUInt32();
		compoundHeader.MiniFatFirstSector = reader.ReadUInt32();
		compoundHeader.MiniFatSectorCount = reader.ReadInt32();
		compoundHeader.DifFirstSector = reader.ReadUInt32();
		compoundHeader.DifSectorCount = reader.ReadInt32();
		List<uint> list = new List<uint>();
		for (int i = 0; i < 109; i++)
		{
			list.Add(reader.ReadUInt32());
		}
		compoundHeader.First109DifSectorChain = list;
		return compoundHeader;
	}

	private List<uint> ReadDifSectorChain(BinaryReader reader)
	{
		List<uint> list = new List<uint>(Header.First109DifSectorChain);
		if (Header.DifFirstSector != 4294967294u)
		{
			try
			{
				uint sector = Header.DifFirstSector;
				for (int i = 0; i < Header.DifSectorCount; i++)
				{
					List<uint> list2 = ReadSectorAsUInt32s(reader, sector);
					list.AddRange(list2.GetRange(0, list2.Count - 1));
					sector = list2[list2.Count - 1];
				}
			}
			catch (EndOfStreamException inner)
			{
				throw new CompoundDocumentException("The excel file may be corrupt or truncated. We've read past the end of the file.", inner);
			}
		}
		TrimSectorChain(list, FATMARKERS.FAT_FreeSpace);
		TrimSectorChain(list, FATMARKERS.FAT_EndOfChain);
		return list;
	}

	private List<uint> ReadSectorTable(BinaryReader reader, List<uint> chain)
	{
		List<uint> list = new List<uint>(Header.SectorSize / 4 * chain.Count);
		try
		{
			foreach (uint item in chain)
			{
				List<uint> collection = ReadSectorAsUInt32s(reader, item);
				list.AddRange(collection);
			}
		}
		catch (EndOfStreamException inner)
		{
			throw new CompoundDocumentException("The excel file may be corrupt or truncated. We've read past the end of the file.", inner);
		}
		TrimSectorChain(list, FATMARKERS.FAT_FreeSpace);
		return list;
	}

	private List<uint> ReadSectorAsUInt32s(BinaryReader reader, uint sector)
	{
		List<uint> list = new List<uint>(Header.SectorSize / 4);
		reader.BaseStream.Seek(GetSectorOffset(sector), SeekOrigin.Begin);
		for (int i = 0; i < Header.SectorSize / 4; i++)
		{
			uint item = reader.ReadUInt32();
			list.Add(item);
		}
		return list;
	}

	private void TrimSectorChain(List<uint> chain, FATMARKERS marker)
	{
		while (chain.Count > 0 && chain[chain.Count - 1] == (uint)marker)
		{
			chain.RemoveAt(chain.Count - 1);
		}
	}

	private uint GetNextSector(uint sector, List<uint> sectorTable)
	{
		if (sector < sectorTable.Count)
		{
			return sectorTable[(int)sector];
		}
		return 4294967294u;
	}
}
