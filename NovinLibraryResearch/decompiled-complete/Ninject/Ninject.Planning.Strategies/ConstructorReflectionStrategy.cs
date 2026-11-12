using System;
using System.Collections.Generic;
using System.Reflection;
using Ninject.Components;
using Ninject.Infrastructure;
using Ninject.Infrastructure.Language;
using Ninject.Injection;
using Ninject.Planning.Directives;
using Ninject.Selection;

namespace Ninject.Planning.Strategies;

public class ConstructorReflectionStrategy : NinjectComponent, IPlanningStrategy, INinjectComponent, IDisposable
{
	public ISelector Selector { get; private set; }

	public IInjectorFactory InjectorFactory { get; set; }

	public ConstructorReflectionStrategy(ISelector selector, IInjectorFactory injectorFactory)
	{
		Ensure.ArgumentNotNull(selector, "selector");
		Ensure.ArgumentNotNull(injectorFactory, "injectorFactory");
		Selector = selector;
		InjectorFactory = injectorFactory;
	}

	public void Execute(IPlan plan)
	{
		Ensure.ArgumentNotNull(plan, "plan");
		IEnumerable<ConstructorInfo> enumerable = Selector.SelectConstructorsForInjection(plan.Type);
		if (enumerable == null)
		{
			return;
		}
		foreach (ConstructorInfo item in enumerable)
		{
			bool hasInjectAttribute = item.HasAttribute(base.Settings.InjectAttribute);
			bool hasObsoleteAttribute = item.HasAttribute(typeof(ObsoleteAttribute));
			ConstructorInjectionDirective directive = new ConstructorInjectionDirective(item, InjectorFactory.Create(item))
			{
				HasInjectAttribute = hasInjectAttribute,
				HasObsoleteAttribute = hasObsoleteAttribute
			};
			plan.Add(directive);
		}
	}
}
