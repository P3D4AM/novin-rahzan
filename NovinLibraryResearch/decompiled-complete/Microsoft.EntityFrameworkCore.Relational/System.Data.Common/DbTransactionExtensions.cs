using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Common;

internal static class DbTransactionExtensions
{
	private static readonly Func<DbTransaction, CancellationToken, Task> _commitAsync;

	private static readonly Func<DbTransaction, CancellationToken, Task> _rollbackAsync;

	static DbTransactionExtensions()
	{
		MethodInfo method = typeof(DbTransaction).GetMethod("CommitAsync", new Type[1] { typeof(CancellationToken) });
		if (method != null)
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(DbTransaction), "transaction");
			ParameterExpression parameterExpression2 = Expression.Parameter(typeof(CancellationToken), "cancellationToken");
			_commitAsync = Expression.Lambda<Func<DbTransaction, CancellationToken, Task>>(Expression.Call(parameterExpression, method, parameterExpression2), new ParameterExpression[2] { parameterExpression, parameterExpression2 }).Compile();
		}
		else
		{
			_commitAsync = CommitSync;
		}
		MethodInfo method2 = typeof(DbTransaction).GetMethod("RollbackAsync", new Type[1] { typeof(CancellationToken) });
		if (method2 != null)
		{
			ParameterExpression parameterExpression3 = Expression.Parameter(typeof(DbTransaction), "transaction");
			ParameterExpression parameterExpression4 = Expression.Parameter(typeof(CancellationToken), "cancellationToken");
			_rollbackAsync = Expression.Lambda<Func<DbTransaction, CancellationToken, Task>>(Expression.Call(parameterExpression3, method2, parameterExpression4), new ParameterExpression[2] { parameterExpression3, parameterExpression4 }).Compile();
		}
		else
		{
			_rollbackAsync = RollbackSync;
		}
	}

	public static Task CommitAsync(this DbTransaction transaction, CancellationToken cancellationToken)
	{
		return _commitAsync(transaction, cancellationToken);
	}

	private static Task CommitSync(DbTransaction transaction, CancellationToken cancellationToken)
	{
		if (cancellationToken.IsCancellationRequested)
		{
			return Task.FromCanceled(cancellationToken);
		}
		try
		{
			transaction.Commit();
			return Task.CompletedTask;
		}
		catch (Exception exception)
		{
			return Task.FromException(exception);
		}
	}

	public static Task RollbackAsync(this DbTransaction transaction, CancellationToken cancellationToken)
	{
		return _rollbackAsync(transaction, cancellationToken);
	}

	private static Task RollbackSync(DbTransaction transaction, CancellationToken cancellationToken)
	{
		if (cancellationToken.IsCancellationRequested)
		{
			return Task.FromCanceled(cancellationToken);
		}
		try
		{
			transaction.Rollback();
			return Task.CompletedTask;
		}
		catch (Exception exception)
		{
			return Task.FromException(exception);
		}
	}
}
