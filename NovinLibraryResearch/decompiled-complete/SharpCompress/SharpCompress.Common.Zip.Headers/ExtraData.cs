namespace SharpCompress.Common.Zip.Headers;

internal class ExtraData
{
	internal ExtraDataType Type { get; }

	internal ushort Length { get; }

	internal byte[] DataBytes { get; }

	public ExtraData(ExtraDataType type, ushort length, byte[] dataBytes)
	{
		Type = type;
		Length = length;
		DataBytes = dataBytes;
	}
}
