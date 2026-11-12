using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata;

public abstract class ParameterBinding
{
	public virtual Type ParameterType { get; }

	public virtual IReadOnlyList<IPropertyBase> ConsumedProperties { get; }

	protected ParameterBinding([JetBrains.Annotations.NotNull] Type parameterType, [JetBrains.Annotations.NotNull] params IPropertyBase[] consumedProperties)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(parameterType, "parameterType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(consumedProperties, "parameterType");
		ParameterType = parameterType;
		ConsumedProperties = consumedProperties;
	}

	public abstract Expression BindToParameter(ParameterBindingInfo bindingInfo);
}
