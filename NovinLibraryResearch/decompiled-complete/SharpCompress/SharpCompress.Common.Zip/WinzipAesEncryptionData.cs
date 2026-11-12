using System;
using System.Buffers.Binary;
using System.Security.Cryptography;

namespace SharpCompress.Common.Zip;

internal class WinzipAesEncryptionData
{
	private const int RFC2898_ITERATIONS = 1000;

	private readonly byte[] _salt;

	private readonly WinzipAesKeySize _keySize;

	private readonly byte[] _passwordVerifyValue;

	private readonly string _password;

	private byte[] _generatedVerifyValue;

	internal byte[] IvBytes { get; set; }

	internal byte[] KeyBytes { get; set; }

	private int KeySizeInBytes => KeyLengthInBytes(_keySize);

	internal WinzipAesEncryptionData(WinzipAesKeySize keySize, byte[] salt, byte[] passwordVerifyValue, string password)
	{
		_keySize = keySize;
		_salt = salt;
		_passwordVerifyValue = passwordVerifyValue;
		_password = password;
		Initialize();
	}

	internal static int KeyLengthInBytes(WinzipAesKeySize keySize)
	{
		return keySize switch
		{
			WinzipAesKeySize.KeySize128 => 16, 
			WinzipAesKeySize.KeySize192 => 24, 
			WinzipAesKeySize.KeySize256 => 32, 
			_ => throw new InvalidOperationException(), 
		};
	}

	private void Initialize()
	{
		Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(_password, _salt, 1000);
		KeyBytes = rfc2898DeriveBytes.GetBytes(KeySizeInBytes);
		IvBytes = rfc2898DeriveBytes.GetBytes(KeySizeInBytes);
		_generatedVerifyValue = rfc2898DeriveBytes.GetBytes(2);
		short num = BinaryPrimitives.ReadInt16LittleEndian(_passwordVerifyValue);
		if (_password != null)
		{
			short num2 = BinaryPrimitives.ReadInt16LittleEndian(_generatedVerifyValue);
			if (num != num2)
			{
				throw new InvalidFormatException("bad password");
			}
		}
	}
}
