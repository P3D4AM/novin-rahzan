using System;
using System.Collections.Concurrent;
using System.IO;
using System.Security.Cryptography;

namespace Microsoft.Data.SqlClient;

internal class SqlAeadAes256CbcHmac256Algorithm : SqlClientEncryptionAlgorithm
{
	internal const string AlgorithmName = "AEAD_AES_256_CBC_HMAC_SHA256";

	private const int _KeySizeInBytes = 32;

	private const int _BlockSizeInBytes = 16;

	private const int _MinimumCipherTextLengthInBytesNoAuthenticationTag = 33;

	private const int _MinimumCipherTextLengthInBytesWithAuthenticationTag = 65;

	private const CipherMode _cipherMode = CipherMode.CBC;

	private const PaddingMode _paddingMode = PaddingMode.PKCS7;

	private readonly bool _isDeterministic;

	private readonly byte _algorithmVersion;

	private readonly SqlAeadAes256CbcHmac256EncryptionKey _columnEncryptionKey;

	private readonly ConcurrentQueue<AesCryptoServiceProvider> _cryptoProviderPool;

	private static readonly byte[] _version = new byte[1] { 1 };

	private static readonly byte[] _versionSize = new byte[1] { 1 };

	internal SqlAeadAes256CbcHmac256Algorithm(SqlAeadAes256CbcHmac256EncryptionKey encryptionKey, SqlClientEncryptionType encryptionType, byte algorithmVersion)
	{
		_columnEncryptionKey = encryptionKey;
		_algorithmVersion = algorithmVersion;
		_version[0] = algorithmVersion;
		if (encryptionType == SqlClientEncryptionType.Deterministic)
		{
			_isDeterministic = true;
		}
		_cryptoProviderPool = new ConcurrentQueue<AesCryptoServiceProvider>();
	}

	internal override byte[] EncryptData(byte[] plainText)
	{
		return EncryptData(plainText, hasAuthenticationTag: true);
	}

	protected byte[] EncryptData(byte[] plainText, bool hasAuthenticationTag)
	{
		byte[] array = new byte[16];
		if (_isDeterministic)
		{
			SqlSecurityUtility.GetHMACWithSHA256(plainText, _columnEncryptionKey.IVKey, array);
		}
		else
		{
			SqlSecurityUtility.GenerateRandomBytes(array);
		}
		int num = plainText.Length / 16 + 1;
		int num2 = (hasAuthenticationTag ? 32 : 0);
		int num3 = 1 + num2;
		int num4 = num3 + 16;
		int num5 = 1 + num2 + array.Length + num * 16;
		byte[] array2 = new byte[num5];
		array2[0] = _algorithmVersion;
		Buffer.BlockCopy(array, 0, array2, num3, array.Length);
		if (!_cryptoProviderPool.TryDequeue(out var result))
		{
			result = new AesCryptoServiceProvider();
			try
			{
				result.Key = _columnEncryptionKey.EncryptionKey;
				result.Mode = CipherMode.CBC;
				result.Padding = PaddingMode.PKCS7;
			}
			catch (Exception)
			{
				result?.Dispose();
				throw;
			}
		}
		try
		{
			result.IV = array;
			using (ICryptoTransform cryptoTransform = result.CreateEncryptor())
			{
				int num6 = 0;
				int num7 = num4;
				if (num > 1)
				{
					num6 = (num - 1) * 16;
					num7 += cryptoTransform.TransformBlock(plainText, 0, num6, array2, num7);
				}
				byte[] array3 = cryptoTransform.TransformFinalBlock(plainText, num6, plainText.Length - num6);
				Buffer.BlockCopy(array3, 0, array2, num7, array3.Length);
				num7 += array3.Length;
			}
			if (hasAuthenticationTag)
			{
				using HMACSHA256 hMACSHA = new HMACSHA256(_columnEncryptionKey.MACKey);
				hMACSHA.TransformBlock(_version, 0, _version.Length, _version, 0);
				hMACSHA.TransformBlock(array, 0, array.Length, array, 0);
				hMACSHA.TransformBlock(array2, num4, num * 16, array2, num4);
				hMACSHA.TransformFinalBlock(_versionSize, 0, _versionSize.Length);
				byte[] hash = hMACSHA.Hash;
				Buffer.BlockCopy(hash, 0, array2, 1, num2);
			}
		}
		finally
		{
			_cryptoProviderPool.Enqueue(result);
		}
		return array2;
	}

	internal override byte[] DecryptData(byte[] cipherText)
	{
		return DecryptData(cipherText, hasAuthenticationTag: true);
	}

	protected byte[] DecryptData(byte[] cipherText, bool hasAuthenticationTag)
	{
		byte[] array = new byte[16];
		int num = (hasAuthenticationTag ? 65 : 33);
		if (cipherText.Length < num)
		{
			throw SQL.InvalidCipherTextSize(cipherText.Length, num);
		}
		int num2 = 0;
		if (cipherText[num2] != _algorithmVersion)
		{
			throw SQL.InvalidAlgorithmVersion(cipherText[num2], _algorithmVersion);
		}
		num2++;
		int buffer2Index = 0;
		if (hasAuthenticationTag)
		{
			buffer2Index = num2;
			num2 += 32;
		}
		Buffer.BlockCopy(cipherText, num2, array, 0, array.Length);
		num2 += array.Length;
		int offset = num2;
		int num3 = cipherText.Length - num2;
		if (hasAuthenticationTag)
		{
			byte[] array2 = PrepareAuthenticationTag(array, cipherText, offset, num3);
			if (!SqlSecurityUtility.CompareBytes(array2, cipherText, buffer2Index, array2.Length))
			{
				throw SQL.InvalidAuthenticationTag();
			}
		}
		return DecryptData(array, cipherText, offset, num3);
	}

	private byte[] DecryptData(byte[] iv, byte[] cipherText, int offset, int count)
	{
		if (!_cryptoProviderPool.TryDequeue(out var result))
		{
			result = new AesCryptoServiceProvider();
			try
			{
				result.Key = _columnEncryptionKey.EncryptionKey;
				result.Mode = CipherMode.CBC;
				result.Padding = PaddingMode.PKCS7;
			}
			catch (Exception)
			{
				result?.Dispose();
				throw;
			}
		}
		try
		{
			result.IV = iv;
			using MemoryStream memoryStream = new MemoryStream();
			using ICryptoTransform transform = result.CreateDecryptor();
			using CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
			cryptoStream.Write(cipherText, offset, count);
			cryptoStream.FlushFinalBlock();
			return memoryStream.ToArray();
		}
		finally
		{
			_cryptoProviderPool.Enqueue(result);
		}
	}

	private byte[] PrepareAuthenticationTag(byte[] iv, byte[] cipherText, int offset, int length)
	{
		byte[] array = new byte[32];
		byte[] hash;
		using (HMACSHA256 hMACSHA = new HMACSHA256(_columnEncryptionKey.MACKey))
		{
			int num = 0;
			num = hMACSHA.TransformBlock(_version, 0, _version.Length, _version, 0);
			num = hMACSHA.TransformBlock(iv, 0, iv.Length, iv, 0);
			num = hMACSHA.TransformBlock(cipherText, offset, length, cipherText, offset);
			hMACSHA.TransformFinalBlock(_versionSize, 0, _versionSize.Length);
			hash = hMACSHA.Hash;
		}
		Buffer.BlockCopy(hash, 0, array, 0, array.Length);
		return array;
	}
}
