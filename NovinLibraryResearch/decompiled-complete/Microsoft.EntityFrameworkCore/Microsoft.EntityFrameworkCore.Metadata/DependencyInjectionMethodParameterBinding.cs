using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata;

public class DependencyInjectionMethodParameterBinding : DependencyInjectionParameterBinding
{
	public virtual MethodInfo Method { get; }

	public DependencyInjectionMethodParameterBinding([JetBrains.Annotations.NotNull] Type parameterType, [JetBrains.Annotations.NotNull] Type serviceType, [JetBrains.Annotations.NotNull] MethodInfo method, [JetBrains.Annotations.CanBeNull] IPropertyBase serviceProperty = null)
		: base(parameterType, serviceType, serviceProperty)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(method, "method");
		Method = method;
	}

	public override Expression BindToParameter(Expression materializationExpression, Expression entityTypeExpression)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(materializationExpression, "materializationExpression");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityTypeExpression, "entityTypeExpression");
		ParameterExpression[] array = Method.GetParameters().Select((ParameterInfo p, int i) => Expression.Parameter(p.ParameterType, "param" + i)).ToArray();
		ParameterExpression parameterExpression = Expression.Variable(ServiceType, "service");
		ParameterExpression parameterExpression2 = Expression.Variable(ParameterType, "delegate");
		ParameterExpression[] variables = new ParameterExpression[2] { parameterExpression, parameterExpression2 };
		List<Expression> obj = new List<Expression> { Expression.Assign(parameterExpression, base.BindToParameter(materializationExpression, entityTypeExpression)) };
		BinaryExpression test = Expression.ReferenceEqual(parameterExpression, Expression.Constant(null));
		ConstantExpression ifTrue = Expression.Constant(null, ParameterType);
		MethodInfo method = Method;
		Expression[] arguments = array;
		obj.Add(Expression.Assign(parameterExpression2, Expression.Condition(test, ifTrue, Expression.Lambda(Expression.Call(parameterExpression, method, arguments), array))));
		obj.Add(parameterExpression2);
		return Expression.Block(variables, obj);
	}
}
