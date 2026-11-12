using System;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.SignalR.Internal;

public class DefaultHubActivator<THub> : IHubActivator<THub> where THub : Hub
{
	private static readonly Lazy<ObjectFactory> _objectFactory = new Lazy<ObjectFactory>(() => ActivatorUtilities.CreateFactory(typeof(THub), Type.EmptyTypes));

	private readonly IServiceProvider _serviceProvider;

	private bool? _created;

	public DefaultHubActivator(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
	}

	public virtual THub Create()
	{
		_created = false;
		THub val = _serviceProvider.GetService<THub>();
		if (val == null)
		{
			val = (THub)_objectFactory.Value(_serviceProvider, Array.Empty<object>());
			_created = true;
		}
		return val;
	}

	public virtual void Release(THub hub)
	{
		if (hub == null)
		{
			throw new ArgumentNullException("hub");
		}
		if (_created.Value)
		{
			hub.Dispose();
		}
	}
}
