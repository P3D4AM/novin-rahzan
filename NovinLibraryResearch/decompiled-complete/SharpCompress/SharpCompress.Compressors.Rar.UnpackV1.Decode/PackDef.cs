namespace SharpCompress.Compressors.Rar.UnpackV1.Decode;

internal static class PackDef
{
	public const int MAXWINSIZE = 4194304;

	public const int MAXWINMASK = 4194303;

	public const uint MAX_LZ_MATCH = 4097u;

	public const uint MAX3_LZ_MATCH = 257u;

	public const int LOW_DIST_REP_COUNT = 16;

	public const int NC = 299;

	public const int DC = 60;

	public const int LDC = 17;

	public const int RC = 28;

	public const int HUFF_TABLE_SIZE = 404;

	public const int BC = 20;

	public const uint NC30 = 299u;

	public const uint DC30 = 60u;

	public const uint LDC30 = 17u;

	public const uint RC30 = 28u;

	public const uint BC30 = 20u;

	public const uint HUFF_TABLE_SIZE30 = 404u;

	public const int NC20 = 298;

	public const int DC20 = 48;

	public const int RC20 = 28;

	public const int BC20 = 19;

	public const int MC20 = 257;

	public const uint LARGEST_TABLE_SIZE = 306u;
}
