using System.Runtime.InteropServices;

namespace Microsoft.Identity.Client.Extensions.Msal;

internal static class LinuxNativeMethods
{
	public const int RootUserId = 0;

	[DllImport("libc")]
	public static extern int getuid();
}
