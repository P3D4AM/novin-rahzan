namespace SharpCompress.Compressors.LZMA;

internal interface ISetCoderProperties
{
	void SetCoderProperties(CoderPropId[] propIDs, object[] properties);
}
