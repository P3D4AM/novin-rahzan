using System;
using System.IO;
using Microsoft.Extensions.Logging.Abstractions;

namespace Microsoft.Extensions.Logging.Console;

public abstract class ConsoleFormatter
{
	public string Name { get; }

	protected ConsoleFormatter(string name)
	{
		Name = name ?? throw new ArgumentNullException("name");
	}

	public abstract void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider scopeProvider, TextWriter textWriter);
}
