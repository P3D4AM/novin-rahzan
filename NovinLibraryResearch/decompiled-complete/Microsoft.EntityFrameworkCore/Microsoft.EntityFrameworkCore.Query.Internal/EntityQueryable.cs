using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class EntityQueryable<TResult> : IOrderedQueryable<TResult>, IEnumerable<TResult>, IEnumerable, IOrderedQueryable, IQueryable, IQueryable<TResult>, IAsyncEnumerable<TResult>, IDetachableContext, IListSource
{
	private static readonly EntityQueryable<TResult> _detached = new EntityQueryable<TResult>(NullAsyncQueryProvider.Instance);

	private readonly IAsyncQueryProvider _queryProvider;

	public virtual Type ElementType => typeof(TResult);

	public virtual Expression Expression { get; }

	public virtual IQueryProvider Provider => _queryProvider;

	bool IListSource.ContainsListCollection => false;

	public EntityQueryable([JetBrains.Annotations.NotNull] IAsyncQueryProvider queryProvider)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(queryProvider, "queryProvider");
		_queryProvider = queryProvider;
		Expression = Expression.Constant(this);
	}

	public EntityQueryable([JetBrains.Annotations.NotNull] IAsyncQueryProvider queryProvider, [JetBrains.Annotations.NotNull] Expression expression)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(queryProvider, "queryProvider");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(expression, "expression");
		_queryProvider = queryProvider;
		Expression = expression;
	}

	public virtual IEnumerator<TResult> GetEnumerator()
	{
		return _queryProvider.Execute<IEnumerable<TResult>>(Expression).GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return _queryProvider.Execute<IEnumerable>(Expression).GetEnumerator();
	}

	public virtual IAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
	{
		return _queryProvider.ExecuteAsync<IAsyncEnumerable<TResult>>(Expression).GetAsyncEnumerator(cancellationToken);
	}

	IDetachableContext IDetachableContext.DetachContext()
	{
		return _detached;
	}

	IList IListSource.GetList()
	{
		throw new NotSupportedException(CoreStrings.DataBindingWithIListSource);
	}
}
