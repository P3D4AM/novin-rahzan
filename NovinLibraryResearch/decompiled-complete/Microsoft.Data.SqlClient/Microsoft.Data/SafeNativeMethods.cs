using System;
using System.Runtime.InteropServices;

namespace Microsoft.Data;

internal static class SafeNativeMethods
{
	[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = true)]
	internal static extern IntPtr GetProcAddress(IntPtr HModule, [In][MarshalAs(UnmanagedType.LPStr)] string funcName);
}
