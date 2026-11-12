using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Brotli;

internal class NativeLibraryLoader
{
	internal static bool IsWindows;

	internal static bool IsLinux;

	internal static bool IsMacOSX;

	internal static bool IsNetCore;

	internal static bool Is64Bit;

	private const int RtldLazy = 1;

	private const int RtldGlobal = 8;

	private readonly string _libraryPath;

	internal IntPtr Handle { get; private set; }

	static NativeLibraryLoader()
	{
		IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
		IsLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
		IsMacOSX = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
		IsNetCore = RuntimeInformation.FrameworkDescription.StartsWith(".NET Core");
		if (!IsWindows && !IsLinux && !IsMacOSX)
		{
			throw new InvalidOperationException("Unsupported platform.");
		}
		Is64Bit = IntPtr.Size == 8;
	}

	internal NativeLibraryLoader(string libraryPath)
	{
		_libraryPath = libraryPath;
		Handle = LoadLibrary(_libraryPath, out var errorMsg);
		if (!string.IsNullOrEmpty(errorMsg))
		{
			throw new BrotliException("unable to load library " + libraryPath);
		}
	}

	public static IntPtr GetWin32ModuleHandle(string moduleName)
	{
		IntPtr result = IntPtr.Zero;
		foreach (ProcessModule module in Process.GetCurrentProcess().Modules)
		{
			if (module.ModuleName == moduleName)
			{
				result = module.BaseAddress;
				break;
			}
		}
		return result;
	}

	private IntPtr LoadSymbol(string symbolName)
	{
		if (IsWindows)
		{
			return WindowsLoader.GetProcAddress(Handle, symbolName);
		}
		if (IsLinux)
		{
			if (IsNetCore)
			{
				return CoreCLRLoader.dlsym(Handle, symbolName);
			}
			return LinuxLoader.dlsym(Handle, symbolName);
		}
		if (IsMacOSX)
		{
			return MacOSXLoader.dlsym(Handle, symbolName);
		}
		throw new InvalidOperationException("Unsupported platform.");
	}

	public void FillDelegate<T>(out T delegateType) where T : class
	{
		string text = typeof(T).Name;
		string text2 = "Delegate";
		if (text.EndsWith(text2))
		{
			text = text.Substring(0, text.Length - text2.Length);
		}
		delegateType = GetNativeMethodDelegate<T>(text);
	}

	public T GetNativeMethodDelegate<T>(string methodName) where T : class
	{
		IntPtr intPtr = LoadSymbol(methodName);
		if (intPtr == IntPtr.Zero)
		{
			throw new MissingMethodException($"The native method \"{methodName}\" does not exist");
		}
		return Marshal.GetDelegateForFunctionPointer(intPtr, typeof(T)) as T;
	}

	internal static string[] GetPossibleRuntimeDirectories()
	{
		string directoryName = Path.GetDirectoryName(typeof(LibPathBootStrapper).GetTypeInfo().Assembly.Location);
		string arg = "win";
		if (IsLinux)
		{
			arg = "linux";
		}
		if (IsMacOSX)
		{
			arg = "osx";
		}
		string text = $"runtimes/{arg}/native";
		string text2 = Path.Combine(directoryName, text);
		string text3 = AppDomain.CurrentDomain.BaseDirectory + "bin/" + text;
		string text4 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Replace("file:\\", "") + "/" + text;
		string text5 = Path.Combine(directoryName, "../..", text);
		return new string[6] { directoryName, text2, text, text5, text3, text4 }.Select((string v) => v.Replace('/', Path.DirectorySeparatorChar)).ToArray();
	}

	internal static bool FreeLibrary(IntPtr handle)
	{
		string errorMsg = null;
		if (IsWindows)
		{
			return WindowsLoader.FreeLibrary(handle);
		}
		if (IsLinux)
		{
			if (IsNetCore)
			{
				return UnloadLibraryPosix(CoreCLRLoader.dlclose, CoreCLRLoader.dlerror, handle, out errorMsg);
			}
			return UnloadLibraryPosix(LinuxLoader.dlclose, LinuxLoader.dlerror, handle, out errorMsg);
		}
		if (IsMacOSX)
		{
			return UnloadLibraryPosix(MacOSXLoader.dlclose, MacOSXLoader.dlerror, handle, out errorMsg);
		}
		throw new InvalidOperationException("Unsupported platform.");
	}

	internal static IntPtr LoadLibrary(string libraryPath, out string errorMsg)
	{
		if (IsWindows)
		{
			errorMsg = null;
			IntPtr intPtr = WindowsLoader.LoadLibrary(libraryPath);
			if (intPtr == IntPtr.Zero)
			{
				throw new Win32Exception("failed to load library " + libraryPath);
			}
			return intPtr;
		}
		if (IsLinux)
		{
			if (IsNetCore)
			{
				return LoadLibraryPosix(CoreCLRLoader.dlopen, CoreCLRLoader.dlerror, libraryPath, out errorMsg);
			}
			return LoadLibraryPosix(LinuxLoader.dlopen, LinuxLoader.dlerror, libraryPath, out errorMsg);
		}
		if (IsMacOSX)
		{
			return LoadLibraryPosix(MacOSXLoader.dlopen, MacOSXLoader.dlerror, libraryPath, out errorMsg);
		}
		throw new InvalidOperationException("Unsupported platform.");
	}

	private static IntPtr LoadLibraryPosix(Func<string, int, IntPtr> dlopenFunc, Func<IntPtr> dlerrorFunc, string libraryPath, out string errorMsg)
	{
		errorMsg = null;
		IntPtr intPtr = dlopenFunc(libraryPath, 9);
		if (intPtr == IntPtr.Zero)
		{
			errorMsg = Marshal.PtrToStringAnsi(dlerrorFunc());
		}
		return intPtr;
	}

	private static bool UnloadLibraryPosix(Func<IntPtr, int> dlcloseFunc, Func<IntPtr> dlerrorFunc, IntPtr handle, out string errorMsg)
	{
		errorMsg = null;
		if (dlcloseFunc(handle) != 0)
		{
			errorMsg = Marshal.PtrToStringAnsi(dlerrorFunc());
			return false;
		}
		return true;
	}
}
