using System;
using System.Runtime.InteropServices;

namespace Microsoft.Identity.Client.Extensions.Msal;

internal static class MacNativeMethods
{
	public const string securityFrameworkLib = "/System/Library/Frameworks/Security.framework/Security";

	public const string coreFoundationFrameworkLib = "/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation";

	public const int errSecSuccess = 0;

	public const int errSecDuplicateItem = -25299;

	public const int errSecItemNotFound = -25300;

	[DllImport("/System/Library/Frameworks/Security.framework/Security")]
	internal static extern int SecKeychainFindGenericPassword(IntPtr keychainOrArray, uint serviceNameLength, string serviceName, uint accountNameLength, string accountName, out uint passwordLength, out IntPtr passwordData, out IntPtr itemRef);

	[DllImport("/System/Library/Frameworks/Security.framework/Security")]
	internal static extern int SecKeychainAddGenericPassword(IntPtr keychain, uint serviceNameLength, string serviceName, uint accountNameLength, string accountName, uint passwordLength, byte[] passwordData, out IntPtr itemRef);

	[DllImport("/System/Library/Frameworks/Security.framework/Security")]
	internal static extern int SecKeychainItemModifyAttributesAndData(IntPtr itemRef, IntPtr attrList, uint length, byte[] data);

	[DllImport("/System/Library/Frameworks/Security.framework/Security")]
	internal static extern int SecKeychainItemDelete(IntPtr itemRef);

	[DllImport("/System/Library/Frameworks/Security.framework/Security")]
	internal static extern int SecKeychainItemFreeContent(IntPtr attrList, IntPtr data);

	[DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
	internal static extern void CFRelease(IntPtr cf);
}
