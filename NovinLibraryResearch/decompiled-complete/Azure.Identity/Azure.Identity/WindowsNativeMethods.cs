using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Azure.Identity;

internal static class WindowsNativeMethods
{
	public enum CRED_PERSIST : uint
	{
		CRED_PERSIST_SESSION = 1u,
		CRED_PERSIST_LOCAL_MACHINE,
		CRED_PERSIST_ENTERPRISE
	}

	public enum CRED_TYPE
	{
		GENERIC = 1,
		DOMAIN_PASSWORD,
		DOMAIN_CERTIFICATE,
		DOMAIN_VISIBLE_PASSWORD,
		MAXIMUM
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct CredentialData
	{
		public uint Flags;

		public CRED_TYPE Type;

		public string TargetName;

		public string Comment;

		public FILETIME LastWritten;

		public uint CredentialBlobSize;

		public IntPtr CredentialBlob;

		public CRED_PERSIST Persist;

		public uint AttributeCount;

		public IntPtr Attributes;

		public string TargetAlias;

		public string UserName;
	}

	private static class Imports
	{
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32 | DllImportSearchPath.AssemblyDirectory)]
		public static extern uint FormatMessage(uint dwFlags, IntPtr lpSource, int dwMessageId, uint dwLanguageId, ref IntPtr lpBuffer, uint nSize, IntPtr pArguments);

		[DllImport("ntdll.dll")]
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32 | DllImportSearchPath.AssemblyDirectory)]
		public static extern int RtlNtStatusToDosError(int Status);

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, EntryPoint = "CredReadW", SetLastError = true)]
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32 | DllImportSearchPath.AssemblyDirectory)]
		public static extern bool CredRead(string target, CRED_TYPE type, int reservedFlag, out IntPtr userCredential);

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, EntryPoint = "CredWriteW", SetLastError = true)]
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32 | DllImportSearchPath.AssemblyDirectory)]
		public static extern bool CredWrite(IntPtr userCredential, int reservedFlag);

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, EntryPoint = "CredDeleteW", SetLastError = true)]
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32 | DllImportSearchPath.AssemblyDirectory)]
		public static extern bool CredDelete(string target, CRED_TYPE type, int reservedFlag);

		[DllImport("advapi32.dll", SetLastError = true)]
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32 | DllImportSearchPath.AssemblyDirectory)]
		public static extern void CredFree([In] IntPtr buffer);
	}

	public const int ERROR_NOT_FOUND = 1168;

	public const uint FORMAT_MESSAGE_ALLOCATE_BUFFER = 256u;

	public const uint FORMAT_MESSAGE_IGNORE_INSERTS = 512u;

	public const uint FORMAT_MESSAGE_FROM_SYSTEM = 4096u;

	public static IntPtr CredRead(string target, CRED_TYPE type)
	{
		ThrowIfFailed(Imports.CredRead(target, type, 0, out var userCredential), "CredRead");
		return userCredential;
	}

	public static void CredWrite(IntPtr userCredential)
	{
		ThrowIfFailed(Imports.CredWrite(userCredential, 0), "CredWrite");
	}

	public static void CredDelete(string target, CRED_TYPE type)
	{
		ThrowIfFailed(Imports.CredDelete(target, type, 0), "CredDelete");
	}

	public static void CredFree(IntPtr userCredential)
	{
		if (userCredential != IntPtr.Zero)
		{
			Imports.CredFree(userCredential);
		}
	}

	private static void ThrowIfFailed(bool isSucceeded, [CallerMemberName] string methodName = null)
	{
		if (isSucceeded)
		{
			return;
		}
		int lastWin32Error = Marshal.GetLastWin32Error();
		throw new InvalidOperationException((lastWin32Error == 1168) ? (methodName + " has failed but error is unknown.") : MessageFromErrorCode(lastWin32Error));
	}

	private static string MessageFromErrorCode(int errorCode)
	{
		uint dwFlags = 4864u;
		IntPtr lpBuffer = IntPtr.Zero;
		string text = null;
		try
		{
			if (Imports.FormatMessage(dwFlags, IntPtr.Zero, errorCode, 0u, ref lpBuffer, 0u, IntPtr.Zero) == 0)
			{
				return new Win32Exception(Imports.RtlNtStatusToDosError(errorCode)).Message;
			}
		}
		finally
		{
			if (lpBuffer != IntPtr.Zero)
			{
				text = Marshal.PtrToStringUni(lpBuffer);
				Marshal.FreeHGlobal(lpBuffer);
			}
		}
		return text ?? string.Empty;
	}
}
