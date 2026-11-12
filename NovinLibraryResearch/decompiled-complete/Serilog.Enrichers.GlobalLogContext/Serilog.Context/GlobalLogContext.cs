using System;
using System.Threading;
using System.Threading.Tasks;
using Serilog.Core;
using Serilog.Core.Enrichers;
using Serilog.Events;

namespace Serilog.Context;

public static class GlobalLogContext
{
	private sealed class ContextLock : IDisposable
	{
		public void Dispose()
		{
			_semaphoreSlim.Release();
		}
	}

	private sealed class ContextStackBookmark : IDisposable
	{
		private readonly ImmutableStack<ILogEventEnricher> _bookmark;

		public ContextStackBookmark(ImmutableStack<ILogEventEnricher> bookmark)
		{
			_bookmark = bookmark;
		}

		public void Dispose()
		{
			Enrichers = _bookmark;
		}
	}

	private static readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

	private static ImmutableStack<ILogEventEnricher> _data;

	private static ImmutableStack<ILogEventEnricher> Enrichers
	{
		get
		{
			return _data;
		}
		set
		{
			_data = value;
		}
	}

	public static IDisposable Lock()
	{
		_semaphoreSlim.Wait();
		return new ContextLock();
	}

	public static async Task<IDisposable> LockAsync()
	{
		await _semaphoreSlim.WaitAsync().ConfigureAwait(continueOnCapturedContext: false);
		return new ContextLock();
	}

	public static IDisposable PushProperty(string name, object value, bool destructureObjects = false)
	{
		return Push(new PropertyEnricher(name, value, destructureObjects));
	}

	public static IDisposable Push(ILogEventEnricher enricher)
	{
		if (enricher == null)
		{
			throw new ArgumentNullException("enricher");
		}
		ImmutableStack<ILogEventEnricher> orCreateEnricherStack = GetOrCreateEnricherStack();
		ContextStackBookmark result = new ContextStackBookmark(orCreateEnricherStack);
		Enrichers = orCreateEnricherStack.Push(enricher);
		return result;
	}

	public static IDisposable Push(params ILogEventEnricher[] enrichers)
	{
		if (enrichers == null)
		{
			throw new ArgumentNullException("enrichers");
		}
		ImmutableStack<ILogEventEnricher> immutableStack = GetOrCreateEnricherStack();
		ContextStackBookmark result = new ContextStackBookmark(immutableStack);
		for (int i = 0; i < enrichers.Length; i++)
		{
			immutableStack = immutableStack.Push(enrichers[i]);
		}
		Enrichers = immutableStack;
		return result;
	}

	public static IDisposable Suspend()
	{
		ContextStackBookmark result = new ContextStackBookmark(GetOrCreateEnricherStack());
		Enrichers = ImmutableStack<ILogEventEnricher>.Empty;
		return result;
	}

	public static void Reset()
	{
		if (Enrichers != null && Enrichers != ImmutableStack<ILogEventEnricher>.Empty)
		{
			Enrichers = ImmutableStack<ILogEventEnricher>.Empty;
		}
	}

	internal static void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
	{
		ImmutableStack<ILogEventEnricher> enrichers = Enrichers;
		if (enrichers == null || enrichers == ImmutableStack<ILogEventEnricher>.Empty)
		{
			return;
		}
		foreach (ILogEventEnricher item in enrichers)
		{
			item.Enrich(logEvent, propertyFactory);
		}
	}

	private static ImmutableStack<ILogEventEnricher> GetOrCreateEnricherStack()
	{
		ImmutableStack<ILogEventEnricher> immutableStack = Enrichers;
		if (immutableStack == null)
		{
			immutableStack = (Enrichers = ImmutableStack<ILogEventEnricher>.Empty);
		}
		return immutableStack;
	}
}
