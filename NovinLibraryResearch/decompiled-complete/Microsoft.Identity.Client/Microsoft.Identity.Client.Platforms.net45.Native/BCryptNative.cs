using System;
using System.Runtime.InteropServices;

namespace Microsoft.Identity.Client.Platforms.net45.Native;

internal static class BCryptNative
{
	internal static class AlgorithmName
	{
		internal const string Aes = "AES";

		internal const string Rng = "RNG";

		internal const string Rsa = "RSA";

		internal const string TripleDes = "3DES";

		internal const string Sha1 = "SHA1";

		internal const string Sha256 = "SHA256";

		internal const string Sha384 = "SHA384";

		internal const string Sha512 = "SHA512";

		internal const string Pbkdf2 = "PBKDF2";
	}

	[Flags]
	internal enum AlgorithmProviderOptions
	{
		None = 0,
		HmacAlgorithm = 8
	}

	[Flags]
	internal enum AuthenticatedCipherModeInfoFlags
	{
		None = 0,
		ChainCalls = 1,
		InProgress = 2
	}

	internal static class ChainingMode
	{
		internal const string Cbc = "ChainingModeCBC";

		internal const string Ccm = "ChainingModeCCM";

		internal const string Cfb = "ChainingModeCFB";

		internal const string Ecb = "ChainingModeECB";

		internal const string Gcm = "ChainingModeGCM";
	}

	internal enum ErrorCode
	{
		Success = 0,
		AuthenticationTagMismatch = -1073700862,
		BufferToSmall = -1073741789
	}

	internal static class HashPropertyName
	{
		internal const string HashLength = "HashDigestLength";
	}

	internal enum KeyBlobMagicNumber
	{
		RsaPublic = 826364754,
		RsaPrivate = 843141970,
		KeyDataBlob = 1296188491
	}

	internal static class KeyBlobType
	{
		internal const string KeyDataBlob = "KeyDataBlob";

		internal const string RsaFullPrivateBlob = "RSAFULLPRIVATEBLOB";

		internal const string RsaPrivateBlob = "RSAPRIVATEBLOB";

		internal const string RsaPublicBlob = "RSAPUBLICBLOB";
	}

	internal enum ParameterTypes
	{
		KdfHashAlgorithm = 0,
		KdfSalt = 15,
		KdfIterationCount = 16
	}

	internal static class ProviderName
	{
		internal const string MicrosoftPrimitiveProvider = "Microsoft Primitive Provider";
	}

	internal struct BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO
	{
		internal int cbSize;

		internal int dwInfoVersion;

		internal IntPtr pbNonce;

		internal int cbNonce;

		internal IntPtr pbAuthData;

		internal int cbAuthData;

		internal IntPtr pbTag;

		internal int cbTag;

		internal IntPtr pbMacContext;

		internal int cbMacContext;

		internal int cbAAD;

		internal long cbData;

		internal AuthenticatedCipherModeInfoFlags dwFlags;
	}

	internal struct BCRYPT_KEY_DATA_BLOB
	{
		internal KeyBlobMagicNumber dwMagic;

		internal int dwVersion;

		internal int cbKeyData;
	}

	internal struct BCRYPT_KEY_LENGTHS_STRUCT
	{
		internal int dwMinLength;

		internal int dwMaxLength;

		internal int dwIncrement;
	}

	internal struct BCRYPT_OAEP_PADDING_INFO
	{
		[MarshalAs(UnmanagedType.LPWStr)]
		internal string pszAlgId;

		internal IntPtr pbLabel;

		internal int cbLabel;
	}

	internal struct BCRYPT_PKCS1_PADDING_INFO
	{
		[MarshalAs(UnmanagedType.LPWStr)]
		internal string pszAlgId;
	}

	internal struct BCRYPT_PSS_PADDING_INFO
	{
		[MarshalAs(UnmanagedType.LPWStr)]
		internal string pszAlgId;

		internal int cbSalt;
	}

	internal struct BCRYPT_RSAKEY_BLOB
	{
		internal KeyBlobMagicNumber Magic;

		internal int BitLength;

		internal int cbPublicExp;

		internal int cbModulus;

		internal int cbPrime1;

		internal int cbPrime2;
	}

	internal struct BCryptBuffer
	{
		internal int cbBuffer;

		internal int BufferType;

		internal IntPtr pvBuffer;
	}

	internal struct BCryptBufferDesc
	{
		internal int ulVersion;

		internal int cBuffers;

		internal IntPtr pBuffers;
	}
}
