using System;
using Ninject.Infrastructure;

namespace Ninject.Activation.Providers;

public class CallbackProvider<T> : Provider<T>
{
	public Func<IContext, T> Method { get; private set; }

	public CallbackProvider(Func<IContext, T> method)
	{
		Ensure.ArgumentNotNull(method, "method");
		Method = method;
	}

	protected override T CreateInstance(IContext context)
	{
		return Method(context);
	}
}
