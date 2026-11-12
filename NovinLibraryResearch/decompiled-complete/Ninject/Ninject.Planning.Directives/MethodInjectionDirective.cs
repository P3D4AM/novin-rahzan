using System.Reflection;
using Ninject.Injection;

namespace Ninject.Planning.Directives;

public class MethodInjectionDirective : MethodInjectionDirectiveBase<MethodInfo, MethodInjector>
{
	public MethodInjectionDirective(MethodInfo method, MethodInjector injector)
		: base(method, injector)
	{
	}
}
