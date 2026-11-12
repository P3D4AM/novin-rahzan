namespace SharpCompress.Compressors.Rar.UnpackV1.Decode;

internal enum FilterType : byte
{
	FILTER_DELTA,
	FILTER_E8,
	FILTER_E8E9,
	FILTER_ARM,
	FILTER_AUDIO,
	FILTER_RGB,
	FILTER_ITANIUM,
	FILTER_PPM,
	FILTER_NONE
}
