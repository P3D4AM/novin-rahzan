namespace SharpCompress.Common.Rar.Headers;

internal static class HeaderFlagsV5
{
	public const ushort HAS_EXTRA = 1;

	public const ushort HAS_DATA = 2;

	public const ushort KEEP = 4;

	public const ushort SPLIT_BEFORE = 8;

	public const ushort SPLIT_AFTER = 16;

	public const ushort CHILD = 32;

	public const ushort PRESERVE_CHILD = 64;
}
