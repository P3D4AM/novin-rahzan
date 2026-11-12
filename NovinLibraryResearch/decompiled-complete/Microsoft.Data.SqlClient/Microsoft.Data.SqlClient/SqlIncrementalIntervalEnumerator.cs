using System;

namespace Microsoft.Data.SqlClient;

internal class SqlIncrementalIntervalEnumerator : SqlRetryIntervalBaseEnumerator
{
	private readonly int maxRandom;

	private readonly int minRandom;

	private readonly Random random = new Random();

	public SqlIncrementalIntervalEnumerator(TimeSpan timeInterval, TimeSpan maxTimeInterval, TimeSpan minTimeInterval)
		: base(timeInterval, maxTimeInterval, minTimeInterval)
	{
		double num = base.GapTimeInterval.TotalMilliseconds * 1.2;
		double num2 = base.GapTimeInterval.TotalMilliseconds * 0.8;
		maxRandom = ((num < 2147483647.0) ? Convert.ToInt32(num) : int.MaxValue);
		minRandom = ((num2 < 2147483647.0) ? Convert.ToInt32(num2) : Convert.ToInt32(1288490188.2));
	}

	protected override TimeSpan GetNextInterval()
	{
		double num = base.Current.TotalMilliseconds + (double)random.Next(minRandom, maxRandom);
		num = ((num < base.MaxTimeInterval.TotalMilliseconds) ? num : (random.NextDouble() * (base.MaxTimeInterval.TotalMilliseconds * 0.2) + base.MaxTimeInterval.TotalMilliseconds * 0.8));
		TimeSpan timeSpan = TimeSpan.FromMilliseconds(num);
		base.Current = ((timeSpan < base.MinTimeInterval) ? base.MinTimeInterval : timeSpan);
		return base.Current;
	}

	public override object Clone()
	{
		return MemberwiseClone();
	}
}
