using System;
using System.Buffers.Binary;

namespace SharpCompress.Common.Zip.Headers;

internal sealed class Zip64ExtendedInformationExtraField : ExtraData
{
	public long UncompressedSize { get; private set; }

	public long CompressedSize { get; private set; }

	public long RelativeOffsetOfEntryHeader { get; private set; }

	public uint VolumeNumber { get; private set; }

	public Zip64ExtendedInformationExtraField(ExtraDataType type, ushort length, byte[] dataBytes)
		: base(type, length, dataBytes)
	{
	}

	public void Process(long uncompressedFileSize, long compressedFileSize, long relativeHeaderOffset, ushort diskNumber)
	{
		int num = ((uncompressedFileSize == uint.MaxValue) ? 8 : 0) + ((compressedFileSize == uint.MaxValue) ? 8 : 0) + ((relativeHeaderOffset == uint.MaxValue) ? 8 : 0) + ((diskNumber == ushort.MaxValue) ? 4 : 0);
		int num2 = 0;
		if (num > base.DataBytes.Length)
		{
			throw new ArchiveException("Zip64 extended information extra field is not large enough for the required information");
		}
		if (uncompressedFileSize == uint.MaxValue)
		{
			UncompressedSize = BinaryPrimitives.ReadInt64LittleEndian(MemoryExtensions.AsSpan(base.DataBytes, num2));
			num2 += 8;
		}
		if (compressedFileSize == uint.MaxValue)
		{
			CompressedSize = BinaryPrimitives.ReadInt64LittleEndian(MemoryExtensions.AsSpan(base.DataBytes, num2));
			num2 += 8;
		}
		if (relativeHeaderOffset == uint.MaxValue)
		{
			RelativeOffsetOfEntryHeader = BinaryPrimitives.ReadInt64LittleEndian(MemoryExtensions.AsSpan(base.DataBytes, num2));
			num2 += 8;
		}
		if (diskNumber == ushort.MaxValue)
		{
			VolumeNumber = BinaryPrimitives.ReadUInt32LittleEndian(MemoryExtensions.AsSpan(base.DataBytes, num2));
		}
	}
}
