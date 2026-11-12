using System;
using System.Runtime.InteropServices;

namespace Brotli;

internal static class MacOSXLoader
{
	[DllImport("libSystem.dylib")]
	internal static extern IntPtr dlopen(string filename, int flags);

	[DllImport("libSystem.dylib")]
	internal static extern IntPtr dlerror();

	[DllImport("libSystem.dylib")]
	internal static extern IntPtr dlsym(IntPtr handle, string symbol);

	[DllImport("libSystem.dylib")]
	internal static extern int dlclose(IntPtr handle);
}
