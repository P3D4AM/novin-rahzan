using System;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata;

public class ServiceParameterBindingFactory : IParameterBindingFactory
{
	private readonly Type _serviceType;

	public ServiceParameterBindingFactory([JetBrains.Annotations.NotNull] Type serviceType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(serviceType, "serviceType");
		_serviceType = serviceType;
	}

	public virtual bool CanBind(Type parameterType, string parameterName)
	{
		return parameterType == _serviceType;
	}

	public virtual ParameterBinding Bind(IMutableEntityType entityType, Type parameterType, string parameterName)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(parameterType, "parameterType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(parameterName, "parameterName");
		return new DependencyInjectionParameterBinding(_serviceType, _serviceType, entityType.GetServiceProperties().FirstOrDefault((IMutableServiceProperty p) => p.ClrType == _serviceType));
	}

	public virtual ParameterBinding Bind(IConventionEntityType entityType, Type parameterType, string parameterName)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(parameterType, "parameterType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(parameterName, "parameterName");
		return new DependencyInjectionParameterBinding(_serviceType, _serviceType, entityType.GetServiceProperties().FirstOrDefault((IConventionServiceProperty p) => p.ClrType == _serviceType));
	}
}
