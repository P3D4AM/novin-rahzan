using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata;

public class FactoryMethodBinding : InstantiationBinding
{
	private readonly object _factoryInstance;

	private readonly MethodInfo _factoryMethod;

	public override Type RuntimeType { get; }

	public FactoryMethodBinding([JetBrains.Annotations.NotNull] MethodInfo factoryMethod, [JetBrains.Annotations.NotNull] IReadOnlyList<ParameterBinding> parameterBindings, [JetBrains.Annotations.NotNull] Type runtimeType)
		: base(parameterBindings)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(factoryMethod, "factoryMethod");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(runtimeType, "runtimeType");
		_factoryMethod = factoryMethod;
		RuntimeType = runtimeType;
	}

	public FactoryMethodBinding([JetBrains.Annotations.NotNull] object factoryInstance, [JetBrains.Annotations.NotNull] MethodInfo factoryMethod, [JetBrains.Annotations.NotNull] IReadOnlyList<ParameterBinding> parameterBindings, [JetBrains.Annotations.NotNull] Type runtimeType)
		: this(factoryMethod, parameterBindings, runtimeType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(factoryInstance, "factoryInstance");
		_factoryInstance = factoryInstance;
	}

	public override Expression CreateConstructorExpression(ParameterBindingInfo bindingInfo)
	{
		IEnumerable<Expression> arguments = ParameterBindings.Select((ParameterBinding b) => b.BindToParameter(bindingInfo));
		Expression expression = ((_factoryInstance == null) ? Expression.Call(_factoryMethod, arguments) : Expression.Call(Expression.Constant(_factoryInstance), _factoryMethod, arguments));
		if (_factoryMethod.ReturnType != RuntimeType)
		{
			expression = Expression.Convert(expression, RuntimeType);
		}
		return expression;
	}
}
