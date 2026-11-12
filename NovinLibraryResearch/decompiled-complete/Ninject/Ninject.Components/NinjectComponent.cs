using System;
using Ninject.Infrastructure.Disposal;

namespace Ninject.Components;

public abstract class NinjectComponent : DisposableObject, INinjectComponent, IDisposable
{
	public INinjectSettings Settings { get; set; }
}
