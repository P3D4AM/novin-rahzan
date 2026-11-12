using System;
using System.Runtime.InteropServices;

namespace Brotli;

internal static class CoreCLRLoader
{
	[DllImport("libcoreclr.so")]
	internal static extern IntPtr dlopen(string filename, int flags);

	[DllImport("libcoreclr.so")]
	internal static extern IntPtr dlerror();

	[DllImport("libcoreclr.so")]
	internal static extern IntPtr dlsym(IntPtr handle, string symbol);

	[DllImport("libcoreclr.so")]
	internal static extern int dlclose(IntPtr handle);
}
