namespace SharpCompress.Common.Rar.Headers;

internal static class EndArchiveFlagsV4
{
	public const ushort NEXT_VOLUME = 1;

	public const ushort DATA_CRC = 2;

	public const ushort REV_SPACE = 4;

	public const ushort VOLUME_NUMBER = 8;
}
