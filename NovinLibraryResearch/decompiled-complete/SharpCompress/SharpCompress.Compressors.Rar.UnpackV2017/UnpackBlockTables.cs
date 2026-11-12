namespace SharpCompress.Compressors.Rar.UnpackV2017;

internal struct UnpackBlockTables
{
	public DecodeTable LD;

	public DecodeTable DD;

	public DecodeTable LDD;

	public DecodeTable RD;

	public DecodeTable BD;

	public void Init()
	{
		LD = new DecodeTable();
		DD = new DecodeTable();
		LDD = new DecodeTable();
		RD = new DecodeTable();
		BD = new DecodeTable();
	}
}
