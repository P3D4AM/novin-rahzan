namespace SharpCompress.Compressors.Rar.UnpackV2017;

internal class AudioVariables
{
	public int K1;

	public int K2;

	public int K3;

	public int K4;

	public int K5;

	public int D1;

	public int D2;

	public int D3;

	public int D4;

	public int LastDelta;

	public readonly uint[] Dif = new uint[11];

	public uint ByteCount;

	public int LastChar;
}
