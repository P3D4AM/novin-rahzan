using System;

namespace Microsoft.Extensions.Options;

public class ConfigureOptions<TOptions> : IConfigureOptions<TOptions> where TOptions : class
{
	public Action<TOptions>? Action { get; }

	public ConfigureOptions(Action<TOptions>? action)
	{
		Action = action;
	}

	public virtual void Configure(TOptions options)
	{
		System.ThrowHelper.ThrowIfNull(options, "options");
		Action?.Invoke(options);
	}
}
