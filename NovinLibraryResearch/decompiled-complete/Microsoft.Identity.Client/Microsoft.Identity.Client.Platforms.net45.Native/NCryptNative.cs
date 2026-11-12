using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.Identity.Client.Platforms.net45.Native;

internal static class NCryptNative
{
	internal static class KeyPropertyName
	{
		internal const string Length = "Length";
	}

	internal enum NCryptAlgorithmClass
	{
		None = 0,
		AsymmetricEncryption = 3,
		SecretAgreement = 4,
		Signature = 5
	}

	internal enum ErrorCode
	{
		Success = 0,
		BadSignature = -2146893818,
		BufferTooSmall = -2146893784,
		NoMoreItems = -2146893782
	}

	internal struct NCryptAlgorithmName
	{
		[MarshalAs(UnmanagedType.LPWStr)]
		internal string pszName;

		internal NCryptAlgorithmClass dwClass;

		internal NCryptAlgorithmOperations dwAlgOperations;

		internal int dwFlags;
	}

	internal struct NCryptKeyName
	{
		[MarshalAs(UnmanagedType.LPWStr)]
		internal string pszName;

		[MarshalAs(UnmanagedType.LPWStr)]
		internal string pszAlgId;

		internal int dwLegacyKeySpec;

		internal int dwFlags;
	}

	internal struct NCryptProviderName
	{
		[MarshalAs(UnmanagedType.LPWStr)]
		internal string pszName;

		[MarshalAs(UnmanagedType.LPWStr)]
		internal string pszComment;
	}

	[SuppressUnmanagedCodeSecurity]
	internal static class UnsafeNativeMethods
	{
		[DllImport("ncrypt.dll")]
		internal static extern ErrorCode NCryptEnumAlgorithms(SafeNCryptProviderHandle hProvider, NCryptAlgorithmOperations dwAlgOperations, out uint pdwAlgCount, out SafeNCryptBuffer ppAlgList, int dwFlags);

		[DllImport("ncrypt.dll")]
		internal static extern ErrorCode NCryptEnumKeys(SafeNCryptProviderHandle hProvider, [In][MarshalAs(UnmanagedType.LPWStr)] string pszScope, out SafeNCryptBuffer ppKeyName, [In][Out] ref IntPtr ppEnumState, CngKeyOpenOptions dwFlags);

		[DllImport("ncrypt.dll")]
		internal static extern ErrorCode NCryptEnumStorageProviders(out uint pdwProviderCount, out SafeNCryptBuffer ppProviderList, int dwFlags);

		[DllImport("ncrypt.dll")]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[SuppressUnmanagedCodeSecurity]
		internal static extern ErrorCode NCryptFreeBuffer(IntPtr pvInput);

		[DllImport("ncrypt.dll")]
		internal static extern ErrorCode NCryptOpenStorageProvider(out SafeNCryptProviderHandle phProvider, [MarshalAs(UnmanagedType.LPWStr)] string pszProviderName, int dwFlags);

		[DllImport("ncrypt.dll")]
		internal static extern ErrorCode NCryptSignHash(SafeNCryptKeyHandle hKey, [In] ref BCryptNative.BCRYPT_PKCS1_PADDING_INFO pPaddingInfo, [In][MarshalAs(UnmanagedType.LPArray)] byte[] pbHashValue, int cbHashValue, [Out][MarshalAs(UnmanagedType.LPArray)] byte[] pbSignature, int cbSignature, out int pcbResult, AsymmetricPaddingMode dwFlags);

		[DllImport("ncrypt.dll")]
		internal static extern ErrorCode NCryptSignHash(SafeNCryptKeyHandle hKey, [In] ref BCryptNative.BCRYPT_PSS_PADDING_INFO pPaddingInfo, [In][MarshalAs(UnmanagedType.LPArray)] byte[] pbHashValue, int cbHashValue, [Out][MarshalAs(UnmanagedType.LPArray)] byte[] pbSignature, int cbSignature, out int pcbResult, AsymmetricPaddingMode dwFlags);
	}

	[SecurityCritical]
	private delegate ErrorCode NCryptDecryptor<T>(SafeNCryptKeyHandle hKey, byte[] pbInput, int cbInput, ref T pvPadding, byte[] pbOutput, int cbOutput, out int pcbResult, AsymmetricPaddingMode dwFlags);

	[SecurityCritical]
	private delegate ErrorCode NCryptEncryptor<T>(SafeNCryptKeyHandle hKey, byte[] pbInput, int cbInput, ref T pvPadding, byte[] pbOutput, int cbOutput, out int pcbResult, AsymmetricPaddingMode dwFlags);

	[SecurityCritical]
	private delegate ErrorCode NCryptHashSigner<T>(SafeNCryptKeyHandle hKey, ref T pvPaddingInfo, byte[] pbHashValue, int cbHashValue, byte[] pbSignature, int cbSignature, out int pcbResult, AsymmetricPaddingMode dwFlags);

	[SecurityCritical]
	private static byte[] SignHash<T>(SafeNCryptKeyHandle key, byte[] hash, ref T paddingInfo, AsymmetricPaddingMode paddingMode, NCryptHashSigner<T> signer) where T : struct
	{
		int pcbResult = 0;
		ErrorCode errorCode = signer(key, ref paddingInfo, hash, hash.Length, null, 0, out pcbResult, paddingMode);
		if (errorCode != 0 && errorCode != ErrorCode.BufferTooSmall)
		{
			throw new CryptographicException((int)errorCode);
		}
		byte[] array = new byte[pcbResult];
		errorCode = signer(key, ref paddingInfo, hash, hash.Length, array, array.Length, out pcbResult, paddingMode);
		if (errorCode != 0)
		{
			throw new CryptographicException((int)errorCode);
		}
		return array;
	}

	[SecurityCritical]
	internal static byte[] SignHashPkcs1(SafeNCryptKeyHandle key, byte[] hash, string hashAlgorithm)
	{
		BCryptNative.BCRYPT_PKCS1_PADDING_INFO paddingInfo = default(BCryptNative.BCRYPT_PKCS1_PADDING_INFO);
		paddingInfo.pszAlgId = hashAlgorithm;
		return SignHash(key, hash, ref paddingInfo, AsymmetricPaddingMode.Pkcs1, UnsafeNativeMethods.NCryptSignHash);
	}

	[SecurityCritical]
	internal static byte[] SignHashPss(SafeNCryptKeyHandle key, byte[] hash, string hashAlgorithm, int saltBytes)
	{
		BCryptNative.BCRYPT_PSS_PADDING_INFO paddingInfo = default(BCryptNative.BCRYPT_PSS_PADDING_INFO);
		paddingInfo.pszAlgId = hashAlgorithm;
		paddingInfo.cbSalt = saltBytes;
		return SignHash(key, hash, ref paddingInfo, AsymmetricPaddingMode.Pss, UnsafeNativeMethods.NCryptSignHash);
	}
}
