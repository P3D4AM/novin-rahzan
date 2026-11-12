using Serilog.Sinks.Http.Private;

namespace Serilog.Sinks.Http;

public static class ByteSize
{
	public const long B = 1L;

	public const long KB = 1024L;

	public const long MB = 1048576L;

	public const long GB = 1073741824L;

	public static long From(string text)
	{
		return Encoding.UTF8WithoutBom.GetByteCount(text);
	}
}
