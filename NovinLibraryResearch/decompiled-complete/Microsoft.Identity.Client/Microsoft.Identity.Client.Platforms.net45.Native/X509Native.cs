using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.Identity.Client.Platforms.net45.Native;

internal static class X509Native
{
	internal enum AcquireCertificateKeyOptions
	{
		None = 0,
		AcquireOnlyNCryptKeys = 0x40000
	}

	[SuppressUnmanagedCodeSecurity]
	internal static class UnsafeNativeMethods
	{
		[DllImport("crypt32.dll")]
		internal static extern SafeCertContextHandle CertDuplicateCertificateContext(IntPtr certContext);

		[DllImport("crypt32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool CryptAcquireCertificatePrivateKey(SafeCertContextHandle pCert, AcquireCertificateKeyOptions dwFlags, IntPtr pvReserved, out SafeNCryptKeyHandle phCryptProvOrNCryptKey, out int dwKeySpec, [MarshalAs(UnmanagedType.Bool)] out bool pfCallerFreeProvOrNCryptKey);
	}

	[SecurityCritical]
	internal static SafeCertContextHandle DuplicateCertContext(IntPtr context)
	{
		return UnsafeNativeMethods.CertDuplicateCertificateContext(context);
	}

	[SecurityCritical]
	internal static SafeNCryptKeyHandle AcquireCngPrivateKey(SafeCertContextHandle certificateContext)
	{
		bool pfCallerFreeProvOrNCryptKey = true;
		SafeNCryptKeyHandle phCryptProvOrNCryptKey = null;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			int dwKeySpec = 0;
			if (!UnsafeNativeMethods.CryptAcquireCertificatePrivateKey(certificateContext, AcquireCertificateKeyOptions.AcquireOnlyNCryptKeys, IntPtr.Zero, out phCryptProvOrNCryptKey, out dwKeySpec, out pfCallerFreeProvOrNCryptKey))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			return phCryptProvOrNCryptKey;
		}
		finally
		{
			if (phCryptProvOrNCryptKey != null && !pfCallerFreeProvOrNCryptKey)
			{
				bool success = false;
				phCryptProvOrNCryptKey.DangerousAddRef(ref success);
			}
		}
	}
}
