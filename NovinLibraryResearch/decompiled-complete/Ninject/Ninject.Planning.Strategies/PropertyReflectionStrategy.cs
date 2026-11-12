using System;
using System.Reflection;
using Ninject.Components;
using Ninject.Infrastructure;
using Ninject.Injection;
using Ninject.Planning.Directives;
using Ninject.Selection;

namespace Ninject.Planning.Strategies;

public class PropertyReflectionStrategy : NinjectComponent, IPlanningStrategy, INinjectComponent, IDisposable
{
	public ISelector Selector { get; private set; }

	public IInjectorFactory InjectorFactory { get; set; }

	public PropertyReflectionStrategy(ISelector selector, IInjectorFactory injectorFactory)
	{
		Ensure.ArgumentNotNull(selector, "selector");
		Ensure.ArgumentNotNull(injectorFactory, "injectorFactory");
		Selector = selector;
		InjectorFactory = injectorFactory;
	}

	public void Execute(IPlan plan)
	{
		Ensure.ArgumentNotNull(plan, "plan");
		foreach (PropertyInfo item in Selector.SelectPropertiesForInjection(plan.Type))
		{
			plan.Add(new PropertyInjectionDirective(item, InjectorFactory.Create(item)));
		}
	}
}
