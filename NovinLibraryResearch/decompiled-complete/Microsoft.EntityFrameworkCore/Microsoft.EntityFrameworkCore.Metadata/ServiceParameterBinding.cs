using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata;

public abstract class ServiceParameterBinding : ParameterBinding
{
	private Func<MaterializationContext, IEntityType, object, object> _serviceDelegate;

	public virtual Type ServiceType { get; }

	public virtual Func<MaterializationContext, IEntityType, object, object> ServiceDelegate => NonCapturingLazyInitializer.EnsureInitialized(ref _serviceDelegate, this, delegate(ServiceParameterBinding b)
	{
		ParameterExpression parameterExpression = Expression.Parameter(typeof(MaterializationContext));
		ParameterExpression parameterExpression2 = Expression.Parameter(typeof(IEntityType));
		ParameterExpression parameterExpression3 = Expression.Parameter(typeof(object));
		return Expression.Lambda<Func<MaterializationContext, IEntityType, object, object>>(b.BindToParameter(parameterExpression, parameterExpression2), new ParameterExpression[3] { parameterExpression, parameterExpression2, parameterExpression3 }).Compile();
	});

	protected ServiceParameterBinding([JetBrains.Annotations.NotNull] Type parameterType, [JetBrains.Annotations.NotNull] Type serviceType, [JetBrains.Annotations.CanBeNull] IPropertyBase serviceProperty = null)
		: base(parameterType, (serviceProperty == null) ? Array.Empty<IPropertyBase>() : new IPropertyBase[1] { serviceProperty })
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(serviceType, "serviceType");
		ServiceType = serviceType;
	}

	public override Expression BindToParameter(ParameterBindingInfo bindingInfo)
	{
		return BindToParameter(bindingInfo.MaterializationContextExpression, Expression.Constant(bindingInfo.EntityType));
	}

	public abstract Expression BindToParameter([JetBrains.Annotations.NotNull] Expression materializationExpression, [JetBrains.Annotations.NotNull] Expression entityTypeExpression);
}
