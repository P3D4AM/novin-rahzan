namespace SharpCompress.Compressors.LZMA;

internal enum CoderPropId
{
	DefaultProp,
	DictionarySize,
	UsedMemorySize,
	Order,
	BlockSize,
	PosStateBits,
	LitContextBits,
	LitPosBits,
	NumFastBytes,
	MatchFinder,
	MatchFinderCycles,
	NumPasses,
	Algorithm,
	NumThreads,
	EndMarker
}
