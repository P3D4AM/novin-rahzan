using System;
using Ninject.Infrastructure;

namespace Ninject.Activation;

public abstract class Provider<T> : IProvider<T>, IProvider
{
	public virtual Type Type => typeof(T);

	public object Create(IContext context)
	{
		Ensure.ArgumentNotNull(context, "context");
		return CreateInstance(context);
	}

	protected abstract T CreateInstance(IContext context);
}
