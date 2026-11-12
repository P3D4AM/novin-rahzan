using System;
using System.Runtime.InteropServices;

namespace Brotli;

internal class Delegate32
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr BrotliEncoderCreateInstanceDelegate(IntPtr allocFunc, IntPtr freeFunc, IntPtr opaque);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate bool BrotliEncoderSetParameterDelegate(IntPtr state, BrotliEncoderParameter parameter, uint value);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate bool BrotliEncoderCompressStreamDelegate(IntPtr state, BrotliEncoderOperation op, ref uint availableIn, ref IntPtr nextIn, ref uint availableOut, ref IntPtr nextOut, out uint totalOut);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate bool BrotliEncoderIsFinishedDelegate(IntPtr state);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void BrotliEncoderDestroyInstanceDelegate(IntPtr state);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate uint BrotliEncoderVersionDelegate();

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr BrotliEncoderTakeOutputDelegate(IntPtr state, ref uint size);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr BrotliDecoderCreateInstanceDelegate(IntPtr allocFunc, IntPtr freeFunc, IntPtr opaque);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate bool BrotliDecoderSetParameter(IntPtr state, BrotliDecoderParameter param, uint value);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate BrotliDecoderResult BrotliDecoderDecompressStreamDelegate(IntPtr state, ref uint availableIn, ref IntPtr nextIn, ref uint availableOut, ref IntPtr nextOut, out uint totalOut);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void BrotliDecoderDestroyInstanceDelegate(IntPtr state);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate uint BrotliDecoderVersionDelegate();

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate bool BrotliDecoderIsUsedDelegate(IntPtr state);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate bool BrotliDecoderIsFinishedDelegate(IntPtr state);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate int BrotliDecoderGetErrorCodeDelegate(IntPtr state);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr BrotliDecoderErrorStringDelegate(int code);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr BrotliDecoderTakeOutputDelegate(IntPtr state, ref uint size);
}
