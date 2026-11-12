namespace SharpCompress.Common.Rar.Headers;

internal static class FileFlagsV4
{
	public const ushort SPLIT_BEFORE = 1;

	public const ushort SPLIT_AFTER = 2;

	public const ushort PASSWORD = 4;

	public const ushort COMMENT = 8;

	public const ushort SOLID = 16;

	public const ushort WINDOW_MASK = 224;

	public const ushort WINDOW64 = 0;

	public const ushort WINDOW128 = 32;

	public const ushort WINDOW256 = 64;

	public const ushort WINDOW512 = 96;

	public const ushort WINDOW1024 = 128;

	public const ushort WINDOW2048 = 160;

	public const ushort WINDOW4096 = 192;

	public const ushort DIRECTORY = 224;

	public const ushort LARGE = 256;

	public const ushort UNICODE = 512;

	public const ushort SALT = 1024;

	public const ushort VERSION = 2048;

	public const ushort EXT_TIME = 4096;

	public const ushort EXT_FLAGS = 8192;
}
