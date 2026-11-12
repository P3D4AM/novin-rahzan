namespace SharpCompress.Common.Rar.Headers;

internal static class ArchiveFlagsV4
{
	public const ushort VOLUME = 1;

	public const ushort COMMENT = 2;

	public const ushort LOCK = 4;

	public const ushort SOLID = 8;

	public const ushort NEW_NUMBERING = 16;

	public const ushort AV = 32;

	public const ushort PROTECT = 64;

	public const ushort PASSWORD = 128;

	public const ushort FIRST_VOLUME = 256;

	public const ushort ENCRYPT_VER = 512;
}
