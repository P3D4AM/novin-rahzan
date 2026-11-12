using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Data.SqlClient;

internal class SqlRetryLogicProvider : SqlRetryLogicBaseProvider
{
	private const string TypeName = "SqlRetryLogicProvider";

	private readonly ConcurrentBag<SqlRetryLogicBase> _retryLogicPool = new ConcurrentBag<SqlRetryLogicBase>();

	public SqlRetryLogicProvider(SqlRetryLogicBase retryLogic)
	{
		base.RetryLogic = retryLogic;
	}

	private SqlRetryLogicBase GetRetryLogic()
	{
		SqlRetryLogicBase result = null;
		if (!_retryLogicPool.TryTake(out result))
		{
			result = base.RetryLogic.Clone() as SqlRetryLogicBase;
		}
		else
		{
			result?.Reset();
		}
		return result;
	}

	private void RetryLogicPoolAdd(SqlRetryLogicBase retryLogic)
	{
		if (retryLogic != null)
		{
			_retryLogicPool.Add(retryLogic);
		}
	}

	public override TResult Execute<TResult>(object sender, Func<TResult> function)
	{
		if (function == null)
		{
			throw new ArgumentNullException("function");
		}
		SqlRetryLogicBase sqlRetryLogicBase = null;
		List<Exception> list = new List<Exception>();
		while (true)
		{
			try
			{
				TResult result = function();
				RetryLogicPoolAdd(sqlRetryLogicBase);
				return result;
			}
			catch (Exception ex)
			{
				if (base.RetryLogic.RetryCondition(sender) && base.RetryLogic.TransientPredicate(ex))
				{
					sqlRetryLogicBase = sqlRetryLogicBase ?? GetRetryLogic();
					SqlClientEventSource.Log.TryTraceEvent("<sc.{0}.Execute<TResult>|INFO> Found an action eligible for the retry policy (retried attempts = {1}).", "SqlRetryLogicProvider", sqlRetryLogicBase.Current);
					list.Add(ex);
					if (sqlRetryLogicBase.TryNextInterval(out var intervalTime))
					{
						ApplyRetryingEvent(sender, sqlRetryLogicBase, intervalTime, list, ex);
						Thread.Sleep(intervalTime);
						continue;
					}
					throw CreateException(list, sqlRetryLogicBase);
				}
				RetryLogicPoolAdd(sqlRetryLogicBase);
				throw;
			}
		}
	}

	public override async Task<TResult> ExecuteAsync<TResult>(object sender, Func<Task<TResult>> function, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (function == null)
		{
			throw SqlReliabilityUtil.ArgumentNull("function");
		}
		SqlRetryLogicBase retryLogic = null;
		List<Exception> exceptions = new List<Exception>();
		while (true)
		{
			try
			{
				TResult result = await function();
				RetryLogicPoolAdd(retryLogic);
				return result;
			}
			catch (Exception ex)
			{
				if (base.RetryLogic.RetryCondition(sender) && base.RetryLogic.TransientPredicate(ex))
				{
					retryLogic = retryLogic ?? GetRetryLogic();
					SqlClientEventSource.Log.TryTraceEvent("<sc.{0}.ExecuteAsync<TResult>|INFO> Found an action eligible for the retry policy (retried attempts = {1}).", "SqlRetryLogicProvider", retryLogic.Current);
					exceptions.Add(ex);
					if (retryLogic.TryNextInterval(out var intervalTime))
					{
						ApplyRetryingEvent(sender, retryLogic, intervalTime, exceptions, ex);
						await Task.Delay(intervalTime, cancellationToken);
						continue;
					}
					throw CreateException(exceptions, retryLogic);
				}
				RetryLogicPoolAdd(retryLogic);
				throw;
			}
		}
	}

	public override async Task ExecuteAsync(object sender, Func<Task> function, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (function == null)
		{
			throw new ArgumentNullException("function");
		}
		SqlRetryLogicBase retryLogic = null;
		List<Exception> exceptions = new List<Exception>();
		while (true)
		{
			try
			{
				await function();
				RetryLogicPoolAdd(retryLogic);
			}
			catch (Exception ex)
			{
				if (base.RetryLogic.RetryCondition(sender) && base.RetryLogic.TransientPredicate(ex))
				{
					retryLogic = retryLogic ?? GetRetryLogic();
					SqlClientEventSource.Log.TryTraceEvent("<sc.{0}.ExecuteAsync|INFO> Found an action eligible for the retry policy (retried attempts = {1}).", "SqlRetryLogicProvider", retryLogic.Current);
					exceptions.Add(ex);
					if (retryLogic.TryNextInterval(out var intervalTime))
					{
						ApplyRetryingEvent(sender, retryLogic, intervalTime, exceptions, ex);
						await Task.Delay(intervalTime, cancellationToken);
						continue;
					}
					throw CreateException(exceptions, retryLogic);
				}
				RetryLogicPoolAdd(retryLogic);
				throw;
			}
			break;
		}
	}

	private Exception CreateException(IList<Exception> exceptions, SqlRetryLogicBase retryLogic, bool manualCancellation = false)
	{
		AggregateException result = SqlReliabilityUtil.ConfigurableRetryFail(exceptions, retryLogic, manualCancellation);
		if (!manualCancellation)
		{
			SqlClientEventSource.Log.TryTraceEvent("<sc.{0}.{1}|ERR|THROW> Exiting retry scope (exceeded the max allowed attempts = {2}).", "SqlRetryLogicProvider", MethodBase.GetCurrentMethod().Name, retryLogic.NumberOfTries);
		}
		_retryLogicPool.Add(retryLogic);
		return result;
	}

	private void ApplyRetryingEvent(object sender, SqlRetryLogicBase retryLogic, TimeSpan intervalTime, List<Exception> exceptions, Exception lastException)
	{
		string name = MethodBase.GetCurrentMethod().Name;
		if (base.Retrying != null)
		{
			SqlRetryingEventArgs sqlRetryingEventArgs = new SqlRetryingEventArgs(retryLogic.Current, intervalTime, exceptions);
			SqlClientEventSource.Log.TryTraceEvent("<sc.{0}.{1}|INFO> Running the retrying event.", "SqlRetryLogicProvider", name);
			base.Retrying?.Invoke(sender, sqlRetryingEventArgs);
			if (sqlRetryingEventArgs.Cancel)
			{
				SqlClientEventSource.Log.TryTraceEvent("<sc.{0}.{1}|INFO> Retry attempt cancelled (current retry number = {2}).", "SqlRetryLogicProvider", name, retryLogic.Current);
				throw CreateException(exceptions, retryLogic, manualCancellation: true);
			}
		}
		SqlClientEventSource.Log.TryTraceEvent("<sc.{0}.{1}|INFO> Wait '{2}' and run the action for retry number {3}.", "SqlRetryLogicProvider", name, intervalTime, retryLogic.Current);
	}
}
