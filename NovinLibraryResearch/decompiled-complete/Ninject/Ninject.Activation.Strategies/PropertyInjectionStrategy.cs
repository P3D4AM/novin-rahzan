using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ninject.Infrastructure;
using Ninject.Infrastructure.Introspection;
using Ninject.Injection;
using Ninject.Parameters;
using Ninject.Planning.Directives;
using Ninject.Planning.Targets;

namespace Ninject.Activation.Strategies;

public class PropertyInjectionStrategy : ActivationStrategy
{
	private const BindingFlags DefaultFlags = BindingFlags.Instance | BindingFlags.Public;

	public IInjectorFactory InjectorFactory { get; set; }

	private BindingFlags Flags
	{
		get
		{
			if (!base.Settings.InjectNonPublic)
			{
				return BindingFlags.Instance | BindingFlags.Public;
			}
			return BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
		}
	}

	public PropertyInjectionStrategy(IInjectorFactory injectorFactory)
	{
		InjectorFactory = injectorFactory;
	}

	public override void Activate(IContext context, InstanceReference reference)
	{
		Ensure.ArgumentNotNull(context, "context");
		Ensure.ArgumentNotNull(reference, "reference");
		List<IPropertyValue> list = context.Parameters.OfType<IPropertyValue>().ToList();
		foreach (PropertyInjectionDirective item in context.Plan.GetAll<PropertyInjectionDirective>())
		{
			object value = GetValue(context, item.Target, list);
			item.Injector(reference.Instance, value);
		}
		AssignPropertyOverrides(context, reference, list);
	}

	private void AssignPropertyOverrides(IContext context, InstanceReference reference, IList<IPropertyValue> propertyValues)
	{
		PropertyInfo[] properties = reference.Instance.GetType().GetProperties(Flags);
		foreach (IPropertyValue propertyValue in propertyValues)
		{
			string propertyName = propertyValue.Name;
			PropertyInfo propertyInfo = properties.FirstOrDefault((PropertyInfo property) => string.Equals(property.Name, propertyName, StringComparison.Ordinal));
			if (propertyInfo == null)
			{
				throw new ActivationException(ExceptionFormatter.CouldNotResolvePropertyForValueInjection(context.Request, propertyName));
			}
			PropertyInjectionDirective propertyInjectionDirective = new PropertyInjectionDirective(propertyInfo, InjectorFactory.Create(propertyInfo));
			object value = GetValue(context, propertyInjectionDirective.Target, propertyValues);
			propertyInjectionDirective.Injector(reference.Instance, value);
		}
	}

	private object GetValue(IContext context, ITarget target, IEnumerable<IPropertyValue> allPropertyValues)
	{
		Ensure.ArgumentNotNull(context, "context");
		Ensure.ArgumentNotNull(target, "target");
		IPropertyValue propertyValue = allPropertyValues.SingleOrDefault((IPropertyValue p) => p.Name == target.Name);
		if (propertyValue == null)
		{
			return target.ResolveWithin(context);
		}
		return propertyValue.GetValue(context, target);
	}
}
