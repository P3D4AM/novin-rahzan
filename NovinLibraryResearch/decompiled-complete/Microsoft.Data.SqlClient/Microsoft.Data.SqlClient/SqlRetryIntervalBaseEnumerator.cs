using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Data.SqlClient;

public abstract class SqlRetryIntervalBaseEnumerator : IEnumerator<TimeSpan>, IDisposable, IEnumerator, ICloneable
{
	private readonly TimeSpan _minValue = TimeSpan.Zero;

	private readonly TimeSpan _maxValue = TimeSpan.FromSeconds(120.0);

	public TimeSpan GapTimeInterval { get; protected set; }

	public TimeSpan MaxTimeInterval { get; protected set; }

	public TimeSpan MinTimeInterval { get; protected set; }

	public TimeSpan Current { get; protected set; } = TimeSpan.Zero;


	object IEnumerator.Current => Current;

	public SqlRetryIntervalBaseEnumerator()
	{
		GapTimeInterval = TimeSpan.Zero;
		MaxTimeInterval = TimeSpan.Zero;
		MinTimeInterval = TimeSpan.Zero;
	}

	public SqlRetryIntervalBaseEnumerator(TimeSpan timeInterval, TimeSpan maxTime, TimeSpan minTime)
	{
		Validate(timeInterval, maxTime, minTime);
		GapTimeInterval = timeInterval;
		MaxTimeInterval = maxTime;
		MinTimeInterval = minTime;
	}

	public virtual void Reset()
	{
		Current = TimeSpan.Zero;
	}

	protected virtual void Validate(TimeSpan timeInterval, TimeSpan maxTimeInterval, TimeSpan minTimeInterval)
	{
		if (minTimeInterval < _minValue || minTimeInterval > _maxValue)
		{
			throw SqlReliabilityUtil.ArgumentOutOfRange("minTimeInterval", minTimeInterval, _minValue, _maxValue);
		}
		if (maxTimeInterval < _minValue || maxTimeInterval > _maxValue)
		{
			throw SqlReliabilityUtil.ArgumentOutOfRange("maxTimeInterval", maxTimeInterval, _minValue, _maxValue);
		}
		if (timeInterval < _minValue || timeInterval > _maxValue)
		{
			throw SqlReliabilityUtil.ArgumentOutOfRange("timeInterval", timeInterval, _minValue, _maxValue);
		}
		if (maxTimeInterval < minTimeInterval)
		{
			throw SqlReliabilityUtil.InvalidMinAndMaxPair("minTimeInterval", minTimeInterval, "maxTimeInterval", maxTimeInterval);
		}
	}

	protected abstract TimeSpan GetNextInterval();

	public virtual bool MoveNext()
	{
		TimeSpan timeSpan = Current;
		if (Current < MaxTimeInterval)
		{
			timeSpan = GetNextInterval();
		}
		bool flag = timeSpan <= MaxTimeInterval;
		if (flag)
		{
			Current = timeSpan;
		}
		return flag;
	}

	public virtual void Dispose()
	{
	}

	public virtual object Clone()
	{
		throw new NotImplementedException();
	}
}
