using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ninject.Infrastructure;
using Ninject.Infrastructure.Introspection;
using Ninject.Infrastructure.Language;
using Ninject.Parameters;
using Ninject.Planning;
using Ninject.Planning.Directives;
using Ninject.Planning.Targets;
using Ninject.Selection;
using Ninject.Selection.Heuristics;

namespace Ninject.Activation.Providers;

public class StandardProvider : IProvider
{
	public Type Type { get; private set; }

	public IPlanner Planner { get; private set; }

	public IConstructorScorer ConstructorScorer { get; private set; }

	public StandardProvider(Type type, IPlanner planner, IConstructorScorer constructorScorer)
	{
		Ensure.ArgumentNotNull(type, "type");
		Ensure.ArgumentNotNull(planner, "planner");
		Ensure.ArgumentNotNull(constructorScorer, "constructorScorer");
		Type = type;
		Planner = planner;
		ConstructorScorer = constructorScorer;
	}

	public static Func<IContext, IProvider> GetCreationCallback(Type prototype)
	{
		Ensure.ArgumentNotNull(prototype, "prototype");
		return (IContext ctx) => new StandardProvider(prototype, ctx.Kernel.Components.Get<IPlanner>(), ctx.Kernel.Components.Get<ISelector>().ConstructorScorer);
	}

	public static Func<IContext, IProvider> GetCreationCallback(Type prototype, ConstructorInfo constructor)
	{
		Ensure.ArgumentNotNull(prototype, "prototype");
		return (IContext ctx) => new StandardProvider(prototype, ctx.Kernel.Components.Get<IPlanner>(), new SpecificConstructorSelector(constructor));
	}

	public virtual object Create(IContext context)
	{
		Ensure.ArgumentNotNull(context, "context");
		if (context.Plan == null)
		{
			context.Plan = Planner.GetPlan(GetImplementationType(context.Request.Service));
		}
		ConstructorInjectionDirective constructorInjectionDirective = DetermineConstructorInjectionDirective(context);
		object[] arguments = constructorInjectionDirective.Targets.Select((ITarget target) => GetValue(context, target)).ToArray();
		object obj = context.Cache.TryGet(context);
		if (obj != null)
		{
			return obj;
		}
		return constructorInjectionDirective.Injector(arguments);
	}

	public object GetValue(IContext context, ITarget target)
	{
		Ensure.ArgumentNotNull(context, "context");
		Ensure.ArgumentNotNull(target, "target");
		IConstructorArgument constructorArgument = context.Parameters.OfType<IConstructorArgument>().SingleOrDefault((IConstructorArgument p) => p.AppliesToTarget(context, target));
		if (constructorArgument == null)
		{
			return target.ResolveWithin(context);
		}
		return constructorArgument.GetValue(context, target);
	}

	public Type GetImplementationType(Type service)
	{
		Ensure.ArgumentNotNull(service, "service");
		if (!Type.ContainsGenericParameters)
		{
			return Type;
		}
		return Type.MakeGenericType(service.GetGenericArguments());
	}

	private ConstructorInjectionDirective DetermineConstructorInjectionDirective(IContext context)
	{
		IList<ConstructorInjectionDirective> constructorInjectionDirectives = context.Plan.ConstructorInjectionDirectives;
		if (constructorInjectionDirectives.Count == 1)
		{
			return constructorInjectionDirectives[0];
		}
		IGrouping<int, ConstructorInjectionDirective> bestDirectives = (from option in constructorInjectionDirectives
			group option by ConstructorScorer.Score(context, option) into g
			orderby g.Key descending
			select g).FirstOrDefault();
		if (bestDirectives == null)
		{
			throw new ActivationException(ExceptionFormatter.NoConstructorsAvailable(context));
		}
		return bestDirectives.SingleOrThrowException(() => new ActivationException(ExceptionFormatter.ConstructorsAmbiguous(context, bestDirectives)));
	}
}
