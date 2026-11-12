using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Brotli;

public class Brolib
{
	private static bool UseX86 = IntPtr.Size == 4;

	public static IntPtr BrotliEncoderCreateInstance()
	{
		if (UseX86)
		{
			return Brolib32.BrotliEncoderCreateInstance(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
		}
		return Brolib64.BrotliEncoderCreateInstance(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
	}

	public static IntPtr GetModuleHandle(string moduleName)
	{
		IntPtr result = IntPtr.Zero;
		foreach (ProcessModule module in Process.GetCurrentProcess().Modules)
		{
			if (string.Compare(module.ModuleName, moduleName, ignoreCase: true) == 0)
			{
				result = module.BaseAddress;
				break;
			}
		}
		return result;
	}

	public static void FreeLibrary()
	{
		IntPtr zero = IntPtr.Zero;
		zero = GetModuleHandle(LibPathBootStrapper.LibPath);
		if (zero != IntPtr.Zero)
		{
			NativeLibraryLoader.FreeLibrary(zero);
		}
	}

	public static bool BrotliEncoderSetParameter(IntPtr state, BrotliEncoderParameter parameter, uint value)
	{
		if (UseX86)
		{
			return Brolib32.BrotliEncoderSetParameter(state, parameter, value);
		}
		return Brolib64.BrotliEncoderSetParameter(state, parameter, value);
	}

	public static bool BrotliEncoderCompressStream(IntPtr state, BrotliEncoderOperation op, ref uint availableIn, ref IntPtr nextIn, ref uint availableOut, ref IntPtr nextOut, out uint totalOut)
	{
		if (UseX86)
		{
			return Brolib32.BrotliEncoderCompressStream(state, op, ref availableIn, ref nextIn, ref availableOut, ref nextOut, out totalOut);
		}
		ulong availableIn2 = availableIn;
		ulong availableOut2 = availableOut;
		ulong totalOut2 = 0uL;
		bool result = Brolib64.BrotliEncoderCompressStream(state, op, ref availableIn2, ref nextIn, ref availableOut2, ref nextOut, out totalOut2);
		availableIn = (uint)availableIn2;
		availableOut = (uint)availableOut2;
		totalOut = (uint)totalOut2;
		return result;
	}

	public static bool BrotliEncoderIsFinished(IntPtr state)
	{
		if (UseX86)
		{
			return Brolib32.BrotliEncoderIsFinished(state);
		}
		return Brolib64.BrotliEncoderIsFinished(state);
	}

	public static void BrotliEncoderDestroyInstance(IntPtr state)
	{
		if (UseX86)
		{
			Brolib32.BrotliEncoderDestroyInstance(state);
		}
		else
		{
			Brolib64.BrotliEncoderDestroyInstance(state);
		}
	}

	public static uint BrotliEncoderVersion()
	{
		if (UseX86)
		{
			return Brolib32.BrotliEncoderVersion();
		}
		return Brolib64.BrotliEncoderVersion();
	}

	public static IntPtr BrotliDecoderTakeOutput(IntPtr state, ref uint size)
	{
		if (UseX86)
		{
			return Brolib32.BrotliDecoderTakeOutput(state, ref size);
		}
		ulong size2 = size;
		IntPtr result = Brolib64.BrotliDecoderTakeOutput(state, ref size2);
		size = (uint)size2;
		return result;
	}

	public static IntPtr BrotliDecoderCreateInstance()
	{
		if (UseX86)
		{
			return Brolib32.BrotliDecoderCreateInstance(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
		}
		return Brolib64.BrotliDecoderCreateInstance(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
	}

	public static bool BrotliDecoderSetParameter(IntPtr state, BrotliDecoderParameter param, uint value)
	{
		if (UseX86)
		{
			return Brolib32.BrotliDecoderSetParameter(state, param, value);
		}
		return Brolib64.BrotliDecoderSetParameter(state, param, value);
	}

	public static BrotliDecoderResult BrotliDecoderDecompressStream(IntPtr state, ref uint availableIn, ref IntPtr nextIn, ref uint availableOut, ref IntPtr nextOut, out uint totalOut)
	{
		if (UseX86)
		{
			return Brolib32.BrotliDecoderDecompressStream(state, ref availableIn, ref nextIn, ref availableOut, ref nextOut, out totalOut);
		}
		ulong availableIn2 = availableIn;
		ulong availableOut2 = availableOut;
		ulong totalOut2 = 0uL;
		BrotliDecoderResult result = Brolib64.BrotliDecoderDecompressStream(state, ref availableIn2, ref nextIn, ref availableOut2, ref nextOut, out totalOut2);
		availableIn = (uint)availableIn2;
		availableOut = (uint)availableOut2;
		totalOut = (uint)totalOut2;
		return result;
	}

	public static void BrotliDecoderDestroyInstance(IntPtr state)
	{
		if (UseX86)
		{
			Brolib32.BrotliDecoderDestroyInstance(state);
		}
		else
		{
			Brolib64.BrotliDecoderDestroyInstance(state);
		}
	}

	public static uint BrotliDecoderVersion()
	{
		if (UseX86)
		{
			return Brolib32.BrotliDecoderVersion();
		}
		return Brolib64.BrotliDecoderVersion();
	}

	public static bool BrotliDecoderIsUsed(IntPtr state)
	{
		if (UseX86)
		{
			return Brolib32.BrotliDecoderIsUsed(state);
		}
		return Brolib64.BrotliDecoderIsUsed(state);
	}

	public static bool BrotliDecoderIsFinished(IntPtr state)
	{
		if (UseX86)
		{
			return Brolib32.BrotliDecoderIsFinished(state);
		}
		return Brolib64.BrotliDecoderIsFinished(state);
	}

	public static int BrotliDecoderGetErrorCode(IntPtr state)
	{
		if (UseX86)
		{
			return Brolib32.BrotliDecoderGetErrorCode(state);
		}
		return Brolib64.BrotliDecoderGetErrorCode(state);
	}

	public static string BrotliDecoderErrorString(int code)
	{
		IntPtr zero = IntPtr.Zero;
		zero = ((!UseX86) ? Brolib64.BrotliDecoderErrorString(code) : Brolib32.BrotliDecoderErrorString(code));
		if (zero != IntPtr.Zero)
		{
			return Marshal.PtrToStringAnsi(zero);
		}
		return string.Empty;
	}

	public static IntPtr BrotliEncoderTakeOutput(IntPtr state, ref uint size)
	{
		if (UseX86)
		{
			return Brolib32.BrotliEncoderTakeOutput(state, ref size);
		}
		ulong size2 = size;
		IntPtr result = Brolib64.BrotliEncoderTakeOutput(state, ref size2);
		size = (uint)size2;
		return result;
	}
}
