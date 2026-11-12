using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection.ServiceLookup;

namespace Microsoft.Extensions.DependencyInjection;

public sealed class ServiceProvider : IServiceProvider, IDisposable, IAsyncDisposable
{
	internal const string RequiresDynamicCodeMessage = "Using Microsoft.Extensions.DependencyInjection requires generating code dynamically at runtime. For example, when using enumerable and generic ValueType services.";

	private readonly CallSiteValidator _callSiteValidator;

	private readonly Func<Type, Func<ServiceProviderEngineScope, object>> _createServiceAccessor;

	internal ServiceProviderEngine _engine;

	private bool _disposed;

	private ConcurrentDictionary<Type, Func<ServiceProviderEngineScope, object>> _realizedServices;

	internal CallSiteFactory CallSiteFactory { get; }

	internal ServiceProviderEngineScope Root { get; }

	internal static bool VerifyOpenGenericServiceTrimmability { get; } = AppContext.TryGetSwitch("Microsoft.Extensions.DependencyInjection.VerifyOpenGenericServiceTrimmability", out var isEnabled) && isEnabled;


	[System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Using Microsoft.Extensions.DependencyInjection requires generating code dynamically at runtime. For example, when using enumerable and generic ValueType services.")]
	internal ServiceProvider(ICollection<ServiceDescriptor> serviceDescriptors, ServiceProviderOptions options)
	{
		Root = new ServiceProviderEngineScope(this, isRootScope: true);
		_engine = GetEngine();
		_createServiceAccessor = CreateServiceAccessor;
		_realizedServices = new ConcurrentDictionary<Type, Func<ServiceProviderEngineScope, object>>();
		CallSiteFactory = new CallSiteFactory(serviceDescriptors);
		CallSiteFactory.Add(typeof(IServiceProvider), new ServiceProviderCallSite());
		CallSiteFactory.Add(typeof(IServiceScopeFactory), new ConstantCallSite(typeof(IServiceScopeFactory), Root));
		CallSiteFactory.Add(typeof(IServiceProviderIsService), new ConstantCallSite(typeof(IServiceProviderIsService), CallSiteFactory));
		if (options.ValidateScopes)
		{
			_callSiteValidator = new CallSiteValidator();
		}
		if (options.ValidateOnBuild)
		{
			List<Exception> list = null;
			foreach (ServiceDescriptor serviceDescriptor in serviceDescriptors)
			{
				try
				{
					ValidateService(serviceDescriptor);
				}
				catch (Exception item)
				{
					if (list == null)
					{
						list = new List<Exception>();
					}
					list.Add(item);
				}
			}
			if (list != null)
			{
				throw new AggregateException("Some services are not able to be constructed", list.ToArray());
			}
		}
		DependencyInjectionEventSource.Log.ServiceProviderBuilt(this);
	}

	public object? GetService(Type serviceType)
	{
		return GetService(serviceType, Root);
	}

	internal bool IsDisposed()
	{
		return _disposed;
	}

	public void Dispose()
	{
		DisposeCore();
		Root.Dispose();
	}

	public ValueTask DisposeAsync()
	{
		DisposeCore();
		return Root.DisposeAsync();
	}

	private void DisposeCore()
	{
		_disposed = true;
		DependencyInjectionEventSource.Log.ServiceProviderDisposed(this);
	}

	private void OnCreate(ServiceCallSite callSite)
	{
		_callSiteValidator?.ValidateCallSite(callSite);
	}

	private void OnResolve(Type serviceType, IServiceScope scope)
	{
		_callSiteValidator?.ValidateResolution(serviceType, scope, Root);
	}

	internal object? GetService(Type serviceType, ServiceProviderEngineScope serviceProviderEngineScope)
	{
		if (_disposed)
		{
			ThrowHelper.ThrowObjectDisposedException();
		}
		Func<ServiceProviderEngineScope, object> orAdd = _realizedServices.GetOrAdd(serviceType, _createServiceAccessor);
		OnResolve(serviceType, serviceProviderEngineScope);
		DependencyInjectionEventSource.Log.ServiceResolved(this, serviceType);
		return orAdd(serviceProviderEngineScope);
	}

	private void ValidateService(ServiceDescriptor descriptor)
	{
		if (descriptor.ServiceType.IsGenericType && !descriptor.ServiceType.IsConstructedGenericType)
		{
			return;
		}
		try
		{
			ServiceCallSite callSite = CallSiteFactory.GetCallSite(descriptor, new CallSiteChain());
			if (callSite != null)
			{
				OnCreate(callSite);
			}
		}
		catch (Exception ex)
		{
			throw new InvalidOperationException($"Error while validating the service descriptor '{descriptor}': {ex.Message}", ex);
		}
	}

	[System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Using Microsoft.Extensions.DependencyInjection requires generating code dynamically at runtime. For example, when using enumerable and generic ValueType services.")]
	private Func<ServiceProviderEngineScope, object> CreateServiceAccessor(Type serviceType)
	{
		ServiceCallSite callSite = CallSiteFactory.GetCallSite(serviceType, new CallSiteChain());
		if (callSite != null)
		{
			DependencyInjectionEventSource.Log.CallSiteBuilt(this, serviceType, callSite);
			OnCreate(callSite);
			if (callSite.Cache.Location == CallSiteResultCacheLocation.Root)
			{
				object value = CallSiteRuntimeResolver.Instance.Resolve(callSite, Root);
				return (ServiceProviderEngineScope scope) => value;
			}
			return _engine.RealizeService(callSite);
		}
		return (ServiceProviderEngineScope _) => (object)null;
	}

	internal void ReplaceServiceAccessor(ServiceCallSite callSite, Func<ServiceProviderEngineScope, object?> accessor)
	{
		_realizedServices[callSite.ServiceType] = accessor;
	}

	internal IServiceScope CreateScope()
	{
		if (_disposed)
		{
			ThrowHelper.ThrowObjectDisposedException();
		}
		return new ServiceProviderEngineScope(this, isRootScope: false);
	}

	[System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Using Microsoft.Extensions.DependencyInjection requires generating code dynamically at runtime. For example, when using enumerable and generic ValueType services.")]
	private ServiceProviderEngine GetEngine()
	{
		return new DynamicServiceProviderEngine(this);
	}
}
