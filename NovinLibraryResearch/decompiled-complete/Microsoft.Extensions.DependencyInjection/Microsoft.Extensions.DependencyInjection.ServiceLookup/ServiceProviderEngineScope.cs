using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.Extensions.Internal;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup;

internal sealed class ServiceProviderEngineScope : IServiceScope, IDisposable, IServiceProvider, IAsyncDisposable, IServiceScopeFactory
{
	private bool _disposed;

	private List<object> _disposables;

	internal IList<object> Disposables
	{
		get
		{
			IList<object> disposables = _disposables;
			return disposables ?? Array.Empty<object>();
		}
	}

	internal Dictionary<ServiceCacheKey, object?> ResolvedServices { get; }

	internal object Sync => ResolvedServices;

	public bool IsRootScope { get; }

	internal ServiceProvider RootProvider { get; }

	public IServiceProvider ServiceProvider => this;

	public ServiceProviderEngineScope(ServiceProvider provider, bool isRootScope)
	{
		ResolvedServices = new Dictionary<ServiceCacheKey, object>();
		RootProvider = provider;
		IsRootScope = isRootScope;
	}

	public object? GetService(Type serviceType)
	{
		if (_disposed)
		{
			ThrowHelper.ThrowObjectDisposedException();
		}
		return RootProvider.GetService(serviceType, this);
	}

	public IServiceScope CreateScope()
	{
		return RootProvider.CreateScope();
	}

	[return: NotNullIfNotNull("service")]
	internal object? CaptureDisposable(object? service)
	{
		object service2 = service;
		if (this == service2 || (!(service2 is IDisposable) && !(service2 is IAsyncDisposable)))
		{
			return service2;
		}
		bool flag = false;
		lock (Sync)
		{
			if (_disposed)
			{
				flag = true;
			}
			else
			{
				if (_disposables == null)
				{
					_disposables = new List<object>();
				}
				_disposables.Add(service2);
			}
		}
		if (flag)
		{
			if (service2 is IDisposable disposable)
			{
				disposable.Dispose();
			}
			else
			{
				Task.Run(() => ((IAsyncDisposable)service2).DisposeAsync().AsTask()).GetAwaiter().GetResult();
			}
			ThrowHelper.ThrowObjectDisposedException();
		}
		return service2;
	}

	public void Dispose()
	{
		List<object> list = BeginDispose();
		if (list == null)
		{
			return;
		}
		int num = list.Count - 1;
		while (num >= 0)
		{
			if (list[num] is IDisposable disposable)
			{
				disposable.Dispose();
				num--;
				continue;
			}
			throw new InvalidOperationException(System.SR.Format(System.SR.AsyncDisposableServiceDispose, TypeNameHelper.GetTypeDisplayName(list[num])));
		}
	}

	public ValueTask DisposeAsync()
	{
		List<object> list = BeginDispose();
		if (list != null)
		{
			try
			{
				for (int num = list.Count - 1; num >= 0; num--)
				{
					object obj = list[num];
					if (obj is IAsyncDisposable asyncDisposable)
					{
						ValueTask vt2 = asyncDisposable.DisposeAsync();
						if (!vt2.IsCompletedSuccessfully)
						{
							return Await(num, vt2, list);
						}
						vt2.GetAwaiter().GetResult();
					}
					else
					{
						((IDisposable)obj).Dispose();
					}
				}
			}
			catch (Exception exception)
			{
				return new ValueTask(Task.FromException(exception));
			}
		}
		return default(ValueTask);
		static async ValueTask Await(int i, ValueTask vt, List<object> toDispose)
		{
			await vt.ConfigureAwait(continueOnCapturedContext: false);
			i--;
			while (i >= 0)
			{
				object obj2 = toDispose[i];
				if (obj2 is IAsyncDisposable asyncDisposable2)
				{
					await asyncDisposable2.DisposeAsync().ConfigureAwait(continueOnCapturedContext: false);
				}
				else
				{
					((IDisposable)obj2).Dispose();
				}
				i--;
			}
		}
	}

	private List<object> BeginDispose()
	{
		lock (Sync)
		{
			if (_disposed)
			{
				return null;
			}
			DependencyInjectionEventSource.Log.ScopeDisposed(RootProvider.GetHashCode(), ResolvedServices.Count, _disposables?.Count ?? 0);
			_disposed = true;
		}
		if (IsRootScope && !RootProvider.IsDisposed())
		{
			RootProvider.Dispose();
		}
		return _disposables;
	}
}
