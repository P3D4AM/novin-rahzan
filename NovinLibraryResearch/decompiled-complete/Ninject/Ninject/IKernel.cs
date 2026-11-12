using System;
using System.Collections.Generic;
using System.Reflection;
using Ninject.Activation.Blocks;
using Ninject.Components;
using Ninject.Infrastructure.Disposal;
using Ninject.Modules;
using Ninject.Planning.Bindings;
using Ninject.Syntax;

namespace Ninject;

public interface IKernel : IBindingRoot, IFluentSyntax, IResolutionRoot, IServiceProvider, IDisposableObject, IDisposable, INotifyWhenDisposed
{
	INinjectSettings Settings { get; }

	IComponentContainer Components { get; }

	IEnumerable<INinjectModule> GetModules();

	bool HasModule(string name);

	void Load(IEnumerable<INinjectModule> m);

	void Load(IEnumerable<string> filePatterns);

	void Load(IEnumerable<Assembly> assemblies);

	void Unload(string name);

	IEnumerable<IBinding> GetBindings(Type service);

	IActivationBlock BeginBlock();
}
