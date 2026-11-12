using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata;

public class ObjectArrayParameterBinding : ParameterBinding
{
	private readonly IReadOnlyList<ParameterBinding> _bindings;

	public ObjectArrayParameterBinding([JetBrains.Annotations.NotNull] IReadOnlyList<ParameterBinding> bindings)
		: base(typeof(object[]), Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(bindings, "bindings").SelectMany((ParameterBinding b) => b.ConsumedProperties).ToArray())
	{
		_bindings = bindings;
	}

	public override Expression BindToParameter(ParameterBindingInfo bindingInfo)
	{
		return Expression.NewArrayInit(typeof(object), _bindings.Select(delegate(ParameterBinding b)
		{
			Expression expression = b.BindToParameter(bindingInfo);
			if (expression.Type.GetTypeInfo().IsValueType)
			{
				expression = Expression.Convert(expression, typeof(object));
			}
			return expression;
		}));
	}
}
