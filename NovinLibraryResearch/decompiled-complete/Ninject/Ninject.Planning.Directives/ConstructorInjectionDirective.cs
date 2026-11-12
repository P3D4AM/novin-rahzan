using System.Reflection;
using Ninject.Injection;

namespace Ninject.Planning.Directives;

public class ConstructorInjectionDirective : MethodInjectionDirectiveBase<ConstructorInfo, ConstructorInjector>
{
	public ConstructorInfo Constructor { get; set; }

	public bool HasInjectAttribute { get; set; }

	public bool HasObsoleteAttribute { get; set; }

	public ConstructorInjectionDirective(ConstructorInfo constructor, ConstructorInjector injector)
		: base(constructor, injector)
	{
		Constructor = constructor;
	}
}
