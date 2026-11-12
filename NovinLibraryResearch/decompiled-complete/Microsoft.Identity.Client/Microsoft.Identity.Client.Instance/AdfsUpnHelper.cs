using System;

namespace Microsoft.Identity.Client.Instance;

internal static class AdfsUpnHelper
{
	public static string GetDomainFromUpn(string upn)
	{
		if (!upn.Contains("@"))
		{
			throw new ArgumentException("userPrincipalName does not contain @ character.");
		}
		return upn.Split(new char[1] { '@' })[1];
	}
}
