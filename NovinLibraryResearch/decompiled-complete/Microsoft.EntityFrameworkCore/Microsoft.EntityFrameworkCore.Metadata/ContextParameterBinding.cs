using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata;

public class ContextParameterBinding : ServiceParameterBinding
{
	public ContextParameterBinding([JetBrains.Annotations.NotNull] Type contextType, [JetBrains.Annotations.CanBeNull] IPropertyBase serviceProperty = null)
		: base(contextType, contextType, serviceProperty)
	{
	}

	public override Expression BindToParameter(Expression materializationExpression, Expression entityTypeExpression)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(materializationExpression, "materializationExpression");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityTypeExpression, "entityTypeExpression");
		MemberExpression memberExpression = Expression.Property(materializationExpression, MaterializationContext.ContextProperty);
		if (!(ServiceType != typeof(DbContext)))
		{
			return memberExpression;
		}
		return Expression.TypeAs(memberExpression, ServiceType);
	}
}
