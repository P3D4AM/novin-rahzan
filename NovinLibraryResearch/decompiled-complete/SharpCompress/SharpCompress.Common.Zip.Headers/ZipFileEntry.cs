using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;

namespace SharpCompress.Common.Zip.Headers;

internal abstract class ZipFileEntry : ZipHeader
{
	internal bool IsDirectory
	{
		get
		{
			if (StringExtensions.EndsWith(Name, '/'))
			{
				return true;
			}
			if (CompressedSize == 0L && UncompressedSize == 0L)
			{
				return StringExtensions.EndsWith(Name, '\\');
			}
			return false;
		}
	}

	internal Stream PackedStream { get; set; }

	internal ArchiveEncoding ArchiveEncoding { get; }

	internal string Name { get; set; }

	internal HeaderFlags Flags { get; set; }

	internal ZipCompressionMethod CompressionMethod { get; set; }

	internal long CompressedSize { get; set; }

	internal long? DataStartPosition { get; set; }

	internal long UncompressedSize { get; set; }

	internal List<ExtraData> Extra { get; set; }

	public string Password { get; set; }

	internal WinzipAesEncryptionData WinzipAesEncryptionData { get; set; }

	internal ushort LastModifiedDate { get; set; }

	internal ushort LastModifiedTime { get; set; }

	internal uint Crc { get; set; }

	internal ZipFilePart Part { get; set; }

	internal bool IsZip64 => CompressedSize >= uint.MaxValue;

	protected ZipFileEntry(ZipHeaderType type, ArchiveEncoding archiveEncoding)
		: base(type)
	{
		Extra = new List<ExtraData>();
		ArchiveEncoding = archiveEncoding;
	}

	internal PkwareTraditionalEncryptionData ComposeEncryptionData(Stream archiveStream)
	{
		if (archiveStream == null)
		{
			throw new ArgumentNullException("archiveStream");
		}
		byte[] array = new byte[12];
		archiveStream.ReadFully(array);
		return PkwareTraditionalEncryptionData.ForRead(Password, this, array);
	}

	protected void LoadExtra(byte[] extra)
	{
		ushort num;
		for (int i = 0; i < extra.Length - 4; i += num + 4)
		{
			ExtraDataType extraDataType = (ExtraDataType)BinaryPrimitives.ReadUInt16LittleEndian(MemoryExtensions.AsSpan(extra, i));
			if (!Enum.IsDefined(typeof(ExtraDataType), extraDataType))
			{
				extraDataType = ExtraDataType.NotImplementedExtraData;
			}
			num = BinaryPrimitives.ReadUInt16LittleEndian(MemoryExtensions.AsSpan(extra, i + 2));
			if (num > extra.Length)
			{
				break;
			}
			byte[] array = new byte[num];
			Buffer.BlockCopy(extra, i + 4, array, 0, num);
			Extra.Add(LocalEntryHeaderExtraFactory.Create(extraDataType, num, array));
		}
	}
}
