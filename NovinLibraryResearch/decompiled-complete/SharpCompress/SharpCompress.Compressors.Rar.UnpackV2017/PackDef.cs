namespace SharpCompress.Compressors.Rar.UnpackV2017;

internal static class PackDef
{
	public const uint MAX_LZ_MATCH = 4097u;

	public const uint MAX3_LZ_MATCH = 257u;

	public const uint LOW_DIST_REP_COUNT = 16u;

	public const uint NC = 306u;

	public const uint DC = 64u;

	public const uint LDC = 16u;

	public const uint RC = 44u;

	public const uint HUFF_TABLE_SIZE = 430u;

	public const uint BC = 20u;

	public const uint NC30 = 299u;

	public const uint DC30 = 60u;

	public const uint LDC30 = 17u;

	public const uint RC30 = 28u;

	public const uint BC30 = 20u;

	public const uint HUFF_TABLE_SIZE30 = 404u;

	public const uint NC20 = 298u;

	public const uint DC20 = 48u;

	public const uint RC20 = 28u;

	public const uint BC20 = 19u;

	public const uint MC20 = 257u;

	public const uint LARGEST_TABLE_SIZE = 306u;

	public const int FILTER_DELTA = 0;

	public const int FILTER_E8 = 1;

	public const int FILTER_E8E9 = 2;

	public const int FILTER_ARM = 3;

	public const int FILTER_AUDIO = 4;

	public const int FILTER_RGB = 5;

	public const int FILTER_ITANIUM = 6;

	public const int FILTER_PPM = 7;

	public const int FILTER_NONE = 8;
}
