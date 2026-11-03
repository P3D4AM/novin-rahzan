using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace DevComponents.Schedule;

internal class _2A34
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct _2A2C
	{
		[MarshalAs(UnmanagedType.I4)]
		public int _2A2D;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string _2A2E;

		public _2A35 _2A2F;

		[MarshalAs(UnmanagedType.I4)]
		public int _2A30;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string _2A31;

		public _2A35 _2A32;

		[MarshalAs(UnmanagedType.I4)]
		public int _2A33;

		public _2A2C(_2A3E dtzi)
		{
			_2A2D = dtzi._2A2D;
			_2A2E = dtzi._2A2E;
			_2A2F = dtzi._2A2F;
			_2A30 = dtzi._2A30;
			_2A31 = dtzi._2A31;
			_2A32 = dtzi._2A32;
			_2A33 = dtzi._2A33;
		}
	}

	internal struct _2A35
	{
		[MarshalAs(UnmanagedType.U2)]
		public short _2A36;

		[MarshalAs(UnmanagedType.U2)]
		public short _2A37;

		[MarshalAs(UnmanagedType.U2)]
		public short _2A38;

		[MarshalAs(UnmanagedType.U2)]
		public short _2A39;

		[MarshalAs(UnmanagedType.U2)]
		public short _2A3A;

		[MarshalAs(UnmanagedType.U2)]
		public short _2A3B;

		[MarshalAs(UnmanagedType.U2)]
		public short _2A3C;

		[MarshalAs(UnmanagedType.U2)]
		public short _2A3D;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct _2A3E
	{
		[MarshalAs(UnmanagedType.I4)]
		public int _2A2D;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string _2A2E;

		public _2A35 _2A2F;

		[MarshalAs(UnmanagedType.I4)]
		public int _2A30;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string _2A31;

		public _2A35 _2A32;

		[MarshalAs(UnmanagedType.I4)]
		public int _2A33;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string _2A3F;
	}

	internal struct _2A40
	{
		[MarshalAs(UnmanagedType.I4)]
		public int _2A2D;

		[MarshalAs(UnmanagedType.I4)]
		public int _2A30;

		[MarshalAs(UnmanagedType.I4)]
		public int _2A33;

		public _2A35 _2A2F;

		public _2A35 _2A32;

		public _2A40(_2A2C tzi)
		{
			_2A2D = tzi._2A2D;
			_2A2F = tzi._2A2F;
			_2A30 = tzi._2A30;
			_2A32 = tzi._2A32;
			_2A33 = tzi._2A33;
		}

		public _2A40(byte[] bytes)
		{
			if (bytes == null || bytes.Length != 44)
			{
				throw new ArgumentException("Argument_InvalidREG_TZI_FORMAT", "bytes");
			}
			_2A2D = BitConverter.ToInt32(bytes, 0);
			_2A30 = BitConverter.ToInt32(bytes, 4);
			_2A33 = BitConverter.ToInt32(bytes, 8);
			_2A2F._2A36 = BitConverter.ToInt16(bytes, 12);
			_2A2F._2A37 = BitConverter.ToInt16(bytes, 14);
			_2A2F._2A38 = BitConverter.ToInt16(bytes, 16);
			_2A2F._2A39 = BitConverter.ToInt16(bytes, 18);
			_2A2F._2A3A = BitConverter.ToInt16(bytes, 20);
			_2A2F._2A3B = BitConverter.ToInt16(bytes, 22);
			_2A2F._2A3C = BitConverter.ToInt16(bytes, 24);
			_2A2F._2A3D = BitConverter.ToInt16(bytes, 26);
			_2A32._2A36 = BitConverter.ToInt16(bytes, 28);
			_2A32._2A37 = BitConverter.ToInt16(bytes, 30);
			_2A32._2A38 = BitConverter.ToInt16(bytes, 32);
			_2A32._2A39 = BitConverter.ToInt16(bytes, 34);
			_2A32._2A3A = BitConverter.ToInt16(bytes, 36);
			_2A32._2A3B = BitConverter.ToInt16(bytes, 38);
			_2A32._2A3C = BitConverter.ToInt16(bytes, 40);
			_2A32._2A3D = BitConverter.ToInt16(bytes, 42);
		}
	}
}
[Flags]
internal enum _2A41
{
	ᘶ = 1,
	_2A42 = 2
}
[SuppressUnmanagedCodeSecurity]
internal static class _2A43
{
	[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
	internal static extern int GetDynamicTimeZoneInformation(out _2A34._2A3E lpDynamicTimeZoneInformation);

	[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
	internal static extern int GetTimeZoneInformation(out _2A34._2A2C lpTimeZoneInformation);

	[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	internal static extern bool GetFileMUIPath(int flags, [MarshalAs(UnmanagedType.LPWStr)] string filePath, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder language, ref int languageLength, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder fileMuiPath, ref int fileMuiPathLength, ref long enumerator);

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	[SecurityCritical]
	internal static extern _2A44 LoadLibraryEx(string libFilename, IntPtr reserved, int flags);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "LoadStringW", ExactSpelling = true, SetLastError = true)]
	[SecurityCritical]
	internal static extern int LoadString(_2A44 handle, int id, StringBuilder buffer, int bufferLength);
}
[SuppressUnmanagedCodeSecurity]
[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
internal sealed class _2A44 : SafeHandleZeroOrMinusOneIsInvalid
{
	internal _2A44()
		: base(ownsHandle: true)
	{
	}

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	private static extern bool FreeLibrary(IntPtr hModule);

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	internal static extern _2A44 LoadLibraryEx(string libFilename, IntPtr reserved, int flags);

	protected override bool ReleaseHandle()
	{
		return FreeLibrary(handle);
	}
}
