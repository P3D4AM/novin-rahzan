using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Options;

public class OptionsMonitor<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TOptions> : IOptionsMonitor<TOptions>, IDisposable where TOptions : class
{
	internal sealed class ChangeTrackerDisposable : IDisposable
	{
		private readonly Action<TOptions, string> _listener;

		private readonly OptionsMonitor<TOptions> _monitor;

		public ChangeTrackerDisposable(OptionsMonitor<TOptions> monitor, Action<TOptions, string> listener)
		{
			_listener = listener;
			_monitor = monitor;
		}

		public void OnChange(TOptions options, string name)
		{
			_listener(options, name);
		}

		public void Dispose()
		{
			_monitor._onChange -= OnChange;
		}
	}

	private readonly IOptionsMonitorCache<TOptions> _cache;

	private readonly IOptionsFactory<TOptions> _factory;

	private readonly List<IDisposable> _registrations = new List<IDisposable>();

	public TOptions CurrentValue => Get(Options.DefaultName);

	internal event Action<TOptions, string>? _onChange;

	public OptionsMonitor(IOptionsFactory<TOptions> factory, IEnumerable<IOptionsChangeTokenSource<TOptions>> sources, IOptionsMonitorCache<TOptions> cache)
	{
		_factory = factory;
		_cache = cache;
		if (sources is IOptionsChangeTokenSource<TOptions>[] array)
		{
			IOptionsChangeTokenSource<TOptions>[] array2 = array;
			foreach (IOptionsChangeTokenSource<TOptions> source2 in array2)
			{
				RegisterSource(source2);
				void RegisterSource(IOptionsChangeTokenSource<TOptions> source)
				{
					IDisposable item = ChangeToken.OnChange(source.GetChangeToken, InvokeChanged, source.Name);
					_registrations.Add(item);
				}
			}
			return;
		}
		foreach (IOptionsChangeTokenSource<TOptions> source3 in sources)
		{
			RegisterSource(source3);
		}
	}

	private void InvokeChanged(string name)
	{
		if (name == null)
		{
			name = Options.DefaultName;
		}
		_cache.TryRemove(name);
		TOptions arg = Get(name);
		this._onChange?.Invoke(arg, name);
	}

	public virtual TOptions Get(string? name)
	{
		if (!(_cache is OptionsCache<TOptions> optionsCache))
		{
			string localName = name ?? Options.DefaultName;
			IOptionsFactory<TOptions> localFactory = _factory;
			return _cache.GetOrAdd(localName, () => localFactory.Create(localName));
		}
		return optionsCache.GetOrAdd(name, (string name, IOptionsFactory<TOptions> factory) => factory.Create(name), _factory);
	}

	public IDisposable OnChange(Action<TOptions, string> listener)
	{
		ChangeTrackerDisposable changeTrackerDisposable = new ChangeTrackerDisposable(this, listener);
		_onChange += changeTrackerDisposable.OnChange;
		return changeTrackerDisposable;
	}

	public void Dispose()
	{
		foreach (IDisposable registration in _registrations)
		{
			registration.Dispose();
		}
		_registrations.Clear();
	}
}
