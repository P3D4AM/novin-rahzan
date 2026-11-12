using System;
using System.IO;
using System.Text;
using SharpCompress.IO;

namespace SharpCompress.Common.Rar.Headers;

internal class FileHeader : RarHeader
{
	private uint _fileCrc;

	private bool isEncryptedRar5;

	private ushort Flags { get; set; }

	internal uint FileCrc
	{
		get
		{
			if (base.IsRar5 && !HasFlag(4))
			{
				throw new InvalidOperationException("TODO rar5");
			}
			return _fileCrc;
		}
		private set
		{
			_fileCrc = value;
		}
	}

	internal byte CompressionMethod { get; private set; }

	internal bool IsStored => CompressionMethod == 0;

	internal byte CompressionAlgorithm { get; private set; }

	public bool IsSolid { get; private set; }

	internal uint WindowSize { get; private set; }

	internal byte[] R4Salt { get; private set; }

	private byte HostOs { get; set; }

	internal uint FileAttributes { get; private set; }

	internal long CompressedSize { get; private set; }

	internal long UncompressedSize { get; private set; }

	internal string FileName { get; private set; }

	internal byte[] SubData { get; private set; }

	internal int RecoverySectors { get; private set; }

	internal long DataStartPosition { get; set; }

	public Stream PackedStream { get; set; }

	public bool IsSplitBefore
	{
		get
		{
			if (!base.IsRar5)
			{
				return HasFlag(1);
			}
			return HasHeaderFlag(8);
		}
	}

	public bool IsSplitAfter
	{
		get
		{
			if (!base.IsRar5)
			{
				return HasFlag(2);
			}
			return HasHeaderFlag(16);
		}
	}

	public bool IsDirectory => HasFlag((ushort)(base.IsRar5 ? 1 : 224));

	public bool IsEncrypted
	{
		get
		{
			if (!base.IsRar5)
			{
				return HasFlag(4);
			}
			return isEncryptedRar5;
		}
	}

	internal DateTime? FileLastModifiedTime { get; private set; }

	internal DateTime? FileCreatedTime { get; private set; }

	internal DateTime? FileLastAccessedTime { get; private set; }

	internal DateTime? FileArchivedTime { get; private set; }

	public FileHeader(RarHeader header, RarCrcBinaryReader reader, HeaderType headerType)
		: base(header, reader, headerType)
	{
	}

	protected override void ReadFinish(MarkingBinaryReader reader)
	{
		if (base.IsRar5)
		{
			ReadFromReaderV5(reader);
		}
		else
		{
			ReadFromReaderV4(reader);
		}
	}

	private void ReadFromReaderV5(MarkingBinaryReader reader)
	{
		Flags = reader.ReadRarVIntUInt16();
		long num = checked((long)reader.ReadRarVInt());
		UncompressedSize = (HasFlag(8) ? long.MaxValue : num);
		FileAttributes = reader.ReadRarVIntUInt32();
		if (HasFlag(2))
		{
			FileLastModifiedTime = Utility.UnixTimeToDateTime(reader.ReadUInt32());
		}
		if (HasFlag(4))
		{
			FileCrc = reader.ReadUInt32();
		}
		ushort num2 = reader.ReadRarVIntUInt16();
		CompressionAlgorithm = (byte)((num2 & 0x3F) + 50);
		IsSolid = (num2 & 0x40) == 64;
		CompressionMethod = (byte)((uint)(num2 >> 7) & 7u);
		WindowSize = ((!IsDirectory) ? ((uint)(131072 << ((num2 >> 10) & 0xF))) : 0u);
		HostOs = reader.ReadRarVIntByte();
		ushort count = reader.ReadRarVIntUInt16();
		byte[] array = reader.ReadBytes(count);
		FileName = ConvertPathV5(Encoding.UTF8.GetString(array, 0, array.Length));
		if (base.ExtraSize != RemainingHeaderBytes(reader))
		{
			throw new InvalidFormatException("rar5 header size / extra size inconsistency");
		}
		isEncryptedRar5 = false;
		while (RemainingHeaderBytes(reader) > 0)
		{
			ushort num3 = reader.ReadRarVIntUInt16();
			int num4 = RemainingHeaderBytes(reader);
			switch (reader.ReadRarVIntUInt16())
			{
			case 1:
				isEncryptedRar5 = true;
				break;
			case 3:
			{
				ushort num5 = reader.ReadRarVIntUInt16();
				bool isWindowsTime = (num5 & 1) == 0;
				if ((num5 & 2) == 2)
				{
					FileLastModifiedTime = ReadExtendedTimeV5(reader, isWindowsTime);
				}
				if ((num5 & 4) == 4)
				{
					FileCreatedTime = ReadExtendedTimeV5(reader, isWindowsTime);
				}
				if ((num5 & 8) == 8)
				{
					FileLastAccessedTime = ReadExtendedTimeV5(reader, isWindowsTime);
				}
				break;
			}
			}
			int num6 = num4 - RemainingHeaderBytes(reader);
			int num7 = num3 - num6;
			if (num7 > 0)
			{
				reader.ReadBytes(num7);
			}
		}
		if (base.AdditionalDataSize != 0L)
		{
			CompressedSize = base.AdditionalDataSize;
		}
	}

	private static DateTime ReadExtendedTimeV5(MarkingBinaryReader reader, bool isWindowsTime)
	{
		if (isWindowsTime)
		{
			return DateTime.FromFileTime(reader.ReadInt64());
		}
		return Utility.UnixTimeToDateTime(reader.ReadUInt32());
	}

	private static string ConvertPathV5(string path)
	{
		if (Path.DirectorySeparatorChar == '\\')
		{
			return path.Replace('\\', '-').Replace('/', '\\');
		}
		return path;
	}

	private void ReadFromReaderV4(MarkingBinaryReader reader)
	{
		Flags = base.HeaderFlags;
		IsSolid = HasFlag(16);
		WindowSize = ((!IsDirectory) ? ((uint)(65536 << ((Flags & 0xE0) >> 5))) : 0u);
		uint num = reader.ReadUInt32();
		HostOs = reader.ReadByte();
		FileCrc = reader.ReadUInt32();
		FileLastModifiedTime = Utility.DosDateToDateTime(reader.ReadUInt32());
		CompressionAlgorithm = reader.ReadByte();
		CompressionMethod = (byte)(reader.ReadByte() - 48);
		short num2 = reader.ReadInt16();
		FileAttributes = reader.ReadUInt32();
		uint x = 0u;
		uint x2 = 0u;
		if (HasFlag(256))
		{
			x = reader.ReadUInt32();
			x2 = reader.ReadUInt32();
		}
		else if (num == uint.MaxValue)
		{
			num = uint.MaxValue;
			x2 = 2147483647u;
		}
		CompressedSize = UInt32To64(x, checked((uint)base.AdditionalDataSize));
		UncompressedSize = UInt32To64(x2, num);
		num2 = (short)((num2 > 4096) ? 4096 : num2);
		byte[] array = reader.ReadBytes(num2);
		switch (base.HeaderCode)
		{
		case 116:
			if (HasFlag(512))
			{
				int i;
				for (i = 0; i < array.Length && array[i] != 0; i++)
				{
				}
				if (i != num2)
				{
					i++;
					FileName = FileNameDecoder.Decode(array, i);
				}
				else
				{
					FileName = base.ArchiveEncoding.Decode(array);
				}
			}
			else
			{
				FileName = base.ArchiveEncoding.Decode(array);
			}
			FileName = ConvertPathV4(FileName);
			break;
		case 122:
		{
			int num3 = base.HeaderSize - 32 - num2;
			if (HasFlag(1024))
			{
				num3 -= 8;
			}
			if (num3 > 0)
			{
				SubData = reader.ReadBytes(num3);
			}
			if (NewSubHeaderType.SUBHEAD_TYPE_RR.Equals(array))
			{
				RecoverySectors = SubData[8] + (SubData[9] << 8) + (SubData[10] << 16) + (SubData[11] << 24);
			}
			break;
		}
		}
		if (HasFlag(1024))
		{
			R4Salt = reader.ReadBytes(8);
		}
		if (HasFlag(4096) && RemainingHeaderBytes(reader) >= 2)
		{
			ushort extendedFlags = reader.ReadUInt16();
			FileLastModifiedTime = ProcessExtendedTimeV4(extendedFlags, FileLastModifiedTime, reader, 0);
			FileCreatedTime = ProcessExtendedTimeV4(extendedFlags, null, reader, 1);
			FileLastAccessedTime = ProcessExtendedTimeV4(extendedFlags, null, reader, 2);
			FileArchivedTime = ProcessExtendedTimeV4(extendedFlags, null, reader, 3);
		}
	}

	private static long UInt32To64(uint x, uint y)
	{
		return (long)(((ulong)x << 32) + y);
	}

	private static DateTime? ProcessExtendedTimeV4(ushort extendedFlags, DateTime? time, MarkingBinaryReader reader, int i)
	{
		uint num = (uint)extendedFlags >> (3 - i) * 4;
		if ((num & 8) == 0)
		{
			return null;
		}
		if (i != 0)
		{
			uint iTime = reader.ReadUInt32();
			time = Utility.DosDateToDateTime(iTime);
		}
		if ((num & 4) == 0)
		{
			time = time.Value.AddSeconds(1.0);
		}
		uint num2 = 0u;
		int num3 = (int)(num & 3);
		for (int j = 0; j < num3; j++)
		{
			byte b = reader.ReadByte();
			num2 |= (uint)(b << (j + 3 - num3) * 8);
		}
		return time.Value.AddMilliseconds((double)num2 * Math.Pow(10.0, -4.0));
	}

	private static string ConvertPathV4(string path)
	{
		if (Path.DirectorySeparatorChar == '/')
		{
			return path.Replace('\\', '/');
		}
		if (Path.DirectorySeparatorChar == '\\')
		{
			return path.Replace('/', '\\');
		}
		return path;
	}

	public override string ToString()
	{
		return FileName;
	}

	private bool HasFlag(ushort flag)
	{
		return (Flags & flag) == flag;
	}
}
