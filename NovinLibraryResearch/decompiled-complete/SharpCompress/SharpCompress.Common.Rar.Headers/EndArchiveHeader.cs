using SharpCompress.IO;

namespace SharpCompress.Common.Rar.Headers;

internal class EndArchiveHeader : RarHeader
{
	private ushort Flags { get; set; }

	internal int? ArchiveCrc { get; private set; }

	internal short? VolumeNumber { get; private set; }

	public EndArchiveHeader(RarHeader header, RarCrcBinaryReader reader)
		: base(header, reader, HeaderType.EndArchive)
	{
	}

	protected override void ReadFinish(MarkingBinaryReader reader)
	{
		if (base.IsRar5)
		{
			Flags = reader.ReadRarVIntUInt16();
			return;
		}
		Flags = base.HeaderFlags;
		if (HasFlag(2))
		{
			ArchiveCrc = reader.ReadInt32();
		}
		if (HasFlag(8))
		{
			VolumeNumber = reader.ReadInt16();
		}
	}

	private bool HasFlag(ushort flag)
	{
		return (Flags & flag) == flag;
	}
}
