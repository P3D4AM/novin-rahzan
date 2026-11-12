using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata;

public class ConstructorBinding : InstantiationBinding
{
	public virtual ConstructorInfo Constructor { get; }

	public override Type RuntimeType => Constructor.DeclaringType;

	public ConstructorBinding([JetBrains.Annotations.NotNull] ConstructorInfo constructor, [JetBrains.Annotations.NotNull] IReadOnlyList<ParameterBinding> parameterBindings)
		: base(parameterBindings)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(constructor, "constructor");
		Constructor = constructor;
	}

	public override Expression CreateConstructorExpression(ParameterBindingInfo bindingInfo)
	{
		return Expression.New(Constructor, ParameterBindings.Select((ParameterBinding b) => b.BindToParameter(bindingInfo)));
	}
}
