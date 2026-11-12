using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class EntityQueryProvider : IAsyncQueryProvider, IQueryProvider
{
	private static readonly MethodInfo _genericCreateQueryMethod = typeof(EntityQueryProvider).GetRuntimeMethods().Single((MethodInfo m) => m.Name == "CreateQuery" && m.IsGenericMethod);

	private readonly MethodInfo _genericExecuteMethod;

	private readonly IQueryCompiler _queryCompiler;

	public EntityQueryProvider([JetBrains.Annotations.NotNull] IQueryCompiler queryCompiler)
	{
		_queryCompiler = queryCompiler;
		_genericExecuteMethod = queryCompiler.GetType().GetRuntimeMethods().Single((MethodInfo m) => m.Name == "Execute" && m.IsGenericMethod);
	}

	public virtual IQueryable<TElement> CreateQuery<TElement>(Expression expression)
	{
		return new EntityQueryable<TElement>(this, expression);
	}

	public virtual IQueryable CreateQuery(Expression expression)
	{
		return (IQueryable)_genericCreateQueryMethod.MakeGenericMethod(expression.Type.GetSequenceType()).Invoke(this, new object[1] { expression });
	}

	public virtual TResult Execute<TResult>(Expression expression)
	{
		return _queryCompiler.Execute<TResult>(expression);
	}

	public virtual object Execute(Expression expression)
	{
		return _genericExecuteMethod.MakeGenericMethod(expression.Type).Invoke(_queryCompiler, new object[1] { expression });
	}

	public virtual TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default(CancellationToken))
	{
		return _queryCompiler.ExecuteAsync<TResult>(expression, cancellationToken);
	}
}
