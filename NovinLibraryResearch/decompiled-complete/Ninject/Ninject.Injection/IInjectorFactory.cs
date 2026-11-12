using System;
using System.Reflection;
using Ninject.Components;

namespace Ninject.Injection;

public interface IInjectorFactory : INinjectComponent, IDisposable
{
	ConstructorInjector Create(ConstructorInfo constructor);

	PropertyInjector Create(PropertyInfo property);

	MethodInjector Create(MethodInfo method);
}
