using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Microsoft.SqlServer.Management.HadrModel;

public static class StringExtensionMethods
{
	public static string SecureStringToString(this SecureString secureString)
	{
		return new string(SecureStringToCharArray(secureString));
	}

	public static SecureString StringToSecureString(this string unsecureString)
	{
		return CharArrayToSecureString(unsecureString.ToCharArray());
	}

	private static char[] SecureStringToCharArray(SecureString secureString)
	{
		if (secureString == null)
		{
			return null;
		}
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

	private static SecureString CharArrayToSecureString(char[] charArray)
	{
		if (charArray == null)
		{
			return null;
		}
		SecureString secureString = new SecureString();
		foreach (char c in charArray)
		{
			secureString.AppendChar(c);
		}
		secureString.MakeReadOnly();
		return secureString;
	}
}
