using System;
using System.Globalization;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Text;
using Microsoft.Identity.Client.Internal;
using Microsoft.Identity.Client.Platforms.net45.Native;
using Microsoft.Identity.Client.PlatformsCommon.Interfaces;
using Microsoft.Identity.Client.Utils;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.Identity.Client.Platforms.net45;

internal class NetDesktopCryptographyManager : ICryptographyManager
{
	public string CreateBase64UrlEncodedSha256Hash(string input)
	{
		if (!string.IsNullOrEmpty(input))
		{
			return Base64UrlHelpers.Encode(CreateSha256HashBytes(input));
		}
		return null;
	}

	public string GenerateCodeVerifier()
	{
		byte[] array = new byte[32];
		using (RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider())
		{
			rNGCryptoServiceProvider.GetBytes(array);
		}
		return Base64UrlHelpers.Encode(array);
	}

	public string CreateSha256Hash(string input)
	{
		if (!string.IsNullOrEmpty(input))
		{
			return Convert.ToBase64String(CreateSha256HashBytes(input));
		}
		return null;
	}

	public byte[] CreateSha256HashBytes(string input)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		SHA256Cng val = new SHA256Cng();
		try
		{
			return ((HashAlgorithm)(object)val).ComputeHash(Encoding.UTF8.GetBytes(input));
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public string Encrypt(string message)
	{
		throw new NotImplementedException();
	}

	public string Decrypt(string encryptedMessage)
	{
		throw new NotImplementedException();
	}

	public byte[] Encrypt(byte[] message)
	{
		throw new NotImplementedException();
	}

	public byte[] Decrypt(byte[] encryptedMessage)
	{
		throw new NotImplementedException();
	}

	public byte[] SignWithCertificate(string message, X509Certificate2 certificate)
	{
		if (certificate.PublicKey.Key.KeySize < ClientCredentialWrapper.MinKeySizeInBits)
		{
			throw new ArgumentOutOfRangeException("certificate", string.Format(CultureInfo.InvariantCulture, "The certificate used must have a key size of at least {0} bits", ClientCredentialWrapper.MinKeySizeInBits));
		}
		byte[] bytes = Encoding.UTF8.GetBytes(message);
		using RSA rSA = certificate.GetRSAPrivateKey();
		return rSA.SignData(bytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
	}

	private static RSACryptoServiceProvider GetCryptoProviderForSha256_Net45(X509Certificate2 certificate)
	{
		RSACryptoServiceProvider rSACryptoServiceProvider;
		try
		{
			rSACryptoServiceProvider = certificate.PrivateKey as RSACryptoServiceProvider;
		}
		catch (CryptographicException innerException)
		{
			throw new MsalClientException("crypto_net45", "Could not use the certificate for signing. See inner exception for details. Possible cause: this may be a known issue with apps build against .NET Desktop 4.6 or lower. Either target a higher version of .NET desktop - 4.6.1 and above, or use a different certificate type (non-CNG) or sign your own assertion as described at https://aka.ms/msal-net-signed-assertion", innerException);
		}
		if (rSACryptoServiceProvider == null)
		{
			throw new MsalClientException("The provided certificate has a key that is not accessable.");
		}
		if ((rSACryptoServiceProvider.CspKeyContainerInfo.ProviderType == 1 || rSACryptoServiceProvider.CspKeyContainerInfo.ProviderType == 12) && !rSACryptoServiceProvider.CspKeyContainerInfo.HardwareDevice)
		{
			CspParameters cspParameters = new CspParameters
			{
				ProviderType = 24,
				KeyContainerName = rSACryptoServiceProvider.CspKeyContainerInfo.KeyContainerName,
				KeyNumber = (int)rSACryptoServiceProvider.CspKeyContainerInfo.KeyNumber
			};
			if (rSACryptoServiceProvider.CspKeyContainerInfo.MachineKeyStore)
			{
				cspParameters.Flags = CspProviderFlags.UseMachineKeyStore;
			}
			cspParameters.Flags |= CspProviderFlags.UseExistingKey;
			return new RSACryptoServiceProvider(cspParameters);
		}
		return rSACryptoServiceProvider;
	}

	[SecurityCritical]
	internal static CngKey GetCngPrivateKey(X509Certificate2 certificate)
	{
		using SafeCertContextHandle certificateContext = GetCertificateContext(certificate);
		using SafeNCryptKeyHandle keyHandle = X509Native.AcquireCngPrivateKey(certificateContext);
		new PermissionSet(PermissionState.Unrestricted).Assert();
		return CngKey.Open(keyHandle, CngKeyHandleOpenOptions.None);
	}

	[SecurityCritical]
	internal static SafeCertContextHandle GetCertificateContext(X509Certificate certificate)
	{
		SafeCertContextHandle result = X509Native.DuplicateCertContext(certificate.Handle);
		GC.KeepAlive(certificate);
		return result;
	}
}
