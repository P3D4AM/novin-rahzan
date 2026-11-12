using System;

namespace Microsoft.Data.SqlClient;

internal class SqlExponentialIntervalEnumerator : SqlRetryIntervalBaseEnumerator
{
	private int internalCounter = 1;

	private readonly int maxRandom;

	private readonly int minRandom;

	private readonly Random random = new Random();

	public SqlExponentialIntervalEnumerator(TimeSpan deltaBackoffTime, TimeSpan maxTimeInterval, TimeSpan minTimeInterval)
		: base(deltaBackoffTime, maxTimeInterval, minTimeInterval)
	{
		double num = base.GapTimeInterval.TotalMilliseconds * 1.2;
		double num2 = base.GapTimeInterval.TotalMilliseconds * 0.8;
		maxRandom = ((num < 2147483647.0) ? Convert.ToInt32(num) : int.MaxValue);
		minRandom = ((num2 < 2147483647.0) ? Convert.ToInt32(num2) : Convert.ToInt32(1288490188.2));
	}

	protected override TimeSpan GetNextInterval()
	{
		double num = (Math.Pow(2.0, internalCounter++) - 1.0) * (double)random.Next(minRandom, maxRandom);
		double num2 = base.MinTimeInterval.TotalMilliseconds + num;
		num2 = ((num2 < base.MaxTimeInterval.TotalMilliseconds) ? num2 : (random.NextDouble() * (base.MaxTimeInterval.TotalMilliseconds * 0.2) + base.MaxTimeInterval.TotalMilliseconds * 0.8));
		TimeSpan timeSpan = TimeSpan.FromMilliseconds(num2);
		base.Current = ((timeSpan < base.MinTimeInterval) ? base.MinTimeInterval : timeSpan);
		return base.Current;
	}

	public override void Reset()
	{
		base.Reset();
		internalCounter = 1;
	}

	public override object Clone()
	{
		return MemberwiseClone();
	}
}
