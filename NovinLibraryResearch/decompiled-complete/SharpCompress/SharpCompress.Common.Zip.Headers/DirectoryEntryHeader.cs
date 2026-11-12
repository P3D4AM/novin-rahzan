using System.IO;
using System.Linq;

namespace SharpCompress.Common.Zip.Headers;

internal class DirectoryEntryHeader : ZipFileEntry
{
	internal ushort Version { get; private set; }

	public ushort VersionNeededToExtract { get; set; }

	public long RelativeOffsetOfEntryHeader { get; set; }

	public uint ExternalFileAttributes { get; set; }

	public ushort InternalFileAttributes { get; set; }

	public ushort DiskNumberStart { get; set; }

	public string? Comment { get; private set; }

	public DirectoryEntryHeader(ArchiveEncoding archiveEncoding)
		: base(ZipHeaderType.DirectoryEntry, archiveEncoding)
	{
	}

	internal override void Read(BinaryReader reader)
	{
		Version = reader.ReadUInt16();
		VersionNeededToExtract = reader.ReadUInt16();
		base.Flags = (HeaderFlags)reader.ReadUInt16();
		base.CompressionMethod = (ZipCompressionMethod)reader.ReadUInt16();
		base.LastModifiedTime = reader.ReadUInt16();
		base.LastModifiedDate = reader.ReadUInt16();
		base.Crc = reader.ReadUInt32();
		base.CompressedSize = reader.ReadUInt32();
		base.UncompressedSize = reader.ReadUInt32();
		ushort count = reader.ReadUInt16();
		ushort count2 = reader.ReadUInt16();
		ushort count3 = reader.ReadUInt16();
		DiskNumberStart = reader.ReadUInt16();
		InternalFileAttributes = reader.ReadUInt16();
		ExternalFileAttributes = reader.ReadUInt32();
		RelativeOffsetOfEntryHeader = reader.ReadUInt32();
		byte[] bytes = reader.ReadBytes(count);
		byte[] extra = reader.ReadBytes(count2);
		byte[] bytes2 = reader.ReadBytes(count3);
		if (base.Flags.HasFlag(HeaderFlags.Efs))
		{
			base.Name = base.ArchiveEncoding.DecodeUTF8(bytes);
			Comment = base.ArchiveEncoding.DecodeUTF8(bytes2);
		}
		else
		{
			base.Name = base.ArchiveEncoding.Decode(bytes);
			Comment = base.ArchiveEncoding.Decode(bytes2);
		}
		LoadExtra(extra);
		ExtraData extraData = base.Extra.FirstOrDefault((ExtraData u) => u.Type == ExtraDataType.UnicodePathExtraField);
		if (extraData != null)
		{
			base.Name = ((ExtraUnicodePathExtraField)extraData).UnicodeName;
		}
		Zip64ExtendedInformationExtraField zip64ExtendedInformationExtraField = base.Extra.OfType<Zip64ExtendedInformationExtraField>().FirstOrDefault();
		if (zip64ExtendedInformationExtraField != null)
		{
			zip64ExtendedInformationExtraField.Process(base.UncompressedSize, base.CompressedSize, RelativeOffsetOfEntryHeader, DiskNumberStart);
			if (base.CompressedSize == uint.MaxValue)
			{
				base.CompressedSize = zip64ExtendedInformationExtraField.CompressedSize;
			}
			if (base.UncompressedSize == uint.MaxValue)
			{
				base.UncompressedSize = zip64ExtendedInformationExtraField.UncompressedSize;
			}
			if (RelativeOffsetOfEntryHeader == uint.MaxValue)
			{
				RelativeOffsetOfEntryHeader = zip64ExtendedInformationExtraField.RelativeOffsetOfEntryHeader;
			}
		}
	}
}
