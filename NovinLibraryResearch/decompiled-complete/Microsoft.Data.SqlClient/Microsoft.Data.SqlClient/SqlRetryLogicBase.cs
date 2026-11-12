using System;

namespace Microsoft.Data.SqlClient;

public abstract class SqlRetryLogicBase : ICloneable
{
	public int NumberOfTries { get; protected set; }

	public int Current { get; protected set; }

	public SqlRetryIntervalBaseEnumerator RetryIntervalEnumerator { get; protected set; }

	public Predicate<Exception> TransientPredicate { get; protected set; }

	public virtual bool RetryCondition(object sender)
	{
		return true;
	}

	public abstract bool TryNextInterval(out TimeSpan intervalTime);

	public abstract void Reset();

	public virtual object Clone()
	{
		throw new NotImplementedException();
	}
}
