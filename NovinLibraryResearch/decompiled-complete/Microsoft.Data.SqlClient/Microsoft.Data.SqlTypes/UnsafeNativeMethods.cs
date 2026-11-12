using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Microsoft.Data.Common;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.Data.SqlTypes;

[SuppressUnmanagedCodeSecurity]
internal static class UnsafeNativeMethods
{
	internal enum Method
	{
		METHOD_BUFFERED,
		METHOD_IN_DIRECT,
		METHOD_OUT_DIRECT,
		METHOD_NEITHER
	}

	internal enum Access
	{
		FILE_ANY_ACCESS,
		FILE_READ_ACCESS,
		FILE_WRITE_ACCESS
	}

	internal enum FileType : uint
	{
		Unknown = 0u,
		Disk = 1u,
		Char = 2u,
		Pipe = 3u,
		Remote = 32768u
	}

	internal struct OBJECT_ATTRIBUTES
	{
		internal int length;

		internal IntPtr rootDirectory;

		internal SafeHandle objectName;

		internal int attributes;

		internal IntPtr securityDescriptor;

		internal SafeHandle securityQualityOfService;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct UNICODE_STRING
	{
		internal ushort length;

		internal ushort maximumLength;

		internal string buffer;
	}

	internal enum SecurityImpersonationLevel
	{
		SecurityAnonymous,
		SecurityIdentification,
		SecurityImpersonation,
		SecurityDelegation
	}

	internal struct SECURITY_QUALITY_OF_SERVICE
	{
		internal uint length;

		[MarshalAs(UnmanagedType.I4)]
		internal int impersonationLevel;

		internal byte contextDynamicTrackingMode;

		internal byte effectiveOnly;
	}

	internal struct IO_STATUS_BLOCK
	{
		internal uint status;

		internal IntPtr information;
	}

	internal struct FILE_FULL_EA_INFORMATION
	{
		internal uint nextEntryOffset;

		internal byte flags;

		internal byte EaNameLength;

		internal ushort EaValueLength;

		internal byte EaName;
	}

	[Flags]
	internal enum CreateOption : uint
	{
		FILE_WRITE_THROUGH = 2u,
		FILE_SEQUENTIAL_ONLY = 4u,
		FILE_NO_INTERMEDIATE_BUFFERING = 8u,
		FILE_SYNCHRONOUS_IO_NONALERT = 0x20u,
		FILE_RANDOM_ACCESS = 0x800u
	}

	internal enum CreationDisposition : uint
	{
		FILE_SUPERSEDE,
		FILE_OPEN,
		FILE_CREATE,
		FILE_OPEN_IF,
		FILE_OVERWRITE,
		FILE_OVERWRITE_IF
	}

	[Flags]
	internal enum Attributes : uint
	{
		Inherit = 2u,
		Permanent = 0x10u,
		Exclusive = 0x20u,
		CaseInsensitive = 0x40u,
		OpenIf = 0x80u,
		OpenLink = 0x100u,
		KernelHandle = 0x200u,
		ForceAccessCheck = 0x400u,
		ValidAttributes = 0x7F2u
	}

	private static readonly Version ThreadErrorModeMinOsVersion = new Version(6, 1, 7600);

	internal const ushort FILE_DEVICE_FILE_SYSTEM = 9;

	internal const int ERROR_INVALID_HANDLE = 6;

	internal const int ERROR_MR_MID_NOT_FOUND = 317;

	internal const uint STATUS_INVALID_PARAMETER = 3221225485u;

	internal const uint STATUS_SHARING_VIOLATION = 3221225539u;

	internal const uint STATUS_OBJECT_NAME_NOT_FOUND = 3221225524u;

	internal const uint SEM_FAILCRITICALERRORS = 1u;

	internal const int FILE_READ_DATA = 1;

	internal const int FILE_WRITE_DATA = 2;

	internal const int FILE_READ_ATTRIBUTES = 128;

	internal const int SYNCHRONIZE = 1048576;

	[DllImport("NtDll.dll", CharSet = CharSet.Unicode)]
	internal static extern uint NtCreateFile(out SafeFileHandle fileHandle, int desiredAccess, ref OBJECT_ATTRIBUTES objectAttributes, out IO_STATUS_BLOCK ioStatusBlock, ref long allocationSize, uint fileAttributes, FileShare shareAccess, uint createDisposition, uint createOptions, SafeHandle eaBuffer, uint eaLength);

	[DllImport("Kernel32.dll", SetLastError = true)]
	internal static extern FileType GetFileType(SafeFileHandle hFile);

	[DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	private static extern int GetFullPathName(string path, int numBufferChars, StringBuilder buffer, IntPtr lpFilePartOrNull);

	internal static string SafeGetFullPathName(string path)
	{
		StringBuilder stringBuilder = new StringBuilder(path.Length + 1);
		int fullPathName = GetFullPathName(path, stringBuilder.Capacity, stringBuilder, IntPtr.Zero);
		if (fullPathName > stringBuilder.Capacity)
		{
			stringBuilder.Capacity = fullPathName;
			fullPathName = GetFullPathName(path, stringBuilder.Capacity, stringBuilder, IntPtr.Zero);
		}
		if (fullPathName == 0)
		{
			int lastWin32Error = Marshal.GetLastWin32Error();
			if (lastWin32Error == 0)
			{
				throw ADP.Argument(StringsHelper.GetString(Strings.SqlFileStream_InvalidPath), "path");
			}
			Win32Exception ex = new Win32Exception(lastWin32Error);
			ADP.TraceExceptionAsReturnValue(ex);
			throw ex;
		}
		return stringBuilder.ToString();
	}

	[DllImport("Kernel32.dll", ExactSpelling = true)]
	private static extern uint SetErrorMode(uint mode);

	[DllImport("Kernel32.dll", ExactSpelling = true, SetLastError = true)]
	private static extern bool SetThreadErrorMode(uint newMode, out uint oldMode);

	internal static void SetErrorModeWrapper(uint mode, out uint oldMode)
	{
		if (Environment.OSVersion.Version >= ThreadErrorModeMinOsVersion)
		{
			if (!SetThreadErrorMode(mode, out oldMode))
			{
				throw new Win32Exception();
			}
		}
		else
		{
			oldMode = SetErrorMode(mode);
		}
	}

	[DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	internal static extern bool DeviceIoControl(SafeFileHandle fileHandle, uint ioControlCode, IntPtr inBuffer, uint cbInBuffer, IntPtr outBuffer, uint cbOutBuffer, out uint cbBytesReturned, IntPtr overlapped);

	[DllImport("NtDll.dll")]
	internal static extern uint RtlNtStatusToDosError(uint status);

	internal static uint CTL_CODE(ushort deviceType, ushort function, byte method, byte access)
	{
		if (function > 4095)
		{
			throw ADP.ArgumentOutOfRange("function");
		}
		return (uint)((deviceType << 16) | (access << 14) | (function << 2) | method);
	}
}
