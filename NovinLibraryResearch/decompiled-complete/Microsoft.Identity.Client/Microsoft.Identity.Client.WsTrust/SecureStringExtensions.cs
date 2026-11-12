using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Microsoft.Identity.Client.WsTrust;

internal static class SecureStringExtensions
{
	public static char[] PasswordToCharArray(this SecureString secureString)
	{
		if (secureString == null)
		{
			return null;
		}
		char[] array = new char[secureString.Length];
		IntPtr intPtr = Marshal.SecureStringToCoTaskMemUnicode(secureString);
		for (int i = 0; i < secureString.Length; i++)
		{
			array[i] = (char)Marshal.ReadInt16(intPtr, i * 2);
		}
		Marshal.ZeroFreeCoTaskMemUnicode(intPtr);
		return array;
	}
}
