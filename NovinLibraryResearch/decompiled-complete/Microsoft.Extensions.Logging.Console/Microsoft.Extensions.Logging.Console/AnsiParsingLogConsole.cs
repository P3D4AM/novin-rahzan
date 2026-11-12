using System;
using System.IO;
using System.Runtime.Versioning;

namespace Microsoft.Extensions.Logging.Console;

[UnsupportedOSPlatform("android")]
[UnsupportedOSPlatform("browser")]
[UnsupportedOSPlatform("ios")]
[UnsupportedOSPlatform("tvos")]
internal sealed class AnsiParsingLogConsole : IConsole
{
	private readonly TextWriter _textWriter;

	private readonly AnsiParser _parser;

	public AnsiParsingLogConsole(bool stdErr = false)
	{
		_textWriter = (stdErr ? System.Console.Error : System.Console.Out);
		_parser = new AnsiParser(WriteToConsole);
	}

	public void Write(string message)
	{
		_parser.Parse(message);
	}

	private bool SetColor(ConsoleColor? background, ConsoleColor? foreground)
	{
		bool flag = SetBackgroundColor(background);
		return SetForegroundColor(foreground) || flag;
	}

	private bool SetBackgroundColor(ConsoleColor? background)
	{
		if (background.HasValue)
		{
			System.Console.BackgroundColor = background.Value;
			return true;
		}
		return false;
	}

	private bool SetForegroundColor(ConsoleColor? foreground)
	{
		if (foreground.HasValue)
		{
			System.Console.ForegroundColor = foreground.Value;
			return true;
		}
		return false;
	}

	private void ResetColor()
	{
		System.Console.ResetColor();
	}

	private void WriteToConsole(string message, int startIndex, int length, ConsoleColor? background, ConsoleColor? foreground)
	{
		ReadOnlySpan<char> readOnlySpan = MemoryExtensions.AsSpan(message, startIndex, length);
		bool flag = SetColor(background, foreground);
		_textWriter.Write(readOnlySpan.ToString());
		if (flag)
		{
			ResetColor();
		}
	}
}
