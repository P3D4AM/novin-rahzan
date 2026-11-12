using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Data.SqlClient;

internal sealed class KeyConverter
{
	internal static RSA CreateRSAFromPublicKeyBlob(byte[] keyBlob)
	{
		CngKey key = CngKey.Import(keyBlob, CngKeyBlobFormat.GenericPublicBlob);
		return new RSACng(key);
	}

	internal static ECDiffieHellman CreateECDiffieHellmanFromPublicKeyBlob(byte[] keyBlob)
	{
		CngKey key = CngKey.Import(keyBlob, CngKeyBlobFormat.GenericPublicBlob);
		return new ECDiffieHellmanCng(key);
	}

	internal static ECDiffieHellman CreateECDiffieHellman(int keySize)
	{
		ECDiffieHellmanCng eCDiffieHellmanCng = new ECDiffieHellmanCng(keySize);
		eCDiffieHellmanCng.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
		eCDiffieHellmanCng.HashAlgorithm = CngAlgorithm.Sha256;
		return eCDiffieHellmanCng;
	}

	public static byte[] GetECDiffieHellmanPublicKeyBlob(ECDiffieHellman ecDiffieHellman)
	{
		if (ecDiffieHellman is ECDiffieHellmanCng eCDiffieHellmanCng)
		{
			return eCDiffieHellmanCng.Key.Export(CngKeyBlobFormat.EccPublicBlob);
		}
		throw new InvalidOperationException();
	}

	internal static byte[] DeriveKey(ECDiffieHellman ecDiffieHellman, ECDiffieHellmanPublicKey publicKey)
	{
		if (ecDiffieHellman is ECDiffieHellmanCng eCDiffieHellmanCng)
		{
			return eCDiffieHellmanCng.DeriveKeyMaterial(publicKey);
		}
		throw new InvalidOperationException();
	}

	internal static RSA GetRSAFromCertificate(X509Certificate2 certificate)
	{
		RSAParameters parameters;
		using (RSA rSA = certificate.GetRSAPublicKey())
		{
			parameters = rSA.ExportParameters(includePrivateParameters: false);
		}
		RSACng rSACng = new RSACng();
		rSACng.ImportParameters(parameters);
		return rSACng;
	}
}
