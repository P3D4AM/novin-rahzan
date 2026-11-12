using System;
using System.IO;

namespace Microsoft.Extensions.Logging.Console;

internal static class TextWriterExtensions
{
	public static void WriteColoredMessage(this TextWriter textWriter, string message, ConsoleColor? background, ConsoleColor? foreground)
	{
		if (background.HasValue)
		{
			textWriter.Write(AnsiParser.GetBackgroundColorEscapeCode(background.Value));
		}
		if (foreground.HasValue)
		{
			textWriter.Write(AnsiParser.GetForegroundColorEscapeCode(foreground.Value));
		}
		textWriter.Write(message);
		if (foreground.HasValue)
		{
			textWriter.Write("\u001b[39m\u001b[22m");
		}
		if (background.HasValue)
		{
			textWriter.Write("\u001b[49m");
		}
	}
}
