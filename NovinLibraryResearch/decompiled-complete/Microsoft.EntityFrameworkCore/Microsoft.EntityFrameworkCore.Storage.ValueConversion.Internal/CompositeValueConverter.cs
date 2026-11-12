using System;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

public class CompositeValueConverter<TModel, TMiddle, TProvider> : ValueConverter<TModel, TProvider>
{
	public CompositeValueConverter([JetBrains.Annotations.NotNull] ValueConverter converter1, [JetBrains.Annotations.NotNull] ValueConverter converter2, [JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base(Compose((Expression<Func<TModel, TMiddle>>)converter1.ConvertToProviderExpression, (Expression<Func<TMiddle, TProvider>>)converter2.ConvertToProviderExpression), Compose((Expression<Func<TProvider, TMiddle>>)converter2.ConvertFromProviderExpression, (Expression<Func<TMiddle, TModel>>)converter1.ConvertFromProviderExpression), mappingHints)
	{
	}

	private static Expression<Func<TIn, TOut>> Compose<TIn, TOut>(Expression<Func<TIn, TMiddle>> upper, Expression<Func<TMiddle, TOut>> lower)
	{
		return Expression.Lambda<Func<TIn, TOut>>(ReplacingExpressionVisitor.Replace(lower.Parameters.Single(), upper.Body, lower.Body), new ParameterExpression[1] { upper.Parameters.Single() });
	}
}
