using Ninject.Modules;

namespace Ninject;

public abstract class GlobalKernelRegistrationModule<TGlobalKernelRegistry> : NinjectModule where TGlobalKernelRegistry : GlobalKernelRegistration
{
	public override void Load()
	{
		GlobalKernelRegistration.RegisterKernelForType(base.Kernel, typeof(TGlobalKernelRegistry));
	}

	public override void Unload()
	{
		GlobalKernelRegistration.UnregisterKernelForType(base.Kernel, typeof(TGlobalKernelRegistry));
	}
}
