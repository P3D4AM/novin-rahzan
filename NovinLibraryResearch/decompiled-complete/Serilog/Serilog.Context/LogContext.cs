using System;
using System.Threading;
using Serilog.Core;
using Serilog.Core.Enrichers;
using Serilog.Events;

namespace Serilog.Context;

public static class LogContext
{
	private sealed class ContextStackBookmark : IDisposable
	{
		private readonly EnricherStack _bookmark;

		public ContextStackBookmark(EnricherStack bookmark)
		{
			_bookmark = bookmark;
		}

		public void Dispose()
		{
			Enrichers = _bookmark;
		}
	}

	private static readonly AsyncLocal<EnricherStack?> Data = new AsyncLocal<EnricherStack>();

	private static EnricherStack? Enrichers
	{
		get
		{
			return Data.Value;
		}
		set
		{
			Data.Value = value;
		}
	}

	public static IDisposable PushProperty(string name, object? value, bool destructureObjects = false)
	{
		return Push(new PropertyEnricher(name, value, destructureObjects));
	}

	public static IDisposable Push(ILogEventEnricher enricher)
	{
		Guard.AgainstNull(enricher, "enricher");
		EnricherStack orCreateEnricherStack = GetOrCreateEnricherStack();
		ContextStackBookmark result = new ContextStackBookmark(orCreateEnricherStack);
		Enrichers = orCreateEnricherStack.Push(enricher);
		return result;
	}

	public static IDisposable Push(params ILogEventEnricher[] enrichers)
	{
		Guard.AgainstNull(enrichers, "enrichers");
		EnricherStack enricherStack = GetOrCreateEnricherStack();
		ContextStackBookmark result = new ContextStackBookmark(enricherStack);
		for (int i = 0; i < enrichers.Length; i++)
		{
			enricherStack = enricherStack.Push(enrichers[i]);
		}
		Enrichers = enricherStack;
		return result;
	}

	public static ILogEventEnricher Clone()
	{
		return new SafeAggregateEnricher(GetOrCreateEnricherStack());
	}

	public static IDisposable Suspend()
	{
		ContextStackBookmark result = new ContextStackBookmark(GetOrCreateEnricherStack());
		Enrichers = EnricherStack.Empty;
		return result;
	}

	public static void Reset()
	{
		if (Enrichers != null && Enrichers != EnricherStack.Empty)
		{
			Enrichers = EnricherStack.Empty;
		}
	}

	private static EnricherStack GetOrCreateEnricherStack()
	{
		EnricherStack enricherStack = Enrichers;
		if (enricherStack == null)
		{
			enricherStack = (Enrichers = EnricherStack.Empty);
		}
		return enricherStack;
	}

	internal static void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
	{
		EnricherStack enrichers = Enrichers;
		if (enrichers == null || enrichers == EnricherStack.Empty)
		{
			return;
		}
		foreach (ILogEventEnricher item in enrichers)
		{
			item.Enrich(logEvent, propertyFactory);
		}
	}
}
