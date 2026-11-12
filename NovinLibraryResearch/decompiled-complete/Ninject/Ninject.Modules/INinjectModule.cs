using Ninject.Infrastructure;

namespace Ninject.Modules;

public interface INinjectModule : IHaveKernel
{
	string Name { get; }

	void OnLoad(IKernel kernel);

	void OnUnload(IKernel kernel);

	void OnVerifyRequiredModules();
}
