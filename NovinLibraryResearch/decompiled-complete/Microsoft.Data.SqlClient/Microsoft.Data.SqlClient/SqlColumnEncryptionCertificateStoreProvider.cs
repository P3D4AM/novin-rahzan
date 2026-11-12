using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Microsoft.Data.SqlClient;

public class SqlColumnEncryptionCertificateStoreProvider : SqlColumnEncryptionKeyStoreProvider
{
	public const string ProviderName = "MSSQL_CERTIFICATE_STORE";

	internal const string RSAEncryptionAlgorithmWithOAEP = "RSA_OAEP";

	private const string _certLocationLocalMachine = "LocalMachine";

	private const string _certLocationCurrentUser = "CurrentUser";

	private const string _myCertificateStore = "My";

	private const string _certificatePathFormat = "[LocalMachine|CurrentUser]/My/[Thumbprint]";

	private const string _hashingAlgorithm = "SHA256";

	private readonly byte[] _version = new byte[1] { 1 };

	public override byte[] DecryptColumnEncryptionKey(string masterKeyPath, string encryptionAlgorithm, byte[] encryptedColumnEncryptionKey)
	{
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		ValidateNonEmptyCertificatePath(masterKeyPath, isSystemOp: true);
		if (encryptedColumnEncryptionKey == null)
		{
			throw SQL.NullEncryptedColumnEncryptionKey();
		}
		if (encryptedColumnEncryptionKey.Length == 0)
		{
			throw SQL.EmptyEncryptedColumnEncryptionKey();
		}
		ValidateEncryptionAlgorithm(encryptionAlgorithm, isSystemOp: true);
		ValidateCertificatePathLength(masterKeyPath, isSystemOp: true);
		X509Certificate2 certificateByPath = GetCertificateByPath(masterKeyPath, isSystemOp: true);
		int num = certificateByPath.PublicKey.Key.KeySize / 8;
		if (encryptedColumnEncryptionKey[0] != _version[0])
		{
			throw SQL.InvalidAlgorithmVersionInEncryptedCEK(encryptedColumnEncryptionKey[0], _version[0]);
		}
		int num2 = _version.Length;
		short num3 = BitConverter.ToInt16(encryptedColumnEncryptionKey, num2);
		num2 += 2;
		int num4 = BitConverter.ToInt16(encryptedColumnEncryptionKey, num2);
		num2 += 2;
		num2 += num3;
		if (num4 != num)
		{
			throw SQL.InvalidCiphertextLengthInEncryptedCEK(num4, num, masterKeyPath);
		}
		int num5 = encryptedColumnEncryptionKey.Length - num2 - num4;
		if (num5 != num)
		{
			throw SQL.InvalidSignatureInEncryptedCEK(num5, num, masterKeyPath);
		}
		byte[] array = new byte[num4];
		Buffer.BlockCopy(encryptedColumnEncryptionKey, num2, array, 0, array.Length);
		num2 += num4;
		byte[] array2 = new byte[num5];
		Buffer.BlockCopy(encryptedColumnEncryptionKey, num2, array2, 0, array2.Length);
		SHA256Cng val = new SHA256Cng();
		byte[] hash;
		try
		{
			((HashAlgorithm)(object)val).TransformFinalBlock(encryptedColumnEncryptionKey, 0, encryptedColumnEncryptionKey.Length - array2.Length);
			hash = ((HashAlgorithm)(object)val).Hash;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if (!RSAVerifySignature(hash, array2, certificateByPath))
		{
			throw SQL.InvalidCertificateSignature(masterKeyPath);
		}
		return RSADecrypt(array, certificateByPath);
	}

	public override byte[] EncryptColumnEncryptionKey(string masterKeyPath, string encryptionAlgorithm, byte[] columnEncryptionKey)
	{
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		ValidateNonEmptyCertificatePath(masterKeyPath, isSystemOp: false);
		if (columnEncryptionKey == null)
		{
			throw SQL.NullColumnEncryptionKey();
		}
		if (columnEncryptionKey.Length == 0)
		{
			throw SQL.EmptyColumnEncryptionKey();
		}
		ValidateEncryptionAlgorithm(encryptionAlgorithm, isSystemOp: false);
		ValidateCertificatePathLength(masterKeyPath, isSystemOp: false);
		X509Certificate2 certificateByPath = GetCertificateByPath(masterKeyPath, isSystemOp: false);
		int num = certificateByPath.PublicKey.Key.KeySize / 8;
		byte[] array = new byte[1] { _version[0] };
		byte[] bytes = Encoding.Unicode.GetBytes(masterKeyPath.ToLowerInvariant());
		byte[] bytes2 = BitConverter.GetBytes((short)bytes.Length);
		byte[] array2 = RSAEncrypt(columnEncryptionKey, certificateByPath);
		byte[] bytes3 = BitConverter.GetBytes((short)array2.Length);
		SHA256Cng val = new SHA256Cng();
		byte[] hash;
		try
		{
			((HashAlgorithm)(object)val).TransformBlock(array, 0, array.Length, array, 0);
			((HashAlgorithm)(object)val).TransformBlock(bytes2, 0, bytes2.Length, bytes2, 0);
			((HashAlgorithm)(object)val).TransformBlock(bytes3, 0, bytes3.Length, bytes3, 0);
			((HashAlgorithm)(object)val).TransformBlock(bytes, 0, bytes.Length, bytes, 0);
			((HashAlgorithm)(object)val).TransformFinalBlock(array2, 0, array2.Length);
			hash = ((HashAlgorithm)(object)val).Hash;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		byte[] array3 = RSASignHashedData(hash, certificateByPath);
		int num2 = array.Length + bytes3.Length + bytes2.Length + array2.Length + bytes.Length + array3.Length;
		byte[] array4 = new byte[num2];
		int num3 = 0;
		Buffer.BlockCopy(array, 0, array4, num3, array.Length);
		num3 += array.Length;
		Buffer.BlockCopy(bytes2, 0, array4, num3, bytes2.Length);
		num3 += bytes2.Length;
		Buffer.BlockCopy(bytes3, 0, array4, num3, bytes3.Length);
		num3 += bytes3.Length;
		Buffer.BlockCopy(bytes, 0, array4, num3, bytes.Length);
		num3 += bytes.Length;
		Buffer.BlockCopy(array2, 0, array4, num3, array2.Length);
		num3 += array2.Length;
		Buffer.BlockCopy(array3, 0, array4, num3, array3.Length);
		return array4;
	}

	public override byte[] SignColumnMasterKeyMetadata(string masterKeyPath, bool allowEnclaveComputations)
	{
		byte[] dataToSign = ComputeMasterKeyMetadataHash(masterKeyPath, allowEnclaveComputations, isSystemOp: false);
		X509Certificate2 certificateByPath = GetCertificateByPath(masterKeyPath, isSystemOp: false);
		return RSASignHashedData(dataToSign, certificateByPath);
	}

	public override bool VerifyColumnMasterKeyMetadata(string masterKeyPath, bool allowEnclaveComputations, byte[] signature)
	{
		byte[] dataToVerify = ComputeMasterKeyMetadataHash(masterKeyPath, allowEnclaveComputations, isSystemOp: true);
		X509Certificate2 certificateByPath = GetCertificateByPath(masterKeyPath, isSystemOp: true);
		return RSAVerifySignature(dataToVerify, signature, certificateByPath);
	}

	private byte[] ComputeMasterKeyMetadataHash(string masterKeyPath, bool allowEnclaveComputations, bool isSystemOp)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		ValidateNonEmptyCertificatePath(masterKeyPath, isSystemOp);
		ValidateCertificatePathLength(masterKeyPath, isSystemOp);
		string text = "MSSQL_CERTIFICATE_STORE" + masterKeyPath + allowEnclaveComputations;
		text = text.ToLowerInvariant();
		byte[] bytes = Encoding.Unicode.GetBytes(text.ToLowerInvariant());
		SHA256Cng val = new SHA256Cng();
		try
		{
			((HashAlgorithm)(object)val).TransformFinalBlock(bytes, 0, bytes.Length);
			return ((HashAlgorithm)(object)val).Hash;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void ValidateEncryptionAlgorithm(string encryptionAlgorithm, bool isSystemOp)
	{
		if (encryptionAlgorithm == null)
		{
			throw SQL.NullKeyEncryptionAlgorithm(isSystemOp);
		}
		if (!string.Equals(encryptionAlgorithm, "RSA_OAEP", StringComparison.OrdinalIgnoreCase))
		{
			throw SQL.InvalidKeyEncryptionAlgorithm(encryptionAlgorithm, "RSA_OAEP", isSystemOp);
		}
	}

	private void ValidateCertificatePathLength(string masterKeyPath, bool isSystemOp)
	{
		if (masterKeyPath.Length >= 32767)
		{
			throw SQL.LargeCertificatePathLength(masterKeyPath.Length, 32767, isSystemOp);
		}
	}

	private string[] GetValidCertificateLocations()
	{
		return new string[2] { "LocalMachine", "CurrentUser" };
	}

	private void ValidateNonEmptyCertificatePath(string masterKeyPath, bool isSystemOp)
	{
		if (string.IsNullOrWhiteSpace(masterKeyPath))
		{
			if (masterKeyPath == null)
			{
				throw SQL.NullCertificatePath(GetValidCertificateLocations(), isSystemOp);
			}
			throw SQL.InvalidCertificatePath(masterKeyPath, GetValidCertificateLocations(), isSystemOp);
		}
	}

	private X509Certificate2 GetCertificateByPath(string keyPath, bool isSystemOp)
	{
		StoreLocation storeLocation = StoreLocation.LocalMachine;
		StoreName storeName = StoreName.My;
		string[] array = keyPath.Split(new char[1] { '/' });
		if (array.Length > 3)
		{
			throw SQL.InvalidCertificatePath(keyPath, GetValidCertificateLocations(), isSystemOp);
		}
		if (array.Length > 2)
		{
			if (string.Equals(array[0], "LocalMachine", StringComparison.OrdinalIgnoreCase))
			{
				storeLocation = StoreLocation.LocalMachine;
			}
			else
			{
				if (!string.Equals(array[0], "CurrentUser", StringComparison.OrdinalIgnoreCase))
				{
					throw SQL.InvalidCertificateLocation(array[0], keyPath, GetValidCertificateLocations(), isSystemOp);
				}
				storeLocation = StoreLocation.CurrentUser;
			}
		}
		if (array.Length > 1)
		{
			if (!string.Equals(array[^2], "My", StringComparison.OrdinalIgnoreCase))
			{
				throw SQL.InvalidCertificateStore(array[^2], keyPath, "My", isSystemOp);
			}
			storeName = StoreName.My;
		}
		string text = array[^1];
		if (string.IsNullOrEmpty(text))
		{
			throw SQL.EmptyCertificateThumbprint(keyPath, isSystemOp);
		}
		return GetCertificate(storeLocation, storeName, keyPath, text, isSystemOp);
	}

	private X509Certificate2 GetCertificate(StoreLocation storeLocation, StoreName storeName, string masterKeyPath, string thumbprint, bool isSystemOp)
	{
		X509Store x509Store = null;
		try
		{
			x509Store = new X509Store(storeName, storeLocation);
			x509Store.Open(OpenFlags.OpenExistingOnly);
			X509Certificate2Collection x509Certificate2Collection = x509Store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, validOnly: false);
			if (x509Certificate2Collection == null || x509Certificate2Collection.Count == 0)
			{
				throw SQL.CertificateNotFound(thumbprint, storeName.ToString(), storeLocation.ToString(), isSystemOp);
			}
			X509Certificate2 x509Certificate = x509Certificate2Collection[0];
			if (!x509Certificate.HasPrivateKey)
			{
				throw SQL.CertificateWithNoPrivateKey(masterKeyPath, isSystemOp);
			}
			return x509Certificate;
		}
		finally
		{
			x509Store?.Close();
		}
	}

	private byte[] RSAEncrypt(byte[] plainText, X509Certificate2 certificate)
	{
		RSACryptoServiceProvider rSACryptoServiceProvider = (RSACryptoServiceProvider)certificate.PublicKey.Key;
		return rSACryptoServiceProvider.Encrypt(plainText, fOAEP: true);
	}

	private byte[] RSADecrypt(byte[] cipherText, X509Certificate2 certificate)
	{
		RSACryptoServiceProvider rSACryptoServiceProvider = (RSACryptoServiceProvider)certificate.PrivateKey;
		return rSACryptoServiceProvider.Decrypt(cipherText, fOAEP: true);
	}

	private byte[] RSASignHashedData(byte[] dataToSign, X509Certificate2 certificate)
	{
		RSACryptoServiceProvider cSPFromCertificatePrivateKey = GetCSPFromCertificatePrivateKey(certificate);
		RSAPKCS1SignatureFormatter rSAPKCS1SignatureFormatter = new RSAPKCS1SignatureFormatter(cSPFromCertificatePrivateKey);
		rSAPKCS1SignatureFormatter.SetHashAlgorithm("SHA256");
		return rSAPKCS1SignatureFormatter.CreateSignature(dataToSign);
	}

	private bool RSAVerifySignature(byte[] dataToVerify, byte[] signature, X509Certificate2 certificate)
	{
		RSACryptoServiceProvider cSPFromCertificatePrivateKey = GetCSPFromCertificatePrivateKey(certificate);
		RSAPKCS1SignatureDeformatter rSAPKCS1SignatureDeformatter = new RSAPKCS1SignatureDeformatter(cSPFromCertificatePrivateKey);
		rSAPKCS1SignatureDeformatter.SetHashAlgorithm("SHA256");
		return rSAPKCS1SignatureDeformatter.VerifySignature(dataToVerify, signature);
	}

	private RSACryptoServiceProvider GetCSPFromCertificatePrivateKey(X509Certificate2 certificate)
	{
		CspParameters cspParameters = new CspParameters();
		cspParameters = new CspParameters();
		cspParameters.KeyContainerName = ((RSACryptoServiceProvider)certificate.PrivateKey).CspKeyContainerInfo.KeyContainerName;
		cspParameters.ProviderType = 24;
		cspParameters.KeyNumber = (int)((RSACryptoServiceProvider)certificate.PrivateKey).CspKeyContainerInfo.KeyNumber;
		if (((RSACryptoServiceProvider)certificate.PrivateKey).CspKeyContainerInfo.MachineKeyStore)
		{
			cspParameters.Flags = CspProviderFlags.UseMachineKeyStore;
		}
		else
		{
			cspParameters.Flags = CspProviderFlags.UseExistingKey;
		}
		return new RSACryptoServiceProvider(cspParameters);
	}
}
