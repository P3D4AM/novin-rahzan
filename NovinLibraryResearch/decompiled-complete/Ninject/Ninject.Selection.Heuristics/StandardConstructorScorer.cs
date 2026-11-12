using System;
using System.Collections;
using System.Linq;
using Ninject.Activation;
using Ninject.Components;
using Ninject.Infrastructure;
using Ninject.Parameters;
using Ninject.Planning.Bindings;
using Ninject.Planning.Directives;
using Ninject.Planning.Targets;

namespace Ninject.Selection.Heuristics;

public class StandardConstructorScorer : NinjectComponent, IConstructorScorer, INinjectComponent, IDisposable
{
	public virtual int Score(IContext context, ConstructorInjectionDirective directive)
	{
		Ensure.ArgumentNotNull(context, "context");
		Ensure.ArgumentNotNull(directive, "directive");
		if (directive.HasInjectAttribute)
		{
			return int.MaxValue;
		}
		if (directive.HasObsoleteAttribute)
		{
			return int.MinValue;
		}
		int num = 1;
		ITarget[] targets = directive.Targets;
		foreach (ITarget target in targets)
		{
			if (ParameterExists(context, target))
			{
				num++;
				continue;
			}
			if (BindingExists(context, target))
			{
				num++;
				continue;
			}
			num++;
			if (num > 0)
			{
				num += int.MinValue;
			}
		}
		return num;
	}

	protected virtual bool BindingExists(IContext context, ITarget target)
	{
		return BindingExists(context.Kernel, context, target);
	}

	protected virtual bool BindingExists(IKernel kernel, IContext context, ITarget target)
	{
		Type targetType = GetTargetType(target);
		IRequest request = context.Request.CreateChild(targetType, context, target);
		if (!kernel.GetBindings(targetType).Any((IBinding b) => !b.IsImplicit && b.Matches(request)))
		{
			return target.HasDefaultValue;
		}
		return true;
	}

	protected virtual bool ParameterExists(IContext context, ITarget target)
	{
		return context.Parameters.OfType<IConstructorArgument>().Any((IConstructorArgument parameter) => parameter.AppliesToTarget(context, target));
	}

	private Type GetTargetType(ITarget target)
	{
		Type type2 = target.Type;
		if (type2.IsArray)
		{
			type2 = type2.GetElementType();
		}
		if (type2.IsGenericType && type2.GetInterfaces().Any((Type type) => type == typeof(IEnumerable)))
		{
			type2 = type2.GetGenericArguments()[0];
		}
		return type2;
	}
}
