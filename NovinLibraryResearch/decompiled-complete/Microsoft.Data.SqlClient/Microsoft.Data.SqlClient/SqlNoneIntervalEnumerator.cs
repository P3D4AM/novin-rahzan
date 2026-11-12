using System;

namespace Microsoft.Data.SqlClient;

internal class SqlNoneIntervalEnumerator : SqlRetryIntervalBaseEnumerator
{
	protected override TimeSpan GetNextInterval()
	{
		return base.Current;
	}

	public override object Clone()
	{
		return MemberwiseClone();
	}
}
