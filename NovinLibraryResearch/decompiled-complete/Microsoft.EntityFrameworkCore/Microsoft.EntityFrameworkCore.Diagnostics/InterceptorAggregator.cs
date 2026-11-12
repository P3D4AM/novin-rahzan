using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public abstract class InterceptorAggregator<TInterceptor> : IInterceptorAggregator where TInterceptor : class, IInterceptor
{
	private TInterceptor _interceptor;

	private bool _resolved;

	public virtual Type InterceptorType => typeof(TInterceptor);

	public virtual IInterceptor AggregateInterceptors(IReadOnlyList<IInterceptor> interceptors)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(interceptors, "interceptors");
		if (!_resolved)
		{
			if (interceptors.Count == 1)
			{
				_interceptor = interceptors[0] as TInterceptor;
			}
			else if (interceptors.Count > 1)
			{
				List<TInterceptor> list = interceptors.OfType<TInterceptor>().ToList();
				if (list.Count == 1)
				{
					_interceptor = list[0];
				}
				else if (list.Count > 1)
				{
					_interceptor = CreateChain(list);
				}
			}
			_resolved = true;
		}
		return _interceptor;
	}

	protected abstract TInterceptor CreateChain([JetBrains.Annotations.NotNull] IEnumerable<TInterceptor> interceptors);
}
