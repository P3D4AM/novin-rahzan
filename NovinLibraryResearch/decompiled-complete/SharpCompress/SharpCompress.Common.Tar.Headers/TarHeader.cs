using System;
using System.Buffers.Binary;
using System.IO;
using System.Text;

namespace SharpCompress.Common.Tar.Headers;

internal sealed class TarHeader
{
	internal static readonly DateTime EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

	internal const int BLOCK_SIZE = 512;

	private static readonly byte[] eightSpaces = new byte[8] { 32, 32, 32, 32, 32, 32, 32, 32 };

	internal string Name { get; set; }

	internal string LinkName { get; set; }

	internal long Size { get; set; }

	internal DateTime LastModifiedTime { get; set; }

	internal EntryType EntryType { get; set; }

	internal Stream PackedStream { get; set; }

	internal ArchiveEncoding ArchiveEncoding { get; }

	public long? DataStartPosition { get; set; }

	public string Magic { get; set; }

	public TarHeader(ArchiveEncoding archiveEncoding)
	{
		ArchiveEncoding = archiveEncoding;
	}

	internal void Write(Stream output)
	{
		byte[] array = new byte[512];
		WriteOctalBytes(511L, array, 100, 8);
		WriteOctalBytes(0L, array, 108, 8);
		WriteOctalBytes(0L, array, 116, 8);
		int byteCount = ArchiveEncoding.GetEncoding().GetByteCount(Name);
		if (byteCount > 100)
		{
			WriteStringBytes("././@LongLink", array, 0, 100);
			array[156] = 76;
			WriteOctalBytes(byteCount + 1, array, 124, 12);
		}
		else
		{
			WriteStringBytes(ArchiveEncoding.Encode(Name), array, 100);
			WriteOctalBytes(Size, array, 124, 12);
			WriteOctalBytes((long)(LastModifiedTime.ToUniversalTime() - EPOCH).TotalSeconds, array, 136, 12);
			array[156] = (byte)EntryType;
			if (Size >= 8589934591L)
			{
				Span<byte> span = stackalloc byte[12];
				BinaryPrimitives.WriteInt64BigEndian(span.Slice(4), Size);
				span[0] |= 128;
				span.CopyTo(MemoryExtensions.AsSpan(array, 124));
			}
		}
		WriteOctalBytes(RecalculateChecksum(array), array, 148, 8);
		output.Write(array, 0, array.Length);
		if (byteCount > 100)
		{
			WriteLongFilenameHeader(output);
			Name = ArchiveEncoding.Decode(ArchiveEncoding.Encode(Name), 0, 100 - ArchiveEncoding.GetEncoding().GetMaxByteCount(1));
			Write(output);
		}
	}

	private void WriteLongFilenameHeader(Stream output)
	{
		byte[] array = ArchiveEncoding.Encode(Name);
		output.Write(array, 0, array.Length);
		int num = 512 - array.Length % 512;
		if (num == 0)
		{
			num = 512;
		}
		Span<byte> span = stackalloc byte[num];
		output.Write(span);
	}

	internal bool Read(BinaryReader reader)
	{
		byte[] array = ReadBlock(reader);
		if (array.Length == 0)
		{
			return false;
		}
		if (ReadEntryType(array) == EntryType.SymLink)
		{
			LinkName = ArchiveEncoding.Decode(array, 157, 100).TrimNulls();
		}
		if (ReadEntryType(array) == EntryType.LongName)
		{
			Name = ReadLongName(reader, array);
			array = ReadBlock(reader);
		}
		else
		{
			Name = ArchiveEncoding.Decode(array, 0, 100).TrimNulls();
		}
		EntryType = ReadEntryType(array);
		Size = ReadSize(array);
		long num = ReadAsciiInt64Base8(array, 136, 11);
		DateTime ePOCH = EPOCH;
		LastModifiedTime = ePOCH.AddSeconds(num).ToLocalTime();
		Magic = ArchiveEncoding.Decode(array, 257, 6).TrimNulls();
		if (!string.IsNullOrEmpty(Magic) && "ustar".Equals(Magic))
		{
			string source = ArchiveEncoding.Decode(array, 345, 157);
			source = source.TrimNulls();
			if (!string.IsNullOrEmpty(source))
			{
				Name = source + "/" + Name;
			}
		}
		if (EntryType != EntryType.LongName && Name.Length == 0)
		{
			return false;
		}
		return true;
	}

	private string ReadLongName(BinaryReader reader, byte[] buffer)
	{
		int num = (int)ReadSize(buffer);
		byte[] array = reader.ReadBytes(num);
		int num2 = 512 - num % 512;
		if (num2 < 512)
		{
			reader.ReadBytes(num2);
		}
		return ArchiveEncoding.Decode(array, 0, array.Length).TrimNulls();
	}

	private static EntryType ReadEntryType(byte[] buffer)
	{
		return (EntryType)buffer[156];
	}

	private long ReadSize(byte[] buffer)
	{
		if ((buffer[124] & 0x80) == 128)
		{
			return BinaryPrimitives.ReadInt64BigEndian(MemoryExtensions.AsSpan(buffer, 128));
		}
		return ReadAsciiInt64Base8(buffer, 124, 11);
	}

	private static byte[] ReadBlock(BinaryReader reader)
	{
		byte[] array = reader.ReadBytes(512);
		if (array.Length != 0 && array.Length < 512)
		{
			throw new InvalidOperationException("Buffer is invalid size");
		}
		return array;
	}

	private static void WriteStringBytes(ReadOnlySpan<byte> name, Span<byte> buffer, int length)
	{
		name.CopyTo(buffer);
		int num = Math.Min(length, name.Length);
		buffer.Slice(num, length - num).Clear();
	}

	private static void WriteStringBytes(string name, byte[] buffer, int offset, int length)
	{
		int i;
		for (i = 0; i < length && i < name.Length; i++)
		{
			buffer[offset + i] = (byte)name[i];
		}
		for (; i < length; i++)
		{
			buffer[offset + i] = 0;
		}
	}

	private static void WriteOctalBytes(long value, byte[] buffer, int offset, int length)
	{
		string text = Convert.ToString(value, 8);
		int num = length - text.Length - 1;
		for (int i = 0; i < num; i++)
		{
			buffer[offset + i] = 32;
		}
		for (int j = 0; j < text.Length; j++)
		{
			buffer[offset + j + num] = (byte)text[j];
		}
	}

	private static int ReadAsciiInt32Base8(byte[] buffer, int offset, int count)
	{
		string value = Encoding.UTF8.GetString(buffer, offset, count).TrimNulls();
		if (string.IsNullOrEmpty(value))
		{
			return 0;
		}
		return Convert.ToInt32(value, 8);
	}

	private static long ReadAsciiInt64Base8(byte[] buffer, int offset, int count)
	{
		string value = Encoding.UTF8.GetString(buffer, offset, count).TrimNulls();
		if (string.IsNullOrEmpty(value))
		{
			return 0L;
		}
		return Convert.ToInt64(value, 8);
	}

	private static long ReadAsciiInt64(byte[] buffer, int offset, int count)
	{
		string value = Encoding.UTF8.GetString(buffer, offset, count).TrimNulls();
		if (string.IsNullOrEmpty(value))
		{
			return 0L;
		}
		return Convert.ToInt64(value);
	}

	internal static int RecalculateChecksum(byte[] buf)
	{
		eightSpaces.CopyTo(buf, 148);
		int num = 0;
		foreach (byte b in buf)
		{
			num += b;
		}
		return num;
	}

	internal static int RecalculateAltChecksum(byte[] buf)
	{
		eightSpaces.CopyTo(buf, 148);
		int num = 0;
		foreach (byte b in buf)
		{
			num = (((b & 0x80) != 128) ? (num + b) : (num - (b ^ 0x80)));
		}
		return num;
	}
}
