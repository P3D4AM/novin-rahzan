using System;
using System.Runtime.InteropServices;

namespace Brotli;

internal static class LinuxLoader
{
	[DllImport("libdl.so")]
	internal static extern IntPtr dlopen(string filename, int flags);

	[DllImport("libdl.so")]
	internal static extern IntPtr dlerror();

	[DllImport("libdl.so")]
	internal static extern IntPtr dlsym(IntPtr handle, string symbol);

	[DllImport("libdl.so")]
	internal static extern int dlclose(IntPtr handle);
}
