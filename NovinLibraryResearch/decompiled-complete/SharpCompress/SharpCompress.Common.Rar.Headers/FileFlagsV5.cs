namespace SharpCompress.Common.Rar.Headers;

internal static class FileFlagsV5
{
	public const ushort DIRECTORY = 1;

	public const ushort HAS_MOD_TIME = 2;

	public const ushort HAS_CRC32 = 4;

	public const ushort UNPACKED_SIZE_UNKNOWN = 8;
}
