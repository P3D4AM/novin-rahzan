using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.Options;

public interface IOptionsMonitor<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] out TOptions>
{
	TOptions CurrentValue { get; }

	TOptions Get(string? name);

	IDisposable? OnChange(Action<TOptions, string?> listener);
}
