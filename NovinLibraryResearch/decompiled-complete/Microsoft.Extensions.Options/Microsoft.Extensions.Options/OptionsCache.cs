using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.Options;

public class OptionsCache<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TOptions> : IOptionsMonitorCache<TOptions> where TOptions : class
{
	private readonly ConcurrentDictionary<string, Lazy<TOptions>> _cache = new ConcurrentDictionary<string, Lazy<TOptions>>(1, 31, StringComparer.Ordinal);

	public void Clear()
	{
		_cache.Clear();
	}

	public virtual TOptions GetOrAdd(string? name, Func<TOptions> createOptions)
	{
		System.ThrowHelper.ThrowIfNull(createOptions, "createOptions");
		if (name == null)
		{
			name = Options.DefaultName;
		}
		if (!_cache.TryGetValue(name, out var value))
		{
			value = _cache.GetOrAdd(name, new Lazy<TOptions>(createOptions));
		}
		return value.Value;
	}

	internal TOptions GetOrAdd<TArg>(string? name, Func<string, TArg, TOptions> createOptions, TArg factoryArgument)
	{
		string localName = name;
		Func<string, TArg, TOptions> localCreateOptions = createOptions;
		TArg localFactoryArgument = factoryArgument;
		return GetOrAdd(name, () => localCreateOptions(localName ?? Options.DefaultName, localFactoryArgument));
	}

	internal bool TryGetValue(string? name, [MaybeNullWhen(false)] out TOptions options)
	{
		if (_cache.TryGetValue(name ?? Options.DefaultName, out var value))
		{
			options = value.Value;
			return true;
		}
		options = null;
		return false;
	}

	public virtual bool TryAdd(string? name, TOptions options)
	{
		TOptions options2 = options;
		System.ThrowHelper.ThrowIfNull(options2, "options");
		return _cache.TryAdd(name ?? Options.DefaultName, new Lazy<TOptions>(() => options2));
	}

	public virtual bool TryRemove(string? name)
	{
		Lazy<TOptions> value;
		return _cache.TryRemove(name ?? Options.DefaultName, out value);
	}
}
