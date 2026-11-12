namespace SharpCompress.Compressors.Rar.UnpackV2017;

internal class UnpackFilter
{
	public byte Type;

	public uint BlockStart;

	public uint BlockLength;

	public byte Channels;

	public bool NextWindow;
}
