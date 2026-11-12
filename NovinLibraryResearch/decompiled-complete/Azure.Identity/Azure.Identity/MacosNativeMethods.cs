using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Azure.Identity;

internal static class MacosNativeMethods
{
	public static class Imports
	{
		private const string CoreFoundationLibrary = "/System/Library/Frameworks/CoreFoundation.framework/Versions/A/CoreFoundation";

		private const string SecurityLibrary = "/System/Library/Frameworks/Security.framework/Security";

		[DllImport("/System/Library/Frameworks/CoreFoundation.framework/Versions/A/CoreFoundation", CharSet = CharSet.Unicode)]
		[DefaultDllImportSearchPaths(DllImportSearchPath.SafeDirectories | DllImportSearchPath.AssemblyDirectory)]
		public static extern void CFRelease(IntPtr cfRef);

		[DllImport("/System/Library/Frameworks/Security.framework/Security")]
		[DefaultDllImportSearchPaths(DllImportSearchPath.SafeDirectories | DllImportSearchPath.AssemblyDirectory)]
		public static extern int SecKeychainFindGenericPassword(IntPtr keychainOrArray, int serviceNameLength, byte[] serviceName, int accountNameLength, byte[] accountName, out int passwordLength, out IntPtr passwordData, out IntPtr itemRef);

		[DllImport("/System/Library/Frameworks/Security.framework/Security")]
		[DefaultDllImportSearchPaths(DllImportSearchPath.SafeDirectories | DllImportSearchPath.AssemblyDirectory)]
		public static extern int SecKeychainAddGenericPassword(IntPtr keychain, int serviceNameLength, byte[] serviceName, int accountNameLength, byte[] accountName, int passwordLength, byte[] passwordData, out IntPtr itemRef);

		[DllImport("/System/Library/Frameworks/Security.framework/Security")]
		[DefaultDllImportSearchPaths(DllImportSearchPath.SafeDirectories | DllImportSearchPath.AssemblyDirectory)]
		public static extern int SecKeychainItemDelete(IntPtr itemRef);

		[DllImport("/System/Library/Frameworks/Security.framework/Security")]
		[DefaultDllImportSearchPaths(DllImportSearchPath.SafeDirectories | DllImportSearchPath.AssemblyDirectory)]
		public static extern int SecKeychainItemFreeContent(IntPtr attrList, IntPtr data);

		[DllImport("/System/Library/Frameworks/Security.framework/Security")]
		[DefaultDllImportSearchPaths(DllImportSearchPath.SafeDirectories | DllImportSearchPath.AssemblyDirectory)]
		public static extern IntPtr SecCopyErrorMessageString(int status, IntPtr reserved);
	}

	public const int SecStatusCodeSuccess = 0;

	public const int SecStatusCodeNoSuchKeychain = -25294;

	public const int SecStatusCodeInvalidKeychain = -25295;

	public const int SecStatusCodeAuthFailed = -25293;

	public const int SecStatusCodeDuplicateItem = -25299;

	public const int SecStatusCodeItemNotFound = -25300;

	public const int SecStatusCodeInteractionNotAllowed = -25308;

	public const int SecStatusCodeInteractionRequired = -25315;

	public const int SecStatusCodeNoSuchAttr = -25303;

	public static void SecKeychainFindGenericPassword(IntPtr keychainOrArray, string serviceName, string accountName, out int passwordLength, out IntPtr credentialsPtr, out IntPtr itemRef)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(serviceName);
		byte[] bytes2 = Encoding.UTF8.GetBytes(accountName);
		ThrowIfError(Imports.SecKeychainFindGenericPassword(keychainOrArray, bytes.Length, bytes, bytes2.Length, bytes2, out passwordLength, out credentialsPtr, out itemRef));
	}

	public static void SecKeychainAddGenericPassword(IntPtr keychainOrArray, string serviceName, string accountName, string password, out IntPtr itemRef)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(serviceName);
		byte[] bytes2 = Encoding.UTF8.GetBytes(accountName);
		byte[] bytes3 = Encoding.UTF8.GetBytes(password);
		ThrowIfError(Imports.SecKeychainAddGenericPassword(keychainOrArray, bytes.Length, bytes, bytes2.Length, bytes2, password.Length, bytes3, out itemRef));
	}

	public static void SecKeychainItemDelete(IntPtr itemRef)
	{
		ThrowIfError(Imports.SecKeychainItemDelete(itemRef));
	}

	public static void SecKeychainItemFreeContent(IntPtr attrList, IntPtr data)
	{
		ThrowIfError(Imports.SecKeychainItemFreeContent(attrList, data));
	}

	public static void CFRelease(IntPtr cfRef)
	{
		if (cfRef != IntPtr.Zero)
		{
			Imports.CFRelease(cfRef);
		}
	}

	private static void ThrowIfError(int status)
	{
		if (status != 0)
		{
			throw new InvalidOperationException(GetErrorMessageString(status));
		}
	}

	private static string GetErrorMessageString(int status)
	{
		return status switch
		{
			-25294 => $"The keychain does not exist. [{status}]", 
			-25295 => $"The keychain is not valid. [{status}]", 
			-25293 => $"Authorization/Authentication failed. [{status}]", 
			-25299 => $"The item already exists. [{status}]", 
			-25300 => $"The item cannot be found. [{status}]", 
			-25308 => $"Interaction with the Security Server is not allowed. [{status}]", 
			-25315 => $"User interaction is required. [{status}]", 
			-25303 => $"The attribute does not exist. [{status}]", 
			_ => $"Unknown error. [{status}]", 
		};
	}
}
