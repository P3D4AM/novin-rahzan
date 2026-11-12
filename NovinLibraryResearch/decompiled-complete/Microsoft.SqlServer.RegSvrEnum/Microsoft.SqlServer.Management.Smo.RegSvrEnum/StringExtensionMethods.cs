using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;

namespace Microsoft.SqlServer.Management.Smo.RegSvrEnum;

internal static class StringExtensionMethods
{
	public static string SecureStringToString(this SecureString secureString)
	{
		return new string(secureString.SecureStringToCharArray());
	}

	public static SecureString StringToSecureString(this string unsecureString)
	{
		return unsecureString.ToCharArray().CharArrayToSecureString();
	}

	private static char[] SecureStringToCharArray(this SecureString secureString)
	{
		char[] array = new char[secureString.Length];
		IntPtr intPtr = Marshal.SecureStringToGlobalAllocUnicode(secureString);
		try
		{
			Marshal.Copy(intPtr, array, 0, secureString.Length);
			return array;
		}
		finally
		{
			Marshal.ZeroFreeGlobalAllocUnicode(intPtr);
		}
	}

	private static SecureString CharArrayToSecureString(this IEnumerable<char> charArray)
	{
		SecureString secureString = new SecureString();
		foreach (char item in charArray)
		{
			secureString.AppendChar(item);
		}
		secureString.MakeReadOnly();
		return secureString;
	}
}
