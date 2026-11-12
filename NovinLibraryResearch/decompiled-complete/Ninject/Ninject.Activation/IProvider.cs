using System;

namespace Ninject.Activation;

public interface IProvider
{
	Type Type { get; }

	object Create(IContext context);
}
public interface IProvider<T> : IProvider
{
}
