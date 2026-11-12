using System;

namespace Microsoft.Data.SqlClient;

internal class SqlFixedIntervalEnumerator : SqlRetryIntervalBaseEnumerator
{
	private readonly int maxRandom;

	private readonly int minRandom;

	private readonly Random random = new Random();

	public SqlFixedIntervalEnumerator(TimeSpan gapTimeInterval, TimeSpan maxTimeInterval, TimeSpan minTimeInterval)
		: base(gapTimeInterval, maxTimeInterval, minTimeInterval)
	{
		double num = base.GapTimeInterval.TotalMilliseconds * 1.2;
		double num2 = base.GapTimeInterval.TotalMilliseconds * 0.8;
		maxRandom = ((num < 2147483647.0) ? Convert.ToInt32(num) : int.MaxValue);
		minRandom = ((num2 < 2147483647.0) ? Convert.ToInt32(num2) : Convert.ToInt32(1288490188.2));
	}

	protected override TimeSpan GetNextInterval()
	{
		base.Current = TimeSpan.FromMilliseconds(random.Next(minRandom, maxRandom));
		return base.Current;
	}

	public override object Clone()
	{
		return MemberwiseClone();
	}
}
