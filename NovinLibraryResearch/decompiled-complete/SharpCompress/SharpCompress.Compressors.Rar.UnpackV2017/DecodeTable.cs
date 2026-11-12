namespace SharpCompress.Compressors.Rar.UnpackV2017;

internal sealed class DecodeTable
{
	public uint MaxNum;

	public readonly uint[] DecodeLen = new uint[16];

	public readonly uint[] DecodePos = new uint[16];

	public uint QuickBits;

	public readonly byte[] QuickLen = new byte[1024];

	public readonly ushort[] QuickNum = new ushort[1024];

	public readonly ushort[] DecodeNum = new ushort[306];
}
