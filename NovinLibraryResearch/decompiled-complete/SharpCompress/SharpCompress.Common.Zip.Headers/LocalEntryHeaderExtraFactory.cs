namespace SharpCompress.Common.Zip.Headers;

internal static class LocalEntryHeaderExtraFactory
{
	internal static ExtraData Create(ExtraDataType type, ushort length, byte[] extraData)
	{
		return type switch
		{
			ExtraDataType.UnicodePathExtraField => new ExtraUnicodePathExtraField(type, length, extraData), 
			ExtraDataType.Zip64ExtendedInformationExtraField => new Zip64ExtendedInformationExtraField(type, length, extraData), 
			_ => new ExtraData(type, length, extraData), 
		};
	}
}
