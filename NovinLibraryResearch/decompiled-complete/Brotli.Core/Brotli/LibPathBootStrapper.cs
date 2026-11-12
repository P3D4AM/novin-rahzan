using System;
using System.IO;

namespace Brotli;

internal class LibPathBootStrapper
{
	internal static string LibPath { get; private set; }

	static LibPathBootStrapper()
	{
		string text = null;
		if (NativeLibraryLoader.IsWindows)
		{
			text = ((!NativeLibraryLoader.Is64Bit) ? "brolib_x86.dll" : "brolib_x64.dll");
		}
		else if (NativeLibraryLoader.IsLinux)
		{
			text = ((!NativeLibraryLoader.Is64Bit) ? "brolib_x86.so" : "brolib_x64.so");
		}
		else if (NativeLibraryLoader.IsMacOSX && NativeLibraryLoader.Is64Bit)
		{
			text = "brolib_x64.dylib";
		}
		if (string.IsNullOrEmpty(text))
		{
			throw new NotSupportedException("OS not supported:" + Environment.OSVersion.ToString());
		}
		string[] possibleRuntimeDirectories = NativeLibraryLoader.GetPossibleRuntimeDirectories();
		bool flag = false;
		string[] array = possibleRuntimeDirectories;
		for (int i = 0; i < array.Length; i++)
		{
			string text2 = Path.Combine(array[i], text);
			if (File.Exists(text2))
			{
				LibPath = text2;
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			throw new NotSupportedException("Unable to find library " + text);
		}
	}
}
