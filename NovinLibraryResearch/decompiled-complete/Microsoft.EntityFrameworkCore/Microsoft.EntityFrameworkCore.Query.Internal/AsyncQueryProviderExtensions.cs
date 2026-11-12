using System;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public static class AsyncQueryProviderExtensions
{
	private static readonly MethodInfo _createEntityQueryableMethod = typeof(AsyncQueryProviderExtensions).GetTypeInfo().GetDeclaredMethod("CreateEntityQueryable");

	public static ConstantExpression CreateEntityQueryableExpression([JetBrains.Annotations.NotNull] this IAsyncQueryProvider entityQueryProvider, [JetBrains.Annotations.NotNull] Type type)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityQueryProvider, "entityQueryProvider");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(type, "type");
		return Expression.Constant(_createEntityQueryableMethod.MakeGenericMethod(type).Invoke(null, new object[1] { entityQueryProvider }));
	}

	[JetBrains.Annotations.UsedImplicitly]
	private static EntityQueryable<TResult> CreateEntityQueryable<TResult>(IAsyncQueryProvider entityQueryProvider)
	{
		return new EntityQueryable<TResult>(entityQueryProvider);
	}
}
