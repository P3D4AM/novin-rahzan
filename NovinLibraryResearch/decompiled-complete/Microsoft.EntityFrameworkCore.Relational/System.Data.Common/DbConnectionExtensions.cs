using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Common;

internal static class DbConnectionExtensions
{
	private static readonly Func<DbConnection, IsolationLevel, CancellationToken, ValueTask<DbTransaction>> _beginTransactionAsync;

	private static readonly Func<DbConnection, Task> _closeAsync;

	static DbConnectionExtensions()
	{
		MethodInfo method = typeof(DbConnection).GetMethod("BeginTransactionAsync", new Type[2]
		{
			typeof(IsolationLevel),
			typeof(CancellationToken)
		});
		if (method != null)
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(DbConnection), "connection");
			ParameterExpression parameterExpression2 = Expression.Parameter(typeof(IsolationLevel), "isolationLevel");
			ParameterExpression parameterExpression3 = Expression.Parameter(typeof(CancellationToken), "cancellationToken");
			_beginTransactionAsync = Expression.Lambda<Func<DbConnection, IsolationLevel, CancellationToken, ValueTask<DbTransaction>>>(Expression.Call(parameterExpression, method, parameterExpression2, parameterExpression3), new ParameterExpression[3] { parameterExpression, parameterExpression2, parameterExpression3 }).Compile();
		}
		else
		{
			_beginTransactionAsync = BeginTransactionSync;
		}
		MethodInfo method2 = typeof(DbConnection).GetMethod("CloseAsync", Type.EmptyTypes);
		if (method2 != null)
		{
			ParameterExpression parameterExpression4 = Expression.Parameter(typeof(DbConnection), "connection");
			_closeAsync = Expression.Lambda<Func<DbConnection, Task>>(Expression.Call(parameterExpression4, method2), new ParameterExpression[1] { parameterExpression4 }).Compile();
		}
		else
		{
			_closeAsync = CloseSync;
		}
	}

	public static ValueTask<DbTransaction> BeginTransactionAsync(this DbConnection connection, IsolationLevel isolationLevel, CancellationToken cancellationToken)
	{
		return _beginTransactionAsync(connection, isolationLevel, cancellationToken);
	}

	private static ValueTask<DbTransaction> BeginTransactionSync(DbConnection connection, IsolationLevel isolationLevel, CancellationToken cancellationToken)
	{
		if (cancellationToken.IsCancellationRequested)
		{
			return new ValueTask<DbTransaction>(Task.FromCanceled<DbTransaction>(cancellationToken));
		}
		try
		{
			return new ValueTask<DbTransaction>(connection.BeginTransaction(isolationLevel));
		}
		catch (Exception exception)
		{
			return new ValueTask<DbTransaction>(Task.FromException<DbTransaction>(exception));
		}
	}

	public static Task CloseAsync(this DbConnection connection)
	{
		return _closeAsync(connection);
	}

	private static Task CloseSync(DbConnection connection)
	{
		try
		{
			connection.Close();
			return Task.CompletedTask;
		}
		catch (Exception exception)
		{
			return Task.FromException(exception);
		}
	}
}
