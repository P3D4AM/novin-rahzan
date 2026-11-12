using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.Logging.Console;

internal sealed class FormatterOptionsMonitor<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TOptions> : IOptionsMonitor<TOptions> where TOptions : ConsoleFormatterOptions
{
	private TOptions _options;

	public TOptions CurrentValue => _options;

	public FormatterOptionsMonitor(TOptions options)
	{
		_options = options;
	}

	public TOptions Get(string name)
	{
		return _options;
	}

	public IDisposable OnChange(Action<TOptions, string> listener)
	{
		return null;
	}
}
