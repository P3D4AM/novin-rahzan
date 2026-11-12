using System;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata;

public class DependencyInjectionParameterBinding : ServiceParameterBinding
{
	private static readonly MethodInfo _getServiceMethod = typeof(InfrastructureExtensions).GetMethod("GetService");

	public DependencyInjectionParameterBinding([JetBrains.Annotations.NotNull] Type parameterType, [JetBrains.Annotations.NotNull] Type serviceType, [JetBrains.Annotations.CanBeNull] IPropertyBase serviceProperty = null)
		: base(parameterType, serviceType, serviceProperty)
	{
	}

	public override Expression BindToParameter(Expression materializationExpression, Expression entityTypeExpression)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(materializationExpression, "materializationExpression");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityTypeExpression, "entityTypeExpression");
		return Expression.Call(_getServiceMethod.MakeGenericMethod(ServiceType), Expression.Convert(Expression.Property(materializationExpression, MaterializationContext.ContextProperty), typeof(IInfrastructure<IServiceProvider>)));
	}
}
