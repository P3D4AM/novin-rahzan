#define DEBUG
using System;
using System.Diagnostics;
using System.IO;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;

namespace Serilog.Sinks.Debug;

internal class DebugSink : ILogEventSink
{
	private readonly ITextFormatter _formatter;

	public DebugSink(ITextFormatter formatter)
	{
		_formatter = formatter ?? throw new ArgumentNullException("formatter");
	}

	public void Emit(LogEvent logEvent)
	{
		using StringWriter stringWriter = new StringWriter();
		_formatter.Format(logEvent, stringWriter);
		System.Diagnostics.Debug.Write(stringWriter.ToString());
	}
}
