namespace Microsoft.Identity.Client.AuthScheme.PoP;

internal interface IPoPCryptoProvider
{
	string CannonicalPublicKeyJwk { get; }

	byte[] Sign(byte[] data);
}
