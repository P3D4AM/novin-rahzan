using System;
using System.Collections.Generic;
using Ninject.Components;

namespace Ninject.Planning.Bindings;

public interface IBindingPrecedenceComparer : INinjectComponent, IDisposable, IComparer<IBinding>
{
}
