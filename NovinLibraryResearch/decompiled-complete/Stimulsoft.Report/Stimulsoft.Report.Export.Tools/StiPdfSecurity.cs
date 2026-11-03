using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Export.Tools;

[SuppressUnmanagedCodeSecurity]
public class StiPdfSecurity
{
	private class Win32
	{
		public struct CERT_CONTEXT
		{
			public uint dwCertEncodingType;

			public IntPtr pbCertEncoded;

			public uint cbCertEncoded;

			public IntPtr pCertInfo;

			public IntPtr hCertStore;
		}

		public struct CRYPT_OBJID_BLOB
		{
			public uint cbData;

			public IntPtr pbData;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct CRYPT_ALGORITHM_IDENTIFIER
		{
			[MarshalAs(UnmanagedType.LPStr)]
			public string pszObjId;

			public CRYPT_OBJID_BLOB Parameters;
		}

		public struct CRYPT_SIGN_MESSAGE_PARA
		{
			public uint cbSize;

			public uint dwMsgEncodingType;

			public IntPtr pSigningCert;

			public CRYPT_ALGORITHM_IDENTIFIER HashAlgorithm;

			public IntPtr pvHashAuxInfo;

			public uint cMsgCert;

			public IntPtr rgpMsgCert;

			public uint cMsgCrl;

			public IntPtr rgpMsgCrl;

			public uint cAuthAttr;

			public IntPtr rgAuthAttr;

			public uint cUnauthAttr;

			public IntPtr rgUnauthAttr;

			public uint dwFlags;

			public uint dwInnerContentType;

			public CRYPT_ALGORITHM_IDENTIFIER HashEncryptionAlgorithm;

			public IntPtr pvHashEncryptionAuxInfo;
		}

		public const uint PKCS_7_ASN_ENCODING = 65536u;

		public const uint X509_ASN_ENCODING = 1u;

		public const uint CERT_SYSTEM_STORE_CURRENT_USER = 65536u;

		public const uint CERT_SYSTEM_STORE_LOCAL_MACHINE = 131072u;

		public const uint CERT_FIND_SUBJECT_NAME = 131079u;

		public const uint CERT_FIND_SUBJECT_STR = 524295u;

		public const int CERT_STORE_PROV_SYSTEM = 10;

		public const uint CERT_STORE_READONLY_FLAG = 32768u;

		public const uint CERT_CLOSE_STORE_CHECK_FLAG = 2u;

		public const string szOID_RSA_SHA1RSA = "1.2.840.113549.1.1.5";

		public const string szOID_sha256NoSign = "2.16.840.1.101.3.4.2.1";

		public const uint CRYPTUI_SELECT_ISSUEDTO_COLUMN = 1u;

		public const uint CRYPTUI_SELECT_ISSUEDBY_COLUMN = 2u;

		public const uint CRYPTUI_SELECT_INTENDEDUSE_COLUMN = 4u;

		public const uint CRYPTUI_SELECT_FRIENDLYNAME_COLUMN = 8u;

		public const uint CRYPTUI_SELECT_LOCATION_COLUMN = 16u;

		public const uint CRYPTUI_SELECT_EXPIRATION_COLUMN = 32u;

		public const string szOID_CP_GOST_R3411 = "1.2.643.2.2.9";

		public const string szOID_CP_GOST_R3411_R3410 = "1.2.643.2.2.4";

		public const string szOID_CP_GOST_R3411_R3410EL = "1.2.643.2.2.3";

		[DllImport("crypt32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		public static extern bool CryptSignMessage(ref CRYPT_SIGN_MESSAGE_PARA pSignPara, bool fDetachedSignature, int cToBeSigned, IntPtr[] rgpbToBeSigned, int[] rgcbToBeSigned, byte[] pbSignedBlob, ref int pcbSignedBlob);

		[DllImport("Crypt32.dll", SetLastError = true)]
		public static extern IntPtr CertOpenStore(IntPtr lpszStoreProvider, uint dwEncodingType, IntPtr hCryptProv, uint dwFlags, byte[] pvPara);

		[DllImport("Crypt32.dll", SetLastError = true)]
		public static extern IntPtr CertFindCertificateInStore(IntPtr hCertStore, uint dwCertEncodingType, uint dwFindFlags, uint dwFindType, IntPtr pvFindPara, IntPtr pPrevCertContext);

		[DllImport("crypt32.dll", SetLastError = true)]
		public static extern int CertCloseStore(IntPtr hCertStore, uint dwFlags);

		[DllImport("crypt32.dll", SetLastError = true)]
		public static extern int CertFreeCertificateContext(IntPtr pCertContext);

		[DllImport("cryptUI.dll", SetLastError = true)]
		public static extern IntPtr CryptUIDlgSelectCertificateFromStore(IntPtr hCertStore, IntPtr hwnd, [MarshalAs(UnmanagedType.LPWStr)] string pwszTitle, [MarshalAs(UnmanagedType.LPWStr)] string pwszDisplayString, uint dwDontUseColumn, uint dwFlags, IntPtr pvReserved);

		[DllImport("crypt32.dll")]
		public static extern int CertVerifyTimeValidity(IntPtr pTimeToVerify, IntPtr pCertInfo);
	}

	public class StiCertificateInfo
	{
		public string Name;

		public string Issuer;

		public DateTime From;

		public DateTime To;

		public string Thumbprint;
	}

	private static byte[] paddingString = new byte[32]
	{
		40, 191, 78, 94, 78, 117, 138, 65, 100, 0,
		78, 86, 255, 250, 1, 8, 46, 46, 0, 182,
		208, 104, 62, 128, 47, 12, 169, 254, 100, 83,
		105, 122
	};

	private byte[] ownerValue = new byte[48];

	private byte[] userValue = new byte[48];

	private byte[] ownerExtendedValue = new byte[32];

	private byte[] userExtendedValue = new byte[32];

	private byte[] permsValue = new byte[16];

	private byte[] encryptionKey;

	private byte[] IDValue;

	private string passwordOwner = string.Empty;

	private string passwordUser = string.Empty;

	private uint securityFlags;

	private StiPdfEncryptionKeyLength keyLength = StiPdfEncryptionKeyLength.Bit40;

	private StiPdfExportService pdfService;

	private byte[] PadPassword(string InputPassword)
	{
		StringBuilder stringBuilder = new StringBuilder(InputPassword);
		for (int i = 0; i < paddingString.Length; i++)
		{
			stringBuilder.Append((char)paddingString[i]);
		}
		if (stringBuilder.Length > 32)
		{
			stringBuilder.Remove(32, stringBuilder.Length - 32);
		}
		byte[] array = new byte[32];
		for (int j = 0; j < 32; j++)
		{
			array[j] = (byte)stringBuilder[j];
		}
		return array;
	}

	public bool ComputingCryptoValues(StiUserAccessPrivileges userAccessPrivileges, string passwordInputOwner, string passwordInputUser, StiPdfEncryptionKeyLength keyLength, byte[] IDValue)
	{
		securityFlags = 4294967232u;
		uint num = 0u;
		if ((userAccessPrivileges & StiUserAccessPrivileges.PrintDocument) != 0)
		{
			num |= 4u;
		}
		if ((userAccessPrivileges & StiUserAccessPrivileges.ModifyContents) != 0)
		{
			num |= 8u;
		}
		if ((userAccessPrivileges & StiUserAccessPrivileges.CopyTextAndGraphics) != 0)
		{
			num |= 0x10u;
		}
		if ((userAccessPrivileges & StiUserAccessPrivileges.AddOrModifyTextAnnotations) != 0)
		{
			num |= 0x20u;
		}
		securityFlags |= num;
		if (passwordInputOwner == null)
		{
			passwordInputOwner = string.Empty;
		}
		if (passwordInputUser == null)
		{
			passwordInputUser = string.Empty;
		}
		passwordOwner = passwordInputOwner;
		passwordUser = passwordInputUser;
		this.keyLength = keyLength;
		this.IDValue = IDValue;
		bool result = false;
		if (passwordOwner.Length > 0 || passwordUser.Length > 0 || userAccessPrivileges != StiUserAccessPrivileges.All)
		{
			result = true;
			if (!ComputingCryptoValues2())
			{
				result = false;
				throw new Exception("Encryption not work!");
			}
		}
		return result;
	}

	private bool ComputingCryptoValues2()
	{
		if (keyLength == StiPdfEncryptionKeyLength.Bit128_r4)
		{
			return ComputingCryptoValuesV4();
		}
		if (keyLength == StiPdfEncryptionKeyLength.Bit256_r5 || keyLength == StiPdfEncryptionKeyLength.Bit256_r6)
		{
			return ComputingCryptoValuesV5();
		}
		string inputPassword = passwordOwner;
		if (passwordOwner == string.Empty)
		{
			inputPassword = passwordUser;
		}
		byte[] array = PadPassword(inputPassword);
		byte[] array2 = StiMD5Helper.ComputeHash(array);
		if (keyLength == StiPdfEncryptionKeyLength.Bit128)
		{
			for (int i = 0; i < 50; i++)
			{
				array2 = StiMD5Helper.ComputeHash(array2);
			}
		}
		if (keyLength == StiPdfEncryptionKeyLength.Bit40)
		{
			byte[] array3 = new byte[5];
			Array.Copy(array2, array3, 5);
			array2 = array3;
		}
		byte[] bytes = PadPassword(passwordUser);
		StiEncryption.RC4(ref bytes, array2);
		if (keyLength == StiPdfEncryptionKeyLength.Bit128)
		{
			for (int j = 1; j <= 19; j++)
			{
				for (int k = 0; k < 16; k++)
				{
					array2[k] ^= (byte)(j - 1);
					array2[k] ^= (byte)j;
				}
				StiEncryption.RC4(ref bytes, array2);
			}
		}
		bytes.CopyTo(ownerValue, 0);
		string inputPassword2 = passwordUser;
		byte[] array4 = PadPassword(inputPassword2);
		byte[] bytes2 = BitConverter.GetBytes(securityFlags);
		MemoryStream memoryStream = new MemoryStream();
		memoryStream.Write(array4, 0, array4.Length);
		memoryStream.Write(bytes, 0, bytes.Length);
		memoryStream.Write(bytes2, 0, bytes2.Length);
		memoryStream.Write(IDValue, 0, IDValue.Length);
		memoryStream.Seek(0L, SeekOrigin.Begin);
		array2 = StiMD5Helper.ComputeHash((Stream)memoryStream);
		memoryStream.Close();
		if (keyLength == StiPdfEncryptionKeyLength.Bit128)
		{
			for (int l = 0; l < 50; l++)
			{
				array2 = StiMD5Helper.ComputeHash(array2);
			}
		}
		encryptionKey = ((keyLength == StiPdfEncryptionKeyLength.Bit128) ? new byte[16] : new byte[5]);
		for (int m = 0; m < encryptionKey.Length; m++)
		{
			encryptionKey[m] = array2[m];
		}
		byte[] bytes3 = new byte[32];
		if (keyLength == StiPdfEncryptionKeyLength.Bit128)
		{
			memoryStream = new MemoryStream();
			memoryStream.Write(paddingString, 0, paddingString.Length);
			memoryStream.Write(IDValue, 0, IDValue.Length);
			memoryStream.Seek(0L, SeekOrigin.Begin);
			array2 = StiMD5Helper.ComputeHash((Stream)memoryStream);
			memoryStream.Close();
			StiEncryption.RC4(ref array2, encryptionKey);
			byte[] array5 = new byte[16];
			for (int n = 1; n <= 19; n++)
			{
				for (int num = 0; num < 16; num++)
				{
					array5[num] = (byte)(encryptionKey[num] ^ (byte)n);
				}
				StiEncryption.RC4(ref array2, array5);
			}
			paddingString.CopyTo(bytes3, 0);
			array2.CopyTo(bytes3, 0);
		}
		else
		{
			paddingString.CopyTo(bytes3, 0);
			StiEncryption.RC4(ref bytes3, encryptionKey);
		}
		bytes3.CopyTo(userValue, 0);
		return true;
	}

	private bool ComputingCryptoValuesV4()
	{
		string inputPassword = passwordOwner;
		if (passwordOwner == string.Empty)
		{
			inputPassword = passwordUser;
		}
		byte[] array = PadPassword(inputPassword);
		byte[] array2 = StiMD5Helper.ComputeHash(array);
		for (int i = 0; i < 50; i++)
		{
			array2 = StiMD5Helper.ComputeHash(array2);
		}
		byte[] bytes = PadPassword(passwordUser);
		StiEncryption.RC4(ref bytes, array2);
		for (int j = 1; j <= 19; j++)
		{
			for (int k = 0; k < 16; k++)
			{
				array2[k] ^= (byte)(j - 1);
				array2[k] ^= (byte)j;
			}
			StiEncryption.RC4(ref bytes, array2);
		}
		bytes.CopyTo(ownerValue, 0);
		string inputPassword2 = passwordUser;
		byte[] array3 = PadPassword(inputPassword2);
		byte[] bytes2 = BitConverter.GetBytes(securityFlags);
		MemoryStream memoryStream = new MemoryStream();
		memoryStream.Write(array3, 0, array3.Length);
		memoryStream.Write(bytes, 0, bytes.Length);
		memoryStream.Write(bytes2, 0, bytes2.Length);
		memoryStream.Write(IDValue, 0, IDValue.Length);
		memoryStream.Seek(0L, SeekOrigin.Begin);
		array2 = StiMD5Helper.ComputeHash((Stream)memoryStream);
		memoryStream.Close();
		for (int l = 0; l < 50; l++)
		{
			array2 = StiMD5Helper.ComputeHash(array2);
		}
		encryptionKey = new byte[16];
		for (int m = 0; m < encryptionKey.Length; m++)
		{
			encryptionKey[m] = array2[m];
		}
		byte[] array4 = new byte[32];
		memoryStream = new MemoryStream();
		memoryStream.Write(paddingString, 0, paddingString.Length);
		memoryStream.Write(IDValue, 0, IDValue.Length);
		memoryStream.Seek(0L, SeekOrigin.Begin);
		array2 = StiMD5Helper.ComputeHash((Stream)memoryStream);
		memoryStream.Close();
		StiEncryption.RC4(ref array2, encryptionKey);
		byte[] array5 = new byte[16];
		for (int n = 1; n <= 19; n++)
		{
			for (int num = 0; num < 16; num++)
			{
				array5[num] = (byte)(encryptionKey[num] ^ (byte)n);
			}
			StiEncryption.RC4(ref array2, array5);
		}
		paddingString.CopyTo(array4, 0);
		array2.CopyTo(array4, 0);
		array4.CopyTo(userValue, 0);
		return true;
	}

	private bool ComputingCryptoValuesV5()
	{
		Random random = new Random();
		byte[] array = null;
		byte[] array2 = null;
		byte[] array3 = new byte[32];
		random.NextBytes(array3);
		byte[] array4 = new byte[8];
		byte[] array5 = new byte[8];
		byte[] array6 = new byte[8];
		byte[] array7 = new byte[8];
		random.NextBytes(array4);
		random.NextBytes(array5);
		random.NextBytes(array6);
		random.NextBytes(array7);
		byte[] array8 = Encoding.UTF8.GetBytes(passwordUser);
		if (array8.Length > 127)
		{
			byte[] array9 = new byte[127];
			Array.Copy(array8, 0, array9, 0, 127);
			array8 = array9;
		}
		array2 = GetHashV5(array8, array4, null);
		array2.CopyTo(userValue, 0);
		array4.CopyTo(userValue, 32);
		array5.CopyTo(userValue, 40);
		array2 = GetHashV5(array8, array5, null);
		EncodeKeyDataV5(array2, array3, cbc: true).CopyTo(userExtendedValue, 0);
		string s = passwordOwner;
		if (passwordOwner == string.Empty)
		{
			s = passwordUser;
		}
		byte[] array10 = Encoding.UTF8.GetBytes(s);
		if (array10.Length > 127)
		{
			byte[] array11 = new byte[127];
			Array.Copy(array10, 0, array11, 0, 127);
			array10 = array11;
		}
		array2 = GetHashV5(array10, array6, userValue);
		array2.CopyTo(ownerValue, 0);
		array6.CopyTo(ownerValue, 32);
		array7.CopyTo(ownerValue, 40);
		array2 = GetHashV5(array10, array7, userValue);
		EncodeKeyDataV5(array2, array3, cbc: true).CopyTo(ownerExtendedValue, 0);
		array = new byte[16];
		random.NextBytes(array);
		ulong value = securityFlags | 0xFFFFFFFF00000000uL;
		BitConverter.GetBytes(value).CopyTo(array, 0);
		array[8] = 84;
		array[9] = 97;
		array[10] = 100;
		array[11] = 98;
		permsValue = EncodeKeyDataV5(array3, array, cbc: false);
		array2 = GetHashV5(array8, array5, null);
		encryptionKey = DecodeKeyDataV5(array2, userExtendedValue, cbc: true);
		array = DecodeKeyDataV5(encryptionKey, permsValue, cbc: false);
		if (array[9] != 97 || array[10] != 100 || array[11] != 98)
		{
			ThrowEncryptionError(102);
		}
		return true;
	}

	private byte[] EncodeKeyDataV5(byte[] key, byte[] data, bool cbc)
	{
		byte[] array = null;
		using RijndaelManaged rijndaelManaged = new RijndaelManaged();
		rijndaelManaged.Mode = (cbc ? CipherMode.CBC : CipherMode.ECB);
		rijndaelManaged.BlockSize = 128;
		rijndaelManaged.Padding = PaddingMode.None;
		rijndaelManaged.Key = key;
		rijndaelManaged.IV = new byte[16];
		ICryptoTransform transform = rijndaelManaged.CreateEncryptor(rijndaelManaged.Key, rijndaelManaged.IV);
		using MemoryStream memoryStream = new MemoryStream();
		using CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
		cryptoStream.Write(data, 0, data.Length);
		cryptoStream.FlushFinalBlock();
		return memoryStream.ToArray();
	}

	private byte[] DecodeKeyDataV5(byte[] key, byte[] data, bool cbc)
	{
		byte[] array = new byte[32];
		using RijndaelManaged rijndaelManaged = new RijndaelManaged();
		rijndaelManaged.Mode = (cbc ? CipherMode.CBC : CipherMode.ECB);
		rijndaelManaged.BlockSize = 128;
		rijndaelManaged.Padding = PaddingMode.None;
		rijndaelManaged.Key = key;
		rijndaelManaged.IV = new byte[16];
		ICryptoTransform transform = rijndaelManaged.CreateDecryptor(rijndaelManaged.Key, rijndaelManaged.IV);
		using MemoryStream stream = new MemoryStream(data);
		using CryptoStream cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Read);
		cryptoStream.Read(array, 0, array.Length);
		return array;
	}

	private byte[] GetHashV5(byte[] password, byte[] salt, byte[] ownerkey)
	{
		if (keyLength == StiPdfEncryptionKeyLength.Bit256_r5)
		{
			return Calculate_hash_r5(password, salt, ownerkey);
		}
		return Calculate_hash_r6(password, salt, ownerkey);
	}

	private static byte[] Calculate_hash_r5(byte[] password, byte[] salt, byte[] ownerkey)
	{
		byte[] array = new byte[password.Length + 8 + ((ownerkey != null) ? 48 : 0)];
		password.CopyTo(array, 0);
		salt.CopyTo(array, password.Length);
		ownerkey?.CopyTo(array, password.Length + 8);
		SHA256Managed sHA256Managed = new SHA256Managed();
		return sHA256Managed.ComputeHash(array);
	}

	private static byte[] Calculate_hash_r6(byte[] password, byte[] salt, byte[] ownerkey)
	{
		byte[] array = new byte[15360];
		byte[] array2 = new byte[64];
		int num = 32;
		int num2 = 0;
		byte[] array3 = null;
		byte[] array4 = new byte[16];
		byte[] array5 = new byte[16];
		SHA256Managed sHA256Managed = new SHA256Managed();
		SHA384Managed sHA384Managed = new SHA384Managed();
		SHA512Managed sHA512Managed = new SHA512Managed();
		RijndaelManaged rijndaelManaged = new RijndaelManaged();
		rijndaelManaged.Mode = CipherMode.CBC;
		rijndaelManaged.BlockSize = 128;
		rijndaelManaged.Padding = PaddingMode.None;
		array3 = new byte[password.Length + 8 + ((ownerkey != null) ? 48 : 0)];
		password.CopyTo(array3, 0);
		salt.CopyTo(array3, password.Length);
		ownerkey?.CopyTo(array3, password.Length + 8);
		sHA256Managed.ComputeHash(array3).CopyTo(array2, 0);
		for (int i = 0; i < 64 || i < array[num2 * 64 - 1] + 32; i++)
		{
			password.CopyTo(array, 0);
			Array.Copy(array2, 0, array, password.Length, num);
			num2 = password.Length + num;
			if (ownerkey != null)
			{
				ownerkey.CopyTo(array, password.Length + num);
				num2 += 48;
			}
			for (int j = 1; j < 64; j++)
			{
				Array.Copy(array, 0, array, j * num2, num2);
			}
			byte[] array6 = null;
			Array.Copy(array2, 0, array4, 0, 16);
			Array.Copy(array2, 16, array5, 0, 16);
			rijndaelManaged.Key = array4;
			rijndaelManaged.IV = array5;
			ICryptoTransform transform = rijndaelManaged.CreateEncryptor(rijndaelManaged.Key, rijndaelManaged.IV);
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
				cryptoStream.Write(array, 0, num2 * 64);
				cryptoStream.FlushFinalBlock();
				array6 = memoryStream.ToArray();
			}
			array6.CopyTo(array, 0);
			int num3 = 0;
			for (int k = 0; k < 16; k++)
			{
				num3 += array[k];
			}
			num = 32 + num3 % 3 * 16;
			switch (num)
			{
			case 32:
				sHA256Managed.ComputeHash(array, 0, num2 * 64).CopyTo(array2, 0);
				break;
			case 48:
				sHA384Managed.ComputeHash(array, 0, num2 * 64).CopyTo(array2, 0);
				break;
			case 64:
				sHA512Managed.ComputeHash(array, 0, num2 * 64).CopyTo(array2, 0);
				break;
			}
		}
		byte[] array7 = new byte[32];
		Array.Copy(array2, 0, array7, 0, 32);
		return array7;
	}

	public byte[] EncryptData(byte[] data, int currentObjectNumber, int currentGenerationNumber)
	{
		if (keyLength == StiPdfEncryptionKeyLength.Bit40 || keyLength == StiPdfEncryptionKeyLength.Bit128)
		{
			byte[] array = new byte[encryptionKey.Length + 5];
			encryptionKey.CopyTo(array, 0);
			BitConverter.GetBytes(currentObjectNumber).CopyTo(array, array.Length - 5);
			BitConverter.GetBytes((ushort)currentGenerationNumber).CopyTo(array, array.Length - 2);
			byte[] array2 = StiMD5Helper.ComputeHash(array);
			if (keyLength == StiPdfEncryptionKeyLength.Bit40)
			{
				byte[] array3 = new byte[10];
				Array.Copy(array2, array3, 10);
				array2 = array3;
			}
			StiEncryption.RC4(ref data, array2);
		}
		else if (keyLength == StiPdfEncryptionKeyLength.Bit128_r4)
		{
			byte[] array4 = new byte[25];
			encryptionKey.CopyTo(array4, 0);
			BitConverter.GetBytes(currentObjectNumber).CopyTo(array4, 16);
			BitConverter.GetBytes((ushort)currentGenerationNumber).CopyTo(array4, 19);
			array4[21] = 115;
			array4[22] = 65;
			array4[23] = 108;
			array4[24] = 84;
			byte[] key = StiMD5Helper.ComputeHash(array4);
			using RijndaelManaged rijndaelManaged = new RijndaelManaged();
			rijndaelManaged.Mode = CipherMode.CBC;
			rijndaelManaged.BlockSize = 128;
			rijndaelManaged.Key = key;
			rijndaelManaged.GenerateIV();
			ICryptoTransform transform = rijndaelManaged.CreateEncryptor(rijndaelManaged.Key, rijndaelManaged.IV);
			using MemoryStream memoryStream = new MemoryStream();
			using CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
			cryptoStream.Write(rijndaelManaged.IV, 0, rijndaelManaged.IV.Length);
			cryptoStream.Write(data, 0, data.Length);
			cryptoStream.FlushFinalBlock();
			data = memoryStream.ToArray();
		}
		else
		{
			using RijndaelManaged rijndaelManaged2 = new RijndaelManaged();
			rijndaelManaged2.Mode = CipherMode.CBC;
			rijndaelManaged2.BlockSize = 128;
			rijndaelManaged2.Key = encryptionKey;
			rijndaelManaged2.GenerateIV();
			ICryptoTransform transform2 = rijndaelManaged2.CreateEncryptor(rijndaelManaged2.Key, rijndaelManaged2.IV);
			using MemoryStream memoryStream2 = new MemoryStream();
			using CryptoStream cryptoStream2 = new CryptoStream(memoryStream2, transform2, CryptoStreamMode.Write);
			cryptoStream2.Write(rijndaelManaged2.IV, 0, rijndaelManaged2.IV.Length);
			cryptoStream2.Write(data, 0, data.Length);
			cryptoStream2.FlushFinalBlock();
			data = memoryStream2.ToArray();
		}
		return data;
	}

	private void ThrowEncryptionError(int step)
	{
		Win32Exception ex = new Win32Exception(Marshal.GetLastWin32Error());
		throw new Exception(string.Format("{0} {1}, code #{2:X8}: {3}", StiLocalization.Get("Export", "EncryptionError"), step, ex.ErrorCode, ex.Message));
	}

	internal void RenderEncodeRecord(StreamWriter sw)
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		int num = 32;
		if (keyLength == StiPdfEncryptionKeyLength.Bit256_r5 || keyLength == StiPdfEncryptionKeyLength.Bit256_r6)
		{
			num = 48;
		}
		for (int i = 0; i < num; i++)
		{
			stringBuilder.Append((char)ownerValue[i]);
			stringBuilder2.Append((char)userValue[i]);
		}
		sw.WriteLine("/Filter /Standard");
		if (keyLength == StiPdfEncryptionKeyLength.Bit128)
		{
			sw.WriteLine("/CF<</StdCF<</AuthEvent/DocOpen/CFM/V2/Length 16>>>>");
			sw.WriteLine("/StmF /StdCF");
			sw.WriteLine("/StrF /StdCF");
			sw.WriteLine("/R 3");
			sw.WriteLine("/V 2");
			sw.WriteLine("/Length 128");
		}
		else if (keyLength == StiPdfEncryptionKeyLength.Bit128_r4)
		{
			sw.WriteLine("/CF<</StdCF<</AuthEvent/DocOpen/CFM/AESV2/Length 16>>>>");
			sw.WriteLine("/StmF /StdCF");
			sw.WriteLine("/StrF /StdCF");
			sw.WriteLine("/R 4");
			sw.WriteLine("/V 4");
			sw.WriteLine("/Length 128");
		}
		else if (keyLength == StiPdfEncryptionKeyLength.Bit256_r5 || keyLength == StiPdfEncryptionKeyLength.Bit256_r6)
		{
			sw.WriteLine("/CF<</StdCF<</AuthEvent/DocOpen/CFM/AESV3/Length 32>>>>");
			sw.WriteLine("/StmF /StdCF");
			sw.WriteLine("/StrF /StdCF");
			sw.WriteLine("/R {0}", (keyLength == StiPdfEncryptionKeyLength.Bit256_r5) ? 5 : 6);
			sw.WriteLine("/V 5");
			sw.WriteLine("/Length 256");
			StringBuilder stringBuilder3 = new StringBuilder();
			StringBuilder stringBuilder4 = new StringBuilder();
			for (int j = 0; j < 32; j++)
			{
				stringBuilder3.Append((char)ownerExtendedValue[j]);
				stringBuilder4.Append((char)userExtendedValue[j]);
			}
			StringBuilder stringBuilder5 = new StringBuilder();
			for (int k = 0; k < 16; k++)
			{
				stringBuilder5.Append((char)permsValue[k]);
			}
			pdfService.StoreString($"/OE ({StiPdfExportService.ConvertToEscapeSequencePlusTabs(stringBuilder3.ToString())})");
			sw.WriteLine("");
			pdfService.StoreString($"/UE ({StiPdfExportService.ConvertToEscapeSequencePlusTabs(stringBuilder4.ToString())})");
			sw.WriteLine("");
			pdfService.StoreString($"/Perms ({StiPdfExportService.ConvertToEscapeSequencePlusTabs(stringBuilder5.ToString())})");
			sw.WriteLine("");
		}
		else
		{
			sw.WriteLine("/R 2");
			sw.WriteLine("/V 1");
			sw.WriteLine("/Length 40");
		}
		pdfService.StoreString($"/O ({StiPdfExportService.ConvertToEscapeSequencePlusTabs(stringBuilder.ToString())})");
		sw.WriteLine("");
		pdfService.StoreString($"/U ({StiPdfExportService.ConvertToEscapeSequencePlusTabs(stringBuilder2.ToString())})");
		sw.WriteLine("");
		sw.WriteLine("/P {0}", (int)securityFlags);
	}

	private static void ThrowDigitalSignError(int step, string message = null, bool showCode = false)
	{
		string text = StiLocalization.Get("Export", "DigitalSignatureError");
		Win32Exception ex = new Win32Exception(Marshal.GetLastWin32Error());
		if (string.IsNullOrWhiteSpace(message))
		{
			throw new Exception(string.Format(text + " {0}, code #{1:X8}: {2}", step, ex.ErrorCode, ex.Message));
		}
		if (showCode)
		{
			throw new Exception(string.Format(text + " {0}: {1} \r\n(code #{2:X8}: {3})", step, message, ex.ErrorCode, ex.Message));
		}
		throw new Exception(string.Format(text + " {0}: {1}", step, message));
	}

	public byte[] CreateSignature(byte[] buf, bool getCertificateFromCryptoUI, string certificateID, bool useLocalMachineCertificates, byte[] certificateData, string certificatePassword, string certificateThumbprint, out bool isGost, ref string signedBy, int offsetFilter, int offsetSignedBy)
	{
		int pcbSignedBlob = 0;
		byte[] array = null;
		IntPtr intPtr = IntPtr.Zero;
		IntPtr intPtr2 = IntPtr.Zero;
		X509Store x509Store = null;
		X509Certificate2Collection x509Certificate2Collection = null;
		X509Certificate2 x509Certificate = null;
		uint num = 65537u;
		string text = "MY";
		uint dwFlags = 65536u;
		if (useLocalMachineCertificates)
		{
			dwFlags = 163840u;
		}
		if (getCertificateFromCryptoUI)
		{
			intPtr2 = Win32.CertOpenStore((IntPtr)10, 0u, IntPtr.Zero, dwFlags, Encoding.Unicode.GetBytes(text));
			if (intPtr2 == IntPtr.Zero)
			{
				ThrowDigitalSignError(1);
			}
			intPtr = Win32.CryptUIDlgSelectCertificateFromStore(intPtr2, IntPtr.Zero, null, null, 26u, 0u, IntPtr.Zero);
			if (intPtr == IntPtr.Zero)
			{
				ThrowDigitalSignError(2, StiLocalization.Get("Export", "DigitalSignatureCertificateNotSelected"));
			}
		}
		else
		{
			if (certificateData != null)
			{
				try
				{
					x509Certificate = (string.IsNullOrEmpty(certificatePassword) ? new X509Certificate2(certificateData) : ((!useLocalMachineCertificates) ? new X509Certificate2(certificateData, certificatePassword) : new X509Certificate2(certificateData, certificatePassword, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet)));
					if (x509Certificate == null)
					{
						ThrowDigitalSignError(2, "Certificate data or password is incorrect");
					}
				}
				catch (Exception ex)
				{
					ThrowDigitalSignError(21, ex.Message);
				}
			}
			if (x509Certificate == null)
			{
				if (string.IsNullOrWhiteSpace(certificateThumbprint) && string.IsNullOrEmpty(certificateID))
				{
					ThrowDigitalSignError(2, "Subject name string is empty");
				}
				x509Store = new X509Store(text, (!useLocalMachineCertificates) ? StoreLocation.CurrentUser : StoreLocation.LocalMachine);
				x509Store.Open(OpenFlags.ReadOnly);
				if (!string.IsNullOrWhiteSpace(certificateThumbprint))
				{
					x509Certificate2Collection = x509Store.Certificates.Find(X509FindType.FindByThumbprint, certificateThumbprint, validOnly: false);
				}
				else
				{
					x509Certificate2Collection = x509Store.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, certificateID, validOnly: false);
					if (x509Certificate2Collection.Count == 0)
					{
						x509Certificate2Collection = x509Store.Certificates.Find(X509FindType.FindBySubjectName, certificateID, validOnly: false);
					}
				}
				x509Certificate2Collection = x509Certificate2Collection.Find(X509FindType.FindByTimeValid, DateTime.Now, validOnly: false);
				if (x509Certificate2Collection.Count > 0)
				{
					x509Certificate = x509Certificate2Collection[0];
				}
			}
			if (x509Certificate != null)
			{
				intPtr = x509Certificate.Handle;
			}
			else
			{
				x509Certificate2Collection.Clear();
				x509Certificate2Collection = null;
				x509Store.Close();
				x509Store = null;
				intPtr2 = Win32.CertOpenStore((IntPtr)10, 0u, IntPtr.Zero, dwFlags, Encoding.Unicode.GetBytes(text));
				if (intPtr2 == IntPtr.Zero)
				{
					ThrowDigitalSignError(1);
				}
				int num2 = 0;
				IntPtr pvFindPara = Marshal.StringToBSTR(certificateID);
				Win32.CERT_CONTEXT cERT_CONTEXT;
				do
				{
					intPtr = Win32.CertFindCertificateInStore(intPtr2, num, 0u, 524295u, pvFindPara, intPtr);
					if (intPtr == IntPtr.Zero)
					{
						ThrowDigitalSignError(2, "Certificate is not found", showCode: true);
					}
					cERT_CONTEXT = (Win32.CERT_CONTEXT)Marshal.PtrToStructure(intPtr, typeof(Win32.CERT_CONTEXT));
				}
				while (Win32.CertVerifyTimeValidity(IntPtr.Zero, cERT_CONTEXT.pCertInfo) != 0);
			}
		}
		if (x509Certificate == null)
		{
			try
			{
				x509Certificate = new X509Certificate2(intPtr);
			}
			catch
			{
			}
		}
		isGost = x509Certificate != null && (x509Certificate.SignatureAlgorithm.Value == "1.2.643.2.2.4" || x509Certificate.SignatureAlgorithm.Value == "1.2.643.2.2.3");
		if (isGost)
		{
			Encoding.ASCII.GetBytes("CryptoPro#20PDF").CopyTo(buf, offsetFilter);
		}
		if (string.IsNullOrEmpty(signedBy) && x509Certificate != null)
		{
			try
			{
				string subject = x509Certificate.Subject;
				int num3 = subject.IndexOf("CN=", StringComparison.InvariantCulture);
				if (num3 != -1)
				{
					num3 += 3;
					if (subject[num3] == '"')
					{
						int num4 = subject.IndexOf('"', num3 + 1);
						if (num4 != -1)
						{
							signedBy = subject.Substring(num3 + 1, num4 - num3 - 1);
						}
					}
					else
					{
						int num5 = subject.IndexOf(',', num3);
						if (num5 == -1)
						{
							num5 = subject.Length;
						}
						signedBy = subject.Substring(num3, num5 - num3);
					}
				}
			}
			catch
			{
			}
			if (!string.IsNullOrEmpty(signedBy))
			{
				signedBy = MakeSignedByString(signedBy);
				if (!string.IsNullOrWhiteSpace(signedBy))
				{
					byte[] array2 = new byte[signedBy.Length];
					for (int i = 0; i < signedBy.Length; i++)
					{
						array2[i] = (byte)signedBy[i];
					}
					array2.CopyTo(buf, offsetSignedBy);
				}
			}
		}
		Win32.CRYPT_SIGN_MESSAGE_PARA pSignPara = default(Win32.CRYPT_SIGN_MESSAGE_PARA);
		pSignPara.cbSize = (uint)Marshal.SizeOf(typeof(Win32.CRYPT_SIGN_MESSAGE_PARA));
		pSignPara.dwMsgEncodingType = num;
		pSignPara.pSigningCert = intPtr;
		if (isGost)
		{
			pSignPara.HashAlgorithm.pszObjId = "1.2.643.2.2.9";
		}
		else if (StiOptions.Export.Pdf.DigitalSignatureDigestSHA256)
		{
			pSignPara.HashAlgorithm.pszObjId = "2.16.840.1.101.3.4.2.1";
		}
		else
		{
			pSignPara.HashAlgorithm.pszObjId = "1.2.840.113549.1.1.5";
		}
		pSignPara.HashAlgorithm.Parameters.cbData = 0u;
		pSignPara.HashAlgorithm.Parameters.pbData = IntPtr.Zero;
		pSignPara.pvHashAuxInfo = IntPtr.Zero;
		pSignPara.cMsgCrl = 0u;
		pSignPara.rgpMsgCrl = IntPtr.Zero;
		pSignPara.cAuthAttr = 0u;
		pSignPara.rgAuthAttr = IntPtr.Zero;
		pSignPara.cUnauthAttr = 0u;
		pSignPara.rgUnauthAttr = IntPtr.Zero;
		pSignPara.dwFlags = 0u;
		pSignPara.dwInnerContentType = 0u;
		pSignPara.pvHashEncryptionAuxInfo = IntPtr.Zero;
		pSignPara.cMsgCert = 1u;
		IntPtr[] array3 = new IntPtr[1] { Marshal.AllocHGlobal(buf.Length) };
		Marshal.Copy(buf, 0, array3[0], buf.Length);
		int[] rgcbToBeSigned = new int[1] { buf.Length };
		GCHandle gCHandle = GCHandle.Alloc(intPtr, GCHandleType.Pinned);
		pSignPara.rgpMsgCert = gCHandle.AddrOfPinnedObject();
		if (!Win32.CryptSignMessage(ref pSignPara, fDetachedSignature: true, 1, array3, rgcbToBeSigned, null, ref pcbSignedBlob))
		{
			ThrowDigitalSignError(3);
		}
		array = new byte[pcbSignedBlob];
		if (!Win32.CryptSignMessage(ref pSignPara, fDetachedSignature: true, 1, array3, rgcbToBeSigned, array, ref pcbSignedBlob))
		{
			ThrowDigitalSignError(31);
		}
		gCHandle.Free();
		if (array3[0] != IntPtr.Zero)
		{
			Marshal.FreeHGlobal(array3[0]);
		}
		if (x509Certificate != null)
		{
			x509Certificate.Reset();
			x509Certificate = null;
		}
		if (x509Store != null)
		{
			x509Certificate2Collection.Clear();
			x509Certificate2Collection = null;
			x509Store.Close();
			x509Store = null;
		}
		if (intPtr2 != IntPtr.Zero)
		{
			int num6 = 0;
			if (intPtr != IntPtr.Zero && Win32.CertFreeCertificateContext(intPtr) == 0)
			{
				ThrowDigitalSignError(4);
			}
			if (Win32.CertCloseStore(intPtr2, 2u) == 0)
			{
				ThrowDigitalSignError(5);
			}
		}
		return array;
	}

	public string MakeSignedByString(string input, bool padding = true)
	{
		string text = pdfService.ConvertToHexString(input, escaping: true);
		if (padding)
		{
			if (text.Length < 256)
			{
				text += new string(' ', 256 - text.Length);
			}
			else if (text.Length > 256)
			{
				text = null;
			}
		}
		return text;
	}

	public static string GetCertificateThumbprintFromCryptoUI(bool useLocalMachineCertificates, out string certificateName, out string errorMessage)
	{
		string result = null;
		errorMessage = null;
		try
		{
			IntPtr zero = IntPtr.Zero;
			IntPtr zero2 = IntPtr.Zero;
			string s = "MY";
			uint dwFlags = 65536u;
			if (useLocalMachineCertificates)
			{
				dwFlags = 163840u;
			}
			zero2 = Win32.CertOpenStore((IntPtr)10, 0u, IntPtr.Zero, dwFlags, Encoding.Unicode.GetBytes(s));
			if (zero2 == IntPtr.Zero)
			{
				ThrowDigitalSignError(1);
			}
			zero = Win32.CryptUIDlgSelectCertificateFromStore(zero2, IntPtr.Zero, null, null, 26u, 0u, IntPtr.Zero);
			if (zero == IntPtr.Zero)
			{
				ThrowDigitalSignError(2, StiLocalization.Get("Export", "DigitalSignatureCertificateNotSelected"));
			}
			try
			{
				X509Certificate2 x509Certificate = new X509Certificate2(zero);
				if (!x509Certificate.HasPrivateKey)
				{
					x509Certificate.Reset();
					errorMessage = "The certificate contains no private key!";
					throw new CryptographicException(errorMessage);
				}
				result = x509Certificate.Thumbprint;
				certificateName = GetFieldValue(x509Certificate.Subject, "CN");
				x509Certificate.Reset();
			}
			catch
			{
				certificateName = "";
			}
			if (zero2 != IntPtr.Zero)
			{
				int num = 0;
				if (zero != IntPtr.Zero && Win32.CertFreeCertificateContext(zero) == 0)
				{
					ThrowDigitalSignError(44);
				}
				if (Win32.CertCloseStore(zero2, 2u) == 0)
				{
					ThrowDigitalSignError(45);
				}
			}
		}
		catch
		{
			certificateName = "";
		}
		return result;
	}

	public static List<StiCertificateInfo> GetCertificatesList(bool useLocalMachineCertificates)
	{
		List<StiCertificateInfo> list = new List<StiCertificateInfo>();
		try
		{
			string storeName = "MY";
			X509Store x509Store = new X509Store(storeName, (!useLocalMachineCertificates) ? StoreLocation.CurrentUser : StoreLocation.LocalMachine);
			x509Store.Open(OpenFlags.ReadOnly);
			X509Certificate2Collection x509Certificate2Collection = x509Store.Certificates.Find(X509FindType.FindByTimeValid, DateTime.Now, validOnly: false);
			if (x509Certificate2Collection.Count > 0)
			{
				X509Certificate2Enumerator enumerator = x509Certificate2Collection.GetEnumerator();
				while (enumerator.MoveNext())
				{
					X509Certificate2 current = enumerator.Current;
					if (current.HasPrivateKey)
					{
						StiCertificateInfo stiCertificateInfo = new StiCertificateInfo();
						stiCertificateInfo.Name = ((!string.IsNullOrWhiteSpace(current.FriendlyName)) ? current.FriendlyName : GetFieldValue(current.Subject, "CN"));
						stiCertificateInfo.Issuer = GetFieldValue(current.Issuer, "CN");
						stiCertificateInfo.From = current.NotBefore;
						stiCertificateInfo.To = current.NotAfter;
						stiCertificateInfo.Thumbprint = current.Thumbprint;
						list.Add(stiCertificateInfo);
					}
				}
			}
			x509Certificate2Collection.Clear();
			x509Store.Close();
		}
		catch
		{
		}
		return list;
	}

	private static string GetFieldValue(string certName, string fieldName)
	{
		try
		{
			int num = certName.IndexOf(fieldName + "=");
			if (num < 0)
			{
				return certName;
			}
			string text = certName.Substring(num + fieldName.Length + 1);
			int num2 = text.IndexOf(",");
			if (num2 < 0)
			{
				return text;
			}
			return text.Substring(0, num2);
		}
		catch
		{
			return certName;
		}
	}

	public StiPdfSecurity(StiPdfExportService service)
	{
		pdfService = service;
	}
}
