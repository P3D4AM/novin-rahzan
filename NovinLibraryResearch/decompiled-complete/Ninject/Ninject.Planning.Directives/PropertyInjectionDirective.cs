using System.Reflection;
using Ninject.Injection;
using Ninject.Planning.Targets;

namespace Ninject.Planning.Directives;

public class PropertyInjectionDirective : IDirective
{
	public PropertyInjector Injector { get; private set; }

	public ITarget Target { get; private set; }

	public PropertyInjectionDirective(PropertyInfo member, PropertyInjector injector)
	{
		Injector = injector;
		Target = CreateTarget(member);
	}

	protected virtual ITarget CreateTarget(PropertyInfo propertyInfo)
	{
		return new PropertyTarget(propertyInfo);
	}
}
