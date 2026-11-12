using System.Security.Cryptography;
using Microsoft.Identity.Client.Utils;

namespace Microsoft.Identity.Client.AuthScheme.PoP;

internal class InMemoryCryptoProvider : IPoPCryptoProvider
{
	internal const int RsaKeySize = 2048;

	private RSA _signingKey;

	public string CannonicalPublicKeyJwk { get; private set; }

	public InMemoryCryptoProvider()
	{
		InitializeSigningKey();
	}

	private void InitializeSigningKey()
	{
		_signingKey = RSA.Create();
		_signingKey.KeySize = 2048;
		RSAParameters rsaPublicKey = _signingKey.ExportParameters(includePrivateParameters: false);
		CannonicalPublicKeyJwk = ComputeCannonicalJwk(rsaPublicKey);
	}

	public byte[] Sign(byte[] payload)
	{
		return Sign(_signingKey, payload);
	}

	private static string ComputeCannonicalJwk(RSAParameters rsaPublicKey)
	{
		return "{\"e\":\"" + Base64UrlHelpers.Encode(rsaPublicKey.Exponent) + "\",\"kty\":\"RSA\",\"n\":\"" + Base64UrlHelpers.Encode(rsaPublicKey.Modulus) + "\"}";
	}

	public static byte[] Sign(RSA RsaKey, byte[] payload)
	{
		return RsaKey.SignData(payload, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
	}
}
