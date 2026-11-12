using System;
using System.Collections.Generic;
using System.Linq;
using Ninject.Components;
using Ninject.Infrastructure;
using Ninject.Infrastructure.Disposal;
using Ninject.Planning.Bindings;

namespace Ninject.Activation.Caching;

public class Cache : NinjectComponent, ICache, INinjectComponent, IDisposable, IPruneable
{
	private class CacheEntry
	{
		public IContext Context { get; private set; }

		public InstanceReference Reference { get; private set; }

		public CacheEntry(IContext context, InstanceReference reference)
		{
			Context = context;
			Reference = reference;
		}
	}

	private readonly IDictionary<object, Multimap<IBindingConfiguration, CacheEntry>> entries = new Dictionary<object, Multimap<IBindingConfiguration, CacheEntry>>(new WeakReferenceEqualityComparer());

	public IPipeline Pipeline { get; private set; }

	public int Count => GetAllCacheEntries().Count();

	public Cache(IPipeline pipeline, ICachePruner cachePruner)
	{
		Ensure.ArgumentNotNull(pipeline, "pipeline");
		Ensure.ArgumentNotNull(cachePruner, "cachePruner");
		Pipeline = pipeline;
		cachePruner.Start(this);
	}

	public override void Dispose(bool disposing)
	{
		if (disposing && !base.IsDisposed)
		{
			Clear();
		}
		base.Dispose(disposing);
	}

	public void Remember(IContext context, InstanceReference reference)
	{
		Ensure.ArgumentNotNull(context, "context");
		object scope = context.GetScope();
		CacheEntry value = new CacheEntry(context, reference);
		lock (entries)
		{
			ReferenceEqualWeakReference weakScopeReference = new ReferenceEqualWeakReference(scope);
			if (!entries.ContainsKey(weakScopeReference))
			{
				entries[weakScopeReference] = new Multimap<IBindingConfiguration, CacheEntry>();
				if (scope is INotifyWhenDisposed notifyWhenDisposed)
				{
					notifyWhenDisposed.Disposed += delegate
					{
						Clear(weakScopeReference);
					};
				}
			}
			entries[weakScopeReference].Add(context.Binding.BindingConfiguration, value);
		}
	}

	public object TryGet(IContext context)
	{
		Ensure.ArgumentNotNull(context, "context");
		object scope = context.GetScope();
		if (scope == null)
		{
			return null;
		}
		lock (entries)
		{
			if (!entries.TryGetValue(scope, out var value))
			{
				return null;
			}
			foreach (CacheEntry item in value[context.Binding.BindingConfiguration])
			{
				if (context.HasInferredGenericArguments)
				{
					Type[] genericArguments = item.Context.GenericArguments;
					Type[] genericArguments2 = context.GenericArguments;
					if (!genericArguments.SequenceEqual(genericArguments2))
					{
						continue;
					}
				}
				return item.Reference.Instance;
			}
			return null;
		}
	}

	public bool Release(object instance)
	{
		lock (entries)
		{
			bool result = false;
			foreach (ICollection<CacheEntry> item in entries.Values.SelectMany((Multimap<IBindingConfiguration, CacheEntry> bindingEntries) => bindingEntries.Values).ToList())
			{
				foreach (CacheEntry item2 in item.Where((CacheEntry cacheEntry) => instance == cacheEntry.Reference.Instance).ToList())
				{
					Forget(item2);
					item.Remove(item2);
					result = true;
				}
			}
			return result;
		}
	}

	public void Prune()
	{
		lock (entries)
		{
			foreach (KeyValuePair<object, Multimap<IBindingConfiguration, CacheEntry>> item in (from scope in entries
				where !((ReferenceEqualWeakReference)scope.Key).IsAlive
				select (scope)).ToList())
			{
				entries.Remove(item.Key);
				Forget(GetAllBindingEntries(item.Value));
			}
		}
	}

	public void Clear(object scope)
	{
		lock (entries)
		{
			if (entries.TryGetValue(scope, out var value))
			{
				entries.Remove(scope);
				Forget(GetAllBindingEntries(value));
			}
		}
	}

	public void Clear()
	{
		lock (entries)
		{
			Forget(GetAllCacheEntries());
			entries.Clear();
		}
	}

	private static IEnumerable<CacheEntry> GetAllBindingEntries(Multimap<IBindingConfiguration, CacheEntry> bindings)
	{
		return bindings.Values.SelectMany((ICollection<CacheEntry> bindingEntries) => bindingEntries);
	}

	private IEnumerable<CacheEntry> GetAllCacheEntries()
	{
		return entries.SelectMany((KeyValuePair<object, Multimap<IBindingConfiguration, CacheEntry>> scopeCache) => GetAllBindingEntries(scopeCache.Value));
	}

	private void Forget(IEnumerable<CacheEntry> cacheEntries)
	{
		foreach (CacheEntry item in cacheEntries.ToList())
		{
			Forget(item);
		}
	}

	private void Forget(CacheEntry entry)
	{
		Clear(entry.Reference.Instance);
		Pipeline.Deactivate(entry.Context, entry.Reference);
	}
}
