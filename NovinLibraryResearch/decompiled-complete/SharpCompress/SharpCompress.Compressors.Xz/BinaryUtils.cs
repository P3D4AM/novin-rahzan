using System;
using System.Buffers.Binary;
using System.IO;

namespace SharpCompress.Compressors.Xz;

public static class BinaryUtils
{
	public static int ReadLittleEndianInt32(this BinaryReader reader)
	{
		return BinaryPrimitives.ReadInt32LittleEndian(reader.ReadBytes(4));
	}

	internal static uint ReadLittleEndianUInt32(this BinaryReader reader)
	{
		return (uint)reader.ReadLittleEndianInt32();
	}

	public static int ReadLittleEndianInt32(this Stream stream)
	{
		Span<byte> span = stackalloc byte[4];
		if (!stream.ReadFully(span))
		{
			throw new EndOfStreamException();
		}
		return BinaryPrimitives.ReadInt32LittleEndian(span);
	}

	internal static uint ReadLittleEndianUInt32(this Stream stream)
	{
		return (uint)stream.ReadLittleEndianInt32();
	}

	internal static byte[] ToBigEndianBytes(this uint uint32)
	{
		byte[] bytes = BitConverter.GetBytes(uint32);
		if (BitConverter.IsLittleEndian)
		{
			Array.Reverse((Array)bytes);
		}
		return bytes;
	}

	internal static byte[] ToLittleEndianBytes(this uint uint32)
	{
		byte[] bytes = BitConverter.GetBytes(uint32);
		if (!BitConverter.IsLittleEndian)
		{
			Array.Reverse((Array)bytes);
		}
		return bytes;
	}
}
