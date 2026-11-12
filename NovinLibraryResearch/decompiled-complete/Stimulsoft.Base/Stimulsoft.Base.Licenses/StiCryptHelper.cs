using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using Stimulsoft.Base.Helpers;

namespace Stimulsoft.Base.Licenses;

public static class StiCryptHelper
{
	private const string encryptionKey = "fheur89fhsodifjs04ejfdszifhv8urg";

	public static byte[] Encrypt(byte[] bytes, string password = null)
	{
		if (string.IsNullOrWhiteSpace(password))
		{
			password = "fheur89fhsodifjs04ejfdszifhv8urg";
		}
		using SymmetricAlgorithm symmetricAlgorithm = CreateAlgorithm();
		using ICryptoTransform transform = symmetricAlgorithm.CreateEncryptor(new PasswordDeriveBytes(password, null).GetBytes(16), new byte[16]);
		using MemoryStream memoryStream = new MemoryStream();
		using CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
		cryptoStream.Write(bytes, 0, bytes.Length);
		cryptoStream.FlushFinalBlock();
		return memoryStream.ToArray();
	}

	public static byte[] Decrypt(byte[] bytes, string password = null)
	{
		if (string.IsNullOrWhiteSpace(password))
		{
			password = "fheur89fhsodifjs04ejfdszifhv8urg";
		}
		using SymmetricAlgorithm symmetricAlgorithm = CreateAlgorithm();
		using ICryptoTransform transform = symmetricAlgorithm.CreateDecryptor(new PasswordDeriveBytes(password, null).GetBytes(16), new byte[16]);
		using MemoryStream stream = new MemoryStream(bytes);
		using CryptoStream input = new CryptoStream(stream, transform, CryptoStreamMode.Read);
		using BinaryReader reader = new BinaryReader(input);
		return reader.ReadAllBytes();
	}

	public static byte[] Recrypt(byte[] bytes, string oldPassword, string newPassword)
	{
		bytes = Decrypt(bytes, oldPassword);
		return Encrypt(bytes, newPassword);
	}

	public static string Encrypt(string str, string password = null)
	{
		byte[] bytes = StiBytesToStringConverter.ConvertStringToBytes(str);
		byte[] inArray = Encrypt(bytes, password);
		return Convert.ToBase64String(inArray);
	}

	public static string Decrypt(string str, string password = null)
	{
		if (string.IsNullOrWhiteSpace(password))
		{
			password = "fheur89fhsodifjs04ejfdszifhv8urg";
		}
		byte[] buffer = Convert.FromBase64String(str);
		using SymmetricAlgorithm symmetricAlgorithm = CreateAlgorithm();
		using ICryptoTransform transform = symmetricAlgorithm.CreateDecryptor(new PasswordDeriveBytes(password, null).GetBytes(16), new byte[16]);
		using MemoryStream stream = new MemoryStream(buffer);
		using CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read);
		using StreamReader streamReader = new StreamReader(stream2);
		return streamReader.ReadToEnd();
	}

	public static string Recrypt(string str, string oldPassword, string newPassword)
	{
		str = Decrypt(str, oldPassword);
		return Encrypt(str, newPassword);
	}

	internal static byte[] ReadAllBytes(this BinaryReader reader)
	{
		int num = 4096;
		using MemoryStream memoryStream = new MemoryStream();
		byte[] array = new byte[num];
		int count;
		while ((count = reader.Read(array, 0, array.Length)) != 0)
		{
			memoryStream.Write(array, 0, count);
		}
		return memoryStream.ToArray();
	}

	internal static string MD5(string value)
	{
		return StiMD5Helper.ComputeHash(value);
	}

	private static SymmetricAlgorithm CreateAlgorithm()
	{
		try
		{
			return Rijndael.Create();
		}
		catch (TargetInvocationException)
		{
			return new AesCryptoServiceProvider();
		}
	}
}
