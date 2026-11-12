using System;
using System.Buffers.Binary;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using SharpCompress.Common;
using SharpCompress.Common.Zip;
using SharpCompress.Common.Zip.Headers;

namespace SharpCompress.Writers.Zip;

internal class ZipCentralDirectoryEntry
{
	private readonly ZipCompressionMethod compression;

	private readonly string fileName;

	private readonly ArchiveEncoding archiveEncoding;

	internal DateTime? ModificationTime { get; set; }

	internal string? Comment { get; set; }

	internal uint Crc { get; set; }

	internal ulong Compressed { get; set; }

	internal ulong Decompressed { get; set; }

	internal ushort Zip64HeaderOffset { get; set; }

	internal ulong HeaderOffset { get; }

	public ZipCentralDirectoryEntry(ZipCompressionMethod compression, string fileName, ulong headerOffset, ArchiveEncoding archiveEncoding)
	{
		this.compression = compression;
		this.fileName = fileName;
		HeaderOffset = headerOffset;
		this.archiveEncoding = archiveEncoding;
	}

	internal unsafe uint Write(Stream outputStream)
	{
		byte[] array = archiveEncoding.Encode(fileName);
		byte[] array2 = archiveEncoding.Encode(Comment ?? string.Empty);
		bool flag = Compressed >= uint.MaxValue || Decompressed >= uint.MaxValue;
		bool flag2 = flag || HeaderOffset >= uint.MaxValue;
		ZipCompressionMethod zipCompressionMethod = compression;
		uint value = (uint)(flag2 ? uint.MaxValue : Compressed);
		uint value2 = (uint)(flag2 ? uint.MaxValue : Decompressed);
		uint value3 = (uint)(flag2 ? uint.MaxValue : HeaderOffset);
		int num = (flag2 ? 32 : 0);
		byte b = (byte)(flag2 ? 45u : 20u);
		HeaderFlags headerFlags = (object.Equals(archiveEncoding.GetEncoding(), Encoding.UTF8) ? HeaderFlags.Efs : HeaderFlags.None);
		if (!outputStream.CanSeek)
		{
			if (!flag)
			{
				headerFlags |= HeaderFlags.UsePostDataDescriptor;
			}
			if (zipCompressionMethod == ZipCompressionMethod.LZMA)
			{
				headerFlags |= HeaderFlags.Bit1;
			}
		}
		if (Decompressed == 0L && Compressed == 0L)
		{
			zipCompressionMethod = ZipCompressionMethod.None;
		}
		byte* num2 = stackalloc byte[8];
		// IL cpblk instruction
		Unsafe.CopyBlock(num2, ref global::_003CPrivateImplementationDetails_003E.A8E7965ED3ECE3EEC2792792AA84C05711ECB81CA05100766489F52C0A6483B8, 8);
		num2[4] = b;
		num2[6] = b;
		Span<byte> span = new Span<byte>(num2, 8);
		outputStream.Write(span);
		BinaryPrimitives.WriteUInt16LittleEndian(span, (ushort)headerFlags);
		outputStream.Write(span.Slice(0, 2));
		BinaryPrimitives.WriteUInt16LittleEndian(span, (ushort)zipCompressionMethod);
		outputStream.Write(span.Slice(0, 2));
		BinaryPrimitives.WriteUInt32LittleEndian(span, ModificationTime.DateTimeToDosTime());
		outputStream.Write(span.Slice(0, 4));
		BinaryPrimitives.WriteUInt32LittleEndian(span, Crc);
		outputStream.Write(span.Slice(0, 4));
		BinaryPrimitives.WriteUInt32LittleEndian(span, value);
		outputStream.Write(span.Slice(0, 4));
		BinaryPrimitives.WriteUInt32LittleEndian(span, value2);
		outputStream.Write(span.Slice(0, 4));
		BinaryPrimitives.WriteUInt16LittleEndian(span, (ushort)array.Length);
		outputStream.Write(span.Slice(0, 2));
		BinaryPrimitives.WriteUInt16LittleEndian(span, (ushort)num);
		outputStream.Write(span.Slice(0, 2));
		BinaryPrimitives.WriteUInt16LittleEndian(span, (ushort)array2.Length);
		outputStream.Write(span.Slice(0, 2));
		BinaryPrimitives.WriteUInt16LittleEndian(span, 0);
		outputStream.Write(span.Slice(0, 2));
		BinaryPrimitives.WriteUInt16LittleEndian(span, (ushort)headerFlags);
		outputStream.Write(span.Slice(0, 2));
		BinaryPrimitives.WriteUInt16LittleEndian(span, (ushort)headerFlags);
		outputStream.Write(span.Slice(0, 2));
		BinaryPrimitives.WriteUInt16LittleEndian(span, 33024);
		outputStream.Write(span.Slice(0, 2));
		BinaryPrimitives.WriteUInt32LittleEndian(span, value3);
		outputStream.Write(span.Slice(0, 4));
		outputStream.Write(array, 0, array.Length);
		if (flag2)
		{
			BinaryPrimitives.WriteUInt16LittleEndian(span, 1);
			outputStream.Write(span.Slice(0, 2));
			BinaryPrimitives.WriteUInt16LittleEndian(span, (ushort)(num - 4));
			outputStream.Write(span.Slice(0, 2));
			BinaryPrimitives.WriteUInt64LittleEndian(span, Decompressed);
			outputStream.Write(span);
			BinaryPrimitives.WriteUInt64LittleEndian(span, Compressed);
			outputStream.Write(span);
			BinaryPrimitives.WriteUInt64LittleEndian(span, HeaderOffset);
			outputStream.Write(span);
			BinaryPrimitives.WriteUInt32LittleEndian(span, 0u);
			outputStream.Write(span.Slice(0, 4));
		}
		outputStream.Write(array2, 0, array2.Length);
		return (uint)(46 + array.Length + num + array2.Length);
	}
}
