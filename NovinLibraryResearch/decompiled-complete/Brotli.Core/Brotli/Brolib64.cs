namespace Brotli;

internal class Brolib64
{
	internal static Delegate64.BrotliEncoderCreateInstanceDelegate BrotliEncoderCreateInstance;

	internal static Delegate64.BrotliEncoderSetParameterDelegate BrotliEncoderSetParameter;

	internal static Delegate64.BrotliEncoderCompressStreamDelegate BrotliEncoderCompressStream;

	internal static Delegate64.BrotliEncoderIsFinishedDelegate BrotliEncoderIsFinished;

	internal static Delegate64.BrotliEncoderDestroyInstanceDelegate BrotliEncoderDestroyInstance;

	internal static Delegate64.BrotliEncoderVersionDelegate BrotliEncoderVersion;

	internal static Delegate64.BrotliEncoderTakeOutputDelegate BrotliEncoderTakeOutput;

	internal static Delegate64.BrotliDecoderCreateInstanceDelegate BrotliDecoderCreateInstance;

	internal static Delegate64.BrotliDecoderSetParameter BrotliDecoderSetParameter;

	internal static Delegate64.BrotliDecoderDecompressStreamDelegate BrotliDecoderDecompressStream;

	internal static Delegate64.BrotliDecoderDestroyInstanceDelegate BrotliDecoderDestroyInstance;

	internal static Delegate64.BrotliDecoderVersionDelegate BrotliDecoderVersion;

	internal static Delegate64.BrotliDecoderIsUsedDelegate BrotliDecoderIsUsed;

	internal static Delegate64.BrotliDecoderIsFinishedDelegate BrotliDecoderIsFinished;

	internal static Delegate64.BrotliDecoderGetErrorCodeDelegate BrotliDecoderGetErrorCode;

	internal static Delegate64.BrotliDecoderErrorStringDelegate BrotliDecoderErrorString;

	internal static Delegate64.BrotliDecoderTakeOutputDelegate BrotliDecoderTakeOutput;

	static Brolib64()
	{
		NativeLibraryLoader nativeLibraryLoader = new NativeLibraryLoader(LibPathBootStrapper.LibPath);
		nativeLibraryLoader.FillDelegate<Delegate64.BrotliEncoderCreateInstanceDelegate>(out BrotliEncoderCreateInstance);
		nativeLibraryLoader.FillDelegate<Delegate64.BrotliEncoderSetParameterDelegate>(out BrotliEncoderSetParameter);
		nativeLibraryLoader.FillDelegate<Delegate64.BrotliEncoderCompressStreamDelegate>(out BrotliEncoderCompressStream);
		nativeLibraryLoader.FillDelegate<Delegate64.BrotliEncoderIsFinishedDelegate>(out BrotliEncoderIsFinished);
		nativeLibraryLoader.FillDelegate<Delegate64.BrotliEncoderDestroyInstanceDelegate>(out BrotliEncoderDestroyInstance);
		nativeLibraryLoader.FillDelegate<Delegate64.BrotliEncoderVersionDelegate>(out BrotliEncoderVersion);
		nativeLibraryLoader.FillDelegate<Delegate64.BrotliEncoderVersionDelegate>(out BrotliEncoderVersion);
		nativeLibraryLoader.FillDelegate<Delegate64.BrotliEncoderTakeOutputDelegate>(out BrotliEncoderTakeOutput);
		nativeLibraryLoader.FillDelegate<Delegate64.BrotliDecoderCreateInstanceDelegate>(out BrotliDecoderCreateInstance);
		nativeLibraryLoader.FillDelegate<Delegate64.BrotliDecoderSetParameter>(out BrotliDecoderSetParameter);
		nativeLibraryLoader.FillDelegate<Delegate64.BrotliDecoderDecompressStreamDelegate>(out BrotliDecoderDecompressStream);
		nativeLibraryLoader.FillDelegate<Delegate64.BrotliDecoderDestroyInstanceDelegate>(out BrotliDecoderDestroyInstance);
		nativeLibraryLoader.FillDelegate<Delegate64.BrotliDecoderVersionDelegate>(out BrotliDecoderVersion);
		nativeLibraryLoader.FillDelegate<Delegate64.BrotliDecoderIsUsedDelegate>(out BrotliDecoderIsUsed);
		nativeLibraryLoader.FillDelegate<Delegate64.BrotliDecoderIsFinishedDelegate>(out BrotliDecoderIsFinished);
		nativeLibraryLoader.FillDelegate<Delegate64.BrotliDecoderGetErrorCodeDelegate>(out BrotliDecoderGetErrorCode);
		nativeLibraryLoader.FillDelegate<Delegate64.BrotliDecoderErrorStringDelegate>(out BrotliDecoderErrorString);
		nativeLibraryLoader.FillDelegate<Delegate64.BrotliDecoderTakeOutputDelegate>(out BrotliDecoderTakeOutput);
	}
}
