using System;
using System.IO;

namespace Serilog.Debugging;

public static class SelfLog
{
	private static Action<string>? _output;

	public static void Enable(TextWriter output)
	{
		TextWriter output2 = output;
		Guard.AgainstNull(output2, "output");
		Enable(delegate(string m)
		{
			output2.WriteLine(m);
			output2.Flush();
		});
	}

	public static void Enable(Action<string> output)
	{
		_output = Guard.AgainstNull(output, "output");
	}

	public static void Disable()
	{
		_output = null;
	}

	public static void WriteLine(string format, object? arg0 = null, object? arg1 = null, object? arg2 = null)
	{
		_output?.Invoke(string.Format(DateTime.UtcNow.ToString("o") + " " + format, arg0, arg1, arg2));
	}
}
