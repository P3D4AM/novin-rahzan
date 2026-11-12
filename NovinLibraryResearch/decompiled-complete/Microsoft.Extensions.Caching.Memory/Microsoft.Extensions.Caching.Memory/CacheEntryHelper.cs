using System;
using System.Threading;

namespace Microsoft.Extensions.Caching.Memory;

internal class CacheEntryHelper
{
	private sealed class ScopeLease : IDisposable
	{
		private readonly CacheEntryStack _cacheEntryStack;

		public ScopeLease(CacheEntryStack cacheEntryStack)
		{
			_cacheEntryStack = cacheEntryStack;
		}

		public void Dispose()
		{
			Scopes = _cacheEntryStack;
		}
	}

	private static readonly AsyncLocal<CacheEntryStack> _scopes = new AsyncLocal<CacheEntryStack>();

	internal static CacheEntryStack Scopes
	{
		get
		{
			return _scopes.Value;
		}
		set
		{
			_scopes.Value = value;
		}
	}

	internal static CacheEntry Current => GetOrCreateScopes().Peek();

	internal static IDisposable EnterScope(CacheEntry entry)
	{
		CacheEntryStack orCreateScopes = GetOrCreateScopes();
		ScopeLease result = new ScopeLease(orCreateScopes);
		Scopes = orCreateScopes.Push(entry);
		return result;
	}

	private static CacheEntryStack GetOrCreateScopes()
	{
		CacheEntryStack cacheEntryStack = Scopes;
		if (cacheEntryStack == null)
		{
			cacheEntryStack = (Scopes = CacheEntryStack.Empty);
		}
		return cacheEntryStack;
	}
}
