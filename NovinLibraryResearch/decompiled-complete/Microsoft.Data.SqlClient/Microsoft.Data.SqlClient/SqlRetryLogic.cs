using System;
using System.Reflection;
using System.Transactions;

namespace Microsoft.Data.SqlClient;

internal sealed class SqlRetryLogic : SqlRetryLogicBase
{
	private const int counterDefaultValue = 0;

	private const int maxAttempts = 60;

	private const string TypeName = "SqlRetryLogic";

	public Predicate<string> PreCondition { get; private set; }

	public SqlRetryLogic(int numberOfTries, SqlRetryIntervalBaseEnumerator enumerator, Predicate<Exception> transientPredicate, Predicate<string> preCondition)
	{
		if (numberOfTries <= 0 || numberOfTries > 60)
		{
			throw SqlReliabilityUtil.ArgumentOutOfRange("numberOfTries", numberOfTries, 1, 60);
		}
		base.NumberOfTries = numberOfTries;
		base.RetryIntervalEnumerator = enumerator;
		base.TransientPredicate = transientPredicate;
		PreCondition = preCondition;
		base.Current = 0;
	}

	public SqlRetryLogic(int numberOfTries, SqlRetryIntervalBaseEnumerator enumerator, Predicate<Exception> transientPredicate)
		: this(numberOfTries, enumerator, transientPredicate, null)
	{
	}

	public SqlRetryLogic(SqlRetryIntervalBaseEnumerator enumerator, Predicate<Exception> transientPredicate)
		: this(1, enumerator, transientPredicate)
	{
	}

	public override void Reset()
	{
		base.Current = 0;
		base.RetryIntervalEnumerator.Reset();
	}

	public override bool TryNextInterval(out TimeSpan intervalTime)
	{
		intervalTime = TimeSpan.Zero;
		bool flag = base.Current < base.NumberOfTries - 1;
		if (flag)
		{
			base.Current++;
			base.RetryIntervalEnumerator.MoveNext();
			intervalTime = base.RetryIntervalEnumerator.Current;
			SqlClientEventSource.Log.TryTraceEvent("<sc.{0}.{1}|INFO> Next gap time will be '{2}' before the next retry number {3}", "SqlRetryLogic", MethodBase.GetCurrentMethod().Name, intervalTime, base.Current);
		}
		else
		{
			SqlClientEventSource.Log.TryTraceEvent("<sc.{0}.{1}|INFO> Current retry ({2}) has reached the maximum attempts (total attempts excluding the first run = {3}).", "SqlRetryLogic", MethodBase.GetCurrentMethod().Name, base.Current, base.NumberOfTries - 1);
		}
		return flag;
	}

	public override bool RetryCondition(object sender)
	{
		bool flag = true;
		if (sender is SqlCommand sqlCommand)
		{
			flag = Transaction.Current == null && sqlCommand.Transaction == null && (PreCondition == null || PreCondition(sqlCommand.CommandText));
			SqlClientEventSource.Log.TryTraceEvent("<sc.{0}.{1}|INFO> (retry condition = '{2}') Avoids retry if it runs in a transaction or is skipped in the command's statement checking.", "SqlRetryLogic", MethodBase.GetCurrentMethod().Name, flag);
		}
		return flag;
	}

	public override object Clone()
	{
		SqlRetryLogic sqlRetryLogic = new SqlRetryLogic(base.NumberOfTries, base.RetryIntervalEnumerator.Clone() as SqlRetryIntervalBaseEnumerator, base.TransientPredicate, PreCondition);
		sqlRetryLogic.RetryIntervalEnumerator.Reset();
		return sqlRetryLogic;
	}
}
