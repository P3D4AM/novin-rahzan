using SharpCompress.IO;

namespace SharpCompress.Common.Rar.Headers;

internal sealed class ProtectHeader : RarHeader
{
	internal uint DataSize => checked((uint)base.AdditionalDataSize);

	internal byte Version { get; private set; }

	internal ushort RecSectors { get; private set; }

	internal uint TotalBlocks { get; private set; }

	internal byte[]? Mark { get; private set; }

	public ProtectHeader(RarHeader header, RarCrcBinaryReader reader)
		: base(header, reader, HeaderType.Protect)
	{
		if (base.IsRar5)
		{
			throw new InvalidFormatException("unexpected rar5 record");
		}
	}

	protected override void ReadFinish(MarkingBinaryReader reader)
	{
		Version = reader.ReadByte();
		RecSectors = reader.ReadUInt16();
		TotalBlocks = reader.ReadUInt32();
		Mark = reader.ReadBytes(8);
	}
}
