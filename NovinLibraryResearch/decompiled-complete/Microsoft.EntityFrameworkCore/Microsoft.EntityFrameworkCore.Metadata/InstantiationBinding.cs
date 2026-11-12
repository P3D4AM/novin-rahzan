using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata;

public abstract class InstantiationBinding
{
	public virtual IReadOnlyList<ParameterBinding> ParameterBindings { get; }

	public abstract Type RuntimeType { get; }

	protected InstantiationBinding([JetBrains.Annotations.NotNull] IReadOnlyList<ParameterBinding> parameterBindings)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(parameterBindings, "parameterBindings");
		ParameterBindings = parameterBindings;
	}

	public abstract Expression CreateConstructorExpression(ParameterBindingInfo bindingInfo);
}
