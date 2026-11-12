using System;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Caching;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace Microsoft.Data.SqlClient;

internal abstract class VirtualizationBasedSecurityEnclaveProviderBase : EnclaveProviderBase
{
	private static readonly MemoryCache rootSigningCertificateCache = new MemoryCache("RootSigningCertificateCache", (NameValueCollection)null);

	private const int DiffieHellmanKeySize = 384;

	private const int VsmHGSProtocolId = 3;

	private static readonly EnclaveIdentity ExpectedPolicy = new EnclaveIdentity
	{
		OwnerId = new byte[32]
		{
			16, 32, 48, 64, 65, 49, 33, 17, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0
		},
		UniqueId = new byte[0],
		AuthorId = new byte[32]
		{
			4, 55, 202, 226, 83, 125, 139, 155, 7, 118,
			182, 27, 17, 230, 206, 211, 210, 50, 233, 48,
			143, 96, 226, 26, 218, 178, 253, 145, 227, 218,
			149, 152
		},
		FamilyId = new byte[16]
		{
			254, 254, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0
		},
		ImageId = new byte[16]
		{
			25, 23, 18, 0, 1, 5, 32, 19, 0, 5,
			20, 3, 18, 1, 34, 5
		},
		EnclaveSvn = 0u,
		SecureKernelSvn = 0u,
		PlatformSvn = 1u,
		Flags = 0u,
		SigningLevel = 0u,
		Reserved = 0u
	};

	internal override void GetEnclaveSession(EnclaveSessionParameters enclaveSessionParameters, bool generateCustomData, out SqlEnclaveSession sqlEnclaveSession, out long counter, out byte[] customData, out int customDataLength)
	{
		GetEnclaveSessionHelper(enclaveSessionParameters, shouldGenerateNonce: false, out sqlEnclaveSession, out counter, out customData, out customDataLength);
	}

	internal override SqlEnclaveAttestationParameters GetAttestationParameters(string attestationUrl, byte[] customData, int customDataLength)
	{
		ECDiffieHellman clientDiffieHellmanKey = KeyConverter.CreateECDiffieHellman(384);
		return new SqlEnclaveAttestationParameters(3, new byte[0], clientDiffieHellmanKey);
	}

	internal override void CreateEnclaveSession(byte[] attestationInfo, ECDiffieHellman clientDHKey, EnclaveSessionParameters enclaveSessionParameters, byte[] customData, int customDataLength, out SqlEnclaveSession sqlEnclaveSession, out long counter)
	{
		sqlEnclaveSession = null;
		counter = 0L;
		try
		{
			((ObjectCache)EnclaveProviderBase.ThreadRetryCache).Remove(Thread.CurrentThread.ManagedThreadId.ToString(), (string)null);
			sqlEnclaveSession = GetEnclaveSessionFromCache(enclaveSessionParameters, out counter);
			if (sqlEnclaveSession == null)
			{
				if (string.IsNullOrEmpty(enclaveSessionParameters.AttestationUrl))
				{
					throw new AlwaysEncryptedAttestationException(Strings.FailToCreateEnclaveSession);
				}
				AttestationInfo attestationInfo2 = new AttestationInfo(attestationInfo);
				VerifyEnclavePolicy(attestationInfo2.EnclaveReportPackage);
				VerifyAttestationInfo(enclaveSessionParameters.AttestationUrl, attestationInfo2.HealthReport, attestationInfo2.EnclaveReportPackage);
				byte[] sharedSecret = GetSharedSecret(attestationInfo2.Identity, attestationInfo2.EnclaveDHInfo, clientDHKey);
				sqlEnclaveSession = AddEnclaveSessionToCache(enclaveSessionParameters, sharedSecret, attestationInfo2.SessionId, out counter);
			}
		}
		finally
		{
			UpdateEnclaveSessionLockStatus(sqlEnclaveSession);
		}
	}

	internal override void InvalidateEnclaveSession(EnclaveSessionParameters enclaveSessionParameters, SqlEnclaveSession enclaveSessionToInvalidate)
	{
		InvalidateEnclaveSessionHelper(enclaveSessionParameters, enclaveSessionToInvalidate);
	}

	private void VerifyAttestationInfo(string attestationUrl, HealthReport healthReport, EnclaveReportPackage enclaveReportPackage)
	{
		bool flag = false;
		bool flag2;
		do
		{
			flag2 = false;
			X509Certificate2Collection signingCertificate = GetSigningCertificate(attestationUrl, flag);
			X509ChainStatusFlags x509ChainStatusFlags = VerifyHealthReportAgainstRootCertificate(signingCertificate, healthReport.Certificate);
			if (x509ChainStatusFlags != 0)
			{
				if (flag)
				{
					throw new AlwaysEncryptedAttestationException(string.Format(Strings.VerifyHealthCertificateChainFormat, attestationUrl, x509ChainStatusFlags));
				}
				flag = true;
				flag2 = true;
			}
		}
		while (flag2);
		VerifyEnclaveReportSignature(enclaveReportPackage, healthReport.Certificate);
	}

	protected abstract byte[] MakeRequest(string url);

	private X509Certificate2Collection GetSigningCertificate(string attestationUrl, bool forceUpdate)
	{
		attestationUrl = GetAttestationUrl(attestationUrl);
		X509Certificate2Collection x509Certificate2Collection = (X509Certificate2Collection)((ObjectCache)rootSigningCertificateCache)[attestationUrl];
		if (forceUpdate || x509Certificate2Collection == null || AnyCertificatesExpired(x509Certificate2Collection))
		{
			byte[] rawData = MakeRequest(attestationUrl);
			X509Certificate2Collection x509Certificate2Collection2 = new X509Certificate2Collection();
			try
			{
				x509Certificate2Collection2.Import(rawData);
			}
			catch (CryptographicException innerException)
			{
				throw new AlwaysEncryptedAttestationException(string.Format(Strings.GetAttestationSigningCertificateFailedInvalidCertificate, attestationUrl), innerException);
			}
			((ObjectCache)rootSigningCertificateCache).Add(attestationUrl, (object)x509Certificate2Collection2, (DateTimeOffset)DateTime.Now.AddDays(1.0), (string)null);
		}
		return (X509Certificate2Collection)((ObjectCache)rootSigningCertificateCache)[attestationUrl];
	}

	protected abstract string GetAttestationUrl(string attestationUrl);

	private bool AnyCertificatesExpired(X509Certificate2Collection certificates)
	{
		return certificates.OfType<X509Certificate2>().Any((X509Certificate2 c) => c.NotAfter < DateTime.Now);
	}

	private X509ChainStatusFlags VerifyHealthReportAgainstRootCertificate(X509Certificate2Collection signingCerts, X509Certificate2 healthReportCert)
	{
		X509Chain x509Chain = new X509Chain();
		X509Certificate2Enumerator enumerator = signingCerts.GetEnumerator();
		while (enumerator.MoveNext())
		{
			X509Certificate2 current = enumerator.Current;
			x509Chain.ChainPolicy.ExtraStore.Add(current);
		}
		x509Chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
		if (!x509Chain.Build(healthReportCert))
		{
			bool flag = false;
			X509ChainStatus[] chainStatus = x509Chain.ChainStatus;
			for (int i = 0; i < chainStatus.Length; i++)
			{
				X509ChainStatus x509ChainStatus = chainStatus[i];
				if (x509ChainStatus.Status == X509ChainStatusFlags.UntrustedRoot)
				{
					flag = true;
					continue;
				}
				return x509ChainStatus.Status;
			}
			if (flag)
			{
				for (int j = 0; j < x509Chain.ChainElements.Count; j++)
				{
					X509ChainElement x509ChainElement = x509Chain.ChainElements[x509Chain.ChainElements.Count - 1 - j];
					X509Certificate2Enumerator enumerator2 = signingCerts.GetEnumerator();
					while (enumerator2.MoveNext())
					{
						X509Certificate2 current2 = enumerator2.Current;
						if (x509ChainElement.Certificate.Thumbprint == current2.Thumbprint)
						{
							return X509ChainStatusFlags.NoError;
						}
					}
				}
				return X509ChainStatusFlags.UntrustedRoot;
			}
		}
		return X509ChainStatusFlags.NoError;
	}

	private void VerifyEnclaveReportSignature(EnclaveReportPackage enclaveReportPackage, X509Certificate2 healthReportCert)
	{
		uint num = Convert.ToUInt32(enclaveReportPackage.PackageHeader.GetSizeInPayload()) + enclaveReportPackage.PackageHeader.SignedStatementSize + enclaveReportPackage.PackageHeader.SignatureSize;
		if (num != enclaveReportPackage.PackageHeader.PackageSize)
		{
			throw new ArgumentException(Strings.VerifyEnclaveReportFormatFailed);
		}
		using RSA rSA = KeyConverter.GetRSAFromCertificate(healthReportCert);
		if (!rSA.VerifyData(enclaveReportPackage.ReportAsBytes, enclaveReportPackage.SignatureBlob, HashAlgorithmName.SHA256, RSASignaturePadding.Pss))
		{
			throw new ArgumentException(Strings.VerifyEnclaveReportFailed);
		}
	}

	private void VerifyEnclavePolicy(EnclaveReportPackage enclaveReportPackage)
	{
		EnclaveIdentity identity = enclaveReportPackage.Report.Identity;
		VerifyEnclavePolicyProperty("OwnerId", identity.OwnerId, ExpectedPolicy.OwnerId);
		VerifyEnclavePolicyProperty("AuthorId", identity.AuthorId, ExpectedPolicy.AuthorId);
		VerifyEnclavePolicyProperty("FamilyId", identity.FamilyId, ExpectedPolicy.FamilyId);
		VerifyEnclavePolicyProperty("ImageId", identity.ImageId, ExpectedPolicy.ImageId);
		VerifyEnclavePolicyProperty("EnclaveSvn", identity.EnclaveSvn, ExpectedPolicy.EnclaveSvn);
		VerifyEnclavePolicyProperty("SecureKernelSvn", identity.SecureKernelSvn, ExpectedPolicy.SecureKernelSvn);
		VerifyEnclavePolicyProperty("PlatformSvn", identity.PlatformSvn, ExpectedPolicy.PlatformSvn);
		if (identity.Flags != ExpectedPolicy.Flags)
		{
			throw new InvalidOperationException(Strings.VerifyEnclaveDebuggable);
		}
	}

	private void VerifyEnclavePolicyProperty(string property, byte[] actual, byte[] expected)
	{
		if (!actual.SequenceEqual(expected))
		{
			string message = string.Format(Strings.VerifyEnclavePolicyFailedFormat, property, BitConverter.ToString(actual), BitConverter.ToString(expected));
			throw new ArgumentException(message);
		}
	}

	private void VerifyEnclavePolicyProperty(string property, uint actual, uint expected)
	{
		if (actual < expected)
		{
			string message = string.Format(Strings.VerifyEnclavePolicyFailedFormat, property, actual, expected);
			throw new ArgumentException(message);
		}
	}

	private byte[] GetSharedSecret(EnclavePublicKey enclavePublicKey, EnclaveDiffieHellmanInfo enclaveDHInfo, ECDiffieHellman clientDHKey)
	{
		using (RSA rSA = KeyConverter.CreateRSAFromPublicKeyBlob(enclavePublicKey.PublicKey))
		{
			if (!rSA.VerifyData(enclaveDHInfo.PublicKey, enclaveDHInfo.PublicKeySignature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1))
			{
				throw new ArgumentException(Strings.GetSharedSecretFailed);
			}
		}
		using ECDiffieHellman eCDiffieHellman = KeyConverter.CreateECDiffieHellmanFromPublicKeyBlob(enclaveDHInfo.PublicKey);
		return KeyConverter.DeriveKey(clientDHKey, eCDiffieHellman.PublicKey);
	}
}
