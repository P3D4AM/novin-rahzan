using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class ParameterBindingFactories : IParameterBindingFactories
{
	private readonly IRegisteredServices _registeredServices;

	private readonly List<IParameterBindingFactory> _parameterBindingFactories;

	public ParameterBindingFactories([JetBrains.Annotations.CanBeNull] IEnumerable<IParameterBindingFactory> registeredFactories, [JetBrains.Annotations.NotNull] IRegisteredServices registeredServices)
	{
		_registeredServices = registeredServices;
		_parameterBindingFactories = registeredFactories?.ToList() ?? new List<IParameterBindingFactory>();
	}

	public virtual IParameterBindingFactory FindFactory(Type parameterType, string parameterName)
	{
		object obj = _parameterBindingFactories.FirstOrDefault((IParameterBindingFactory f) => f.CanBind(parameterType, parameterName));
		if (obj == null)
		{
			if (!_registeredServices.Services.Contains(parameterType))
			{
				return null;
			}
			obj = new ServiceParameterBindingFactory(parameterType);
		}
		return (IParameterBindingFactory)obj;
	}
}
