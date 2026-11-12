namespace Brotli;

internal class Brolib32
{
	internal static Delegate32.BrotliEncoderCreateInstanceDelegate BrotliEncoderCreateInstance;

	internal static Delegate32.BrotliEncoderSetParameterDelegate BrotliEncoderSetParameter;

	internal static Delegate32.BrotliEncoderCompressStreamDelegate BrotliEncoderCompressStream;

	internal static Delegate32.BrotliEncoderIsFinishedDelegate BrotliEncoderIsFinished;

	internal static Delegate32.BrotliEncoderDestroyInstanceDelegate BrotliEncoderDestroyInstance;

	internal static Delegate32.BrotliEncoderVersionDelegate BrotliEncoderVersion;

	internal static Delegate32.BrotliEncoderTakeOutputDelegate BrotliEncoderTakeOutput;

	internal static Delegate32.BrotliDecoderCreateInstanceDelegate BrotliDecoderCreateInstance;

	internal static Delegate32.BrotliDecoderSetParameter BrotliDecoderSetParameter;

	internal static Delegate32.BrotliDecoderDecompressStreamDelegate BrotliDecoderDecompressStream;

	internal static Delegate32.BrotliDecoderDestroyInstanceDelegate BrotliDecoderDestroyInstance;

	internal static Delegate32.BrotliDecoderVersionDelegate BrotliDecoderVersion;

	internal static Delegate32.BrotliDecoderIsUsedDelegate BrotliDecoderIsUsed;

	internal static Delegate32.BrotliDecoderIsFinishedDelegate BrotliDecoderIsFinished;

	internal static Delegate32.BrotliDecoderGetErrorCodeDelegate BrotliDecoderGetErrorCode;

	internal static Delegate32.BrotliDecoderErrorStringDelegate BrotliDecoderErrorString;

	internal static Delegate32.BrotliDecoderTakeOutputDelegate BrotliDecoderTakeOutput;

	static Brolib32()
	{
		NativeLibraryLoader nativeLibraryLoader = new NativeLibraryLoader(LibPathBootStrapper.LibPath);
		nativeLibraryLoader.FillDelegate<Delegate32.BrotliEncoderCreateInstanceDelegate>(out BrotliEncoderCreateInstance);
		nativeLibraryLoader.FillDelegate<Delegate32.BrotliEncoderSetParameterDelegate>(out BrotliEncoderSetParameter);
		nativeLibraryLoader.FillDelegate<Delegate32.BrotliEncoderCompressStreamDelegate>(out BrotliEncoderCompressStream);
		nativeLibraryLoader.FillDelegate<Delegate32.BrotliEncoderIsFinishedDelegate>(out BrotliEncoderIsFinished);
		nativeLibraryLoader.FillDelegate<Delegate32.BrotliEncoderDestroyInstanceDelegate>(out BrotliEncoderDestroyInstance);
		nativeLibraryLoader.FillDelegate<Delegate32.BrotliEncoderVersionDelegate>(out BrotliEncoderVersion);
		nativeLibraryLoader.FillDelegate<Delegate32.BrotliEncoderVersionDelegate>(out BrotliEncoderVersion);
		nativeLibraryLoader.FillDelegate<Delegate32.BrotliEncoderTakeOutputDelegate>(out BrotliEncoderTakeOutput);
		nativeLibraryLoader.FillDelegate<Delegate32.BrotliDecoderCreateInstanceDelegate>(out BrotliDecoderCreateInstance);
		nativeLibraryLoader.FillDelegate<Delegate32.BrotliDecoderSetParameter>(out BrotliDecoderSetParameter);
		nativeLibraryLoader.FillDelegate<Delegate32.BrotliDecoderDecompressStreamDelegate>(out BrotliDecoderDecompressStream);
		nativeLibraryLoader.FillDelegate<Delegate32.BrotliDecoderDestroyInstanceDelegate>(out BrotliDecoderDestroyInstance);
		nativeLibraryLoader.FillDelegate<Delegate32.BrotliDecoderVersionDelegate>(out BrotliDecoderVersion);
		nativeLibraryLoader.FillDelegate<Delegate32.BrotliDecoderIsUsedDelegate>(out BrotliDecoderIsUsed);
		nativeLibraryLoader.FillDelegate<Delegate32.BrotliDecoderIsFinishedDelegate>(out BrotliDecoderIsFinished);
		nativeLibraryLoader.FillDelegate<Delegate32.BrotliDecoderGetErrorCodeDelegate>(out BrotliDecoderGetErrorCode);
		nativeLibraryLoader.FillDelegate<Delegate32.BrotliDecoderErrorStringDelegate>(out BrotliDecoderErrorString);
		nativeLibraryLoader.FillDelegate<Delegate32.BrotliDecoderTakeOutputDelegate>(out BrotliDecoderTakeOutput);
	}
}
