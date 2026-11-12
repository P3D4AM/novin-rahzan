using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;

namespace Microsoft.Data.Common;

[SuppressUnmanagedCodeSecurity]
internal static class SafeNativeMethods
{
	internal sealed class Wrapper
	{
		private Wrapper()
		{
		}

		internal static void ClearErrorInfo()
		{
			SetErrorInfo(0, ADP.PtrZero);
		}
	}

	[DllImport("ole32.dll")]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	internal static extern IntPtr CoTaskMemAlloc(IntPtr cb);

	[DllImport("ole32.dll")]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal static extern void CoTaskMemFree(IntPtr handle);

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
	internal static extern int GetUserDefaultLCID();

	[DllImport("kernel32.dll")]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal static extern void ZeroMemory(IntPtr dest, IntPtr length);

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	internal unsafe static IntPtr InterlockedExchangePointer(IntPtr lpAddress, IntPtr lpValue)
	{
		IntPtr intPtr = *(IntPtr*)lpAddress.ToPointer();
		IntPtr intPtr2;
		do
		{
			intPtr2 = intPtr;
			intPtr = Interlocked.CompareExchange(ref *(IntPtr*)lpAddress.ToPointer(), lpValue, intPtr2);
		}
		while (intPtr != intPtr2);
		return intPtr;
	}

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetComputerNameExW", SetLastError = true)]
	internal static extern int GetComputerNameEx(int nameType, StringBuilder nameBuffer, ref int bufferSize);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
	internal static extern int GetCurrentProcessId();

	[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, ThrowOnUnmappableChar = true)]
	internal static extern IntPtr GetModuleHandle([In][MarshalAs(UnmanagedType.LPTStr)] string moduleName);

	[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = true)]
	internal static extern IntPtr GetProcAddress(IntPtr HModule, [In][MarshalAs(UnmanagedType.LPStr)] string funcName);

	[DllImport("kernel32.dll", SetLastError = true)]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	internal static extern IntPtr LocalAlloc(int flags, IntPtr countOfBytes);

	[DllImport("kernel32.dll", SetLastError = true)]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal static extern IntPtr LocalFree(IntPtr handle);

	[DllImport("oleaut32.dll", CharSet = CharSet.Unicode)]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	internal static extern IntPtr SysAllocStringLen(string src, int len);

	[DllImport("oleaut32.dll")]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal static extern void SysFreeString(IntPtr bstr);

	[DllImport("oleaut32.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
	private static extern void SetErrorInfo(int dwReserved, IntPtr pIErrorInfo);

	[DllImport("kernel32.dll", SetLastError = true)]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	internal static extern int ReleaseSemaphore(IntPtr handle, int releaseCount, IntPtr previousCount);

	[DllImport("kernel32.dll", SetLastError = true)]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	internal static extern int WaitForMultipleObjectsEx(uint nCount, IntPtr lpHandles, bool bWaitAll, uint dwMilliseconds, bool bAlertable);

	[DllImport("kernel32.dll")]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	internal static extern int WaitForSingleObjectEx(IntPtr lpHandles, uint dwMilliseconds, bool bAlertable);

	[DllImport("ole32.dll", PreserveSig = false)]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal static extern void PropVariantClear(IntPtr pObject);

	[DllImport("oleaut32.dll", PreserveSig = false)]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal static extern void VariantClear(IntPtr pObject);
}
