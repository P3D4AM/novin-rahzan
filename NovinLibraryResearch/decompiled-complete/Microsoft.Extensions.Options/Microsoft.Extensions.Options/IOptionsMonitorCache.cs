using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.Options;

public interface IOptionsMonitorCache<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TOptions> where TOptions : class
{
	TOptions GetOrAdd(string? name, Func<TOptions> createOptions);

	bool TryAdd(string? name, TOptions options);

	bool TryRemove(string? name);

	void Clear();
}
