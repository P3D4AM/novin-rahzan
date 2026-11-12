namespace SharpCompress.Common.Rar.Headers;

internal static class HeaderCodeV
{
	public const byte RAR4_MARK_HEADER = 114;

	public const byte RAR4_ARCHIVE_HEADER = 115;

	public const byte RAR4_FILE_HEADER = 116;

	public const byte RAR4_COMMENT_HEADER = 117;

	public const byte RAR4_AV_HEADER = 118;

	public const byte RAR4_SUB_HEADER = 119;

	public const byte RAR4_PROTECT_HEADER = 120;

	public const byte RAR4_SIGN_HEADER = 121;

	public const byte RAR4_NEW_SUB_HEADER = 122;

	public const byte RAR4_END_ARCHIVE_HEADER = 123;

	public const byte RAR5_ARCHIVE_HEADER = 1;

	public const byte RAR5_FILE_HEADER = 2;

	public const byte RAR5_SERVICE_HEADER = 3;

	public const byte RAR5_ARCHIVE_ENCRYPTION_HEADER = 4;

	public const byte RAR5_END_ARCHIVE_HEADER = 5;
}
