namespace SharpCompress.Common.Rar.Headers;

internal static class EncryptionFlagsV5
{
	public const uint CHFL_CRYPT_PSWCHECK = 1u;

	public const uint FHEXTRA_CRYPT_PSWCHECK = 1u;

	public const uint FHEXTRA_CRYPT_HASHMAC = 2u;
}
