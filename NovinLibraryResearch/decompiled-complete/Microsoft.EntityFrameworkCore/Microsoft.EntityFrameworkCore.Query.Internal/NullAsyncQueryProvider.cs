using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class NullAsyncQueryProvider : IAsyncQueryProvider, IQueryProvider
{
	public static readonly IAsyncQueryProvider Instance = new NullAsyncQueryProvider();

	private NullAsyncQueryProvider()
	{
	}

	IQueryable IQueryProvider.CreateQuery(Expression expression)
	{
		throw new NotImplementedException();
	}

	IQueryable<TElement> IQueryProvider.CreateQuery<TElement>(Expression expression)
	{
		throw new NotImplementedException();
	}

	object IQueryProvider.Execute(Expression expression)
	{
		throw new NotImplementedException();
	}

	TResult IQueryProvider.Execute<TResult>(Expression expression)
	{
		throw new NotImplementedException();
	}

	TResult IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
}
