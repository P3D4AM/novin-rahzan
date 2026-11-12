using SharpCompress.IO;

namespace SharpCompress.Common.Rar.Headers;

internal sealed class ArchiveHeader : RarHeader
{
	private ushort Flags { get; set; }

	internal int? VolumeNumber { get; private set; }

	internal short? HighPosAv { get; private set; }

	internal int? PosAv { get; private set; }

	private byte? EncryptionVersion { get; set; }

	public bool? IsEncrypted
	{
		get
		{
			if (!base.IsRar5)
			{
				return HasFlag(128);
			}
			return null;
		}
	}

	public bool OldNumberingFormat
	{
		get
		{
			if (!base.IsRar5)
			{
				return !HasFlag(16);
			}
			return false;
		}
	}

	public bool IsVolume => HasFlag((ushort)(base.IsRar5 ? 1 : 1));

	public bool IsFirstVolume
	{
		get
		{
			if (!base.IsRar5)
			{
				return HasFlag(256);
			}
			return !VolumeNumber.HasValue;
		}
	}

	public bool IsSolid => HasFlag((ushort)(base.IsRar5 ? 4 : 8));

	public ArchiveHeader(RarHeader header, RarCrcBinaryReader reader)
		: base(header, reader, HeaderType.Archive)
	{
	}

	protected override void ReadFinish(MarkingBinaryReader reader)
	{
		if (base.IsRar5)
		{
			Flags = reader.ReadRarVIntUInt16();
			if (HasFlag(2))
			{
				VolumeNumber = (int)reader.ReadRarVIntUInt32();
			}
			return;
		}
		Flags = base.HeaderFlags;
		HighPosAv = reader.ReadInt16();
		PosAv = reader.ReadInt32();
		if (HasFlag(512))
		{
			EncryptionVersion = reader.ReadByte();
		}
	}

	private void ReadLocator(MarkingBinaryReader reader)
	{
		reader.ReadRarVIntUInt16();
		if (reader.ReadRarVIntUInt16() != 1)
		{
			throw new InvalidFormatException("expected locator record");
		}
		ushort num = reader.ReadRarVIntUInt16();
		if ((num & 1) == 1)
		{
			reader.ReadRarVInt();
		}
		if ((num & 2) == 2)
		{
			reader.ReadRarVInt();
		}
	}

	private bool HasFlag(ushort flag)
	{
		return (Flags & flag) == flag;
	}
}
