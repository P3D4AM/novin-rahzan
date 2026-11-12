using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public static class ValueComparerExtensions
{
	private class NonNullNullableValueComparer<T> : ValueComparer<T>
	{
		public NonNullNullableValueComparer(LambdaExpression equalsExpression, LambdaExpression hashCodeExpression, LambdaExpression snapshotExpression)
			: base((Expression<Func<T, T, bool>>)equalsExpression, (Expression<Func<T, int>>)hashCodeExpression, (Expression<Func<T, T>>)snapshotExpression)
		{
		}
	}

	public static ValueComparer ToNonNullNullableComparer([JetBrains.Annotations.NotNull] this ValueComparer comparer)
	{
		Type type = comparer.EqualsExpression.Parameters[0].Type;
		Type type2 = type.MakeNullable();
		ParameterExpression parameterExpression = Expression.Parameter(type2, "v1");
		ParameterExpression parameterExpression2 = Expression.Parameter(type2, "v2");
		ParameterExpression parameterExpression3 = Expression.Parameter(type2, "v");
		ParameterExpression parameterExpression4 = Expression.Parameter(type2, "v");
		return (ValueComparer)Activator.CreateInstance(typeof(NonNullNullableValueComparer<>).MakeGenericType(type2), Expression.Lambda(comparer.ExtractEqualsBody(Expression.Convert(parameterExpression, type), Expression.Convert(parameterExpression2, type)), parameterExpression, parameterExpression2), Expression.Lambda(comparer.ExtractHashCodeBody(Expression.Convert(parameterExpression3, type)), parameterExpression3), Expression.Lambda(Expression.Convert(comparer.ExtractSnapshotBody(Expression.Convert(parameterExpression4, type)), type2), parameterExpression4));
	}
}
