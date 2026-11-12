using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.SqlServer.Management.Smo.RegSvrEnum;

internal sealed class DataProtection
{
	private sealed class CryptoNativeMethods
	{
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct DATA_BLOB
		{
			public int cbData;

			public IntPtr pbData;
		}

		public const int UIForbidden = 1;

		public const int LocalMachine = 4;

		[DllImport("crypt32", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CryptProtectData(ref DATA_BLOB dataIn, string szDataDescr, IntPtr optionalEntropy, IntPtr pvReserved, IntPtr pPromptStruct, int dwFlags, ref DATA_BLOB pDataOut);

		[DllImport("crypt32", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CryptUnprotectData(ref DATA_BLOB dataIn, StringBuilder ppszDataDescr, IntPtr optionalEntropy, IntPtr pvReserved, IntPtr pPromptStruct, int dwFlags, ref DATA_BLOB pDataOut);

		[DllImport("kernel32")]
		internal static extern IntPtr LocalFree(IntPtr hMem);
	}

	private DataProtection()
	{
	}

	public static string ProtectData(string data)
	{
		return ProtectData(data, 1);
	}

	public static string UnprotectData(string data)
	{
		byte[] array = UnprotectData(Convert.FromBase64String(data));
		string result = null;
		if (array != null)
		{
			result = ((array.Length != 0) ? Encoding.Unicode.GetString(array) : null);
		}
		array = null;
		return result;
	}

	public static byte[] ProtectData(byte[] data)
	{
		return ProtectData(data, 1);
	}

	public static byte[] UnprotectData(byte[] data)
	{
		return UnprotectData(data, 1);
	}

	private static string ProtectData(string data, int flags)
	{
		byte[] array = ProtectData(Encoding.Unicode.GetBytes(data), flags);
		string result = null;
		if (array != null)
		{
			result = Convert.ToBase64String(array);
		}
		return result;
	}

	private static byte[] UnprotectData(byte[] data, int dwFlags)
	{
		byte[] array = null;
		CryptoNativeMethods.DATA_BLOB dataIn = default(CryptoNativeMethods.DATA_BLOB);
		StringBuilder ppszDataDescr = new StringBuilder();
		dataIn.cbData = data.Length;
		dataIn.pbData = Marshal.AllocHGlobal(dataIn.cbData);
		Marshal.Copy(data, 0, dataIn.pbData, dataIn.cbData);
		CryptoNativeMethods.DATA_BLOB pDataOut = default(CryptoNativeMethods.DATA_BLOB);
		try
		{
			if (CryptoNativeMethods.CryptUnprotectData(ref dataIn, ppszDataDescr, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, dwFlags, ref pDataOut))
			{
				array = new byte[pDataOut.cbData];
				Marshal.Copy(pDataOut.pbData, array, 0, pDataOut.cbData);
				Marshal.Copy(new byte[pDataOut.cbData], 0, pDataOut.pbData, pDataOut.cbData);
				CryptoNativeMethods.LocalFree(pDataOut.pbData);
				pDataOut.pbData = IntPtr.Zero;
				return array;
			}
			throw new Exception(SRError.CryptUnprotectDataWin32Error(Marshal.GetLastWin32Error()));
		}
		finally
		{
			if (dataIn.pbData != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(dataIn.pbData);
			}
			if (pDataOut.pbData != IntPtr.Zero)
			{
				CryptoNativeMethods.LocalFree(pDataOut.pbData);
				pDataOut.pbData = IntPtr.Zero;
			}
		}
	}

	private static byte[] ProtectData(byte[] data, int dwFlags)
	{
		string szDataDescr = "Default";
		byte[] array = null;
		CryptoNativeMethods.DATA_BLOB dataIn = default(CryptoNativeMethods.DATA_BLOB);
		dataIn.cbData = data.Length;
		dataIn.pbData = Marshal.AllocHGlobal(dataIn.cbData);
		Marshal.Copy(data, 0, dataIn.pbData, dataIn.cbData);
		CryptoNativeMethods.DATA_BLOB pDataOut = default(CryptoNativeMethods.DATA_BLOB);
		try
		{
			bool num = CryptoNativeMethods.CryptProtectData(ref dataIn, szDataDescr, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, dwFlags, ref pDataOut);
			Marshal.Copy(new byte[dataIn.cbData], 0, dataIn.pbData, dataIn.cbData);
			if (num)
			{
				array = new byte[pDataOut.cbData];
				Marshal.Copy(pDataOut.pbData, array, 0, pDataOut.cbData);
				CryptoNativeMethods.LocalFree(pDataOut.pbData);
				pDataOut.pbData = IntPtr.Zero;
				return array;
			}
			throw new Exception(SRError.CryptProtectDataWin32Error(Marshal.GetLastWin32Error()));
		}
		finally
		{
			if (dataIn.pbData != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(dataIn.pbData);
			}
			if (pDataOut.pbData != IntPtr.Zero)
			{
				CryptoNativeMethods.LocalFree(pDataOut.pbData);
				pDataOut.pbData = IntPtr.Zero;
			}
		}
	}
}
