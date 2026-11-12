using System;
using System.Linq;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class EntityTypeParameterBindingFactory : IParameterBindingFactory
{
	public virtual bool CanBind(Type parameterType, string parameterName)
	{
		return parameterType == typeof(IEntityType);
	}

	public virtual ParameterBinding Bind(IMutableEntityType entityType, Type parameterType, string parameterName)
	{
		return new EntityTypeParameterBinding(entityType.GetServiceProperties().FirstOrDefault((IMutableServiceProperty p) => p.ClrType == parameterType));
	}

	public virtual ParameterBinding Bind(IConventionEntityType entityType, Type parameterType, string parameterName)
	{
		return new EntityTypeParameterBinding(entityType.GetServiceProperties().FirstOrDefault((IConventionServiceProperty p) => p.ClrType == parameterType));
	}
}
