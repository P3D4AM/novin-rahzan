using System;
using Ninject.Infrastructure.Disposal;
using Ninject.Syntax;

namespace Ninject.Activation.Blocks;

public interface IActivationBlock : IResolutionRoot, IFluentSyntax, INotifyWhenDisposed, IDisposableObject, IDisposable
{
}
