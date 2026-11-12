using System;
using System.Collections.Generic;
using Ninject.Infrastructure;
using Ninject.Infrastructure.Language;
using Ninject.Planning.Bindings;
using Ninject.Syntax;

namespace Ninject.Modules;

public abstract class NinjectModule : BindingRoot, INinjectModule, IHaveKernel
{
	public IKernel Kernel { get; private set; }

	public virtual string Name => GetType().FullName;

	public ICollection<IBinding> Bindings { get; private set; }

	protected override IKernel KernelInstance => Kernel;

	protected NinjectModule()
	{
		Bindings = new List<IBinding>();
	}

	public void OnLoad(IKernel kernel)
	{
		Ensure.ArgumentNotNull(kernel, "kernel");
		Kernel = kernel;
		Load();
	}

	public void OnUnload(IKernel kernel)
	{
		Ensure.ArgumentNotNull(kernel, "kernel");
		Unload();
		Bindings.Map(Kernel.RemoveBinding);
		Kernel = null;
	}

	public void OnVerifyRequiredModules()
	{
		VerifyRequiredModulesAreLoaded();
	}

	public abstract void Load();

	public virtual void Unload()
	{
	}

	public virtual void VerifyRequiredModulesAreLoaded()
	{
	}

	public override void Unbind(Type service)
	{
		Kernel.Unbind(service);
	}

	public override void AddBinding(IBinding binding)
	{
		Ensure.ArgumentNotNull(binding, "binding");
		Kernel.AddBinding(binding);
		Bindings.Add(binding);
	}

	public override void RemoveBinding(IBinding binding)
	{
		Ensure.ArgumentNotNull(binding, "binding");
		Kernel.RemoveBinding(binding);
		Bindings.Remove(binding);
	}
}
