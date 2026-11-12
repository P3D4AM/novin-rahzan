using System;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens;

public class SymmetricKeyWrapProvider : KeyWrapProvider
{
	private static readonly byte[] _defaultIV = new byte[8] { 166, 166, 166, 166, 166, 166, 166, 166 };

	private const int _blockSizeInBits = 64;

	private const int _blockSizeInBytes = 8;

	private static object _encryptorLock = new object();

	private static object _decryptorLock = new object();

	private SymmetricAlgorithm _symmetricAlgorithm;

	private ICryptoTransform _symmetricAlgorithmEncryptor;

	private ICryptoTransform _symmetricAlgorithmDecryptor;

	private bool _disposed;

	public override string Algorithm { get; }

	public override string Context { get; set; }

	public override SecurityKey Key { get; }

	public SymmetricKeyWrapProvider(SecurityKey key, string algorithm)
	{
		if (key == null)
		{
			throw LogHelper.LogArgumentNullException("key");
		}
		if (string.IsNullOrEmpty(algorithm))
		{
			throw LogHelper.LogArgumentNullException("algorithm");
		}
		if (!IsSupportedAlgorithm(key, algorithm))
		{
			throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant("IDX10661: Unable to create the KeyWrapProvider.\nKeyWrapAlgorithm: '{0}', SecurityKey: '{1}'\n is not supported.", algorithm, key)));
		}
		Algorithm = algorithm;
		Key = key;
		_symmetricAlgorithm = GetSymmetricAlgorithm(key, algorithm);
		if (_symmetricAlgorithm == null)
		{
			throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant("IDX10669: Failed to create symmetric algorithm.")));
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (!_disposed && disposing)
		{
			if (_symmetricAlgorithm != null)
			{
				_symmetricAlgorithm.Dispose();
				_symmetricAlgorithm = null;
			}
			if (_symmetricAlgorithmEncryptor != null)
			{
				_symmetricAlgorithmEncryptor.Dispose();
				_symmetricAlgorithmEncryptor = null;
			}
			if (_symmetricAlgorithmDecryptor != null)
			{
				_symmetricAlgorithmDecryptor.Dispose();
				_symmetricAlgorithmDecryptor = null;
			}
			_disposed = true;
		}
	}

	private static byte[] GetBytes(ulong i)
	{
		byte[] bytes = BitConverter.GetBytes(i);
		if (BitConverter.IsLittleEndian)
		{
			Array.Reverse((Array)bytes);
		}
		return bytes;
	}

	protected virtual SymmetricAlgorithm GetSymmetricAlgorithm(SecurityKey key, string algorithm)
	{
		if (key == null)
		{
			throw LogHelper.LogArgumentNullException("key");
		}
		byte[] array = null;
		SecurityKey key2;
		if (key is SymmetricSecurityKey symmetricSecurityKey)
		{
			array = symmetricSecurityKey.Key;
		}
		else if (key is JsonWebKey webKey && JsonWebKeyConverter.TryConvertToSymmetricSecurityKey(webKey, out key2))
		{
			array = (key2 as SymmetricSecurityKey).Key;
		}
		if (array == null)
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX10657: The SecurityKey provided for the symmetric key wrap algorithm cannot be converted to byte array. Type is: '{0}'.", key.GetType())));
		}
		ValidateKeySize(array, algorithm);
		try
		{
			Aes aes = Aes.Create();
			aes.Mode = CipherMode.ECB;
			aes.Padding = PaddingMode.None;
			aes.KeySize = array.Length * 8;
			aes.Key = array;
			byte[] array2 = new byte[aes.BlockSize >> 3];
			Utility.Zero(array2);
			aes.IV = array2;
			return aes;
		}
		catch (Exception innerException)
		{
			throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant("IDX10663: Failed to create symmetric algorithm with SecurityKey: '{0}', KeyWrapAlgorithm: '{1}'.", key, algorithm), innerException));
		}
	}

	protected virtual bool IsSupportedAlgorithm(SecurityKey key, string algorithm)
	{
		return SupportedAlgorithms.IsSupportedSymmetricKeyWrap(algorithm, key);
	}

	public override byte[] UnwrapKey(byte[] keyBytes)
	{
		if (keyBytes == null || keyBytes.Length == 0)
		{
			throw LogHelper.LogArgumentNullException("keyBytes");
		}
		if (keyBytes.Length % 8 != 0)
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX10664: The length of input must be a multiple of 64 bits. The input size is: '{0}' bits.", keyBytes.Length << 3), "keyBytes"));
		}
		if (_disposed)
		{
			throw LogHelper.LogExceptionMessage(new ObjectDisposedException(GetType().ToString()));
		}
		try
		{
			return UnwrapKeyPrivate(keyBytes, 0, keyBytes.Length);
		}
		catch (Exception ex)
		{
			throw LogHelper.LogExceptionMessage(new SecurityTokenKeyWrapException(LogHelper.FormatInvariant("IDX10659: UnwrapKey failed, exception from cryptographic operation: '{0}'", ex)));
		}
	}

	private byte[] UnwrapKeyPrivate(byte[] inputBuffer, int inputOffset, int inputCount)
	{
		byte[] array = new byte[8];
		Array.Copy(inputBuffer, inputOffset, array, 0, 8);
		int num = inputCount - 8 >> 3;
		byte[] array2 = new byte[num << 3];
		Array.Copy(inputBuffer, inputOffset + 8, array2, 0, inputCount - 8);
		if (_symmetricAlgorithmDecryptor == null)
		{
			lock (_decryptorLock)
			{
				if (_symmetricAlgorithmDecryptor == null)
				{
					_symmetricAlgorithmDecryptor = _symmetricAlgorithm.CreateDecryptor();
				}
			}
		}
		byte[] array3 = new byte[16];
		for (int num2 = 5; num2 >= 0; num2--)
		{
			for (int num3 = num; num3 > 0; num3--)
			{
				ulong i = (ulong)(num * num2 + num3);
				Utility.Xor(array, GetBytes(i), 0, inPlace: true);
				Array.Copy(array, array3, 8);
				Array.Copy(array2, num3 - 1 << 3, array3, 8, 8);
				byte[] sourceArray = _symmetricAlgorithmDecryptor.TransformFinalBlock(array3, 0, 16);
				Array.Copy(sourceArray, array, 8);
				Array.Copy(sourceArray, 8, array2, num3 - 1 << 3, 8);
			}
		}
		if (Utility.AreEqual(array, _defaultIV))
		{
			byte[] array4 = new byte[num << 3];
			for (int j = 0; j < num; j++)
			{
				Array.Copy(array2, j << 3, array4, j << 3, 8);
			}
			return array4;
		}
		throw LogHelper.LogExceptionMessage(new InvalidOperationException("IDX10665: Data is not authentic"));
	}

	private void ValidateKeySize(byte[] key, string algorithm)
	{
		if ("A128KW".Equals(algorithm, StringComparison.Ordinal) || "http://www.w3.org/2001/04/xmlenc#kw-aes128".Equals(algorithm, StringComparison.Ordinal))
		{
			if (key.Length != 16)
			{
				throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException("key", LogHelper.FormatInvariant("IDX10662: The KeyWrap algorithm '{0}' requires a key size of '{1}' bits. Key '{2}', is of size:'{3}'.", algorithm, 128, Key.KeyId, key.Length << 3)));
			}
			return;
		}
		if ("A256KW".Equals(algorithm, StringComparison.Ordinal) || "http://www.w3.org/2001/04/xmlenc#kw-aes256".Equals(algorithm, StringComparison.Ordinal))
		{
			if (key.Length != 32)
			{
				throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException("key", LogHelper.FormatInvariant("IDX10662: The KeyWrap algorithm '{0}' requires a key size of '{1}' bits. Key '{2}', is of size:'{3}'.", algorithm, 256, Key.KeyId, key.Length << 3)));
			}
			return;
		}
		throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException("algorithm", LogHelper.FormatInvariant("IDX10652: The algorithm '{0}' is not supported.", algorithm)));
	}

	public override byte[] WrapKey(byte[] keyBytes)
	{
		if (keyBytes == null || keyBytes.Length == 0)
		{
			throw LogHelper.LogArgumentNullException("keyBytes");
		}
		if (keyBytes.Length % 8 != 0)
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX10664: The length of input must be a multiple of 64 bits. The input size is: '{0}' bits.", keyBytes.Length << 3), "keyBytes"));
		}
		if (_disposed)
		{
			throw LogHelper.LogExceptionMessage(new ObjectDisposedException(GetType().ToString()));
		}
		try
		{
			return WrapKeyPrivate(keyBytes, 0, keyBytes.Length);
		}
		catch (Exception ex)
		{
			throw LogHelper.LogExceptionMessage(new SecurityTokenKeyWrapException(LogHelper.FormatInvariant("IDX10658: WrapKey failed, exception from cryptographic operation: '{0}'", ex)));
		}
	}

	private byte[] WrapKeyPrivate(byte[] inputBuffer, int inputOffset, int inputCount)
	{
		byte[] array = _defaultIV.Clone() as byte[];
		int num = inputCount >> 3;
		byte[] array2 = new byte[num << 3];
		Array.Copy(inputBuffer, inputOffset, array2, 0, inputCount);
		if (_symmetricAlgorithmEncryptor == null)
		{
			lock (_encryptorLock)
			{
				if (_symmetricAlgorithmEncryptor == null)
				{
					_symmetricAlgorithmEncryptor = _symmetricAlgorithm.CreateEncryptor();
				}
			}
		}
		byte[] array3 = new byte[16];
		for (int i = 0; i < 6; i++)
		{
			for (int j = 0; j < num; j++)
			{
				ulong i2 = (ulong)(num * i + j + 1);
				Array.Copy(array, array3, array.Length);
				Array.Copy(array2, j << 3, array3, 8, 8);
				byte[] sourceArray = _symmetricAlgorithmEncryptor.TransformFinalBlock(array3, 0, 16);
				Array.Copy(sourceArray, array, 8);
				Utility.Xor(array, GetBytes(i2), 0, inPlace: true);
				Array.Copy(sourceArray, 8, array2, j << 3, 8);
			}
		}
		byte[] array4 = new byte[num + 1 << 3];
		Array.Copy(array, array4, array.Length);
		for (int k = 0; k < num; k++)
		{
			Array.Copy(array2, k << 3, array4, k + 1 << 3, 8);
		}
		return array4;
	}
}
