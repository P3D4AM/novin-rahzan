using System;
using System.Linq;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class ContextParameterBindingFactory : IParameterBindingFactory
{
	public virtual bool CanBind(Type parameterType, string parameterName)
	{
		return typeof(DbContext).IsAssignableFrom(parameterType);
	}

	public virtual ParameterBinding Bind(IMutableEntityType entityType, Type parameterType, string parameterName)
	{
		return new ContextParameterBinding(parameterType, entityType.GetServiceProperties().FirstOrDefault((IMutableServiceProperty p) => p.ClrType == parameterType));
	}

	public virtual ParameterBinding Bind(IConventionEntityType entityType, Type parameterType, string parameterName)
	{
		return new ContextParameterBinding(parameterType, entityType.GetServiceProperties().FirstOrDefault((IConventionServiceProperty p) => p.ClrType == parameterType));
	}
}
