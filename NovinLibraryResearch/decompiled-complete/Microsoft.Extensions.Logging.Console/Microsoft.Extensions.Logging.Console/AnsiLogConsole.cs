using System;
using System.IO;

namespace Microsoft.Extensions.Logging.Console;

internal sealed class AnsiLogConsole : IConsole
{
	private readonly TextWriter _textWriter;

	public AnsiLogConsole(bool stdErr = false)
	{
		_textWriter = (stdErr ? System.Console.Error : System.Console.Out);
	}

	public void Write(string message)
	{
		_textWriter.Write(message);
	}
}
