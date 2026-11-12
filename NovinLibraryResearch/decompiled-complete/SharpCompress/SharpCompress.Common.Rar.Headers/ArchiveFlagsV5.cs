namespace SharpCompress.Common.Rar.Headers;

internal static class ArchiveFlagsV5
{
	public const ushort VOLUME = 1;

	public const ushort HAS_VOLUME_NUMBER = 2;

	public const ushort SOLID = 4;

	public const ushort PROTECT = 8;

	public const ushort LOCK = 16;
}
