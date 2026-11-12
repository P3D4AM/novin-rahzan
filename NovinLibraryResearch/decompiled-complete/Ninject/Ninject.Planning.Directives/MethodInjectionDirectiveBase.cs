using System.Linq;
using System.Reflection;
using Ninject.Infrastructure;
using Ninject.Planning.Targets;

namespace Ninject.Planning.Directives;

public abstract class MethodInjectionDirectiveBase<TMethod, TInjector> : IDirective where TMethod : MethodBase
{
	public TInjector Injector { get; private set; }

	public ITarget[] Targets { get; private set; }

	protected MethodInjectionDirectiveBase(TMethod method, TInjector injector)
	{
		Ensure.ArgumentNotNull(method, "method");
		Ensure.ArgumentNotNull(injector, "injector");
		Injector = injector;
		Targets = CreateTargetsFromParameters(method);
	}

	protected virtual ITarget[] CreateTargetsFromParameters(TMethod method)
	{
		return (from parameter in method.GetParameters()
			select new ParameterTarget(method, parameter)).ToArray();
	}
}
