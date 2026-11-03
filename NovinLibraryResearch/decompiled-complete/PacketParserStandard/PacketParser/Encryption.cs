using System;
using System.Security.Cryptography;
using System.Text;
using PacketParser.Entities;

namespace PacketParser;

public static class Encryption
{
	private static string _oldPass = "NovinPassHT";

	public static string EncryptData(this string message, string pass = "sdfgfgjdjkfdhfnjlk@srtdf,mdfklgjdg8845345okjmvxm,c bmXDFDFGHDGDR ")
	{
		try
		{
			UTF8Encoding uTF8Encoding = new UTF8Encoding();
			MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
			byte[] key = mD5CryptoServiceProvider.ComputeHash(uTF8Encoding.GetBytes(pass));
			TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider
			{
				Key = key,
				Mode = CipherMode.ECB,
				Padding = PaddingMode.PKCS7
			};
			byte[] bytes = uTF8Encoding.GetBytes(message);
			byte[] inArray;
			try
			{
				inArray = tripleDESCryptoServiceProvider.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length);
			}
			finally
			{
				tripleDESCryptoServiceProvider.Clear();
				mD5CryptoServiceProvider.Clear();
			}
			return Convert.ToBase64String(inArray);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "EncryptData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Encryption.cs", 40);
			return "";
		}
	}

	public static string DecryptString(this string message, string pass = "sdfgfgjdjkfdhfnjlk@srtdf,mdfklgjdg8845345okjmvxm,c bmXDFDFGHDGDR ")
	{
		int num = 0;
		try
		{
			num = 1;
			if (string.IsNullOrEmpty(message))
			{
				return "";
			}
			num = 2;
			UTF8Encoding uTF8Encoding = new UTF8Encoding();
			num = 3;
			MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
			num = 4;
			byte[] key = mD5CryptoServiceProvider.ComputeHash(uTF8Encoding.GetBytes(pass));
			num = 5;
			TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider
			{
				Key = key,
				Mode = CipherMode.ECB,
				Padding = PaddingMode.PKCS7
			};
			num = 6;
			byte[] array = null;
			try
			{
				array = Convert.FromBase64String(message);
			}
			catch (Exception)
			{
				throw new Exception("data is not valid bas64\r\ndata:" + message);
			}
			num = 7;
			num = 8;
			byte[] bytes;
			try
			{
				num = 9;
				ICryptoTransform cryptoTransform = tripleDESCryptoServiceProvider.CreateDecryptor();
				num = 10;
				bytes = cryptoTransform.TransformFinalBlock(array, 0, array.Length);
				num = 11;
			}
			finally
			{
				num = 12;
				tripleDESCryptoServiceProvider.Clear();
				num = 13;
				mD5CryptoServiceProvider.Clear();
				num = 14;
			}
			num = 15;
			return uTF8Encoding.GetString(bytes);
		}
		catch (Exception ex2)
		{
			if (pass == "sdfgfgjdjkfdhfnjlk@srtdf,mdfklgjdg8845345okjmvxm,c bmXDFDFGHDGDR ")
			{
				return message.DecryptString(_oldPass);
			}
			WebErrorLog.Instance.StartLog(ex2, "Exception:" + ex2.Message + "\r\n" + string.Format("NewLog//Line:{0}//message:{1}//key:{2}", num, message ?? "Null!!!!", pass), "DecryptString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Encryption.cs", 102);
			return message;
		}
	}
}
