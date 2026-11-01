using System.Diagnostics.CodeAnalysis;

namespace System.Text.Encodings.Web;

internal static class ThrowHelper
{
	[DoesNotReturn]
	internal static void ThrowArgumentNullException(ExceptionArgument argument)
	{
		throw new ArgumentNullException(GetArgumentName(argument));
	}

	[DoesNotReturn]
	internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument)
	{
		throw new ArgumentOutOfRangeException(GetArgumentName(argument));
	}

	private static string GetArgumentName(ExceptionArgument argument)
	{
		return argument.ToString();
	}
}
