using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore.Diagnostics.Internal;

public class Interceptors : IInterceptors
{
	private readonly IServiceProvider _serviceProvider;

	private readonly IEnumerable<IInterceptor> _injectedInterceptors;

	private readonly Dictionary<Type, IInterceptorAggregator> _aggregators;

	private CoreOptionsExtension _coreOptionsExtension;

	private List<IInterceptor> _interceptors;

	private IReadOnlyList<IInterceptor> RegisteredInterceptors
	{
		get
		{
			if (_interceptors == null)
			{
				List<IInterceptor> list = _injectedInterceptors.ToList();
				IEnumerable<IInterceptor> enumerable = CoreOptionsExtension?.Interceptors;
				if (enumerable != null)
				{
					list.AddRange(enumerable);
				}
				_interceptors = list;
			}
			return _interceptors;
		}
	}

	private CoreOptionsExtension CoreOptionsExtension => _coreOptionsExtension ?? (_coreOptionsExtension = ServiceProviderServiceExtensions.GetService<IDbContextOptions>(_serviceProvider).Extensions.OfType<CoreOptionsExtension>().FirstOrDefault());

	public Interceptors([JetBrains.Annotations.NotNull] IServiceProvider serviceProvider, [JetBrains.Annotations.NotNull] IEnumerable<IInterceptor> injectedInterceptors, [JetBrains.Annotations.NotNull] IEnumerable<IInterceptorAggregator> interceptorAggregators)
	{
		_serviceProvider = serviceProvider;
		_injectedInterceptors = injectedInterceptors;
		_aggregators = interceptorAggregators.ToDictionary((IInterceptorAggregator i) => i.InterceptorType);
	}

	public virtual TInterceptor Aggregate<TInterceptor>() where TInterceptor : class, IInterceptor
	{
		return (TInterceptor)_aggregators[typeof(TInterceptor)].AggregateInterceptors(RegisteredInterceptors);
	}
}
