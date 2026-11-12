#define TRACE
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.SqlServer.Management.Common;

internal class SafeNativeMethods
{
	[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	public static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, out SafeAccessTokenHandle phToken);

	internal static SafeAccessTokenHandle GetUserToken(string user, string domain, string password)
	{
		if (!LogonUser(user, domain, password, 2, 0, out var phToken))
		{
			int lastWin32Error = Marshal.GetLastWin32Error();
			Trace.TraceInformation("LogonUser failed with error code : {0}", lastWin32Error);
			throw new Win32Exception(lastWin32Error);
		}
		return phToken;
	}
}
