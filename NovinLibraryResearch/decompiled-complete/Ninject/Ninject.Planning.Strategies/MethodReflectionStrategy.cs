using System;
using System.Reflection;
using Ninject.Components;
using Ninject.Infrastructure;
using Ninject.Injection;
using Ninject.Planning.Directives;
using Ninject.Selection;

namespace Ninject.Planning.Strategies;

public class MethodReflectionStrategy : NinjectComponent, IPlanningStrategy, INinjectComponent, IDisposable
{
	public ISelector Selector { get; private set; }

	public IInjectorFactory InjectorFactory { get; set; }

	public MethodReflectionStrategy(ISelector selector, IInjectorFactory injectorFactory)
	{
		Ensure.ArgumentNotNull(selector, "selector");
		Ensure.ArgumentNotNull(injectorFactory, "injectorFactory");
		Selector = selector;
		InjectorFactory = injectorFactory;
	}

	public void Execute(IPlan plan)
	{
		Ensure.ArgumentNotNull(plan, "plan");
		foreach (MethodInfo item in Selector.SelectMethodsForInjection(plan.Type))
		{
			plan.Add(new MethodInjectionDirective(item, InjectorFactory.Create(item)));
		}
	}
}
