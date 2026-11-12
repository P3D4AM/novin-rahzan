using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class ConstructorBindingFactory : IConstructorBindingFactory
{
	private readonly IPropertyParameterBindingFactory _propertyFactory;

	private readonly IParameterBindingFactories _factories;

	public ConstructorBindingFactory([JetBrains.Annotations.NotNull] IPropertyParameterBindingFactory propertyFactory, [JetBrains.Annotations.NotNull] IParameterBindingFactories factories)
	{
		_propertyFactory = propertyFactory;
		_factories = factories;
	}

	public virtual bool TryBindConstructor(IMutableEntityType entityType, ConstructorInfo constructor, out InstantiationBinding binding, out IEnumerable<ParameterInfo> unboundParameters)
	{
		return TryBindConstructor(entityType, constructor, (IParameterBindingFactory f, IEntityType e, Type p, string n) => f?.Bind((IMutableEntityType)e, p, n), out binding, out unboundParameters);
	}

	public virtual bool TryBindConstructor(IConventionEntityType entityType, ConstructorInfo constructor, out InstantiationBinding binding, out IEnumerable<ParameterInfo> unboundParameters)
	{
		return TryBindConstructor(entityType, constructor, (IParameterBindingFactory f, IEntityType e, Type p, string n) => f?.Bind((IConventionEntityType)e, p, n), out binding, out unboundParameters);
	}

	private bool TryBindConstructor(IEntityType entityType, ConstructorInfo constructor, Func<IParameterBindingFactory, IEntityType, Type, string, ParameterBinding> bind, out InstantiationBinding binding, out IEnumerable<ParameterInfo> unboundParameters)
	{
		IEnumerable<(ParameterInfo, ParameterBinding)> source = (from p in constructor.GetParameters()
			select (p: p, _propertyFactory.FindParameter(entityType, p.ParameterType, p.Name) ?? bind(_factories.FindFactory(p.ParameterType, p.Name), entityType, p.ParameterType, p.Name))).ToList();
		if (source.Any<(ParameterInfo, ParameterBinding)>(((ParameterInfo Parameter, ParameterBinding Binding) b) => b.Binding == null))
		{
			unboundParameters = from b in source
				where b.Binding == null
				select b.Parameter;
			binding = null;
			return false;
		}
		unboundParameters = null;
		binding = new ConstructorBinding(constructor, source.Select<(ParameterInfo, ParameterBinding), ParameterBinding>(((ParameterInfo Parameter, ParameterBinding Binding) b) => b.Binding).ToList());
		return true;
	}
}
