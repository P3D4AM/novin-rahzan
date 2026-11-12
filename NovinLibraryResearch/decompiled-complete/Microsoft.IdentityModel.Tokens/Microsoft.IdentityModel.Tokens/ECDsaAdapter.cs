using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens;

internal class ECDsaAdapter
{
	private enum KeyBlobMagicNumber : uint
	{
		BCRYPT_ECDSA_PUBLIC_P256_MAGIC = 827540293u,
		BCRYPT_ECDSA_PUBLIC_P384_MAGIC = 861094725u,
		BCRYPT_ECDSA_PUBLIC_P521_MAGIC = 894649157u,
		BCRYPT_ECDSA_PRIVATE_P256_MAGIC = 844317509u,
		BCRYPT_ECDSA_PRIVATE_P384_MAGIC = 877871941u,
		BCRYPT_ECDSA_PRIVATE_P521_MAGIC = 911426373u
	}

	internal readonly CreateECDsaDelegate CreateECDsaFunction = ECDsaNotSupported;

	internal static ECDsaAdapter Instance = new ECDsaAdapter();

	internal ECDsaAdapter()
	{
		CreateECDsaFunction = CreateECDsaUsingCNGKey;
	}

	internal ECDsa CreateECDsa(JsonWebKey jsonWebKey, bool usePrivateKey)
	{
		return CreateECDsaFunction(jsonWebKey, usePrivateKey);
	}

	private ECDsa CreateECDsaUsingCNGKey(JsonWebKey jsonWebKey, bool usePrivateKey)
	{
		if (jsonWebKey == null)
		{
			throw LogHelper.LogArgumentNullException("jsonWebKey");
		}
		if (jsonWebKey.Crv == null)
		{
			throw LogHelper.LogArgumentNullException("Crv");
		}
		if (jsonWebKey.X == null)
		{
			throw LogHelper.LogArgumentNullException("X");
		}
		if (jsonWebKey.Y == null)
		{
			throw LogHelper.LogArgumentNullException("Y");
		}
		GCHandle gCHandle = default(GCHandle);
		try
		{
			uint magicValue = GetMagicValue(jsonWebKey.Crv, usePrivateKey);
			uint keyByteCount = GetKeyByteCount(jsonWebKey.Crv);
			byte[] array = ((!usePrivateKey) ? new byte[2 * keyByteCount + 2 * Marshal.SizeOf<uint>()] : new byte[3 * keyByteCount + 2 * Marshal.SizeOf<uint>()]);
			gCHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
			IntPtr intPtr = gCHandle.AddrOfPinnedObject();
			byte[] array2 = Base64UrlEncoder.DecodeBytes(jsonWebKey.X);
			if (array2.Length > keyByteCount)
			{
				throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException("jsonWebKey", LogHelper.FormatInvariant("IDX10675: Cannot create a ECDsa object from the '{0}', the bytes from the decoded value of '{1}' must be less than the size associated with the curve: '{2}'. Size was: '{3}'.", "jsonWebKey", "X", keyByteCount, array2.Length)));
			}
			byte[] array3 = Base64UrlEncoder.DecodeBytes(jsonWebKey.Y);
			if (array3.Length > keyByteCount)
			{
				throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException("jsonWebKey", LogHelper.FormatInvariant("IDX10675: Cannot create a ECDsa object from the '{0}', the bytes from the decoded value of '{1}' must be less than the size associated with the curve: '{2}'. Size was: '{3}'.", "jsonWebKey", "Y", keyByteCount, array3.Length)));
			}
			Marshal.WriteInt64(intPtr, 0, magicValue);
			Marshal.WriteInt64(intPtr, 4, keyByteCount);
			int num = 8;
			byte[] array4 = array2;
			foreach (byte val in array4)
			{
				Marshal.WriteByte(intPtr, num++, val);
			}
			array4 = array3;
			foreach (byte val2 in array4)
			{
				Marshal.WriteByte(intPtr, num++, val2);
			}
			if (usePrivateKey)
			{
				if (jsonWebKey.D == null)
				{
					throw LogHelper.LogArgumentNullException("D");
				}
				byte[] array5 = Base64UrlEncoder.DecodeBytes(jsonWebKey.D);
				if (array5.Length > keyByteCount)
				{
					throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException("jsonWebKey", LogHelper.FormatInvariant("IDX10675: Cannot create a ECDsa object from the '{0}', the bytes from the decoded value of '{1}' must be less than the size associated with the curve: '{2}'. Size was: '{3}'.", "jsonWebKey", "D", keyByteCount, array5.Length)));
				}
				array4 = array5;
				foreach (byte val3 in array4)
				{
					Marshal.WriteByte(intPtr, num++, val3);
				}
				Marshal.Copy(intPtr, array, 0, array.Length);
				CngKey cngKey = CngKey.Import(array, CngKeyBlobFormat.EccPrivateBlob);
				try
				{
					return new ECDsaCng(cngKey);
				}
				finally
				{
					((IDisposable)(object)cngKey)?.Dispose();
				}
			}
			Marshal.Copy(intPtr, array, 0, array.Length);
			CngKey cngKey2 = CngKey.Import(array, CngKeyBlobFormat.EccPublicBlob);
			try
			{
				return new ECDsaCng(cngKey2);
			}
			finally
			{
				((IDisposable)(object)cngKey2)?.Dispose();
			}
		}
		catch (Exception inner)
		{
			throw LogHelper.LogExceptionMessage(new CryptographicException("IDX10689: Unable to create an ECDsa object. See inner exception for more details.", inner));
		}
		finally
		{
			if (gCHandle.IsAllocated)
			{
				gCHandle.Free();
			}
		}
	}

	internal static ECDsa ECDsaNotSupported(JsonWebKey jsonWebKey, bool usePrivateKey)
	{
		throw LogHelper.LogExceptionMessage(new PlatformNotSupportedException("IDX10690: ECDsa creation is not supported by the current platform. For more details, see https://aka.ms/IdentityModel/create-ecdsa"));
	}

	private static uint GetKeyByteCount(string curveId)
	{
		if (string.IsNullOrEmpty(curveId))
		{
			throw LogHelper.LogArgumentNullException("curveId");
		}
		switch (curveId)
		{
		case "P-256":
			return 32u;
		case "P-384":
			return 48u;
		case "P-512":
		case "P-521":
			return 66u;
		default:
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX10645: Elliptical Curve not supported for curveId: '{0}'", curveId)));
		}
	}

	private static uint GetMagicValue(string curveId, bool willCreateSignatures)
	{
		if (string.IsNullOrEmpty(curveId))
		{
			throw LogHelper.LogArgumentNullException("curveId");
		}
		switch (curveId)
		{
		case "P-256":
			if (willCreateSignatures)
			{
				return 844317509u;
			}
			return 827540293u;
		case "P-384":
			if (willCreateSignatures)
			{
				return 877871941u;
			}
			return 861094725u;
		case "P-512":
		case "P-521":
			if (willCreateSignatures)
			{
				return 911426373u;
			}
			return 894649157u;
		default:
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX10645: Elliptical Curve not supported for curveId: '{0}'", curveId)));
		}
	}

	[MethodImpl(MethodImplOptions.NoOptimization)]
	private static bool SupportsCNGKey()
	{
		try
		{
			_ = CngKeyBlobFormat.EccPrivateBlob;
			return true;
		}
		catch
		{
			return false;
		}
	}
}
