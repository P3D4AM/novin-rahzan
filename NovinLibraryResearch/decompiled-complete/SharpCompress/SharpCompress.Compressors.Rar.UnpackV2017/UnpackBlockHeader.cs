namespace SharpCompress.Compressors.Rar.UnpackV2017;

internal struct UnpackBlockHeader
{
	public int BlockSize;

	public int BlockBitSize;

	public int BlockStart;

	public int HeaderSize;

	public bool LastBlockInFile;

	public bool TablePresent;
}
