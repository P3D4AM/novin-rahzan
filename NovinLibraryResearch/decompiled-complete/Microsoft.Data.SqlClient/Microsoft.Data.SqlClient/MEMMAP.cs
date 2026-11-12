using System.Runtime.InteropServices;

namespace Microsoft.Data.SqlClient;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct MEMMAP
{
	[MarshalAs(UnmanagedType.U4)]
	internal uint dbgpid;

	[MarshalAs(UnmanagedType.U4)]
	internal uint fOption;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
	internal byte[] rgbMachineName;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
	internal byte[] rgbDllName;

	[MarshalAs(UnmanagedType.U4)]
	internal uint cbData;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 255)]
	internal byte[] rgbData;
}
