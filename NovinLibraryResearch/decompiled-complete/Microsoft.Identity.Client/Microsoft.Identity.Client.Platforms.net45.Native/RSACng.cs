using System;
using System.Security;
using System.Security.Cryptography;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.Identity.Client.Platforms.net45.Native;

internal sealed class RSACng : RSA, ICngAsymmetricAlgorithm, ICngAlgorithm
{
	private static KeySizes[] s_legalKeySizes = new KeySizes[1]
	{
		new KeySizes(384, 16384, 8)
	};

	private CngKey m_key;

	private AsymmetricPaddingMode m_encryptionPaddingMode = AsymmetricPaddingMode.Oaep;

	private CngAlgorithm m_encryptionHashAlgorithm = CngAlgorithm.Sha256;

	private AsymmetricPaddingMode m_signaturePaddingMode = AsymmetricPaddingMode.Pkcs1;

	private CngAlgorithm m_signatureHashAlgorithm = CngAlgorithm.Sha256;

	private int m_signatureSaltBytes = 20;

	public CngAlgorithm EncryptionHashAlgorithm
	{
		get
		{
			return m_encryptionHashAlgorithm;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			m_encryptionHashAlgorithm = value;
		}
	}

	public AsymmetricPaddingMode EncryptionPaddingMode
	{
		get
		{
			return m_encryptionPaddingMode;
		}
		set
		{
			if (value != AsymmetricPaddingMode.Oaep && value != AsymmetricPaddingMode.Pkcs1)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			m_encryptionPaddingMode = value;
		}
	}

	public CngKey Key
	{
		[SecurityCritical]
		[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
		get
		{
			if (m_key != null && m_key.KeySize != KeySize)
			{
				m_key.Dispose();
				m_key = null;
			}
			if (m_key == null)
			{
				CngKeyCreationParameters cngKeyCreationParameters = new CngKeyCreationParameters();
				CngProperty item = new CngProperty("Length", BitConverter.GetBytes(KeySize), CngPropertyOptions.None);
				cngKeyCreationParameters.Parameters.Add(item);
				m_key = CngKey.Create(new CngAlgorithm("RSA"), null, cngKeyCreationParameters);
			}
			return m_key;
		}
		private set
		{
			if (value.AlgorithmGroup != CngAlgorithmGroup.Rsa)
			{
				throw new ArgumentException("Key Must Be Rsa", "value");
			}
			if (m_key != null)
			{
				m_key.Dispose();
			}
			m_key = value;
			KeySize = m_key.KeySize;
		}
	}

	private SafeNCryptKeyHandle KeyHandle
	{
		[SecurityCritical]
		[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
		get
		{
			return Key.Handle;
		}
	}

	public override string KeyExchangeAlgorithm => "RSA-PKCS1-KeyEx";

	public CngProvider Provider
	{
		[SecurityCritical]
		get
		{
			return Key.Provider;
		}
	}

	public override string SignatureAlgorithm => "http://www.w3.org/2000/09/xmldsig#rsa-sha1";

	public CngAlgorithm SignatureHashAlgorithm
	{
		get
		{
			return m_signatureHashAlgorithm;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			m_signatureHashAlgorithm = value;
		}
	}

	public AsymmetricPaddingMode SignaturePaddingMode
	{
		get
		{
			return m_signaturePaddingMode;
		}
		set
		{
			if (value != AsymmetricPaddingMode.Pkcs1 && value != AsymmetricPaddingMode.Pss)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			m_signaturePaddingMode = value;
		}
	}

	public int SignatureSaltBytes
	{
		get
		{
			return m_signatureSaltBytes;
		}
		set
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			m_signatureSaltBytes = value;
		}
	}

	public RSACng()
		: this(2048)
	{
	}

	public RSACng(int keySize)
	{
		LegalKeySizesValue = s_legalKeySizes;
		KeySize = keySize;
	}

	[SecurityCritical]
	public RSACng(CngKey key)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		LegalKeySizesValue = s_legalKeySizes;
		((CodeAccessPermission)new SecurityPermission(SecurityPermissionFlag.UnmanagedCode)).Assert();
		Key = CngKey.Open(key.Handle, key.IsEphemeral ? CngKeyHandleOpenOptions.EphemeralKey : CngKeyHandleOpenOptions.None);
		CodeAccessPermission.RevertAssert();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && m_key != null)
		{
			m_key.Dispose();
		}
	}

	private static KeyContainerPermission BuildKeyContainerPermission(CngKey key, KeyContainerPermissionFlags flags)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		if (key.IsEphemeral || string.IsNullOrEmpty(key.KeyName))
		{
			return null;
		}
		KeyContainerPermissionAccessEntry val = new KeyContainerPermissionAccessEntry(key.KeyName, flags);
		val.ProviderName = key.Provider.Provider;
		KeyContainerPermission val2 = new KeyContainerPermission(PermissionState.None);
		val2.AccessEntries.Add(val);
		return val2;
	}

	private HashAlgorithm CreateSignatureHashObject()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		if (m_signatureHashAlgorithm == CngAlgorithm.Sha256)
		{
			return (HashAlgorithm)new SHA256Cng();
		}
		if (m_signatureHashAlgorithm == CngAlgorithm.Sha384)
		{
			return (HashAlgorithm)new SHA384Cng();
		}
		if (m_signatureHashAlgorithm == CngAlgorithm.Sha512)
		{
			return (HashAlgorithm)new SHA512Cng();
		}
		throw new InvalidOperationException("Invalid Signature Hash Algorithm");
	}

	public byte[] SignData(byte[] data)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		return SignData(data, 0, data.Length);
	}

	public byte[] SignData(byte[] data, int offset, int count)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		if (offset < 0)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (count > data.Length - offset)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		using HashAlgorithm hashAlgorithm = CreateSignatureHashObject();
		byte[] hash = hashAlgorithm.ComputeHash(data, offset, count);
		return SignHash(hash);
	}

	public byte[] SignHash(byte[] hash)
	{
		return SignHash(hash, SignatureHashAlgorithm);
	}

	[SecurityCritical]
	public byte[] SignHash(byte[] hash, CngAlgorithm hashAlgorithm)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		if (hash == null)
		{
			throw new ArgumentNullException("hash");
		}
		if (hashAlgorithm == null)
		{
			throw new ArgumentNullException("hashAlgorithm");
		}
		CngKey key = Key;
		KeyContainerPermission val = BuildKeyContainerPermission(key, (KeyContainerPermissionFlags)256);
		if (val != null)
		{
			((CodeAccessPermission)val).Demand();
		}
		((CodeAccessPermission)new SecurityPermission(SecurityPermissionFlag.UnmanagedCode)).Assert();
		SafeNCryptKeyHandle handle = key.Handle;
		CodeAccessPermission.RevertAssert();
		return SignaturePaddingMode switch
		{
			AsymmetricPaddingMode.Pkcs1 => NCryptNative.SignHashPkcs1(handle, hash, hashAlgorithm.Algorithm), 
			AsymmetricPaddingMode.Pss => NCryptNative.SignHashPss(handle, hash, hashAlgorithm.Algorithm, SignatureSaltBytes), 
			_ => throw new InvalidOperationException("Unsupported Padding Mode"), 
		};
	}

	public override byte[] DecryptValue(byte[] rgb)
	{
		throw new NotImplementedException();
	}

	public override byte[] EncryptValue(byte[] rgb)
	{
		throw new NotImplementedException();
	}

	public override RSAParameters ExportParameters(bool includePrivateParameters)
	{
		throw new NotImplementedException();
	}

	public override void ImportParameters(RSAParameters parameters)
	{
		throw new NotImplementedException();
	}
}
