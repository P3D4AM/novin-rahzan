using System.Security;

namespace Azure.Identity;

internal static class StringExtensions
{
	public static SecureString ToSecureString(this string plainString)
	{
		if (plainString == null)
		{
			return null;
		}
		SecureString secureString = new SecureString();
		char[] array = plainString.ToCharArray();
		foreach (char c in array)
		{
			secureString.AppendChar(c);
		}
		return secureString;
	}
}
